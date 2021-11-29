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

namespace Passpot.Controls
{
    public partial class TestControl : UserControl
    {
        public TestControl()
        {
            InitializeComponent();
        }

        public static TextControl CreateControl(string valueText, string valueValue)
        {
            var c = new TextControl();
            c.titleBox.Text = valueText;
            c.valueBox.Text = valueValue;

            return c;
        }
    }
}
