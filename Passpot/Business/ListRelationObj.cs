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
    public class ListRelationObj
    {
        public string FldName { get; set; }
        public string NameObj { get; set; }

        public ListRelationObj(string fldName, string nameObj)
        {
            FldName = fldName;
            NameObj = nameObj;
        }
    }
}
