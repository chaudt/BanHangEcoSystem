using Config.Server.Kafka.Helper;

namespace LapDat.Domain.Core.EventSourcing
{
    public interface ILapDatProducer
    {
        void Publish(Message_Customer message);
    }
}
