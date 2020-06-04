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

    [JsonObject]
    public class Pet
    {
        public Pet(int id, string name, double weight)
        {
            Id = id;
            Name = name;
            Weight = weight;
        }
        [Json]
        public int Id { get; set; }
        [Json]
        public string Name { get; set; }
        [Json]
        public double Weight { get; set; }
    }

    class Program
    {
        static void Main()
        {
            string writePath = @"C:\Users\cherm\Desktop\Json.txt";
            Person person1 = new Person(1, "Jack");
            Person person2 = new Person(2, "Jill");
            Person person3 = new Person(3, "Jull");
            Person person4 = new Person(4, "Joll");
            person1.Children.Add(person2);
            person1.Children.Add(person3);
            person2.Children.Add(person4);
            JsonGenerator<Person> p1 = new JsonGenerator<Person>(writePath);
            p1.GenerateJson(person1);
            Console.WriteLine("-----------------------------------------");
            JsonGenerator<Pet> p2 = new JsonGenerator<Pet>();
            Pet pet1 = new Pet(1, "Bill", 7.5);
            p2.GenerateJson(pet1);
            Console.ReadLine();
        }
    }
}
