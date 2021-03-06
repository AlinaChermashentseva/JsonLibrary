﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsonLibrary;
using System.Collections.Generic;

namespace JsonUnitTest
{
    [JsonObject]
    public class Person // Класс, чтобы проверить GenerateJson (должен сгенерироваться)
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

    public class WrongPerson // Класс, чтобы проверить исключение NotJsonObject
    {                        // Исключение, потому что нет [JsonObject]
        public WrongPerson(int id, string name, double weight)
        {
            Id = id;
            Name = name;
            Weight = weight;
        }
        [Json]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Weight { get; set; }
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

    [TestClass]
    public class JsonTests
    {

        [TestMethod]
        public void TestGenerateJsonForPerson()
        {
            Person person1 = new Person(1, "Jack");
            Person person2 = new Person(2, "Jill");
            Person person3 = new Person(3, "Jull");
            Person person4 = new Person(4, "Joll");
            person1.Children.Add(person2);
            person1.Children.Add(person3);
            person2.Children.Add(person4);
            JsonGenerator<Person> p1 = new JsonGenerator<Person>();
            p1.GenerateJson(person1);
        }

        [TestMethod]
        public void TestGenerateJsonForPersonWriteInFile()
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
        }

        [TestMethod]
        public void TestGenerateJsonForPets()
        {
            Pet pet1 = new Pet(1, "Bill", 7.5);
            JsonGenerator<Pet> p2 = new JsonGenerator<Pet>();
            p2.GenerateJson(pet1);
        }

        [TestMethod]
        public void NotJsonObject()
        {
            JsonGenerator<WrongPerson> p1 = new JsonGenerator<WrongPerson>();
            Assert.ThrowsException<NotJsonObject>(() => p1.GenerateJson(new WrongPerson(1, "Jack", 63.5)));
        }

        [TestMethod]
        public void TestСycleError()
        {
            Person person1 = new Person(1, "Jack");
            Person person2 = new Person(2, "Jill");
            Person person3 = new Person(3, "Jull");
            Person person4 = new Person(4, "Joll");
            Person person5 = new Person(5, "Bill");
            Person person6 = new Person(6, "Joe");
            person1.Children.Add(person2);
            person2.Children.Add(person3);
            person3.Children.Add(person4);
            person4.Children.Add(person5);
            person5.Children.Add(person1);
            JsonGenerator<Person> p1 = new JsonGenerator<Person>();
            Assert.ThrowsException<CycleError>(() => p1.GenerateJson(person1));
        }
    }
}
