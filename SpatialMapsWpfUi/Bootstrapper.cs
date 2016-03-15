using Microsoft.Practices.Unity;
using SpatialMaps;

namespace SpatialMapsWpfUi
{
    public static class Bootstrapper
    {
        public static void Bootstrap(this IUnityContainer ioc)
        {
            ioc.RegisterType<IOService, DesktopIOService>(new ContainerControlledLifetimeManager());
            ioc.RegisterType<ISpatialMapsModel, SpatialMapsModel>(new ContainerControlledLifetimeManager());
            ioc.RegisterType<ISpatialMapsViewModel, SpatialMapsViewModel>(new ContainerControlledLifetimeManager());
        }
    }
}
