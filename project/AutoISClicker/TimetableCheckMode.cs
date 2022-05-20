namespace AutoISClicker
{
    public class TimetableCheckMode : IRunMode
    {
        public void RunTask(Timetable timetable, string subjectsFolder)
        {
            string message = string.Empty;
            if (!timetable.CheckForConflictsInTimetable(subjectsFolder))
            {
                Console.WriteLine("\nThere are conflicts in your timetable, please FIX them!");
            }
            else
            {
                Console.WriteLine("\nNo conflicts in timetable, do you want to start the load now? (Yes/No)");
                string str = Console.ReadLine().Trim().ToLower();

                if (str == "yes" || str == "y")
                {
                    var mod = new SignUpMode();
                    mod.RunTask(timetable, subjectsFolder);
                }
            }
            return;
        }
    }
}
