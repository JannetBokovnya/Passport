using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Browser;
using Media.Interfaces;
using Passpot.Business;
using Passpot.Controls;
using Services.ServiceReference;


namespace Passpot.Model
{
    public class PassportModel : ModelBase, IGridData
    {
        #region Private fields

        public string visiblebutonShema;
        public string visibleChildTabItem;
        private int _countParent;
        private string _nameObg;
        //private List<DataParentList> _selectParentList;
        //private List<DataOneParent> _selectOneDataParent;
        private string _getNameObjOnDelete;
        private bool _isShow;
        private string _nameObjDetail;
        private string _objTypeAndName;

        #endregion Private fields

        #region Ctor

        public PassportModel(MainModel mainModel)
        {
            MainModel = mainModel;
        }

        #endregion Ctor

        #region Properties

        public string PassportKey { get; private set; }
        public string ParentKey { get; private set; }
        private string _getCheckLinkOrganization;

        public int CountParent
        {
            get { return _countParent; }
            set
            {
                _countParent = value;
                FirePropertyChanged("CountParent");
            }
        }

        public string GetCheckLinkOrganization
        {
            get { return _getCheckLinkOrganization; }
            set
            {
                _getCheckLinkOrganization = value;
                FirePropertyChanged("GetCheckLinkOrganization");
            }
        }

        public string GetNameObjOnDelete
        {
            get { return _getNameObjOnDelete; }
            set 
            { 
                _getNameObjOnDelete = value;
                
            }
        }

       

        public string NameObj
        {
            get { return _nameObg; }
            set
            {
                _nameObg = value;
                FirePropertyChanged("NameObj");
            }
        }

        /// <summary>
        /// Список метаданныч по текущему паспорту 
        /// </summary>
        public List<FieldMetaDataItem> MetaDataList { get; private set; }

        public GridData GridData { get; private set; }

        public MainModel MainModel { get; private set; }

        public PassportOpenParam PassportOpenParams { get; private set; }

        public bool IsShow
        {
            get { return _isShow; }
            set
            {
                IsShowBusy = value;
                
            }
        }

        public string NameObjDetail
        {
            get { return _nameObjDetail; }
            set
            {
                _nameObjDetail = value;
                FirePropertyChanged("NameObjDetailP");
            }
        }

        public string ObjTypeAndName
        {
            get { return _objTypeAndName; }
            set
            {
                _objTypeAndName = value;
                FirePropertyChanged("ObjTypeAndNameP");
            }
        }

        

        #endregion Properties

        #region Public methods

        public void InitModel(PassportOpenParam passportOpenParams)  //(string passportKey)
        {
            //PassportKey = passportKey;
            PassportOpenParams = passportOpenParams;
            PassportKey = PassportOpenParams.EntityKey;

            //GetUserSession();
            //инфо по паренту паспорта 
            ParentKey = PassportOpenParams.ParentKey;
            StartGetPassportInfo(PassportOpenParams.ParentKey, GlobalContext.Context);


            ServiceDataClient.GetGridDataCompleted += OnGetGridDataCompleted;
            ServiceDataClient.GetGridDataAsync(PassportOpenParams.EntityKey, "0", "1", PassportOpenParams.ParentKey, GlobalContext.Context);   //(passportKey,"0","1");
        }

        public void StartGetPassportInfo(string inObjKey, string context)
        {
            ServiceDataClient.GetPassportInfoCompleted += GetPassportInfoCompleted;
            ServiceDataClient.GetPassportInfoAsync(inObjKey, context);
        }

        private void GetPassportInfoCompleted(object sender, GetPassportInfoCompletedEventArgs e)
        {
            ServiceDataClient.GetPassportInfoCompleted -= GetPassportInfoCompleted;
            if (e.Result.IsValid)
            {
                
                NameObjDetail = e.Result.PassportInfo.GetNameObj; //Название объекта по ключу
                ObjTypeAndName = e.Result.PassportInfo.NameEntityObj; //"Название типа объекта и объекта по ключу
               
            }
            else
            {

            }
        }

