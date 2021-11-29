using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Configuration;
using App_Code.Passport_API;

public partial class ServiceData : IServiceData
{

    private string ext_k;

    /// <summary>
    /// Сохранение файла в базе
    /// </summary>
    /// <param name="mediaFileName"></param>
    /// <param name="fileName"></param>
    /// <param name="fileName_s"></param>
    /// <param name="mediaType"></param>
    /// <param name="createDate"></param>
    /// <param name="coment"></param>
    /// <param name="keyUser"></param>

    public StatusAnswer InsertMediaFile(string mediaFileName, string mediaFileName_small, string comment, string passportKey, string fileName, string typeMedia, string context)
    {
        var result = new StatusAnswer();
        string _comment;

        if (comment == "")
        {
            _comment = " ";
        }
        else
        {
            _comment = comment;
        }

        try
        {
            //для большого файла
            FileStream fs = new System.IO.FileStream(mediaFileName, FileMode.Open, FileAccess.Read);
            byte[] b = new byte[fs.Length];
            fs.Read(b, 0, b.Length);
            fs.Close();
            try
            {
                int onRes = 1;

                DBConn.DBParam[] oip = new DBConn.DBParam[7];
                oip[0] = new DBConn.DBParam();
                oip[0].ParameterName = "in_nCTX";
                oip[0].DbType = DBConn.DBTypeCustom.Number;
                oip[0].Direction = ParameterDirection.Input;
                oip[0].Value = int.Parse(context);

                oip[1] = new DBConn.DBParam();
                oip[1].ParameterName = "in_nObjectKey";
                oip[1].DbType = DBConn.DBTypeCustom.Number;
                oip[1].Direction = ParameterDirection.Input;
                oip[1].Value = int.Parse(passportKey);

                oip[2] = new DBConn.DBParam();
                oip[2].ParameterName = "in_nMediaType";
                oip[2].DbType = DBConn.DBTypeCustom.Number;
                oip[2].Direction = ParameterDirection.Input;
                oip[2].Value = int.Parse(typeMedia);

                oip[6] = new DBConn.DBParam();
                oip[6].ParameterName = "in_cFileName";
                oip[6].DbType = DBConn.DBTypeCustom.VarChar;
                oip[6].Direction = ParameterDirection.Input;
                oip[6].Value = fileName;

                oip[3] = new DBConn.DBParam();
                oip[3].ParameterName = "in_cComment";
                oip[3].DbType = DBConn.DBTypeCustom.VarChar;
                oip[3].Direction = ParameterDirection.Input;
                oip[3].Value = _comment;

                oip[4] = new DBConn.DBParam();
                oip[4].ParameterName = "in_blobSmall";
                oip[4].DbType = DBConn.DBTypeCustom.Blob;
                oip[4].Direction = ParameterDirection.Input;

                if (mediaFileName_small != "")
                {
                    //для маленького файла
                    FileStream fs_small = new System.IO.FileStream(mediaFileName_small, FileMode.Open,
                                                                   FileAccess.Read);
                    byte[] b_s = new byte[fs_small.Length];
                    fs_small.Read(b_s, 0, b_s.Length);
                    fs_small.Close();

                    oip[4].Value = b_s;

                }
                else
                {
                    oip[4].Value = new byte[0];
                }

                oip[5] = new DBConn.DBParam();
                oip[5].ParameterName = "in_blobData";
                oip[5].DbType = DBConn.DBTypeCustom.Blob;
                oip[5].Direction = ParameterDirection.Input;
                oip[5].Value = b;

                //DataOracleHelper.ExecNonQuery("db_api.passport_sl_api.addMedia_p", ref oip);getAddMediaQuery
                DataOracleHelper.ExecNonQuery(OracleQuerys.GetAddMediaQuery, ref oip);
                result.ErrorMessage = onRes.ToString();


            }
            catch (Exception ex)
            {
                log_passport.Error(ex);
                throw ex;
            }
        }
        catch (Exception ex)
        {
            log_passport.Error(ex);
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }
        return result;
        #region через MS SQL
        //string conStr = WebConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
        //SqlConnection conn = new SqlConnection(conStr);
        //SqlCommand cmd1 = new SqlCommand();
        //cmd1.Connection = conn;

        //cmd1.CommandType = CommandType.StoredProcedure;
        //cmd1.CommandText = "Passport_SL_API.addMedia";
        //cmd1.Parameters.AddWithValue("in_nObjectKey", passportKey);
        //cmd1.Parameters.AddWithValue("in_nCTX", Convert.ToDouble(context));
        //cmd1.Parameters.Add("in_blobData", OleDbType.VarBinary).Value = b;
        //if (mediaFileName_small != "")
        //{
        //    //для маленького файла
        //    FileStream fs_small = new System.IO.FileStream(mediaFileName_small, FileMode.Open, FileAccess.Read);
        //    byte[] b_s = new byte[fs_small.Length];
        //    fs_small.Read(b_s, 0, b_s.Length);
        //    fs_small.Close();
        //    cmd1.Parameters.Add("in_blobSmall", OleDbType.VarBinary).Value = b_s;
        //}
        //else
        //{
        //    cmd1.Parameters.Add("in_blobSmall", OleDbType.VarBinary).Value = new byte[0];
        //}

        //cmd1.Parameters.AddWithValue("in_cComment", _comment);
        //cmd1.Parameters.AddWithValue("in_cFileName", fileName); //fileName
        //cmd1.Parameters.AddWithValue("in_nMediaType", typeMedia); //fileName
        //SqlParameter retValue = new SqlParameter();
        //retValue.ParameterName = "return_addMedia";
        //retValue.Direction = ParameterDirection.ReturnValue;
        //cmd1.Parameters.Add(retValue);
        //conn.Open();
        //cmd1.ExecuteNonQuery();
        //string res = cmd1.Parameters["return_addMedia"].Value.ToString();
        //conn.Close();
        ////end SqlCommand
        #endregion
    }


    


