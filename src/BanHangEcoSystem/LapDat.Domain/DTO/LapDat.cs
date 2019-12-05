using System;
using System.Collections.Generic;
using System.Text;

namespace LapDat.Domain.DTO
{
    public class LapDat
    {
        public int ThongTinKhachHangId { get; set; }
        public string SoDienThoai { get; set; }
        public string TenKhachHang { get; set; }
        public string DiaChiGiaoHang { get; set; }
        public DateTime NgayLapDat { get; set; }
    }
}
