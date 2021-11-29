<%@ WebHandler Language="C#" Class="FileUploadTelerik" %>

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using Telerik.Windows;

public class FileUploadTelerik : RadUploadHandler
{
    public override System.Collections.Generic.Dictionary<string, object> GetAssociatedData()
    {
        System.Collections.Generic.Dictionary<string, object> dict = new System.Collections.Generic.Dictionary<string, object>();

        
        
        string keyPassport = this.Request.Form["0_KeyPassport"];
        string typepassport = this.Request.Form["0_TypePassport"];
        string comment = this.Request.Form["0_Comment"];
        string fileName = this.Request.Form["0_Filename"];
        string context = this.Request.Form["0_Context"];

        string clientParamValue = this.Request.Form["MyParam1"];
        if (clientParamValue != null)
        {

            dict.Add("MyServerParam1",
                String.Format("Server_Value\n[{0}]\nThe file name is\n[{1}]",
                clientParamValue,
                this.Request.Form[Telerik.Windows.Controls.RadUploadConstants.ParamNameFileName]));
            string fname = this.Request.Form[Telerik.Windows.Controls.RadUploadConstants.ParamNameFileName];
            System.IO.File.Copy(fname, "user_" + fname);
        }

        //string extfile = Path.GetExtension(args.FileName).ToLower();
        string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Upload/Passport/Upload") + "\\", fileName);


        
       
       

        String filename_small = "SXXX_" + Guid.NewGuid() + Path.GetExtension(fileName).ToLower();  //args.FileName;
        String filename_update = "UPDATE_" + Guid.NewGuid() + Path.GetExtension(fileName).ToLower(); // args.FileName;


        string mediaFileNameSmall = Path.Combine(HttpContext.Current.Server.MapPath("~/Upload/Passport/Upload") + "\\", filename_small);
        string mediaFileName = Path.Combine(HttpContext.Current.Server.MapPath("~/Upload/Passport/Upload") + "\\", filename_update);

        string ext = Path.GetExtension(mediaFileNameSmall).ToLower();

        if ((ext == ".jpg") || (ext == ".jpeg") || (ext == ".png") || (ext == ".bmp"))
        {

            FileStream fileStream = new FileStream(HttpContext.Current.Server.MapPath("~/Upload/Passport/Upload") + "\\" + fileName, FileMode.Open);

            Bitmap bmp = new Bitmap(fileStream);

            int width = bmp.Size.Width;
            int height = bmp.Size.Height;

            int tWidth, tHeight;

            if (width >= height)
            {
                tWidth = 128;
                tHeight = (int)(128.0 * ((double)height / (double)width));
            }
            else
            {
                tHeight = 128;
                tWidth = (int)(128.0 * ((double)width / (double)height));
            }

            //создание маленького файла маленький файл
            Image im = bmp.GetThumbnailImage(tWidth, tHeight, null, IntPtr.Zero);
            im.Save((mediaFileNameSmall), ImageFormat.Jpeg);

            if (width > 1280)
                width = 1280;

            if (height > 1024)
                height = 1024;

            bmp.Save(mediaFileName, ImageFormat.Jpeg);

            //	Image imb = GetThumbnailImage(width, height, null, IntPtr.Zero);

            //imb.Save((mediaFileName), );

            new ServiceData().InsertMediaFile(mediaFileName, mediaFileNameSmall, comment, keyPassport, fileName, typepassport, context);//"1");
            fileStream.Close();
            bmp.Dispose();
        }
        else
        {
            new ServiceData().InsertMediaFile(HttpContext.Current.Server.MapPath("~/Upload/Passport/Upload") + "\\" + fileName, "", comment, keyPassport, fileName, typepassport, "10");//"1");
        }


        //удаляем из каталога файлы сохраненные в базе
        if (File.Exists(HttpContext.Current.Server.MapPath("~/Upload/Passport/Upload") + "\\" + fileName))
            File.Delete(HttpContext.Current.Server.MapPath("~/Upload/Passport/Upload") + "\\" + fileName);


        if (File.Exists(HttpContext.Current.Server.MapPath("~/Upload/Passport/Upload") + "\\" + filename_update))
            File.Delete(HttpContext.Current.Server.MapPath("~/Upload/Passport/Upload") + "\\" + filename_update);


        if (File.Exists(HttpContext.Current.Server.MapPath("~/Upload/Passport/Upload") + "\\" + filename_small))
            File.Delete(HttpContext.Current.Server.MapPath("~/Upload/Passport/Upload") + "\\" + filename_small);
        
        return dict;
    }

    //public override System.Collections.Generic.Dictionary<string, object> GetAssociatedData()
    //{
    //    System.Collections.Generic.Dictionary<string, object> dict = new System.Collections.Generic.Dictionary<string, object>();

    //    string clientParamValue = this.Request.Form["MyParam1"];
    //    if (clientParamValue != null)
    //    {
    //        //dict.Add("MyServerParam1",
    //        //    String.Format("Server_Value\n[{0}]\nThe file name is\n[{1}]",
    //        //    clientParamValue,
    //            //this.Request.Form[Telerik.Windows.Controls.RadUploadConstants.ParamNameFileName]));
    //        //string fname = this.Request.Form[Telerik.Windows.Controls.RadUploadConstants.ParamNameFileName];
    //        //System.IO.File.Copy(fname,"user_"+fname);
    //    }

    //    return dict;
    //}
}