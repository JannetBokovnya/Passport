using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OracleClient;
using System.Data;
using System.Web;
using System.Web.Configuration;
using log4net;

namespace TreeLinks
{
    public class treeNodeReader
    {
        //private readonly string dbConnection;
        //private const string query = "Forester.GetNodes";
        //private static readonly ILog log_passport = LogManager.GetLogger(typeof(treeNodeReader).Name);

        //public List<Pair> CreateRootOfTree(string con, string Query, string rootKey, string context, string visibleNode, out string ErrString)
        //{
        //    List<Pair> list = new List<Pair>();
          
        //    DataSet ds = new DataSet();
        //    DBConn.DBParam[] oip = new DBConn.DBParam[6];
           
        //    oip[0] = new DBConn.DBParam();
        //    oip[0].ParameterName = "in_nParentNode";
        //    oip[0].DbType = DBConn.DBTypeCustom.Number;
        //    oip[0].Direction = ParameterDirection.Input;
        //    oip[0].Value = 0;

        //    oip[1] = new DBConn.DBParam();
        //    oip[1].ParameterName = "in_nKey1";
        //    oip[1].DbType = DBConn.DBTypeCustom.Number;
        //    oip[1].Direction = ParameterDirection.Input;
        //    oip[1].Value = 0;

        //    oip[2] = new DBConn.DBParam();
        //    oip[2].ParameterName = "in_nKey2";
        //    oip[2].DbType = DBConn.DBTypeCustom.Number;
        //    oip[2].Direction = ParameterDirection.Input;
        //    oip[2].Value = 0;

        //    oip[3] = new DBConn.DBParam();
        //    //oip[3].ParameterName = "in_nMode";
        //    oip[3].ParameterName = "in_bFullTree";
        //    oip[3].DbType = DBConn.DBTypeCustom.Number;
        //    oip[3].Direction = ParameterDirection.Input;
        //    oip[3].Value = Convert.ToInt32(visibleNode); 

        //    oip[4] = new DBConn.DBParam();
        //    oip[4].ParameterName = "in_bwithchild";
        //    oip[4].DbType = DBConn.DBTypeCustom.Number;
        //    oip[4].Direction = ParameterDirection.Input;
        //    oip[4].Value = 1;

            
        //    oip[5] = new DBConn.DBParam();
        //    oip[5].ParameterName = "in_nCTX";
        //    oip[5].DbType = DBConn.DBTypeCustom.Int32;
        //    oip[5].Direction = ParameterDirection.Input;
        //    oip[5].Value = Convert.ToInt32(context);

        //   DataOracleHelper.ExecQuery(Query, ref oip, ref ds);

          

        //    ErrString = ""; //инитим выходной параметр
        //    try
        //    {
        //        string nkey = "";
        //        string cname = "";
        //        string nvalue1 = "";
        //        string nvalue2 = "";
        //        string caction = "";
        //        string bhaschild = "";


        //        //string TreeValue = "";
        //        //string TreeTitle = "";

        //        foreach (DataRow dtr in ds.Tables[0].Rows)
        //        {
        //            nkey = dtr[0].ToString().Trim();
        //            cname = dtr[1].ToString().Trim();

        //            string[] arr = cname.Split('#');
        //            if (arr.Length == 7)
        //            {
        //                string newText = cname;
        //                char[] chars = newText.ToCharArray();
        //                for (int ii = 0; ii < chars.Length; ii++)
        //                {
        //                    if (chars[ii] == 'z')
        //                    {
        //                        chars[ii] = '~';
        //                        break;
        //                    }
        //                }
        //                newText = new string(chars);
        //                cname = newText;
        //            }

        //            nvalue1 = dtr[2].ToString().Trim();
        //            nvalue2 = dtr[3].ToString().Trim();
        //            caction = dtr[4].ToString().Trim();
        //            bhaschild = dtr[5].ToString().Trim();
        //            list.Add(new Pair() { Key = nkey + "~" + nvalue1 + "~" + nvalue2, Texts = cname });
        //        }

               
               
        //    }
        //    catch (Exception ex)
        //    {
        //        log_passport.Error(ex);
        //        string s = ex.Message.ToString();
        //        ErrString = s;
        //        return null;
        //    }
        //    return list;
        //}

