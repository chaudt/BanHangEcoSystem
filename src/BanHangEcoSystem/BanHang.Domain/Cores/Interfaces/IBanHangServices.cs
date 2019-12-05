using BanHang.Domain.DTO;

namespace BanHang.Domain.Cores.Interfaces
{
    public interface IBanHangServices
    {
        int KhoiTao_DonHang();

        void Them_DonHang(ThongTinKhachHang order);

        ThongTinKhachHang ThongTinDonHang(int id);
    }
}
