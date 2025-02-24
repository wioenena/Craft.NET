using System.Net;
using wioenena.Craft.NET.Server;

var server = new CraftServer(new(IPAddress.Loopback, 25565, 100, "A Minecraft Server"));

server.Start();
