using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace AutoISClicker
{
    public class ISInstance
    {
        IWebDriver driver { get; }
        public ISInstance(string driverLocation = "./../../../WebDriver/")
        {
            driver = new FirefoxDriver("./../../../WebDriver/");
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
                driver.Url = "https://is.muni.cz/";
                driver.FindElement(By.XPath("/html/body/div/div[2]/main/div[1]/div/div/a")).Click();

                // To make it less "flashy"
                Thread.Sleep(1000);

                driver.FindElement(By.XPath("/html/body/div/div[2]/div[2]/main/div/div/form/div[1]/div/div/span[1]/input"))
                            .SendKeys(uco);

                driver.FindElement(By.XPath("/html/body/div/div[2]/div[2]/main/div/div/form/div[1]/div/div/span[3]/input"))
                            .SendKeys(password + Keys.Enter);

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                wait.Until(driver => driver.Url == "https://is.muni.cz/auth/");
            }
            catch
            {
                Console.WriteLine("[error] this task has some error");
            }
            return driver;
        }

        public IWebDriver SignUpForGroup(string url)
        {
            //while(!Utilities.AccessLock(1)) { }

            driver.Navigate().GoToUrl(url);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(driver => driver.Url == url);

            return driver;
        }

        public void SignUpForGroupsFromSubject(string filePath)
        {
            var fileLines = System.IO.File.ReadLines(filePath);
 
            while(Utilities.OperationCounter < Utilities.OperationLimit)
            {
                while (!Utilities.AccessLock(fileLines.Count())) { }
                
                foreach (var line in fileLines)
                {
                    this.SignUpForGroup(line);
                }
            }
        }
    }
}
