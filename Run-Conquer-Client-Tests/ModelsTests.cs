using AssemblyCSharp;
using NUnit.Framework;

namespace RunConquerClientTests
{
    [TestFixture]
    public class Test
    {
        [Test]
        public void GameInstanceMapAssociation()
        {
            var map = new MapModel();
            var gameInstance = new GameInstanceModel();
            Assert.IsNull(gameInstance.Map);

            gameInstance.Map = map;
            Assert.AreEqual(gameInstance.Map, map);
        }

        [Test]
        public void GameInstancePlayerAssociation()
        {
            var gameInstance = new GameInstanceModel();
            var player1 = new PlayerModel(1);
            var player2 = new PlayerModel(2);

            CollectionAssert.IsEmpty(gameInstance.Players);

            gameInstance.Players.Add(player1);
            CollectionAssert.Contains(gameInstance.Players, player1);
            CollectionAssert.DoesNotContain(gameInstance.Players, player2);

            Assert.AreEqual(gameInstance, player1.GameInstance);
            Assert.AreNotEqual(gameInstance, player2.GameInstance);

            gameInstance.Players.Remove(player1);
            CollectionAssert.DoesNotContain(gameInstance.Players, player1);
        }

        [Test]
        public void GameInstanceTeamAssociation()
        {
            var gameInstance = new GameInstanceModel();
            var team1 = new TeamModel(1);
            var team2 = new TeamModel(2);

            CollectionAssert.IsEmpty(gameInstance.Teams);
            Assert.IsNull(team1.GameInstance);
            Assert.IsNull(team2.GameInstance);

            gameInstance.Teams.Add(team1);
            Assert.AreEqual(gameInstance, team1.GameInstance);
            CollectionAssert.Contains(gameInstance.Teams, team1);

            gameInstance.Teams.Remove(team1);
            Assert.IsNull(team1.GameInstance);
            CollectionAssert.DoesNotContain(gameInstance.Teams, team1);

            team2.GameInstance = gameInstance;
            Assert.AreEqual(gameInstance, team2.GameInstance);
            CollectionAssert.Contains(gameInstance.Teams, team2);

            gameInstance.Teams.Clear();
            Assert.IsNull(team2.GameInstance);
            CollectionAssert.IsEmpty(gameInstance.Teams);
        }

        [Test]
        public void TeamPlayerAssociation()
        {
            var team1 = new TeamModel(1);
            var team2 = new TeamModel(2);
            var player1 = new PlayerModel(1);
            var player2 = new PlayerModel(2);

            CollectionAssert.IsEmpty(team1.Players);
            CollectionAssert.IsEmpty(team2.Players);
            Assert.IsNull(player1.Team);
            Assert.IsNull(player2.Team);

            team1.Players.Add(player1);
            CollectionAssert.Contains(team1.Players, player1);

            team1.Players.Remove(player1);
            CollectionAssert.DoesNotContain(team1.Players, player1);

            team2.Players.Add(player1);
            CollectionAssert.Contains(team2.Players, player1);

            team1.Players.Add(player1);
            CollectionAssert.Contains(team1.Players, player1);
            CollectionAssert.DoesNotContain(team2.Players, player1);

            team1.Players.Add(player2);
            CollectionAssert.Contains(team1.Players, player2);

            team1.Players.Clear();
            CollectionAssert.DoesNotContain(team1.Players, player1);
            CollectionAssert.DoesNotContain(team1.Players, player2);
            Assert.IsNull(player1.Team);
            Assert.IsNull(player2.Team);

            player1.Team = team1;
            Assert.AreEqual(team1, player1.Team);
            CollectionAssert.Contains(team1.Players, player1);

            player1.Team = team2;
            Assert.AreEqual(player1.Team, team2);
            CollectionAssert.Contains(team2.Players, player1);
            CollectionAssert.DoesNotContain(team1.Players, player1);
        }
    }
}