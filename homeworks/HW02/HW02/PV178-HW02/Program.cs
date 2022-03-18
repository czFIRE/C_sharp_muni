Console.WriteLine("Ahoj");

// Using 
// var smth = FormulaAPI.Entities.

var neco = FormulaAPI.F1.GetDrivers(limit: 1000); // max 853
var neco1 = FormulaAPI.F1.GetStatuses(limit: 1000); // max 137
var neco2 = FormulaAPI.F1.GetResults(limit: 1000);
;

Console.WriteLine("Konec");