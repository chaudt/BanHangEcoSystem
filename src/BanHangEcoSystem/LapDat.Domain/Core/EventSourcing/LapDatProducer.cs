using System.Text;
using Config.Server.Kafka.Configs;
using Config.Server.Kafka.Helper;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace LapDat.Domain.Core.EventSourcing
{
    public class LapDatProducer : ILapDatProducer
    {
        #region ILapDatProducer Members

        public void Publish(Message_Customer message)
        {
            var config = KafkaConfigManagement.Instance;
            var msg = JsonConvert.SerializeObject(message);
            using (var producer = new ProducerBuilder<Null, string>(config.GetProducerConfig()).Build())
            {
                //producer.ProduceAsync(config.GetTopic2, null, msg);
                //producer.ProduceAsync(config.GetTopic3, null, msg); // publish to trang thai
                //producer.Flush(config.TimeOut);

                producer.ProduceAsync(config.GetTopic2, new Message<Null, string>
                                                        {
                                                                Value = msg
                                                        })
                        .ContinueWith(c =>
                                      {
                                      });
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
