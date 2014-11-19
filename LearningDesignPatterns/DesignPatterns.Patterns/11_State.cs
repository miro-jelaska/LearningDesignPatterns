using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns.Patterns.State
{
    class GumballMachine
    {
        public GumballMachine
        (
            int gumballCount
        )
        {
            Contract.Requires(gumballCount >= 0);
            this.GumballCount = gumballCount;
            if(gumballCount == 0)
            {
                this.State = new _SoldOut(this);
            }
            else
            {
                this.State = new _NoQuarter(this);
            }
        }
        protected int                 GumballCount { get; set; }
        protected IGumballMachineState State       { get; set; }

        protected interface IGumballMachineState
        {
            void InsertQuarter  ();
            void EjectsQuarter  ();
            void TurnsCrank     ();
            void DispenseGumball();
        }
        class _NoQuarter : IGumballMachineState
        {
            public _NoQuarter
            (
                GumballMachine machine
            )
            {
                Contract.Requires(machine != null);
                this.machine = machine;
            }
            private readonly GumballMachine machine;


            public void InsertQuarter()
            {
                Console.WriteLine("You inserted a quarter");
                machine.State = new _HasQuarter(machine);
            }
            public void EjectsQuarter()
            {
                Console.WriteLine("You haven’t inserted a quarter");
            }
            public void TurnsCrank()
            {
                Console.WriteLine("You turned but there’s no quarter");
            }
            public void DispenseGumball()
            {
                Console.WriteLine("You need to pay first");
            }
        }
        class _HasQuarter : IGumballMachineState
        {
            public _HasQuarter
            (
                GumballMachine machine
            )
            {
                Contract.Requires(machine != null);
                this.machine = machine;
            }
            private readonly GumballMachine machine;


            public void InsertQuarter()
            {
                Console.WriteLine("You can’t insert another quarter");
            }
            public void EjectsQuarter()
            {
                Console.WriteLine("Quarter returned");
                machine.State = new _NoQuarter(machine);
            }
            public void TurnsCrank()
            {
                Console.WriteLine("You turned...");
                machine.State = new _Sold(machine);
            }
            public void DispenseGumball()
            {
                Console.WriteLine("No gumball dispensed");
            }
        }
        class _Sold : IGumballMachineState
        {
            public _Sold
            (
                GumballMachine machine
            )
            {
                Contract.Requires(machine != null);
                
                this.machine = machine;
            }
            private readonly GumballMachine machine;


            public void InsertQuarter()
            {
                Console.WriteLine("Please wait, we’re already giving you a gumball");
            }

            public void EjectsQuarter()
            {
                Console.WriteLine("Sorry, you already turned the crank");
            }

            public void TurnsCrank()
            {
                Console.WriteLine("Turning twice doesn’t get you another gumball");
            }

            public void DispenseGumball()
            {
                Console.WriteLine("A gumball comes rolling out the slot");
                machine.GumballCount = machine.GumballCount - 1;
                if(machine.GumballCount == 0)
                {
                    machine.State = new _SoldOut(machine);
                }
                else
                {
                    machine.State = new _NoQuarter(machine);
                }
            }
        }
        class _SoldOut : IGumballMachineState
        {
            public _SoldOut
            (
                GumballMachine machine
            )
            {
                Contract.Requires(machine != null);
                Console.WriteLine("Oops, out of gumballs!");
                this.machine = machine;
            }
            private readonly GumballMachine machine;


            public void InsertQuarter()
            {
                Console.WriteLine("You can’t insert a quarter, the machine is sold out");
            }
            public void EjectsQuarter()
            {
                Console.WriteLine("You can’t eject, you haven’t inserted a quarter yet");
            }
            public void TurnsCrank()
            {
                Console.WriteLine("You turned, but there are no gumballs");
            }
            public void DispenseGumball()
            {
                Console.WriteLine("No gumball dispensed");
            }
        }
    
        public void InsertQuarter()
        {
            State.InsertQuarter();
        }
        public void EjectsQuarter()
        {
            State.EjectsQuarter();
        }
        public void TurnsCrank()
        {
            State.TurnsCrank();
        }
        public void DispenseGumball()
        {
            State.DispenseGumball();
        }
    }
}
namespace DesignPatterns.Patterns.State
{
    public abstract class Executor
    {
        public static void Execute()
        {
            var machine = new GumballMachine(1);
            machine.InsertQuarter();
            machine.EjectsQuarter();
            machine.InsertQuarter();
            machine.TurnsCrank();
            machine.DispenseGumball();
            machine.TurnsCrank();
        }
    }
}
