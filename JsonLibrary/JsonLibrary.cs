using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

namespace JsonLibrary
{
    public static class JsonGenerator
    {
        public static void GenerateJson<T>(T person)
        {
            var s = "{\n"; // Строка, в которой в итоге будет результат
            var type = person.GetType();
            int tabCount = 1; // Счетчик отступов (для красоты)
            if (type.GetProperties().Length != 0) // Если нет свойств, тогда исключение
            {
                foreach (var item in type.GetProperties()) // Для каждого свойства выполняем GetPropertyValue
                {
                    s = GetPropertyValue(person, s, person, item, tabCount);
                }
            }
            else
                throw new NotJsonObject("Этот объкт нельзя сериализовать");
            s += "}";
            Output(s);
        }

        private static string GetPropertyValue<T>(T person, string s, T obj, PropertyInfo propertyInfo, int tabCount)
        {
            if (propertyInfo.PropertyType == typeof(int) || propertyInfo.PropertyType == typeof(string))
            {
                s += $"{Tab(tabCount)}{propertyInfo.Name}: {propertyInfo.GetValue(obj)},\n";
            }
            else
            {
                if (propertyInfo.PropertyType == typeof(List<T>))
                {
                    s += $"{Tab(tabCount)}{propertyInfo.Name}: [\n";
                    s += $"{Tab(tabCount + 1)}" + "{ \n";
                    tabCount++;
                    if (propertyInfo.PropertyType == typeof(List<T>))
                    {
                        foreach (var p in (List<T>)propertyInfo.GetValue(obj)) // Для каждого элемента списка
                        {
                            foreach (var prop in typeof(T).GetProperties()) // Для каждого свойства элемента списка
                            {
                                if (!(prop.GetValue(p) is List<T> l && l.Count == 0))
                                {
                                    s = GetPropertyValue(person, s, p, prop, tabCount + 1);
                                }
                            }
                        }
                    }
                    s += $"{Tab(tabCount)}" + "} \n";
                    s += $"{Tab(tabCount-1)}" + "]\n";
                }
                else
                    throw new NotJsonObject("Этот объкт нельзя сериализовать"); // Могу дописать условия для double, bool... Это нужно?
            }
            return s;
        }

        private static string Tab(int tabCount) // Для отступов
        {
            string tab = null;
            for (int i = 0; i < tabCount; i++)
                tab += "  ";
            return tab;
        }  
        
        private static void Output(string s)
        {
            string writePath = @"C:\Users\cherm\Desktop\Json.txt"; // Запись в файл
            using (StreamWriter f = new StreamWriter(writePath, false, System.Text.Encoding.Default))
            {
                f.WriteLine(s);
            }
            Console.WriteLine(s); // Вывод в консоль
            //Console.ReadLine(); // Закомментировала, чтобы выполнялся юнит тест
        }
    }

    public class NotJsonObject : Exception
    {
        public NotJsonObject(string Message) : base(Message)
        { }
    }

    public class JsonAttribute : Attribute // Не знаю, нужно ли использовать как-то атрибуты, вроде как работает и без них
    {
    }

    public class JsonObject : Attribute
    {
    }
}
