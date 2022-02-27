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
                Dungeons[i].LevelUpEnemies(i);
            }

            PlayerData = new Player(Entities.AdventurerList);
        }

        public int Run()
        {

            Utilities.PrintSupportedCommands();

            // TODO: Fix this loop
            // Yes, I know that break exists
            while (true)
            {
                Utilities.InputOutputHandler.WriteLine("");
                if (ParsePlayerCommand() != 0)
                {
                    break;
                }

                if (PlayerData.DiamondPieces == Constants.DiamondShards)
                {
                    Utilities.InputOutputHandler.WriteLine("You've collected all the diamond shards, congratualitions!");
                    Utilities.InputOutputHandler.WriteLine("");
                    break;
                }
            }

            return 0;
        }

        public int ParsePlayerCommand()
        {
            var command = Utilities.InputOutputHandler.ReadLine();

            Utilities.InputOutputHandler.WriteLine("");

            if (!Enum.TryParse(command, out Constants.Commands cmd))
            {
                Utilities.PrintIncorectCommandError(command);
                return 0;
            }

            switch (cmd)
            {
                case Constants.Commands.inspect:
                    Utilities.InputOutputHandler.WriteLine("Next diamond piece is guarded by these enemies: ");
                    Dungeons[PlayerData.DungeonNumber].PrintDungeonDeffenders();
                    break;
                case Constants.Commands.fight:
                    this.Fight();
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
                case Constants.Commands.help:
                    Utilities.PrintSupportedCommands();
                    break;
                // new unsupported command
                default:
                    Utilities.InputOutputHandler.WriteLine("Internal error, add new commnad to switch!");
                    return 2;
            }

            return 0;
        }

        public bool Fight()
        {
            // do the rounds

            int rounds = 0;

            Dungeon currDungeon = Dungeons[PlayerData.DungeonNumber];

            while (currDungeon.EnemiesAlive > 0 && PlayerData.AdventurersAlive > 0)
            {
                rounds++;
                Enemy currEnemy = currDungeon.Enemies[Constants.EnemySquadSize - currDungeon.EnemiesAlive];
                Adventurer currAdventurer = PlayerData.Adventurers[Constants.PlayerSquadSize - PlayerData.AdventurersAlive];

                Utilities.InputOutputHandler.Write("Round " + rounds + ": ");
                Utilities.PrintColouredName(currAdventurer);
                Utilities.InputOutputHandler.Write(" vs ");
                Utilities.PrintColouredName(currEnemy);
                Utilities.InputOutputHandler.WriteLine("");

                // choose who starts

                // do the fight

                while (currEnemy.Hitpoints > 0 && currAdventurer.Hitpoints > 0)
                {
                    if (currAdventurer.Speed >= currEnemy.Speed)
                    {
                        if (currAdventurer.AttackEntity(currEnemy) <= 0)
                            break;
                        currEnemy.AttackEntity(currAdventurer);
                    }
                    else
                    {
                        if (currEnemy.AttackEntity(currAdventurer) <= 0)
                            break;
                        currAdventurer.AttackEntity(currEnemy);
                    }
                }

                // print outcomes
                if (currEnemy.Hitpoints <= 0)
                {
                    Utilities.PrintColouredName(currEnemy);
                    Utilities.InputOutputHandler.WriteLine(" is defeated!");
                    currDungeon.EnemiesAlive--;
                }
                else
                {
                    Utilities.PrintColouredName(currAdventurer);
                    Utilities.InputOutputHandler.WriteLine(" is defeated!");
                    PlayerData.AdventurersAlive--;
                }
            }

            if (currDungeon.EnemiesAlive == 0)
            {
                PlayerData.PlayerFightEnd(true);
                return true;
            }

            currDungeon.ResetEnemyHP();
            PlayerData.PlayerFightEnd(false);
            return false;
        }
    }
}
