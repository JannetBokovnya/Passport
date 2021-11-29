using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Media;
using Media.Interfaces;
using Media.Resources;
using Passpot.Business;
using Passpot.Grid;
using Passpot.Model;
using Services.ServiceReference;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Data;

namespace Passpot.Controls
{
    public partial class ChildWindowLookAllLink : ChildWindow
    {
        public string KeyPassportRelation = "";
        private ChildWindowDelete _popupWindowDelete;
        List<DataListRelationObjItems> _relationList = new List<DataListRelationObjItems>();
        private string nameartifactIdOnDeletePriv = "";
        private bool _isEdit ;


        public event PropertyChangedEventHandler PropertyChanged;

        public void FirePropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }

        //public LinkModel LinkModel
        //{
        //    get { return DataContext as LinkModel; }
        //}

        public PassportDetailModel Model
        {
            get { return DataContext as PassportDetailModel; }
        }

        //private MainModel Model
        //{
        //    get { return DataContext as MainModel; }
        //}

        public ChildWindowLookAllLink()
        {
            InitializeComponent();
        }

        public void GetGridLinkData(List<DataListRelationObjItems> relationList, bool isEdit, List<DataConnectList> DataConnectList)
        {
            _relationList = relationList;
            _isEdit = isEdit;

            if (DataConnectList.Count > 1)
            {
                tbNameObj.Visibility = Visibility.Collapsed;
                GroupDescriptor descriptor = new GroupDescriptor();
                descriptor.Member = "NameEntity";
                descriptor.DisplayContent = ProjectResources.GridRelationNameEntity;
                grid.GroupDescriptors.Add(descriptor);

                //     <!--<telerikGridView:RadGridView.GroupDescriptors>
                //    <data:GroupDescriptor  Member="NameEntity" 
                //                           DisplayContent="{Binding Source={StaticResource ResProvider}, Path=ProjectResources.GridRelationNameEntity,  Mode=OneWay}"/>
                //</telerikGridView:RadGridView.GroupDescriptors>-->

            }
            else
            {
                tbNameObj.Text = DataConnectList[0].NAMECONNECT;
            }

            BuildGrid(isEdit);
              
            grid.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(OnSelectionChanged), true);  

        }

        public void BuildGrid(bool isEdit)
        {
            if (isEdit)
            {
                var templateStyle = Resources["buttonDeleteCellTemplate"] as DataTemplate;
                grid.ItemsSource = null;
                grid.Columns.Clear();
                var columnButton = new GridViewColumn { CellTemplate = templateStyle };
                columnButton.Width = 40;
                grid.Columns.Add(columnButton);     
            }

            var col = new GridViewDataColumn();
            col.UniqueName = "KeyObj";
            col.Header = "KeyObj";
            nameartifactIdOnDeletePriv = "KeyObj";
            col.IsVisible = false;
            grid.Columns.Add(col);

            var headerStyleCell = Resources["HeaderStyleCell"] as Style;
            col = new GridViewDataColumn { HeaderCellStyle = headerStyleCell };
            col.UniqueName = "NameEntity";
            col.Header = ProjectResources.GridRelationNameEntity;
            col.IsVisible = true;
            col.HeaderTextAlignment = TextAlignment.Center;
            grid.Columns.Add(col);

             if (isEdit)
             {
                 col = new GridViewDataColumn { HeaderCellStyle = headerStyleCell };
                 col.UniqueName = "NameObj";
                 col.Header = ProjectResources.GridRelationNameObj;
                 col.IsVisible = true;
                 col.HeaderTextAlignment = TextAlignment.Center;
                 grid.Columns.Add(col);
             }
             else
             {
                 var cellStyle = Resources["StyleCell"] as Style;
                 col = new GridViewDataColumn { HeaderCellStyle = headerStyleCell };
                 col.UniqueName = "NameObj";
                 col.CellStyle = cellStyle;
                 col.Header = ProjectResources.GridRelationNameObj;
                 col.IsVisible = true;
                 col.HeaderTextAlignment = TextAlignment.Center;
                 col.TextDecorations = TextDecorations.Underline;
                 grid.Columns.Add(col);     
             }
            
            grid.ItemsSource = _relationList;

        }


         //обрабатывается клик мыши в таблице
        private void OnSelectionChanged(object sender, MouseEventArgs e)
        {
            
            UIElement clicked = e.OriginalSource as UIElement;
            if (clicked != null)
            {

                var row = clicked.ParentOfType<GridViewRow>();
                if (row != null)
                {
                    var gr_item = ((DataListRelationObjItems)(((RadRowItem)(row)).Item));
                    dynamic gr_d = gr_item;

                    if (gr_d.GetType().GetProperty("KeyObj") != null)
                    {
                        KeyPassportRelation = gr_d.GetType().GetProperty("KeyObj").GetValue(gr_d, null).ToString();

                        if (!_isEdit)
                        {
                            Model.MainModel.NeedOpenPassportDetail = new PassportDetailOpenParam() { PassportKey = KeyPassportRelation };
                            Model.FirePropertyChanged("FindTreeOnkeyPassport");

                            Navigation navigation = new Navigation
                                         (
                                                "NeedOpenPassportDetail",
                                                KeyPassportRelation,
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
                           Model.FirePropertyChanged("BackTrue");
                            Model.MainModel.sForward.Clear();
                            Model.FirePropertyChanged("ForwardFalse");                       
                        }
                        else
                        {
                            if (KeyPassportRelation != "")
                            {
                                _popupWindowDelete = new ChildWindowDelete();
                                _popupWindowDelete.Title = ProjectResources.GridControlMessageTitle; //"Подтверждение удаления";
                                _popupWindowDelete.titleBox.Text = ProjectResources.GridControlMessageDel + "   " + gr_d.GetType().GetProperty("NameObj").GetValue(gr_d, null).ToString();
                                _popupWindowDelete.Show();
                                _popupWindowDelete.OKButtonDelete.Click += OKButtonDelete;
                            }
                        }

                        Close();
                    }


                }
            }
        }


        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            
            var gr = (sender as UIElement).ParentOfType<GridViewRow>();

            var gr_item = ((DataListRelationObjItems)(((RadRowItem)(gr)).Item));
            dynamic gr_d = gr_item;

            if (gr_d.GetType().GetProperty("KeyObj") != null)
            {
                KeyPassportRelation = gr_d.GetType().GetProperty("KeyObj").GetValue(gr_d, null).ToString();
            }

             if (KeyPassportRelation != "")
             {
                 _popupWindowDelete = new ChildWindowDelete();
                 _popupWindowDelete.Title = ProjectResources.GridControlMessageTitle; //"Подтверждение удаления";
                 _popupWindowDelete.titleBox.Text = ProjectResources.GridControlMessageDel + "   " + gr_d.GetType().GetProperty("NameObj").GetValue(gr_d, null).ToString(); 
                 _popupWindowDelete.Show();
                 _popupWindowDelete.OKButtonDelete.Click += OKButtonDelete;
             }
        }

        void OKButtonDelete(object sender, RoutedEventArgs e)
        {
            _popupWindowDelete.OKButtonDelete.Click -= OKButtonDelete;

            if (_popupWindowDelete.DialogResult == true)
            {
                for (int ii = 0; ii < _relationList.Count; ii++)
                {

                    if (_relationList[ii].KeyObj == KeyPassportRelation)
                    {
                        _relationList.RemoveAt(ii);
                    }

                }

                grid.ItemsSource = null;
                grid.ItemsSource = _relationList;
               // Model.KeyPassportOnDeleteLinkOnButton = KeyPassportRelation;
                //Model.FirePropertyChanged("KeyLinkDeleteControlBu"); 
            }
        }

       
    }
}

