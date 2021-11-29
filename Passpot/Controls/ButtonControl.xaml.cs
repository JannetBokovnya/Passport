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
using linkControl.Control;
using Media.Interfaces;
using Passpot.Business;
using Passpot.Model;
using Services.ServiceReference;

namespace Passpot.Controls
{
    public partial class ButtonControl : UserControl
    {
        private ServiceDataClient _serviceDataClient;
        private ChildWindowRelationObj _popupWindowRelationObj;
        private PassportDetailModel _passportModel;
        private FieldMetaDataItem _metaData;
        private List<DataListRelationObjItems> _dataRelationList;
        private List<DataConnectList> _dataConnectList;
        private PopUpChildWindow _linkTableWindow;
        private int _typeVisible;
        private string newvalue_c;

        private ServiceDataClient ServiceDataClient
        {
            get
            {
                if (_serviceDataClient == null)
                    _serviceDataClient = new ServiceDataClient();
                return _serviceDataClient;
            }
        }


        public ButtonControl()
        {
            InitializeComponent();
            buttonSeeSv.Click += buttonSeeSv_Click;
        }

        void buttonSeeSv_Click(object sender, RoutedEventArgs e)
        {
            if (_passportModel.PassportDetailOpenParams.PassportKey != "0")
            {
                //ВНИМАНИЕ!!!!!!!!!! ПЕРЕНЕСТИ ВЫЗОВ ПРОЦНДУР ИЗ ОСНОВНОЙ ФОРМЫ!!!!!
                //_passportModel.StartOnGetRelationObj(_passportModel.PassportDetailOpenParams.PassportKey,
                //                                     _passportModel.PassportDetailOpenParams.EntityKey,
                //                                     _metaData.FLDKEY.ToString(), _metaData.FLDNAME, _typeVisible, 2);
            }
            //_passportModel.StartOnDataConnect(_passportModel.PassportDetailOpenParams.EntityKey, "Название поля связи", "tb");

           
        }
        /// <summary>
        /// !!!! FieldMetaDataItem metaData  должны обязательно передать!!! но на момент отладки не обязательно!!!!!
        /// FieldMetaDataItem metaData - берем название поля по которой выводим связанные объекты
        /// typeVisible - 1- при просмотре (чайлд окна) - кнопки добавить - невидимые!
        /// </summary>
        /// <param name="metaData"></param>
        /// <param name="passportModel"></param>
        /// <returns></returns>
        public static ButtonControl CreateControl(FieldMetaDataItem metaData, PassportDetailModel passportModel, int typeVisible)
        {

            var c = new ButtonControl();
            c._metaData = metaData;
            c._passportModel = passportModel;
            c._typeVisible = typeVisible;
            //c._metaData = metaData;
            c.buttonSeeSv.Content = metaData.TITUL;
            c.titleBox.Text = metaData.TITUL;
            return c;
        }


        public void OnGetDataConnect_GridData(List<DataConnectList> connectList)
        {
            //показываем форму из кнока - вся таблица - добавить(если связи один ко многим)
            _dataConnectList = connectList;
            var m = DataContext as PassportDetailModel;
            GridData gridData = new GridData();

            if (_dataConnectList.Count == 0)
            {
                // сообщение - нет данных для связи! нет объектов связи!!!
            }
            else
                try
                {
                    var linkModel = new LinkModel(m.MetaDataList, gridData, _dataConnectList);
                    _linkTableWindow = new PopUpChildWindow();
                    _linkTableWindow.Title = "Связи объекта ";
                    // _linkTableWindow.DataContext = linkModel;
                    _linkTableWindow.DataContext = m;

                    _linkTableWindow.StubLinkData(_dataConnectList);
                    _linkTableWindow.Show();
                    _linkTableWindow.btnOk.Click += ButtonOkClick;

                }
                catch (Exception ex)
                {
                    m.MainModel.Report(ex.Message);
                }

        }



        public void OnGetDataAllRelationObj_GridData(List<DataListRelationObjItems> relationList, int typevis)
        {
            _dataRelationList = relationList;
            var m = DataContext as PassportDetailModel;

            

            //if (_dataRelationList.Count == 0)
            //{
            //    // сообщение - нет данных для связи! нет объектов связи!!!
            //}
            //else
                try
                {

                    _popupWindowRelationObj = new ChildWindowRelationObj();
                    _popupWindowRelationObj.Title = "Связи объекта ";
                    _popupWindowRelationObj.DataContext = m;
                    _popupWindowRelationObj.BuildChildRelationObj(_passportModel.PassportKey, _passportModel.EntityKey, _metaData, _dataRelationList, _passportModel, typevis);
                    _popupWindowRelationObj.Show();



                    

                }
                catch (Exception ex)
                {
                    m.MainModel.Report(ex.Message);
                }
        }
        public FieldMetaDataItem MetaData
        {
            get { return _metaData; }
        }

        public void ButtonOkClick(object sender, RoutedEventArgs e)
        {
            _linkTableWindow.btnOk.Click -= ButtonOkClick;
            newvalue_c = _linkTableWindow._keyPassportConnect;



            ServiceDataClient.SaveRelationObjCompleted += ServiceDataClient_SaveRelationObjCompleted;
            ServiceDataClient.SaveRelationObjAsync(_passportModel.PassportKey, _metaData.FLDKEY.ToString(), _linkTableWindow._keyPassportConnect, GlobalContext.Context);


            //ServiceDataClient.GetNameObj_OnObjKeyCompleted += OnObjConnectKeyCompleted;
            //ServiceDataClient.GetNameObj_OnObjKeyAsync(_linkTableWindow._keyPassportConnect);
            // textBox.Text = _linkTableWindow._keyPassportConnect;

        }

        void ServiceDataClient_SaveRelationObjCompleted(object sender, SaveRelationObjCompletedEventArgs e)
        {
            ServiceDataClient.SaveRelationObjCompleted -= ServiceDataClient_SaveRelationObjCompleted;

            if (e.Result.IsValid)
            {
                if (e.Result.KeyRelation_OnSaveObj_result.KeyRelation_OnSaveObj != "0")
                {
                    //ServiceDataClient.GetNameObj_OnObjKeyCompleted += OnObjConnectKeyCompleted;
                    //ServiceDataClient.GetNameObj_OnObjKeyAsync(_linkTableWindow._keyPassportConnect);
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


        //void OnObjConnectKeyCompleted(object sender, GetNameObj_OnObjKeyCompletedEventArgs e)
        //{
        //    ServiceDataClient.GetNameObj_OnObjKeyCompleted -= OnObjConnectKeyCompleted;
        //    if (e.Result.IsValid)
        //    {
        //        //textBox.Text = _linkTableWindow._keyPassportConnect = e.Result.NameObjOnObjKey_result.NameObjOnObjKey;
        //        //_value_c = newvalue_c;
        //    }
        //    else
        //    {
        //        _passportModel.MainModel.Report(e.Result.ErrorMessage);
        //    }
        //}
    }

}
