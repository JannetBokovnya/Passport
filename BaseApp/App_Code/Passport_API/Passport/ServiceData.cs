using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using App_Code.Admin_module_API;
using App_Code.Passport_API;


public partial class ServiceData : IServiceData
{

    public PassportData GetPassport(string passportKey, int intypePassport, string context)
    {
        var result = new PassportData();

        result.Data = new Dictionary<string, string>();
        try
        {
            DataSet ds = new DataSet();
            DBConn.DBParam[] oip = new DBConn.DBParam[5];
            oip[0] = new DBConn.DBParam();
            // oip[0].ParameterName = "inEntityKey";
            oip[0].ParameterName = "in_nEntityKey";
            oip[0].DbType = DBConn.DBTypeCustom.Int32;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = Convert.ToInt32("-1");

            oip[1] = new DBConn.DBParam();
            //oip[1].ParameterName = "inObjKey";
            oip[1].ParameterName = "in_nObjectKey";
            oip[1].DbType = DBConn.DBTypeCustom.Int64;                     //.Int32
            oip[1].Direction = ParameterDirection.Input;
            oip[1].Value = Convert.ToInt64(passportKey);                   //ToInt32(passportKey);

            oip[2] = new DBConn.DBParam();
            //oip[2].ParameterName = "inParentKey";
            oip[2].ParameterName = "in_nParentObjectKey";
            oip[2].DbType = DBConn.DBTypeCustom.Double;                     //.Int32
            oip[2].Direction = ParameterDirection.Input;
            oip[2].Value = Convert.ToDouble("0");                           //.ToInt32("0");

            oip[3] = new DBConn.DBParam();
            //oip[3].ParameterName = "intypePassport";
            oip[3].ParameterName = "in_nView";
            oip[3].DbType = DBConn.DBTypeCustom.Double;                     //.Int32
            oip[3].Direction = ParameterDirection.Input;
            oip[3].Value = Convert.ToDouble(intypePassport);                //ToInt32(intypePassport);

            oip[4] = new DBConn.DBParam();
            oip[4].ParameterName = "in_nCTX";
            oip[4].DbType = DBConn.DBTypeCustom.Number;                     //.Int32
            oip[4].Direction = ParameterDirection.Input;
            oip[4].Value = Convert.ToDouble(context);                       //ToInt32(context);

            DataOracleHelper.ExecQuery(OracleQuerys.GetObjForEntityKeyQuery, ref oip, ref ds);

            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                int fieldCount = ds.Tables[0].Columns.Count;

                for (int i = 0; i < fieldCount; i++)
                {

                    if (ds.Tables[0].Columns[i].DataType.ToString() == "System.DateTime")
                    {
                        if (dr[i] is DateTime)
                        {
                            //обычная дата
                            DateTime date;
                            date = (DateTime)(dr[i]);

                            string dateStr = date.ToString("dd.MM.yyyy HH:mm:ss"); //ToString(CultureInfo.InvariantCulture);//ToString("dd.MM.yyyy HH:mm:ss");

                            //CultureInfo ruCultureInfo = new CultureInfo("ru-RU");
                            //String dateStr = date.ToString(ruCultureInfo);

                            result.Data.Add(ds.Tables[0].Columns[i].ColumnName.ToUpper(), dateStr);

                            //timestamp
                            //double timestamp = (Double)(dr[i]);
                            //string dateStr = ConvertToDateTime(timestamp);
                            //result.Data.Add(ds.Tables[0].Columns[i].ColumnName.ToUpper(), dateStr);

                        }
                        else
                        {
                            result.Data.Add(ds.Tables[0].Columns[i].ColumnName.ToUpper(), dr[i].ToString());

                        }
                    }
                    else
                    {
                        if (ds.Tables[0].Columns[i].DataType.ToString() == "System.Decimal")
                        {
                            string r = dr[i].ToString();
                            if (!DBNull.Value.Equals(r))
                            {
                                if ((r).Contains("."))
                                {
                                    r = (r).TrimEnd('0').TrimEnd('.');
                                }
                                else if ((r).Contains(","))
                                {
                                    r = (r).TrimEnd('0').TrimEnd(',');
                                }
                            }

                            result.Data.Add(ds.Tables[0].Columns[i].ColumnName.ToUpper(), r);

                        }
                        else
                        {
                            result.Data.Add(ds.Tables[0].Columns[i].ColumnName.ToUpper(), dr[i].ToString());
                        }

                        

                    }

                }
            }

            else
            {

                result.IsValid = false;
                result.ErrorMessage = string.Format("Паспорт не найден, ключ = {0}", passportKey);
            }

        }
        catch (Exception ex)
        {

            log_passport.Error(ex);
            result.IsValid = false;
            result.ErrorMessage = "для паспортной формы Passport_SL_API.getFieldValues " + " in_nEntityKey = -1 " +
                    " in_nObjectKey = " + passportKey + " in_nParentObjectKey=0 " + " in_nView = " + intypePassport +
                    " in_nCTX = " + context + ex.Message;
        }

