using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// The composite design pattern describes a group of objects that are treated the same way as a single instance of the same type.
/// In this example we have several different GraphicObjects/Circles/Squares that are all added to one composite list and printed to the console.
/// </summary>
namespace CompositeGeometricShapes
{
    public class GraphicObject
    {
        public virtual string Name { get; set; } = "Group";
        public string Color;

        private Lazy<List<GraphicObject>> children = new Lazy<List<GraphicObject>>();
        public List<GraphicObject> Children => children.Value;

        private void Print(StringBuilder sb, int depth)
        {
            sb.Append(new string('*', depth))
                .Append(string.IsNullOrWhiteSpace(Color) ? string.Empty : $"{Color}")
                .AppendLine(Name);

            foreach (var child in Children)
            {
                child.Print(sb, depth+1);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            Print(sb, 0);
            return sb.ToString();
        }
    }

    public class Circle : GraphicObject
    {
        public override string Name => "Circle";
    }

    public class Square : GraphicObject
    {
        public override string Name => "Square";
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var drawing = new GraphicObject {Name = "My Drawing"};
            drawing.Children.Add(new Square { Color = "Red"});
            drawing.Children.Add(new Circle { Color = "Yellow"});

            var group = new GraphicObject();
            group.Children.Add(new Circle {Color= "Blue"});
            group.Children.Add(new Square() {Color= "Blue"});
            drawing.Children.Add(group);

            Console.WriteLine(drawing);
        }
    }
}
