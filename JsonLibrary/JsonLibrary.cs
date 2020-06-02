using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace JsonLibrary
{
    public static class JsonGenerator
    {
        public static void GenerateJson<T>(T person)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append ("{\n");
            var type = person.GetType();
            int tabCount = 1; // Счетчик отступов (для красоты)
            if (Attribute.IsDefined(type, typeof(JsonObject))) // Если есть [JsonObject], иначе исключение
            {
                foreach (var item in type.GetProperties()) // Для каждого свойства выполняем GetPropertyValue
                {
                    if (Attribute.IsDefined(item, typeof(JsonAttribute))) // Если есть [Json]
                        sb.Append(GetPropertyValue(person, person, item, tabCount));
                }
            }
            else
                throw new NotJsonObject("Not Json object");
            sb.Append("}");
            Output.FileOutput(sb);
        }

        private static string GetPropertyValue<T>(T person, T obj, PropertyInfo propertyInfo, int tabCount)
        {
            StringBuilder sb= new StringBuilder();            
            if (propertyInfo.PropertyType != typeof(List<T>))
            {
                sb.Append($"{Tab(tabCount)}{propertyInfo.Name}: {propertyInfo.GetValue(obj)},\n");
            }
            if (propertyInfo.PropertyType == typeof(List<T>))
            {
                sb.Append($"{Tab(tabCount)}{propertyInfo.Name}: [\n");
                sb.Append($"{Tab(tabCount + 1)}" + "{ \n");
                tabCount++;
                if (propertyInfo.PropertyType == typeof(List<T>))
                {
                    foreach (var p in (List<T>)propertyInfo.GetValue(obj)) // Для каждого элемента списка
                    {
                        foreach (var prop in typeof(T).GetProperties()) // Для каждого свойства элемента списка
                        {
                            if (!(prop.GetValue(p) is List<T> l && l.Count == 0))
                            {
                                sb.Append(GetPropertyValue(person, p, prop, tabCount + 1));
                            }
                        }
                    }
                }
                sb.Append($"{Tab(tabCount)}" + "} \n");
                sb.Append($"{Tab(tabCount-1)}" + "]\n"); 
            }
            return sb.ToString();
        }

        private static string Tab(int tabCount) // Для отступов
        {
            string tab = null;
            for (int i = 0; i < tabCount; i++)
                tab += "  ";
            return tab;
        }  
    }

    public class Output // Класс для вывода
    {
        public static void FileOutput(StringBuilder sb)
        {
            string writePath = @"C:\Users\cherm\Desktop\Json.txt"; // Запись в файл
            using (StreamWriter f = new StreamWriter(writePath, false, Encoding.Default))
            {
                f.WriteLine(sb);
            }
            Console.WriteLine(sb);
        }
    }


    public class NotJsonObject : Exception
    {
        public NotJsonObject(string Message) : base(Message)
        { }
    }

    public class JsonAttribute : Attribute
    {
    }

    public class JsonObject : Attribute
    {
    }
}
