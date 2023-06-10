using Demo1.Model;
using Demo1.View;
using System;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Demo1.ViewModel
{
    public class OrderTrackingModel : BaseViewModel
    {
        public ICommand LoadParcelInfoCommand { get; private set; }


        private string _BasicStatus;
        public string BasicStatus
        {
            get
            {
                return _BasicStatus;
            }
            set { _BasicStatus = value; OnPropertyChanged(); }
        }
        private string _ParcelID;
        public string ParcelID
        {
            get { return _ParcelID; }
            set
            {
                _ParcelID = value;
                OnPropertyChanged();
                ValidateParcelID();
            }
        }

        private ObservableCollection<string> _routeInfoList;
        public ObservableCollection<string> RouteInfoList
        {
            get { return _routeInfoList; }
            set
            {
                _routeInfoList = value;
                OnPropertyChanged();
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
        public class ParcelInfo
        {
            public int ID { get; set; }
            public string ParcelName { get; set; }
            public string Details { get; set; }
            public double Mass {get; set; }
            public string SimpleStatus { get; set; }
        }
        public ICommand SelectParcelCommand { get; }

        private void ExecuteSelectParcel(object parameter)
        {
            if (parameter is ParcelInfo selectedParcel)
            {
                ParcelID = Convert.ToString(selectedParcel.ID);
            }
        }

        public OrderTrackingModel()
        {
            
            ParcelTrackingCommand =
               new RelayCommand<object>((p) =>
               {
                   return true;
               }, (p) => GetParcelRoute());
            ParcelInfoList = new ObservableCollection<ParcelInfo>();
            SelectParcelCommand = new RelayCommand<object>((p) => true, ExecuteSelectParcel);
            LoadParcelInfoCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                ParcelInfoList.Clear();
                LoadParcelInfoList(Convert.ToInt32(ParcelID));
            } ) ;
            LoadParcelInfoList(Convert.ToInt32(ParcelID));

        }


        public void LoadParcelInfoList(int parcelID)
        {
            OrderTrackingManager OTM = new OrderTrackingManager();
            ParcelInfoList = OTM.LoadParcelInfoList(parcelID);
        }


       
        public ICommand ParcelTrackingCommand { get; set; }

        public void ValidateParcelID()
        {
            
            OrderTrackingManager OTM = new OrderTrackingManager();

            // Gọi phương thức ValidateParcelID() trong ParcelModel
            bool isValidParcelID = OTM.ValidateParcelID(ParcelID);

            
            if (isValidParcelID)
            {
                // Mã đơn hàng hợp lệ, tiếp tục xử lý
            }
            else
            {
                // Mã đơn hàng không hợp lệ, xử lý thông báo hoặc hành động tương ứng
            }

        }



        public void GetParcelRoute()
        {
          
            OrderTrackingManager OTM= new OrderTrackingManager();
            int iParcel=Convert.ToInt32(ParcelID);
            RouteInfoList=OTM.GetParcelRoute(iParcel);
        }
      
    }
}
