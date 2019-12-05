using Config.Server.Kafka.Configs;
using Config.Server.Kafka.Helper;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace BanHang.Domain.Cores.EventSourcing
{
    public class BanHangProducer : IBanHangProducer
    {
        #region IBanHangProducer Members

        public void PublishMessage(Message_Customer message)
        {
            var config = KafkaConfigManagement.Instance;
            var msg = JsonConvert.SerializeObject(message);
            //using (var producer = new Producer<Null, string>(config.GetConfigProducer(), null, new StringSerializer(Encoding.UTF8)))
            //{
            //    producer.ProduceAsync(config.GetTopic, null, msg).GetAwaiter().GetResult();
            //    //producer.Flush(config.TimeOut);
            //}

            using (var producer = new ProducerBuilder<Null, string>(config.GetProducerConfig()).Build())
            {
                producer.ProduceAsync(config.GetTopic, new Message<Null, string>
                {
                    Value = msg
                })
                        .GetAwaiter()
                        .GetResult();

                //for (int i = 0; i < 10; i++)
                //{
                //    producer.ProduceAsync(new TopicPartition(config.GetTopic, new Partition(i)), new Message<Null, string>
                //                                                                                 {
                //                                                                                         Value = msg
                //                                                                                 });
                //}
                producer.Flush(config.TimeOut);

                
            }
        }

        #endregion
    }
}
