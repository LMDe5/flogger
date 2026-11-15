using HttpServer.Controllers;
using HttpServer.Framework.Attributes;
using HttpServer.Framework.HttpResponses;
using MiniHttpServer.Models;
using MiniHttpServer.ORM;

namespace MiniHttpServer.Controllers
{
    [HttpController("cards")]
    internal class CardController : ControllerBase
    {
        private static string connectionString = "Host=localhost;Port=5432;Database=myDataBase;Username=postgres;Password=piskoed228";
        private readonly IOrmContext ormContext = new OrmContext(connectionString);

        [Get("")]
        public IHttpResponseTypeResult GetAll()
        {
            var res = ormContext.GetAllAsync<Card>("cards").GetAwaiter().GetResult();

            var viedCards = res.Select(c => new { CardId = c.Id, CardName = c.Name,
                CardType = c.Type, CardAlignment = c.Alignment, CardAbility = c.Ability,
                CardDescription = c.Description, CardImage = c.ImageUrl }).ToList();

            return Html("semestrovaya/cardsPage.html", new { Cards = viedCards });
        }

        [Get("view")]
        public IHttpResponseTypeResult GetCardById(int id)
        {
            var res = ormContext.GetAllAsync<Card>("cards").GetAwaiter().GetResult();
            var card = res.FirstOrDefault(c => c.Id == id);

            if (card == null)
            {
                string notFoundStr = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "static", "404.html"));
                return Html(notFoundStr);
            }

            var model = new
            {
                CardId = card.Id,
                CardName = card.Name,
                CardType = card.Type,
                CardAlignment = card.Alignment,
                CardAbility = card.Ability,
                CardDescription = card.Description,
                CardImage = card.ImageUrl
            };

            return Html("semestrovaya/cardInfo.html", model);
        }
    }
}