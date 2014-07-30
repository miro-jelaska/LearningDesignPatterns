using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Remoting.Contexts;

namespace DesignPatterns.Patterns.Singleton
{
    public class ChocolateBoiler {
        // When initialized using "if(_chocolateBoiler == null)..." => Danger of race condition
        public static ChocolateBoiler GetChocolateBoiler
        {
            get { return _chocolateBoiler.Value; }
        }
        private readonly static Lazy<ChocolateBoiler> _chocolateBoiler = new Lazy<ChocolateBoiler>(() => new ChocolateBoiler());

        private ChocolateBoiler
        (
        ) 
        {
            IsEmpty = true;
            IsBoiled = false;
        }

        public bool IsEmpty { get; private set; }
        public bool IsBoiled { get; private set; }

        public void fill() {
            if (IsEmpty) {
                IsEmpty = false;
                IsBoiled = false;
                // fill the boiler with a milk/chocolate mixture
            }
        }
        public void drain() {
            if (!IsEmpty && IsBoiled) {
                // drain the boiled milk and chocolate
                IsEmpty = true;
            }
        }
        public void boil() {
            if (!IsEmpty && !IsBoiled) {
                // bring the contents to a boil
                IsBoiled = true;
            }
        }
    }
}
namespace DesignPatterns.Patterns.Singleton
{
    public abstract class Executor
    {
        public static void Execute()
        {
            Console.WriteLine(ChocolateBoiler.GetChocolateBoiler.IsBoiled);
        }
    }
}
