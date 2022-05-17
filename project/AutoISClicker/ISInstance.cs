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
            while(!Utilities.AccessLock(1)) { }

            Driver.Navigate().GoToUrl(url);
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(30));
            wait.Until(driver => driver.Url == url);

            return Driver.FindElement(By.XPath("/html/body/div/div[2]/div/div/h3")).Text == "Úspěšně přihlášeno.";
        }

        public void SignUpForGroupsFromSubject(string filePath)
        {
            var fileLines = System.IO.File.ReadLines(filePath);
 
            while(Utilities.OperationCounter < Utilities.OperationLimit)
            {
                // while (!Utilities.AccessLock(fileLines.Count())) { }
                
                foreach (var line in fileLines)
                {
                    if(this.SignUpForGroup(line))
                    {
                        Console.WriteLine("Signed up for group: " + line);
                    } else
                    {
                        Console.WriteLine("FAILED for group: " + line);
                    }
                }

                return;
            }
        }

        ~ISInstance()
        {
            Driver.Quit();
        }
    }
}
