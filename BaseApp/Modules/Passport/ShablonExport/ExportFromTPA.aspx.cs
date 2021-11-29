using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using App_Code.Passport_API;

public partial class Modules_Passport_ShablonExport_ExportFromTPA : System.Web.UI.Page
{
    private string keytpa0 = "";
    private string keytpa1 = "";
    private string keytpa2 = "";
    private int numberRow = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
                GetData(Request.QueryString["passportIDParent"].ToString());     
        }
        else
        {
            btnLoadIntoWord.Visible = false;

            //bind data to data bound controls and do other stuff

            Response.Clear(); //this clears the Response of any headers or previous output
            Response.Buffer = true; //make sure that the entire output is rendered simultaneously

            //Set content type to MS Excel sheet
            //Use "application/msword" for MS Word doc files
            //"application/pdf" for PDF files

            //Response.ContentType = "application/vnd.ms-excel";
            Response.ContentType = "application/msword";

            StringWriter stringWriter = new StringWriter(); //System.IO namespace should be used

            HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);

            //Render the entire Page control in the HtmlTextWriter object
            //We can render individual controls also, like a DataGrid to be
            //exported in custom format (excel, word etc)
            this.RenderControl(htmlTextWriter);
            Response.Write(stringWriter.ToString());
            Response.End();
        }
    }

    protected void btnLoadIntoWord_Click(object sender, EventArgs e)
    {

    }
    private void GetData(string key)
    {
        try
        {
            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(WebConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);
            DataSet ds = new DataSet();
            DBConn.DBParam[] oip = new DBConn.DBParam[5];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nEntityKey";
            oip[0].DbType = DBConn.DBTypeCustom.Double;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = 15936672;

            oip[1] = new DBConn.DBParam();
            oip[1].ParameterName = "in_nObjectKey";
            oip[1].DbType = DBConn.DBTypeCustom.Number;
            oip[1].Direction = ParameterDirection.Input;
            oip[1].Value = 0;

            oip[2] = new DBConn.DBParam();
            oip[2].ParameterName = "in_nParentObjectKey";
            oip[2].DbType = DBConn.DBTypeCustom.Double;
            oip[2].Direction = ParameterDirection.Input;
            oip[2].Value = Convert.ToDouble(key);

            oip[3] = new DBConn.DBParam();
            oip[3].ParameterName = "in_nView";
            oip[3].DbType = DBConn.DBTypeCustom.Number;
            oip[3].Direction = ParameterDirection.Input;
            oip[3].Value = 1;

            oip[4] = new DBConn.DBParam();
            oip[4].ParameterName = "in_nCTX";
            oip[4].DbType = DBConn.DBTypeCustom.Number;
            oip[4].Direction = ParameterDirection.Input;
            oip[4].Value = 10;

            DataOracleHelper.ExecQuery(OracleQuerys.GetObjForEntityKeyQuery, ref oip, ref ds);

            
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                     //if (!string.IsNullOrEmpty(dr["CNAME"].ToString()))
                   
                    if (row["NAZNACHENIE"].ToString() == "линейная")
                    {
                        numberRow = 1;
                        lblFactoryNuber.Text = row["CNAME"].ToString();
                        lblZraType.Text = row["TIP_TPA"].ToString();
                        lblSposobPrisoedineniya.Text = row["SPOS_PRIS_K_TR_TRUBO"].ToString();
                        lblZapornOrganType.Text = "";
                        lblGostZra.Text = row["STANDART_IZGOTOVLENI"].ToString();
                        lblFactoryVender.Text = ""; // row["NZAVODIZGOTOVITEL"].ToString(); пока не работает - передается ключ
                        lblCountry.Text = "";
                        lblFactoryNumber.Text = row["ZAVODSKOI_NOMER"].ToString();
                        lblDiamUslovnyi.Text = row["USLO_DIAM_NA_V_VIHOD"].ToString();
                        lblDavlRab.Text = row["RABOCHEE_DAVLENIE"].ToString();
                        lblPrim.Text = row["PRIM"].ToString();
                        lbldateEkcpl.Text = row["DATA_VVODA_V_EKSPLUATACIYU"].ToString();
                        keytpa0 = row["ARTIFACT_ID"].ToString();
                        GetDataPrivod(keytpa0, 0);
                        GetDataZOrgan(keytpa0);

                    }
                    else
                    {
                        if (numberRow == 1)
                        {
                            numberRow = 2;
                            lblNameTPA1.Text = row["CNAME"].ToString();
                            lblTypeTPA.Text = row["TIP_TPA"].ToString();
                            lblDiam.Text = row["USLO_DIAM_NA_V_VIHOD"].ToString();
                            lblNAznTPA.Text = row["NAZNACHENIE"].ToString();
                            keytpa1 = row["ARTIFACT_ID"].ToString();
                            GetDataPrivod(keytpa1, 1);
                        }
                        else
                        {
                            if (numberRow == 2)
                            {
                                lblNameTPA2.Text = row["CNAME"].ToString();
                                lblTypeTPA2.Text = row["TIP_TPA"].ToString();
                                lblDiam2.Text = row["USLO_DIAM_NA_V_VIHOD"].ToString();
                                lblNaznTPA2.Text = row["NAZNACHENIE"].ToString();
                                keytpa2 = row["ARTIFACT_ID"].ToString();
                                GetDataPrivod(keytpa2, 2);
                            } 
                        }
                        
                    }
                }
            }

            GetPassportInfo(keytpa0);

        }
        catch (Exception e)
        {
            string tt = e.ToString();
        }

    }

    //данные по приводу(дочерний объект)
    private void GetDataPrivod(string key, int numberRowtpa)
    {
        try
        {
            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(WebConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);
            DataSet ds = new DataSet();
            DBConn.DBParam[] oip = new DBConn.DBParam[5];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nEntityKey";
            oip[0].DbType = DBConn.DBTypeCustom.Number;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = 15939672;

            oip[1] = new DBConn.DBParam();
            oip[1].ParameterName = "in_nObjectKey";
            oip[1].DbType = DBConn.DBTypeCustom.Number;
            oip[1].Direction = ParameterDirection.Input;
            oip[1].Value = 0;

            oip[2] = new DBConn.DBParam();
            oip[2].ParameterName = "in_nParentObjectKey";
            oip[2].DbType = DBConn.DBTypeCustom.Number;
            oip[2].Direction = ParameterDirection.Input;
            oip[2].Value = Convert.ToDouble(key);

            oip[3] = new DBConn.DBParam();
            oip[3].ParameterName = "in_nView";
            oip[3].DbType = DBConn.DBTypeCustom.Number;
            oip[3].Direction = ParameterDirection.Input;
            oip[3].Value = 1;

            oip[4] = new DBConn.DBParam();
            oip[4].ParameterName = "in_nCTX";
            oip[4].DbType = DBConn.DBTypeCustom.Number;
            oip[4].Direction = ParameterDirection.Input;
            oip[4].Value = 10;

            DataOracleHelper.ExecQuery(OracleQuerys.GetObjForEntityKeyQuery, ref oip, ref ds);


            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    switch (numberRowtpa)
                    {
                        case 0:
                            {
                                lblSposobUprPriv3.Text = row["SPOSOB_UPRAVLENIYA"].ToString();
                                lblVidUprPriv.Text = row["VID_UPRAVLENIYA"].ToString();
                                break;
                            }
                        case 1:
                            {
                                lblSposobUpr.Text = row["SPOSOB_UPRAVLENIYA"].ToString();
                                break;
                            }
                        case 2:
                            {
                                lblSposobUpr2.Text = row["SPOSOB_UPRAVLENIYA"].ToString();
                                break;
                            }

                    }
                   
                }
            }

        }
        catch (Exception e)
        {
            string tt = e.ToString();
        }
    }
    //данные по запорному органу - дочерний объект
    private void GetDataZOrgan(string key)
    {
        try
        {
            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(WebConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);
            DataSet ds = new DataSet();
            DBConn.DBParam[] oip = new DBConn.DBParam[5];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nEntityKey";
            oip[0].DbType = DBConn.DBTypeCustom.Number;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = 15936678;

            oip[1] = new DBConn.DBParam();
            oip[1].ParameterName = "in_nObjectKey";
            oip[1].DbType = DBConn.DBTypeCustom.Number;
            oip[1].Direction = ParameterDirection.Input;
            oip[1].Value = 0;

            oip[2] = new DBConn.DBParam();
            oip[2].ParameterName = "in_nParentObjectKey";
            oip[2].DbType = DBConn.DBTypeCustom.Number;
            oip[2].Direction = ParameterDirection.Input;
            oip[2].Value = Convert.ToDouble(key);

            oip[3] = new DBConn.DBParam();
            oip[3].ParameterName = "in_nView";
            oip[3].DbType = DBConn.DBTypeCustom.Number;
            oip[3].Direction = ParameterDirection.Input;
            oip[3].Value = 1;

            oip[4] = new DBConn.DBParam();
            oip[4].ParameterName = "in_nCTX";
            oip[4].DbType = DBConn.DBTypeCustom.Number;
            oip[4].Direction = ParameterDirection.Input;
            oip[4].Value = 10;

            DataOracleHelper.ExecQuery(OracleQuerys.GetObjForEntityKeyQuery, ref oip, ref ds);


            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    lblZapornOrganType.Text = row["ROD_ZAPORN_ORGANA"].ToString();
                }
            }

        }
        catch (Exception e)
        {
            string tt = e.ToString();
        }
    }

    private void GetPassportInfo(string inObjKey)
    {
        try
        {
            DBConn.DBParam[] oip = new DBConn.DBParam[9];

            oip[8] = new DBConn.DBParam();
            oip[8].ParameterName = "in_nCTX";
            oip[8].DbType = DBConn.DBTypeCustom.Number;
            oip[8].Direction = ParameterDirection.Input;
            oip[8].Value = 10;

            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nObjectKey";
            oip[0].DbType = DBConn.DBTypeCustom.Int64;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = Convert.ToDouble(inObjKey);

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



            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();

            connOra.ConnectionString(ConfigurationManager.ConnectionStrings["DBConn"].ToString());
            DataSet ds = connOra.ExecuteQuery(OracleQuerys.GetFullNameParentObjQuery, false, oip);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row["Name"].ToString() == "out_cparentobjectname")
                    {
                        lblPlaseState.Text = row["Value"].ToString();
                    }
                }
            }
            

        }
        catch (Exception e)
        {
            string tt = e.ToString();
        }
    }
    
}