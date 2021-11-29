using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using Passpot.Business;
using Passpot.Business.DataTable;
using Media.Resources;
using Services.ServiceReference;
using Telerik.Windows.Controls;
using System.Windows;
using Passpot.Model;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Data;

using Telerik.Windows.Controls;

namespace Passpot.Grid
{
    public static class GridHelper
    {

        
        public static void BuildPasportGrid(RadGridView grid, DataTemplate template, Style cellStyle, Style cellStyleAll, Style headerCellStyle, IGridData gridData, bool isShowAllField)
        {
            grid.ItemsSource = null;
            grid.Columns.Clear();
           
            int columnAggregat = 0;
            int columnStyle = 0;


            if (template != null)
            {
                var columnButton = new GridViewColumn { CellTemplate = template };
                columnButton.Width = 80;
                CountFunction fс = new CountFunction();
                //fс.Caption = "Всего: ";
                //columnButton.AggregateFunctions.Add(fс);
               // grid.Columns.Add(columnButton);


            }

            for (int i = 0; i < gridData.MetaDataList.Count; i++)
            {
                var col = new GridViewDataColumn();
                col.HeaderTextAlignment = TextAlignment.Center;
                col.TextTrimming = TextTrimming.WordEllipsis;
                col.MaxWidth = 300;
                col.Width = GridViewLength.SizeToCells;
               

                //col.HeaderCellStyle = headerCellStyle;
                //TextBlock tb = new TextBlock();
                //tb.Text = "'dlgjldkj;";
                //tb.
                //col.Header = tb;
                
                //col.Header = new TextBlock() { Text = "oj;lkj;l", ToolTipService.ToolTip = "Your tooltip" };

               
                if (gridData.MetaDataList[i].FLDNAME.ToUpper() == "ARTIFACT_ID")
                {
                    col.DataType = GetColumnType(gridData.MetaDataList[i].TYPECONTROL);
                    col.UniqueName = gridData.MetaDataList[i].FLDNAME.ToUpper();

                    col.Header = gridData.MetaDataList[i].TITUL;

                    
                    col.DataMemberBinding = new Binding(gridData.MetaDataList[i].FLDNAME.ToUpper());
                    col.IsVisible = false;

                    grid.Columns.Add(col);
                }
                else
                {
                    if (gridData.MetaDataList[i].IS_VISIBLE != 0)
                    {
                        if (isShowAllField)
                        {
                            if (gridData.MetaDataList[i].BASIC_FLD == 1)
                            {
                                //не показываем горизонтальные связи
                                if ((gridData.MetaDataList[i].TYPECONTROL == 8) || 
                                    (gridData.MetaDataList[i].TYPECONTROL == 9) ||
                                    (gridData.MetaDataList[i].TYPECONTROL == 12)||
                                    (gridData.MetaDataList[i].TYPECONTROL == 13)||
                                    (gridData.MetaDataList[i].TYPECONTROL == 5)
                                    )
                                    {

                                    }
                                    else
                                    {
                                        
                                        col.DataType = GetColumnType(gridData.MetaDataList[i].TYPECONTROL);
                                        col.UniqueName = gridData.MetaDataList[i].FLDNAME.ToUpper();
                                        col.Header = gridData.MetaDataList[i].TITUL;
                                            

                                        TextBlock header_t = new TextBlock();
                                        header_t.TextAlignment = TextAlignment.Center;
                                        header_t.TextTrimming = TextTrimming.WordEllipsis;
                                       
                                        header_t.Text = gridData.MetaDataList[i].TITUL;
                                        col.Header = header_t;
                                        ToolTipService.SetToolTip(header_t, gridData.MetaDataList[i].TITUL);
                                        

                                        col.DataMemberBinding = new Binding(gridData.MetaDataList[i].FLDNAME.ToUpper());
                                        if (columnAggregat == 0)
                                        {
                                            CountFunction fс = new CountFunction();
                                            //fс.ResultFormatString = "{}Count: {0}";

                                            fс.Caption = ProjectResources.CountFunctionGrid;
                                            col.AggregateFunctions.Add(fс);
                                            columnAggregat = 1;
                                            col.TextDecorations = TextDecorations.Underline;
                                            col.CellStyle = cellStyle;

                                        }
                                        else
                                        {
                                            col.CellStyle = cellStyleAll;
                                        }
                                        grid.Columns.Add(col);
                                    }
                       
                            }
                        }
                        else
                        {
 
                                //не показываем горизонтальные связи
                                if ((gridData.MetaDataList[i].TYPECONTROL == 8) ||
                                    (gridData.MetaDataList[i].TYPECONTROL == 9) ||
                                    (gridData.MetaDataList[i].TYPECONTROL == 12) ||
                                    (gridData.MetaDataList[i].TYPECONTROL == 13) ||
                                    (gridData.MetaDataList[i].TYPECONTROL == 5)
                                    )
                                {

                                }
                                else
                                {
                                    col.DataType = GetColumnType(gridData.MetaDataList[i].TYPECONTROL); 
                                    //if (gridData.MetaDataList[i].TYPECONTROL == 3)
                                    //{

                                    //    col.DataType = typeof (DateTime);
                                    //    col.DataFormatString = "{0:dd.M.yyyy}"; //"{} {0:C0}" ;
                                    //    //col.CreateConvertedAndFormattedValueFunc();
                                    //}
                                    //else
                                    //{
                                    //    col.DataType = GetColumnType(gridData.MetaDataList[i].TYPECONTROL);    
                                    //}
                                    
                                    col.UniqueName = gridData.MetaDataList[i].FLDNAME.ToUpper();
                                    col.Header = gridData.MetaDataList[i].TITUL;

                                    TextBlock header_t = new TextBlock();
                                    header_t.TextAlignment = TextAlignment.Center;
                                    header_t.TextTrimming = TextTrimming.WordEllipsis;
                                    header_t.Text = gridData.MetaDataList[i].TITUL;
                                    col.Header = header_t;
                                    ToolTipService.SetToolTip(header_t, gridData.MetaDataList[i].TITUL);

                                    

                                    col.DataMemberBinding = new Binding(gridData.MetaDataList[i].FLDNAME.ToUpper());
                                    if (columnAggregat == 0)
                                    {
                                        CountFunction fс = new CountFunction();
                                        fс.Caption = ProjectResources.CountFunctionGrid;//"Всего: ";
                                        col.AggregateFunctions.Add(fс);
                                        columnAggregat = 1;
                                        col.TextDecorations = TextDecorations.Underline;
                                        col.CellStyle = cellStyle;

                                    }
                                    else
                                    {
                                        col.CellStyle = cellStyleAll;
                                    }

                                    grid.Columns.Add(col);    
                                }
                         
                        }
                    }
                }
            }

            var table = new DataTable();
            Type columnType;

            for (int i = 0; i < gridData.GridData.FieldNames.Count; i++)
            {

                columnType = GetDataType(gridData.GridData.FieldNames[i], gridData.MetaDataList);

                var column = new DataColumn();
                column.ColumnName = gridData.GridData.FieldNames[i];
                column.DataType = columnType;
                table.Columns.Add(column);

            }


            for (int i = 0; i < gridData.GridData.Rows.Count; i++)
            {
                var row = new DataRow();
                for (int j = 0; j < gridData.GridData.FieldNames.Count; j++)
                {
                    var tt = gridData.GridData.Rows[i].Cels[j];
                    row[gridData.GridData.FieldNames[j]] = gridData.GridData.Rows[i].Cels[j];
                }
                table.Rows.Add(row);
            }

            //table.Columns.Add(new DataColumn() { ColumnName = "ID", DataType = typeof(int) });
            //table.Columns.Add(new DataColumn() { ColumnName = "Name", DataType = typeof(string) });
            //table.Columns.Add(new DataColumn() { ColumnName = "UnitPrice", DataType = typeof(decimal?) });
            //table.Columns.Add(new DataColumn() { ColumnName = "Date", DataType = typeof(DateTime) });
            //table.Columns.Add(new DataColumn() { ColumnName = "Discontinued", DataType = typeof(bool) });
            if (table.ToList().Count == 0)
            {
                grid.ShowGroupPanel = false;
            }
            else
            {
                grid.ShowGroupPanel = true;
            }
            grid.ItemsSource = table.ToList();

            grid.DataContext = gridData;
        }

