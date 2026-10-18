using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleApp
{
    public class HtmlTemplateRenderer : IHtmlTemplateRenderer
    {
        public string RenderFromFile(string filePath, object dataModel)
        {
            throw new NotImplementedException();
        }

        public string RenderFromString(string htmlTemplate, object dataModel)
        {
            Type mtype = dataModel.GetType();
            string pattern = "\\${\\S*}";

            Regex regex = new Regex(pattern);
            MatchCollection matchCollection = regex.Matches(htmlTemplate);

            regex.Replace(htmlTemplate, (match) =>
            {
                string fieldName = match.Groups[1].Value.Trim('.');
                var something = mtype.GetProperty(fieldName,
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                return 
            });
            return regex.Replace(htmlTemplate, pattern, dataModel.ToString());
        }

        public string RenderToFile(string inputFilePath, string outputFilePath, object dataModel)
        {
            throw new NotImplementedException();
        }
    }
}
