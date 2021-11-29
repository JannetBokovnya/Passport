using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Media.Resources;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;

namespace Passpot.Business
{
    public class ExportCommand : ICommand
    {
        private readonly ExportingModel model;

        public ExportCommand(ExportingModel model)
        {
            this.model = model;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            this.model.Export(parameter);
        }
    }

    public class ExportingModel : ViewModelBase
    {
        public ExportingModel()
        {
            this.PageSize = 25;
            this.ExportCommand = new ExportCommand(this);
        }

        private ExportCommand exportCommand = null;

        public ExportCommand ExportCommand
        {
            get
            {
                return this.exportCommand;
            }
            set
            {
                if (this.exportCommand != value)
                {
                    this.exportCommand = value;
                    OnPropertyChanged("ExportCommand");
                }
            }
        }


        private int pageSize;

        public int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                if (this.PageSize != value)
                {
                    pageSize = value;
                    OnPropertyChanged("PageSize");
                }
            }
        }


        private int pageIndex;

        public int PageIndex
        {
            get
            {
                return pageIndex;
            }
            set
            {
                if (this.pageIndex != value)
                {
                    pageIndex = value;
                    OnPropertyChanged("PageIndex");
                }
            }
        }

        public void Export(object parameter)
        {
            var grid = parameter as RadGridView;
            if (grid != null)
            {
                
                grid.ElementExporting -= this.ElementExporting;
                grid.ElementExporting += this.ElementExporting;

                string extension = "";
                var format = ExportFormat.Html;

                switch (SelectedExportFormat)
                {
                    case "Excel": extension = "xls";
                        format = ExportFormat.Html;
                        break;
                    case "ExcelML": extension = "xml";
                        format = ExportFormat.ExcelML;
                        break;
                    case "Word": extension = "doc";
                        format = ExportFormat.Html;
                        break;
                    case "Csv": extension = "csv";
                        format = ExportFormat.Csv;
                        break;
                }


                var dialog = new SaveFileDialog();
                dialog.DefaultExt = extension;
                dialog.Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", extension, SelectedExportFormat);
                dialog.FilterIndex = 1;

                //if (rPager.Source != null)
                //{
                //    this.grdDevices.Export(myStream, new GridViewExportOptions { Items = this.gridPager.Source as IEnumerable<MyDataObject });
                //}

               

                if (dialog.ShowDialog() == true)
                {
                    int originalPageSize = this.PageSize;
                    int originalPageIndex = this.PageIndex;

                    using (var stream = dialog.OpenFile())
                    {
                        this.PageSize = 0;
                        GridViewExportOptions exportOptions = new GridViewExportOptions();
                        exportOptions.Format = format;
                        exportOptions.ShowColumnFooters = true;
                        exportOptions.ShowColumnHeaders = true;
                        exportOptions.ShowGroupFooters = true;
                        exportOptions.Encoding = Encoding.Unicode;

                        grid.Export(stream, exportOptions);
                    }

                    this.PageSize = originalPageSize;
                    this.PageIndex = originalPageIndex;
                   
                }
            }
        }

        IEnumerable<string> exportFormats;
        public IEnumerable<string> ExportFormats
        {
            get
            {
                if (exportFormats == null)
                {
                    exportFormats = new string[] { "Excel", "ExcelML", "Word", "Csv" };
                }

                return exportFormats;
            }
        }

        string selectedExportFormat;

        public string SelectedExportFormat
        {
            get
            {
                return selectedExportFormat;
            }
            set
            {
                if (!object.Equals(selectedExportFormat, value))
                {
                    selectedExportFormat = value;

                    OnPropertyChanged("SelectedExportFormat");
                }
            }
        }

        private void ElementExporting(object sender, GridViewElementExportingEventArgs e)
        {

            if (e.Value != null && e.Value.GetType() == typeof(TextBlock))
                e.Value = (e.Value as TextBlock).Text;

          
            if (e.Element == ExportElement.HeaderRow || e.Element == ExportElement.FooterRow
                || e.Element == ExportElement.GroupFooterRow)
            {
               
                e.Background = HeaderBackground;
                e.Foreground = HeaderForeground;
                e.FontSize = 20;
                e.FontWeight = FontWeights.Bold;
                
            }
            else if (e.Element == ExportElement.Row)
            {
                e.Background = RowBackground;
                e.Foreground = RowForeground;
            }
            else if (e.Element == ExportElement.Cell &&
                e.Value != null && e.Value.Equals("Chocolade"))
            {
                e.FontFamily = new FontFamily("Verdana");
                e.Background = Colors.LightGray;
                e.Foreground = Colors.Blue;
            }
            else if (e.Element == ExportElement.GroupHeaderRow)
            {
                e.FontFamily = new FontFamily("Verdana");
                e.Background = Colors.LightGray;
                e.Height = 30;
            }
            else if (e.Element == ExportElement.GroupHeaderCell &&
                e.Value != null && e.Value.Equals("Chocolade"))
            {
                e.Value = "MyNewValue";
            }


            if ((e.Element == ExportElement.GroupFooterCell) || (e.Element == ExportElement.FooterCell))
            {
                GridViewDataColumn column = e.Context as GridViewDataColumn;
               
                if (column != null && column.AggregateFunctions!= null)
                {
                    if (column.AggregateFunctions.Count > 0)
                    {
                        string group = e.Value.ToString();
                        e.Value = ProjectResources.CountFunctionGrid + " " + group;
                        e.Background = HeaderBackground;
                        e.Foreground = HeaderForeground;
                        e.FontSize = 20;
                        e.FontWeight = FontWeights.Bold;
                    }
                    
                }
            }

        }

        private string GetAggregates(QueryableCollectionViewGroup group, GridViewDataColumn column)
        {
            List<string> aggregates = new List<string>();

            foreach (AggregateFunction f in column.AggregateFunctions)
            {
                foreach (AggregateResult r in group.AggregateResults)
                {
                    if (f.FunctionName == r.FunctionName && r.FormattedValue != null)
                    {
                        aggregates.Add(r.FormattedValue.ToString());
                    }
                }
            }

            return String.Join(",", aggregates.ToArray());
        }


        private Color headerBackground = Colors.LightGray;
        public Color HeaderBackground
        {
            get
            {
                return this.headerBackground;
            }
            set
            {
                if (this.headerBackground != value)
                {
                    this.headerBackground = value;
                    OnPropertyChanged("HeaderBackground");
                }
            }
        }

        private Color rowBackground = Colors.White;
        public Color RowBackground
        {
            get
            {
                return this.rowBackground;
            }
            set
            {
                if (this.rowBackground != value)
                {
                    this.rowBackground = value;
                    OnPropertyChanged("RowBackground");
                }
            }
        }

        Color headerForeground = Colors.Black;
        public Color HeaderForeground
        {
            get
            {
                return this.headerForeground;
            }
            set
            {
                if (this.headerForeground != value)
                {
                    this.headerForeground = value;
                    OnPropertyChanged("HeaderForeground");
                }
            }
        }

        Color rowForeground = Colors.Black;

        public Color RowForeground
        {
            get
            {
                return this.rowForeground;
            }
            set
            {
                if (this.rowForeground != value)
                {
                    this.rowForeground = value;
                    OnPropertyChanged("RowForeground");
                }
            }
        }
    }
}
//GridViewDataColumn column = e.Context as GridViewDataColumn;
//QueryableCollectionViewGroup qcvGroup = e.Value as QueryableCollectionViewGroup;

//if (column != null && qcvGroup != null)
//{
//    if (column.AggregateFunctions.Count > 0)
//    {
//        e.Value = "Всего: " + GetAggregates(qcvGroup, column);
//        e.FontSize = 15;
//    }
//    else
//    {
//        e.Value = GetAggregates(qcvGroup, column);
//    }

//}
