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
using Passpot.Business.DataTable;
using Passpot.Grid;
using Passpot.Model;
using Services.ServiceReference;
using Telerik.Windows.Controls;

namespace Passpot.Controls
{
    public partial class GridControl : UserControl, IControlValueChanged
    {
        private ServiceDataClient _serviceDataClient;
        private ChildWindowRelationObj _popupWindowRelationObj;
        private PassportDetailModel _passportModel;
        private FieldMetaDataItem _metaData;
        private List<DataListRelationObjItems> _dataRelationList;
        private List<DataConnectList> _dataConnectList;
        //private PopUpChildWindow _linkTableWindow;
        private ChildWindowLinkObj _linkTableWindow;
        private List<DataConnectList> _selectConnectList;
        public FindKeyOnTree findKeyOnTree;

        private bool _typeVisible;
        public string _keyPassportRelation = "";
        public string _namePassport;
        private ChildWindowDelete _popupWindowDelete;
        private ChildWindowAttantion _popupWindowAttantion;
        private List<DataListRelationObjItems> _selectRelationObjGridList = new List<DataListRelationObjItems>();
        private List<DataListRelationObjItems> old_selectRelationObjGridList = new List<DataListRelationObjItems>();
        private static int countrelationObj;



        private ServiceDataClient ServiceDataClient
        {
            get
            {
                if (_serviceDataClient == null)
                    _serviceDataClient = new ServiceDataClient();
                return _serviceDataClient;
            }
        }

        public GridControl()
        {
            //LocalizationManager.DefaultResourceManager = ResourceGrid.ResourceManager;
            InitializeComponent();
        }
        public static GridControl CreateControl(FieldMetaDataItem metaData, PassportDetailModel passportModel, bool editpassport)
        {
            var c = new GridControl();
            c._metaData = metaData;
            c._passportModel = passportModel;

            c.titleBox.Text = metaData.TITUL;

            switch (metaData.BASIC_FLD)
            {
                case 1:
                    c.titleBox.FontWeight = FontWeights.Bold;
                    break;
                case 0:

                    break;

            }


            switch (metaData.MANDATORYVALUE_FLD)
            {
                case 1:
                    c.titleBox.Foreground = new SolidColorBrush(Colors.Red);

                    break;
                case 0:

                    break;
            }



            if (editpassport)
            {
                c.addRelation.IsEnabled = true;
                c.button_dell.IsEnabled = true;
            }
            else
            {
                c.addRelation.IsEnabled = false;
                c.button_dell.IsEnabled = false;
               
            }
            //if (metaData.IS_EDITED == 1)
            //{
            //    c.addRelation.Visibility = Visibility.Visible;
            //    c.button_dell.Visibility = Visibility.Visible;

            //}
            //else
            //{
            //    c.addRelation.Visibility = Visibility.Collapsed;
            //    c.button_dell.Visibility = Visibility.Collapsed;
            //}



            //if (c._passportModel.PassportDetailOpenParams.PassportKey != "new")
            if (c._passportModel.PassportDetailOpenParams.PassportKey != "0")
            {


                c.StartOnGetRelationObj(c._passportModel.PassportDetailOpenParams.PassportKey,
                                                       c._passportModel.PassportDetailOpenParams.EntityKey,
                                                       c._metaData.FLDKEY.ToString(), c._metaData.FLDNAME, 1, 1);


            }
            else
            {
                c.addRelation.IsEnabled = false; 
                c.button_dell.IsEnabled = false; 
            }


            if (metaData.IS_EDITED == 0)
            {
                c.IsEnabled = false;
            }
            else
            {
                c.IsEnabled = true;
            }

            return c;
        }


        public void StartOnGetRelationObj(string keyObj, string keyEntity, string fldKey, string fldName, int typeVisible, int typeControl)
        {
            //получаем данные для таблицы связей(название поля - обязательно!!!!!!!!)
            ServiceDataClient.GetDataRelationObjCompleted += GetDataRelationObjCompletedGrid;
            //ServiceDataClient.GetDataRelationObjAsync(_passportModel.PassportDetailOpenParams.PassportKey, _metaData.FLDKEY.ToString(), GlobalContext.Context);
            ServiceDataClient.GetDataRelationObjAsync(_passportModel.PassportDetailOpenParams.PassportKey, _metaData.FLDNAME.ToString(), GlobalContext.Context);
        }

