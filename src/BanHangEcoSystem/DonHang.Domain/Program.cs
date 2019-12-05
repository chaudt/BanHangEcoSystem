using System;
using System.Text;
using Config.Server;
using DonHang.Domain.Core.Interfaces;
using DonHang.Domain.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DonHang.Domain
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "=== MODULE DON-HANG ===";
            Console.WriteLine("=== MODULE DON-HANG ===");
            Console.OutputEncoding = Encoding.UTF8;
            var collection = new ServiceCollection();
            collection.AddScoped<IDonHangServices, DonHangServices>();
            collection.AddScoped<IConnectedDatabase, ConnectedDatabase>();

            var serviceProvider = collection.BuildServiceProvider();

            var donhangService = serviceProvider.GetService<IDonHangServices>();
            donhangService.Subscribe(Console.WriteLine);
            serviceProvider.Dispose();
        }
    }
}
