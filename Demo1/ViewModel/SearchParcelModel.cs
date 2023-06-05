using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Demo1.DTO;
using Demo1.Model;

using Demo1.UserInfo;

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

        void GetAllParcels()
        {
            using (var context = new PBL3_demoEntities())
            {
                ParcelInfoListInSearch.Clear();
                var parcels = context.Parcels.ToList();
                //ParcelInfoListInSearch = new ObservableCollection<ParcelInfoInSearch>();


                foreach (var parcel in parcels)
                {
                    var parcelInfo = new ParcelInfoInSearch
                    {
                        ID = parcel.parcelID,
                        ParcelName = parcel.parcelName,
                        ParcelType = ((bool)parcel.type) ? "Hàng dễ vỡ" : "Hàng bình thường",
                        ParcelValue = (double)parcel.parcelValue
                    };

                    ParcelInfoListInSearch.Add(parcelInfo);
                }
            }
        }

        // Xử lý sự kiện click chuột vào TextBlock ParcelName
        public void OpenResultOfSerchWindow()
        {
            ResultOfSearch ros = new ResultOfSearch();
            SetAllParcelInfo();
            ros.ShowDialog();
            
        }
        
        public void LoadAllParcelSearched()
        {
            int parcelID;
            if (int.TryParse(SearchParcelText, out parcelID))
            {
                using (var dbContext = new Model.PBL3_demoEntities())
                {
                    var query = dbContext.Parcels
                        .Where(x => x.parcelID.ToString().Contains(SearchParcelText) || x.parcelName.Contains(SearchParcelText))
                        .Select(y => new ParcelInfoInSearch
                        {
                            ID = y.parcelID,
                            ParcelName = y.parcelName,
                            ParcelType = (bool)y.type ? "Hàng dễ vỡ/điện tử" : "Bình thường",
                            ParcelValue = (double)y.parcelValue
                        }); ;

                    List<ParcelInfoInSearch> queryResult = query.ToList();

                    ParcelInfoListInSearch.Clear(); // Xóa tất cả các phần tử trong danh sách hiện có

                    foreach (ParcelInfoInSearch item in queryResult)
                    {
                      
                        ParcelInfoListInSearch.Add(item);
                    }
                }
            }
            else
            {
                ParcelInfoListInSearch.Clear();
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
            ShippingFee = Convert.ToString(ParcelInfo.Instance.GetParcelTotalCost(SearchParcelText));
            ParcelValue=Convert.ToString(ParcelInfo.Instance.GetParcelValue(SearchParcelText));
            Cost = (isCOD) ? Convert.ToString(Convert.ToDouble(ShippingFee) - Convert.ToDouble(ParcelValue)) : ShippingFee;
           
            if (ParcelInfo.Instance.GetShippingMethod(SearchParcelText)) ShippingMethod = "Giao hàng nhanh";
            else ShippingMethod = "Giao hàng chậm";
            CreateTime=ParcelInfo.Instance.GetCreateTime(SearchParcelText);
            Details=ParcelInfo.Instance.GetDetails(SearchParcelText);
           
        }
        
    }
        
}
