using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Configuration;
using System.Web.UI;
using log4net;


public partial class ServiceData : IServiceData
{
    // private static readonly ILog log_passport = LogManager.GetLogger(typeof(treeNodeReader).Name);

    public PairAll CreateRootOfTree(string rootKey, string context, string visibleNode)
        {
           
            var list = new PairAll();
            
            list.PairItemList = new List<PairItem>();

            DataSet ds = new DataSet();
            try
            {

            DBConn.DBParam[] oip = new DBConn.DBParam[6];

            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nParentNode";
            oip[0].DbType = DBConn.DBTypeCustom.Number;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = 0;

            oip[1] = new DBConn.DBParam();
            oip[1].ParameterName = "in_nKey1";
            oip[1].DbType = DBConn.DBTypeCustom.Number;
            oip[1].Direction = ParameterDirection.Input;
            oip[1].Value = 0;

            oip[2] = new DBConn.DBParam();
            oip[2].ParameterName = "in_nKey2";
            oip[2].DbType = DBConn.DBTypeCustom.Number;
            oip[2].Direction = ParameterDirection.Input;
            oip[2].Value = 0;

            oip[3] = new DBConn.DBParam();
            //oip[3].ParameterName = "in_nMode";
            oip[3].ParameterName = "in_bFullTree";
            oip[3].DbType = DBConn.DBTypeCustom.Number;
            oip[3].Direction = ParameterDirection.Input;
            oip[3].Value = Convert.ToInt32(visibleNode);

            oip[4] = new DBConn.DBParam();
            oip[4].ParameterName = "in_bwithchild";
            oip[4].DbType = DBConn.DBTypeCustom.Number;
            oip[4].Direction = ParameterDirection.Input;
            oip[4].Value = 1;


            oip[5] = new DBConn.DBParam();
            oip[5].ParameterName = "in_nCTX";
            oip[5].DbType = DBConn.DBTypeCustom.Int32;
            oip[5].Direction = ParameterDirection.Input;
            oip[5].Value = Convert.ToInt32(context);

            DataOracleHelper.ExecQuery(Query.TreeNode, ref oip, ref ds);

                string nkey = "";
                string cname = "";
                string nvalue1 = "";
                string nvalue2 = "";
                string caction = "";
                string bhaschild = "";


                //string TreeValue = "";
                //string TreeTitle = "";
                if (ds.Tables[0].Rows.Count != 0)
                {

                    list.ErrorMessage = "начало дерева данные Passport_SL_API.GetNodes in_nParentNode = 0, in_nKey1 = 0, in_nKey2 = 0, in_bFullTree = " + visibleNode +
                    "in_bwithchild = 1, in_nCTX= " + context;

                    foreach (DataRow dtr in ds.Tables[0].Rows)
                    {
                        nkey = dtr["nnodeKey"].ToString().Trim();
                        cname = dtr["cName"].ToString().Trim();
                        //nkey = dtr[0].ToString().Trim();
                        //cname = dtr[1].ToString().Trim();

                        string[] arr = cname.Split('#');
                        if (arr.Length == 7)
                        {
                            string newText = cname;
                            char[] chars = newText.ToCharArray();
                            for (int ii = 0; ii < chars.Length; ii++)
                            {
                                if (chars[ii] == 'z')
                                {
                                    chars[ii] = '~';
                                    break;
                                }
                            }
                            newText = new string(chars);
                            cname = newText;
                        }

                        nvalue1 = dtr["nValue1"].ToString().Trim();
                        nvalue2 = dtr["nValue2"].ToString().Trim();
                        caction = dtr["cAction"].ToString().Trim();
                        bhaschild = dtr["bHasChild"].ToString().Trim();
                        //nvalue1 = dtr[2].ToString().Trim();
                        //nvalue2 = dtr[3].ToString().Trim();
                        //caction = dtr[4].ToString().Trim();
                        //bhaschild = dtr[5].ToString().Trim();
                        list.PairItemList.Add(
                            new PairItem
                            {
                                Key = nkey + "~" + nvalue1 + "~" + nvalue2,
                                Texts = cname
                            }
                    );
                        // list.Add(new Pair() { Key = nkey + "~" + nvalue1 + "~" + nvalue2, Texts = cname });

                    }
                    
                }
                else
                {
                    list.ErrorMessage = "начало дерева нет данных -  Passport_SL_API.GetNodes in_nParentNode = 0, in_nKey1 = 0, in_nKey2 = 0, in_bFullTree = " + visibleNode +
                    "in_bwithchild = 1, in_nCTX= " + context ;
                }

            }
            catch (Exception ex)
            {
               
                log_passport.Error(ex);
                list.IsValid = false;
                list.ErrorMessage = "начало дерева Passport_SL_API.GetNodes in_nParentNode = 0, in_nKey1 = 0, in_nKey2 = 0, in_bFullTree = " + visibleNode +
                    "in_bwithchild = 1, in_nCTX= " + context + ex.Message;
            }
            return list;
        }

