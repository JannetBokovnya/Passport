using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Media;
using Media.Interfaces;
using Passpot.Business;
using Media.Resources;
using linkControl.Control;
using Passpot.Model;
using Services.ServiceReference;


namespace Passpot.Controls
{
    public partial class TextBoxAndButton : UserControl, IControlValueChanged
    {

        #region Private fields

        private ServiceDataClient _serviceDataClient;
        private string _oldValue;
        private FieldMetaDataItem _metaData;
        private PassportDetailModel _passportModel;
        private string _entityKey;
        //переменная в которую записываем переданное строковое значение ключа связи
        private string _value_c;
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _editPassport;
        private List<DataListRelationObjItems> _selectRelationObjGridList = new List<DataListRelationObjItems>();
        private List<DataConnectList> _dataConnectList;
        //private PopUpChildWindow _linkTableWindow;
        private ChildWindowLinkObj _linkTableWindow;

        private string newvalue_c;
        private List<DataConnectList> _selectConnectList;
        private ChildWindowDelete _popupWindowDelete;


        private List<DataListRelationObjItems> old_selectRelationObjGridList = new List<DataListRelationObjItems>();

        private ChildWindowLinkObj _linkTableWindowAdd;
        public DataListRelationObjItems newItems;



        #endregion Private fields

        public void FirePropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }

        private PassportDetailModel Model
        {
            get { return DataContext as PassportDetailModel; }
        }



        public TextBoxAndButton()
        {
            InitializeComponent();


        }

        //public static TextBoxAndButton CreateControl(FieldMetaDataItem metaData, ControlAttributeItem attrOneContr, PassportDetailModel passportModel, string nameC, string value)
        public static TextBoxAndButton CreateControl(FieldMetaDataItem metaData,
                                                     ControlAttributeItem attrOneContr,
                                                     PassportDetailModel passportModel,
                                                     bool editpassport)
        {
            var c = new TextBoxAndButton();


            c._passportModel = passportModel;
            //c._oldValue = value;
            c._metaData = metaData;

            //c._value_c = value;
            c.titleBox.Text = metaData.TITUL;
            //c.textBox.Text = nameC;
            c._editPassport = editpassport;

            switch (metaData.BASIC_FLD)
            {
                case 1:
                    c.titleBox.FontWeight = FontWeights.Bold;

                    break;
                case 0:

                    break;
            }

            if ((!c._editPassport) || (c._metaData.IS_EDITED == 0))
            {
                c.button_addRelation.Visibility = Visibility.Collapsed;
            }


            switch (metaData.MANDATORYVALUE_FLD)
            {
                case 1:
                    c.titleBox.Foreground = new SolidColorBrush(Color.FromArgb(255, Byte.Parse("220"), Byte.Parse("70"), Byte.Parse("74")));

                    break;
                case 0:

                    break;
            }


            if (c._passportModel.PassportDetailOpenParams.PassportKey != "0")
            {
                c.StartOnGetRelationObj(c._passportModel.PassportDetailOpenParams.PassportKey,
                                        c._passportModel.PassportDetailOpenParams.EntityKey,
                                        c._metaData.FLDKEY.ToString(), c._metaData.FLDNAME, 1, 1);
            }

            return c;
        }

        public void StartOnGetRelationObj(string keyObj, string keyEntity, string fldKey, string fldName, int typeVisible, int typeControl)
        {
            //получаем данные для таблицы связей(название поля - обязательно!!!!!!!!)
           // _passportModel.MainModel.Indicator(true);
            busyIndicatorBoxButton.IsBusy = true;

            ServiceDataClient.GetDataRelationObjCompleted += GetDataRelationObjCompletedtb;
            ServiceDataClient.GetDataRelationObjAsync(_passportModel.PassportDetailOpenParams.PassportKey, _metaData.FLDNAME.ToString(), GlobalContext.Context);
        }

