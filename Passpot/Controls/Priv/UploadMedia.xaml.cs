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

namespace Passpot.Controls.Priv
{
    public partial class UploadMedia : ChildWindow
    {
        public UploadMedia()
        {
            InitializeComponent();
            
        }
        public static UploadMedia GetUpload()
        {
            var c = new UploadMedia();
            SilverlightControl1 priv1 = new SilverlightControl1();
            c.ttt.Children.Add(priv1);
            return c;
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