    public PairAll GetNextNode(string val, string nTreeKey, string context, string visibleNode, string keyPassport)
    {
        var list = new PairAll();
        list.PairItemList = new List<PairItem>();

        string nkey = string.Empty;
        string nvalue1 = string.Empty;
        string nvalue2 = string.Empty;
        string[] ArrayKeys;

       // List<Pair> list = new List<Pair>();
        try
        {
            int i = val.IndexOf("~");
            ArrayKeys = val.Split('~');
            if (i != -1)
            {
                nkey = ArrayKeys[0];
                nvalue1 = ArrayKeys[1];
                nvalue2 = ArrayKeys[2];
            }
            else
            {
                nkey = val;
                nvalue1 = "0";
                nvalue2 = "0";
            }
            Int64 t1 = Convert.ToInt64(nvalue1);
            Int64 t2 = Convert.ToInt64(nvalue2);
            DataSet ds = new DataSet();
            DBConn.DBParam[] oip = new DBConn.DBParam[5];

            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nParentNode";
            oip[0].DbType = DBConn.DBTypeCustom.Int64;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = Convert.ToInt64(nkey);

            oip[1] = new DBConn.DBParam();
            oip[1].ParameterName = "in_nKey1";
            oip[1].DbType = DBConn.DBTypeCustom.Int64;
            oip[1].Direction = ParameterDirection.Input;
            oip[1].Value = t1;

            oip[2] = new DBConn.DBParam();
            oip[2].ParameterName = "in_nKey2";
            oip[2].DbType = DBConn.DBTypeCustom.Int64;
            oip[2].Direction = ParameterDirection.Input;
            oip[2].Value =t2;

            oip[3] = new DBConn.DBParam();
            oip[3].ParameterName = "in_bFullTree";
            oip[3].DbType = DBConn.DBTypeCustom.Number;
            oip[3].Direction = ParameterDirection.Input;
            oip[3].Value = Convert.ToInt32(visibleNode);

            oip[4] = new DBConn.DBParam();
            oip[4].ParameterName = "in_nCTX";
            oip[4].DbType = DBConn.DBTypeCustom.Int32;
            oip[4].Direction = ParameterDirection.Input;
            oip[4].Value = Convert.ToInt32(context);

            DataOracleHelper.ExecQuery(Query.TreeNode, ref oip, ref ds);


            string treeValue = "";
            string treeTitle = "";

            list.ErrorMessage = "клик по дереву Passport_SL_API.GetNodes in_nParentNode = " + nkey + ", in_nKey1 = "+ t1+ ", in_nKey2 = "+t2+", in_bFullTree = " + visibleNode +
                    ", in_nCTX= " + context;

            if (ds.Tables[0].Rows.Count <= 1000)
            {
                //ErrString = ""; //инитим выходной параметр

                

                foreach (DataRow dtr in ds.Tables[0].Rows)
                {
                    string helpValue1;
                    string helpValue2;
                    string skey;
                    string caction;
                    string bhaschild;

                    //treeValue = dtr[0].ToString();
                    //treeTitle = dtr[1].ToString();

                    treeValue = dtr["nnodeKey"].ToString().Trim();
                    treeTitle = dtr["cName"].ToString().Trim();

                    string[] arr = treeTitle.Split('#');
                    if (arr.Length == 7)
                    {
                        string newText = treeTitle;
                        char[] chars = newText.ToCharArray();
                        for (int ii = 0; ii < chars.Length; ii++)
                        {
                            if (chars[ii] == 'z')
                            {
                                chars[ii] = '~';
                                break;
                            }
                        }

                        newText = new string(chars);
                        treeTitle = newText;
                    }

                    //helpValue1 = dtr[2].ToString();
                    //helpValue2 = dtr[3].ToString();
                    //caction = dtr[4].ToString();
                    //bhaschild = dtr[5].ToString();

                    helpValue1 = dtr["nValue1"].ToString();
                    helpValue2 = dtr["nValue2"].ToString();
                    caction = dtr["cAction"].ToString();
                    bhaschild = dtr["bHasChild"].ToString();


                    //добавление  корней дерева

                    list.PairItemList.Add(
                        new PairItem
                            {
                                Key = treeValue + "~" + helpValue1 + "~" + helpValue2 + "~" + caction,
                                Texts = treeTitle
                            }
                            );
                }
            }
            else
            {
                foreach (DataRow dtr in ds.Tables[0].Rows)
                {
                   // if (dtr[2].ToString() == keyPassport)
                    if (dtr["nValue1"].ToString() == keyPassport)
                    {
                        string helpValue1;
                        string helpValue2;
                        string skey;
                        string caction;
                        string bhaschild;

                        //treeValue = dtr[0].ToString();
                        //treeTitle = dtr[1].ToString();

                        treeValue = dtr["nnodeKey"].ToString().Trim();
                        treeTitle = dtr["cName"].ToString().Trim();

                        string[] arr = treeTitle.Split('#');
                        if (arr.Length == 7)
                        {
                            string newText = treeTitle;
                            char[] chars = newText.ToCharArray();
                            for (int ii = 0; ii < chars.Length; ii++)
                            {
                                if (chars[ii] == 'z')
                                {
                                    chars[ii] = '~';
                                    break;
                                }
                            }

                            newText = new string(chars);
                            treeTitle = newText;
                        }

                        //helpValue1 = dtr[2].ToString();
                        //helpValue2 = dtr[3].ToString();
                        //caction = dtr[4].ToString();
                        //bhaschild = dtr[5].ToString();

                        helpValue1 = dtr["nValue1"].ToString();
                        helpValue2 = dtr["nValue2"].ToString();
                        caction = dtr["cAction"].ToString();
                        bhaschild = dtr["bHasChild"].ToString();

                        //добавление  корней дерева

                        list.PairItemList.Add(
                            new PairItem
                            {
                                Key = treeValue + "~" + helpValue1 + "~" + helpValue2 + "~" + caction,
                                Texts = treeTitle
                            }
                                );
                    }
                }
            }

        }
        catch (Exception ex)
        {
             log_passport.Error(ex);
                list.IsValid = false;
                list.ErrorMessage = " клик по ветке дерева Passport_SL_API.GetNodes in_nParentNode = " + nkey + ", in_nKey1 = " +nvalue1+
                    ", in_nKey2 = " + nvalue2 + ", in_bFullTree = " + visibleNode +
                    "  in_nCTX= " + context + ex.Message;
        }
        return list;


    } // end public List<Pair> GetNextNode

    //public List<Pair> CreateRootOfTree(string rootKey, string context, string visibleNode)
    //{
    //    string conn, query, ErrString;
    //    conn = DataOracleHelper.GetConnectionString(); //WebConfigurationManager.ConnectionStrings["Ora_meta"].ConnectionString;
    //    query = Query.TreeNode;

    //    return new TreeLinks.treeNodeReader().CreateRootOfTree(conn, query, rootKey, context, visibleNode, out ErrString);
    //}

    //public List<Pair> GetNextNode(string key, string nTreeKey, string context, string visibleNode)
    //{
    //    string conn, query;
    //    conn = DataOracleHelper.GetConnectionString(); //WebConfigurationManager.ConnectionStrings["Ora_meta"].ConnectionString;
    //    query = Query.TreeNode;
    //    return new TreeLinks.treeNodeReader().GetNextNode(conn, query, key, nTreeKey, context, visibleNode);
    //}
}
