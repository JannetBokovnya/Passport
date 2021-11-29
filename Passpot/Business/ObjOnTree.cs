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
using System.Collections.Generic;

namespace Passpot.Business
{
    public class ObjOnTree
    {
         public string Key { get; private set; }

        public string ParentKey { get; private set; }

        public string Name { get; set; }

        public List<ObjOnTree> Children { get; private set; }

        public ObjOnTree(string key, string parentKey, string name)
        {
            Key = key;
            ParentKey = parentKey;
            Name = name;
            Children = new List<ObjOnTree>();
        }
    }
}
