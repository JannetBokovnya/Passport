using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using App_Code.Passport_API;

public partial class ServiceData : IServiceData
{
    //, string inObjkey
    public FieldMetaData MetaData(string iEntityKey, string iTypePassport, string context, string inObjKey)
    {
        var result = new FieldMetaData();

        result.FieldMetaDataList = new List<FieldMetaDataItem>();
        int ii = 0;
        #region TestValue
        // TODO: Заменить на получение реальных данных
        //result.FieldMetaDataList.Add(new FieldMetaDataItem { IS_VISIBLE = 1, TITUL = "ArtifactId", TYPECONTROL = 7, FLDNAME = "ArtifactId", IS_EDITED = 1, BASIC_FLD = 0, TYPEVALIDATION = 1 });
        //result.FieldMetaDataList.Add(new FieldMetaDataItem { IS_VISIBLE = 1, TITUL = "Ключ", TYPECONTROL = 7, FLDNAME = "key", IS_EDITED = 1, BASIC_FLD = 0, TYPEVALIDATION = 1 });
        //result.FieldMetaDataList.Add(new FieldMetaDataItem { IS_VISIBLE = 1, TITUL = "Тип контрола текст", TYPECONTROL = 1, FLDNAME = "textfield", IS_EDITED = 1, BASIC_FLD = 0, TYPEVALIDATION = 2 });
        //result.FieldMetaDataList.Add(new FieldMetaDataItem { IS_VISIBLE = 1, TITUL = "Тип контрола текст - для числовый значений", TYPECONTROL = 7, FLDNAME = "numberfield", IS_EDITED = 1, BASIC_FLD = 0, TYPEVALIDATION = 2 });
        //result.FieldMetaDataList.Add(new FieldMetaDataItem { IS_VISIBLE = 1, TITUL = "Словарь", TYPECONTROL = 7, FLDNAME = "numberdict", IS_EDITED = 1, BASIC_FLD = 0, });
        //result.FieldMetaDataList.Add(new FieldMetaDataItem { IS_VISIBLE = 1, TITUL = "Дата", TYPECONTROL = 3, FLDNAME = "datafield", IS_EDITED = 1, BASIC_FLD = 0 });
        //result.FieldMetaDataList.Add(new FieldMetaDataItem { IS_VISIBLE = 1, TITUL = "Вывод справочных данных(фото)", TYPECONTROL = 6, FLDNAME = "textnoneditlabel", IS_EDITED = 1, BASIC_FLD = 0 });
        //result.FieldMetaDataList.Add(new FieldMetaDataItem { IS_VISIBLE = 1, TITUL = "Добавление связей", TYPECONTROL = 7, FLDNAME = "numberconnect", IS_EDITED = 1, BASIC_FLD = 0 });
        //result.FieldMetaDataList.Add(new FieldMetaDataItem { IS_VISIBLE = 1, TITUL = "Чек бокс", TYPECONTROL = 7, FLDNAME = "checkbox", IS_EDITED = 1, BASIC_FLD = 0 });


        //========================================

        #endregion TestValue

        int instr = 0;

        try
        {

            DataSet ds = new DataSet();
            DBConn.DBParam[] oip = new DBConn.DBParam[4];
            oip[0] = new DBConn.DBParam();
            oip[0].ParameterName = "in_nentitykey";
            oip[0].DbType = DBConn.DBTypeCustom.Int64;
            oip[0].Direction = ParameterDirection.Input;
            oip[0].Value = Convert.ToInt64(iEntityKey);

            oip[1] = new DBConn.DBParam();
            oip[1].ParameterName = "in_nview";
            oip[1].DbType = DBConn.DBTypeCustom.Number;
            oip[1].Direction = ParameterDirection.Input;
            oip[1].Value = Convert.ToDouble(iTypePassport);

            oip[2] = new DBConn.DBParam();
            oip[2].ParameterName = "in_nobjectkey";
            oip[2].DbType = DBConn.DBTypeCustom.Int64;
            oip[2].Direction = ParameterDirection.Input;
            oip[2].Value = Convert.ToInt64(inObjKey);

            oip[3] = new DBConn.DBParam();
            oip[3].ParameterName = "in_nCTX";
            oip[3].DbType = DBConn.DBTypeCustom.Number;
            oip[3].Direction = ParameterDirection.Input;
            oip[3].Value = Convert.ToDouble(context);

            DataOracleHelper.ExecQuery(OracleQuerys.GetMetaDataQuery, ref oip, ref ds);

            //------------------------------
            
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //1. вначале записываем все isbasik = 1
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    instr ++;
                    if ( GetValueRow(row["isBasic"]) ==1)
                    {
                        result.FieldMetaDataList.Add(
                            new FieldMetaDataItem
                            {
                                FLDNAME = row["CFIELDNAME"].ToString(),
                                TITUL = row["CTITLE"].ToString(),
                                INPUTMASK = row["CINPUTMASK"].ToString(),
                                OUTPUTMASK = row["COUTPUTMASK"].ToString(),
                                IS_VISIBLE = int.Parse(row["ISVISIBLE"].ToString()),
                                TYPECONTROL = int.Parse(row["nControlType"].ToString()),
                                IS_EDITED = Convert.ToInt32(row["isEditable"]),
                                //int.Parse(row["IS_EDITED"].ToString()),
                                IS_PK = GetValueRow(row["ISPK"]), //IS_PK = Convert.ToInt32(row["IS_PK"]),
                                PAGENUM = int.Parse(row["nPageNumber"].ToString()),
                                BASIC_FLD = GetValueRow(row["isBasic"]),
                                // BASIC_FLD = int.Parse(row["Basic_fld"].ToString()),
                                FLDKEY = GetValueRow(row["nFieldKey"]),
                                //FLDKEY = int.Parse(row["FLDKEY"].ToString()),
                                FLDTYP = row["cDataType"].ToString(),
                                KEYDICT = row["nDictKey"].ToString(),
                                REPORT_VISIBLE = GetValueRow(row["isPrintable"]),
                                NUMONPAGE = GetValueRow(row["nNumOnPage"]),
                                COLOR_FLD = row["cColor"].ToString(),
                                TYPEVALIDATION = GetValueRow(row["nValidationType"]),
                                MANDATORYVALUE_FLD = GetValueRow(row["isRequired"]),
                            });

                    }
                }


                //2. потом оставшиеся
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ii = ii + 1;
                    if (GetValueRow(row["isBasic"]) != 1)
                    {
                        result.FieldMetaDataList.Add(
                            new FieldMetaDataItem
                                {
                                    FLDNAME = row["CFIELDNAME"].ToString(),
                                    TITUL = row["CTITLE"].ToString(),
                                    INPUTMASK = row["CINPUTMASK"].ToString(),
                                    OUTPUTMASK = row["COUTPUTMASK"].ToString(),
                                    IS_VISIBLE = int.Parse(row["ISVISIBLE"].ToString()),
                                    TYPECONTROL = int.Parse(row["nControlType"].ToString()),
                                    IS_EDITED = Convert.ToInt32(row["isEditable"]),
                                    //int.Parse(row["IS_EDITED"].ToString()),
                                    IS_PK = GetValueRow(row["ISPK"]), //IS_PK = Convert.ToInt32(row["IS_PK"]),
                                    PAGENUM = int.Parse(row["nPageNumber"].ToString()),
                                    BASIC_FLD = GetValueRow(row["isBasic"]),
                                    // BASIC_FLD = int.Parse(row["Basic_fld"].ToString()),
                                    FLDKEY = GetValueRow(row["nFieldKey"]),
                                    //FLDKEY = int.Parse(row["FLDKEY"].ToString()),
                                    FLDTYP = row["cDataType"].ToString(),
                                    KEYDICT = row["nDictKey"].ToString(),
                                    REPORT_VISIBLE = GetValueRow(row["isPrintable"]),
                                    NUMONPAGE = GetValueRow(row["nNumOnPage"]),
                                    COLOR_FLD = row["cColor"].ToString(),
                                    TYPEVALIDATION = GetValueRow(row["nValidationType"]),
                                    MANDATORYVALUE_FLD = GetValueRow(row["isRequired"]),
                                });


                    }

                   
                    if (GetValueRow(row["ISPK"]) == 1)
                    {
                        result.FieldMetaDataList.Add(
                            new FieldMetaDataItem
                                {
                                    FLDNAME = "ARTIFACT_ID",
                                    IS_VISIBLE = 0,
                                    TYPECONTROL = int.Parse(row["nControlType"].ToString()),
                                    IS_EDITED = 0,
                                    FLDTYP = row["cDataType"].ToString(),
                                });
                    }

                }

