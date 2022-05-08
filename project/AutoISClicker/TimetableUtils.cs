using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
