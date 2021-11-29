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

namespace Passpot.Controls.Priv
{
    public partial class PrivNewControlTexBlock : UserControl, IControlValueChanged
    {
        //private ChildWindowPriv _linkTableWindow;
        private ChildWindowNewPriv _linkTableWindow2;
        private ServiceDataClient _serviceDataClient;
        private PassportDetailModel _passportModel;
        private FieldMetaDataItem _metaData;
        //private List<FormParamsPrivItems> FormParams;
        //private List<FormTypePrivItems> FormTypePriv;
        //private List<FldParamsPrivItems> FldParamsPriv;
        private string NKEY = "";
        private string artifactIdOnDeletePriv;
        private string nameartifactIdOnDeletePriv;
        private ChildWindowDelete _popupWindowDeleteChild;
        private int CountPriv = 0;
        private bool _editpassport = true;
        private List<PrivItems> _privItemList = new List<PrivItems>();
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

        private PassportDetailModel Model
        {
            get { return DataContext as PassportDetailModel; }
        }

        public void FirePropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }


        public PrivNewControlTexBlock()
        {
            InitializeComponent();
            _popupWindowDeleteChild = new ChildWindowDelete();
        }

        //+ передавать на вход  FieldMetaDataItem metaData, PassportDetailModel passportModel
        public static PrivNewControlTexBlock CreateControl(FieldMetaDataItem metaData, PassportDetailModel passportModel, bool editpassport)
        {
            var c = new PrivNewControlTexBlock();
            c._passportModel = passportModel;
            c._editpassport = editpassport;
            c._metaData = metaData;
            //если паспорт new не показывать  кнопку добавить паспорт

            if (editpassport)
            {

                c.addPriv.Visibility = Visibility.Visible;
            }
            else
            {
                c.addPriv.Visibility = Visibility.Collapsed;
            }

            if (c._passportModel.PassportDetailOpenParams.PassportKey != "0")
            {
                //запрос в базу который возвращает данные по привязке для грида
                //на главной форме

                c.StartOnGetGridDataPriv(passportModel.PassportKey);

            }
            else
            {

                c.addPriv.Visibility = Visibility.Collapsed;

            }

            return c;
        }


        public void StartOnGetGridDataPriv(string inObjKey)
        {
            ////возвращает тип привязки и создана или нет привязка для объекта
            //ServiceDataClient.GetTypePrivCompleted += ServiceDataClient_GetTypePrivCompleted;
            //ServiceDataClient.GetTypePrivAsync(_passportModel.EntityKey, _passportModel.PassportKey,
            //                                   GlobalContext.Context);

            _passportModel.TYPEPRIV = _metaData.FLDTYP;
           
            busyIndicatorPriv.IsBusy = true;

            ServiceDataClient.GetAllPrivCompleted += ServiceDataClient_GetAllPrivCompleted;
            ServiceDataClient.GetAllPrivAsync(_passportModel.PassportKey, GlobalContext.Context);

        }

        //закоментировано!!! не нужно!!!
        //private void ServiceDataClient_GetTypePrivCompleted(object sender, GetTypePrivCompletedEventArgs e)
        //{
        //    ServiceDataClient.GetTypePrivCompleted -= ServiceDataClient_GetTypePrivCompleted;
        //    if (e.Result.IsValid)
        //    {
        //        FormTypePriv = new List<FormTypePrivItems>(e.Result.FormParamsTypePrivItemsList);
        //        _passportModel.ISLOCATIONEXIST = FormTypePriv[0].ISLOCATIONEXIST;
        //        //_passportModel.TYPEPRIV = "3";

        //        #region закоментировано  - раскоментировать после тестирования

        //        //пока закоментировано   - РАССКОМЕНТИРОВАТЬ!!!!!!
        //        _passportModel.TYPEPRIV = FormTypePriv[0].TYPEPRIV;

        //        #endregion закоментировано  - раскоментировать после тестирования

        //        //по ключу объекта возвращает название полей в гриде - не нужно
        //        //ServiceDataClient.GetFldParamsprivCompleted += ServiceDataClient_GetFldParamsprivCompleted;
        //        //ServiceDataClient.GetFldParamsprivAsync(_passportModel.EntityKey, GlobalContext.Context);

