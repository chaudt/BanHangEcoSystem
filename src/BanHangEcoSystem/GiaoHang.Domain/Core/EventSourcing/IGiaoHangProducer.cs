using Config.Server.Kafka.Helper;

namespace GiaoHang.Domain.Core.EventSourcing
{
    public interface IGiaoHangProducer
    {
        void Publish(Message_Customer message);
    }
}
