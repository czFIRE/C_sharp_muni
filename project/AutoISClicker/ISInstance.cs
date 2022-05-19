using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace AutoISClicker
{
    public class ISInstance
    {
        public IWebDriver Driver { get; }
        public ISInstance(string driverLocation = "./../../../WebDriver/")
        {
            Driver = new FirefoxDriver("./../../../WebDriver/");
        }

        /// <summary>
        /// Creates new session and logs into IS.
        /// </summary>
        /// <param name="uco">UCO of the user</param>
        /// <param name="password">Password of the user</param>
        /// <returns>IWebdriver logged into IS</returns>
        public IWebDriver LoginToIS(string uco, string password)
        {
            while (!Utilities.AccessLock(3)) { }

            try
            {
                Driver.Url = "https://is.muni.cz/";
                Driver.FindElement(By.XPath("/html/body/div/div[2]/main/div[1]/div/div/a")).Click();

                // To make it less "flashy"
                Thread.Sleep(1000);

                Driver.FindElement(By.XPath("/html/body/div/div[2]/div[2]/main/div/div/form/div[1]/div/div/span[1]/input"))
                            .SendKeys(uco);

                Driver.FindElement(By.XPath("/html/body/div/div[2]/div[2]/main/div/div/form/div[1]/div/div/span[3]/input"))
                            .SendKeys(password + Keys.Enter);

                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(30));
                wait.Until(driver => driver.Url == "https://is.muni.cz/auth/");
            }
            catch
            {
                Console.WriteLine("[error] this task has some error");
            }
            return Driver;
        }

        public bool SignUpForGroup(string url)
        {
            while (!Utilities.AccessLock(1)) { }

            Driver.Navigate().GoToUrl(url);
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(30));
            wait.Until(driver => driver.Url == url);

            return Driver.FindElement(By.XPath("/html/body/div/div[2]/div/div/h3")).Text == "Úspěšně přihlášeno.";
        }

        public void SignUpForGroupsFromSubject(IEnumerable<string> fileLines)
        {

            while (Interlocked.Read(ref Utilities.OperationCounter) < Utilities.OperationLimit)
            {

                foreach (var line in fileLines)
                {
                    if (this.SignUpForGroup(line))
                    {
                        Console.WriteLine("Signed up for group: " + line);
                    }
                    else
                    {
                        Console.WriteLine("FAILED for group: " + line);
                    }
                }

                return;
            }
        }

        public Subject ParseSubjectFromGroupSignUp(string url)
        {
            Driver.Navigate().GoToUrl(url);
            var infoBox = Driver.FindElement(By.XPath("/html/body/div/div[2]/div/table[1]/tbody/tr/td/div"));

            var infoLine = infoBox.Text.Split("\n")[0].Trim().Split(" ");

            int offset = TimetableUtils.DayToOffset(infoLine[1]);

            DateTime fromTime = TimetableUtils.DateFromTime(infoLine[9].Split("–")[0], offset);
            DateTime toTime = TimetableUtils.DateFromTime(infoLine[9].Split("–")[1], offset);

            string rooms = infoLine[10];
            string subjectCode = infoLine[0];

            string subjectName = Driver.FindElement(By.XPath("/html/body/div/div[2]/div/h3")).Text.Split(" ", 2)[1];

            return new Subject(fromTime, toTime, subjectName, subjectCode, rooms);
        }

        ~ISInstance()
        {
            Driver.Quit();
        }
    }
}
