using ChamSocKhachHang.Domain.Core.EventSourcing;
using ChamSocKhachHang.Domain.Core.Interfaces;
using ChamSocKhachHang.Domain.Core.Services;
using Config.Server;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ChamSocKhachHang.Domain
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "=== MODULE CHAM-SOC-KHACH-HANG ===";
            Console.WriteLine("=== MODULE CHAM-SOC-KHACH-HANG ===");
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var collection = new ServiceCollection();
            collection.AddScoped<IChamSocKhachHangServices, ChamSocKhachHangServices>();
            collection.AddScoped<IConnectedDatabase, ConnectedDatabase>();
            collection.AddScoped<IChamSocKhachHangProducer, ChamSocKhachHangProducer>();

            var serviceProvider = collection.BuildServiceProvider();

            var cskhService = serviceProvider.GetService<IChamSocKhachHangServices>();
            cskhService.Subscribe(Console.WriteLine);
            serviceProvider.Dispose();
        }
    }
}
