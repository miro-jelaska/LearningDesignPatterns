using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns.Patterns.TemplateMethod
{
    public abstract class CaffeineBeverage
    {
        protected CaffeineBeverage()
        {
            this.boilWater();
            this.Brew();
            this.pourInCup();
            this.AddCondiments();
        }

        private void boilWater()
        {
            Console.WriteLine("Boiling water...");
        }
        private void pourInCup()
        {
            Console.WriteLine("Pouring in cup...");
        }
        protected abstract void Brew();
        protected abstract void AddCondiments();
    }

    class Coffe : CaffeineBeverage
    {
        public Coffe() : base() {}

        protected override void Brew()
        {
            Console.WriteLine("Dripping Coffe through filter.");
        }

        protected override void AddCondiments()
        {
            Console.WriteLine("Adding sugar.");
        }
    }
    class GreenTea : CaffeineBeverage
    {
        public GreenTea() : base() {}

        protected override void Brew()
        {
            Console.WriteLine("Steeping the tea.");
        }

        protected override void AddCondiments()
        {
            Console.WriteLine(" - None to add. - ");
        }
    }

    // Note: some TemplateMethods provide hook
}
namespace DesignPatterns.Patterns.TemplateMethod
{
    public abstract class Executor
    {
        public static void Execute()
        {
            var coffe = new Coffe();
            var tea = new GreenTea();
        }
    }
}
