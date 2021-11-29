using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ExportFormZRA : System.Web.UI.Page
{
    string Du = "1400";
    string Ru = "75";
    string Number = "0133";

    protected void Page_Load(object sender, EventArgs e)
    {
        #region Заполняем необходимые поля формы атрубутами Паспорта
        //Заполняем необходимые поля страницы при ее первой загрузке
       
        
        #endregion

        if (!IsPostBack)
        {

            GetData(Request.QueryString["passportId"].ToString());
            
            
            
            //Получаем тестовый объект DataTable для привязки к GridView grvWorks
            DataTable dt = CreateTestDataSource();

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
    private DataTable CreateTestDataSource()
    {
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

        for (int i = 0; i < 5; i++)
        {
            DataRow row = dt.NewRow();
            row["WORK_NUMBER"] = (i + 1).ToString();
            row["WORK_DATE"] = "1" + i + ".0" + i + ".200" + i;
            row["WORK_DESCRIPTION"] = "Комплекс работ выполнен вовремя и с соотвествующим уровнем качества.";
            row["WORK_ORGANIZATION"] = "ООО УкрДизайн";
            row["WORK_SIGHN"] = "Иванов И.Н.";
            dt.Rows.Add(row);
        }
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

            DataTable dt = connOra.ExecuteQuery<DataTable>("gis_meta_api.FORMATED_PRINTING.TPA_Main", oip);


            foreach (DataRow dr in dt.Rows)
            {
                lblLpuMgName.Text = "???????????????";
                lblFactoryNuber.Text = dr["Nomer"].ToString();
                lblDy.Text = Du;
                lblRu.Text = Ru;
                lblZraType.Text = dr["Rod_ZA"].ToString();
                lblSposobPrisoedineniya.Text = dr["Sposob"].ToString();
                lblZapornOrganType.Text = dr["Tip_ZO"].ToString();
                lblGostZra.Text = dr["Standart"].ToString();
                lblFactoryVender.Text = dr["Zavod"].ToString();
                lblCountry.Text = dr["Strana"].ToString();
                lblFactoryNumber.Text = dr["Nomer"].ToString();
                lblDateOfCreating.Text = dr["Data_vypuska"].ToString();
                lblDiamUslovnyi.Text = dr["DiamNom"].ToString();
                lblDavlRab.Text = dr["rabochee_davlenie"].ToString();
                lblObvyazka.Text = dr["Obvyazka"].ToString();
                lblPrivod.Text = dr["Privod"].ToString();
                lblUpravleniye.Text = dr["Upravlenie"].ToString();
                lblProchee.Text = dr["primechanie"].ToString();
                lblPlaceOfInstallation.Text = dr["Mesto"].ToString();
                lblDateOnInstallation.Text = dr["Data_vvoda"].ToString();
                lblNumberOfInstallation.Text = dr["Prisvoenyj_No"].ToString();
                lblOboznNaScheme.Text = dr["Uslovnoe_obozn"].ToString();
                lblPost.Text = "?????????????????";
                lblFIO.Text = "???????????????????";
                lblDateOfFilling.Text = "???????????????????";
                lblObjectName.Text = "????????????????";
                lblDu.Text = Du;
                lblRy.Text = Ru;
                lblNum.Text = Number;


            }







        }
        catch (Exception)
        {

            throw;
        }

    }
    #endregion
}