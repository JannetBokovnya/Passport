using System.Collections.Generic;
using System.ServiceModel;
using System.Runtime.Serialization;


[ServiceContract]
public interface IServiceData
{
    //------------------------------------------------
    /// <summary>
    /// Возвращает данные для  корневых  элементов дерева
    /// </summary>            
    /// <returns>список пар</returns>
    [OperationContract]
    PairAll CreateRootOfTree(string rootKey, string context, string visibleNode);
   // [OperationContract]
   // List<Pair> CreateRootOfTree(string rootKey, string context, string visibleNode);
    /// <summary>
    /// возвращает список пар дочерних  объектов дерева
    /// </summary>
    /// <param name="key">ключ</param>
    /// <returns></returns>
    [OperationContract]
    PairAll GetNextNode(string key, string nTreeKey, string context, string visibleNode, string keyPassport);
   // [OperationContract]
   // List<Pair> GetNextNode(string key, string nTreeKey, string context, string visibleNode);

    //------------------------------------------------
    //загрузка метаданных
    [OperationContract]
    FieldMetaData MetaData(string iKeyEntity, string iTypePassport, string context, string inObjKey);

    //загрузка данных паспорта
    [OperationContract]
    PassportData GetPassport(string passportKey, int intypePassport, string context);

    // сохранение паспорта  с возвращением ключа паспорта
    [OperationContract]
    //StatusAnswer SavePassport(string passportKey, string iEntityKey, string iParentKey, string inStringFields );
    KeyParentOnSave SavePassport(string passportKey, string iEntityKey, string iParentKey, PassportData passportData, string context, Dictionary<string, string> password);

    //зацементировать нкт с возвратом ключа зацементированного нкт
    [OperationContract]
    KeyCementNKT CementNkt_OnkeyNKT(string passportKey, string context);

    //0 - не показывать строку ошибок, 1- показывать строку ошибок
    //[OperationContract]
    //LogOnVisibleForm LogVisible(string context);
    //0 - не показывать кнопку копирования дерева, 1- показывать 
    [OperationContract]
    BuCopyTreeVisibleForm BuCopyVisible(string context);

    //удаление паспорта со всеми детишками...
    [OperationContract]
    StatusAnswer DeleteObj(string passportkey, string context);
    //Копирование паспорта
    [OperationContract]
    KeyPassportOnCopy CopyPassport(string passportKey, string context);
    //Смена родителей паспорта(перетаскивание по дереву)
    [OperationContract]
    KeyParentOnUpdateTree UpdateParent(string passportKey, string parentKey, string context);
    //Показывает можно ли перетаскивать по дереву(можно ли привязать к другому родителю)
    [OperationContract]
    KeyparentofchildTree UpdateParenttoChild(string passportKey, string parentKey, string context);
    // по двум ключам из схемы аркгис получаюю ключ паспорта
    [OperationContract]
    KeyonShemaArcgis ReturnKeyObjOnShema(string tsdiagram, string tsobject, string context);
    //по ключу объекта - показывать кнопку локализовать на схеме
    [OperationContract]
    KeyonKartaObjList ReturnVisibleObjOnKarta(string passportKey, string context);
    //по ключу объекта определяем доступ(можно ли редактировать или нет и т.д.)
    [OperationContract]
    AccessLevelObjKey ReturnAccessObj(string passportKey, string context);
    //сессия
    //[OperationContract]
    //UserSession2 ReturnUserSession();

    //по ключу объекта показывать кнопку зацементировать или нет
    [OperationContract]
    ButtonNKTObjKey ReturnVisibleNKTObj(string passportKey, string context);

    //получаем таблицу всех объектов энтиту
    [OperationContract]
    DataEntityList_result GetDataAllEntityObj(string context);


    /// <summary>
    /// по ключу паспорта получаем XML паспорта
    /// </summary>
    /// <param name="objKey"></param>
    /// <returns></returns>
    [OperationContract]
    PasportToXML GetXMLPassportToObjKey(string objKey);


    //по ключу папорта возвращает кнопочки на толлбар
    [OperationContract]
    FielButtonData GetButtonToolbar(string passportkey, string buttonTollBar, string context);

    //подписи к основной форме
    [OperationContract]
    DisplayLabels GetDisplayLabels(string context);

    //отдельное дерево
    [OperationContract]
    DataTreeViewOnObjkey ReturnTreeOnObjKey(string objKey, string context);

    //отдельное "полное" дерево по ключу объекта
    [OperationContract]
    DataTreeViewOnObjkey ReturnAllTreeOnObjKey(string objKey, string context);

    //по ключу объекта получаем иерархию связанных объектов(для дерева по ключу)
    [OperationContract]
    TreeFullHierarchyPathToObject GetTreeFullHierarchyPathToObject(string objKey, string context);

    /// <summary>
    /// История изменения паспорта
    /// </summary>
    /// <param name="inObjKey"></param>
    /// <returns></returns>
    [OperationContract]
    DataListHistoryObj GetDataHistoryObj(string inObjKey, string context);

