using Demo1.DTO;
using Demo1.Model;
using Demo1.UserInfo;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using Demo1.View;
using System.Text.RegularExpressions;

namespace Demo1.ViewModel
{

    public partial class AddFunctionModel:PropertiesCollection
    {
        private AddManager _AddManager;
        public ICommand AddCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public CompositeCommand AddAndCreateInvoiceCommand { get; set; }

        public ICommand CreateInvoiceCommand { get; set; }
        int isValid = 0;
       
        // đơn vị là kg
        double MassFee(double mass)
        {
            double massFee;
            switch (mass)
            {
                case double n when n < 10.0:
                    massFee = 0;
                    break;
                case double n when n < 25.0:
                    massFee = 20000;
                    break;
                case double n when n < 50.0:
                    massFee = 40000;
                    break;
                default:
                    massFee = 65000;
                    break;
            }
            return massFee;
        }

        // đơn vị là mm
        double VolumeFee(double length, double width, double height)
        {
            double volumeFee;
            double volume = length * width * height;
            switch (volume)
            {
                case double n when n < 1000000.0:
                    volumeFee = 0;
                    break;
                case double n when n < 27000000.0:
                    volumeFee = 15000;
                    break;
                case double n when n < 125000000.0:
                    volumeFee = 30000;
                    break;
                default:
                    volumeFee = 50000;
                    break;
            }
            return volumeFee;
        }
        double ShippingFeeFunc()
        {
        
            double posDiffValue;
            Dictionary<string, int> dictionary = DictionaryData.GetDictionary();

            bool extraFeeCheck = isSpec;
            bool transportationFeeCheck = isFast;
            bool cODFeeCheck = isCOD;
            posDiffValue = Math.Abs(dictionary[SCustomerCity] - dictionary[RCustomerCity]);
            double posFee = 17000 + posDiffValue * 5000;
            double TotalFee = posFee;
            if (extraFeeCheck) TotalFee += 17000;
            if (transportationFeeCheck) TotalFee += 17000;
            if (cODFeeCheck) TotalFee += (12000 + double.Parse(ParcelValue));
            TotalFee += MassFee(double.Parse(ParcelMass)) +
            VolumeFee(double.Parse(ParcelLength), double.Parse(ParcelWidth), double.Parse(ParcelHeight));


            return TotalFee;
        }

        private double _TotalCost;
        public double TotalCost
        {
            get { return _TotalCost; }
            set { _TotalCost = value; OnPropertyChanged(); }
        }
        
