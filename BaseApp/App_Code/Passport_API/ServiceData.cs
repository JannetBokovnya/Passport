using System.Collections.Generic;
using System.Configuration;
using System.Data.OracleClient;
using System;
using System.Data;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Configuration;
using App_Code.Admin_module_API;
using App_Code.Passport_API;
using log4net;



[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public partial class ServiceData: IServiceData
{
    /// <summary>
    /// Блок глобальных переменных
    /// </summary>
    protected string con_str;//строка подключения к мета данным

    private static readonly ILog log_passport = LogManager.GetLogger(typeof(ServiceData).Name);

    public void SetContext (string context)
    {

        DataSet ds = new DataSet();
        DBConn.DBParam[] oip = new DBConn.DBParam[1];
        oip[0] = new DBConn.DBParam();
        oip[0].ParameterName = "in_nContext";
        oip[0].DbType = DBConn.DBTypeCustom.Number;
        oip[0].Direction = ParameterDirection.Input;
        oip[0].Value = Convert.ToDouble(context);

        DataOracleHelper.ExecNonQuery(OracleQuerys.SetContext, ref oip);

    }

    public DataMGList_result GetDataMGList(string context)
   {
       var result = new DataMGList_result();
       result.DataMGLists = new List<DataMGList>();

       

       try
       {

           DataSet ds = new DataSet();
           DataOracleHelper.ExecQuery(OracleQuerys.GetListMtQuery, ref ds);

           if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
           {
               foreach (DataRow row in ds.Tables[0].Rows)
               {

                   result.DataMGLists.Add(
                       new DataMGList { KEYMG = row["nkey"].ToString(), NAMEMG = row["cname"].ToString() });
               }
           }
           else
           {
               result.IsValid = false;
               result.ErrorMessage = string.Format("Нет данных...");
           }
       }
       catch (Exception ex)
       {
           log_passport.Error(ex);
           result.IsValid = false;
           result.ErrorMessage = ex.Message;
       }

       return result;
   }


    public DataNitList_result GetDataNitList(string inKeyMg, string context)
   {
       var result = new DataNitList_result();
       result.DataNitLists = new List<DataNitList>();

       #region TestValue
       //result.DataNitLists.Add(new DataNitList { KEYNIT = "1", NAMENIT = "Союз" });
       //result.DataNitLists.Add(new DataNitList { KEYNIT = "2", NAMENIT = "Союз2" });
       //result.DataNitLists.Add(new DataNitList { KEYNIT = "3", NAMENIT = "Союз3" });
       //result.DataNitLists.Add(new DataNitList { KEYNIT = "4", NAMENIT = "Союз4" });
       //result.DataNitLists.Add(new DataNitList { KEYNIT = "5", NAMENIT = "Союз5" });

       //result.DataNitLists.Add(new DataNitList { KEYNIT = "-1", NAMENIT = "--Выбрать--" });

       #endregion TestValue
       try
       {
           DBConn.DBParam[] oip = new DBConn.DBParam[1];
           oip[0] = new DBConn.DBParam();
           oip[0].ParameterName = "in_npipelinekey";
           oip[0].DbType = DBConn.DBTypeCustom.Int64;
           oip[0].Direction = ParameterDirection.Input;
           oip[0].Value = Convert.ToInt64(inKeyMg);

           DataSet ds = new DataSet();
           DataOracleHelper.ExecQuery(OracleQuerys.GetPipeKeyOnMtQuery, ref oip, ref ds);

           if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
           {
               foreach (DataRow row in ds.Tables[0].Rows)
               {

                   result.DataNitLists.Add(
                       new DataNitList { KEYNIT = row["nkey"].ToString(), NAMENIT = row["cname"].ToString() });
               }
           }
           else
           {
               result.IsValid = false;
               result.ErrorMessage = string.Format("Нет данных...");
           }
       }
       catch (Exception ex)
       {
           log_passport.Error(ex);
           result.IsValid = false;
           result.ErrorMessage = ex.Message;
       }


       return result;
   }


    //public NamePlaseState_result GetNamePlaseState(string inKeyPlaseState, string context)
    //{
    //    var result = new NamePlaseState_result();
    //    result.NameParentOnPlaseState_result = new NameParent();
    //    try
    //    {

    //        result.NameParentOnPlaseState_result.NameParentOnPlaseState = String.Empty;

    //        DBConn.DBParam[] oip = new DBConn.DBParam[9];

    //        oip[8] = new DBConn.DBParam();
    //        oip[8].ParameterName = "in_nCTX";
    //        oip[8].DbType = DBConn.DBTypeCustom.Number;
    //        oip[8].Direction = ParameterDirection.Input;
    //        oip[8].Value = Convert.ToDouble(context);

    //        oip[0] = new DBConn.DBParam();
    //        oip[0].ParameterName = "in_nObjectKey";
    //        oip[0].DbType = DBConn.DBTypeCustom.Int64;
    //        oip[0].Direction = ParameterDirection.Input;
    //        oip[0].Value = Convert.ToDouble(inKeyPlaseState);

    //        oip[2] = new DBConn.DBParam();
    //        oip[2].ParameterName = "out_nEntityKey";
    //        oip[2].DbType = DBConn.DBTypeCustom.Int64;
    //        oip[2].Direction = ParameterDirection.Output;

    //        oip[3] = new DBConn.DBParam();
    //        oip[3].ParameterName = "out_cEntityName";
    //        oip[3].DbType = DBConn.DBTypeCustom.String;
    //        oip[3].Size = 1000;
    //        oip[3].Direction = ParameterDirection.Output;

    //        oip[4] = new DBConn.DBParam();
    //        oip[4].ParameterName = "out_cObjectName";
    //        oip[4].DbType = DBConn.DBTypeCustom.String;
    //        oip[4].Size = 1000;
    //        oip[4].Direction = ParameterDirection.Output;

    //        oip[5] = new DBConn.DBParam();
    //        oip[5].ParameterName = "out_nParentObjectKey";
    //        oip[5].DbType = DBConn.DBTypeCustom.Int64;
    //        oip[5].Direction = ParameterDirection.Output;

    //        oip[6] = new DBConn.DBParam();
    //        oip[6].ParameterName = "out_nParentEntityKey";
    //        oip[6].DbType = DBConn.DBTypeCustom.Int64;
    //        oip[6].Direction = ParameterDirection.Output;

    //        oip[7] = new DBConn.DBParam();
    //        oip[7].ParameterName = "out_cParentEntityName";
    //        oip[7].DbType = DBConn.DBTypeCustom.String;
    //        oip[7].Size = 1000;
    //        oip[7].Direction = ParameterDirection.Output;

    //        oip[1] = new DBConn.DBParam();
    //        oip[1].ParameterName = "out_cparentobjectname";
    //        oip[1].DbType = DBConn.DBTypeCustom.String;
    //        oip[1].Size = 1000;
    //        oip[1].Direction = ParameterDirection.Output;



    //        DbConnAuth dbConnAuth = new DbConnAuth();
    //        DBConn.Conn connOra = dbConnAuth.connOra();

    //        connOra.ConnectionString(ConfigurationManager.ConnectionStrings["DBConn"].ToString());
    //        DataSet ds = connOra.ExecuteQuery(OracleQuerys.GetFullNameParentObjQuery, false, oip);

    //        string cParentEntityName = "";
    //        string cparentobjectname = "";

    //        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //        {
    //            foreach (DataRow row in ds.Tables[0].Rows)
    //            {
    //                if (row["Name"].ToString() == "out_cParentEntityName")
    //                {
    //                    cParentEntityName = row["Value"].ToString();
    //                    if (row["Value"].ToString() != "")
    //                    {
    //                        cParentEntityName = cParentEntityName + ". ";
    //                    }


    //                }

    //                if (row["Name"].ToString() == "out_cparentobjectname")
    //                {

    //                    cparentobjectname = row["Value"].ToString();
    //                }

    //            }

    //            result.NameParentOnPlaseState_result.NameParentOnPlaseState = cParentEntityName + cparentobjectname;
    //        }
    //        else
    //        {
    //            result.IsValid = false;
    //            result.ErrorMessage = string.Format("Нет данных...");
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        log_passport.Error(ex);
    //        result.IsValid = false;
    //        result.ErrorMessage = ex.Message;
    //    }


    //    return result;


    //}


    /// <summary>
    /// //////////////////////////
    /// </summary>
    /// <param name="inObjKey"></param>
    /// <param name="context"></param>
    /// <returns></returns>

    //public NameObjOnKey GetNameObj_OnObjKey(string inObjKey, string context)
    //{
    //    var result = new NameObjOnKey();
    //    result.NameObjOnObjKey_result = new NameObj_ObjKey();
    //    try
    //    {


    //        DBConn.DBParam[] oip = new DBConn.DBParam[9];

    //        oip[8] = new DBConn.DBParam();
    //        oip[8].ParameterName = "in_nCTX";
    //        oip[8].DbType = DBConn.DBTypeCustom.Number;
    //        oip[8].Direction = ParameterDirection.Input;
    //        oip[8].Value = Convert.ToDouble(context);

    //        oip[0] = new DBConn.DBParam();
    //        oip[0].ParameterName = "in_nObjectKey";
    //        oip[0].DbType = DBConn.DBTypeCustom.Int64;
    //        oip[0].Direction = ParameterDirection.Input;
    //        oip[0].Value = Convert.ToDouble(inObjKey);

    //        oip[2] = new DBConn.DBParam();
    //        oip[2].ParameterName = "out_nEntityKey";
    //        oip[2].DbType = DBConn.DBTypeCustom.Int64;
    //        oip[2].Direction = ParameterDirection.Output;

    //        oip[3] = new DBConn.DBParam();
    //        oip[3].ParameterName = "out_cEntityName";
    //        oip[3].DbType = DBConn.DBTypeCustom.String;
    //        oip[3].Size = 1000;
    //        oip[3].Direction = ParameterDirection.Output;

    //        oip[4] = new DBConn.DBParam();
    //        oip[4].ParameterName = "out_cObjectName";
    //        oip[4].DbType = DBConn.DBTypeCustom.String;
    //        oip[4].Size = 1000;
    //        oip[4].Direction = ParameterDirection.Output;

    //        oip[5] = new DBConn.DBParam();
    //        oip[5].ParameterName = "out_nParentObjectKey";
    //        oip[5].DbType = DBConn.DBTypeCustom.Int64;
    //        oip[5].Direction = ParameterDirection.Output;

    //        oip[6] = new DBConn.DBParam();
    //        oip[6].ParameterName = "out_nParentEntityKey";
    //        oip[6].DbType = DBConn.DBTypeCustom.Int64;
    //        oip[6].Direction = ParameterDirection.Output;

    //        oip[7] = new DBConn.DBParam();
    //        oip[7].ParameterName = "out_cParentEntityName";
    //        oip[7].DbType = DBConn.DBTypeCustom.String;
    //        oip[7].Size = 1000;
    //        oip[7].Direction = ParameterDirection.Output;

    //        oip[1] = new DBConn.DBParam();
    //        oip[1].ParameterName = "out_cparentobjectname";
    //        oip[1].DbType = DBConn.DBTypeCustom.String;
    //        oip[1].Size = 1000;
    //        oip[1].Direction = ParameterDirection.Output;



    //        DbConnAuth dbConnAuth = new DbConnAuth();
    //        DBConn.Conn connOra = dbConnAuth.connOra();

    //        connOra.ConnectionString(ConfigurationManager.ConnectionStrings["DBConn"].ToString());
    //        DataSet ds = connOra.ExecuteQuery(OracleQuerys.GetFullNameParentObjQuery, false, oip);


    //        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //        {
    //            foreach (DataRow row in ds.Tables[0].Rows)
    //            {
    //                if (row["Name"].ToString() == "out_cobjectname")
    //                {
    //                    result.NameObjOnObjKey_result.NameObjOnObjKey = row["Value"].ToString();
    //                }

    //            }
    //        }
    //        else
    //        {
    //            result.IsValid = false;
    //            result.ErrorMessage = string.Format("Нет данных...");
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        log_passport.Error(ex);
    //        result.IsValid = false;
    //        result.ErrorMessage = ex.Message;
    //    }


    //    return result;

    //}


    /// <summary>
    /// 
    /// </summary>
    /// <param name="inObjKey"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    //public NameEntity_and_ObjOnKey GetObjTypeAndName(string inObjKey, string context)
    //{
    //    var result = new NameEntity_and_ObjOnKey();
    //    result.NameEntity_and_ObjOnObjKey_result = new NameEntity_and_Obj_ObjKey();
    //    try
    //    {



    //        DBConn.DBParam[] oip = new DBConn.DBParam[9];

    //        oip[8] = new DBConn.DBParam();
    //        oip[8].ParameterName = "in_nCTX";
    //        oip[8].DbType = DBConn.DBTypeCustom.Number;
    //        oip[8].Direction = ParameterDirection.Input;
    //        oip[8].Value = Convert.ToDouble(context);

    //        oip[0] = new DBConn.DBParam();
    //        oip[0].ParameterName = "in_nObjectKey";
    //        oip[0].DbType = DBConn.DBTypeCustom.Int64;
    //        oip[0].Direction = ParameterDirection.Input;
    //        oip[0].Value = Convert.ToDouble(inObjKey);

    //        oip[2] = new DBConn.DBParam();
    //        oip[2].ParameterName = "out_nEntityKey";
    //        oip[2].DbType = DBConn.DBTypeCustom.Int64;
    //        oip[2].Direction = ParameterDirection.Output;

    //        oip[3] = new DBConn.DBParam();
    //        oip[3].ParameterName = "out_cEntityName";
    //        oip[3].DbType = DBConn.DBTypeCustom.String;
    //        oip[3].Size = 1000;
    //        oip[3].Direction = ParameterDirection.Output;

    //        oip[4] = new DBConn.DBParam();
    //        oip[4].ParameterName = "out_cObjectName";
    //        oip[4].DbType = DBConn.DBTypeCustom.String;
    //        oip[4].Size = 1000;
    //        oip[4].Direction = ParameterDirection.Output;

    //        oip[5] = new DBConn.DBParam();
    //        oip[5].ParameterName = "out_nParentObjectKey";
    //        oip[5].DbType = DBConn.DBTypeCustom.Int64;
    //        oip[5].Direction = ParameterDirection.Output;

    //        oip[6] = new DBConn.DBParam();
    //        oip[6].ParameterName = "out_nParentEntityKey";
    //        oip[6].DbType = DBConn.DBTypeCustom.Int64;
    //        oip[6].Direction = ParameterDirection.Output;

    //        oip[7] = new DBConn.DBParam();
    //        oip[7].ParameterName = "out_cParentEntityName";
    //        oip[7].DbType = DBConn.DBTypeCustom.String;
    //        oip[7].Size = 1000;
    //        oip[7].Direction = ParameterDirection.Output;

    //        oip[1] = new DBConn.DBParam();
    //        oip[1].ParameterName = "out_cparentobjectname";
    //        oip[1].DbType = DBConn.DBTypeCustom.String;
    //        oip[1].Size = 1000;
    //        oip[1].Direction = ParameterDirection.Output;



    //        DbConnAuth dbConnAuth = new DbConnAuth();
    //        DBConn.Conn connOra = dbConnAuth.connOra();

    //        connOra.ConnectionString(ConfigurationManager.ConnectionStrings["DBConn"].ToString());
    //        DataSet ds = connOra.ExecuteQuery(OracleQuerys.GetFullNameParentObjQuery, false, oip);



    //        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //        {
    //            foreach (DataRow row in ds.Tables[0].Rows)
    //            {
    //                if (row["Name"].ToString() == "out_cEntityName")
    //                {
    //                    result.NameEntity_and_ObjOnObjKey_result.NameEntity_and_ObjOnObjKey = row["Value"].ToString() + ". ";
    //                }


    //                if (row["Name"].ToString() == "out_cObjectName")
    //                {
    //                    result.NameEntity_and_ObjOnObjKey_result.NameEntity_and_ObjOnObjKey =
    //                        result.NameEntity_and_ObjOnObjKey_result.NameEntity_and_ObjOnObjKey + row["Value"].ToString();
    //                }

    //            }

    //        }
    //        else
    //        {
    //            result.IsValid = false;
    //            result.ErrorMessage = string.Format("Нет данных...");
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        log_passport.Error(ex);
    //        result.IsValid = false;
    //        result.ErrorMessage = ex.Message;
    //    }


    //    return result;
    //}


    /// <summary>
    /// 
    /// </summary>
    /// <param name="inObjKey"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    //public KeyParentOnObjKey GetKeyParent(string inObjKey, string context)
    //{
    //    var result = new KeyParentOnObjKey();
    //    result.KeyParentOnObjKey_result = new KeyParentOnObj();
    //    try
    //    {

    //        //string cFullNameParentObj = String.Empty;
    //        DBConn.DBParam[] oip = new DBConn.DBParam[9];

    //        oip[8] = new DBConn.DBParam();
    //        oip[8].ParameterName = "in_nCTX";
    //        oip[8].DbType = DBConn.DBTypeCustom.Number;
    //        oip[8].Direction = ParameterDirection.Input;
    //        oip[8].Value = Convert.ToDouble(context);

    //        oip[0] = new DBConn.DBParam();
    //        oip[0].ParameterName = "in_nObjectKey";
    //        oip[0].DbType = DBConn.DBTypeCustom.Int64;
    //        oip[0].Direction = ParameterDirection.Input;
    //        oip[0].Value = Convert.ToDouble(inObjKey);

    //        oip[2] = new DBConn.DBParam();
    //        oip[2].ParameterName = "out_nEntityKey";
    //        oip[2].DbType = DBConn.DBTypeCustom.Int64;
    //        oip[2].Direction = ParameterDirection.Output;

    //        oip[3] = new DBConn.DBParam();
    //        oip[3].ParameterName = "out_cEntityName";
    //        oip[3].DbType = DBConn.DBTypeCustom.String;
    //        oip[3].Size = 1000;
    //        oip[3].Direction = ParameterDirection.Output;

    //        oip[4] = new DBConn.DBParam();
    //        oip[4].ParameterName = "out_cObjectName";
    //        oip[4].DbType = DBConn.DBTypeCustom.String;
    //        oip[4].Size = 1000;
    //        oip[4].Direction = ParameterDirection.Output;

    //        oip[5] = new DBConn.DBParam();
    //        oip[5].ParameterName = "out_nParentObjectKey";
    //        oip[5].DbType = DBConn.DBTypeCustom.Int64;
    //        oip[5].Direction = ParameterDirection.Output;

    //        oip[6] = new DBConn.DBParam();
    //        oip[6].ParameterName = "out_nParentEntityKey";
    //        oip[6].DbType = DBConn.DBTypeCustom.Int64;
    //        oip[6].Direction = ParameterDirection.Output;

    //        oip[7] = new DBConn.DBParam();
    //        oip[7].ParameterName = "out_cParentEntityName";
    //        oip[7].DbType = DBConn.DBTypeCustom.String;
    //        oip[7].Size = 1000;
    //        oip[7].Direction = ParameterDirection.Output;

    //        oip[1] = new DBConn.DBParam();
    //        oip[1].ParameterName = "out_cparentobjectname";
    //        oip[1].DbType = DBConn.DBTypeCustom.String;
    //        oip[1].Size = 1000;
    //        oip[1].Direction = ParameterDirection.Output;



    //        DbConnAuth dbConnAuth = new DbConnAuth();
    //        DBConn.Conn connOra = dbConnAuth.connOra();

    //        connOra.ConnectionString(ConfigurationManager.ConnectionStrings["DBConn"].ToString());
    //        DataSet ds = connOra.ExecuteQuery(OracleQuerys.GetFullNameParentObjQuery, false, oip);

    //        //result.KeyParentOnObjKey_result.KeyParentObjKey = cFullNameParentObj;
    //        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //        {
    //            foreach (DataRow row in ds.Tables[0].Rows)
    //            {
    //                if (row["Name"].ToString() == "out_nparentobjectkey")
    //                {
    //                    result.KeyParentOnObjKey_result.KeyParentObjKey = row["Value"].ToString();
    //                }

    //            }
    //        }
    //        else
    //        {
    //            result.IsValid = false;
    //            result.ErrorMessage = string.Format("Нет данных...");
    //        }


    //    }

    //    catch (Exception ex)
    //    {
    //        log_passport.Error(ex);
    //        result.IsValid = false;
    //        result.ErrorMessage = ex.Message;
    //    }


    //    return result;


    //}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="objKey"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    //public KeyEntity_Obj_result GetKeyEntityObj(string objKey, string context)
    //{
    //    var result = new KeyEntity_Obj_result();
    //    result.KeyEntityOnKeyObj_result = new KeyEntity_Obj();
    //    try
    //    {

    //        //string cFullNameParentObj = String.Empty;

    //        DBConn.DBParam[] oip = new DBConn.DBParam[9];

    //        oip[8] = new DBConn.DBParam();
    //        oip[8].ParameterName = "in_nCTX";
    //        oip[8].DbType = DBConn.DBTypeCustom.Number;
    //        oip[8].Direction = ParameterDirection.Input;
    //        oip[8].Value = Convert.ToDouble(context);

    //        oip[0] = new DBConn.DBParam();
    //        oip[0].ParameterName = "in_nObjectKey";
    //        oip[0].DbType = DBConn.DBTypeCustom.Int64;
    //        oip[0].Direction = ParameterDirection.Input;
    //        oip[0].Value = Convert.ToDouble(objKey);

    //        oip[2] = new DBConn.DBParam();
    //        oip[2].ParameterName = "out_nEntityKey";
    //        oip[2].DbType = DBConn.DBTypeCustom.Int64;
    //        oip[2].Direction = ParameterDirection.Output;

    //        oip[3] = new DBConn.DBParam();
    //        oip[3].ParameterName = "out_cEntityName";
    //        oip[3].DbType = DBConn.DBTypeCustom.String;
    //        oip[3].Size = 1000;
    //        oip[3].Direction = ParameterDirection.Output;

    //        oip[4] = new DBConn.DBParam();
    //        oip[4].ParameterName = "out_cObjectName";
    //        oip[4].DbType = DBConn.DBTypeCustom.String;
    //        oip[4].Size = 1000;
    //        oip[4].Direction = ParameterDirection.Output;

    //        oip[5] = new DBConn.DBParam();
    //        oip[5].ParameterName = "out_nParentObjectKey";
    //        oip[5].DbType = DBConn.DBTypeCustom.Int64;
    //        oip[5].Direction = ParameterDirection.Output;

    //        oip[6] = new DBConn.DBParam();
    //        oip[6].ParameterName = "out_nParentEntityKey";
    //        oip[6].DbType = DBConn.DBTypeCustom.Int64;
    //        oip[6].Direction = ParameterDirection.Output;

    //        oip[7] = new DBConn.DBParam();
    //        oip[7].ParameterName = "out_cParentEntityName";
    //        oip[7].DbType = DBConn.DBTypeCustom.String;
    //        oip[7].Size = 1000;
    //        oip[7].Direction = ParameterDirection.Output;

    //        oip[1] = new DBConn.DBParam();
    //        oip[1].ParameterName = "out_cparentobjectname";
    //        oip[1].DbType = DBConn.DBTypeCustom.String;
    //        oip[1].Size = 1000;
    //        oip[1].Direction = ParameterDirection.Output;



    //        DbConnAuth dbConnAuth = new DbConnAuth();
    //        DBConn.Conn connOra = dbConnAuth.connOra();

    //        connOra.ConnectionString(ConfigurationManager.ConnectionStrings["DBConn"].ToString());
    //        DataSet ds = connOra.ExecuteQuery(OracleQuerys.GetFullNameParentObjQuery, false, oip);

    //        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //        {
    //            foreach (DataRow row in ds.Tables[0].Rows)
    //            {
    //                if (row["Name"].ToString() == "out_nentitykey")
    //                {
    //                    result.KeyEntityOnKeyObj_result.KeyEntityOnKeyObj = row["Value"].ToString();
    //                }

    //            }
    //        }
    //        else
    //        {
    //            result.IsValid = false;
    //            result.ErrorMessage = string.Format("Нет данных...");
    //        }


    //    }
    //    catch (Exception ex)
    //    {
    //        log_passport.Error(ex);
    //        result.IsValid = false;
    //        result.ErrorMessage = ex.Message;
    //    }


    //    return result;
    //}



    public PassportInfo_result GetPassportInfo(string inObjKey, string context)
    {
        var result = new PassportInfo_result();
        result.PassportInfo = new PassportInfo();

        try
        {
            DBConn.DBParam[] oip = new DBConn.DBParam[10];

            oip[8] = new DBConn.DBParam();
            oip[8].ParameterName = "in_nCTX";
            oip[8].DbType = DBConn.DBTypeCustom.Number;
            oip[8].Direction = ParameterDirection.Input;
            oip[8].Value = Convert.ToDouble(context);

            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nObjectKey";
            oip[0].DbType = DBConn.DBTypeCustom.Int64;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = Convert.ToInt64(inObjKey);

            oip[2] = new DBConn.DBParam();
            oip[2].ParameterName = "out_nEntityKey";
            oip[2].DbType = DBConn.DBTypeCustom.Int64;
            oip[2].Direction = ParameterDirection.Output;

            oip[3] = new DBConn.DBParam();
            oip[3].ParameterName = "out_cEntityName";
            oip[3].DbType = DBConn.DBTypeCustom.String;
            oip[3].Size = 1000;
            oip[3].Direction = ParameterDirection.Output;

            oip[4] = new DBConn.DBParam();
            oip[4].ParameterName = "out_cObjectName";
            oip[4].DbType = DBConn.DBTypeCustom.String;
            oip[4].Size = 1000;
            oip[4].Direction = ParameterDirection.Output;

            oip[5] = new DBConn.DBParam();
            oip[5].ParameterName = "out_nParentObjectKey";
            oip[5].DbType = DBConn.DBTypeCustom.Int64;
            oip[5].Direction = ParameterDirection.Output;

            oip[6] = new DBConn.DBParam();
            oip[6].ParameterName = "out_nParentEntityKey";
            oip[6].DbType = DBConn.DBTypeCustom.Int64;
            oip[6].Direction = ParameterDirection.Output;

            oip[7] = new DBConn.DBParam();
            oip[7].ParameterName = "out_cParentEntityName";
            oip[7].DbType = DBConn.DBTypeCustom.String;
            oip[7].Size = 1000;
            oip[7].Direction = ParameterDirection.Output;

            oip[1] = new DBConn.DBParam();
            oip[1].ParameterName = "out_cparentobjectname";
            oip[1].DbType = DBConn.DBTypeCustom.String;
            oip[1].Size = 1000;
            oip[1].Direction = ParameterDirection.Output;

            oip[9] = new DBConn.DBParam();
            oip[9].ParameterName = "out_nIsEditable";
            oip[9].DbType = DBConn.DBTypeCustom.Int64;
            oip[9].Direction = ParameterDirection.Output;
           


            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();

            connOra.ConnectionString(ConfigurationManager.ConnectionStrings["DBConn"].ToString());
            DataSet ds = connOra.ExecuteQuery(OracleQuerys.GetFullNameParentObjQuery, false, oip);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables["Parameters"].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables["Parameters"].Rows)
                {
                    if (row["Name"].ToString() == "out_nEntityKey")
                    {
                        result.PassportInfo.KeyEntityObj = row["Value"].ToString();
                    }

                    if (row["Name"].ToString() == "out_cEntityName")
                    {
                        result.PassportInfo.NameEntityObj = row["Value"].ToString();
                    }

                    if (row["Name"].ToString() == "out_nParentObjectKey")
                    {
                        result.PassportInfo.KeyParentOnkeyObj = row["Value"].ToString();
                    }

                    if (row["Name"].ToString() == "out_nParentEntityKey")
                    {
                        result.PassportInfo.ParentEntityKey = row["Value"].ToString();
                    }

                    if (row["Name"].ToString() == "out_cparentobjectname")
                    {
                        result.PassportInfo.NameParentOnPlaseState = row["Value"].ToString();
                    }

                    if (row["Name"].ToString() == "out_cObjectName")
                    {
                        result.PassportInfo.GetNameObj = row["Value"].ToString();
                    }

                    if (row["Name"].ToString() == "out_cParentEntityName")
                    {
                        result.PassportInfo.NameParentEntityKey = row["Value"].ToString();
                    }

                    if (row["Name"].ToString() == "out_nIsEditable")
                    {
                        string edit = String.Empty;
                        edit = row["Value"].ToString();
                        if (!String.IsNullOrEmpty(edit))
                        {
                            result.PassportInfo.IsEditedCurrentPasport = Convert.ToInt32(row["Value"]);
                        }
                        else
                        {
                            result.PassportInfo.IsEditedCurrentPasport = 1;
                        }
                        
                    }

                    //пока не сделана функция 
                    result.PassportInfo.IsDeleteCurrentPassport = 1; //1 - можно удалять редактировать
                    //result.PassportInfo.IsEditedCurrentPasport = 1;
                }
            }
            //else
            //{
            //    result.IsValid = false;
            //    result.ErrorMessage = string.Format("Нет данных...");
            //}

        }
        catch (Exception ex)
        {
            log_passport.Error(ex);
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }


        return result;
    }




    public HashPassword SetHashPassword(string in_cpass)
    {
        var result = new HashPassword();
        result.CurrentUser_OnLog_result = new HashPassword_OnLog();
        result.CurrentUser_OnLog_result.HashPassword_OnLogBase = Auth.Hashing(in_cpass);
        return result;
    }

    //для табличного(списочного) представления определяем название объекта
    public NameObj_result GetNameObj(string entityKey, string context)
    {
        var result = new NameObj_result();
        result.NameObjectOnEntityKey_result = new NameObj();

        try
        {
 
            string cnameObj = String.Empty;
            DBConn.DBParam[] oip = new DBConn.DBParam[3];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nEntityKey";
            oip[0].DbType = DBConn.DBTypeCustom.Int64;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = Convert.ToDouble(entityKey);

            oip[1] = new DBConn.DBParam();
            oip[1].ParameterName = "in_nCTX";
            oip[1].DbType = DBConn.DBTypeCustom.Number;
            oip[1].Direction = ParameterDirection.Input;
            oip[1].Value =Convert.ToDouble(context);

            oip[2] = new DBConn.DBParam();
            oip[2].ParameterName = "return_EntityName";
            oip[2].DbType = DBConn.DBTypeCustom.String;
            oip[2].Size = 1000;
            oip[2].Direction = ParameterDirection.Output;


            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();

            connOra.ConnectionString(ConfigurationManager.ConnectionStrings["DBConn"].ToString());
            DataSet ds = connOra.ExecuteQuery(OracleQuerys.GetNameObjQuery, false, oip);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row["Name"].ToString() == "return_EntityName")
                    {
                        result.NameObjectOnEntityKey_result.NameObjectOnEntityKey = row["Value"].ToString();    
                    }
                   
                }
            }
           
        }
        catch (Exception ex)
        {
            log_passport.Error(ex);
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }


        return result;
    }

    public GetCheckOrganizationLink_result GetCheckLink(string EntityKey, string ObjParentKey, string LinkType,
                                                        string context)
    {
        var result = new GetCheckOrganizationLink_result();
        result.GetCheckOrganizationLinkOnkeyPassport_result = new GetCheckOrganizationLink();

        try
        {
            string res = String.Empty;
            
            DBConn.DBParam[] oip = new DBConn.DBParam[4];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nEntityKey";
            oip[0].DbType = DBConn.DBTypeCustom.Int64;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = Convert.ToInt64(EntityKey);

            oip[1] = new DBConn.DBParam();
            oip[1].ParameterName = "in_nObjParentKey";
            oip[1].DbType = DBConn.DBTypeCustom.Int64;
            oip[1].Direction = ParameterDirection.Input;
            oip[1].Value = Convert.ToInt64(ObjParentKey);

            oip[2] = new DBConn.DBParam();
            oip[2].ParameterName = "in_nLinkType";
            oip[2].DbType = DBConn.DBTypeCustom.Int64;
            oip[2].Direction = ParameterDirection.Input;
            oip[2].Value = Convert.ToInt64(LinkType);

            oip[3] = new DBConn.DBParam();
            oip[3].ParameterName = "in_nCTX";
            oip[3].DbType = DBConn.DBTypeCustom.Number;
            oip[3].Direction = ParameterDirection.Input;
            oip[3].Value = Convert.ToDouble(context);


            DataOracleHelper.ExecQuery(OracleQuerys.GetCheckOrganizationLink, ref oip, ref res);


            double outres;

            if (double.TryParse(res, out outres))
            {
                result.GetCheckOrganizationLinkOnkeyPassport_result.GetCheckOrganizationLinkOnkeyPassport = res;
            }
            else
            {
                result.GetCheckOrganizationLinkOnkeyPassport_result.GetCheckOrganizationLinkOnkeyPassport = "";
                result.ErrorMessage = res;
            }
            ////oip[2] = new DBConn.DBParam();
            ////oip[2].ParameterName = "return_EntityName";
            ////oip[2].DbType = DBConn.DBTypeCustom.String;
            ////oip[2].Size = 1000;
            ////oip[2].Direction = ParameterDirection.Output;


            //DbConnAuth dbConnAuth = new DbConnAuth();
            //DBConn.Conn connOra = dbConnAuth.connOra();

            //connOra.ConnectionString(ConfigurationManager.ConnectionStrings["DBConn"].ToString());
            //DataSet ds = connOra.ExecuteQuery(OracleQuerys.GetNameObjQuery, false, oip);

            //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    foreach (DataRow row in ds.Tables[0].Rows)
            //    {
            //        if (row["Name"].ToString() == "return_EntityName")
            //        {
            //            result.NameObjectOnEntityKey_result.NameObjectOnEntityKey = row["Value"].ToString();
            //        }

            //    }
            //}

        }
        catch (Exception ex)
        {
            log_passport.Error(ex);
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }
    
    






    public FielButtonData GetButtonToolbar(string passportkey, string buttonTollBar, string context)
    {

        var result = new FielButtonData();
        result.FielButtonDataItemList = new List<FielButtonDataItem>();

        try
        {
   
            string tt = "";

            DataSet ds = new DataSet();
            DBConn.DBParam[] oip = new DBConn.DBParam[2];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nobjectkey";
            oip[0].DbType = DBConn.DBTypeCustom.Int64;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = Convert.ToInt64(passportkey);

            oip[1] = new DBConn.DBParam();
            oip[1].ParameterName = "in_nCTX";
            oip[1].DbType = DBConn.DBTypeCustom.Number;
            oip[1].Direction = ParameterDirection.Input;
            oip[1].Value = Convert.ToDouble(context);

            DataOracleHelper.ExecQuery(OracleQuerys.GetObjToolBarButtonsQuery, ref oip, ref ds);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    {
                        result.FielButtonDataItemList.Add(new FielButtonDataItem
                        {
                            NameJpg = row["cImageName"].ToString(),
                            ContentJpg = row["cComment"].ToString(),
                            CAttribute = row["cAttribute"].ToString(),
                            CEvent = row["cEvent"].ToString(),
                            CParams = row["cParams"].ToString(),
                            NButtonType = row["nButtonType"].ToString()
                            
                        });
                    }

                    
                }
            }

        }
        catch (Exception ex)
        {
            log_passport.Error(ex);
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }
        return result;

    }


    public KeyParentOnUpdateTree UpdateParent(string passportKey, string parentKey, string context)
    {
        var result = new KeyParentOnUpdateTree();
        result.KeyParent_OnUpdateTree_result = new KeyParent_OnUpdate();
        try
        {
  
            int onResult = 0;
            DBConn.DBParam[] oip = new DBConn.DBParam[2];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "i_nPassportKey";
            oip[0].DbType = DBConn.DBTypeCustom.Int64;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = Convert.ToInt64(passportKey);

            oip[1] = new DBConn.DBParam();
            oip[1].ParameterName = "i_nParentKey";
            oip[1].DbType = DBConn.DBTypeCustom.Int64;
            oip[1].Direction = ParameterDirection.Input;
            oip[1].Value = Convert.ToInt64(parentKey);

            DataOracleHelper.ExecQuery(OracleQuerys.GetDragAndDroponForesterQuery, ref oip, ref onResult);
            result.KeyParent_OnUpdateTree_result.KeyParent_OnUpdateTree = onResult.ToString();

        }
        catch (Exception ex)
        {
            log_passport.Error(ex);
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }
        return result;
    }

    public KeyparentofchildTree UpdateParenttoChild(string passportKey, string parentKey, string context)
    {
        var result = new KeyparentofchildTree();
        result.Keyparentofchild_result = new Keyparentofchild();
        try
        {
            //by Gaitov (17.09.2012)

            //OracleCommand cmd = DataOracleHelper.GetStoredProcCommand("util.isentityparentofchild");
            //cmd.Parameters.AddWithValue("i_nchildentitykey", passportKey);
            //cmd.Parameters.AddWithValue("i_nparententitykey", parentKey);
            //cmd.Parameters.Add("onResult", OracleType.Number).Direction = ParameterDirection.Output;



            //DataOracleHelper.ExecQuery(cmd);
        }
        catch (Exception ex)
        {
            log_passport.Error(ex);
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }
        return result;
    }

    public KeyonShemaArcgis ReturnKeyObjOnShema(string tsdiagram, string tsobject, string context)
    {
        var result = new KeyonShemaArcgis();
        result.KeyonShema_result = new KeyonShema();
        try
        {
            //by Gaitov (17.09.2012)

            //OracleCommand cmd = DataOracleHelper.GetStoredProcCommand("gis_meta.topo_util.getobjkeybytechgid");
            //cmd.Parameters.AddWithValue("in_nTSKey", tsdiagram);
            //cmd.Parameters.AddWithValue("in_nGID", tsobject);
            //cmd.Parameters.Add("out_nobjkey", OracleType.VarChar, 100).Direction = ParameterDirection.Output;



            //DataOracleHelper.ExecQuery(cmd);
            //result.KeyonShema_result.KeyonShema_arcgis = cmd.Parameters["out_nobjkey"].Value.ToString();
        }
        catch (Exception ex)
        {
            log_passport.Error(ex);
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }
        return result;

    }

    public KeyonKartaObjList ReturnVisibleObjOnKarta(string passportKey, string context)
    {
        var result = new KeyonKartaObjList();
        result.KeyonKartaList_result = new List<KeyonKartaList>();
        try
        {
            //by Gaitov (17.09.2012)

            //OracleCommand cmd = DataOracleHelper.GetStoredProcCommand("gis_data.link_api.Test_GetGeomByObjOnTS");
            //cmd.Parameters.AddWithValue("(in_nObjKey", passportKey);

            //cmd.Parameters.Add("out_cGeomKey", OracleType.VarChar, 100).Direction = ParameterDirection.Output;
            //cmd.Parameters.Add("out_nTS", OracleType.Number).Direction = ParameterDirection.Output;
            //DataOracleHelper.ExecQuery(cmd);
            //result.KeyonKartaList_result.Add(
            //    new KeyonKartaList { cGeomKey = cmd.Parameters["out_cGeomKey"].Value.ToString(), nTS = cmd.Parameters["out_nTS"].Value.ToString() });


        }
        catch (Exception ex)
        {
            log_passport.Error(ex);
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }
        return result;

    }


    public DataEntityList_result GetDataAllEntityObj(string context)
    {
        var result = new DataEntityList_result();
        result.DataEntityLists = new List<DataEntityList>();
        try
        {
           
            DataSet ds = new DataSet();
            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nCTX";
            oip[0].DbType = DBConn.DBTypeCustom.Number;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value =Convert.ToDouble(context);

            DataOracleHelper.ExecQuery(OracleQuerys.GetEntityNamesQuery, ref oip, ref ds);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    result.DataEntityLists.Add(
                        new DataEntityList { KEYENTITY = row["NENTITYKEY"].ToString(), VALUEENTITY = row["CENTITYNAME"].ToString() });
                        
                }
            }
            else
            {
                result.IsValid = false;
                result.ErrorMessage = string.Format("Нет данных...");
            }
        }
        catch (Exception ex)
        {
            log_passport.Error(ex);
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }

    //-------------------------------------------------------------

    //----------------чайлд------------------------------------------

    public DataChildList_result GetDataChild(string entityKey, string context)
    {
        var result = new DataChildList_result();
        result.DataChildLists = new List<DataChildList>();
        try
        {
          
            DBConn.DBParam[] oip = new DBConn.DBParam[2];
            oip[0] = new DBConn.DBParam();
            //oip[0].ParameterName = "inEntityKey";in_nentitykey
            oip[0].ParameterName = "in_nentitykey";
            oip[0].DbType = DBConn.DBTypeCustom.Int64;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = Convert.ToInt64(entityKey);

            oip[1] = new DBConn.DBParam();
            oip[1].ParameterName = "in_nCTX";
            oip[1].DbType = DBConn.DBTypeCustom.Number;
            oip[1].Direction = ParameterDirection.Input;
            oip[1].Value = Convert.ToDouble(context);


            DataSet ds = new DataSet();
            DataOracleHelper.ExecQuery(OracleQuerys.GetAllChildObjQuery, ref oip, ref ds);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    result.DataChildLists.Add(
                        new DataChildList { ENTITYKEYCHILDS = row["nentitykey"].ToString(), NAMECHILDS = row["centityname"].ToString() });
                }
            }
            //else
            //{
            //    result.IsValid = false;
            //    result.ErrorMessage = string.Format("Нет данных, ключ = {0}", entityKey);
            //}
        }
        catch (Exception ex)
        {
            log_passport.Error(ex);
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }

        return result;


    }

    public CountChild_result GetCountChild(string entityKey, string context)
    {
        var result = new CountChild_result();
        result.CountChildOnEntityKey_result = new CountChild();
        result.CountChildOnEntityKey_result.CountChildOnEntityKey = 0;

        try
        {
       
            //int iCountChild = 0;
            //DBConn.DBParam[] oip = new DBConn.DBParam[1];
            //oip[0] = new DBConn.DBParam();
            //oip[0].ParameterName = "inEntityKey";
            //oip[0].DbType = DBConn.DBTypeCustom.Int32;
            //oip[0].Direction = ParameterDirection.Input;
            //oip[0].Value = Convert.ToInt32(entityKey);

            //DataOracleHelper.ExecQuery(OracleQuerys.GetCountChildQuery, ref oip, ref iCountChild);
            //result.CountChildOnEntityKey_result.CountChildOnEntityKey = Convert.ToInt32(iCountChild);
        }
        catch (Exception ex)
        {
            log_passport.Error(ex);
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }

    //---------------------------------------------------------
    //--------связи-------------------------------------------------

    public List<DataLinkList> GetDataLink(string entityKey, string context)
    {
        var result = new List<DataLinkList>();
        return result;
    }

    //получаем привязанные связи паспорта(горизонтальные связи)
    public DataListRelationObj GetDataRelationObj(string inObjkey, string keyFld, string context)
    {
        var result = new DataListRelationObj();
        result.DataListRelationObjList = new List<DataListRelationObjItems>();



        try
        {
          
            DataSet ds = new DataSet();
            DBConn.DBParam[] oip = new DBConn.DBParam[3];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nobjectkey";
            oip[0].DbType = DBConn.DBTypeCustom.Int64;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = Convert.ToInt64(inObjkey);

            oip[1] = new DBConn.DBParam();
            oip[1].ParameterName = "in_cfieldname";
            oip[1].DbType = DBConn.DBTypeCustom.VarChar2;
            oip[1].Direction = ParameterDirection.Input;
            oip[1].Value = keyFld;

            oip[2] = new DBConn.DBParam();
            oip[2].ParameterName = "in_nCTX";
            oip[2].DbType = DBConn.DBTypeCustom.Number;
            oip[2].Direction = ParameterDirection.Input;
            oip[2].Value = Convert.ToDouble(context);


            DataOracleHelper.ExecQuery(OracleQuerys.GetDataListRelationObjQuery, ref oip, ref ds);


            foreach (DataRow row in ds.Tables[0].Rows)
            {

                result.DataListRelationObjList.Add(
                new DataListRelationObjItems { KeyObj = row["nObjectKey"].ToString(), NameEntity = row["nEnityKey"].ToString(), NameObj = row["cObjectName"].ToString() });
                 //row["nEnityKey"]
            }
            //}
            //else
            //{
            //    result.IsValid = false;
            //    result.ErrorMessage = string.Format("Нет данных, ключ = {0}", inObjkey);
            //}

        }
        catch (Exception ex)
        {
            log_passport.Error(ex);
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }
        return result;
    }

    public KeyRelationOnSave SaveRelationObj(string inObjkey, string keyFld, string keyRelation, string context)
    {
        var result = new KeyRelationOnSave();
        result.KeyRelation_OnSaveObj_result = new KeyRelation_OnSave();

        try
        {
            //int onRes = 0;
            //DBConn.DBParam[] oip = new DBConn.DBParam[3];
            //oip[0] = new DBConn.DBParam();
            //oip[0].ParameterName = "inObjKey";
            //oip[0].DbType = DBConn.DBTypeCustom.Int32;
            //oip[0].Direction = ParameterDirection.Input;
            //oip[0].Value = int.Parse(inObjkey);

            //oip[1] = new DBConn.DBParam();
            //oip[1].ParameterName = "inFieldKey";
            //oip[1].DbType = DBConn.DBTypeCustom.Int32;
            //oip[1].Direction = ParameterDirection.Input;
            //oip[1].Value = int.Parse(keyFld);

            //oip[2] = new DBConn.DBParam();
            //oip[2].ParameterName = "inLinkObj";
            //oip[2].DbType = DBConn.DBTypeCustom.Int32;
            //oip[2].Direction = ParameterDirection.Input;
            //oip[2].Value = int.Parse(keyRelation);

            //DataOracleHelper.ExecQuery(OracleQuerys.SaveLinkQuery, ref oip, ref onRes);
            //result.KeyRelation_OnSaveObj_result.KeyRelation_OnSaveObj = onRes.ToString();
        }
        catch (Exception ex)
        {
            log_passport.Error(ex);
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }
        return result;

    }
    //!!!!! не используется переведено на сохранение всего паспорта с горизонтальными связями
    public KeyRelationOnDel DelRelationObj(string inObjkey, string keyFld, string keyRelation, string context)
    {
        var result = new KeyRelationOnDel();
        result.KeyRelation_OnDelObj_result = new KeyRelation_OnDel();

        try
        {
 
            //int onRes = 0;
            //DBConn.DBParam[] oip = new DBConn.DBParam[3];
            //oip[0] = new DBConn.DBParam();
            //oip[0].ParameterName = "inObjKey";
            //oip[0].DbType = DBConn.DBTypeCustom.Int32;
            //oip[0].Direction = ParameterDirection.Input;
            //oip[0].Value = int.Parse(inObjkey);

            //oip[1] = new DBConn.DBParam();
            //oip[1].ParameterName = "inFieldKey";
            //oip[1].DbType = DBConn.DBTypeCustom.Int32;
            //oip[1].Direction = ParameterDirection.Input;
            //oip[1].Value = int.Parse(keyFld);

            //oip[2] = new DBConn.DBParam();
            //oip[2].ParameterName = "inLinkObj";
            //oip[2].DbType = DBConn.DBTypeCustom.Int32;
            //oip[2].Direction = ParameterDirection.Input;
            //oip[2].Value = int.Parse(keyRelation);

            //DataOracleHelper.ExecQuery(OracleQuerys.DeleteLinkQuery, ref oip, ref onRes);
            //result.KeyRelation_OnDelObj_result.KeyRelation_OnDelObj = onRes.ToString();

        }
        catch (Exception ex)
        {
            log_passport.Error(ex);
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }
        return result;


    }

    public AccessLevelObjKey ReturnAccessObj(string passportKey, string context)
    {
        var result = new AccessLevelObjKey();
        result.AccessLevel_OnKey_result = new AccessLevel_OnKey();

        try
        {
         
            int onRes = 0;
            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nObjKey";
            oip[0].DbType = DBConn.DBTypeCustom.Int64;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = Convert.ToInt64(passportKey);

            DataOracleHelper.ExecQuery(OracleQuerys.GetObjectAccessLevelQuery, ref oip, ref onRes);
            result.AccessLevel_OnKey_result.AccessLevel = onRes.ToString();

        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }
        return result;
    }


    public ButtonNKTObjKey ReturnVisibleNKTObj(string passportKey, string context)
    {
        var result = new ButtonNKTObjKey();
        result.ButtonNKT_OnKet_result = new ButtonNKT_OnKey();

        try
        {
            //by Gaitov (17.09.2012)

            //OracleCommand cmd = DataOracleHelper.GetStoredProcCommand("passport_silverlight_api.isShowNKTbutton");
            //cmd.Parameters.AddWithValue("inObjKey", int.Parse(passportKey).ToString());
            //cmd.Parameters.Add("onShowNKTbutton", OracleType.Number).Direction = ParameterDirection.Output;

            //DataOracleHelper.ExecQuery(cmd);

            //result.ButtonNKT_OnKet_result.ButtonNKTVisible = cmd.Parameters["onShowNKTbutton"].Value.ToString();

        }
        catch (Exception ex)
        {
            log_passport.Error(ex);
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }


    public DataMediaAttribute_result GetMediaAttribute(string passpoerKey, string context)
    {
        var result = new DataMediaAttribute_result();
        result.DataMediaAttributeList = new List<DataMediaAttribute>();
        try
        {
         
            DataSet ds = new DataSet();
            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam();
           // oip[0].ParameterName = "inObjKey";
            oip[0].ParameterName = "in_nObjectKey";
            oip[0].DbType = DBConn.DBTypeCustom.String;
            oip[0].Size = 100;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = int.Parse(passpoerKey);

            oip[1] = new DBConn.DBParam();
            oip[1].ParameterName = "in_nCTX";
            oip[1].DbType = DBConn.DBTypeCustom.Number;
            oip[1].Direction = ParameterDirection.Input;
            oip[1].Value = Convert.ToDouble(context);

            DataOracleHelper.ExecQuery(OracleQuerys.GetMediaInfoListOnPasportKeyQuery, ref oip, ref ds);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    result.DataMediaAttributeList.Add(
                        new DataMediaAttribute { type_media = row["nMediaType"].ToString(), comment_media = row["cComment"].ToString(), name_file = row["cFileName"].ToString() });
                }
            }
            else
            {
                result.IsValid = false;
                result.ErrorMessage = string.Format("Нет данных GetMediaInfoListOnPasportKey, ключ = {0}", passpoerKey);
            }

        }
        catch (Exception ex)
        {
            log_passport.Error(ex);
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }
        return result;

    }

    public DataConnectList_result GetDataConnect(string entityKey, string fldName, string context)
    {
        var result = new DataConnectList_result();
        result.DataConnectLists = new List<DataConnectList>();
        try
        {
          
            DBConn.DBParam[] oip = new DBConn.DBParam[3];
            oip[0] = new DBConn.DBParam();
            //oip[0].ParameterName = "inEntityKey";
            oip[0].ParameterName = "in_nentitykey";
            oip[0].DbType = DBConn.DBTypeCustom.Int64;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = Convert.ToInt64(entityKey);

            oip[1] = new DBConn.DBParam();
            //oip[1].ParameterName = "inFldname";
            oip[1].ParameterName = "in_cfieldname";
            oip[1].DbType = DBConn.DBTypeCustom.VarChar2;
            //oip[1].Size = 100;
            oip[1].Direction = ParameterDirection.Input;
            oip[1].Value = fldName;

            oip[2] = new DBConn.DBParam();
            oip[2].ParameterName = "in_nCTX";
            oip[2].DbType = DBConn.DBTypeCustom.Number;
            oip[2].Direction = ParameterDirection.Input;
            oip[2].Value = Convert.ToDouble(context);
            

            DataSet ds = new DataSet();
            DataOracleHelper.ExecQuery(OracleQuerys.GetAllConnectObjQuery, ref oip, ref ds);
            
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    result.DataConnectLists.Add(
                       // new DataConnectList { KEYCONNECT = row["entitykey"].ToString(), NAMECONNECT = row["description"].ToString() });
                        new DataConnectList { KEYCONNECT = row["nenitykey"].ToString(), NAMECONNECT = row["cenityname"].ToString() });
                }
            }
            else
            {
                result.IsValid = false;
                result.ErrorMessage = string.Format("Нет данных, ключ = {0}", entityKey);
            }
        }
        catch (Exception ex)
        {
            log_passport.Error(ex);
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }

        return result;


    }

    public DataListLinkedObjects GetListLinkedObjects(string objectkey, string entitykey, 
                                                      string fieldname, string linkedentitykey, 
                                                      string linkedobjects, string context)
    {
        var result = new DataListLinkedObjects();
        result.DataListLinkedObjectsList = new List<DataListLinkedObjectsItems>();
        try
        {
             //SetContext(context);
             DBConn.DBParam[] oip = new DBConn.DBParam[6];
             oip[0] = new DBConn.DBParam();
             oip[0].ParameterName = "in_nobjectkey";
             oip[0].DbType = DBConn.DBTypeCustom.Int64;
             oip[0].Direction = ParameterDirection.Input;
             oip[0].Value = Convert.ToInt64(objectkey);

             oip[1] = new DBConn.DBParam(); 
             oip[1].ParameterName = "in_nentitykey";
             oip[1].DbType = DBConn.DBTypeCustom.Int64;
             oip[1].Direction = ParameterDirection.Input;
             oip[1].Value = Convert.ToInt64(entitykey);

             oip[2] = new DBConn.DBParam();
             oip[2].ParameterName = "in_cfieldname";
             oip[2].DbType = DBConn.DBTypeCustom.VarChar2;
             oip[2].Direction = ParameterDirection.Input;
             oip[2].Value = fieldname;

             oip[3] = new DBConn.DBParam();
             oip[3].ParameterName = "in_nlinkedentitykey";
             oip[3].DbType = DBConn.DBTypeCustom.Int64;
             oip[3].Direction = ParameterDirection.Input;
             oip[3].Value = Convert.ToInt64(linkedentitykey);

             oip[4] = new DBConn.DBParam();
             oip[4].ParameterName = "in_clinkedobjects";
             oip[4].DbType = DBConn.DBTypeCustom.VarChar2;
             oip[4].Direction = ParameterDirection.Input;
             oip[4].Value = linkedobjects;

             oip[5] = new DBConn.DBParam();
             oip[5].ParameterName = "in_nCTX";
             oip[5].DbType = DBConn.DBTypeCustom.Number;
             oip[5].Direction = ParameterDirection.Input;
             oip[5].Value = Convert.ToDouble(context);


             DataSet ds = new DataSet();
             DataOracleHelper.ExecQuery(OracleQuerys.GetObjForLinksQuery, ref oip, ref ds);

             //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
             //{}
             //}
             //else
             //{
             //    result.IsValid = false;
             //    result.ErrorMessage = string.Format("Нет данных, ключ = {0}", inObjkey);
             //}


             foreach (DataRow row in ds.Tables[0].Rows)
             {

                 result.DataListLinkedObjectsList.Add
                     (new DataListLinkedObjectsItems
                     {EntityKey = row["nenitykey"].ToString(), 
                      ObjKey = row["nobjectkey"].ToString(),
                      ObjName = row["cobjectname"].ToString()

                 });
              
             }
           

        }
        catch (Exception ex)
        {
            log_passport.Error(ex);
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }


        return result;

    }
    #region OldCode
    //---------------------------------------------------------

    //-------------логон в базу
    //public CurrentUser SetCurrentSession(string in_clogin, string in_cpass)
    //{

    //    var result = new CurrentUser();
    //    result.CurrentUser_OnLog_result = new CurrentUser_OnLog();


    //    try
    //    {

    //        OracleCommand cmd = DataOracleHelper.GetStoredProcCommand("gis_admin.gis_login");
    //        cmd.Parameters.AddWithValue("in_clogin", in_clogin);
    //        cmd.Parameters.AddWithValue("in_cpass", in_cpass);
    //        cmd.Parameters.Add("result", OracleType.VarChar, 100).Direction = ParameterDirection.ReturnValue;
    //        DataOracleHelper.ExecQuery(cmd);
    //        string res = cmd.Parameters["result"].Value.ToString();

    //        result.CurrentUser_OnLog_result.CurrentUser_OnLogBase = cmd.Parameters["result"].Value.ToString();
    //    }
    //    catch (Exception ex)
    //    {

    //        log.Error(ex);
    //        result.IsValid = false;
    //        result.ErrorMessage = ex.Message;
    //    }

    //    return result;



    //}
    //-------------------end логон в базу

    
}

