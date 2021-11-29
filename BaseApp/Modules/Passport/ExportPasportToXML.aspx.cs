using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
//using System.Web.Security;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;



public partial class Modules_Passport_ExportPasportToXML : System.Web.UI.Page
{
    #region Блок глобальных переменных
    string _passportId = String.Empty;
    #endregion

    
    protected void Page_Load(object sender, EventArgs e)
    {



        if (!String.IsNullOrEmpty(Request.QueryString["passportId"]))
        {
            try
            {
                string resXML = String.Empty;

                _passportId = Request.QueryString["passportId"];

                DBConn.DBParam[] oip = new DBConn.DBParam[2];
                oip[0] = new DBConn.DBParam();
                oip[0].ParameterName = "in_nobjectkey";
                oip[0].DbType = DBConn.DBTypeCustom.Float;
                oip[0].Direction = ParameterDirection.Input;
                oip[0].Value = Convert.ToDouble(_passportId);

                oip[1] = new DBConn.DBParam();
                oip[1].ParameterName = "result";
                oip[1].DbType = DBConn.DBTypeCustom.Clob;
                oip[1].Direction = ParameterDirection.ReturnValue;

                DataOracleHelper.ExecQuery("gis_meta.meta_xml.exp_object_data", ref oip, ref resXML);

                UnicodeEncoding uniEncoding = new UnicodeEncoding();
                byte[] firstString = uniEncoding.GetBytes(resXML);

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ClearHeaders();
                //HttpContext.Current.Response.AppendHeader("Content-Type", "application/msword");
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + "Passport.xml");
                HttpContext.Current.Response.AddHeader("Content-Length", Convert.ToString(firstString.Length));
                HttpContext.Current.Response.BinaryWrite(firstString);
                HttpContext.Current.Response.End();
              




                //HttpContext.Current.Response.ContentType = "application/octet-stream";
                //HttpContext.Current.Response.AddHeader("Content-Disposition",
                //  "attachment; filename=" + "Passport.xml");
                //HttpContext.Current.Response.Charset = "UTF-8";
                //HttpContext.Current.Response.Clear();
                //HttpContext.Current.Response.BinaryWrite(firstString);
                //HttpContext.Current.Response.End();


            }
            catch (Exception ex)
            {
                
            }

        }
        else
        {
            divMain.Visible = true;
            
        }
        
    }
    protected void bntLoadXmlInDb_Click(object sender, EventArgs e)
    {
        try
        {
            if (FileField.HasFile)
            {
                //string path = FileField.PostedFile.FileName;
                string uploadPath = Server.MapPath("~/Upload/Passport/Upload");
                string path = FileField.FileName;
                string result = String.Empty;
                uploadPath = Path.Combine(uploadPath, FileField.FileName);
                FileField.SaveAs(uploadPath);
                string contentOfXmlFile = System.IO.File.ReadAllText(uploadPath, Encoding.Unicode);
                //lblResultMessage.Text = contentOfXmlFile;


                DBConn.DBParam[] oip = new DBConn.DBParam[1];
                oip[0] = new DBConn.DBParam();
                oip[0].ParameterName = "in_cxml";
                oip[0].DbType = DBConn.DBTypeCustom.Clob;
                oip[0].Direction = ParameterDirection.Input;
                oip[0].Value = contentOfXmlFile;


                DataOracleHelper.ExecQuery("gis_meta.meta_xml.imp_object_data", ref oip, ref result);
                lblResultMessage.Text = result;
                if (File.Exists(uploadPath))
                    File.Delete(uploadPath);

            }
            else
            {
                lblResultMessage.Text = "Файл не найден! ";
            }

           
        }
        catch (Exception ex)
        {
            lblResultMessage.Text = "Ошибка: " + ex.Message;
        }
        
    }
}