using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Passpot.Controls
{
    //класс - расширитель для TextBox - возможноть писать подсказку в TextBox (типа watermark) - ставим вместо TextBox
    public class WatermarkTextBox : TextBox
    {
        public bool displayWatermark = true;
        private bool hasFocus = false;
        public WatermarkTextBox()
        {
            this.GotFocus += new RoutedEventHandler(WatermarkTextBox_GotFocus);
            this.LostFocus += new RoutedEventHandler(WatermarkTextBox_LostFocus);
            this.TextChanged += new TextChangedEventHandler(WatermarkTextBox_TextChanged);
            this.Unloaded += new RoutedEventHandler(WatermarkTextBox_Unloaded);
            
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion



        private void WatermarkTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!hasFocus && Text == "")
            {
                setMode(true);
                displayWatermark = true;
                this.Text = Watermark;
            }
        }

        private void WatermarkTextBox_Unloaded(object sender, RoutedEventArgs e)
        {
            this.GotFocus -= WatermarkTextBox_GotFocus;
            this.LostFocus -= WatermarkTextBox_LostFocus;
            this.Unloaded -= WatermarkTextBox_Unloaded;
            this.TextChanged -= WatermarkTextBox_TextChanged;
        }

        private void WatermarkTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            hasFocus = true;
            if (displayWatermark)
            {
                setMode(false);
                this.Text = "";
            }
        }
        private void WatermarkTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            hasFocus = false;
            if (this.Text == "")
            {
                displayWatermark = true;
                setMode(true);
                this.Text = Watermark;
            }
            else
            {
                displayWatermark = false;
            }
        }
        public void setMode(bool watermarkStyle)
        {
            if (watermarkStyle)
            {
                this.FontStyle = FontStyles.Italic;
                this.Foreground = new SolidColorBrush(Colors.Gray);
            }
            else
            {
                this.FontStyle = FontStyles.Normal;
                this.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
        public new string Watermark
        {
            get { return GetValue(WatermarkProperty) as string; }
            set { SetValue(WatermarkProperty, value); }
        }
        public static new readonly DependencyProperty WatermarkProperty =
            DependencyProperty.Register("Watermark", typeof(string), typeof(WatermarkTextBox), new PropertyMetadata(watermarkPropertyChanged));
        private static void watermarkPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            WatermarkTextBox textBox = obj as WatermarkTextBox;
            if (textBox.displayWatermark)
            {
                textBox.Text = e.NewValue.ToString();
                textBox.setMode(true);
            }
        }
    }
}