        void GetDataRelationObjCompletedGrid(object sender, GetDataRelationObjCompletedEventArgs e)
        {
            ServiceDataClient.GetDataRelationObjCompleted -= GetDataRelationObjCompletedGrid;
            if (e.Result.IsValid)
            {
                
                _selectRelationObjGridList = new List<DataListRelationObjItems>(e.Result.DataListRelationObjList);

                old_selectRelationObjGridList = new List<DataListRelationObjItems>(e.Result.DataListRelationObjList);

                
                OnRelationObj_GridData(_selectRelationObjGridList);

                //наполняем лист для вывода в ексель горизонтальных связей 

                string _nameObj = "";

                for (int i = 0; i < _selectRelationObjGridList.Count; i++)
                {
                    DataListRelationObjItems dd = _selectRelationObjGridList[i];
                    _nameObj = _nameObj + dd.NameObj + ";";


                }
                _passportModel.ListRelationObjList.Add(new ListRelationObj(_metaData.FLDNAME, _nameObj));


            }
            else
            {
                _passportModel.MainModel.Report("Список связей GetDataRelationObj err = " + e.Result.ErrorMessage);
                grid.Visibility = Visibility.Collapsed;
            }
        }


        public void OnRelationObj_GridData(List<DataListRelationObjItems> relationList)
        {

            //var template = Resources["buttonEditDeleteCellTemplate"] as DataTemplate;
            //var columnButton = new GridViewColumn { CellTemplate = template };
            //grid.Columns.Add(columnButton);
            countrelationObj = relationList.Count;
            if (countrelationObj > 0)
            {
                grid.ItemsSource = relationList;
                grid.Visibility = Visibility.Visible;
                
            }
            else
            {
                grid.Visibility = Visibility.Collapsed;
                button_dell.Visibility = Visibility.Collapsed;
            }
            
        }



        private void button_dell_Click(object sender, RoutedEventArgs e)
        {
            if (_keyPassportRelation == "")
            {
                _popupWindowAttantion = new ChildWindowAttantion();
                _popupWindowAttantion.titleBox.Text = ProjectResources.GridControlMessage; //"Не выбран в таблице объект удаления!!!! ";
                _popupWindowAttantion.Show();
            }
            else
            {
                _popupWindowDelete = new ChildWindowDelete();
                _popupWindowDelete.Title = ProjectResources.GridControlMessageTitle; //"Подтверждение удаления";
                _popupWindowDelete.titleBox.Text = ProjectResources.GridControlMessageDel + _namePassport;  //"Удалить связь  с объектом  "
                _popupWindowDelete.Show();
                _popupWindowDelete.OKButtonDelete.Click += OKButtonDelete;
            }


        }

        void OKButtonDelete(object sender, RoutedEventArgs e)
        {
            _popupWindowDelete.OKButtonDelete.Click -= OKButtonDelete;

            if (_popupWindowDelete.DialogResult == true)
            {
                for (int ii = 0; ii < _selectRelationObjGridList.Count; ii++)
                {

                    if (_selectRelationObjGridList[ii].KeyObj == _keyPassportRelation)
                    {
                        _selectRelationObjGridList.RemoveAt(ii);
                    }

                }

                grid.ItemsSource = null;
                grid.ItemsSource = _selectRelationObjGridList;


                // ServiceDataClient.DelRelationObjCompleted += DelRelationObjCompleted;
                // ServiceDataClient.DelRelationObjAsync(_passportModel.PassportDetailOpenParams.PassportKey, _metaData.FLDKEY.ToString(), _keyPassportRelation, GlobalContext.Context);
            }
        }

        void DelRelationObjCompleted(object sender, DelRelationObjCompletedEventArgs e)
        {
            ServiceDataClient.DelRelationObjCompleted -= DelRelationObjCompleted;

            if (e.Result.IsValid)
            {
                if (e.Result.KeyRelation_OnDelObj_result.KeyRelation_OnDelObj != "0")
                {
                    //перегружаем таблицу связей
                    ServiceDataClient.GetDataRelationObjCompleted += GetDataRelationObjCompleted;
                    ServiceDataClient.GetDataRelationObjAsync(_passportModel.PassportDetailOpenParams.PassportKey, _metaData.FLDKEY.ToString(), GlobalContext.Context);
                }
                else
                {
                    _passportModel.MainModel.Report("Связь не удалилась!!!!!");
                }
            }
            else
            {
                _passportModel.MainModel.Report("Ошибка!!! - DeleteRelationObj =  " + e.Result.ErrorMessage);
            }
            _keyPassportRelation = "";
        }

