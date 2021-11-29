// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System.Windows.Controls;
using System.Windows;
//using System.Windows.Controls.Samples;
using System.Windows.Data;
using System.Linq;
using Media.Interfaces;
using Passpot.Business;
using Passpot.Grid;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Services.ServiceReference;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using System;
using System.Net;
using System.Windows.Documents;
using System.Windows.Input;
using Passpot.Model;


namespace linkControl.Control
{
    /// <summary>
    /// Sample ChildWindow for nstration purposes.
    /// </summary>
    public partial class PopUpChildWindow : ChildWindow
    {
        private Collection<DataLinkList> _fieldLinkDataList;
        private List<DataConnectList> _fieldDataConnectList;
        private string _keyLink;
        private ServiceDataClient _serviceDataClient;
        GridData _gridData;
        public string _keyPassportConnect;

        public PopUpChildWindow()
        {
            //LocalizationManager.DefaultResourceManager = ResourceGrid.ResourceManager;
            InitializeComponent();
 
            //StubLinkData();
        }

        #region Properties

        #region Ctor

        public LinkModel LinkModel
        {
            get { return DataContext as LinkModel; }
        }

        public PassportDetailModel Model
        {
            get { return DataContext as PassportDetailModel; }
        }

        #endregion

        #endregion Properties

        #region Private methods

        public void StubLinkData(List<DataConnectList> DataConnectList)
        {

            OnGetGetConnectListCompleted(DataConnectList);
           

        }

        




        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used by event defined in Xaml.")]
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {


            //_keyPassportConnect = GridHelper.GetArtifactIdByClick(sender, "ARTIFACT_ID", grid);
            
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //GridHelper.BuildPasportGrid(grid,null,Model);
        }

        #endregion

        private ServiceDataClient ServiceDataClient
        {
            get
            {
                if (_serviceDataClient == null)
                    _serviceDataClient = new ServiceDataClient();
                return _serviceDataClient;
            }
        }


        public void OnGetGetConnectListCompleted(List<DataConnectList> fieldDataConnectList)
        {

            comboBoxLink.ItemsSource = fieldDataConnectList;
            comboBoxLink.DisplayMemberPath = "NAMECONNECT";
            comboBoxLink.SelectedIndex = 0;
        }



        private void ComboBoxLink_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (comboBoxLink.SelectedItem != null)
            {
                _keyLink = ((DataConnectList)comboBoxLink.SelectedItem).KEYCONNECT;

                ServiceDataClient.GetGridDataCompleted += OnGetGridLinkDataCompleted;
                ServiceDataClient.GetGridDataAsync(_keyLink, "0", "1", Model.PassportKey, GlobalContext.Context);
               
            }

        }


        private void OnGetGridLinkDataCompleted(object sender, GetGridDataCompletedEventArgs e)
        {


            ServiceDataClient.GetGridDataCompleted -= OnGetGridLinkDataCompleted;
            _gridData = e.Result;

            if (_gridData != null)
            {
                ServiceDataClient.MetaDataCompleted += OnMetaDataCompleted;
                ServiceDataClient.MetaDataAsync(_keyLink, "1", GlobalContext.Context, "0");
            }



        }

        private void OnMetaDataCompleted(object sender, MetaDataCompletedEventArgs e)
        {
            ServiceDataClient.MetaDataCompleted -= OnMetaDataCompleted;

            List<FieldMetaDataItem> metaDataList = new List<FieldMetaDataItem>(e.Result.FieldMetaDataList);

            if (_gridData.Rows.Count == 0)
            {
                Model.MainModel.Report("ChildWindow нет данных " + e.Result.ErrorMessage);
            }
            else
            {
                try
                {

                    IGridData connectGridData = new ConnectGridData(metaDataList, _gridData);
                    var template = Resources["buttonEditDeleteCellTemplate"] as DataTemplate;
                    var cellStyle = Resources["StyleCell"] as Style;
                    var cellStyleAll = Resources["StyleCellAll"] as Style;
                    var headerCellStyle = Resources["MyHeaderCellStyle"] as Style;

                    GridHelper.BuildPasportGrid(grid, null, cellStyle, cellStyleAll, headerCellStyle, connectGridData, true);
                    //GridHelper.BuildPasportGrid(grid, template, connectGridData, true);
                   
                }
                catch (Exception ex)
                {
                    Model.MainModel.Report(ex.Message);
                }
            }
        }


        private void okButton_Click(object sender, RoutedEventArgs e)
        {

            _keyPassportConnect = GridHelper.GetArtifactIdByClick(e.OriginalSource, "ARTIFACT_ID", grid);

        }

        private void grid_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            var tt = grid.SelectedItem.GetType().GetProperty("ARTIFACT_ID").GetValue(grid.SelectedItem, null);
            _keyPassportConnect = tt.ToString();
        }

 
       
    }
}



