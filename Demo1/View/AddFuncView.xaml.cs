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

namespace Demo1
{
    /// <summary>
    /// Interaction logic for AddFuncView.xaml
    /// </summary>
    public partial class AddFuncView : UserControl
    {
        public AddFuncView()
        {
            InitializeComponent();
        }

        //private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var comboBox = sender as ComboBox;
        //    var selectedCity = comboBox.SelectedItem as string;
        //    var viewModel = DataContext as AddFunctionModel;
        //    viewModel.SCustomerCity = selectedCity;
        //    viewModel.HandleSelectedSCity();
        //}

        //private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        //{
        //    var comboBox1 = sender as ComboBox;
        //    var selectedCity = comboBox1.SelectedItem as string;
        //    var viewModel = DataContext as AddFunctionModel;
        //    viewModel.RCustomerCity = selectedCity;
        //    viewModel.HandleSelectedRCity();
        //}
    }
}
