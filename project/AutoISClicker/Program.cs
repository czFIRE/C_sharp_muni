// See https://aka.ms/new-console-template for more information
using OpenQA.Selenium;

Console.WriteLine("Hello, World!");

Console.WriteLine(AutoISClicker.GlobalStorage.GetNetworkTime());

// AutoISClicker.GlobalStorage.operationLock;

AutoISClicker.Utilities.GetUserLoginData("./../../../data.txt");

var driver = AutoISClicker.ISLogin.LoginToIS(AutoISClicker.Utilities.UCO, AutoISClicker.Utilities.Password);
driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(5);

Console.WriteLine(driver.Url);

driver.Navigate().GoToUrl("https://is.muni.cz/auth/discussion/predmetove/ped/jaro2022/NJ_B202/");

Console.WriteLine(driver.Url);

Console.ReadLine();

//IWebDriver driver2 = driver.;
//driver2.Navigate().GoToUrl("https://is.muni.cz/auth/el/1433/jaro2022/PA152/");
//Console.WriteLine(driver2.Url);
//Console.ReadKey();
//driver2.Quit();

driver.Quit();
