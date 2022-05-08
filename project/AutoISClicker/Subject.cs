using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoISClicker
{
    internal class Subject
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
        public int periodicity { get; set; }

        public Subject(DateTime subjectStart, DateTime subjectEnd, string subjectName, string subjectCode, string subjectRoom, int periodicity = 1)
        {
            SubjectStart = subjectStart;
            SubjectEnd = subjectEnd;
            SubjectName = subjectName;
            SubjectCode = subjectCode;
            SubjectRoom = subjectRoom;
            this.periodicity = periodicity;
        }

        public override string? ToString()
        {
            return SubjectStart.Hour + ":" + SubjectStart.Minute + " - " + SubjectEnd.Hour + ":" + SubjectEnd.Minute + "\tSubject code: " + SubjectCode + " | Subject room: " + SubjectRoom
                   + "\tSubject Name: " + SubjectName;
        }
    }
}
