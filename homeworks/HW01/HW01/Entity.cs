namespace HW01
{
    internal class Entity
    {
        public string Name { get; }
        public int Attack { get; protected set; }
        public int Hitpoints { get; set; }
        protected int MaxHitpoints { get; set; }
        public int Speed { get; }
        public Constants.Colours Colour { get; }

        public int Experience = 0;
        public int Level = 1;

        public Entity(string name, int attack, int hitpoints, int speed, Constants.Colours colour)
        {
            this.Name = name;
            this.Attack = attack;
            this.Hitpoints = hitpoints;
            this.MaxHitpoints = hitpoints;
            this.Speed = speed;
            this.Colour = colour;
        }

        public Entity((string Name, int Attack, int Hitpoints, int Speed, Constants.Colours Colour) EntityTuple) :
            this(EntityTuple.Name, EntityTuple.Attack, EntityTuple.Hitpoints, EntityTuple.Speed, EntityTuple.Colour)
        {
        }

        public int AttackEntity(Entity attacked)
        {
            int attackValue = this.Attack * Constants.EffectiveAttack(this.Colour, attacked.Colour);
            attacked.Hitpoints -= attackValue;

            Utilities.PrintColouredName(this);
            Utilities.InputOutputHandler.Write(" dealt " + attackValue + " damage to ");
            Utilities.PrintColouredName(attacked);
            Utilities.InputOutputHandler.WriteLine(".");

            if (attacked.Hitpoints > 0)
            {
                Utilities.PrintColouredName(attacked);
                Utilities.InputOutputHandler.WriteLine(" has currently " + attacked.Hitpoints + " HP.");
            }

            return attacked.Hitpoints;
        }

        public bool AddExp(int exp)
        {
            this.Experience += exp;

            if (this.Experience >= Constants.LevelUpThreshold)
            {
                this.LevelUp(this.Experience / Constants.LevelUpThreshold);
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
                this.MaxHitpoints += rnd.Next(Constants.MinHPIncrease, Constants.MaxHPIncrease + 1);

                Utilities.LevelUpMessage(this);
            }
        }

        public override string ToString()
        {
            return Name + ": " + Attack + " Attack, " + Hitpoints + " HP, " + Speed + " Speed";
        }

        public void ResetHP()
        {
            this.Hitpoints = this.MaxHitpoints;
            ;
        }
    }
}
