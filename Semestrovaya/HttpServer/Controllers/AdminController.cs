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
        private const string defaultImageUrl = "/semestrovaya/images/default.png";

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
            var card = FindCard(id);
            if (card != null)
            {
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

                return Html("semestrovaya/admin3.html", model);
            }


            var emptyCard = new
            {
                CardId = 0,
                CardName = "",
                CardType = "Персонаж",
                CardAlignment = "Нейтрал",
                CardAbility = "",
                CardDescription = ""
            };
            return Html("semestrovaya/admin3.html", emptyCard);
        }

        [Post("create")]
        public IHttpResponseTypeResult CreateCard(string CardName,
            string CardType, string CardAlignment, string CardAbility,
            string CardDescription, string CardImage)
        {
            var newCard = new Card
            {
                Name = CardName,
                Type = CardType,
                Alignment = CardAlignment,
                Ability = CardAbility,
                Description = CardDescription,
                ImageUrl = defaultImageUrl
            };

            try
            {
                if (!string.IsNullOrEmpty(CardImage))
                {
                    newCard.ImageUrl = SaveImage(CardImage);
                }
            }
            catch (FormatException)
            {
                return Json("Вставьте другой формат картинки");
            }

            var res = ormContext.InsertAsync<Card>("cards", newCard);
            return Json(res);
        }

        [Post("update")]
        public IHttpResponseTypeResult UpdateCard(string CardId, string CardName,
            string CardType, string CardAlignment, string CardAbility,
            string CardDescription, string CardImage)
        {
            var card = FindCard(CardId);
            if (card == null)
            {
                return Json("Карточка не найдена");
            }

            card.Name = CardName;
            card.Type = CardType;
            card.Alignment = CardAlignment;
            card.Ability = CardAbility;
            card.Description = CardDescription;
            try
            {
                if (!string.IsNullOrEmpty(CardImage))
                {
                    card.ImageUrl = SaveImage(CardImage);
                }
            }
            catch(FormatException)
            {
                return Json("Вставьте другой формат картинки");
            }

            var res = ormContext.UpdateAsync<Card>("cards", card.Id, card).GetAwaiter().GetResult();
            return Json(res);
        }

        [Get("delete")]
        public IHttpResponseTypeResult DeleteCard(string id)
        {
            int.TryParse(id, out int cardId);

            var res = ormContext.DeleteAsync("cards", cardId).GetAwaiter().GetResult();
            return Json(res);
        }

        private Card? FindCard(string id)
        {
            int.TryParse(id, out int cardId);
            if (cardId > 0)
            {
                var res = ormContext.GetAllAsync<Card>("cards").GetAwaiter().GetResult();
                return res.FirstOrDefault(c => c.Id == cardId);
            }
            return null;
        }

        private string SaveImage(string modelUrl)
        {
            var parts = modelUrl.Split(',', 2);
            if (parts.Length != 2)
            {
                throw new FormatException();
            }

            string extension;
            if (parts[0] == "data:image/png;base64")
            {
                extension = ".png";
            }
            else if (parts[0] == "data:image/jpeg;base64")
            {
                extension = ".jpeg";
            }
            else
            {
                throw new FormatException();
            }

            byte[] bytes = Convert.FromBase64String(parts[1]);
            string fileName = Guid.NewGuid().ToString("N") + extension;
            string filePath = Path.Combine("static", "semestrovaya", "images", fileName);
            File.WriteAllBytes(filePath, bytes);
            return $"/semestrovaya/images/{fileName}";
        }
    }
}