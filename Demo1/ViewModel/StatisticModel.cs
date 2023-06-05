using Demo1.Model;
using Demo1.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static Demo1.ViewModel.SearchParcelModel;

namespace Demo1.ViewModel
{
    public class StatisticModel : BaseViewModel
    {
        private ObservableCollection<Warehouse> sortedWarehouseToView;
        private ObservableCollection<string> sortedWarehouseString;
        private string statisticInfoString;
        private string statisticCompareString;

        private DateTime _selectedDate;
        private int _selectedMonth;
        private int _selectedYear;
        private string _SearchWHID;
        public string SearchWHID
        {
            get { return _SearchWHID; }
            set { _SearchWHID = value; OnPropertyChanged(nameof(SearchWHID)); }
        }
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                SelectedMonth = _selectedDate.Month;
                SelectedYear = _selectedDate.Year;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

        public int SelectedMonth
        {
            get { return _selectedMonth; }
            set
            {
                _selectedMonth = value;
                OnPropertyChanged(nameof(SelectedMonth));
            }
        }

        public int SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                _selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
            }
        }

        
        public ObservableCollection<Warehouse> SortedWarehouseToView
        {
            get { return sortedWarehouseToView; }
            set
            {
                sortedWarehouseToView = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> SortedWarehouseString
        {
            get { return sortedWarehouseString; }
            set
            {
                sortedWarehouseString = value;
                OnPropertyChanged();
            }
        }

        public string StatisticInfoString
        {
            get { return statisticInfoString; }
            set
            {
                statisticInfoString = value;
                OnPropertyChanged();
            }
        }

        public string StatisticCompareString
        {
            get { return statisticCompareString; }
            set
            {
                statisticCompareString = value;
                OnPropertyChanged();
            }
        }
        private int  _TotalParcel;
        public int TotalParcel
        {
            get
            {
                return _TotalParcel;
            }
            set
            {
                _TotalParcel = value;
                OnPropertyChanged();
            }
        }
        private int _NewCustomer;
        public int NewCustomer
        {
            get
            {
                return _NewCustomer;
            }
            set
            {
                _NewCustomer = value;
                OnPropertyChanged();
            }
        }
        private string _ComparedParcelFigure;
        public string ComparedParcelFigure
        {
            get => _ComparedParcelFigure;
            set
            {
                _ComparedParcelFigure = value;
                OnPropertyChanged();
            }
        }
        private string _ComparedNewCustomerFigure;
        public string ComparedNewCustomerFigure
        {
            get { return _ComparedNewCustomerFigure; }
            set { _ComparedNewCustomerFigure = value; OnPropertyChanged(); }   
        }
        private string _ComparedRevenueFigure;
        public string ComparedRevenueFigure
        {
            get { return _ComparedRevenueFigure; }
            set { _ComparedRevenueFigure = value; OnPropertyChanged(); }
        }
        private double _Revenue;
        public double Revenue
        {
            get { return _Revenue; }
            set { _Revenue = value; OnPropertyChanged();}
        }
        public ICommand IDSortCommand { get; set; }
        public ICommand CapacitySortCommand { get; set; }
        public ICommand NumberOfStaffSortCommand { get; set; }

        public ICommand NumberOfParcelSortCommand { get; set; }
        public ICommand RevenueSortCommand { get; set; }

        // ham sort nguoc 
        public ICommand IDSortAscendingCommand { get; set; }
        public ICommand CapacitySortAscendingCommand { get; set; }
        public ICommand NumberOfStaffSortAscendingCommand { get; set; }

        public ICommand NumberOfParcelSortAscendingCommand { get; set; }
        public ICommand RevenueSortAscendingCommand { get; set; }

        public ICommand ResetSortCommand { get; set; }

        private List<WarehouseModel> _warehouseList;

        public List<WarehouseModel> warehouseList
        {
            get { return _warehouseList; }
            set
            {
                if (_warehouseList != value)
                {
                    _warehouseList = value;
                    OnPropertyChanged();
                }
            }
        }
        public StatisticModel()
        {
            SelectedDate = DateTime.Now;

            SortedWarehouseToView = new ObservableCollection<Warehouse>();
            using (var context = new PBL3_demoEntities())
            {
                var warehouseList = context.Warehouses.Select(x => x).ToList();
                foreach (var w in warehouseList)
                {
                    SortedWarehouseToView.Add(w);
                }
                //int monthView = 5;
                //int yearView = 2023;

                //StatisticInfoString = $"Doanh thu : {TotalRevenue(monthView, yearView)}, Tổng đơn : {TotalNumberOfParcel()}, Khách hàng mới : {TotalNumberOfNewCustomer(monthView, yearView)}";
                //StatisticCompareString = $"So sánh danh thu : {PercentageOfRevenue(monthView, yearView, TotalRevenue(monthView, yearView))} \n" +
                //                         $"So sánh tổng đơn : {PercentageOfParcel(monthView, yearView, TotalNumberOfParcel(monthView, yearView))} \n" +
                //                         $"So sánh khách hàng mới : {PercentageOfNewCustomer(monthView, yearView, TotalNumberOfNewCustomer(monthView, yearView))}";
                SortedWarehouseString = new ObservableCollection<string>();
                SortedWarehouseString = ToStringAfterSort(SortedWarehouseToView, "none", SelectedMonth, SelectedYear, context);
                IDSortCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortedWarehouseString = ToStringAfterSort(SortedWarehouseToView, "ID", SelectedMonth, SelectedYear, context); Change(); });

                IDSortAscendingCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortedWarehouseString = Reverse(ToStringAfterSort(SortedWarehouseToView, "ID", SelectedMonth, SelectedYear, context)); Change(); });
                CapacitySortCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortedWarehouseString = ToStringAfterSort(SortedWarehouseToView, "capacity", SelectedMonth, SelectedYear, context); Change(); });
                NumberOfStaffSortCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortedWarehouseString = ToStringAfterSort(SortedWarehouseToView, "numOfEmployee", SelectedMonth, SelectedYear, context); Change(); });
                NumberOfParcelSortCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortedWarehouseString = ToStringAfterSort(SortedWarehouseToView, "numOfParcel", SelectedMonth, SelectedYear, context); Change(); });
                RevenueSortCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortedWarehouseString = ToStringAfterSort(SortedWarehouseToView, "revenue", SelectedMonth, SelectedYear, context); Change(); });
                ResetSortCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortedWarehouseString = ToStringAfterSort(SortedWarehouseToView, "del", SelectedMonth, SelectedYear, context); Change(); });


                CapacitySortAscendingCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortedWarehouseString = Reverse(ToStringAfterSort(SortedWarehouseToView, "capacity",SelectedMonth, SelectedYear, context)); Change(); });
                NumberOfStaffSortAscendingCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortedWarehouseString = Reverse(ToStringAfterSort(SortedWarehouseToView, "numOfEmployee", SelectedMonth, SelectedYear, context)); Change(); });
                NumberOfParcelSortAscendingCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortedWarehouseString = Reverse(ToStringAfterSort(SortedWarehouseToView, "numOfParcel", SelectedMonth, SelectedYear, context)); Change(); });
                RevenueSortAscendingCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortedWarehouseString = Reverse(ToStringAfterSort(SortedWarehouseToView, "revenue", SelectedMonth, SelectedYear, context)); Change(); });
            }
        }

        public void LoadAllWHInfoSearched()
        {
            warehouseList = new List<WarehouseModel>();

            foreach (string row in SortedWarehouseString)
            {
                string[] elements = row.Split(',');

                WarehouseModel warehouseModel = new WarehouseModel
                {
                    STT = warehouseList.Count + 1,
                    WHID = elements[0],
                    Capacity = elements[1],
                    NumParcels = elements[2],
                    Revenue = elements[3],
                    NumStaffs = elements[4]
                };
                if(warehouseModel.WHID.Contains(SearchWHID))
                warehouseList.Add(warehouseModel);
            }
        }
        public void SelectedDateChangedAction(DateTime selectedDate)
        {
            //xem co nen bo tham so SelectedMonth,SelectedYear khong
            // Thực hiện hành động cần thiết khi người dùng chọn ngày
            //Phần statistic phía trên
            SelectedDate = selectedDate;
            SelectedMonth = selectedDate.Month;
            SelectedYear = selectedDate.Year;
            if(SelectedYear > DateTime.Now.Year || (SelectedYear == DateTime.Now.Year && SelectedMonth > DateTime.Now.Month))
                    MessageBoxWindow.Show("Thời gian hiện tại là tháng " + DateTime.Now.Month + ". Vui lòng lựa chọn lại!");
            // Ví dụ: Cập nhật dữ liệu thống kê dựa trên ngày được chọn
            TotalParcel = TotalNumberOfParcel(SelectedMonth,SelectedYear);
            NewCustomer=TotalNumberOfNewCustomer(SelectedMonth,SelectedYear);
            Revenue = TotalRevenue(SelectedMonth,SelectedYear);
            ComparedParcelFigure = PercentageOfParcel(SelectedMonth, SelectedYear, TotalNumberOfParcel(SelectedMonth, SelectedYear));
            ComparedNewCustomerFigure = PercentageOfNewCustomer(SelectedMonth, SelectedYear, TotalNumberOfNewCustomer(SelectedMonth, SelectedYear));
            ComparedRevenueFigure = PercentageOfRevenue(SelectedMonth, SelectedYear, TotalRevenue(SelectedMonth, SelectedYear));
            //phần statistic phía dưới là command
        }
        //set lai view sau moi lan thuc hien command
        void Change()
        {
            warehouseList = new List<WarehouseModel>();

            foreach (string row in SortedWarehouseString)
            {
                string[] elements = row.Split(',');

                WarehouseModel warehouseModel = new WarehouseModel
                {
                    STT = warehouseList.Count + 1,
                    WHID = elements[0],
                    Capacity = elements[1],
                    NumParcels = elements[2],
                    Revenue = elements[3],
                    NumStaffs = elements[4]
                };

                warehouseList.Add(warehouseModel);
            }
        }
 
        public ObservableCollection<string> ToStringAfterSort(ObservableCollection<Warehouse> warehouses, string typeOfSort, int month, int year, PBL3_demoEntities context)
        {
            List<string> warehouseStrings = new List<string>();
            using (var newContext = new PBL3_demoEntities())
            {

                ObservableCollection<Warehouse> warehouseListAfterSort = new ObservableCollection<Warehouse>();
                if (typeOfSort == "del")
                {
                    List<Warehouse> tempWarehouseList = new List<Warehouse>();
                    tempWarehouseList = newContext.Warehouses.Select(x => x).ToList();
                    foreach (var w in tempWarehouseList)
                    {
                        warehouseListAfterSort.Add(w);
                    }
                }
                else if (typeOfSort != "del")
                {
                    warehouseListAfterSort = CallSortWarehouse(warehouses, typeOfSort, month, year, context);
                }
                var query = newContext.Warehouses.Include("Parcels").ToList();
                foreach (var item in warehouseListAfterSort)
                {
                    double totalRevenue = 0.0;
                    int numParcel = query.FirstOrDefault(w => w.warehouseID == item.warehouseID)?.Parcels.Count(p => p.createTime.Month == month && p.createTime.Year == year) ?? 0;

                    double warehouseRevenue = item.Invoices
                        .Where(x => x.startWarehouseID == item.warehouseID && x.outputTime.Month == month && x.outputTime.Year == year) // Lọc các hóa đơn thuộc kho hàng hiện tại và tháng đã chọn
                        .Sum(x => x.shippingFee);// lấy ra giá tiền của từng đơn
                    totalRevenue += warehouseRevenue; // Cộng tổng doanh thu của các đơn vào tổng doanh thu chung của kho hàng hiện tại
                    // tính tổng số nhân viên 
                    var queryR = newContext.Warehouses.Include("Receptionists").ToList();
                    var queryS = newContext.Warehouses.Include("WarehouseStaffs").ToList();
                    var queryM = newContext.Warehouses.Include("WarehouseManagers").ToList();
                    // cộng tổng 3 loại nhân viên của từng kho
                    int totalEmployees =
                    (queryR.FirstOrDefault(c => c.warehouseID == item.warehouseID)?.Receptionists.Count ?? 0) +
                    (queryS.FirstOrDefault(c => c.warehouseID == item.warehouseID)?.WarehouseStaffs.Count ?? 0) +
                    (queryM.FirstOrDefault(c => c.warehouseID == item.warehouseID)?.WarehouseManagers.Count ?? 0);
                    //string resultString = $"WarehouseID: {item.warehouseID}, Capacity: {item.capacity}, Address: {item.warehouseAddress}, NumParcels: {numParcel}, Revenue : {totalRevenue}, TotalEmployees: {totalEmployees}";
                    string resultString = $"{item.warehouseID},{item.capacity},{numParcel},{totalRevenue},{totalEmployees}";

                    warehouseStrings.Add(resultString);

                }
            }

            return new ObservableCollection<string>(warehouseStrings);
        }

        public class WarehouseModel
        {
            public int STT { get; set; }
            public string WHID { get; set; }
            public string Capacity { get; set; }  
            public string NumParcels { get; set; }
            public string Revenue { get; set; }
            public string NumStaffs {get;set; }
            
        }



        public ObservableCollection<string> Reverse(ObservableCollection<string> warehouses)
        {
            var reversedCollection = new ObservableCollection<string>();
            for (int i = warehouses.Count - 1; i >= 0; i--)
            {
                reversedCollection.Add(warehouses[i]);
            }

            return reversedCollection;
        }



        public ObservableCollection<Warehouse> SortWarehouse<T>(ObservableCollection<Warehouse> warehouses, Func<Warehouse, T> keySelector) where T : IComparable<T>
        {
            var sortedWarehouses = warehouses.OrderByDescending(keySelector).ToList();
            return new ObservableCollection<Warehouse>(sortedWarehouses);

        }

        public ObservableCollection<Warehouse> CallSortWarehouse(ObservableCollection<Warehouse> warehouses, string typeOfSort, int month, int year, PBL3_demoEntities context)
        {
            ObservableCollection<Warehouse> tempWarehouses = new ObservableCollection<Warehouse>();
            if (typeOfSort == "ID")
            {
                tempWarehouses = SortWarehouse(warehouses, w => w.warehouseID);
            }
            else if (typeOfSort == "capacity")
            {
                tempWarehouses = SortWarehouse(warehouses, w => w.capacity);
            }
            else if (typeOfSort == "numOfEmployee")
            {
                using (var newContext = new PBL3_demoEntities())
                {
                    var queryR = newContext.Warehouses.Include("Receptionists").ToList();
                    var queryS = newContext.Warehouses.Include("WarehouseStaffs").ToList();
                    var queryM = newContext.Warehouses.Include("WarehouseManagers").ToList();

                    tempWarehouses = SortWarehouse(warehouses, w =>
                        (queryR.FirstOrDefault(c => c.warehouseID == w.warehouseID)?.Receptionists.Count ?? 0) +
                        (queryS.FirstOrDefault(c => c.warehouseID == w.warehouseID)?.WarehouseStaffs.Count ?? 0) +
                        (queryM.FirstOrDefault(c => c.warehouseID == w.warehouseID)?.WarehouseManagers.Count ?? 0));
                }

            }
            else if (typeOfSort == "numOfParcel")
            {
                using (var newContext = new PBL3_demoEntities())
                {
                    var query = newContext.Warehouses.Include("Parcels").ToList();
                    tempWarehouses = SortWarehouse(warehouses, w => query.FirstOrDefault(c => c.warehouseID == w.warehouseID)?.Parcels.Count(p => p.createTime.Month == month && p.createTime.Year == year) ?? 0);
                }
            }
            else if (typeOfSort == "revenue")
            {
                using (var newContext = new PBL3_demoEntities())
                {
                    var query = newContext.Warehouses.Include("Invoices").ToList();
                    tempWarehouses = SortWarehouse(warehouses, w => query.FirstOrDefault(c => c.warehouseID == w.warehouseID)?.Invoices.Where(x => x.outputTime.Month == month && x.outputTime.Year == year).Sum(i => i.shippingFee) ?? 0.0);
                }
            }

            else if (typeOfSort == "none")
            {
                // tempWarehouses.Clear();
                List<Warehouse> tempList = context.Warehouses.Select(x => x).ToList();
                foreach (var item in tempList)
                {
                    tempWarehouses.Add(item);
                }
            }
            warehouses.Clear();
            foreach (Warehouse w in tempWarehouses)
            {
                warehouses.Add(w);
            }
            return warehouses;
        }

        //tam done binding
        public string PercentageTextOutput(decimal percentage)
        {

            // Tạo chuỗi kết quả
            string result;
            if (percentage > 0)
            {
                result = string.Format("Tăng {0}% so với tháng trước.", Math.Abs(percentage));
            }
            else if (percentage < 0)
            {
                result = string.Format("Giảm {0}% so với tháng trước.", Math.Abs(percentage));
            }
            else
            {
                result = "Không có sự thay đổi so với tháng trước.";
            }

            return result;
        }


        public int TotalNumberOfParcel(int month,int year)
        {
            month = SelectedMonth;
            year = SelectedYear;
            int numberOfParcel;
            using (var context = new PBL3_demoEntities())
            {

                numberOfParcel = context.Parcels.Count(x => x.createTime.Month == month && x.createTime.Year == year);
            }
            return numberOfParcel;
        }

        public string PercentageOfParcel(int month, int year, int numberOfParcel)
        {
            // tính toán thời gian tháng trước
            int previousMonth = month - 1;
            int previousYear = year;
            if (month == 1)
            {
                previousMonth = 12;
                previousYear = year - 1;

            }

            // Lấy số lượng đơn hàng của tháng trước đó
            using (var context = new PBL3_demoEntities())
            {
                int previousNumberOfParcel = TotalNumberOfParcel(previousMonth, previousYear);
                if (previousNumberOfParcel == 0)
                {
                    return "N/A"; // Trả về giá trị "N/A" nếu không có tổng đơn hàng của tháng trước
                }
                // Tính phần trăm tăng hoặc giảm
                decimal percentage = ((decimal)numberOfParcel - previousNumberOfParcel) / previousNumberOfParcel * 100;
                decimal roundedPercentage = decimal.Round(percentage, 3);
                return PercentageTextOutput(roundedPercentage);

            }
        }


        public int TotalNumberOfNewCustomer(int month, int year)
        {
            
            int numberOfNewCustomer;
            using (var context = new PBL3_demoEntities())
            {
                numberOfNewCustomer = context.Customers.Count(x => x.joinTime.Month == month && x.joinTime.Year == year);
            }
            return numberOfNewCustomer;
        }

        public string PercentageOfNewCustomer(int month, int year, int numberOfNewCustomer)
        {
            // tính toán thời gian tháng trước
            int previousMonth = month - 1;
            int previousYear = year;
            if (month == 1)
            {
                previousMonth = 12;
                previousYear = year - 1;

            }
            // Lấy số lượng đơn hàng của tháng trước đó
            using (var context = new PBL3_demoEntities())
            {
                int previousNumberOfNewCustomer = TotalNumberOfNewCustomer(previousMonth, previousYear);
                if (previousNumberOfNewCustomer == 0)
                {
                    return "N/A"; // Trả về giá trị "N/A" nếu không có khách hàng mới của tháng trước
                }
                // Tính phần trăm tăng hoặc giảm
                decimal percentage = ((decimal)numberOfNewCustomer - previousNumberOfNewCustomer) / previousNumberOfNewCustomer * 100;
                decimal roundedPercentage = decimal.Round(percentage, 3);
                return PercentageTextOutput(roundedPercentage);
            }
        }

        public double TotalRevenue(int month, int year)
        {
            double revenue = 0.0;
            using (var context = new PBL3_demoEntities())
            {
                revenue = context.Invoices
                    .Where(x => x.outputTime.Month == month && x.outputTime.Year == year) // Lọc các hóa đơn có outputTime trùng với month
                    .Sum(x => (double?)x.shippingFee) ?? 0.0; // Tính tổng giá trị shippingFee của các hóa đơn và sử dụng giá trị mặc định 0.0 nếu giá trị là null

            }
            return revenue;
        }

        public string PercentageOfRevenue(int month, int year, double revenue)
        {
            // tính toán thời gian tháng trước
            int previousMonth = month - 1;
            int previousYear = year;
            if (month == 1)
            {
                previousMonth = 12;
                previousYear = year - 1;

            }
            // Lấy số lượng đơn hàng của tháng trước đó
            using (var context = new PBL3_demoEntities())
            {
                double previousRevenue = TotalRevenue(previousMonth, previousYear);
                if (previousRevenue == 0.0)
                {
                    return "N/A"; // Trả về giá trị "N/A" nếu không có doanh thu tháng trước
                }
                // Tính phần trăm tăng hoặc giảm
                decimal percentage = ((decimal)revenue - (decimal)previousRevenue) / (decimal)previousRevenue * 100;
                decimal roundedPercentage = decimal.Round(percentage, 3);
                return PercentageTextOutput(roundedPercentage);
            }
        }

    }
}