    //-------------------------------------------------------
    //Привязка!!!-------------------------------------
    #region Привязка
    //Привязка
    /// <summary>
    /// получаем таблицу привязки по ключу паспорта
    /// </summary>
    /// <param name="inObjKey"></param>
    /// <returns></returns>
    //[OperationContract]
    //GridData GetGridDataPriv(string inObjKey, string context);


    ///// <summary>
    ///// показывать какие закладки в форме привязки показывать
    ///// </summary>
    ///// <param name="inObjKey"></param>
    ///// <returns></returns>
    //[OperationContract]
    //FormParamsPriv GetparamsPriv(string inObjKey, string context);


    /// <summary>
    /// по ключу объекта возвращает тип привязки
    /// </summary>
    /// <param name="inEntityKey"></param>
    /// <returns></returns>
    //[OperationContract]
    //FormTypePriv GetTypePriv(string inEntityKey, string p_nobjkey, string context);

    /// <summary>
    /// возвращает поля привязки в гриде
    /// </summary>
    /// <param name="inEntitykey"></param>
    /// <returns></returns>
    //[OperationContract]
    //FldParamsPriv GetFldParamspriv(string inEntitykey, string context);


    /// <summary>
    /// при редактирование привязки - ее расчитываем - возврат стринг(Json)
    /// </summary>
    /// <param name="inObjKey"></param>
    /// <param name="strJason"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    [OperationContract]
    CalcPrivJson GetCalcPrivJson(string inObjKey, string strJason, string context);
    /// <summary>
    /// получаем все поля всех привязок с данными привязки по ключу
    /// </summary>
    /// <param name="inObjKey"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    [OperationContract]
    AllPriv GetAllPriv(string inObjKey, string context);
    /// <summary>
    /// сохранение привязки
    /// </summary>
    /// <param name="passportkey"></param>
    /// <param name="mediaKey"></param>
    /// <returns></returns>
    //[OperationContract]
    //StatusAnswer Createlocation2pipe_ByKm(string p_nObjectKey, string p_nObjType, string p_nPipeKey, string p_km, string p_nDistance, string context);

    //[OperationContract]
    //StatusAnswer Addlinktopipe(string p_npipekey, string p_nobjkey, string context);

    //[OperationContract]
    //StatusAnswer BuildSegmentLocationByPipeKM(string p_nObjectKey, string p_nObjType, string p_nPipeKey,
    //            string p_kmBeg, string p_kmEnd, string p_nDistanceBeg, string p_nDistanceEnd, string context);

    //[OperationContract]
    //StatusAnswer BuildPointLocationByPipeXY(string p_nObjectKey, string p_nObjType, string p_nPipeKey,
    //                                        string p_nX, string p_nY, string context);

    //[OperationContract]
    //StatusAnswer BuildSegmentLocationByPipeXY(string p_nObjectKey, string p_nObjType, string p_nPipeKey,
    //                                          string p_nXBeg, string p_nYBeg, string p_nXEnd, string p_nYEnd, string context);

    //[OperationContract]
    //StatusAnswer BuildPointLocationByPipeKM(string p_nObjectKey, string p_nObjType, string p_nPipeKey,
    //                                        string p_km, string p_nDistance, string context);

    ////удаление привязки
    //[OperationContract]
    //StatusAnswer DropLinkToPipe(string p_nObjectKey, string p_nPipeKey, string context);

    //[OperationContract]
    //StatusAnswer DELETELOCATION(string p_nObjectKey, string context);

    [OperationContract]
    StatusAnswer DeleteLinkToPipeByKey(string p_nkey, string context);

    #endregion
    //--------------------------------------------------------------------
    //--------------------------------------------------------
    //// подключение к базе
    //[OperationContract]
    //CurrentUser SetCurrentSession(string in_clogin, string in_cpass);


    [OperationContract]
    HashPassword SetHashPassword(string in_cpass);

    //-----------медиа -------------------------------------
    // сохранение в базе медиаматериалов
    [OperationContract]
    StatusAnswer InsertMediaFile(string mediaFileName, string mediaFileName_small, string comment, string passportKey, string fileName, string typeMedia, string context);

    [OperationContract]
    StatusAnswer DeleteMediaOnObj(string passportkey, string mediaKey, string context);

    /// <summary>
    /// по ключу объекта возвращает все атрибуты медиаматериалов данного пасспорта
    /// </summary>
    /// <param name="passpoerKey"></param>
    /// <returns></returns>
    [OperationContract]
    DataMediaAttribute_result GetMediaAttribute(string passpoerKey, string context);


    //-----------словари -----------------------

    //загрузка ВСЕХ словарей - справочников данного паспорта
    [OperationContract]
    DictDataOnEntityKey DictValueData(string iKeyEntity, string context);

    [OperationContract]
    List<DictDataFiltr> DictDataFiltr(string context);

    [OperationContract]
    nameNSIOnKey GetNameNsiOnKeyNSI(string inKeyNSI, string context);