        //public List<Pair> GetNextNode(string con, string Query, string val, string nTreeKey, string context, string visibleNode)
        //{
        //    var result = new List<Pair>();
           
        //    List<Pair> list = new List<Pair>();

        //    try
        //    {

        //        string nkey;
        //        string nvalue1;
        //        string nvalue2;
        //        string[] ArrayKeys;

        //        int i = val.IndexOf("~");
        //        ArrayKeys = val.Split('~');
        //        if (i != -1)
        //        {
        //            nkey = ArrayKeys[0];
        //            nvalue1 = ArrayKeys[1];
        //            nvalue2 = ArrayKeys[2];
        //        }
        //        else
        //        {
        //            nkey = val;
        //            nvalue1 = "0";
        //            nvalue2 = "0";
        //        }

        //        DataSet ds = new DataSet();
        //        DBConn.DBParam[] oip = new DBConn.DBParam[5];

        //        oip[0] = new DBConn.DBParam();
        //        oip[0].ParameterName = "in_nParentNode";
        //        oip[0].DbType = DBConn.DBTypeCustom.Number;
        //        oip[0].Direction = ParameterDirection.Input;
        //        oip[0].Value = Convert.ToDouble(nkey);

        //        oip[1] = new DBConn.DBParam();
        //        oip[1].ParameterName = "in_nKey1";
        //        oip[1].DbType = DBConn.DBTypeCustom.Number;
        //        oip[1].Direction = ParameterDirection.Input;
        //        oip[1].Value = Convert.ToDouble(nvalue1);

        //        oip[2] = new DBConn.DBParam();
        //        oip[2].ParameterName = "in_nKey2";
        //        oip[2].DbType = DBConn.DBTypeCustom.Number;
        //        oip[2].Direction = ParameterDirection.Input;
        //        oip[2].Value = Convert.ToDouble(nvalue2);

        //        oip[3] = new DBConn.DBParam();
        //        oip[3].ParameterName = "in_bFullTree";
        //        oip[3].DbType = DBConn.DBTypeCustom.Number;
        //        oip[3].Direction = ParameterDirection.Input;
        //        oip[3].Value = Convert.ToInt32(visibleNode);

        //        oip[4] = new DBConn.DBParam();
        //        oip[4].ParameterName = "in_nCTX";
        //        oip[4].DbType = DBConn.DBTypeCustom.Int32;
        //        oip[4].Direction = ParameterDirection.Input;
        //        oip[4].Value = Convert.ToInt32(context);

        //        DataOracleHelper.ExecQuery(Query, ref oip, ref ds);

        //        if (ds.Tables[0].Rows.Count <= 1000)
        //        {
        //            //ErrString = ""; //инитим выходной параметр

        //            string TreeValue = "";
        //            string TreeTitle = "";

        //            foreach (DataRow dtr in ds.Tables[0].Rows)
        //            {
        //                string helpValue1;
        //                string helpValue2;
        //                string skey;
        //                string caction;
        //                string bhaschild;

        //                TreeValue = dtr[0].ToString();
        //                TreeTitle = dtr[1].ToString();

        //                string[] arr = TreeTitle.Split('#');
        //                if (arr.Length == 7)
        //                {
        //                    string newText = TreeTitle;
        //                    char[] chars = newText.ToCharArray();
        //                    for (int ii = 0; ii < chars.Length; ii++)
        //                    {
        //                        if (chars[ii] == 'z')
        //                        {
        //                            chars[ii] = '~';
        //                            break;
        //                        }
        //                    }

        //                    newText = new string(chars);
        //                    TreeTitle = newText;
        //                }

        //                helpValue1 = dtr[2].ToString();
        //                helpValue2 = dtr[3].ToString();
        //                caction = dtr[4].ToString();
        //                bhaschild = dtr[5].ToString();

        //                //добавление  корней дерева
        //                list.Add(new Pair()
        //                    {
        //                        Texts = TreeTitle,
        //                        Key = TreeValue + "~" + helpValue1 + "~" + helpValue2 + "~" + caction
        //                    });
        //            }





        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log_passport.Error(ex);
        //        //ErrString = "Ошибка при построении дерева: " + oraErr.ToString();
        //        return null;
        //    }
        //    return list;


        //} // end public List<Pair> GetNextNode

    }
}
//DbConnAuth dbConnAuth = new DbConnAuth();
//DBConn.Conn connOra = dbConnAuth.connOra();

