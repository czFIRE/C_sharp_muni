namespace HW01
{
    internal class Entity
    {
        public string Name { get; }
        public int Attack { get; protected set; }
        public int Hitpoints { get; protected set; }
        public int Speed { get; }
        public Constants.Colours Colour { get; }

        public int Experience = 0;
        public int Level = 1;

        public Entity(string name, int attack, int hitpoints, int speed, Constants.Colours colour)
        {
            this.Name = name;
            this.Attack = attack;
            this.Hitpoints = hitpoints;
            this.Speed = speed;
            this.Colour = colour;
        }

        public Entity((string, int, int, int, Constants.Colours) EntityTuple) :
            this(EntityTuple.Item1, EntityTuple.Item2, EntityTuple.Item3, EntityTuple.Item4, EntityTuple.Item5)
        {
        }

        public int AttackEntity(Entity attacked)
        {
            attacked.Hitpoints -= this.Attack * Constants.EffectiveAttack(this.Colour, attacked.Colour);
            return attacked.Hitpoints;
        }

        public bool AddExp(int exp)
        {
            this.Experience += exp;

            if (this.Experience >= Constants.LevelUpThreshold)
            {
                this.LevelUp(exp / Constants.LevelUpThreshold);
                this.Experience %= Constants.LevelUpThreshold;

                return true;
            }
            return false;
        }

        public void LevelUp(int levels)
        {
            Random rnd = new Random();

            for (int i = 0; i < levels; i++)
            {
                this.Attack += rnd.Next(Constants.MinDMGIncrease, Constants.MaxDMGIncrease + 1);
                this.Hitpoints = rnd.Next(Constants.MinHPIncrease, Constants.MaxHPIncrease + 1);
            }
        }

    }
}
