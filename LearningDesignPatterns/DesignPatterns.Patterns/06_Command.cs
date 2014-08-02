using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text.RegularExpressions;

using DesignPatterns.Patterns.Command.Entities;

namespace DesignPatterns.Patterns.Command.Entities
{
    public interface IDevice
    {
        Code RemoteCode { get; }
    }
    public class Code
    {
        public Code(string code)
        {
            Contract.Requires(Regex.IsMatch(code, @"^\d{4}$"));
            this.Value = code;
        }
        public string Value { get; private set; }

        public static Code FactoryDefault { get { return new Code("0000"); } }
    }

    public class Tv : IDevice
    {
        public Tv
        (
            Code remoteCode
        )
        {
            Contract.Requires(remoteCode != null);

            this.RemoteCode    = remoteCode;
            this.Channel = 0;
            _isTurnedOn  = false;
        }
        public  Code RemoteCode        { get; private set; }
        public  int  Channel     { get; private set; }
        private bool _isTurnedOn { get; set; }

        public void TurnOn()
        {
            _isTurnedOn = true;
            Console.WriteLine("TV turned ON.");
        }
        public void TurnOff()
        {
            _isTurnedOn = false;
            Console.WriteLine("TV turned OFF.");
        }
        public void ChangeChannel(int newChannel)
        {
            this.Channel = newChannel;
            Console.WriteLine("Channel set to {0}", this.Channel);
        }
    }

    public enum Temperature
    {
        High,
        Medium,
        Low
    }
    public class AirConditioner : IDevice
    {
        public AirConditioner
        (
            Code remoteCode
        )
        {
            Contract.Requires(remoteCode != null);

            this.RemoteCode        = remoteCode;
            this.Temperature = Temperature.Medium;
            _isTurnedOn      = false;
        }
        public  Code         RemoteCode            { get; private set; }
        public  Temperature  Temperature     { get; private set; }
        private bool         _isTurnedOn     { get; set; }

        public void TurnOn()
        {
            _isTurnedOn = true;
            Console.WriteLine("AirConditioner turned ON.");
        }
        public void TurnOff()
        {
            _isTurnedOn = false;
            Console.WriteLine("AirConditioner turned OFF.");
        }
        public void SetTemperature(Temperature newTemperature)
        {
            Contract.Requires(newTemperature != null);
            this.Temperature = newTemperature;
            Console.WriteLine("Temperature set to {0}", this.Temperature);
        }
    }

    public interface IRemoteCommand
    {
        void Execute(IDevice device);
        IDevice RemoteControllerCode { get; }
    }
    public class TurnOnTV : IRemoteCommand
    {
        public TurnOnTV
        (
            RemoteController remoteController
        )
        {
            this.RemoteControllerCode = remoteController;
        }
        public IDevice   RemoteControllerCode { get; private set; }

        public void Execute(IDevice device)
        {
            Contract.Requires(device != null);

            var tv = device as Tv;
            if(tv != null && tv.RemoteCode.Value == RemoteControllerCode.RemoteCode.Value)
            {
                tv.TurnOn();
            }
        }
    }
    public class TurnOffTV : IRemoteCommand
    {
        public TurnOffTV
        (
            RemoteController remoteController
        )
        {
            this.RemoteControllerCode = remoteController;
        }
        public IDevice   RemoteControllerCode { get; private set; }

        public void Execute(IDevice device)
        {
            Contract.Requires(device != null);

            var tv = device as Tv;
            if(tv != null && tv.RemoteCode.Value == RemoteControllerCode.RemoteCode.Value)
            {
                tv.TurnOff();
            }
        }
    }
    public class ChangeChannelTV : IRemoteCommand
    {
        public ChangeChannelTV
        (
            RemoteController remoteController,
            int newChannel
        )
        {
            this.RemoteControllerCode         = remoteController;
            this._newChannel = newChannel;
        }
        public IDevice   RemoteControllerCode { get; private set; }
        private readonly int _newChannel;

        public void Execute(IDevice device)
        {
            Contract.Requires(device != null);

            var tv = device as Tv;
            if(tv != null && tv.RemoteCode.Value == RemoteControllerCode.RemoteCode.Value)
            {
                tv.ChangeChannel(_newChannel);
            }
        }
    }

