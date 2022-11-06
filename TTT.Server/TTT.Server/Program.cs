using System.Threading;
using TTT.Server;

var server = new NetworkServer();
server.Start();

while (true)
{
    server.PollEvents();
    Thread.Sleep(15);
}