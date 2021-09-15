using System;
using System.Collections;
using System.Collections.Generic;

namespace OpenClosedPrinciple
{
    class Program
    {
        public enum Color
        {
            Red, Green, Blue
        }

        public enum Size
        {
            Small, Medium, Large, Huge
        }

        public enum Price
        {
            Cheap, Normal, Expensive
        }

        public class Product
        {
            public string Name;
            public Color Color;
            public Size Size;
            public Price Price;

            public Product(string name, Color color, Size size, Price price)
            {

                Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
                Color = color;
                Size = size;
                Price = price;
            }
        }

        public class ProductFilter
        {
            public IEnumerable<Product> FilterBySize(IEnumerable<Product> products, Size size)
            {
                foreach (var p in products)
                    if (p.Size == size)
                        yield return p;
            }

            public IEnumerable<Product> FilterByColor(IEnumerable<Product> products, Color color)
            {
                foreach (var p in products)
                    if (p.Color == color)
                        yield return p;
            }

            public IEnumerable<Product> FilterBySizeAndColor(IEnumerable<Product> products, Size size, Color color)
            {
                foreach (var p in products)
                    if (p.Size == size && p.Color == color)
                        yield return p;
            }
        }

        /// <summary>
        /// ISpecification implements the specification pattern which dictates wether or not an object satisfies criteria.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public interface ISpecification<T>
        {
            bool IsSatisfied(T t);
        }

        /// <summary>
        /// IFilter is a filter machanism that filters objects based on there specification.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public interface IFilter<T>
        {
            IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
        }

        public class ColorSpecification : ISpecification<Product>
        {
            private readonly Color color;

            public ColorSpecification(Color color)
            {
                this.color = color;
            }

            public bool IsSatisfied(Product t) => t.Color == color;
        }

        public class AndSpecification<T> : ISpecification<T>
        {
            private readonly ISpecification<T> first, second;

            public AndSpecification(ISpecification<T> first, ISpecification<T> second)
            {
                // C# 7 language.
                this.first = first ?? throw new ArgumentNullException(paramName: nameof(first));
                this.second = second ?? throw new ArgumentNullException(paramName: nameof(second));
            }

            public bool IsSatisfied(T t) => first.IsSatisfied(t) && second.IsSatisfied(t);
        }

        public class SizeSpecification : ISpecification<Product>
        {
            private readonly Size size;

            public SizeSpecification(Size size)
            {
                this.size = size;
            }

            public bool IsSatisfied(Product t) => t.Size == size;
        }

        public class PriceSpecification: ISpecification<Product>
        {
            private readonly Price price;

            public PriceSpecification(Price price)
            {
                this.price = price;
            }

            public bool IsSatisfied(Product t) => t.Price == price;
        }

        public class BetterProductFilter : IFilter<Product>
        {
            public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec)
            {
                foreach (var i in items)
                    if (spec.IsSatisfied(i))
                        yield return i;
            }
        }

        static void Main(string[] args)
        {
            var apple = new Product("Apple", Color.Green, Size.Small, Price.Cheap);
            var tree = new Product("Tree", Color.Green, Size.Large, Price.Normal);
            var house = new Product("House", Color.Blue, Size.Large, Price.Expensive);

            Product[] products = {apple, tree, house};

            var pf = new ProductFilter();

            Console.WriteLine("Green products (old):");
            foreach (var p in pf.FilterByColor(products, Color.Green))
                Console.WriteLine($" - {p.Name} is green");

            var bf = new BetterProductFilter();
            Console.WriteLine("Green products (new):");
            foreach (var p in bf.Filter(products, new ColorSpecification(Color.Green)))
                Console.WriteLine($" - {p.Name} is green");

            Console.WriteLine("Large blue items");
            foreach (var p in bf.Filter(products, new AndSpecification<Product>(
                new ColorSpecification(Color.Blue),
                new AndSpecification<Product>(
                    new SizeSpecification(Size.Large),
                    new PriceSpecification(Price.Expensive)
                    )
                )))
                Console.WriteLine($" - {p.Name} is large and blue");

        }
    }
}
