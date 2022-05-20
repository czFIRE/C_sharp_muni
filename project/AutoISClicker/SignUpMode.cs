namespace AutoISClicker
{
    public class SignUpMode : IRunMode
    {
        public void RunTask(Timetable timetable, string subjectsFolder)
        {
            SignUP(subjectsFolder).Wait();
        }

        private async Task SignUP(string subjectsFolder)
        {
            // start reseting counter for operations
            Utilities.CounterReseter();

            var files = Directory.EnumerateFiles(subjectsFolder).ToArray();

            var tasks = new Task[files.Count()];

            Console.WriteLine("\nPlease input the payload start time in format \"HH:MM(:SS)\" (seconds are optional):");

            int[] timeLine;
            try
            {
                timeLine = Console.ReadLine().Trim().Split(":").Select(x => Int32.Parse(x)).ToArray();
            }
            catch
            {
                Console.Error.WriteLine("Your skills of typing time are hilariously bad! Try again next time!");
                throw;
            }

            Console.WriteLine("\nPlease input how many tries per subject you want:");

            int tries;
            try
            {
                tries = Int32.Parse(Console.ReadLine().Trim());
            }
            catch
            {
                Console.Error.WriteLine("Your skills of typing time are hilariously bad! Try again next time!");
                throw;
            }

            var timeInfo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, timeLine[0], timeLine[1], timeLine.Length == 3 ? timeLine[2] : 0);

            var isTime = new ISTime();

            Console.WriteLine("\n\nEntering delay till specified time: " + DateTime.Now);
            var delay = Task.Delay((int)(timeInfo - isTime.GetISTime()).TotalMilliseconds);

            for (int i = 0; i < files.Count(); i++)
            {
                int tmp = i;

                tasks[tmp] = Task.Run(async () =>
                {
                    var iSInstance = new ISInstance();
                    iSInstance.LoginToIS(AutoISClicker.Utilities.UCO, AutoISClicker.Utilities.Password);

                    var fileLines = System.IO.File.ReadLines(files[tmp]);

                    // here wait for time

                    await delay;
                    Console.WriteLine("Ending delay " + DateTime.Now);

                    // 

                    iSInstance.SignUpForGroupsFromSubject(fileLines, tries);
                });
            }

            await Task.WhenAll(tasks);
            Console.WriteLine("\n\nAll ended!");

            return;
        }

    }
}
