using Demo1.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static Demo1.ViewModel.OrderTrackingModel;

namespace Demo1.Model
{
    public class OrderTrackingManager
    {
        public OrderTrackingManager()
        {
            ParcelInfoList= new ObservableCollection<ParcelInfo>();
        }
        public ObservableCollection<ParcelInfo> ParcelInfoList { get; set; }

        public ObservableCollection<ParcelInfo> LoadParcelInfoList(int parcelID)
        {
            ParcelInfoList = new ObservableCollection<ParcelInfo>(); // Khởi tạo ParcelInfoList

            using (var dbContext = new Model.PBL3_demoEntities())
            {
                var query = dbContext.Parcels
                    .Join(dbContext.Routes, parcel => parcel.parcelID, route => route.parcelID, (parcel, route) => new { parcel, route })
                    .GroupBy(p => p.parcel)
                    .Select(g => new ParcelInfo
                    {
                        ParcelName = g.Key.parcelName,
                        ID = g.Key.parcelID,
                        Details = g.FirstOrDefault(r => r.route.routeID == g.Max(x => x.route.routeID)) != null ? g.FirstOrDefault(r => r.route.routeID == g.Max(x => x.route.routeID)).route.details : null
                    });

                List<ParcelInfo> queryResult = query.ToList();

                for (int i = queryResult.Count - 1; i >= 0; i--)
                {
                    ParcelInfoList.Add(queryResult[i]);
                    ParcelInfoList[ParcelInfoList.Count - 1].SimpleStatus = SetBasicParcelStatus(queryResult[i]);
                }
            }

            return ParcelInfoList;
        }

        public string SetBasicParcelStatus(ParcelInfo parcelInfo)
        {
            int iParcelID = parcelInfo.ID;
            string basicStatus = string.Empty;
            bool isNhapKhoDich = false;
            bool isXuatKhoDich = false;
            bool isTraLai = false;
            using (var context = new PBL3_demoEntities())
            {
                var routes = context.Routes
                    .Join(context.Parcels, r => r.parcelID, p => p.parcelID, (r, p) => new { Route = r, Parcel = p })
                    .Where(rp => rp.Route.parcelID == iParcelID)
                    .OrderBy(rp => rp.Route.routeID)
                    .ToList();

                foreach (var route in routes)
                {
                    string details = route.Route.details;
                    bool status = route.Parcel.parcelStatus ?? false;

                    if (details.Contains("Nhập đơn hàng vào kho đích"))
                    {
                        isNhapKhoDich = true;
                    }
                    else if (details.Contains("Xuất đơn hàng") && isNhapKhoDich)
                    {
                        isXuatKhoDich = true;
                    }

                    if (details.Contains("thành công"))
                    {
                        basicStatus = "Giao thành công";
                    }
                    else if (details.Contains("khởi tạo"))
                    {
                        basicStatus = "Vừa được khởi tạo";
                    }
                    else if (details.Contains("kho đích") && !status)
                    {
                        basicStatus = "Đến kho đích";
                    }
                    else if (details.Contains("thất bại") && !status)
                    {
                        basicStatus = "Giao thất bại";
                    }
                    else if (details.Contains("kho đích") && status)
                    {
                        basicStatus = "Bị trả lại";
                        isTraLai = true;
                    }
                    else if (isXuatKhoDich && !isTraLai)
                    {
                        basicStatus = "Đang giao hàng";
                    }
                    else
                    {
                        basicStatus = "Đang vận chuyển";
                    }
                }
            }

            return basicStatus;
        }
        public ObservableCollection<string> RouteInfoList { get; set; }

        public ObservableCollection<string> GetParcelRoute(int parcelID)
        {
            RouteInfoList = new ObservableCollection<string>();

            using (var context = new PBL3_demoEntities())
            {
                var parcelRoute = context.Routes
                    .Where(x => x.parcelID == parcelID)
                    .OrderBy(s => s.routeID);

                if (parcelRoute.Any())
                {
                    foreach (var route in parcelRoute)
                    {
                        string tempRoute = route.details + " vào lúc " + route.time.ToString("dd/MM/yyyy HH:mm:ss") + "\n";
                        RouteInfoList.Add(tempRoute);
                    }

                    var checkWarehouseNull = context.Parcels
                        .Where(x => x.parcelID == parcelID)
                        .Select(x => x.currentWarehouseID)
                        .FirstOrDefault() == null;

                    var thisParcel = UserInfo.ParcelInfo.Instance.GetParcelRecordInt(parcelID).parcelStatus;

                    if (checkWarehouseNull && (thisParcel != false))
                    {
                        RouteInfoList.Add("Đang vận chuyển " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "\n");
                    }
                }
                else
                {
                    RouteInfoList.Add("Đơn hàng này chưa có lộ trình");
                }
            }
            return RouteInfoList;
        }
        public bool ValidateParcelID(string parcelID)
        {
            int parsedParcelID;
            if (int.TryParse(parcelID, out parsedParcelID))
            {
                using (var context = new PBL3_demoEntities())
                {
                    var thisParcel = context.Parcels.Where(x => x.parcelID == parsedParcelID).FirstOrDefault();
                    if (thisParcel == null)
                    {
                        MessageBoxWindow.Show("Đơn này không tồn tại trong hệ thống");
                        return false;
                    }
                }
            }
            else
            {
                MessageBoxWindow.Show("Mã đơn hàng vừa nhập không hợp lệ");
                return false;
            }

            return true;
        }
    }
}
