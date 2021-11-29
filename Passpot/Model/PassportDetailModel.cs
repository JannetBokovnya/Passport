using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Browser;
using Media;
using Media.Interfaces;
using Media.Resources;
using Passpot.Controls;
using System.Windows.Controls;
using Services.ServiceReference;
using System.IO;
using Passpot.Business;


namespace Passpot.Model
{
    public class PassportDetailModel : ModelBase, IGridData, IPassportDetailModel
    {
        #region Ctor

        public PassportDetailModel(MainModel mainModel)
        {
            MainModel = mainModel;
        }

        #endregion Ctor

        #region Properties

        public string PassportKey { get; private set; }
        public string ParentNameKeyOnAdmin { get; private set; }
        public bool isFindTreeonkey { get; private set; }
        public string visiblebutonShema;
        private string _visibleChildTabItem;
        public string FieldNameTextonButton;
        public string FieldNameButton;
        public int TypeVisible;
        public int TypeControl;

        public string TextPrivExel = string.Empty;

        public PassportDetailOpenParam PassportDetailOpenParams { get; private set; }
        private string _oldPassportKey; //сохраняем ключ паспорта  - для перестроения ветки дерева(если паспорт был редактирован)


        public string _cGeomKey;
        public string _ts;
        public string _accesslevel;
        public string _accesslevel_1;
        public string _buttonNKTVisible;
        public string UserSession1;
        public IMainModel MainModel { get; private set; }

        public string _getKeyCementNKT;
        private string _entityKey;
        private string _namePlaseState;
        private string _objTypeAndName;
        private string _nameObjDetail;
        private string _buttonObj;
        private string _reportOnSavePassport;
        public List<FielButtonDataItem> ButtonDataList { get; private set; }
        private string _keyParent;
        private int _countChild;
        private string _getKeyPassport;
        private string _nameObjConnect;
        private string _getNameDict;
        public string keyPassportOnDelete = string.Empty;
        public string keyMediaOnDelete = string.Empty;
        public string nameMediaOnDelete = string.Empty;
        private SaveFileDialog dialog = new SaveFileDialog();

        private string _typeControl = "";

        private List<DataChildList> _selectChildList;
        private List<DataListHistoryItems> _getHistory;
        private List<DataConnectList> _selectConnectList;
        //private List<DataListRelationObjItems> _selectRelationObjGridList;

        //public ListRelationObj ListRelationObj = new ListRelationObj("",""); 


        public int _deleteMediaEnd;
        private string _TYPEPRIV;
        private string _ISLOCATIONEXIST;
        private string _reloadGridPriv;
        private ChildWindowAttantion _popupWindowAttantion;
        private string _keyParentInfo;
        private string _entityKeyInfo;
        private string _entityParentName;
        private bool _VisibleBuPrintTPA;
        private bool _isShow;
        private string _keyPassportOnDeleteLinkOnButton;
        private int _isDeleteCurrentPassport;
        private int _isEditedCurrentPassport;
        



        public string EntityKeyInfo
        {
            get { return _entityKeyInfo; }
            set
            {
                _entityKeyInfo = value;
            }
        }

        public string EntityKey
        {
            get { return _entityKey; }
            set
            {
                _entityKey = value;
                FirePropertyChanged("EntityKey");
            }
        }

        public bool IsShow
        {
            get { return _isShow; }
            set
            {
                IsShowBusy = value;

            }
        }

        public string GeomKey
        {
            get { return _cGeomKey; }
            set
            {
                _cGeomKey = value;
                FirePropertyChanged("VisibleButtonShema");
            }
        }

        //показывать кнопку форматированная печать ТПА
        public bool VisibleBuPrintTPA
        {
            get { return _VisibleBuPrintTPA; }
            set
            {
                _VisibleBuPrintTPA = value;
                FirePropertyChanged("VisibleBuPrintTPA");
            }
        }

        #region //переменные для привязки, какие вкладки показывать


        public string TYPEPRIV
        {
            get { return _TYPEPRIV; }
            set { _TYPEPRIV = value; }
        }

        public string ISLOCATIONEXIST
        {
            get { return _ISLOCATIONEXIST; }
            set { _ISLOCATIONEXIST = value; }
        }

        #endregion

        //ключ паспорта для удаления из дочернего окна childwindowLookAllLink
        public string KeyPassportOnDeleteLinkOnButton
        {
            get { return _keyPassportOnDeleteLinkOnButton; }
            set { _keyPassportOnDeleteLinkOnButton = value; }
        }

        public string VisibleChildTabItem
        {
            get { return _visibleChildTabItem; }
            set { _visibleChildTabItem = value; }
        }

        public string TS
        {
            get { return _ts; }
            set
            {
                _ts = value;
            }

        }

        public string Accesslevel
        {
            get { return _accesslevel; }
            set
            {
                _accesslevel = value;
                FirePropertyChanged("VisibleButtonRedo");
            }
        }

        public string ButtonNKTVisible
        {
            get { return _buttonNKTVisible; }
            set
            {
                _buttonNKTVisible = value;
                FirePropertyChanged("ButtonNKTVisible");
            }
        }



        public string NameObjConnect
        {
            get { return _nameObjConnect; }
            set
            {
                _nameObjConnect = value;
                FirePropertyChanged("NameObjConnect");
            }
        }

        public int CountChild
        {
            get { return _countChild; }
            set
            {
                _countChild = value;
                FirePropertyChanged("CountChild");
            }
        }