//работа с файлом на диске
//thumb.Name = "Aasdadfad";
//thumb.Comment = "Коментарии";
//thumb.Key = 1111;

//TODO: please change it at reading from database
// string path = @"E:\All\pic\Космос\habbl_18.jpg"; //@"C:\2010_1 001.jpg";

//FileStream fileStream = new FileStream(path, FileMode.Open);
//BinaryReader br = new BinaryReader(fileStream);
//long numBytes = new FileInfo(path).Length;
//byte[] fileBytes = br.ReadBytes((int)numBytes);
//thumb.Data = fileBytes;
//br.Close();

//result.Thumbnails.Add(thumb);


//FileStream fileStream = new FileStream(path, FileMode.Open);
//BinaryReader br = new BinaryReader(fileStream);
//long numBytes = new FileInfo(path).Length;
//byte[] fileBytes = br.ReadBytes((int)numBytes);
//thumb.Data = fileBytes;
//br.Close();

//result.Thumbnails.Add(thumb);



//public GridData GetGridData(string entityKey)
//{
//    GridData g = new GridData();

//    g.FieldNames = new List<string> {"asd", "bbb", "ccc"};

//    g.Rows = new List<GridRow>();

//    var gr = new GridRow();
//    gr.Cels = new List<object>();
//    gr.Cels.Add("0.0");
//    gr.Cels.Add("0.1");
//    gr.Cels.Add(0.2);
//    g.Rows.Add(gr);

