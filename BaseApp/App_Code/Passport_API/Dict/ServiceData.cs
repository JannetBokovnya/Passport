using System.Collections.Generic;
using System;
using System.Data.OracleClient;
using System.Data;
using System.Web.Configuration;
using App_Code.Passport_API;

public partial class ServiceData : IServiceData
    {


        public DictDataOnEntityKey DictValueData(string iKeyEntity, string context)
        {
            var result = new DictDataOnEntityKey();
            result.DictData_result = new List<DictData>();
            try
            {
                DBConn.DBParam[] oip = new DBConn.DBParam[2];
                oip[0] = new DBConn.DBParam();
                oip[0].ParameterName = "in_nEntityKey";
                oip[0].DbType = DBConn.DBTypeCustom.Double;
                oip[0].Direction = ParameterDirection.Input;
                oip[0].Value = Convert.ToDouble(iKeyEntity);

                oip[1] = new DBConn.DBParam();
                oip[1].ParameterName = "in_nCTX";
                oip[1].DbType = DBConn.DBTypeCustom.Number;
                oip[1].Direction = ParameterDirection.Input;
                oip[1].Value = Convert.ToDouble(context);

                DataSet ds = new DataSet();

                DataOracleHelper.ExecQuery(OracleQuerys.GetDictOneEntityKeyQuery, ref oip, ref ds);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {

                        result.DictData_result.Add(
                            new DictData { VALUEKEYDICT = row["nNsiValueKey"].ToString(), VALUEDICT = row["cNsiValue"].ToString(), KEYDICT = row["nNsiKey"].ToString() });
                    }


                }
                //else
                //{
                //    result.IsValid = false;
                //    result.ErrorMessage = string.Format("Нет данных, ключ = {0}", iKeyEntity);
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

        public List<DictDataFiltr> DictDataFiltr(string context)
        {

            var result = new List<DictDataFiltr>();
            return result;
        }

        public DictDataOne OneDictValueData(string iKeyDict, string context)
        {
            var result = new DictDataOne();
            result.DictData_result = new List<OneDictData>();
            try
            {
               
                DBConn.DBParam[] oip = new DBConn.DBParam[2];
                oip[0] = new DBConn.DBParam();
                oip[0].ParameterName = "in_nNsiKey";
                oip[0].DbType = DBConn.DBTypeCustom.Double;
                oip[0].Direction = ParameterDirection.Input;
                oip[0].Value = Convert.ToDouble(iKeyDict);

                oip[1] = new DBConn.DBParam();
                oip[1].ParameterName = "in_nCTX";
                oip[1].DbType = DBConn.DBTypeCustom.Number;
                oip[1].Direction = ParameterDirection.Input;
                oip[1].Value = Convert.ToDouble(context);

                DataSet ds = new DataSet();
                DataOracleHelper.ExecQuery(OracleQuerys.GetOneDictQuery, ref oip, ref ds);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {

                        result.DictData_result.Add(
                            new OneDictData { KEYVALUEDICT = row["nNsiValueKey"].ToString(), VALUEDICT = row["cNsiValue"].ToString() });
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



        public ListNsiOnSave SaveNsi(string i_nKeyNsiValue, string i_cNsiValue, string i_nKeyNsi, string context)
        {
            var result = new ListNsiOnSave();
            result.DictData_result = new List<OneDictData>();

            try
            {
               
                DBConn.DBParam[] oip = new DBConn.DBParam[4];
                oip[0] = new DBConn.DBParam();
                oip[0].ParameterName = "in_nNsiValueKey";
                oip[0].DbType = DBConn.DBTypeCustom.Int64;
                oip[0].Direction = ParameterDirection.Input;
                oip[0].Value = Convert.ToInt64(i_nKeyNsiValue);

                oip[1] = new DBConn.DBParam();
                oip[1].ParameterName = "in_cNsiValue";
                oip[1].DbType = DBConn.DBTypeCustom.VarChar2;
                oip[1].Direction = ParameterDirection.Input;
                oip[1].Value = i_cNsiValue;

                oip[2] = new DBConn.DBParam();
                oip[2].ParameterName = "in_nNsiKey";
                oip[2].DbType = DBConn.DBTypeCustom.Int64;
                oip[2].Direction = ParameterDirection.Input;
                oip[2].Value = Convert.ToInt64(i_nKeyNsi);

                oip[3] = new DBConn.DBParam();
                oip[3].ParameterName = "in_nCTX";
                oip[3].DbType = DBConn.DBTypeCustom.Number;
                oip[3].Direction = ParameterDirection.Input;
                oip[3].Value = Convert.ToDouble(context);

                DataSet ds = new DataSet();
                DataOracleHelper.ExecQuery(OracleQuerys.SaveNsiValueQuery, ref oip, ref ds);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {

                        result.DictData_result.Add(
                            new OneDictData { KEYVALUEDICT = row["NNSI_VALUEKEY"].ToString(), VALUEDICT = row["cname"].ToString() });
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


        public nameNSIOnKey GetNameNsiOnKeyNSI(string inKeyNSI, string context)
        {
            var result = new nameNSIOnKey();
            result.nameNSIonKey_result = new nameNSI_Key();
            try
            {

                DBConn.DBParam[] oip = new DBConn.DBParam[2];
                oip[0] = new DBConn.DBParam();
                oip[0].ParameterName = "in_nNsiKey";
                oip[0].DbType = DBConn.DBTypeCustom.Int64;
                oip[0].Direction = ParameterDirection.Input;
                oip[0].Value = Convert.ToInt64(inKeyNSI);

                oip[1] = new DBConn.DBParam();
                oip[1].ParameterName = "in_nCTX";
                oip[1].DbType = DBConn.DBTypeCustom.Number;
                oip[1].Direction = ParameterDirection.Input;
                oip[1].Value = Convert.ToDouble(context);

                string res = String.Empty;
                DataOracleHelper.ExecQuery(OracleQuerys.GetNameNsiQuery, ref oip, ref res);
                result.nameNSIonKey_result.NameNSIonkey = res;
            }
            catch (Exception ex)
            {
                log_passport.Error(ex);
                result.IsValid = false;
                result.ErrorMessage = ex.Message;
            }

            return result;

        }



    }