        return result;


        //// Создаем сериализатор
        //var serializer = new System.Xml.Serialization.XmlSerializer(typeof(DataSet));

        //// Создаем строку в которую будем писать результат сериализации
        //var sb = new StringBuilder();

        //// Создает поток в котрый будем сохранять сериализацию
        //using (var stream = new StringWriter(sb))
        //{
        //    // Сериализуем
        //    serializer.Serialize(stream, ds);

        //    // В "возврат метода" вставляем результат сериализации (XML)  в виде строки;
        //    result.Data = stream.ToString();
    }

    private static string ConvertToDateTime(double timestamp)
    {
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return origin.AddSeconds(timestamp).ToString("dd.MM.yyyy");

    }

    public KeyParentOnSave SavePassport(string passportKey, string iEntityKey, string iParentKey,
                                        PassportData passportData, string context, Dictionary<string, string> password)
    {
        var result = new KeyParentOnSave();
        string _iParentKey;
        result.KeyObj_OnSaveObj_result = new KeyObj_OnSave();
        if (iParentKey != null)
            _iParentKey = iParentKey;
        else
            _iParentKey = "0";

        string strResult = "";

        try
        {
            // TODO Сохранит изменения в базе
            foreach (var field in passportData.Data)
            {
                strResult = strResult + " " + field.Key.ToString() + "=" + field.Value.ToString() + "~&~";
            }

            if (password.Count != 0)
            {
                foreach (var fieldPassword in password)
                {
                    HashPassword hash = SetHashPassword(fieldPassword.Value);
                    if (hash.IsValid)
                    {
                        strResult = strResult + " " + fieldPassword.Key.ToString() + "=" +
                                    hash.CurrentUser_OnLog_result.HashPassword_OnLogBase + "~&~";
                    }
                    else
                    {
                        result.ErrorMessage = hash.ErrorMessage;
                    }
                }
            }
            if (!String.IsNullOrEmpty(strResult))
            {


                //  OracleCommand cmd = DataOracleHelper.GetStoredProcCommand("Passport_Silverlight_API.SavePassport_f");
                string res = String.Empty;

                //для Oracl
                DBConn.DBParam[] oip = new DBConn.DBParam[6];
                oip[0] = new DBConn.DBParam();
                oip[0].ParameterName = "in_nObjectKey";
                oip[0].DbType = DBConn.DBTypeCustom.Int64;
                oip[0].Direction = ParameterDirection.Input;
                oip[0].Value = Convert.ToInt64(passportKey);

                oip[1] = new DBConn.DBParam();
                oip[1].ParameterName = "in_nEntityKey";
                oip[1].DbType = DBConn.DBTypeCustom.Int64;
                oip[1].Direction = ParameterDirection.Input;
                oip[1].Value = Convert.ToInt64(iEntityKey);

                oip[2] = new DBConn.DBParam();
                oip[2].ParameterName = "in_nParentObjectKey";
                oip[2].DbType = DBConn.DBTypeCustom.Int64;
                oip[2].Direction = ParameterDirection.Input;
                oip[2].Value = Convert.ToInt64(_iParentKey);

                oip[3] = new DBConn.DBParam();
                oip[3].ParameterName = "in_cFieldValues";
                oip[3].DbType = DBConn.DBTypeCustom.VarChar;
                oip[3].Direction = ParameterDirection.Input;
                oip[3].Value = strResult;

                oip[4] = new DBConn.DBParam();
                oip[4].ParameterName = "in_nCTX";
                oip[4].DbType = DBConn.DBTypeCustom.Number;
                oip[4].Direction = ParameterDirection.Input;
                oip[4].Value = Convert.ToDouble(context);

                oip[5] = new DBConn.DBParam();
                oip[5].ParameterName = "o_cCallRes";
                oip[5].DbType = DBConn.DBTypeCustom.String;
                oip[5].Size = 1000;
                oip[5].Direction = ParameterDirection.Output;

                DbConnAuth dbConnAuth = new DbConnAuth();
                DBConn.Conn connOra = dbConnAuth.connOra();

                connOra.ConnectionString(ConfigurationManager.ConnectionStrings["DBConn"].ToString());
                DataSet ds = connOra.ExecuteQuery(OracleQuerys.SavePassportQuery, false, oip);
                double convertres;
                string callres = "";

                if (ds != null && ds.Tables.Count > 0 && ds.Tables["Parameters"].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables["Parameters"].Rows)
                    {
                        if (row["Name"].ToString() == "o_cCallRes")
                        {
                            callres = row["Value"].ToString();
                            // result.KeyObj_OnSaveObj_result.KeyObj_OnSaveObj = row["Value"].ToString();
                        }
                    }
                    if (double.TryParse(callres, out convertres))
                    {
                        result.KeyObj_OnSaveObj_result.KeyObj_OnSaveObj = callres;
                        result.ErrorMessage = "Passport_SL_API.saveFieldValues in_nObjectKey = " + passportKey + " in_nEntityKey = " + iEntityKey +
                            " in_nParentObjectKey = " + _iParentKey + " in_cFieldValues = " + strResult;
                    }
                    else
                    {
                        result.KeyObj_OnSaveObj_result.KeyObj_OnSaveObj = "";
                        result.ErrorMessage = callres;
                    }
                }

            }
        }
            catch
            (Exception ex)
            {
                log_passport.Error(ex);
                result.IsValid = false;
                result.ErrorMessage = ex.Message + "Passport_SL_API.saveFieldValues in_nObjectKey = " + passportKey + " in_nEntityKey = " + iEntityKey +
                        " in_nParentObjectKey = " + _iParentKey + " in_cFieldValues = " + strResult; ;
            }
       
        return result;
    }

    //очень очень старое
    //try
    //{
    //    long number = Convert.ToInt64(res);
    //    result.KeyObj_OnSaveObj_result.KeyObj_OnSaveObj = res;
    //}
    //catch (Exception)
    //{

    //    result.KeyObj_OnSaveObj_result.KeyObj_OnSaveObj = "";
    //    result.ErrorMessage = res +"Ошибк конвертации ToInt64" + res + "  Passport_SL_API.saveFieldValues in_nObjectKey = " + passportKey + " in_nEntityKey = " + iEntityKey +
    //        " in_nParentObjectKey = " + _iParentKey + " in_cFieldValues = " + strResult; ;
    //}

    //Было 
    //DataOracleHelper.ExecQuery(OracleQuerys.SavePassportQuery, ref oip, ref res);
    //double outres;
    //if (double.TryParse(res, out outres))
    //{
    //    result.KeyObj_OnSaveObj_result.KeyObj_OnSaveObj = res;
    //    result.ErrorMessage = "Passport_SL_API.saveFieldValues in_nObjectKey = " + passportKey + " in_nEntityKey = " + iEntityKey +
    //        " in_nParentObjectKey = " + _iParentKey + " in_cFieldValues = " + strResult;
    //}
    //else
    //{
    //    result.KeyObj_OnSaveObj_result.KeyObj_OnSaveObj = "";
    //    result.ErrorMessage = res + "Passport_SL_API.saveFieldValues in_nObjectKey = " + passportKey + " in_nEntityKey = " + iEntityKey +
    //        " in_nParentObjectKey = " + _iParentKey + " in_cFieldValues = " + strResult; ;
    //}

    //зацементировать нкт
    public KeyCementNKT CementNkt_OnkeyNKT(string passportKey, string context)
    {
        var result = new KeyCementNKT();
        result.KeyObj_OnCementNKT_result = new KeyObj_OnCement();

        try
        {
            //by Gaitov (18.09.2012)

            //OracleCommand cmd = DataOracleHelper.GetStoredProcCommand("Passport_Silverlight_API.cementnkt");
            //cmd.Parameters.AddWithValue("inNKTKey", passportKey);
            //cmd.Parameters.Add("onCNKTKey", OracleType.Number).Direction = ParameterDirection.Output;
            //DataOracleHelper.ExecQuery(cmd);

            //result.KeyObj_OnCementNKT_result.KeyObj_OnCementNKT = cmd.Parameters["onCNKTKey"].Value.ToString();


        }
        catch (Exception ex)
        {
            log_passport.Error(ex);
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }
        return result;
    }

    //public LogOnVisibleForm LogVisible(string context)
    //{
    //    var result = new LogOnVisibleForm();
    //    result.LogOnVisible_result = new LogOnVisible_Form();
    //    try
    //    {

    //        int res = 0;

    //        DBConn.DBParam[] oip = new DBConn.DBParam[1];

    //        oip[0] = new DBConn.DBParam();
    //        oip[0].ParameterName = "in_nCTX";
    //        oip[0].DbType = DBConn.DBTypeCustom.Number;
    //        oip[0].Direction = ParameterDirection.Input;
    //        oip[0].Value = Convert.ToDouble(context);


    //        DataOracleHelper.ExecQuery(OracleQuerys.IsShowErrorQuery, ref oip, ref res);
    //        result.LogOnVisible_result.LogOnVisible = res.ToString();
    //    }
    //    catch (Exception ex)
    //    {
    //        log_passport.Error(ex);
    //        result.IsValid = false;
    //        result.ErrorMessage = ex.Message;
    //    }
    //    return result;
    //}

    public BuCopyTreeVisibleForm BuCopyVisible(string context)
    {
        var result = new BuCopyTreeVisibleForm();
        result.BuCopyTreeVisible_result = new BuCopyTreeVisible_Form();
        try
        {
            int res = 0;
            DataSet ds = new DataSet();
            DBConn.DBParam[] oip = new DBConn.DBParam[1];

            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nCTX";
            oip[0].DbType = DBConn.DBTypeCustom.Number;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = Convert.ToDouble(context);

            DataOracleHelper.ExecQuery(OracleQuerys.IsShowBuCopyTreeQuery, ref oip, ref res);
            result.BuCopyTreeVisible_result.BuCopyTreeVisible = res.ToString();
        }
        catch (Exception ex)
        {
            log_passport.Error(ex);
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }
        return result;
    }

    /// <summary>
    /// по ключу паспорта получаем XMLpasport
    /// </summary>
    /// <param name="objKey"></param>
    /// <returns></returns>
    public PasportToXML GetXMLPassportToObjKey(string objKey)
    {
        var result = new PasportToXML();
        result.PasportToXMLItemsList = new List<PasportToXMLItems>();
        DataSet ds = new DataSet();
        try
        {
            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nObjectKey";
            oip[0].DbType = DBConn.DBTypeCustom.Int64;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = Convert.ToInt64(objKey);

            DataOracleHelper.ExecQuery(OracleQuerys.ExpObjectDataQuery, ref oip, ref ds);

            if (ds != null)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    result.PasportToXMLItemsList.Add(
                       new PasportToXMLItems
                       {
                           strToXML = row[""].ToString()
                       });
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

 

    /// <summary>
    /// привязка для редактирования и добавления
    /// </summary>
    /// <param name="inObjKey"></param>
    /// <param name="strJason"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public CalcPrivJson GetCalcPrivJson(string inObjKey, string strXml, string context)
    {
        var result = new CalcPrivJson();
        result.CalcPrivOnEditJson_result = new CalcPrivOnEditJson();


        string res = String.Empty;

        try
        {
            DBConn.DBParam[] oip = new DBConn.DBParam[3];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_cData";
            oip[0].DbType = DBConn.DBTypeCustom.String;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = strXml;

            oip[1] = new DBConn.DBParam();
            oip[1].ParameterName = "in_nObjKey";
            oip[1].DbType = DBConn.DBTypeCustom.Double;
            oip[1].Direction = ParameterDirection.Input;
            oip[1].Value = Convert.ToDouble(inObjKey);

            oip[2] = new DBConn.DBParam();
            oip[2].ParameterName = "out_cData";
            oip[2].DbType = DBConn.DBTypeCustom.String;
            oip[2].Size = 10000000;
            oip[2].Direction = ParameterDirection.Output;


            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();

            connOra.ConnectionString(ConfigurationManager.ConnectionStrings["DBConn"].ToString());
            DataSet ds = connOra.ExecuteQuery(OracleQuerys.GetCreateLocationAllField, false, oip);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row["Name"].ToString() == "out_cData")
                    {

                        result.CalcPrivOnEditJson_result.CalcPrivOnEditJsonOnKeyPassport = row["Value"].ToString();
                    }

                }
            }
           // result.CalcPrivOnEditJson_result.CalcPrivOnEditJsonOnKeyPassport = strXml;
        }
        catch (Exception ex)
        {

            log_passport.Error(ex);
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }




        //для тестирования возвращаю то что отправила
        // result.CalcPrivOnEditJson_result.CalcPrivOnEditJsonOnKeyPassport = strXml;

        //запрос в базу к данным возврат стринг - нужно парсить из джейсона в структуру
        return result;
    }



    //возвращает все поля независимо от типа привязки
    public AllPriv GetAllPriv(string inObjKey, string context)
    {
        var result = new AllPriv();
        result.PrivItemsList = new List<PrivItems>();
        DataSet ds = new DataSet();

        try
        {


            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nObjKey";
            oip[0].DbType = DBConn.DBTypeCustom.Int64;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = Convert.ToInt64(inObjKey);

            DataOracleHelper.ExecQuery(OracleQuerys.GetObjectLocationQueryAllField, ref oip, ref ds);

            if (ds != null)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    result.PrivItemsList.Add(
                        new PrivItems
                        {

                            CNAME = row["CNAME"].ToString(),
                            NKMORT1 = row["NKMORT1"].ToString(),
                            NDISTANCEBEG = row["NDISTANCEBEG"].ToString(),
                            NX1 = row["NX1"].ToString(),
                            NY1 = row["NY1"].ToString(),
                            NZ1 = row["NZ1"].ToString(),
                            NKEY = row["NKEY"].ToString(),
                            NH1 = row["NH1"].ToString(),
                            NKMTRUE1 = row["NKMTRUE1"].ToString(),
                            NX2 = row["NX2"].ToString(),
                            NY2 = row["NY2"].ToString(),
                            NZ2 = row["NZ2"].ToString(),
                            NH2 = row["NH2"].ToString(),
                            NKMORT2 = row["NKMORT2"].ToString(),
                            NKMTRUE2 = row["NKMTRUE2"].ToString(),
                            NDISTANCEEND = row["NDISTANCEEND"].ToString(),
                            nMtKey = row["nMtKey"].ToString(),
                            nPipeKey = row["nPipeKey"].ToString(),
                            NBUILDTYPE = "",
                            ISEDITED = ""

                        });
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

 


    public StatusAnswer DeleteLinkToPipeByKey(string p_nkey, string context)
    {
        var result = new StatusAnswer();
        int res = 0;

        try
        {

            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "p_nkey";
            oip[0].DbType = DBConn.DBTypeCustom.Int32;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = Convert.ToInt32(p_nkey);

            DataOracleHelper.ExecQuery(OracleQuerys.DeleteLinkToPipeByKeyQuery, ref oip, ref res);

        }
        catch (Exception ex)
        {
            log_passport.Error(ex);
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }
    public DisplayLabels GetDisplayLabels(string context)
    {
        var result = new DisplayLabels();
        result.DisplayLabelsItemList = new List<DisplayLabelsItem>();
        try
        {

            DataSet ds = new DataSet();

            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nCTX";
            oip[0].DbType = DBConn.DBTypeCustom.Number;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = Convert.ToDouble(context);


            DataOracleHelper.ExecQuery(OracleQuerys.GetDisplayLablesQuery, ref oip, ref ds);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    {
                        result.DisplayLabelsItemList.Add(new DisplayLabelsItem
                                                             {
                                                                 Сplace = row["cplace"].ToString(),
                                                                 Сvalue = row["cvalue"].ToString()
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

    public KeyPassportOnCopy CopyPassport(string passportKey, string context)
    {
        var result = new KeyPassportOnCopy();
        result.KeyObj_OnCopyObj_result = new KeyObj_OnCopy();

        try
        {
            int res = 0;
            DBConn.DBParam[] oip = new DBConn.DBParam[2];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nObjectKey";
            oip[0].DbType = DBConn.DBTypeCustom.Int64;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = Convert.ToInt64(passportKey);

            oip[1] = new DBConn.DBParam();
            oip[1].ParameterName = "in_nCTX";
            oip[1].DbType = DBConn.DBTypeCustom.Number;
            oip[1].Direction = ParameterDirection.Input;
            oip[1].Value = Convert.ToDouble(context);

            DataOracleHelper.ExecQuery(OracleQuerys.CopyPassportQuery, ref oip, ref res);
            result.KeyObj_OnCopyObj_result.KeyObj_OnCopyObj = res.ToString();
        }
        catch (Exception ex)
        {
            log_passport.Error(ex);
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }
        return result;
    }

    public DataListHistoryObj GetDataHistoryObj(string inObjKey, string context)
    {
        var result = new DataListHistoryObj();
        result.DataListHistoryObjList = new List<DataListHistoryItems>();
        try
        {

            DataSet ds = new DataSet();
            DBConn.DBParam[] oip = new DBConn.DBParam[2];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nObjKey";
            oip[0].DbType = DBConn.DBTypeCustom.Int64;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = Convert.ToInt64(inObjKey);

            oip[1] = new DBConn.DBParam();
            oip[1].ParameterName = "in_nCTX";
            oip[1].DbType = DBConn.DBTypeCustom.Number;
            oip[1].Direction = ParameterDirection.Input;
            oip[1].Value = Convert.ToDouble(context);

            DataOracleHelper.ExecQuery(OracleQuerys.GetPassportHistoryQuery, ref oip, ref ds);

            //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            if (ds != null)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    result.DataListHistoryObjList.Add(
                        new DataListHistoryItems
                            {
                                CUser = row["cUser"].ToString(),
                                CFieldName = row["cFieldName"].ToString(),
                                Coldval = row["coldval"].ToString(),
                                // Cnewval = row["cnewval"].ToString(), 
                                Doperationtime = row["doperationtime"].ToString()
                            });
                }
            }
            else
            {

                result.IsValid = false;
                result.ErrorMessage = string.Format("Нет данных, ключ = {0}", inObjKey);
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

    //по ключу объекта получаем иерархию связанных объектов(для дерева по ключу)
    public TreeFullHierarchyPathToObject GetTreeFullHierarchyPathToObject(string objKey, string context)
    {
        var result = new TreeFullHierarchyPathToObject();
        result.TreeFullHierarchyPathToObjectList = new List<TreeFullHierarchyPathToObjectItems>();
        try
        {
            DataSet ds = new DataSet();
            DBConn.DBParam[] oip = new DBConn.DBParam[2];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nObjKey";
            oip[0].DbType = DBConn.DBTypeCustom.Int64;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = Convert.ToInt64(objKey);

            oip[1] = new DBConn.DBParam();
            oip[1].ParameterName = "in_nCTX";
            oip[1].DbType = DBConn.DBTypeCustom.Number;
            oip[1].Direction = ParameterDirection.Input;
            oip[1].Value = Convert.ToDouble(context);

            //DataOracleHelper.ExecQuery("FORESTER.getparttreetoobject_f", ref oip, ref ds);
            DataOracleHelper.ExecQuery(OracleQuerys.GetFullHierarchyPathToObjectQuery, ref oip, ref ds);


            if (ds != null)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    result.TreeFullHierarchyPathToObjectList.Add(
                        new TreeFullHierarchyPathToObjectItems
                        {
                            ObjKey = row["nObjKey"].ToString(),
                            ObjTypeKey = row["nObjTypeKey"].ToString(),
                            Level = row["level"].ToString(),

                        });
                }
            }
            else
            {

                result.IsValid = false;
                result.ErrorMessage = string.Format("Нет данных, ключ = {0}", objKey);
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

    //отдельное дерево

    public DataTreeViewOnObjkey ReturnTreeOnObjKey(string objKey, string context)
    {

        int _level;
        int parentKey;

        var result = new DataTreeViewOnObjkey();
        result.DataTreeViewOnObjkeyList = new List<DataTreeViewOnObjkeyItems>();

        var result2 = new DataTreeViewOnObjkey();
        result2.DataTreeViewOnObjkeyList = new List<DataTreeViewOnObjkeyItems>();

        try
        {

            DataSet ds = new DataSet();
            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nObjKey";
            oip[0].DbType = DBConn.DBTypeCustom.Int64;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = Convert.ToInt64(objKey);

            //DataOracleHelper.ExecQuery("FORESTER.getparttreetoobject_f", ref oip, ref ds);

            DataOracleHelper.ExecQuery(OracleQuerys.GetFullHierarchyPathToObjectQuery, ref oip, ref ds);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow row = ds.Tables[0].Rows[i];
                    string one = row["nParentKey"].ToString();
                    if (one == "-1")
                    {
                        result.DataTreeViewOnObjkeyList.Add(new DataTreeViewOnObjkeyItems()
                        {
                            Key = "100" + "z0z" + row["nObjTypeKey"].ToString(),
                            Texts = row["nObjTypeName"].ToString(),
                            Level = row["nObjKey"].ToString(),
                            ParentKey = ""

                        });
                        result.DataTreeViewOnObjkeyList.Add(new DataTreeViewOnObjkeyItems()
                        {
                            Key =
                                "101" + "z" + row["nObjKey"].ToString() + "z" +
                                row["nObjTypeKey"].ToString(),
                            Texts = row["nObjName"].ToString(),
                            Level = row["nObjKey"].ToString(),
                            ParentKey = row["nObjKey"].ToString(),


                        });
                        break;
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
        return result2;

        #region OldCode

        //string nObjKey = "";
        //string nObjTypeKey = "";
        //string nObjName = "";
        //string nObjTypeName = "";
        //string level = "";



        //var result = new DataTreeViewOnObjkey();

        //result.DataTreeViewOnObjkeyList = new List<DataTreeViewOnObjkeyItems>();
        //try
        //{
        //    OracleCommand cmd = DataOracleHelper.GetStoredProcCommand("FORESTER.GetFullHierarchyPathToObject");
        //    //OracleCommand cmd = DataOracleHelper.GetStoredProcCommand("FORESTER.getparttreetoobject");
        //    cmd.Parameters.AddWithValue("in_nObjKey", int.Parse(objKey).ToString());
        //    cmd.Parameters.Add("result", OracleType.Cursor).Direction = ParameterDirection.ReturnValue;



        //    DataSet ds = DataOracleHelper.ExecQuery(cmd);





        //    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //    {
        //        DataRow row0 = ds.Tables[0].Rows[0];

        //        result.DataTreeViewOnObjkeyList.Add(new DataTreeViewOnObjkeyItems()
        //        {
        //            Key = "100" + "z0z" + row0["nObjTypeKey"].ToString(),
        //            Texts = row0["nObjTypeName"].ToString(),
        //            Level = "0",
        //            ParentKey = ""


        //        });

        //        result.DataTreeViewOnObjkeyList.Add(new DataTreeViewOnObjkeyItems()
        //        {
        //            Key = "101" + "z" + row0["nObjKey"].ToString() + "z" + row0["nObjTypeKey"].ToString(),
        //            Texts = row0["nObjName"].ToString(),
        //            Level = "1",
        //            ParentKey = "0"


        //        });
        //        int _level = 1;


        //        for (int i = 1; i < ds.Tables[0].Rows.Count; i++)
        //        {
        //            DataRow row = ds.Tables[0].Rows[i];

        //            result.DataTreeViewOnObjkeyList.Add(new DataTreeViewOnObjkeyItems()
        //            {
        //                Key = "100" + "z" + row["nObjKey"].ToString() + "z" + row["nObjTypeKey"].ToString(),
        //                Texts = row["nObjTypeName"].ToString(),
        //                Level = (_level + 1).ToString(),
        //                ParentKey = (_level).ToString()


        //            });

        //            result.DataTreeViewOnObjkeyList.Add(new DataTreeViewOnObjkeyItems()
        //            {
        //                Key = "101" + "z" + row["nObjKey"].ToString() + "z" + row["nObjTypeKey"].ToString() + "z" + row["nObjKey"].ToString(),
        //                Texts = row["nObjName"].ToString(),
        //                Level = (_level + 2).ToString(),
        //                ParentKey = (_level + 1).ToString()


        //            });

        //            _level = _level + 2;
        //        }
        //        //foreach (DataRow row in ds.Tables[0].Rows)
        //        //{

        //        //        nObjKey = row["nObjKey"].ToString();
        //        //            nObjTypeKey = row["nObjTypeKey"].ToString();
        //        //            nObjName = row["nObjName"].ToString();
        //        //            nObjTypeName = row["nObjTypeName"].ToString();
        //        //            level = row["level"].ToString();

        //        //        result.DataTreeViewOnObjkeyList.Add(new DataTreeViewOnObjkeyItems()
        //        //        {
        //        //           Key = nObjKey +"z"+nObjTypeKey, 
        //        //           Texts= nObjName+":"+nObjTypeName, 
        //        //           Level=level,
        //        //           ParentKey="1"


        //        //        });

        //        //}
        //    }
        //}
        //catch (Exception ex)
        //{
        //    result.IsValid = false;
        //    result.ErrorMessage = ex.Message;
        //}

        //return result;
        #endregion OldCode
    }

    private int _newGenericKey = 0;

    private string GetNewGenericKey()
    {
        _newGenericKey++;
        return "header" + _newGenericKey.ToString();
    }


    //отдельное "полное" дерево по ключу объекта
    public DataTreeViewOnObjkey ReturnAllTreeOnObjKey(string objKey, string context)
    {
        _newGenericKey = 0;
        var result = new DataTreeViewOnObjkey();
        result.DataTreeViewOnObjkeyList = new List<DataTreeViewOnObjkeyItems>();

        try
        {

            DataSet ds = new DataSet();
            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nObjKey";
            oip[0].DbType = DBConn.DBTypeCustom.Int64;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = Convert.ToInt64(objKey);

            //DataOracleHelper.ExecQuery("FORESTER.getparttreetoobject_f", ref oip, ref ds);
            DataOracleHelper.ExecQuery(OracleQuerys.GetPartTreeToObjectQuery, ref oip, ref ds);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                bool _hasHeaderZerroLevel = false;
                string newGenericKey = "0";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow row = ds.Tables[0].Rows[i];
                    if (row["nParentKey"].ToString() == "-1")
                    {
                        if (!_hasHeaderZerroLevel)
                        {
                            newGenericKey = GetNewGenericKey();
                            _hasHeaderZerroLevel = true;
                            var itemHeader = new DataTreeViewOnObjkeyItems()
                                                 {
                                                     Key = "100" + "z0z" + row["nObjTypeKey"],
                                                     Texts = row["nObjTypeName"].ToString(),
                                                     Level = newGenericKey,
                                                     ParentKey = ""
                                                 };

                            Debug.WriteLine(string.Format("Header(0): {0}, {1}, {2} ", itemHeader.Level, itemHeader.ParentKey, itemHeader.Texts));
                            result.DataTreeViewOnObjkeyList.Add(itemHeader);
                        }

                        if (!"0".Equals(row["nObjKey"].ToString()))
                        {
                            var item = new DataTreeViewOnObjkeyItems()
                                           {
                                               Key = "101" + "z" + row["nObjKey"] + "z" + row["nObjTypeKey"],
                                               Texts = row["nObjName"].ToString(),
                                               Level = row["nObjKey"].ToString(),
                                               ParentKey = newGenericKey
                                           };

                            Debug.WriteLine(string.Format("Add header's element(0): {0}, {1}, {2} ", item.Level, item.ParentKey, item.Texts));

                            result.DataTreeViewOnObjkeyList.Add(item);
                            ProcessChildred(result.DataTreeViewOnObjkeyList, ds, row["nObjKey"].ToString(), context);
                        }
                        else
                        {
                            Debug.WriteLine(string.Format("Skip: {0}, {1}, {2} ", row["nObjKey"].ToString(), newGenericKey, row["nObjName"].ToString()));
                        }

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

    private void ProcessChildred(List<DataTreeViewOnObjkeyItems> list, DataSet ds, string parentObjKey, string context)
    {
        List<TmpDataRow> childrenDR = new List<TmpDataRow>();
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            DataRow row = ds.Tables[0].Rows[i];
            if (row["nParentKey"].ToString() == parentObjKey)
            {
                childrenDR.Add(new TmpDataRow(row["nObjKey"].ToString(), row["nObjTypeKey"].ToString(), row["nObjName"].ToString(),
                    (string)row["nObjTypeName"]));
            }
        }

        while (childrenDR.Count > 0)
        {
            string newGenericKey = GetNewGenericKey();
            TmpDataRow row = childrenDR[0];

            var itemHeader = new DataTreeViewOnObjkeyItems()
            {
                Key = "100" + "z" + parentObjKey + "z" + row.TypeKey,
                Texts = row.TypeName,
                Level = newGenericKey,
                ParentKey = parentObjKey
            };

            Debug.WriteLine(string.Format("Header: {0}, {1}, {2} ", itemHeader.Level, itemHeader.ParentKey, itemHeader.Texts));

            list.Add(itemHeader);

            while (null != GetRowByObjTypeKey(childrenDR, row.TypeKey, context))
            {
                TmpDataRow r = GetRowByObjTypeKey(childrenDR, row.TypeKey, context);
                if (!"0".Equals(r.Key))
                {
                    var item = new DataTreeViewOnObjkeyItems()
                                   {
                                       Key = "101" + "z" + r.Key + "z" + r.TypeKey,
                                       Texts = r.Name,
                                       Level = r.Key,
                                       ParentKey = newGenericKey
                                   };
                    list.Add(item);

                    Debug.WriteLine(string.Format("Add element: {0}, {1}, {2} ", item.Level, item.ParentKey, item.Texts));

                    ProcessChildred(list, ds, r.Key, context);
                }
                else
                {
                    Debug.WriteLine(string.Format("Skip: {0}, {1}, {2} ", r.Key, newGenericKey, r.Name));
                }

                childrenDR.Remove(r);
            }
        }
    }

    private TmpDataRow GetRowByObjTypeKey(IEnumerable<TmpDataRow> list, string objTypeKey, string context)
    {
        return list.FirstOrDefault(dataRow => dataRow.TypeKey == objTypeKey);
    }

}

public class TmpDataRow
{
    public string Key { get; set; }
    public string TypeKey { get; set; }
    public string Name { get; set; }
    public string TypeName { get; set; }

    public TmpDataRow(string key, string typeKey, string name, string typeName)
    {
        Key = key;
        TypeKey = typeKey;
        Name = name;
        TypeName = typeName;
    }
}




