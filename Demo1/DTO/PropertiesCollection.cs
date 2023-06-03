using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Demo1.Model;
using Demo1.ViewModel;

namespace Demo1.DTO
{
    public class PropertiesCollection:BaseViewModel
    {
        private string _SCustomerName;
        private string _SCustomerID;
        private string _SCustomerAddress;
        private string _SCustomerPhoneNumber;
        private string _SCustomerDistrict;
        private string _SCustomerCity;

        //RCustomer
        private string _RCustomerName;
        private string _RCustomerID;
        private string _RCustomerAddress;
        private string _RCustomerPhoneNumber;
        private string _RCustomerDistrict;
        private string _RCustomerCity;

        //Parcel
        private string _ParcelID;
        private string _ParcelName;
        private string _ParcelValue;
        private string _ParcelMass;
        private string _ParcelWidth;
        private string _ParcelHeight;
        private string _ParcelLength;

        //
        private bool _isSpec;
        private bool _isFast;
        private bool _isSlow;
        private bool _isCOD;
        //
        private string _warehouseID;

        private ObservableCollection<string> cities;

        public ObservableCollection<string> Cities
        {
            get { return cities; }
            set
            {
                cities = value;
                OnPropertyChanged(nameof(Cities));
            }
        }

        void ValidateParcelID()
        {
            int parcelID;
            if (int.TryParse(ParcelID, out parcelID)||int.TryParse(SearchText,out parcelID))
            {
                // Chuỗi chứa số nguyên hợp lệ
                using (var context = new PBL3_demoEntities())
                {
                    var thisParcel = context.Parcels.Where(x => x.parcelID == parcelID).FirstOrDefault();
                    if (thisParcel == null)
                    {
                        MessageBoxWindow.Show("Đơn này không tồn tại trong hệ thống");
                    }
                }
            }
            else
            {
                // Chuỗi không chứa số nguyên hợp lệ
                MessageBoxWindow.Show("Mã đơn hàng vừa nhập không hợp lệ");
            }
        }
      

        private string _ShippingMethod;
        private string _ShippingFee;
        public string ShippingMethod
        {
            get
            {
                return _ShippingMethod;
            }
            set
            {
                _ShippingMethod = value;
                OnPropertyChanged();
            }
        }
        public string WarehouseID
        {
            get
            {
                return _warehouseID;
            }
            set
            {
                _warehouseID = value;
                OnPropertyChanged();
            }
        }
        private string _Type;
        public string Type
        {
            get
            {
                return _Type;
            }
            set
            {
                _Type = value;
                OnPropertyChanged();
            }
        }
        public string ShippingFee
        {
            get
            {
                return _ShippingFee;
            }
            set
            {
                _ShippingFee = value;
                OnPropertyChanged();
            }
        }
        public string SCustomerName
        {
            get
            {
                return _SCustomerName;
            }
            set
            {
                _SCustomerName = value;
                OnPropertyChanged();
            }
        }
        public string SCustomerID
        {
            get
            {
                return _SCustomerID;
            }
            set
            {
                _SCustomerID = value;
                OnPropertyChanged();
            }
        }
        public string SCustomerAddress
        {
            get
            {
                return _SCustomerAddress;
            }
            set
            {
                _SCustomerAddress = value;
                OnPropertyChanged();
            }
        }
        public string SCustomerPhoneNumber
        {
            get
            {
                return _SCustomerPhoneNumber;
            }
            set
            {
                _SCustomerPhoneNumber = value;
                OnPropertyChanged();
            }
        }
        public string SCustomerDistrict
        {
            get
            {
                return _SCustomerDistrict;
            }
            set
            {
                _SCustomerDistrict = value;
                OnPropertyChanged();
            }
        }
        public string SCustomerCity
        {
            get
            {
                return _SCustomerCity;
            }
            set
            {
                _SCustomerCity = value;
                OnPropertyChanged(nameof(SCustomerCity));
            }
        }
        //
        public string RCustomerName
        {
            get
            {
                return _RCustomerName;
            }
            set
            {
                _RCustomerName = value;
                OnPropertyChanged();
            }
        }
        public string RCustomerID
        {
            get
            {
                return _RCustomerID;
            }
            set
            {
                _RCustomerID = value;
                OnPropertyChanged();
            }
        }
        public string RCustomerAddress
        {
            get
            {
                return _RCustomerAddress;
            }
            set
            {
                _RCustomerAddress = value;
                OnPropertyChanged();
            }
        }
        public string RCustomerPhoneNumber
        {
            get
            {
                return _RCustomerPhoneNumber;
            }
            set
            {
                _RCustomerPhoneNumber = value;
                OnPropertyChanged();
            }
        }
        public string RCustomerDistrict
        {
            get
            {
                return _RCustomerDistrict;
            }
            set
            {
                _RCustomerDistrict = value;
                OnPropertyChanged();
            }
        }
        public string RCustomerCity
        {
            get
            {
                return _RCustomerCity;
            }
            set
            {
                _RCustomerCity = value;
                OnPropertyChanged();
            }
        }
        //
        public string ParcelName
        {
            get
            {
                return _ParcelName;
            }
            set
            {
                _ParcelName = value;
                OnPropertyChanged();
            }
        }
        public string ParcelValue
        {
            get
            {
                return _ParcelValue;
            }
            set
            {
                _ParcelValue = value;
                OnPropertyChanged();
            }
        }

