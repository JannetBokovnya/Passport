using System;
using System.Globalization;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Media.Resources;
using Passpot.Business;
using Passpot.Business.DataTable;
using Passpot.Model;
using Services.ServiceReference;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;

namespace Passpot.Controls.Grid
{
    public static class GridToOffic
    {


        private static Color _headerBackground = Colors.LightGray;
        public static Color HeaderBackground
        {
            get
            {
                return _headerBackground;
            }
            set
            {
                if (_headerBackground != value)
                {
                    _headerBackground = value;

                }
            }
        }

        private static Color _rowBackground = Colors.White;
        public static Color RowBackground
        {
            get
            {
                return _rowBackground;
            }
            set
            {
                if (_rowBackground != value)
                {
                    _rowBackground = value;

                }
            }
        }

        static Color _headerForeground = Colors.Black;
        public static Color HeaderForeground
        {
            get
            {
                return _headerForeground;
            }
            set
            {
                if (_headerForeground != value)
                {
                    _headerForeground = value;

                }
            }
        }

        static Color _rowForeground = Colors.Black;
        public static Color RowForeground
        {
            get
            {
                return _rowForeground;
            }
            set
            {
                if (_rowForeground != value)
                {
                    _rowForeground = value;

                }
            }
        }

        public static DataTable GetDataTableOffic(PassportDetailModel Model)
        {
            DataTable table = new DataTable();
            var column = new DataColumn();
            column.ColumnName = "NameAttr";
            column.DataType = typeof(string);
            table.Columns.Add(column);

            var column2 = new DataColumn();
            column2.ColumnName = "ValueAttr";
            column2.DataType = typeof(string);
            table.Columns.Add(column2);


            //Model.IsEditedCurrentPassport

            int countMetadata = 0;
            if (Model.PassportDetailOpenParams.IsEditedPassport == 0)
            {
                countMetadata = Model.MetaDataList.Count;
            }
            else
            {
                countMetadata = Model.MetaEditDataList.Count;

            }


            for (int i = 0; i < countMetadata; i++)
            {
                FieldMetaDataItem meta;

                if (Model.PassportDetailOpenParams.IsEditedPassport == 0)
                {
                    meta = Model.MetaDataList[i];
                }
                else
                {
                    meta = Model.MetaEditDataList[i];
                }

                string valueStr = "";



                if (Model.PassportDetailOpenParams.IsEditedPassport == 0)
                {
                    if (Model.PassportData != null)
                    {

                        if ((meta.FLDNAME != "") && (meta.TYPECONTROL != 9))
                        {
                            valueStr = Model.PassportData[meta.FLDNAME.ToUpper()];
                        }

                    }
                }
                else
                {
                    if (Model.PassportEditData != null)
                    {

                        if ((meta.FLDNAME != "") && (meta.TYPECONTROL != 9))
                        {
                            valueStr = Model.PassportEditData[meta.FLDNAME.ToUpper()];
                        }

                    }
                }

                FieldMetaDataItem metaData;

                if (Model.PassportDetailOpenParams.IsEditedPassport == 0)
                {
                    metaData = Model.MetaDataList[i]; ;
                }
                else
                {
                    metaData = Model.MetaEditDataList[i];
                }


                if (metaData.IS_VISIBLE == 1)
                {
                    switch (meta.TYPECONTROL)
                    {
                        case 1:
                        case 7:
                        case 6:
                            {
                                DataRow dr = new DataRow();
                                dr["NameAttr"] = metaData.TITUL;
                                dr["ValueAttr"] = valueStr;
                                table.Rows.Add(dr);
                                break;
                            }
                        case 3:
                            {
                                if (Model.PassportDetailOpenParams.IsEditedPassport != 0)
                                {
                                    if (valueStr != "")
                                    {
                                        valueStr = (DateTime.Parse(valueStr, CultureInfo.CurrentCulture)).ToString();
                                    }
                                }

                                DataRow dr = new DataRow();
                                dr["NameAttr"] = metaData.TITUL;
                                dr["ValueAttr"] = valueStr;
                                table.Rows.Add(dr);

                                break;

                            }
                        case 4:
                            {
                                if (Model.PassportDetailOpenParams.IsEditedPassport != 0)
                                {
                                    if (!string.IsNullOrEmpty(valueStr))
                                    {
                                        for (int idl = 0; idl < Model.DictDataList.Count; idl++)
                                        {
                                            DictData dd = Model.DictDataList[idl];
                                            if ((dd.VALUEKEYDICT.ToString() == valueStr))
                                            {
                                                valueStr = dd.VALUEDICT;
                                                break;
                                            }
                                        }
                                    }

                                }

                                DataRow dr = new DataRow();
                                dr["NameAttr"] = metaData.TITUL;
                                dr["ValueAttr"] = valueStr;
                                table.Rows.Add(dr);
                                break;

                            }
                        case 9:
                            {
                                DataRow dr = new DataRow();
                                dr["NameAttr"] = metaData.TITUL;
                                dr["ValueAttr"] = Model.TextPrivExel;
                                table.Rows.Add(dr);

                                break;
                            }
                        case 5:
                        case 8:
                        case 12:
                        case 13:
                            {
                                DataRow dr = new DataRow();
                                dr["NameAttr"] = metaData.TITUL;
                                dr["ValueAttr"] = "";

                                for (int ii = 0; ii < Model.ListRelationObjList.Count; ii++)
                                {
                                    if (Model.ListRelationObjList[ii].FldName == metaData.FLDNAME)
                                    {

                                        dr["ValueAttr"] = Model.ListRelationObjList[ii].NameObj;

                                    }
                                }

                                table.Rows.Add(dr);

                                break;
                            }
                    }
                }
            }
            return table;
        }

