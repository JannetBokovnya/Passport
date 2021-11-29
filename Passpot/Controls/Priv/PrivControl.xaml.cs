using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Media.Resources;
using Passpot.Controls.Priv;
using linkControl.Control;
using Media.Interfaces;
using Passpot.Business;
using Passpot.Business.DataTable;
using Passpot.Grid;
using Services.ServiceReference;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;

namespace Passpot.Controls
{
    public partial class PrivControl : UserControl
    {
        private ChildWindowPriv _linkTableWindow;
        private ChildWindowNewPriv _linkTableWindow2;
        private ServiceDataClient _serviceDataClient;
        private PassportDetailModel _passportModel;
        private FieldMetaDataItem _metaData;
        private List<FormParamsPrivItems> FormParams;
        private List<FormTypePrivItems> FormTypePriv;
        private List<FldParamsPrivItems> FldParamsPriv;
        private string artifactIdOnDeletePriv;
        private string nameartifactIdOnDeletePriv;
        private ChildWindowDelete _popupWindowDeleteChild;
        private int CountPriv = 0;
        private bool _editpassport = true;
        private List<PrivItems> _privItemList = new List<PrivItems>();
        private DataTable table = new DataTable();
        private DataRow row = new DataRow();


        private ServiceDataClient ServiceDataClient
        {
            get
            {
                if (_serviceDataClient == null)
                    _serviceDataClient = new ServiceDataClient();
                return _serviceDataClient;
            }
        }


        public PrivControl()
        {
            //LocalizationManager.DefaultResourceManager = ResourceGrid.ResourceManager;
            InitializeComponent();
            _popupWindowDeleteChild = new ChildWindowDelete();
        }

        //+ передавать на вход  FieldMetaDataItem metaData, PassportDetailModel passportModel
        public static PrivControl CreateControl(PassportDetailModel passportModel, bool editpassport)
        {
            var c = new PrivControl();
            c._passportModel = passportModel;
            c._editpassport = editpassport;
            //если паспорт new не показывать  кнопку добавить паспорт




            if (editpassport)
            {
              
                c.addPriv.Visibility = Visibility.Visible;
            }
            else
            {
               
                c.addPriv.Visibility = Visibility.Collapsed;
            }

            if (c._passportModel.PassportDetailOpenParams.PassportKey != "0")
            {
                //запрос в базу который возвращает данные по привязке для грида
                //на главной форме

                c.StartOnGetGridDataPriv(passportModel.PassportKey);

            }
            else
            {
               
               c.addPriv.Visibility = Visibility.Collapsed;

            }

            return c;
        }


        public void StartOnGetGridDataPriv(string inObjKey)
        {
            //возвращает тип привязки и создана или нет привязка для объекта
            ServiceDataClient.GetTypePrivCompleted += ServiceDataClient_GetTypePrivCompleted;
            ServiceDataClient.GetTypePrivAsync(_passportModel.EntityKey, _passportModel.PassportKey, GlobalContext.Context);

        }

        void ServiceDataClient_GetTypePrivCompleted(object sender, GetTypePrivCompletedEventArgs e)
        {
            ServiceDataClient.GetTypePrivCompleted -= ServiceDataClient_GetTypePrivCompleted;
            if (e.Result.IsValid)
            {
                FormTypePriv = new List<FormTypePrivItems>(e.Result.FormParamsTypePrivItemsList);
                _passportModel.ISLOCATIONEXIST = FormTypePriv[0].ISLOCATIONEXIST;
                //_passportModel.TYPEPRIV = "3";

                #region закоментировано  - раскоментировать после тестирования
                //пока закоментировано   - РАССКОМЕНТИРОВАТЬ!!!!!!
                _passportModel.TYPEPRIV = FormTypePriv[0].TYPEPRIV;


                //временно для привязки!!!! _passportModel.ISLOCATIONEXIST == "0"
                // было _passportModel.ISLOCATIONEXIST == "1" - поверить запрос!

   //ВСЕГДА ПОКАЗЫВАЕМ КНОПКУ ДОБАВИТЬ ПРИВЯЗКУ!!!!!!!!!!!!!!!!!!!!!!!!!
   //РЕДАКТИРУЕМ ПО КНОПКЕ!!!

                //if (_passportModel.ISLOCATIONEXIST == "1")
                //{

                //    addPriv.Visibility = Visibility.Collapsed;
                //}
                //else
                //{
                //    if (_editpassport)
                //    {

                //        addPriv.Visibility = Visibility.Visible;
                //    }

                //    else
                //    {

                //        addPriv.Visibility = Visibility.Collapsed;
                //    }
                //}

                #endregion закоментировано  - раскоментировать после тестирования
                //по ключу объекта возвращает название полей в гриде
                ServiceDataClient.GetFldParamsprivCompleted += ServiceDataClient_GetFldParamsprivCompleted;
                ServiceDataClient.GetFldParamsprivAsync(_passportModel.EntityKey, GlobalContext.Context);
            }
            else
            {
                _passportModel.MainModel.Report(" Тип привязки err = " + e.Result.ErrorMessage);
            }
        }

