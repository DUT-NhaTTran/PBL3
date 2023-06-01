using Demo1.Model;
using Demo1.UserInfo;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Demo1.ViewModel
{

    public partial class AddFunctionModel:BaseViewModel
    {
        
        public ICommand AddCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public CompositeCommand AddAndCreateInvoiceCommand { get; set; }
        
        
        int isValid = 0;
        //SCustomer
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
        public ICommand CreateInvoiceCommand { get; set; }

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
        void createInvoice()
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
        Dictionary<string, int> dictionary = new Dictionary<string, int>();
        double posDiffValue;
        // danh dau cac thanh pho vao cac vung
        // vung 1 la Trung du va miền núi bắc bộ
        // vùng 2 là đồng bằng sông hồng
        // vùng 3 là Bắc trung bộ
        // vùng 4 là nam trung bộ
        // vùng 5 là đông nam bộ
        // vùng 6 là đông bằng sông cửu long

        // vùng 1
            dictionary.Add("Lai Châu", 1);
            dictionary.Add("Điện Biên", 1);
            dictionary.Add("Sơn La", 1);
            dictionary.Add("Lào Cai", 1);
            dictionary.Add("Yên Bái", 1);
            dictionary.Add("Hà Giang", 1);
            dictionary.Add("Tuyên Quang", 1);
            dictionary.Add("Cao Bằng", 1);
            dictionary.Add("Bắc Kạn", 1);
            dictionary.Add("Phú Thọ", 1);
            dictionary.Add("Thái Nguyên", 1);
            dictionary.Add("Lạng Sơn", 1);
            dictionary.Add("Bắc Giang", 1);
            dictionary.Add("Quảng Ninh", 1);
            dictionary.Add("Hòa Bình", 1);

            // vùng 2
            dictionary.Add("Vĩnh Phúc", 2);
            dictionary.Add("Bắc Ninh", 2);
            dictionary.Add("Hải Dương", 2);
            dictionary.Add("Hưng Yên", 2);
            dictionary.Add("Hà Nam", 2);
            dictionary.Add("Thái Bình", 2);
            dictionary.Add("Nam Định", 2);
            dictionary.Add("Ninh Bình", 2);
            dictionary.Add("Hà Nội", 2);
            dictionary.Add("Hải Phòng", 2);

            // vùng 3
            dictionary.Add("Thanh Hóa", 3);
            dictionary.Add("Nghệ An", 3);
            dictionary.Add("Hà Tĩnh", 3);
            dictionary.Add("Quảng Bình", 3);
            dictionary.Add("Quảng Trị", 3);
            dictionary.Add("Thừa Thiên Huế", 3);

            // vùng 4
            dictionary.Add("Đà Nẵng", 4);
            dictionary.Add("Quảng Nam", 4);
            dictionary.Add("Quảng Ngãi", 4);
            dictionary.Add("Bình Định", 4);
            dictionary.Add("Phú Yên", 4);
            dictionary.Add("Khánh Hòa", 4);

            // vùng 5
            dictionary.Add("Kom Tum", 5);
            dictionary.Add("Gia Lai", 5);
            dictionary.Add("Đắk Lắk", 5);
            dictionary.Add("Đắk Nông", 5);
            dictionary.Add("Lâm Đồng", 5);

            // vùng 6
            dictionary.Add("Ninh Thuận", 6);
            dictionary.Add("Bình Thuận", 6);
            dictionary.Add("Đồng Nai", 6);
            dictionary.Add("Bình Phước", 6);
            dictionary.Add("Tây Ninh", 6);
            dictionary.Add("Bình Dương", 6);
            dictionary.Add("Vũng Tàu", 6);
            dictionary.Add("Hồ Chí Minh", 6);
            dictionary.Add("TP.HCM", 6);

            //vùng 7
            dictionary.Add("Long An", 7);
            dictionary.Add("Đồng Tháp", 7);
            dictionary.Add("Tiền Giang", 7);
            dictionary.Add("Bến Tre", 7);
            dictionary.Add("Vĩnh Long", 7);
            dictionary.Add("Trà Vinh", 7);
            dictionary.Add("Sóc Trăng", 7);
            dictionary.Add("Cần Thơ", 7);
            dictionary.Add("Hậu Giang", 7);
            dictionary.Add("An Giang", 7);
            dictionary.Add("Kiên Giang", 7);
            dictionary.Add("Bạc Liêu", 7);
            dictionary.Add("Cà Mau", 7);

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
        
        public AddFunctionModel()
        {
          
            string accountID = AccountManager.Instance.GetAccountID();
            WarehouseID = AccountManager.Instance.GetUserWarehouseID(accountID);
            Cities = new ObservableCollection<string>
            {
                "An Giang","Vũng Tàu","Bạc Liêu","Bắc Kạn","Bắc Giang","Bắc Ninh","Bến Tre","Bình Dương","Bình Định","Bình Phước","Cà Mau","Cao Bằng","Cần Thơ",
                "Đà Nẵng","Đắk Lắk","Đắk Nông", "Điện Biên", "Đồng Nai", "Đồng Tháp", "Gia Lai", "Hà Giang", "Hà Nam", "Hà Nội", "TP.HCM", "Hậu Giang", "Hưng Yên",
                "Khánh Hoà", "Kiên Giang", "Kon Tum", "Lai Châu", "Lào Cai", "Lạng Sơn", "Lâm Đồng", "Long An", "Nam Định", "Nghệ An", "Ninh Bình",
                "Ninh Thuận", "Phú Thọ","Phú Yên","Quảng Bình","Quảng Nam","Quảng Ngãi","Quảng Ninh","Quảng Trị","Sóc Trăng","Sơn La","Tây Ninh",
                "Thái Bình","Thanh Hoá","Thừa Thiên Huế","Tiền Giang","Trà Vinh","Tuyên Quang","Vĩnh Long","Vĩnh Phúc","Yên Bái"


            };
           
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
                createInvoice();
                ShippingFee = Convert.ToString(shippingFeeFunc());
                int lastInvoiceID;
                using(var context1=new PBL3_demoEntities())
                {
                    var maxInvoiceID = context1.Invoices.Max(i => i.invoiceID);
                    lastInvoiceID= maxInvoiceID;
                }
                using (var context = new Model.PBL3_demoEntities())
                {
                    // Tạo một đối tượng Invoice mới
                    var newInvoice = new Model.Invoice
                    {
                        invoiceID = lastInvoiceID + 1,
                        parcelID = Convert.ToInt32(ParcelID),
                        customerID = SCustomerID,
                        cost = Convert.ToDouble(ShippingFee),
                        outputTime = DateTime.Now
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
