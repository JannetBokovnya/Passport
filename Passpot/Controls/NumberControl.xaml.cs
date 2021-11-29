//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Animation;
//using System.Windows.Shapes;

//namespace Passpot.Controls
//{
//    public partial class NumberControl : UserControl
//    {
//        public NumberControl()
//        {
//            InitializeComponent();
//        }
//    }
//}

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Passpot.Business;
using Services.ServiceReference;


namespace Passpot.Controls
{
    public partial class NumberControl : UserControl, IControlValueChanged
    {
        public NumberControl()
        {
            InitializeComponent();
        }

        public NumberModel Model
        {
            get { return DataContext as NumberModel; }
        }

        public static NumberControl CreateControl(FieldMetaDataItem metaData, ControlAttributeItem attrOneContr, string value)
        {
            
            var c = new NumberControl();
            var model = new NumberModel(metaData, attrOneContr, value);
            c.DataContext = model;
            model.IsValidate = true;

           c.Model.PropertyChanged -= c.Model_PropertyChanged;
           c.Model.PropertyChanged += c.Model_PropertyChanged;

            //c.Height = attrOneContr.HEIGHT;
            //c.valueBox.Width = attrOneContr.WIDTH;
            //c.valueBox.FontSize = attrOneContr.FONTSIZE;
            // c.valueBox.BorderBrush = attrOneContr.COLORFON;
            //((Brush)attrOneContr).COLORFON;
            //valueNumberBox
            //titleNumberBox

            switch (metaData.BASIC_FLD)
            {
                case 1:
                   // c.titleNumberBox.FontSize = 14;
                    c.titleNumberBox.FontWeight = FontWeights.Bold;
                    c.titleNumberBox.Foreground = new SolidColorBrush(Colors.Black);

                    break;
                case 0:
                 
                    break;
            }


            switch (metaData.MANDATORYVALUE_FLD)
            {
                case 1:
                    c.titleNumberBox.Foreground = new SolidColorBrush(Color.FromArgb(255, Byte.Parse("220"), Byte.Parse("70"), Byte.Parse("74")));
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

       private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
       {
            if (e.PropertyName.Equals("Value"))
            {
                valueNumberBox.Text = Model.Value;
            }
       }


        private void TextBox_BindingValidationError(object sender, ValidationErrorEventArgs e)
        {
            valueNumberBox.Focus();
        }




        #region IControlValueChanged Members

        public bool HasChanges
        {
            get { return valueNumberBox.Text != Model.OldValue; }
        }

        public string NewValue
        {
           
            get
            {
                
               //договорились что передаем в оракл разделительный знак "."
                string checkValue="";
                checkValue = valueNumberBox.Text.Replace(",", ".");
                return checkValue;
            }
        }


        #endregion

        #region IControlValueChanged Members


        FieldMetaDataItem IControlValueChanged.MetaData
        {
            get 
            { 
                return Model.MetaData; 
            }
        }

        #endregion

        
    }
}
