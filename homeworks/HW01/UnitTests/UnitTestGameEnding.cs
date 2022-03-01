using HW01;
using NUnit.Framework;
using System.Linq;

namespace UnitTests
{
    public class Tests
    {

        [Test]
        public void TestLoss()
        {
            Game game = new Game(null, null);
            game.Dungeons[0] = new Dungeon(Entities.EnemyList.Take(Constants.EnemySquadSize).ToArray());
            game.PlayerData.SetAdventurers(Entities.AdventurerList.Take(Constants.PlayerSquadSize).ToArray());

            foreach (var enemy in game.Dungeons[0].Enemies)
            {
                enemy.Speed += 1;
            }

            Constants.MinExpReward = 10;
            Constants.MaxExpReward = 10;
            Constants.LevelUpThreshold = 100;

            game.ParsePlayerCommand(false, Constants.Commands.fight);

            for (int i = 0; i < Constants.PlayerSquadSize; i++)
            {
                Assert.True(game.PlayerData.Adventurers[i].Attack == Entities.AdventurerList[i].Attack);
                Assert.True(game.PlayerData.Adventurers[i].Hitpoints == Entities.AdventurerList[i].Hitpoints);
            }

            for (int i = 0; i < Constants.EnemySquadSize; i++)
            {
                Assert.True(game.PlayerData.Adventurers[i].Attack == Entities.AdventurerList[i].Attack);
                Assert.True(game.PlayerData.Adventurers[i].Hitpoints == Entities.AdventurerList[i].Hitpoints);
            }

            Assert.True(game.PlayerData.DiamondPieces == 0);
        }

        [Test]
        public void TestWin()
        {
            Game game = new Game(null, null);
            game.Dungeons[0] = new Dungeon(Entities.EnemyList.Take(Constants.EnemySquadSize).ToArray());
            game.PlayerData.SetAdventurers(Entities.AdventurerList.Take(Constants.PlayerSquadSize).ToArray());

            Constants.MinExpReward = 100;
            Constants.MaxExpReward = 100;
            Constants.LevelUpThreshold = 100;

            Constants.MinDMGIncrease = 1;
            Constants.MinDMGIncrease = 1;

            game.ParsePlayerCommand(false, Constants.Commands.fight);

            for (int i = 0; i < Constants.PlayerSquadSize; i++)
            {
                Assert.True(game.PlayerData.Adventurers[i].Attack > Entities.AdventurerList[i].Attack);
                Assert.True(game.PlayerData.Adventurers[i].Hitpoints > Entities.AdventurerList[i].Hitpoints);
                Assert.True(game.PlayerData.Adventurers[i].Level > 1);
            }

            Assert.True(game.PlayerData.DiamondPieces == 1);
        }

        [Test]
        public void ConstantsMakeSense()
        {
            Assert.True(Constants.MinExpReward <= Constants.MaxExpReward);
            Assert.True(Constants.MinDMGIncrease <= Constants.MaxDMGIncrease);
            Assert.True(Constants.MinHPIncrease <= Constants.MaxHPIncrease);
        }
    }
}