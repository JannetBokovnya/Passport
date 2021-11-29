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
using Passpot.Business;
using Passpot.Model;
using Services.ServiceReference;

namespace Passpot.Controls
{
    public partial class DataListControl : UserControl
    {
        private FieldMetaDataItem _metaData;
        
        public DataListControl()
        {
            InitializeComponent();
        }
        public static DataListControl CreateControl(FieldMetaDataItem metaData, ControlAttributeItem attrOneContr, PassportDetailModel passportModel, string nameC)
        {
            var c = new DataListControl();
            if (metaData.IS_EDITED == 0)
            {
                c.IsEnabled = false;
            }
            else
            {
                c.IsEnabled = true;
            }
            return c;
        }


        public FieldMetaDataItem MetaData
        {
            get { return _metaData; }
        }
    }
}
