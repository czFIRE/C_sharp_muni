using HW02;
using HW02.Export;
using HW02.Modelling;

namespace HW02
{
    internal class RequestParser
    {
        // Contains status IDs that should be excluded
        public List<int> rejectedStatuses = new List<int>();

        // Constructs this on program startup => could be saved and loaded for better performance
        public RequestParser()
        {
            int statusOffset = 0;
            List<FormulaAPI.Entities.Status> statuses;

            while ((statuses = FormulaAPI.F1.GetStatuses(Constants.LIMIT, statusOffset)).Count() != 0)
            {
                foreach (FormulaAPI.Entities.Status status in statuses)
                {
                    foreach (string exclude in Constants.notInclude)
                    {
                        if (status.Name.Contains(exclude))
                        {
                            rejectedStatuses.Add(status.Id);
                        }
                    }
                }
                statusOffset += Constants.LIMIT;
            }
        }

        // Parsing commands and calling 
        public void ParseCommands()
        {
            var failures = DriverLogic.GetDriverFailures("Lewis", "Hamilton", rejectedStatuses);
            CsvExporter.Export("test.csv", failures);
            ModelGenerator.RunProcessMining("test.csv");
            ;

        }
    }
}
