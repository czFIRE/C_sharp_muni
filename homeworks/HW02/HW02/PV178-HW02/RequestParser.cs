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
        public int ParseCommands()
        {
            Console.WriteLine("Please select one of the modes:" +
                            "\n1) Driver name" +
                            "\n2) Nationality");

            int choice;
            if (! int.TryParse(Console.ReadLine(), out choice))
            {
                Console.Error.WriteLine("Invalid command!");
                return Constants.ERROR_UNSUPPORTED;
            }

            switch (choice)
            {
                case 1:
                    return NameVariant();
                
                case 2:
                    return NationalityVariant();    

                default:
                    Console.Error.WriteLine("Such a mode isn't supported!");
                    return Constants.ERROR_UNSUPPORTED;
            }
        }

        private int SaveProgress(List<(int, string)> failures)
        {
            Console.WriteLine("Please specify file name to which you want to save the data:");
            string filename = Console.ReadLine();
            
            if (filename == null)
            {
                Console.Error.WriteLine("Filename is null");
                return Constants.ERROR_NULL;
            }

            if (!filename.Contains(".csv"))
            {
                filename += ".csv";
            }

            CsvExporter.Export(filename, failures);
            ModelGenerator.RunProcessMining(filename);

            return Constants.SUCCESS;
        }

        private int NameVariant()
        {
            Console.WriteLine("Please input forname:");
            string forename = Console.ReadLine();
            Console.WriteLine("Please input surname:");
            string surname = Console.ReadLine();

            if (forename == null || surname == null)
            {
                Console.Error.WriteLine("Name is null!");
                return Constants.ERROR_NULL;
            }

            var failures = DriverLogic.GetDriverFailures(forename, surname, rejectedStatuses);

            if (failures == null || failures.Count == 0)
            {
                Console.Error.WriteLine("Such a driver doesn't exist!");
                return Constants.ERROR_NULL;
            }

            return SaveProgress(failures);
        }

        private int NationalityVariant()
        {
            Console.WriteLine("Please input nationality:");
            string nationality = Console.ReadLine();

            if (nationality == null)
            {
                Console.Error.WriteLine("Name is null!");
                return Constants.ERROR_NULL;
            }

            var failures = NationalityLogic.GetNationalityFailures(nationality, rejectedStatuses);

            if (failures == null || failures.Count == 0)
            {
                Console.Error.WriteLine("Such a nationality doesn't exist!");
                return Constants.ERROR_NULL;
            }

            return SaveProgress(failures);
        }
    }
}