        public static Type GetColumnType(int typeIndex)
        {
            //    Text = 1,
            //    CheckBox = 2,
            //    DataPicker = 3,
            //    ComboBox = 4,
            //    TextAndButton = 5,
            //    Label = 6,
            //    Number = 7

            switch (typeIndex)
            {
                case 1:
                case 4:
                case 5:
                case 6:
                case 8:
                case 9:
                case 13:
                case 14:
                    return typeof(string);
                case 2:
                    return typeof(bool);
                   // return typeof(string);
                case 3:
               case 10:
                    return typeof(String);
               // return typeof(DateTime);
                case 7:
                    return typeof(String);
                   // return typeof(decimal);
                default:
                    throw new Exception(String.Format("GetColumnType, Неизвестное значение типа {0}.", typeIndex));
            }
        }

        public static Type GetDataType(string fldName, List<FieldMetaDataItem> metaDataList)
        {
            for (int j = 0; j < metaDataList.Count; j++)
            {

                if (metaDataList[j].FLDNAME.Equals(fldName, StringComparison.OrdinalIgnoreCase))
                {
                    //if ((fldName == "USAGE_VTO_ENE") || (fldName == "NAL_AVT_SIS_UPR"))
                    //{
                    //    return GetColumnType(7);
                    //}
                    //else

                    return GetColumnType(metaDataList[j].TYPECONTROL);
                }

            }
            throw new Exception(String.Format("GetDataType, Для поля {0} не найден тип", fldName));
        }
        //если таблица формируется динамически
        public static string GetArtifactIdByClick(object sender, string nameKeyFld, RadGridView grid)
        {


            //var ggg = gr.GetType().GetMembers().FirstOrDefault(f => f.Name == "ARTIFACT_ID");
            //  var a1 = gr.GetType().GetProperty("ARTIFACT_ID").GetValue(gr, null);
            //var w = (gr as Telerik.Windows.Controls.RadRowItem).Item.GetType().GetFields();



            string result = string.Empty;

            var gr = (sender as UIElement).ParentOfType<GridViewRow>();

            var gr_item = ((System.Linq.Dynamic.DynamicClass)(((RadRowItem)(gr)).Item));
            dynamic gr_d = gr_item;
            //var g5 = g4.ARTIFACT_ID;
            if (gr_d.GetType().GetProperty(nameKeyFld) != null)
            {
                result = gr_d.GetType().GetProperty(nameKeyFld).GetValue(gr_d, null).ToString();
            }




            //можно получить список полей
            var pp = gr_d.GetType().GetProperties();
            List<string> li = new List<string>();

            foreach (var fi in pp)
            {
                // Debug.WriteLine("---> " + fi.Name);
                li.Add(fi.Name);
            }



            //teleric 2009
            // var aa = ((gg2.GetType())gr.Item).ARTIFACT_ID;
            //if (p != null)
            //{
            //    object r = p.GetValue(p1);
            //}

            //if (gr != null)
            //{

            //    var cells = gr.Cells;


            //    if (cells != null)
            //    {
            //        for (int i = 0; i < cells.Count; i++)
            //        {
            //            if (cells[i].Column.Header != null)
            //            {
            //                if (cells[i].Column.Header.ToString().Equals(nameKeyFld,
            //                                                             StringComparison.OrdinalIgnoreCase))
            //                {

            //                    result = ((GridViewCell)(cells[i])).Value.ToString();
            //                    //teleric 2009
            //                    //result = cells[i].Content.ToString();
            //                    break;
            //                }
            //            }
            //        }
            //    }
            //}

            return result;
        }
        //если таблица формируетмя обычно - с заданными полями
        public static string GetKeyByClick(object sender, string nameKeyFld, RadGridView grid)
        {

            string result = string.Empty;

            var gr = (sender as UIElement).ParentOfType<GridViewRow>();

            var gr_item = ((DataEntityList)(((RadRowItem)(gr)).Item));
            dynamic gr_d = gr_item;

            if (gr_d.GetType().GetProperty(nameKeyFld) != null)
            {
                result = gr_d.GetType().GetProperty(nameKeyFld).GetValue(gr_d, null).ToString();
            }








            return result;
        }
    }


}
//"ARTIFACT_ID"



