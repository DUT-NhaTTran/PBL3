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

namespace Demo1.View
{
    /// <summary>
    /// Interaction logic for UpdateUserInfoView.xaml
    /// </summary>
    public partial class UpdateUserInfoView : UserControl
    {
        public UpdateUserInfoView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = (UpdateUserInfoViewModel)DataContext;
            if (viewModel != null)
            {
               viewModel.SetView();
                Locationcbb.SelectedItem = viewModel.UserLocation;

             

            }
            
        }
    }
}