        public string NamePlaseState
        {
            get { return _namePlaseState; }
            set
            {
                _namePlaseState = value;
                FirePropertyChanged("NamePlaseState");
            }
        }

        /// <summary>
        /// возможность редактировать данный паспорт 1- можно 0-нельзя
        /// </summary>
        public int IsEditedCurrentPassport
        {
            get { return _isEditedCurrentPassport; }
            set
            {
                _isEditedCurrentPassport = value;
                FirePropertyChanged("IsEditedCurrentPassport");
            }
        }


        /// <summary>
        /// возможность удалять данный пасспорт 1-можно 0-нельзя
        /// </summary>
        public int IsDeleteCurrentPassport
        {
            get { return _isDeleteCurrentPassport; }
            set
            {
                _isDeleteCurrentPassport = value;
                FirePropertyChanged("IsDeleteCurrentPassport");
            }
        }


        public string ReloadGridPriv
        {
            get { return _reloadGridPriv; }
            set
            {
                _reloadGridPriv = value;
                FirePropertyChanged("ReloadGridPriv");
            }
        }

        public string ObjTypeAndName
        {
            get { return _objTypeAndName; }
            set
            {
                _objTypeAndName = value;
                FirePropertyChanged("ObjTypeAndName");
            }
        }

        /// <summary>
        /// название entity парента
        /// </summary>
        public string EntityParentName
        {
            get { return _entityParentName; }
            set
            {
                _entityParentName = value;

            }
        }

        public string NameObjDetail
        {
            get { return _nameObjDetail; }
            set
            {
                _nameObjDetail = value;
                FirePropertyChanged("NameObjDetail");
            }
        }

        public string ReportOnSavePassport
        {
            get { return _reportOnSavePassport; }
            set
            {
                _reportOnSavePassport = value;
                FirePropertyChanged("ReportOnSavePassport");
            }
        }

        public string ButtonObj
        {
            get { return _buttonObj; }
            set
            {
                _buttonObj = value;
                FirePropertyChanged("ButtonObj");
            }
        }
        public string KeyParentInfo
        {
            get { return _keyParentInfo; }
            set
            {
                _keyParentInfo = value;

            }
        }
        public string KeyParent
        {
            get { return _keyParent; }
            set
            {
                _keyParent = value;
                FirePropertyChanged("KeyParent");
            }
        }
        public List<DataChildList> SelectChildList
        {
            get
            {
                return _selectChildList;
            }
            set
            {
                _selectChildList = value;
                FirePropertyChanged("SelectChildList");
            }
        }

        public List<DataListHistoryItems> GetHistory
        {
            get { return _getHistory; }
            set { _getHistory = value; FirePropertyChanged("GetHistory"); }
        }

        public List<DataConnectList> SelectConnectList
        {
            get
            {
                return _selectConnectList;
            }
            set
            {
                _selectConnectList = value;
                //контрол текст и буттон
                switch (_typeControl)
                {
                    case "tb":
                        {
                            FirePropertyChanged("SelectConnectList");
                            break;
                        }
                    case "b":
                        {
                            FirePropertyChanged("SelectConnectListButtonRelation");
                            break;
                        }
                    case "g":
                        {
                            FirePropertyChanged("SelectConnectListGridRelation");
                            break;
                        }
                }
            }
        }

        public string GetNameDict
        {
            get { return _getNameDict; }
            set
            {
                _getNameDict = value;
                FirePropertyChanged("StartOnNameNSI");
            }
        }

        public string GetKeyPassport
        {
            get { return _getKeyPassport; }
            set
            {
                _getKeyPassport = value;
                FirePropertyChanged("GetKeyPassport");
            }
        }

        public string GetKeyCementNKT
        {
            get { return _getKeyCementNKT; }
            set
            {
                _getKeyCementNKT = value;
                FirePropertyChanged("GetCementNKT");
            }
        }

        public int DeleteMediaEnd
        {
            get { return _deleteMediaEnd; }
            set
            {
                _deleteMediaEnd = value;
                FirePropertyChanged("DeleteMediaEnd");
            }
        }


        /// <summary>
        ///  Список данных словаря по текущему паспорту
        /// </summary>
        public List<DictData> DictDataList { get; private set; }

        /// <summary>
        /// Список значений аттрибутов всех контроллов 
        /// </summary>
        public List<ControlAttributeItem> ControlAttributeListAll { get; private set; }

        // входные данные паспорта: поле, значение(для просмотра)
        public Dictionary<string, string> PassportData { get; private set; }
        // входные данные паспорта: поле, значение(для редактирования)
        public Dictionary<string, string> PassportEditData { get; private set; }
        //данные для горизонтальных связей - нужно для вывода в ексель
        public ObservableCollection<ListRelationObj> ListRelationObjList { get; set; }

        //метаданные для редактирования паспорта
        public List<FieldMetaDataItem> MetaEditDataList { get; private set; }

        #endregion Properties

        #region Public methods

