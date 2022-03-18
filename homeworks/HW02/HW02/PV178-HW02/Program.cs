using HW02;

Console.WriteLine("Ahoj");

// Using the included classes
// var smth = FormulaAPI.Entities.


//var neco = FormulaAPI.F1.GetDrivers(limit: 1000); // max 853
//var neco1 = FormulaAPI.F1.GetStatuses(limit: 1000); // max 137
//var neco2 = FormulaAPI.F1.GetResults(limit: 1000, offset: 1000); // offset and limit 

//var smth = FormulaAPI.F1.GetDriver(1);
//var smth1 = FormulaAPI.F1.GetStatus(2);
//;

RequestParser parser = new RequestParser();
parser.ParseCommands();


Console.WriteLine("Konec");