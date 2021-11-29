using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Media;
using Media.Interfaces;
using Passpot.Business;
using Passpot.Grid;
using System.Collections.Generic;
using Passpot.Controls;
using Media.Resources;
using Passpot.Model;
using Services.ServiceReference;
using Telerik.Windows.Controls;

namespace Passpot
{
    public partial class PassportChildView : UserControl
    {
        #region Private fields

        private string _currentPassportKey;
        private ServiceDataClient _serviceDataClient;
        private ChildWindowDelete _popupWindowDeleteChild;
        private Collection<DataChildList> _fieldDataChildList;
        private string _keyChild;
        private string artifactIdOnDeleteChild;
        GridData _gridData;

        // public List<FieldMetaDataItem> MetaDataListChildGrid { get; private set; }

        #endregion Private fields

        #region Properties

        private ServiceDataClient ServiceDataClient
        {
            get
            {
                if (_serviceDataClient == null)
                    _serviceDataClient = new ServiceDataClient();
                return _serviceDataClient;
            }
        }
        #endregion Properties

        public PassportChildView()
        {
            //LocalizationManager.DefaultResourceManager = ResourceGrid.ResourceManager;
            InitializeComponent();
            _popupWindowDeleteChild = new ChildWindowDelete();
        }

        private PassportDetailModel Model
        {
            get { return this.DataContext as PassportDetailModel; }
        }

        #region Public methods

        public void InitControl(string passportKey, bool forceInit)
        {

            if (passportKey != _currentPassportKey || forceInit)
            {
                reportChild.Visibility = Visibility.Collapsed;
                _currentPassportKey = passportKey;
                Model.StartGetDataChild(Model.EntityKey);

            }
            
            //if (countChild != 0)
            //{
            //    if (passportKey != _currentPassportKey || forceInit)
            //    {
            //        reportChild.Visibility = Visibility.Collapsed;
            //        _currentPassportKey = passportKey;


            //        //ServiceDataClient.GetChildListCompleted += OnGetGetChildListCompleted;
            //        //ServiceDataClient.GetChildListAsync(_currentPassportKey, MediaType);

            //        // заметь строку на верхние
            //        //OnGetGetChildListCompleted();
            //        Model.StartGetDataChild(Model.EntityKey);
            //        //comboBoxChild.ItemsSource = Model.SelectChildList;
            //        //comboBoxChild.DisplayMemberPath = "NAMECHILDS";
            //        //comboBoxChild.SelectedIndex = 0;

            //    }
            //}
            //else
            //{
            //    comboBoxChild.Visibility = Visibility.Collapsed;
            //    buNuwChild.Visibility = Visibility.Collapsed;
            //    grid.Visibility = Visibility.Collapsed;
            //    reportChild.Visibility = Visibility.Visible;
            //}
        }

        #endregion

        public void OnGetGetChildListCompleted(List<DataChildList> fieldDataChildList)
        {
            if (fieldDataChildList.Count != 0)
            {
                //comboBoxParent.ItemsSource = null;
                comboBoxChild.ItemsSource = fieldDataChildList;
                comboBoxChild.DisplayMemberPath = "NAMECHILDS";
                comboBoxChild.SelectedIndex = 0;
            }
            else
            {
                comboBoxChild.Visibility = Visibility.Collapsed;
                buNuwChild.Visibility = Visibility.Collapsed;
                grid.Visibility = Visibility.Collapsed;
                reportChild.Visibility = Visibility.Visible;
            }

          
        }


        private void buNuwChild_Click(object sender, RoutedEventArgs e)
        {
            Model.MainModel.NeedOpenPassportDetail = new PassportDetailOpenParam() { 
                PassportKey = "0", 
                EntityKey = _keyChild, 
                ParentKey = _currentPassportKey, 
                IsChildPassport = 1,
                IsVisibleButtonTransit = Model.PassportDetailOpenParams.IsVisibleButtonTransit
            };
        }

