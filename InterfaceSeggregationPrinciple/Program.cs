using System;

namespace InterfaceSeggregationPrinciple
{
    class Program
    {
        public class Document
        {

        }

        // Problem: creating an interface with multiple responsibilities and thus multiple reasons to change, gives problems implementing it on the OldFashioned printer.
        // this is also een machine but cannot fax or scan documents. You would have 2 no-op methods.
        public interface IMachine
        {
            void Print(Document doc);
            void Scan(Document doc);
            void Fax(Document doc);
        }

        // Seggrating IMachine into different smaller interfaces each responsible for only 1 task solves the problem.
        // You could always combine multiple interfaces into a new interface if you have classes that always implement both.
        public interface IPrinter
        {
            void Print(Document doc);
        }

        public interface IScanner
        {
            void Scan(Document doc);
        }

        public interface IFaxer
        {
            void Fax(Document doc);
        }

        public interface IMultiFunctionMachine: IScanner, IPrinter //...
        {

        }

        public class MultiFunctionPrinter : IMachine
        {
            public void Fax(Document doc)
            {
                //
            }

            public void Print(Document doc)
            {
                //
            }

            public void Scan(Document doc)
            {
                //
            }
        }

        public class OldFashionedPrinter : IMachine
        {
            public void Fax(Document doc)
            {
                throw new NotImplementedException();
            }

            public void Print(Document doc)
            {
                //
            }

            public void Scan(Document doc)
            {
                throw new NotImplementedException();
            }
        }

        public class PhotoCopier : IMultiFunctionMachine
        {
            public void Print(Document doc)
            {
                //
            }

            public void Scan(Document doc)
            {
                //
            }
        }

        public class MultiFunctionMachine : IMultiFunctionMachine
        {
            private IPrinter printer;
            private IScanner scanner;
            

            cto
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
