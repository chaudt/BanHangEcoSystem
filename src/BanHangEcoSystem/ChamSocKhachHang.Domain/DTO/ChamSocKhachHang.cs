using System;

namespace ChamSocKhachHang.Domain.DTO
{
    public class ChamSocKhachHang
    {
        public int Id { get; set; }
        public int ThongTinKhachHangId { get; set; }
        public string SoDienThoai { get; set; }
        public string TenKhachHang { get; set; }
        public string DiaChi { get; set; }
        public DateTime NgayChamSoc { get; set; }
    }
}