//    gr = new GridRow();
//    gr.Cels = new List<object>();
//    gr.Cels.Add("1.0");
//    gr.Cels.Add("1.1");
//    gr.Cels.Add(1.2);
//    g.Rows.Add(gr);

//    return g;
//}


// вызов этой функции  закомментировано в  private void NewDetalPassportButton_Click(object sender, RoutedEventArgs e)
//public CountParent GetCountParent(string entityKey, string context)
//{

//    var result = new CountParent();

//    #region Old Code
//    //OracleCommand cmd = DataOracleHelper.GetStoredProcCommand("Passport_Silverlight_API.GetCountParent");
//    //cmd.Parameters.AddWithValue("iEntityKey", entityKey);
//    //cmd.Parameters.Add("CountParent", OracleType.Int32).Direction = ParameterDirection.Output;

//    //DataOracleHelper.ExecQuery(cmd);
//    //result.CountParentOnEntityKey = (int)cmd.Parameters["CountParent"].Value;
//    #endregion


//    #region Add Context

//    //DbConnAuth dbConnAuth = new DbConnAuth();
//    //DBConn.Conn connOra = dbConnAuth.connOra();

//    //connOra.ConnectionString(WebConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);
//    //DBConn.DBParam[] oip1 = new DBConn.DBParam[1];

