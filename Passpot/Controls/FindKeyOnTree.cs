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

namespace Passpot.Controls
{
    public class FindKeyOnTree
    {
        protected string _keyPassportOn_Tree;
        public FindKeyOnTree(string keyPassportTree)
         {
             KeyPassportOn_Tree = keyPassportTree;
         }
        public string KeyPassportOn_Tree
        {
            get;
            private set;
        }
    }
}
