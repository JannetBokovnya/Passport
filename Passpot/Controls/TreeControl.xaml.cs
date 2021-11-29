using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Media;
using Media.Interfaces;
using Passpot.Business;
using Services.ServiceReference;
using Telerik.Windows.Controls;
using System.Windows.Media;
using Telerik.Windows.Controls.GridView;
using System.Collections.Generic;
using Passpot.Model;
using MouseButtonEventArgs = Telerik.Windows.Input.MouseButtonEventArgs;


namespace Passpot.Controls
{
    public partial class TreeControl : UserControl
    {
        private RadTreeViewItem currentItem;

        private bool _isShowBusyTree = false;
        public event PropertyChangedEventHandler PropertyChanged;


        private RadTreeViewItem _lastSelected;
        private object _item;
        private ObservableCollection<DataEntityList> dataEntityList;

        private RadTreeViewItem _lastSelected_2;
        private RadTreeViewItem _lastSelectedDropKeyPressed;
        private RadTreeViewItem _lastDragOnDrop;
        private RadTreeViewItem currentItemDrag;
        private string _keyPassport_onCopy;
        private string _keyPassport_onDrop;
        private string _keyPassport_onDropParent;
        private string _namePassport_onDrop;
        private string _namePassport_onDropParent;
        private string _namePassport_onDrop1 = "";
        private string _namePassport_onDropParent1 = "";
        string keyChald = "";
        string keyParent = "";
        private string keyPassportOnGrid = "";

        private ServiceDataClient _serviceDataClient;
        private ServiceDataClient proxy;
        private string _rootKey;
        private ChildWindowDelete _popupWindowDelete;
        private string keyDragStart;
        private string parentOnChald = "1";
        private RadTreeViewDragEndedEventArgs edd;
        private ChildWindowAttantion _popupWindowAttantion;
        private string[] arr;
        public IMainModel MainModel { get; private set; }
        public List<ObjOnTree> Source { get; private set; }
        public string _accesslevelTree;
        private string keyEntity;

        private string keyPressed = "";
        private RadTreeViewItem _selectedItem;
        private string _visibleNode;
        private ObservableCollection<ObjByType> objByTypes = new ObservableCollection<ObjByType>();
        private ObjByType objByType;
        private ObservableCollection<TreeFullHierarchyPathToObjectItems> selectionPath = new ObservableCollection<TreeFullHierarchyPathToObjectItems>();

        public string AccesslevelInTree
        {
            get { return _accesslevelTree; }
            set
            {
                _accesslevelTree = value;
                FirePropertyChanged("VisiblePassport");
            }
        }

        public MyViewModelObjByType MyViewModelObjByType
        {
            get { return _myViewModelObjByType; }
            set
            {
                _myViewModelObjByType = value;

            }
        }

        public bool IsShowBusyTree
        {
            get { return _isShowBusyTree; }
            protected set
            {
                _isShowBusyTree = value;
                FirePropertyChanged("IsShowBusyTree");
            }
        }

        public string RootKey
        {
            get { return _rootKey; }
            set
            {
                _rootKey = value;

            }
        }

        public void FirePropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }

        private MainModel Model { get; set; }
 
