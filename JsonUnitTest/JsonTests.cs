using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    {                        // исключение, потому что есть поле с типом double
        public WrongPerson(int id, string name, double weight)
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
        public void TestGenerateJson()
        {
            Person person1 = new Person(1, "Jack");
            Person person2 = new Person(2, "Jill");
            Person person3 = new Person(3, "Jull");
            Person person4 = new Person(4, "Joll");
            person1.Children.Add(person2);
            person1.Children.Add(person3);
            person2.Children.Add(person4);
            JsonGenerator.GenerateJson(person1);
        }

        [TestMethod]
        public void NotJsonObject_NoProperty()
        {
            Assert.ThrowsException<NotJsonObject>(() => JsonGenerator.GenerateJson(3));
        }

        [TestMethod]
        public void NotJsonObject_PropertyTypeIsDouble()
        {
            Assert.ThrowsException<NotJsonObject>(() => JsonGenerator.GenerateJson(new WrongPerson(1, "Jack", 63.5)));
        }
    }
}
