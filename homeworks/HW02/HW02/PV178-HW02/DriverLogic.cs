namespace HW02
{
    internal static class DriverLogic
    {
        // Yes, we could use the NationalityDriver class here
        // No it doesn't make sense for me to use it just for a single value
        public static List<(int, string)> GetDriverFailures(string forename, string surname, List<int> rejectedStatuses)
        {
            // First find driver ID

            int driverID = FindDriverID(forename, surname);

            if (driverID == -1)
            {
                return null;
            } 

            // Get all results
            return FindRelevantResults(driverID, rejectedStatuses);
        }

        private static int FindDriverID(string forename, string surname)
        {
            // Default value, ID is non negative
            int driverID = -1;
            int driverOffset = 0;
            List<FormulaAPI.Entities.Driver> drivers;

            while ((drivers = FormulaAPI.F1.GetDrivers(Constants.LIMIT, driverOffset)).Count() != 0)
            {
                foreach (FormulaAPI.Entities.Driver driver in drivers)
                {
                    if (driver.Forename.ToLower() == forename && driver.Surname.ToLower() == surname)
                    {
                        driverID = driver.Id;
                        break;
                    }
                }
                driverOffset += Constants.LIMIT;
            }
            
            return driverID;
        }

        private static List<(int, string)> FindRelevantResults(int driverID, List<int> rejectedStatuses)
        {
            int resultOffset = 0;
            List<(int, string)> failures = new List<(int, string)>();
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
                            failures.Add((driverID, status.Name));
                        }
                    }
                }
                resultOffset += Constants.LIMIT;
            }

            return failures;
        }
    }
}
