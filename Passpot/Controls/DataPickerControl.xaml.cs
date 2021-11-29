using System;
using System.Collections.Generic;
using System.Globalization;
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
    public partial class DataPickerControl : UserControl, IControlValueChanged
    {
        private string _oldValue;
        private FieldMetaDataItem _metaData;

        public DataPickerControl()
        {
            InitializeComponent();
        }
        public static DataPickerControl CreateControl(FieldMetaDataItem metaData, string value)
        {
            var c = new DataPickerControl();
            //double timestamp;
            //double.TryParse(value, out timestamp);
            //string dateStr = ConvertToDateTime(timestamp);


           // c._oldValue = dateStr; // value;
            c._oldValue = value; // value;
            c._metaData = metaData;

            c.titleBox.Text = metaData.TITUL;
            if (value != "")
               
            {

                //value = "8/31/1965";
               
                //CultureInfo ci = new CultureInfo("ru-Ru");
                //DateTime ddd = DateTime.ParseExact(value, "dd.MM.yyyy", ci , System.Globalization.DateTimeStyles.None);
               // c.StartDatePicker.SelectedDate = DateTime.ParseExact(value, "dd.MM.yyyy", null);


               // c.StartDatePicker.SelectedDate = DateTime.Parse(value);
               // c.StartDatePicker.SelectedDate = DateTime.Parse(dateStr);


                c.StartDatePicker.SelectedDate = DateTime.Parse(value, CultureInfo.CurrentCulture);




                //DateTime.Parse
                //c.StartDatePicker.SelectedDate = 
                //DateTime.Parse(value);



                /////////////////
                //DateTime date;
                //date = DateTime.Parse(value);
                //String dateStr = date.ToString("dd.M.yyyy");
                //c.StartDatePicker.SelectedDate = DateTime.Parse(dateStr);
                /////////////////
                //                System.Globalization.CultureInfo vNewCulture = (CultureInfo) System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
                //vNewCulture.DateTimeFormat.ShortDatePattern = “dd-MMM-yyyy”;
                //vNewCulture.DateTimeFormat.DateSeparator = “-”;
                //vNewCulture.DateTimeFormat.ShortTimePattern = “hh:mm”;  // add “tt” if you need AM/PM descriptors
                //vNewCulture.DateTimeFormat.LongTimePattern = “hh:mm:ss”
                //System.Threading.Thread.CurrentThread.CurrentCulture = vNewCulture;





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

        private static string ConvertToDateTime(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp).ToString("dd.MM.yyyy");

        }

        #region IControlValueChanged Members

        public bool HasChanges
        {

            get
            {
                string cc;

                //int thisMonthIndex = DateTime.Today.Month;
                //DateTime dayOfMonth = new DateTime(DateTime.Today.Year, thisMonthIndex, 1);

                //dayOfMonth = StartDatePicker.SelectedDate();
                //dt = StartDatePicker.SelectedDate.Value.DayOfYear;
                //        ///////////////////////////
                if (StartDatePicker.SelectedDate != null)
                {
                    DateTime d1 = (DateTime)StartDatePicker.SelectedDate;
                    cc = d1.ToString("dd.MM.yyyy");
                }
                else
                    cc = "";


                


                /////////////////////////

                //cc = StartDatePicker.SelectedDate.ToString();

                if (cc != _oldValue)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                //return StartDatePicker.SelectedDate != DateTime.Parse(_oldValue); }
            }
        }

        public string NewValue
        {
            get
            {
                if (StartDatePicker.SelectedDate != null)
                {
                    DateTime d1 = (DateTime)StartDatePicker.SelectedDate;
                   // return d1.ToString("dd.MM.yyyy");
                    return ConvertToUnixTime(d1);




                }
                else
                    return "";

                //return StartDatePicker.SelectedDate.ToString();
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
