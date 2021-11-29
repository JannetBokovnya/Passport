using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Media;
using Media.Interfaces;
using Media.Resources;
using Passpot.Business;
using Passpot.Controls;

using Passpot.Grid;
using Services.ServiceReference;
using Telerik.Windows.Controls.GridView;
using linkControl.Control;
using Passpot.Model;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;


namespace Passpot
{
    public partial class PassportView : UserControl
    {
        #region Private fields

        private SelectParentDialog _selectParentDialog;
        private ChildWindowDelete _popupWindowDelete;
        private PassportOpenParam _passportOpenParam;
        private ChildWindowAttantion _popupWindowAttantion;
        public FindKeyOnTree findKeyOnTree;
       
        private string artifactIdOnDelete;
        private string _keyPassportOnDelete = "";


        #endregion Private fields

        #region Ctor

        public PassportView()
        {
            InitializeComponent();
            _popupWindowDelete = new ChildWindowDelete();
        }

        #endregion Ctor

        #region Properties

        public PassportModel Model
        {
            get { return DataContext as PassportModel; }
        }

        #endregion Properties


        #region Private methods

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Model.PassportOpenParams.ParentKey == "-1")
            {
                //newDetalPassportButton.Visibility = Visibility.Collapsed;

                var image2 = newDetalPassportButton.Content as Image;
                Uri uri2 = new Uri("/Passpot;component/Images/add_24_d.png", UriKind.Relative);
                image2.Source = new BitmapImage(uri2);
                newDetalPassportButton.IsEnabled = false;

            }
            else
            {
                var image3 = newDetalPassportButton.Content as Image;
                Uri uri3 = new Uri("/Passpot;component/Images/add_24_a.png", UriKind.Relative);
                image3.Source = new BitmapImage(uri3);
                newDetalPassportButton.IsEnabled = true;
            }
            Model.PropertyChanged -= Model_PropertyChanged;
            Model.PropertyChanged += Model_PropertyChanged;

