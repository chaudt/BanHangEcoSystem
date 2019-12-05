using System;
using System.Text;
using Config.Server;
using LapDat.Domain.Core.EventSourcing;
using LapDat.Domain.Core.Interfaces;
using LapDat.Domain.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LapDat.Domain
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "=== MODULE LAP-DAT ===";
            Console.WriteLine("=== MODULE LAP-DAT ===");
            Console.OutputEncoding = Encoding.UTF8;
            var collection = new ServiceCollection();
            collection.AddScoped<ILapDatServices, LapDatServices>();
            collection.AddScoped<IConnectedDatabase, ConnectedDatabase>();
            collection.AddScoped<ILapDatProducer, LapDatProducer>();

            var serviceProvider = collection.BuildServiceProvider();

            var lapdatService = serviceProvider.GetService<ILapDatServices>();
            lapdatService.Subscribe(Console.WriteLine);
            serviceProvider.Dispose();
        }
    }
}
