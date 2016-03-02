using Microsoft.Practices.Unity;
using SpatialMaps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpatialMapsWpfUi
{
    public static class Bootstrapper
    {
        public static void Bootstrap(this IUnityContainer ioc)
        {
            ioc.RegisterType<IOService, DesktopIOService>(new ContainerControlledLifetimeManager());
            ioc.RegisterType<IMapsApplicationModel, MapsApplicationModel>(new ContainerControlledLifetimeManager());
        }
    }
}
