//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

//public partial class ExportForms_ExportForm : System.Web.UI.Page
//{
//    protected void Page_Load(object sender, EventArgs e)
//    {

//    }
//}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Configuration;

public partial class ExportForm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Заполняем необходимые поля страницы при ее первой загрузке
            Label2.Text = Request.QueryString["passportId"];
            Label3.Text = "Тестовый текст";

           // GetData("121212");

        }

        //В случае, если был выполнен клик на кнопке "Выгрузить в Word",
        //в Page_Load() сработает PostBack. При этом выполнение кода зайдет в блок кода ниже и
        //выгрузит содержимое данной страницы в Word
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
    }

    //Выгрузка в Word
    protected void btnLoadIntoWord_Click(object sender, EventArgs e)
    {

    }


    //private void GetData(string key)
    //{
    //    DbConnAuth dbConnAuth = new DbConnAuth();
    //    DBConn.Conn connOra = dbConnAuth.connOra();
    //    connOra.ConnectionString(WebConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);
    //    DataTable dt = connOra.ExecuteQuery<DataTable>("gis_meta_api.st_application.getapplications");
    //    foreach (DataRow dr in dt.Rows)
    //    {
    //         Label3.Text =  dr["CAPPNAME"].ToString();
            
    //    }
    //}
}