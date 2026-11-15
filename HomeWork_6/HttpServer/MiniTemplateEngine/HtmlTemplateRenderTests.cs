using ConsoleApp;

namespace MiniTemplateEngine
{
    [TestClass]
    public sealed class HtmlTemplateRenderTests
    {
        [TestMethod]
        public void RenderFromString_NullTest1()
        {
            string template = "$if(gender) <p>Выпивка бесплатна</p> $else <p>Плати 5к</p>$endif";
            MultiClass multiClass = null;
            string expectedString = "null";

            var render = new HtmlTemplateRenderer();
            string res = render.RenderFromString(template, multiClass);
            Assert.AreEqual(expectedString, res);
        }
        [TestMethod]
        public void RenderFromString_NullTest2()
        {
            string template = null;
            Pen pen1 = new Pen("pen1", "white", "2mm", 14);
            Pen pen2 = new Pen("pen2", "orange", "1mm", 88);
            MultiClass multiClass = new MultiClass("Meowl", false, new List<Pen> { pen1, pen2 });
            string expectedString = "null";

            var render = new HtmlTemplateRenderer();
            string res = render.RenderFromString(template, multiClass);
            Assert.AreEqual(expectedString, res);
        }

        [TestMethod]
        public void RenderFromString_IfElseTest1()
        {
            string template = "$if(gender) <p>Выпивка бесплатна</p> $else <p>Плати 5к</p>$endif";
            Pen pen1 = new Pen("pen1", "white", "2mm", 14);
            Pen pen2 = new Pen("pen2", "orange", "1mm", 88);
            MultiClass multiClass = new MultiClass("Meowl", false, new List<Pen> { pen1, pen2 });
            string expectedString = $"<p>Плати 5к</p>";

            var render = new HtmlTemplateRenderer();
            string res = render.RenderFromString(template, multiClass);
            Assert.AreEqual(expectedString, res);
        }
        [TestMethod]
        public void RenderFromString_IfElseTest2()
        {
            string template = "$if(gender) <p>Выпивка бесплатна</p> $else <p>Плати 5к</p>$endif";
            Pen pen1 = new Pen("pen1", "white", "2mm", 14);
            Pen pen2 = new Pen("pen2", "orange", "1mm", 88);
            MultiClass multiClass = new MultiClass("Meowl", true, new List<Pen> { pen1, pen2 });
            string expectedString = $"<p>Выпивка бесплатна</p>";

            var render = new HtmlTemplateRenderer();
            string res = render.RenderFromString(template, multiClass);
            Assert.AreEqual(expectedString, res);
        }
        [TestMethod]
        public void RenderFromString_IfElseTest3()
        {
            string template = "$if(gender) <p>Выпивка бесплатна</p>$endif";
            Pen pen1 = new Pen("pen1", "white", "2mm", 14);
            Pen pen2 = new Pen("pen2", "orange", "1mm", 88);
            MultiClass multiClass = new MultiClass("Meowl", false, new List<Pen> { pen1, pen2 });
            string expectedString = $"";

            var render = new HtmlTemplateRenderer();
            string res = render.RenderFromString(template, multiClass);
            Assert.AreEqual(expectedString, res);
        }

        [TestMethod]
        public void RenderFromString_ForeachTest1()
        {
            string template = "$foreach(var item in items) <p>Товар: ${name}, цена: ${price}</p> $endfor";
            Pen pen1 = new Pen("pen1", "white", "2mm", 14);
            Pen pen2 = new Pen("pen2", "orange", "1mm", 88);
            MultiClass multiClass = new MultiClass("Meowl", false, new List<Pen> { pen1, pen2 });
            string expectedString = "<p>Товар: pen1, цена: 14</p><p>Товар: pen2, цена: 88</p>";

            var render = new HtmlTemplateRenderer();
            string res = render.RenderFromString(template, multiClass);
            Assert.AreEqual(expectedString, res);
        }
        [TestMethod]
        public void RenderFromString_ForeachTest2()
        {
            string template = "$foreach(var item in items) <p>Товар: ${name}, цена: ${price}</p> $endfor";
            Pen pen1 = null;
            Pen pen2 = null;
            MultiClass multiClass = new MultiClass("Meowl", false, new List<Pen> { pen1, pen2 });
            string expectedString = "";

            var render = new HtmlTemplateRenderer();
            string res = render.RenderFromString(template, multiClass);
            Assert.AreEqual(expectedString, res);
        }
        [TestMethod]
        public void RenderFromString_ForeachTest3()
        {
            string template = "$foreach(var item in items) <p>Товар: ${name}, цена: ${price}</p> $endfor";
            Pen pen1 = new Pen("pen1", "white", "2mm", 14);
            Pen pen2 = null;
            MultiClass multiClass = new MultiClass("Meowl", false, new List<Pen> { pen1, pen2 });
            string expectedString = "<p>Товар: pen1, цена: 14</p>";

            var render = new HtmlTemplateRenderer();
            string res = render.RenderFromString(template, multiClass);
            Assert.AreEqual(expectedString, res);
        }
        [TestMethod]
        public void RenderFromString_ForeachTest4()
        {
            string template = "$foreach(var item in items) <p>Товар: ${name}, цена: ${price}</p> $endfor";
            MultiClass multiClass = new MultiClass("Meowl", false, null);
            string expectedString = "";

            var render = new HtmlTemplateRenderer();
            string res = render.RenderFromString(template, multiClass);
            Assert.AreEqual(expectedString, res);
        }
        [TestMethod]
        public void RenderFromString_ForeachTest5()
        {
            string template = "$foreach(var item in items) <p>Товар: ${name}, цена: ${price}</p> $endfor";
            User user = new User("meowl", "14-88", 67, true);
            string expectedString = "";

            var render = new HtmlTemplateRenderer();
            string res = render.RenderFromString(template, user);
            Assert.AreEqual(expectedString, res);
        }