        void OnGetGridDataCompleted(object sender, GetGridDataCompletedEventArgs e)
        {
            MainModel.Report(e.Result.ErrorMessage);

            if (e.Error != null)
            {
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                //code.SessionEnd.Redirect();
            }
            else
            {
                
                if (e.Result.IsValid)
                {
                    GridData gridData = e.Result;

                    if (gridData != null)
                    {
                        GridData = gridData;

                        ////MetaDataList = GetMetaData(PassportKey);

                        if (gridData.Rows.Count == 0)
                        {

                            //FirePropertyChanged("ModelInited");
                            //FirePropertyChanged("GridDataCount");
                            MainModel.Report("PassportModel = " + e.Result.ErrorMessage);
                            ServiceDataClient.MetaDataCompleted += OnMetaDataCompleted;
                            ServiceDataClient.MetaDataAsync(PassportKey, "1", GlobalContext.Context, "0");
                            


                        }
                        else
                        {
                            //GetUserSession();
                            ServiceDataClient.MetaDataCompleted += OnMetaDataCompleted;
                            ServiceDataClient.MetaDataAsync(PassportKey, "1", GlobalContext.Context, "0");
                        }

                    }
                }
                else
                {
                    MainModel.Report("Ошибка при выводе данных!!! OnGetGridDataCompleted err = " + e.Result.ErrorMessage);
                }
            }
        }

        void OnMetaDataCompleted(object sender, MetaDataCompletedEventArgs e)
        {
            //IsShowBusy = false;
            MainModel.Report(e.Result.ErrorMessage);

            if (e.Result.IsValid)
            {

                //  e.Result.FieldMetaDataList

                MetaDataList = new List<FieldMetaDataItem>(e.Result.FieldMetaDataList);
                if (e.Result.FieldMetaDataList.Count != 0)
                {
                    //MetaDataList = GetMetaData(PassportKey);

                    FirePropertyChanged("ModelInited");
                }
                else
                {
                    MainModel.Report("для таблицы OnMetaDataCompleted  " + e.Result.ErrorMessage);
                    MainModel.Indicator(false);
                }
                
            }
            else
            {
                MainModel.Report("Ошибка метаданных табличного представления OnMetaDataCompleted err = " + e.Result.ErrorMessage);
            }

        }


        // вызов этой функции  закомментировано в  private void NewDetalPassportButton_Click(object sender, RoutedEventArgs e)
        //не используется
        //выборка которая считает парентов... если нет парентов - значит объект самый главный
        //тогда сразу вызываем новый паспорт   Model.MainModel.NeedOpenPassportDetail = "new";
        //иначе открываем по кнопке форму
        //public void StartGetCountParent(string PassportKey)
        //{
        //    //GetUserSession();
        //    ServiceDataClient.GetCountParentCompleted += GetCountParentCompleted;
        //    ServiceDataClient.GetCountParentAsync(PassportKey, GlobalContext.Context);
        //}

        //void GetCountParentCompleted(object sender, GetCountParentCompletedEventArgs e)
        //{
        //    CountParent = e.Result.CountParentOnEntityKey;
        //}

        /// <summary>
        /// получаем кардинальность по типу дочерней связи
        /// </summary>
        /// <param name="EntityKey"></param>
        /// <param name="ObjParentKey"></param>
        /// <param name="LinkType"></param>
        /// <param name="context"></param>
        public void GetKordinalnostLink(string EntityKey, string ObjParentKey, string LinkType,
                                        string context)
        {
            ServiceDataClient.GetCheckLinkCompleted += GetCheckLink;
            ServiceDataClient.GetCheckLinkAsync(EntityKey, ObjParentKey, LinkType, context);

        }

        void GetCheckLink(object sender, GetCheckLinkCompletedEventArgs e)
        {
            if (e.Result.IsValid)
            {
                GetCheckLinkOrganization =
                    e.Result.GetCheckOrganizationLinkOnkeyPassport_result.GetCheckOrganizationLinkOnkeyPassport;
            }
            else
            {
                MainModel.Report("Ошибка получения кардинальности GetKordinalnostLink err = " + e.Result.ErrorMessage);
            }
        }
        //закоментировано!!!! родитель не нужен!!!!
        //public void StartGetDataParent(string passportKey)
        //{

        //    //GetUserSession();
        //    ServiceDataClient.GetDataParentCompleted += GetDataParentCompleted;
        //    ServiceDataClient.GetDataParentAsync(passportKey, GlobalContext.Context);
        //}

