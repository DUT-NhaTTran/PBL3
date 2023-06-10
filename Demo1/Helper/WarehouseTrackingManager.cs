using Demo1.DTO;
using Demo1.Model;
using Demo1.View;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Demo1.ViewModel.WarehouseTrackingModel;

namespace Demo1.Model
{
    public class WarehouseTrackingManager
    {
        public WarehouseTrackingManager()
        {
            WarehouseInfoList = new ObservableCollection<WarehouseInfo>();
            ParcelInfoList= new ObservableCollection<ParcelInfo>();

        }
        public ObservableCollection<ParcelInfo> ParcelInfoList { get; set; }
        public ObservableCollection<ParcelInfo> LoadParcelInfoList(string WHID)
        {
            using (var dbContext = new Model.PBL3_demoEntities())
            {
                var parcels = dbContext.Parcels
                    .Where(p => p.currentWarehouseID == WHID)
                    .Select(p => new ParcelInfo
                    {
                        ParcelName = p.parcelName,
                        ID = p.parcelID,
                        Mass = p.parcelMass
                    })
                    .ToList();

                ParcelInfoList.Clear();

                foreach (var parcel in parcels)
                {
                    ParcelInfoList.Add(parcel);
                }
            }
            return ParcelInfoList;  
        }
        public string SetNumberOfWarehouseFiterd(string str)
        {
            ObservableCollection<string> warehouseIDs = FindRegionOfWarehouses(str);
            int count = 0;

            using (var context = new Model.PBL3_demoEntities())
            {
                foreach (string warehouseID in warehouseIDs)
                {
                    var query = context.Warehouses
                        .Where(w => w.warehouseID == warehouseID)
                        .Count();

                    count += query;
                    //giai thich nghe
                }
            }

            return count.ToString();
        }
        string GetProvinceFromWarehouseName(string warehouseName)
        {
            string provinceName = warehouseName.Replace("Kho ", "");
            return provinceName;
        }
        public ObservableCollection<WarehouseInfo> WarehouseInfoList { get; set; }
       
        public ObservableCollection<WarehouseInfo> GetWarehouseSearched(string str)
        {
            using (var context = new Model.PBL3_demoEntities())
            {
                // Xóa kết quả tìm kiếm cũ
                WarehouseInfoList.Clear();

                var warehouses = context.Warehouses
                    .Where(w => w.warehouseName.Contains(str) || w.warehouseID.Contains(str))
                    .ToList();

                if (warehouses.Any())
                {
                    foreach (var warehouse in warehouses)
                    {
                        var warehouseInfo = new WarehouseInfo
                        {
                            WarehouseID = warehouse.warehouseID,
                            WarehouseName = warehouse.warehouseName,
                            Capacity = warehouse.capacity,
                            ColumnData = SetColumnData(warehouse.warehouseID)
                        };
                        WarehouseInfoList.Add(warehouseInfo);
                    }
                }
                else
                {
                    MessageBoxWindow.Show("Không tìm thấy thông tin kho");
                }
               
            }
            return WarehouseInfoList;
        }
        public ChartValues<double> SetColumnData(string WHID)
        {
            ChartValues<double> columnData = new ChartValues<double>();

            using (var context = new PBL3_demoEntities())
            {
                var warehouse = context.Warehouses.FirstOrDefault(w => w.warehouseID == WHID);
                if (warehouse != null)
                {
                    int capacity = warehouse.capacity;
                    var parcels = context.Parcels.Where(p => p.currentWarehouseID == WHID).ToList();
                    int count = parcels.Count();
                    double usedCapacity = (double)count / capacity * 100;

                    columnData.Add(usedCapacity / 100);
                }
            }

            return columnData;
        }


        public ObservableCollection<WarehouseInfo> GetWarehouseFiltered(string str)
        {
            ObservableCollection<string> warehouseIDs = FindRegionOfWarehouses(str);

            using (var context = new Model.PBL3_demoEntities())
            {
                WarehouseInfoList.Clear(); // Clear the old elements in WarehouseInfoList

                foreach (string warehouseID in warehouseIDs)
                {
                    var query = context.Warehouses
                        .Where(w => w.warehouseID == warehouseID)
                        .Select(w => new WarehouseInfo
                        {
                            WarehouseID = w.warehouseID,
                            WarehouseName = w.warehouseName,
                            Capacity = w.capacity
                        })
                        .FirstOrDefault();

                    if (query != null)
                    {
                        query.ColumnData = SetColumnData(warehouseID);
                        WarehouseInfoList.Add(query);
                    }
                }
            }
            return WarehouseInfoList;
        }
        Dictionary<string, string> provinceRegions = DictionaryData.ProvinceRegions;
        public ObservableCollection<string> FindRegionOfWarehouses(string region)
        {
            ObservableCollection<string> warehousesOfThisRegionCollection = new ObservableCollection<string>();
            using (var context = new PBL3_demoEntities())
            {
                List<string> warehouseAddresses = context.Warehouses.Select(x => x.warehouseName).ToList();
                List<string> warehousesInThisRegion = new List<string>();
                if (region != "All")
                {
                    warehousesInThisRegion = warehouseAddresses.Where(address =>
                    {
                        string province = GetProvinceFromWarehouseName(address);
                        return provinceRegions.ContainsKey(province) && provinceRegions[province] == region;
                    }).ToList();
                }
                else
                {
                    warehousesInThisRegion = context.Warehouses.Select(x => x.warehouseName).ToList();
                }
                foreach (string warehouse in warehousesInThisRegion)
                {

                    warehousesOfThisRegionCollection.Add(context.Warehouses.Where(x => x.warehouseName == warehouse).Select(x => x.warehouseID).FirstOrDefault());
                }
            }
            return warehousesOfThisRegionCollection;
        }
        public string FindContained(string WHID)
        {
            double res = 0;
            int capacity = 0;
            int count = 0;
            //lay suc chua cua kho i
            using (var context1 = new PBL3_demoEntities())
            {
                var Capacity = context1.Warehouses.FirstOrDefault(x => x.warehouseID == WHID);
                if (Capacity != null)
                {
                    capacity = Capacity.capacity;
                }
            }
            //dem don dang trong kho i
            using (var context = new PBL3_demoEntities())
            {
                var Count = context.Parcels.Count(o => o.currentWarehouseID == WHID);
                count = Count;
            }
            res = (double)count / capacity;
            double roundedResult = Math.Round(res * 100, 3);
            return roundedResult.ToString() + "%";
        }

    }
}
