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
    public class EventParam
    {
        public string ENTITYKEY { get; set; }
        public string EntityName { get; set; }
        public string ParentKey { get; set; }
        public string PassportKey { get; set; }
        public string PassportName { get; set; }
        public string NOBJKEY { get; set; }

    }
}
