using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ExportForm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region Заполняем необходимые поля формы атрубутами Паспорта
        //Заполняем необходимые поля страницы при ее первой загрузке
        //lblUpzNumber.Text = Request.QueryString["passportId"];
        //lblDayEndMonth.Text = "10.11";
        //lblbYear.Text = "12";
        //lblPlaceOfImplementation.Text = "Преход через а/д Одесса-Брест, 112 км.";
        //lvlDateOfEnterInExpluatation.Text = "14.08.2008";
        //lblMgName.Text = "АТИ";
        //lblZoneOfProtection.Text = "0,06";
        //lblProjectOrganization.Text = "76, весьма усиленная";
        //lblCountOfProtectorsInGroup.Text = "2хПМ-10у + 2хПМ-10у";
        //lblSechAndMarkaSoedProvodov.Text = "ВПП-4";
        //lblDistanceFromProtectorsToBuiding.Text = "6,7";
        //lblDistanceBetweenProtectors.Text = "-";
        //lblDeepOfProtectors.Text = "3,0";
        //lblThreadSoprotivleniye.Text = "26";
        //lblThreadTok.Text = "0,05";
        //lblRaznostPotenstialov.Text = "-1,3";
        //lblUdelnoyeSoprotivlenye.Text = "30";
        //lblComment.Text = "К паспорту прилагается принципиальная схема и план размещения протекторной установки.";
        //lblNazvaniyeUchastka.Text = "УПЗ, АТИ 112 км.";
        //lblNomerUchastka.Text = "13";

        ////Получаем тестовый объект DataTable для привязки к GridView grvWorks
        //DataTable dt = CreateTestDataSource();

        ////Добавляем программно первую строку таблицы с римскими цифрами (для нумерации столбцов)
        //DataRow row = dt.NewRow();
        //row["WORK_NUMBER"] = "I";
        //row["WORK_DATE"] = "II";
        //row["WORK_DESCRIPTION"] = "III";
        //row["WORK_ORGANIZATION"] = "IV";
        //row["WORK_SIGHN"] = "V";
        //dt.Rows.InsertAt(row, 0);

        ////Привязываем GridView
        //grvWorks.DataSource = dt;
        //grvWorks.DataBind();
        
        #endregion

        if (!IsPostBack)
        {


            GetData(Request.QueryString["passportId"].ToString());

            //Получаем тестовый объект DataTable для привязки к GridView grvWorks
            DataTable dt = CreateTestDataSource(Request.QueryString["passportId"].ToString());

            //Добавляем программно первую строку таблицы с римскими цифрами (для нумерации столбцов)
            DataRow row = dt.NewRow();
            row["WORK_NUMBER"] = "I";
            row["WORK_DATE"] = "II";
            row["WORK_DESCRIPTION"] = "III";
            row["WORK_ORGANIZATION"] = "IV";
            row["WORK_SIGHN"] = "V";
            dt.Rows.InsertAt(row, 0);

            //Привязываем GridView
            grvWorks.DataSource = dt;
            grvWorks.DataBind();

        }


        #region Выгрузка содержимого страницы в формат MS Word
        //В случае, если был выполнен клик на кнопке "Выгрузить в Word",
        //в Page_Load() сработает PostBack. При этом выполнение кода зайдет в блок кода ниже и
        //выгрузит содержимое данной страницы в Word);
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
        #endregion
    }

    #region Инициирование выгрузки содержимого страницы в формат MS Word
    //Выгрузка в Word
    protected void btnLoadIntoWord_Click(object sender, EventArgs e)
    {

    }
    #endregion

    #region Получаем тестовый объект DataTable для привязки к GridView grvWorks
    private DataTable CreateTestDataSource(string key)
    {


        DbConnAuth dbConnAuth = new DbConnAuth();
        DBConn.Conn connOra = dbConnAuth.connOra();
        connOra.ConnectionString(WebConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);


        DBConn.DBParam[] oip = new DBConn.DBParam[1];
        oip[0] = new DBConn.DBParam();
        oip[0].ParameterName = "in_nPK";
        oip[0].DbType = DBConn.DBTypeCustom.Int32;
        //oip[0].Direction = ParameterDirection.Input;
        oip[0].Value = Convert.ToInt32(key);

        DataTable dt1 = connOra.ExecuteQuery<DataTable>("gis_meta_api.FORMATED_PRINTING.UPZ_Jobs", oip);
        
        
        DataTable dt = new DataTable();
        //Создаем стобцы таблицы
        for (int i = 0; i < 5; i++)
        {
            DataColumn dataColumn;
            dataColumn = new DataColumn();
            dataColumn.DataType = Type.GetType("System.String");
            switch (i)
            {
                case 0:
                    dataColumn.ColumnName = "WORK_NUMBER";
                    break;
                case 1:
                    dataColumn.ColumnName = "WORK_DATE";
                    break;
                case 2:
                    dataColumn.ColumnName = "WORK_DESCRIPTION";
                    break;
                case 3:
                    dataColumn.ColumnName = "WORK_ORGANIZATION";
                    break;
                case 4:
                    dataColumn.ColumnName = "WORK_SIGHN";
                    break;
            }

            dt.Columns.Add(dataColumn);
        }
        foreach (DataRow dr in dt1.Rows)
        {
            DataRow row = dt.NewRow();
            row["WORK_NUMBER"] = dr["NNO"].ToString();
            row["WORK_DATE"] = dr["DDATE"].ToString();
            row["WORK_DESCRIPTION"] = dr["cDescription"].ToString();
            row["WORK_ORGANIZATION"] = dr["cExecutor"].ToString();
            row["WORK_SIGHN"] = dr["cSubscribe"].ToString();
            dt.Rows.Add(row);
        }
        //for (int i = 0; i < 5; i++)
        //{
        //    DataRow row = dt.NewRow();
        //    row["WORK_NUMBER"] = (i + 1).ToString();
        //    row["WORK_DATE"] = "1" + i + ".0" + i + ".200" + i;
        //    row["WORK_DESCRIPTION"] = "Комплекс работ выполнен вовремя и с соотвествующим уровнем качества.";
        //    row["WORK_ORGANIZATION"] = "ООО УкрДизайн";
        //    row["WORK_SIGHN"] = "Иванов И.Н.";
        //    dt.Rows.Add(row);
        //}
        return dt;
    }

    private void GetData(string key)
    {
        

        try
        {

            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(WebConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);

            
            DBConn.DBParam[] oip = new DBConn.DBParam[1];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nPK";
            oip[0].DbType = DBConn.DBTypeCustom.Int32;
            //oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = Convert.ToInt32(key);

            DataTable dt = connOra.ExecuteQuery<DataTable>("gis_meta_api.FORMATED_PRINTING.UPZ_Main", oip);
           

                    foreach (DataRow dr in dt.Rows)
                    {
                        lblUpzNumber.Text = dr["cNumber"].ToString();
                        lblDayEndMonth.Text = dr["cDate"].ToString();
                        //lblbYear.Text = "12";
                        lblPlaceOfImplementation.Text = dr["cPlace"].ToString();
                        lvlDateOfEnterInExpluatation.Text = dr["dRunDate"].ToString();
                        lblMgName.Text = dr["cPipeline"].ToString();
                        lblZoneOfProtection.Text = dr["nProtectionZoneLenth"].ToString();
                        lblProjectOrganization.Text = dr["cProjectOrg"].ToString();
                        lblCountOfProtectorsInGroup.Text = dr["koli_prot_v_gr_grupp"].ToString();
                        lblSechAndMarkaSoedProvodov.Text = dr["cMark"].ToString();
                        lblDistanceFromProtectorsToBuiding.Text = dr["nDistance"].ToString();
                        lblDistanceBetweenProtectors.Text = dr["rassto_mejdu__protek"].ToString();
                        lblDeepOfProtectors.Text = dr["glubin_zalega_protek"].ToString();
                        lblThreadSoprotivleniye.Text = dr["nResistance"].ToString();
                        lblThreadTok.Text = dr["nCurrent"].ToString();
                        lblRaznostPotenstialov.Text = dr["nPD_tz"].ToString();
                        lblUdelnoyeSoprotivlenye.Text = dr["nResistivity"].ToString();
                        lblComment.Text = "К паспорту прилагается принципиальная схема и план размещения протекторной установки.";
                        lblNazvaniyeUchastka.Text = dr["cPipeline"].ToString() + ", " + dr["cPlace"].ToString();
                        lblNomerUchastka.Text = dr["cNumber"].ToString(); 
                        

                    }
                    
                





        }
        catch (Exception)
        {
            
            throw;
        }

       
    }
    #endregion
}