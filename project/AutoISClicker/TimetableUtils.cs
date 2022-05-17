using System.Xml;
using System.Xml.Serialization;

namespace AutoISClicker
{
    public class TimetableUtils
    {
        // TODO: Add loading from WebPage => https://is.muni.cz/predmety/obdobi
        // start => /html/body/div[1]/div[2]/div[2]/main/form/table[1]/tbody/tr[17]/td[9]
        // end => /html/body/div[1]/div[2]/div[2]/main/form/table[1]/tbody/tr[18]/td[8]
        public static DateTime SemesterStart { get; set; } = new DateTime(2022, 9, 12);
        public static DateTime SemesterEnd { get; set; } = new DateTime(2022, 12, 12);

        public static int SemesterDurationInWeeks = (SemesterEnd - SemesterStart).Days / 7;


        private const int DAYS_IN_WEEK = 5;

        // Enum should be used here if we will ever use this construct
        private static int DayToOffset(string day)
        {
            return day.Split(" ")[0] switch
            {
                "Po" => 0,
                "Út" => 1,
                "St" => 2,
                "Čt" => 3,
                "Pá" => 4,
                "So" => 5,
                "Ne" => 6,
                _ => throw new ArgumentException("Invalid day in file"),
            };
        }

        private static DateTime DateFromTime(XmlAttribute time, int offset)
        {
            var hourMinute = time.Value.Split(':');

            return new DateTime(SemesterStart.Year, SemesterStart.Month, SemesterStart.Day, Int32.Parse(hourMinute[0]), Int32.Parse(hourMinute[1]), 0).AddDays(offset);
        }

        private static Subject SubjectFromSlot(XmlNode slot, int offset)
        {
            var attributes = slot.Attributes;

            DateTime fromTime = DateFromTime(attributes["odcas"], offset);
            DateTime toTime = DateFromTime(attributes["docas"], offset);

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

        private static Break BreakFromSlot(XmlNode slot, int offset)
        {
            var attributes = slot.Attributes;

            var tmp = attributes["odcas"];

            DateTime fromTime = DateFromTime(attributes["odcas"], offset);
            DateTime toTime = DateFromTime(attributes["docas"], offset);

            return new Break(fromTime, toTime);
        }

        public static List<Subject>[] DeserializeTimetableFromISExport(string filename)
        {
            List<Subject>[] result = new List<Subject>[DAYS_IN_WEEK];
            for (int i = 0; i < DAYS_IN_WEEK; i++)
            {
                result[i] = new List<Subject>();
            }


            XmlDocument doc = new XmlDocument();
            doc.Load(filename);

            XmlNodeList nodeList = doc.SelectNodes("/rozvrh/tabulka/den");

            // for each day
            foreach (XmlNode day in nodeList)
            {

                int offset = DayToOffset(day.Attributes["id"].Value);

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

        public static void SerializeTimetable(List<Subject>[] timetable, string folderPath = "./../../../data/serialized/")
        {
            var serializer = new XmlSerializer(typeof(Subject));
            for (int i = 0; i < timetable.Length; i++)
            {
                using var writer = new StreamWriter(folderPath + Enum.GetName(typeof(DayOfWeek), (i + 1) % 7) + ".xml", false);

                foreach (var subject in timetable[i])
                {
                    serializer.Serialize(writer, subject);
                }
            }

        }

        public static List<Subject>[] DeserializeTimetable(string folderPath = "./../../../data/serialized/")
        {
            // maybe replace this with directory "size"
            List<Subject>[] timetable = new List<Subject>[DAYS_IN_WEEK];
            var serializer = new XmlSerializer(typeof(Subject));

            for (int i = 0; i < DAYS_IN_WEEK; i++)
            {
                timetable[i] = new List<Subject>();

                using var reader = new StreamReader(folderPath + Enum.GetName(typeof(DayOfWeek), (i + 1) % 7) + ".xml");

                var tmp = serializer.Deserialize(reader);
                ;
            }



            return timetable;
        }
    }
}
