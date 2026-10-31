using HtmlAgilityPack;
using System.Text.Json;

namespace HttpServer.Framework.Http
{
    public class SteamParser
    {
        private HttpClient client = new HttpClient();

        public async Task GetHeroItems(string heroName)
        {
            string src = $"https://steamcommunity.com/market/search/render/?norender=1&start=0&count=10&appid=570&category_Hero=npc_dota_hero_{heroName}&currency=5&l=russian";

            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");

            try
            {
                //возвращаем тело ответа в виде строки
                string response = await client.GetStringAsync(src);
                //преобразуем нашу строку в Json док
                using JsonDocument doc = JsonDocument.Parse(response);
                //берем корень всего Json объекта
                JsonElement root = doc.RootElement;

                //проверяем есть ли поля results
                if (!root.TryGetProperty("results", out JsonElement resultArray))
                {
                    Console.WriteLine("Не смог найти поля results");
                    return;
                }

                Console.WriteLine("Вещи персонажа");
                foreach(var item in resultArray.EnumerateArray())
                {
                    string name = item.GetProperty("name").GetString();
                    string price = item.GetProperty("sell_price_text").GetString();
                    string sale = item.GetProperty("sale_price_text").GetString();
                    int count = item.GetProperty("sell_listings").GetInt32();

                    Console.WriteLine($"название:{name}, получите денег:{price}, продают за: {sale}, кол-во:{count}");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}
