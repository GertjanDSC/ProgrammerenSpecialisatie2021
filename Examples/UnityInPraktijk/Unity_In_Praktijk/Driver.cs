using System;
using UnityInPraktijk.Interfaces;

namespace UnityInPraktijk
{
    class Driver
    {
        private readonly ICar _car = null;
        private readonly string _name = string.Empty;
        private readonly ICarKey _key = null;

        public Driver(ICar car)
        {
            _car = car;
            _name = "Unknown";
        }

        public Driver(ICar car, ICarKey key)
        {
            _car = car;
            _key = key;
            _name = "Unknown";
        }

        public Driver(ICar car, string name)
        {
            _car = car;
            _name = name;
        }
        public void RunCar()
        {
            Console.WriteLine("Running {0} with {1} - {2} Mile", _car.GetType().Name, _key.GetType().Name, _car.Run());
            // Console.WriteLine("{0} is running {1} - {2} Mile",_name, _car.GetType().Name, _car.Run());
        }
    }
}
