using Demo1.UserInfo;
using Demo1.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Demo1.ViewModel.SearchParcelModel;
using static Demo1.ViewModel.WarehouseTrackingModel;

namespace Demo1.View
{
    /// <summary>
    /// Interaction logic for SearchParcel.xaml
    /// </summary>
    public partial class SearchParcel : UserControl
    {
        public SearchParcel()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            var viewModel = (SearchParcelModel)DataContext;
           
            if (viewModel != null)
            {
                viewModel.SearchParcelText = ((TextBox)sender).Text; // Cập nhật giá trị SearchParcelText từ TextBox
                
                //MessageBox.Show(viewModel.SearchParcelText); // Hiển thị kết quả mới nhất
                viewModel.LoadAllParcelSearched(); // Tải danh sách Parcel mới dựa trên kết quả tìm kiếm

            }



        }

        //private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{

        //    var listBox = (ListBox)sender;
        //    var viewModel = (SearchParcelModel)DataContext;
        //    var selectedItem = listBox.SelectedItem as ParcelInfoInSearch;

        //    if (selectedItem != null)
        //    {
        //        var textBlock = (TextBlock)e.OriginalSource;
        //        var id = textBlock.Text;
        //        viewModel.SearchParcelText = id;
               
        //    }

        //    viewModel.OpenResultOfSerchWindow();
          

        //}

        private void IDTextBlock_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

            var textBlock = (TextBlock)sender;
            var id = textBlock.Text;

            var viewModel = (SearchParcelModel)DataContext;
            viewModel.SearchParcelText = id;
            viewModel.OpenResultOfSerchWindow();

        }

        //private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    ResultOfSearch r = new ResultOfSearch();
        //    r.ShowDialog(); 


        //}
    }
}
