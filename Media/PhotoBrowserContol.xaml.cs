using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

using Media.Interfaces;
using Media.Resources;
using Services.ServiceReference;




namespace Media
{
    public partial class PhotoBrowserContol : UserControl
    {

        #region Private fields

        private string _currentPassportKey;
        private MediaChildWindowDelete _popupWindowDeleteMedia;
        private ServiceDataClient _serviceDataClient;
        string keyMedia;
        private string nameMedia;
        private int newMediaType = 0;
        private SaveFileDialog dialog = new SaveFileDialog();
        /// <summary>
        /// Keeps an instance of a ChildWindow that will be shown when a button is clicked.
        /// </summary>
        private OpenBigMedia dcw;
        private string ext;



        #endregion Private fields

        #region Ctor

        public PhotoBrowserContol()
        {
            InitializeComponent();
            try
            {
                this.dialog = new SaveFileDialog();

                this.dialog.DefaultExt = ".txt";


            }
            catch (Exception ex)
            {
                Model.MainModel.Report("Ошибка в конфигурации SaveFileDialog: " + ex.Message);
            }




        }


        #endregion

        #region Public methods

        public void InitPage(string passportKey, bool forceInit, int typeMedia)
        {
            newMediaType = typeMedia;
            if (passportKey != _currentPassportKey || forceInit)
            {

                _currentPassportKey = passportKey;
                dcw = new OpenBigMedia();
                dcw.OnNextPressed += new EventHandler<BrowserItemEventArgs>(dcw_OnNextPressed);

                ServiceDataClient.GetThumbnailListCompleted += OnGetThumbnailListCompleted;
                //ServiceDataClient.GetThumbnailListAsync(passportKey, MediaType, GlobalContext.Context);
                ServiceDataClient.GetThumbnailListAsync(passportKey, newMediaType, GlobalContext.Context);
            }
            _popupWindowDeleteMedia = new MediaChildWindowDelete();
        }

        private void dcw_OnNextPressed(object sender, BrowserItemEventArgs e)
        {
            try
            {
            var bi = e.BrowserItem;
            
            if ((e.IsNext && bi.Number < bi.Count-1) || (!e.IsNext && bi.Number > 0))
            {
                int newIndex;
                if (e.IsNext)
                {
                    newIndex = bi.Number + 1;
                }
                else
                {
                    newIndex = bi.Number - 1;
                }

                BrowserItem newItem = browser.Items[newIndex] as BrowserItem;
                
                string _nameFoto = newItem.Name;
                if (!string.IsNullOrEmpty(_nameFoto))
                {
                    string[] ar = _nameFoto.Split('.');
                    if (ar.Length > 0)
                        ext = ar[ar.Length - 1];
                    if ((ext.ToLower() == "jpg") || (ext.ToLower() == "jpeg") || (ext.ToLower() == "png"))
                    {
                        string key = newItem.Key.ToString();// image.Tag.ToString();
                        _currentNumber = newItem.Number;
                        _count = newItem.Count;
                        ServiceDataClient.GetMediaFileCompleted += ServiceDataClient_GetMediaFileCompleted;
                        ServiceDataClient.GetMediaFileAsync(key, GlobalContext.Context);
                    }
                    else
                    {
                         if ((e.IsNext && newItem.Number < newItem.Count - 1) || (!e.IsNext && newItem.Number > 0))
                         {

                             dcw_OnNextPressed(this, new BrowserItemEventArgs() { BrowserItem = newItem, IsNext = e.IsNext });
                         }
                    }
                }
            }
            }
            catch (Exception ee)
            {
                Model.MainModel.Report("ошибка Media dcw_OnNextPressed = " +ee);
            }
        }

        #endregion

        #region Dependency Properties

        public int MediaType
        {
            get { return (int)GetValue(MediaTypeProperty); }
            set { SetValue(MediaTypeProperty, value); }
        }

        public static readonly DependencyProperty MediaTypeProperty = DependencyProperty.Register(
          "MediaType", typeof(int), typeof(PhotoBrowserContol), new PropertyMetadata(1));



