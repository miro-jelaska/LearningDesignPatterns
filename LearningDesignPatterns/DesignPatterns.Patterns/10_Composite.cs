using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace DesignPatterns.Patterns.Composite
{
    interface IMenuComponent
    {
        void Add(IMenuComponent menuComponents);
        void Print();
    }
    class MenuItem : IMenuComponent
    {
        public MenuItem
        (
            int price,
            string name
        )
        {
            this.Price = price;
            this.Name = name;
        }
        public int Price { get; private set; }
        public string Name  { get; private set; }

        public override string ToString()
        {
            return "Price: " + this.Price + "    Name: " + this.Name;
        }
    
        public void Add(IMenuComponent menuComponents)
        {
            Console.WriteLine("Cannot add item to leaf aka MenuItem");
        }
        
        public void Print()
        {
            Console.WriteLine(this);
        }
    }
    class MenuComposite : IMenuComponent
    {
        public MenuComposite
        (
            string name
        )
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(name));

            this.Name           = name;
            this.MenuComponents = new List<IMenuComponent>();
        }
        public string                Name           { get; private set; }
        private List<IMenuComponent> MenuComponents { get; set; }

        public override string ToString()
        {
            return "▀▄ Menu Name: " + this.Name + " ▄▀";
        }
    
        public void Add(IMenuComponent menuComponents)
        {
            this.MenuComponents.Add(menuComponents);
        }
        
        public void Print()
        {
            Console.WriteLine(this);
            foreach (var component in MenuComponents)
            {
                component.Print();
            }
        }
    }
}

namespace DesignPatterns.Patterns.Composite
{
    public abstract class Executor
    {
        //............MainMenu
        //......___________________
        //......|........|.........|
        //...Vegan.....Fast.....Breakfest
        //.....|
        //...ExclusiveVegan
        public static void Execute()
        {
            var menuComposite  = new MenuComposite("MainMenu");
            var vegan          = new MenuComposite("Vegan");
            var exclusiveVegan = new MenuComposite("Exclusive Vegan");
            var fast           = new MenuComposite("Fast");
            var breakfest      = new MenuComposite("Breakfest");
            menuComposite .Add(new MenuItem(15, "Pancake "));
            menuComposite .Add(new MenuItem(5, "Cupcake "));
            vegan         .Add(new MenuItem(30, "Tofu"));
            vegan         .Add(new MenuItem(32, "Soy pizza"));
            exclusiveVegan.Add(new MenuItem(55, "Very special Tofu"));
            fast          .Add(new MenuItem(12, "Pizza"));
            breakfest     .Add(new MenuItem(12, "Waffle"));

            vegan        .Add(exclusiveVegan);
            menuComposite.Add(vegan);
            menuComposite.Add(fast);
            menuComposite.Add(breakfest);

            menuComposite.Print();
        }
    }
}
