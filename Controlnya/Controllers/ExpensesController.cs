using controlnyaMeow.Core;
using controlnyaMeow.Core.Attributes;

namespace controlnyaMeow.Controllers
{
    [HttpController("expenses")]
    internal class ExpensesController : ControllerBase
    {
        private readonly ExpenseStore store = new ExpenseStore();
        [Get("")]
        public void List(string category)
        {
            Console.WriteLine($"GET/expenses category: {category}");

            var items = store.List(category);
            Console.WriteLine($"нашли: {items.Count}");
            foreach(var i  in items)
            {
                Console.WriteLine($"{i.Id}, {i.Category}, {i.Amount}, {i.Status}");
                
            }
        }

        [Get("details")]
        public void Details(string id)
        {
            Console.WriteLine($"Get/expense/details: {id}");
            var r = store.ById(int.Parse(id));
            if (r == null )
            {
                return;
            }
            Console.WriteLine($"{r.Id}, {r.Category}, {r.Amount}, {r.Status}");
        }



        [Post("create")]
        public void Create(string category, string amount)
        {
            Console.WriteLine($"Put/expense/create. Category: {category}, amount: {amount}");
            if (string.IsNullOrEmpty(category) || string.IsNullOrEmpty(amount))
            {
                return;
            }
            int a = int.Parse(amount);
            var r = store.Add(category, a);
            Console.WriteLine($"{r.Id}, {r.Category}, {r.Amount}, {r.Status}");
        }

        [Put("update")]
        public void Update(string id, string category, string amount, string status)
        {
            Console.WriteLine($"Post/expense/update. Category: {category}, amount: {amount}");
            if (string.IsNullOrEmpty(category) || string.IsNullOrEmpty(amount) || string.IsNullOrEmpty(status) || string.IsNullOrEmpty(id))
            {
                return;
            }
            int eid = int.Parse(id);
            int a = int.Parse(amount);
            if (!store.Update(eid, category, a, status, out int err))
            {
                if (err == 404)
                {
                    Console.WriteLine($"{eid} не нашли 404");
                }
                if (err == 409)
                {
                    Console.WriteLine($"{eid} предмет уже approved 409");
                }
            }

            Console.WriteLine($"добавлено: {eid}, {category}, {a}, {status}");
        }
    }
}