        private ServiceDataClient ServiceDataClient
        {
            get
            {
                if (_serviceDataClient == null)
                    _serviceDataClient = new ServiceDataClient();
                return _serviceDataClient;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Model.PropertyChanged -= Model_PropertyChanged;
            Model.PropertyChanged += Model_PropertyChanged;
            //пока закоментирована вызов кнопки копировать ветку
            // ServiceDataClient.BuCopyVisibleCompleted += ServiceDataClient_BuCopyVisibleCompleted;
            // ServiceDataClient.BuCopyVisibleAsync(GlobalContext.Context);
            //MyViewModelObjByType = new MyViewModelObjByType();
            //Model.Report("!!!!!!!!!!!пока закоментирована вызов кнопки копировать ветку вызов из базы BuCopyVisible");

        }

        void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if ("EditPassportSaved".Equals(e.PropertyName))
            {
                if (_lastSelected != null)
                {
                    _lastSelected.Items.Clear();
                }
            }

            if (e.PropertyName == "DisplayLabel")
            {
                if (DisplayLabel.Dict.ContainsKey("ObjectTreeTab"))
                {
                    treeTabItem.Header = DisplayLabel.Dict["ObjectTreeTab"];

                }
                if (DisplayLabel.Dict.ContainsKey("ObjectTypesTab"))
                {
                    if (!string.IsNullOrEmpty(DisplayLabel.Dict["ObjectTypesTab"].ToString()))
                    {
                        gridTabItem.Header = DisplayLabel.Dict["ObjectTypesTab"];
                    }
                    else
                    {
                        gridTabItem.Visibility = Visibility.Collapsed;
                    }
                    
                }
            }
            if (e.PropertyName == "FindTreeOnkeyPassport")
            {
                Model.Report("пришло событие FindTreeOnkeyPassport");
                AddNewTreeOnkey();
            }
            if ("BackTrue".Equals(e.PropertyName))
            {
                btnBack.IsEnabled = true;
            }
            if ("ForwardFalse".Equals(e.PropertyName))
            {
                btnForward.IsEnabled = false;
            }

        }

        public TreeControl()
        {
            InitializeComponent();
            _popupWindowDelete = new ChildWindowDelete();
            proxy = new ServiceDataClient();

        }

        #region Создание дерева
        public static TreeControl CreateTree(string rootKey, string typeTree, string visibleNode, MainModel model)
        {

            var c = new TreeControl();
            if (c.treeTabItem.IsSelected == false)
            {
                c.treeTabItem.IsSelected = true;
            }
           
            
            c.RootKey = rootKey;
            c.busyIndicatorTree1.IsBusy = false;
            c.Model = model;
            if (typeTree == "2")
            {
                //проверяем TreeFullHierarchyPathToObjectItems;
                c.proxy.GetTreeFullHierarchyPathToObjectCompleted +=c.proxy_GetTreeFullHierarchyPathToObjectCompleted1;
                c.proxy.GetTreeFullHierarchyPathToObjectAsync(c._rootKey, GlobalContext.Context);
            }
            else
            {
                c.TreeHolder.Children.Clear();
                var treeControl_Only = TreeControl_Only.CreateTree(rootKey, typeTree, visibleNode, c.selectionPath);
                treeControl_Only.DataContext = c.Model;
                c.TreeHolder.Children.Add(treeControl_Only);
            }
           
            return c;
        }

        void proxy_GetTreeFullHierarchyPathToObjectCompleted1(object sender, GetTreeFullHierarchyPathToObjectCompletedEventArgs e)
        {
            proxy.GetTreeFullHierarchyPathToObjectCompleted -= proxy_GetTreeFullHierarchyPathToObjectCompleted1;

            if (e.Error != null)
            {
                Model.Report("TreeControl.CreateRootOfTreeCompleted = " + e.Error.Message);
                return;
            }

            selectionPath = new ObservableCollection<TreeFullHierarchyPathToObjectItems>();
            selectionPath = e.Result.TreeFullHierarchyPathToObjectList;

            if (selectionPath.Count > 0)
            {
                TreeHolder.Children.Clear();
                var treeControl_Only = TreeControl_Only.CreateTree(Model.KeyOntree, "2", Model.VisibleNode, selectionPath);
                treeControl_Only.DataContext = Model;
                TreeHolder.Children.Add(treeControl_Only);
            }
        }

        #endregion

        #region кнопка Refresh - обновить дерево
        private void btnRefreshTree_Click(object sender, RoutedEventArgs e)
        {
            Model.FirePropertyChanged("RefreshTree");
            Model.FirePropertyChanged("ClosePassportDetal");
        }
        #endregion


        private void AddNewTreeOnkey()
        {
            if (treeTabItem.IsSelected == false)
            {
                treeTabItem.IsSelected = true;
            }

            //проверяем TreeFullHierarchyPathToObjectItems;
            proxy.GetTreeFullHierarchyPathToObjectCompleted += proxy_GetTreeFullHierarchyPathToObjectCompleted;
            proxy.GetTreeFullHierarchyPathToObjectAsync(Model.NeedOpenPassportDetail.PassportKey, GlobalContext.Context);

           
        }