        public void InitModel(PassportDetailOpenParam passportDetailOpenParam)
        {
            ListRelationObjList = new ObservableCollection<ListRelationObj>();
            PassportDetailOpenParams = passportDetailOpenParam;
            PassportKey = passportDetailOpenParam.PassportKey;
            ParentNameKeyOnAdmin = passportDetailOpenParam.ParentNameKeyOnAdmin;
           
            try
            {
                ControlAttributeListAll = new List<ControlAttributeItem>();
               // StartAttrControl();
                //создание нового паспорта
                if (passportDetailOpenParam.PassportKey == "0")
                {
                    //атрибуты паспорта - лабл для паспорта
                    StartGetPassportInfoNewpasport(PassportDetailOpenParams.EntityKey, PassportDetailOpenParams.ParentKey);
                    //метаданные для редактирования паспорта
                }
                //если паспорт создан
                else
                {
                    //атрибуты паспорта - ключ ентиту, название ентиту, ключ ентиту родителя, название родителя и т.д.
                    StartGetPassportInfo(passportDetailOpenParam.PassportKey);
                    //кнопки на тулбаре
                    StartGetButtonToolBarOnObj(passportDetailOpenParam.PassportKey, visiblebutonShema);

                }
            }
            catch (Exception)
            {
                MainModel.Report("InitModel err = Ошибка иницилизирования модели пасспорт" ); // Выводим сообщение об ошибке
            }

        }

        #region основные характеристики НОВОГО паспорта - объект, родитель
        public void StartGetPassportInfoNewpasport(string inEntityKey, string inParentKey)
        {
            ServiceDataClient.GetPassportInfoCompleted += GetPassportInfoParentNewPassportCompleted;
            ServiceDataClient.GetPassportInfoAsync(inParentKey, GlobalContext.Context);
           
        }

        private void GetPassportInfoParentNewPassportCompleted(object sender, GetPassportInfoCompletedEventArgs e)
        {
            ServiceDataClient.GetPassportInfoCompleted -= GetPassportInfoParentNewPassportCompleted;
            if (e.Result.IsValid)
            {
                NamePlaseState = e.Result.PassportInfo.GetNameObj; //"Место установки - название парента
                EntityParentName = e.Result.PassportInfo.NameEntityObj; //название entity парента
                //NameObjDetail = e.Result.PassportInfo.GetNameObj; //Название объекта по ключу

                //возможность удалять паспорт
                IsDeleteCurrentPassport = e.Result.PassportInfo.IsDeleteCurrentPassport;
                    //возможность редактировать паспорт
                IsEditedCurrentPassport = e.Result.PassportInfo.IsEditedCurrentPasport;

                //KeyParentInfo = e.Result.PassportInfo.KeyParentOnkeyObj; //ключ парента
                EntityKeyInfo = PassportDetailOpenParams.EntityKey; //ключ объекта
                //ObjTypeAndName = e.Result.PassportInfo.NameEntityObj; //"Название типа объекта и объекта по ключу
                
            }

            if (string.IsNullOrEmpty(PassportDetailOpenParams.EntityKey))
            {
                EntityKey = EntityKeyInfo;
                PassportDetailOpenParams.EntityKey = EntityKey;

            }
            else
            {
                EntityKey = PassportDetailOpenParams.EntityKey;
            }


            if (!String.IsNullOrEmpty(EntityKey))
            {
                StartMetaDataEditedOnPassportDetail(EntityKey);    
            }
            else
            {
                MainModel.Report("Ошибка EntityKey не определено! " );
               
            }
            
            //потом
            ServiceDataClient.GetNameObjCompleted += GetNameObjCompleted;
            ServiceDataClient.GetNameObjAsync(PassportDetailOpenParams.EntityKey, GlobalContext.Context);
        }

        void GetNameObjCompleted(object sender, GetNameObjCompletedEventArgs e)
        {
            if (e.Result.IsValid)
            {
                NameObjDetail = ProjectResources.NewPassport;
                ObjTypeAndName = e.Result.NameObjectOnEntityKey_result.NameObjectOnEntityKey;
                
            }
            else
            {
                MainModel.Report("Ошибка названия объекта GetNameObjCompleted err = " + e.Result.ErrorMessage);
                MainModel.Indicator(false);
            }
        }
        
        #endregion основные характеристики паспорта - название, объект, родитель и т.д.
        //основные характеристики паспорта - название, объект, родитель и т.д.
        #region основные характеристики паспорта - название, объект, родитель и т.д.
        public void StartGetPassportInfo(string inObjKey)
        {
            ServiceDataClient.GetPassportInfoCompleted += GetPassportInfoCompleted;
            ServiceDataClient.GetPassportInfoAsync(inObjKey, GlobalContext.Context);

        }

        void GetPassportInfoCompleted(object sender, GetPassportInfoCompletedEventArgs e)
        {
            ServiceDataClient.GetPassportInfoCompleted -= GetPassportInfoCompleted;
            if (e.Result.IsValid)
            {
                NamePlaseState = e.Result.PassportInfo.NameParentOnPlaseState; //"Место установки - название парента
                NameObjDetail = e.Result.PassportInfo.GetNameObj; //Название объекта по ключу
                EntityParentName = e.Result.PassportInfo.NameParentEntityKey; //название entity парента
                KeyParentInfo = e.Result.PassportInfo.KeyParentOnkeyObj; //ключ парента
                EntityKeyInfo = e.Result.PassportInfo.KeyEntityObj; //ключ объекта
                ObjTypeAndName = e.Result.PassportInfo.NameEntityObj; //"Название типа объекта и объекта по ключу
                //возможность удалять паспорт
                IsDeleteCurrentPassport = e.Result.PassportInfo.IsDeleteCurrentPassport;
                //возможность редактировать паспорт
                IsEditedCurrentPassport = e.Result.PassportInfo.IsEditedCurrentPasport;

                if (string.IsNullOrEmpty(PassportDetailOpenParams.EntityKey))
                {
                    EntityKey = EntityKeyInfo;
                    PassportDetailOpenParams.EntityKey = EntityKey;

                }
                else
                {
                    EntityKey = PassportDetailOpenParams.EntityKey;
                }

                if (PassportDetailOpenParams.IsEditedPassport == 0)
                {
                    //просмотр паспорта
                    //метаданные на просмотр (1)
                    StartMetaDataComplitedOnPassportDetail(EntityKey);

                }
                else
                {
                    //редактирование паспорта

                    //формируем метаданные для редактирования пасспорта
                    StartMetaDataEditedOnPassportDetail(EntityKey);
                    //данные для паспорта - для просмотра - 1, для редактирования - 2


                }

            }
            else
            {
                MainModel.Report("Ошибка GetPassportInfo err = " + e.Result.ErrorMessage);
                MainModel.Indicator(false);
                
            }

            

          
        }

