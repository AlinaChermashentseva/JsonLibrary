using System;
using System.Collections.Generic;
using JsonLibrary;

namespace JsonSerialization // Чтобы можно было посмотреть, как работает
{
    [JsonObject]
    public class Person 
    {
        public Person(int id, string name)
        {
            Id = id;
            Name = name;
            Children = new List<Person>();
        }
        [Json]
        public int Id { get; set; }
        [Json]
        public string Name { get; set; }
        [Json]
        public List<Person> Children { get; set; }
    }
    class Program
    {
        static void Main()
        {
            Person person1 = new Person(1, "Jack");
            Person person2 = new Person(2, "Jill");
            Person person3 = new Person(3, "Jull");
            Person person4 = new Person(4, "Joll");
            person1.Children.Add(person2);
            person1.Children.Add(person3);
            person2.Children.Add(person4);
            JsonGenerator.GenerateJson(person1);
            Console.ReadLine();
        }
    }
}
