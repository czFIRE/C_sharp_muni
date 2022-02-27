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

        public int AdventurersAlive { get; set; } = Constants.PlayerSquadSize;

        public Player((string Name, int Attack, int Hitpoints, int Speed, Constants.Colours Colour)[] AdventurerList)
        {
            Adventurers = GetPlayerAdventurers(AdventurerList);
        }

        public void PlayerWon()
        {
            Random rnd = new Random();
            int expAmount = rnd.Next(Constants.MinExpReward, Constants.MaxExpReward);

            Utilities.InputOutputHandler.WriteLine("You have won the match! Adventurers gain " + expAmount + " XP! You acquired the diamond piece!");

            for (int i = 0; i < Adventurers.Length; i++)
            {
                Adventurers[i].AddExp(expAmount);
            }

            DiamondPieces++;
            DungeonNumber++;

            this.ResetAdventurerHP();
        }

        public void PlayerLost()
        {
            Random rnd = new Random();
            int expAmount = (int)(rnd.Next(Constants.MinExpReward, Constants.MaxExpReward) * Constants.LossPenalty);

            Utilities.InputOutputHandler.WriteLine("You have lost the match! Adventurers gain " + expAmount + " XP!");

            for (int i = 0; i < Adventurers.Length; i++)
            {
                if (Adventurers[i].AddExp(expAmount))
                {
                    Utilities.LevelUpMessage(Adventurers[i]);
                }
            }

            this.ResetAdventurerHP();
        }

        public static int[] GetPlayerChoices(int maxAllowedNumber)
        {
            int[] playerChoices = new int[Constants.PlayerSquadSize];

            bool parsed = false;

            do
            {
                parsed = true;

                string? line = Utilities.InputOutputHandler.ReadLine();
                if (line == null)
                {
                    parsed = false;
                    continue;
                }

                line = line.Trim();

                string[] picks = line.Split(' ');

                if (picks.Length != Constants.PlayerSquadSize)
                {
                    Utilities.InputOutputHandler.WriteLine("Incorrect amount of numbers! Try again:");
                    parsed = false;
                    continue;
                }

                for (int i = 0; i < Constants.PlayerSquadSize; i++)
                {
                    if (!int.TryParse(picks[i], out playerChoices[i]))
                    {
                        Utilities.InputOutputHandler.WriteLine("One of the inputs wasn't a number! Try again:");
                        parsed = false;
                        break;
                    }

                    playerChoices[i]--;

                    if (playerChoices[i] < 0 || playerChoices[i] >= maxAllowedNumber)
                    {
                        Utilities.InputOutputHandler.WriteLine("Number is out of range! Try again:");
                        parsed = false;
                        break;
                    }

                    for (int j = 0; j < i; j++)
                    {
                        if (playerChoices[i] == playerChoices[j])
                        {
                            Utilities.InputOutputHandler.WriteLine("You chose the same number multiple times! Try again:");
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
                Utilities.InputOutputHandler.Write((i + 1) + ": ");
                Utilities.PrintEntityBasic(AdventurerList[i]);
            }

            Utilities.InputOutputHandler.WriteLine("\nPlease select your fighters (" + Constants.PlayerSquadSize + " numbers on a line):");

            int[] choices = GetPlayerChoices(AdventurerList.Length);

            Adventurer[] adventurers = new Adventurer[Constants.PlayerSquadSize];

            for (int i = 0; i < choices.Length; i++)
            {
                adventurers[i] = new Adventurer(AdventurerList[choices[i]]);
            }

            return adventurers;
        }

        public void ReorderAdventurers()
        {
            Utilities.InputOutputHandler.WriteLine("Choose new order of your adventurers:");

            foreach (Adventurer adventurer in Adventurers)
            {
                Utilities.PrintEntityWithLevels(adventurer);
            }

            int[] choices = GetPlayerChoices(Constants.PlayerSquadSize);

            Adventurer[] adventurers = new Adventurer[Constants.PlayerSquadSize];

            for (int i = 0; i < Constants.PlayerSquadSize; i++)
            {
                adventurers[i] = Adventurers[choices[i]];
            }

            Adventurers = adventurers;

            Utilities.InputOutputHandler.WriteLine("The new order of your adventurers:");

            foreach (Adventurer adventurer in Adventurers)
            {
                Utilities.PrintEntityWithLevels(adventurer);
            }
        }

        public void PrintPlayerInfo()
        {
            Utilities.InputOutputHandler.WriteLine("Diamond pieces collected: " + DiamondPieces);
            Utilities.InputOutputHandler.WriteLine("Your Adventurers:");

            foreach (Adventurer adventurer in Adventurers)
            {
                Utilities.PrintEntityWithLevels(adventurer);
            }
        }

        public void ResetAdventurerHP()
        {
            foreach (Adventurer adventurer in Adventurers)
            {
                adventurer.ResetHP();
            }

            AdventurersAlive = Constants.PlayerSquadSize;
        }
    }
}
