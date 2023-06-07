using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Demo1.View
{
    public partial class CustomMessageBox : INotifyPropertyChanged
    {
        private string _message;

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public CustomMessageBox()
        {
            InitializeComponent();
            DataContext = this;
            //Với việc gán DataContext là this, các thành phần con trong UserControl có thể liên kết dữ liệu trực tiếp với các thuộc tính của CustomMessageBox mà không cần phải tham chiếu đến một nguồn dữ liệu khác.
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
  
}
