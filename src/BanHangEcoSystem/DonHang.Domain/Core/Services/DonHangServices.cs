using System;
using Config.Server;
using Config.Server.Kafka.Configs;
using Config.Server.Kafka.Helper;
using Confluent.Kafka;
using Dapper;
using DonHang.Domain.Core.Interfaces;
using Newtonsoft.Json;

namespace DonHang.Domain.Core.Services
{
    public class DonHangServices : IDonHangServices
    {
        private readonly IConnectedDatabase _connection;

        public DonHangServices(IConnectedDatabase connection)
        {
            _connection = connection;
        }

        #region IDonHangServices Members

        public void Subscribe(Action<string> action)
        {
            var config = KafkaConfigManagement.Instance;
            using (var consumer = new ConsumerBuilder<Ignore, string>(config.GetConsumerConfig()).Build())
            {
                consumer.Subscribe(config.GetTopic3);
                try
                {
                    while (true)
                    {
                        try
                        {
                            var cr = consumer.Consume();
                            //Console.WriteLine($"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");
                            var msg_status = JsonConvert.DeserializeObject<Message_Customer>(cr.Value);
                            CapNhat_TrangThai(msg_status.Id, msg_status.Status.ToString());

                            action($"Thong tin khach hang {msg_status.Id} => cap nhat trang thai {msg_status.Status}.");
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occured: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // Ensure the consumer leaves the group cleanly and final offsets are committed.
                    consumer.Close();
                }
                //consumer.OnMessage += (_, e) =>
                //{
                //    var msg_status = Newtonsoft.Json.JsonConvert.DeserializeObject<Message_Customer>(e.Value);
                //    CapNhat_TrangThai(msg_status.Id, msg_status.Status.ToString());

                //    action($"Thong tin khach hang {msg_status.Id} => cap nhat trang thai {msg_status.Status}.");
                //};

                //while (true)
                //{
                //    consumer.Poll(config.TimeOut);
                //}
            }
        }

        #endregion

        private void CapNhat_TrangThai(int id, string trangthai)
        {
            using (var db = _connection.GetConnection())
            {
                //db.Get()
                db.Execute("UPDATE info.ThongTinKhachHang SET TrangThai=@TrangThai WHERE Id=@Id;", new
                                                                                                   {
                                                                                                           TrangThai = trangthai,
                                                                                                           Id = id
                                                                                                   });
            }
        }
    }
}
