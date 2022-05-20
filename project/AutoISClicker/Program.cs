using AutoISClicker;

Console.WriteLine("Welcome to AutoISClicker!\n");

AutoISClicker.Utilities.GetUserLoginData();

Console.WriteLine("\nWhich mode would you like to do:");

foreach (var value in Enum.GetValues(typeof(Timetable.CreationMode)))
{
    Console.WriteLine('\t' + value.ToString());
}
Console.WriteLine();

Timetable.CreationMode mode;
try
{
    mode = (Timetable.CreationMode)Enum.Parse(typeof(Timetable.CreationMode), Console.ReadLine());
}
catch
{
    Console.Error.WriteLine("Your typing skills are hilariously bad! Try again next time!");
    throw;
}

string tmp = mode == Timetable.CreationMode.SAVED_XML ? "folder" : "file";

Console.WriteLine($"\nPlease input the {tmp} with the timetable:");

Timetable timetable = new Timetable(Console.ReadLine(), mode);

Console.WriteLine("\nPlease input the folder with registration load:");
string regLoc = Console.ReadLine();


Console.WriteLine("\nNow select the mode you wanna run (input the number in front):");

var type = typeof(IRunMode);
var types = AppDomain.CurrentDomain.GetAssemblies()
    .SelectMany(s => s.GetTypes())
    .Where(p => type.IsAssignableFrom(p))
    .Where(type => !type.IsAbstract &&
                   !type.IsGenericType)
    .Select(type => type.GetConstructor(Type.EmptyTypes))
    .ToList();

for (int i = 0; i < types.Count(); i++)
{
    Console.WriteLine($"{i})\t{types[i].DeclaringType.Name}");
}
Console.WriteLine();

IRunMode modeToRun;
try
{
    int runMode = Int32.Parse(Console.ReadLine());
    modeToRun = (IRunMode)types[runMode].Invoke(new object[] { });
}
catch
{
    Console.Error.WriteLine("Your typing skills are hilariously bad! Try again next time!");
    throw;
}

modeToRun.RunTask(timetable, regLoc);

System.GC.Collect();
return;

/*

 Cheatsheet:

./../../../../data
ISEXPORT
./../../../data/sample_timetable.xml
./../../../data/subjects/
0

*/