namespace HW01
{
    /**
     * <summary> Class storing all the possible entities. </summary>
     **/
    internal static class Entities
    {
        // Yes, this should be done as a input from a file
        // No, I am not doing it, I am lazy :D
        private static (string Name, int Attack, int Hitpoints, int Speed, Constants.Colours Colour)[] EntityList =
        {
            ("Thason", 4, 6, 4, Constants.Colours.Blue),
            ("Geralt", 6, 4, 4, Constants.Colours.Red),
            ("Jatasi", 5, 6, 4, Constants.Colours.Green),
            ("Jaina", 3, 6, 4, Constants.Colours.Blue),
            ("Razziash", 2, 10, 3, Constants.Colours.Red),
            ("Shrek", 6, 10, 2, Constants.Colours.Green),
            ("Hermione", 2, 9, 5, Constants.Colours.Blue),
            ("Tyrael", 2, 15, 4, Constants.Colours.Red),
            ("Legolas", 6, 1, 5, Constants.Colours.Green),

        };

        public static (string Name, int Attack, int Hitpoints, int Speed, Constants.Colours Colour)[] AdventurerList = EntityList;

        public static (string Name, int Attack, int Hitpoints, int Speed, Constants.Colours Colour)[] EnemyList = EntityList;

        // I should probably make a method to generate a random entity as an enemy
    }
}
