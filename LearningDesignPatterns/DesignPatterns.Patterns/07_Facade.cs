using System;
using System.Collections.Generic;
using System.Linq;

using DesignPatterns.Patterns.Command.Facade;

namespace DesignPatterns.Patterns.Command.Facade
{
    public class Item
    {
        public Item
        (
            string name,
            int    price
        )
        {
            this.Name = name;
            this.Price = price;
        }
        public string Name { get; private set; }
        public int    Price { get; private set; }
    }
    
    class ItemsDummyRepo
    {
        public static IReadOnlyCollection<Item> AllItems
        {
            get { return _allItems.Value; }
        }
        private static readonly Lazy<IReadOnlyCollection<Item>> _allItems = new Lazy<IReadOnlyCollection<Item>>(
            valueFactory: () => new List<Item>()
            {
                new Item("printer", 350),
                new Item("tablet" , 1100),
                new Item("router" , 720)
            }
        );
    }

    public class UserService
    {
        public enum UserRole
        {
            Anonymous,
            Admin
        }

        public UserRole GetUserRoles()
        {
            return UserRole.Admin;
        }
        public bool IsUserAuthorized(UserRole role)
        {
            return role == UserRole.Admin;
        }
    }
    public class AvailabilityService
    {
        public bool IsItemAvailable(string itemName)
        {
            return ItemsDummyRepo.AllItems.Any(item => item.Name == itemName);
        }
    }
    public class ProductService
    {
        public Item GetItem_byName(string name)
        {
            return ItemsDummyRepo.AllItems.Single(item => item.Name == name);
        }
    }

    public class ProductFacade
    {
        public class UserNotAuthenticatedException : Exception { }
        public class UserNotAuthorisedException    : Exception { }
        public class ItemNotFoundException         : Exception { }

        public ProductFacade
        (
            UserService         userService,
            AvailabilityService availabilityService,
            ProductService      productService
        )
        {
            this.userService         = userService;
            this.availabilityService = availabilityService;
            this.productService      = productService;
        }
        private readonly UserService         userService;
        private readonly AvailabilityService availabilityService;
        private readonly ProductService      productService;

        public Item TryGetItem_byName(string itemName)
        {
            var userRole = userService.GetUserRoles();
            if(userRole == UserService.UserRole.Anonymous) { throw new UserNotAuthenticatedException(); }
            
            var isAuthorized = userService.IsUserAuthorized(userRole);
            if (!isAuthorized) { throw new UserNotAuthorisedException(); }

            var isItemAvailable = availabilityService.IsItemAvailable(itemName);
            if (!isItemAvailable) { throw new ItemNotFoundException(); }

            return productService.GetItem_byName(itemName);
        }
    }
}
namespace DesignPatterns.Patterns.Facade
{
    public abstract class Executor
    {
        public static void Execute()
        {
            var productFacade = new ProductFacade(
                userService:         new UserService(),
                availabilityService: new AvailabilityService(),
                productService:      new ProductService()
            );
            var productName = "printer";

            _printItemPrice(productName, productFacade);
        }
        private static void _printItemPrice(string itemName, ProductFacade productProvider)
        {
            try
            {
                var item = productProvider.TryGetItem_byName(itemName);
                Console.WriteLine(item.Price);
            }
            catch (ProductFacade.UserNotAuthenticatedException e)
            {
                Console.WriteLine("UserNotAuthenticated");
            }
            catch (ProductFacade.UserNotAuthorisedException e)
            {
                Console.WriteLine("UserNotAuthorised");
            }
            catch (ProductFacade.ItemNotFoundException e)
            {
                Console.WriteLine("ItemNotFound");
            }
        }
    }
}
