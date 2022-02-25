namespace HW01
{
    /**
     * <summary>Class for "general knowledge" about the game and its constants.</summary>
     **/
    internal static class Constants
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
            rip
        }

        public static string[] KnownCommands = { "inspect", "fight", "info", "reorder", "rip" };

        public const int DiamondShards = 7;
        public const int Caves = DiamondShards;

        public const int PlayerSquadSize = 3;
        public const int EnemySquadSize = PlayerSquadSize;

        public const int EffectiveAttackModifier = 2;
        public const int BaseAttackModifier = 1;

        public const float BailoutPenalty = 0.5f;

        public const int MinDMGIncrease = 1;
        public const int MaxDMGIncrease = 3;

        public const int MinHPIncrease = 1;
        public const int MaxHPIncrease = 4;

        public const int LevelUpThreshold = 100;

        public const int MinExpReward = 50;
        public const int MaxExpReward = 150;

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
