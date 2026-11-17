using System.Text.RegularExpressions;
using System.Reflection;
using System.Collections;
using System.Text;

namespace ConsoleApp
{
    //Рендеринг HTML-шаблонов, позволяющий работать с условиями и циклами
    public class HtmlTemplateRenderer : IHtmlTemplateRenderer
    {
        //Читает шаблон ИЗ файла и рендерит его, после возвращает данные
        public string RenderFromFile(string filePath, object dataModel)
        {
            string content = File.ReadAllText(filePath); //читаем весь текст
            return RenderFromString(content, dataModel); //пердаем в метод рендеринга
        }

        //Рендерит шаблон строки, заменяет данные строки на данные пришедшей
        //даты-модели, поддерживает обработку циклов и условий
        public string RenderFromString(string htmlTemplate, object dataModel)
        {
            Type dataModelType = dataModel.GetType(); //получаем тип объекта данный
            string res = htmlTemplate; //создаем копию шаблона

            string ifElsePattern = @"\$if\s*\(([^)]+)\)((?:(?!\$if|\$endif|\$else)[\s\S])*)(?:\$else((?:(?!\$if|\$endif)[\s\S])*))?\$endif"; //регулярка для поиска if\else 
            Regex ifElseRegex = new Regex(ifElsePattern); //создаем объект регулярного выражения

            res = ifElseRegex.Replace(res, (match) => //перегрузка метода replace у регулярок, заменяем найденные значения на нужные нам
            {
                string condition = match.Groups[1].Value; //получаем условие
                string ifBody = match.Groups[2].Value;//получаем тело if
                string elseBody = match.Groups[3].Value;//получаем тело else 

                if (string.IsNullOrEmpty(elseBody)) { elseBody = ""; }//если тела else нет - заменяем на пустоту

                bool isTrue = IsConditionTruth(condition, dataModel); //проверяем условие true или false
                string chosenBody = isTrue ? ifBody : elseBody; //выбираем тело 

                return RenderFromString(chosenBody, dataModel); //идем в рекурсию для рендеринга дальше
            });

            string foreachPattern = @"\$foreach\s*\(\s*var\s+(\w+)\s+in\s+(\w+)\s*\)((?:(?!\$foreach|\$endfor)[\s\S])*)\$endfor"; //регулярка для поиска foreach
            Regex foreachRegex = new Regex(foreachPattern);//создаем объект регулярного выражения

            res = foreachRegex.Replace(res, (match) =>//перегрузка метода replace у регулярок, заменяем найденные значения на нужные нам
            {
                string itemName = match.Groups[1].Value; //получаем название предмета
                string collectionName = match.Groups[2].Value;//получаем название коллекции
                string body = match.Groups[3].Value;//получаем тело цикла

                PropertyInfo? property = dataModelType.GetProperty(collectionName); //получаем свойства коллекции в объекте
                if (property != null)
                {
                    object? collectionValue = property.GetValue(dataModel); //получаем значения коллекции
                    IEnumerable collectiom = collectionValue as IEnumerable; //приводим к IEnumerable для прохода по коллекции
                    if (collectiom ==  null) { return ""; } // нет коллекции - меняем на пустоту

                    StringBuilder sb = new StringBuilder(); //создаем строкБилдер для склеивания результатов
                    foreach(var item in collectiom)//проходимся по каждому элементу коллекции
                    {
                        sb.Append(RenderFromString(body, item)); //рендерим тело пришедшего нам СЕЙЧАС элемента и склеиваем 
                    }
                    return sb.ToString(); //меняем на получившуюся строку
                }
                return ""; //если свойств нет - меняем на пустоту
            });

            string pattern = "\\$\\{([^}]+)\\}"; //регулярка для поиска ${name} и подобных  
            Regex regex = new Regex(pattern);//создаем объект регулярного выражения

            res = regex.Replace(res, (match) =>//перегрузка метода replace у регулярок, заменяем найденные значения на нужные нам
            {
                string fieldName = match.Groups[1].Value; //получаем имя переменной 
                PropertyInfo? property = dataModelType.GetProperty(fieldName); //получаем свойства с таким именем из объекта

                if (property != null)
                {
                    object? value = property.GetValue(dataModel);//получаем значения свойства
                    if (value == null) { return match.Value; }//если null - меняем на исходник

                    //это мое, вам оно не нужно
                    //Type valueType = value.GetType(); 
                    //bool isSimple = IsSimple(valueType);

                    //if (!isSimple)
                    //{
                    //    string zaglushka = "<p>group: ${group}</p>";
                    //    return RenderFromString(zaglushka, value);
                    //}

                    return value.ToString(); //возвращаем строку, в которой вместо ${name} значение объекта
                }
                Console.WriteLine($"null! замена не удаласб в {fieldName}");
                return match.Value;//если null - меняем на исходник
            });
            return res;
        }
        
        //рендеринг шаблона из файла и запись в другой файл
        public string RenderToFile(string inputFilePath, string outputFilePath, object dataModel)
        {
            string res = RenderFromFile(inputFilePath, dataModel);//получаем отрендеренную строку
            File.WriteAllText(outputFilePath, res);//запись в файл
            return res;//возврат (если будет нужен)
        }

        //метод для проверки условия на правду\ложь
        private bool IsConditionTruth(string condition, object dataModel)
        {
            condition = condition.Trim();//убираем пробелы

            PropertyInfo? property = dataModel.GetType().GetProperty(condition);//получаем свойства по имени условия
            if (property == null) { return false; }//если нет свойства - ложь
            object? value = property.GetValue(dataModel); //получаем значения свойства
            
            if (value is bool b) { return b; } //если это булевое условие - возвращаем его
            if (value is string s) { return !string.IsNullOrEmpty(s); } //если это строка, то ложь, если пустая
            return value != null; //для остальных типов проверка если null - ложь
        }

        //это мое, вам оно не нужно
        //private bool IsSimple(Type type)
        //{
        //    return type.IsPrimitive
        //      || type.IsEnum
        //      || type.Equals(typeof(string))
        //      || type.Equals(typeof(decimal));
        //}
    }
}