using HttpServer.Framework.Attributes;
using HttpServer.Models;
using MiniHttpServer.ORM;
using System.Numerics;

namespace HttpServer.Controllers
{
    [HttpController("player")]
    internal class PlayerController : ControllerBase
    {
        private static string connectionString = "Host=localhost;Port=5432;Database=myDataBase;Username=postgres;Password=piskoed228";
        private readonly IOrmContext ormContext = new OrmContext(connectionString);

        [Get("insert")]
        public object Insert(string name, int level)
        {
            var newPlayer = new Player { Name = name, Level = level };
            var res = ormContext.InsertAsync<Player>("Players", newPlayer).GetAwaiter().GetResult();

            return Json(res);
            //http://127.0.0.1:8888/user/Insert?name=TimLx&level=20
        }

        [Get("getAll")]
        public object GetAll()
        {
            var res = ormContext.GetAllAsync<Player>("players").GetAwaiter().GetResult();

            return Html("Templates/Player.html", new { Players = res });
            //http://127.0.0.1:8888/user/Getall
        }

        [Get("delete")]
        public object Delete(int id)
        {
            var res = ormContext.DeleteAsync("players", id).GetAwaiter().GetResult();

            return Json(res);
            //http://127.0.0.1:8888/user/Delete?id=1
        }

        [Get("update")]
        public object Update(int id, string name, int level)
        {
            var newPlayer = new Player { Name = name, Level = level };
            var res = ormContext.UpdateAsync<Player>("players", id, newPlayer).GetAwaiter().GetResult();

            return Json(res);
            //http://127.0.0.1:8888/user/update?id=1&name=Piskar&level=88
        }
    }
}