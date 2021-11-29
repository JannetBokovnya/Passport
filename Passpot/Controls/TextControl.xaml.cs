using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Passpot.Business;
using System.Windows;
using Services.ServiceReference;


namespace Passpot.Controls
{
    public partial class TextControl : UserControl, IControlValueChanged
    {
        public TextControl()
        {
            InitializeComponent();
        }

        public TextModel Model
        {
            get { return DataContext as TextModel; }
        }

        private string keyPressed = "";
        private string h = "";
        
        public static TextControl CreateControl(FieldMetaDataItem metaData, ControlAttributeItem attrOneContr, string value)
        {
            var c = new TextControl();
            var model = new TextModel( metaData, attrOneContr,  value);
            c.DataContext = model;
            model.IsValidate = true;

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
                    c.titleBox.Foreground = c.titleBox.Foreground = new SolidColorBrush(Color.FromArgb(255, Byte.Parse("220"), Byte.Parse("70"), Byte.Parse("74")));

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
            get { return (valueBox.Text).Trim() != Model.OldValue; }
        }

        public string NewValue
        {
            get { return (valueBox.Text).Trim(); }
        }


        public FieldMetaDataItem MetaData
        {
            get { return Model.MetaData; }
        }

        #endregion

        private void titleBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            string hh = "";
            if ((keyPressed + e.Key.ToString()) == "CtrlC")
            {
                string s = string.Empty;

                s = titleBox.Text;
                System.Windows.Clipboard.SetText(s);
                keyPressed = "";

            }
        }

        private void titleBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string ff = "";
            ff = e.OriginalSource.ToString();
            h = ((TextBlock) (((System.Windows.RoutedEventArgs) (e)).OriginalSource)).Text;
            
        }
    }
}