//for (int i = 0; i < gridData.GridData.Rows.Count; i++)
//{
//    var row = new DataRow();
//    for (int j = 0; j < gridData.GridData.FieldNames.Count; j++)
//    {
//        row[gridData.GridData.FieldNames[j]] = gridData.GridData.Rows[i].Cels[j];
//    }
//    table.Rows.Add(row);
//}




//var cell = grid.ChildrenOfType<GridViewCell>();
//.Where(c => c.Content.ToString() == "ARTIFACT_ID").First();
//cell.IsInEditMode = true;
//DynamicClass r;
// var t = ((System.Linq.Dynamic.DynamicClass)(((DataControl)(grid)).ItemsSource));
// var t = ((((DataControl)(grid)).Items.Count));

//var gr2 = (sender as UIElement).ParentOfType<GridViewRowItem>();
//var kk = gr2.GridViewDataControl.Items.Count;


//var p = (sender as UIElement).ParentOfType<RadRowItem>();
//var p1 = p.Item;


//var p2 = p1.GetType().GetFields().Count((f => f.Name == "ARTIFACT_ID"));
//var p_1 = p1.GetType().GetFields().FirstOrDefault(f => f.Name == "ARTIFACT_ID");
//var p_3 = p1.GetType();




//if (p != null)
//{

