namespace HW01
{
    /**
    * <summary>Contains general player information</summary>
    **/
    internal class Player
    {
        public Adventurer[] Adventurers { get; private set; }
        public int DiamondPieces { get; set; } = 0;

        public Player(Adventurer[] playerChoices)
        {
            Adventurers = playerChoices;
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

        }
    }
}
