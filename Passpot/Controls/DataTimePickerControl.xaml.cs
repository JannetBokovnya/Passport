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
using Services.ServiceReference;

namespace Passpot.Controls
{
    public partial class DataTimePickerControl : UserControl, IControlValueChanged
    {
        private string _oldValue;
        private FieldMetaDataItem _metaData;
        
        public DataTimePickerControl()
        {
            InitializeComponent();
        }

        public static DataTimePickerControl CreateControl(FieldMetaDataItem metaData, string value)
        {
            var c = new DataTimePickerControl();
            c._oldValue = value;
            c._metaData = metaData;

            c.titleBox.Text = metaData.TITUL;
            if (value != "")
            {

                c.StartDatePicker.SelectedValue = DateTime.Parse(value, c.StartDatePicker.Culture);


            }

            switch (metaData.BASIC_FLD)
            {
                case 1:
                    c.titleBox.FontWeight = FontWeights.Bold;
                    break;
                default:
                    break;
            }




            switch (metaData.MANDATORYVALUE_FLD)
            {
                case 1:
                    c.titleBox.Foreground = new SolidColorBrush(Color.FromArgb(255, Byte.Parse("220"), Byte.Parse("70"), Byte.Parse("74")));

                    break;
                case 0:

                    break;
            }

            if (metaData.IS_EDITED == 0)
            {
                c.IsEnabled = false;
            }
            else
            {
                c.IsEnabled = true;
            }


            return c;
        }



        #region IControlValueChanged Members

        public bool HasChanges
        {

            get
            {
                string cc;

                if (StartDatePicker.SelectedValue != null)
                {
                    DateTime d1 = (DateTime)StartDatePicker.SelectedValue;

                    cc = d1.ToString();
                }
                else
                    cc = "";

                /////////////////////////

                if (cc != _oldValue)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public string NewValue
        {
            get
            {
                if (StartDatePicker.SelectedValue != null)
                {
                    DateTime d1 = (DateTime)StartDatePicker.SelectedValue;

                    //return d1.ToString();
                    return ConvertToUnixTime(d1);
                }
                else
                    return "";

            }
        }


        public FieldMetaDataItem MetaData
        {
            get { return _metaData; }
        }

        #endregion

        private static string ConvertToUnixTime(DateTime datatimevalue)
        {
            string result = "";
            result = (datatimevalue - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds.ToString();
            return result;

        }
    }
}
