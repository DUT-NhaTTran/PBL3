using Demo1.DTO;
using Demo1.Model;
using Demo1.ViewModel;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Demo1.ViewModel
{
    public class WarehouseTrackingModel : PropertiesCollection
    {
        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                if (searchText != value)
                {
                    searchText = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _WarehouseName;
        public string WarehouseName
        {
            get { return _WarehouseName; }
            set
            {
                if (_WarehouseName != value)
                {
                    _WarehouseName = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _WarehouseID;
        //public string WarehouseID
        //{
        //    get { return _WarehouseID; }
        //    set
        //    {
        //        if (_WarehouseID != value)
        //        {
        //            _WarehouseID = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}
        //private string _All;
        //public string All
        //{
        //    get { return _All; }
        //    set
        //    {
        //        _All = value;
        //        OnPropertyChanged();
        //    }
        //}
        //private string _South;
        //public string South
        //{
        //    get { return _South; }
        //    set
        //    {
        //        _South = value;
        //        OnPropertyChanged();
        //    }
        //}
        //private string _Center;
        //public string Center
        //{
        //    get { return _Center; }
        //    set
        //    {
        //        _Center = value;
        //        OnPropertyChanged();
        //    }
        //}
        //private string _North;
        //public string North
        //{
        //    get { return _North; }
        //    set
        //    {
        //        _North = value;
        //        OnPropertyChanged();
        //    }
        //}
        //private string _WHAddress;
        //public string WHAddress
        //{
        //    get { return _WHAddress; }
        //    set
        //    {
        //        _WHAddress = value;
        //        OnPropertyChanged();
        //    }
        //}
        //private string _WHCapacity;
        //public string WHCapacity
        //{
        //    get { return _WHCapacity; }
        //    set
        //    {
        //        _WHCapacity = value;
        //        OnPropertyChanged();
        //    }
        //}
        //private string _WHContained;
        //public string WHContained
        //{
        //    get { return _WHContained; }
        //    set
        //    {
        //        _WHContained = value;
        //        OnPropertyChanged();
        //    }
        //}


        public ICommand SearchCommand { get; set; }
        public ICommand StatisticOneWarehouseCommand { get; set; }

        public ICommand StatisticAllWarehouseCommand { get; set; }

        public ICommand NorthernWarehousesCollectionCommand { get; set; }
        public ICommand CentralWarehousesCollectionCommand { get; set; }
        public ICommand SouthernWarehousesCollectionCommand { get; set; }

        public ICommand AllWarehousesCollectionCommand { get; set; }

        Dictionary<string, string> provinceRegions = new Dictionary<string, string>
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
        public class WarehouseInfo
        {
            public string WarehouseID { get; set; }
            public string WarehouseName { get; set; }
            public int Capacity { get; set; }
        }
        private ObservableCollection<WarehouseInfo> warehouseInfoList;
        public ObservableCollection<WarehouseInfo> WarehouseInfoList
        {
            get { return warehouseInfoList; }
            set
            {
                warehouseInfoList = value;
                OnPropertyChanged(nameof(WarehouseInfoList));
            }
        }
        
        private ObservableCollection<ParcelInfo> _parcelInfoList;
        public ObservableCollection<ParcelInfo> ParcelInfoList
        {
            get { return _parcelInfoList; }
            set
            {
                _parcelInfoList = value;
                OnPropertyChanged();
            }
        }
        public void SetAllParcelInfo()
        {
            SCustomerName = UserInfo.ParcelInfo.Instance.GetCustomerName(SearchText, 1);
            RCustomerName = UserInfo.ParcelInfo.Instance.GetCustomerName(SearchText, 2);
            SCustomerAddress = UserInfo.ParcelInfo.Instance.GetCustomerAddress(SearchText, 1);
            RCustomerAddress = UserInfo.ParcelInfo.Instance.GetCustomerAddress(SearchText, 2);
            SCustomerPhoneNumber = UserInfo.ParcelInfo.Instance.GetCustomerPhoneNumber(SearchText, 1);
            RCustomerPhoneNumber = UserInfo.ParcelInfo.Instance.GetCustomerPhoneNumber(SearchText, 2);
            ShippingFee = Convert.ToString(UserInfo.ParcelInfo.Instance.GetParcelTotalCost(SearchText));
            Cost = Convert.ToString(Convert.ToDouble(ShippingFee) - Convert.ToDouble(ParcelValue));
            if (UserInfo.ParcelInfo.Instance.GetShippingMethod(SearchText)) ShippingMethod = "Giao hàng nhanh";
            else ShippingMethod = "Giao hàng chậm";
            CreateTime = UserInfo.ParcelInfo.Instance.GetCreateTime(SearchText);
            Details = UserInfo.ParcelInfo.Instance.GetDetails(SearchText);

        }
        public class ParcelInfo
        {
            public int ID { get; set; }
            public string ParcelName { get; set; }
            public double Mass { get; set; }
        }
      

        public WarehouseTrackingModel()
        {
            ParcelInfoList = new ObservableCollection<ParcelInfo>();
            WarehouseInfoList = new ObservableCollection<WarehouseInfo>();
            NorthernWarehousesCollectionCommand = new RelayCommand<object>((p) => { return true; }, (p) => GetWarehouseFilterd("Bắc"));
            CentralWarehousesCollectionCommand = new RelayCommand<object>((p) => { return true; }, (p) => GetWarehouseFilterd("Trung"));
            SouthernWarehousesCollectionCommand = new RelayCommand<object>((p) => { return true; }, (p) =>GetWarehouseFilterd("Nam"));
            AllWarehousesCollectionCommand = new RelayCommand<object>((p) => { return true; }, (p) => GetWarehouseFilterd("All"));
            SearchCommand = new RelayCommand<object>((p) => { return true; },
                (p) => { GetWarehouseSearched(SearchText);});
            All = SetNumberOfWarehouseFiterd("All");
            South = SetNumberOfWarehouseFiterd("Nam");
            Center = SetNumberOfWarehouseFiterd("Trung");
            North = SetNumberOfWarehouseFiterd("Bắc");
            


        }
        public void LoadParcelInfoList(string WHID)
        {
            using (var dbContext = new Model.PBL3_demoEntities())
            {
                var parcels = dbContext.Parcels
                    .Where(p => p.currentWarehouseID == WHID)
                    .Select(p => new ParcelInfo
                    {
                        ParcelName = p.parcelName,
                        ID = p.parcelID,
                        Mass = p.parcelMass
                    })
                    .ToList();

                
                foreach (var parcel in parcels)
                {
                    ParcelInfoList.Add(parcel);
                }
            }
        }
        string SetNumberOfWarehouseFiterd(string str)
        {
            ObservableCollection<string> warehouseIDs = FindRegionOfWarehouses(str);
            int count = 0;

            using (var context = new Model.PBL3_demoEntities())
            {
                foreach (string warehouseID in warehouseIDs)
                {
                    var query = context.Warehouses
                        .Where(w => w.warehouseID == warehouseID)
                        .Count();

                    count += query;
                    //giai thich nghe
                }
            }

            return count.ToString();
        }
        public void GetWarehouseSearched(string str)
        {
            using (var context = new Model.PBL3_demoEntities())
            {
                // Xóa kết quả tìm kiếm cũ
                WarehouseInfoList.Clear();

                var result = context.Warehouses
                    .Where(w => w.warehouseName.Contains(str) || w.warehouseID.Contains(str))
                    .Select(w => new WarehouseInfo
                    {
                        WarehouseID = w.warehouseID,
                        WarehouseName = w.warehouseName,
                        Capacity = w.capacity
                    })
                    .ToList();

                if (result.Any())
                {
                    foreach (var i in result)
                    {
                        WarehouseInfoList.Add(i);
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin kho");
                }
            }
        }
        public void GetWarehouseFilterd(string str)
        {
            ObservableCollection<string> warehouseIDs = FindRegionOfWarehouses(str);

            using (var context = new Model.PBL3_demoEntities())
            {
                WarehouseInfoList.Clear(); // Xóa đi các phần tử cũ trong WarehouseInfoList

                foreach (string warehouseID in warehouseIDs)
                {
                    var query = context.Warehouses
                        .Where(w => w.warehouseID == warehouseID) // Sử dụng toán tử so sánh bằng để tìm kiếm warehouseID chính xác
                        .Select(w => new WarehouseInfo
                        {
                            WarehouseID = w.warehouseID,
                            WarehouseName = w.warehouseName,
                            Capacity = w.capacity
                        })
                        .FirstOrDefault(); // Sử dụng FirstOrDefault để chỉ lấy một kết quả đầu tiên (nếu có)

                    if (query != null)
                    {
                        WarehouseInfoList.Add(query);
                    }
                }
            }
        }
        string GetProvinceFromWarehouseName(string warehouseName)
        {
            string provinceName = warehouseName.Replace("Kho ", "");
            return provinceName;
        }


        ObservableCollection<string> FindRegionOfWarehouses(string region)
        {
            ObservableCollection<string> warehousesOfThisRegionCollection = new ObservableCollection<string>();
            using (var context = new PBL3_demoEntities())
            {
                List<string> warehouseAddresses = context.Warehouses.Select(x => x.warehouseName).ToList();
                List<string> warehousesInThisRegion = new List<string>();
                if (region != "All")
                {
                    warehousesInThisRegion = warehouseAddresses.Where(address =>
                    {
                        string province = GetProvinceFromWarehouseName(address);
                        return provinceRegions.ContainsKey(province) && provinceRegions[province] == region;
                    }).ToList();
                }
                else
                {
                    warehousesInThisRegion = context.Warehouses.Select(x => x.warehouseName).ToList();
                }
                foreach (string warehouse in warehousesInThisRegion)
                {

                    warehousesOfThisRegionCollection.Add(context.Warehouses.Where(x => x.warehouseName == warehouse).Select(x => x.warehouseID).FirstOrDefault());
                }
            }
            return warehousesOfThisRegionCollection;
        }
        //viewmodel.WarehouseCapacity=Get(warehouseID)
        public string FindContained(string WHID)
        {
            double res = 0;
            int capacity = 0;
            int count = 0;
            //lay suc chua cua kho i
            using (var context1 = new PBL3_demoEntities())
            {
                var Capacity = context1.Warehouses.FirstOrDefault(x => x.warehouseID == WHID);
                if(Capacity != null) 
                {
                    capacity = Capacity.capacity;
                }
            }
            //dem don dang trong kho i
            using (var context = new PBL3_demoEntities())
            {
                var Count= context.Parcels.Count(o => o.currentWarehouseID == WHID);
                count = Count;
            }
            res = (double)count / capacity;
            return Convert.ToString(res) + "%";
        }
      
    }
}
