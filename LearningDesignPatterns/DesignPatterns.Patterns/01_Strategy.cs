using System;

namespace DesignPatterns.Patterns.Strategy
{
    abstract class Quacks
    {
        public interface IQuack
        {
            void Quack();
        }
        public class LongQuacks : IQuack
        {
            public void Quack()
            {
                Console.WriteLine("Long QUACK!");
            }
        }
        public class Squee : IQuack
        {
            public void Quack()
            {
                Console.WriteLine("SqueeSqUeee");
            }
        }
        public class Silent : IQuack
        {
            public void Quack()
            {
                Console.WriteLine("<nothing>");
            }
        }
    }

    class Duck : Quacks.IQuack
    {
        public Duck
        (
            Quacks.IQuack quack
        )
        {
            this.quack = quack;
        }
        private readonly Quacks.IQuack quack;

        public void Quack()
        {
            this.quack.Quack();
        }
    }

    
}
namespace DesignPatterns.Patterns.Strategy
{
    public abstract class Executor
    {
        public static void Execute()
        { 
            var dIsForDuck = new Duck( new Quacks.LongQuacks());
            dIsForDuck.Quack();
        }
    }
}
