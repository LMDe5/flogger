using HttpServer.Controllers;
using HttpServer.Framework.Attributes;
using HttpServer.Framework.HttpResponses;
using MiniHttpServer.Models;
using MiniHttpServer.ORM;
using MiniHttpServer.Services.Authentication;

namespace MiniHttpServer.Controllers
{
    [HttpController("admin")]
    [Authorize]
    internal class AdminController : ControllerBase
    {
        private static string connectionString = "Host=localhost;Port=5432;Database=myDataBase;Username=postgres;Password=piskoed228";
        private readonly IOrmContext ormContext = new OrmContext(connectionString);

        [Get("")]
        public IHttpResponseTypeResult Dashboard()
        {
            return Html("semestrovaya/admin1.html", new { });
        }

        [Get("cards")]
        public IHttpResponseTypeResult CardsList()
        {
            var res = ormContext.GetAllAsync<Card>("cards").GetAwaiter().GetResult();
            var viewCards = res.Select(c => new
            {
                CardId = c.Id,
                CardName = c.Name,
                CardType = c.Type,
                CardAlignment = c.Alignment,
                CardImage = c.ImageUrl
            }).ToList();

            return Html("semestrovaya/admin2.html", new { Cards = viewCards });
        }

        [Get("edit")]
        public IHttpResponseTypeResult EditCard(string id)
        {
            int.TryParse(id, out int cardId);

            if (cardId > 0)
            {
                var res = ormContext.GetAllAsync<Card>("cards").GetAwaiter().GetResult();
                var card = res.FirstOrDefault(c => c.Id == cardId);

                if (card != null)
                {

                    var model = new 
                    {
                        CardId = card.Id,
                        CardName = card.Name,
                        CardType = card.Type,
                        CardAlignment = card.Alignment,
                        CardAbility = card.Ability,
                        CardDescription = card.Description
                    };

                    return Html("semestrovaya/admin3.html", model);
                }
            }

            var emptyCard = new
            {
                CardId = 0,CardName = "",CardType = "Персонаж",CardAlignment = "Нейтрал",
                CardAbility = "", CardDescription = ""
            };
            return Html("semestrovaya/admin3.html", emptyCard);
        }

        [Post("create")]
        public IHttpResponseTypeResult CreateCard(string CardName,
            string CardType, string CardAlignment, string CardAbility,
            string CardDescription)
        {
            var newCard = new Card
            {
                Name = CardName,
                Type = CardType,
                Alignment = CardAlignment,
                Ability = CardAbility,
                Description = CardDescription, ImageUrl = "semestrovaya/default.jpg"
            };
            var res = ormContext.InsertAsync("cards", newCard).GetAwaiter().GetResult();
            return Json(res);
        }

        [Post("update")]
        public IHttpResponseTypeResult UpdateCard(string CardId, string CardName,
            string CardType, string CardAlignment, string CardAbility,
            string CardDescription)
        {
            int.TryParse(CardId, out int id);

            var updateCard = new Card
            {
                Name = CardName,
                Type = CardType,
                Alignment = CardAlignment,
                Ability = CardAbility,
                Description = CardDescription,
                ImageUrl = "semestrovaya/default.jpg"
            };
            var res = ormContext.UpdateAsync<Card>("cards", id, updateCard).GetAwaiter().GetResult();
            return Json(res);
        }

        [Get("delete")]
        public IHttpResponseTypeResult DeleteCard(string id)
        {
            int.TryParse(id, out int cardId);

            var res = ormContext.DeleteAsync("cards", cardId).GetAwaiter().GetResult();
            return Json(res);
        }
    }
}