//    //oip1[0] = new DBConn.DBParam();
//    //oip1[0].ParameterName = "in_ncontext";
//    //oip1[0].DbType = DBConn.DBTypeCustom.Number;
//    //oip1[0].Value = context;

//    //IDbConnection dbConnection =
//    //    connOra.Connect(WebConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);

//    //connOra.ExecuteNonQuery(dbConnection, "Passport_Silverlight_API.setContext", oip1);
//    //connOra.Disconnect(dbConnection);

//    #endregion



//    int countParent = 0;
//    DBConn.DBParam[] oip = new DBConn.DBParam[1];
//    oip[0] = new DBConn.DBParam();
//    oip[0].ParameterName = "iEntityKey";
//    oip[0].DbType = DBConn.DBTypeCustom.Int32;
//    oip[0].Direction = ParameterDirection.Input;
//    oip[0].Value = int.Parse(entityKey);

//    DataOracleHelper.ExecQuery(OracleQuerys.GetCountParentQuery, ref oip, ref countParent);
//    result.CountParentOnEntityKey = countParent;

//    return result;
//}

#region Old Code
//OracleCommand cmd = DataOracleHelper.GetStoredProcCommand("Passport_Silverlight_API.GetFullNameParentObj");
//cmd.Parameters.AddWithValue("inObjKey", inKeyPlaseState);
//cmd.Parameters.Add("cFullNameParentObj", OracleType.VarChar, 1000).Direction = ParameterDirection.Output;
//DataOracleHelper.ExecQuery(cmd);
//result.NameParentOnPlaseState_result.NameParentOnPlaseState = cmd.Parameters["cFullNameParentObj"].Value.ToString();
#endregion

