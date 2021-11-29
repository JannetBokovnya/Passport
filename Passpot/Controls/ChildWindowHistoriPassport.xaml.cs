using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Services.ServiceReference;
using Telerik.Windows.Controls;

namespace Passpot.Business
{
    public partial class ChildWindowHistoriPassport : ChildWindow
    {
        public ChildWindowHistoriPassport()
        {
            //LocalizationManager.DefaultResourceManager = ResourceGrid.ResourceManager;
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        public void CreateHistoryGrid(List<DataListHistoryItems> fieldHistoryGridList, string namePassport)
        {
            tbD.Text = namePassport;
            //if (fieldHistoryGridList.Count == 0)
            //{
            //    tbD.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    tbD.Visibility = Visibility.Collapsed;
            //}
            gridHistory.ItemsSource = fieldHistoryGridList;
            
            
        }
    }
}

