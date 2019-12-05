using System;
using System.Collections.Generic;
using System.Text;

namespace DonHang.Domain.Core.Interfaces
{
    public interface IDonHangServices
    {
        void Subscribe(Action<string> action);
    }
}
