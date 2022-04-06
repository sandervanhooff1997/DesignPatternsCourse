using System;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalBuilder
{
    public class Person
    {
        public string Name, Position;

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Position)}: {Position}";
        }
    }

    /**
     * This is a generic functional builder
     * It holds a private list of functions
     * Do() transforms an action to a function and adds it to the list
     * Executing Build() constructs a new object of TSubject
     * And then performs all actions on the object one by one
     * Because every action returns TSelf, the end result of Build() is a fully build TSelf
     */
    public abstract class FunctionalBuilder<TSubject, TSelf>
        where TSelf : FunctionalBuilder<TSubject, TSelf>
        where TSubject : new() // Must have a default constructor
    {
        private readonly List<Func<TSubject, TSubject>> actions = new();

        public TSelf Do(Action<TSubject> action) => AddAction(action);

        private TSelf AddAction(Action<TSubject> action)
        {
            actions.Add(p =>
            {
                action(p);
                return p;
            });

            return (TSelf)this;
        }
        public TSubject Build() => actions.Aggregate(new TSubject(), (p, f) => f(p));
    }

    // Example implementation of FunctionalBuilder
    public sealed class PersonBuilder : FunctionalBuilder<Person, PersonBuilder> 
    {
        public PersonBuilder Called(string name) => Do(p => p.Name = name);
    }

    public static class PersonBuilderExtensions
    {
        public static PersonBuilder WorksAs(this PersonBuilder builder, string position) => builder.Do(p => p.Position = position);
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var person = new PersonBuilder()
                .Called("Sarah")
                .WorksAs("Developer")
                .Build();

            Console.WriteLine(person);
        }
    }
}
