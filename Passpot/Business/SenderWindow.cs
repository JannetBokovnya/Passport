using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Passpot.Business
{
    public class SenderWindow
    {
        public EventParam EventParam { get; set; }
    public SenderWindow()
    {
        EventParam = new EventParam();
    }

    }
}
