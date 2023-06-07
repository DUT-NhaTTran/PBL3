using Demo1.Model;
using Demo1.ViewModel;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Demo1
{
    /// <summary>
    /// Interaction logic for StatisticsView.xaml
    /// </summary>
    public partial class StatisticsView : UserControl
    {
        public StatisticsView()
        {
            InitializeComponent();
        }
      

     
       
        
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
         
            if (sender is DatePicker datePicker)
            {
                if (datePicker.SelectedDate.HasValue)
                {
                    DateTime selectedDate = datePicker.SelectedDate.Value;
                   
                    ((StatisticModel)DataContext).SelectedDateChangedAction(selectedDate);
                   
                    using (var context = new PBL3_demoEntities())
                    {
                        if (DataContext is StatisticModel statisticModel)
                        {

                            statisticModel.SortedWarehouseString = statisticModel.ToStringAfterSort(
                                statisticModel.SortedWarehouseToView, "del", statisticModel.SelectedMonth,
                                statisticModel.SelectedYear, context);
                            statisticModel.Change();
                        }
                    }
                }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var viewModel = (StatisticModel)DataContext;

            if (viewModel != null)
            {
                viewModel.SearchWHID = ((TextBox)sender).Text; // Cập nhật giá trị SearchWHIDText từ TextBox

                //MessageBox.Show(viewModel.SearchParcelText); // Hiển thị kết quả mới nhất
                viewModel.LoadAllWHInfoSearched(); // Tải danh sách Parcel mới dựa trên kết quả tìm kiếm

            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            chart.Visibility = Visibility.Visible;
        }
    }
}
