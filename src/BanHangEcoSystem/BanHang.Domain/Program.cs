using BanHang.Domain.Cores.EventSourcing;
using BanHang.Domain.Cores.Interfaces;
using BanHang.Domain.Cores.Services;
using BanHang.Domain.DTO;
using Config.Server;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace BanHang.Domain
{
    class Program
    {
        static int maxNumber=Int32.MaxValue;

        static void Main(string[] args)
        {
            Environment.SetEnvironmentVariable("KAFKA_ADVERTISED_HOST_NAME", "172.16.50.93");
            Environment.SetEnvironmentVariable("KAFKA_ADVERTISED_PORT", "9092");

            Console.Title = "=== MODULE BAN-HANG ===";
            Console.WriteLine("=== MODULE BAN-HANG ===");
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var collection = new ServiceCollection();
            collection.AddScoped<IBanHangServices, BanHangServices>();
            collection.AddScoped<IConnectedDatabase, ConnectedDatabase>();
            collection.AddScoped<IBanHangProducer, BanHangProducer>();

            var serviceProvider = collection.BuildServiceProvider();

            var banhangService = serviceProvider.GetService<IBanHangServices>();

            int key = 0;
            do
            {
                key = PrintMenu();
                switch (key)
                {
                    case 1:
                        var id = ThemMoiDonHang(banhangService);
                        if (id > 0)
                        {
                            MuaHang(id, banhangService);
                        }
                        break;
                    case 2:
                        TruyVanThongTin(banhangService);
                        break;
                    case 3:
                        var idDonHang = ThemMoiDonHang(banhangService);
                        if(idDonHang > 0)
                        {
                            MuaHangNhanh(idDonHang, banhangService);
                        }
                        break;
                    case 4:
                        for (int i = 0; i < maxNumber; i++)
                        {
                            var ids = ThemMoiDonHang(banhangService);
                            if (ids > 0)
                            {
                                MuaHangNhanh(ids, banhangService);
                            }
                            Thread.Sleep(2000);
                        }
                        
                        break;
                    default:
                        break;
                }
            }
            while (key > 0);
            
            serviceProvider.Dispose();
        }


        private static void TruyVanThongTin(IBanHangServices banhangService)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Nhap ma thong tin khach hang: ");
            int id = 0;
            int.TryParse(Console.ReadLine(), out id);
            if (id > 0)
            {
                var info = banhangService.ThongTinDonHang(id);
                if (info == null)
                {
                    Console.WriteLine("==>Khong tim thay don hang nay.");
                }
                else
                {
                    Console.WriteLine($"    Label                        Value");
                    Console.WriteLine($"    -------------------------------------------------------------");
                    Console.WriteLine($"    Ma thong tin khach hang:   | {info.Id}");
                    Console.WriteLine($"    Ten khach hang:            | {info.TenKhachHang}");
                    Console.WriteLine($"    Dia chi:                   | {info.DiaChi}");
                    Console.WriteLine($"    Dien thoai:                | {info.DienThoai}");
                    Console.WriteLine($"    Trang thai don hang:       | {info.TrangThai}");
                    Console.WriteLine($"    Ngay tao:                  | {info.NgayTao}");
                    Console.WriteLine($"    Bi Xoa:                    | {info.IsDeleted}");
                    Console.WriteLine($"    Trang thai khoi tao:       | {info.IsInit}");
                    Console.WriteLine($"    -------------------------------------------------------------");
                }
            }
            else
            {
                Console.WriteLine("Ma thong tin khach hang khong dung format");
            }
        }

        private static void MuaHang(int id, IBanHangServices service)
        {
            var order = new DTO.ThongTinKhachHang();
            order.Id = id;


            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("===== THONG TIN KHACH HANG ====");
            Console.WriteLine("     Ten khach hang:");
            order.TenKhachHang = Console.ReadLine();

            Console.WriteLine("     Dia chi giao hang:");
            order.DiaChi = Console.ReadLine();

            Console.WriteLine("     Dien thoai lien lac:");
            order.DienThoai = Console.ReadLine();

            order.TrangThai = Config.Server.Kafka.Helper.Status.Da_Thanh_Toan.ToString();
            order.NgayTao = DateTime.Now;
            order.IsInit = true;
            order.IsDeleted = false;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("===== SAN PHAM MUA ====");
            int key = 0;
            do
            {
                var sp = new SanPhamMua();
                Console.WriteLine("     Ten San Pham: ");
                sp.TenSanPham = Console.ReadLine();

                int sl = 0;
                Console.WriteLine("     So luong: ");
                int.TryParse(Console.ReadLine(), out sl);
                sp.SoLuong = sl;

                order.SanPhams.Add(sp);

                Console.WriteLine("1: tiep tuc mua hang, 0: mua xong");
                int.TryParse(Console.ReadLine(), out key);
            }
            while (key > 0);

            service.Them_DonHang(order);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Mua hang thanh cong....");
            Console.ResetColor();
        }

        private static void MuaHangNhanh(int id, IBanHangServices service)
        {
            var order = new DTO.ThongTinKhachHang();
            order.Id = id;


            Console.ForegroundColor = ConsoleColor.Green;

            var name = AppUtil.GetRandomName();
            Console.WriteLine("===== THONG TIN KHACH HANG ====");
            Console.WriteLine($"     Ten khach hang: {name}");
            order.TenKhachHang = name;

            var address = AppUtil.GetRandomAddress();
            Console.WriteLine($"     Dia chi giao hang: {address}");
            order.DiaChi = address;

            var phoneNumber = AppUtil.GetRandomPhoneNumber();
            Console.WriteLine($"     Dien thoai lien lac: {phoneNumber}");
            order.DienThoai = phoneNumber;

            order.TrangThai = Config.Server.Kafka.Helper.Status.Da_Thanh_Toan.ToString();
            order.NgayTao = DateTime.Now;
            order.IsInit = true;
            order.IsDeleted = false;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("===== SAN PHAM MUA ====");

            var sp = new SanPhamMua();
            var productName = AppUtil.GetRandomProduct();
            Console.WriteLine($"     Ten San Pham: {productName}");
            sp.TenSanPham = productName;

            int sl = 3;
            var productQuantity = AppUtil.GetRandomNumber();
            Console.WriteLine($"     So luong: {productQuantity}");
            sp.SoLuong = productQuantity;

            order.SanPhams.Add(sp);

            service.Them_DonHang(order);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Mua hang thanh cong....");
            Console.WriteLine();
            Console.ResetColor();
        }

        private static int ThemMoiDonHang(IBanHangServices service)
        {
            int id = service.KhoiTao_DonHang();
            var isSuccess = Print_KhoiTaoDonHang(id);

            return id;
        }

        private static int PrintMenu()
        {
            int key = 0;
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("===== MENU ====");
            Console.WriteLine("     0: Exit program ");
            Console.WriteLine("     1: Them moi don hang ");
            Console.WriteLine("     2: Truy van thong tin ");
            Console.WriteLine("     3: Them nhanh ");
            Console.WriteLine($"     4: Them nhanh {maxNumber} don hang");

            Console.ResetColor();
            int.TryParse(Console.ReadLine(), out key);
            return key;
        }

        private static bool Print_KhoiTaoDonHang(int id)
        {
            var isFlag = false;
            if (id > 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"=> Thong tin khach hang {id} duoc khoi tao thanh cong.");
                isFlag = true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"=> Khoi tao Thong tin khach hang bi that bai.");
            }
            Console.ResetColor();
            return isFlag;
        }
    }
}