    public class TurnOnAirConditioner : IRemoteCommand
    {
        public TurnOnAirConditioner
        (
            RemoteController remoteController
        )
        {
            this.RemoteControllerCode = remoteController;
        }
        public IDevice   RemoteControllerCode { get; private set; }

        public void Execute(IDevice device)
        {
            Contract.Requires(device != null);

            var airConditioner = device as AirConditioner;
            if(airConditioner != null && airConditioner.RemoteCode.Value == RemoteControllerCode.RemoteCode.Value)
            {
                airConditioner.TurnOn();
            }
        }
    }
    public class TurnOffAirConditioner : IRemoteCommand
    {
        public TurnOffAirConditioner
        (
            RemoteController remoteController
        )
        {
            this.RemoteControllerCode = remoteController;
        }
        public IDevice   RemoteControllerCode { get; private set; }

        public void Execute(IDevice device)
        {
            Contract.Requires(device != null);

            var airConditioner = device as AirConditioner;
            if(airConditioner != null && airConditioner.RemoteCode.Value == RemoteControllerCode.RemoteCode.Value)
            {
                airConditioner.TurnOff();
            }
        }
    }
    public class ChangeAirConditionerTemperature : IRemoteCommand
    {
        public ChangeAirConditionerTemperature
        (
            Temperature       newTemperature,
            RemoteController  remoteController
        )
        {
            _newTemperature   = newTemperature;
            RemoteControllerCode = remoteController;
        }
        private readonly Temperature _newTemperature;
        public IDevice  RemoteControllerCode { get; private set; }

        public void Execute(IDevice device)
        {
            Contract.Requires(device != null);

            var airConditioner = device as AirConditioner;
            if(airConditioner != null && airConditioner.RemoteCode.Value == RemoteControllerCode.RemoteCode.Value)
            {
                airConditioner.SetTemperature(_newTemperature);
                Console.WriteLine("AirConditioner temperature set to {0}", _getTemperatureName(_newTemperature));
            }
        }
        private string _getTemperatureName(Temperature temperature)
        {
            var temperatureToName = new Dictionary<Temperature, string>()
            {
                { Temperature.High,   "High"   },
                { Temperature.Medium, "Medium" },
                { Temperature.Low,    "Low"    }
            };
            return temperatureToName[temperature];
        }
    }

    public class RemoteController : IDevice
    {
        public RemoteController
        (
            Code remoteCode
        ) 
        {
            this.RemoteCode  = remoteCode;
            this._commands   = new List<IRemoteCommand>();
            this._deviseList = new List<IDevice>();
        }
        public  Code RemoteCode   { get; private set; }
        
        private readonly List<IDevice>        _deviseList;
        private readonly List<IRemoteCommand> _commands;


        public void AddDevice(IDevice device)
        {
            _deviseList.Add(device);
        }
        public void AddCommand(IRemoteCommand newCommand)
        {
            _commands.Add(newCommand);
        }
        public void PressButton(int number)
        {
            Contract.Requires(number >= 0);
            _deviseList.ForEach(_commands[number].Execute);
        }
        public void ChangeCode(Code newCode)
        {
            Contract.Requires(newCode != null);
            this.RemoteCode = newCode;
        }
    }
}
namespace DesignPatterns.Patterns.Command
{
    public abstract class Executor
    {
        public static void Execute()
        {
            var masterCode = new Code("2501");
            var remote = new RemoteController(masterCode);
            (new List<IDevice>(){
            
                new Tv(Code.FactoryDefault),
                new Tv(masterCode),
                new AirConditioner(masterCode),
            }).ForEach(remote.AddDevice);

            (new List<IRemoteCommand>(){
                new TurnOnTV(remote),
                new TurnOffTV(remote),
                new ChangeChannelTV(remote, 11),
                new TurnOnAirConditioner(remote),
                new ChangeAirConditionerTemperature(Temperature.High, remote)

            }).ForEach(remote.AddCommand);
            
            remote.PressButton(0);
            remote.PressButton(1);
            remote.PressButton(2);
            remote.PressButton(4);
        }
    }
}
