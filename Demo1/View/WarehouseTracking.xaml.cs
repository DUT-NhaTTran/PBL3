﻿using Demo1.ViewModel;
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
using static Demo1.ViewModel.WarehouseTrackingModel;
using Demo1.UserInfo;
using Demo1.Model;
using Caliburn.Micro;
using ParcelInfo = Demo1.ViewModel.WarehouseTrackingModel.ParcelInfo;



namespace Demo1.View
{
    /// <summary>
    /// Interaction logic for WarehouseTracking.xaml
    /// </summary>
    public partial class WarehouseTracking : UserControl
    {
        public WarehouseTracking()
        {
            InitializeComponent();
        }
       
        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var listBox = (ListBox)sender;
            var viewModel = (WarehouseTrackingModel)DataContext;
            var warehouseInfo = listBox.SelectedItem as WarehouseInfo;

            if (warehouseInfo != null)
            {
                viewModel.WarehouseID = warehouseInfo.WarehouseID;
                viewModel.WarehouseName = warehouseInfo.WarehouseName;
                viewModel.WHAddress = WarehouseData.Instance.GetWarehouseAddress(viewModel.WarehouseID);
                viewModel.WHCapacity = Convert.ToString(WarehouseData.Instance.GetWarehouseCapacity(viewModel.WarehouseID));
                viewModel.WHContained=viewModel.FindContained(viewModel.WarehouseID);
                //TabControl_SelectionChanged(tabControl, new SelectionChangedEventArgs(TabControl.SelectionChangedEvent, new List<TabItem>(), new List<TabItem>()));
               
                MyTab2Function();

            }
        }

        //private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (e.Source is TabControl tabControl && tabControl.SelectedItem is TabItem selectedTab)
        //    {
        //        if (selectedTab.Header.ToString() == "Thông tin các đơn hàng trong kho")
        //        {
        //            var viewModel = (WarehouseTrackingModel)DataContext;
        //            viewModel.LoadParcelInfoList(viewModel.WarehouseID);

        //        }

        //        // Các tab khác có thể được xử lý tương tự
        //    }
        //}
        private void MyTab2Function()
        {
            var viewModel = (WarehouseTrackingModel)DataContext;
            viewModel.LoadParcelInfoList(viewModel.WarehouseID);
        }


    }
}
