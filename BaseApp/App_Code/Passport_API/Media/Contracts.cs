using System.Collections.Generic;
using System.Runtime.Serialization;

    [DataContract]
    public class Thumbnail
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public double Key { get; set; }

        [DataMember]
        public byte[] Data { get; set; }

        [DataMember]
        public string TypeMedia { get; set; }
    }

    [DataContract]
    public class ThumbnailList : StatusAnswer
    {
        [DataMember]
        public List<Thumbnail> Thumbnails { get; set; }
    }


    [DataContract]
    public class ThumbnailBigMedia
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public byte[] Data { get; set; }

       
    }

    [DataContract]
    public class ThumbnailListBigMedia : StatusAnswer
    {
        [DataMember]
        public List<ThumbnailBigMedia> ThumbnailBigMedia { get; set; }
    }