        void GetDataRelationObjCompletedtb(object sender, GetDataRelationObjCompletedEventArgs e)
        {
            ServiceDataClient.GetDataRelationObjCompleted -= GetDataRelationObjCompletedtb;
            if (e.Result.IsValid)
            {

                _selectRelationObjGridList = new List<DataListRelationObjItems>(e.Result.DataListRelationObjList);

                old_selectRelationObjGridList = new List<DataListRelationObjItems>(e.Result.DataListRelationObjList);


                if ((_editPassport) && (_metaData.IS_EDITED == 1))
                {
                    //для редактирования

                    if (_selectRelationObjGridList.Count == 0)
                    {
                        List<DataListRelationObjItems> tt = new List<DataListRelationObjItems>();
                        tt.Add(new DataListRelationObjItems { KeyObj = "", NameEntity = "", NameObj = "" });


                        LinkControlEdit tc = LinkControlEdit.CreateLinkControlEdit(tt[0], _passportModel, _metaData, _selectRelationObjGridList, true, true, false);
                        spTextBlock.Children.Add(tc);

                    }
                    else
                    {
                        // spTextBlockButton.Visibility = Visibility.Collapsed;
                        for (int ii = 0; ii < _selectRelationObjGridList.Count; ii++)
                        {
                            LinkControlEdit tc;
                            if (ii == 0)
                            {
                                tc = LinkControlEdit.CreateLinkControlEdit(_selectRelationObjGridList[ii], _passportModel, _metaData, _selectRelationObjGridList, false, true, false);
                            }
                            else
                            {
                                tc = LinkControlEdit.CreateLinkControlEdit(_selectRelationObjGridList[ii], _passportModel, _metaData, _selectRelationObjGridList, false, false, false);
                            }

                            spTextBlock.Children.Add(tc);
                        }

                        button_addRelation.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    //для просмотра
                    //  contentHolderEdit.Visibility = Visibility.Collapsed;
                    //здесь создаем контролы - просмотр
                    for (int ii = 0; ii < _selectRelationObjGridList.Count; ii++)
                    {
                        TextBlock tc = CreateTextBlock(_selectRelationObjGridList[ii]);
                        // lbLinks.Items.Add(tc);
                        spTextBlock.Children.Add(tc);
                    }
                }



            }
            else
            {
                _passportModel.MainModel.Report("Список связей GetDataRelationObj err = " + e.Result.ErrorMessage);
            }
            //_passportModel.MainModel.Indicator(true);
            busyIndicatorBoxButton.IsBusy = false;
        }

        //создание связей - контрол текст блок
        private TextBlock CreateTextBlock(DataListRelationObjItems relationList)
        {
            TextBlock tb = new TextBlock();
            tb.Name = relationList.KeyObj;
            tb.Text = relationList.NameObj;
            tb.TextDecorations = TextDecorations.Underline;
            tb.Cursor = Cursors.Hand;
            tb.TextTrimming = TextTrimming.WordEllipsis;
            tb.HorizontalAlignment = HorizontalAlignment.Stretch;
            tb.VerticalAlignment = VerticalAlignment.Top;
            tb.FontFamily = new FontFamily("Tahoma");
            tb.Margin = new Thickness(4);
            tb.Height = 23;
            tb.MouseLeftButtonDown += (tb_MouseLeftButtonDown);
            tb.Foreground = new SolidColorBrush(Color.FromArgb(
                                      255,
                                      Byte.Parse("0"),
                                      Byte.Parse("150"),
                                      Byte.Parse("219")));


            return tb;
        }

        void tb_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var tt = sender as TextBlock;
            _passportModel.MainModel.NeedOpenPassportDetail = new PassportDetailOpenParam() { PassportKey = tt.Name };
            _passportModel.MainModel.FirePropertyChanged("FindTreeOnkeyPassport");

        }

        private ServiceDataClient ServiceDataClient
        {
            get
            {
                if (_serviceDataClient == null)
                    _serviceDataClient = new ServiceDataClient();
                return _serviceDataClient;
            }
        }






        public void ButtonOkClick_SaveRelation(object sender, RoutedEventArgs e)
        {
            _linkTableWindow.btnOk.Click -= ButtonOkClick_SaveRelation;


            var t = _linkTableWindow.LinkOnGridItem;

        }

        //кнопка добавить связь
        private void Button_addRelation_OnClick(object sender, RoutedEventArgs e)
        {
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
                _linkTableWindow.Title = ProjectResources.ChildWindowLinkObjTitle; //"Связи объекта ";
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

            newItems = null;
            newItems = new DataListRelationObjItems();
            newItems.KeyObj = t.ObjKey;
            newItems.NameObj = t.ObjName;
            newItems.NameEntity = t.EntityKey;

            KeyLinkAdd();

        }