//OracleCommand cmd = DataOracleHelper.GetStoredProcCommand("Passport_SL_API.getObjectInfo");

//закоментировано не работает через DBConn!!

//OracleCommand cmd = DataOracleHelper.GetStoredProcCommand(OracleQuerys.GetFullNameParentObjQuery);
//cmd.Parameters.AddWithValue("in_nObjectKey", inKeyPlaseState);
//cmd.Parameters.AddWithValue("in_nCTX", context);
//cmd.Parameters.Add("out_cparentobjectname", OracleType.VarChar, 1000).Direction = ParameterDirection.Output;
//cmd.Parameters.Add("out_nEntityKey", OracleType.Number).Direction = ParameterDirection.Output;
//cmd.Parameters.Add("out_cEntityName", OracleType.VarChar, 1000).Direction = ParameterDirection.Output;
//cmd.Parameters.Add("out_cObjectName", OracleType.VarChar, 1000).Direction = ParameterDirection.Output;
//cmd.Parameters.Add("out_nParentObjectKey", OracleType.Number).Direction = ParameterDirection.Output;
//cmd.Parameters.Add("out_nParentEntityKey", OracleType.Number).Direction = ParameterDirection.Output;
//cmd.Parameters.Add("out_cParentEntityName", OracleType.VarChar, 1000).Direction = ParameterDirection.Output;

