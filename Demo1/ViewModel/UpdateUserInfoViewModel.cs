using Demo1.DTO;

using Demo1.Model;
using Demo1.UserInfo;
using Demo1.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Demo1.ViewModel
{
    public class UpdateUserInfoViewModel:PropertiesCollection
    {
        private string _RoleID;
        public string RoleID
        {
            get { return _RoleID; }
            set { _RoleID = value; OnPropertyChanged(); }
        }
        private string _RoleName;
        public string RoleName
        {
            get { return _RoleName; }
            set { _RoleName = value; OnPropertyChanged(); }
        }
        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; OnPropertyChanged(); }
        }
        private string _UserLocation;
        public string UserLocation
        {
            get { return _UserLocation; }
            set { _UserLocation = value; OnPropertyChanged(); }
        }
        private string _UserPhoneNumber;
        public string UserPhoneNumber
        {
            get { return _UserPhoneNumber; }
            set { _UserPhoneNumber= value; OnPropertyChanged(); }
        }
        public event EventHandler<string> UserNameChanged;

        public void UpdateUserName(string newUserName)
        {
            // Thực hiện các hoạt động cập nhật thông tin người dùng

            // Khi có sự thay đổi tên người dùng, kích hoạt sự kiện UserNameChanged
            OnUserNameChanged(newUserName);
        }

        protected virtual void OnUserNameChanged(string newUserName)
        {
            UserNameChanged?.Invoke(this, newUserName);
        }

        public ICommand RefreshCommand { get; set; }
        public ICommand AcceptCommand { get; set; }
        private UpdateUserInfoManager UUIVM;
        public UpdateUserInfoViewModel()
        {
            Cities = DictionaryData.GetCities();
            int RolePermission = AccountManager.Instance.GetAccessRight();
            RefreshCommand = new RelayCommand<object>((t) => { return true; }, (t) =>
            {
                
                ClearView();

            });
            AcceptCommand = new RelayCommand<object>((t) => { return IsValid(UserPhoneNumber)&&!string.IsNullOrEmpty(UserLocation); }, (t) =>
            {
                UpdateUserInfo(RoleID, RolePermission, UserName, UserLocation, UserPhoneNumber);
                MessageBoxWindow.Show("Đã cập nhật thông tin cá nhân");
               
               
            });
            UUIVM = new UpdateUserInfoManager();
            SetView();


        }
        public void SetView()
        {
            RoleID = AccountManager.Instance.GetAccountID();
            WarehouseID = AccountManager.Instance.GetUserWarehouseID(RoleID);

            UserName = AccountManager.Instance.GetUserName(RoleID);
            int RolePermission = AccountManager.Instance.GetAccessRight();
            RoleName = AccountManager.Instance.GetRoleName(RolePermission);

            UserLocation = AccountManager.Instance.GetUserLocation(RoleID);
            UserPhoneNumber = AccountManager.Instance.GetUserPhoneNumber(RoleID);
        }
        public bool IsValid(string inputStr)
        {
         
            string pattern = @"^0\d{9}$"; // Định dạng số điện thoại gồm 10 chữ số và có số 0 ở trước

            if (inputStr?.Length > 0 && Regex.IsMatch(inputStr, pattern) )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void ClearView()
        {

           
            UserPhoneNumber = "";
            UserName = "";
            UserLocation = "";
            
        }
        public void UpdateUserInfo(string roleId, int rolePermission, string updateName, string updateLocation, string updatePhoneNumber)
        {
            UpdateUserInfoManager manager = new UpdateUserInfoManager();
            manager.UpdateInfo(roleId, rolePermission, updateName, updateLocation, updatePhoneNumber);
        }
    }
}
