using Demo1.Model;

using System;

using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Linq;
using System.Windows.Input;

namespace Demo1.ViewModel
{
    public class OrderTrackingModel : BaseViewModel
    {

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
        //    string startWHID = "";
        //    using (var context = new PBL3_demoEntities())
        //    {
        //        var res = context.Invoices.FirstOrDefault(x => x.parcelID ==iParcelID);
        //        //if !=null
        //        startWHID = res.startWarehouseID;

        //    }
        //    //check xem đơn giao thành công/thất bại/bị trả lại

        //    using(var context1 = new PBL3_demoEntities()) 
        //    {
        //        int check = GetDeliveryStatus();
        //        var res = context1.Parcels.FirstOrDefault(x => x.parcelID == iParcelID);

        //        if (res.currentWarehouseID == startWHID) BasicStatus = "Đơn hàng vừa được khởi tạo";
        //        if(res.currentWarehouseID!=startWHID && res.isFinalWarehouse==false) BasicStatus= "Đơn hàng vừa nhập kho";
        //        if (res.isFinalWarehouse == true && check==-1) BasicStatus = "Đơn hàng đang ở kho đích";
        //        if (res.isFinalWarehouse == true && check == 1) BasicStatus = "Đơn hàng đã được giao thành công";
        //        if(res.isFinalWarehouse == true && res.parcelStatus==true) BasicStatus = "Đơn hàng bị trả lại";
        //        else if (res.isFinalWarehouse == true && check == 0) BasicStatus = "Đơn hàng giao thất bại";
        //        if (res.currentWarehouseID == null) BasicStatus = "Đang vận chuyển";
        //    }
        //    return BasicStatus;
        //}
        public string SetBasicParcelStatus(ParcelInfo parcelInfo)
        {
            int iParcelID = parcelInfo.ID;
            
            using(var context =new PBL3_demoEntities())
            {
                bool status=false;
                var check = context.Parcels.FirstOrDefault(x => x.parcelStatus == true);
                if (check != null)
                {
                    status = check.parcelStatus ?? false;

                }
                var route = context.Routes
                     .Where(x=> x.parcelID == iParcelID)
                     .OrderByDescending(x=> x.routeID)
                     .FirstOrDefault();

                string details = route?.details;
                if (details.Contains("thành công")) BasicStatus = "Đơn được giao thành công";
                else if (details.Contains("khởi tạo")) BasicStatus = "Đơn vừa được khởi tạo";
                else if (details.Contains("kho đích") && status) BasicStatus = "Đơn hàng bị trả lại";
                else if (details.Contains("kho đích")&&status==false) BasicStatus = "Đơn được giao đến kho đích";                
                else if (details.Contains("thất bại")&& status == false) BasicStatus = "Đơn được giao thất bại";

                else BasicStatus = "Đang vận chuyển";
            }
            return BasicStatus;

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
                        
                        if (checkWarehouseNull)
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
