using System;

namespace ChamSocKhachHang.Domain.Core.Interfaces
{
    public interface IChamSocKhachHangServices
    {
        void Subscribe(Action<string> action);
    }
}