        private void ComboBoxChild_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            // загрузка таблицы
            if (comboBoxChild.SelectedItem != null)
            {
                _keyChild = ((DataChildList)comboBoxChild.SelectedItem).ENTITYKEYCHILDS;

                ServiceDataClient.GetGridDataCompleted += OnGetGridDataCompleted;
                ServiceDataClient.GetGridDataAsync(_keyChild, "0", "1", _currentPassportKey, GlobalContext.Context);
            }
        }

        void OnGetGridDataCompleted(object sender, GetGridDataCompletedEventArgs e)
        {
            ServiceDataClient.GetGridDataCompleted -= OnGetGridDataCompleted;
            _gridData = e.Result;

            if (_gridData != null)
            {
                if (_gridData.Rows.Count == 0)
                {
                    Model.MainModel.Report("PassportChildView " + e.Result.ErrorMessage);
                }
                ServiceDataClient.MetaDataCompleted += OnMetaDataCompleted;
                ServiceDataClient.MetaDataAsync(_keyChild, "1", GlobalContext.Context, "0");
            }
        }

        private void OnMetaDataCompleted(object sender, MetaDataCompletedEventArgs e)
        {
            ServiceDataClient.MetaDataCompleted -= OnMetaDataCompleted;

            List<FieldMetaDataItem> metaDataList = new List<FieldMetaDataItem>(e.Result.FieldMetaDataList);
            if (e.Result.FieldMetaDataList.Count != 0)
            {
                try
                {

                    IGridData childGridData = new ChildGridData(metaDataList, _gridData);
                    var template = Resources["buttonEditDeleteCellTemplate"] as DataTemplate;
                    var cellStyle = Resources["StyleCell"] as Style;
                    var cellStyleAll = Resources["StyleCellAll"] as Style;
                    var headerCellStyle = Resources["MyHeaderCellStyle"] as Style;

                    GridHelper.BuildPasportGrid(grid, template, cellStyle, cellStyleAll, headerCellStyle, childGridData, true);

                }
                catch (Exception ex)
                {
                    Model.MainModel.Report(ex.Message);
                }
            }
            else
            {
                Model.MainModel.Report(" метаданные дл чайдлов " + e.Result.ErrorMessage);
            }
            //if (_gridData.Rows.Count == 0)
            //{
            //    // Выводим сообщение "нет данных"
            //}
            //else
            //{
                
           // }
        }

        private void OnEditButtonClick(object sender, RoutedEventArgs e)
        {
            string artifactId = GridHelper.GetArtifactIdByClick(sender, "ARTIFACT_ID", grid);

            Model.MainModel.NeedOpenPassportDetail = new PassportDetailOpenParam() { PassportKey = artifactId };
        }

        private void OnDeleteButtonClick(object sender, RoutedEventArgs e)
        {
            artifactIdOnDeleteChild = GridHelper.GetArtifactIdByClick(sender, "ARTIFACT_ID", grid);

            _popupWindowDeleteChild.Title = ProjectResources.GridControlMessageTitle; //"Подтверждение удаления";
            _popupWindowDeleteChild.titleBox.Text = ProjectResources.PassportChildTitleBox; //"Удалить паспорт дочерних объектов вместе со связями?";
            _popupWindowDeleteChild.Show();
            _popupWindowDeleteChild.OKButtonDelete.Click += OKButtonDelete;
        }


        void OKButtonDelete(object sender, RoutedEventArgs e)
        {
            _popupWindowDeleteChild.OKButtonDelete.Click -= OKButtonDelete;

            if (_popupWindowDeleteChild.DialogResult == true)
            {
                
                Model.DeletePassport(artifactIdOnDeleteChild);
                ServiceDataClient.GetGridDataCompleted += OnGetGridDataCompleted;
                ServiceDataClient.GetGridDataAsync(_keyChild, "0", "1", _currentPassportKey, GlobalContext.Context);


                //передаем ключ парента
                //var template = Resources["buttonEditDeleteCellTemplate"] as DataTemplate;
                //GridHelper.BuildPasportGrid(grid, template, Model, true);
                //Model.InitModel(Model.PassportOpenParams);



            }
        }

    }
}
