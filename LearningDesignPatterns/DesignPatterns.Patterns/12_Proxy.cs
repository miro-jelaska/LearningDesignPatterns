using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesignPatterns.Patterns.Proxy
{
    interface IGumballMachine
    {
        int    Id { get; }
        int    BallCount { get; set; }
        string Location  { get; }
    }

    class RemoteGumballMachine : IGumballMachine
    {
        public RemoteGumballMachine
        (
            int id,
            int ballCount,
            string location
        )
        {
            this.Id = id;
            this.BallCount = ballCount;
            this.Location = location;
        }

        public int    Id        {get; private set;}
        public int    BallCount {get; set;}
        public string Location  {get; private set;}
    }

    static class Server
    {
        private static List<IGumballMachine> machines = new List<IGumballMachine>(){
            new RemoteGumballMachine(45, 200, "Allen Street 35a"),
            new RemoteGumballMachine(32, 10, "Ann Street  10"),
            new RemoteGumballMachine(11, 180, "Forsyth Street 75c"),
            new RemoteGumballMachine(15, 127, "Irving Place 105"),
        };

        public static Task<Tuple<int, int, string>> GetGumballMachine_byId(int id)
        {
            return new Task<Tuple<int, int, string>>(() => {

                var searchedMachine = machines.SingleOrDefault(machine => machine.Id == id);

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));

                return new Tuple<int, int, string>(searchedMachine.Id, searchedMachine.BallCount, searchedMachine.Location);
            });
        }
        
        public static Action UpdateGumballMachine_ballCount(int machineId,int newBallCount)
        {
            return new Action(() => {

                var searchedMachine = machines.SingleOrDefault(machine => machine.Id == machineId);

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
                
                searchedMachine.BallCount = newBallCount;
            });
        }
    }

    class GumballMachineProxy : IGumballMachine
    { 
        public GumballMachineProxy
        (
            int id
        )
        {
            this.Id = id;
        }

        public int    Id        { get; private set; }
        public int    BallCount
        {
            get
            {
                var a = Server.GetGumballMachine_byId(Id).Result.Item2;
                return a;
            }
            
            set
            {
                Server.UpdateGumballMachine_ballCount(Id, value);
            }
        }
        public string Location 
        {
            get
            {
                return Server.GetGumballMachine_byId(Id).Result.Item3;
            }
        }
    }

}
namespace DesignPatterns.Patterns.Proxy
{
    public abstract class Executor
    {
        public static void Execute()
        {
            var proxy1 = new GumballMachineProxy(45);
            var number = proxy1.BallCount;
            Console.WriteLine(number);
        }
    }
}
