using System.Collections.Generic;
using System.Runtime.Serialization;

/// <summary>
/// загрузка данных паспорта 
/// </summary>
[DataContract]
public class PassportData : StatusAnswer
{
    //[DataMember]
    //public bool IsValid { get; set; }

    //[DataMember]
    //public string ErrorMessage { get; set; }


    /// <summary>
    /// первое поле - название столбца, второе - его значение
    /// </summary>
    [DataMember]
    public Dictionary<string, string> Data { get; set; }
}
[DataContract]
public class DataLinkList
{
    [DataMember]
    public string KEYLINK { get; set; }
    [DataMember]
    public string NAMELINK { get; set; }
}

/// <summary>
/// по ключу объекта какие вкладки показывать
/// </summary>

//[DataContract]
//public class FormParamsPriv : StatusAnswer
//{
//    [DataMember]
//    public List<FormParamsPrivItems> FormParamsPrivItemsList { get; set; }
//}


//[DataContract]
//public class FormParamsPrivItems
//{
//    [DataMember]
//    public string ISXYZSHEETNEEDED { get; set; }

//    [DataMember]
//    public string IS2PIPESHEETNEEDED { get; set; }

//    [DataMember]
//    public string ISENDVISIBLE { get; set; }

//    [DataMember]
//    public string ISJUSTPIPE { get; set; }

//}





/// <summary>
/// возвращает тип привязки
/// </summary>
[DataContract]
public class FormTypePriv : StatusAnswer
{
    [DataMember]
    public List<FormTypePrivItems> FormParamsTypePrivItemsList { get; set; }
}

[DataContract]
public class FormTypePrivItems
{
    [DataMember]
    public string TYPEPRIV { get; set; }

    [DataMember]
    public string ISLOCATIONEXIST { get; set; }

}


/// <summary>
/// метаданные по привязке - какие поля выводить в гриде созданной привязке
/// </summary>
[DataContract]
public class FldParamsPriv : StatusAnswer
{
    [DataMember]
    public List<FldParamsPrivItems> FldParamsPrivItemsList { get; set; }
}



[DataContract]
public class FldParamsPrivItems
{
    [DataMember]
    public string CNAME { get; set; }

    [DataMember]
    public string CEXPR { get; set; }

    [DataMember]
    public string CCAPTION { get; set; }

    [DataMember]
    public string CDATA_TYPE { get; set; }

    [DataMember]
    public int ISPK { get; set; }

}


//при редактирование привязки - ее расчитываем - возврат стринг(Json)
[DataContract]
public class CalcPrivJson: StatusAnswer
{
    [DataMember]
    public CalcPrivOnEditJson CalcPrivOnEditJson_result { get; set; }

}
[DataContract]
public class CalcPrivOnEditJson
{
    [DataMember]
    public string CalcPrivOnEditJsonOnKeyPassport { get; set; }

}


//возвращается привязка
[DataContract]
public class AllPriv : StatusAnswer
{
    [DataMember]
    public List<PrivItems> PrivItemsList { get; set; }
}


[DataContract]
public class PrivItems
{
    [DataMember]
    public string CNAME { get; set; }

    [DataMember]
    public string NKMORT1 { get; set; }

    [DataMember]
    public string NDISTANCEBEG { get; set; }

    [DataMember]
    public string NX1 { get; set; }

    [DataMember]
    public string NY1 { get; set; }

    [DataMember]
    public string NZ1 { get; set; }

    [DataMember]
    public string NKEY { get; set; }

    [DataMember]
    public string NH1 { get; set; }

    [DataMember]
    public string NKMTRUE1 { get; set; }

    [DataMember]
    public string NX2 { get; set; }

    [DataMember]
    public string NY2 { get; set; }

    [DataMember]
    public string NZ2 { get; set; }

    [DataMember]
    public string NH2 { get; set; }

    [DataMember]
    public string NKMORT2 { get; set; }

    [DataMember]
    public string NKMTRUE2 { get; set; }

    [DataMember]
    public string NDISTANCEEND { get; set; }

    [DataMember]
    public string nPipeKey { get; set; }

    [DataMember]
    public string nMtKey { get; set; }

    [DataMember]
    public string NBUILDTYPE { get; set; }

    [DataMember]
    public string ISEDITED { get; set; }

}



