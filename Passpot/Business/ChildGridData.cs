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
using Passpot.Model;
using Services.ServiceReference;

namespace Passpot.Business
{
    public class ChildGridData: IGridData
    {

        public ChildGridData(List<FieldMetaDataItem> metaDataList, GridData gridData)
        {
            MetaDataList = metaDataList;
            GridData = gridData;
        }

        #region IGridData Members

        public List<FieldMetaDataItem> MetaDataList
        {
            get; private set;
        }

        public GridData GridData
        {
            get ; private set;
        }

        #endregion
    }
}