    public StatusAnswer DeleteObj(string passportkey, string context)
    {
        var result = new StatusAnswer();
        result.IsValid = true;
        try
        {

            string onRes = "";
            DBConn.DBParam[] oip = new DBConn.DBParam[2];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nObjectKey";
            oip[0].DbType = DBConn.DBTypeCustom.Int64;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = Convert.ToInt64(passportkey);

            oip[1] = new DBConn.DBParam();
            oip[1].ParameterName = "in_nCTX";
            oip[1].DbType = DBConn.DBTypeCustom.Number;
            oip[1].Direction = ParameterDirection.Input;
            oip[1].Value = Convert.ToDouble(context);

            DataOracleHelper.ExecQuery(OracleQuerys.DellObjQuery, ref oip, ref onRes);

            double outres;

            if (double.TryParse(onRes, out outres))
            {

                //result..KeyObj_OnSaveObj_result.KeyObj_OnSaveObj = res;
            }
            else
            {
                //result.KeyObj_OnSaveObj_result.KeyObj_OnSaveObj = "";
                result.IsValid = false;
                result.ErrorMessage = onRes;
            }

            #region через Oracle
            // result.ErrorMessage = onRes;


            //using (OracleConnection conn = DataOracleHelper.GetOracleConnection())
            //{
            //    OracleTransaction tx = null;

            //    if (conn.State != ConnectionState.Open)
            //        conn.Open();

            //    try
            //    {

            //        OracleCommand cmd = conn.CreateCommand();
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Transaction = tx;
            //        cmd.CommandText = OracleQuerys.DellObjQuery;
            //        cmd.Parameters.AddWithValue("inObjKey", passportkey);

            //        #region Устанавливаем в БД сессию текущего пользователя.

            //        OracleConnection con = DataOracleHelper.GetOracleConnection();

            //        //                oracleEngine.SetCurrentSession(con, new AuthorisationAPP(UserSession.UserGuid, UserSession.IpAdress,
            //        //UserSession.CompName, UserSession.SessionKey, UserSession.BrowserName));

            //        #endregion

            //        cmd.ExecuteNonQuery();
            //        //tx.Commit();
            //    }
            //    catch (Exception ex)
            //  {
            //       log_passport.Error(ex);
            //       if (tx != null)
            //           tx.Rollback();
            //       throw ex;
            //   }
            // }
            #endregion
        }
        catch (Exception ex)
        {
            log_passport.Error(ex);
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }
        return result;


    }

