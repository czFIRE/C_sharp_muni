using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoISClicker
{
    public class Utilities
    {
        public static string UCO = String.Empty;
        public static string Password = String.Empty;

        public static void GetUserLoginData(string location = "./data.txt")
        {
            
            try
            {
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
    }
}
