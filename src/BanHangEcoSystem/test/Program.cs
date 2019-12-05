using Config.Server.Kafka.Configs;
using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var message = new KafkaTransfer<IEnumerable<REQ>>
            {
                Data = new List<REQ> {
                    new REQ {
                        Action = "Place",
                        Status = "REQUEST",
                        Label = "LB",
                        WIId = "1",
                        
                    }
                },
                Links = new HATEOAS {
                    href = "asdasd",
                    rel = "xxccxc",
                    type = "POST"

                },
                Object = "WI",
                Sender ="OPSCore"
            };
            var msg = JsonConvert.SerializeObject(message);
            var config = KafkaConfigManagement.Instance;
            using (var producer = new ProducerBuilder<Null, string>(config.GetProducerConfig()).Build())
            {
                //producer.ProduceAsync(config.GetTopic3, null, msg);
                //producer.Flush(config.TimeOut);

                producer.ProduceAsync("CTL-WI", new Message<Null, string>
                {
                    Value = msg
                })
                        .ContinueWith(c =>
                        {
                        });
                producer.Flush(config.TimeOut);
            }
        }
    }
    public class HATEOAS
    {
        public string href { get; set; }
        public string rel { get; set; }
        public string type { get; set; }
    }
    public class KafkaTransfer<T>
    {
        public string Sender { get; set; }
        public string Object { get; set; }
        public T Data { get; set; }
        public HATEOAS Links { get; set; }
    }
    public class REQ
    {
        public REQ()
        {

        }
        public string WIId { get; set; }
        public string Status { get; set; }
        public string Action { get; set; }
        public string Label { get; set; }
    }
}
