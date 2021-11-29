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
using Passpot.Model;
using Services.ServiceReference;


namespace Passpot.Controls
{
    public partial class HashControl : UserControl, IControlValueChanged
    {
        private string _oldValue;
        private FieldMetaDataItem _metaData;
        private ServiceDataClient _serviceDataClient;
        private PassportDetailModel _passportModel;
        private PasswordModel model ;
        private Dictionary<string, string> setValues;
       

        public HashControl()
        {
            InitializeComponent();
        }

        public PasswordModel Model
        {
            get { return DataContext as PasswordModel; }
        }

        protected ServiceDataClient ServiceDataClient
        {
            get
            {
                if (_serviceDataClient == null)
                    _serviceDataClient = new ServiceDataClient();
                return _serviceDataClient;
            }
        }

        public static HashControl CreateControl(FieldMetaDataItem metaData, PassportDetailModel passportModel, string value)
        {
            var c = new HashControl();

            c.setValues = new Dictionary<string, string>();
            c.setValues.Add(value, value);

            var model = new PasswordModel(metaData, c.setValues);

           

            c.DataContext = model;
            c.model = model;
            model.IsValidate = true;
            c._passportModel = passportModel;

            c._oldValue = value;
            c._metaData = metaData;

            c.titleBox.Text = metaData.TITUL;

            switch (metaData.BASIC_FLD)
            {
                case 1:
                    c.titleBox.FontWeight = FontWeights.Bold;
                    //c.titleBox2.FontWeight = FontWeights.Bold;

                    break;
                case 0:

                    break;
            }

            switch (metaData.MANDATORYVALUE_FLD)
            {
                case 1:
                    c.titleBox.Foreground = new SolidColorBrush(Color.FromArgb(255, Byte.Parse("220"), Byte.Parse("70"), Byte.Parse("74")));
                    //c.titleBox2.Foreground = new SolidColorBrush(Color.FromArgb(255, Byte.Parse("220"), Byte.Parse("70"), Byte.Parse("74")));
                    break;
                case 0:

                    break;
            }

            return c;
        }


        public string HasPassword="";
       
        private void Password()
        {
            ServiceDataClient.SetHashPasswordCompleted += ServiceDataClient_SetHashPasswordCompleted;
            ServiceDataClient.SetHashPasswordAsync(valueBox.Password);
            
        }

        void ServiceDataClient_SetHashPasswordCompleted(object sender, SetHashPasswordCompletedEventArgs e)
        {
             ServiceDataClient.SetHashPasswordCompleted -= ServiceDataClient_SetHashPasswordCompleted;
            if (e.Result.IsValid)
            {
                HasPassword = e.Result.CurrentUser_OnLog_result.HashPassword_OnLogBase;
                model.Hashp = HasPassword;
            }
            else
            {
                _passportModel.MainModel.Report("Ошибка! пароль не хеширован");
            }
        }


        public bool HasChanges
        {
            get
            {
                return valueBox.Password != _oldValue;
            }
        }

        public string NewValue
        {
            get { return valueBox.Password; }
        }

        public FieldMetaDataItem MetaData
        {
            get { return Model.MetaData; }
        }
       
    }
}
