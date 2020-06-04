using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace JsonLibrary
{
    public class JsonGenerator<T>
    {
        private readonly List<T> alreadySerialised;
        public string WritePath { get; set; }

        public JsonGenerator(string w)
        {
            WritePath = w;
            alreadySerialised = new List<T>();
        }

        public JsonGenerator()
        {
            alreadySerialised = new List<T>();
        }

        public void GenerateJson(T person)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append ("{\n");
            alreadySerialised.Add(person);
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
            if (WritePath != null)
            {
                FileOutput.WriteInFile(sb, WritePath);               
            }
            ConsoleOutput.WriteInConsole(sb);
        }

        private string GetPropertyValue(T person, T obj, PropertyInfo propertyInfo, int tabCount)
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
                        if (!(alreadySerialised.Contains(p)))
                        {
                            alreadySerialised.Add(p);
                            foreach (var prop in typeof(T).GetProperties()) // Для каждого свойства элемента списка
                            {
                                if (!(prop.GetValue(p) is List<T> l && l.Count == 0))
                                {
                                    sb.Append(GetPropertyValue(person, p, prop, tabCount + 1));
                                }
                            }
                        }
                        else
                            throw new CycleError("Cycle");
                    }
                }
                sb.Append($"{Tab(tabCount)}" + "} \n");
                sb.Append($"{Tab(tabCount-1)}" + "]\n"); 
            }
            return sb.ToString();
        }

        private string Tab(int tabCount) // Для отступов
        {
            string tab = null;
            for (int i = 0; i < tabCount; i++)
                tab += "  ";
            return tab;
        }  
    }

    public class FileOutput // Класс для записи в файл
    {
        public static void WriteInFile(StringBuilder sb, string writePath)
        {
            using (StreamWriter f = new StreamWriter(writePath, false, Encoding.Default))
            {
                f.WriteLine(sb);
            }
        }
    }

    public class ConsoleOutput // Класс для вывода
    {
        public static void WriteInConsole(StringBuilder sb)
        {
            Console.WriteLine(sb);
        }
    }

    public class NotJsonObject : Exception
    {
        public NotJsonObject(string Message) : base(Message)
        { }
    }

    public class CycleError : Exception
    {
        public CycleError(string Message) : base(Message)
        { }
    }

    public class JsonAttribute : Attribute
    {
    }

    public class JsonObject : Attribute
    {
    }
}
