using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo1.DTO
{
    public static class DictionaryData
    {
        public static Dictionary<string, int> GetDictionary()
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            // vùng 1
            dictionary.Add("Lai Châu", 1);
            dictionary.Add("Điện Biên", 1);
            dictionary.Add("Sơn La", 1);
            dictionary.Add("Lào Cai", 1);
            dictionary.Add("Yên Bái", 1);
            dictionary.Add("Hà Giang", 1);
            dictionary.Add("Tuyên Quang", 1);
            dictionary.Add("Cao Bằng", 1);
            dictionary.Add("Bắc Kạn", 1);
            dictionary.Add("Phú Thọ", 1);
            dictionary.Add("Thái Nguyên", 1);
            dictionary.Add("Lạng Sơn", 1);
            dictionary.Add("Bắc Giang", 1);
            dictionary.Add("Quảng Ninh", 1);
            dictionary.Add("Hòa Bình", 1);

            // vùng 2
            dictionary.Add("Vĩnh Phúc", 2);
            dictionary.Add("Bắc Ninh", 2);
            dictionary.Add("Hải Dương", 2);
            dictionary.Add("Hưng Yên", 2);
            dictionary.Add("Hà Nam", 2);
            dictionary.Add("Thái Bình", 2);
            dictionary.Add("Nam Định", 2);
            dictionary.Add("Ninh Bình", 2);
            dictionary.Add("Hà Nội", 2);
            dictionary.Add("Hải Phòng", 2);

            // vùng 3
            dictionary.Add("Thanh Hóa", 3);
            dictionary.Add("Nghệ An", 3);
            dictionary.Add("Hà Tĩnh", 3);
            dictionary.Add("Quảng Bình", 3);
            dictionary.Add("Quảng Trị", 3);
            dictionary.Add("Thừa Thiên Huế", 3);

            // vùng 4
            dictionary.Add("Đà Nẵng", 4);
            dictionary.Add("Quảng Nam", 4);
            dictionary.Add("Quảng Ngãi", 4);
            dictionary.Add("Bình Định", 4);
            dictionary.Add("Phú Yên", 4);
            dictionary.Add("Khánh Hòa", 4);

            // vùng 5
            dictionary.Add("Kom Tum", 5);
            dictionary.Add("Gia Lai", 5);
            dictionary.Add("Đắk Lắk", 5);
            dictionary.Add("Đắk Nông", 5);
            dictionary.Add("Lâm Đồng", 5);

            // vùng 6
            dictionary.Add("Ninh Thuận", 6);
            dictionary.Add("Bình Thuận", 6);
            dictionary.Add("Đồng Nai", 6);
            dictionary.Add("Bình Phước", 6);
            dictionary.Add("Tây Ninh", 6);
            dictionary.Add("Bình Dương", 6);
            dictionary.Add("Vũng Tàu", 6);
            dictionary.Add("Hồ Chí Minh", 6);
            dictionary.Add("TP.HCM", 6);

            //vùng 7
            dictionary.Add("Long An", 7);
            dictionary.Add("Đồng Tháp", 7);
            dictionary.Add("Tiền Giang", 7);
            dictionary.Add("Bến Tre", 7);
            dictionary.Add("Vĩnh Long", 7);
            dictionary.Add("Trà Vinh", 7);
            dictionary.Add("Sóc Trăng", 7);
            dictionary.Add("Cần Thơ", 7);
            dictionary.Add("Hậu Giang", 7);
            dictionary.Add("An Giang", 7);
            dictionary.Add("Kiên Giang", 7);
            dictionary.Add("Bạc Liêu", 7);
            dictionary.Add("Cà Mau", 7);

            return dictionary;
        }


        public static ObservableCollection<string> GetCities()
        {
            ObservableCollection<string> cities = new ObservableCollection<string>
            {
                "An Giang", "Vũng Tàu", "Bạc Liêu", "Bắc Kạn", "Bắc Giang", "Bắc Ninh", "Bến Tre", "Bình Dương", "Bình Định", "Bình Phước", "Cà Mau", "Cao Bằng", "Cần Thơ",
                "Đà Nẵng", "Đắk Lắk", "Đắk Nông", "Điện Biên", "Đồng Nai", "Đồng Tháp", "Gia Lai", "Hà Giang", "Hà Nam", "Hà Nội", "TP.HCM", "Hậu Giang", "Hưng Yên",
                "Khánh Hoà", "Kiên Giang", "Kon Tum", "Lai Châu", "Lào Cai", "Lạng Sơn", "Lâm Đồng", "Long An", "Nam Định", "Nghệ An", "Ninh Bình",
                "Ninh Thuận", "Phú Thọ", "Phú Yên", "Quảng Bình", "Quảng Nam", "Quảng Ngãi", "Quảng Ninh", "Quảng Trị", "Sóc Trăng", "Sơn La", "Tây Ninh",
                "Thái Bình", "Thanh Hoá", "Thừa Thiên Huế", "Tiền Giang", "Trà Vinh", "Tuyên Quang", "Vĩnh Long", "Vĩnh Phúc", "Yên Bái"
            };

            return cities;
        }
        public static Dictionary<string, string> ProvinceRegions = new Dictionary<string, string>
        {
                // Các tỉnh thành tương ứng ở miền Bắc
                {"Hà Giang","Bắc"},
                {"Bắc Cạn","Bắc"},
                {"Cao Bằng","Bắc"},
                {"Tuyên Quang","Bắc"},
                {"Lạng Sơn","Bắc"},
                {"Thái Nguyên","Bắc"},
                {"Bắc Giang","Bắc"},
                {"Phú Thọ","Bắc"},
                {"Quảng Ninh","Bắc"},
                {"Lào Cai","Bắc"},
                {"Điện Biên","Bắc"},
                {"Yên Bái","Bắc"},
                {"Lai Châu","Bắc"},
                {"Hòa Bình","Bắc"},
                {"Sơn La","Bắc"},
                {"Hà Nội", "Bắc" },
                {"Bắc Ninh","Bắc"},
                {"Hải Dương","Bắc"},
                {"Hà Nam","Bắc"},
                {"Hải Phòng", "Bắc" },
                {"Nam Định", "Bắc" },
                {"Hưng Yên", "Bắc" },
                {"Thái Bình", "Bắc" },
                {"Ninh Bình", "Bắc" },
                {"Vĩnh Phúc", "Bắc" },

                // Các tỉnh thành tương ứng ở miền Trung
                { "Nghệ An", "Trung" },
                { "Thanh Hóa", "Trung" },
                { "Hà Tĩnh", "Trung" },
                { "Quảng Trị", "Trung" },
                { "Quảng Bình", "Trung" },
                { "Thừa Thiên – Huế", "Trung" },
                { "Khánh Hoà", "Trung" },
                { "Bình Thuận", "Trung" },
                { "Ninh Thuận", "Trung" },
                { "Quảng Nam", "Trung" },
                { "Đà Nẵng", "Trung" },
                { "Quảng Ngãi", "Trung" },
                { "Bình Định", "Trung" },
                { "Phú Yên ", "Trung" },
                { "Kon Tum", "Trung" },
                { "Gia Lai", "Trung" },
                { "Đắk Nông", "Trung" },
                { "Đắk Lắk", "Trung" },
                { "Lâm Đồng", "Trung" },

                // Các tỉnh thành tương ứng ở miền Nam
                { "Bình Dương", "Nam" },
                { "Bình Phước", "Nam" },
                { "Tây Ninh", "Nam" },
                { "Đồng Nai", "Nam" },
                { "Bà Rịa - Vũng Tàu", "Nam" },
                { "TP.HCM", "Nam" },
                { "Long An", "Nam" },
                { "Đồng Tháp", "Nam" },
                { "An Giang", "Nam" },
                { "Tiền Giang", "Nam" },
                { "Bến Tre", "Nam" },
                { "Trà Vinh", "Nam" },
                { "Vĩnh Long", "Nam" },
                { "Kiên Giang", "Nam" },
                { "Hậu Giang", "Nam" },
                { "Bạc Liêu", "Nam" },
                { "Sóc Trăng", "Nam" },
                { "Cà Mau", "Nam" },
                { "Cần Thơ", "Nam" },

            };
        }
}
