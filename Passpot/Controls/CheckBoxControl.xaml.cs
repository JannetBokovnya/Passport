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
    /// <summary>
    /// 
    /// </summary>
    public partial class CheckBoxControl : UserControl, IControlValueChanged
    {
        private string _oldValue;
        private FieldMetaDataItem _metaData;
        private string value_c;

        public CheckBoxControl()
        {
            InitializeComponent();
        }
        public static CheckBoxControl CreateControl(FieldMetaDataItem metaData, string value)
        {
            var c = new CheckBoxControl();

            c._oldValue = value;
            c._metaData = metaData;
            c.value_c = value;

            c.titleBox.Text = metaData.TITUL;

            switch (metaData.BASIC_FLD)
            {
                case 1:
                    c.titleBox.FontWeight = FontWeights.Bold;
                    break;
                case 0:

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




            //если на вход приходит значение 1 - то ставим галочку(1 - ВСЕГДА ДА!)
            if (value == "True")
            {
                c.checkBox.IsChecked = true;
            }
            else
                c.checkBox.IsChecked = false;
            
            return c;
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            if (checkBox.IsChecked == true)
            {
                value_c = "1";
            }

            if (checkBox.IsChecked == false)
            {
                value_c = "0";
            }


        }
        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (checkBox.IsChecked == true)
            {
                value_c = "1";
            }

            if (checkBox.IsChecked == false)
            {
                value_c = "0";
            }
        }

        #region IControlValueChanged Members

        public bool HasChanges
        {
            get { return value_c != _oldValue; }
        }

        public string NewValue
        {
            get { return value_c; }
        }


        #endregion


        #region IControlValueChanged Members


        FieldMetaDataItem IControlValueChanged.MetaData
        {
            get { return _metaData; }
        }

        #endregion

        
    }
}