        void ServiceDataClient_GetFldParamsprivCompleted(object sender, GetFldParamsprivCompletedEventArgs e)
        {
            ServiceDataClient.GetFldParamsprivCompleted -= ServiceDataClient_GetFldParamsprivCompleted;
            if (e.Result.IsValid)
            {
                FldParamsPriv = new List<FldParamsPrivItems>(e.Result.FldParamsPrivItemsList);
                //по ключу возвращает привязку - если есть

                //ServiceDataClient.GetAllPrivCompleted += ServiceDataClient_GetAllPrivCompleted;
                //ServiceDataClient.GetAllPrivAsync(_passportModel.PassportKey, GlobalContext.Context);

                //возвращение привязки для контрола типа грид


                //удалила старая привязка!!!

                //ServiceDataClient.GetGridDataPrivCompleted += ServiceDataClient_GetGridDataPrivCompleted;
                //ServiceDataClient.GetGridDataPrivAsync(_passportModel.PassportKey, GlobalContext.Context);
                ////ServiceDataClient.GetGridDataPrivAsync("24852101");

            }
            else
            {
                _passportModel.MainModel.Report(" Поля вывода в грид привязки err = " + e.Result.ErrorMessage);
            }
        }

        //новая привязка
        //void ServiceDataClient_GetAllPrivCompleted(object sender, GetAllPrivCompletedEventArgs e)
        //{
        //    ServiceDataClient.GetAllPrivCompleted -= ServiceDataClient_GetAllPrivCompleted;
        //    if (e.Result.IsValid)
        //    {
        //        _privItemList = new List<PrivItems>(e.Result.PrivItemsList);

        //        if (_privItemList.Count > 0)
        //        {
        //             for (int ii = 0; ii < _privItemList.Count; ii++)
        //             {
        //                 PrivLinkControl plc = PrivLinkControl.CreateLinkControlEdit(_privItemList[ii], _passportModel, _metaData,
        //                                                                          true, true);

        //                 linkPriv.Children.Add(plc);  
        //             }
        //        }
               
                
                
        //    }
        //    else
        //    {
        //        _passportModel.MainModel.Report(" Ошибка выводв привязки err = " + e.Result.ErrorMessage);
        //    }

        //}
       //создание динамически таблицы

        //удалила - старая привязка - просмотр привязки - грид

        //void ServiceDataClient_GetGridDataPrivCompleted(object sender, GetGridDataPrivCompletedEventArgs e)
        //{
        //    string strToExcel = "";
        //    if (e.Error != null)
        //    {

        //    }
        //    else
        //    {

        //        if (e.Result.IsValid)
        //        {
        //            GridData gridData = e.Result;

                    
        //            grid.ItemsSource = null;
        //            grid.Columns.Clear();

        //            var template = Resources["buttonEditDeleteCellTemplate"] as DataTemplate;
        //            var columnButton = new GridViewColumn { CellTemplate = template };
        //            columnButton.Width = 40;
        //            grid.Columns.Add(columnButton);


        //            //создаем новые поля в гриде
        //            for (int i = 0; i < FldParamsPriv.Count; i++)
        //            {
        //                var col = new GridViewDataColumn();
        //                col.IsFilterable = false;
        //                if (FldParamsPriv[i].ISPK == 1)
        //                {
        //                    col.IsVisible = false;
        //                    nameartifactIdOnDeletePriv = FldParamsPriv[i].CNAME;
        //                }
        //                col.DataType = GetColumnType(FldParamsPriv[i].CDATA_TYPE);
        //                col.UniqueName = FldParamsPriv[i].CEXPR.ToUpper();
        //                //ToolTip
        //                TextBlock header_t = new TextBlock();
        //                header_t.Text = FldParamsPriv[i].CNAME;
        //                col.Header = header_t;
        //                ToolTipService.SetToolTip(header_t, FldParamsPriv[i].CCAPTION);

