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
using Media.Interfaces;
using Media.Resources;
using Passpot.Business;
using Passpot.Model;
using Services.ServiceReference;

namespace Passpot.Controls
{
    public partial class TextBlockAsButtonControl : UserControl, IControlValueChanged
    {

        private ServiceDataClient _serviceDataClient;
        private PassportDetailModel _passportModel;
        private FieldMetaDataItem _metaData;
        private bool _editPassport;
        private List<DataListRelationObjItems> _selectRelationObjGridList = new List<DataListRelationObjItems>();
        private List<DataListRelationObjItems> old_selectRelationObjGridList = new List<DataListRelationObjItems>();
        private ChildWindowLookAllLink _linkTableWindow;
        private List<DataConnectList> _selectConnectList = new List<DataConnectList>();
        private ChildWindowLinkObj _linkTableWindowAdd;
        public DataListRelationObjItems newItems;

        public event PropertyChangedEventHandler PropertyChanged;

        private ServiceDataClient ServiceDataClient
        {
            get
            {
                if (_serviceDataClient == null)
                    _serviceDataClient = new ServiceDataClient();
                return _serviceDataClient;
            }
        }

        public void FirePropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }

        private PassportDetailModel Model
        {
            get { return DataContext as PassportDetailModel; }
        }


        public TextBlockAsButtonControl()
        {
            InitializeComponent();
        }


        private void TextBlockAsButtonControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            Model.PropertyChanged -= Model_PropertyChanged;
            Model.PropertyChanged += Model_PropertyChanged;
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //событие на удаление связи
            if (e.PropertyName.Equals("KeyLinkDeleteControlBu"))
            {
                //удаляем из коллекции
                for (int ii = 0; ii < _selectRelationObjGridList.Count; ii++)
                {
                    if (_selectRelationObjGridList[ii].KeyObj == Model.KeyPassportOnDeleteLinkOnButton)
                    {
                        _selectRelationObjGridList.RemoveAt(ii);
                    }
                }
            }
        }

        public static TextBlockAsButtonControl CreateTextBlockControl(FieldMetaDataItem metaData,
                                                              PassportDetailModel passportModel, bool editpassport)
        {
            var c = new TextBlockAsButtonControl();

            c._metaData = metaData;
            c._passportModel = passportModel;
            c.titleTextBlock.Text = metaData.TITUL;
            c._editPassport = editpassport;


            switch (metaData.BASIC_FLD)
            {
                case 1:
                    c.titleTextBlock.FontWeight = FontWeights.Bold;
                    break;
                case 0:

                    break;

            }


            switch (metaData.MANDATORYVALUE_FLD)
            {
                case 1:
                    c.titleTextBlock.Foreground = new SolidColorBrush(Colors.Red);

                    break;
                case 0:

                    break;
            }
            if (editpassport)
            {
                c.button_addRelation.Visibility = Visibility.Visible;
                c.buLink.Foreground = new SolidColorBrush(Color.FromArgb(
                                      255,
                                      Byte.Parse("76"),
                                      Byte.Parse("147"),
                                      Byte.Parse("77")));
            }
            else
            {
                c.button_addRelation.Visibility = Visibility.Collapsed;
                c.buLink.Foreground = new SolidColorBrush(Color.FromArgb(
                                     255,
                                     Byte.Parse("0"),
                                     Byte.Parse("150"),
                                     Byte.Parse("219")));
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
            busyIndicatorBlock.IsBusy = true;
            ServiceDataClient.GetDataRelationObjCompleted += GetDataRelationObjCompletedGrid;
            ServiceDataClient.GetDataRelationObjAsync(_passportModel.PassportDetailOpenParams.PassportKey, _metaData.FLDNAME.ToString(), GlobalContext.Context);
        }



        void GetDataRelationObjCompletedGrid(object sender, GetDataRelationObjCompletedEventArgs e)
        {
            ServiceDataClient.GetDataRelationObjCompleted -= GetDataRelationObjCompletedGrid;
            if (e.Result.IsValid)
            {

                _selectRelationObjGridList = new List<DataListRelationObjItems>(e.Result.DataListRelationObjList);

                old_selectRelationObjGridList = new List<DataListRelationObjItems>(e.Result.DataListRelationObjList);
                //var m = DataContext as PassportDetailModel;

                //if (_selectRelationObjGridList.Count > 0)
                //{
                //    _linkTableWindow = new ChildWindowLookAllLink();
                //    _linkTableWindow.Title = ProjectResources.ChildLinkObjAll; //"Связи объекта ";
                //    _linkTableWindow.DataContext = m;
                //    _linkTableWindow.GetGridLinkData(_selectRelationObjGridList);
                //    _linkTableWindow.Show();     
                //}




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

            }

           // _passportModel.MainModel.Indicator(true);
            busyIndicatorBlock.IsBusy = false;
        }

        //нажатие на кнопки отобразить все связи на просмотр
        private void tb_MouseLeftButtonDownVisibleAlllink(object sender, MouseButtonEventArgs e)
        {

            ServiceDataClient.GetDataConnectCompleted += OnGetDataConnectCompleted1;
            ServiceDataClient.GetDataConnectAsync(_passportModel.PassportDetailOpenParams.EntityKey, _metaData.FLDNAME, GlobalContext.Context);

        }

        private void OnGetDataConnectCompleted1(object sender, GetDataConnectCompletedEventArgs e)
        {
            ServiceDataClient.GetDataConnectCompleted -= OnGetDataConnectCompleted1;
            var m = DataContext as PassportDetailModel;


            if (e.Result.IsValid)
            {
                _selectConnectList = new List<DataConnectList>(e.Result.DataConnectLists);
            }

            if (_selectRelationObjGridList.Count > 0)
            {
                _linkTableWindow = new ChildWindowLookAllLink();
                _linkTableWindow.Title = ProjectResources.ChildLinkObjAll; //"Связи объекта ";
                _linkTableWindow.DataContext = m;
                _linkTableWindow.GetGridLinkData(_selectRelationObjGridList, _editPassport, _selectConnectList);
                _linkTableWindow.Show();
            }

        }



        //кнопка прир редактировании + - добавление горизонтальных связей
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
                _linkTableWindowAdd = new ChildWindowLinkObj();
                _linkTableWindowAdd.Title = ProjectResources.ChildWindowLinkObjTitle; //"Связи объекта ";
                _linkTableWindowAdd.DataContext = m;
                _linkTableWindowAdd.StubLinkData(_selectConnectList, _metaData.FLDNAME, allKeyRelation);
                _linkTableWindowAdd.Show();
                _linkTableWindowAdd.btnOk.Click += ButtonOkClick;

            }
            else
            {
                _passportModel.MainModel.Report("Список связей OnGetDataConnect err = " + e.Result.ErrorMessage);
            }
        }

        //сохраняем выбранный элемент в массиве
        public void ButtonOkClick(object sender, RoutedEventArgs e)
        {


            _linkTableWindowAdd.btnOk.Click -= ButtonOkClick;

            var t = _linkTableWindowAdd.LinkOnGridItem;
            string _nameEntity = "";
            _nameEntity = _linkTableWindowAdd.NameEntity;

            if (t != null)
            {
                newItems = null;
                newItems = new DataListRelationObjItems();
                newItems.KeyObj = t.ObjKey;
                newItems.NameObj = t.ObjName;
                newItems.NameEntity = t.EntityKey;


                _selectRelationObjGridList.Add(new DataListRelationObjItems { KeyObj = t.ObjKey, NameEntity = _nameEntity, NameObj = t.ObjName });
            }



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
