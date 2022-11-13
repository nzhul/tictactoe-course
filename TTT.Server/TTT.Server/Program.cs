using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using TTT.Server;
using TTT.Server.Infrastructure;

var serviceProvider = Container.Configure();
var _server = serviceProvider.GetRequiredService<NetworkServer>();
_server.Start();

while (true)
{
    _server.PollEvents();
    Thread.Sleep(15);
}