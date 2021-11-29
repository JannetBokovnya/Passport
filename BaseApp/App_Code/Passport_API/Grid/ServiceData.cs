using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using App_Code.Passport_API;

public partial class ServiceData : IServiceData
    {
        public GridData GetGridData(string ipassportKey, string inObjKey, string iTypePassport, string inParentKey, string context)
        {
            var result = new GridData();
            result.FieldNames = new List<string>();
            result.Rows = new List<GridRow>();
            string _inParentKey = "";


            if (inParentKey == null)
                _inParentKey = "0";
            else
                _inParentKey = inParentKey;

            Int64 t1 = Convert.ToInt64(_inParentKey);
            Int64 t2 = Convert.ToInt64(inObjKey);

            try
            {
               
                DataSet ds = new DataSet();
                DBConn.DBParam[] oip = new DBConn.DBParam[5];
                oip[0] = new DBConn.DBParam();
                oip[0].ParameterName = "in_nEntityKey";
                oip[0].DbType = DBConn.DBTypeCustom.Int64;
                oip[0].Direction = ParameterDirection.Input;
                oip[0].Value = Convert.ToInt64(ipassportKey);

                oip[1] = new DBConn.DBParam();
                oip[1].ParameterName = "in_nObjectKey";
                oip[1].DbType = DBConn.DBTypeCustom.Int64;
                oip[1].Direction = ParameterDirection.Input;
                oip[1].Value = Convert.ToInt64(inObjKey);

                oip[2] = new DBConn.DBParam();
                oip[2].ParameterName = "in_nParentObjectKey";
                oip[2].DbType = DBConn.DBTypeCustom.Int64;
                oip[2].Direction = ParameterDirection.Input;
                oip[2].Value = Convert.ToInt64(_inParentKey);

                oip[3] = new DBConn.DBParam();
                oip[3].ParameterName = "in_nView";
                oip[3].DbType = DBConn.DBTypeCustom.Number;
                oip[3].Direction = ParameterDirection.Input;
                oip[3].Value = Convert.ToDouble(iTypePassport);

                oip[4] = new DBConn.DBParam();
                oip[4].ParameterName = "in_nCTX";
                oip[4].DbType = DBConn.DBTypeCustom.Number;
                oip[4].Direction = ParameterDirection.Input;
                oip[4].Value = Convert.ToDouble(context);

                DataOracleHelper.ExecQuery(OracleQuerys.GetObjForEntityKeyQuery, ref oip, ref ds);

                result.ErrorMessage = "для списочной формы Passport_SL_API.getFieldValues " + " in_nEntityKey = " + ipassportKey +
                    " in_nObjectKey = " + inObjKey + " in_nParentObjectKey= " + _inParentKey + " in_nView = " + iTypePassport +
                    " in_nCTX = " + context ;

                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    int fieldCount = ds.Tables[0].Columns.Count;

                    for (int i = 0; i < fieldCount; i++)
                    {
                        result.FieldNames.Add(ds.Tables[0].Columns[i].ColumnName.ToUpper());
                    }
                    //ds.Tables[0].Rows.Count
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        var row = new GridRow();
                        row.Cels = new List<object>();
                        
                        DataRow tableRow = ds.Tables[0].Rows[i];
                        for (int j = 0; j < fieldCount; j++)
                        {
                            var r = tableRow[j];

                            //by Gaitov
                            if (r.GetType().ToString() == "System.DateTime")
                            {
                                r = (DBNull.Value.Equals(r)) ? "" : tableRow[j].ToString();
                                r = r.ToString().Substring(0, r.ToString().Length - 8);
                            }

                            if (r.GetType().ToString() == "System.Decimal")
                            {
                                if (DBNull.Value.Equals(r))
                                {
                                    r = "";
                                }
                                else
                                {
                                    r = tableRow[j].ToString();
                                    if ((tableRow[j].ToString()).Contains("."))
                                    {
                                        r=(tableRow[j].ToString()).TrimEnd('0').TrimEnd('.');
                                    }
                                    else if ((tableRow[j].ToString()).Contains(","))
                                    {
                                        r=(tableRow[j].ToString()).TrimEnd('0').TrimEnd(',');
                                    }
                                       
                                }
                            }

                            if (r.GetType().ToString() == "System.Double")
                            {
                                r = (DBNull.Value.Equals(r)) ? "" : tableRow[j].ToString();
                            }

                            if (r.GetType().ToString() == "System.Int64")
                            {
                                r = (DBNull.Value.Equals(r)) ? "" : tableRow[j].ToString();
                            }

                            if (r.GetType().ToString() == "System.Int32")
                            {
                                r = (DBNull.Value.Equals(r)) ? "" : tableRow[j].ToString();
                            }

                            //if (r.GetType().ToString() == "System.Boolean")
                            //{
                            //    r = (DBNull.Value.Equals(r)) ? False : tableRow[j].;
                            //}

                            if (DBNull.Value.Equals(r))
                            {
                                r = null;
                            }

                            row.Cels.Add(r);
                            
                            
                            // var r = tableRow[j];
                           
                            
                           //if (DBNull.Value.Equals(r))
                           
                           // {
                           //     r = "";
                           // }

                           // row.Cels.Add(r);
                        }
                        result.Rows.Add(row);
                    }
                }
                else
                {
                    //обрабатываем результат на форме
                    //result.IsValid = false;
                    //result.ErrorMessage = string.Format("Нет данных, ключ = {0}", ipassportKey);
                }
            }
            catch (Exception ex)
            {
                log_passport.Error(ex);
                result.IsValid = false;
                result.ErrorMessage = "для списочной формы Passport_SL_API.getFieldValues " + " in_nEntityKey = " + ipassportKey +
                    " in_nObjectKey = " + inObjKey + " in_nParentObjectKey= " + _inParentKey + " in_nView = " + iTypePassport +
                    " in_nCTX = " + context + ex.Message;
            }

            return result;
        }
    }

//через  MS SQL напрямую
 //string conStr = WebConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
 //               SqlConnection conn = new SqlConnection(conStr);
 //               SqlCommand cmd1 = new SqlCommand();
 //               cmd1.Connection = conn;

 //               cmd1.CommandType = CommandType.StoredProcedure;
 //               cmd1.CommandText = OracleQuerys.GetObjForEntityKeyQuery;
 //               cmd1.Parameters.AddWithValue("in_nObjectKey", Convert.ToDouble(inObjKey));
 //               cmd1.Parameters.AddWithValue("in_nEntityKey", Convert.ToDouble(ipassportKey));
 //               cmd1.Parameters.AddWithValue("in_nParentObjectKey", Convert.ToDouble(_inParentKey));
 //               cmd1.Parameters.AddWithValue("in_nView", Convert.ToDouble(iTypePassport));
 //               cmd1.Parameters.AddWithValue("in_nCTX", Convert.ToDouble(context));
 //               conn.Open();
 //               SqlDataAdapter da = new SqlDataAdapter(cmd1);
 //               DataTable dt = new DataTable();
 //               da.Fill(dt);
