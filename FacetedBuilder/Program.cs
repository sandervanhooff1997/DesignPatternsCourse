using System;

/**
 * This faceted builder builds a person object with multiple builders, all concerning a part of the person.
 */
namespace FacetedBuilder
{
    public class Person
    {
        // address
        public string StreetAddress, Postcode, City;

        // employment
        public string CompanyName, Position;
        public int AnnualIncome;

        public override string ToString()
        {
            return $"{nameof(StreetAddress)}: {StreetAddress}, {nameof(Postcode)}: {Postcode}, {nameof(City)}: {City}, {nameof(CompanyName)}: {CompanyName}, {nameof(Position)}: {Position}, {nameof(AnnualIncome)}: {AnnualIncome}";
        }
    }

    public class PersonBuilder
    {
        protected Person person = new();

        // One way to expose the person object after build process.
        public Person Build() => person;

        public static implicit operator Person(PersonBuilder builder) => builder.person;

        public PersonJobBuilder Works() => new(person);
        public PersonAddressBuilder Lives() => new(person);
    }

    public class PersonAddressBuilder : PersonBuilder
    {
        public PersonAddressBuilder(Person person)
        {
            this.person = person;
        }

        public PersonAddressBuilder At(string streetAddress)
        {
            person.StreetAddress = streetAddress;
            return this;
        }

        public PersonAddressBuilder WithPostcode(string postcode)
        {
            person.Postcode = postcode;
            return this;
        }

        public PersonAddressBuilder In(string city)
        {
            person.City = city;
            return this;
        }
    }

    public class PersonJobBuilder : PersonBuilder
    {
        public PersonJobBuilder(Person person)
        {
            this.person = person;
        }

        public PersonJobBuilder At(string company)
        {
            person.CompanyName = company;
            return this;
        }

        public PersonJobBuilder AsA(string position)
        {
            person.Position = position;
            return this;
        }

        public PersonJobBuilder Earning(int amount)
        {
            person.AnnualIncome = amount;
            return this;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var pb = new PersonBuilder();
            Person person = pb
                .Works()
                .AsA("IT Specialist")
                .At("McDonalds")
                .Earning(12500)
                .Lives() // Possible to call Lives() after Earning() because both builders inherit from PersonBuilder, that exposes these methods.
                .In("Eindhoven")
                .At("Stratumseind")
                .WithPostcode("5611PB");

            Console.WriteLine(person);
        }
    }
}