        #endregion основные характеристики паспорта - название, объект, родитель и т.д.

        //данные для паспорта - ПРОСМОТР
        private void OnGetPassportCompleted(object sender, GetPassportCompletedEventArgs e)
        {
            ServiceDataClient.GetPassportCompleted -= OnGetPassportCompleted;
            ListRelationObjList = new ObservableCollection<ListRelationObj>();

            PassportData passport = e.Result;

            if (passport.IsValid)
            {
                //данные для паспорта
                PassportData = passport.Data;
                MainModel.Report("данные для просмотра + OnGetPassportCompleted + количество " + e.Result.Data.Count);

                //кнопка форматированная печать ТПА и УКЗ - временно
                if (EntityKeyInfo == "15936672")//15936672
                {
                    foreach (var dd in PassportData)
                    {
                        if ((dd.Key == "NAZNACHENIE") && ((dd.Value == "линейная") || (dd.Value == "linear")))//15936870
                        {
                            {
                                VisibleBuPrintTPA = true;
                            }
                        }
                    }
                }

                if (EntityKeyInfo == "15936870")
                {
                    VisibleBuPrintTPA = true;
                }

                StartAttrControl();

            }
            else
            {
                MainModel.Report("OnGetPassportCompleted err = " + passport.ErrorMessage); // Выводим сообщение об ошибке
                MainModel.Indicator(false);

            }
        }

        //данные для редактирования паспорта
        void OnGetPassportEditCompleted(object sender, GetPassportCompletedEventArgs e)
        {
            ServiceDataClient.GetPassportCompleted -= OnGetPassportEditCompleted;

            PassportData passportEdit = e.Result;

            if (passportEdit.IsValid)
            {
                
                PassportEditData = passportEdit.Data;
                MainModel.Report("данные для редактирования + OnGetPassportCompleted + количество " + e.Result.Data.Count);
                StartAttrControl();
            }
            else
            {
                MainModel.Report("Данные для редактирования OnGetPassportEditCompleted err = " + passportEdit.ErrorMessage); // Выводим сообщение об ошибке
                
            }


            //MainModel.Indicator(false);
        }

        #region метаданные для просмотра пасспорта - 1
        //метаданные для просмотра пасспорта - 1
        public void StartMetaDataComplitedOnPassportDetail(string EntityKey)
        {
            ServiceDataClient.MetaDataCompleted += OnMetaDataCompleted;
            ServiceDataClient.MetaDataAsync(EntityKey, "1", GlobalContext.Context, PassportDetailOpenParams.PassportKey);
        }

        void OnMetaDataCompleted(object sender, MetaDataCompletedEventArgs e)
        {
            ServiceDataClient.MetaDataCompleted -= OnMetaDataCompleted;
            if (e.Result.IsValid)
            {
                if (e.Result.FieldMetaDataList.Count != 0)
                {
                    MetaDataList = new List<FieldMetaDataItem>(e.Result.FieldMetaDataList);
                    MainModel.Report("Метаданные просмотр OnMetaDataCompleted СФОРМИРОВАНЫ!  ");
                    MainModel.Report("Метаданные для просмотр количество полей " + e.Result.FieldMetaDataList.Count);

                    if (PassportDetailOpenParams.PassportKey != "0")
                    {
                        //данные для паспорта - для просмотра - 1, для редактирования - 2
                        ServiceDataClient.GetPassportCompleted += OnGetPassportCompleted;
                        ServiceDataClient.GetPassportAsync(PassportDetailOpenParams.PassportKey, 1, GlobalContext.Context);
                    }     
                }
                else
                {
                    MainModel.Report("Метаданные просмотр OnMetaDataCompleted нет данных " + e.Result.ErrorMessage);
                    MainModel.Indicator(false);
                }
            }
            else
            {
                MainModel.Report("Метаданные просмотр OnMetaDataCompleted err = " + e.Result.ErrorMessage);
                MainModel.Indicator(false);
            }
        }

        #endregion метаданные для просмотра пасспорта - 1

        #region формируем метаданные для редактирования пасспорта -2

        public void StartMetaDataEditedOnPassportDetail(string EntityKey)
        {
            ServiceDataClient.MetaDataCompleted += OnMetaDataEditedCompleted;
            ServiceDataClient.MetaDataAsync(EntityKey, "2", GlobalContext.Context, PassportDetailOpenParams.PassportKey);
        }

