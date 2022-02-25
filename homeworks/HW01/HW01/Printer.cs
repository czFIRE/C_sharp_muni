﻿namespace HW01
{
    internal static class Printer
    {
        private static IIOHandler InputOutputHandler { get; set; } = new IOHandler();

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

        public static void PrintEntityWithLevels(Entity entity)
        {
            PrintColouredName((ConsoleColor)entity.Colour, entity.Name);

            InputOutputHandler.WriteLine(": " + entity.Attack + " Attack, " + entity.Hitpoints + " HP, " + entity.Speed + " Speed, Level "
                              + entity.Level + ", " + entity.Experience + "/" + Constants.LevelUpThreshold + "XP");
        }

        public static void PrintIncorectCommandError(string command)
        {
            InputOutputHandler.WriteLine("Command '" + command + "' is invalid!");
        }

        public static void PrintSupportedCommands()
        {
            InputOutputHandler.WriteLine("Your commands are: {0}", string.Join(", ", Enum.GetNames(typeof(Constants.Commands))));
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
