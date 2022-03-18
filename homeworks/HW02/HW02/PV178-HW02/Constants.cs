namespace HW02
{
    internal static class Constants
    {
        public static readonly string[] notInclude = { "Finished", "Disqualified", "Retired", "Withdrew", "Not classified",
            "Did not qualify", "Did not prequalify", "Safety concerns", "Driver unwell", "Excluded", "Eye injury", "Illness",
            " Lap"};

        public static int MIN_RESULTS = 10;

        public static int LIMIT = 1000;

        public static int SUCCESS = 0;
        public static int ERROR_UNSUPPORTED = 1;
        public static int ERROR_NULL = 2;
    }
}
