namespace HW02
{
    internal static class DriverLogic
    {
        public static List<String> GetDriverFailures(string forename, string surname, List<int> rejectedStatuses)
        {
            List<String> failures = new List<String>();

            // First find driver ID

            // Default value, ID is non zero
            int driverID = 0;
            int driverOffset = 0;
            List<FormulaAPI.Entities.Driver> drivers;

            while ((drivers = FormulaAPI.F1.GetDrivers(Constants.LIMIT, driverOffset)).Count() != 0)
            {
                foreach (FormulaAPI.Entities.Driver driver in drivers)
                {
                    if (driver.Forename == forename && driver.Surname == surname)
                    {
                        driverID = driver.Id;
                        break;
                    }
                }
                driverOffset += Constants.LIMIT;
            }

            // Get all results

            int resultOffset = 0;
            List<FormulaAPI.Entities.Result> results;
            while ((results = FormulaAPI.F1.GetResults(Constants.LIMIT, resultOffset)).Count() != 0)
            {
                foreach (FormulaAPI.Entities.Result result in results)
                {
                    if (result.DriverId == driverID)
                    {
                        var status = FormulaAPI.F1.GetStatus(result.StatusId);

                        // Go through all rejected statuses
                        if (!rejectedStatuses.Contains(status.Id))
                        {
                            failures.Add(status.Name);
                        } 
                    }
                }
                resultOffset += Constants.LIMIT;
            }

            return failures;
        }
    }
}
