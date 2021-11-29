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

public partial class Modules_Passport_ShablonExport_ExportFromUKZ : System.Web.UI.Page
{

    private string keyUKZ = "";
    private string keyparentUkz = "";
    private string keyParentSistZah = "";
    private string keyNit = "";
    private string keyZachPokr = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetData(Request.QueryString["passportIDParent"].ToString());  
        }
        else
        {
            btnLoadIntoWord.Visible = false;

            Response.Clear(); //this clears the Response of any headers or previous output
            Response.Buffer = true; //make sure that the entire output is rendered simultaneously
            //Response.ContentType = "application/vnd.ms-excel";
            Response.ContentType = "application/msword";

            StringWriter stringWriter = new StringWriter(); //System.IO namespace should be used
            HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);

            this.RenderControl(htmlTextWriter);
            Response.Write(stringWriter.ToString());
            Response.End();
        }
    }

    //общая часть
    private void GetData(string key)
    {
        try
        {
            DataSet ds = new DataSet();
            //данные по укз
            //ds = GetGridDate(key, 15936870);  //укз
            ds = GetdataPassport(key);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    lblNumberUKZ.Text = row["NOMER_UKZ"].ToString();
                    lblNumber.Text = row["NOMER_UKZ"].ToString();
                    lblEkcplKm.Text = row["EKSPLUATA_KILOMETRAJ"].ToString();
                    lblDiamTr.Text = "";
                    lblIzolTp.Text = "";
                    lblproektOrg.Text = row["NPROEKTNAY_ORGANIZACI"].ToString();
                    lblDataMont.Text = row["DATA_MONTAJA"].ToString();
                    lblDataEkcpl.Text = row["DATA_VVODA_V_EKSPLUATACIYU"].ToString();
                    keyUKZ = row["NPGTS_UKZKEY"].ToString();

                    GetDataKatodSt(keyUKZ);
                    GetDataDrenagKatod(keyUKZ);
                    GetDataAnod(keyUKZ);

                }
            }

            PassportInfo(keyUKZ, 1);

        }
        catch (Exception e)
        {
            string tt = e.ToString();
        }

    }



    private DataSet GetdataPassport(string passportKey)
    {
        DataSet ds = new DataSet();
        try
        {
            
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
            oip[1].DbType = DBConn.DBTypeCustom.Number;                     //.Int32
            oip[1].Direction = ParameterDirection.Input;
            oip[1].Value = Convert.ToDouble(passportKey);                   //ToInt32(passportKey);

            oip[2] = new DBConn.DBParam();
            //oip[2].ParameterName = "inParentKey";
            oip[2].ParameterName = "in_nParentObjectKey";
            oip[2].DbType = DBConn.DBTypeCustom.Number;                     //.Int32
            oip[2].Direction = ParameterDirection.Input;
            oip[2].Value = Convert.ToDouble("0");                           //.ToInt32("0");

            oip[3] = new DBConn.DBParam();
            //oip[3].ParameterName = "intypePassport";
            oip[3].ParameterName = "in_nView";
            oip[3].DbType = DBConn.DBTypeCustom.Number;                     //.Int32
            oip[3].Direction = ParameterDirection.Input;
            oip[3].Value = 1;                //ToInt32(intypePassport);

            oip[4] = new DBConn.DBParam();
            oip[4].ParameterName = "in_nCTX";
            oip[4].DbType = DBConn.DBTypeCustom.Number;                     //.Int32
            oip[4].Direction = ParameterDirection.Input;
            oip[4].Value = 10;

            DataOracleHelper.ExecQuery(OracleQuerys.GetObjForEntityKeyQuery, ref oip, ref ds);

        }
        catch (Exception e)
        {
            string tt = e.ToString();
        }

        return ds;
       
    }
    //возвращает данные
    private DataSet GetGridDate(string in_nParentObjectKey, int in_nEntityKey)
    {

        DataSet ds = new DataSet();

        try
        {
            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(WebConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);

            DBConn.DBParam[] oip = new DBConn.DBParam[5];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nEntityKey";
            oip[0].DbType = DBConn.DBTypeCustom.Number;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = in_nEntityKey;

            oip[1] = new DBConn.DBParam();
            oip[1].ParameterName = "in_nObjectKey";
            oip[1].DbType = DBConn.DBTypeCustom.Number;
            oip[1].Direction = ParameterDirection.Input;
            oip[1].Value = 0;

            oip[2] = new DBConn.DBParam();
            oip[2].ParameterName = "in_nParentObjectKey";
            oip[2].DbType = DBConn.DBTypeCustom.Number;
            oip[2].Direction = ParameterDirection.Input;
            oip[2].Value = Convert.ToDouble(in_nParentObjectKey);

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

        }
        catch (Exception e)
        {
            string tt = e.ToString();
        }

        return ds;
    }

    //место установки, ключ парента
    private DataSet GetPassportInfo1(string inObjKey)
    {
        DataSet ds = new DataSet();
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
            ds = connOra.ExecuteQuery(OracleQuerys.GetFullNameParentObjQuery, false, oip);

        }
        catch (Exception e)
        {
            string tt = e.ToString();
        }
        return ds;
    }

   

    //место установки ukz, ключ парента укз
    private void PassportInfo(string inObjKey, int ukz)
    {
        string keyEntity = "";
        try
        {
            DataSet ds = new DataSet();
            ds = GetPassportInfo1(inObjKey);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row["Name"].ToString() == "out_nParentEntityKey")
                    {
                        keyEntity = row["Value"].ToString();
                    }

                    if (row["Name"].ToString() == "out_nParentObjectKey")
                    {
                        keyparentUkz = row["Value"].ToString();
                      
                    }

                    if (ukz == 1)
                    {
                        if (row["Name"].ToString() == "out_cparentobjectname")
                        {
                            lblPlaceState.Text = row["Value"].ToString();
                        }
                    }
                   
                }
            }

            while (keyEntity != "15939654")//15939654 нитка(43315 тирасполь)
                        {
                            DataSet ds2 = new DataSet();
                            ds2 = GetPassportInfo1(keyparentUkz);
                            if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                                foreach (DataRow row2 in ds2.Tables[0].Rows)
                                {
                                    if (row2["Name"].ToString() == "out_nParentObjectKey")
                                    {
                                        keyNit = row2["Value"].ToString();
                                    }
                                    if (row2["Name"].ToString() == "out_nParentEntityKey")
                                    {
                                        keyEntity = row2["Value"].ToString();
                                    }
                                }
                        }

            if (!String.IsNullOrEmpty(keyNit))
            {
                DataSet dsNit = new DataSet();

                //dsNit = GetGridDate(keyNit, 43315); GetdataPassport(string passportKey, int intypePassport)

                dsNit = GetdataPassport(keyNit);

                if (dsNit != null && dsNit.Tables.Count > 0 &&
                    dsNit.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow rowNit in dsNit.Tables[0].Rows)
                    {

                        lblDiamTr.Text = rowNit["DIAMETR_OSNOVNOY"].ToString();
                        DataSet dsZA = new DataSet();
                        dsZA = GetGridDate(keyNit, 15936624); //защитное покрытие  15936624(40663 тирасполь)
                        if (dsZA != null && dsZA.Tables.Count > 0 && dsZA.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow rowZA in dsZA.Tables[0].Rows)
                            {
                                lblIzolTp.Text = rowZA["VID_ZASHIT_POKRITIYA"].ToString();
                            }
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

  //дочерние объекты - катодная станция
    private void GetDataKatodSt(string key)
    {
        try
        {
            DataSet ds = new DataSet();

            ds = GetGridDate(key, 15936876); //станция катодной защиты 15936876 (40879 тирасполь)
           
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    lblTypeSkz.Text = row["TIP"].ToString();
                    lblYearProduction.Text = row["GOD_IZGOTOVLENIYA"].ToString();
                    lblFactoryNumber.Text = row["ZAVODSKOY_NOMER"].ToString();
                    lblPrim1.Text = row["PRIMECHANIE"].ToString();
                }
            }

        }
        catch (Exception e)
        {
            string tt = e.ToString();
        }
    }

    //дочерний - дренажная катодная линия
    private void GetDataDrenagKatod(string key)
    {
        string keydrenagK = "";
        try
        {
            DataSet ds = new DataSet();

            ds = GetGridDate(key, 15936888); //15936888 дренажная катодная линия (40903 тирасполь)

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    lblVidProhElectr.Text = row["VID_LINII"].ToString();
                    lblDlina.Text = row["DLINA"].ToString();
                    keydrenagK = row["ARTIFACT_ID"].ToString();
                }
            }

            if (!String.IsNullOrEmpty(keydrenagK))
            {
                DataSet dsProvodKabel = new DataSet();
                dsProvodKabel = GetGridDate(keydrenagK, 15937626); //15937626 провод кабель (43357 тирасполь)
                if (dsProvodKabel != null && dsProvodKabel.Tables.Count > 0 && dsProvodKabel.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row2 in dsProvodKabel.Tables[0].Rows)
                    {
                        lblMarkaKab.Text = row2["MARKA_PROVOD_KABELYA"].ToString(); ;
                        lblSechPr.Text = row2["SECHEN_PROVOD_KABELY"].ToString(); ;
                        lblCoprotiv.Text = row2["UDELNOE_SOPROTIVLENI"].ToString();
                        break;
                    }
                }
            }
            
        }
        catch (Exception e)
        {
            string tt = e.ToString();
        }
    }

    //анодное заземление
    private void GetDataAnod(string key)
    {
        string typeAz = "";
        try
        {
            DataSet ds = new DataSet();

            ds = GetGridDate(key, 15938958); //15938958 анодное заземление (40885 тирасполь)

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    lblMaterial.Text = "";
                    lblTypeAnodZ.Text = row["TIP"].ToString();
                    lblNumberElectr.Text = row["KOLICHEST_ELEKTRODOV"].ToString();
                    lblLenghtAnod.Text = row["DLINA_ANODNO_KONSTRU"].ToString();
                    lblDataIzmAZ.Text = row["RASSTO_MEJDU__ELEKTR"].ToString();
                    lblKonstrukt.Text = row["KONSTRUKT_OSOBENNOST"].ToString();
                    lblTypeAktiv.Text = row["TIP_AKTIVATORA"].ToString();
                    lblSoprotiv.Text = row["UD_SO_GR_V__ME_RA_AZ"].ToString();
                    lblDateIzmA.Text = row["DATA_IZME_SOPR_AZ"].ToString();
                    lblSoprotRast.Text = row["SOPROT_RASTEK_AZ"].ToString();
                    lblDataIzmAZ.Text = row["DATA_IZME_UDEL_SOPRO"].ToString(); //lblDepth
                    if (row["TIP"].ToString() == "Глубинное")
                    {
                       DataSet dsSkv = GetGridDate(key, 15938958); //15938958 анодное заземление (40885 тирасполь)

                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row2 in ds.Tables[0].Rows)
                            {
                            }
                        }
                    }
                    else
                    {
                        lblDepth.Text = row["GLU_ZAL_AZ__KRO_GLUB"].ToString();
                    }

                }
            }
            //если тип -  глубинное то глубину находим по скважине
            

        }
        catch (Exception e)
        {
            string tt = e.ToString();
        }
    }
    
    protected void btnLoadIntoWord_Click(object sender, EventArgs e)
    {

    }
}

