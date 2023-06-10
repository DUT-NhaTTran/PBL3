using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Demo1.ViewModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Demo1.View
{
    /// <summary>
    /// Interaction logic for ManageEmployeesView.xaml
    /// </summary>
    public partial class ManageEmployeesView : UserControl
    {
        public ManageEmployeesView()
        {
            InitializeComponent();
        }



        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var viewModel = (ManageEmployeesModel)DataContext;

            if (viewModel != null)
            {
                viewModel.RoleText = ((TextBox)sender).Text; // Cập nhật giá trị SearchParcelText từ TextBox

                //MessageBox.Show(viewModel.SearchParcelText); // Hiển thị kết quả mới nhất
                viewModel.SetView(); // Tải danh sách Parcel mới dựa trên kết quả tìm kiếm
                Locationcbb.SelectedItem = viewModel.NewLocation;
                WHIDcbb.SelectedItem = viewModel.WHID;


            }
        }
    }
}