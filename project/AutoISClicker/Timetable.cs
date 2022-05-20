using System.Xml;
using System.Xml.Serialization;

namespace AutoISClicker
{
    public class Timetable
    {
        public List<Subject>[] timetable;

        public enum CreationMode
        {
            ISEXPORT,
            SAVED_XML
        }

        public Timetable(string filename, CreationMode mode = CreationMode.ISEXPORT)
        {
            timetable = mode switch
            {
                CreationMode.ISEXPORT => DeserializeTimetableFromISExport(filename),
                CreationMode.SAVED_XML => DeserializeTimetable(filename),
                _ => throw new NotImplementedException(),
            };
        }

        public Subject SubjectFromSlot(XmlNode slot, int offset)
        {
            var attributes = slot.Attributes;

            DateTime fromTime = Utilities.DateFromTime(attributes["odcas"], offset);
            DateTime toTime = Utilities.DateFromTime(attributes["docas"], offset);

            // this will take all from the whole file?!
            // var rooms = slot.SelectNodes("//*[local-name()='mistnostozn']");

            string rooms = string.Empty;
            string subjectName = string.Empty;
            string subjectCode = string.Empty;

            foreach (XmlNode child in slot.ChildNodes)
            {
                switch (child.Name)
                {
                    case "mistnosti":
                        foreach (XmlNode room in child.ChildNodes)
                        {
                            rooms += room.FirstChild.InnerText;
                        }
                        break;
                    case "akce":
                        foreach (XmlNode subject in child.ChildNodes)
                        {
                            switch (subject.Name)
                            {
                                case "kod":
                                    subjectCode = subject.InnerText;
                                    break;
                                case "nazev":
                                    subjectName = subject.InnerText;
                                    break;
                                case "predmetid":
                                    break;
                                case "fakulta_url":
                                    break;
                                case "obdobi_url":
                                    break;

                                default:
                                    break;
                            }
                        }
                        break;
                    default:
                        break;
                }

            }

            return new Subject(fromTime, toTime, subjectName, subjectCode, rooms);
        }

        public Subject BreakFromSlot(XmlNode slot, int offset)
        {
            var attributes = slot.Attributes;

            var tmp = attributes["odcas"];

            DateTime fromTime = Utilities.DateFromTime(attributes["odcas"], offset);
            DateTime toTime = Utilities.DateFromTime(attributes["docas"], offset);

            return new Subject(fromTime, toTime);
        }

        public List<Subject>[] DeserializeTimetableFromISExport(string filename)
        {
            List<Subject>[] result = new List<Subject>[Utilities.DAYS_IN_WEEK];
            for (int i = 0; i < Utilities.DAYS_IN_WEEK; i++)
            {
                result[i] = new List<Subject>();
            }


            XmlDocument doc = new XmlDocument();
            doc.Load(filename);

            XmlNodeList nodeList = doc.SelectNodes("/rozvrh/tabulka/den");

            // for each day
            foreach (XmlNode day in nodeList)
            {

                int offset = Utilities.DayToOffset(day.Attributes["id"].Value);

                // for each row
                foreach (XmlNode row in day.ChildNodes)
                {
                    // here add something about collisions right now in timetable


                    // for each slot / break
                    foreach (XmlNode slot in row.ChildNodes)
                    {
                        result[offset].Add(slot.Name == "slot" ? SubjectFromSlot(slot, offset) : BreakFromSlot(slot, offset));
                    }
                }
            }


            return result;
        }

        public void SerializeTimetable(string folderPath = "./../../../data/serialized/")
        {
            var serializer = new XmlSerializer(typeof(List<Subject>), new Type[] { typeof(Subject) });
            for (int i = 0; i < timetable.Length; i++)
            {
                using var writer = new StreamWriter(folderPath + Enum.GetName(typeof(DayOfWeek), (i + 1) % 7) + ".xml", false);

                serializer.Serialize(writer, timetable[i]);
            }

        }

        public List<Subject>[] DeserializeTimetable(string folderPath = "./../../../data/serialized/")
        {
            // maybe replace this with directory "size"
            List<Subject>[] timetable = new List<Subject>[Utilities.DAYS_IN_WEEK];
            var serializer = new XmlSerializer(typeof(List<Subject>), new Type[] { typeof(Subject) });

            for (int i = 0; i < Utilities.DAYS_IN_WEEK; i++)
            {
                using var reader = new StreamReader(folderPath + Enum.GetName(typeof(DayOfWeek), (i + 1) % 7) + ".xml");

                timetable[i] = (List<Subject>)serializer.Deserialize(reader);
            }

            return timetable;
        }


        public bool InsertSubjectToTimetable(Subject subject)
        {

            int day = (((int)subject.SubjectStart.DayOfWeek) - 1) % 7;

            bool canInsert = false;

            // I am not expecting for you to have a one hour long subject at the end of the day => if you do, this will be a bug!
            // This also only works for 2 hour long seminars => if there is enough time, fix this
            for (int i = 0; i < timetable[day].Count() - 1; i++)
            {
                if (timetable[day][i].IsBreak && timetable[day][i + 1].IsBreak &&
                    timetable[day][i].SubjectStart <= subject.SubjectStart && timetable[day][i + 1].SubjectEnd >= subject.SubjectEnd)
                {
                    timetable[day].Insert(i, subject);
                    timetable[day].RemoveRange(i + 1, 2);
                    canInsert = true;
                }

            }
            return canInsert;
        }

        public bool CheckForConflictsInTimetable(string dirPath = "./../../../data/subjects/")
        {
            var iSInstance = new AutoISClicker.ISInstance();
            iSInstance.LoginToIS(AutoISClicker.Utilities.UCO, AutoISClicker.Utilities.Password);

            var files = Directory.EnumerateFiles(dirPath).ToArray();

            bool noConflicts = true;

            foreach (var file in files)
            {
                Console.WriteLine("Now working on: " + file);
                var fileLines = System.IO.File.ReadLines(file);

                foreach (var line in fileLines)
                {
                    var subject = iSInstance.ParseSubjectFromGroupSignUp(line);

                    bool canInsert = InsertSubjectToTimetable(subject);
                    noConflicts &= canInsert;

                    Console.WriteLine($"Can insert: {canInsert} - {line}");
                }

                Console.WriteLine("\n");
            }

            return noConflicts;
        }

        public void PrintTimetable(List<Subject>[] timetable)
        {
            foreach (var day in timetable)
            {
                foreach (var slot in day)
                {
                    Console.WriteLine(slot.ToString());
                }

                Console.WriteLine("\n\n");
            }
        }
    }
}
