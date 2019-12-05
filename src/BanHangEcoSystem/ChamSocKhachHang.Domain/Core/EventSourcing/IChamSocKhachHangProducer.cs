using Config.Server.Kafka.Helper;

namespace ChamSocKhachHang.Domain.Core.EventSourcing
{
    public interface IChamSocKhachHangProducer
    {
        void Publish(Message_Customer message);
    }
}