        void OnMetaDataEditedCompleted(object sender, MetaDataCompletedEventArgs e)
        {
            ServiceDataClient.MetaDataCompleted -= OnMetaDataEditedCompleted;

            if (e.Result.IsValid)
            {
                MainModel.Report("Метаданные для редактирования  OnMetaDataEditedCompleted СФОРМИРОВАНЫ ");
                MainModel.Report("Метаданные для редактирования количество полей " + e.Result.FieldMetaDataList.Count);

                if (e.Result.FieldMetaDataList.Count != 0)
                {
                    MetaEditDataList = new List<FieldMetaDataItem>(e.Result.FieldMetaDataList);

                    //загружаем словари!
                    StarDictOnPassportDetailEdited(PassportDetailOpenParams.EntityKey);    
                }

                else
                {
                    MainModel.Report("Метаданные для редактирования  OnMetaDataEditedCompleted  " + e.Result.ErrorMessage);
                    MainModel.Indicator(false);
                    
                }
                

            }
            else
            {
                MainModel.Report("Метаданные для редактирования  OnMetaDataEditedCompleted err = " + e.Result.ErrorMessage);
                MainModel.Indicator(false);
            }

        }

        #endregion формируем метаданные для редактирования пасспорта

        #region загружаем словари
        public void StarDictOnPassportDetailEdited(string EntityKey)
        {
            //MainModel.Indicator(true);
            DictDataList = new List<DictData>();
            ServiceDataClient.DictValueDataCompleted += DictValueDataCompleted;
            ServiceDataClient.DictValueDataAsync(PassportDetailOpenParams.EntityKey, GlobalContext.Context);
        }
        void DictValueDataCompleted(object sender, DictValueDataCompletedEventArgs e)
        {
            ServiceDataClient.DictValueDataCompleted -= DictValueDataCompleted;
            if (e.Result.IsValid)
            {
                MainModel.Report("Словари DictValueDataCompleted ЗАГРУЖЕНЫ!!!!!!!! ");
                DictDataList = new List<DictData>(e.Result.DictData_result);

                if (PassportDetailOpenParams.PassportKey == "0")
                {
                    StartAttrControl();
                }
                else
                {
                    //выборка уже с конкретным ключем из базы (но уже для редактирования паспорта)
                    // для просмотра - 1, для редактирования - 2

                    ServiceDataClient.GetPassportCompleted += OnGetPassportEditCompleted;
                    ServiceDataClient.GetPassportAsync(PassportDetailOpenParams.PassportKey, 2, GlobalContext.Context);
                }
            }
            else
            {
                MainModel.Report("Словари DictValueDataCompleted err = " + e.Result.ErrorMessage);
            }


        }
        #endregion загрузка словарей - пасмотр для редактирования

        //атрибуты всех типов контролов
        public void StartAttrControl()
        {
            ServiceDataClient.AttrControlCompleted += AttrControlCompleted;
            ServiceDataClient.AttrControlAsync(GlobalContext.Context);
        }
        void AttrControlCompleted(object sender, AttrControlCompletedEventArgs e)
        {
            ServiceDataClient.AttrControlCompleted -= AttrControlCompleted;
            if (e.Result.IsValid)
            {
                ControlAttributeListAll = new List<ControlAttributeItem>(e.Result.ControlAttributeList);
                MainModel.Report("Аттрибуты контроллов загружены ");
                MainModel.Report("Аттрибуты контроллов всего " + e.Result.ControlAttributeList.Count);

                MainModel.Report("Инициируем ModelInited ");
                FirePropertyChanged("ModelInited");
            }
            else
            {
                MainModel.Report("Аттрибуты AttrControlCompleted err = " + e.Result.ErrorMessage);
                MainModel.Indicator(false);
            }

        }

        //сохранение паспорта
        #region сохранение паспорта
        public void SavePassport(string passportKey, string iEntityKey, string iParentKey, PassportData passportData, Dictionary<string, string> hashPassword )
        {
            _oldPassportKey = passportKey;
            IsShowBusy = true;
            ServiceDataClient.SavePassportCompleted += OnSavePassportCompleted;
            ServiceDataClient.SavePassportAsync(passportKey, iEntityKey, iParentKey, passportData, GlobalContext.Context, hashPassword);
        }