        //                //col.Header = FldParamsPriv[i].CNAME;
        //                col.DataMemberBinding = new Binding(FldParamsPriv[i].CEXPR.ToUpper());
        //                grid.Columns.Add(col);
        //            }


        //            if (gridData != null)
        //            {
        //                if (gridData.Rows.Count == 0)
        //                {

        //                }
        //                else
        //                {

                           
        //                    for (int i = 0; i < gridData.FieldNames.Count; i++)
        //                    {
        //                        var column = new DataColumn();
        //                        column.ColumnName = gridData.FieldNames[i];
        //                        gridData.GetType();
        //                        //column.DataType =gridData.GetType();
        //                        table.Columns.Add(column);
        //                    }

        //                    for (int i = 0; i < gridData.Rows.Count; i++)
        //                    {

        //                        for (int j = 0; j < gridData.FieldNames.Count; j++)
        //                        {
        //                            {
        //                                row[gridData.FieldNames[j]] = gridData.Rows[i].Cels[j];
        //                                //данные для вывода в ексель
        //                                for (int i1 = 0; i1 < FldParamsPriv.Count; i1++)
        //                                {
        //                                    if (gridData.FieldNames[j].ToLower() == FldParamsPriv[i1].CEXPR.ToLower())
        //                                    {
        //                                        if (FldParamsPriv[i1].ISPK != 1)
        //                                        {
        //                                            strToExcel = strToExcel + FldParamsPriv[i1].CCAPTION + " : " + gridData.Rows[i].Cels[j] + "; ";
        //                                        }

        //                                    }
        //                                }
        //                            }

        //                        }
        //                        table.Rows.Add(row);
        //                    }
        //                    CountPriv = table.Rows.Count;
        //                    grid.ItemsSource = table.ToList();
        //                    _passportModel.ListRelationObjList.Add(new ListRelationObj("PRIV", strToExcel));


        //                    //for (int ic=0; ic < table.Columns.Count; ic++)
        //                    //{

        //                    //    for (int iif=0; iif<FldParamsPriv.Count; iif++)
        //                    //       {

        //                    //           if (table.Columns[ic].ColumnName == FldParamsPriv[iif].CEXPR)
        //                    //           {
        //                    //               strToExcel = strToExcel + FldParamsPriv[iif].CCAPTION + ":";
        //                    //               for (int ir = 0; ir < table.Rows.Count; ir++)
        //                    //               {

        //                    //                  // strToExcel = strToExcel + table.Columns[1

        //                    //               }
        //                    //           }


        //                    //       }
        //                    //http://forums.asp.net/t/1736538.aspx/2/10
        //                    //    foreach (DataGridViewRow row in tb_checkDataGridView.Rows)
        //                    //    {
        //                    //        foreach (DataGridViewCell cell in row.Cells)
        //                    //        {
        //                    //            strText = cell.Value.ToString();
        //                    //            oTable.Cell(r + 1, c).Range.Text = strText;
        //                    //        }
        //                    //    }


        //                    //    //strToExcel = strToExcel+ grid.Columns[ii].Header +":"+
        //                    //}

        //                }

        //            }
        //        }
        //        else
        //        {
        //            _passportModel.MainModel.Report("Ошибка при выводе данных!!! OnGetGridDataCompleted err = " + e.Result.ErrorMessage);
        //        }
        //    }
        //}

        private void addPriv_Click(object sender, RoutedEventArgs e)
        {
            var m = DataContext as PassportDetailModel;

            //добавление старой привязки
            //_linkTableWindow = new ChildWindowPriv();
            //_linkTableWindow.Title = ProjectResources.PrivControlLinkTableWindowTitle;  //"Привязка объекта ";
            //_linkTableWindow.DataContext = m;
            ////_linkTableWindow.InitConstraint_Add(CountPriv);
            //_linkTableWindow.InitConstraint_Add(grid.Items.Count);
            //_linkTableWindow.Show();




            //добавление новой привязки
            _linkTableWindow2 = new ChildWindowNewPriv();
            _linkTableWindow2.Title = ProjectResources.PrivControlLinkTableWindowTitle;  //"Привязка объекта ";
            _linkTableWindow2.DataContext = m;
           
            // _linkTableWindow2.NavigationPrivList 
            //нужно разобраться!!! нужна  переменная - есть привязка или нет, для нового контрола
            //_linkTableWindow2.InitConstraint_Add(_privItemList.Count);


            //убрала
            //_linkTableWindow2.InitConstraint_Add(grid.Items.Count);
            //_linkTableWindow2.Show();


            //_linkTableWindow.buCreatePrivKm.Click += (buCreatePriv_Click);
            //_linkTableWindow.buCreatePrivKoord.Click += (buCreatePriv_Click);

        }