    [OperationContract]
    ListNsiOnSave SaveNsi(string i_nKeyNsiValue, string i_cNsiValue, string i_nKeyNsi, string context);

    [OperationContract]
    DictDataOne OneDictValueData(string iKeyDict, string context);
    //--------------------------------------

    //загрузка атрибутов ВСЕХ контролов (длина, цвет, высота и т.д.)
    [OperationContract]
    ControlAttribute AttrControl(string context);

    //загрузка атрибутов только одного контрола!
    [OperationContract]
    List<AttributOneControl> AttrOneControl(string iTypeControl, string context);
    //--------------------------------------


    [OperationContract]
    GridData GetGridData(string ipassportKey, string inObjKey, string iTypePassport, string inParentKey, string context);

    //загрузка медиаматериалов - маленькая картинка
    [OperationContract]
    ThumbnailList GetThumbnailList(string passportKey, int mediaType, string context);

    //загрузка медиаматериалов - большая картинка - просмотр
    [OperationContract]
    ThumbnailListBigMedia GetMediaFile(string mediaKey, string context);

    //---паренты---------------------------------------------------
    //[OperationContract]
    //NamePlaseState_result GetNamePlaseState(string inKeyPlaseState, string context);

    //загрузка парентов в комбобокс
    //[OperationContract]
    //DataParentList_result GetDataParent(string entityKey, string context);


    //загрузка выбранного парента в таблицу
    //не актуально!!!!! для добавления нового паспорта не используем выбор парента!!!!
    //[OperationContract]
    //DataOneParent_result GetDataOneParent(string entityParent, string inObjKey_Parent, string context);
    //List<DataOneParent> GetDataOneParent(string entityParent, string inObjKey_Parent);

    // вызов этой функции  закомментировано в  private void NewDetalPassportButton_Click(object sender, RoutedEventArgs e)
    //возвращает количество парентов для сущности
    //[OperationContract]
    //CountParent GetCountParent(string entityKey, string context);



    //------связи------------------------------------------
    [OperationContract]
    List<DataLinkList> GetDataLink(string entityKey, string context);

    [OperationContract]
    DataConnectList_result GetDataConnect(string entityKey, string fldName, string context);

    [OperationContract]
    DataListRelationObj GetDataRelationObj(string inObjKey, string keyFld, string context);

    [OperationContract]
    DataListLinkedObjects GetListLinkedObjects(string objectkey, string entitykey, string fieldname, string linkedentitykey, string linkedobjects, string context);

    [OperationContract]
    KeyRelationOnSave SaveRelationObj(string inObjkey, string keyFld, string keyRelation, string context);

    [OperationContract]
    KeyRelationOnDel DelRelationObj(string inObjkey, string keyFld, string keyRelation, string context);

    [OperationContract]
    GetCheckOrganizationLink_result GetCheckLink(string EntityKey, string ObjParentKey, string LinkType, string context);
    //---чайдлы----------------------------------

    //загрузка чайдлов в комбобокс
    [OperationContract]
    DataChildList_result GetDataChild(string entityKey, string context);

    [OperationContract]
    CountChild_result GetCountChild(string entityKey, string context);



    //------всякие мелочи---------------------------------


    //возвращает название сущности по ключу entityKey
    [OperationContract]
    NameObj_result GetNameObj(string entityKey, string context);

    //возвращает информацию по ключу паспорта(название объекта, название парента, ключ и т.д.)
    [OperationContract]
    PassportInfo_result GetPassportInfo(string inObjKey, string context);

    //возвращает ключ сущности по детальному ключу паспорта
    //[OperationContract]
    //KeyEntity_Obj_result GetKeyEntityObj(string objKey, string context);

    /// <summary>
    /// возвращает название объекта по ключу
    /// </summary>
    /// <param name="inObjKey"></param>
    /// <returns></returns>
    //[OperationContract]
    //NameObjOnKey GetNameObj_OnObjKey(string inObjKey, string context);

    //[OperationContract]
    //KeyParentOnObjKey GetKeyParent(string inObjKey, string context);

    /// <summary>
    /// возвращает тип объекта и его название по ключу
    /// </summary>
    /// <param name="inObjKey"></param>
    /// <returns></returns>

    //[OperationContract]
    //NameEntity_and_ObjOnKey GetObjTypeAndName(string inObjKey, string context);

    //-----------------данные --------------------

    //загрузка мг в комбобокс
    [OperationContract]
    DataMGList_result GetDataMGList(string context);

    //загрузка нитей по ключу мг
    [OperationContract]
    DataNitList_result GetDataNitList(string inKeyMg, string context);
}



////место установки
//[DataMember]
//public string PLASE_STATE { get; set; }
//типы контролов:
//1 - текст или нумерик
//2 - чек бокс
//3 - тип дата - календарь
//4 - комбобокс для словарей, справочников
//5 - контрол с кнопочкой - для объектов связи
//6 - просто лабел (количество фоток или место установки  и т.д.)

