namespace HW01
{
    internal class Enemy : Entity
    {
        public Enemy((string, int, int, int, Constants.Colours) p) : base(p)
        {
        }

        public Enemy(string name, int attack, int hitpoints, int speed, Constants.Colours colour) : base(name, attack, hitpoints, speed, colour)
        {
        }
    }
}
