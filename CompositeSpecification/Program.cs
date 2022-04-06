using System;
using System.Collections.Generic;
using System.Linq;

namespace CompositeSpecification
{
    public enum Color
    {
        Red, Green, Blue
    }

    public class Product
    {
        public string Name;
        public Color Color;

        public Product(string name, Color color)
        {

            Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
            Color = color;
        }
    }
    public class ColorSpecification : ISpecification<Product>
    {
        private readonly Color color;

        public ColorSpecification(Color color)
        {
            this.color = color;
        }

        public override bool IsSatisfied(Product t) => t.Color == color;
    }

    public abstract class ISpecification<T>
    {
        public abstract bool IsSatisfied(T p);

        public static ISpecification<T> operator &(ISpecification<T> first, ISpecification<T> second)
        {
            return new AndSpecification<T>(first, second);
        }
    }

    // Composite
    public abstract class CompositeSpecification<T> : ISpecification<T>
    {
        protected readonly ISpecification<T>[] items;

        public CompositeSpecification(params ISpecification<T>[] items)
        {
            this.items = items;
        }
    }

    public class AndSpecification<T> : CompositeSpecification<T>
    {
        public AndSpecification(params ISpecification<T>[] items) : base(items)
        {
        }

        public override bool IsSatisfied(T t) => items.All(i => i.IsSatisfied(t));
    }

    public class OrSpecification<T> : CompositeSpecification<T>
    {
        public OrSpecification(params ISpecification<T>[] items) : base(items)
        {
        }

        public override bool IsSatisfied(T t) => items.Any(i => i.IsSatisfied(t));
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
