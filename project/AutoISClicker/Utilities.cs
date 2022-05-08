﻿namespace AutoISClicker
{
    public class Utilities
    {
        public static string UCO = String.Empty;
        public static string Password = String.Empty;

        public static int OperationCounter = 0;
        public static ReaderWriterLockSlim OperationLock = new ReaderWriterLockSlim();
        public const int OperationLimit = 100;

        public static void GetUserLoginData(string location = "./data.txt")
        {

            try
            {
                Console.WriteLine("Trying to open file: " + location);
                var file = System.IO.File.ReadLines(location);

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

                Console.WriteLine("Enter you password.");
                Password = Console.ReadLine().Trim();
            }
        }

        ~Utilities()
        {
            if (OperationLock != null) OperationLock.Dispose();
        }
    }
}
