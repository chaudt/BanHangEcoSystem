using System.Linq;
using BanHang.Domain.Cores.EventSourcing;
using BanHang.Domain.Cores.Interfaces;
using BanHang.Domain.DTO;
using Config.Server;
using Config.Server.Kafka.Helper;
using Dapper;

namespace BanHang.Domain.Cores.Services
{
    public class BanHangServices : IBanHangServices
    {
        private readonly IConnectedDatabase _connection;

        private readonly IBanHangProducer _producer;

        public BanHangServices(IConnectedDatabase connection,
                               IBanHangProducer producer)
        {
            _connection = connection;
            _producer = producer;
        }

        #region IBanHangServices Members

        public int KhoiTao_DonHang()
        {
            int id = 0;
            using (var db = _connection.GetConnection())
            {
                id = db.ExecuteScalar<int>("INSERT INTO info.ThongTinKhachHang(IsInit) OUTPUT INSERTED.Id VALUES(@isInit)", new
                                                                                                                            {
                                                                                                                                    isInit = false
                                                                                                                            });
            }

            return id;
        }

        public void Them_DonHang(ThongTinKhachHang order)
        {
            using (var db = _connection.GetConnection())
            {
                // insert ThongTinKhachHang
                db.Execute("UPDATE info.ThongTinKhachHang SET TenKhachHang=@TenKhachHang,DiaChi=@DiaChi,DienThoai=@DienThoai,TrangThai=@TrangThai,NgayTao=@NgayTao,IsDeleted=@IsDeleted, IsInit=@IsInit WHERE Id=@Id;",
                           new
                           {
                                   order.Id,
                                   order.TenKhachHang,
                                   order.DiaChi,
                                   order.DienThoai,
                                   order.TrangThai,
                                   order.NgayTao,
                                   order.IsDeleted,
                                   order.IsInit
                           });
                // insert SanPhamMua

                db.Execute("INSERT INTO info.SanPhamMua(ThongTinKhachHangId,TenSanPham,SoLuong) VALUES(@ThongTinKhachHangId,@TenSanPham,@SoLuong);",
                           order.SanPhams.Select(p => new
                                                      {
                                                              ThongTinKhachHangId = order.Id,
                                                              p.TenSanPham,
                                                              p.SoLuong
                                                      }));
            }

            // producer publish message
            _producer.PublishMessage(new Message_Customer
                                     {
                                             Id = order.Id, //ThongTinKhachHangId
                                             CustomerAddress = order.DiaChi,
                                             PhoneNumber = order.DienThoai,
                                             CustomerName = order.TenKhachHang,
                                             Status = Status.Da_Thanh_Toan
                                     });
        }

        public ThongTinKhachHang ThongTinDonHang(int id)
        {
            var result = new ThongTinKhachHang();
            using (var db = _connection.GetConnection())
            {
                result = db.QuerySingle<ThongTinKhachHang>("SELECT * FROM info.ThongTinKhachHang WHERE Id=@Id;", new
                                                                                                                 {
                                                                                                                         Id = id
                                                                                                                 });
                result.SanPhams = db
                                  .Query<SanPhamMua
                                  >("SELECT Id,TenSanPham,SoLuong FROM info.SanPhamMua WHERE ThongTinKhachHangId=@ThongTinKhachHangId;",
                                    new
                                    {
                                            ThongTinKhachHangId = id
                                    })
                                  .ToList();
            }

            return result;
        }

        #endregion
    }
}