        private void KeyLinkAdd()
        {
            try
            {
                _selectRelationObjGridList.Add(new DataListRelationObjItems { KeyObj = newItems.KeyObj, NameEntity = newItems.NameEntity, NameObj = newItems.NameObj });
                LinkControlEdit tc = LinkControlEdit.CreateLinkControlEdit(_selectRelationObjGridList[0], _passportModel, _metaData, _selectRelationObjGridList, false, true, false);
                spTextBlock.Children.Clear();
                spTextBlock.Children.Add(tc);

                button_addRelation.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {

                _passportModel.MainModel.Report("Нельзя добавить связь = " + ex.Message);
            }

            

            //var child = spTextBlock.Children;
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

                keyLink = keyLink.Substring(0, keyLink.Length - 1);
            }
            return keyLink;
        }

        #region IControlValueChanged Members

        public bool HasChanges
        {
            //get { return _value_c != _oldValue; }
            get { return (_selectRelationObjGridList !=null); }
        }

        public string NewValue
        {
            //get { return _value_c; }
            get
            {
                string keyLink = KeyLink();
                return keyLink;
            }
        }


        public FieldMetaDataItem MetaData
        {
            get { return _metaData; }
        }
        
       


        #endregion

      

        private void TextBoxAndButton_OnLoaded(object sender, RoutedEventArgs e)
        {
            Model.PropertyChanged -= Model_PropertyChanged;
            Model.PropertyChanged += Model_PropertyChanged;
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //событие на удаление связи
            if (e.PropertyName.Equals("KeyLinkDelete"))
            {
                if (spTextBlock.Children.Count != 0)
                {
                    foreach (var child in spTextBlock.Children)
                    {
                        var lce = child as LinkControlEdit;
                        if (lce != null && lce.klickButton)
                        {
                            string _keyLinkOnDelete = lce.KeyLink;
                            //удаляем из spTextBlock
                            spTextBlock.Children.Remove(lce);

                            //удаляем из коллекции
                            for (int ii = 0; ii < _selectRelationObjGridList.Count; ii++)
                            {
                                if (_selectRelationObjGridList[ii].KeyObj == _keyLinkOnDelete)
                                {
                                    _selectRelationObjGridList.RemoveAt(ii);
                                }
                            }
                            button_addRelation.Visibility = Visibility.Visible;
                            return;
                        }
                    }
                    button_addRelation.Visibility = Visibility.Visible;
                }


            }
          
        }
    }
}




//ПЕРЕДЕЛАТЬ!!!!!!!!!!!!!!!!!

//// textBox.Text = _linkTableWindow._keyPassportConnect = e.Result.NameObjOnObjKey_result.NameObjOnObjKey;
// textBox.Text = _linkTableWindow._keyPassportConnect = t.ObjName;
// _passportModel.ListRelationObjList.Add(new ListRelationObj(_metaData.FLDNAME, textBox.Text));
// //_value_c = newvalue_c;
// _value_c = t.ObjKey;


//_selectRelationObjGridList.Add(new DataListRelationObjItems { KeyObj = t.ObjKey, NameEntity = t.EntityKey, NameObj = t.ObjName });
//grid.ItemsSource = null;
//grid.ItemsSource = _selectRelationObjGridList;



//Изменено!!! сохраняется при сохранении паспорта
//newvalue_c = _linkTableWindow._keyPassportConnect;
//В начале сохраняем связь в таблицей связей!!!
//ServiceDataClient.SaveRelationObjCompleted += ServiceDataClient_SaveRelationObjCompleted;
//ServiceDataClient.SaveRelationObjAsync(_passportModel.PassportKey, _metaData.FLDKEY.ToString(), _linkTableWindow._keyPassportConnect, GlobalContext.Context);


//void ServiceDataClient_SaveRelationObjCompleted(object sender, SaveRelationObjCompletedEventArgs e)
//{
//    ServiceDataClient.SaveRelationObjCompleted -= ServiceDataClient_SaveRelationObjCompleted;

//    if (e.Result.IsValid)
//    {
//        if (e.Result.KeyRelation_OnSaveObj_result.KeyRelation_OnSaveObj !="0")
//        {
//            var m = DataContext as PassportDetailModel;
//            ServiceDataClient.GetNameObj_OnObjKeyCompleted += OnObjConnectKeyCompleted;
//            ServiceDataClient.GetNameObj_OnObjKeyAsync(_linkTableWindow._keyPassportConnect, GlobalContext.Context);
//        }
//        else
//        {
//             _passportModel.MainModel.Report("Не сохранилась связь в таблице связей!!!!!");
//        }
//    }
//    else
//    {
//        _passportModel.MainModel.Report("Ошибка!!! - SaveRelationObj =  "+e.Result.ErrorMessage);
//    }
//}

