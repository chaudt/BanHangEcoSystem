using System;

namespace GiaoHang.Domain.Core.Interfaces
{
    public interface IGiaoHangServices
    {
        void Subscribe(Action<string> action);
    }
}
