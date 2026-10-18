//using ConsoleApp;

//string userTemplate = "<p>Имя: ${name}</p>" +
//    "<p>группа: ${group}</p>" +
//    "<p>возраст: ${age}</p>" +
//    "<p>пол ${sex}</p>";
//User user = new User("Vanya", "11-541", 22, true);

//HtmlTemplateRenderer htmlTemplateRenderer = new HtmlTemplateRenderer();
//string res = htmlTemplateRenderer.RenderFromString();
//Console.WriteLine(res);

//string bookTemplate = "<p>Название книги: ${name}</p>" +
//    "<p>автор: ${author}</p>" +
//    "<p>год издания: ${realisePage}</p>";
//Book book = new Book("Main Kumpth", "hui", "1488");

//HtmlTemplateRenderer htmlTemplateRenderer2 = new HtmlTemplateRenderer();
//string res = htmlTemplateRenderer.RenderFromString();
//Console.WriteLine(res);

//string penTemplate = "<p>Название: ${name}</p>" +
//    "<p>цвет: ${color}</p>" +
//    "<p>размер: ${widht}</p>" +
//    "<p>цена ${price}</p>";
//Pen pen = new Pen("", "", "", 14);

//HtmlTemplateRenderer htmlTemplateRenderer3 = new HtmlTemplateRenderer();
//string res = htmlTemplateRenderer.RenderFromString();
//Console.WriteLine(res);

using ConsoleApp;
using System.Runtime.InteropServices;

string str = "<p>Имя: ${name}</p>" +
    "<p>группа: ${group}</p>" +
    "<p>возраст: ${age}</p>" +
    "<p>пол ${sex}</p>";

HtmlTemplateRenderer htmlTemplateRenderer = new HtmlTemplateRenderer();
Console.WriteLine(htmlTemplateRenderer.RenderFromString(str, "Pisun"));