namespace HW01
{
    internal class Dungeon
    {
        public Enemy[] Enemies { get; private set; }

        public Dungeon(Enemy[] dungeonDeffenders)
        {
            Enemies = dungeonDeffenders;
        }

        public Dungeon((string Name, int Attack, int Hitpoints, int Speed, Constants.Colours Colour)[] EnemyList)
        {
            Enemies = new Enemy[Constants.EnemySquadSize];

            for (int i = 0; i < Constants.EnemySquadSize; i++)
            {
                Enemies[i] = new Enemy(EnemyList[i]);
            }
        }

        public void PrintDungeonDeffenders()
        {
            foreach (Enemy enemy in Enemies)
            {
                Utilities.PrintEntityBasic(enemy);
            }
        }
    }
}
