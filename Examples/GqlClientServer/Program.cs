using GqlClientServer.Database;
using System;

namespace GqlClientServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //TimeGraphContext context = new();
            //context.UseInMemoryDatabase("TimeGraphServer");

            var server = new SimpleTCP.SimpleTcpServer().Start(1111);
            server.DelimiterDataReceived += Server_DelimiterDataReceived;
            Console.ReadLine();
        }

        private static void Server_DelimiterDataReceived(object sender, SimpleTCP.Message e)
        {
            Console.WriteLine(e.MessageString);
        }
    }
}

// In package manager console: dotnet add package Microsoft.EntityFrameworkCore.InMemory
/*
dotnet add package HotChocolate
 */