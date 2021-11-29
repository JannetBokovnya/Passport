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
    public class Navigation
    {
        public string TypePassport { get; set; }
        public string PassportKey { get; set; }
        public int IsEditedPassport { get; set; }
        public string EntityKey { get; set; }
        public string ParentKey { get; set; }
        public bool IsShowAllField { get; set; }
        public bool IsShowTreeOnFindkey { get; set; }
        public  string Link { get; set; }
        public string Path { get; set; }
        public string Text { get; set; }
        public string KeyNode { get; set; }

        public Navigation(string typePassport, string passportKey, int isEditedPassport, string entityKey,
                          string parentKey, bool isShowAllField, bool isShowTreeOnFindkey, string link, string path, string text, string keyNode)
        {
            TypePassport = typePassport;
            PassportKey = passportKey;
            IsEditedPassport = isEditedPassport;
            EntityKey = entityKey;
            ParentKey = parentKey;
            IsShowAllField = isShowAllField;
            IsShowTreeOnFindkey = isShowTreeOnFindkey;
            Link = link;
            Path = path;
            Text = text;
            KeyNode = keyNode;

        }
        
        



    }
}
