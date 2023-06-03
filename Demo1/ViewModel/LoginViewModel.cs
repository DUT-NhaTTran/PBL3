
using Demo1.UserInfo;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Demo1.ViewModel
{
    class LoginViewModel : BaseViewModel
    {
        public string LoginID;
        public int accessRight;
        
        public bool isLogin { get; set; }
        private string _UserName;
        public string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                _UserName = value;
                OnPropertyChanged();
            }
        }
        
        private string _Password;
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public LoginViewModel()
        {
            isLogin = false;
            UserName = "";
            Password = "";
            LoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { Login(p); });
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });


        }
        void Login(Window p)
        {
            using (var context = new Model.PBL3_demoEntities())
            {
                var count = context.Accounts.FirstOrDefault(x => x.accountName == UserName && x.accountPassword == Password);
                if (count != null)
                {

                    isLogin = true;
                    LoginID = count.accountID;
                    accessRight = rolePermission(LoginID);
                    AccountManager.Instance.SetLoginInfo(count.accountID, count.accountName, count.accountPassword,count.accessRightID);
                    p.Close();
                }
                else
                {
                    isLogin = false;
                    MessageBoxWindow.Show("Sai tài khoản hoặc mật khẩu!");
                }
            }
        }
        
        int rolePermission(string loginID)
        {
            char firstChar = loginID.Substring(0, 1)[0];
            if (firstChar == 'R') return 1;
            else if (firstChar == 'S') return 2;
            else if (firstChar == 'M') return 3;
            return -1;
        }
       
    }
}
