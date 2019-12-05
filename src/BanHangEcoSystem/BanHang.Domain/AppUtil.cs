using System;
using System.Collections.Generic;
using System.Text;

namespace BanHang.Domain
{
    public class AppUtil
    {
        private static readonly List<string> listNames = new List<string>
                                                         {
                                                                 "Bui Tien Dung",
                                                                 "Nguyen Cong Phuong",
                                                                 "Luong Xuan Truong",
                                                                 "Nguyen Tuan Anh",
                                                                 "Nguyen Van Toan",
                                                                 "Nguyen Anh Duc",
                                                                 "Nguyen Trong Hoang",
                                                                 "Tran Duy Manh",
                                                                 "Nguyen Van Hau",
                                                                 "Dinh Trong",
                                                                 "Hung Dung",
                                                                 "Duc Huy",
                                                                 "Tran Dai Viet"
                                                         };

        private static readonly List<string> listAddresses = new List<string>
                                                             {
                                                                     "Quan 1",
                                                                     "Quan 2",
                                                                     "Quan 3",
                                                                     "Quan 4",
                                                                     "Quan 5",
                                                                     "Quan 6",
                                                                     "Quan 7",
                                                                     "Quan 8",
                                                                     "Quan 9",
                                                                     "Quan 10",
                                                                     "Quan 11",
                                                                     "Quan 12",
                                                                     "Quan Binh Thanh",
                                                                     "Quan Tan Binh",
                                                                     "Quan Binh Tan",
                                                                     "Quan Thu Duc",
                                                                     "Quan Go Vap",
                                                                     "Quan Phu Nhuan",
                                                                     "Quan Tan Phu",
                                                                     "Huyen Nha Be",
                                                                     "Huyen Binh Chanh",
                                                                     "Huyen Cu Chi",
                                                                     "Huyen Hoc Mon",
                                                                     "Huyen Can Gio"
                                                             };

        private static readonly string listNumber = "0123456789";

        private static readonly List<string> listProducts = new List<string>
                                                            {
                                                                    "TV",
                                                                    "Tu lanh",
                                                                    "May giat",
                                                                    "Smartphone",
                                                                    "Dieu hoa",
                                                                    "Bep tu",
                                                                    "Dong ho",
                                                                    "Amply",
                                                                    "Ban ghe"
                                                            };

        private static string GetRandomFromList(List<string> list)
        {
            var r = new Random();
            return list[r.Next(0, list.Count - 1)];
        }

        public static string GetRandomName()
        {
            return GetRandomFromList(listNames);
        }

        public static string GetRandomAddress()
        {
            return GetRandomFromList(listAddresses);
        }

        public static string GetRandomPhoneNumber()
        {
            StringBuilder builder = new StringBuilder();
            Random r = new Random();
            for (int i = 0; i < 8; i++)
            {
                builder.Append(listNumber[r.Next(0, 9)]);
            }

            return "09" + builder;
        }

        public static string GetRandomProduct()
        {
            return GetRandomFromList(listProducts);
        }

        public static int GetRandomNumber()
        {
            var r = new Random();
            return r.Next(1, 10);
        }
    }
}
