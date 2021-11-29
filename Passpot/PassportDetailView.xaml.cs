using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Media.Interfaces;
using Media.Resources;
using Passpot.Business;
using Passpot.Business.DataTable;
using Passpot.Controls;
using Passpot.Controls.Grid;
using Passpot.Grid;
using System.Windows.Browser;
using DC.FileUpload;
using linkControl.Control;
using Services.ServiceReference;
using Media;
using Passpot.Model;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;
using SelectionChangedEventArgs = System.Windows.Controls.SelectionChangedEventArgs;



namespace Passpot
{
    public partial class PassportDetailView : UserControl
    {


        #region Enums


        #endregion Enums


        #region Private fields

       
        private BaseDataControl _dce;// Главный динамический контрол
        private TextBoxAndButton _tbab; //динамический контрол с кнопочкой
        private TreeControl _treeControl;
        private ChildWindowHistoriPassport _popupWindowHistory;
        private MainView _mainView;
        private PassportDetailOpenParam _passportDetailOpenParam;
        private string visibleButtonTullbar;
        private ChildWindowAttantion _popupWindowAttantion;
        private bool ismedia = false; //признак того, что перешли на вкладку media
        private ChildWindowDelete _popupWindowDelete;
        private int mediaType = 0;
        PhotoBrowserContol fotoControl = new PhotoBrowserContol();
        
        

        #endregion Private fields

        public PassportDetailView()
        {
            InitializeComponent();
        }

        #region Properties

        public PassportDetailModel Model
        {
            get { return DataContext as PassportDetailModel; }
        }


        #endregion Properties

        #region Public methods


        #endregion Public methods

        #region Private methods

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Model.PropertyChanged -= Model_PropertyChanged;
            Model.PropertyChanged += Model_PropertyChanged;
           //если новый паспорт то  медиа - недоступны
            if (Model.PassportKey == "0")
            {
                mediaTabItem.IsEnabled = false;
            }
            else
            {
                mediaTabItem.IsEnabled = true;
            }
        }

