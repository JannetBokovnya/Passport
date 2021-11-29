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
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Browser;
using Media.Interfaces;
using Passpot.Controls;
using Media.Resources;
using Services.ServiceReference;


namespace DC.FileUpload
{
    public partial class FileUploadControl : UserControl
    {
        public int MaxConcurrentUploads { get; set; }
        public long UploadChunkSize { get; set; }
        public bool ResizeImage { get; set; }
        public int ImageSize { get; set; }
        private DateTime start;
        private ServiceDataClient _serviceDataClient;
        private ChildWindowAttantion _popupWindowAttantion;

        protected ServiceDataClient ServiceDataClient
        {
            get
            {
                if (_serviceDataClient == null)
                    _serviceDataClient = new ServiceDataClient();
                return _serviceDataClient;
            }
        }

        public Brush BackgroundColor
        {
            get { return controlBorder.Background; }
            set { controlBorder.Background = value; }
        }
        public CornerRadius CornerRadius
        {
            get { return controlBorder.CornerRadius; }
            set { controlBorder.CornerRadius = value; }
        }
        public Brush BorderBrushColor
        {
            get { return controlBorder.BorderBrush; }
            set { controlBorder.BorderBrush = value; }
        }
        public Thickness BorderThickness
        {
            get { return controlBorder.BorderThickness; }
            set { controlBorder.BorderThickness = value; }
        }

        public string Filter { get; set; }
        public bool Multiselect { get; set; }
        public Uri UploadUrl { get; set; }
        public string passportKey { get; set; }
        public string context { get; set; }
        public string JavascriptCompleteFunction { get; set; }

        private bool uploading { get; set; }

        private long TotalUploadSize { get; set; }
        private long TotalUploaded { get; set; }

        public long MaximumTotalUpload { get; set; }
        public long MaximumUpload { get; set; }

        public int MaxNumberToUpload { get; set; }
        private int count = 0;

        public bool AllowThumbnail 
        {
            get { return displayThumbailChckBox.Visibility == Visibility.Visible; }
            set 
            {
                bool temp = value;
                if (temp) 
                    displayThumbailChckBox.Visibility = Visibility.Visible; 
                else 
                    displayThumbailChckBox.Visibility = Visibility.Collapsed; 
            }
        }

        private ScrollHelper helper;

        private ObservableCollection<FileUpload> files;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="b"></param>
        public delegate void callBackMediaPanelReDraw(string str, bool b);

        private List<callBackMediaPanelReDraw> reDrawMedia;// = null;
        


        public FileUploadControl()
        {
            InitializeComponent();
            

            files = new ObservableCollection<FileUpload>();
            files.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(files_CollectionChanged);
            MaxConcurrentUploads = 1;
            MaxNumberToUpload = -1;
            MaximumTotalUpload = MaximumUpload = -1;
            Filter = "All Files|*.*";
            Multiselect = true;
            uploading = false;
            UploadChunkSize = 4194304;

            addFilesButton.Click += new RoutedEventHandler(addFilesButton_Click);
            uploadButton.Click += new RoutedEventHandler(uploadButton_Click);
            clearFilesButton.Click += new RoutedEventHandler(clearFilesButton_Click);

            displayThumbailChckBox.Checked += new RoutedEventHandler(displayThumbailChckBox_Checked);
            displayThumbailChckBox.Unchecked += new RoutedEventHandler(displayThumbailChckBox_Checked);

            helper = new ScrollHelper(filesScrollViewer);

            Loaded += new RoutedEventHandler(FileUploadControl_Loaded);
        }

        void displayThumbailChckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chkbox = sender as CheckBox;

            foreach (FileUpload fu in files)
            {
                if (fu.DisplayThumbnail != chkbox.IsChecked)
                    fu.DisplayThumbnail = (bool)chkbox.IsChecked;
            }
        }
        public FileUploadControl(Uri uploadUrl)
            : this()
        {
            UploadUrl = uploadUrl;
        }