//DataOracleHelper.ExecQuery(cmd);
//result.NameParentOnPlaseState_result.NameParentOnPlaseState = cmd.Parameters["out_cparentobjectname"].Value.ToString();



//public List<DataOneParent> GetDataOneParent(string entityParent, string inObjKey_Parent)

//не актуально!!!!! для добавления нового паспорта не используем выбор парента!!!!
//использовалось в private void ComboBoxParent_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
//public DataOneParent_result GetDataOneParent(string entityParent, string inObjKey_Parent, string context)
//{

//    var result = new DataOneParent_result();
//    result.DataOneParentList = new List<DataOneParent>();

//    try
//    {
//        //by Gaitov (17.09.2012)

//        //OracleCommand cmd = DataOracleHelper.GetStoredProcCommand("Passport_Silverlight_API.GetAllParent_Child_Obj");
//        //cmd.Parameters.AddWithValue("iEntityKey", int.Parse(entityParent).ToString());
//        //cmd.Parameters.AddWithValue("inObjKey_Parent", int.Parse(inObjKey_Parent).ToString());
//        //cmd.Parameters.Add("p_cursor1", OracleType.Cursor).Direction = ParameterDirection.Output;

//        //DataSet ds = DataOracleHelper.ExecQuery(cmd);

//        //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
//        //{
//        //    foreach (DataRow row in ds.Tables[0].Rows)
//        //    {

//        //        result.DataOneParentList.Add(
//        //            new DataOneParent { KEYONEPARENT = row["key"].ToString(), NAMEONEPARENT = row["name"].ToString() });
//        //    }
//        //}
//        //else
//        //{
//        //    result.IsValid = false;
//        //    result.ErrorMessage = string.Format("Нет данных, ключ = {0}", entityParent);
//        //}

//    }

//    catch (Exception ex)
//    {
//        log_passport.Error(ex);
//        result.IsValid = false;
//        result.ErrorMessage = ex.Message;
//    }

//    return result;
//}


//public  UserSession2 ReturnUserSession()
//{
//    var result = new UserSession2();
//    result.UserSession_syst_result = new UserSession_syst();
//    try
//    {
//        result.UserSession_syst_result.UserSession1 = UserSession.SessionKey;
//    }
//    catch (Exception ex)
//    {
//        result.IsValid = false;
//        result.ErrorMessage = ex.Message;
//    }
//    return result;
//}


#endregion Old Code