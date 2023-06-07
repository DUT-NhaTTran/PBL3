using Demo1.Model;
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
using static Demo1.ViewModel.OrderTrackingModel;

namespace Demo1.View
{
    /// <summary>
    /// Interaction logic for AddFuncView.xaml
    /// </summary>
    public partial class OrderTracking : UserControl
    {

        public OrderTracking()
        {
            InitializeComponent();
        }
       
        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var listBox = (ListBox)sender;
            var viewModel = (OrderTrackingModel)DataContext;
            var parcelInfo = listBox.SelectedItem as ParcelInfo;

            if (parcelInfo != null)
            {
                viewModel.ParcelID = Convert.ToString(parcelInfo.ID);
                viewModel.ParcelTrackingCommand.Execute(null);
            }
        }
    }
}