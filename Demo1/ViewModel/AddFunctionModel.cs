using Demo1.DTO;
using Demo1.Model;
using Demo1.UserInfo;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Input;


namespace Demo1.ViewModel
{

    public partial class AddFunctionModel:PropertiesCollection
    {
        public ICommand AddCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public CompositeCommand AddAndCreateInvoiceCommand { get; set; }

        public ICommand CreateInvoiceCommand { get; set; }
        int isValid = 0;
        void CreateInvoice()
        {
            if (isSlow) ShippingMethod = "Chuyển phát chậm";
            else ShippingMethod = "Chuyển phát nhanh";
            if (isSpec) Type = "Hàng dễ vỡ/ đồ điện tử";
            else Type = "";


        }
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
        double shippingFeeFunc()
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
                return checkValid();
            }, (p) =>
            {
               
            //int setParcelID;
            SCustomerCity=SelectedSCity;
            RCustomerCity=SelectedRCity;    
            using (var context = new Model.PBL3_demoEntities())
            {
                var sc = context.Customers.FirstOrDefault(x => x.customerID == SCustomerID);
                if (sc == null)
                {
                    var newSCustomer = new Customer { customerID = SCustomerID, customerLocation = SCustomerAddress + "," + SCustomerDistrict + "," + SCustomerCity, customerPhoneNumber = SCustomerPhoneNumber, customerName = SCustomerName };
                    context.Customers.Add(newSCustomer);
                }
                else
                {
                    if ((sc.customerLocation != SCustomerAddress + "," + SCustomerDistrict + "," + SCustomerCity) || (sc.customerPhoneNumber != SCustomerPhoneNumber))
                    {
                        sc.customerLocation = SCustomerAddress + "," + SCustomerDistrict + "," + SCustomerCity;
                        sc.customerPhoneNumber = SCustomerPhoneNumber;
                    }

                }

                var rc = context.Customers.FirstOrDefault(x => x.customerID == RCustomerID);
                if (rc == null)
                {
                    var newRCustomer = new Customer { customerID = RCustomerID, customerLocation = RCustomerAddress + "," + RCustomerDistrict + "," + RCustomerCity, customerPhoneNumber = RCustomerPhoneNumber, customerName = RCustomerName };
                    context.Customers.Add(newRCustomer);
                }
                else
                {
                    if ((rc.customerLocation != RCustomerAddress + "," + RCustomerDistrict + "," + RCustomerCity) || (rc.customerPhoneNumber != RCustomerPhoneNumber))
                    {
                        rc.customerLocation = RCustomerAddress + "," + RCustomerDistrict + "," + RCustomerCity;
                        rc.customerPhoneNumber = RCustomerPhoneNumber;
                    }

                }
                    context.SaveChanges();
                   
            }
                 
                using (var context1 = new Model.PBL3_demoEntities())
                {
                    var lastRow = context1.Parcels.OrderByDescending(x => x.parcelID).FirstOrDefault();
                    ParcelID = Convert.ToString(Convert.ToInt32(lastRow.parcelID)+ 1);
                    var newParcel = new Parcel { parcelID = Convert.ToInt32(ParcelID), parcelName = ParcelName, parcelMass = Convert.ToDouble(ParcelMass), parcelSize = ParcelLength + " x "+ParcelWidth+" x "+ParcelHeight, parcelValue = Convert.ToDouble(ParcelValue), type = isSpec, RCustomerID = RCustomerID, SCustomerID = SCustomerID, shippingMethod = isFast, isCOD = isCOD,createTime=DateTime.Now,currentWarehouseID=WarehouseID};
                    context1.Parcels.Add(newParcel);
                    context1.SaveChanges();
                }
                using (var context = new Model.PBL3_demoEntities())
                {
                    var newRoute = new Route {parcelID= Convert.ToInt32(ParcelID),relatedWarehouseID=WarehouseID,time=DateTime.Now,details="Đơn hàng đã được khởi tạo"};
                    context.Routes.Add(newRoute);
                    context.SaveChanges();
                }
            });

           
            bool checkValid()
            {

                
                if (string.IsNullOrEmpty(SCustomerName) || string.IsNullOrEmpty(SCustomerID) || string.IsNullOrEmpty(SCustomerAddress) ||
                         string.IsNullOrEmpty(SCustomerPhoneNumber) || string.IsNullOrEmpty(SCustomerDistrict) || string.IsNullOrEmpty(SCustomerCity) ||
                         string.IsNullOrEmpty(RCustomerName) || string.IsNullOrEmpty(RCustomerID) || string.IsNullOrEmpty(RCustomerAddress) ||string.IsNullOrEmpty(ParcelLength) ||
                         string.IsNullOrEmpty(RCustomerPhoneNumber) || string.IsNullOrEmpty(RCustomerDistrict) || string.IsNullOrEmpty(RCustomerCity) || string.IsNullOrEmpty(ParcelHeight)||
                         string.IsNullOrEmpty(ParcelName)|| string.IsNullOrEmpty(ParcelMass) || string.IsNullOrEmpty(ParcelWidth)
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
                if (isNumeric&&isNumeric1&&isNumeric2&&isNumeric3&&isNumeric4&&isNumeric5&&isNumeric6)
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
                CreateInvoice();
                TotalCost = shippingFeeFunc();
                int lastInvoiceID;
                using(var context1=new PBL3_demoEntities())
                {
                    var maxInvoiceID = context1.Invoices.Max(i => i.invoiceID);
                    lastInvoiceID= maxInvoiceID;
                }
                string account = AccountManager.Instance.GetAccountID();
                string startWHID = AccountManager.Instance.GetUserWarehouseID(account);
                using (var context = new Model.PBL3_demoEntities())
                {
                   
                    // Tạo một đối tượng Invoice mới
                    var newInvoice = new Model.Invoice
                    {
                        invoiceID = lastInvoiceID + 1,
                        parcelID = Convert.ToInt32(ParcelID),
                        customerID = SCustomerID,
                        cost = Convert.ToDouble(TotalCost),
                        outputTime = DateTime.Now,
                        shippingFee = TotalCost - Convert.ToDouble(ParcelValue),
                        startWarehouseID = startWHID
                    };

                    // Thêm đối tượng Invoice mới vào ngữ cảnh và lưu các thay đổi
                    context.Invoices.Add(newInvoice);
                    context.SaveChanges();
                }
                Invoice invoice = new Invoice();
                
                invoice.DataContext = this;
                invoice.Show();
            });
            AddAndCreateInvoiceCommand = new CompositeCommand();
            AddAndCreateInvoiceCommand.RegisterCommand(AddCommand);
            AddAndCreateInvoiceCommand.RegisterCommand(CreateInvoiceCommand);
            
        }
    }
}
