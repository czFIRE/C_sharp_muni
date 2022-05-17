namespace AutoISClicker
{
    public class Utilities
    {
        public static string UCO = String.Empty;
        public static string Password = String.Empty;

        public static int OperationCounter = 0;
        public static ReaderWriterLockSlim OperationLock = new ReaderWriterLockSlim();
        public const int OperationLimit = 50; // max 100

        public static void GetUserLoginData(string filePath = "./data.txt")
        {

            try
            {
                Console.WriteLine("Trying to open file: " + filePath);
                var file = System.IO.File.ReadLines(filePath);

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
                            return;
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Enter you uco.");
                UCO = Console.ReadLine().Trim();

                Console.WriteLine("password: ");
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

            if (OperationCounter > OperationLimit && operationCost != 0)
            {
                return false;
            }

            OperationLock.EnterWriteLock();

            //critical section

            if (OperationCounter > OperationLimit && operationCost != 0)
            {
                OperationLock.ExitWriteLock();
                return false;
            }

            if (operationCost == 0)
            {
                // nulling the counter
                OperationCounter = operationCost;
            } else
            {
                OperationCounter += operationCost;
            }

            //

            OperationLock.ExitWriteLock();
            return true;
        }

        public static void CounterReseter()
        {
            Task.Run(() =>
            {
                while(true)
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