//void OnObjConnectKeyCompleted(object sender, GetNameObj_OnObjKeyCompletedEventArgs e)
//{
//    ServiceDataClient.GetNameObj_OnObjKeyCompleted -= OnObjConnectKeyCompleted;
//    if (e.Result.IsValid)
//    {
//        textBox.Text = _linkTableWindow._keyPassportConnect = e.Result.NameObjOnObjKey_result.NameObjOnObjKey;
//        _passportModel.ListRelationObjList.Add(new ListRelationObj(_metaData.FLDNAME, textBox.Text));
//        _value_c = newvalue_c;
//    }
//    else
//    {
//        _passportModel.MainModel.Report(e.Result.ErrorMessage);
//    }
//}
//кнопка прир редактировании + - добавление горизонтальных связей

//void OnGetDataConnectCompleted(object sender, GetDataConnectCompletedEventArgs e)
//{
//    ServiceDataClient.GetDataConnectCompleted -= OnGetDataConnectCompleted;
//    SelectConnectList = new List<DataConnectList>(e.Result.DataConnectLists); 


//}

//private void OnGetGridLinkDataCompleted(object sender, GetGridDataCompletedEventArgs e)
//{
//    GridData gridData = e.Result;
//    var m = DataContext as PassportDetailModel;

//    if (gridData.IsValid)

//    {

//        if (gridData.Rows.Count == 0)
//        {
//            // Выводим сообщение "нет данных"
//        }
//        else
//            try
//            {
//                // TODO: Убрать и заменить на реальное получение метаданных.
//                //var m = DataContext as PassportDetailModel;
//                var linkModel = new LinkModel(m.MetaDataList, _dataConnectList);
//                _linkTableWindow = new PopUpChildWindow();
//                _linkTableWindow.Title = "Связи объекта ";
//               // _linkTableWindow.DataContext = linkModel;
//                _linkTableWindow.DataContext = m;
//                _linkTableWindow.Show();
//                _linkTableWindow.btnOk.Click += ButtonOkClick;

//            }
//            catch (Exception ex)
//            {
//                m.MainModel.Report(ex.Message);
//            }





//        //// TODO: Убрать и заменить на реальное получение метаданных.
//        //var m = DataContext as PassportDetailModel;
//        //var linkModel = new LinkModel(m.MetaDataList, gridData);
//        //_linkTableWindow = new PopUpChildWindow();
//        //_linkTableWindow.Title = "Здесь заголовок окна";
//        //_linkTableWindow.DataContext = linkModel;
//        //_linkTableWindow.Show();
//        //_linkTableWindow.btnOk.Click += ButtonOkClick;

//        //???????????????????????????????????????????
//        gridData.IsValid = false;
//    }

//}


//public void OnGetDataConnect_GridData(List<DataConnectList> connectList)
//        {
//            _dataConnectList = connectList;
//            var m = DataContext as PassportDetailModel;
//            GridData gridData = new GridData();

//            if (_dataConnectList.Count == 0)
//            {
//                // сообщение - нет данных для связи! нет объектов связи!!!
//            }
//            else
//                try
//                {
//                    var linkModel = new LinkModel(m.MetaDataList, gridData, _dataConnectList);
//                    _linkTableWindow = new PopUpChildWindow();
//                    _linkTableWindow.Title = "Связи объекта ";
//                    // _linkTableWindow.DataContext = linkModel;
//                    _linkTableWindow.DataContext = m;

//                    _linkTableWindow.StubLinkData(_dataConnectList);
//                    _linkTableWindow.Show();
//                    _linkTableWindow.btnOk.Click += ButtonOkClick_SaveRelation;

//                }
//                catch (Exception ex)
//                {
//                    m.MainModel.Report(ex.Message);
//                }

//        }

//private void Button_Click_addRelation(object sender, RoutedEventArgs e)
//{


//    //_passportModel.StartOnDataConnect(_passportModel.PassportDetailOpenParams.EntityKey, _metaData.FLDNAME, "tb");

