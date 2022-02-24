namespace HW01
{
    internal class Dungeon
    {
        public Enemy[] Enemies { get; private set; }

        public Dungeon(Enemy[] dungeonDeffenders)
        {
            Enemies = dungeonDeffenders;
        }
    }
}