        //        ServiceDataClient.GetAllPrivCompleted += ServiceDataClient_GetAllPrivCompleted;
        //        ServiceDataClient.GetAllPrivAsync(_passportModel.PassportKey, GlobalContext.Context);
        //    }
        //    else
        //    {
        //        _passportModel.MainModel.Report(" Тип привязки err = " + e.Result.ErrorMessage);
        //    }
        //}

        //новая привязка
        private void ServiceDataClient_GetAllPrivCompleted(object sender, GetAllPrivCompletedEventArgs e)
        {
           
            ServiceDataClient.GetAllPrivCompleted -= ServiceDataClient_GetAllPrivCompleted;
            if (e.Result.IsValid)
            {
                _privItemList = new List<PrivItems>(e.Result.PrivItemsList);

                if (_privItemList.Count > 0)
                {
                    titleTextBlock.Visibility = Visibility.Collapsed;
                    for (int ii = 0; ii < _privItemList.Count; ii++)
                    {
                        PrivLinkControl plc = PrivLinkControl.CreateLinkControlEdit(_privItemList[ii], _passportModel,
                                                                                    _metaData,
                                                                                    true, true, _editpassport);

                        linkPriv.Children.Add(plc);
                    }
                }
                else
                {
                    if (_editpassport)
                    {
                        titleTextBlock.Text = ProjectResources.PrivControlEditAdd;
                        titleTextBlock.VerticalAlignment = VerticalAlignment.Center;
                        titleTextBlock.FontStyle = FontStyles.Italic;
                        titleTextBlock.Foreground = new SolidColorBrush(Color.FromArgb(
                                             255,
                                             Byte.Parse("165"),
                                             Byte.Parse("165"),
                                             Byte.Parse("165")));
                    }

                }

               

            }
            else
            {
                
                _passportModel.MainModel.Report(" Ошибка выводв привязки err = " + e.Result.ErrorMessage);
            }

            busyIndicatorPriv.IsBusy = false;
        }

        //кнопка добавить привязку
        private void addPriv_Click(object sender, RoutedEventArgs e)
        {

            var m = DataContext as PassportDetailModel;


          //  PrivChildModel privChildModel = new PrivChildModel(_passportModel.PassportKey, _editpassport, _privItemList, Model, Model.TYPEPRIV);

            //добавление новой привязки
            _linkTableWindow2 = ChildWindowNewPriv.CreateFormpriv(_passportModel.PassportKey, _editpassport, _privItemList, Model, Model.TYPEPRIV);
            _linkTableWindow2.Title = ProjectResources.PrivControlLinkTableWindowTitle; //"Привязка объекта ";
            //_linkTableWindow2.DataContext = privChildModel; // m;
            if (_privItemList.Count > 0)
            {
                for (int ii = 0; ii < _privItemList.Count; ii++)
                {
                    // _linkTableWindow2.NavigationPrivList  =_linkTableWindow2.NavigationPrivList.Add(new NavigationPriv("1", "", "", SelectIndexMg, SelectIndexPipe, "", "", ""));

                }

            }

           // _linkTableWindow2.InitConstraint_Add(_editpassport);
            _linkTableWindow2.Show();

            //--------------------------------
            //_linkTableWindow.buCreatePrivKm.Click += (buCreatePriv_Click);
            //_linkTableWindow.buCreatePrivKoord.Click += (buCreatePriv_Click);

        }

