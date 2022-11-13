using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using TTT.Server.Extensions;
using TTT.Server.NetworkShared.Registries;

namespace TTT.Server.Infrastructure
{
    public static class Container
    {
        public static IServiceProvider Configure()
        {
            var services = new ServiceCollection();

            ConfigureServices(services);

            return services.BuildServiceProvider();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(c => c.AddSimpleConsole());
            services.AddSingleton<NetworkServer>();
            services.AddSingleton<PacketRegistry>();
            services.AddSingleton<HandlerRegistry>();
            services.AddPacketHandlers();
        }
    }
}