        //по ключу объекта разворачиваем дерево
        void proxy_GetTreeFullHierarchyPathToObjectCompleted(object sender, GetTreeFullHierarchyPathToObjectCompletedEventArgs e)
        {
            proxy.GetTreeFullHierarchyPathToObjectCompleted -= proxy_GetTreeFullHierarchyPathToObjectCompleted;

            if (e.Error != null)
            {
                Model.Report("TreeControl.CreateRootOfTreeCompleted = " + e.Error.Message);
                return;
            }

            selectionPath = new ObservableCollection<TreeFullHierarchyPathToObjectItems>();
            selectionPath = e.Result.TreeFullHierarchyPathToObjectList;
            Model.Report("GetTreeFullHierarchyPathToObjectCompleted Count =" + selectionPath.Count);
            if (selectionPath.Count > 0)
            {
                TreeHolder.Children.Clear();
                string key = Model.NeedOpenPassportDetail.PassportKey;
                var treeControl_Only = TreeControl_Only.CreateTree(Model.KeyOntree, "2", Model.VisibleNode, selectionPath);
                treeControl_Only.DataContext = Model;
                TreeHolder.Children.Add(treeControl_Only);
            }

        }

        #region Переход по закладкам - дерево - объекты по типам
        private void Tree_findTabControl_OnSelectionChanged(object sender, RadSelectionChangedEventArgs e)
        {
            if (tree_findTabControl != null)
            {
                if (tree_findTabControl.SelectedItem is RadTabItem)
                {
                    var tab = tree_findTabControl.SelectedItem as RadTabItem;

                    if (tab.Name.Equals("gridTabItem", StringComparison.OrdinalIgnoreCase))
                    {
                        if (gridObjType.Items.Count == 0)
                        {
                            proxy = new ServiceDataClient();
                            IsShowBusyTree = true;
                            Model.Indicator(true);
                            //busyIndicatorTree1.IsBusy = true;
                            proxy.GetDataAllEntityObjCompleted += GetDataAllEntityObjCompleted;
                            proxy.GetDataAllEntityObjAsync(GlobalContext.Context);
                        }

                        btnRefreshTree.Content = GetImageByButton("/Passpot;component/Images/undo_24_d.png");
                        btnVisibleAll.IsEnabled = false;
                        btnRefreshTree.IsEnabled = false;


                    }
                    if (tab.Name.Equals("treeTabItem", StringComparison.OrdinalIgnoreCase))
                    {
                        btnRefreshTree.Content = GetImageByButton("/Passpot;component/Images/undo_24_a.png");
                        btnVisibleAll.IsEnabled = true;
                        btnRefreshTree.IsEnabled = true;

                    }
                }
            }
        }

