using System;
using System.Diagnostics.Contracts;
using System.Dynamic;
using System.Linq;
using System.Collections.Generic;

using DesignPatterns.Patterns.FactoryMethod.Entities;

namespace DesignPatterns.Patterns.FactoryMethod.Entities
{
    public interface IDroneIdProvider
    {
        int GetNextId();
    }
    public class DroneIdProvider : IDroneIdProvider
    {
        public static IDroneIdProvider GetDroneIdProvider
        {
            get
            {
                if (_droneIdProvider == null)
                {
                    _droneIdProvider = new Lazy<IDroneIdProvider>(() => new DroneIdProvider());
                }
                return _droneIdProvider.Value;
            }
        }
        private static Lazy<IDroneIdProvider> _droneIdProvider;


        private DroneIdProvider()
        {
            currentId = 0;
        }
        private int currentId { get; set; }

        public int GetNextId()
        {
            var current = this.currentId;
            this.currentId = this.currentId + 1;
            return current;
        }
    }

    interface IMissionLocationProvider
    {
         string GetLocationName_byDressCode(DressCode dressCode);
    }

    public enum DressCode
    {
        Red,
        Blue,
        Yellow
    }
    public sealed class Mission : IMissionLocationProvider
    {
        private Dictionary<DressCode, string> DressCodeToLocationName = new Dictionary<DressCode, string>()
        {
            { DressCode.Red,    "Mars"  },
            { DressCode.Blue,   "Titan" },
            { DressCode.Yellow, "Moon"  }
        };
        public string GetLocationName_byDressCode(DressCode dressCode)
        {
            return DressCodeToLocationName[dressCode];
        }
    }

    public enum Purpose
    {
        Scout,
        Repair
    }
    public abstract class Drone
    {
        public Drone
        (
            int id,
            DressCode dressCode
        )
        {
            this.Id           = id;
            this.DressCode    = dressCode;
            this.IsInFunction = true;
            this._MissionLocationProvider = new Mission(); //Tightly coupled
        }
        public abstract Purpose Purpose { get; }

        internal readonly IMissionLocationProvider _MissionLocationProvider;

        public int          Id              { get; private set; }
        public DressCode    DressCode       { get; private set; }
        public bool         IsInFunction    { get; private set;}

        public abstract void DoTheJob();
    }
    class ScoutDrone : Drone
    {
        public ScoutDrone(int id, DressCode dressCode) : base(id, dressCode) { }
        

        public override void DoTheJob()
        {
            Console.WriteLine("Drone {0} is scouting on {1}.", this.Id, _MissionLocationProvider.GetLocationName_byDressCode(this.DressCode));
        }

        public override Purpose Purpose
        {
            get { return Purpose.Scout; }
        }
    }
    class RepairDrone : Drone
    {
        public RepairDrone(int id, DressCode dressCode) : base(id, dressCode) { }


        public override void DoTheJob()
        {
            Console.WriteLine("Drone {0} is repairing on {1}.", this.Id, _MissionLocationProvider.GetLocationName_byDressCode(this.DressCode));
        }

        public override Purpose Purpose
        {
            get { return Purpose.Repair; }
        }
    }
    class DeactivatedDrone : Drone
    {
        public DeactivatedDrone(Drone activeDrone) : base(activeDrone.Id, activeDrone.DressCode)
        {
            this._purpose = activeDrone.Purpose;
        }
        private Purpose _purpose { get; set; }

        public override Purpose Purpose
        {
            get { return this._purpose; }
        }

        public override void DoTheJob()
        {
            Console.WriteLine("Drone {0} is terminated and therefore unoperable.", this.Id);
        }
    }
}
namespace DesignPatterns.Patterns.FactoryMethod
{
    public abstract class DroneCreator
    {
        public DroneCreator
        (
            IDroneIdProvider droneIdProvider
        )
        {
            Contract.Requires(droneIdProvider != null);

            _droneIdProvider         = droneIdProvider;
        }
        protected readonly IDroneIdProvider         _droneIdProvider;

        public abstract Drone Create(Purpose purpose);
        
        public Drone Terminate(Drone drone)
        {
            Console.WriteLine("Drone {0} terminated.", drone.Id);
            return new DeactivatedDrone(drone);
        }
    }

    public class MarsDroneCreator : DroneCreator
    {
        public MarsDroneCreator(IDroneIdProvider droneIdProvider) : base(droneIdProvider) { }
        public override Drone Create(Purpose purpose)
        {
            var id = _droneIdProvider.GetNextId();
            if (purpose == Purpose.Scout) { return new ScoutDrone(id, DressCode.Red); }
            if (purpose == Purpose.Repair) { return new RepairDrone(id, DressCode.Red); }
            return null;
        }
    }
    public class MoonDroneCreator : DroneCreator
    {
        public MoonDroneCreator(IDroneIdProvider droneIdProvider) : base(droneIdProvider) { }
        public override Drone Create(Purpose purpose)
        {
            var id = _droneIdProvider.GetNextId();
            if (purpose == Purpose.Scout) { return new ScoutDrone(id, DressCode.Yellow); }
            if (purpose == Purpose.Repair) { return new RepairDrone(id, DressCode.Yellow); }
            return null;
        }
    }
}
namespace DesignPatterns.Patterns.FactoryMethod
{
    public abstract class Executor
    {
        public static void Execute()
        {
            var marsDroneCreator = new MarsDroneCreator(DroneIdProvider.GetDroneIdProvider);
            var marsScout = marsDroneCreator.Create(Purpose.Scout);
            var marsRepair = marsDroneCreator.Create(Purpose.Repair);

            var moonDroneCreator = new MoonDroneCreator(DroneIdProvider.GetDroneIdProvider);
            var moonRepairer = moonDroneCreator.Create(Purpose.Repair);


            marsScout.DoTheJob();
            marsRepair.DoTheJob();
            marsScout = marsDroneCreator.Terminate(marsScout);
            marsScout.DoTheJob();
        }
    }
}
