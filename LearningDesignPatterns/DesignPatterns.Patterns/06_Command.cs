using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text.RegularExpressions;

namespace DesignPatterns.Patterns.Command.Entities
{
    interface IRemoteCode
    {
        Code Code { get; }
    }
    public class Code
    {
        public Code(string code)
        {
            Contract.Requires(Regex.IsMatch(code, @"^\d{4}$"));
        }
        public string Value { get; private set; }
    }

    public class Tv : IRemoteCode
    {
        public Tv
        (
            Code code
        )
        {
            Contract.Requires(code != null);

            this.Code    = code;
            this.Channel = 0;
            _isTurnedOn  = false;
        }
        public  Code Code        { get; private set; }
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

    enum Temperature
    {
        High,
        Medium,
        Low
    }
    public class AirConditioner : IRemoteCode
    {
        public AirConditioner
        (
            Code code
        )
        {
            Contract.Requires(code != null);

            this.Code        = code;
            this.Temperature = Temperature.Medium;
            _isTurnedOn      = false;
        }
        public  Code         Code            { get; private set; }
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
        void Execute(IRemoteCode remoteControllerCode);
        IRemoteCode RemoteControllerCode;
    }
    public class TurnOnTV : IRemoteCommand
    {
        public TurnOnTV
        (
            Tv tv
        )
        {
            this._tv = tv;
        }
        private readonly Tv _tv;

        public void Execute(IRemoteCode remoteControllerCode)
        {
            Contract.Requires(remoteControllerCode != null);
            if(this._tv.Code.Value == remoteControllerCode.Code.Value)
            {
                this._tv.TurnOn();
            }
        }
    }
    public class TurnOffTV : IRemoteCommand
    {
        public TurnOffTV
        (
            Tv tv
        )
        {
            this._tv = tv;
        }
        private readonly Tv _tv;

        public void Execute(IRemoteCode remoteControllerCode)
        {
            Contract.Requires(remoteControllerCode != null);
            if(this._tv.Code.Value == remoteControllerCode.Code.Value)
            {
                this._tv.TurnOff();
            }
        }
    }
    public class ChangeChannelTV : IRemoteCommand
    {
        public ChangeChannelTV
        (
            Tv tv,
            int newChannel
        )
        {
            this._tv         = tv;
            this._newChannel = newChannel;
        }
        private readonly Tv  _tv;
        private readonly int _newChannel;

        public void Execute(IRemoteCode remoteControllerCode)
        {
            Contract.Requires(remoteControllerCode != null);
            if(this._tv.Code.Value == remoteControllerCode.Code.Value)
            {
                this._tv.ChangeChannel(_newChannel);
            }
        }
    }

    public class TurnOnAirConditioner : IRemoteCommand
    {
        public TurnOnAirConditioner
        (
            AirConditioner airConditioner
        )
        {
            this._airConditioner = airConditioner;
        }
        private readonly AirConditioner _airConditioner;

        public void Execute(IRemoteCode remoteControllerCode)
        {
            Contract.Requires(remoteControllerCode != null);
            if(this._airConditioner.Code.Value == remoteControllerCode.Code.Value)
            {
                this._airConditioner.TurnOn();
            }
        }
    }
    public class TurnOffAirConditioner : IRemoteCommand
    {
        public TurnOffAirConditioner
        (
            AirConditioner airConditioner
        )
        {
            this._airConditioner = airConditioner;
        }
        private readonly AirConditioner _airConditioner;

        public void Execute(IRemoteCode remoteControllerCode)
        {
            Contract.Requires(remoteControllerCode != null);
            if(this._airConditioner.Code.Value == remoteControllerCode.Code.Value)
            {
                this._airConditioner.TurnOff();
            }
        }
    }
    public class TurnOffAirSetTemperature : IRemoteCommand
    {
        public TurnOffAirSetTemperature
        (
            AirConditioner airConditioner,
            Temperature    newTemperature
        )
        {
            this._airConditioner = airConditioner;
        }
        private readonly AirConditioner _airConditioner;
        private readonly Temperature    _newTemperature;

        public void Execute(IRemoteCode remoteControllerCode)
        {
            Contract.Requires(remoteControllerCode != null);
            if(this._airConditioner.Code.Value == remoteControllerCode.Code.Value)
            {
                this._airConditioner.SetTemperature(_newTemperature);
            }
        }
    }

    public class RemoteController : IRemoteCode
    {
        public RemoteController
        (
            Code code
        ) 
        {
            this.Code      = code;
            this._commands = new List<IRemoteCommand>();
        }
        public  Code Code   { get; private set; }
        
        private readonly List<IRemoteCommand> _commands;


        public void AddCommand(IRemoteCommand newCommand)
        {
            _commands.Add(newCommand);
        }
        public void PressButton(int number)
        {
            _commands[number].Execute(this);
        }
        public void ChangeCode(Code newCode)
        {
            Contract.Requires(newCode != null);
            this.Code = newCode;
        }
    }
}
namespace DesignPatterns.Patterns.Command
{
    public abstract class Executor
    {
        public static void Execute()
        {
            
        }
    }
}
