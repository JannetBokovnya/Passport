using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Media.Interfaces;
using Passpot.Business;
using Passpot.Grid;
using Passpot.Model;
using Services.ServiceReference;
using Telerik.Windows.Controls;

namespace Passpot.Controls
{
    public partial class ChildWindowLinkObj : ChildWindow
    {
        private string _keyLink;
        private ServiceDataClient _serviceDataClient;
        private string fldnameforLink;
        public string _keyPassportConnect;
        public string keyLinkOnGridItem;
        public DataListLinkedObjectsItems LinkOnGridItem;
        public string NameEntity = "";
        private string _nameEntity = "";
        private string  selectRelationObjGridList="";

        private ServiceDataClient ServiceDataClient
        {
            get
            {
                if (_serviceDataClient == null)
                    _serviceDataClient = new ServiceDataClient();
                return _serviceDataClient;
            }
        }

        public ChildWindowLinkObj()
        {
            InitializeComponent();
        }

        #region Properties

        #region Ctor

        //public LinkModel LinkModel
        //{
        //    get { return DataContext as LinkModel; }
        //}

        public PassportDetailModel Model
        {
            get { return DataContext as PassportDetailModel; }
        }

        #endregion

        #endregion Properties

        #region Private methods

        public void StubLinkData(List<DataConnectList> DataConnectList, string fieldname, string SelectRelationObjGridList)
        {
            fldnameforLink = fieldname;
            selectRelationObjGridList = SelectRelationObjGridList;

            if (DataConnectList.Count > 1)
            {
                tbNameObj.Visibility = Visibility.Collapsed;
                OnGetGetConnectListCompleted(DataConnectList);    
            }
            else
            {
                comboBoxLink.Visibility = Visibility.Collapsed;
                tbNameObj.Text = DataConnectList[0].NAMECONNECT;
                _nameEntity = DataConnectList[0].NAMECONNECT;

                ServiceDataClient.GetListLinkedObjectsCompleted += ServiceDataClient_GetListLinkedObjectsCompleted;
                ServiceDataClient.GetListLinkedObjectsAsync(Model.PassportKey, Model.EntityKey,
                                                            fldnameforLink, DataConnectList[0].KEYCONNECT, selectRelationObjGridList, GlobalContext.Context);
            }
            

        }

        public void OnGetGetConnectListCompleted(List<DataConnectList> fieldDataConnectList)
        {
            
            comboBoxLink.ItemsSource = fieldDataConnectList;
            comboBoxLink.DisplayMemberPath = "NAMECONNECT";
            comboBoxLink.SelectedIndex = 0;
        }


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

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


        private void ComboBoxLink_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (comboBoxLink.SelectedItem != null)
            {
                _keyLink = ((DataConnectList)comboBoxLink.SelectedItem).KEYCONNECT;

                _nameEntity = ((DataConnectList) comboBoxLink.SelectedItem).NAMECONNECT;


                ServiceDataClient.GetListLinkedObjectsCompleted += ServiceDataClient_GetListLinkedObjectsCompleted;
                ServiceDataClient.GetListLinkedObjectsAsync(Model.PassportKey, Model.EntityKey,
                                                            fldnameforLink, _keyLink, selectRelationObjGridList, GlobalContext.Context);

            }

        }

        void ServiceDataClient_GetListLinkedObjectsCompleted(object sender, GetListLinkedObjectsCompletedEventArgs e)
        {

            ServiceDataClient.GetListLinkedObjectsCompleted -= ServiceDataClient_GetListLinkedObjectsCompleted;
             if (e.Result.IsValid)
             {
                 Model.MainModel.Report(" GetListLinkedObjectsCompleted OK" );
                 List<DataListLinkedObjectsItems> _selectDataListLinkedObjectsGridList;
                 _selectDataListLinkedObjectsGridList = new List<DataListLinkedObjectsItems>(e.Result.DataListLinkedObjectsList);
                 grid.ItemsSource = _selectDataListLinkedObjectsGridList;
                 
             }
             else
             {
                 Model.MainModel.Report("Ошибка GetListLinkedObjectsCompleted" + e.Result);
             }

        }



        private void grid_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            LinkOnGridItem = ((DataListLinkedObjectsItems)grid.SelectedItem);
            NameEntity = _nameEntity;
        }

    }
}

