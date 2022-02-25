namespace HW01
{
    internal static class Utilities
    {
        public static IIOHandler InputOutputHandler { get; set; } = new IOHandler();

        // This should probably take EntityColour instead of Console color
        public static void PrintColouredName(ConsoleColor colour, string name)
        {
            ConsoleColor currentColour = InputOutputHandler.GetForegroundColor();

            InputOutputHandler.SetForegroundColour(colour);
            InputOutputHandler.Write(name);
            InputOutputHandler.SetForegroundColour(currentColour);
        }

        public static void PrintEntityBasic(Entity entity)
        {
            PrintColouredName((ConsoleColor)entity.Colour, entity.Name);

            InputOutputHandler.WriteLine(": " + entity.Attack + " Attack, " + entity.Hitpoints + " HP, " + entity.Speed + " Speed");
        }

        public static void PrintEntityBasic((string Name, int Attack, int Hitpoints, int Speed, Constants.Colours Colour) EntityTuple)
        {
            PrintColouredName((ConsoleColor) EntityTuple.Colour, EntityTuple.Name);

            InputOutputHandler.WriteLine(": " + EntityTuple.Attack + " Attack, " + EntityTuple.Hitpoints + " HP, " + EntityTuple.Speed + " Speed");
        }

        public static void PrintEntityWithLevels(Entity entity)
        {
            PrintColouredName((ConsoleColor)entity.Colour, entity.Name);

            InputOutputHandler.WriteLine(": " + entity.Attack + " Attack, " + entity.Hitpoints + " HP, " + entity.Speed + " Speed, Level "
                              + entity.Level + ", " + entity.Experience + "/" + Constants.LevelUpThreshold + "XP");
        }

        public static void PrintIncorectCommandError(string command)
        {
            InputOutputHandler.WriteLine("Command '" + command + "' is invalid! \nPlease try again:");
        }

        public static void PrintSupportedCommands()
        {
            var stuff = Enum.GetNames(typeof(Constants.Commands));
            InputOutputHandler.WriteLine("Your commands are: " + string.Join(", ", stuff));
        }


        public static void LevelUpMessage(Entity entity)
        {
            PrintColouredName((ConsoleColor)entity.Colour, entity.Name);
            InputOutputHandler.WriteLine(" has leveled up!");
        }

        public static void LevelUpMessage(Adventurer adventurer)
        {
            InputOutputHandler.Write("Adventurer ");
            LevelUpMessage((Entity) adventurer);
        }
    }
}
