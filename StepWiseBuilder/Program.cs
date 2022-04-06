using System;

/**
 * Provide a wizard-like interface for building objects step-by-step
 */
namespace StepWiseBuilder
{
    public enum CarType
    {
        Sedan,
        CrossOver
    }

    public class Car
    {
        public CarType Type;
        public int WheelSize;
    }
    
    // Interface seggratation on the different car specifications.
    public interface ISpecifyCarType
    {
        // Forcing a wizard-like car building process by returning the next operation after car type
        ISpecifyWheelSize OfType(CarType type);
    }

    public interface ISpecifyWheelSize
    {
        // At the end of the build process we return the builder itself to construct te car
        IBuildCar WithWheels(int size);
    }

    public interface IBuildCar
    {
        Car Build();
    }

    public class CarBuilder
    {
        // This class implements all build properties and
        // holds a private Car object which is only exposed by the Build() method
        private class Impl :
            ISpecifyCarType,
            ISpecifyWheelSize,
            IBuildCar
        {
            private Car car = new Car();    

            public ISpecifyWheelSize OfType(CarType type)
            {
                car.Type = type;
                return this;
            }
            
            public IBuildCar WithWheels(int size)
            {
                // Car.Type conditional validation
                switch(car.Type)
                {
                    case CarType.CrossOver when size < 17 || size > 20:
                    case CarType.Sedan when size < 15 || size > 17:
                        throw new ArgumentException($"Wrong size of wheel for {car.Type}");
                }

                car.WheelSize = size;
                return this;
            }

            public Car Build() => car;
        }

        public static ISpecifyCarType Create()
        {
            return new Impl();
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var car = CarBuilder.Create()   // ISpecifyCarType
                .OfType(CarType.CrossOver)  // ISpecifyWheelSize
                .WithWheels(18)             // IBuildCar
                .Build();                   // Car
        }
    }
}
