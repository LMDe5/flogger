using HttpServer.Controllers;
using HttpServer.Framework.Attributes;
using HttpServer.Models;
using MiniHttpServer.ORM;

namespace MiniHttpServer.Controllers
{
    [HttpController("item")]
    internal class ItemController : ControllerBase
    {
        private static string connectionString = "Host=localhost;Port=5432;Database=myDataBase;Username=postgres;Password=piskoed228";
        private readonly IOrmContext ormContext = new OrmContext(connectionString);

        [Get("getAll")]
        public object GetAll()
        {
            var res = ormContext.GetAllAsync<Item>("items").GetAwaiter().GetResult();

            return Json(res);
            //http://127.0.0.1:8888/item/getAll
        }

        [Get("delete")]
        public object Delete(int id)
        {
            var res = ormContext.DeleteAsync("items", id).GetAwaiter().GetResult();

            return Json(res);
            //http://127.0.0.1:8888/item/Delete?id=1
        }

        [Get("update")]
        public object Update(int id, string name, int price, string rarity)
        {
            var newItem = new Item { Name = name, Price = price, Rarity = rarity };
            var res = ormContext.UpdateAsync<Item>("items", id, newItem).GetAwaiter().GetResult();

            return Json(res);
            //http://127.0.0.1:8888/item/update?id=1&name=butilka&price=12&rarity=epic
        }

        [Get("insert")]
        public object Insert(string name, int price, string rarity)
        {
            var newItem = new Item { Name = name, Price = price, Rarity = rarity };
            var res = ormContext.InsertAsync<Item>("items", newItem).GetAwaiter().GetResult();

            return Json(res);
            //http://127.0.0.1:8888/item/insert?name=butilka&price=12&rarity=epic
        }
    }
}
