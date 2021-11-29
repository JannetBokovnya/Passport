using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class LinkControlInVisible :  UserControl, IControlValueChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private string parentKey = "";
        private PassportDetailModel _passportModel;
        private FieldMetaDataItem _metaData;

        public LinkControlInVisible()
        {
            InitializeComponent();
        }

        public void FirePropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }

        private PassportDetailModel Model
        {
            get { return DataContext as PassportDetailModel; }
        }


        public static LinkControlInVisible CreateControl(FieldMetaDataItem metaData,
                                                         PassportDetailModel passportModel )
        {
            var c = new LinkControlInVisible();
            c._metaData = metaData;
            c._passportModel = passportModel;
           // c.gridlink.RowDefinitions[1].Height = new GridLength(0);
            c.titleBox.Text = metaData.TITUL;
            c.valueBox.Text = passportModel.ParentNameKeyOnAdmin;
            c.gridlink.RowDefinitions[0].Height = new GridLength(0);
            return c;
        }

        #region IControlValueChanged Members

        public bool HasChanges
        {
            
            get { return true; }
        }

        public string NewValue
        {
            get { return (valueBox.Text).Trim(); }
        }


        public FieldMetaDataItem MetaData
        {
            get { return _metaData; }
        }

        #endregion
    }
}