    public StatusAnswer DeleteMediaOnObj(string passportkey, string mediaKey, string context)
    {
        var resylt = new StatusAnswer();
        try
        {
            
            int onRes = 1;
            DBConn.DBParam[] oip = new DBConn.DBParam[3];
            oip[0] = new DBConn.DBParam();
            //oip[0].ParameterName = "inObjKey";
            oip[0].ParameterName = "in_nObjectKey";
            oip[0].DbType = DBConn.DBTypeCustom.Int64;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = Convert.ToInt64(passportkey);

            oip[1] = new DBConn.DBParam();
            //oip[1].ParameterName = "inMediaKey";
            oip[1].ParameterName = "in_nMediaKey";
            oip[1].DbType = DBConn.DBTypeCustom.Int64;
            oip[1].Direction = ParameterDirection.Input;
            oip[1].Value = Convert.ToInt64(mediaKey);

            oip[2] = new DBConn.DBParam();
            oip[2].ParameterName = "in_nCTX";
            oip[2].DbType = DBConn.DBTypeCustom.Number;
            oip[2].Direction = ParameterDirection.Input;
            oip[2].Value = Convert.ToDouble(context);

            DataOracleHelper.ExecQuery(OracleQuerys.DelMediaQuery, ref oip, ref onRes);
        }
        catch (Exception ex)
        {
            log_passport.Error(ex);
            resylt.IsValid = false;
            resylt.ErrorMessage = ex.Message;
        }
        return resylt;
    }

    public ThumbnailList GetThumbnailList(string passportKey, int mediaType, string context)
    {

        var result = new ThumbnailList();
        result.Thumbnails = new List<Thumbnail>();

        try
        {

            var thumb = new Thumbnail();

            


            DataSet ds = new DataSet();
            DBConn.DBParam[] oip = new DBConn.DBParam[3];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nObjectKey";
            oip[0].DbType = DBConn.DBTypeCustom.Int64;
            oip[0].Direction = ParameterDirection.Input;
            //oip[0].Size = 1000;
            oip[0].Value = Convert.ToInt64(passportKey);

            oip[1] = new DBConn.DBParam();
            oip[1].ParameterName = "in_nMediaType";
            oip[1].DbType = DBConn.DBTypeCustom.Number;
            oip[1].Direction = ParameterDirection.Input;
            oip[1].Value = Convert.ToDouble(mediaType);

            oip[2] = new DBConn.DBParam();
            oip[2].ParameterName = "in_nCTX";
            oip[2].DbType = DBConn.DBTypeCustom.Number;
            oip[2].Direction = ParameterDirection.Input;
            oip[2].Value = Convert.ToDouble(context);

            DataOracleHelper.ExecQuery(OracleQuerys.GetMediaListOnKeyPassportQuery, ref oip, ref ds);

            foreach (DataRow dtr in ds.Tables[0].Rows)
            {
                

                thumb.Data = dtr[4] as byte[];
                thumb.Comment = dtr[3].ToString();//reader.GetString(3);
                thumb.Key = Convert.ToDouble(dtr[0].ToString());// reader.GetInt64(1);
                thumb.Name = dtr[2].ToString();//reader.GetString(4);
                thumb.TypeMedia = dtr[1].ToString();
                

                result.Thumbnails.Add(thumb);
                thumb = new Thumbnail();
            }
            for (int i = 0; i < result.Thumbnails.Count; i++)
            {
                Thumbnail thumb_1 = result.Thumbnails[i];
                if (!string.IsNullOrEmpty(thumb_1.Name))
                {
                    string[] ar = thumb_1.Name.Split('.');
                    if (ar.Length > 0)
                        ext_k = ar[ar.Length - 1];

                    string extension = "";
                    extension = ext_k.ToLower();
                    string path = "";

                    switch (extension)
                    {
                        case "doc":
                        case "docx":
                            {
                                path = AppDomain.CurrentDomain.BaseDirectory + "Upload/Passport/UploadJPG\\DOC.png";
                                break;
                            }
                        case "cad":
                            {
                                path = AppDomain.CurrentDomain.BaseDirectory + "Upload/Passport/UploadJPG\\CAD.png";
                                break;
                            }
                        case "csv":
                            {
                                path = AppDomain.CurrentDomain.BaseDirectory + "Upload/Passport/UploadJPG\\CSV.png";
                                break;
                            }
                        case "pdf":
                            {
                                path = AppDomain.CurrentDomain.BaseDirectory + "Upload/Passport/UploadJPG\\PDF.png";
                                break;
                            }
                        case "rar":
                            {
                                path = AppDomain.CurrentDomain.BaseDirectory + "Upload/Passport/UploadJPG\\RAR.png";
                                break;
                            }
                        case "txt":
                            {
                                path = AppDomain.CurrentDomain.BaseDirectory + "Upload/Passport/UploadJPG\\TXT.png";
                                break;
                            }
                        case "vsd":
                            {
                                path = AppDomain.CurrentDomain.BaseDirectory + "Upload/Passport/UploadJPG\\VSD.png";
                                break;
                            }
                        case "xls":
                        case "xlsx":
                            {
                                path = AppDomain.CurrentDomain.BaseDirectory + "Upload/Passport/UploadJPG\\XLS.png";
                                break;
                            }
                        case "zip":
                            {
                                path = AppDomain.CurrentDomain.BaseDirectory + "Upload/Passport/UploadJPG\\ZIP.png";
                                break;
                            }

                        default:
                            {
                                if (extension != "jpg")
                                    if (extension != "png")
                                        if (extension != "jpeg")
                                            if (extension != "bmp")
                                             path = AppDomain.CurrentDomain.BaseDirectory + "Upload/Passport/UploadJPG\\other.png";
                                break;
                            }
                    }

                    if (extension != "jpg")
                        if (extension != "png")
                            if (extension != "jpeg")
                                if (extension != "bmp")
                            {

                                FileStream fileStream = new FileStream(path, FileMode.Open);
                                BinaryReader br = new BinaryReader(fileStream);
                                long numBytes = new FileInfo(path).Length;
                                byte[] fileBytes = br.ReadBytes((int)numBytes);
                                //fileBytes.Read(fileBytes, 0, fileBytes.Length);
                                result.Thumbnails[i].Data = fileBytes;
                                fileStream.Close();
                                br.Close();
                            }
                 
                }
            }

            #region обращение к базе простым запросом
            //           using (OracleConnection conn = DataOracleHelper.GetOracleConnection())
            //            {

            //                if (conn.State != ConnectionState.Open)
            //                    conn.Open();
            //                OracleCommand cmd = conn.CreateCommand();
            //                cmd.CommandType = CommandType.Text;
            //                cmd.CommandText = @"select tt.mediasmol,
            //                                            t.key_media,
            //                                            t.type_media,
            //                                            t.comment_media,
            //                                            t.name_file
            //                                            
            //                                            from media_type_test_kill t, media_test_kill tt
            //                                            where t.key_media = tt.key
            //                                            and t.key_obj = 101
            //                                            and t.type_media = " + mediaType;

            //                OracleDataReader reader = cmd.ExecuteReader();
            //                using (reader)
            //                {
            //                    while (reader.Read())
            //                    {
            //                        //Obtain the LOBs 
            //                        OracleLob blob = reader.GetOracleLob(0); // блоб в первой колонке под индексом 0

            //                        //Example - Reading binary data (in chunks). 
            //                        byte[] buffer = new byte[blob.Length];
            //                        blob.Read(buffer, 0, buffer.Length);
            //                        thumb.Data = buffer;

            //                        //FileStream f = new FileStream(@"c:\test.jpg", FileMode.CreateNew);
            //                        //f.Write(buffer, 0, buffer.Length);
            //                        //f.Close();



            //                        thumb.Comment = reader.GetString(3);
            //                        thumb.Key = reader.GetInt64(1);
            //                        thumb.Name = reader.GetString(4);

            //                        result.Thumbnails.Add(thumb);
            //                        thumb = new Thumbnail();
            //                    }
            //                }
            //            }
            #endregion
        }
        catch (Exception ex)
        {
            log_passport.Error(ex);
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }

        return result;
    }

