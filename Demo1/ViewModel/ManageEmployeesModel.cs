using Demo1.DTO;
using Demo1.Model;
using Demo1.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace Demo1.ViewModel
{
    public class ManageEmployeesModel : BaseViewModel
    {
        private string _NewAccountName;
        public string NewAccountName
        {
            get { return _NewAccountName; }
            set { _NewAccountName = value; OnPropertyChanged(); }
        }
        private string _NewPassword;
        public string NewPassword
        {
            get { return _NewPassword; }
            set { _NewPassword = value; OnPropertyChanged(); }
        }

        private ObservableCollection<string> cities;
        public ObservableCollection<string> Cities
        {
            get { return cities; }
            set
            {
                cities = value;
                OnPropertyChanged(nameof(Cities));
            }
        }
        private string _Roles;
        public string Roles
        {
            get { return _Roles; }
            set
            {
                _Roles = value;
                OnPropertyChanged(nameof(Roles));
            }
        }
        private ObservableCollection<string> _WHIDs;
        public ObservableCollection<string> WHIDs
        {
            get { return _WHIDs; }
            set
            {
                _WHIDs = value;
                OnPropertyChanged();
            }
        }
        private string _NewPhoneNumber;
        public string NewPhoneNumber
        {
            get { return _NewPhoneNumber; }
            set
            {
                _NewPhoneNumber = value;
                OnPropertyChanged(nameof(NewPhoneNumber));
            }
        }
        private string _RoleText;
        public string RoleText
        {
            get { return _RoleText; }
            set
            {
                _RoleText = value;
                OnPropertyChanged(nameof(RoleText));

            }
        }
        private string _NewEmployeeName;
        public string NewEmployeeName
        {
            get { return _NewEmployeeName; }
            set
            {
                _NewEmployeeName = value; OnPropertyChanged(nameof(NewEmployeeName));
            }
        }
        public ICommand RefreshCommand { get; set; }
        public ICommand AcceptCommand { get; set; }
        private AddEmployeeManager AEM;

        public ManageEmployeesModel()
        {
            AEM = new AddEmployeeManager();
            Cities = DictionaryData.GetCities();

            WHIDs = DictionaryData.GetWHIDs();
            RefreshCommand = new RelayCommand<object>((t) => { return !string.IsNullOrEmpty(RoleText); }, (t) =>
            {
                RoleText = "";
                ClearView();

            });
            AcceptCommand = new RelayCommand<object>((t) => { return !string.IsNullOrEmpty(RoleText) && IsValid(NewPhoneNumber); }, (t) =>
            {
                Account a = GetAccountRecord(RoleText);
                if (a == null) CreateNewEmployee();

                if (a != null) UpdateEmployee();

            });
        }


        //public int AddMethod = 0;
        //public int UpdateMethod = 0;
        private string _WHID;
        public string WHID
        {
            get { return _WHID; }
            set { _WHID = value; OnPropertyChanged(WHID); }
        }

        private string _NewLocation;
        public string NewLocation
        {
            get { return _NewLocation; }
            set
            {
                _NewLocation = value; OnPropertyChanged(NewLocation);
            }
        }
        public bool CheckIsEmployee(string str)
        {
            return AEM.CheckIsEmployee(str);
        }
        public Account GetAccountRecord(string roleID)
        {
            return AEM.GetAccountRecord(roleID);
        }

        public Receptionist GetReceptionistRecord(string receptionistID)
        {
            return AEM.GetReceptionistRecord(receptionistID);
        }

        public WarehouseStaff GetWHStaffRecord(string WHStaffID)
        {
            return AEM.GetWHStaffRecord(WHStaffID);
        }

        public void ValidateRoleText()
        {

            if (RoleText.Length > 1 && (RoleText[0] == 'R' || RoleText[0] == 'S'))
            {
                for (int i = 1; i < RoleText.Length; i++)
                {
                    if (!char.IsDigit(RoleText[i]))
                    {
                        // Ký tự không phải là số
                        MessageBoxWindow.Show("Mã chức vụ vừa nhập không hợp lệ");
                        return;
                    }
                }

                // Chuỗi hợp lệ
                return;
            }
            else
            {
                // Chuỗi không chứa 'R' hoặc 'S' ở đầu
                MessageBoxWindow.Show("Mã chức vụ vừa nhập không hợp lệ");
            }
        }
     
        //public int GetRole()
        //{
        //    if (RoleText.Contains("R")) return 1;
        //    else if (RoleText.Contains("S")) return 2;
        //    else return 0;

        //}
        public void SetView()
        {
            ValidateRoleText();
            //tim thay tai khoan
            if (GetAccountRecord(RoleText) != null)
            {
                //update
                //UpdateMethod = 1;
                if (AEM.GetRole(RoleText) == 1)
                {
                    NewAccountName = GetAccountRecord(RoleText).accountName;

                    Roles = "Lễ tân kho";
                    NewPhoneNumber = GetReceptionistRecord(RoleText).receptionistPhoneNumber;
                    NewLocation = GetReceptionistRecord(RoleText).receptionistLocation;
                    NewEmployeeName = GetReceptionistRecord(RoleText).receptionistName;
                    WHID = GetReceptionistRecord(RoleText).warehouseID;

                }
                else if (AEM.GetRole(RoleText) == 2)
                {
                    NewAccountName = GetAccountRecord(RoleText).accountName;

                    Roles = "Nhân viên kho";
                    NewPhoneNumber = GetWHStaffRecord(RoleText).warehouseStaffPhoneNumber;
                    NewLocation = GetWHStaffRecord(RoleText).warehouseStaffLocation;
                    NewEmployeeName = GetWHStaffRecord(RoleText).warehouseStaffName;
                    WHID = GetWHStaffRecord(RoleText).warehouseID;
                }
                else ClearView();

            }
            else
            {
                //add
                //AddMethod = 1;
                ClearView();

                if (AEM.GetRole(RoleText) == 1)
                {


                    Roles = "Lễ tân kho";
                }
                else
                {
                    Roles = "Nhân viên kho";
                }
            }

        }
        public void ClearView()
        {

            Roles = "";
            NewPhoneNumber = "";
            NewAccountName = "";
            NewPassword = "";
            NewLocation = "";
            NewEmployeeName = "";
            WHID = "";
        }
        public void CreateNewEmployee()
        {

            bool isValid = AEM.IsValid(RoleText, NewPhoneNumber, NewAccountName, NewPassword);

            bool accountExist = AEM.IsAccountUserNameExist(NewAccountName);
            if (!accountExist)
            {
                AEM.CreateNewEmployee(RoleText, NewAccountName, NewPassword, NewPhoneNumber, NewLocation, NewEmployeeName, WHID);
                MessageBoxWindow.Show("Đã tạo mới tài khoản");
            }
            else
            {
                MessageBoxWindow.Show("Tài khoản đã tồn tại");
            }

        }
        public bool IsAccountUserNameExist(string NewAccountName)
        {

            return AEM.IsAccountUserNameExist(NewAccountName);

        }
        public bool IsValid(string inputStr)
        {
            Account a = GetAccountRecord(RoleText);
            string pattern = @"^0\d{9}$"; // Định dạng số điện thoại gồm 10 chữ số và có số 0 ở trước

            if (inputStr?.Length > 0 && Regex.IsMatch(inputStr, pattern) && !string.IsNullOrEmpty(NewPhoneNumber))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void UpdateEmployee()
        {
            AEM.UpdateEmployee(RoleText, NewAccountName, NewPassword, NewPhoneNumber, NewLocation, NewEmployeeName, WHID);

            MessageBoxWindow.Show("Đã cập nhật tài khoản");
        }

    }
}
