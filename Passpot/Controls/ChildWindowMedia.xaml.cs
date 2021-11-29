using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using DC.FileUpload;
using Media.Interfaces;
using Passpot.Business;
using Passpot.Model;
using Telerik.Windows.Controls;

namespace Passpot
{
    public partial class ChildWindowMedia : ChildWindow
    {
        public delegate void callBackMediaPanelReDraw(string str, bool b);

        private string passportKey { get; set; }
        private string context { get; set; }
        private int mediaType { get; set; }
        private bool addnewmedia = false;

        private List<FileUploadControl.callBackMediaPanelReDraw> reDrawMedia;

        private PassportDetailModel Model { get; set; }
      


        public ChildWindowMedia()
        {
            InitializeComponent();
           // this.Loaded += new RoutedEventHandler(FileUpload_Loaded);
        }

        //http://msdn.microsoft.com/en-us/library/cc838156(v=vs.95).aspx
        //http://www.telerik.com/forums/background-image-in-drag-drop-area

        //param name="windowless" value="false" - ставим false, для окна с Drag and Drop телериковской компоненты (перетаскиваем медиаматериалы)

        public ChildWindowMedia CreateControl(string passportKey, string context, int mediaType, PassportDetailModel Model)
        {
            var control = new ChildWindowMedia();

            
            string path = HtmlPage.Document.DocumentUri.AbsoluteUri;
            int indexSlash = path.LastIndexOf('/');
            path = path.Substring(0, indexSlash);
            string uri = path + "/FileUploadTelerik.ashx";

            control.RadUpload1.UploadServiceUrl = uri;
            control.mediaType = mediaType;
            control.passportKey = passportKey;
            control.context = context;
            control.Model = Model;
            //control.reDrawMedia = inReDrawMedia;
            return control;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.SetFileCountState();
            this.SetBrowseFilter();
            this.SetNumericUpDnFormat();
        }

        //private void FileUpload_Loaded(object sender, RoutedEventArgs e)
        //{
        //    this.SetFileCountState();
        //    this.SetBrowseFilter();
        //    this.SetNumericUpDnFormat();
        //    string path = HtmlPage.Document.DocumentUri.AbsoluteUri;
        //    int indexSlash = path.LastIndexOf('/');
        //    path = path.Substring(0, indexSlash);
        //    string uri = path + "/FileUploadTelerik.ashx";
            
        //    RadUpload1.UploadServiceUrl = uri;
        //}

        private void SetNumericUpDnFormat()
        {
            System.Globalization.NumberFormatInfo info = new NumberFormatInfo();
            info.NumberDecimalDigits = 0;
         
        }

        private void SetBrowseFilter()
        {
           
        }

        private void SetFileCountState()
        {
            if (this.RadUpload1 != null)
            {
                bool multiple = this.RadUpload1.IsMultiselect == true;
               
            }
        }

        private void RadUpload1_FileUploadStarting(object sender, Telerik.Windows.Controls.FileUploadStartingEventArgs e)
        {
            // Pass a new parameter to the server handler

            // e.FileParameters.Add("MyParam1", paramValue);
            e.FileParameters.Add("KeyPassport", passportKey);
            e.FileParameters.Add("TypePassport", mediaType);
            e.FileParameters.Add("Comment", Comment.Text);
            e.FileParameters.Add("Filename", e.SelectedFile.Name);
            e.FileParameters.Add("Context", context);
            
        }

        private void RadUpload1_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            // Get the value of the returned Parameter from the server
            //ServerReturnedValue.Text = e.HandlerData.CustomData["MyServerParam1"].ToString();
         //   addnewmedia = true;
           // this.Close();
           

        }

        private void ButtonAllowMoreFiles_Checked(object sender, RoutedEventArgs e)
        {
            if (this.RadUpload1 != null)
            {
                CheckBox cb = sender as CheckBox;
                if (cb != null)
                {
                    this.RadUpload1.IsAppendFilesEnabled = cb.IsChecked == true;
                }
            }
            this.SetFileCountState();
        }

        private void ButtonAllowDrop_Checked(object sender, RoutedEventArgs e)
        {
            if (this.RadUpload1 != null)
            {
                CheckBox cb = sender as CheckBox;
                if (cb != null)
                {
                    this.RadUpload1.AllowDrop = cb.IsChecked == true;
                }
            }
        }

        private void RadUpload1_UploadStarted(object sender, UploadStartedEventArgs e)
        {
           // this.paramValue = MyParam.Text;
           
           
        }

        private void RadUploadDropPanel1_DragEnter(object sender, DragEventArgs e)
        {
            Color backgroundColor = new Color();
            backgroundColor.R = 208;
            backgroundColor.G = 232;
            backgroundColor.B = 254;
            this.RadUploadDropPanel1.Background = new SolidColorBrush(backgroundColor);
        }

        private void RadUploadDropPanel_DragLeave(object sender, DragEventArgs e)
        {
            this.RadUploadDropPanel1.Background = new SolidColorBrush(Colors.White);
        }

        private void ChildWindowMedia_OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (addnewmedia)
            {
               Model.FirePropertyChanged("AddNewMedia");    
            }
            
        }


        private void RadUpload1_OnUploadFinished(object sender, RoutedEventArgs e)
        {
           
            addnewmedia = true;
            this.Close();
        }
    }
}

