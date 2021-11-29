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
    public class PassportOpenParam
    {
        public string EntityKey { get; set; }
        public string ParentKey { get; set; }
        public bool IsShowAllField { get; set; }
        public bool IsShowTreeOnFindkey { get; set; }

    }
}
