using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ExportFormSKZ : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {



        #region Выгрузка содержимого страницы в формат MS Word
        //В случае, если был выполнен клик на кнопке "Выгрузить в Word",
        //в Page_Load() сработает PostBack. При этом выполнение кода зайдет в блок кода ниже и
        //выгрузит содержимое данной страницы в Word);
        if (IsPostBack)
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
    #endregion
}