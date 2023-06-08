using Demo1.Model;
using Demo1.View;
using System;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Demo1.ViewModel
{
    public class OrderTrackingModel : BaseViewModel
    {
        public ICommand LoadParcelInfoCommand { get; private set; }


        private string _BasicStatus;
        public string BasicStatus
        {
            get
            {
                return _BasicStatus;
            }
            set { _BasicStatus = value; OnPropertyChanged(); }
        }
        private string _ParcelID;
        public string ParcelID
        {
            get { return _ParcelID; }
            set
            {
                _ParcelID = value;
                OnPropertyChanged();
                ValidateParcelID();
            }
        }

        private ObservableCollection<string> _routeInfoList;
        public ObservableCollection<string> RouteInfoList
        {
            get { return _routeInfoList; }
            set
            {
                _routeInfoList = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<ParcelInfo> _parcelInfoList;
        public ObservableCollection<ParcelInfo> ParcelInfoList
        {
            get { return _parcelInfoList; }
            set
            {
                _parcelInfoList = value;
                OnPropertyChanged();
            }
        }
        public class ParcelInfo
        {
            public int ID { get; set; }
            public string ParcelName { get; set; }
            public string Details { get; set; }
            public double Mass {get; set; }
            public string SimpleStatus { get; set; }
        }
        public ICommand SelectParcelCommand { get; }

        private void ExecuteSelectParcel(object parameter)
        {
            if (parameter is ParcelInfo selectedParcel)
            {
                ParcelID = Convert.ToString(selectedParcel.ID);
            }
        }

        public OrderTrackingModel()
        {
            
            ParcelTrackingCommand =
               new RelayCommand<object>((p) =>
               {
                   return true;
               }, (p) => GetParcelRoute());
            ParcelInfoList = new ObservableCollection<ParcelInfo>();
            SelectParcelCommand = new RelayCommand<object>((p) => true, ExecuteSelectParcel);
            LoadParcelInfoCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                ParcelInfoList.Clear();
                LoadParcelInfoList();
            } ) ;
            LoadParcelInfoList();

        }
        private void LoadParcelInfoList()
        {
            using (var dbContext = new Model.PBL3_demoEntities())
            {
                //dbContext.Parcels là bảng Parcels trong context của đối tượng DbContext.
                //.Join(dbContext.Routes, parcel => parcel.parcelID, route => route.parcelID, (parcel, route) => new { parcel, route }) thực hiện phép kết hợp dữ liệu từ hai bảng Parcels và Routes dựa trên trường parcelID.
                //Kết quả của phép kết hợp là một danh sách các cặp(parcel, route).
                //.GroupBy(p => p.parcel) nhóm các kết quả theo parcel, tức là các đối tượng parcel giống nhau được nhóm lại thành một nhóm.
                //neu khong phan nhom thi ra ve to hop cua 2 bang
                //key sẽ là một thuộc tính của mỗi nhóm kết quả(parcel) và đại diện cho giá trị parcel được nhóm dựa trên.
                //elements
                var query = dbContext.Parcels
                        .Join(dbContext.Routes, parcel => parcel.parcelID, route => route.parcelID, (parcel, route) => new { parcel, route })
                        .GroupBy(p => p.parcel)
                        .Select(g => new ParcelInfo
                        {
                            ParcelName = g.Key.parcelName,
                            ID = g.Key.parcelID,
                            //Đối tượng g đại diện cho một nhóm các đối tượng có cùng giá trị của Parcel 
                            //r đại diện cho mỗi đối tượng trong nhóm g.
                            Details = g.FirstOrDefault(r => r.route.routeID == g.Max(x => x.route.routeID)) != null ? g.FirstOrDefault(r => r.route.routeID == g.Max(x => x.route.routeID)).route.details : null,
                           

                            //xet thu co co routeID khac null khong, neu co thi gan =,khong thi de bang null
                        });
                //Kết quả của query sẽ là danh sách các đối tượng ParcelInfo được tạo ra từ các nhóm 
                //in từ đơn hàng mới tạo gần nhất
                List<ParcelInfo> queryResult = query.ToList();

                for (int i = queryResult.Count - 1; i >= 0; i--)
                {
                    ParcelInfoList.Add(queryResult[i]);
                    ParcelInfoList[ParcelInfoList.Count - 1].SimpleStatus = SetBasicParcelStatus(queryResult[i]);

                }
            }
        }

        //public string SetBasicParcelStatus(ParcelInfo parcelInfo)
        //{
        //    int iParcelID = parcelInfo.ID;
        //    string basicStatus = string.Empty;
        //    bool isNhapKhoDich = false;
        //    using (var context = new PBL3_demoEntities())
        //    {

        //        var route = context.Routes
        //            .Join(context.Parcels,
        //                r => r.parcelID,
        //                p => p.parcelID,
        //                (r, p) => new { Route = r, Parcel = p })
        //            .Where(rp => rp.Route.parcelID == iParcelID)
        //            .OrderByDescending(rp => rp.Route.routeID)
        //            .Select(rp => new { rp.Route.details, rp.Parcel.parcelStatus })
        //            .FirstOrDefault();

        //        if (route != null)
        //        {
        //            string details = route.details;
        //            bool status = context.Parcels
        //                .Where(p => p.parcelID == iParcelID)
        //                .Select(p => p.parcelStatus)
        //                .FirstOrDefault() ?? false;

        //            //if (details.Contains("thành công"))
        //            //    basicStatus = "Giao thành công";
        //            //else if (details.Contains("khởi tạo"))
        //            //    basicStatus = "Vừa được khởi tạo";
        //            //else if (details.Contains("kho đích") && status == true)
        //            //    basicStatus = "Bị trả lại";
        //            //else if (details.Contains("Nhập đơn hàng vào kho đích") && status == false)
        //            //{
        //            //    basicStatus = "Đến kho đích"; 

        //            //}


        //            //else if (details.Contains("thất bại") && status == false)
        //            //    basicStatus = "Giao thất bại";
        //            //else
        //            //    basicStatus = "Đang vận chuyển";




        //            if (details.Contains("thành công"))
        //            {
        //                basicStatus = "Giao thành công";
        //            }
        //            else if (details.Contains("khởi tạo"))
        //            {
        //                basicStatus = "Vừa được khởi tạo";
        //            }
        //            else if (details.Contains("kho đích") && status == true)
        //            {
        //                basicStatus = "Bị trả lại";
        //            }
        //            else if (details.Contains("kho đích") && status == false)
        //            {
        //                basicStatus = "Đến kho đích";
        //                isNhapKhoDich = true;

        //            }
        //            else if (isNhapKhoDich)
        //            {
        //                MessageBox.Show("Đây nè");
        //                basicStatus = "Đang giao hàng";
        //            }
        //            else if (details.Contains("thất bại") && status == false)
        //            {
        //                basicStatus = "Giao thất bại";
        //            }
        //            else
        //            {
        //                basicStatus = "Đang vận chuyển";
        //            }

        //        }
        //    }

        //    return basicStatus;
        //}
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
                    .Join(context.Parcels,
                        r => r.parcelID,
                        p => p.parcelID,
                        (r, p) => new { Route = r, Parcel = p })
                    .Where(rp => rp.Route.parcelID == iParcelID)
                    .OrderBy(rp => rp.Route.routeID)
                    .ToList();

                foreach (var route in routes)
                {
                    string details = route.Route.details;
                    bool status = route.Parcel.parcelStatus ?? false;

                    // Kiểm tra điều kiện nhập kho đích và xuất kho đích
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
                    else if (isXuatKhoDich&&!isTraLai)
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

        int GetDeliveryStatus()
        {
            int iParcelID = TryParseParcelID(ParcelID);
            int Delivery = -1;
            using (var context1 = new PBL3_demoEntities())
            {
                var res = context1.Routes.FirstOrDefault(x => x.parcelID == iParcelID && x.details.Contains("thành công"));
                if (res != null) Delivery = 1;
                var res1 = context1.Routes.FirstOrDefault(x => x.parcelID == iParcelID && x.details.Contains("thất bại"));
                if (res1 != null) Delivery = 0;

            }
            return Delivery;
        }
        public ICommand ParcelTrackingCommand { get; set; }

        void ValidateParcelID()
        {
            int parcelID;
            if (int.TryParse(ParcelID, out parcelID))
            {
                // Chuỗi chứa số nguyên hợp lệ
                using (var context = new PBL3_demoEntities())
                {
                    var thisParcel = context.Parcels.Where(x => x.parcelID == parcelID).FirstOrDefault();
                    if (thisParcel == null)
                    {
                        MessageBoxWindow.Show("Đơn này không tồn tại trong hệ thống");
                    }
                }
            }
            else
            {
                // Chuỗi không chứa số nguyên hợp lệ
                MessageBoxWindow.Show("Mã đơn hàng vừa nhập không hợp lệ");
            }
        }



        void GetParcelRoute()
        {
            int iParcelID;
            RouteInfoList = new ObservableCollection<string>();
            if (int.TryParse(ParcelID, out iParcelID))
            {
                // Chuỗi chứa số nguyên hợp lệ
                using (var context = new PBL3_demoEntities())
                {
                    var parcelRoute = context.Routes.Where(x => x.parcelID == iParcelID).OrderBy(s => s.routeID);
                    if (parcelRoute.Any())
                    {
                        foreach (var route in parcelRoute)
                        {
                            string tempRoute = route.details + " vào lúc " + route.time.ToString("dd/MM/yyyy HH:mm:ss") +
                                               "\n";
                            RouteInfoList.Add(tempRoute);
                        }

                        var checkWarehouseNull = context.Parcels.Where(x => x.parcelID == iParcelID)
                            .Select(x => x.currentWarehouseID).FirstOrDefault() == null;
                        var thisParcel = UserInfo.ParcelInfo.Instance.GetParcelRecordInt(iParcelID).parcelStatus;
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
            }
            else
            {
                MessageBoxWindow.Show("Mã đơn hàng vừa nhập không hợp lệ");
            }

        }
        public int TryParseParcelID(string parcelID)
        {
            int parsedParcelID;
            if (int.TryParse(parcelID, out parsedParcelID))
            {
                return parsedParcelID;
            }
            else
            {
                // Xử lý khi không thể chuyển đổi parcelID thành kiểu int
                return -1;
            }
        }
    }
}
