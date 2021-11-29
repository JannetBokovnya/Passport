using System.Runtime.Serialization;
using System.Collections.Generic;

    /// <summary>
    /// атрибуты ВСЕХ контролов
    /// </summary>

    [DataContract]
    public class ControlAttribute : StatusAnswer
    {

        [DataMember]
        public List<ControlAttributeItem> ControlAttributeList { get; set; }
    }

    [DataContract]
    public class ControlAttributeItem
    {
        /// <summary>
        /// тип контрола
        /// </summary>
        [DataMember]
        public int TYPECONTROL { get; set; }

        /// <summary>
        /// ширина
        /// </summary>
        [DataMember]
        public int WIDTH { get; set; }

        /// <summary>
        /// высота
        /// </summary>
        [DataMember]
        public int HEIGHT { get; set; }

        /// <summary>
        /// цвет фона
        /// </summary>

        [DataMember]
        public string COLORFON { get; set; }


        /// <summary>
        /// цвет текста
        /// </summary>
        [DataMember]
        public string COLORTEXT { get; set; }

        /// <summary>
        /// размер текста
        /// </summary>
        [DataMember]
        public int FONTSIZE { get; set; }

        /// <summary>
        /// подчеркивание
        /// </summary>
        [DataMember]
        public int UNDERLINE { get; set; }

    }
    /// <summary>
    /// аттрибуты одного  контрола
    /// </summary>

    [DataContract]
    public class AttributOneControl
    {

        /// <summary>
        /// ширина
        /// </summary>
        [DataMember]
        public int WIDTH { get; set; }

        /// <summary>
        /// высота
        /// </summary>
        [DataMember]
        public int HEIGHT { get; set; }

        /// <summary>
        /// цвет фона
        /// </summary>

        [DataMember]
        public string COLORFON { get; set; }


        /// <summary>
        /// цвет текста
        /// </summary>
        [DataMember]
        public string COLORTEXT { get; set; }

        /// <summary>
        /// размер текста
        /// </summary>
        [DataMember]
        public int FONTSIZE { get; set; }

        /// <summary>
        /// подчеркивание
        /// </summary>
        [DataMember]
        public int UNDERLINE { get; set; }

    }