        public AddFunctionModel()
        {
            _AddManager = new AddManager();

            string accountID = AccountManager.Instance.GetAccountID();
            WarehouseID = AccountManager.Instance.GetUserWarehouseID(accountID);
            Cities = DictionaryData.GetCities();


            void ResetData()
            {
                SCustomerName = "";
                SCustomerID = "";
                SCustomerPhoneNumber = "";
                SCustomerAddress = "";
                SCustomerDistrict = "";
                SCustomerCity = "";
                RCustomerName = "";
                RCustomerID = "";
                RCustomerPhoneNumber = "";
                RCustomerAddress = "";
                RCustomerDistrict = "";
                RCustomerCity = "";
                ParcelName = "";
                ParcelValue = "";
                ParcelMass = "";
                ParcelWidth = "";
                ParcelLength = "";
                ParcelHeight = "";
                isSlow = false;
                isSpec = false;
                isFast = false;
                isCOD = false;


            }
            
            RefreshCommand = new RelayCommand<object>((t) => { return true; }, (t) =>
            {
                ResetData();

            });
            AddCommand = new RelayCommand<object>((p) =>
            {
                return checkValid() && SCustomerID != RCustomerID&&SCustomerPhoneNumber!=RCustomerPhoneNumber;
            }, (p) =>
            {
               
                //int setParcelID;
                SCustomerCity =SelectedSCity;
                RCustomerCity = SelectedRCity;
                AddOrUpdateCustomer(SCustomerID, SCustomerName, SCustomerAddress + "," + SCustomerDistrict + "," + SCustomerCity, SCustomerPhoneNumber);
                AddOrUpdateCustomer(RCustomerID, RCustomerName, RCustomerAddress + "," + RCustomerDistrict + "," + RCustomerCity, RCustomerPhoneNumber);
                ParcelID = _AddManager.GetParcelID();
                AddNewParcel(ParcelName,Convert.ToDouble(ParcelMass), ParcelLength + " x " + ParcelWidth + " x " + ParcelHeight, Convert.ToDouble(ParcelValue), isSpec, RCustomerID, SCustomerID, isFast, isCOD, WarehouseID);
                AddNewRoute(Convert.ToInt32(ParcelID), WarehouseID);
            });

           
            bool checkValid()
            {
                if (string.IsNullOrEmpty(SCustomerName) || string.IsNullOrEmpty(SCustomerID) || string.IsNullOrEmpty(SCustomerAddress) ||
                         string.IsNullOrEmpty(SCustomerPhoneNumber) || string.IsNullOrEmpty(SCustomerDistrict) || string.IsNullOrEmpty(SCustomerCity) ||
                         string.IsNullOrEmpty(RCustomerName) || string.IsNullOrEmpty(RCustomerID) || string.IsNullOrEmpty(RCustomerAddress) || string.IsNullOrEmpty(ParcelLength) ||
                         string.IsNullOrEmpty(RCustomerPhoneNumber) || string.IsNullOrEmpty(RCustomerDistrict) || string.IsNullOrEmpty(RCustomerCity) || string.IsNullOrEmpty(ParcelHeight) ||
                         string.IsNullOrEmpty(ParcelName) || string.IsNullOrEmpty(ParcelMass) || string.IsNullOrEmpty(ParcelWidth)
                         || (isSlow == false && isFast == false))

                    isValid = 0;

                else isValid = 1;

                double number, number1, number2, number3, number4;
                double number5, number6;
                ////con valid sdt chua lam
                bool isNumeric = double.TryParse(ParcelValue, out number);
                bool isNumeric1 = double.TryParse(ParcelMass, out number1);
                bool isNumeric2 = double.TryParse(ParcelHeight, out number2);
                bool isNumeric3 = double.TryParse(ParcelWidth, out number3);
                bool isNumeric4 = double.TryParse(ParcelLength, out number4);
                bool isNumeric5 = double.TryParse(RCustomerID, out number5);
                bool isNumeric6 = double.TryParse(SCustomerID, out number6);
                bool isNumeric7 = IsPhoneNumber(SCustomerPhoneNumber);
                bool isNumeric8 = IsPhoneNumber(RCustomerPhoneNumber);
                if (isNumeric && isNumeric1 && isNumeric2 && isNumeric3 && isNumeric4 && isNumeric5 && isNumeric6 && isNumeric7 && isNumeric8 && IsID(SCustomerID) && IsID(RCustomerID))
                {
                    isValid = 1;
                }
                else
                {
                    isValid = 0;
                }

                if (isValid == 0) return false;
                else return true;
            }

           
            CreateInvoiceCommand = new RelayCommand<object>((x) => { return true; }, (x) =>
            {
                CreateNewInvoice();
                View.Invoice invoice = new View.Invoice();
                ShippingMethod = (isFast) ? "Giao hàng nhanh " : "Giao hàng chậm";
                Type = (isSpec) ? "Hàng dễ vỡ/ đồ điện tử" : "Bình thường";
                TotalCost = ShippingFeeFunc();
                invoice.DataContext = this;
                invoice.Show();
            });
            AddAndCreateInvoiceCommand = new CompositeCommand();
            AddAndCreateInvoiceCommand.RegisterCommand(AddCommand);
            AddAndCreateInvoiceCommand.RegisterCommand(CreateInvoiceCommand);
            
        }
        public void AddOrUpdateCustomer(string customerID, string customerName, string customerLocation, string customerPhoneNumber)
        {
            _AddManager.AddOrUpdateCustomer(customerID, customerName, customerLocation, customerPhoneNumber);
        }

        public void AddNewParcel(string parcelName, double parcelMass, string parcelSize, double parcelValue, bool isSpec, string RCustomerID, string SCustomerID, bool isFast, bool isCOD, string WarehouseID)
        {
            _AddManager.AddNewParcel(parcelName, parcelMass, parcelSize, parcelValue, isSpec, RCustomerID, SCustomerID, isFast, isCOD, WarehouseID);
        }

        public void AddNewRoute(int parcelID, string relatedWarehouseID)
        {
            _AddManager.AddNewRoute(parcelID, relatedWarehouseID);
        }
        public void CreateNewInvoice()
        {

            _AddManager.CreateInvoice(ParcelID, SCustomerID, ShippingFeeFunc(), WarehouseID,GetShippingFee());
        }
        bool IsPhoneNumber(string inputStr)
        {
            string pattern = @"^0\d{9}$"; // Định dạng số điện thoại gồm 10 chữ số và có số 0 ở trước

            if (inputStr?.Length > 0 && Regex.IsMatch(inputStr, pattern))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        double GetShippingFee()
        {
            double res = isCOD ? (ShippingFeeFunc() - Convert.ToDouble(ParcelValue)) : ShippingFeeFunc();
            return res;
        }
        bool IsID(string input)
        {
            return input.Length == 12 && input.All(char.IsDigit);
        }
    }
}
