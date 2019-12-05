using System;
using Confluent.Kafka;

namespace Config.Server.Kafka.Configs
{
    public class KafkaConfigManagement
    {
        private static readonly Lazy<KafkaConfigManagement> _instanceLazy = new Lazy<KafkaConfigManagement>(() => new KafkaConfigManagement());

        public readonly string GetTopic = "THG-CMS2TTOSTopic";

        public readonly string GetTopic1 = "THG-TTOSPlanLocTopic";

        public readonly string GetTopic2 = "TM2-CMS2TTOSTopic";

        public readonly string GetTopic3 = "TM2-TTOSPlanLocTopic";

        public readonly string GetTopic4 = "topic_ban_hang_online4";

        public readonly string GetTopic5 = "topic_ban_hang_online5";

        public readonly TimeSpan TimeOut = TimeSpan.FromSeconds(5);

        public readonly string UrlServer = "113.166.120.51:9092";

        public static KafkaConfigManagement Instance
        {
            get { return _instanceLazy.Value; }
        }

        //public Dictionary<string, string> GetConfigConsumer()
        //{
        //    var config = new Dictionary<string, string>
        //    {
        //        { "group.id","banhang_chanel"},
        //        { "bootstrap.servers","localhost:9092"},
        //        { "enable.auto.commit","true"},
        //          { "auto.offset.reset", "largest"}
        //        //{ "partition.assignment.strategy", "roundrobin" },
        //        //{ "auto.commit.interval.ms", "1000"},
        //        //{ "session.timeout.ms", "30000"},
        //        //{ "auto.offset.reset", "smallest"}

        //    };

        //    return config;
        //}

        public ProducerConfig GetProducerConfig()
        {
            return new ProducerConfig
                   {
                           BootstrapServers = UrlServer
            };
        }

        public ProducerConfig GetProducerConfig(string bootstrapServers)
        {
            return new ProducerConfig
                   {
                           BootstrapServers = bootstrapServers
                   };
        }

        public ConsumerConfig GetConsumerConfig()
        {
            return new ConsumerConfig
                   {
                           BootstrapServers = UrlServer,
                           GroupId = "banhang_chanel",
                           //GroupId = Guid.NewGuid().ToString(),
                           EnableAutoCommit = true,
                           AutoCommitIntervalMs = 5000,
                           AutoOffsetReset = AutoOffsetReset.Earliest
                   };
        }

        public ConsumerConfig GetConsumerConfig(string bootstrapServers, string groupId, AutoOffsetReset autoOffset = AutoOffsetReset.Latest)
        {
            return new ConsumerConfig
                   {
                           BootstrapServers = bootstrapServers,
                           GroupId = groupId,
                           EnableAutoCommit = true,
                           AutoCommitIntervalMs = 5000,
                           AutoOffsetReset = autoOffset
                   };
        }
    }
}
