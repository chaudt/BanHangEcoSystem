using System;
using System.Text;
using Config.Server;
using Config.Server.Kafka.Configs;
using Config.Server.Kafka.Helper;
using Confluent.Kafka;
using Dapper;
using LapDat.Domain.Core.EventSourcing;
using LapDat.Domain.Core.Interfaces;
using Newtonsoft.Json;

namespace LapDat.Domain.Core.Services
{
    public class LapDatServices : ILapDatServices
    {
        private readonly IConnectedDatabase _connection;

        private readonly ILapDatProducer _producer;

        public LapDatServices(IConnectedDatabase connection, ILapDatProducer producer)
        {
            _connection = connection;
            _producer = producer;
        }

        #region ILapDatServices Members

        public void Subscribe(Action<string> action)
        {
            var config = KafkaConfigManagement.Instance;
            using (var consumer = new ConsumerBuilder<Ignore, string>(config.GetConsumerConfig()).Build())
            {
                consumer.Subscribe(config.GetTopic1);
                try
                {
                    while (true)
                    {
                        try
                        {
                            var cr = consumer.Consume();
                           //action($"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");
                            var msg_status = JsonConvert.DeserializeObject<Message_Customer>(cr.Value);
                            if (msg_status.Status == Status.Da_Giao_Hang)
                            {
                                LapDat(new DTO.LapDat
                                       {
                                               DiaChiGiaoHang = msg_status.CustomerAddress,
                                               NgayLapDat = DateTime.Now,
                                               SoDienThoai = msg_status.PhoneNumber,
                                               TenKhachHang = msg_status.CustomerName,
                                               ThongTinKhachHangId = msg_status.Id
                                       });

                                action($"Thong tin khach hang {msg_status.Id} => cap nhat trang thai {Status.Da_Lap_Dat}.");
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
                //                          if(msg_status.Status == Status.Da_Giao_Hang)
                //                          {
                //                              LapDat(new DTO.LapDat
                //                                     {
                //                                             DiaChiGiaoHang = msg_status.CustomerAddress,
                //                                             NgayLapDat = DateTime.Now,
                //                                             SoDienThoai = msg_status.PhoneNumber,
                //                                             TenKhachHang = msg_status.CustomerName,
                //                                             ThongTinKhachHangId = msg_status.Id
                //                                     });

                //                              action($"Thong tin khach hang {msg_status.Id} => cap nhat trang thai {Status.Da_Lap_Dat}.");
                //                          }
                //                      };

                //while (true)
                //{
                //    consumer.Poll(config.TimeOut);
                //}
            }
        }

        #endregion

        private void LapDat(DTO.LapDat lapDat)
        {
            Call(lapDat);

            using (var db = _connection.GetConnection())
            {
                db.Execute("INSERT INTO setting.LapDat(ThongTinKhachHangId,SoDienThoai,TenKhachHang,DiaChiGiaoHang,NgayLapDat) VALUES(@ThongTinKhachHangId,@SoDienThoai,@TenKhachHang,@DiaChiGiaoHang,@NgayLapDat);", new
                                                                                                                                                                                                                      {
                                                                                                                                                                                                                              lapDat.ThongTinKhachHangId,
                                                                                                                                                                                                                              lapDat.SoDienThoai,
                                                                                                                                                                                                                              lapDat.TenKhachHang,
                                                                                                                                                                                                                              lapDat.DiaChiGiaoHang,
                                                                                                                                                                                                                              NgayLapDat = DateTime.Now
                                                                                                                                                                                                                      });
            }

            //producer publish message
            _producer.Publish(new Message_Customer
                              {
                                      Id = lapDat.ThongTinKhachHangId,
                                      CustomerAddress = lapDat.DiaChiGiaoHang,
                                      CustomerName = lapDat.TenKhachHang,
                                      PhoneNumber = lapDat.SoDienThoai,
                                      Status = Status.Da_Lap_Dat
                              });
        }

        private void Call(DTO.LapDat lapDat)
        {
            Console.WriteLine($"Call to {lapDat.SoDienThoai}");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(" ABS-Services:");
            Console.WriteLine($"        Alo, xin chao (anh/chi) {lapDat.TenKhachHang}");
            Console.WriteLine($"        Chung toi, co the den lap dat tai dia chi {lapDat.DiaChiGiaoHang} duoc khong?");

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($" {lapDat.TenKhachHang}:");
            Console.WriteLine("         Duoc em!");
            Console.ResetColor();
        }
    }
}
