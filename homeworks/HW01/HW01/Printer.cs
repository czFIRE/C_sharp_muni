namespace HW01
{
    internal static class Printer
    {
        private static IIOHandler inputOutputHandler = new IOHandler();
        // This should probably take EntityColour instead of Console color
        public static void PrintColouredName(ConsoleColor colour, string name)
        {
            ConsoleColor currentColour = IOHandler.GetForegroundColor();

            IOHandler.SetForegroundColour(colour);
            IOHandler.Write(name);
            IOHandler.SetForegroundColour(currentColour);
        }

        public static void PrintEntityBasic(Entity entity)
        {
            PrintColouredName((ConsoleColor)entity.Colour, entity.Name);

            IOHandler.WriteLine(": " + entity.Attack + " Attack, " + entity.Hitpoints + " HP, " + entity.Speed + " Speed");
        }

        public static void PrintEntityWithLevels(Entity entity)
        {
            PrintColouredName((ConsoleColor)entity.Colour, entity.Name);

            IOHandler.WriteLine(": " + entity.Attack + " Attack, " + entity.Hitpoints + " HP, " + entity.Speed + " Speed, Level "
                              + entity.Level + ", " + entity.Experience + "/" + Constants.LevelUpThreshold + "XP");
        }

        public static void PrintIncorectCommandError(string command)
        {
            IOHandler.WriteLine("Command '" + command + "' is invalid!");
        }

        public static void PrintSupportedCommands()
        {
            IOHandler.WriteLine("Your commands are: {0}", string.Join(", ", Enum.GetNames(typeof(Constants.Commands))));
        }
    }
}