//    //ServiceDataClient.GetDataConnectCompleted += OnGetDataConnectCompletedTB;
//    //ServiceDataClient.GetDataConnectAsync(_passportModel.PassportDetailOpenParams.EntityKey, _metaData.FLDNAME, GlobalContext.Context);

//    ServiceDataClient.GetDataConnectCompleted += OnGetDataConnectCompletedTB;
//    ServiceDataClient.GetDataConnectAsync(_passportModel.PassportDetailOpenParams.EntityKey, _metaData.FLDNAME, GlobalContext.Context);


//    //перенесли  в PassportDetailModel
//    ////вначале получаем список всех возможных связей для заполнения комбобокса

//    //ServiceDataClient.GetDataConnectCompleted += OnGetDataConnectCompleted;
//    //ServiceDataClient.GetDataConnectAsync(_entityKey, _metaData.FLDNAME);

//    //получение таблицы - пока закоментировали!
//    //ServiceDataClient.GetGridDataCompleted += OnGetGridLinkDataCompleted;
//    //ServiceDataClient.GetGridDataAsync("324","0","1");
//}

//void OnGetDataConnectCompletedTB(object sender, GetDataConnectCompletedEventArgs e)
//{
//    ServiceDataClient.GetDataConnectCompleted -= OnGetDataConnectCompletedTB;
//    //if (e.Result.IsValid)
//    //{
//    //    _selectConnectList = new List<DataConnectList>(e.Result.DataConnectLists);
//    //    var m = DataContext as PassportDetailModel;

//    //    _linkTableWindow = new PopUpChildWindow();
//    //    _linkTableWindow.Title = "Связи объекта ";
//    //    _linkTableWindow.DataContext = m;
//    //    _linkTableWindow.StubLinkData(_selectConnectList);
//    //    _linkTableWindow.Show();
//    //    _linkTableWindow.btnOk.Click += ButtonOkClick_SaveRelation;

//    //}

//    if (e.Result.IsValid)
//    {
//        _selectConnectList = new List<DataConnectList>(e.Result.DataConnectLists);
//        var m = DataContext as PassportDetailModel;
//        string keyrelation = "";
//        if (!String.IsNullOrEmpty(_value_c))
//        {
//            keyrelation = _value_c;
//        }
//        else
//        {
//            keyrelation = "0";
//        }
//         List<DataListRelationObjItems> _selectRelationObjGridList = new List<DataListRelationObjItems>();
//        // _linkTableWindow = new PopUpChildWindow();
//        _linkTableWindow = new ChildWindowLinkObj();
//        _linkTableWindow.Title = ProjectResources.LinkTableTitle; //"Связи объекта ";
//        _linkTableWindow.DataContext = m;
//        _linkTableWindow.StubLinkData(_selectConnectList, _metaData.FLDNAME, keyrelation);
//        _linkTableWindow.Show();
//        _linkTableWindow.btnOk.Click += ButtonOkClick_SaveRelation;

//    }

//    else
//    {
//        _passportModel.MainModel.Report("Список связей OnGetDataConnect err = " + e.Result.ErrorMessage);
//    }
//}
//private void Button_addRelation_OnClick(object sender, RoutedEventArgs e)
//{
//    ServiceDataClient.GetDataConnectCompleted += OnGetDataConnectCompleted;
//    ServiceDataClient.GetDataConnectAsync(_passportModel.PassportDetailOpenParams.EntityKey, _metaData.FLDNAME, GlobalContext.Context);
//}

//void OnGetDataConnectCompleted(object sender, GetDataConnectCompletedEventArgs e)
//{
//    ServiceDataClient.GetDataConnectCompleted -= OnGetDataConnectCompleted;
//    if (e.Result.IsValid)
//    {

//        string allKeyRelation = "";

//        if (_selectRelationObjGridList.Count > 0)
//        {
//            for (int ii = 0; ii < _selectRelationObjGridList.Count; ii++)
//            {
//                allKeyRelation = allKeyRelation + _selectRelationObjGridList[ii].KeyObj + ", ";
//            }
//            allKeyRelation = allKeyRelation.Substring(0, allKeyRelation.Length - 2);
//        }
//        else
//        {
//            allKeyRelation = "0";
//        }



//        _selectConnectList = new List<DataConnectList>(e.Result.DataConnectLists);
//        var m = DataContext as PassportDetailModel;

