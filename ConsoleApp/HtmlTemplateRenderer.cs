using System.Text.RegularExpressions;
using System.Reflection;
using System.Collections;
using System.Text;

namespace ConsoleApp
{
    public class HtmlTemplateRenderer : IHtmlTemplateRenderer
    {
        public string RenderFromFile(string filePath, object dataModel)
        {
            string content = File.ReadAllText(filePath);
            return RenderFromString(content, dataModel);
        }

        public string RenderFromString(string htmlTemplate, object dataModel)
        {
            Type dataModelType = dataModel.GetType();
            string res = htmlTemplate;

            string ifElsePattern = @"\$if\s*\(([^)]+)\)((?:(?!\$if|\$endif|\$else)[\s\S])*)(?:\$else((?:(?!\$if|\$endif)[\s\S])*))?\$endif";
            Regex ifElseRegex = new Regex(ifElsePattern);

            res = ifElseRegex.Replace(res, (match) =>
            {
                string condition = match.Groups[1].Value;
                string ifBody = match.Groups[2].Value;
                string elseBody = match.Groups[3].Value;

                if (string.IsNullOrEmpty(elseBody)) { elseBody = ""; }

                bool isTrue = IsConditionTruth(condition, dataModel);
                string chosenBody = isTrue ? ifBody : elseBody;

                return RenderFromString(chosenBody, dataModel);
            });

            string foreachPattern = @"\$foreach\s*\(\s*var\s+(\w+)\s+in\s+(\w+)\s*\)((?:(?!\$foreach|\$endfor)[\s\S])*)\$endfor";
            Regex foreachRegex = new Regex(foreachPattern);

            res = foreachRegex.Replace(res, (match) =>
            {
                string itemName = match.Groups[1].Value;
                string collectionName = match.Groups[2].Value;
                string body = match.Groups[3].Value;

                PropertyInfo? property = dataModelType.GetProperty(collectionName);
                if (property != null)
                {
                    object? collectionValue = property.GetValue(dataModel);
                    IEnumerable collectiom = collectionValue as IEnumerable;
                    if (collectiom ==  null) { return ""; }

                    StringBuilder sb = new StringBuilder();
                    foreach(var item in collectiom)
                    {
                        sb.Append(RenderFromString(body, item));
                    }
                    return sb.ToString();
                }
                return "";
            });

            string pattern = "\\$\\{([^}]+)\\}";
            Regex regex = new Regex(pattern);

            foreach(Match match in regex.Matches(htmlTemplate))
            {
                Console.WriteLine(match.Groups[1].Value);
            }

            res = regex.Replace(res, (match) =>
            {
                string fieldName = match.Groups[1].Value;
                PropertyInfo? property = dataModelType.GetProperty(fieldName);

                if (property != null)
                {
                    object? value = property.GetValue(dataModel);
                    if (value == null) { return match.Value; }

                    Type valueType = value.GetType();
                    bool isSimple = IsSimple(valueType);

                    if (!isSimple)
                    {
                        string zaglushka = "<p>group: ${group}</p>";
                        return RenderFromString(zaglushka, value);
                    }

                    return value.ToString();
                }
                Console.WriteLine($"null! замена не удаласб в {fieldName}");
                return match.Value;
            });
            return res;
        }
        
        public string RenderToFile(string inputFilePath, string outputFilePath, object dataModel)
        {
            string res = RenderFromFile(inputFilePath, dataModel);
            File.WriteAllText(outputFilePath, res);
            return res;
        }

        private bool IsConditionTruth(string condition, object dataModel)
        {
            condition = condition.Trim();

            PropertyInfo? property = dataModel.GetType().GetProperty(condition);
            if (property == null) { return false; }
            object? value = property.GetValue(dataModel);
            
            if (value is bool b) { return b; }
            if (value is string s) { return !string.IsNullOrEmpty(s); }
            return value != null;
        }

        private bool IsSimple(Type type)
        {
            return type.IsPrimitive
              || type.IsEnum
              || type.Equals(typeof(string))
              || type.Equals(typeof(decimal));
        }
    }
}