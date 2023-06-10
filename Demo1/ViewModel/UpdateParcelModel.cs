﻿
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Demo1.Model;
using Demo1.UserInfo;
using Demo1.View;

namespace Demo1.ViewModel
{
    public class UpdateParcelModel : BaseViewModel
    {
        private DateTime _dateTime;
        private string accoutWarehouse;
        private string parcelID;
        private string warehouseID;
        private string _shippingMethod;

        private DateTime _createTime;
        private string _status;
        private bool isFinalWarehouse;
        private string finalWarehouseDetail;

        private ObservableCollection<string> routeCollection;
        private ObservableCollection<Parcel> parcelCollection;



        public DateTime DateTime
        {
            get { return _dateTime; }
            set
            {
                if (_dateTime != value)
                {
                    _dateTime = value;
                    OnPropertyChanged();
                }
            }
        }
        public DateTime CreateTime
        {
            get { return _createTime; }
            set
            {
                if (_createTime != value)
                {
                    _createTime = value;
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
        public string Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        }
        public string ShippingMethod
        {
            get { return _shippingMethod; }
            set
            {
                if (_shippingMethod != value)
                {
                    _shippingMethod = value;
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
                    ValidateParcelID(true);
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


        public bool IsFinalWarehouse
        {
            get { return isFinalWarehouse; }
            set
            {
                if (isFinalWarehouse != value)
                {
                    isFinalWarehouse = value;
                    OnPropertyChanged();
                }
            }
        }


        public string FinalWarehouseDetail
        {
            get { return finalWarehouseDetail; }
            set
            {
                if (finalWarehouseDetail != value)
                {
                    finalWarehouseDetail = value;
                    OnPropertyChanged();
                }
            }
        }


        int ValidateParcelID(bool isShowErrorMessage)
        {
            int parcelID;
            bool isValid = int.TryParse(ParcelID, out parcelID);

            if (!isValid)
            {
                // Chuỗi không chứa số nguyên hợp lệ
                if (isShowErrorMessage)
                {
                    ShowErrorMessage("Mã đơn hàng vừa nhập không hợp lệ");
                }

                return parcelID;
            }
            var thisParcel = ParcelInfo.Instance.GetParcelRecordInt(parcelID);
            if (thisParcel == null && isShowErrorMessage)
            {
                ShowErrorMessage("Đơn hàng không tồn tại trong hệ thống! Xin vui lòng thử lại");
            }


            return parcelID;
        }


        void ShowErrorMessage(string message)
        {
            // Hiển thị thông báo lỗi theo cơ chế thông báo của WPF (ví dụ: Validation.ErrorTemplate)
            // Hoặc thực hiện xử lý hiển thị thông báo lỗi khác trong giao diện WPF
            // Ví dụ: gán thông báo lỗi vào một thuộc tính và sử dụng Binding để hiển thị thông báo đó trong giao diện WPF
            // Hoặc sử dụng một cơ chế thông báo tương tự trong thư viện MVVM
            MessageBoxWindow.Show(message);
        }


        public ICommand ShowParcelInfoCommand { get; set; }

        public ICommand ExportfromWareHouseCommand { get; set; }

        public ICommand ImportintoWreHouseCommand { get; set; }

        public ICommand ParcelRouteCommand { get; set; }

        public ICommand SuccessDeliveryCommand { get; set; }
        public ICommand FailDeliveryCommand { get; set; }
        public UpdateParcelModel()
        {
            DateTime = DateTime.Now;
            string accountID = AccountManager.Instance.GetAccountID();
            WarehouseID = AccountManager.Instance.GetUserWarehouseID(accountID);
            //hien thi thong tin don hang
            ShowParcelInfoCommand =
               new RelayCommand<object>((p) => { return !string.IsNullOrEmpty(ParcelID); }, (p) => GetParcelInfo());
            ExportfromWareHouseCommand =
                new RelayCommand<object>((p) => { return !string.IsNullOrEmpty(ParcelID) && !CanExcuteFinalRouteCommand() && !isThreeTimeDeliveryFail(); }, (p) => ExportFromWarehouse());

            ImportintoWreHouseCommand = new RelayCommand<object>((p) => { return !string.IsNullOrEmpty(ParcelID); }, (p) => ImportIntoWarehouse());

            ParcelRouteCommand = new RelayCommand<object>((p) => { return !string.IsNullOrEmpty(ParcelID); },
                (p => RouteCollection = ParcelRoute()));

            SuccessDeliveryCommand = new RelayCommand<object>((p) =>
            {
                return !string.IsNullOrEmpty(ParcelID) && CanExcuteFinalRouteCommand();
            }, (p) => AddSuccessDeliveryIntoRoutes());

            FailDeliveryCommand = new RelayCommand<object>((p) =>
            {
                return !string.IsNullOrEmpty(ParcelID) && CanExcuteFinalRouteCommand();
            }, (p) => AddFailDeliveryIntoRoutes());

        }

        // check the validation of the ParcelID


        // get the ID wareHouse of this account belong to ( work in)
        string lastStatus;
        //tim tinh trang cuoi cung cua don hang phuc vu cho ham GetParcelInfo
        public string LastStatus()
        {
            UpdateParcelManager UPM = new UpdateParcelManager();
            int parcelID = ValidateParcelID(false);

            // if thisParcel hasnt Exist in dtb -> Function return false -> condition will be true -> return
            if (!ParcelInfo.Instance.IsParcelExist(parcelID))
            {
                return "";
            }
            //TH mới tạo đơn chưa nhập vào kho
            var checkifparcelexist = UPM.GetRouteFromParcelID(parcelID);
            if (checkifparcelexist == null)
            {
                return "";
            }
            else
            {
                lastStatus = UPM.GetLastStatus(checkifparcelexist, parcelID);
            }

            return lastStatus;
        }
        public void GetParcelInfo()
        {
            int parcelID = ValidateParcelID(false);
            if (parcelID != 0)
            {
                var getInfo = ParcelInfo.Instance.GetParcelRecordInt(parcelID);
                if (getInfo != null)
                {
                    CreateTime = (DateTime)getInfo.createTime;
                    if (getInfo.shippingMethod) ShippingMethod = "Nhanh";
                    else ShippingMethod = "Chậm";
                    Status = LastStatus();
                }
                else
                {
                    MessageBoxWindow.Show("Đơn hàng không tồn tại!");
                }
            }

        }


        // check if the warehouseID of this parcelID is equal with the warehouseID of this account
        int CheckIDParcel(string _parcelID)
        {
            UpdateParcelManager UPM = new UpdateParcelManager();
            int parcelID = ValidateParcelID(false);
            if (parcelID != 0)
            {
                string thisWarehouseID = WarehouseID;
                int check;

                var warehouseIDOfParcel = UPM.GetWarehouseIDFromParcelID(parcelID);
                check = (thisWarehouseID == warehouseIDOfParcel) ? 1 : 0;
                return check;
            }
            return -1;
        }


        //export

        void ExportFromWarehouse()
        {
            UpdateParcelManager UPM = new UpdateParcelManager();
            // if the currentWH or Parcel  == thisWH (a.k.a == 1) so that the Parcel in this WH where the account belongs to -> you can export
            if (CheckIDParcel(ParcelID) == 1)
            {
                int parcelID = ValidateParcelID(false);
                // if thisParcel hasnt Exist in dtb -> Function return false -> condition will be true -> return
                if (!ParcelInfo.Instance.IsParcelExist(parcelID))
                {
                    return;
                }

                var thisParcel = UserInfo.ParcelInfo.Instance.GetParcelRecordInt(parcelID);
                if (thisParcel.currentWarehouseID != null)
                {
                    UPM.SetNullForExport(parcelID);
                }

                string thisWarehouseID = WarehouseID;
                string thisWarehouseName = UPM.GetWarehouseNameFromWHID(thisWarehouseID);
                UPM.AddExportToRoute(parcelID, thisWarehouseID, thisWarehouseName);
                // if isFinalWarehouse --> after you exportFromwarehouse you cant ImportIntoWarehouse in any warehouse
                // --> call function checkfinalwarehouse, if it's true -> Parcel.isFinalWarehouse = true
                CheckFinalWarehouse();
                MessageBoxWindow.Show("Xuất thành công đơn hàng " + ParcelID + " ra khỏi " + thisWarehouseName);

            }
            else
            {
                MessageBoxWindow.Show(
                    "Đơn hàng này hiện tại không thuộc kho của bạn nên không thể cập nhật, xin vui lòng kiểm tra lại");
            }
            GetParcelInfo();
        }


        //import

        void ImportIntoWarehouse()
        {
            using (var context = new PBL3_demoEntities())
            {
                int parcelID = ValidateParcelID(false);
                // if thisParcel hasnt Exist in dtb -> Function return false -> condition will be true -> return
                if (!ParcelInfo.Instance.IsParcelExist(parcelID))
                {
                    return;
                }
                var thisParcel = context.Parcels.Where(x => x.parcelID == parcelID).FirstOrDefault();
                string thisWarehouseID = WarehouseID;
                string thisWarehouseName = context.Warehouses.Where(x => x.warehouseID == thisWarehouseID)
                    .Select(x => x.warehouseName).FirstOrDefault();
                var SuccessDeliveryRoute = context.Routes.Where(x =>
                        x.parcelID == parcelID && (x.details.Contains("thành công")))
                    .FirstOrDefault();
                var WarehouseIDAtFinalRoutes = context.Routes.Where(x => x.parcelID == parcelID && x.details.Contains("đích"))
                    .Select(x => x.relatedWarehouseID).FirstOrDefault();

                // check that thisWarehouseID == WarehouseIDAtFinalRoutes or not (check that if after you export your parcel at final Warehouse , only final Warehouse can import this Parcel again
                bool canImportAgainToFinalWarehouse;
                if (WarehouseIDAtFinalRoutes != null)
                {
                    canImportAgainToFinalWarehouse = thisWarehouseID == WarehouseIDAtFinalRoutes;
                }
                else canImportAgainToFinalWarehouse = true;
                // check if this parcel has existed in this warehouse , if currentWH of parcel != this account WH -> (a.k.a == 0) so that this parcel isn't belong to this WH, so that you can Import (parcel isnt exist in thisWH)
                if (CheckIDParcel(ParcelID) == 0)
                {
                    // we get the FinalRoute , which means it will check you have delivered successfull/fail before or not , if SuccessOrFailDeliveryRoute == null -> so it have never delivered before, you can continue doing
                    if (SuccessDeliveryRoute == null)
                    {
                        // check if this parcel currentWarehouseID is null, if it not null so it wouldn't be this WareHouseID because we had checked it before -> it would be in another wareHouse so we cant update
                        if (thisParcel.currentWarehouseID == null)
                        {
                            // if it can Import to the FinalWarehouse and not in the FinalWarehouse
                            if ((thisParcel.isFinalWarehouse == false || thisParcel.isFinalWarehouse == null) &&
                                canImportAgainToFinalWarehouse)
                            {
                                string details;
                                thisParcel.currentWarehouseID = thisWarehouseID;
                                //MessageBoxWindow.Show(CheckFinalWarehouse().ToString());
                                // check if it is the final Warehouse , so we update the detail with the final words
                                // changes the value of isFinalWarehouse
                                thisParcel.isFinalWarehouse = CheckFinalWarehouse();
                                if (thisParcel.isFinalWarehouse == true)
                                {
                                    MessageBoxWindow.Show("Đơn hàng đã tới kho đích");
                                    details = "Nhập đơn hàng vào kho đích tại " + thisWarehouseName;
                                    //FinalWarehouseDetail = "Đơn hàng đã đến đích";
                                }
                                else
                                {
                                    details = "Nhập đơn hàng vào " + thisWarehouseName;
                                    //FinalWarehouseDetail = "";
                                }

                                var newRoute = new Route
                                {
                                    parcelID = parcelID,
                                    relatedWarehouseID = thisWarehouseID,
                                    details = details,
                                    time = DateTime.Now,
                                };
                                context.Routes.Add(newRoute);
                                context.SaveChanges();

                                MessageBoxWindow.Show(
                                    "Cập nhật thành công đơn hàng " + ParcelID + " vào " + thisWarehouseName);
                            }
                            else
                            {
                                // if it is FinalWarehouse but you Import it into another Warehouse (not the FinalWarehouse) , you cant Import
                                MessageBoxWindow.Show(
                                    "Vì đơn hàng của bạn đã xuất ra khỏi từ kho đích nên không thể nhập vào các kho khác");
                            }
                        }
                        else
                        {
                            MessageBoxWindow.Show(
                                "Đơn hàng này thuộc thẩm quyền của kho khác, hiện tại không thuộc kho của bạn nên không thể cập nhật, xin vui lòng kiểm tra lại");

                        }
                    }
                    else
                    {
                        // if you have delivered it one time , you cant update it into wareHouse
                        MessageBoxWindow.Show("Đơn hàng này đã được giao thành công , không thể cập nhật vào kho");
                    }
                }
                else
                {
                    MessageBoxWindow.Show("Đã tồn tại đơn hàng này trong kho, không thể cập nhật");
                }
            }
            GetParcelInfo();
        }



        //parcel route 

        ObservableCollection<string> ParcelRoute()
        {
            UpdateParcelManager UPM = new UpdateParcelManager();
            RouteCollection = new ObservableCollection<string>();
            int parcelID = ValidateParcelID(false);
            // if thisParcel hasnt Exist in dtb -> Function return false -> condition will be true -> return
            if (!ParcelInfo.Instance.IsParcelExist(parcelID))
            {
                return RouteCollection;
            }
            ObservableCollection<string> parcelRoutes = new ObservableCollection<string>();

            var parcelRoute = UPM.GetRoutesFromParcelID(parcelID);
            if (parcelRoute.Any())
            {
                foreach (var route in parcelRoute)
                {
                    string tempRoute = route.details + " vào lúc " + route.time.ToString("dd/MM/yyyy HH:mm:ss") +
                                        "\n";
                    parcelRoutes.Add(tempRoute);
                }

                var checkWarehouseNull = UPM.GetWarehouseIDFromParcelID(parcelID) == null;
                // if you after Delivery your Parcel Success , 
                if (checkWarehouseNull && CanExcuteFinalRouteCommand() && (ParcelInfo.Instance.GetParcelRecordInt(parcelID).parcelStatus != false))
                {
                    parcelRoutes.Add("Đang vận chuyển " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "\n");
                }
            }
            else
            {
                parcelRoutes.Add("Đơn hàng này chưa có lộ trình");
            }

            return parcelRoutes;
        }



        // check if this wareHouse is the final place (wareHouse city's position == RCustomer city's position)

        bool CheckFinalWarehouse()
        {
            UpdateParcelManager UPM = new UpdateParcelManager();
            bool finalWarehouseCheck = true;
            int parcelID = ValidateParcelID(false);

            var thisParcel = UserInfo.ParcelInfo.Instance.GetParcelRecordInt(parcelID);
            // more important , if this.currentWarehouse == null , so it is in delivery -> it absolutely not in the final werehouse
            // get the city of the ReceiverCustomer of the Parcel

            if (thisParcel != null)
            {
                if (thisParcel.currentWarehouseID != null)
                {
                    // it is not null , so it is in wareHouse -> so check is fall
                    //IsFinalWarehouse = false;
                    finalWarehouseCheck = false;
                }
                else
                {
                    string thisReceiverCustomerILocation = UPM.GetRLocationFromParcelID(thisParcel, parcelID);
                    string[] parts = thisReceiverCustomerILocation.Split(',');
                    string thisReceiverCustomerCity = parts[parts.Length - 1];
                    // get the city of this wareHouse
                    string thisWarehouseID = WarehouseID;
                    string thisWarehouseName = UPM.GetWarehouseNameFromWHID(thisWarehouseID);
                    string thisCityofWarehouse = thisWarehouseName.Replace("Kho ", "");
                    // compare 
                    finalWarehouseCheck = (thisCityofWarehouse == thisReceiverCustomerCity);
                    //IsFinalWarehouse = finalWarehouseCheck;
                }
            }

            //return IsFinalWarehouse;
            return finalWarehouseCheck;
        }

        void AddSuccessDeliveryIntoRoutes()
        {
            int parcelID = ValidateParcelID(false);
            if (parcelID == 0) return;

            UpdateParcelManager UPM = new UpdateParcelManager();
            var thisParcel = UserInfo.ParcelInfo.Instance.GetParcelRecordInt(parcelID);
            string thisWarehouseID = WarehouseID;
            // thisParcel.currentWarehouse == null  and thisParcel.isFinalWarehouse == true because after it go to the finalWarehouse(thisParcel.isFinalWarehouse == true) and Export (thisParcel.currentWarehouse == null)
            if (thisParcel.isFinalWarehouse == true && thisParcel.currentWarehouseID == null)
            {
                UPM.AddSuccessToRoute(parcelID, thisWarehouseID);
            }

            GetParcelInfo();
        }


        void AddFailDeliveryIntoRoutes()
        {
            UpdateParcelManager UPM = new UpdateParcelManager();
            int parcelID = ValidateParcelID(false);
            if (parcelID == 0) return;


            var thisParcel = UserInfo.ParcelInfo.Instance.GetParcelRecordInt(parcelID);
            string thisWarehouseID = WarehouseID;
            // thisParcel.currentWarehouse == null  and thisParcel.isFinalWarehouse == true because after it go to the finalWarehouse(thisParcel.isFinalWarehouse == true) and Export (thisParcel.currentWarehouse == null)
            if (thisParcel.isFinalWarehouse == true && thisParcel.currentWarehouseID == null)
            {
                UPM.AddFailToRoute(parcelID, thisWarehouseID);
            }

            GetParcelInfo();
        }
        bool CanExcuteFinalRouteCommand()
        {
            int parcelID = ValidateParcelID(false);
            // if thisParcel hasnt Exist in dtb -> Function return false -> condition will be true -> return
            if (!ParcelInfo.Instance.IsParcelExist(parcelID))
            {
                return false;
            }
            bool _canexecute = false;
            string thisWarehouseID = WarehouseID;
            using (var context = new PBL3_demoEntities())
            {
                var thisParcel = context.Parcels.Where(x => x.parcelID == parcelID).FirstOrDefault();
                // check if thisWarehouse can call Fail or Success DeliveryCommand (check in Routes, find the detail with keyword "đích" then get the relatedwWarehouseID , after that , compare this with thisWarehouseID
                var WarehouseIDAtFinalRoutes = context.Routes.Where(x => x.parcelID == parcelID && x.details.Contains("đích"))
                    .Select(x => x.relatedWarehouseID).FirstOrDefault();
                // get the success or Fail DeliveryRoute
                var SuccessOrFailDeliveryRoute = context.Routes.Where(x =>
                        x.parcelID == parcelID && (x.details.Contains("thành công")))
                    .FirstOrDefault();
                {
                    // check if you deliver less than three times or not 
                    if (!isThreeTimeDeliveryFail())
                    {
                        // check if Routes have Success or Fail Delivery , if it is not null -> you cant do it anymore -> so that you have only 1 times to deliver your Parcels, if it is null so you can call Fail or Success DeliveryCommand 
                        if (SuccessOrFailDeliveryRoute == null)
                        {
                            // if in WarehouseID in the FinalRoute == thisWarehouseID , you have ability to call Fail or Success DeliveryCommand
                            if (WarehouseIDAtFinalRoutes == thisWarehouseID)
                            {
                                // this if is check that after you Import The FinalWarehouse , you will have ability to call Fail or Success DeliveryCommand
                                // if it is false or null -> so it's not the finalWarehouse
                                if (thisParcel.isFinalWarehouse == true && thisParcel.currentWarehouseID == null)
                                {
                                    _canexecute = true;
                                }
                                else
                                {
                                    // if it is true -> so it is the finalWarehouse -> false

                                }
                            }
                            else
                            {
                                // isFinalWarehouse = false because u have no ability to call Fail or Success DeliveryCommand -> false

                            }
                        }
                        else
                        {
                            // so this it not null -> you cant do it anymore because you have delivered your parcels one time before -> false


                        }
                    }
                    else
                    {
                        // if you has deliver failed three time , you cant deliver anymore
                    }
                }

            }
            return _canexecute;
        }

        // check if you have delivery fail three times 
        bool isThreeTimeDeliveryFail()
        {
            UpdateParcelManager UPM = new UpdateParcelManager();
            int parcelID = ValidateParcelID(false);

            var thisParcel = UserInfo.ParcelInfo.Instance.GetParcelRecordInt(parcelID);
            var TimesOfDeliveryFail = UPM.CountFailDeliveryOfThisParcel(parcelID);

            if (thisParcel != null && thisParcel.parcelStatus == true)
            {
                return true;
            }

            if (TimesOfDeliveryFail != 3)
            {
                return false;
            }

            UPM.AddReturnedOrderToRoute(parcelID, WarehouseID);
            GetParcelInfo();
            return true;

        }

    }
}

