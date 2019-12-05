using Config.Server.Kafka.Configs;
using Config.Server.Kafka.Helper;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace ChamSocKhachHang.Domain.Core.EventSourcing
{
    public class ChamSocKhachHangProducer : IChamSocKhachHangProducer
    {
        #region IChamSocKhachHangProducer Members

        public void Publish(Message_Customer message)
        {
            var config = KafkaConfigManagement.Instance;
            var msg = JsonConvert.SerializeObject(message);
            using (var producer = new ProducerBuilder<Null, string>(config.GetProducerConfig()).Build())
            {
                //producer.ProduceAsync(config.GetTopic3, null, msg);
                //producer.Flush(config.TimeOut);

                producer.ProduceAsync(config.GetTopic3, new Message<Null, string>
                                                        {
                                                                Value = msg
                                                        })
                        .ContinueWith(c =>
                                      {
                                      });
                producer.Flush(config.TimeOut);
            }
        }

        #endregion
    }
}
