using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace AutoISClicker
{
    public class ISLogin
    {
        /// <summary>
        /// Creates new session and logs into IS.
        /// </summary>
        /// <param name="uco">UCO of the user</param>
        /// <param name="password">Password of the user</param>
        /// <returns>IWebdriver logged into IS</returns>
        public static IWebDriver LoginToIS(string uco, string password)
        {
            IWebDriver driver = new FirefoxDriver("./../../../WebDriver/");
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
    }
}
