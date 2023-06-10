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
            if (UserInfo.ParcelInfo.Instance.GetShippingMethod(WarehouseText)==1) ShippingMethod = "Giao hàng nhanh";
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
            WarehouseTrackingManager WHTM= new WarehouseTrackingManager();
            ParcelInfoList=WHTM.LoadParcelInfoList(WHID);   
        }
        public string SetNumberOfWarehouseFiterd(string str)
        {
            WarehouseTrackingManager WHTM = new WarehouseTrackingManager();
            return WHTM.SetNumberOfWarehouseFiterd(str);
        }
       
        public void GetWarehouseSearched(string str)
        {
            WarehouseTrackingManager WHTM = new WarehouseTrackingManager();
            WarehouseInfoList=WHTM.GetWarehouseSearched(str);
        }

        public ChartValues<double> SetColumnData(string WHID)
        {
            WarehouseTrackingManager WHTM = new WarehouseTrackingManager();
            return WHTM.SetColumnData(WHID);
        }


        public void GetWarehouseFiltered(string str)
        {
            WarehouseTrackingManager WHTM = new WarehouseTrackingManager();
            WarehouseInfoList=WHTM.GetWarehouseFiltered(str);

        }


        //string GetProvinceFromWarehouseName(string warehouseName)
        //{
        //    string provinceName = warehouseName.Replace("Kho ", "");
        //    return provinceName;
        //}


        //public ObservableCollection<string> FindRegionOfWarehouses(string region)
        //{
        //    WarehouseTrackingManager WHTM = new WarehouseTrackingManager();
        //    return WHTM.FindRegionOfWarehouses(region);
        //}

        public string FindContained(string WHID)
        {
            WarehouseTrackingManager WHTM = new WarehouseTrackingManager();
            return WHTM.FindContained(WHID);
        }


    }
}
