namespace HW01
{
    public class Adventurer : Entity
    {
        public Adventurer((string, int, int, int, Constants.Colours) p) : base(p)
        {
        }

        public Adventurer(string name, int attack, int hitpoints, int speed, Constants.Colours colour) : base(name, attack, hitpoints, speed, colour)
        {
        }
    }
}