        private void addRelation_Click(object sender, RoutedEventArgs e)
        {
            //_passportModel.StartOnDataConnect(_passportModel.PassportDetailOpenParams.EntityKey, _metaData.FLDNAME, "g");
            ServiceDataClient.GetDataConnectCompleted += OnGetDataConnectCompleted;
            ServiceDataClient.GetDataConnectAsync(_passportModel.PassportDetailOpenParams.EntityKey, _metaData.FLDNAME, GlobalContext.Context);
        }

        void OnGetDataConnectCompleted(object sender, GetDataConnectCompletedEventArgs e)
        {
            ServiceDataClient.GetDataConnectCompleted -= OnGetDataConnectCompleted;
            if (e.Result.IsValid)
            {

                string allKeyRelation = "";

                if (_selectRelationObjGridList.Count > 0)
                {
                    for (int ii = 0; ii < _selectRelationObjGridList.Count; ii++)
                    {
                        allKeyRelation = allKeyRelation + _selectRelationObjGridList[ii].KeyObj + ", ";
                    }
                    allKeyRelation = allKeyRelation.Substring(0, allKeyRelation.Length - 2);
                }
                else
                {
                    allKeyRelation = "0";
                }



                _selectConnectList = new List<DataConnectList>(e.Result.DataConnectLists);
                var m = DataContext as PassportDetailModel;

                // _linkTableWindow = new PopUpChildWindow();
                _linkTableWindow = new ChildWindowLinkObj();
                _linkTableWindow.Title = ProjectResources.GridControlLinkObj; //"Связи объекта ";
                _linkTableWindow.DataContext = m;
                _linkTableWindow.StubLinkData(_selectConnectList, _metaData.FLDNAME, allKeyRelation);
                _linkTableWindow.Show();
                _linkTableWindow.btnOk.Click += ButtonOkClick;

            }
            else
            {
                _passportModel.MainModel.Report("Список связей OnGetDataConnect err = " + e.Result.ErrorMessage);
            }
        }

        public void ButtonOkClick(object sender, RoutedEventArgs e)
        {
           
            _linkTableWindow.btnOk.Click -= ButtonOkClick;

            var t = _linkTableWindow.LinkOnGridItem;

            _selectRelationObjGridList.Add(new DataListRelationObjItems { KeyObj = t.ObjKey, NameEntity = t.EntityKey, NameObj = t.ObjName });
            grid.ItemsSource = null;
            grid.ItemsSource = _selectRelationObjGridList;
            grid.Visibility = Visibility.Visible;
           


            //не используется - другой метод
            //если сохраняем связь то вызываем сервис добавления связи
            //В начале сохраняем связь в таблицей связей!!!
            // ServiceDataClient.SaveRelationObjCompleted += ServiceDataClient_SaveRelationObjCompleted;
            // ServiceDataClient.SaveRelationObjAsync(_passportModel.PassportKey, _metaData.FLDKEY.ToString(), _linkTableWindow._keyPassportConnect, GlobalContext.Context);

        }




        void GetDataRelationObjCompleted(object sender, GetDataRelationObjCompletedEventArgs e)
        {
            if (e.Result.IsValid)
            {
                grid.ItemsSource = e.Result.DataListRelationObjList;
            }
        }




        private void grid_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            _keyPassportRelation = ((DataListRelationObjItems)grid.SelectedItem).KeyObj;
            _namePassport = ((DataListRelationObjItems)grid.SelectedItem).NameObj;

        }

        private void OnEditButtonClick(object sender, RoutedEventArgs e)
        {
            //string artifactId = GridHelper.GetArtifactIdByClick(sender, "KeyObj", grid);
            //_passportModel.MainModel.NeedOpenPassportDetail = new PassportDetailOpenParam() { PassportKey = artifactId };
            _keyPassportRelation = ((DataListRelationObjItems)grid.SelectedItem).KeyObj;
            _passportModel.MainModel.NeedOpenPassportDetail = new PassportDetailOpenParam() { PassportKey = _keyPassportRelation };
            _passportModel.MainModel.FirePropertyChanged("FindTreeOnkeyPassport");
            //string keyLink = "";
            //for (int ii = 0; ii < _selectRelationObjGridList.Count; ii++)
            //{
            //    keyLink = keyLink + _selectRelationObjGridList[ii].KeyObj + ",";
            //}
            //keyLink = keyLink.Substring(0, keyLink.Length - 1);
        }

