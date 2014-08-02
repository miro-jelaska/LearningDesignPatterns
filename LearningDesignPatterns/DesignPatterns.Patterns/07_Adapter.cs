using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text.RegularExpressions;

using DesignPatterns.Patterns.Command.Adapter;
using DesignPatterns.Patterns.Command.Entities;

namespace DesignPatterns.Patterns.Command.Adapter
{
    public class User
    {
        public User
        (
            string Username,
            string FirstName,
            string LastName,
            string Country,
            string Hometown
        )
        {
            
            this.Username  = Username;
            this.FirstName = FirstName;
            this.LastName  = LastName;
            this.Country   = Country;
            this.Hometown  = Hometown;
        }
        public string Username  { get; private set; }
        public string FirstName { get; private set; }
        public string LastName  { get; private set; }
        public string Country   { get; private set; }
        public string Hometown  { get; private set; }
    }
    public interface IUserQuery
    {
        IReadOnlyCollection<User> GetAllUsers();
    }
    
    class UserDummyRepo
    {
        public static IReadOnlyCollection<User> AllUsers
        {
            get { return _allUsers.Value; }
        }
        private static readonly Lazy<IReadOnlyCollection<User>> _allUsers = new Lazy<IReadOnlyCollection<User>>(
            valueFactory: () => new List<User>()
            {
                new User("jlocke",     "John",   "Locke",     "England",           "Essex"),
                new User("taquinas",   "Thomas", "Aquinas",   "Kingdom of Sicily", "Roccasecca"),
                new User("rdescartes", "René",   "Descartes", "Kingdom of France", "La Haye en Touraine"),
                new User("thobbes ",   "Thomas", "Hobbes ",   "England",           "Westport"),
            }
        );
    }

    public class SimpleUserQuery : IUserQuery
    {
        public IReadOnlyCollection<User> GetAllUsers()
        {
            return UserDummyRepo.AllUsers;
        }
    }

    public class ComplexUserQuery
    {
        public class UserIdentityInfo
        {
            public UserIdentityInfo
            (
                string Username,
                string FirstName,
                string LastName
            )
            {
                
                this.Username  = Username;
                this.FirstName = FirstName;
                this.LastName  = LastName;
            }
            public string Username  { get; private set; }
            public string FirstName { get; private set; }
            public string LastName  { get; private set; }
        }
        public class UserLocationInfo
        {
            public UserLocationInfo
            (
                string Username,
                string Country,
                string Hometown
            )
            {
                
                this.Username  = Username;
                this.Country   = Country;
                this.Hometown  = Hometown;
            }
            public string Username  { get; private set; }
            public string Country   { get; private set; }
            public string Hometown  { get; private set; }
        }

        public IReadOnlyCollection<UserIdentityInfo> GetAllUserIdentityInfos()
        { 
            return 
                UserDummyRepo.AllUsers
                .Select(user => new UserIdentityInfo(user.Username, user.FirstName, user.LastName))
                .ToList();
        }
        public IReadOnlyCollection<UserLocationInfo> GetAllUserLocationInfos()
        { 
            return 
                UserDummyRepo.AllUsers
                .Select(user => new UserLocationInfo(user.Username, user.Country, user.Hometown))
                .ToList();
        }
    }

    public class ComplexUserQueryAdapter : IUserQuery
    {
        public ComplexUserQueryAdapter
        (
            ComplexUserQuery complexUserQuery
        )
        {
            _complexUserQuery = complexUserQuery;
        }
        private readonly ComplexUserQuery _complexUserQuery;

        public IReadOnlyCollection<User> GetAllUsers()
        {
            var userIdentityInfos = _complexUserQuery.GetAllUserIdentityInfos();
            var userLocationInfos = _complexUserQuery.GetAllUserLocationInfos();

            return 
                userIdentityInfos
                .GroupJoin(
                    inner: userLocationInfos,
                    outerKeySelector: (indentityInfo) =>indentityInfo.Username,
                    innerKeySelector: (locationInfos) => locationInfos.Username,
                    resultSelector: 
                        (identityInfo, locationInfos) => 
                            new User(identityInfo.Username, identityInfo.FirstName, identityInfo.LastName, locationInfos.Single().Country,  locationInfos.Single().Hometown)
                )
                .ToList();
        }
    }
}
namespace DesignPatterns.Patterns.Adapter
{
    public abstract class Executor
    {
        public static void Execute()
        {
            var simple = new SimpleUserQuery();
            _printUsers(simple.GetAllUsers());

            var complex = new ComplexUserQuery();
            var adapter = new ComplexUserQueryAdapter(complex);
            _printUsers(adapter.GetAllUsers());
        }

        private static void _printUsers(IEnumerable<User> users)
        {
            foreach (var user in users)
            {
                _printUser(user);
            }
            Console.WriteLine();
        }
        private static void _printUser(User user)
        {
            Console.WriteLine("username: {0} | firstname: {1} | country: {2}", user.Username, user.FirstName, user.Country);
        }
    }
}