        public static RadGridView CreateGrid(DataTable dt)
        {
            RadGridView gridExel = new RadGridView();
            gridExel.ItemsSource = null;
            gridExel.Columns.Clear();
            gridExel.AutoGenerateColumns = false;
            gridExel.HorizontalAlignment = HorizontalAlignment.Stretch;
            gridExel.VerticalAlignment = VerticalAlignment.Stretch;
            gridExel.CanUserResizeColumns = false;

            var col1 = new Telerik.Windows.Controls.GridViewDataColumn();
            col1.DataType = typeof(string);
            col1.DataMemberBinding = new System.Windows.Data.Binding("NameAttr");
            col1.Header = ProjectResources.exportGridName; // "Название";
            col1.Width = new GridViewLength(150);
            gridExel.Columns.Add(col1);

            var col2 = new Telerik.Windows.Controls.GridViewDataColumn();
            col2.DataType = typeof(string);
            //    col2.UniqueName = "ValueAttr";
            col2.DataMemberBinding = new System.Windows.Data.Binding("ValueAttr");
            col2.Header = ProjectResources.exportGridDate; //"Данные";
            col2.Width = new GridViewLength(150);
            gridExel.Columns.Add(col2);

            gridExel.ItemsSource = dt;

            gridExel.ElementExporting -= (grid_ElementExporting);
            gridExel.ElementExporting += (grid_ElementExporting);

            return gridExel;
        }
        static void grid_ElementExporting(object sender, GridViewElementExportingEventArgs e)
        {
            //    e.Element == ExportElement.HeaderCell)
            if (e.Element == ExportElement.HeaderRow ||
                e.Element == ExportElement.FooterRow ||
                e.Element == ExportElement.GroupFooterRow)
            {
                e.Background = HeaderBackground;
                e.Foreground = HeaderForeground;
                e.FontSize = 20;
                e.FontWeight = FontWeights.Bold;
                e.TextAlignment = TextAlignment.Center;
                e.VerticalAlignment = VerticalAlignment.Center;


            }
            else if (e.Element == ExportElement.Row)
            {
                e.Background = RowBackground;
                e.Foreground = RowForeground;
            }

            else if (e.Element == ExportElement.GroupHeaderRow)
            {
                e.FontFamily = new FontFamily("Verdana");
                e.Background = Colors.LightGray;

            }

            else if (e.Element == ExportElement.GroupFooterCell)
            {
                GridViewDataColumn column = e.Context as GridViewDataColumn;
                QueryableCollectionViewGroup qcvGroup = e.Value as QueryableCollectionViewGroup;


            }
        }



    }
}

//if (e.Element == ExportElement.HeaderRow || 
//               e.Element == ExportElement.FooterRow || 
//               e.Element == ExportElement.GroupFooterRow)
//           {
//if (column != null && qcvGroup != null && column.AggregateFunctions.Count() > 0)
//{
//    e.Value = GetAggregates(qcvGroup, column);
//}