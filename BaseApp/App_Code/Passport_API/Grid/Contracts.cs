using System.Collections.Generic;
using System.Runtime.Serialization;


    [DataContract]
    public class GridRow
    {
        [DataMember]
       public List<object> Cels { get; set; }
       
    }

    [DataContract]
    public class GridData : StatusAnswer
    {
        [DataMember]
        public List<string> FieldNames { get; set; }

        [DataMember]
        public List<GridRow> Rows { get; set; }
    }