/// <summary>
/// Отдельное дерево(по ключу выстраивается весь путь дерева)
/// </summary>
/// 
[DataContract]
public class DataTreeViewOnObjkey : StatusAnswer
{
    [DataMember]
    public List<DataTreeViewOnObjkeyItems> DataTreeViewOnObjkeyList { get; set; }
}


[DataContract]
public class DataTreeViewOnObjkeyItems
{
    /// <summary>
    /// Вывод текста в веточке дерева
    /// </summary>
    [DataMember]
    public string Texts { get; set; }
    /// <summary>
    /// Ключ для показаза или паспорта или таблицы
    /// </summary>
    [DataMember]
    public string Key { get; set; }
    /// <summary>
    /// Нужно для иерархии дерева
    /// </summary>
    [DataMember]
    public string Level { get; set; }
    /// <summary>
    /// нужно для иерархии дерева
    /// </summary>
    [DataMember]
    public string ParentKey { get; set; }
}

/// <summary>
/// по ключу объекта получаем иерархию связанных объектов
/// </summary>
[DataContract]
public class TreeFullHierarchyPathToObject : StatusAnswer
{
    [DataMember]
    public List<TreeFullHierarchyPathToObjectItems> TreeFullHierarchyPathToObjectList { get; set; }
}


[DataContract]
public class TreeFullHierarchyPathToObjectItems
{

    /// <summary>
    /// Ключ  паспорта
    /// </summary>
    [DataMember]
    public string ObjKey { get; set; }
    /// <summary>
    /// тип паспорта
    /// </summary>
    [DataMember]
    public string ObjTypeKey { get; set; }
    /// <summary>
    /// Нужно для иерархии дерева
    /// </summary>
    [DataMember]
    public string Level { get; set; }

}






/// <summary>
/// доступ к паспортам
/// </summary>

[DataContract]
public class AccessLevelObjKey : StatusAnswer
{
    [DataMember]
    public AccessLevel_OnKey AccessLevel_OnKey_result { get; set; }
}


[DataContract]
public class AccessLevel_OnKey
{
    [DataMember]
    public string AccessLevel { get; set; }
}


/// Определяем сессию 

[DataContract]
public class UserSession2 : StatusAnswer
{
    [DataMember]
    public UserSession_syst UserSession_syst_result { get; set; }
}

[DataContract]
public class UserSession_syst
{
    [DataMember]
    public string UserSession1 { get; set; }
}



/// <summary>
/// кнопка зацементировать НКТ(показывать или нет)
/// </summary>

[DataContract]
public class ButtonNKTObjKey : StatusAnswer
{
    [DataMember]
    public ButtonNKT_OnKey ButtonNKT_OnKet_result { get; set; }
}


[DataContract]
public class ButtonNKT_OnKey
{
    [DataMember]
    public string ButtonNKTVisible { get; set; }
}

/// <summary>
/// Получение всех горизонтальных связей данного объекта (все привязанные связи)
/// </summary>
[DataContract]
public class DataListRelationObj : StatusAnswer
{
    [DataMember]
    public List<DataListRelationObjItems> DataListRelationObjList { get; set; }
}


[DataContract]
public class DataListRelationObjItems
{
    /// <summary>
    /// Ключ объекта связи
    /// </summary>
    [DataMember]
    public string KeyObj { get; set; }
    /// <summary>
    /// название entity
    /// </summary>
    [DataMember]
    public string NameEntity { get; set; }
    /// <summary>
    /// название объекта
    /// </summary>
    [DataMember]
    public string NameObj { get; set; }
}

[DataContract]
public class DataListLinkedObjects : StatusAnswer
{
    [DataMember]
    public List<DataListLinkedObjectsItems> DataListLinkedObjectsList { get; set; }
}

[DataContract]
public class DataListLinkedObjectsItems
{
    /// <summary>
    /// Ключ объекта связи
    /// </summary>
    [DataMember]
    public string ObjKey { get; set; }
    /// <summary>
    /// название entity
    /// </summary>
    [DataMember]
    public string EntityKey { get; set; }
    /// <summary>
    /// название объекта
    /// </summary>
    [DataMember]
    public string ObjName { get; set; }
}


