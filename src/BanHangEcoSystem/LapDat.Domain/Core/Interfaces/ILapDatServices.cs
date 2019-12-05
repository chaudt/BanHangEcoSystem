using System;

namespace LapDat.Domain.Core.Interfaces
{
    public interface ILapDatServices
    {
        void Subscribe(Action<string> action);
    }
}