        public string ParcelWidth
        {
            get
            {
                return _ParcelWidth;
            }
            set
            {
                _ParcelWidth = value;
                OnPropertyChanged();
            }
        }
        public string ParcelHeight
        {
            get
            {
                return _ParcelHeight;
            }
            set
            {
                _ParcelHeight = value;
                OnPropertyChanged();
            }
        }
        public string ParcelLength
        {
            get
            {
                return _ParcelLength;
            }
            set
            {
                _ParcelLength = value;
                OnPropertyChanged();
            }
        }
        public string ParcelMass
        {
            get
            {
                return _ParcelMass;
            }
            set
            {
                _ParcelMass = value;
                OnPropertyChanged();
            }
        }
        public string ParcelID
        {
            get
            {
                return _ParcelID;
            }
            set
            {
                _ParcelID = value;
                OnPropertyChanged();
            }
        }
        public bool isSpec
        {
            get { return _isSpec; }
            set
            {
                _isSpec = value;
                OnPropertyChanged(nameof(isSpec));
            }
        }


        // if it is slow shipping -> false , fast  shipping -> true
        public bool isSlow
        {
            get { return _isSlow; }
            set
            {
                _isSlow = value;
                OnPropertyChanged(nameof(isSlow));
            }
        }

        public bool isFast
        {
            get { return _isFast; }
            set
            {
                _isFast = value;
                OnPropertyChanged(nameof(isFast));
            }
        }
        public bool isCOD
        {
            get { return _isCOD; }
            set
            {
                _isCOD = value;
                OnPropertyChanged(nameof(isCOD));
            }
        }

        private string selectedSCity;
        public string SelectedSCity
        {
            get { return selectedSCity; }
            set
            {
                selectedSCity = value;

                OnPropertyChanged(nameof(SelectedSCity));


            }
        }
        private string selectedRCity;
        public string SelectedRCity
        {
            get { return selectedRCity; }
            set
            {
                selectedRCity = value;

                OnPropertyChanged(nameof(SelectedRCity));


            }
        }
        private string parcelType;

        public string ParcelType
        {
            get { return parcelType; }
            set
            {
                if (parcelType != value)
                {
                    parcelType = value;
                    OnPropertyChanged();
                }
            }
        }


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
                    ValidateParcelID();
                }
            }
        }
        private string _Cost;
        public string Cost
        {
            get { return _Cost; }
            set
            {
                if (_Cost != value)
                {
                    _Cost = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime? _CreateTime;
        public DateTime? CreateTime
        {
            get { return _CreateTime; }
            set
            {
                if (_CreateTime != value)
                {
                    _CreateTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _Details;
        public string Details
        {
            get { return _Details; }
            set
            {
                if (_Details != value)
                {
                    _Details = value;
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
        private string _All;
        public string All
        {
            get { return _All; }
            set
            {
                _All = value;
                OnPropertyChanged();
            }
        }
        private string _South;
        public string South
        {
            get { return _South; }
            set
            {
                _South = value;
                OnPropertyChanged();
            }
        }
        private string _Center;
        public string Center
        {
            get { return _Center; }
            set
            {
                _Center = value;
                OnPropertyChanged();
            }
        }
        private string _North;
        public string North
        {
            get { return _North; }
            set
            {
                _North = value;
                OnPropertyChanged();
            }
        }
        private string _WHAddress;
        public string WHAddress
        {
            get { return _WHAddress; }
            set
            {
                _WHAddress = value;
                OnPropertyChanged();
            }
        }
        private string _WHCapacity;
        public string WHCapacity
        {
            get { return _WHCapacity; }
            set
            {
                _WHCapacity = value;
                OnPropertyChanged();
            }
        }
        private string _WHContained;
        public string WHContained
        {
            get { return _WHContained; }
            set
            {
                _WHContained = value;
                OnPropertyChanged();
            }
        }

    }
}
