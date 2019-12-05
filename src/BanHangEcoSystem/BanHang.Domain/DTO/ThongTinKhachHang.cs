using System;
using System.Collections.Generic;

namespace BanHang.Domain.DTO
{
    public class ThongTinKhachHang
    {
        public ThongTinKhachHang()
        {
            SanPhams = new List<SanPhamMua>();
        }

        public int Id { get; set; }

        public string TenKhachHang { get; set; }

        public string DiaChi { get; set; }

        public string DienThoai { get; set; }

        public string TrangThai { get; set; }

        public DateTime NgayTao { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsInit { get; set; }

        public List<SanPhamMua> SanPhams { get; set; }
    }
}