        private void PrivNewControlTexBlock_OnLoaded(object sender, RoutedEventArgs e)
        {
            Model.PropertyChanged -= Model_PropertyChanged;
            Model.PropertyChanged += Model_PropertyChanged;
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //событие на удаление связи
            if (e.PropertyName.Equals("KeyPrivDelete"))
            {
                if (linkPriv.Children.Count > 0)
                {

                    foreach (var child in linkPriv.Children)
                    {
                        var lce = child as PrivLinkControl;
                        if (lce != null && lce.klickButton)
                        {
                            //сохраняем ключ привязки для удаления из массива
                            NKEY = lce.NKEY;
                            //удаляем из linkPriv
                            linkPriv.Children.Remove(lce);


                            if (_linkTableWindow2 != null)
                            {
                                for (int t = 0; t < _linkTableWindow2.ListprivItemsNew.Count; t++)
                                {
                                    if (_linkTableWindow2.ListprivItemsNew[t].NKEY == NKEY)
                                    {
                                        _linkTableWindow2.ListprivItemsNew.Remove(_linkTableWindow2.ListprivItemsNew[t]);
                                        break;
                                    }
                                }     
                            }
                           
                            break;
                        }
                    }
                }

                _privItemList.Clear();
                if (_linkTableWindow2 != null)
                {
                    //перестраиваем массив

                    List<PrivItems> lpin = new List<PrivItems>();
                    lpin = _linkTableWindow2.ListprivItemsNew;
                    //SavePriv.ListSavePriv tt = new SavePriv.ListSavePriv();
                    //tt = _linkTableWindow2.PrivItemsNew;
                   
                    for (int t = 0; t < lpin.Count; t++)
                    {
                        if (lpin[t].NKEY != NKEY)
                        {
                            _privItemList.Add(
                         new PrivItems
                         {

                             CNAME = lpin[t].CNAME,
                             NKMORT1 = lpin[t].NKMORT1,
                             NDISTANCEBEG = lpin[t].NDISTANCEBEG,
                             NX1 = lpin[t].NX1,
                             NY1 = lpin[t].NY1,
                             NZ1 = lpin[t].NZ1,
                             NKEY = lpin[t].NKEY,
                             NH1 = lpin[t].NH1,
                             NKMTRUE1 = lpin[t].NKMTRUE1,
                             NX2 = lpin[t].NX2,
                             NY2 = lpin[t].NY2,
                             NZ2 = lpin[t].NZ2,
                             NH2 = lpin[t].NH2,
                             NKMORT2 = lpin[t].NKMORT2,
                             NKMTRUE2 = lpin[t].NKMTRUE2,
                             NDISTANCEEND = lpin[t].NDISTANCEEND,
                             nMtKey = lpin[t].nMtKey,
                             nPipeKey = lpin[t].nPipeKey,
                             NBUILDTYPE =lpin[t].NBUILDTYPE,
                             ISEDITED = lpin[t].ISEDITED

                         });
                        }
                    }


                }
                else
                {
                    if (_editpassport)
                    {
                        titleTextBlock.Text = ProjectResources.PrivControlEditAdd;
                        titleTextBlock.VerticalAlignment = VerticalAlignment.Center;
                        titleTextBlock.FontStyle = FontStyles.Italic;
                        titleTextBlock.Foreground = new SolidColorBrush(Color.FromArgb(
                                             255,
                                             Byte.Parse("165"),
                                             Byte.Parse("165"),
                                             Byte.Parse("165")));
                         titleTextBlock.Visibility = Visibility.Visible;
                    }
                }

                


            }

            if (e.PropertyName.Equals("KeyPrivAdd"))
            {
                //перезаписываем привязку из 
                 List<PrivItems> lpin = new List<PrivItems>();
                lpin = _linkTableWindow2.ListprivItemsNew;
                _privItemList.Clear();

                for (int t = 0; t < lpin.Count; t++)
                {
                   
                        _privItemList.Add(
                     new PrivItems
                     {
                         CNAME = lpin[t].CNAME,
                         NKMORT1 = lpin[t].NKMORT1,
                         NDISTANCEBEG = lpin[t].NDISTANCEBEG,
                         NX1 = lpin[t].NX1,
                         NY1 = lpin[t].NY1,
                         NZ1 = lpin[t].NZ1,
                         NKEY = lpin[t].NKEY,
                         NH1 = lpin[t].NH1,
                         NKMTRUE1 = lpin[t].NKMTRUE1,
                         NX2 = lpin[t].NX2,
                         NY2 = lpin[t].NY2,
                         NZ2 = lpin[t].NZ2,
                         NH2 = lpin[t].NH2,
                         NKMORT2 = lpin[t].NKMORT2,
                         NKMTRUE2 = lpin[t].NKMTRUE2,
                         NDISTANCEEND = lpin[t].NDISTANCEEND,
                         nMtKey = lpin[t].nMtKey,
                         nPipeKey = lpin[t].nPipeKey,
                         NBUILDTYPE = lpin[t].NBUILDTYPE,
                         ISEDITED = lpin[t].ISEDITED

                     });
                  
                }
                linkPriv.Children.Clear();
                    if (_privItemList.Count > 0)
                    {
                        titleTextBlock.Visibility = Visibility.Collapsed;
                        for (int ii = 0; ii < _privItemList.Count; ii++)
                        {
                            PrivLinkControl plc = PrivLinkControl.CreateLinkControlEdit(_privItemList[ii], _passportModel,
                                                                                        _metaData,
                                                                                        true, true, _editpassport);

                            linkPriv.Children.Add(plc);
                        }
                    }
                    else
                    {
                        if (_editpassport)
                        {
                            titleTextBlock.Text = ProjectResources.PrivControlEditAdd;
                            titleTextBlock.VerticalAlignment = VerticalAlignment.Center;
                            titleTextBlock.FontStyle = FontStyles.Italic;
                            titleTextBlock.Foreground = new SolidColorBrush(Color.FromArgb(
                                                 255,
                                                 Byte.Parse("165"),
                                                 Byte.Parse("165"),
                                                 Byte.Parse("165")));
                        }

                    }


            }
          
        }

