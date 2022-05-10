using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoISClicker
{
    public class Break : Subject
    {
        public Break(DateTime subjectStart, DateTime subjectEnd, string subjectName, string subjectCode, string subjectRoom, int periodicity = 1) 
            : base(subjectStart, subjectEnd, subjectName, subjectCode, subjectRoom, periodicity)
        {

        }

        public Break(DateTime subjectStart, DateTime subjectEnd, int periodicity = 1) : base(subjectStart, subjectEnd, string.Empty, string.Empty, string.Empty, periodicity)
        {

        }

        public override string? ToString()
        {
            return SubjectStart.Hour.ToString("D2") + ":" + SubjectStart.Minute.ToString("D2") + " - " + SubjectEnd.Hour.ToString("D2") + ":" + SubjectEnd.Minute.ToString("D2") + "\tBREAK";
        }
    }
}