/// <summary>
/// сохранение связей, возврат ключ связи
/// </summary>
[DataContract]
public class KeyRelationOnSave : StatusAnswer
{
    [DataMember]
    public KeyRelation_OnSave KeyRelation_OnSaveObj_result { get; set; }
}


[DataContract]
public class KeyRelation_OnSave
{
    [DataMember]
    public string KeyRelation_OnSaveObj { get; set; }
}
/// <summary>
/// удаление связей
/// </summary>
public class KeyRelationOnDel : StatusAnswer
{
    [DataMember]
    public KeyRelation_OnDel KeyRelation_OnDelObj_result { get; set; }
}
[DataContract]
public class KeyRelation_OnDel
{
    [DataMember]
    public string KeyRelation_OnDelObj { get; set; }
}





/// <summary>
/// Получение истории изменения паспорта
/// </summary>
[DataContract]
public class DataListHistoryObj : StatusAnswer
{
    [DataMember]
    public List<DataListHistoryItems> DataListHistoryObjList { get; set; }
}


[DataContract]
public class DataListHistoryItems
{
    /// <summary>
    /// Имя пользователя
    /// </summary>
    [DataMember]
    public string CUser { get; set; }
    /// <summary>
    /// дата/время
    /// </summary>
    [DataMember]
    public string Doperationtime { get; set; }
    /// <summary>
    /// название атрибута
    /// </summary>
    [DataMember]
    public string CFieldName { get; set; }
    /// <summary>
    /// старое значение
    /// </summary>
    [DataMember]
    public string Coldval { get; set; }
    /// <summary>
    /// новое значение
    /// </summary>
    [DataMember]
    public string Cnewval { get; set; }
}


/// <summary>
/// сохранение пкспорта с возвратом ключа объекта
/// </summary>

[DataContract]
public class KeyParentOnSave : StatusAnswer
{
    [DataMember]
    public KeyObj_OnSave KeyObj_OnSaveObj_result { get; set; }
}


[DataContract]
public class KeyObj_OnSave
{
    [DataMember]
    public string KeyObj_OnSaveObj { get; set; }
}


/// <summary>
/// зацементировать НКТ с возвратом ключа нового созданного объекта
/// </summary>

[DataContract]
public class KeyCementNKT : StatusAnswer
{
    [DataMember]
    public KeyObj_OnCement KeyObj_OnCementNKT_result { get; set; }
}


[DataContract]
public class KeyObj_OnCement
{
    [DataMember]
    public string KeyObj_OnCementNKT { get; set; }
}

/// <summary>
/// показывать или нет строку сообщения
/// </summary>

[DataContract]
public class LogOnVisibleForm : StatusAnswer
{
    [DataMember]
    public LogOnVisible_Form LogOnVisible_result { get; set; }
}


[DataContract]
public class LogOnVisible_Form
{
    [DataMember]
    public string LogOnVisible { get; set; }
}

/// <summary>
/// показывать или нет кнопку копирования дерева
/// </summary>
[DataContract]
public class BuCopyTreeVisibleForm : StatusAnswer
{
    [DataMember]
    public BuCopyTreeVisible_Form BuCopyTreeVisible_result { get; set; }
}


[DataContract]
public class BuCopyTreeVisible_Form
{
    [DataMember]
    public string BuCopyTreeVisible { get; set; }
}




/// <summary>
///копирование паспорта с возвратом ключа объекта(пока только возвращает 1 - скопирован или -1 если ошибка)
/// </summary>

[DataContract]
public class KeyPassportOnCopy : StatusAnswer
{
    [DataMember]
    public KeyObj_OnCopy KeyObj_OnCopyObj_result { get; set; }
}



[DataContract]
public class KeyObj_OnCopy
{
    [DataMember]
    public string KeyObj_OnCopyObj { get; set; }
}

//активация кноопки - показать на схеме + ключи для схемы

[DataContract]
public class KeyonKartaObjList : StatusAnswer
{
    [DataMember]
    public List<KeyonKartaList> KeyonKartaList_result { get; set; }
}


[DataContract]
public class KeyonKartaList
{
    [DataMember]
    public string cGeomKey { get; set; }

    [DataMember]
    public string nTS { get; set; }
}




// ---------Drag and Drop----------

/// <summary>
/// Смена родителей при "перетаскивании в дереве"
/// </summary>

