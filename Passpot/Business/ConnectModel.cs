using System.Collections.Generic;
using System.Collections.ObjectModel;
using Passpot.Model;
using Services.ServiceReference;


namespace Passpot.Business
{
    public class ConnectModel : ModelBase, IGridData
    {
        #region Ctor

        public ConnectModel(List<FieldMetaDataItem> metaDataList, GridData gridData)
        {
            MetaDataList = metaDataList;
            GridData = gridData;
        }

        #endregion

        #region IGridData Members

        public List<FieldMetaDataItem> MetaDataList
        {
            get; private set;

        }

        public GridData GridData
        {
            get; private set;
        }

        #endregion
    }
}



