using System.Collections.Generic;
using System.Runtime.Serialization;
using System;

    /// <summary>
    /// метаданные паспорта
    /// </summary>


    [DataContract]
    public class FieldMetaData : StatusAnswer
    {

        [DataMember]
        public List<FieldMetaDataItem> FieldMetaDataList { get; set; }
    }


    [DataContract]
    public class FieldMetaDataItem
    {
       // // <summary>
       // // вычисляемое или нет поле
       // // </summary>
        //[DataMember]
        //public int IS_CALC { get; set; }

        /// <summary>
        /// видимое поле или нет в репортсе
        /// </summary>
        [DataMember]
        public int REPORT_VISIBLE { get; set; }

        //// <summary>
        //// тип словаря-справочника
        //// </summary>
        //[DataMember]
        //public int DICTTYPE { get; set; }

        /// <summary>
        /// уникальный ключ поля
        /// </summary>
        [DataMember]
        public int FLDKEY { get; set; }

        /// <summary>
        /// тип поля
        /// </summary>
        [DataMember]
        public string FLDTYP { get; set; }

        /// <summary>
        /// название поля в базе
        /// </summary>
        [DataMember]
        public string FLDNAME { get; set; }

        /// <summary>
        /// название поля на клиенте
        /// </summary>

        [DataMember]
        public string TITUL { get; set; }

        /// <summary>
        ///маска вывода 
        /// </summary>
        [DataMember]
        public string INPUTMASK { get; set; }

        /// <summary>
        ///маска записи в базу 
        /// </summary>
        [DataMember]
        public string OUTPUTMASK { get; set; }

        /// <summary>
        ///видимый на клиенте или нет 
        /// </summary>
        [DataMember]
        public int IS_VISIBLE { get; set; }

        /// <summary>
        ///тип контрола (для клиента) 
        /// </summary>
        [DataMember]
        public int TYPECONTROL { get; set; }

        /// <summary>
        ///номер по очереди вывода 
        /// </summary>
        [DataMember]
        public int NUMONPAGE { get; set; }

        /// <summary>
        /// редактируемый или нет
        /// </summary>
        [DataMember]
        public int IS_EDITED { get; set; }

        /// <summary>
        ///ключевое поле 
        /// </summary>
        [DataMember]
        public int IS_PK { get; set; }



        /// <summary>
        /// вывод на закладке
        /// </summary>
        [DataMember]
        public int PAGENUM { get; set; }

        ///// <summary>
        /////тип поля  
        ///// </summary>
        //[DataMember]
        //public string INPUTOBJTYPE { get; set; }

        /// <summary>
        /// основные поля вывода(выводить необходимые поля)
        /// </summary>
        [DataMember]
        public int BASIC_FLD { get; set; }

        /// <summary>
        /// поля, необходимые для заполнения
        /// </summary>
        [DataMember]
        public int MANDATORYVALUE_FLD { get; set; }
        /// <summary>
        /// цвет рамки контрола
        /// </summary>
        [DataMember]
        public string COLOR_FLD { get; set; }
        /// <summary>
        /// ключ словаря (или название справочника - вьюха)
        /// </summary>
        [DataMember]
        public string KEYDICT { get; set; }

        /// <summary>
        /// валидация данных - относится только к типам контролов - числовое и символ значения
        /// </summary>
        [DataMember]
        public int TYPEVALIDATION { get; set; }


        #region added by Max 30.06.2010
        /*
     * эти дополнительные поля  мы  будем  использовать  для  организации  поддержки в движком паспортов 
     * свободного  расположения контролов  на странице паспорта; поддержки иерархий; группирований; 
     * указания размеров контролов; местоположения на странице
     */
        /// <summary>
        /// высота элемента
        /// </summary>
        [DataMember]
        public int HEIGHT_CONTROL { get; set; }
        /// <summary>
        /// ширина элемента
        /// </summary>
        [DataMember]
        public int WIDTH_CONTROL { get; set; }

        [DataMember]
        public int COLUMN_NUM { get; set; }

        /// <summary>
        /// уникальное имя элемента
        /// </summary>
        [DataMember]
        public Guid UNIQ_CONTROL_NAME { get; set; }
        /// <summary>
        /// имя родительского элемента
        /// </summary>
        [DataMember]
        public Guid PARENT { get; set; }

        #endregion



    }

