namespace HW02
{
    internal static class NationalityLogic
    {
        public static List<(int, string)> GetNationalityFailures(string nationality, List<int> rejectedStatuses)
        {

            // First find all drivers of a nationality and their relevant results
            List<NationalityDriver> nationalityDrivers = FindDrivers(nationality);

            if (nationalityDrivers == null || nationalityDrivers.Count() == 0)
            {
                return null;
            }

            _ = FindResults(nationalityDrivers, rejectedStatuses);

            // Get all results
            List<(int, string)> failures = new List<(int, string)>();

            foreach (NationalityDriver driver in nationalityDrivers)
            {
                if (driver.totalResults >= Constants.MIN_RESULTS)
                {
                    foreach (string failure in driver.relevantResults)
                    {
                        failures.Add((driver.driverID, failure));
                    }
                }
            }

            return failures;
        }

        private static List<NationalityDriver> FindDrivers(string nationality)
        {
            int driverOffset = 0;
            List<NationalityDriver> nationalityDrivers = new List<NationalityDriver>();

            List<FormulaAPI.Entities.Driver> drivers;
            while ((drivers = FormulaAPI.F1.GetDrivers(Constants.LIMIT, driverOffset)).Count() != 0)
            {
                foreach (FormulaAPI.Entities.Driver driver in drivers)
                {
                    if (driver.Nationality == nationality)
                    {
                        // All drivers have unique ID
                        nationalityDrivers.Add(new NationalityDriver(driver.Id));
                    }
                }

                driverOffset += Constants.LIMIT;
            }

            return nationalityDrivers;
        }

        private static List<NationalityDriver> FindResults(List<NationalityDriver> nationalityDrivers, List<int> rejectedStatuses)
        {
            int resultOffset = 0;
            List<FormulaAPI.Entities.Result> results;
            while ((results = FormulaAPI.F1.GetResults(Constants.LIMIT, resultOffset)).Count() != 0)
            {
                foreach (FormulaAPI.Entities.Result result in results)
                {
                    foreach (NationalityDriver driver in nationalityDrivers)
                    {
                        if (driver.driverID == result.DriverId)
                        {
                            driver.totalResults++;

                            var status = FormulaAPI.F1.GetStatus(result.StatusId);

                            // Go through all rejected statuses
                            if (!rejectedStatuses.Contains(status.Id))
                            {
                                driver.relevantResults.Add(status.Name);
                            }

                            // optimizition since we know, it won't match multiple times
                            break;
                        }
                    }
                }
                resultOffset += Constants.LIMIT;
            }

            return nationalityDrivers;
        }
    }
}