[DataContract]
public class KeyParentOnUpdateTree : StatusAnswer
{
    [DataMember]
    public KeyParent_OnUpdate KeyParent_OnUpdateTree_result { get; set; }
}



[DataContract]
public class KeyParent_OnUpdate
{
    [DataMember]
    public string KeyParent_OnUpdateTree { get; set; }
}

/// <summary>
/// Можно ли менять родителей  при "перетаскивании в дереве"
/// </summary>

[DataContract]
public class KeyparentofchildTree : StatusAnswer
{
    [DataMember]
    public Keyparentofchild Keyparentofchild_result { get; set; }
}


[DataContract]
public class Keyparentofchild
{
    [DataMember]
    public string Keyparentofchild_Tree { get; set; }
}

// End ---------Drag and Drop----------

// по двум ключам из схемы аркгис получаюю ключ паспорта

[DataContract]
public class KeyonShemaArcgis : StatusAnswer
{
    [DataMember]
    public KeyonShema KeyonShema_result { get; set; }
}


[DataContract]
public class KeyonShema
{
    [DataMember]
    public string KeyonShema_arcgis { get; set; }
}

//end --------------------------------------------------

//---------------паренты------------------------------------
/// <summary>
/// определяем место установки(по сути название парента) по ключу места установки
/// </summary>
//[DataContract]
//public class NamePlaseState_result : StatusAnswer
//{
//    [DataMember]
//    public NameParent NameParentOnPlaseState_result { get; set; }

//}
[DataContract]
public class NameParent
{
    [DataMember]
    public string NameParentOnPlaseState { get; set; }

}

/// <summary>
/// по ключу обїекта получаем ключ его парента
/// </summary>
[DataContract]
public class keyParent : StatusAnswer
{
    [DataMember]
    public KeyParent KeyParentOnkeyObj_result { get; set; }
}


[DataContract]
public class KeyParent
{
    [DataMember]
    public string KeyParentOnkeyObj { get; set; }
}





/// <summary>
/// список всех парентов для конкретной сущности(ключ сущности)
/// </summary>
[DataContract]
public class DataParentList_result : StatusAnswer
{
    [DataMember]
    public List<DataParentList> DataParentLists { get; set; }
}
/// <summary>
/// список всех парентов для конкретного паспорта
/// </summary>
[DataContract]
public class DataParentList
{
    /// <summary>
    /// Ключ entitykey парента
    /// </summary>
    [DataMember]
    public string KEYPARENT { get; set; }

    /// <summary>
    /// Название парента
    /// </summary>
    [DataMember]
    public string NAMEPARENT { get; set; }
}





/// <summary>
/// список газопроводов
/// </summary>
[DataContract]
public class DataMGList_result : StatusAnswer
{
    [DataMember]
    public List<DataMGList> DataMGLists { get; set; }
}


[DataContract]
public class DataMGList
{
    /// <summary>
    /// Ключ газопровода
    /// </summary>
    [DataMember]
    public string KEYMG { get; set; }

    /// <summary>
    /// Название газопровода
    /// </summary>
    [DataMember]
    public string NAMEMG { get; set; }
}

/// <summary>
/// списое нитей на газопроводе
/// </summary>
[DataContract]
public class DataNitList_result : StatusAnswer
{
    [DataMember]
    public List<DataNitList> DataNitLists { get; set; }
}


[DataContract]
public class DataNitList
{
    /// <summary>
    /// Ключ газопровода
    /// </summary>
    [DataMember]
    public string KEYNIT { get; set; }

    /// <summary>
    /// Название газопровода
    /// </summary>
    [DataMember]
    public string NAMENIT { get; set; }
}





/// <summary>
/// один парент - загрузка в таблицу
/// </summary>
/// 

[DataContract]
public class DataOneParent_result : StatusAnswer
{
    [DataMember]
    public List<DataOneParent> DataOneParentList { get; set; }
}

/// <summary>
/// таблица уже конкретного парента
/// </summary>
[DataContract]
public class DataOneParent
{
    /// <summary>
    /// ключ парента(уже в выбранной таблице)
    /// </summary>
    [DataMember]
    public string KEYONEPARENT { get; set; }
    /// <summary>
    /// название парента
    /// </summary>
    [DataMember]
    public string NAMEONEPARENT { get; set; }

}