        void OnSavePassportCompleted(object sender, SavePassportCompletedEventArgs e)
        {
            ServiceDataClient.SavePassportCompleted -= OnSavePassportCompleted;
            IsShowBusy = false;
            if (e.Result.IsValid)
            {

                MainModel.Report(e.Result.ErrorMessage);

                if (e.Result.KeyObj_OnSaveObj_result.KeyObj_OnSaveObj == "")
                {
                    MainModel.Report("Ошибка сохранения паспорта" + e.Result.ErrorMessage);
                    MainModel.FirePropertyChanged("PassportNOTSaved");
                    //MainModel.FirePropertyChanged("EditPassportSaved");
                    _popupWindowAttantion = new ChildWindowAttantion();
                    _popupWindowAttantion.titleBox.Text = ProjectResources.ErrSavePasportMessage + e.Result.ErrorMessage;
                    _popupWindowAttantion.Show();
                    
                }
                else
                {

                    GetKeyPassport = e.Result.KeyObj_OnSaveObj_result.KeyObj_OnSaveObj;

                    if (GetKeyPassport == "-1")
                    {
                        MainModel.FirePropertyChanged("PassportNOTSaved");
                        MainModel.Report("Паспорт НЕ сохранен!! Ошибка -1");
                    }
                    else
                    {
                        //перезагружаем паспорт уже с полученным ключем
                        //необходимо сделать для всех, но сделано только для редактируемых паспортов - т.к. заглушка!
                        MainModel.FirePropertyChanged("PassportSaved");
                        MainModel.NeedOpenPassportDetail = new PassportDetailOpenParam() { PassportKey = GetKeyPassport, EntityKey = PassportDetailOpenParams.EntityKey, IsVisibleButtonTransit = PassportDetailOpenParams.IsVisibleButtonTransit, ParentNameKeyOnAdmin = PassportDetailOpenParams.ParentNameKeyOnAdmin }; //  "908918609"  245203
                        MainModel.Report("Паспорт сохранен");
                        ReportOnSavePassport = "Паспорт сохранен";

                        if (_oldPassportKey == "0")
                        {
                            MainModel.FirePropertyChanged("EditPassportSaved");
                        }
                        else
                        {
                            MainModel.FirePropertyChanged("EditPassportReloadTreeNode");
                            //if (GlobalContext.Context == 40.ToString())
                            //{
                            //    MainModel.FirePropertyChanged("EditAdminDostypReloadTreeNode");
                            //}
                            //else
                            //{
                            //    MainModel.FirePropertyChanged("EditPassportReloadTreeNode");
                            //}
                            
                        }
                    }
                }

            }
            else
            {
                MainModel.Report("Сохранение паспорта OnSavePassportCompleted err = " + e.Result.ErrorMessage);
            }
        }

        #endregion сохранение паспорта

        //кнопки на тулбаре
        #region динамические кнопки на тулбаре
        public void StartGetButtonToolBarOnObj(string inObjKey, string visibleButton)
        {
            ServiceDataClient.GetButtonToolbarCompleted += GetButtonToolbarCompleted;
            ServiceDataClient.GetButtonToolbarAsync(inObjKey, visibleButton, GlobalContext.Context);
        }

        void GetButtonToolbarCompleted(object sender, GetButtonToolbarCompletedEventArgs e)
        {
            ServiceDataClient.GetButtonToolbarCompleted -= GetButtonToolbarCompleted;
            if (e.Result.IsValid)
            {
                ButtonDataList = new List<FielButtonDataItem>(e.Result.FielButtonDataItemList);
                FirePropertyChanged("ButtonAdd");
            }
            else
            {
                MainModel.Report("Ошибка добавления кнопок на туллбар err = " + e.Result.ErrorMessage);
            }
        }

        #endregion динамические кнопки на тулбаре

        //список всех дочек паспорта - для закладки дочерние объекты
        #region список всех дочек паспорта - для закладки дочерние объекты
        public void StartGetDataChild(string entityKey)
        {
            //GetUserSession();
            ServiceDataClient.GetDataChildCompleted += GetDataChildCompleted;
            ServiceDataClient.GetDataChildAsync(entityKey, GlobalContext.Context);
        }

        void GetDataChildCompleted(object sender, GetDataChildCompletedEventArgs e)
        {
            ServiceDataClient.GetDataChildCompleted -= GetDataChildCompleted;
            if (e.Result.IsValid)
            {
                SelectChildList = new List<DataChildList>(e.Result.DataChildLists);
            }
            else
            {
                MainModel.Report("Список child GetDataChild err = " + e.Result.ErrorMessage);
            }
        }

        #endregion список всех дочек паспорта - для закладки дочерние объекты

        //история паспорта
        #region история паспорта
        public void StartDataHistory(string inObjKey)
        {
            //GetUserSession();
            ServiceDataClient.GetDataHistoryObjCompleted += GetDataHistoryObjCompleted;
            ServiceDataClient.GetDataHistoryObjAsync(inObjKey, GlobalContext.Context);
        }

        void GetDataHistoryObjCompleted(object sender, GetDataHistoryObjCompletedEventArgs e)
        {
            ServiceDataClient.GetDataHistoryObjCompleted -= GetDataHistoryObjCompleted;
            if (e.Result.IsValid)
            {
                GetHistory = new List<DataListHistoryItems>(e.Result.DataListHistoryObjList);
            }
            else
            {
                MainModel.Report("история GetDataHistoryObj err = " + e.Result.ErrorMessage);
            }
        }

        #endregion история паспорта


        //связи объекта - горизонтальные связи
        #region связи объекта
        public void StartOnDataConnect(string entityKey, string fldName, string typeControl)
        {

            //GetUserSession();
            ServiceDataClient.GetDataConnectCompleted += OnGetDataConnectCompleted;
            ServiceDataClient.GetDataConnectAsync(entityKey, fldName, GlobalContext.Context);
            FieldNameTextonButton = fldName;
            _typeControl = typeControl;
        }

        void OnGetDataConnectCompleted(object sender, GetDataConnectCompletedEventArgs e)
        {
            ServiceDataClient.GetDataConnectCompleted -= OnGetDataConnectCompleted;
            if (e.Result.IsValid)
            {
                SelectConnectList = new List<DataConnectList>(e.Result.DataConnectLists);
            }
            else
            {
                MainModel.Report("Список связей OnGetDataConnect err = " + e.Result.ErrorMessage);
            }
        }

        #endregion связи объекта


        #region удаление  паспорта в дочерних объектах (кнопка в гриде в дочерних объектах)

        //удаление  паспорта в дочерних объектах (кнопка в гриде в дочерних объектах)

