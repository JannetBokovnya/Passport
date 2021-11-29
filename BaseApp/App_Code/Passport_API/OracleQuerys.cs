using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App_Code.Passport_API
{
    /// <summary>
    /// Summary description for OracleQuerys
    /// </summary>
    public static class OracleQuerys
    {
        //App_Code/Passport_API/ServiceData.cs
        //для привязки выбираем мг и нить
        private static string getListMtQuery = "DB_API.passport_location_api.get_list_mt";
        private static string getPipeKeyOnMtQuery = "DB_API.passport_location_api.getpipekey_on_mt";

        private static string setContext = "DB_API.Passport_SL_API.setContext";

        //использовалось в private void ComboBoxParent_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        //private static string getAllParentQuery = "Passport_Silverlight_API.GetAllParent_f";
        // вызов этой функции  закомментировано в  private void NewDetalPassportButton_Click(object sender, RoutedEventArgs e)
        //private static string getCountParentQuery = "Passport_Silverlight_API.GetCountParent_f";

        // проверки возможности организации связи - кардинальность
        private static string getCheckOrganizationLink = "DB_API.PASSPORT_SL_API.checkLinkFilled";

        private static string getFullNameParentObjQuery = "DB_API.Passport_SL_API.getObjectInfo";  //  "Passport_Silverlight_API.GetFullNameParentObj_f";
       // private static string getFullNameParentObjQuery = "Passport_Silverlight_API.GetFullNameParentObj_f";
        //private static string getFullObjNameQuery = "Passport_Silverlight_API.GetFullObjName_f";
        //private static string getObjTypeAndNameQuery = "passport_silverlight_api.GetObjTypeAndName_f";
        //private static string getKeyParentQuery = "Passport_Silverlight_API.GetKey_Parent_f";
        //private static string getKeyEntityObjQuery = "Passport_Silverlight_API.GetKeyEntity_Obj_f";

      
       // private static string getNameObjQuery = "Passport_Silverlight_API.GetName_Obj_f";
        private static string getNameObjQuery = "DB_API.Passport_SL_API.getEntityName";


        //private static string getObjToolBarButtonsQuery = "Passport_Silverlight_API.getobjtoolbarbuttons_f";
        private static string getObjToolBarButtonsQuery = "DB_API.Passport_SL_API.getObjectToolbar";

        private static string getDragAndDroponForesterQuery = "DB_API.Passport_Silverlight_API.DragAndDroponForester_f";
        
       
        //private static string getEntityNamesQuery = "gis_meta.util.getentitynames";
        private static string getEntityNamesQuery = "DB_API.Passport_SL_API.getEntityList";

        //private static string getAllChildObjQuery = "Passport_Silverlight_API.GetAllChildObj_f";
        private static string getAllChildObjQuery = "DB_API.Passport_SL_API.getchildentities";

        //не используется
        //private static string getCountChildQuery = "Passport_Silverlight_API.GetCountChild_f";
        //не работают

        //ФУНКЦИЯ Passport_SL_API.getlinkedobject НЕ РАБОТАЕТ С КЛЮЧЯМИ objkey = 63902603 in_nfieldkey=652189
        //private static string getDataListRelationObjQuery = "Passport_Silverlight_API.GetDataListRelationObj_f";
        private static string getDataListRelationObjQuery = "DB_API.Passport_SL_API.Getlinkedobjects";

        //для горизонтальных связей, которые еще можно увязать
        private static string getObjForLinksQuery = "DB_API.Passport_SL_API.getObjectsForLink";

       // private static string getAllConnectObjQuery = "Passport_Silverlight_API.GetAllConnectObj_f";
        private static string getAllConnectObjQuery = "DB_API.Passport_SL_API.getentitiesforlink";

        //private static string saveLinkQuery = "Passport_Silverlight_API.SaveLink_f";
        //private static string deleteLinkQuery = "Passport_Silverlight_API.DeleteLink_f";

        private static string getObjectAccessLevelQuery = "DB_API.ext_api_metapi.getobjectaccesslevel";

       // private static string getMediaInfoListOnPasportKeyQuery = "Passport_Silverlight_API.GetMediaInfoListOnPasportKey";
        private static string getMediaInfoListOnPasportKeyQuery = "DB_API.Passport_SL_API.getMediaList";

       

        public static string GetObjForLinksQuery
        {
            get { return getObjForLinksQuery; }
        }

        public static string GetAllConnectObjQuery
        {
            get { return getAllConnectObjQuery; }
        }
        public static string GetMediaInfoListOnPasportKeyQuery
        {
            get { return getMediaInfoListOnPasportKeyQuery; }
        }
        public static string GetObjectAccessLevelQuery
        {
            get { return getObjectAccessLevelQuery; }
        }
        //public static string DeleteLinkQuery
        //{
        //    get { return deleteLinkQuery; }
        //}
        //public static string SaveLinkQuery
        //{
        //    get { return saveLinkQuery; }
        //}
        public static string GetDataListRelationObjQuery
        {
            get { return getDataListRelationObjQuery; }
        }
        //public static string GetCountChildQuery
        //{
        //    get { return getCountChildQuery; }
        //}
        public static string GetAllChildObjQuery
        {
            get { return getAllChildObjQuery; }
        }
        public static string GetEntityNamesQuery
        {
            get { return getEntityNamesQuery; }
        }
        //public static string GetKeyEntityObjQuery
        //{
        //    get { return getKeyEntityObjQuery; }
        //}
        public static string GetNameObjQuery
        {
            get { return getNameObjQuery; }
        }
        public static string GetDragAndDroponForesterQuery
        {
            get { return getDragAndDroponForesterQuery; }
        }
        //public static string GetKeyParentQuery
        //{
        //    get { return getKeyParentQuery; }
        //}
        public static string GetObjToolBarButtonsQuery
        {
            get { return getObjToolBarButtonsQuery; }
        }
        //public static string GetObjTypeAndNameQuery
        //{
        //    get { return getObjTypeAndNameQuery; }
        //}
        //public static string GetFullObjNameQuery
        //{
        //    get { return getFullObjNameQuery; }
        //}
        public static string GetFullNameParentObjQuery
        {
            get { return getFullNameParentObjQuery; }
        }

        //проверка кардинальности
        public static string GetCheckOrganizationLink
        {
            get { return getCheckOrganizationLink; }
        }
        // вызов этой функции  закомментировано в  private void NewDetalPassportButton_Click(object sender, RoutedEventArgs e)
        //public static string GetCountParentQuery
        //{
        //    get { return getCountParentQuery; }
        //}

        //public static string GetAllParentQuery
        //{
        //    get { return getAllParentQuery; }
        //}

        public static string GetPipeKeyOnMtQuery
        {
            get { return getPipeKeyOnMtQuery; }
        }
        public static string GetListMtQuery
        {
            get { return getListMtQuery; }
        }

        public static string SetContext
        {
            get { return setContext; }
        }
        
        //App_Code/Passport_API/Controls/ServiceData.cs
        //private static string getAttrControlQuery = "Passport_Silverlight_API.getattrcontrol_f";
        private static string getAttrControlQuery = "DB_API.Passport_SL_API.getAttrControl";

        public static string GetAttrControlQuery
        {
            get { return getAttrControlQuery; }
        }
        

        //App_Code/Passport_API/Dict/ServiceData.cs
       
        //private static string getDictOneEntityKeyQuery = "Passport_Silverlight_API.getdictonentitykey_f";
        private static string getDictOneEntityKeyQuery = "DB_API.Passport_SL_API.getNsiValuesForEntity";

        //private static string getOneDictQuery = "Passport_Silverlight_API.GetOneDict_f";
        private static string getOneDictQuery = "DB_API.Passport_SL_API.getNsiValues";

        //private static string saveNsiValueQuery = "Passport_Silverlight_API.SaveNsiValue_f";
        private static string saveNsiValueQuery = "DB_API.Passport_SL_API.editNsiValue";

        //private static string getNameNsiQuery = "Passport_Silverlight_API.GetNameNsi_f";
        private static string getNameNsiQuery = "DB_API.Passport_SL_API.getNsiName";

        public static string GetNameNsiQuery
        {
            get { return getNameNsiQuery; }
        }
        public static string SaveNsiValueQuery
        {
            get { return saveNsiValueQuery; }
        }
        public static string GetOneDictQuery
        {
            get { return getOneDictQuery; }
        }
        public static string GetDictOneEntityKeyQuery
        {
            get { return getDictOneEntityKeyQuery; }
        }


        //App_Code/Passport_API/Grid/ServiceData.cs
       // private static string getObjForEntityKeyQuery = "Passport_Silverlight_API.getobjforentitykey_f";
        private static string getObjForEntityKeyQuery = "DB_API.Passport_SL_API.getFieldValues";

        public static string GetObjForEntityKeyQuery
        {
            get { return getObjForEntityKeyQuery; }
        }


        //App_Code/Passport_API/Media/ServiceData.cs
       // private static string getAddMediaQuery = "Passport_Silverlight_API.Add_MediaTest";
        private static string getAddMediaQuery = "DB_API.Passport_SL_API.addMedia_p";

        //private static string dellObjQuery = "Passport_Silverlight_API.DellObj";
        private static string dellObjQuery = "DB_API.Passport_SL_API.deleteObject_Cascade";

        //private static string delMediaQuery = "Passport_Silverlight_API.DelMedia"; deleteMedia
        private static string delMediaQuery = "DB_API.Passport_SL_API.deleteMedia"; 

        //private static string getMediaListOnKeyPassportQuery = "Passport_Silverlight_API.GetMediaListOnKeyPasport_f";
        private static string getMediaListOnKeyPassportQuery = "DB_API.Passport_SL_API.getMediaListWithBlobs";

        //private static string getMediaBigOnKeyMediaQuery = "Passport_Silverlight_API.GetMediaBigOnKeyMedia_f";
        private static string getMediaBigOnKeyMediaQuery = "DB_API.Passport_SL_API.getFullsizeMedia";

        public static string GetMediaBigOnKeyMediaQuery
        {
            get { return getMediaBigOnKeyMediaQuery; }
        }
        public static string GetMediaListOnKeyPassportQuery
        {
            get { return getMediaListOnKeyPassportQuery; }
        }
        public static string DelMediaQuery
        {
            get { return delMediaQuery; }
        }
        public static string DellObjQuery
        {
            get { return dellObjQuery; }
        }
        public static string GetAddMediaQuery
        {
            get { return getAddMediaQuery; }
        }


        //App_Code/Passport_API/Meta/ServiceData.cs
        //private static string getMetaDataQuery = "Passport_Silverlight_API.GetMetadata_f";
        private static string getMetaDataQuery = "DB_API.Passport_SL_API.getmetadata";

        public static string GetMetaDataQuery
        {
            get { return getMetaDataQuery; }
        }


        //App_Code/Passport_API/Passport/ServiceData.cs
       // private static string savePassportQuery = "Passport_Silverlight_API.SavePassport_f";
        private static string savePassportQuery = "DB_API.Passport_SL_API.saveFieldValues";

        //private static string isShowErrorQuery = "Passport_Silverlight_API.isshowerror_f";
        private static string isShowErrorQuery = "DB_API.Passport_SL_API.isShowError";

        //private static string isShowBuCopyTreeQuery = "Passport_Silverlight_API.isShowBuCopyTree";
        private static string isShowBuCopyTreeQuery = "DB_API.Passport_SL_API.isShowBuCopyTree";

        private static string expObjectDataQuery = "gis_meta.meta_xml.exp_object_data";

       
       // private static string formLocationFormParamsQuery = "passport_location_api.formlocation_formparams";

        //private static string getLocationTypeByObjeTypeQuery = "passport_location_api.getlocationtypebyobjtype";
        //private static string getMetaLocationByObjeTypeQuery = "passport_location_api.getmetalocationbyobjtype";


        private static string getObjectLocationQueryAllField = "DB_API.passport_location_api.GetObjectLocation";

        //привязка редактирование и новая - без сохранения
        private static string getCreateLocationAllField = "DB_API.passport_location_api.checkLocation";

      
        //private static string buildPointLocationByPipiKmQuery = "passport_location_api.buildPointLocationByPipeKM";

        //private static string buildSegmentLocationByPipiKmQuery = "passport_location_api.buildSegmentLocationByPipeKM";
       // private static string buildPointLocationByPipiXyQuery = "passport_location_api.buildpointlocationbypipexy";
        //private static string buildSegmentLocationByPipiXyQuery = "passport_location_api.buildSegmentLocationByPipeXY";
        //private static string addLinkToPipeQuery = "passport_location_api.addlinktopipe";
        private static string deleteLocationQuery = "DB_API.passport_location_api.deleteLocation";
        private static string deleteLinkToPipeByKeyQuery = "DB_API.passport_location_api.deleteLinkToPipeByKey";
        
        
        //private static string getFullHierarchyPathToObjectQuery = "forester_obj.GetFullHierarchyPathToObject";
        private static string getFullHierarchyPathToObjectQuery = "DB_API.Passport_SL_API.GetFullHierarchyPathToObject";

        //private static string getDisplayLablesQuery = "Passport_Silverlight_API.getdisplaylabels";
        private static string getDisplayLablesQuery = "DB_API.Passport_SL_API.getDisplayLabels";

       // private static string copyPassportQuery = "Passport_Silverlight_API.CopyPassport_f";
        private static string copyPassportQuery = "DB_API.Passport_SL_API.CopyPassport_f";

        //private static string getPassportHistoryQuery = "Passport_Silverlight_API.GetPassportHistory_f";
        private static string getPassportHistoryQuery = "DB_API.Passport_SL_API.GetPassportHistory";

        //похоже что не используется
        private static string getPartTreeToObjectQuery = "DB_API.forester_obj.getparttreetoobject";

        public static string GetPartTreeToObjectQuery
        {
            get { return getPartTreeToObjectQuery; }
        }
        public static string GetFullHierarchyPathToObjectQuery
        {
            get { return getFullHierarchyPathToObjectQuery; }
        }
        public static string GetPassportHistoryQuery
        {
            get { return getPassportHistoryQuery; }
        }
        public static string CopyPassportQuery
        {
            get { return copyPassportQuery; }
        }
        public static string GetDisplayLablesQuery
        {
            get { return getDisplayLablesQuery; }
        }
        public static string DeleteLinkToPipeByKeyQuery
        {
            get { return deleteLinkToPipeByKeyQuery; }
        }
        //public static string DeleteLocationQuery
        //{
        //    get { return deleteLocationQuery; }
        //}
        //public static string AddLinkToPipeQuery
        //{
        //    get { return addLinkToPipeQuery; }
        ////}
        //public static string BuildSegmentLocationByPipiXyQuery
        //{
        //    get { return buildSegmentLocationByPipiXyQuery; }
        //}
        //public static string BuildPointLocationByPipiXyQuery
        //{
        //    get { return buildPointLocationByPipiXyQuery; }
        //}
        //public static string BuildSegmentLocationByPipiKmQuery
        //{
        //    get { return buildSegmentLocationByPipiKmQuery; }
        //}
        //public static string BuildPointLocationByPipiKmQuery
        //{
        //    get { return buildPointLocationByPipiKmQuery; }
        //}
        //public static string GetObjectLocationQuery
        //{
        //    get { return getObjectLocationQuery; }
        //}

        public static string GetObjectLocationQueryAllField
        {
            get { return getObjectLocationQueryAllField; }
        }

        public static string GetCreateLocationAllField
        {
            get { return getCreateLocationAllField; }
        }
        
        
        //public static string GetMetaLocationByObjeTypeQuery
        //{
        //    get { return getMetaLocationByObjeTypeQuery; }
        //}
        //public static string GetLocationTypeByObjeTypeQuery
        //{
        //    get { return getLocationTypeByObjeTypeQuery; }
        //}
        //public static string FormLocationFormParamsQuery
        //{
        //    get { return formLocationFormParamsQuery; }
        //}
        public static string ExpObjectDataQuery
        {
            get { return expObjectDataQuery; }
        }
        public static string IsShowBuCopyTreeQuery
        {
            get { return isShowBuCopyTreeQuery; }
        }
        public static string IsShowErrorQuery
        {
            get { return isShowErrorQuery; }
        }
        public static string SavePassportQuery
        {
            get { return savePassportQuery; }
        }
    }
}
