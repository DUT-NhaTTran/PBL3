using Demo1.Model;
using Demo1.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Demo1.ViewModel
{
    public class StatisticModel : BaseViewModel
    {
        private string password;
        private string accountName;
        private string accoutWarehouse;
        private string parcelID;
        private string warehouseID;

        private ObservableCollection<string> satisfiedWarehouses;


        private ObservableCollection<string> routeCollection;
        private ObservableCollection<Parcel> parcelCollection;

        private ObservableCollection<Warehouse> sortedWarehouseToView;
        private ObservableCollection<string> sortedWarehouseString;
        public string Password
        {
            get { return password; }
            set
            {
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged();
                }
            }
        }


        public string AccountName
        {
            get { return accountName; }
            set
            {
                if (accountName != value)
                {
                    accountName = value;
                    OnPropertyChanged();
                }
            }
        }
        // this property below not is Login , but I for temporary use
        public string AccountWarehouse
        {
            get { return accoutWarehouse; }
            set
            {
                if (accoutWarehouse != value)
                {
                    accoutWarehouse = value;
                    OnPropertyChanged();
                }
            }
        }



        public string ParcelID
        {
            get { return parcelID; }
            set
            {
                if (parcelID != value)
                {
                    parcelID = value;
                    OnPropertyChanged();
                    ValidateParcelID();
                }
            }
        }


        public ObservableCollection<string> RouteCollection
        {
            get { return routeCollection; }
            set
            {
                routeCollection = value;
                OnPropertyChanged();
            }
        }


        public string WarehouseID
        {
            get { return warehouseID; }
            set
            {
                if (warehouseID != value)
                {
                    warehouseID = value;
                    OnPropertyChanged();
                }
            }
        }


        public ObservableCollection<Parcel> ParcelCollection
        {
            get { return parcelCollection; }
            set
            {
                parcelCollection = value;
                OnPropertyChanged();
            }
        }



        public ObservableCollection<string> SatisfiedWarehouses
        {
            get { return satisfiedWarehouses; }
            set
            {
                satisfiedWarehouses = value;
                OnPropertyChanged();
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


        void ValidateParcelID()
        {
            int parcelID = int.Parse(ParcelID);
            using (var context = new PBL3_demoEntities())
            {
                var thisParcel = context.Parcels.Where(x => x.parcelID == parcelID).FirstOrDefault();
                if (thisParcel == null)
                {
                    MessageBox.Show("Đơn này không tồn tại trong hệ thống hoặc không hợp lệ");
                }
            }
        }
        public ICommand IDSortCommand { get; set; }
        public ICommand CapacitySortCommand { get; set; }
        public ICommand NumberOfStaffSortCommand { get; set; }

        public ICommand NumberOfParcelSortCommand { get; set; }
        public ICommand RevenueSortCommand { get; set; }
        public ICommand ResetSortCommand { get; set; }

        //
        public ICommand IDSortDescendingCommand { get; set; }
        public ICommand CapacitySortDescendingCommand { get; set; }
        public ICommand NumberOfStaffSortDescendingCommand { get; set; }

        public ICommand NumberOfParcelSortDescendingCommand { get; set; }
        public ICommand RevenueSortDescendingCommand { get; set; }
        public StatisticModel()
        {
            SortedWarehouseToView = new ObservableCollection<Warehouse>();
            using (var context = new PBL3_demoEntities())
            {
                var warehouseList = context.Warehouses.Select(x => x).ToList();
                foreach (var w in warehouseList)
                {
                    SortedWarehouseToView.Add(w);
                }
                SortedWarehouseString = new ObservableCollection<string>();
                SortedWarehouseString = ToStringAfterSort(SortedWarehouseToView, "none", context);

                IDSortCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortedWarehouseString = ToStringAfterSort(SortedWarehouseToView, "ID", context); });
                CapacitySortCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortedWarehouseString = ToStringAfterSort(SortedWarehouseToView, "capacity", context); });
                NumberOfStaffSortCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortedWarehouseString = ToStringAfterSort(SortedWarehouseToView, "numOfEmployee", context); });
                NumberOfParcelSortCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortedWarehouseString = ToStringAfterSort(SortedWarehouseToView, "numOfParcel", context); });
                RevenueSortCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortedWarehouseString = ToStringAfterSort(SortedWarehouseToView, "revenue", context); });
                ResetSortCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortedWarehouseString = ToStringAfterSort(SortedWarehouseToView, "del", context); });
                //
                IDSortDescendingCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortedWarehouseString = Reverse(ToStringAfterSort(SortedWarehouseToView, "ID", context)); });
                CapacitySortDescendingCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortedWarehouseString = Reverse(ToStringAfterSort(SortedWarehouseToView, "capacity", context)); });
                NumberOfParcelSortDescendingCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortedWarehouseString = Reverse(ToStringAfterSort(SortedWarehouseToView, "numOfEmployee", context)); });
                NumberOfParcelSortDescendingCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortedWarehouseString = Reverse(ToStringAfterSort(SortedWarehouseToView, "numOfParcel", context)); });
                RevenueSortDescendingCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SortedWarehouseString = Reverse(ToStringAfterSort(SortedWarehouseToView, "revenue", context)); });
            }



        }
        public ObservableCollection<string> ToStringAfterSort(ObservableCollection<Warehouse> warehouses, string typeOfSort, PBL3_demoEntities context)
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
                    warehouseListAfterSort = CallSortWarehouse(warehouses, typeOfSort, context);
                }
                var query = newContext.Warehouses.Include("Parcels").ToList();
                foreach (var item in warehouseListAfterSort)
                {
                    double totalRevenue = 0.0;
                    int numParcel = query.FirstOrDefault(w => w.warehouseID == item.warehouseID)?.Parcels.Count ?? 0;

                    double warehouseRevenue = item.Invoices
                        .Where(x => x.startWarehouseID == item.warehouseID) // Lọc các hóa đơn thuộc kho hàng hiện tại
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
                    string resultString = $"WarehouseID: {item.warehouseID}, Capacity: {item.capacity}, Address: {item.warehouseAddress}, NumParcels: {numParcel}, Revenue : {totalRevenue}, TotalEmployees: {totalEmployees}";

                    warehouseStrings.Add(resultString);

                }
            }

            return new ObservableCollection<string>(warehouseStrings);
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








        public ObservableCollection<Warehouse> SortWarehouse<T>(ObservableCollection<Warehouse> warehouses, Func<Warehouse, T> keySelector, PBL3_demoEntities context) where T : IComparable<T>
        {
            var sortedWarehouses = warehouses.OrderByDescending(keySelector).ToList();
            return new ObservableCollection<Warehouse>(sortedWarehouses);
        }


        public ObservableCollection<Warehouse> CallSortWarehouse(ObservableCollection<Warehouse> warehouses, string typeOfSort, PBL3_demoEntities context)
        {
            ObservableCollection<Warehouse> tempWarehouses = new ObservableCollection<Warehouse>();
            if (typeOfSort == "ID")
            {
                tempWarehouses = SortWarehouse(warehouses, w => w.warehouseID, context);
            }
            else if (typeOfSort == "capacity")
            {
                tempWarehouses = SortWarehouse(warehouses, w => w.capacity, context);
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
                        (queryM.FirstOrDefault(c => c.warehouseID == w.warehouseID)?.WarehouseManagers.Count ?? 0),
                        context);
                }

            }
            else if (typeOfSort == "numOfParcel")
            {
                using (var newContext = new PBL3_demoEntities())
                {
                    var query = newContext.Warehouses.Include("Parcels").ToList();
                    tempWarehouses = SortWarehouse(warehouses, w => query.FirstOrDefault(c => c.warehouseID == w.warehouseID)?.Parcels.Count ?? 0, context);
                }
            }
            else if (typeOfSort == "revenue")
            {
                using (var newContext = new PBL3_demoEntities())
                {
                    var query = newContext.Warehouses.Include("Invoices").ToList();
                    tempWarehouses = SortWarehouse(warehouses, w => query.FirstOrDefault(c => c.warehouseID == w.warehouseID)?.Invoices.Sum(i => i.shippingFee) ?? 0.0, context);
                }
            }

            else
            {
                tempWarehouses.Clear();
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

    }
}
