using ConsoleApp;

string userTemplate = "<p>Имя: ${name}</p>" +
    "<p>группа: ${group}</p>" +
    "<p>возраст: ${age}</p>" +
    "<p>пол: ${sex}</p>";
User user = new User("Ilnur", "11-541", 22, true);

HtmlTemplateRenderer userRender = new HtmlTemplateRenderer();
string res = userRender.RenderFromString(userTemplate, user);
Console.WriteLine(res);

string bookTemplate = "<p>Название книги: ${name}</p>" +
    "<p>автор: ${author}</p>" +
    "<p>год издания: ${realiseDate}</p>";
Book book = new Book("Main Kumpth", "hui", user);

HtmlTemplateRenderer bookRender = new HtmlTemplateRenderer();
Console.WriteLine(bookRender.RenderFromString(bookTemplate, book));

string penTemplate = "<p>Название: ${name}</p>" +
    "<p>цвет: ${colour}</p>" +
    "<p>размер: ${widht}</p>" +
    "<p>цена ${price}</p>";
Pen pen = new Pen("pen", "желтный", "1488мм", 14);

HtmlTemplateRenderer penRender = new HtmlTemplateRenderer();
Console.WriteLine(penRender.RenderFromString(penTemplate, pen));

string multiTemplate = @"<p>Имя: ${name}</p>" +
    "$if(gender) <p>Выпивка бесплатна</p> $else <p>Плати 5к</p> $endif" +
    "$foreach(var item in items) <p>Товар: ${name}, цена: ${price}</p> $endfor";
Pen pen2 = new Pen("pen2", "синий", "6769мм", 68);
MultiClass multiClass = new MultiClass("multiClass", true, new List<Pen> { pen, pen2 } );

HtmlTemplateRenderer multiReader =  new HtmlTemplateRenderer();
Console.WriteLine(multiReader.RenderFromString(multiTemplate, multiClass));