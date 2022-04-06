using System;

/**
 * Expanding builder functionalities by inheritance creates a problem returning 'this' to be able to chain methods.
 * As 'this' refers to the current class, extending from this class would only return the derived class as the chain return type.
 * Therefore you could not chain multiple methods from different inherited classes.
 * 
 * Solution:
 * 1. create an abstract base class with all the base properties (a Person)
 * 2. (optional) create a Build() method returning the person
 * 3. Create PersonInfoBuilder inheriting from base class PersonBuilder and pass a generic type SELF
 * 4. Constraint SELF to be of type PersonInfoBuilder
 * 5. Extend PersonInfoBuilder with new functions by creating PersonJobBuilder and also constraint SELF the same way
 * 6. Because we cannot construct a BirthDateBuilder/JobBuilder/InfoBuilder our selves, create class Builder in Person with the static New property
 * 7. Build a person chaining methods from different builder classes
 */
namespace FluentBuilderRecursiveGenerics
{
    public class Person
    {
        public string Name;

        public string Position;

        public DateTime DateOfBirth;

        public string Address;

        public class Builder : PersonAddressBuilder<Builder>
        {
            internal Builder() { }
        }

        public static Builder New => new Builder();

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Position)}: {Position}, {nameof(DateOfBirth)}: {DateOfBirth}, {nameof(Address)}: {Address}";
        }
    }

    public abstract class PersonBuilder
    {
        protected Person person = new Person();

        public Person Build()
        {
            return person;
        }
    }

    public class PersonInfoBuilder<SELF> : PersonBuilder
      where SELF : PersonInfoBuilder<SELF>
    {
        public SELF Called(string name)
        {
            person.Name = name;
            return (SELF)this;
        }
    }

    public class PersonJobBuilder<SELF>
      : PersonInfoBuilder<PersonJobBuilder<SELF>>
      where SELF : PersonJobBuilder<SELF>
    {
        public SELF WorksAsA(string position)
        {
            person.Position = position;
            return (SELF)this;
        }
    }

    // here's another inheritance level
    // note there's no PersonInfoBuilder<PersonJobBuilder<PersonBirthDateBuilder<SELF>>>!

    public class PersonBirthDateBuilder<SELF>
      : PersonJobBuilder<PersonBirthDateBuilder<SELF>>
      where SELF : PersonBirthDateBuilder<SELF>
    {
        public SELF Born(DateTime dateOfBirth)
        {
            person.DateOfBirth = dateOfBirth;
            return (SELF)this;
        }
    }

    public class PersonAddressBuilder<SELF>
        : PersonBirthDateBuilder<PersonAddressBuilder<SELF>>
        where SELF : PersonAddressBuilder<SELF>
    {
        public SELF LivesIn(string address)
        {
            person.Address = address;
            return (SELF)this;
        }
    }

    internal class Program
    {
        class SomeBuilder : PersonBirthDateBuilder<SomeBuilder>
        {

        }

        public static void Main(string[] args)
        {
            var me = Person.New
              .Called("Dmitri")
              .WorksAsA("Quant")
              .Born(DateTime.UtcNow)
              .LivesIn("Eindhoven")
              .Build();
            Console.WriteLine(me);
        }
    }
}
