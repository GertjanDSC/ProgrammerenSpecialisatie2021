using Unity;
using UnityInPraktijk.Interfaces;
using UnityInPraktijk.ManufacturerKeys;
using UnityInPraktijk.Manufacturers;

namespace UnityInPraktijk
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new UnityContainer(); // we instantieren onze IOC Container; meestal maar eenmaal per app in de Main

            /*
            container.RegisterType<Driver>(new InjectionConstructor(new object[] { new Audi(), "Steve" }));
            var driver = container.Resolve<Driver>(); // Injects Audi and Steve; hier staat nergens new
            driver.RunCar();
            */
            /*
            Driver driver = new(new BMW(), "Piet");
            driver.RunCar();
            */

            container.RegisterType<ICar, BMW>();
            container.RegisterType<ICarKey, BMWKey>();
            Driver drv = container.Resolve<Driver>();
            Driver drv2 = container.Resolve<Driver>();
            drv.RunCar();
            drv2.RunCar();
        }
    }
}