        public void DeletePassport(string passportKey)
        {
            //GetUserSession();
            ServiceDataClient.DeleteObjCompleted += OnDeletePassportCompleted;
            ServiceDataClient.DeleteObjAsync(passportKey, GlobalContext.Context);
            IsShowBusy = true;

        }

        void OnDeletePassportCompleted(object sender, DeleteObjCompletedEventArgs e)
        {
            ServiceDataClient.DeleteObjCompleted -= OnDeletePassportCompleted;
            IsShowBusy = false;
            if (e.Result.IsValid)
            {
                MainModel.Report("Паспорт удален");
                //разобраться! после удаления перечитівать паспорт?

                //ServiceDataClient.GetGridDataCompleted += OnGetGridDataCompleted;
                //ServiceDataClient.GetGridDataAsync(PassportOpenParams.EntityKey, "0", "1"); 

            }
            else
            {
                MainModel.Report("Удаление паспорта OnDeletePassport err = " + e.Result.ErrorMessage);
            }
        }

        #endregion удаление  паспорта в дочерних объектах (кнопка в гриде в дочерних объектах)


        //удаление медиаматериала
        #region удаление медиаматериала
        public void DeleteMedia(string passportKey, string mediaKey, string nameMedia)
        {

            ServiceDataClient.DeleteMediaOnObjCompleted += DeleteMediaOnObj;
            ServiceDataClient.DeleteMediaOnObjAsync(passportKey, mediaKey, GlobalContext.Context);

            //старая версия - клик на картинке буттон - удаление медия
            //ServiceDataClient.DeleteMediaOnObjCompleted += DeleteMediaOnObj;
            //ServiceDataClient.DeleteMediaOnObjAsync(passportKey, mediaKey, GlobalContext.Context);
            //IsShowBusy = true;

        }
        public void Variablesmedia(string passportKey, string mediaKey, string nameMedia)
        {
            keyPassportOnDelete = passportKey;
            keyMediaOnDelete = mediaKey;
            nameMediaOnDelete = nameMedia;
        }

        void DeleteMediaOnObj(object sender, DeleteMediaOnObjCompletedEventArgs e)
        {
            ServiceDataClient.DeleteMediaOnObjCompleted -= DeleteMediaOnObj;
            IsShowBusy = false;
            if (e.Result.IsValid)
            {
                keyPassportOnDelete = "";
                keyMediaOnDelete = "";
                nameMediaOnDelete = "";
                MainModel.Report("удален медиаматериал");
                DeleteMediaEnd = 1;
            }
            else
            {
                MainModel.Report("Удаление медиаматериалов DeleteMedia err = " + e.Result.ErrorMessage);
            }
        }



        public void GetMediaFile(string keyMedia, string nameMedia)
        {
            //dialog.DefaultExt = nameMedia;
            dialog.Filter = "JPG Files|*.jpg" + "|All Files|*.*";

            dialog.DefaultFileName = nameMedia;
            dialog.FilterIndex = 1;
           

            bool? saveClicked = dialog.ShowDialog();

            if (saveClicked == true)
            {
                ServiceDataClient.GetMediaFileCompleted += GetMediaFileOnSaveCompleted;
                ServiceDataClient.GetMediaFileAsync(keyMedia, GlobalContext.Context);
            }

        }

        void GetMediaFileOnSaveCompleted(object sender, GetMediaFileCompletedEventArgs e)
        {
            ServiceDataClient.GetMediaFileCompleted -= GetMediaFileOnSaveCompleted;

            if (e.Result.IsValid)
            {

                using (Stream fs = (Stream)this.dialog.OpenFile())
                {
                    fs.Write(e.Result.ThumbnailBigMedia[0].Data, 0, e.Result.ThumbnailBigMedia[0].Data.Length);
                    fs.Close();

                   MainModel.Report("Медиа Файл сохранен!!!!");
                }

            }
            else
            {
                MainModel.Report("Получение медиа для сохранения ошибка: " + e.Result.ErrorMessage);
            }

        }
           




        #endregion удаление медиаматериала


        #region доступ - можно ли редактировать данный объект - пока не используется
        //доступ - можно ли редактировать данный объект - пока не используется
        public void StartAccesslevel(string inObjKey)
        {
            //GetUserSession();
            MainModel.Report("Доступ ReturnAccessObj err = ВРЕМЕННО ЗАКРЫТ!!!! ");
            ServiceDataClient.ReturnAccessObjCompleted += ReturnAccessObjCompleted;
            ServiceDataClient.ReturnAccessObjAsync(inObjKey, GlobalContext.Context);
        }

        //доступ - можно ли редактировать данный объект - пока не используется
        void ReturnAccessObjCompleted(object sender, ReturnAccessObjCompletedEventArgs e)
        {
            ServiceDataClient.ReturnAccessObjCompleted -= ReturnAccessObjCompleted;
            if (e.Result.IsValid)
            {
                //GetUserSession();
                //ServiceDataClient.ReturnVisibleNKTObjCompleted += ReturnVisibleNKTObjCompleted;
                //ServiceDataClient.ReturnVisibleNKTObjAsync(PassportKey, GlobalContext.Context);
                _accesslevel_1 = e.Result.AccessLevel_OnKey_result.AccessLevel;

            }
            else
            {
                MainModel.Report("Доступ ReturnAccessObj err = " + e.Result.ErrorMessage);
            }
        }

        #endregion доступ - можно ли редактировать данный объект - пока не используется


        #endregion Public methods