////ключ парента укз
//keyparentUkz = row["Value"].ToString();

////по парентах находим ключ нити
//if (!String.IsNullOrEmpty(keyparentUkz))
//{
//    DataSet ds2 = new DataSet();

//    ds2 = GetPassportInfo1(keyparentUkz);

//    if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
//    {
//        foreach (DataRow row2 in ds2.Tables[0].Rows)
//        {

//            if (row2["Name"].ToString() == "out_nParentObjectKey")
//            {
//                //ключ нити
//                keyNit = row2["Value"].ToString();

//                // if ((row2["Name"].ToString() == "out_nParentObjectKey") row["Name"].ToString() == "out_nParentEntityKey"


//                if (!String.IsNullOrEmpty(keyNit))
//                {
//                    DataSet dsNit = new DataSet();

//                    //dsNit = GetGridDate(keyNit, 43315); GetdataPassport(string passportKey, int intypePassport)

//                    dsNit = GetdataPassport(keyNit);

//                    if (dsNit != null && dsNit.Tables.Count > 0 &&
//                        dsNit.Tables[0].Rows.Count > 0)
//                    {
//                        foreach (DataRow rowNit in dsNit.Tables[0].Rows)
//                        {

//                            lblDiamTr.Text = rowNit["DIAMETR_OSNOVNOY"].ToString(); 
//                        }

//                    }
//                }


//            }

//        }
//    }
//}