namespace HW01
{
    /**
     * <summary>Class for "general knowledge" about the game and its constants.</summary>
     **/
    public static class Constants
    {
        public enum Colours
        {
            Red = ConsoleColor.Red,
            Green = ConsoleColor.Green,
            Blue = ConsoleColor.Blue,
        }

        public enum Commands
        {
            inspect,
            fight,
            info,
            reorder,
            rip,
            help
        }

        public static int DiamondShards = 7;
        public static int DungeonCount = DiamondShards;

        public static int PlayerSquadSize = 3;
        public static int EnemySquadSize = PlayerSquadSize;

        public static int EffectiveAttackModifier = 2;
        public static int BaseAttackModifier = 1;

        public static float LossPenalty = 0.5f;

        public static int MinDMGIncrease = 1;
        public static int MaxDMGIncrease = 3;

        public static int MinHPIncrease = 1;
        public static int MaxHPIncrease = 4;

        public static int LevelUpThreshold = 100;

        public static int MinExpReward = 50;
        public static int MaxExpReward = 150;

        public static int EffectiveAttack(Colours attacking, Colours attacked)
        {
            return (attacking, attacked) switch
            {
                (Colours.Red, Colours.Green) or (Colours.Blue, Colours.Red) or (Colours.Green, Colours.Blue) => EffectiveAttackModifier,
                _ => BaseAttackModifier,
            };
        }
    }
}