        #region Private methods

        /// <summary>
        /// Возвращает метаданные
        /// </summary>
        private List<FieldMetaDataItem> GetMetaData(string passportKey)
        {
            var result = new List<FieldMetaDataItem>();
            // TODO: Заменить на получение реальных данных
            result.Add(new FieldMetaDataItem { IS_VISIBLE = 1, TITUL = "ArtifactId", TYPECONTROL = 7, FLDNAME = "ArtifactId", IS_EDITED = 1, BASIC_FLD = 0, TYPEVALIDATION = 1 });
            result.Add(new FieldMetaDataItem { IS_VISIBLE = 1, TITUL = "Ключ", TYPECONTROL = 7, FLDNAME = "key", IS_EDITED = 1, BASIC_FLD = 0, TYPEVALIDATION = 1 });
            result.Add(new FieldMetaDataItem { IS_VISIBLE = 1, TITUL = "Тип контрола текст", TYPECONTROL = 1, FLDNAME = "textfield", IS_EDITED = 1, BASIC_FLD = 0, TYPEVALIDATION = 2 });
            result.Add(new FieldMetaDataItem { IS_VISIBLE = 1, TITUL = "Тип контрола текст - для числовый значений", TYPECONTROL = 7, FLDNAME = "numberfield", IS_EDITED = 1, BASIC_FLD = 0, TYPEVALIDATION = 2 });
            result.Add(new FieldMetaDataItem { IS_VISIBLE = 1, TITUL = "Словарь", TYPECONTROL = 7, FLDNAME = "numberdict", IS_EDITED = 1, BASIC_FLD = 0, });
            result.Add(new FieldMetaDataItem { IS_VISIBLE = 1, TITUL = "Дата", TYPECONTROL = 3, FLDNAME = "datafield", IS_EDITED = 1, BASIC_FLD = 0 });
            result.Add(new FieldMetaDataItem { IS_VISIBLE = 1, TITUL = "Вывод справочных данных(фото)", TYPECONTROL = 6, FLDNAME = "textnoneditlabel", IS_EDITED = 1, BASIC_FLD = 0 });
            result.Add(new FieldMetaDataItem { IS_VISIBLE = 1, TITUL = "Добавление связей", TYPECONTROL = 7, FLDNAME = "numberconnect", IS_EDITED = 1, BASIC_FLD = 0 });
            result.Add(new FieldMetaDataItem { IS_VISIBLE = 1, TITUL = "Чек бокс", TYPECONTROL = 7, FLDNAME = "checkbox", IS_EDITED = 1, BASIC_FLD = 0 });

            return result;
        }

        /// <summary>
        /// Возвращает метаданные
        /// </summary>
        private List<FieldMetaDataItem> GetEditMetaData(string passportKey)
        {
            var result = new List<FieldMetaDataItem>();
            // TODO: Заменить на получение реальных данных
            result.Add(new FieldMetaDataItem { IS_VISIBLE = 1, TITUL = "ArtifactId", TYPECONTROL = 7, FLDNAME = "ArtifactId", IS_EDITED = 1, BASIC_FLD = 0, TYPEVALIDATION = 1 });
            result.Add(new FieldMetaDataItem { IS_VISIBLE = 1, TITUL = "Ключ", TYPECONTROL = 7, FLDNAME = "key", IS_EDITED = 1, BASIC_FLD = 0, TYPEVALIDATION = 1 });
            result.Add(new FieldMetaDataItem { IS_VISIBLE = 1, TITUL = "Тип контрола текст", TYPECONTROL = 1, FLDNAME = "textfield", IS_EDITED = 1, BASIC_FLD = 0, TYPEVALIDATION = 2 });
            result.Add(new FieldMetaDataItem { IS_VISIBLE = 1, TITUL = "Тип контрола текст - для числовый значений", TYPECONTROL = 7, FLDNAME = "numberfield", IS_EDITED = 1, BASIC_FLD = 0, TYPEVALIDATION = 2 });
            result.Add(new FieldMetaDataItem { IS_VISIBLE = 1, TITUL = "Словарь", TYPECONTROL = 7, FLDNAME = "numberdict", IS_EDITED = 1, BASIC_FLD = 0, });
            result.Add(new FieldMetaDataItem { IS_VISIBLE = 1, TITUL = "Дата", TYPECONTROL = 3, FLDNAME = "datafield", IS_EDITED = 1, BASIC_FLD = 0 });
            result.Add(new FieldMetaDataItem { IS_VISIBLE = 1, TITUL = "Вывод справочных данных(фото)", TYPECONTROL = 6, FLDNAME = "textnoneditlabel", IS_EDITED = 1, BASIC_FLD = 0 });
            result.Add(new FieldMetaDataItem { IS_VISIBLE = 1, TITUL = "Добавление связей", TYPECONTROL = 5, FLDNAME = "numberconnect", IS_EDITED = 1, BASIC_FLD = 0 });
            result.Add(new FieldMetaDataItem { IS_VISIBLE = 1, TITUL = "Чек бокс", TYPECONTROL = 7, FLDNAME = "checkbox", IS_EDITED = 1, BASIC_FLD = 0 });


            return result;
        }



        #endregion Private methods


        #region IGridData Members

        public GridData GridData { get; set; }

        /// <summary>
        /// Список метаданныч по текущему паспорту 
        /// </summary>
        public List<FieldMetaDataItem> MetaDataList { get; private set; }


        #endregion


    }
}

