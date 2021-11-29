using System.Runtime.Serialization;
using System.Collections.Generic;


    /// <summary>
    /// загрузка словарей-справочников для конкретного паспорта 
    /// </summary>
    /// 

    [DataContract]
    public class DictDataOnEntityKey : StatusAnswer
    {
        [DataMember]
        public List<DictData> DictData_result { get; set; }
    }

    /// <summary>
    /// загрузка одного словаря для добавления значения
    /// </summary>
    [DataContract]
    public class DictDataOne : StatusAnswer
    {
        [DataMember]
        public List<OneDictData> DictData_result { get; set; }
    }


    /// <summary>
    /// сохранение значения словаря с возвратом всех значений словаря по ключу
    /// </summary>

    [DataContract]
    public class ListNsiOnSave : StatusAnswer
    {
        [DataMember]
        public List<OneDictData> DictData_result { get; set; }
    }





    [DataContract]
    public class DictData
    {

        /// <summary>
        /// Ключ значения словаря
        /// </summary>
        [DataMember]
        public string VALUEKEYDICT { get; set; }

        /// <summary>
        /// Значение словаря
        /// </summary>
        [DataMember]
        public string VALUEDICT { get; set; }

        /// <summary>
        /// ключ словаря
        /// </summary>
        [DataMember]
        public string KEYDICT { get; set; }


    }

    [DataContract]
    public class DictDataFiltr
    {
        /// <summary>
        /// Ключ значения конкретного словаря
        /// </summary>
        [DataMember]
        public string KEYDICT { get; set; }

        /// <summary>
        /// Значение конкретного словаря
        /// </summary>
        [DataMember]
        public string VALUEDICT { get; set; }


    }



    [DataContract]
    public class OneDictData
    {
        /// <summary>
        /// Ключ значения конкретного словаря
        /// </summary>
        [DataMember]
        public string KEYVALUEDICT { get; set; }

        /// <summary>
        /// Значение конкретного словаря
        /// </summary>
        [DataMember]
        public string VALUEDICT { get; set; }


    }


    /// <summary>
    /// по ключу NSI получаем его название
    /// </summary>
    [DataContract]
    public class nameNSIOnKey : StatusAnswer
    {
        [DataMember]
        public nameNSI_Key nameNSIonKey_result { get; set; }
    }


    [DataContract]
    public class nameNSI_Key
    {
        [DataMember]
        public string NameNSIonkey { get; set; }
    }


    //[DataContract]
    //public class NameObj_result : StatusAnswer
    //{
    //    [DataMember]
    //    public NameObj NameObjectOnEntityKey_result { get; set; }

    //}
    //[DataContract]
    //public class NameObj
    //{
    //    /// <summary>
    //    /// Ключ entitykey парента
    //    /// </summary>
    //    [DataMember]
    //    public string NameObjectOnEntityKey { get; set; }

    //}


    //.......................................