        //void GetDataParentCompleted(object sender, GetDataParentCompletedEventArgs e)
        //{
        //    SelectParentList = new List<DataParentList>(e.Result.DataParentLists);
        //}


        //не актуально!!!!! для добавления нового паспорта не используем выбор парента!!!!
        //использовалось в private void ComboBoxParent_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        //public void StartGetDataOneParent(string keyEntityParent)
        //{
        //    //GetUserSession();
        //    ServiceDataClient.GetDataOneParentCompleted += GetDataOneParentCompleted;
        //    ServiceDataClient.GetDataOneParentAsync(keyEntityParent, "0", GlobalContext.Context);
        //}

        //void GetDataOneParentCompleted(object sender, GetDataOneParentCompletedEventArgs e)
        //{
        //    SelectOneParentList = new List<DataOneParent>(e.Result.DataOneParentList);
        //}

        public void StartGetNameObj(string entityKey)
        {
            //GetUserSession();
            ServiceDataClient.GetNameObjCompleted += GetNameObjCompleted;
            ServiceDataClient.GetNameObjAsync(entityKey, GlobalContext.Context);
        }

        void GetNameObjCompleted(object sender, GetNameObjCompletedEventArgs e)
        {
            if (e.Result.IsValid)
            {
                NameObj = e.Result.NameObjectOnEntityKey_result.NameObjectOnEntityKey;
            }
            else
            {
                MainModel.Report("Ошибка названия объекта GetNameObjCompleted err = " + e.Result.ErrorMessage);
            }
        }


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
            ChildWindowAttantion _popupWindowAttantion;
            if (e.Result.IsValid)
            {
                //разобраться! после удаления перечитівать паспорт?

                //ServiceDataClient.GetGridDataCompleted += OnGetGridDataCompleted;
                //ServiceDataClient.GetGridDataAsync(PassportOpenParams.EntityKey, "0", "1"); 
                //GetUserSession();
                ServiceDataClient.GetGridDataAsync(PassportOpenParams.EntityKey, "0", "1", PassportOpenParams.ParentKey, GlobalContext.Context);
                MainModel.FirePropertyChanged("EditPassportSaved");
                MainModel.Report("Паспорт удален");
            }
            else
            {
                MainModel.Report(e.Result.ErrorMessage);
                _popupWindowAttantion = new ChildWindowAttantion();
                _popupWindowAttantion.titleBox.Text = e.Result.ErrorMessage;
                _popupWindowAttantion.Show();
            }
        }


        public void NamepassportOnDelete(string passportKey)
        {
            ServiceDataClient.GetPassportInfoCompleted += ServiceDataClient_GetPassportInfoCompleted;
            ServiceDataClient.GetPassportInfoAsync(passportKey, GlobalContext.Context);
        }

        void ServiceDataClient_GetPassportInfoCompleted(object sender, GetPassportInfoCompletedEventArgs e)
        {
            ServiceDataClient.GetPassportInfoCompleted -= ServiceDataClient_GetPassportInfoCompleted;
            if (e.Result.IsValid)
            {
                GetNameObjOnDelete = e.Result.PassportInfo.GetNameObj;

                if (e.Result.PassportInfo.IsDeleteCurrentPassport == 0)
                {
                    FirePropertyChanged("IsCurrentPassportOnDelete");
                }
                else
                {
                    FirePropertyChanged("GetNameObjOnDelete");
                    
                }
                
            }
            else
            {
                MainModel.Report("Для удаления нельзя определить название объекта" + e.Result.ErrorMessage);
            }
        }

        //void ServiceDataClient_GetObjTypeAndNameCompleted(object sender, GetObjTypeAndNameCompletedEventArgs e)
        //{
        //    ServiceDataClient.GetObjTypeAndNameCompleted -= ServiceDataClient_GetObjTypeAndNameCompleted;
        //    if (e.Result.IsValid)
        //    {
        //        GetNameObjOnDelete = e.Result.NameEntity_and_ObjOnObjKey_result.NameEntity_and_ObjOnObjKey;
        //    }
        //    else
        //    {
        //        MainModel.Report("Для удаления нельзя определить название объекта" + e.Result.ErrorMessage);
        //    }
        //}


        #endregion Private methods


       
    }
}
