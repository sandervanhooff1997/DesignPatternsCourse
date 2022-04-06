using System;
using System.Collections.Generic;
using System.Linq;

/** 
 * High level parts of the system should not depend on low level parts directly. 
 * Instead they should depend on some kind of abstraction.
 */
namespace DependencyInversionPrinciple
{
    internal class Program
    {
        public enum Relationship
        {
            Parent,
            Child,
            Sibling
        }

        public class Person
        {
            public string Name;
        }

        // This is the abstractionlayer that high-level objects depend on when accessing relationships.
        public interface IRelationshipBrowser
        {
            IEnumerable<Person> FindAllChildrenOf(string name);
        }

        // low-level
        public class Relationships : IRelationshipBrowser
        {
            private List<(Person, Relationship, Person)> relations = new List<(Person, Relationship, Person)>();

            public void AddParentAndChild(Person parent, Person child)
            {
                relations.Add((parent, Relationship.Parent, child));
                relations.Add((child, Relationship.Child, parent));
            }

            public IEnumerable<Person> FindAllChildrenOf(string name) => 
                relations.Where(x => x.Item1.Name == "John" && x.Item2 == Relationship.Parent)
                .Select(x => x.Item3);

            // Creating this public property to expose relations to high-level objects is a bad practice.
            // public List<(Person, Relationship, Person)> Relations => relations;
        }

        // high-level
        public class Research
        {
            //public Research(Relationships relationships)
            //{
                // This class demonstrates the wrong approach of using a low-level part of the application in a high-level part.
                // Whats wrong about it is that the relations property of relationships is a private field and we added a public shadow property that now exposes relations.
                // We had to change the low-level part in order to satisfy the high-level part and that is a bad practice.
                // A clean solution for the problem would be to create the abstractionlayer IRelationshipBrowser.

                //var relations = relationships.Relations;
                //foreach (var r in relations.Where(
                //    x => x.Item1.Name == "John" &&
                //    x.Item2 == Relationship.Parent
                //    )) ;
            //}

            public Research(IRelationshipBrowser browser)
            {
                foreach (var child in browser.FindAllChildrenOf("John"))
                    Console.WriteLine($"John has a child called {child.Name}");
            }
        }

        static void Main(string[] args)
        {
            var parent = new Person() { Name = "John" };
            var child1 = new Person() { Name = "Chris" };
            var child2 = new Person() { Name = "Mary" };

            var relationships = new Relationships();
            relationships.AddParentAndChild(parent, child1);
            relationships.AddParentAndChild(parent, child2);

            new Research(relationships);
        }
    }
}
