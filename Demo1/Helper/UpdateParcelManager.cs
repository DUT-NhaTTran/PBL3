using Demo1.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo1.Model
{
    public class UpdateParcelManager
    {
        //public UpdateParcelManager() { }

        public Route GetRouteFromParcelID(int parcelID)
        {
            using (var context = new PBL3_demoEntities())
            {
                var route = context.Routes.FirstOrDefault(p => p.parcelID == parcelID);
                return route;
            }
        }

        public string GetWarehouseIDFromParcelID(int parcelID)
        {
            using (var context = new PBL3_demoEntities())
            {
                var warehouseIDOfParcel =
                      context.Parcels.Where(x => x.parcelID == parcelID).Select(x => x.currentWarehouseID)
                        .FirstOrDefault();
                return warehouseIDOfParcel;
            }
        }

        public string GetLastStatus(Route thisRoute, int parcelID)
        {
            using (var context = new PBL3_demoEntities())
            {
                string lastStatus = null;
                //get the last ID of this route
                int maxId = context.Routes
                .Where(x => x.parcelID == parcelID)
                .Max(x => x.routeID);

                // use maxID here to get the last Status of this Parcel in Route
                var lastRoute = context.Routes
                    .FirstOrDefault(x => x.routeID == maxId);
                if (lastRoute != null)
                {
                    lastStatus = lastRoute.details.ToString();
                }
                return lastStatus;
            }
        }

        public string GetRLocationFromParcelID(Parcel thisParcel, int parcelID)
        {
            using (var context = new PBL3_demoEntities())
            {
                string thisReceiverCustomerILocation = context.Customers
                 .Where(x => x.customerID == thisParcel.RCustomerID).Select(x => x.customerLocation)
                 .FirstOrDefault();
                return thisReceiverCustomerILocation;
            }
        }

        public string GetWarehouseNameFromWHID(string thisWarehouseID)
        {
            using (var context = new PBL3_demoEntities())
            {
                string thisWarehouseName = context.Warehouses.Where(x => x.warehouseID == thisWarehouseID)
                .Select(x => x.warehouseName).FirstOrDefault();
                return thisWarehouseName;
            }
        }

        public void AddExportToRoute(int parcelID, string thisWarehouseID, string thisWarehouseName)
        {
            using (var context = new PBL3_demoEntities())
            {
                var thisParcel = context.Parcels.Where(x => x.parcelID == parcelID).FirstOrDefault();
                string details = "Xuất đơn hàng ra khỏi " + thisWarehouseName;
                var newRoute = new Route
                {
                    parcelID = parcelID,
                    relatedWarehouseID = thisWarehouseID,
                    details = details,
                    time = DateTime.Now,
                };
                context.Routes.Add(newRoute);
                context.SaveChanges();
            }
        }
        public void AddFailToRoute(int parcelID, string thisWarehouseID)
        {
            using (var context = new PBL3_demoEntities())
            {
                var thisParcel = context.Parcels.Where(x => x.parcelID == parcelID).FirstOrDefault();
                string details = "Đơn hàng đã được giao thất bại";
                var newRoute = new Route
                {
                    parcelID = parcelID,
                    relatedWarehouseID = thisWarehouseID,
                    details = details,
                    time = DateTime.Now,
                };
                // after you delivery the parcel , it's no longer in the finalWarehouse
                thisParcel.isFinalWarehouse = false;
                context.Routes.Add(newRoute);
                context.SaveChanges();
            }
        }


        public void AddSuccessToRoute(int parcelID, string thisWarehouseID)
        {
            using (var context = new PBL3_demoEntities())
            {
                var thisParcel = context.Parcels.Where(x => x.parcelID == parcelID).FirstOrDefault();
                string details = "Đơn hàng đã được giao thành công";
                var newRoute = new Route
                {
                    parcelID = parcelID,
                    relatedWarehouseID = thisWarehouseID,
                    details = details,
                    time = DateTime.Now,
                };
                // after you delivery the parcel , it's no longer in the finalWarehouse
                thisParcel.isFinalWarehouse = false;
                // and update ParcelStatus = false (a.k.a deliver successfully)
                thisParcel.parcelStatus = false;
                context.Routes.Add(newRoute);
                context.SaveChanges();
            }
        }


        public IQueryable<Route> GetRoutesFromParcelID(int parcelID)
        {
            using (var context = new PBL3_demoEntities())
            {
                var parcelRoute = context.Routes.Where(x => x.parcelID == parcelID);
                return parcelRoute;
            }
        }

        public int CountFailDeliveryOfThisParcel(int parcelID)
        {
            using (var context = new PBL3_demoEntities())
            {
                var TimesOfDeliveryFail = context.Routes.Count(x => x.parcelID == parcelID && x.details.Contains("thất bại"));
                return TimesOfDeliveryFail;
            }
        }

        public void AddReturnedOrderToRoute(int parcelID, string WarehouseID)
        {
            using (var context = new PBL3_demoEntities())
            {
                var thisParcel = context.Parcels.Where(x => x.parcelID == parcelID).FirstOrDefault();
                thisParcel.parcelStatus = true;
                string details = "Đơn hàng bị trả lại";
                var newRoute = new Route
                {
                    parcelID = parcelID,
                    relatedWarehouseID = WarehouseID,
                    details = details,
                    time = DateTime.Now,
                };
                context.Routes.Add(newRoute);
                context.SaveChanges();
            }
        }

        public void SetNullForExport(int parcelID)
        {
            using (var context = new PBL3_demoEntities())
            {
                var thisParcel = context.Parcels.Where(x => x.parcelID == parcelID).FirstOrDefault();
                thisParcel.currentWarehouseID = null;
                context.SaveChanges();
            }
        }
    }
}
