
using Demo1.UserInfo;
using System;
using System.Linq;
using System.Security.Cryptography;
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
        //void Login(Window p)
        //{
        //    using (var context = new Model.PBL3_demoEntities())
        //    {
        //        string hashedPassword = HashPassword(Password);
        //        var count = context.Accounts.FirstOrDefault(x => x.accountName == UserName && x.accountPassword == hashedPassword);
        //        if (count != null)
        //        {

        //            isLogin = true;
        //            LoginID = count.accountID;
        //            accessRight = rolePermission(LoginID);
        //            AccountManager.Instance.SetLoginInfo(count.accountID, count.accountName, count.accountPassword,count.accessRightID);
        //            p.Close();
        //        }
        //        else
        //        {
        //            isLogin = false;
        //            MessageBoxWindow.Show("Sai tài khoản hoặc mật khẩu!");
        //        }
        //    }
        //}
        void Login(Window p)
        {
            UpdatePasswords();
            using (var context = new Model.PBL3_demoEntities())
            {
                // Lấy thông tin người dùng từ cơ sở dữ liệu
                var user = context.Accounts.FirstOrDefault(x => x.accountName == UserName);

                if (user != null)
                {
                    // Kiểm tra mật khẩu
                    if (VerifyPassword(Password, user.accountPassword, user.Salt))
                    {
                        // Mật khẩu hợp lệ, thực hiện các thao tác tiếp theo
                        isLogin = true;
                        LoginID = user.accountID;
                        accessRight = rolePermission(LoginID);
                        AccountManager.Instance.SetLoginInfo(user.accountID, user.accountName, user.accountPassword,user.accessRightID);
                        p.Close();
                    }
                    else
                    {
                        // Mật khẩu không đúng
                        isLogin = false;
                        MessageBoxWindow.Show("Sai mật khẩu!");
                    }
                }
                else
                {
                    // Người dùng không tồn tại
                    isLogin = false;
                    MessageBoxWindow.Show("Không tồn tại tài khoản!");
                }
            }
        }
        // Hàm băm mật khẩu với salt
        string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                string saltedPassword = password + salt; // Kết hợp mật khẩu và salt
                byte[] hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(saltedPassword));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        // Hàm xác minh mật khẩu
        bool VerifyPassword(string password, string hashedPassword, string salt)
        {
            string hashedInput = HashPassword(password, salt); // Băm mật khẩu nhập vào với salt
            return hashedPassword == hashedInput; // So sánh chuỗi băm
        }
        int rolePermission(string loginID)
        {
            char firstChar = loginID.Substring(0, 1)[0];
            if (firstChar == 'R') return 1;
            else if (firstChar == 'S') return 2;
            else if (firstChar == 'M') return 3;
            return -1;
        }
        string GenerateSalt()
        {
            byte[] saltBytes = new byte[16]; // Độ dài salt có thể thay đổi
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }
        void UpdatePasswords()
        {
            using (var context = new Model.PBL3_demoEntities())
            {
                var accounts = context.Accounts.ToList();

                foreach (var account in accounts)
                {
                    if (account.Salt == null)
                    {
                        string salt = GenerateSalt(); // Tạo salt mới
                        string hashedPassword = HashPassword(account.accountPassword, salt); // Băm mật khẩu với salt mới

                        account.Salt = salt; // Cập nhật giá trị salt
                        account.accountPassword = hashedPassword; // Cập nhật mật khẩu đã băm

                        context.SaveChanges(); // Lưu các thay đổi vào cơ sở dữ liệu
                    }
                }
            }
        }

    }
}