/// <summary>
/// количество парентов для отдельной сущности(ключ сущности)
/// </summary>
//[DataContract]
//public class CountParent
//{
//    [DataMember]
//    public int CountParentOnEntityKey { get; set; }
//}

//---- связи --------------------------------------

/// <summary>
/// список всех связей для конкретной сущности(ключ сущности) и название поля
/// </summary>
[DataContract]
public class DataConnectList_result : StatusAnswer
{
    [DataMember]
    public List<DataConnectList> DataConnectLists { get; set; }
}
/// <summary>
/// список всех связей для конкретного паспорта
/// </summary>
[DataContract]
public class DataConnectList
{
    /// <summary>
    /// Ключ entitykey связи
    /// </summary>
    [DataMember]
    public string KEYCONNECT { get; set; }

    /// <summary>
    /// Название связи
    /// </summary>
    [DataMember]
    public string NAMECONNECT { get; set; }
}

//------чайдлы-----------------------------------
/// <summary>
/// список всех чайдлов сущности
/// </summary>

[DataContract]
public class DataChildList_result : StatusAnswer
{
    [DataMember]
    public List<DataChildList> DataChildLists { get; set; }
}

public class DataChildList
{
    /// <summary>
    /// ключ парента
    /// </summary>
    [DataMember]
    public string ENTITYKEYCHILDS { get; set; }

    /// <summary>
    /// название парента
    /// </summary>
    [DataMember]
    public string NAMECHILDS { get; set; }

}
[DataContract]
public class CountChild_result : StatusAnswer
{
    [DataMember]
    public CountChild CountChildOnEntityKey_result { get; set; }
}

[DataContract]
public class CountChild
{
    [DataMember]
    public int CountChildOnEntityKey { get; set; }
}



//--------всякая всячина---------------------------------

//[DataContract]
//public class KeyEntity_Obj_result : StatusAnswer
//{
//    [DataMember]
//    public KeyEntity_Obj KeyEntityOnObjKey { get; set; }
//}
/// <summary>
/// список всех объектов
/// </summary>
[DataContract]
public class DataEntityList_result : StatusAnswer
{
    [DataMember]
    public List<DataEntityList> DataEntityLists { get; set; }
}
public class DataEntityList
{
    /// <summary>
    /// ключ энтиту
    /// </summary>
    [DataMember]
    public string KEYENTITY { get; set; }

    /// <summary>
    /// название энтити
    /// </summary>
    [DataMember]
    public string VALUEENTITY { get; set; }
}
/// <summary>
/// по ключу паспорта возвращаются все названия медиа файлов+ комментарии+тип медиа
/// </summary>
[DataContract]
public class DataMediaAttribute_result : StatusAnswer
{
    [DataMember]
    public List<DataMediaAttribute> DataMediaAttributeList { get; set; }
}

public class DataMediaAttribute
{
    /// <summary>
    /// тип медияматериалов
    /// </summary>
    [DataMember]
    public string type_media { get; set; }

    /// <summary>
    /// комментарии к медиаматериалам
    /// </summary>
    [DataMember]
    public string comment_media { get; set; }

    /// <summary>
    /// название файла
    /// </summary>
    [DataMember]
    public string name_file { get; set; }

}

/// <summary>
/// ключ сущности по ключу объекта(паспорта)
/// </summary>
/// 
//[DataContract]
//public class KeyEntity_Obj_result : StatusAnswer
//{
//    [DataMember]
//    public KeyEntity_Obj KeyEntityOnKeyObj_result { get; set; }
//}

//[DataContract]
//public class KeyEntity_Obj
//{
//    /// <summary>
//    /// Ключ entitykey парента
//    /// </summary>
//    [DataMember]
//    public string KeyEntityOnKeyObj { get; set; }

//}

/// <summary>
/// название сущности по ключу сущности
/// </summary>
[DataContract]
public class NameObj_result : StatusAnswer
{
    [DataMember]
    public NameObj NameObjectOnEntityKey_result { get; set; }

}
[DataContract]
public class NameObj
{
    /// <summary>
    /// Ключ entitykey парента
    /// </summary>
    [DataMember]
    public string NameObjectOnEntityKey { get; set; }

}