        private bool NeyKey()
        {
            
            bool creat_new_key = false;

            //string old_key = "";
            //string new_key = "";
            //for (int ii = 0; ii < old_selectRelationObjGridList.Count; ii++)
            //{
            //    old_key = old_key + old_selectRelationObjGridList[ii].KeyObj + ",";
            //}
            //old_key = old_key.Substring(0, old_key.Length - 1);
            //string[] arrOld_key = old_key.Split(',');

            //for (int i = 0; i < _selectRelationObjGridList.Count; i++)
            //{
            //    new_key = new_key + _selectRelationObjGridList[i].KeyObj + ",";
            //}

            //new_key = new_key.Substring(0, new_key.Length - 1);
            //string[] arrNew_key = new_key.Split(',');


            //IEnumerable<string> differenceQuery = arrOld_key.Except(arrNew_key);

            //var DifferencesList = old_selectRelationObjGridList.Where(x => !_selectRelationObjGridList.Any(x1 => x1.KeyObj == x.KeyObj))
            //.Union(_selectRelationObjGridList.Where(x => !old_selectRelationObjGridList.Any(x1 => x1.KeyObj == x.KeyObj)));

            //foreach (DataListRelationObjItems items in DifferencesList)
            //{
            //    creat_new_key = true;
            //}

            return creat_new_key;
        }

        private string KeyLink()
        {
            string keyLink = "";
            if (_selectRelationObjGridList.Count > 0)
            {
                for (int ii = 0; ii < _selectRelationObjGridList.Count; ii++)
                {
                    keyLink = keyLink + _selectRelationObjGridList[ii].KeyObj + ",";
                }

                //работает если добавляем одну связь!!! Но еслди удаляем - надо думать!
                // var DifferencesList = old_selectRelationObjGridList.Where(x => !_selectRelationObjGridList.Any(x1 => x1.KeyObj == x.KeyObj))
                //.Union(_selectRelationObjGridList.Where(x => !old_selectRelationObjGridList.Any(x1 => x1.KeyObj == x.KeyObj)));

                // foreach (DataListRelationObjItems items in DifferencesList)
                // {
                //     keyLink = keyLink + items.KeyObj + ",";
                // }

                keyLink = keyLink.Substring(0, keyLink.Length - 1); 
            }
            
           

            return keyLink;
        }

        #region IControlValueChanged Members

        public bool HasChanges
        {
           
            get
            {
                //bool nn = NeyKey();
                //return nn;
                return (_selectRelationObjGridList != null);
            }
        }

        public string NewValue
        {

            get
            {
                string keyLink = KeyLink();
                return keyLink;
            }
        }


        #endregion

        #region IControlValueChanged Members

        public FieldMetaDataItem MetaData
        {
            get { return _metaData; }
        }


        #endregion

    }
}

//return ( _selectRelationObjGridList !=null && _selectRelationObjGridList.Count > 0);

//не используется!!!

//public void OnGetDataAllRelationObj_GridData(List<DataListRelationObjItems> relationList, int typevis)
//{
//    _dataRelationList = relationList;
//    var m = DataContext as PassportDetailModel;



//    //if (_dataRelationList.Count == 0)
//    //{
//    //    // сообщение - нет данных для связи! нет объектов связи!!!
//    //}
//    //else
//    try
//    {

//        _popupWindowRelationObj = new ChildWindowRelationObj();
//        _popupWindowRelationObj.Title = "Связи объекта ";
//        _popupWindowRelationObj.DataContext = m;
//        _popupWindowRelationObj.BuildChildRelationObj(_passportModel.PassportKey, _passportModel.EntityKey, _metaData, _dataRelationList, _passportModel, typevis);
//        _popupWindowRelationObj.Show();





//    }
//    catch (Exception ex)
//    {
//        m.MainModel.Report(ex.Message);
//    }
//}

//void ServiceDataClient_SaveRelationObjCompleted(object sender, SaveRelationObjCompletedEventArgs e)
//{
//    ServiceDataClient.SaveRelationObjCompleted -= ServiceDataClient_SaveRelationObjCompleted;

//    if (e.Result.IsValid)
//    {
//        if (e.Result.KeyRelation_OnSaveObj_result.KeyRelation_OnSaveObj != "0")
//        {
//            перегружаем таблицу связей
//            ServiceDataClient.GetDataRelationObjCompleted += GetDataRelationObjCompleted;
//            ServiceDataClient.GetDataRelationObjAsync(_passportModel.PassportDetailOpenParams.PassportKey, _metaData.FLDKEY.ToString(), GlobalContext.Context);
//        }
//        else
//        {
//            _passportModel.MainModel.Report("Не сохранилась связь в таблице связей!!!!!");
//        }
//    }
//    else
//    {
//        _passportModel.MainModel.Report("Ошибка!!! - SaveRelationObj =  " + e.Result.ErrorMessage);
//    }
//}