        void files_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            countTextBlock.Text = ProjectResources.CountFunctionGrid + "  " + files.Count.ToString();
            TotalUploadSize = files.Sum(f => f.FileLength);
            TotalUploaded = files.Sum(f => f.BytesUploaded);
            totalSizeTextBlock.Text = string.Format("{0} of {1}",
                new ByteConverter().Convert(TotalUploaded, this.GetType(), null, null).ToString(),
                new ByteConverter().Convert(TotalUploadSize, this.GetType(), null, null).ToString());
            progressBar.Maximum = TotalUploadSize;
            progressBar.Value = TotalUploaded;
        }

        void clearFilesButton_Click(object sender, RoutedEventArgs e)
        {
            var q = files.Where(f => f.Status == FileUploadStatus.Uploading);
            foreach (FileUpload fu in q)
                fu.CancelUpload();
            timeLeftTextBlock.Text = "";
            files.Clear();
        }

        void uploadButton_Click(object sender, RoutedEventArgs e)
        {
            if ((string)uploadButton.Content == ProjectResources.FileUploadLoadButton)
            {
                uploadButton.Content = ProjectResources.cancelButtonPasport; //Отменить
                start = DateTime.Now;
                //проверяем на дублирование названий файла
                if (files.Count > 0)
                {
                    ServiceDataClient.GetMediaAttributeCompleted += ServiceDataClient_GetMediaAttributeCompleted;
                    ServiceDataClient.GetMediaAttributeAsync(this.passportKey, GlobalContext.Context);
                }

                //UploadFiles();
            }
            else
            {
                var q = files.Where(f => f.Status == FileUploadStatus.Uploading);
                foreach (FileUpload fu in q)
                    fu.CancelUpload();
                uploading = false;
                uploadButton.Content = ProjectResources.FileUploadLoadButton;
                //"Загрузить";
            }
        }

        void ServiceDataClient_GetMediaAttributeCompleted(object sender, GetMediaAttributeCompletedEventArgs e)
        {
            int k = 0;
            if (e.Result.IsValid)
            {
                DataMediaAttribute _mAtr;
                for (int i =  0; i < e.Result.DataMediaAttributeList.Count; i++)
                {
                    _mAtr = e.Result.DataMediaAttributeList[i];

                    var q = files.Where(f => f.Status != FileUploadStatus.Complete && f.Status != FileUploadStatus.Uploading && f.Status != FileUploadStatus.Resizing);

                    foreach (FileUpload fu in q)
                    {

                        //FileUpload fl=files.First();// = files.First(f => f.Status != FileUploadStatus.Complete && f.Status != FileUploadStatus.Uploading && f.Status != FileUploadStatus.Resizing);
                        if (fu.File.Name == _mAtr.name_file)
                        {

                            if (_popupWindowAttantion == null)
                            {
                                _popupWindowAttantion = new ChildWindowAttantion();
                                _popupWindowAttantion.titleBox.Text =
                                    ProjectResources.FileUploadMessage;
                                   // "одинаковое название файлов"; FileUploadMessage
                                _popupWindowAttantion.Show();
                                k = k + 1;
                                break;
                            }
                            else
                            {
                                _popupWindowAttantion.titleBox.Text =
                                    ProjectResources.FileUploadMessage;
                                   // "одинаковое название файлов";
                                _popupWindowAttantion.Show();
                                k = k + 1;
                                break;
                            }

                        }
                    }
                }
                
            }
            if (k == 0)
            {
                UploadFiles();     
            }
            
        }

        void addFilesButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = Filter;
            dlg.Multiselect = Multiselect;

            if ((bool)dlg.ShowDialog())
            {
                foreach (FileInfo file in dlg.Files)
                {
                    FileUpload upload = new FileUpload(this.Dispatcher, UploadUrl, file);
                    if (UploadChunkSize > 0)
                        upload.ChunkSize = UploadChunkSize;
                    if (ResizeImage)
                    {
                        upload.ResizeImage = ResizeImage;
                        upload.ImageSize = ImageSize;
                    }

                    if (MaxNumberToUpload > -1)
                    {
                        count++;
                        if (count > MaxNumberToUpload)
                        {
                            MessageBox.Show("You have exceeded the total allowable number of files to upload.");
                            break;
                        }
                    }

                    if (MaximumTotalUpload >= 0 && TotalUploadSize + upload.FileLength > MaximumTotalUpload)
                    {
                        MessageBox.Show("You have exceeded the total allowable upload amount.");
                        break;
                    }
                    if (MaximumUpload >= 0 && upload.FileLength > MaximumUpload)
                    {
                        MessageBox.Show(string.Format("The file '{0}' exceeds the maximun upload size.", upload.Name));
                        continue;
                    }
                    upload.DisplayThumbnail = (bool)displayThumbailChckBox.IsChecked;
                    upload.StatusChanged += new EventHandler(upload_StatusChanged);
                    upload.UploadProgressChanged += new ProgressChangedEvent(upload_UploadProgressChanged);
                    upload.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(upload_PropertyChanged);
                    files.Add(upload);
                }
            }
        }

        void upload_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {            
            if (e.PropertyName == "FileLength")
            {
                files_CollectionChanged(null, null);
            }
        }

        void upload_UploadProgressChanged(object sender, UploadProgressChangedEventArgs args)
        {
            

            TotalUploaded += args.BytesUploaded;
            progressBar.Value = TotalUploaded;
            totalSizeTextBlock.Text = string.Format("{0} of {1}",
                 new ByteConverter().Convert(TotalUploaded, this.GetType(), null, null).ToString(),
                new ByteConverter().Convert(TotalUploadSize, this.GetType(), null, null).ToString());

            double ByteProcessTime = 0;
            double EstimatedTime = 0;

            try
            {
                TimeSpan timeSpan = DateTime.Now - start;
                ByteProcessTime = TotalUploaded / timeSpan.TotalSeconds;
                EstimatedTime = (TotalUploadSize - TotalUploaded) / ByteProcessTime;
                timeSpan = TimeSpan.FromSeconds(EstimatedTime);
                timeLeftTextBlock.Text = string.Format("{0:00}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            }
            catch { }
        }

        void upload_StatusChanged(object sender, EventArgs e)
        {
            FileUpload fu = sender as FileUpload;
            if (fu.Status == FileUploadStatus.Complete)
            {
                if (uploading)
                    UploadFiles();
            }
            else if (fu.Status == FileUploadStatus.Removed)
            {
                files.Remove(fu);
                if (uploading)
                    UploadFiles();
            }
            else if (fu.Status == FileUploadStatus.Uploading && files.Count(f => f.Status == FileUploadStatus.Uploading) < MaxConcurrentUploads)
                UploadFiles();
        }

        void FileUploadControl_Loaded(object sender, RoutedEventArgs e)
        {
            fileList.ItemsSource = files;
        }

        private void UploadFiles()
        {
            ComboBoxItem item = ddlFileType.SelectedItem as ComboBoxItem;
            int pageNum=1;
            if (item!=null)
                pageNum=Convert.ToInt32(item.Tag.ToString());

            uploading = true;
            while (files.Count(f => f.Status == FileUploadStatus.Uploading || f.Status == FileUploadStatus.Resizing) < MaxConcurrentUploads && uploading)
            {
                if (files.Count(f => f.Status != FileUploadStatus.Complete && f.Status != FileUploadStatus.Uploading && f.Status != FileUploadStatus.Resizing) > 0)
                {
                    FileUpload fu = files.First(f => f.Status != FileUploadStatus.Complete && f.Status != FileUploadStatus.Uploading && f.Status != FileUploadStatus.Resizing);
                    fu.Description = txtComment.Text;
                    fu.passportKey =   this.passportKey;
                    fu.contextPasport = GlobalContext.Context;
                    fu.FileType = ((ComboBoxItem)ddlFileType.SelectedItem).Tag.ToString();
                    fu.Upload(reDrawMedia[pageNum-1]);
                    //fu.Upload();
                }
                else if (files.Count(f => f.Status == FileUploadStatus.Uploading || f.Status == FileUploadStatus.Resizing) == 0)
                {
                    uploading = false;
                    uploadButton.Content = ProjectResources.FileUploadLoadButton; //"Загрузить";
                    if (!string.IsNullOrEmpty(JavascriptCompleteFunction))
                    {
                        try
                        {
                            HtmlPage.Window.CreateInstance(JavascriptCompleteFunction);
                        }
                        catch { }
                    }
                }
                else
                {
                    break;
                }
            }
        }

        public FileUploadControl CreateControl(Uri uploadUrl, long uploadChunkSize, string passportKey, List<callBackMediaPanelReDraw> inReDrawMedia, string context)
        {
            var control = new FileUploadControl();
            control.UploadUrl = uploadUrl;
            control.UploadChunkSize = uploadChunkSize;
            control.passportKey = passportKey;
            control.context = context;
            control.reDrawMedia = inReDrawMedia;
            return control;
        }

        private void progressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //if (progressBar.Value.ToString() == "100")
            //{
            //    reDrawMedia(passportKey, true);
            //}
        }





        
    }
}
