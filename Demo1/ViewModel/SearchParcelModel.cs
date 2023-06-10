using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using Demo1.DTO;

using Demo1.Model;

using Demo1.UserInfo;
using Demo1.View;

namespace Demo1.ViewModel
{
    public class SearchParcelModel : PropertiesCollection
    {
       
        private string _SearchParcelText;
        public string SearchParcelText
        {
            get { return _SearchParcelText; }
            set { _SearchParcelText = value; OnPropertyChanged(nameof(SearchParcelText)); }
        }

        public ICommand SearchCommand { get; set; }
        public ICommand ParcelNameClickCommand { get; set; }

        private ObservableCollection<ParcelInfoInSearch> _parcelInfoList;
        public ObservableCollection<ParcelInfoInSearch> ParcelInfoListInSearch
        {
            get { return _parcelInfoList; }
            set
            {
                _parcelInfoList = value;
                OnPropertyChanged();
            }
        }
        private string _SisCOD;
        public string SisCOD
        {
            get => _SisCOD;
            set { _SisCOD = value; OnPropertyChanged(); }
        }

        public class ParcelInfoInSearch
        {
            public int ID { get; set; }
            public string ParcelName { get; set; }
            public string ParcelType { get; set; }
            public double ParcelValue { get; set; }
        }
        public SearchParcelModel()
        {
          
            ParcelInfoListInSearch = new ObservableCollection<ParcelInfoInSearch>();
            SearchCommand = new RelayCommand<object>((p) => { return true; },
                (p) => { GetAllParcels(); });


            ParcelNameClickCommand = new RelayCommand<object>((p) => { return true; },
    (p) => { OpenResultOfSerchWindow(); });
        }
      
        void GetSisCOD()
        {
            int iParcelID = Convert.ToInt32(SearchParcelText);
            SearchManager SM = new SearchManager();
            SisCOD = SM.GetSisCOD(iParcelID);

        }

        void GetAllParcels()
        {
            
            SearchManager SM = new SearchManager();
            ParcelInfoListInSearch = SM.GetAllParcels();
        }


        public void OpenResultOfSerchWindow()
        {
            GetSisCOD();
            ResultOfSearch ros = new ResultOfSearch();
            SetAllParcelInfo();
            ros.ShowDialog();
            
        }
        
        public void LoadAllParcelSearched()
        {
            SearchManager SM = new SearchManager();
            List<ParcelInfoInSearch> queryResult = SM.LoadAllParcelSearched(SearchParcelText);

            ParcelInfoListInSearch.Clear(); // Xóa tất cả các phần tử trong danh sách hiện có

            foreach (ParcelInfoInSearch item in queryResult)
            {
                ParcelInfoListInSearch.Add(item);
            }

        }
        void SetAllParcelInfo()
        {
            
            SCustomerName = ParcelInfo.Instance.GetCustomerName(SearchParcelText,1);
            RCustomerName = ParcelInfo.Instance.GetCustomerName(SearchParcelText,2);
            SCustomerAddress=ParcelInfo.Instance.GetCustomerAddress(SearchParcelText,1);
            RCustomerAddress = ParcelInfo.Instance.GetCustomerAddress(SearchParcelText, 2);
            SCustomerPhoneNumber=ParcelInfo.Instance.GetCustomerPhoneNumber(SearchParcelText,1);
            RCustomerPhoneNumber = ParcelInfo.Instance.GetCustomerPhoneNumber(SearchParcelText, 2);
            Cost = Convert.ToString(ParcelInfo.Instance.GetParcelTotalCost(SearchParcelText));
            ParcelValue=Convert.ToString(ParcelInfo.Instance.GetParcelValue(SearchParcelText));
            ShippingFee=Convert.ToString(ParcelInfo.Instance.GetParcelShippingFee(SearchParcelText));   
            if (ParcelInfo.Instance.GetShippingMethod(SearchParcelText)==1) ShippingMethod = "Giao hàng nhanh";
            else ShippingMethod = "Giao hàng chậm";
            CreateTime=ParcelInfo.Instance.GetCreateTime(SearchParcelText);
            var thisParcel = ParcelInfo.Instance.GetParcelRecord(SearchParcelText);
            if (thisParcel.parcelStatus == false || thisParcel.parcelStatus == null)
            {
                Details = ParcelInfo.Instance.GetDetails(SearchParcelText);
            }

            else
            {
                Details = "Đơn hàng bị trả lại";
            }
        }
        
    }
        
}
