using Demo1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo1.UserInfo
{
    public class WarehouseData
    {
        private static WarehouseData instance;

        private WarehouseData() { }
        public static WarehouseData Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WarehouseData();
                }
                return instance;
            }
        }
        public Warehouse GetWarehouseRecord(string WarehouseID)
        {
            
            using (var context = new PBL3_demoEntities())
            {
                var thisWH = context.Warehouses.Where(x => x.warehouseID == WarehouseID).FirstOrDefault();
                if (thisWH != null)
                {
                    return thisWH;
                }
                else
                {
                    return null;
                }

            }
        }
        public string GetWarehouseAddress(string WarehouseID)
        {
            Warehouse thisWH = GetWarehouseRecord(WarehouseID);
            if (thisWH != null) return thisWH.warehouseAddress;
            else return "";
        }
        public int GetWarehouseCapacity(string WarehouseID)
        {

            Warehouse thisWH = GetWarehouseRecord(WarehouseID);
            if (thisWH != null) return thisWH.capacity;
            else return -1;
        }
    }
}
