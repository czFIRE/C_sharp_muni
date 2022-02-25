namespace HW01
{
    internal class GameLoopHandler : IGame
    {
        public Player PlayerData { get; set; }

        public Dungeon[] Dungeons { get; set; }

        public GameLoopHandler()
        {
            Dungeons = new Dungeon[Constants.DungeonCount];
            Random rnd = new Random();
            for (int i = 0; i < Dungeons.Length; i++)
            {
                var loadedData = Entities.EnemyList.OrderBy(x => rnd.Next()).Take(Constants.EnemySquadSize).ToArray();
                Enemy[] enemies = new Enemy[Constants.EnemySquadSize];

                for (int j = 0; j < Constants.EnemySquadSize; j++)
                {
                    enemies[j] = new Enemy(loadedData[j]);
                }

                Dungeons[i] = new Dungeon(enemies);
            }


        }

        public int Run()
        {
            // initialize player

            // initialize dungeons

            // 



            return 0;
        }



        public int ParsePlayerCommand()
        {
            var command = Console.ReadLine();

            if (Enum.TryParse(command, out Constants.Commands cmd))
            {
                Printer.PrintIncorectCommandError(command);
                return 1;
            }

            switch (cmd)
            {
                case Constants.Commands.inspect:
                    break;
                case Constants.Commands.fight:
                    break;
                case Constants.Commands.info:
                    break;
                case Constants.Commands.reorder:
                    break;
                case Constants.Commands.rip:
                    break;
                // new unsupported command
                default:
                    return 2;
            }

            return 0;
        }
    }
}
