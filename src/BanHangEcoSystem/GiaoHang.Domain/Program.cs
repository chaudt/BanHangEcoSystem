using System;
using System.Text;
using Config.Server;
using GiaoHang.Domain.Core.EventSourcing;
using GiaoHang.Domain.Core.Interfaces;
using GiaoHang.Domain.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GiaoHang.Domain
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title="=== MODULE GIAO-HANG ===";
            Console.WriteLine("=== MODULE GIAO-HANG ===");
            Console.OutputEncoding = Encoding.UTF8;
            var collection = new ServiceCollection();
            collection.AddScoped<IGiaoHangServices, GiaoHangServices>();
            collection.AddScoped<IConnectedDatabase, ConnectedDatabase>();
            collection.AddScoped<IGiaoHangProducer, GiaoHangProducer>();

            var serviceProvider = collection.BuildServiceProvider();

            var giaohangService = serviceProvider.GetService<IGiaoHangServices>();
            giaohangService.Subscribe(Console.WriteLine);
            serviceProvider.Dispose();
        }
    }
}
