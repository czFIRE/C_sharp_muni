using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AutoISClicker
{
    // https://docs.microsoft.com/en-us/dotnet/api/system.xml.serialization.xmlserializer.deserialize?view=net-6.0


    [XmlInclude(typeof(Break))]
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

        public Subject()
        {

        }

        public override string? ToString()
        {
            return SubjectStart.Hour.ToString("D2") + ":" + SubjectStart.Minute.ToString("D2") + " - " + SubjectEnd.Hour.ToString("D2") + ":" + SubjectEnd.Minute.ToString("D2") 
                + "\tSubject code: " + SubjectCode + " | Subject room: " + SubjectRoom + "\tSubject Name: " + SubjectName;
        }
    }
}
