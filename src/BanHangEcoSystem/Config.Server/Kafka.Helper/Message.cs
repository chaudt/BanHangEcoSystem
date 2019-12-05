namespace Config.Server.Kafka.Helper
{
    public class Message_Customer
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public Status Status { get; set; }
        //public string Note { get { return ReadTextDummy(); } }

        private string ReadTextDummy() {
            return System.IO.File.ReadAllText(@"Kafka.Helper\Dummy.txt");
        }
    }
  
    public enum Status
    {
        Da_Thanh_Toan = 0,
        Da_Giao_Hang,
        Da_Lap_Dat,
        Da_CSKH
    }
}