/// <summary>
/// проверка кардинальности связи
/// </summary>
[DataContract]
public class GetCheckOrganizationLink_result: StatusAnswer
{
    [DataMember]
    public GetCheckOrganizationLink GetCheckOrganizationLinkOnkeyPassport_result { get; set; }
}
[DataContract]
public class GetCheckOrganizationLink
{
    [DataMember]
    public string GetCheckOrganizationLinkOnkeyPassport { get; set; }
}


[DataContract]
public class PassportInfo_result : StatusAnswer
{
    [DataMember]
    public PassportInfo PassportInfo { get; set; }
}
[DataContract]
public class PassportInfo
{
   
    /// <summary>
    ///  //название места установки - название парента
    /// </summary>
    [DataMember]
    public string NameParentOnPlaseState { get; set; }

    
    /// <summary>
    /// //ключ парента по ключу объекта
    /// </summary>
    [DataMember]
    public string KeyParentOnkeyObj { get; set; }

   /// <summary>
    ///  //ключ entity парента 
   /// </summary>
    [DataMember]
    public string ParentEntityKey { get; set; }

    /// <summary>
    /// //название entity парента 
    /// </summary>
    [DataMember]
    public string NameParentEntityKey { get; set; }

    /// <summary>
    /// основное название паспорта
    /// </summary>
    [DataMember]
    public string GetNameObj { get; set; }

    /// <summary>
    /// //название  entity  паспорта
    /// </summary>
    [DataMember]
    public string NameEntityObj { get; set; }

   /// <summary>
    ///  //ключ entity  паспорта
   /// </summary>
    [DataMember]
    public string KeyEntityObj { get; set; }

    /// <summary>
    /// можно или нет удалять паспорт
    /// </summary>
    [DataMember]
    public int IsDeleteCurrentPassport { get; set; }

    /// <summary>
    /// возможность редактировать паспорт
    /// </summary>
    [DataMember]
    public int IsEditedCurrentPasport { get; set; }

}



/// <summary>
/// по ключу объекта возвращает кнопочки, url в тоолбаре
/// </summary>

[DataContract]
public class FielButtonData : StatusAnswer
{

    [DataMember]
    public List<FielButtonDataItem> FielButtonDataItemList { get; set; }
}

[DataContract]
public class FielButtonDataItem
{
    /// <summary>
    /// название картинки
    /// </summary>
    [DataMember]
    public string NameJpg { get; set; }
    /// <summary>
    /// Подпись к картинке
    /// </summary>
    [DataMember]
    public string ContentJpg { get; set; }
    /// <summary>
    /// Событие
    /// </summary>
    [DataMember]
    public string CEvent { get; set; }

    [DataMember]
    public string CAttribute { get; set; }

    [DataMember]
    public string CParams { get; set; }

    [DataMember]
    public string NButtonType { get; set; }

}

/// <summary>
/// подписи на основных формах
/// </summary>

[DataContract]
public class DisplayLabels : StatusAnswer
{

    [DataMember]
    public List<DisplayLabelsItem> DisplayLabelsItemList { get; set; }
}

[DataContract]
public class DisplayLabelsItem
{
    /// <summary>
    /// название места
    /// </summary>
    [DataMember]
    public string Сplace { get; set; }
    /// <summary>
    /// Подпись 
    /// </summary>
    [DataMember]
    public string Сvalue { get; set; }


}



/// <summary>
/// выгрузка в XML паспорта
/// </summary>

[DataContract]
public class PasportToXML : StatusAnswer
{
    [DataMember]
    public List<PasportToXMLItems> PasportToXMLItemsList { get; set; }
}

[DataContract]
public class PasportToXMLItems
{

    [DataMember]
    public string strToXML { get; set; }

}


// --Вход в базу
[DataContract]
public class CurrentUser : StatusAnswer
{
    [DataMember]
    public CurrentUser_OnLog CurrentUser_OnLog_result { get; set; }
}

[DataContract]
public class CurrentUser_OnLog
{
    [DataMember]
    public string CurrentUser_OnLogBase { get; set; }
}


//hash пароля
[DataContract]
public class HashPassword : StatusAnswer
{
    [DataMember]
    public HashPassword_OnLog CurrentUser_OnLog_result { get; set; }
}

[DataContract]
public class HashPassword_OnLog
{
    [DataMember]
    public string HashPassword_OnLogBase { get; set; }
}