        [TestMethod]
        public void RenderFromString_ReplaceTest1()
        {
            string template = "<p>Имя: ${name}</p>" +
                "<p>группа: ${group}</p>" +
                "<p>возраст: ${age}</p>" +
                "<p>пол: ${sex}</p>";
            User user = new User("meowl", "14-88", 67, true);
            string expectedString = "<p>Имя: meowl</p>" +
                "<p>группа: 14-88</p>" +
                "<p>возраст: 67</p>" +
                "<p>пол: True</p>";

            var render = new HtmlTemplateRenderer();
            string res = render.RenderFromString(template, user);
            Assert.AreEqual(expectedString, res);
        }
        [TestMethod]
        public void RenderFromString_ReplaceTest2()
        {
            string template = "<p>Имя: ${name}</p>" +
                "<p>группа: ${group}</p>" +
                "<p>возраст: ${age}</p>" +
                "<p>пол: ${sex}</p>";
            User user = new User(null, null, 67, true);
            string expectedString = "<p>Имя: ${name}</p>" +
                "<p>группа: ${group}</p>" +
                "<p>возраст: 67</p>" +
                "<p>пол: True</p>";

            var render = new HtmlTemplateRenderer();
            string res = render.RenderFromString(template, user);
            Assert.AreEqual(expectedString, res);
        }
        [TestMethod]
        public void RenderFromString_ReplaceTest3()
        {
            string template = "<p>Имя: ${name}</p>" +
                "<p>группа: ${group}</p>" +
                "<p>возраст: ${age}</p>" +
                "<p>пол: ${sex}</p>";
            Book user = new Book("Mein Kampf", "Xydozhnik", "1488");
            string expectedString = "<p>Имя: Mein Kampf</p>" +
                "<p>группа: ${group}</p>" +
                "<p>возраст: ${age}</p>" +
                "<p>пол: ${sex}</p>";

            var render = new HtmlTemplateRenderer();
            string res = render.RenderFromString(template, user);
            Assert.AreEqual(expectedString, res);
        }

        [TestMethod]
        public void RenderFromFile_Test1()
        {
            string filePath = "";
            User user = new User("Meowl", "14-88", 67, true);
            string expectedString = "nullPath";

            var render = new HtmlTemplateRenderer();
            string res = render.RenderFromFile(filePath, user);
            Assert.AreEqual(expectedString, res);
        }
        [TestMethod]
        public void RenderFromFile_Test2()
        {
            string filePath = "C:\\Users\\user\\source\\repos\\ConsoleApp\\ConsoleApp\\TestTextFile.txt";
            User user = new User("Meowl", "14-88", 67, true);
            string expectedString = "<p>Имя: Meowl</p>" +
                "<p>группа: 14-88</p>" +
                "<p>возраст: 67</p>" +
                "<p>пол: True</p>";

            var render = new HtmlTemplateRenderer();
            string res = render.RenderFromFile(filePath, user);
            Assert.AreEqual(expectedString, res);
        }

        [TestMethod]
        public void RenderToFile_Test1()
        {
            string filePath = null;
            User user = new User("Meowl", "14-88", 67, true);
            string expectedString = "nullPath";

            var render = new HtmlTemplateRenderer();
            string res = render.RenderToFile(filePath, filePath, user);
            Assert.AreEqual(expectedString, res);
        }
        [TestMethod]
        public void RenderToFile_Test2()
        {
            string filePath = "C:\\Users\\user\\source\\repos\\ConsoleApp\\ConsoleApp\\TestTextFile.txt";
            User user = new User("Meowl", "14-88", 67, true);
            string expectedString = "<p>Имя: Meowl</p>" +
                "<p>группа: 14-88</p>" +
                "<p>возраст: 67</p>" +
                "<p>пол: True</p>";

            var render = new HtmlTemplateRenderer();
            string res = render.RenderToFile(filePath, filePath, user);
            Assert.AreEqual(expectedString, res);
        }
    }
}
