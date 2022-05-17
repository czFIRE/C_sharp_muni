// See https://aka.ms/new-console-template for more information

var time = new AutoISClicker.ISTime();
Console.WriteLine(time.GetISTime());

var timetable = AutoISClicker.TimetableUtils.DeserializeTimetableFromISExport("./../../../data/sample_timetable.xml");

foreach (var t in timetable)
{
    foreach (var t2 in t)
    {
        Console.WriteLine(t2.ToString());
    }

    Console.WriteLine("\n\n\n");
}

AutoISClicker.TimetableUtils.SerializeTimetable(timetable, "./../../../data/serialized/");
var timetab2 = AutoISClicker.TimetableUtils.DeserializeTimetable("./../../../data/serialized/");

foreach (var t in timetab2)
{
    foreach (var t2 in t)
    {
        Console.WriteLine(t2.ToString());
    }

    Console.WriteLine("\n\n\n");
}


// AutoISClicker.GlobalStorage.operationLock;

AutoISClicker.Utilities.GetUserLoginData("./../../../../dat");

var iSInstance = new AutoISClicker.ISInstance();
var driver = iSInstance.LoginToIS(AutoISClicker.Utilities.UCO, AutoISClicker.Utilities.Password);
//driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(5);

Console.WriteLine(driver.Url);

iSInstance.SignUpForGroup("https://is.muni.cz/auth/discussion/predmetove/ped/jaro2022/NJ_B202/");

Console.WriteLine(driver.Url);

Console.ReadLine();

driver.Quit();