                result.ErrorMessage = " Passport_SL_API.getmetadata, in_nentitykey = " + iEntityKey + "  in_nview = " +
                                     iTypePassport + "  in_nobjectkey= " + inObjKey + "  in_nCTX = " + context;
            }
            else
            {
                result.ErrorMessage = "не данных Passport_SL_API.getmetadata, in_nentitykey = " + iEntityKey + "  in_nview = " +
                                      iTypePassport + "  in_nobjectkey= " + inObjKey + "  in_nCTX = " + context; 
            }
        }
        catch (Exception ex)
        {
            int i2 = ii;
            log_passport.Error(ex);
            result.IsValid = false;
            result.ErrorMessage = "Passport_SL_API.getmetadata, in_nentitykey = " + iEntityKey + "  in_nview = " +
                iTypePassport + "  in_nobjectkey= " + inObjKey + "  in_nCTX = " + context + 
                ex.Message + "строка " + i2.ToString();
        }

      

        return result;
    }

    public static int GetValueRow(object valueRow)
    {

        if (!DBNull.Value.Equals(valueRow))
        {
            int result;
            if (int.TryParse(valueRow.ToString(), out result))
                return result;

        }
        return 0;

    }

    public static string GetValueRowString(object valueRow)
    {
        string result;

        if (!DBNull.Value.Equals(valueRow))
        {

            if (valueRow.ToString() != "")
            {
                return result = valueRow.ToString();

            }


        }
        return "";



    }

}
//FLDNAME = row["FLDNAME"].ToString(),
//TITUL = row["TITUL"].ToString(),
//INPUTMASK = row["INPUTMASK"].ToString(),
//OUTPUTMASK = row["OUTPUTMASK"].ToString(),
//IS_VISIBLE = int.Parse(row["IS_VISIBLE"].ToString()),
//TYPECONTROL = int.Parse(row["TypeControl"].ToString()),
//IS_EDITED = Convert.ToInt32(row["IS_EDITED"]),  //int.Parse(row["IS_EDITED"].ToString()),
//IS_PK = GetValueRow(row["IS_PK"]), //IS_PK = Convert.ToInt32(row["IS_PK"]),
//PAGENUM = int.Parse(row["PageNum"].ToString()),
//BASIC_FLD = GetValueRow(row["Basic_fld"]), // BASIC_FLD = int.Parse(row["Basic_fld"].ToString()),
//FLDKEY = GetValueRow(row["FLDKEY"]), //FLDKEY = int.Parse(row["FLDKEY"].ToString()),
//FLDTYP = row["FLDTYP"].ToString(),
//KEYDICT = row["KEYDICT"].ToString(),

////DICTTYPE = GetValueRow(row["DICTTYPE"]),
//REPORT_VISIBLE = GetValueRow(row["REPORT_VISIBLE"]),
////IS_CALC = GetValueRow(row["IS_CALC"]),
//NUMONPAGE = GetValueRow(row["NUMONPAGE"]),
//COLOR_FLD = row["COLOR_FLD"].ToString(),
//TYPEVALIDATION = GetValueRow(row["TYPEVALIDATION"]),
//MANDATORYVALUE_FLD = GetValueRow(row["MANDATORYVALUE_FLD"]),