            grid.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(OnSelectionChanged), true);
        }
        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            if (e.PropertyName.Equals("ModelInited"))
            {

                if (DisplayLabel.Dict.ContainsKey("ObjectNameLabelGrid"))
                {
                    tbnameObjGrid.Text = DisplayLabel.Dict["ObjectNameLabelGrid"];
                    tbLabelParentG.Text = DisplayLabel.Dict["ParentNameLabel"];
                }

                if (Model.PassportOpenParams.ParentKey == "-1")
                {
                    //newDetalPassportButton.Visibility = Visibility.Collapsed;
                    var image2 = newDetalPassportButton.Content as Image;
                    Uri uri2 = new Uri("/Passpot;component/Images/add_24_d.png", UriKind.Relative);
                    image2.Source = new BitmapImage(uri2);
                    newDetalPassportButton.IsEnabled = false;

                }
                else
                {
                    var image3 = newDetalPassportButton.Content as Image;
                    Uri uri3 = new Uri("/Passpot;component/Images/add_24_a.png", UriKind.Relative);
                    image3.Source = new BitmapImage(uri3);
                    newDetalPassportButton.IsEnabled = true;
                }

                if (Model.visiblebutonShema == "1")
                {
                    // helpButton.Visibility = Visibility.Collapsed;
                }

                var template = Resources["buttonEditDeleteCellTemplate"] as DataTemplate;

                var cellStyle = Resources["StyleCell"] as Style;
                var cellStyleAll = Resources["StyleCellAll"] as Style;
                var headerCellStyle = Resources["MyHeaderCellStyle"] as Style;
                try
                {

                    GridHelper.BuildPasportGrid(grid, template, cellStyle, cellStyleAll, headerCellStyle, Model, Model.PassportOpenParams.IsShowAllField);
                    Model.StartGetNameObj(Model.PassportKey);



                    //Binding b = new Binding();
                    //b.Mode = BindingMode.TwoWay;
                    //b.ElementName = "grid";
                    //b.Source = radDataPager.PageSize;

                    //radDataPager.SetBinding(RadDataPager.PageSizeProperty, b);
                    
                    //"{Binding Items, ElementName=grid}";
                    //var con = Resources["context"] as ExportingModel;
                    //con.rPager = radDataPager;

                    exportButton.CommandParameter = grid;
                    exportButtonWord.CommandParameter = grid;
                    
                   

                    //grid.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(OnSelectionChanged), true);
                }
                catch (Exception ex)
                {
                    Model.MainModel.Report(ex.Message);
                }


                Model.MainModel.Indicator(false);
                // busyIndicator2.IsBusy = false;
                return;
            }

            if (e.PropertyName.Equals("ObjTypeAndNameP"))
            {
                //nameEntityPlaseStateG.Text = Model.NameObjDetail;
                if (Model.ObjTypeAndName != "")
                    nameEntityPlaseStateP.Text = Model.ObjTypeAndName + ". ";
                if (Model.ObjTypeAndName != "")
                    namePlaseStateP.Text = Model.NameObjDetail + ". ";
                namePlaseStateP.TextTrimming = TextTrimming.WordEllipsis;
                return;
            }

            if (e.PropertyName.Equals("GreedRefresh"))
            {
                var template = Resources["buttonEditDeleteCellTemplate"] as DataTemplate;
                var cellStyle = Resources["StyleCell"] as Style;
                var cellStyleAll = Resources["StyleCellAll"] as Style;
                var headerCellStyle = Resources["MyHeaderCellStyle"] as Style;
                try
                {

                    GridHelper.BuildPasportGrid(grid, template, cellStyle,cellStyleAll, headerCellStyle,  Model, Model.PassportOpenParams.IsShowAllField);
                    Model.StartGetNameObj(Model.PassportKey);

                    exportButton.CommandParameter = grid;
                    exportButtonWord.CommandParameter = grid;

                   // grid.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(OnSelectionChanged), true);


                }
                catch (Exception ex)
                {
                    Model.MainModel.Report(ex.Message);
                }

                Model.IsShow = false;
                //busyIndicator2.IsBusy = false;
                return;
            }

            if (e.PropertyName == "DisplayLabel")
            {
                if (DisplayLabel.Dict.ContainsKey("ObjectNameLabelGrid"))
                {
                    tbnameObjGrid.Text = DisplayLabel.Dict["ObjectNameLabelGrid"];
                    tbLabelParentG.Text = DisplayLabel.Dict["ParentNameLabel"];

                }
            }

            if (e.PropertyName == "GetNameObjOnDelete")
            {
                OpenPopupWindowDelete();
            }

            if (e.PropertyName == "IsCurrentPassportOnDelete")
            {
                _popupWindowAttantion = new ChildWindowAttantion();
                _popupWindowAttantion.titleBox.Text = "У Вас нет доступа для удаления объекта  " + Model.GetNameObjOnDelete; 
                _popupWindowAttantion.Show();
                
                //здесь чайлд виндоус но делете
            }

            if (e.PropertyName.Equals("GridDataCount"))
            {
                Model.IsShow = false;
                //busyIndicator2.IsBusy = false;

            }

            if (e.PropertyName.Equals("NameObj"))
            {
                if (Model.PassportOpenParams.ParentKey == "0")
                {
                    nameEntityObj.Text = ProjectResources.PassportViewNameEntityObj + Model.NameObj; //"  Все объекты   " + Model.NameObj;
                }
                else
                    nameEntityObj.Text = Model.NameObj;

                return;
            }

            if (e.PropertyName.Equals("GetCheckLinkOrganization"))
            {
                switch (Model.GetCheckLinkOrganization)
                {
                    case "1":
                        {
                            Model.MainModel.NeedOpenPassportDetail = new PassportDetailOpenParam() { PassportKey = "0", EntityKey = Model.PassportOpenParams.EntityKey, ParentKey = Model.PassportOpenParams.ParentKey, IsVisibleButtonTransit = 1 };
                            break;
                        }
                    case "-1":
                        {
                            ChildWindowAttantion _popupWindowAttantion = new ChildWindowAttantion();
                            _popupWindowAttantion.titleBox.Text = ProjectResources.CheckLink1; 
                            _popupWindowAttantion.Show();
                            break;
                        }
                    case "-2":
                        {
                            ChildWindowAttantion _popupWindowAttantion = new ChildWindowAttantion();
                            _popupWindowAttantion.titleBox.Text = ProjectResources.CheckLink2;
                            _popupWindowAttantion.Show();
                            break;
                        }
                    case "-3":
                        {
                            ChildWindowAttantion _popupWindowAttantion = new ChildWindowAttantion();
                            _popupWindowAttantion.titleBox.Text = ProjectResources.CheckLink3;
                            _popupWindowAttantion.Show();
                            break;
                        }

                    case "-4":
                        {
                            ChildWindowAttantion _popupWindowAttantion = new ChildWindowAttantion();
                            _popupWindowAttantion.titleBox.Text = ProjectResources.CheckLink4;
                            _popupWindowAttantion.Show();
                            break;
                        }
                    default:
                        {
                            break;
                        }

                }
                return;
            }


        }

        private void OnEditButtonClick(object sender, RoutedEventArgs e)
        {
            string artifactId = GridHelper.GetArtifactIdByClick(sender, "ARTIFACT_ID", grid);

            Model.MainModel.Report("PassportView.OnEditButtonClick(grid) IsShowTreeOnFindkey = " + Model.PassportOpenParams.IsShowTreeOnFindkey);
            if (Model.PassportOpenParams.IsShowTreeOnFindkey)
            {
                string tt = Model.PassportOpenParams.EntityKey;
                string t2 = Model.PassportKey;
                Model.MainModel.NeedOpenPassportDetail = new PassportDetailOpenParam()
                    {
                        PassportKey = artifactId,
                        EntityKey = Model.PassportOpenParams.EntityKey,
                        IsVisibleButtonTransit = 1
                    };

                //по клику в таблице должен в дереве открыться паспорт... - нужен новый алгоритм поиска по дереву

                findKeyOnTree = new FindKeyOnTree(Model.PassportOpenParams.EntityKey);
                Model.MainModel.FirePropertyChanged("FindTreeOnkeyPassport");
            }
            else
                Model.MainModel.NeedOpenPassportDetail = new PassportDetailOpenParam()
                    {
                        PassportKey = artifactId,
                        EntityKey = Model.PassportOpenParams.EntityKey,
                        IsVisibleButtonTransit = 1
                    };
        }


        private void OnDeleteButtonClick(object sender, RoutedEventArgs e)
        {
            //artifactIdOnDelete = GridHelper.GetArtifactIdByClick(sender);
            if (_keyPassportOnDelete == "")
            {
                _popupWindowAttantion = new ChildWindowAttantion();
                _popupWindowAttantion.titleBox.Text = ProjectResources.PassportViewAttantion; //"Не выбран в таблице объект удаления!!!! ";
                _popupWindowAttantion.Show();
            }
            else
            {

                Model.NamepassportOnDelete(_keyPassportOnDelete);

            }



        }

        private void OpenPopupWindowDelete()
        {
            _popupWindowDelete.Title = ProjectResources.GridControlMessageTitle; //"Подтверждение удаления"; 
            _popupWindowDelete.titleBox.Text = ProjectResources.PassportViewPopupWindow + " - " +
                                               Model.GetNameObjOnDelete + "  " + ProjectResources.PassportViewPopupWindow1; //" вместе со связями? ";
            _popupWindowDelete.Show();
            _popupWindowDelete.OKButtonDelete.Click += OKButtonDelete;
        }
        #endregion

        void OKButtonDelete(object sender, RoutedEventArgs e)
        {
            _popupWindowDelete.OKButtonDelete.Click -= OKButtonDelete;

            if (_popupWindowDelete.DialogResult == true)
            {
                //передаем ключ парента
                Model.DeletePassport(_keyPassportOnDelete);
                //var template = Resources["buttonEditDeleteCellTemplate"] as DataTemplate;
                // GridHelper.BuildPasportGrid(grid, template, Model, true);
                // Model.InitModel(Model.PassportOpenParams);
                //grid.Rebind();
            }
        }

        private void NewDetalPassportButton_Click(object sender, RoutedEventArgs e)
        {
            //выборка которая считает парентов... если нет парентов - значит объект самый главный
            //тогда сразу вызываем новый паспорт   Model.MainModel.NeedOpenPassportDetail = "new";
            //иначе открываем по кнопке форму

            // После выполнения этого запроса, измениться свойство CountParent, и модель пришлет сообщение о изменении свойства CountParrent
            //Model.StartGetCountParent(Model.PassportKey);

            //новая методология добавления нового паспорта! - добавляем по кнопке сразу
            //передавать НЕОБХОДИМО ключ парента и ключ сущности для которого будут создаваться метаданные!

            //определяем кардиность дочерней связи - т.е. возможность добавить новый пасапорт
            Model.GetKordinalnostLink(Model.PassportOpenParams.EntityKey, Model.PassportOpenParams.ParentKey, "2", GlobalContext.Context);

           // Model.MainModel.NeedOpenPassportDetail = new PassportDetailOpenParam() { PassportKey = "0", EntityKey = Model.PassportOpenParams.EntityKey, ParentKey = Model.PassportOpenParams.ParentKey, IsVisibleButtonTransit = 1 };


        }
      

        private void checkDetal_Checked(object sender, RoutedEventArgs e)
        {
            var template = Resources["buttonEditDeleteCellTemplate"] as DataTemplate;
            var cellStyle = Resources["StyleCell"] as Style;
            var cellStyleAll = Resources["StyleCellAll"] as Style;
            var headerCellStyle = Resources["MyHeaderCellStyle"] as Style;

            GridHelper.BuildPasportGrid(grid, template, cellStyle, cellStyleAll, headerCellStyle, Model, false);

            exportButton.CommandParameter = grid;
            exportButtonWord.CommandParameter = grid;
          //  grid.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(OnSelectionChanged), true);

        }

        private void checkDetal_Unchecked(object sender, RoutedEventArgs e)
        {
            var template = Resources["buttonEditDeleteCellTemplate"] as DataTemplate;
            var cellStyle = Resources["StyleCell"] as Style;
            var cellStyleAll = Resources["StyleCellAll"] as Style;
            var headerCellStyle = Resources["MyHeaderCellStyle"] as Style;

            GridHelper.BuildPasportGrid(grid, template, cellStyle, cellStyleAll, headerCellStyle, Model, true);

            exportButton.CommandParameter = grid;
            exportButtonWord.CommandParameter = grid;
           // grid.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(OnSelectionChanged), true);
            //Model.MainModel.NeedOpenPassport = new PassportOpenParam() { EntityKey = Model.PassportOpenParams.EntityKey, ParentKey = "", IsShowAllField = false };
        }

        private void helpButton_Click(object sender, RoutedEventArgs e)
        {
            HtmlPage.Window.Invoke("sendEvent", "showHelp", "HelpKey=503", "");
           // MainView.sendEvent("showHelp", "HelpKey=503", "");
        }

        private void OnSelectionChanged(object sender, MouseEventArgs e)
        {
            string artifactId = "";

            UIElement clicked = e.OriginalSource as UIElement;
            if (clicked != null)
            {
                var cel = clicked.ParentOfType<GridViewCell>();
                var row = clicked.ParentOfType<GridViewRow>();
                if (row != null)
                {
                    var gr_item = ((System.Linq.Dynamic.DynamicClass)(((RadRowItem)(row)).Item));
                    dynamic gr_d = gr_item;

                    if (gr_d.GetType().GetProperty("ARTIFACT_ID") != null)
                    {
                        artifactId = gr_d.GetType().GetProperty("ARTIFACT_ID").GetValue(gr_d, null).ToString();

                    }
                }

                if (cel != null)
                {
                    if (cel.Column.AggregateFunctions.Count == 1)
                    {
                        if (row != null)
                        {
                            if (artifactId != null)
                            {
                                Model.MainModel.NeedOpenPassportDetail = new PassportDetailOpenParam() { PassportKey = artifactId, ParentNameKeyOnAdmin = Model.MainModel.NeedOpenPassport.ParentKey };
                                if (Model.PassportOpenParams.IsShowTreeOnFindkey)
                                {
                                    if ((string.IsNullOrEmpty(Model.MainModel.NeedOpenPassport.ParentKey)) ||(Model.MainModel.NeedOpenPassport.ParentKey =="-1"))
                                    {
                                        Model.MainModel.FirePropertyChanged("FindTreeOnkeyPassport");
                                    }
                                    else
                                    {
                                        Model.MainModel.FirePropertyChanged("FindTreeOnkeyPassportIsParent");
                                    }
                                    
                                }

                                Navigation navigation = new Navigation
                                  (
                                         "NeedOpenPassportDetail",
                                         artifactId,
                                         0,
                                         "",
                                         "",
                                         false,
                                         false,
                                         "1",
                                         "",
                                         "",
                                         ""
                                    );
                                Model.MainModel.sBack.Push(navigation);
                                Model.MainModel.FirePropertyChanged("BackTrue");
                                Model.MainModel.sForward.Clear();
                                Model.MainModel.FirePropertyChanged("ForwardFalse");



                                ////по клику в таблице должен в дереве открыться паспорт... - нужен новый алгоритм поиска по дереву

                                //findKeyOnTree = new FindKeyOnTree(Model.PassportOpenParams.EntityKey);
                                //Model.MainModel.FirePropertyChanged("FindTreeOnkeyPassport");
                            }

                        }
                    }

                }
            }
        }


        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //pasportDataScroll.Height = Math.Max(300, this.ActualHeight - 100);
            grid.Height = Math.Max(300, this.ActualHeight - 130);
        }


        void client_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //code.SessionEnd.Redirect();
        }

        private void grid_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (e.AddedItems.Count != 0)
            {
                var tt = grid.SelectedItem.GetType().GetProperty("ARTIFACT_ID").GetValue(grid.SelectedItem, null);
                _keyPassportOnDelete = tt.ToString();
            }
            //_keyPassportOnDelete

        }

        private void exportButton_Click(object sender, RoutedEventArgs e)
        {

            var con = Resources["context"] as ExportingModel;
            con.SelectedExportFormat = "Excel";
           
        }

        private void exportButtonWord_Click(object sender, RoutedEventArgs e)
        {
            var con = Resources["context"] as ExportingModel;
            con.SelectedExportFormat = "Word";
        }

        private void Grid_OnRowLoaded(object sender, RowLoadedEventArgs e)
        {
            if ((e.Row != null) && !(e.Row is GridViewHeaderRow))
            {
                if (e.Row.Cells.Count > 0)
                {

                }


                GridViewCell cell = e.Row.Cells.Cast<GridViewCell>().FirstOrDefault(c => c.Column.UniqueName == "CNAME");
                if (cell != null)
                {
                    cell.Foreground = new SolidColorBrush(Color.FromArgb(
                                       255,
                                       Byte.Parse("0"),
                                       Byte.Parse("150"),
                                       Byte.Parse("219")));

                    cell.FontWeight = FontWeights.Bold;
                    cell.Cursor = Cursors.Hand;
                }
            }
        }

        private void NameEntityPlaseStateP_OnMouseEnter(object sender, MouseEventArgs e)
        {
            namePlaseStateP.Foreground = new SolidColorBrush(Color.FromArgb(255, Byte.Parse("219"), Byte.Parse("255"), Byte.Parse("180")));
            namePlaseStateP.FontStyle = FontStyles.Italic;
            namePlaseStateP.FontWeight = FontWeights.Bold;
        }

        private void NameEntityPlaseStateP_OnMouseLeave(object sender, MouseEventArgs e)
        {
            namePlaseStateP.Foreground = new SolidColorBrush(Colors.White);
            namePlaseStateP.FontStyle = FontStyles.Normal;
            namePlaseStateP.FontWeight = FontWeights.Normal;
        }

        private void NameEntityPlaseStateP_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Model.MainModel.NeedOpenPassportDetail = new PassportDetailOpenParam() { PassportKey = Model.ParentKey };
        }


        private void Grid_OnColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            string tt = "";
        }
    }
}