        #region IControlValueChanged Members

        public bool HasChanges
        {

            get
            {

                return true;
            }
        }

        public string NewValue
        {
            get
            {

                string xmls = "";
                SavePriv.LISTSAVEPRIV listsavpriv = new SavePriv.LISTSAVEPRIV();
                listsavpriv.LISTPRIV = new List<SavePriv.SAVEPRIVITEMS>();

                if (_privItemList.Count == 0)
                {
                    xmls = "";
                }
                else
                {
                    for (int ii = 0; ii < _privItemList.Count; ii++)
                    {

                        SavePriv.SAVEPRIVITEMS privItems = new SavePriv.SAVEPRIVITEMS();

                        privItems.CNAME = "";
                        privItems.NDISTANCEBEG = _privItemList[ii].NDISTANCEBEG;
                        privItems.NDISTANCEEND = _privItemList[ii].NDISTANCEEND;
                        privItems.NH1 = _privItemList[ii].NH1;
                        privItems.NH2 = _privItemList[ii].NH2;
                        privItems.NKEY = _privItemList[ii].NKEY;
                        privItems.NKMORT1 = _privItemList[ii].NKMORT1;
                        privItems.NKMORT2 = _privItemList[ii].NKMORT2;
                        privItems.NKMTRUE1 = _privItemList[ii].NKMTRUE1;
                        privItems.NKMTRUE2 = _privItemList[ii].NKMTRUE2;
                        privItems.NX1 = _privItemList[ii].NX1;
                        privItems.NX2 = _privItemList[ii].NX2;
                        privItems.NY1 = _privItemList[ii].NY1;
                        privItems.NY2 = _privItemList[ii].NY2;
                        privItems.NZ1 = _privItemList[ii].NZ1;
                        privItems.NZ2 = _privItemList[ii].NZ2;
                        privItems.NMTKEY = _privItemList[ii].nMtKey;
                        privItems.NPIPEKEY = _privItemList[ii].nPipeKey;
                        if (string.IsNullOrEmpty(_privItemList[ii].NBUILDTYPE))
                        {
                            privItems.NBUILDTYPE = "0";
                        }
                        else
                        {
                            privItems.NBUILDTYPE = _privItemList[ii].NBUILDTYPE;
                        }
                        if (string.IsNullOrEmpty(_privItemList[ii].ISEDITED))
                        {
                            privItems.ISEDITED = "0";
                        }
                        else
                        {
                            privItems.ISEDITED = _privItemList[ii].ISEDITED;
                        }

                        listsavpriv.LISTPRIV.Add(privItems);

                }
                    xmls = XmlHelper.InternalSerializer<SavePriv.LISTSAVEPRIV>(listsavpriv);
                }

               
               // string jsonExport = JsonHelper.JsonSerializer<SavePriv.LISTSAVEPRIV>(listsavpriv);

                return xmls;
            }
        }


        public FieldMetaDataItem MetaData
        {
            get { return _metaData; }
        }

        #endregion




    }
}


