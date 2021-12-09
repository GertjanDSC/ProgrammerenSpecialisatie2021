using UnityInPraktijk.Interfaces;

namespace UnityInPraktijk.Manufacturers
{
    class Audi : ICar
    {
        private int _miles = 0;

        public int Run()
        {
            return ++_miles; // preincrement: eerst +; anders: _miles++ (postincrement)
        }
    }
}
