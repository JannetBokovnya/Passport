using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Media;
using Media.Interfaces;
using Passpot.Business;
using Services.ServiceReference;
using Telerik.Windows.Controls;
using System.Windows.Media;
using Telerik.Windows.Controls.GridView;
using System.Collections.Generic;
using Passpot.Model;

namespace Passpot.Controls
{
    public partial class TreeControlView : UserControl
    {
        public TreeModel Model
        {
            get { return DataContext as TreeModel; }
        }

 
        public TreeControlView()
        {
            InitializeComponent();
            
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Model.PropertyChanged -= Model_PropertyChanged;
            Model.PropertyChanged += Model_PropertyChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;


        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            if (e.PropertyName.Equals("TreeModelInited"))
            {
                
               TreeControl_Only treeControlOnly;
                if (Model.TreeOpenParam.TypeTree == "2")
                {
                    TreeHolder.Children.Clear();
                    treeControlOnly = TreeControl_Only.CreateTree("123", "2", Model.TreeOpenParam.VisibleNode, Model.SelectionPath);
                }
                else
                {
                    treeControlOnly = TreeControl_Only.CreateTree(Model.TreeOpenParam.RootKey, Model.TreeOpenParam.TypeTree, Model.TreeOpenParam.VisibleNode, Model.SelectionPath);
                }

                treeControlOnly.DataContext = Model;
                TreeHolder.Children.Add(treeControlOnly);
            }
        }
        private void btnRefreshTree_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnVisibleAll_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnBack_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnForward_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Tree_findTabControl_OnSelectionChanged(object sender, RadSelectionChangedEventArgs e)
        {
            
        }

        private void GridObjType_OnSelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            
        }

        private void GridObjType_OnRowLoaded(object sender, RowLoadedEventArgs e)
        {
            
        }

        private void TextBoxFilterValue_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

 
    }
}