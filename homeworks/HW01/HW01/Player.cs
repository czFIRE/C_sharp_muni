namespace HW01
{
    /**
    * <summary>Contains general player information</summary>
    **/
    internal class Player
    {
        public Adventurer[] Adventurers { get; private set; }
        public int DiamondPieces { get; set; } = 0;
        public int DungeonNumber { get; set; } = 0;

        public Player((string Name, int Attack, int Hitpoints, int Speed, Constants.Colours Colour)[] AdventurerList)
        {
            Adventurers = GetPlayerAdventurers(AdventurerList);
        }

        public void PlayerWon()
        {
            Random rnd = new Random();
            for (int i = 0; i < Adventurers.Length; i++)
            {
                if (Adventurers[i].AddExp(rnd.Next(Constants.MinExpReward, Constants.MaxExpReward)))
                {
                    Printer.LevelUpMessage(Adventurers[i]);
                }
            }
        }

        public void PlayerLost()
        {
            Random rnd = new Random();
            for (int i = 0; i < Adventurers.Length; i++)
            {
                if (Adventurers[i].AddExp((int)(rnd.Next(Constants.MinExpReward, Constants.MaxExpReward) * Constants.LossPenalty)))
                {
                    Printer.LevelUpMessage(Adventurers[i]);
                }
            }
        }

        public static int[] GetPlayerChoices(int maxAllowedNumber)
        {
            int[] playerChoices = new int[Constants.PlayerSquadSize];

            bool parsed = false;

            do
            {
                parsed = true;

                string? line = IOHandler.ReadLine();
                if (line == null)
                {
                    parsed = false;
                    continue;
                }

                string[] picks = line.Split(' ');

                if (picks.Length != Constants.PlayerSquadSize)
                {
                    IOHandler.WriteLine("Incorrect amount of numbers! Try again:");
                    parsed = false;
                    continue;
                }

                for (int i = 0; i < Constants.PlayerSquadSize; i++)
                {
                    if (!int.TryParse(picks[i], out playerChoices[i]))
                    {
                        IOHandler.WriteLine("One of the inputs wasn't a number! Try again:");
                        parsed = false;
                        break;
                    }

                    playerChoices[i]--;

                    if (playerChoices[i] < 0 || playerChoices[i] > maxAllowedNumber)
                    {
                        IOHandler.WriteLine("Number is out of range! Try again:");
                        parsed = false;
                        break;
                    }

                    for (int j = 0; j < i; j++)
                    {
                        if (playerChoices[i] == playerChoices[j])
                        {
                            IOHandler.WriteLine("You chose the same number multiple times! Try again:");
                            i = Constants.PlayerSquadSize;
                            parsed = false;
                            break;
                        }
                    }
                }

            } while (!parsed);

            return playerChoices;
        }

        public static Adventurer[] GetPlayerAdventurers((string Name, int Attack, int Hitpoints, int Speed, Constants.Colours Colour)[] AdventurerList)
        {
            for (int i = 0; i < AdventurerList.Length; i++)
            {
                IOHandler.Write((i + 1) + ": ");
                Printer.PrintEntityBasic(AdventurerList[i]);
            }

            IOHandler.WriteLine("\nPlease select your fighters ( " + Constants.PlayerSquadSize + " numbers on a line):");

            int[] choices = GetPlayerChoices(AdventurerList.Length);

            Adventurer[] adventurers = new Adventurer[Constants.PlayerSquadSize];

            for (int i = 0; i < choices.Length; i++)
            {
                adventurers[i] = new Adventurer(AdventurerList[choices[i]]);
            }

            return adventurers;
        }
    }
}
