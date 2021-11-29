using System.Collections.Generic;
using System.Runtime.Serialization;


//---------дерево-------------------------------

/// <summary>
/// описыват пары значений для хранения элементов дерева
/// </summary>
/// 
[DataContract]
public class PairAll : StatusAnswer
{

    [DataMember]
    public List<PairItem> PairItemList { get; set; }
}

[DataContract]
public class PairItem
{
    /// <summary>
    /// отображается в элементе дерева
    /// </summary>
    [DataMember]
    public string Texts;
    /// <summary>
    /// составной ключ позволяющий получить   дочерние объекты,
    /// а также хранящий связь с родительским объектом
    /// </summary>
    [DataMember]
    public string Key;
}


//[DataContract]
//public class Pair
//{
//    /// <summary>
//    /// отображается в элементе дерева
//    /// </summary>
//    [DataMember]
//    public string Texts;
//    /// <summary>
//    /// составной ключ позволяющий получить   дочерние объекты,
//    /// а также хранящий связь с родительским объектом
//    /// </summary>
//    [DataMember]
//    public string Key;

//}



