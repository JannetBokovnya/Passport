using System.Collections.Generic;
using System.Data.OracleClient;
using System.Data;
using System;
using System.Web.Configuration;
using log4net;
using App_Code.Passport_API;

public partial class ServiceData : IServiceData
    {
    

        public ControlAttribute AttrControl(string context)
        {
            var result = new ControlAttribute();
            result.ControlAttributeList = new List<ControlAttributeItem>();

            try
            {
               
                DataSet ds = new DataSet();

                DBConn.DBParam[] oip = new DBConn.DBParam[1];
                oip[0] = new DBConn.DBParam();
                oip[0].ParameterName = "in_nCTX";
                oip[0].DbType = DBConn.DBTypeCustom.Number;
                oip[0].Direction = ParameterDirection.Input;
                oip[0].Value = Convert.ToDouble(context);

                DataOracleHelper.ExecQuery(OracleQuerys.GetAttrControlQuery, ref oip, ref ds);
               

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        result.ControlAttributeList.Add(
                            new ControlAttributeItem

                            {

                                TYPECONTROL = Convert.ToInt32(row["TYPECONTROL"]),
                                COLORFON = row["COLORFON"].ToString(),
                                COLORTEXT = row["COLORTEXT"].ToString(),
                                UNDERLINE = Convert.ToInt32(row["UNDERLINE"]),
                                WIDTH = Convert.ToInt32(row["WIDTH"]),
                                HEIGHT = Convert.ToInt32(row["HEIGHT"]),
                                FONTSIZE = Convert.ToInt32(row["FONTSIZE"])
                            }
                                );


                    }
                }
                else
                {
                    result.ControlAttributeList.Add((new ControlAttributeItem { TYPECONTROL = 1, COLORFON = "Red", COLORTEXT = "Red", UNDERLINE = 1, WIDTH = 20, HEIGHT = 20, FONTSIZE = 16 }));
                    result.ControlAttributeList.Add((new ControlAttributeItem { TYPECONTROL = 2, COLORFON = "Red", COLORTEXT = "Red", UNDERLINE = 1, WIDTH = 20, HEIGHT = 20, FONTSIZE = 12 }));
                    result.ControlAttributeList.Add((new ControlAttributeItem { TYPECONTROL = 3, COLORFON = "Red", COLORTEXT = "Red", UNDERLINE = 1, WIDTH = 20, HEIGHT = 20, FONTSIZE = 12 }));
                    result.ControlAttributeList.Add((new ControlAttributeItem { TYPECONTROL = 4, COLORFON = "Red", COLORTEXT = "Red", UNDERLINE = 1, WIDTH = 20, HEIGHT = 20, FONTSIZE = 12 }));
                    result.ControlAttributeList.Add((new ControlAttributeItem { TYPECONTROL = 5, COLORFON = "Red", COLORTEXT = "Red", UNDERLINE = 1, WIDTH = 20, HEIGHT = 20, FONTSIZE = 12 }));
                    result.ControlAttributeList.Add((new ControlAttributeItem { TYPECONTROL = 6, COLORFON = "Red", COLORTEXT = "Red", UNDERLINE = 1, WIDTH = 20, HEIGHT = 20, FONTSIZE = 12 }));
                    result.ControlAttributeList.Add((new ControlAttributeItem { TYPECONTROL = 7, COLORFON = "Red", COLORTEXT = "Red", UNDERLINE = 1, WIDTH = 20, HEIGHT = 20, FONTSIZE = 12 }));
                    result.ControlAttributeList.Add((new ControlAttributeItem { TYPECONTROL = 8, COLORFON = "Red", COLORTEXT = "Red", UNDERLINE = 1, WIDTH = 20, HEIGHT = 20, FONTSIZE = 12 }));

                    result.ErrorMessage = OracleQuerys.GetAttrControlQuery + " вернул null";
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

        public List<AttributOneControl> AttrOneControl(string iTypeControl, string context)
        {
            var result = new List<AttributOneControl>();
            return result;
        }
    }
