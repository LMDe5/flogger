using HttpServer.Framework.Handlers;
using HttpServer.Models;
using System.Net;
using System.Net.Sockets;

namespace MiniHttpServer.ORM.UnitTests
{
    [TestClass]
    public sealed class OrmTests
    {
        private static string connectionString = "Host=localhost;Port=5432;Database=myDataBase;Username=postgres;Password=piskoed228";

        [TestMethod]
        public void CreateInsertSql()
        {
            var properties = typeof(Player).GetProperties().
                Where(p => !p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase)).ToArray();

            string sql = OrmContext.CreateInsertSql("players", properties);
            Assert.AreEqual("INSERT INTO players (Name, Level) VALUES (@Name, @Level)", sql);
        }
        [TestMethod]
        public void CreateDeleteSql()
        {
            string sql = OrmContext.CreateDeleteSql("players");
            Assert.AreEqual("DELETE FROM players WHERE id = @id", sql);
        }
        [TestMethod]
        public void CreateUpdateSql()
        {
            var properties = typeof(Player).GetProperties().
                Where(p => !p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase)).ToArray();

            string sql = OrmContext.CreateUpdateSql("players", properties);
            Assert.AreEqual("UPDATE players (Name = @Name, Level = @Level) WHERE id = @id", sql);
        }

        [TestMethod]
        public async Task Insert_GetAll_Delete()
        {
            var orm = new OrmContext(connectionString);

            var newPlayer = new Player { Name = "test", Level = 99 };

            int inserted = await orm.InsertAsync("players", newPlayer);
            Assert.AreEqual(1, inserted);


            var players = await orm.GetAllAsync<Player>("players");

            var found = players.FirstOrDefault(p => p.Name == "test");
            Assert.IsNotNull(found);
            Assert.AreEqual(99, found.Level);


            await orm.DeleteAsync("players", found.Id);
        }
    }
}
