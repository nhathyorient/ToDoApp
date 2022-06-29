using Microsoft.AspNetCore.Authorization;

namespace WebApp_UnderTheHood
{
    public static class AppConventionalServicesRegisterExtension
    {
        public static IServiceCollection RegisterAllAppAuthorizationHandlers(this IServiceCollection serviceCollection)
        {
            foreach (var authorizationHandlerType in typeof(Program).Assembly.DefinedTypes
                         .Where(p => p.IsAssignableTo(typeof(IAuthorizationHandler))))
            {
                serviceCollection.AddSingleton(serviceType: typeof(IAuthorizationHandler), implementationType: authorizationHandlerType);
            }

            return serviceCollection;
        }
    }
}
