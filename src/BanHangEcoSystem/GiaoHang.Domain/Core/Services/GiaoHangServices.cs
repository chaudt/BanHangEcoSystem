using System;
using Config.Server;
using Config.Server.Kafka.Configs;
using Config.Server.Kafka.Helper;
using Confluent.Kafka;
using Dapper;
using GiaoHang.Domain.Core.EventSourcing;
using GiaoHang.Domain.Core.Interfaces;
using Newtonsoft.Json;

namespace GiaoHang.Domain.Core.Services
{
    public class GiaoHangServices : IGiaoHangServices
    {
        private readonly IConnectedDatabase _connection;

        private readonly IGiaoHangProducer _producer;

        public GiaoHangServices(IConnectedDatabase connection, IGiaoHangProducer producer)
        {
            _connection = connection;
            _producer = producer;
        }

        #region IGiaoHangServices Members

        public void Subscribe(Action<string> action)
        {
            var config = KafkaConfigManagement.Instance;
            using (var consumer = new ConsumerBuilder<Ignore, string>(config.GetConsumerConfig()).Build())
            {
                consumer.Subscribe(config.GetTopic);
                try
                {
                    while (true)
                    {
                        try
                        {
                            var cr = consumer.Consume();
                            //action($"Consumed message {cr.Offset} '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");
                            var msg_status = JsonConvert.DeserializeObject<Message_Customer>(cr.Value);
                            if(msg_status.Status == Status.Da_Thanh_Toan)
                            {
                                GiaoHang(new DTO.GiaoHang
                                         {
                                                 DiaChiGiaoHang = msg_status.CustomerAddress,
                                                 NgayGiao = DateTime.Now,
                                                 SoDienThoai = msg_status.PhoneNumber,
                                                 TenKhachHang = msg_status.CustomerName,
                                                 ThongTinKhachHangId = msg_status.Id
                                         });

                                action($"Thong tin khach hang {msg_status.Id} => cap nhat trang thai {Status.Da_Giao_Hang}.");
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
                //{
                //    try
                //    {
                //        var msg_status = Newtonsoft.Json.JsonConvert.DeserializeObject<Message_Customer>(e.Value);
                //        if (msg_status.Status == Status.Da_Thanh_Toan)
                //        {
                //            GiaoHang(new DTO.GiaoHang
                //                     {
                //                             DiaChiGiaoHang = msg_status.CustomerAddress,
                //                             NgayGiao = DateTime.Now,
                //                             SoDienThoai = msg_status.PhoneNumber,
                //                             TenKhachHang = msg_status.CustomerName,
                //                             ThongTinKhachHangId = msg_status.Id
                //                     });

                //            action($"Thong tin khach hang {msg_status.Id} => cap nhat trang thai {Status.Da_Giao_Hang}.");
                //        }
                //    }
                //    catch (Exception exception)
                //    {
                //        action(exception.ToString());
                //    }

                //};

                //while (true)
                //{
                //    consumer.Poll(config.TimeOut);
                //}
            }
        }

        #endregion

        private void Call(DTO.GiaoHang giaoHang)
        {
            Console.WriteLine($"Call to {giaoHang.SoDienThoai}");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(" Grab-Services:");
            Console.WriteLine($"        Alo, xin chao (anh/chi) {giaoHang.TenKhachHang}");
            Console.WriteLine($"        Chung toi, co the giao hang den dia chi {giaoHang.DiaChiGiaoHang} duoc khong?");

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($" {giaoHang.TenKhachHang}:");
            Console.WriteLine("         Duoc em!");
            Console.ResetColor();
        }

        private void GiaoHang(DTO.GiaoHang giaoHang)
        {
            Call(giaoHang);

            using (var db = _connection.GetConnection())
            {
                db.Execute("INSERT INTO shipping.GiaoHang(ThongTinKhachHangId,SoDienThoai,TenKhachHang,DiaChiGiaoHang,NgayGiao) VALUES(@ThongTinKhachHangId,@SoDienThoai,@TenKhachHang,@DiaChiGiaoHang,@NgayGiao);", new
                                                                                                                                                                                                                     {
                                                                                                                                                                                                                             giaoHang.ThongTinKhachHangId,
                                                                                                                                                                                                                             giaoHang.SoDienThoai,
                                                                                                                                                                                                                             giaoHang.TenKhachHang,
                                                                                                                                                                                                                             giaoHang.DiaChiGiaoHang,
                                                                                                                                                                                                                             giaoHang.NgayGiao
                                                                                                                                                                                                                     });
            }

            // producer publish message
            _producer.Publish(new Message_Customer
                              {
                                      Id = giaoHang.ThongTinKhachHangId,
                                      CustomerAddress = giaoHang.DiaChiGiaoHang,
                                      CustomerName = giaoHang.TenKhachHang,
                                      PhoneNumber = giaoHang.SoDienThoai,
                                      Status = Status.Da_Giao_Hang
                              });
        }
    }
}
