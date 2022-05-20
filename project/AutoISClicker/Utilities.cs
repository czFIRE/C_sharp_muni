using System.Xml;

namespace AutoISClicker
{
    public class Utilities
    {
        public static string UCO { get; set; } = String.Empty;
        public static string Password { get; set; } = String.Empty;

        public static long OperationCounter = 0;
        public static ReaderWriterLockSlim OperationLock { get; } = new ReaderWriterLockSlim();
        public const int OperationLimit = 50; // max 100

        // TODO: Add loading from WebPage => https://is.muni.cz/predmety/obdobi
        // start => /html/body/div[1]/div[2]/div[2]/main/form/table[1]/tbody/tr[17]/td[9]
        // end => /html/body/div[1]/div[2]/div[2]/main/form/table[1]/tbody/tr[18]/td[8]
        public static DateTime SemesterStart { get; set; } = new DateTime(2022, 9, 12);
        public static DateTime SemesterEnd { get; set; } = new DateTime(2022, 12, 12);
        public static int SemesterDurationInWeeks { get; } = (SemesterEnd - SemesterStart).Days / 7;


        public const int DAYS_IN_WEEK = 5;

        // Enum should be used here if we will ever use this construct
        public static int DayToOffset(string day)
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

        // Somehow merge these two
        public static DateTime DateFromTime(String time, int offset)
        {
            var hourMinute = time.Split(':');

            return new DateTime(SemesterStart.Year, SemesterStart.Month, SemesterStart.Day, Int32.Parse(hourMinute[0]), Int32.Parse(hourMinute[1]), 0).AddDays(offset);
        }

        public static DateTime DateFromTime(XmlAttribute time, int offset)
        {
            var hourMinute = time.Value.Split(':');

            return new DateTime(SemesterStart.Year, SemesterStart.Month, SemesterStart.Day, Int32.Parse(hourMinute[0]), Int32.Parse(hourMinute[1]), 0).AddDays(offset);
        }

        public static void GetUserLoginData()
        {
            try
            {

                Console.WriteLine("Input a file if you have saved your login data:"); // "./../../../../data"
                var file = System.IO.File.ReadLines(Console.ReadLine());

                int i = -1;
                foreach (string line in file)
                {
                    i++;
                    switch (i)
                    {
                        case 0:
                            UCO = line.Trim();
                            break;
                        case 1:
                            Password = line.Trim();
                            break;
                        default:
                            throw new InvalidDataException("This isn't a valid login data file!");
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("From file failed, please input manually!");

                Console.WriteLine("Enter you uco:");
                UCO = Console.ReadLine().Trim();

                Console.WriteLine("Enter your password:");
                while (true)
                {
                    var key = System.Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Enter)
                        break;
                    Password += key.KeyChar;
                }
            }
        }

        // https://stackoverflow.com/questions/42839394/asynchronous-processing-of-work-items-from-a-priority-queue

        public static bool AccessLock(int operationCost)
        {

            if (Interlocked.Read(ref Utilities.OperationCounter) > OperationLimit && operationCost != 0)
            {
                return false;
            }

            OperationLock.EnterWriteLock();

            //critical section

            if (Interlocked.Read(ref Utilities.OperationCounter) > OperationLimit && operationCost != 0)
            {
                OperationLock.ExitWriteLock();
                return false;
            }

            if (operationCost == 0)
            {
                // nulling the counter
                Interlocked.Exchange(ref Utilities.OperationCounter, operationCost);
            }
            else
            {
                Interlocked.Add(ref Utilities.OperationCounter, operationCost);
            }

            //

            OperationLock.ExitWriteLock();
            return true;
        }

        public static void CounterReseter()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    //sleep for a minute
                    Thread.Sleep(60000);

                    // reset counter
                    AccessLock(0);
                }
            });
        }

        ~Utilities()
        {
            if (OperationLock != null) OperationLock.Dispose();
        }
    }
}
