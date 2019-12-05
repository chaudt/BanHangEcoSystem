using System;

namespace GiaoHang.Domain.DTO
{
    public class GiaoHang
    {
        public int ThongTinKhachHangId { get; set; }
        public DateTime NgayGiao { get; set; }
        public string DiaChiGiaoHang { get; set; }
        public string SoDienThoai { get; set; }
        public string TenKhachHang { get; set; }
    }
}
