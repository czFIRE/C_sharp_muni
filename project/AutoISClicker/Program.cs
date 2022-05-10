// See https://aka.ms/new-console-template for more information

var time = new AutoISClicker.ISTime();
Console.WriteLine(time.GetISTime());

var timetable = AutoISClicker.TimetableUtils.DeserializeTimetable("./../../../timetable/wtf.xml");

foreach (var t in timetable)
{
    foreach (var t2 in t)
    {
        Console.WriteLine(t2.ToString());
    }

    Console.WriteLine("\n\n\n");
}

// AutoISClicker.GlobalStorage.operationLock;

AutoISClicker.Utilities.GetUserLoginData("./../../../../dat");

var driver = AutoISClicker.ISLogin.LoginToIS(AutoISClicker.Utilities.UCO, AutoISClicker.Utilities.Password);
//driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(5);

Console.WriteLine(driver.Url);

driver.Navigate().GoToUrl("https://is.muni.cz/auth/discussion/predmetove/ped/jaro2022/NJ_B202/");

Console.WriteLine(driver.Url);

Console.ReadLine();

driver.Quit();