        public int M
        {
            get { return (int)GetValue(MProperty); }
            set { SetValue(MProperty, value); }
        }

        public static readonly DependencyProperty MProperty = DependencyProperty.Register(
          "M", typeof(int), typeof(PhotoBrowserContol), new PropertyMetadata(1));

        #endregion Dependency Properties


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

        private IPassportDetailModel Model
        {
            get { return DataContext as IPassportDetailModel; }
        }

        #endregion Properties


        //private void Button_Click(object sender, RoutedEventArgs e)
        //{

        //    ServiceDataClient.GetThumbnailListCompleted += OnGetThumbnailListCompleted;
        //    ServiceDataClient.GetThumbnailListAsync("passportKey");

        //  //  Image img = Image..FromFile("Obliterate.jpg");

        //}


        private void OnGetThumbnailListCompleted(object sender, GetThumbnailListCompletedEventArgs e)
        {
            browser.Items.Clear();
            if (e.Result.IsValid)
            {
                if (e.Result.Thumbnails.Count > 0)
                {
                    int count = e.Result.Thumbnails.Count;
                    for (int i = 0; i < count; i++)
                    {
                        try
                        {
                            byte[] buffer = e.Result.Thumbnails[i].Data;
                            using (var mediaSmol = new MemoryStream(buffer))
                            {
                                var media = new BitmapImage();
                                media.SetSource(mediaSmol);
                                Thumbnail thumb = e.Result.Thumbnails[i];
                                string _keyName = thumb.Key + "&" + thumb.Name;
                                string nameTypeMedia = "";
                                switch (thumb.TypeMedia)
                                {
                                   
                                    case "1":
                                        nameTypeMedia = ProjectResources.photoTabItem;
                                        break;
                                    case "2":
                                        nameTypeMedia = ProjectResources.shemaTabItem;
                                        break;
                                    case "3":
                                        nameTypeMedia = ProjectResources.documentTabItem;
                                        break;
                                    case "4":
                                        nameTypeMedia = ProjectResources.photoOther;
                                        break;
                                  
                                }
                                var bi = new BrowserItem(thumb.Name, media, thumb.Comment, thumb.Key, _keyName,
                                    i, count, nameTypeMedia);
                                browser.Items.Add(bi);
                            }
                        }
                        catch (Exception ex)
                        {
                            Model.MainModel.Report(ex.Message);
                        }


                    }
                    browser.Visibility = Visibility.Visible;
                }
                else
                {
                    browser.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                Model.MainModel.Report(e.Result.ErrorMessage);
            }
        }

        private int _currentNumber = 0;
        private int _count = 0;

      

        void ServiceDataClient_GetMediaFileCompleted(object sender, GetMediaFileCompletedEventArgs e)
        {
            MemoryStream mediaBig = new MemoryStream(e.Result.ThumbnailBigMedia[0].Data);
            BitmapImage media = new BitmapImage();
            media.SetSource(mediaBig);
            

            ThumbnailBigMedia thumb = e.Result.ThumbnailBigMedia[0];
            BrowserItem bigMedia = new BrowserItem(thumb.Name, media, thumb.Comment, 111, "111", _currentNumber, _count, "Паспорт");
           
            dcw.Title = thumb.Name;
            dcw.DataContext = bigMedia;
            dcw.Show();
        }

        private void buDeleteFoto_Click(object sender, RoutedEventArgs e)
        {

            Button button = e.OriginalSource as Button;
            if (button != null)
            {
                //double keyMedia = (double)button.Tag;
                keyMedia = (button.Tag).ToString();

                _popupWindowDeleteMedia.Title = ProjectResources.GridControlMessageTitle;//"Подтверждение удаления";
                _popupWindowDeleteMedia.titleBox.Text = ProjectResources.MediaDel;//"Удалить медиаматериал?";
                _popupWindowDeleteMedia.Show();
                _popupWindowDeleteMedia.OKButtonDelete.Click += OKButtonDelete;

            }
        }

        void OKButtonDelete(object sender, RoutedEventArgs e)
        {
            _popupWindowDeleteMedia.OKButtonDelete.Click -= OKButtonDelete;

            if (_popupWindowDeleteMedia.DialogResult == true)
            {
                //передаем ключ парента
               // Model.DeleteMedia(_currentPassportKey, keyMedia, "sfgadfgafgadf");


            }
        }

        private void buSaveFoto_Click(object sender, RoutedEventArgs e)
        {

            Button button = e.OriginalSource as Button;
            if (button != null)
            {
                string _keyName = (button.Tag).ToString();
                //keyMedia = (button.Tag).ToString();
                string[] arr = _keyName.Split('&');
                keyMedia = arr[0];
                nameMedia = arr[1];
                this.dialog.DefaultExt = nameMedia;
                this.dialog.Filter = "JPG Files|*.jpg" + "|All Files|*.*";

                this.dialog.FilterIndex = 1;


                bool? saveClicked = this.dialog.ShowDialog();

                if (saveClicked == true)
                {
                    ServiceDataClient.GetMediaFileCompleted += GetMediaFileOnSaveCompleted;
                    ServiceDataClient.GetMediaFileAsync(keyMedia, GlobalContext.Context);
                }

            }

        }


        void GetMediaFileOnSaveCompleted(object sender, GetMediaFileCompletedEventArgs e)
        {
            ServiceDataClient.GetMediaFileCompleted -= GetMediaFileOnSaveCompleted;

            if (e.Result.IsValid)
            {

                using (Stream fs = (Stream)this.dialog.OpenFile())
                {
                    fs.Write(e.Result.ThumbnailBigMedia[0].Data, 0, e.Result.ThumbnailBigMedia[0].Data.Length);
                    fs.Close();

                    Model.MainModel.Report("Медиа Файл сохранен!!!!");
                }

            }
            else
            {
                Model.MainModel.Report("Получение медиа для сохранения ошибка: " + e.Result.ErrorMessage);
            }

        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // change on retry one big picture
            // ServiceDataClient.GetThumbnailListCompleted += OnGetBigPictureCompleted;
            // ServiceDataClient.GetThumbnailListAsync("passportKey");

            if (e.ClickCount == 2)
            {
                Image image = e.OriginalSource as Image;
                if (image != null)
                {
                    BrowserItem browserItem = image.DataContext as BrowserItem;

                    string _nameFoto = browserItem.Name;
                    if (!string.IsNullOrEmpty(_nameFoto))
                    {
                        string[] ar = _nameFoto.Split('.');
                        if (ar.Length > 0)
                            ext = ar[ar.Length - 1];
                        if ((ext.ToLower() == "jpg") || (ext.ToLower() == "jpeg") || (ext.ToLower() == "png"))
                        {
                            string key = image.Tag.ToString();
                           
                            _currentNumber = browserItem.Number;
                            _count = browserItem.Count;
                            ServiceDataClient.GetMediaFileCompleted += ServiceDataClient_GetMediaFileCompleted;
                            ServiceDataClient.GetMediaFileAsync(key, GlobalContext.Context);

                        }
                    }
                }
            }
            if (e.ClickCount == 1)
            {
                //передаем в passprtdetailModel данные для удаления медия при нажатиии в основной форме - удалить
                Image image = e.OriginalSource as Image;
                if (image != null)
                {
                    BrowserItem browserItem = image.DataContext as BrowserItem;

                    string _nameFoto = browserItem.Name;
                    keyMedia = (browserItem.Key).ToString();
                    Model.Variablesmedia(_currentPassportKey, keyMedia, _nameFoto);
                   // Model.DeleteMedia(_currentPassportKey, keyMedia, _nameFoto);
                }
                
            }


            
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            bool doubleClick;
            try
            {
               //// doubleClick = MouseDoubleClickHelper.IsDoubleClick(sender, e);
               // if (doubleClick)
               // {      // - Do some work. 
               // }
            }
            catch (Exception exception)
            {
            }
        }

    }




}