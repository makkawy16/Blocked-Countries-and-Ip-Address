using Blocked_Countries_and_Ip_Address.Repostiores.RegisterServices;
using System.Reflection;

namespace Blocked_Countries_and_Ip_Address.Configurations
{
    public static class ServiceExtensions
    {

        public static void ConfigureISingletoneService(this IServiceCollection services)
        {
            Assembly targetAssembly = Assembly.GetExecutingAssembly();

            Type interfaceType = typeof(ISingletonService);

            //Get Any thing implements ISingletone
            IEnumerable<Type> singletoneServices = targetAssembly.GetTypes()
                .Where(t => interfaceType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

            foreach (Type serviceType in singletoneServices)
            {
                IEnumerable<Type> interfaces = serviceType.GetInterfaces().Where(i => i != typeof(ISingletonService));

                foreach (Type serviceIntefarce in interfaces)
                {
                    // Register the class type for each interface it implements
                    services.AddSingleton(serviceIntefarce, serviceType);
                }
                // Register the class itself
                services.AddSingleton(serviceType);
            }

        }

        public static void ConfigureIScopedService(this IServiceCollection services)
        {
            Assembly targetAssembly = Assembly.GetExecutingAssembly();

            Type interfaceType = typeof(IScopedService);

            //Get Any thing implements IScopedService
            IEnumerable<Type> scopedServices = targetAssembly.GetTypes()
                .Where(t => interfaceType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

            foreach (Type serviceType in scopedServices)
            {
                IEnumerable<Type> interfaces = serviceType.GetInterfaces().Where(i => i != typeof(IScopedService));

                foreach (Type serviceIntefarce in interfaces)
                {
                    // Register the class type for each interface it implements
                    services.AddScoped(serviceIntefarce, serviceType);
                }
                // Register the class itself
                services.AddScoped(serviceType);
            }

        }
    }
}
