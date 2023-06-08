using Demo1.DTO;
using Demo1.Model;
using Demo1.View;
using Demo1.ViewModel;
using LiveCharts;
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

        public ICommand SearchCommand { get; set; }

        private string _WarehouseText;
        public string WarehouseText
        {
            get { return _WarehouseText; }
            set { _WarehouseText = value; OnPropertyChanged(nameof(WarehouseText)); }
        }
        public ICommand NorthernWarehousesCollectionCommand { get; set; }
        public ICommand CentralWarehousesCollectionCommand { get; set; }
        public ICommand SouthernWarehousesCollectionCommand { get; set; }

        public ICommand AllWarehousesCollectionCommand { get; set; }
        Dictionary<string, string> provinceRegions = DictionaryData.ProvinceRegions;

     
        public class WarehouseInfo
        {
            public string WarehouseID { get; set; }
            public string WarehouseName { get; set; }
            public int Capacity { get; set; }
            public ChartValues<double> ColumnData { get; set; }


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
            SCustomerName = UserInfo.ParcelInfo.Instance.GetCustomerName(WarehouseText, 1);
            RCustomerName = UserInfo.ParcelInfo.Instance.GetCustomerName(WarehouseText, 2);
            SCustomerAddress = UserInfo.ParcelInfo.Instance.GetCustomerAddress(WarehouseText, 1);
            RCustomerAddress = UserInfo.ParcelInfo.Instance.GetCustomerAddress(WarehouseText, 2);
            SCustomerPhoneNumber = UserInfo.ParcelInfo.Instance.GetCustomerPhoneNumber(WarehouseText, 1);
            RCustomerPhoneNumber = UserInfo.ParcelInfo.Instance.GetCustomerPhoneNumber(WarehouseText, 2);
            ShippingFee = Convert.ToString(UserInfo.ParcelInfo.Instance.GetParcelTotalCost(WarehouseText));
            Cost = Convert.ToString(Convert.ToDouble(ShippingFee) - Convert.ToDouble(ParcelValue));
            if (UserInfo.ParcelInfo.Instance.GetShippingMethod(WarehouseText)) ShippingMethod = "Giao hàng nhanh";
            else ShippingMethod = "Giao hàng chậm";
            CreateTime = UserInfo.ParcelInfo.Instance.GetCreateTime(WarehouseText);
            Details = UserInfo.ParcelInfo.Instance.GetDetails(WarehouseText);

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
            NorthernWarehousesCollectionCommand = new RelayCommand<object>((p) => { return true; }, (p) => GetWarehouseFiltered("Bắc"));
            CentralWarehousesCollectionCommand = new RelayCommand<object>((p) => { return true; }, (p) => GetWarehouseFiltered("Trung"));
            SouthernWarehousesCollectionCommand = new RelayCommand<object>((p) => { return true; }, (p) =>GetWarehouseFiltered("Nam"));
            AllWarehousesCollectionCommand = new RelayCommand<object>((p) => { return true; }, (p) => GetWarehouseFiltered("All"));
            SearchCommand = new RelayCommand<object>((p) => { return true; },
                (p) => { GetWarehouseSearched(WarehouseText);});
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

                ParcelInfoList.Clear();

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
                        Capacity = w.capacity,
                        ColumnData = SetColumnData(w.warehouseID)
            }).ToList();

                if (result.Any())
                {
                    foreach (var i in result)
                    {
                        WarehouseInfoList.Add(i);
                    }
                }
                else
                {
                    MessageBoxWindow.Show("Không tìm thấy thông tin kho");
                }
            }
        }
        public ChartValues<double> SetColumnData(string WHID)
        {
            ChartValues<double> columnData = new ChartValues<double>();

            using (var context = new PBL3_demoEntities())
            {
                var warehouse = context.Warehouses.FirstOrDefault(w => w.warehouseID == WHID);
                if (warehouse != null)
                {
                    int capacity = warehouse.capacity;
                    var parcels = context.Parcels.Where(p => p.currentWarehouseID == WHID).ToList();
                    int count = parcels.Count();
                    double usedCapacity = (double)count / capacity * 100;
                   
                    columnData.Add(usedCapacity/100);
                }
            }

            return columnData;
        }


        public void GetWarehouseFiltered(string str)
        {
            ObservableCollection<string> warehouseIDs = FindRegionOfWarehouses(str);

            using (var context = new Model.PBL3_demoEntities())
            {
                WarehouseInfoList.Clear(); // Clear the old elements in WarehouseInfoList

                foreach (string warehouseID in warehouseIDs)
                {
                    var query = context.Warehouses
                        .Where(w => w.warehouseID == warehouseID)
                        .Select(w => new WarehouseInfo
                        {
                            WarehouseID = w.warehouseID,
                            WarehouseName = w.warehouseName,
                            Capacity = w.capacity
                        })
                        .FirstOrDefault();

                    if (query != null)
                    {
                        query.ColumnData = SetColumnData(warehouseID);
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

        public string FindContained(string WHID)
        {
            double res = 0;
            int capacity = 0;
            int count = 0;
            //lay suc chua cua kho i
            using (var context1 = new PBL3_demoEntities())
            {
                var Capacity = context1.Warehouses.FirstOrDefault(x => x.warehouseID == WHID);
                if (Capacity != null)
                {
                    capacity = Capacity.capacity;
                }
            }
            //dem don dang trong kho i
            using (var context = new PBL3_demoEntities())
            {
                var Count = context.Parcels.Count(o => o.currentWarehouseID == WHID);
                count = Count;
            }
            res = (double)count / capacity;
            double roundedResult = Math.Round(res * 100, 3);
            return roundedResult.ToString() + "%";
        }


    }
}
