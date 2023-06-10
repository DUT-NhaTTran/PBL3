using Demo1.Model;
using Demo1.ViewModel;
using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static Demo1.ViewModel.SearchParcelModel;
using System.Windows.Media;
using Demo1.View;
using System.Runtime.Remoting.Contexts;


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
        private int _TotalParcel;
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
            set { _Revenue = value; OnPropertyChanged(); }
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
        //properties cho chart
        private ChartValues<double> revenueChartData;
        public ChartValues<double> RevenueChartData
        {
            get { return revenueChartData; }
            set
            {
                revenueChartData = value;
                OnPropertyChanged();
            }
        }
        private ChartValues<int> totalParcelChartData;
        public ChartValues<int> TotalParcelChartData
        {
            get { return totalParcelChartData; }
            set
            {
                totalParcelChartData = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<string> labelsData;
        public ObservableCollection<string> LabelsData
        {
            get { return labelsData; }
            set
            {
                labelsData = value;
                OnPropertyChanged();
            }
        }
        private PBL3_demoEntities context;
        public StatisticModel()

        {
            context = new PBL3_demoEntities();
            SelectedDate = DateTime.Now;
            SortedWarehouseToView = new ObservableCollection<Warehouse>();
            using (var context = new PBL3_demoEntities())
            {
                var warehouseList = context.Warehouses.Select(x => x).ToList();
                foreach (var w in warehouseList)
                {
                    SortedWarehouseToView.Add(w);
                }

                SortedWarehouseString = new ObservableCollection<string>();
                SortedWarehouseString = ToStringAfterSort(SortedWarehouseToView, "none", SelectedMonth, SelectedYear, context);
                IDSortCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortedWarehouseString = ToStringAfterSort(SortedWarehouseToView, "ID", SelectedMonth, SelectedYear, context); Change(); });

                IDSortAscendingCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortedWarehouseString = Reverse(ToStringAfterSort(SortedWarehouseToView, "ID", SelectedMonth, SelectedYear, context)); Change(); });
                CapacitySortCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortedWarehouseString = ToStringAfterSort(SortedWarehouseToView, "capacity", SelectedMonth, SelectedYear, context); Change(); });
                NumberOfStaffSortCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortedWarehouseString = ToStringAfterSort(SortedWarehouseToView, "numOfEmployee", SelectedMonth, SelectedYear, context); Change(); });
                NumberOfParcelSortCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortedWarehouseString = ToStringAfterSort(SortedWarehouseToView, "numOfParcel", SelectedMonth, SelectedYear, context); Change(); });
                RevenueSortCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortedWarehouseString = ToStringAfterSort(SortedWarehouseToView, "revenue", SelectedMonth, SelectedYear, context); Change(); });
                ResetSortCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortedWarehouseString = ResetWarehouse(SortedWarehouseToView, SelectedMonth, SelectedYear, context); Change(); });


                CapacitySortAscendingCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortedWarehouseString = Reverse(ToStringAfterSort(SortedWarehouseToView, "capacity", SelectedMonth, SelectedYear, context)); Change(); });
                NumberOfStaffSortAscendingCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortedWarehouseString = Reverse(ToStringAfterSort(SortedWarehouseToView, "numOfEmployee", SelectedMonth, SelectedYear, context)); Change(); });
                NumberOfParcelSortAscendingCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortedWarehouseString = Reverse(ToStringAfterSort(SortedWarehouseToView, "numOfParcel", SelectedMonth, SelectedYear, context)); Change(); });
                RevenueSortAscendingCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortedWarehouseString = Reverse(ToStringAfterSort(SortedWarehouseToView, "revenue", SelectedMonth, SelectedYear, context)); Change(); });
            }
        }

        private void LoadObservationData()
        {
            ObservableCollection<int> totalParcelData = CalcNumOfParcelsData(SelectedMonth, SelectedYear);
            ObservableCollection<double> revenueData = CalcRevenueData(SelectedMonth, SelectedYear);
            TotalParcelChartData = new ChartValues<int>(totalParcelData);
            RevenueChartData = new ChartValues<double>(revenueData);


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
                if (warehouseModel.WHID.Contains(SearchWHID))
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
            if (SelectedYear > DateTime.Now.Year || (SelectedYear == DateTime.Now.Year && SelectedMonth > DateTime.Now.Month))
                MessageBoxWindow.Show("Thời gian hiện tại là tháng " + DateTime.Now.Month + ". Vui lòng lựa chọn lại!");


            //chart
            LabelsData = CalcDateTime(SelectedMonth, SelectedYear);
            LoadObservationData();


            // Ví dụ: Cập nhật dữ liệu thống kê dựa trên ngày được chọn
            TotalParcel = TotalNumberOfParcel(SelectedMonth, SelectedYear);
            NewCustomer = TotalNumberOfNewCustomer(SelectedMonth, SelectedYear);
            Revenue = TotalRevenue(SelectedMonth, SelectedYear);
            ComparedParcelFigure = PercentageOfParcel(SelectedMonth, SelectedYear, TotalNumberOfParcel(SelectedMonth, SelectedYear));
            ComparedNewCustomerFigure = PercentageOfNewCustomer(SelectedMonth, SelectedYear, TotalNumberOfNewCustomer(SelectedMonth, SelectedYear));
            ComparedRevenueFigure = PercentageOfRevenue(SelectedMonth, SelectedYear, TotalRevenue(SelectedMonth, SelectedYear));


            //phần statistic phía dưới là command
        }
        //set lai view sau moi lan thuc hien command
        public void Change()
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

            StatisticsManager SM = new StatisticsManager();
            return SM.ToStringAfterSort(warehouses, typeOfSort, month, year, context);
        }

        public ObservableCollection<string> ResetWarehouse(ObservableCollection<Warehouse> warehouses, int month, int year, PBL3_demoEntities context)
        {
            SearchWHID = "";
            StatisticsManager SM = new StatisticsManager();
            return SM.ResetWarehouse(warehouses, month, year, context);
        }

        public class WarehouseModel
        {
            public int STT { get; set; }
            public string WHID { get; set; }
            public string Capacity { get; set; }
            public string NumParcels { get; set; }
            public string Revenue { get; set; }
            public string NumStaffs { get; set; }

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

            StatisticsManager SM = new StatisticsManager();
            return SM.CallSortWarehouse(warehouses, typeOfSort, month, year, context);
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

        public int TotalNumberOfParcel(int month, int year)
        {
            StatisticsManager SM = new StatisticsManager();
            return SM.TotalNumberOfParcel(month, year);
        }

        public string PercentageOfParcel(int month, int year, int numberOfParcel)
        {
            StatisticsManager SM = new StatisticsManager();
            return SM.PercentageOfParcel(month, year, numberOfParcel);
        }


        public int TotalNumberOfNewCustomer(int month, int year)
        {
            StatisticsManager SM = new StatisticsManager();
            return SM.TotalNumberOfNewCustomer(month, year);
        }

        public string PercentageOfNewCustomer(int month, int year, int numberOfNewCustomer)
        {
            StatisticsManager SM = new StatisticsManager();
            return SM.PercentageOfNewCustomer(month, year, numberOfNewCustomer);
        }

        public double TotalRevenue(int month, int year)
        {
            StatisticsManager SM = new StatisticsManager();
            return SM.TotalRevenue(month, year);
        }

        public string PercentageOfRevenue(int month, int year, double revenue)
        {
            StatisticsManager SM = new StatisticsManager();
            return SM.PercentageOfRevenue(month, year, revenue);
        }
        /// các hàm phục vụ cho Chart
        /// 
        ObservableCollection<string> CalcDateTime(int month, int year)
        {
            ObservableCollection<string> dateTime = new ObservableCollection<string>();
            for (int i = 0; i < 12; i++)
            {
                dateTime.Add(month.ToString() + "/" + year.ToString());
                if (month == 1)
                {
                    year -= 1;
                    month = 12;
                }
                else
                {
                    month--;
                }
            }

            return new ObservableCollection<string>(dateTime.Reverse());
        }
        ObservableCollection<double> CalcRevenueData(int month, int year)
        {
            ObservableCollection<double> revenueData = new ObservableCollection<double>();
            ObservableCollection<string> dateTime = CalcDateTime(month, year);
            foreach (string date in dateTime)
            {
                int iMonth = month, iYear = year;
                string[] parts = date.Split('/'); // Phân tách chuỗi thành các phần tử
                if (parts.Length == 2) // Đảm bảo rằng có đủ 2  phần tử 
                {
                    if (int.TryParse(parts[0], out int tMonth))
                    {
                        iMonth = tMonth; // Gán số đầu tiên cho rMonth
                    }

                    if (int.TryParse(parts[1], out int tYear))
                    {
                        iYear = tYear; // Gán số thứ hai cho rYear
                    }
                    revenueData.Add(TotalRevenue(iMonth, iYear));
                }

            }
            return revenueData;
        }

        ObservableCollection<int> CalcNumOfParcelsData(int month, int year)
        {
            ObservableCollection<int> numOfParcelsData = new ObservableCollection<int>();
            ObservableCollection<string> dateTime = CalcDateTime(month, year);
            foreach (string date in dateTime)
            {
                int iMonth = month;
                int iYear = year;
                string[] parts = date.Split('/'); // Phân tách chuỗi thành các phần tử
                if (parts.Length == 2) // Đảm bảo rằng có đủ 2  phần tử 
                {
                    if (int.TryParse(parts[0], out int tMonth))
                    {
                        iMonth = tMonth; // Gán số đầu tiên cho rMonth
                    }

                    if (int.TryParse(parts[1], out int tYear))
                    {
                        iYear = tYear; // Gán số thứ hai cho rYear
                    }
                    numOfParcelsData.Add(TotalNumberOfParcel(iMonth, iYear));
                }

            }
            return numOfParcelsData;
        }


    }
}