using System;

namespace LiskovSubstitutionPrinciple
{
    class Program
    {
        public class Rectangle
        {
            // By specifying these properties as 'virtual' we can override them in a direved class like the Square.
            
            //public int Width { get; set; }
            public virtual int Width { get; set; }
            //public int Height { get; set; }
            public virtual int Height { get; set; }

            public Rectangle()
            {

            }

            public Rectangle(int width, int height)
            {
                Width = width;
                Height = height;
            }

            public override string ToString()
            {
                return $"{nameof(Width)}: {Width}, {nameof(Height)}: {Height}";
            }
        }

        public class Square : Rectangle
        {
            //public new int Width
            public override int Width
            {
                set
                { base.Width = base.Height = value; }
            }

            //public new int Height
            public override int Height
            {
                set { base.Width = base.Height = value; }
            }
        }

        static public int Area(Rectangle r) => r.Width * r.Height;

        static void Main(string[] args)
        {
            Rectangle rc = new Rectangle(2, 3);
            Console.WriteLine($"{rc} has area {Area(rc)}");

            // The difference of overriding vs creating a new property is that when you execute this line of code,
            // the overridden properties of Square are called by the override table lookup. 
            // This way the subclass Square is replaceable with its superclass Rectangle while maintaining full Square functionality
            // And therefore satisfies the Liskov Substitution Principle.
            Rectangle sq = new Square();
            sq.Width = 4;
            Console.WriteLine($"{sq} has area {Area(sq)}");
        }
    }
}