        void GetDataAllEntityObjCompleted(object sender, GetDataAllEntityObjCompletedEventArgs e)
        {
            proxy.GetDataAllEntityObjCompleted -= GetDataAllEntityObjCompleted;
            IsShowBusyTree = false;
            
            if (e.Result.IsValid)
            {
               
                for (int i = 0; i < e.Result.DataEntityLists.Count; i++)
                {
                    objByType = new ObjByType(e.Result.DataEntityLists[i].VALUEENTITY, e.Result.DataEntityLists[i].KEYENTITY);
                    objByTypes.Add(objByType);
                }

                gridObjType.ItemsSource = objByTypes;
                // gridObjType.ItemsSource = e.Result.DataEntityLists;

                gridObjType.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(OnSelectionChanged), true);

            }
            else
            {
                Model.Report("Список всех объектов при открытии вкладки Таблица объектов err = " + e.Result.ErrorMessage);
            }
            busyIndicatorTree1.IsBusy = false;
            Model.Indicator(false);
        }

        #endregion
        //обрабатывается клик мыши в таблице
        private void OnSelectionChanged(object sender, MouseEventArgs e)
        {
            UIElement clicked = e.OriginalSource as UIElement;
            if (clicked != null)
            {
                var row = clicked.ParentOfType<GridViewRow>();
                if (row != null)
                {
                    var gr_item = ((ObjByType)(((RadRowItem)(row)).Item));
                    dynamic gr_d = gr_item;

                    if (gr_d.GetType().GetProperty("Key") != null)
                    {
                        string keyEntity1 = gr_d.GetType().GetProperty("Key").GetValue(gr_d, null).ToString();

                        Navigation navigation = new Navigation
                           (
                             "NeedOpenPassport",
                             "",
                             0,
                            keyEntity1,
                            "-1",
                             true,
                            true,
                             "1",
                             "",
                             "",
                             ""
                           );
                        Model.sBack.Push(navigation);
                        if (Model.sBack.Count > 1)
                        {
                            FirePropertyChanged("BackTrue");
                        }

                        Model.sForward.Clear();
                        FirePropertyChanged("ForwardFalse");


                        Model.NeedOpenPassport = new PassportOpenParam()
                            {
                                EntityKey = keyEntity1,
                                ParentKey = "-1",
                                IsShowAllField = true,
                                IsShowTreeOnFindkey = true
                            };
                    }
                }

            }


        }


        #region размер контрола
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // passportHolder.Width = Math.Max(230, this.ActualWidth - 10);

            //treeDataScroll.Width = Math.Max(250, this.ActualWidth - 120);
            string t = treeDataScroll.Width.ToString();
            string h = tree_findTabControl.Width.ToString();
            //tree_findTabControl.Height = Math.Max(300, this.ActualHeight-20 );
            tree_findTabControl.Height = Math.Max(500, this.ActualHeight - 16);
            
            tree_findTabControl.Width = Math.Max(250, this.ActualWidth);
            //brdTreeView.Width = Math.Max(250, this.ActualWidth);
            // treeDataScroll.Height = Math.Max(300, this.ActualHeight-75);

            // gridObjType.Height = Math.Max(300, this.ActualHeight - 110);
            // gridObjType.Width = Math.Max(230, this.ActualWidth - 10);

        }
        #endregion

        private Color GetBackColorFromHex(string myColor)
        {
            return Color.FromArgb(
                    Convert.ToByte(myColor.Substring(1, 2), 16),
                    Convert.ToByte(myColor.Substring(3, 2), 16),
                    Convert.ToByte(myColor.Substring(5, 2), 16),
                    Convert.ToByte(myColor.Substring(7, 2), 16)
                );
        }

        //цвет шрифта в гриде
        private void GridObjType_OnRowLoaded(object sender, RowLoadedEventArgs e)
        {
            if ((e.Row != null) && !(e.Row is GridViewHeaderRow))
            {

                GridViewCell cell = e.Row.Cells.Cast<GridViewCell>().FirstOrDefault(c => c.Column.UniqueName == "Name");
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

        #region Селект в таблице - определяем ключ
        private void GridObjType_OnSelectionChanged(object sender, SelectionChangeEventArgs e)
        {

            if (e.AddedItems.Count != 0)
            {
                keyEntity = gridObjType.SelectedItem.GetType().GetProperty("Key").GetValue(gridObjType.SelectedItem, null).ToString();
                // Model.NeedOpenPassport = new PassportOpenParam() { EntityKey = keyEntity, ParentKey = "-1", IsShowAllField = true, IsShowTreeOnFindkey = true };
            }
            e.Handled = false;

        }
        #endregion
        private Image GetImageByButton(string uri)
        {
            Image img = new Image();
            
            BitmapImage btimg = new BitmapImage();
            string _uri = "";
            _uri = uri;
            Uri u = new Uri(_uri, UriKind.Relative);
            btimg.UriSource = u;
            img.Source = btimg;

            return img;
        }

        private void BtnVisibleAll_OnClick(object sender, RoutedEventArgs e)
        {
            string key = "";
            if (Model.VisibleNode == "0")
            {
                Model.VisibleNode = "1";
            }
            else
            {
                Model.VisibleNode = "0";
            }
            
            selectionPath = new ObservableCollection<TreeFullHierarchyPathToObjectItems>();
          
            if (!string.IsNullOrEmpty(Model.KeyOntree))
            {
                //проверяем TreeFullHierarchyPathToObjectItems;

                proxy.GetTreeFullHierarchyPathToObjectCompleted += proxy_GetTreeFullHierarchyPathToObjectCompletedKeyOntree;
                proxy.GetTreeFullHierarchyPathToObjectAsync(Model.KeyOntree, GlobalContext.Context);

                
            }
            else
            {
                RootKey = "3";
                Model.Report("Не определен ключ паспорта для разворачивания дерева");
                var treeControl_Only = TreeControl_Only.CreateTree(Model.KeyOntree, "1", Model.VisibleNode, selectionPath);
                treeControl_Only.DataContext = Model;
            }

        }

        //по ключу объекта разворачиваем дерево
        void proxy_GetTreeFullHierarchyPathToObjectCompletedKeyOntree(object sender, GetTreeFullHierarchyPathToObjectCompletedEventArgs e)
        {

            proxy.GetTreeFullHierarchyPathToObjectCompleted -= proxy_GetTreeFullHierarchyPathToObjectCompletedKeyOntree;
            if (e.Error != null)
            {
                Model.Report("TreeControl.CreateRootOfTreeCompleted = " + e.Error.Message);
                return;
            }

            selectionPath = new ObservableCollection<TreeFullHierarchyPathToObjectItems>();
            selectionPath = e.Result.TreeFullHierarchyPathToObjectList;

            if (selectionPath.Count > 0)
            {
                TreeHolder.Children.Clear();
                var treeControl_Only = TreeControl_Only.CreateTree(Model.KeyOntree, "2", Model.VisibleNode, selectionPath);
                treeControl_Only.DataContext = Model;
                TreeHolder.Children.Add(treeControl_Only);
                
            }

        }

        #region Кнопки перехода по истории вперед-назад
        private void BtnBack_OnClick(object sender, RoutedEventArgs e)
        {

            if (Model.sBack.Count > 0)
            {
                Model.sForward.Push(Model.sBack.Pop());

                if (Model.sBack.Count > 0)
                {
                    btnForward.IsEnabled = true;
                    Navigation n = Model.sBack.Peek();
                    if (n.TypePassport == "NeedOpenPassportDetail")
                    {
                        Model.NeedOpenPassportDetail = new PassportDetailOpenParam()
                        {
                            PassportKey = n.PassportKey,
                            IsEditedPassport = n.IsEditedPassport,
                            EntityKey = n.EntityKey
                        };
                        Model.Path = n.PassportKey;
                        Model.KeyOntree = "";
                        Model.FirePropertyChanged("FindNodeTree");
                    }
                    else
                    {
                        Model.NeedOpenPassport = new PassportOpenParam()
                        {
                            EntityKey = n.EntityKey,
                            ParentKey = n.ParentKey,
                            IsShowAllField = true,
                            IsShowTreeOnFindkey = true
                        };
                        Model.KeyOntree = n.KeyNode;
                        Model.FirePropertyChanged("FindNodeTree");

                    }

                    if (Model.sBack.Count == 1)
                    {
                       
                        btnBack.IsEnabled = false;

                    }
                }
                else
                {
                    btnBack.IsEnabled = false;

                }
            }


        }

        
        private void BtnForward_OnClick(object sender, RoutedEventArgs e)
        {
                if (Model.sForward.Count > 0)
                {
                    btnBack.IsEnabled = true;
                    Navigation n = Model.sForward.Peek();
                    if (n.TypePassport == "NeedOpenPassportDetail")
                    {
                        Model.NeedOpenPassportDetail = new PassportDetailOpenParam()
                        {
                            PassportKey = n.PassportKey,
                            IsEditedPassport = n.IsEditedPassport,
                            EntityKey = n.EntityKey
                        };

                        Model.Path = n.PassportKey;
                        Model.KeyOntree = "";
                        Model.FirePropertyChanged("FindNodeTree");
                    }
                    else
                    {
                        Model.NeedOpenPassport = new PassportOpenParam()
                        {
                            EntityKey = n.EntityKey,
                            ParentKey = n.ParentKey,
                            IsShowAllField = true,
                            IsShowTreeOnFindkey = true
                        };
                        Model.KeyOntree = n.KeyNode;
                        Model.FirePropertyChanged("FindNodeTree");
                    }

                    Model.sBack.Push(Model.sForward.Pop());

                    if (Model.sForward.Count == 0)
                    {
                        btnForward.IsEnabled = false;
                    }

                }

  
        }

        #endregion

        #region Search

        private List<object> SelectedItems = new List<object>();
        private bool shouldSync = false;

        private CustomFilterDescriptor customFilterDescriptor;
        private Business.MyViewModelObjByType _myViewModelObjByType;

        public CustomFilterDescriptor CustomFilterDescriptor
        {
            get
            {
                if (this.customFilterDescriptor == null)
                {
                    this.customFilterDescriptor = new CustomFilterDescriptor(this.gridObjType.Columns.OfType<GridViewDataColumn>());
                    this.gridObjType.FilterDescriptors.Add(this.customFilterDescriptor);
                }
                return this.customFilterDescriptor;
            }
        }

        private void TextBoxFilterValue_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.CustomFilterDescriptor.FilterValue))
            {
                this.SelectedItems.Clear();
                foreach (var item in this.gridObjType.SelectedItems)
                {
                    if (!this.SelectedItems.Contains(item))
                    {
                        this.SelectedItems.Add(item);
                    }
                }
            }

            if (this.textBoxFilterValue.Text == string.Empty)
            {
                this.shouldSync = false;
                foreach (var item in this.SelectedItems)
                {
                    if (!this.gridObjType.SelectedItems.Contains(item))
                    {
                        this.gridObjType.SelectedItems.Add(item);
                    }
                }
                this.SelectedItems.Clear();
            }

            this.CustomFilterDescriptor.FilterValue = this.textBoxFilterValue.Text;

            this.shouldSync = true;

            this.textBoxFilterValue.Focus();
        }
        #endregion

        #region Показывать кнопку копировать в дереве
        void ServiceDataClient_BuCopyVisibleCompleted(object sender, BuCopyVisibleCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                btnCopy.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (e.Result.IsValid)
                {
                    ServiceDataClient.BuCopyVisibleCompleted -= ServiceDataClient_BuCopyVisibleCompleted;
                    if (e.Result.BuCopyTreeVisible_result.BuCopyTreeVisible == "0")
                        btnCopy.Visibility = Visibility.Collapsed;
                    else
                    {
                        btnCopy.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    btnCopy.Visibility = Visibility.Collapsed;
                }
            }
        }

        #endregion

        #region кнопка копировать паспорт
        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            if (_keyPassport_onCopy != "0")
            {
                Model.Report("Копирование паспорта !!!!!+ ключ = " + _keyPassport_onCopy);
                //treeView.LoadOnDemand += new EventHandler<Telerik.Windows.RadRoutedEventArgs>(treeView_LoadOnDemand);
                ServiceDataClient.CopyPassportCompleted += CopyPassportCompleted;
                ServiceDataClient.CopyPassportAsync(_keyPassport_onCopy, GlobalContext.Context);
            }
            else
            {
                Model.Report("Копирование паспорта c нулевым ключем !!!!! + ключ = " + _keyPassport_onCopy);
            }

        }

        void CopyPassportCompleted(object sender, CopyPassportCompletedEventArgs e)
        {
            ServiceDataClient.CopyPassportCompleted -= CopyPassportCompleted;
            if (e.Result.IsValid)
            {
                Model.Report("Копирование успешно!!!!!");

                _lastSelected_2.Items.Clear();

            }
            else
            {
                Model.Report("Копирование  с ошибкой! err = " + e.Result.ErrorMessage);
            }

        }
        #endregion


    }
}

