using System;
using System.Collections.Generic;
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
    public class SavePriv
    {
        public class SAVEPRIVITEMS
       {
        public string CNAME { get; set; }
        public string NKMORT1 { get; set; }
        public string NDISTANCEBEG { get; set; }
        public string NX1 { get; set; }
        public string NY1 { get; set; }
        public string NZ1 { get; set; }
        public string NKEY { get; set; }
        public string NH1 { get; set; }
        public string NKMTRUE1 { get; set; }
        public string NX2 { get; set; }
        public string NY2 { get; set; }
        public string NZ2 { get; set; }
        public string NH2 { get; set; }
        public string NKMORT2 { get; set; }
        public string NKMTRUE2 { get; set; }
        public string NDISTANCEEND { get; set; }
        public string NPIPEKEY { get; set; }
        public string NMTKEY { get; set; }
        public string NBUILDTYPE { get; set; }
        public string ISEDITED { get; set; }


       }

        public class LISTSAVEPRIV
       {
           public List<SAVEPRIVITEMS> LISTPRIV { get; set; }
       }

        
    }
}
