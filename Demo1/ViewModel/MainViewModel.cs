using Demo1.UserInfo;
using Demo1.View;
using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static Demo1.UserInfo.AccountManager;

namespace Demo1.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
            //moi thu xu ly o day = datacontext
            public bool isLoaded = false;
            public bool isRole1=false, isRole2 = false,isRole3 =false;
            private bool isRole1AllowedToVisible;
            private bool isRole2AllowedToVisible;
            private bool isRole3AllowedToVisible;
            private bool _Role23;
            private bool _Role12;
            public bool Role23
            {
                get
                {
                    return _Role23;

                }
                set
                {
                    _Role23 = value;
                    OnPropertyChanged(nameof(Role23));
                }
            }
            public bool Role12
            {
                get
                {
                    return _Role12;

                }
                set
                {
                    _Role12 = value;
                    OnPropertyChanged(nameof(Role12));
                }
            }
            public bool IsRole1AllowedToVisible
            {
                get { return isRole1AllowedToVisible; }
                set
                {
                    isRole1AllowedToVisible = value;
                    OnPropertyChanged("IsRole1AllowedToVisible");
                }
            }

            public bool IsRole2AllowedToVisible
            {
                get { return isRole2AllowedToVisible; }
                set
                {
                    isRole2AllowedToVisible = value;
                    OnPropertyChanged("IsRole2AllowedToVisible");
                }
            }

            public bool IsRole3AllowedToVisible
            {
                get { return isRole3AllowedToVisible; }
                set
                {
                    isRole3AllowedToVisible = value;
                    OnPropertyChanged("IsRole3AllowedToVisible");
                }
            }
            
            public bool IsRole1Or2AllowedToVisible
            {
                get { return IsRole1AllowedToVisible || IsRole2AllowedToVisible; }
             
            }
            public bool IsRole2Or3AllowedToVisible
            {
                get { return IsRole2AllowedToVisible || IsRole3AllowedToVisible; }
               
            }

            //Command
            public ICommand LoadedWindowCommand { get; set; }         
            public ICommand ShowAddWindowCommand { get; set; }
            public ICommand ShowSearchWindowCommand { get; set; }          
            public ICommand ShowParcelWindowCommand { get; set; }
            public ICommand ShowWindow1Command { get; set; }
            public ICommand ShowUpdateParcelCommand { get; set; }
            public ICommand ShowOrderTrackingCommand { get; set; }
            public ICommand ShowWarehouseTrackingCommand { get; set; }
            public ICommand ShowStatisticCommand { get; set; }
            public ICommand ButtonCommand { get; set; }
            private BaseViewModel _currentChildView;
            private string _caption;
            private IconChar _icon;
         
            private string _roleName;
            private string _userName;

            private string _WHID;
            public string WHID
            {
                get { return _WHID; }
                set
                {
                    _WHID = value;
                    OnPropertyChanged();

                }
            }
            

            public BaseViewModel CurrentChildView
            {
                get
                {
                    return _currentChildView;
                }
                set
                {
                    _currentChildView = value;
                    OnPropertyChanged(nameof(CurrentChildView));
                }
            }
            public string Caption
            {
                get
                {
                    return _caption;

                }
                set
                {
                    _caption = value;
                    OnPropertyChanged(nameof(Caption));
                }
            }

            public IconChar Icon
            {
                get
                {
                    return _icon;
                }
                set
                {
                    _icon = value;
                    OnPropertyChanged(nameof(Icon));
                }
            }
            public string RoleName
            {
                get
                {
                    return _roleName;

                }
                set
                {
                    _roleName = value;
                    OnPropertyChanged(nameof(RoleName));
                }
            }
            public string UserName
            {
                get
                {
                    return _userName;

                }
                set
                {
                    _userName = value;
                    OnPropertyChanged(nameof(UserName));
                }
            }
        public MainViewModel()
        {
          
            LoadedWindowCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                isLoaded = true;
                if (p == null)
                    return;
                p.Hide();
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.ShowDialog();
                if (loginWindow.DataContext == null)
                return;
                    var LoginVM = loginWindow.DataContext as LoginViewModel;
                    if (LoginVM.isLogin)
                    {
                        int accessright = AccountManager.Instance.GetAccessRight();
                        string loginID = AccountManager.Instance.GetAccountID();
                        RoleName = AccountManager.Instance.GetRoleName(accessright);
                        UserName = AccountManager.Instance.GetUserName(loginID);
                        WHID = AccountManager.Instance.GetUserWarehouseID(AccountManager.Instance.GetAccountID());
                        if (accessright == 1) IsRole1AllowedToVisible = true;
                        else if (accessright == 2) IsRole2AllowedToVisible = true;
                        else if (accessright == 3) IsRole3AllowedToVisible = true;

                    if (IsRole2Or3AllowedToVisible) Role23 = true;
                    if (IsRole1Or2AllowedToVisible) Role12 = true;
                        p.Show();
                 
                    }
                    else
                    {
                        p.Close();
                    }
            });
            //default view

            ShowAddWindowCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                CurrentChildView = new AddFunctionModel();
                Caption = "Tạo đơn hàng";
                Icon = IconChar.UserGroup;
            });
            ShowSearchWindowCommand = new RelayCommand<object>((x) => { return true; }, (x) =>
            {
                CurrentChildView = new SearchParcelModel();
                Caption = "Tra cứu đơn";
                Icon = IconChar.UserGroup;
            });
            
            ShowUpdateParcelCommand = new RelayCommand<object>((o) => { return true; }, (o) =>
            {
                CurrentChildView = new UpdateParcelModel();
                Caption = "Cập nhật đơn hàng";
                Icon = IconChar.UserGroup;
            });
            ShowOrderTrackingCommand = new RelayCommand<object>((i) => { return true; }, (i) =>
            {
                CurrentChildView = new OrderTrackingModel();
                Caption = "Theo dõi đơn hàng";
                Icon = IconChar.UserGroup;
            });
            ShowWarehouseTrackingCommand = new RelayCommand<object>((i) => { return true; }, (e) =>
            {
                CurrentChildView = new WarehouseTrackingModel();
                Caption = "Theo dõi kho hàng";
                Icon = IconChar.UserGroup;
            });
            ShowStatisticCommand = new RelayCommand<object>((i) => { return true; }, (e) =>
            {
                CurrentChildView = new StatisticModel();
                Caption = "Thống kê";
                Icon = IconChar.UserGroup;
            });
        }
    }
}
