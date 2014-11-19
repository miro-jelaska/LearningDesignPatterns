using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.Patterns.Iterator
{
    class MenuItem
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
    }

    class PancakeHouseMenu
    {
        readonly List<MenuItem> menuItems = new List<MenuItem>(new []{
            new MenuItem(15, "Pancake "),
            new MenuItem(5, "Cupcake ")
        });

        public  List<MenuItem> GetMenuItems()
        {
            return this.menuItems;
        }
    }
    class DinerMenu
    {
        readonly MenuItem[] menuItems = {
            new MenuItem(25, "Steak "),
            new MenuItem(12, "Meatballs ")
        };

        public  MenuItem[] GetMenuItems()
        {
            return this.menuItems;
        }
    }

    interface IIterator<T>
    {
        bool MoveNext();
        T Current();
        void Reset();
    }

    class PancakeHouseMenuIterator : IIterator<MenuItem>
    {
        public PancakeHouseMenuIterator
        (
            PancakeHouseMenu menu
        )
        {
            this.menuItems = menu.GetMenuItems();
            this.CurrentIteratorIndex = InitialIteratorPosition;
        }

        private const int InitialIteratorPosition = -1;
        private readonly List<MenuItem> menuItems;
        private int CurrentIteratorIndex { get; set; }

        public bool MoveNext()
        {
            var isNullOrEmpty = menuItems == null || !menuItems.Any();
            var isCurrentIteratorsIndexOnLastElement =CurrentIteratorIndex == menuItems.Count - 1;

            if(!isNullOrEmpty && !isCurrentIteratorsIndexOnLastElement)
            {
                CurrentIteratorIndex = CurrentIteratorIndex + 1;
                return true;
            }
            return false;
        }

        public MenuItem Current()
        {
            return 
                (CurrentIteratorIndex != InitialIteratorPosition)
                ? menuItems[CurrentIteratorIndex]
                : null;
        }

        public void Reset()
        {
            CurrentIteratorIndex = InitialIteratorPosition;
        }
    }
    class DinerMenuIterator : IIterator<MenuItem>
    {
        public DinerMenuIterator
        (
            DinerMenu menu
        )
        {
            this.menuItems = menu.GetMenuItems();
            this.CurrentIteratorIndex = InitialIteratorPosition;
        }

        private const int InitialIteratorPosition = -1;
        private readonly MenuItem[] menuItems;
        private int CurrentIteratorIndex { get; set; }

        public bool MoveNext()
        {
            var isNullOrEmpty = menuItems == null || !menuItems.Any();
            var isCurrentIteratorsIndexOnLastElement = CurrentIteratorIndex == menuItems.Length - 1;

            if(!isNullOrEmpty && !isCurrentIteratorsIndexOnLastElement)
            {
                CurrentIteratorIndex = CurrentIteratorIndex + 1;
                return true;
            }
            return false;
        }

        public MenuItem Current()
        {
            return 
                (CurrentIteratorIndex != InitialIteratorPosition)
                ? menuItems[CurrentIteratorIndex]
                : null;
        }

        public void Reset()
        {
            CurrentIteratorIndex = InitialIteratorPosition;
        }
    }

    class Waitress
    {
        public Waitress
        (
            PancakeHouseMenu pancakeHouseMenu,
            DinerMenu        dinerMenu
        )
        {
            this.pancakeHouseMenu = pancakeHouseMenu;
            this.dinerMenu        = dinerMenu;
        }
        
        private readonly PancakeHouseMenu pancakeHouseMenu;
        private readonly DinerMenu        dinerMenu;

        public void PrintMenus()
        {
            var pancakeHouseMenuIterator = new PancakeHouseMenuIterator(pancakeHouseMenu);
            var dinerMenuIterator = new DinerMenuIterator(dinerMenu);

            while (pancakeHouseMenuIterator.MoveNext())
            {
                Console.WriteLine(pancakeHouseMenuIterator.Current());
            }
            while (dinerMenuIterator.MoveNext())
            {
                Console.WriteLine(dinerMenuIterator.Current());
            }
        }
    }
}
namespace DesignPatterns.Patterns.Iterator
{
    public abstract class Executor
    {
        public static void Execute()
        {
            var waitress = new Waitress(new PancakeHouseMenu(), new DinerMenu());
            waitress.PrintMenus();
        }
    }
}