//connOra.ConnectionString(ConfigurationManager.ConnectionStrings["DBConn"].ToString());
//DataSet ds2 = connOra.ExecuteQuery("Passport_SL_API.GetNodes", true, oip);
//ds = ds2;

#region Old Code
//OracleConnection MainOraConnection = new OracleConnection(con);
////создаём запрос к БД
//OracleCommand cmd = new OracleCommand(Query, MainOraConnection);
//cmd.CommandType = CommandType.StoredProcedure;
////создаем входной параметр

//cmd.Parameters.Add(new OracleParameter("in_nTreeKey", OracleType.Number));
//cmd.Parameters["in_nTreeKey"].Value = Convert.ToDouble(rootKey);

//cmd.Parameters.Add(new OracleParameter("in_nParentNode", OracleType.Number));
//cmd.Parameters["in_nParentNode"].Value = 0;

//cmd.Parameters.Add(new OracleParameter("in_nKey1", OracleType.Number));
//cmd.Parameters["in_nKey1"].Value = 0;

//cmd.Parameters.Add(new OracleParameter("in_nKey2", OracleType.Number));
//cmd.Parameters["in_nKey2"].Value = 0;

//cmd.Parameters.Add(new OracleParameter("in_nMode", OracleType.Number));
//cmd.Parameters["in_nMode"].Value = 0;

//cmd.Parameters.Add(new OracleParameter("in_bwithchild", OracleType.Number));
//cmd.Parameters["in_bwithchild"].Value = 1;

////создаём выходной параметр
//cmd.Parameters.Add(new OracleParameter("result", OracleType.Cursor));
//cmd.Parameters["result"].Direction = ParameterDirection.ReturnValue;
#endregion
//создаём подключение
//string con = WebConfigurationManager.ConnectionStrings["OraConnectionString"].ConnectionString;
#region Add Context

//DbConnAuth dbConnAuth = new DbConnAuth();
//DBConn.Conn connOra = dbConnAuth.connOra();

//connOra.ConnectionString(WebConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);
//DBConn.DBParam[] oip1 = new DBConn.DBParam[1];

//oip1[0] = new DBConn.DBParam();
//oip1[0].ParameterName = "in_ncontext";
//oip1[0].DbType = DBConn.DBTypeCustom.Number;
//oip1[0].Value = context;

//IDbConnection dbConnection =
//    connOra.Connect(WebConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);

//connOra.ExecuteNonQuery(dbConnection, "Passport_SL_API.setContext", oip1);
//connOra.Disconnect(dbConnection);

#endregion
#region Old Code
//MainOraConnection.Open();

#region Устанавливаем в БД сессию текущего пользователя.
//OracleConnection conn = DataOracleHelper.GetOracleConnection();

//oracleEngine.SetCurrentSession(conn, new AuthorisationAPP(UserSession.UserGuid, UserSession.IpAdress,
//                               UserSession.CompName, UserSession.SessionKey, UserSession.BrowserName));
//LogonBasa.DoUserLog(HttpContext.Current.Session["KeyUser"].ToString(), HttpContext.Current.Session["IpAdress"].ToString());
#endregion



//OracleDataReader reader = cmd.ExecuteReader();
//while (reader.Read())
//{
//    nkey = reader.GetValue(0).ToString().Trim();
//    cname = reader.GetValue(1).ToString().Trim();
//    nvalue1 = reader.GetValue(2).ToString().Trim();
//    nvalue2 = reader.GetValue(3).ToString().Trim();
//    caction = reader.GetValue(4).ToString().Trim();
//    bhaschild = reader.GetValue(5).ToString().Trim();
//    list.Add(new Pair() { Key = nkey + "z" + nvalue1 + "z" + nvalue2, Texts = cname });


//    //TreeValue = reader.GetValue(0).ToString();
//    //TreeTitle = reader.GetValue(1).ToString();
//    //helpValue1 = reader.GetValue(2).ToString();
//    //helpValue2 = reader.GetValue(3).ToString();
//    //caction = reader.GetValue(4).ToString();
//    //list.Add(new Pair() { Texts = TreeTitle, Key = TreeValue + "z" + helpValue1 + "z" + helpValue2 + "z" + caction });

//}
//reader.Close();
#endregion