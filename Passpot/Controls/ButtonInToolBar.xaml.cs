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
using Passpot.Business;
using linkControl.Control;
using Services.ServiceReference;
using System.Windows.Media.Imaging;
using System.Windows.Browser;
using System.IO;
using Passpot.Model;

namespace Passpot.Controls
{
    public partial class ButtonInToolBar : UserControl
    {
        #region Private fields

        private ServiceDataClient _serviceDataClient;
        private FieldMetaDataItem _metaData;
        private PassportDetailModel _passportModel;
        private string _entityKey;
        private string _cEvent;
        private string _cAttribute;
        private string _cParams;
        private string _buttonType;

        #endregion Private fields


        public ButtonInToolBar()
        {
            InitializeComponent();
            buttonOnUrl.Click += buttonOnUrl_Click;
        }

        public static ButtonInToolBar CreateControl(string Content, string nameImage, string cEvent, string cAttribute, string cParams, string buttonType, PassportDetailModel passportModel)
        {
            var c = new ButtonInToolBar();
           
            c._cEvent =cEvent;
            c._cAttribute = cAttribute;
            c._cParams = cParams;
            c._buttonType = buttonType;
            c._passportModel = passportModel;

            //var panel = c.buttonOnUrl.Content as StackPanel;
            var image = c.buttonOnUrl.Content as Image;
            string _uri = "";
            _uri = "../Images/" + nameImage;
            Uri uri = new Uri(_uri, UriKind.Relative);
            image.Source = new BitmapImage(uri);

            ToolTipService.SetToolTip(c.buttonOnUrl, Content);

            return c;
        }

        private void buttonOnUrl_Click(object sender, RoutedEventArgs e)
        {
            switch (_buttonType)
            {
                case "1":

                    _passportModel.MainModel.Report("Вызов события по кнопке на toolBar, _cEvent = " + _cEvent + "  _cParams = " + " _cAttribute = " + _cAttribute);
                   // MainView.sendEvent(_cEvent, _cParams, _cAttribute);

                    HtmlPage.Window.Invoke("sendEvent", _cEvent, _cParams, _cAttribute);

                    break;
                case "2":
                    break;

                case "3":
                    {
                        string path = HtmlPage.Document.DocumentUri.AbsoluteUri;
                        int indexSlash = path.LastIndexOf('/');
                        path = path.Substring(0, indexSlash);
                        string uri = path + _cEvent;
                        HtmlPage.Window.Navigate(new Uri(uri), "_blank");
                        //HtmlPage.Window.Navigate(new Uri(uri));
                        //http://localhost:26313/BaseApp/Modules/Passport/ShablonExport/ExportFormUPZ.aspx?passportId=162089402"
                    }
                    break;
            }
            //HtmlPage.Window.Navigate(new Uri(_uriButton), "_blank");
        }
    }
}

//Image myImage3 = new Image();
//BitmapImage bi3 = new BitmapImage();
//bi3.UriSource = new Uri("../ToolBar/Images/followUp.png", UriKind.Relative);
//image.Source = bi3;
