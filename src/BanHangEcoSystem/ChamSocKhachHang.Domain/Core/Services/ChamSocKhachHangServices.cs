using System;
using System.Text;
using System.Threading;
using ChamSocKhachHang.Domain.Core.EventSourcing;
using ChamSocKhachHang.Domain.Core.Interfaces;
using Config.Server;
using Config.Server.Kafka.Configs;
using Config.Server.Kafka.Helper;
using Confluent.Kafka;
using Dapper;
using Newtonsoft.Json;

namespace ChamSocKhachHang.Domain.Core.Services
{
    public class ChamSocKhachHangServices : IChamSocKhachHangServices
    {
        private readonly IConnectedDatabase _connection;

        private readonly IChamSocKhachHangProducer _producer;

        public ChamSocKhachHangServices(IConnectedDatabase connection, IChamSocKhachHangProducer producer)
        {
            _connection = connection;
            _producer = producer;
        }

        #region IChamSocKhachHangServices Members

        public void Subscribe(Action<string> action)
        {
            var config = KafkaConfigManagement.Instance;
            using (var consumer = new ConsumerBuilder<Ignore, string>(config.GetConsumerConfig()).Build())
            {
                consumer.Subscribe(config.GetTopic2);

                try
                {
                    while (true)
                    {
                        try
                        {
                            var cr = consumer.Consume();
                            //Console.WriteLine($"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");

                            var msg_status = JsonConvert.DeserializeObject<Message_Customer>(cr.Value);
                            if (msg_status.Status == Status.Da_Lap_Dat)
                            {
                                ChamSoc(new DTO.ChamSocKhachHang
                                        {
                                                DiaChi = msg_status.CustomerAddress,
                                                NgayChamSoc = DateTime.Now,
                                                SoDienThoai = msg_status.PhoneNumber,
                                                TenKhachHang = msg_status.CustomerName,
                                                ThongTinKhachHangId = msg_status.Id
                                        });

                                action($"Thong tin khach hang {msg_status.Id} => cap nhat trang thai {Status.Da_CSKH}.");
                            }
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
                //                      {
                //                          var msg_status = JsonConvert.DeserializeObject<Message_Customer>(e.Value);
                //                          if(msg_status.Status == Status.Da_Lap_Dat)
                //                          {
                //                              ChamSoc(new DTO.ChamSocKhachHang
                //                                      {
                //                                              DiaChi = msg_status.CustomerAddress,
                //                                              NgayChamSoc = DateTime.Now,
                //                                              SoDienThoai = msg_status.PhoneNumber,
                //                                              TenKhachHang = msg_status.CustomerName,
                //                                              ThongTinKhachHangId = msg_status.Id
                //                                      });

                //                              action($"Thong tin khach hang {msg_status.Id} => cap nhat trang thai {Status.Da_CSKH}.");
                //                          }
                //                      };

                //while (true)
                //{
                //    consumer.Poll(config.TimeOut);
                //}
            }
        }

        #endregion

        private void ChamSoc(DTO.ChamSocKhachHang chamSocKhachHang)
        {
            Call(chamSocKhachHang);
            using (var db = _connection.GetConnection())
            {
                db.Execute("INSERT INTO cs.ChamSocKhachHang(ThongTinKhachHangId,SoDienThoai,TenKhachHang,DiaChi,NgayChamSoc) VALUES(@ThongTinKhachHangId,@SoDienThoai,@TenKhachHang,@DiaChi,@NgayChamSoc);", new
                                                                                                                                                                                                             {
                                                                                                                                                                                                                     chamSocKhachHang.ThongTinKhachHangId,
                                                                                                                                                                                                                     chamSocKhachHang.SoDienThoai,
                                                                                                                                                                                                                     chamSocKhachHang.TenKhachHang,
                                                                                                                                                                                                                     chamSocKhachHang.DiaChi,
                                                                                                                                                                                                                     chamSocKhachHang.NgayChamSoc
                                                                                                                                                                                                             });
            }

            //producer publish message
            _producer.Publish(new Message_Customer
                              {
                                      Id = chamSocKhachHang.ThongTinKhachHangId,
                                      CustomerAddress = chamSocKhachHang.DiaChi,
                                      CustomerName = chamSocKhachHang.TenKhachHang,
                                      PhoneNumber = chamSocKhachHang.SoDienThoai,
                                      Status = Status.Da_CSKH
                              });
        }

        private void Call(DTO.ChamSocKhachHang chamSocKhachHang)
        {
            Console.WriteLine($"Call to {chamSocKhachHang.SoDienThoai}");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(" Customer-Services:");
            Console.WriteLine($"        Alo, xin chao (anh/chi) {chamSocKhachHang.TenKhachHang}");
            Console.WriteLine($"        Chung toi, co the giao hang den dia chi {chamSocKhachHang.DiaChi} duoc khong?");

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($" {chamSocKhachHang.TenKhachHang}:");
            Console.WriteLine("         Duoc em!");
            Console.ResetColor();
        }
    }
}
