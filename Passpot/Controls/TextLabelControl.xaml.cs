using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using Services.ServiceReference;

namespace Passpot.Controls
{
    public partial class TextLabelControl : UserControl
    {
        public TextLabelControl()
        {
            InitializeComponent();
        }
        public static TextLabelControl CreateControl(FieldMetaDataItem metaData,  string value)
        {
            var c = new TextLabelControl();
            c.titleBox.Text = metaData.TITUL;
            c.valueBox.Text = value;


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

            //if (metaData.IS_EDITED == 0)
            //{
            //    c.IsEnabled = false;
            //}
            //else
            //{
            //    c.IsEnabled = true;
            //}

            return c;
        }
    }
}
