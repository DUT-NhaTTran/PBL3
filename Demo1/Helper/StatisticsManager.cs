using Demo1.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo1.Model
{
    public class StatisticsManager
    {
        public ObservableCollection<Warehouse> Warehouses { get; set; }

        public StatisticsManager() { }
        public int TotalNumberOfNewCustomer(int month, int year)
        {
            int numberOfNewCustomer;

            using (var context = new PBL3_demoEntities())
            {
                numberOfNewCustomer = context.Customers.Count(x => x.joinTime.Month == month && x.joinTime.Year == year);
            }

            return numberOfNewCustomer;
        }

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
        public double TotalRevenue(int month, int year)
        {
            double revenue = 0.0;

            using (var context = new PBL3_demoEntities())
            {
                revenue = context.Invoices
                    .Where(x => x.outputTime.Month == month && x.outputTime.Year == year)
                    .Sum(x => (double?)x.shippingFee) ?? 0.0;
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

            // Lấy doanh thu của tháng trước đó
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

            // Lấy tổng số lượng đơn hàng của tháng trước đó
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
        public int TotalNumberOfParcel(int month, int year)
        {
            int numberOfParcel;
            using (var context = new PBL3_demoEntities())
            {
                numberOfParcel = context.Parcels.Count(x => x.createTime.Month == month && x.createTime.Year == year);
            }
            return numberOfParcel;
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
                tempWarehouses = SortWarehouse(warehouses, w => CalculateTotalEmployees(w, context));
            }
            else if (typeOfSort == "numOfParcel")
            {
                tempWarehouses = SortWarehouse(warehouses, w => CalculateNumOfParcels(w, month, year, context));
            }
            else if (typeOfSort == "revenue")
            {
                tempWarehouses = SortWarehouse(warehouses, w => CalculateRevenue(w, month, year, context));
            }
            else if (typeOfSort == "none")
            {
                List<Warehouse> tempList = context.Warehouses.ToList();
                tempWarehouses = new ObservableCollection<Warehouse>(tempList);
            }

            return tempWarehouses;
        }

        private int CalculateTotalEmployees(Warehouse warehouse, PBL3_demoEntities context)
        {
            int totalEmployees =
                (warehouse.Receptionists?.Count ?? 0) +
                (warehouse.WarehouseStaffs?.Count ?? 0) +
                (warehouse.WarehouseManagers?.Count ?? 0);

            return totalEmployees;
        }

        private int CalculateNumOfParcels(Warehouse warehouse, int month, int year, PBL3_demoEntities context)
        {
            int numParcel = warehouse.Parcels.Count(p => p.createTime.Month == month && p.createTime.Year == year);
            return numParcel;
        }

        private double CalculateRevenue(Warehouse warehouse, int month, int year, PBL3_demoEntities context)
        {
            double warehouseRevenue = warehouse.Invoices
                .Where(x => x.startWarehouseID == warehouse.warehouseID && x.outputTime.Month == month && x.outputTime.Year == year)
                .Sum(x => x.shippingFee);
            return warehouseRevenue;
        }
        public ObservableCollection<string> ResetWarehouse(ObservableCollection<Warehouse> warehouses, int month, int year, PBL3_demoEntities context)
        {
            using (var newContext = new PBL3_demoEntities())
            {
                ObservableCollection<Warehouse> warehouseCollection = new ObservableCollection<Warehouse>();
                List<string> warehouseStrings = new List<string>();
                List<Warehouse> tempWarehouseList = newContext.Warehouses.Select(x => x).ToList();
                foreach (var w in tempWarehouseList)
                {
                    warehouseCollection.Add(w);
                }
                foreach (var item in warehouseCollection)
                {
                    warehouseStrings.Add(EachWarehouseStringToView(item, month, year, context));
                }

                return new ObservableCollection<string>(warehouseStrings);
            }
        }



        public ObservableCollection<string> ToStringAfterSort(ObservableCollection<Warehouse> warehouses, string typeOfSort, int month, int year, PBL3_demoEntities context)
        {
            List<string> warehouseStrings = new List<string>();

            ObservableCollection<Warehouse> warehouseListAfterSort = new ObservableCollection<Warehouse>();
            if (typeOfSort == "del")
            {
                List<Warehouse> tempWarehouseList = context.Warehouses.Select(x => x).ToList();
                foreach (var w in tempWarehouseList)
                {
                    warehouseListAfterSort.Add(w);
                }
            }
            else if (typeOfSort != "del")
            {
                warehouseListAfterSort = CallSortWarehouse(warehouses, typeOfSort, month, year, context);
            }

            foreach (var item in warehouseListAfterSort)
            {
                warehouseStrings.Add(EachWarehouseStringToView(item, month, year, context));
            }

            return new ObservableCollection<string>(warehouseStrings);
        }



        public string EachWarehouseStringToView(Warehouse item, int month, int year, PBL3_demoEntities context)
        {
            double totalRevenue = 0.0;
            int numParcel = item.Parcels.Count(p => p.createTime.Month == month && p.createTime.Year == year);

            double warehouseRevenue = item.Invoices
                .Where(x => x.startWarehouseID == item.warehouseID && x.outputTime.Month == month && x.outputTime.Year == year)
                .Sum(x => x.shippingFee);
            totalRevenue += warehouseRevenue;

            int totalEmployees =
                (item.Receptionists?.Count ?? 0) +
                (item.WarehouseStaffs?.Count ?? 0) +
                (item.WarehouseManagers?.Count ?? 0);

            string resultString = $"{item.warehouseID},{item.capacity},{numParcel},{totalRevenue},{totalEmployees}";
            return resultString;
        }

    }
}
