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
    public partial class LinkControlEdit : UserControl
    {
        private PassportDetailModel _passportModel;
        public bool klickButton = false;
        public string KeyLink { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private ChildWindowDelete _popupWindowDelete;
        private ServiceDataClient _serviceDataClient;
        private FieldMetaDataItem _metaData;
         private List<DataListRelationObjItems> _selectRelationObjGridList = new List<DataListRelationObjItems>();
         private List<DataConnectList> _selectConnectList;
         private ChildWindowLinkObj _linkTableWindow;
        public DataListRelationObjItems newItems;
       
        private bool _noLinks = false;


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

        public static LinkControlEdit CreateLinkControlEdit(DataListRelationObjItems relationList, PassportDetailModel passportModel, 
            FieldMetaDataItem metaData,  List<DataListRelationObjItems> selectRelationObjGridList, bool noLinks, bool visiblebuAdd, bool oneLink )
        {
            var c = new LinkControlEdit();
            c._noLinks = noLinks;

            if (c._noLinks)
            {
                
                c.button_delRelation.Visibility = Visibility.Collapsed;
                c.titleTextBlock.VerticalAlignment = VerticalAlignment.Center;
                c.titleTextBlock.Text = ProjectResources.LinkControlEditAdd;
                c.titleTextBlock.FontStyle = FontStyles.Italic;
                c.titleTextBlock.Foreground = new SolidColorBrush(Color.FromArgb(
                                     255,
                                     Byte.Parse("165"),
                                     Byte.Parse("165"),
                                     Byte.Parse("165")));

            }
            else
            {
                c.button_delRelation.Tag = relationList.KeyObj;
                c.titleTextBlock.Text = relationList.NameObj.TrimStart();    
            }

            c._passportModel = passportModel;
            c._metaData = metaData;
            c._selectRelationObjGridList = selectRelationObjGridList;

         
            return c;
        }

        public LinkControlEdit()
        {
            InitializeComponent();
        }

        private void Button_delRelation_OnClick(object sender, RoutedEventArgs e)
        {
            KeyLink = button_delRelation.Tag.ToString();
            _popupWindowDelete = new ChildWindowDelete();
            _popupWindowDelete.Title = ProjectResources.GridControlMessageTitle; //"Подтверждение удаления";
            _popupWindowDelete.titleBox.Text = ProjectResources.GridControlMessageDel + titleTextBlock.Text;  //"Удалить связь  с объектом  "
            _popupWindowDelete.Show();
            _popupWindowDelete.OKButtonDelete.Click += OKButtonDelete;

         
        }
        //удалить связь
        void OKButtonDelete(object sender, RoutedEventArgs e)
        {
            _popupWindowDelete.OKButtonDelete.Click -= OKButtonDelete;

            if (_popupWindowDelete.DialogResult == true)
            {
                klickButton = true;
                Model.FirePropertyChanged("KeyLinkDelete");      
            }
        }
        #region //добавить связь

        //перенесено в основную форму

        //добавить связь
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
        //        _linkTableWindow = new ChildWindowLinkObj();
        //        _linkTableWindow.Title = ProjectResources.ChildWindowLinkObjTitle; //"Связи объекта ";
        //        _linkTableWindow.DataContext = m;
        //        _linkTableWindow.StubLinkData(_selectConnectList, _metaData.FLDNAME, allKeyRelation);
        //        _linkTableWindow.Show();
        //        _linkTableWindow.btnOk.Click += ButtonOkClick;

        //    }
        //    else
        //    {
        //        _passportModel.MainModel.Report("Список связей OnGetDataConnect err = " + e.Result.ErrorMessage);
        //    }
        //}

        //public void ButtonOkClick(object sender, RoutedEventArgs e)
        //{
           

        //    _linkTableWindow.btnOk.Click -= ButtonOkClick;

        //    var t = _linkTableWindow.LinkOnGridItem;

        //    //if (_noLinks)
        //    //{
        //    //    _selectRelationObjGridList.Clear();
        //    //}
        //    //_selectRelationObjGridList.Add(new DataListRelationObjItems { KeyObj = t.ObjKey, NameEntity = t.EntityKey, NameObj = t.ObjName });
        //    newItems = null;
        //    newItems = new DataListRelationObjItems();
        //    newItems.KeyObj = t.ObjKey;
        //    newItems.NameObj = t.ObjName;
        //    newItems.NameEntity = t.EntityKey;

        //    Model.FirePropertyChanged("KeyLinkAdd"); 

        //}

        #endregion

        
    }
}
