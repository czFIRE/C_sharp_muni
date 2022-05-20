namespace AutoISClicker
{
    // https://docs.microsoft.com/en-us/dotnet/api/system.xml.serialization.xmlserializer.deserialize?view=net-6.0

    public class Subject
    {
        /// <summary>
        /// First class of the subject
        /// </summary>
        public DateTime SubjectStart { get; set; }

        /// <summary>
        /// End of the first class of the subject
        /// </summary>
        public DateTime SubjectEnd { get; set; }

        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectRoom { get; set; }
        public int Periodicity { get; set; }
        public bool IsBreak { get; set; } = false;

        public Subject(DateTime subjectStart, DateTime subjectEnd, string subjectName, string subjectCode, string subjectRoom, int periodicity = 1)
        {
            SubjectStart = subjectStart;
            SubjectEnd = subjectEnd;
            SubjectName = subjectName;
            SubjectCode = subjectCode;
            SubjectRoom = subjectRoom;
            this.Periodicity = periodicity;
        }

        public Subject(DateTime subjectStart, DateTime subjectEnd, int periodicity = 1) : this(subjectStart, subjectEnd, string.Empty, string.Empty, string.Empty, periodicity)
        {
            this.IsBreak = true;
        }

        // So we can deserialize
        public Subject()
        {

        }

        public override string ToString()
        {
            if (IsBreak)
            {
                return SubjectStart.Hour.ToString("D2") + ":" + SubjectStart.Minute.ToString("D2") + " - " + SubjectEnd.Hour.ToString("D2") + ":" + SubjectEnd.Minute.ToString("D2") + "\tBREAK";
            }
            return SubjectStart.Hour.ToString("D2") + ":" + SubjectStart.Minute.ToString("D2") + " - " + SubjectEnd.Hour.ToString("D2") + ":" + SubjectEnd.Minute.ToString("D2")
                + "\tSubject code: " + SubjectCode + " | Subject room: " + SubjectRoom + "\tSubject Name: " + SubjectName;
        }
    }
}
