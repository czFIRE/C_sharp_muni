﻿namespace HW01
{
    public class Dungeon
    {
        public Enemy[] Enemies { get; set; }

        public int EnemiesAlive { get; set; } = Constants.EnemySquadSize;

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

        public void ResetEnemyHP()
        {
            foreach (Enemy enemy in Enemies)
            {
                enemy.ResetHP();
            }

            EnemiesAlive = Constants.PlayerSquadSize;
        }

        public void LevelUpEnemies(int levels)
        {
            foreach (Enemy enemy in Enemies)
            {
                enemy.LevelUp(levels, false);
                enemy.ResetHP();
            }
        }
    }
}
