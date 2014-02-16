using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns.Patterns.Observer
{
    interface IMeasurements
    {
        int Temperature { get; }
        int Humidity    { get; }
        int Pressure    { get; }
    }

    class WeatherData : IMeasurements
    {
        public WeatherData
        (
        )
        {
            this.Temperature = 20;
            this.Humidity    = 85;
            this.Pressure    = 110000;
        }

        public int Temperature { get; private set; }
        public int Humidity    { get; private set; }
        public int Pressure    { get; private set; }

        private List<IDisplay> subscribers = new List<IDisplay>();
        public void Subscribe(IDisplay display)
        {
            if(!this.subscribers.Any(subscriber => subscriber == display))
            {
                this.subscribers.Add(display);
            }
        }
        public void Unsubscribe(IDisplay display)
        {
            if(this.subscribers.Any(subscriber => subscriber == display))
            {
                this.subscribers.Remove(display);
            }
        }
        private void Update()
        {
            this.subscribers.ForEach(subscriber => subscriber.Display(this));
        }

        public void TemperatureChanged(int currentTemperature)
        {
            Temperature = currentTemperature;
            Update();
        }
    }
    interface IDisplay
    {
        void Display(IMeasurements measurements);
    }
    class Display1 : IDisplay
    {
        public void Display(IMeasurements measurements)
        {
            Console.WriteLine("Tmp changed:{0} (Display 1) ", measurements.Temperature);
        }
    }
    class Display2 : IDisplay
    {
        public void Display(IMeasurements measurements)
        {
            Console.WriteLine("Tmp changed:{0} (Display 2) ", measurements.Temperature);
        }
    }
}
namespace DesignPatterns.Patterns.Observer
{
    public abstract class Executor
    {
        public static void Execute()
        { 
            var data = new WeatherData();

            var d1 = new Display1();
            var d2 = new Display2();

            data.Subscribe(d1);
            data.Subscribe(d2);
            data.TemperatureChanged(25);
            data.Unsubscribe(d2);
            data.TemperatureChanged(27);
        }
    }
}