//удаление привязки
//private void OnDeleteButtonClick(object sender, RoutedEventArgs e)
//{
//    //временно убрала грид и кнопку
//    //artifactIdOnDeletePriv = GridHelper.GetArtifactIdByClick(sender, nameartifactIdOnDeletePriv.ToUpper(), grid);

//    _popupWindowDeleteChild.Title = ProjectResources.GridControlMessageTitle; // "Подтверждение удаления";
//    _popupWindowDeleteChild.titleBox.Text = ProjectResources.PrivControlDelete; //"Удалить привязку?";
//    _popupWindowDeleteChild.Show();
//    _popupWindowDeleteChild.OKButtonDelete.Click += OKButtonDelete;
//}

//void OKButtonDelete(object sender, RoutedEventArgs e)
//{
//    _popupWindowDeleteChild.OKButtonDelete.Click -= OKButtonDelete;

//    if (_popupWindowDeleteChild.DialogResult == true)
//    {

//        //удаляем данные и перестраиваем таблицу!!!

//        ServiceDataClient.DeleteLinkToPipeByKeyCompleted += ServiceDataClient_DeleteLinkToPipeByKeyCompleted;
//        ServiceDataClient.DeleteLinkToPipeByKeyAsync(artifactIdOnDeletePriv, GlobalContext.Context);

//        //ServiceDataClient.DELETELOCATIONCompleted += ServiceDataClient_DELETELOCATIONCompleted;
//        //ServiceDataClient.DELETELOCATIONAsync(_passportModel.PassportKey);
//        //Model.DeletePassport(artifactIdOnDeleteChild);
//        //ServiceDataClient.GetGridDataCompleted += OnGetGridDataCompleted;
//        //ServiceDataClient.GetGridDataAsync(_keyChild, "0", "1", _currentPassportKey);


//    }
//}

//void ServiceDataClient_DeleteLinkToPipeByKeyCompleted(object sender, DeleteLinkToPipeByKeyCompletedEventArgs e)
//{
//    ServiceDataClient.DeleteLinkToPipeByKeyCompleted -= ServiceDataClient_DeleteLinkToPipeByKeyCompleted;
//    if (e.Result.IsValid)
//    {
//        StartOnGetGridDataPriv(_passportModel.PassportKey);

//        //временно убрала грид и кнопку
//        //addPriv.Visibility = Visibility.Visible;
//    }
//    else
//    {
//        _passportModel.MainModel.Report(" Удаление привязки err = " + e.Result.ErrorMessage);
//    }
//}


//void ServiceDataClient_GetFldParamsprivCompleted(object sender, GetFldParamsprivCompletedEventArgs e)
//{
//    ServiceDataClient.GetFldParamsprivCompleted -= ServiceDataClient_GetFldParamsprivCompleted;
//    if (e.Result.IsValid)
//    {
//        FldParamsPriv = new List<FldParamsPrivItems>(e.Result.FldParamsPrivItemsList);
//        //по ключу возвращает привязку - если есть

//        ServiceDataClient.GetAllPrivCompleted += ServiceDataClient_GetAllPrivCompleted;
//        ServiceDataClient.GetAllPrivAsync(_passportModel.PassportKey, GlobalContext.Context);

//        //возвращение привязки для контрола типа грид
//        //ServiceDataClient.GetGridDataPrivCompleted += ServiceDataClient_GetGridDataPrivCompleted;
//        //ServiceDataClient.GetGridDataPrivAsync(_passportModel.PassportKey, GlobalContext.Context);
//        ////ServiceDataClient.GetGridDataPrivAsync("24852101");

//    }
//    else
//    {
//        _passportModel.MainModel.Report(" Поля вывода в грид привязки err = " + e.Result.ErrorMessage);
//    }
//}

//КНОПКУ ВСЕГДА ПОКАЗЫВАЕМ  - ОНА ДЛЯ РЕДАКТИРОВАНИЯ!!!

//if (_passportModel.ISLOCATIONEXIST == "1")
//{

//    addPriv.Visibility = Visibility.Collapsed;
//}
//else
//{
//    if (_editpassport)
//    {

//        addPriv.Visibility = Visibility.Visible;
//    }

//    else
//    {
//        addPriv.Visibility = Visibility.Collapsed;
//    }
//}