//    {
//        DynamicClass r;
//        object pp = (r)p.GetValue(p);
//    }

//}









//var a = p1("ARTIFACT_ID", out result);
//DynamicClass1 p2 = (((RadRowItem)p1).Item);

//((DynamicClass1)(p.Item)).ARTIFACT_ID





// var ll = (sender as UIElement).ParentOfType<GridViewRowItem>();


//var row = mm(grid.Items[0], grid.Columns["ARTIFACT_ID"]);

//var mm_1 = mm.Item;

// dynamic d_1 = mm_1;

//var p9 = d_1.Where(c => c.Column.UniqueName == "ARTIFACT_ID");
//    GetType().GetFields().FirstOrDefault(f => f.Name == "ARTIFACT_ID");

// var mm = (ll.GridViewDataControl.CurrentCellInfo.Item).;
//string t = ((((Telerik.Windows.Controls.RadRowItem) (ll)).Item)).ARTIFACT_ID;
//var row = (sender as UIElement).ChildrenOfType<GridViewRow>().Where(rr => rr.ChildrenOfType<GridViewCell>().Where(c => c.Content.ToString() == "ARTIFACT_ID").Any());

//var row = (sender as UIElement).ChildrenOfType<GridViewCell>().Where(c => c.Content.ToString() == "ARTIFACT_ID");

//grid.CurrentCellInfo = new GridViewCellInfo(gridView.Items[5], gridView.Columns["Text"])



//var gr2 = (sender as UIElement).ParentOfType<GridViewCell>();
//var gr3 = (sender as UIElement).ParentOfType<GridViewDataColumn>();

//TextBlock header_t = new TextBlock();
//header_t.Text = gridData.MetaDataList[i].TITUL;
//col.Header = header_t;
//ToolTipService.SetToolTip(header_t, gridData.MetaDataList[i].TITUL);

//    col.DataMemberBinding = new Binding(gridData.MetaDataList[i].FLDNAME.ToUpper());
//    if (columnAggregat == 0)
//    {
//        CountFunction fс = new CountFunction();

//        fс.Caption = ProjectResources.CountFunctionGrid;
//            //"{Binding Source={StaticResource ResProvider}, Path=ProjectResources.checkDetal, Mode=OneWay}" 

//        col.AggregateFunctions.Add(fс);
//        columnAggregat = 1;
//        col.TextDecorations = TextDecorations.Underline;
//        col.CellStyle = cellStyle;

//       // col.Foreground = new SolidColorBrush(Color.FromArgb(
//       //255,
//       //Byte.Parse("0"),
//       //Byte.Parse("150"),
//       //Byte.Parse("219")));

//       // cell.FontWeight = FontWeights.Bold;
//       // cell.Cursor = Cursors.Hand; 
//    }
//    else
//    {
//        col.CellStyle = cellStyleAll;
//    }
//    grid.Columns.Add(col);
//}

