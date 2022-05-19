// See https://aka.ms/new-console-template for more information

var time = new AutoISClicker.ISTime();
Console.WriteLine(time.GetISTime());

var timetable = AutoISClicker.TimetableUtils.DeserializeTimetableFromISExport("./../../../data/sample_timetable.xml");

AutoISClicker.TimetableUtils.SerializeTimetable(timetable, "./../../../data/serialized/");
var timetab2 = AutoISClicker.TimetableUtils.DeserializeTimetable("./../../../data/serialized/");

AutoISClicker.TimetableUtils.PrintTimetable(timetab2);

AutoISClicker.Utilities.GetUserLoginData("./../../../../data");

AutoISClicker.TimetableUtils.CheckForConflictsInTimetable(timetab2);

AutoISClicker.TimetableUtils.PrintTimetable(timetab2);

return;


/*
var iSInstance = new AutoISClicker.ISInstance();
var driver = iSInstance.LoginToIS(AutoISClicker.Utilities.UCO, AutoISClicker.Utilities.Password);

var hehe = iSInstance.ParseSubjectFromGroupSignUp("https://is.muni.cz/auth/seminare/student?fakulta=1433;obdobi=8404;studium=1144620;predmet=1406159;prihlasit=667837;akce=podrob;provest=1;stopwindow=1;design=m");
*/

//return;



// iSInstance.SignUpForGroup("https://is.muni.cz/auth/seminare/student?fakulta=1433;obdobi=8404;studium=1144620;predmet=1406159;prihlasit=667837;akce=podrob;provest=1;stopwindow=1;design=m");

AutoISClicker.Utilities.RunRealTask();

Console.WriteLine("Operations: " + AutoISClicker.Utilities.OperationCounter);

Console.ReadLine();

Console.WriteLine("Operations: " + AutoISClicker.Utilities.OperationCounter);

// driver.Quit();