        //события
        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("ModelInited"))
            {
                List<FieldMetaDataItem> metaDataList = new List<FieldMetaDataItem>();


                if (DisplayLabel.Dict.ContainsKey("ParentNameLabel"))
                {
                    tbLabelParent.Text = DisplayLabel.Dict["ParentNameLabel"];

                }
                if (DisplayLabel.Dict.ContainsKey("ObjectNameLabel"))
                {
                    tbnameObj.Text = DisplayLabel.Dict["ObjectNameLabel"];

                }

                //тултипы для кнопок во вкладке данные редактировать сохранить, кансел
                ToolTipService.SetToolTip(editButton, ProjectResources.editButtonPasport);
                ToolTipService.SetToolTip(saveButton, ProjectResources.saveButtonPasport);
                ToolTipService.SetToolTip(cancelButton, ProjectResources.cancelButtonPasport);


                passportHolder.Children.Clear();

                //новый паспорт
                if (Model.PassportDetailOpenParams.PassportKey == "0")
                {
                    Model.MainModel.Report(" ModelInited!!! для нового паспорта");
                    var image = editButton.Content as Image;
                    Uri uri = new Uri("/Passpot;component/Images/edit_24_d.png", UriKind.Relative);
                    image.Source = new BitmapImage(uri);
                    editButton.IsEnabled = false;

                    var image2 = saveButton.Content as Image;
                    Uri uri2 = new Uri("/Passpot;component/Images/ok_24_a.png", UriKind.Relative);
                    image2.Source = new BitmapImage(uri2);
                    saveButton.IsEnabled = true;

                    var image3 = cancelButton.Content as Image;
                    Uri uri3 = new Uri("/Passpot;component/Images/cancel_24_a.png", UriKind.Relative);
                    image3.Source = new BitmapImage(uri3);
                    cancelButton.IsEnabled = true;

                  
                    _dce = BaseDataControl.Create(Model, true);

                }
                else
                {
                    Model.MainModel.Report(" ModelInited!!! для созданного паспорта");

                    if (Model.PassportDetailOpenParams.IsEditedPassport == 0)
                    {

                        if (Model.IsEditedCurrentPassport == 0)
                        {
                            var image3 = editButton.Content as Image;
                            Uri uri3 = new Uri("/Passpot;component/Images/edit_24_d.png", UriKind.Relative);
                            image3.Source = new BitmapImage(uri3);
                            editButton.IsEnabled = false;
                        }
                        

                        var image = saveButton.Content as Image;
                        Uri uri = new Uri("/Passpot;component/Images/ok_24_d.png", UriKind.Relative);
                        image.Source = new BitmapImage(uri);
                        saveButton.IsEnabled = false;

                        var image2 = cancelButton.Content as Image;
                        Uri uri2 = new Uri("/Passpot;component/Images/cancel_24_d.png", UriKind.Relative);
                        image2.Source = new BitmapImage(uri2);
                        cancelButton.IsEnabled = false;

                        metaDataList = Model.MetaDataList;
                        _dce = BaseDataControl.Create(Model, false);


                    }
                    else
                    {
                        var image = editButton.Content as Image;
                        Uri uri = new Uri("/Passpot;component/Images/edit_24_d.png", UriKind.Relative);
                        image.Source = new BitmapImage(uri);
                        editButton.IsEnabled = false;

                        var image2 = saveButton.Content as Image;
                        Uri uri2 = new Uri("/Passpot;component/Images/ok_24_a.png", UriKind.Relative);
                        image2.Source = new BitmapImage(uri2);
                        saveButton.IsEnabled = true;

                        var image3 = cancelButton.Content as Image;
                        Uri uri3 = new Uri("/Passpot;component/Images/cancel_24_a.png", UriKind.Relative);
                        image3.Source = new BitmapImage(uri3);
                        cancelButton.IsEnabled = true;

                        metaDataList = Model.MetaEditDataList;
                     
                        _dce = BaseDataControl.Create(Model, true);
                       
                    }
                   
                }

                //показывать-скрывать закладку медиаматериалы
                int findMedia = 0;


                if (Model.PassportDetailOpenParams.PassportKey != "0")
                {
                    for (int i = 0; i < metaDataList.Count; i++)
                    {
                        FieldMetaDataItem meta = metaDataList[i];
                        if (meta.FLDNAME == "Media")
                        {
                            findMedia = 1;
                            if (meta.IS_VISIBLE == 0)
                            {
                                mediaTabItem.Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                mediaTabItem.Visibility = Visibility.Visible;
                            }

                        }
                    }

                    //если в метаданных не пришло поле Media (всегда показываем)
                    if (findMedia == 0)
                    {
                        mediaTabItem.Visibility = Visibility.Visible;
                    }

                }

               
                

                if (!Model.isFindTreeonkey)
                {
                    Model.FirePropertyChanged("FindTreeOnkeyPassport");
                }

                passportHolder.Children.Add(_dce);

                pasportDataScroll.UpdateLayout();

                return;
            }
            if (e.PropertyName == "DisplayLabel")
            {
                if (DisplayLabel.Dict.ContainsKey("ParentNameLabel"))
                {
                    tbLabelParent.Text = DisplayLabel.Dict["ParentNameLabel"];

                }
                if (DisplayLabel.Dict.ContainsKey("ObjectNameLabel"))
                {
                    tbnameObj.Text = DisplayLabel.Dict["ObjectNameLabel"];

                }
            }

            if (e.PropertyName.Equals("EntityKey"))
            {
                var entityKey = Model.EntityKey;
            }

            if (e.PropertyName.Equals("NamePlaseState"))
            {

                if (Model.NamePlaseState != "")
                {
                    namePlaseState.Text = Model.NamePlaseState + " ";
                    namePlaseState.TextTrimming = TextTrimming.WordEllipsis;
                }
                else
                {
                    //пока не понятно для чего это сделано!
                    //plaseState.Visibility = Visibility.Collapsed;
                    namePlaseState.Visibility = Visibility.Collapsed;

                }

                return;
            }


            //if (e.PropertyName.Equals("IsEditedCurrentPassport"))
            //{
            //    if (Model.IsEditedCurrentPassport == 1)
            //    {
                    
            //    }
            //}

            //пасспорт сохранен
            if (e.PropertyName.Equals(("ReportOnSavePassport")))
            {

                Model.MainModel.Report(Model.ReportOnSavePassport);
            }

            if (e.PropertyName.Equals(("PassportSaved")))
            {
               
                    var image = saveButton.Content as Image;
                    Uri uri = new Uri("/Passpot;component/Images/ok_24_d.png", UriKind.Relative);
                    image.Source = new BitmapImage(uri);
                    saveButton.IsEnabled = false;

                    var image2 = cancelButton.Content as Image;
                    Uri uri2 = new Uri("/Passpot;component/Images/cancel_24_d.png", UriKind.Relative);
                    image2.Source = new BitmapImage(uri2);
                    cancelButton.IsEnabled = false;

                    var image3 = editButton.Content as Image;
                    Uri uri3 = new Uri("/Passpot;component/Images/edit_24_a.png", UriKind.Relative);
                    image3.Source = new BitmapImage(uri3);
                    editButton.IsEnabled = true;
               
            }

            if (e.PropertyName.Equals("ObjTypeAndName"))
            {
                nameEntityPlaseState.Text = Model.EntityParentName + ". ";
                if (Model.EntityParentName!="")
                    nameEntityPlaseState.Text = Model.EntityParentName + ". ";
                if (Model.ObjTypeAndName !="")
                    nameEntityObj.Text = Model.ObjTypeAndName + ".";

                nameDetailObj.Text = Model.NameObjDetail;

                return;
            }

            if (e.PropertyName.Equals("KeyParent"))
            {
                Model.MainModel.NeedOpenPassportDetail = new PassportDetailOpenParam() { PassportKey = Model.KeyParent };

                return;
            }

            if (e.PropertyName.Equals("DeleteMediaEnd"))
            {

                //allBrowserControl.InitPage(Model.PassportKey, true, 0);
                //photoBrowserControl.InitPage(Model.PassportKey, true, 1);
                //shemaBrowserControl.InitPage(Model.PassportKey, true, 2);
                //documentBrowserControl.InitPage(Model.PassportKey, true, 3);
                //factoryPassportBrowserControl.InitPage(Model.PassportKey, true, 4);

                spPhotoBrows.Children.Clear();
                fotoControl.InitPage(Model.PassportKey, true, mediaType);
                spPhotoBrows.Children.Add(fotoControl);
                
            }

            if (e.PropertyName.Equals("AddNewMedia"))
            {
                //allBrowserControl.InitPage(Model.PassportKey, true, 0);
                //photoBrowserControl.InitPage(Model.PassportKey, true, 1);
                //shemaBrowserControl.InitPage(Model.PassportKey, true, 2);
                //documentBrowserControl.InitPage(Model.PassportKey, true, 3);
                //factoryPassportBrowserControl.InitPage(Model.PassportKey, true, 4);

                spPhotoBrows.Children.Clear();
                fotoControl.InitPage(Model.PassportKey, true, mediaType);
                spPhotoBrows.Children.Add(fotoControl);
   
            }

            if (e.PropertyName.Equals("SelectChildList"))
            {
                //убрала вкладку дочерние объекты
                //passportChildView.OnGetGetChildListCompleted(Model.SelectChildList);
                return;
            }

            if (e.PropertyName.Equals("VisibleBuPrintTPA"))
            {
                buexportTPA.Visibility = Model.VisibleBuPrintTPA ? Visibility.Visible : Visibility.Collapsed;
            }




            if (e.PropertyName.Equals("SelectConnectList"))
            {
                string nameField = Model.FieldNameTextonButton;

                if (_dce.Validate())
                {

                    foreach (var control in _dce.Controls)
                    {
                        if (control is TextBoxAndButton)
                        {
                            var cc = control as TextBoxAndButton;
                            if (cc.MetaData.FLDNAME == nameField)
                            {
                                break;
                            }
                        }
                    }
                }

            }

            if (e.PropertyName.Equals("SelectConnectListButtonRelation"))
            {
                string nameField = Model.FieldNameTextonButton;

                if (_dce.Validate())
                {

                    foreach (var control in _dce.Controls)
                    {
                        if (control is ButtonControl)
                        {
                            var cc = control as ButtonControl;
                            if (cc.MetaData.FLDNAME == nameField)
                            {
                                cc.OnGetDataConnect_GridData(Model.SelectConnectList);
                                break;
                            }
                        }
                    }
                }

            }
         
            if (e.PropertyName.Equals("GetCementNKT"))
            {
                Model.MainModel.NeedOpenPassportDetail = new PassportDetailOpenParam() { PassportKey = Model.PassportKey };
                Model.MainModel.FirePropertyChanged("FindTreeOnkeyPassport");
            }

            if (e.PropertyName.Equals("GetHistory"))
            {
                _popupWindowHistory = new ChildWindowHistoriPassport();

                _popupWindowHistory.CreateHistoryGrid(Model.GetHistory, Model.NameObjDetail);
                _popupWindowHistory.Show();
            }

            //динамические кнопочки - на тулбаре
            if (e.PropertyName.Equals("ButtonAdd"))
            {

                //mainToolBar.Items.Clear();
                for (int i = 0; i < Model.ButtonDataList.Count; i++)
                {
                    FielButtonDataItem buttonList = Model.ButtonDataList[i];
                    Model.MainModel.Report("buttonList.CParams = " + buttonList.CParams);
                    Control tc = ButtonInToolBar.CreateControl(buttonList.ContentJpg, buttonList.NameJpg, buttonList.CEvent, buttonList.CAttribute, buttonList.CParams, buttonList.NButtonType, Model);
                    mainToolBar.Items.Add(tc);
                }
            }

            //if (e.PropertyName.Equals("IsShowBusy"))
            //{
            //    Model.IsShow =
            //    busyIndicator1.IsBusy = Model.IsShowBusy;
            //}
        }

        
        private ScrollContentPresenter Scp;    
        void pasportDataScroll_LayoutUpdated(object sender, EventArgs e)
        {
            try
            {

                FrameworkElement myFE = (FrameworkElement)VisualTreeHelper.GetChild(pasportDataScroll, 0);
                Scp = (ScrollContentPresenter)myFE.FindName("ScrollContentPresenter");
                

            }
            catch (Exception)
            {


            }
        }
       

        

        //сохранение паспорта
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            if (ismedia)
            {
                //в медиа - это кнопка сохранить на диск

                if ((Model.keyPassportOnDelete == "") || (Model.keyMediaOnDelete == ""))
                {
                    MessageBox.Show(ProjectResources.NoMedia, ProjectResources.Warning, MessageBoxButton.OK);
                }
                else
                {
                    Model.GetMediaFile(Model.keyMediaOnDelete, Model.nameMediaOnDelete);
                }
            }
            else
            {
                int countErr = 0;

                //отдельный контрол для пароля
                var passwordhash = new Dictionary<string, string>();

                if (_dce.Validate())
                {
                    var result = new PassportData();

                    result.Data = new Dictionary<string, string>();

                    foreach (var control in _dce.Controls)
                    {
                        if (control is IControlValueChanged)
                        {
                            var valueChanged = control as IControlValueChanged;

                            if (valueChanged.MetaData.MANDATORYVALUE_FLD == 1)
                            {

                                if (string.IsNullOrEmpty(valueChanged.NewValue))
                                {
                                    _popupWindowAttantion = new ChildWindowAttantion();
                                    _popupWindowAttantion.titleBox.Text =
                                        "Нельзя сохранить паспорт, т.к. основное поле - " + valueChanged.MetaData.TITUL +
                                        " не заполнено!";
                                    _popupWindowAttantion.Show();
                                    Model.MainModel.Report("Основное поле:" + valueChanged.MetaData.TITUL +
                                                           " = не заполнено");
                                    return;
                                }

                                Model.MainModel.Report("Основное поле:" + valueChanged.MetaData.TITUL + " = " +
                                                       valueChanged.NewValue);

                            }
                            if (valueChanged.HasChanges)
                            {
                                //если тип контрол - пароль то передаем отдельно для сохранения и кеширования пароля на сервере
                                if (valueChanged.MetaData.TYPECONTROL == 11)
                                {
                                    passwordhash.Add(valueChanged.MetaData.FLDNAME, valueChanged.NewValue);
                                }
                                else
                                {
                                    if (valueChanged.MetaData.TYPECONTROL == 9)
                                    {

                                        string tt = valueChanged.NewValue;
                                        result.Data.Add(valueChanged.MetaData.FLDNAME, valueChanged.NewValue);
                                    }
                                    else
                                    {
                                        result.Data.Add(valueChanged.MetaData.FLDNAME, valueChanged.NewValue);
                                    }

                                }

                            }

                        }
                    }

                    //ПРИВЯЗКА!!!!! делать проверку на пустой сттринг - всегда отправлять данные в базу

                    String strResult = "";
                    //делаем проверку - были ли изменения вообще
                    // НЕ ЗАБЫТЬ!!!! Сделать валидацию на сохранение паспорта!!!

                    if (((result.Data.Count > 0) || (passwordhash.Count > 0)) && (countErr == 0))
                    {
                        // busyIndicator1.IsBusy = true;
                        Model.IsShow = true;
                        Model.SavePassport(Model.PassportKey, Model.PassportDetailOpenParams.EntityKey,
                                           Model.PassportDetailOpenParams.ParentKey, result, passwordhash);
                    }
                    else
                    {
                        if (countErr == 0)
                        {
                            if (Model.PassportKey != "0")
                            {
                                Model.MainModel.NeedOpenPassportDetail = new PassportDetailOpenParam()
                                    {
                                        PassportKey = Model.PassportKey,
                                        EntityKey = Model.EntityKey,
                                        IsVisibleButtonTransit = 1,
                                        ParentNameKeyOnAdmin = Model.ParentNameKeyOnAdmin
                                    };
                            }

                        }

                    }

                }
                else
                {
                    _popupWindowAttantion = new ChildWindowAttantion();
                    _popupWindowAttantion.titleBox.Text = "Поля не заполнены корректно";
                    _popupWindowAttantion.Show();

                }
            }
        }


        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            //если на вкладке media
            if (ismedia)
            {
                //ChildWindowMedia childWindowMedia = new ChildWindowMedia();
                if (mediaType == 0)
                {
                    MessageBox.Show(ProjectResources.NoMediaType, ProjectResources.Warning, MessageBoxButton.OK);
                }
                else
                {
                    var childWindowMedia = new ChildWindowMedia().CreateControl(Model.PassportKey, GlobalContext.Context, mediaType, Model);
                    childWindowMedia.Show();    
                }
                
                

            }
            else
            {
                Model.MainModel.Report(" закоментировано stackPanelOkCancelSaveData.Visibility ");

                var image = editButton.Content as Image;
                Uri uri = new Uri("/Passpot;component/Images/edit_24_d.png", UriKind.Relative);
                image.Source = new BitmapImage(uri);
                editButton.IsEnabled = false;

                var image2 = saveButton.Content as Image;
                Uri uri2 = new Uri("/Passpot;component/Images/ok_24_a.png", UriKind.Relative);
                if (image2 != null) image2.Source = new BitmapImage(uri2);
                saveButton.IsEnabled = true;

                var image3 = cancelButton.Content as Image;
                Uri uri3 = new Uri("/Passpot;component/Images/cancel_24_a.png", UriKind.Relative);
                image3.Source = new BitmapImage(uri3);
                cancelButton.IsEnabled = true;

                passportHolder.Children.Clear();

                // _dce = DataEditControl.Create(Model, true);
                // passportHolder.Children.Add(_dce);

                Model.MainModel.NeedOpenPassportDetail = new PassportDetailOpenParam() { PassportKey = Model.PassportKey, EntityKey = Model.EntityKey, IsEditedPassport = 1, IsVisibleButtonTransit = 1, ParentNameKeyOnAdmin = Model.ParentNameKeyOnAdmin };    
            }
            
        }

        //отменить  - выход из редактирования без сохранения
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (ismedia)
            {
                //в медиа - это кнопка удалить

                if (Model.keyMediaOnDelete == "")
                {
                    MessageBox.Show(ProjectResources.NoMedia, ProjectResources.Warning, MessageBoxButton.OK);
                }
                else
                {
                    _popupWindowDelete = new ChildWindowDelete();
                    _popupWindowDelete.Title = ProjectResources.GridControlMessageTitle;//"Подтверждение удаления";
                    _popupWindowDelete.titleBox.Text = ProjectResources.MediaDel + "  " + Model.nameMediaOnDelete; //"Удалить медиаматериал?";
                    _popupWindowDelete.Show();
                    _popupWindowDelete.OKButtonDelete.Click += OKButtonDelete;


                    
                }
            }
            else
            {
                if (Model.PassportDetailOpenParams.PassportKey == "0")
                {
                    passportHolder.Children.Clear();
                    if (Model.PassportDetailOpenParams.IsChildPassport == 1)
                    {
                        Model.MainModel.NeedOpenPassportDetail = new PassportDetailOpenParam()
                        {
                            PassportKey = Model.PassportDetailOpenParams.ParentKey,
                            EntityKey = Model.PassportDetailOpenParams.EntityKey,
                            IsEditedPassport = 0,
                            IsChildPassport = 0,
                            IsVisibleButtonTransit = Model.PassportDetailOpenParams.IsVisibleButtonTransit
                        };
                    }
                    else
                    {
                        Model.MainModel.FirePropertyChanged("ClosePassportDetal");
                    }

                }
                else
                {
                    passportHolder.Children.Clear();

                    var image = saveButton.Content as Image;
                    Uri uri = new Uri("/Passpot;component/Images/ok_24_d.png", UriKind.Relative);
                    image.Source = new BitmapImage(uri);
                    saveButton.IsEnabled = false;

                    var image2 = cancelButton.Content as Image;
                    Uri uri2 = new Uri("/Passpot;component/Images/cancel_24_d.png", UriKind.Relative);
                    image2.Source = new BitmapImage(uri2);
                    cancelButton.IsEnabled = false;

                    var image3 = editButton.Content as Image;
                    Uri uri3 = new Uri("/Passpot;component/Images/edit_24_a.png", UriKind.Relative);
                    image3.Source = new BitmapImage(uri3);
                    editButton.IsEnabled = true;

                    Model.MainModel.NeedOpenPassportDetail = new PassportDetailOpenParam()
                    {
                        PassportKey = Model.PassportKey,
                        EntityKey = Model.PassportDetailOpenParams.EntityKey,
                        IsVisibleButtonTransit = Model.PassportDetailOpenParams.IsVisibleButtonTransit

                    };

                }
            }

            
        }


        void OKButtonDelete(object sender, RoutedEventArgs e)
        {
            _popupWindowDelete.OKButtonDelete.Click -= OKButtonDelete;

            if (_popupWindowDelete.DialogResult == true)
            {
                //удаление медиаметариалов
                Model.DeleteMedia(Model.PassportKey, Model.keyMediaOnDelete, Model.nameMediaOnDelete);
            }
        }

        #endregion Private methods

        // RESERVED
        //private void FileUploadedHandler(object sender, EventArgs e)
        //{
        //    // примерно так выглядит обработчик для fileUploader.FileUploaded
        //    var tab = generalTabControl.SelectedItem as TabItem;
        //    if (tab != null)
        //    {
        //        LoadMediaForCurrentTab(tab, true);
        //    }
        //}

       

        private void namePlaseState_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Model.KeyParent = Model.KeyParentInfo;
        }

        private void namePlaseState_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            
            namePlaseState.Foreground = new SolidColorBrush(Color.FromArgb(255, Byte.Parse("219"), Byte.Parse("255"), Byte.Parse("180")));
            namePlaseState.FontWeight = FontWeights.Bold;
        }

        private void namePlaseState_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            namePlaseState.Foreground = new SolidColorBrush(Colors.White);
            namePlaseState.FontStyle = FontStyles.Normal;
            namePlaseState.FontWeight = FontWeights.Normal;

        }

        //показ истории паспорта
        private void historiButton_Click(object sender, RoutedEventArgs e)
        {
            Model.StartDataHistory(Model.PassportKey);


        }

        //размер контрола
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
 
           generalTabControl.Height = Math.Max(500, this.ActualHeight -82);
           generalTabControl.Width = this.ActualWidth;
  
        }

        private void buExcel_Click(object sender, RoutedEventArgs e)
        {
            //создлаем таблицу
            DataTable dt = GridToOffic.GetDataTableOffic(Model);

            //создаем виртуальный грид telerik
            RadGridView gridExel = GridToOffic.CreateGrid(dt);

            string extension = "xls";
           ExportFormat format = ExportFormat.Html;

            SaveFileDialog dialog = new SaveFileDialog()
            {
                DefaultExt = extension,
                Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", extension, "Excel"),
                FilterIndex = 1
            };

            if (dialog.ShowDialog() == true)
            {
                using (Stream stream = dialog.OpenFile())
                {
                    GridViewExportOptions exportOptions = new GridViewExportOptions();
                    exportOptions.Format = format;
                    exportOptions.ShowColumnFooters = true;
                    exportOptions.ShowColumnHeaders = true;
                    exportOptions.ShowGroupFooters = true;
                    exportOptions.Culture = new System.Globalization.CultureInfo("ru-RU");
                    exportOptions.Encoding = System.Text.Encoding.UTF8;

                    gridExel.Export(stream, exportOptions);
                }
            }

        }

        private void buWord_Click(object sender, RoutedEventArgs e)
        {
            //создлаем таблицу
            DataTable dt = GridToOffic.GetDataTableOffic(Model);

            //создаем виртуальный грид telerik
            RadGridView gridWord = GridToOffic.CreateGrid(dt);

            string extension = "doc";
            ExportFormat format = ExportFormat.Html;

            SaveFileDialog dialog = new SaveFileDialog()
            {
                DefaultExt = extension,
                Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", extension, "Word"),
                FilterIndex = 1
            };

            if (dialog.ShowDialog() == true)
            {
                using (Stream stream = dialog.OpenFile())
                {
                    GridViewExportOptions exportOptions = new GridViewExportOptions();
                    exportOptions.Format = format;
                    exportOptions.ShowColumnFooters = true;
                    exportOptions.ShowColumnHeaders = true;
                    exportOptions.ShowGroupFooters = true;
                   // exportOptions.Culture = new System.Globalization.CultureInfo("ru-RU");
                    exportOptions.Encoding = System.Text.Encoding.UTF8;

                    gridWord.Export(stream, exportOptions);
                }
            }

        }

        private void bu_Click(object sender, RoutedEventArgs e)
        {
            Model.MainModel.FirePropertyChanged("ClosePassportDetal");
        }

        //событие на клик по закладке
        private void GeneralTabControl_OnSelectionChanged(object sender, RadSelectionChangedEventArgs e)
        {
            if (generalTabControl != null)
            {
                if (generalTabControl.SelectedItem is RadTabItem)
                {
                    var tab = generalTabControl.SelectedItem as RadTabItem;

                    // Please delete it. For debug only.
                    Model.MainModel.Report(tab.Name);

                    if (tab.Name.Equals("mediaTabItem", StringComparison.OrdinalIgnoreCase))
                    {

                        //тултипы для кнопок во вкладке медиаматериалы редактировать сохранить, кансел
                        ToolTipService.SetToolTip(editButton, ProjectResources.ExpanderAddNewMedia);
                        ToolTipService.SetToolTip(saveButton, ProjectResources.saveMediaOnDisk);
                        ToolTipService.SetToolTip(cancelButton, ProjectResources.MediaDelToolTip);

                        var image = editButton.Content as Image;
                        Uri uri = new Uri("/Passpot;component/Images/add_24_a.png", UriKind.Relative);
                        image.Source = new BitmapImage(uri);
                        editButton.IsEnabled = true;

                        var image2 = saveButton.Content as Image;
                        Uri uri2 = new Uri("/Passpot;component/Images/save_24_a.png", UriKind.Relative);
                        image2.Source = new BitmapImage(uri2);
                        saveButton.IsEnabled = true;

                        var image3 = cancelButton.Content as Image;
                        Uri uri3 = new Uri("/Passpot;component/Images/delete_24_a.png", UriKind.Relative);
                        image3.Source = new BitmapImage(uri3);
                        cancelButton.IsEnabled = true;

                        //признак что нажали вкладку media
                        ismedia = true;

                        spPhotoBrows.Children.Clear();
                        fotoControl= new PhotoBrowserContol();
                        fotoControl.InitPage(Model.PassportKey, false, mediaType);
                        spPhotoBrows.Children.Add(fotoControl);
                        
                        //allBrowserControl.InitPage(Model.PassportKey, false, 0);
                      
                    }
                    else
                        if (tab.Name.Equals("dataTabItem", StringComparison.OrdinalIgnoreCase))
                        {
                            //признак что перешли на другую вкладку, не media
                            ismedia = false;

                            //тултипы для кнопок во вкладке данные редактировать сохранить, кансел
                            ToolTipService.SetToolTip(editButton, ProjectResources.editButtonPasport);
                            ToolTipService.SetToolTip(saveButton, ProjectResources.saveButtonPasport);
                            ToolTipService.SetToolTip(cancelButton, ProjectResources.cancelButtonPasport);


                            if (Model.PassportDetailOpenParams.IsEditedPassport == 0)
                            {
                                var image = saveButton.Content as Image;
                                Uri uri = new Uri("/Passpot;component/Images/ok_24_d.png", UriKind.Relative);
                                image.Source = new BitmapImage(uri);
                                saveButton.IsEnabled = false;

                                var image2 = cancelButton.Content as Image;
                                Uri uri2 = new Uri("/Passpot;component/Images/cancel_24_d.png", UriKind.Relative);
                                image2.Source = new BitmapImage(uri2);
                                cancelButton.IsEnabled = false;

                                var image3 = editButton.Content as Image;
                               
                                if (Model.IsEditedCurrentPassport == 1)
                                {
                                    Uri uri3 = new Uri("/Passpot;component/Images/edit_24_a.png", UriKind.Relative);
                                    image3.Source = new BitmapImage(uri3);
                                    editButton.IsEnabled = true;
                                }
                                else
                                {
                                    Uri uri3 = new Uri("/Passpot;component/Images/edit_24_d.png", UriKind.Relative);
                                    image3.Source = new BitmapImage(uri3);
                                    editButton.IsEnabled = false;
                                }
                               
                            }
                            else
                            {
                                var image = editButton.Content as Image;
                                Uri uri = new Uri("/Passpot;component/Images/edit_24_d.png", UriKind.Relative);
                                image.Source = new BitmapImage(uri);
                                editButton.IsEnabled = false;
                                

                                var image2 = saveButton.Content as Image;
                                Uri uri2 = new Uri("/Passpot;component/Images/ok_24_a.png", UriKind.Relative);
                                image2.Source = new BitmapImage(uri2);
                                saveButton.IsEnabled = true;

                                var image3 = cancelButton.Content as Image;
                                Uri uri3 = new Uri("/Passpot;component/Images/cancel_24_a.png", UriKind.Relative);
                                image3.Source = new BitmapImage(uri3);
                                cancelButton.IsEnabled = true;
                               
                            }
                        }
                       
                }
            }
        }
 

        private void BuAll_OnClick(object sender, RoutedEventArgs e)
        {
            mediaType = 0;
            InitFotoControl(mediaType);
        }

        private void BuPhoto_OnClick(object sender, RoutedEventArgs e)
        {
            mediaType = 1;
            InitFotoControl(mediaType);
        }

        private void BuShema_OnClick(object sender, RoutedEventArgs e)
        {
            mediaType = 2;
            InitFotoControl(mediaType);
        }

        private void BuDoc_OnClick(object sender, RoutedEventArgs e)
        {
            mediaType = 3;
            InitFotoControl(mediaType);
        }

        private void BuOther_OnClick(object sender, RoutedEventArgs e)
        {
            mediaType = 4;
            InitFotoControl(mediaType);
        }

        private void InitFotoControl(int mediaType)
        {
            var image = editButton.Content as Image;
            Uri uri = new Uri("/Passpot;component/Images/add_24_a.png", UriKind.Relative);
            image.Source = new BitmapImage(uri);
            editButton.IsEnabled = true;
            spPhotoBrows.Children.Clear();
            fotoControl = new PhotoBrowserContol();
            fotoControl.InitPage(Model.PassportKey, false, mediaType);
            spPhotoBrows.Children.Add(fotoControl);
        }


        //Временная кнопка!! - убрать!
        private void buExportTPA_OnClick(object sender, RoutedEventArgs e)
        {
            if (Model.EntityKey == "15936672")
            {
                string path = HtmlPage.Document.DocumentUri.AbsoluteUri;
                int indexSlash = path.LastIndexOf('/');
                path = path.Substring(0, indexSlash);
                string uri = path + "/ShablonExport/ExportFromTPA.aspx?passportIDParent=" + Model.KeyParentInfo;
                HtmlPage.Window.Navigate(new Uri(uri), "_blank");
            }
            else
            {
                if (Model.EntityKey == "15936870")//40873
                {
                    string path = HtmlPage.Document.DocumentUri.AbsoluteUri;
                    int indexSlash = path.LastIndexOf('/');
                    path = path.Substring(0, indexSlash);
                    string uri = path + "/ShablonExport/ExportFromUKZ.aspx?passportIDParent=" + Model.PassportKey; //.KeyParentInfo;
                    HtmlPage.Window.Navigate(new Uri(uri), "_blank");
                }
            }




            // string uri = "http://localhost:26313/BaseApp/Modules/Passport/ShablonExport/ExportFromTPA.aspx?passportIDParent=" + Model.KeyParentInfo;
            //  HtmlPage.Window.Navigate(new Uri(uri), "_blank");
            //HtmlPage.Window.Navigate(new Uri(uri));

        }


       
    }
}


 