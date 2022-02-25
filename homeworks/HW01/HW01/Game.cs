namespace HW01
{
    internal class Game : IGame
    {
        public Player PlayerData { get; set; }

        public Dungeon[] Dungeons { get; set; }

        public Game()
        {
            // an argument could be made that it should be generated on the fly and not in advance
            Dungeons = new Dungeon[Constants.DungeonCount];
            Random rnd = new Random();
            for (int i = 0; i < Dungeons.Length; i++)
            {
                Dungeons[i] = new Dungeon(Entities.EnemyList.OrderBy(x => rnd.Next()).Take(Constants.EnemySquadSize).ToArray());
            }

            PlayerData = new Player(Entities.AdventurerList);
        }

        public int Run()
        {
            bool loop = true;

            Utilities.PrintSupportedCommands();

            while (loop)
            {
                if (ParsePlayerCommand() != 0)
                {
                    loop = false;
                }
            }

            // jiny return value
            return 0;
        }

        public int ParsePlayerCommand()
        {
            var command = Utilities.InputOutputHandler.ReadLine();

            if (!Enum.TryParse(command, out Constants.Commands cmd))
            {
#pragma warning disable CS8604 // Possible null reference argument, but it doesn't matter for writing.
                Utilities.PrintIncorectCommandError(command);
#pragma warning restore CS8604 // Possible null reference argument, but it doesn't matter for writing.
                return 0;
            }

            switch (cmd)
            {
                case Constants.Commands.inspect:
                    Utilities.InputOutputHandler.WriteLine("Next diamond piece is guarded by these enemies: ");
                    Dungeons[PlayerData.DungeonNumber].PrintDungeonDeffenders();
                    break;
                case Constants.Commands.fight:
                    Console.WriteLine("Not implemented");
                    break;
                case Constants.Commands.info:
                    PlayerData.PrintPlayerInfo();
                    break;
                case Constants.Commands.reorder:
                    PlayerData.ReorderAdventurers();
                    break;
                case Constants.Commands.rip:
                    Utilities.InputOutputHandler.WriteLine("You have lost the game :(");
                    return 1;
                // new unsupported command
                default:
                    Utilities.InputOutputHandler.WriteLine("Internal error, add new commnad to switch");
                    return 2;
            }

            return 0;
        }
    }
}
