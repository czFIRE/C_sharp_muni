namespace HW01
{
    /**
    * <summary>Contains general player information</summary>
    **/
    internal class Player
    {
        public Adventurer[] Adventurers { get; private set; }
        public int diamondPieces { get; set; }

        public Player(Adventurer[] playerChoices)
        {
            Adventurers = playerChoices;
        }
    }
}
