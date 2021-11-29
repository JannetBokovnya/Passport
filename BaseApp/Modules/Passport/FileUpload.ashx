<%@ WebHandler Language="C#" Class="FileUpload" %>

using System;
using System.Web;
//using App_Code.Passport_API;
using SilverlightFileUpload;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using log4net;


/// <summary>
/// Summary description for $codebehindclassname$
/// </summary>
//[WebService(Namespace = "http://tempuri.org/")]
//[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class FileUpload : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    private HttpContext ctx;
    public void ProcessRequest(HttpContext context)
    {
        ctx = context;
        string uploadPath = context.Server.MapPath("~/Upload/Passport/Upload");
        FileUploadProcess fileUpload = new FileUploadProcess();
        fileUpload.FileUploadCompleted += fileUpload_FileUploadCompleted;
        fileUpload.ProcessRequest(context, uploadPath);
    }

	void fileUpload_FileUploadCompleted(object sender, FileUploadCompletedEventArgs args)
	{
		string id = ctx.Request.QueryString["id"];
		string _passportKey = ctx.Request.QueryString["passportKey"];
		string _comment = ctx.Request.QueryString["description"];
		string _fileType = ctx.Request.QueryString["fileTo"];
        string _context = ctx.Request.QueryString["context"]; 
		//string comment = ctx.Request.QueryString["description"];
		//System.IO.FileInfo fi = new System.IO.FileInfo(args.FilePath);
		//string targetFile = System.IO.Path.Combine(fi.Directory.FullName, args.FileName);
		//if (System.IO.File.Exists(targetFile))
		//    System.IO.File.Delete(targetFile);
		//fi.MoveTo(targetFile);
		////ServiceDataClient.GetMediaFileCompleted += new EventHandler<GetMediaFileCompletedEventArgs>(ServiceDataClient_GetMediaFileCompleted);
		////ServiceDataClient.GetMediaFileAsync(key);

		//Преобразовывваем большой файл в маленький и записываем рядом в том же каталоге что и большой
		//только для фотографий!!!

        string extfile = Path.GetExtension(args.FileName).ToLower();

        String filename_small = "SXXX_" + Guid.NewGuid() +  extfile;  //args.FileName;
	    String filename_update = "UPDATE_" + Guid.NewGuid() +  extfile; // args.FileName;

		//String filename_small = args.FileName;
        string mediaFileNameSmall = Path.Combine(ctx.Server.MapPath("~/Upload/Passport/Upload") + "\\", filename_small);
        string mediaFileName = Path.Combine(ctx.Server.MapPath("~/Upload/Passport/Upload") + "\\", filename_update);

		string ext = Path.GetExtension(mediaFileNameSmall).ToLower();


		if ((ext == ".jpg") || (ext == ".jpeg"))
		{

            FileStream fileStream = new FileStream(ctx.Server.MapPath("~/Upload/Passport/Upload") + "\\" + args.FileName, FileMode.Open);

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

            new ServiceData().InsertMediaFile(mediaFileName, mediaFileNameSmall, _comment, _passportKey, args.FileName, _fileType, _context);//"1");
			fileStream.Close();
			bmp.Dispose();
		}
		else
		{
            new ServiceData().InsertMediaFile(ctx.Server.MapPath("~/Upload/Passport/Upload") + "\\" + args.FileName, "", _comment, _passportKey, args.FileName, _fileType, _context);//"1");
		}




        //new ServiceData().InsertMediaFile(ctx.Server.MapPath("~/Upload/Passport/Upload") + "\\" + args.FileName, savePath_s, _comment, _passportKey, args.FileName, _fileType);//"1");
		//удаляем из каталога файлы сохраненные в базе
        if (File.Exists(ctx.Server.MapPath("~/Upload/Passport/Upload") + "\\" + args.FileName))
            File.Delete(ctx.Server.MapPath("~/Upload/Passport/Upload") + "\\" + args.FileName);


        if (File.Exists(ctx.Server.MapPath("~/Upload/Passport/Upload") + "\\" + filename_update))
            File.Delete(ctx.Server.MapPath("~/Upload/Passport/Upload") + "\\" + filename_update);


        if (File.Exists(ctx.Server.MapPath("~/Upload/Passport/Upload") + "\\" + filename_small))
            File.Delete(ctx.Server.MapPath("~/Upload/Passport/Upload") + "\\" + filename_small);


	}


    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
    }