        void buCreatePriv_Click(object sender, RoutedEventArgs e)
        {
            if (_linkTableWindow.DialogResult == true)
            {
                StartOnGetGridDataPriv(_passportModel.PassportKey);
            }

        }

        public static Type GetColumnType(string typeFld)
        {
            switch (typeFld)
            {
                case "VARCHAR2(2000)":
                case "VARCHAR(MAX)":
                    return typeof(string);
                case "NUMBER":
                case "NUMERIC(20,0)": 
                    return typeof(decimal);
                default:
                    throw new Exception(String.Format("GetColumnType, Неизвестное значение типа {0}.", typeFld));
            }
        }


        //удаление привязки
        private void OnDeleteButtonClick(object sender, RoutedEventArgs e)
        {
            //временно убрала грид и кнопку
            artifactIdOnDeletePriv = GridHelper.GetArtifactIdByClick(sender, nameartifactIdOnDeletePriv.ToUpper(), grid);

            _popupWindowDeleteChild.Title = ProjectResources.GridControlMessageTitle; // "Подтверждение удаления";
            _popupWindowDeleteChild.titleBox.Text = ProjectResources.PrivControlDelete; //"Удалить привязку?";
            _popupWindowDeleteChild.Show();
            _popupWindowDeleteChild.OKButtonDelete.Click += OKButtonDelete;
        }

        void OKButtonDelete(object sender, RoutedEventArgs e)
        {
            _popupWindowDeleteChild.OKButtonDelete.Click -= OKButtonDelete;

            if (_popupWindowDeleteChild.DialogResult == true)
            {

                //удаляем данные и перестраиваем таблицу!!!

                ServiceDataClient.DeleteLinkToPipeByKeyCompleted += ServiceDataClient_DeleteLinkToPipeByKeyCompleted;
                ServiceDataClient.DeleteLinkToPipeByKeyAsync(artifactIdOnDeletePriv, GlobalContext.Context);

                //ServiceDataClient.DELETELOCATIONCompleted += ServiceDataClient_DELETELOCATIONCompleted;
                //ServiceDataClient.DELETELOCATIONAsync(_passportModel.PassportKey);
                //Model.DeletePassport(artifactIdOnDeleteChild);
                //ServiceDataClient.GetGridDataCompleted += OnGetGridDataCompleted;
                //ServiceDataClient.GetGridDataAsync(_keyChild, "0", "1", _currentPassportKey);


            }
        }

        void ServiceDataClient_DeleteLinkToPipeByKeyCompleted(object sender, DeleteLinkToPipeByKeyCompletedEventArgs e)
        {
            ServiceDataClient.DeleteLinkToPipeByKeyCompleted -= ServiceDataClient_DeleteLinkToPipeByKeyCompleted;
            if (e.Result.IsValid)
            {
                StartOnGetGridDataPriv(_passportModel.PassportKey);

                //временно убрала грид и кнопку
                //addPriv.Visibility = Visibility.Visible;
            }
            else
            {
                _passportModel.MainModel.Report(" Удаление привязки err = " + e.Result.ErrorMessage);
            }
        }



        private void PrivControl_Loaded(object sender, RoutedEventArgs e)
        {
            _passportModel.PropertyChanged -= Model_PropertyChanged;
            _passportModel.PropertyChanged += Model_PropertyChanged;
        }



        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("ReloadGridPriv"))
            {
                StartOnGetGridDataPriv(_passportModel.PassportKey);
            }

        }

    }
}

//void ServiceDataClient_DELETELOCATIONCompleted(object sender, DELETELOCATIONCompletedEventArgs e)
//       {
//          ServiceDataClient.DELETELOCATIONCompleted -= ServiceDataClient_DELETELOCATIONCompleted;
//           if (e.Result.IsValid)
//           {
//               StartOnGetGridDataPriv(_passportModel.PassportKey);
//               addPriv.Visibility = Visibility.Visible;
//           }
//           else
//           {
//               _passportModel.MainModel.Report(" Удаление привязки err = " + e.Result.ErrorMessage);
//           }
//       }