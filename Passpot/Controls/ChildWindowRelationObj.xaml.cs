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
using Media.Resources;
using linkControl.Control;
using Media;
using Media.Interfaces;
using Passpot.Business;
using Passpot.Grid;
using Passpot.Model;
using Services.ServiceReference;
using Telerik.Windows.Controls;

namespace Passpot.Controls
{
    public partial class ChildWindowRelationObj : ChildWindow
    {
        private ServiceDataClient _serviceDataClient;
        private PassportDetailModel _passportModel;
        private FieldMetaDataItem _metaData;
        private List<DataConnectList> _selectConnectList;
        private PopUpChildWindow _linkTableWindow;
        private string _keyObj;

        public string _keyPassportRelation;


        private ServiceDataClient ServiceDataClient
        {
            get
            {
                if (_serviceDataClient == null)
                    _serviceDataClient = new ServiceDataClient();
                return _serviceDataClient;
            }
        }
        public ChildWindowRelationObj()
        {
            //LocalizationManager.DefaultResourceManager = ResourceGrid.ResourceManager;
            InitializeComponent();
           
        }

        public void StubLinkData(List<DataConnectList> DataConnectList)
        {

            OnGetRelationListCompleted(DataConnectList);
            

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        public void BuildChildRelationObj(string keyObj, string keyEntity, FieldMetaDataItem metaData, List<DataListRelationObjItems> dataRelationgrid, PassportDetailModel passportModel, int typeVisible)
        {
            if (typeVisible == 1)
            {
                newRelationButton.Visibility = Visibility.Collapsed;
                deleteRelationButton.Visibility = Visibility.Collapsed;
            }
            grid.ItemsSource = dataRelationgrid;
            _passportModel = passportModel;
            _metaData = metaData;
            _keyObj = keyObj;
            ////получаем данные для таблицы связей(название поля - обязательно!!!!!!!!)
            //ServiceDataClient.GetDataRelationObjCompleted += GetDataRelationObjCompleted;
            //ServiceDataClient.GetDataRelationObjAsync(keyObj, metaData.FLDKEY.ToString());
        }

       
        public void OnGetRelationListCompleted(List<DataConnectList> fieldDataConnectList)
        {

            //comboBoxChild.ItemsSource = fieldDataConnectList;
            //comboBoxChild.DisplayMemberPath = "NAMECONNECT";
            //comboBoxChild.SelectedIndex = 0;
        }

       

        private void grid_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            _keyPassportRelation = ((DataListRelationObjItems)grid.SelectedItem).KeyObj;
        }

        private void newDetalPassportButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void newRelationButton_Click(object sender, RoutedEventArgs e)
        {
            
            
            
            //_passportModel.StartOnDataConnect(_passportModel.PassportDetailOpenParams.EntityKey, _metaData.FLDNAME, "b");

            ServiceDataClient.GetDataConnectCompleted += OnGetDataConnectCompleted;
            ServiceDataClient.GetDataConnectAsync(_passportModel.PassportDetailOpenParams.EntityKey, _metaData.FLDNAME, GlobalContext.Context);
            
        }

        void OnGetDataConnectCompleted(object sender, GetDataConnectCompletedEventArgs e)
        {
            ServiceDataClient.GetDataConnectCompleted -= OnGetDataConnectCompleted;
            if (e.Result.IsValid)
            {
                _selectConnectList = new List<DataConnectList>(e.Result.DataConnectLists);
                var m = DataContext as PassportDetailModel;

                _linkTableWindow = new PopUpChildWindow();
                _linkTableWindow.Title = ProjectResources.LinkTableTitle;
                _linkTableWindow.DataContext = m;
                _linkTableWindow.StubLinkData(_selectConnectList);
                _linkTableWindow.Show();
                _linkTableWindow.btnOk.Click += ButtonOkClick;

            }
            else
            {
                //MainModel.Report("Список связей OnGetDataConnect err = " + e.Result.ErrorMessage);
            }
        }

        public void ButtonOkClick(object sender, RoutedEventArgs e)
        {
            //_linkTableWindow.btnOk.Click -= ButtonOkClick;
            //newvalue_c = _linkTableWindow._keyPassportConnect;
            //ServiceDataClient.GetNameObj_OnObjKeyCompleted += OnObjConnectKeyCompleted;
            //ServiceDataClient.GetNameObj_OnObjKeyAsync(_linkTableWindow._keyPassportConnect);
            // textBox.Text = _linkTableWindow._keyPassportConnect;

            //если сохраняем связь то вызываем сервис добавления связи
            _linkTableWindow.btnOk.Click -= ButtonOkClick;
            
            //В начале сохраняем связь в таблицей связей!!!
            ServiceDataClient.SaveRelationObjCompleted += ServiceDataClient_SaveRelationObjCompleted;
            ServiceDataClient.SaveRelationObjAsync(_passportModel.PassportKey, _metaData.FLDKEY.ToString(), _linkTableWindow._keyPassportConnect, GlobalContext.Context);

        }


        void ServiceDataClient_SaveRelationObjCompleted(object sender, SaveRelationObjCompletedEventArgs e)
        {
            ServiceDataClient.SaveRelationObjCompleted -= ServiceDataClient_SaveRelationObjCompleted;

            if (e.Result.IsValid)
            {
                if (e.Result.KeyRelation_OnSaveObj_result.KeyRelation_OnSaveObj != "0")
                {
                    //перегружаем таблицу связей
                    ServiceDataClient.GetDataRelationObjCompleted += GetDataRelationObjCompleted;
                    ServiceDataClient.GetDataRelationObjAsync(_keyObj, _metaData.FLDKEY.ToString(), GlobalContext.Context);
                }
                else
                {
                    _passportModel.MainModel.Report("Не сохранилась связь в таблице связей!!!!!");
                }
            }
            else
            {
                _passportModel.MainModel.Report("Ошибка!!! - SaveRelationObj =  " + e.Result.ErrorMessage);
            }
        }


        void GetDataRelationObjCompleted(object sender, GetDataRelationObjCompletedEventArgs e)
        {
            if (e.Result.IsValid)
            {
                grid.ItemsSource = e.Result.DataListRelationObjList;
            }
        }



        private void OnEditButtonClick(object sender, RoutedEventArgs e)
        {
            string artifactId = GridHelper.GetArtifactIdByClick(sender, "KeyObj", grid);
            _passportModel.MainModel.NeedOpenPassportDetail = new PassportDetailOpenParam() { PassportKey = artifactId };
            

        }
    }
}

