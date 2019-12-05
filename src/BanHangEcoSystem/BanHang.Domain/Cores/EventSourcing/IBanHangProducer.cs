using Config.Server.Kafka.Helper;

namespace BanHang.Domain.Cores.EventSourcing
{
    public interface IBanHangProducer
    {
        void PublishMessage(Message_Customer message);
    }
}
