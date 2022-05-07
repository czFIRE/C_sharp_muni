using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace AutoISClicker
{
    internal class ISLogin
    {

        public static void LoginToIS()
        {
            IWebDriver driver = new FirefoxDriver("./../../../WebDriver/");
            try
            {
                driver.Url = "https://is.muni.cz/";
                driver.FindElement(By.XPath("/html/body/div/div[2]/main/div[1]/div/div/a")).Click();
                Console.WriteLine("Enter you uco.");
                string uco = Console.ReadLine();
                driver.FindElement(By.XPath("/html/body/div/div[2]/div[2]/main/div/div/form/div[1]/div/div/span[1]/input"))
                            .SendKeys(uco);
                Console.WriteLine("Enter you password.");
                string password = Console.ReadLine();
                driver.FindElement(By.XPath("/html/body/div/div[2]/div[2]/main/div/div/form/div[1]/div/div/span[3]/input"))
                            .SendKeys(password + Keys.Enter);
            }
            catch
            {
                Console.WriteLine("[error] this task has some error");
            }
            driver.Close();
        }
    }
}