    public ThumbnailListBigMedia GetMediaFile(string mediaKey, string context)
    {
        var result = new ThumbnailListBigMedia();
        result.ThumbnailBigMedia = new List<ThumbnailBigMedia>();

        try
        {
            var thumbbm = new ThumbnailBigMedia();

           

            DataSet ds = new DataSet();
            DBConn.DBParam[] oip = new DBConn.DBParam[2];
            oip[0] = new DBConn.DBParam();
            //oip[0].ParameterName = "inKeyMedia";
            oip[0].ParameterName = "in_nMediaKey";
            oip[0].DbType = DBConn.DBTypeCustom.Int64;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = Convert.ToInt64(mediaKey);

            oip[1] = new DBConn.DBParam();
            oip[1].ParameterName = "in_nCTX";
            oip[1].DbType = DBConn.DBTypeCustom.Number;
            oip[1].Direction = ParameterDirection.Input;
            oip[1].Value = Convert.ToDouble(context);

            DataOracleHelper.ExecQuery(OracleQuerys.GetMediaBigOnKeyMediaQuery, ref oip, ref ds);

            


            thumbbm.Data = ds.Tables[0].Rows[0][4] as byte[];//buffer;
            thumbbm.Comment = ds.Tables[0].Rows[0][2].ToString();//reader.GetString(1);
            thumbbm.Name = ds.Tables[0].Rows[0][3].ToString();//reader.GetString(2);

            result.ThumbnailBigMedia.Add(thumbbm);
        }
        catch (Exception ex)
        {
            log_passport.Error(ex);
            result.IsValid = false;
            result.ErrorMessage = ex.Message;
        }
        return result;
    }

}

