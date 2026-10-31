using ConsoleApp;

namespace MiniTemplateEngine.Tests;

[TestClass]
public sealed class Test1
{
    public class TestUser
    {
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
        public List<string> Items { get; set; }

        [TestClass]
        public class TemplateTest
        {
            [TestMethod]
            public void Test_PlainText_ReturnsSameText()
            {
                var renderer = new HtmlTemplateRenderer();
                var result = renderer.RenderFromString("<p>Hello</p>", new TestUser());
                Assert.AreEqual("<p>Hello</p>", result);
            }

            [TestMethod]
            public void Test_VariableReplacement()
            {
                var renderer = new HtmlTemplateRenderer();
                var model = new TestUser { Name = "Ivan" };
                var result = renderer.RenderFromString("<p>${Name}</p>", model);
                Assert.AreEqual("<p>Ivan</p>", result);
            }

            [TestMethod]
            public void Test_IfElse_TrueCondition()
            {
                var renderer = new HtmlTemplateRenderer();
                var model = new TestUser { IsAdmin = true };
                var template = "$if(IsAdmin)Yes$else No$endif";
                var result = renderer.RenderFromString(template, model);
                Assert.AreEqual("Yes", result);
            }

            [TestMethod]
            public void Test_Foreach_Loop()
            {
                var renderer = new HtmlTemplateRenderer();
                var model = new TestUser { Items = new List<string> { "A", "B" } };
                var template = "$foreach(var item in Items)$foreach(var i in Items)X$endfor$endfor";
                
                Assert.IsNotNull(renderer.RenderFromString(template, model));
            }
        }
    }
}
