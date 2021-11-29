using System.Collections.Generic;
using System.Collections.ObjectModel;
using Passpot.Model;
using Services.ServiceReference;


namespace Passpot.Business
{
    public class LinkModel: ModelBase, IGridData
    {
        #region Ctor

        public LinkModel(List<FieldMetaDataItem> metaDataList, GridData gridData, List<DataConnectList> fieldDataConnectList)
        {
            MetaDataList = metaDataList;
            GridData = gridData;
            DataConnectList = fieldDataConnectList;
        }

        #endregion

        #region IGridData Members

        public List<FieldMetaDataItem> MetaDataList
        {
            get; private set;

        }

        public List<DataConnectList> DataConnectList
        {
            get;
            private set;
        }
        

        public GridData GridData
        {
            get; private set;
        }

        #endregion
    }
}
