using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Demo1.DTO;
using Demo1.Model;
using Demo1.View;
using MaterialDesignThemes.Wpf;
using Demo1.UserInfo;

namespace Demo1.ViewModel
{
    public class SearchParcelModel : PropertiesCollection
    {
        //private string searchText;
        
      
        //private string parcelType;

        //public string ParcelType
        //{
        //    get { return parcelType; }
        //    set
        //    {
        //        if (parcelType != value)
        //        {
        //            parcelType = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}


        
        //public string SearchText
        //{
        //    get { return searchText; }
        //    set
        //    {
        //        if (searchText != value)
        //        {
        //            searchText = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}
        //private string _Cost;
        //public string Cost
        //{
        //    get { return _Cost; }
        //    set
        //    {
        //        if (_Cost != value)
        //        {
        //            _Cost = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}
        //private DateTime? _CreateTime;
        //public DateTime? CreateTime
        //{
        //    get { return _CreateTime; }
        //    set
        //    {
        //        if (_CreateTime != value)
        //        {
        //            _CreateTime = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        //private string _Details;
        //public string Details
        //{
        //    get { return _Details ; }
        //    set
        //    {
        //        if (_Details != value)
        //        {
        //            _Details = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        

        public ICommand SearchCommand { get; set; }
        public ICommand ParcelNameClickCommand { get; set; }

        //public ICommand SenderCustomerCommand { get; set; }

        //public ICommand ReceiverCustomerCommand { get; set; }
        public SearchParcelModel()
        {

            SearchCommand = new RelayCommand<object>((p) => { return true; },
                (p) => { Search(SearchText); });
            /*            SearchCommand = new RelayCommand<object>((p) => { return !string.IsNullOrEmpty(SearchText); },
                (p) => { Search(SearchText); });*/

            ParcelNameClickCommand = new RelayCommand<object>((p) => { return true; },
    (p) => { OpenResultOfSerchWindow("abc"); });
        }

        void Search(string _parcelID)
        { 
            if (string.IsNullOrEmpty(_parcelID))
            {
                
               MessageBox.Show("Chưa nhập mã đơn hàng");
            }
            else
            {
                using (var context = new PBL3_demoEntities())
                {
                    int num_parcelID = int.Parse(_parcelID);

                    var parcel = context.Parcels.FirstOrDefault(x => x.parcelID == num_parcelID);
                    if (parcel != null)
                    {

                        ParcelName = parcel.parcelName;
                        ParcelType = ((bool)parcel.type) ? "Hàng dễ vỡ" : "Hàng bình thường";
                        ParcelValue = parcel.parcelValue.ToString();

                    }
                    else
                    {
                        MessageBox.Show("Đơn hàng không tồn tại trong hệ thống! Xin vui lòng thử lại");
                    }
                }
            }
            
            
        }

        // Xử lý sự kiện click chuột vào TextBlock ParcelName
        public void OpenResultOfSerchWindow(string _parcelID)
        {
            ResultOfSearch ros = new ResultOfSearch(_parcelID);
            SetAllParcelInfo();
            ros.ShowDialog();
            
        }

        void SetAllParcelInfo()
        {
            
            SCustomerName = ParcelInfo.Instance.GetCustomerName(SearchText,1);
            RCustomerName = ParcelInfo.Instance.GetCustomerName(SearchText,2);
            SCustomerAddress=ParcelInfo.Instance.GetCustomerAddress(SearchText,1);
            RCustomerAddress = ParcelInfo.Instance.GetCustomerAddress(SearchText, 2);
            SCustomerPhoneNumber=ParcelInfo.Instance.GetCustomerPhoneNumber(SearchText,1);
            RCustomerPhoneNumber = ParcelInfo.Instance.GetCustomerPhoneNumber(SearchText, 2);
            ShippingFee = Convert.ToString(ParcelInfo.Instance.GetParcelTotalCost(SearchText));
            Cost=Convert.ToString(Convert.ToDouble(ShippingFee)-Convert.ToDouble(ParcelValue));
            if (ParcelInfo.Instance.GetShippingMethod(SearchText)) ShippingMethod = "Giao hàng nhanh";
            else ShippingMethod = "Giao hàng chậm";
            CreateTime=ParcelInfo.Instance.GetCreateTime(SearchText);
            Details=ParcelInfo.Instance.GetDetails(SearchText);
           
        }
        string GetSenderCustomerInfo (string _parcelID)
        {
            using (var context = new PBL3_demoEntities())
            {
                int num_parcelID = int.Parse(_parcelID);
                var parcelRCustomerID = context.Parcels.Where(x => x.parcelID == num_parcelID).Select(x=> x.RCustomerID).FirstOrDefault();
                if (parcelRCustomerID != null)
                {
                    var RCustomer = context.Customers.Where(x => x.customerID == parcelRCustomerID).FirstOrDefault();
                    string RCustomerInfo = RCustomer.customerName +"\n" + RCustomer.customerLocation+"\n"+ RCustomer.customerPhoneNumber;
                }
                else
                {
                    MessageBox.Show("Đơn hàng không tồn tại trong hệ thống! Xin vui lòng thử lại");
                }
            }
            return "a";
        }

    }
        
}