//        // _linkTableWindow = new PopUpChildWindow();
//        _linkTableWindowAdd = new ChildWindowLinkObj();
//        _linkTableWindowAdd.Title = ProjectResources.ChildWindowLinkObjTitle; //"Связи объекта ";
//        _linkTableWindowAdd.DataContext = m;
//        _linkTableWindowAdd.StubLinkData(_selectConnectList, _metaData.FLDNAME, allKeyRelation);
//        _linkTableWindowAdd.Show();
//        _linkTableWindowAdd.btnOk.Click += ButtonOkClick;

//    }
//    else
//    {
//        _passportModel.MainModel.Report("Список связей OnGetDataConnect err = " + e.Result.ErrorMessage);
//    }
//}

////сохраняем выбранный элемент в массиве
//public void ButtonOkClick(object sender, RoutedEventArgs e)
//{


//    _linkTableWindowAdd.btnOk.Click -= ButtonOkClick;

//    var t = _linkTableWindowAdd.LinkOnGridItem;
//    string _nameEntity = "";
//    _nameEntity = _linkTableWindowAdd.NameEntity;

//    if (t != null)
//    {
//        newItems = null;
//        newItems = new DataListRelationObjItems();
//        newItems.KeyObj = t.ObjKey;
//        newItems.NameObj = t.ObjName;
//        newItems.NameEntity = t.EntityKey;


//        _selectRelationObjGridList.Add(new DataListRelationObjItems { KeyObj = t.ObjKey, NameEntity = _nameEntity, NameObj = t.ObjName });
//    }



//}

  //private void button_dell_Click(object sender, RoutedEventArgs e)
  //      {
  //          //textBox.Text = "";
  //          //_value_c = "";
  //          //ПЕРЕДЕЛАТЬ!!!!
  //          //if (textBox.Text != "")
  //          //{
  //          //    _popupWindowDelete = new ChildWindowDelete();
  //          //    _popupWindowDelete.Title = ProjectResources.GridControlMessageTitle; //"Подтверждение удаления";
  //          //    _popupWindowDelete.titleBox.Text = ProjectResources.GridControlMessageDel +"  " + textBox.Text; //"Удалить связь  с объектом  "
  //          //    _popupWindowDelete.Show();
  //          //    _popupWindowDelete.OKButtonDelete.Click += OKButtonDelete;
  //          //}

  //      }
  //      void OKButtonDelete(object sender, RoutedEventArgs e)
  //      {
  //          _popupWindowDelete.OKButtonDelete.Click -= OKButtonDelete;

  //          //ПЕРЕДЕЛАТЬ!!!!

  //          //if (_popupWindowDelete.DialogResult == true)
  //          //{
  //          //   // ServiceDataClient.DelRelationObjCompleted += DelRelationObjCompletedTB;
  //          //   // ServiceDataClient.DelRelationObjAsync(_passportModel.PassportDetailOpenParams.PassportKey, _metaData.FLDKEY.ToString(), _value_c, GlobalContext.Context);
  //          //    textBox.Text = "";
  //          //    _value_c = "";
  //          //}
  //      }

  //      void DelRelationObjCompletedTB(object sender, DelRelationObjCompletedEventArgs e)
  //      {
  //          ServiceDataClient.DelRelationObjCompleted -= DelRelationObjCompletedTB;

  //          if (e.Result.IsValid)
  //          {
  //              //ПЕРЕДЕЛАТЬ!!!!
  //              //if (e.Result.KeyRelation_OnDelObj_result.KeyRelation_OnDelObj != "0")
  //              //{
  //              //    textBox.Text = "";
  //              //    _value_c = "";
  //              //}
  //              //else
  //              //{
  //              //    _passportModel.MainModel.Report("Связь не удалилась!!!!!");
  //              //}
  //          }
  //          else
  //          {
  //              _passportModel.MainModel.Report("Ошибка!!! - DeleteRelationObj =  " + e.Result.ErrorMessage);
  //          }

  //      }

  //      private void button_relation_Click(object sender, RoutedEventArgs e)
  //      {
  //          //ПЕРЕДЕЛАТЬ!!!!
  //          //if (String.IsNullOrEmpty(_value_c) && ( textBox.Text != ""))
  //          //{

  //          //    _passportModel.MainModel.NeedOpenPassportDetail = new PassportDetailOpenParam() { PassportKey = _value_c, IsEditedPassport = 1 };
  //          //}
  //      }
