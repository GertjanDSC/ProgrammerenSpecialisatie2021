using UnityInPraktijk.Interfaces;

namespace UnityInPraktijk.Manufacturers
{
    class BMW : ICar
    {
        private int _miles = 0;

        public BMW()
        {
            System.Console.WriteLine("BMW");
        }

        public int Run()
        {
            return ++_miles;
        }
    }
}
