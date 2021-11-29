using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Media;
using Media.Interfaces;
using Passpot.Business;
using Passpot.Model;
using Services.ServiceReference;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Primitives;

namespace Passpot.Controls
{
    public partial class TreeControl_Only :  UserControl
    {

        private ChildWindowDelete _popupWindowDelete;
        private ServiceDataClient _serviceDataClient;
        private ServiceDataClient proxy;
        private string _rootKey;
        private string _typeTree;
        private RadTreeViewItem _currentItem;

        public event PropertyChangedEventHandler PropertyChanged;
        public IMainModel MainModel { get; private set; }
        public List<ObjOnTree> Source { get; private set; }
        private string keyPassportOnGrid = "";
        private RadTreeViewItem _lastSelected;
        private RadTreeViewItem _lastSelected_2;
        private string[] arr;
        private string _keyPassport_onCopy;
        private string _keyPassport_onDrop;
        private string _keyPassport_onDropParent;
        private string _namePassport_onDrop;
        private string _namePassport_onDropParent;
        private string _namePassport_onDrop1 = "";
        private string _namePassport_onDropParent1 = "";
        private string keyPressed = "";
        private RadTreeViewItem _lastSelectedDropKeyPressed;
        private RadTreeViewItem _lastDragOnDrop;
        string _keyChald = "";
        string _keyParent = "";
        private PlusPair _plusPairCurrentItem;

        private List<PairItem> res = new List<PairItem>();
        private ObservableCollection<TreeFullHierarchyPathToObjectItems> SelectionPath;
       // private ObservableCollection<TreeFullHierarchyPathToObjectItems> _selectionPath = new ();
        private string _itemKey = "";
        private List<string> _pendingSelectionPath = new List<string>();
        private List<string> _pathOnObject = new List<string>();
        private string _countnextNode;
        private bool _selectPassport = true;
        private string combinedKey;
        private string _visibleNode;
        private bool IsFindTreeOnNavigation = false;

        public string CountnextNode
        {
            get { return _countnextNode; }
            set
            {
                _countnextNode = value;

            }
        }

       // private string visibleNode { get; set; }
        //{
        //    get { return _visibleNode; }
        //    set
        //    {
        //        _visibleNode = value;

        //    }
        //}

        private MainModel Model
        {
            get { return DataContext as MainModel; }
        }

        private ServiceDataClient ServiceDataClient
        {
            get
            {
                if (_serviceDataClient == null)
                    _serviceDataClient = new ServiceDataClient();
                return _serviceDataClient;
            }
        }

        //изменение аттрибутов item дерева 
        //(1,2,3 - (3 символа RGB - цвет )
        // 4 -  толщина шрифта (обозначения в базе b - Bold, n - Normal, t-Thin)
        // 5 -  размер шрифта (число - например - 12)
        // 6 -  стиль шрифта (i - Italic, n - Normal) 
        // аттрибуты разделены между собой - #
        // пример -255#0#0#b#12#i#
        // 255#0#0# - цвет красный, b# - толщина шрифта -  толстый, 12# - размер шрифта 12, i# - наклонный
        // по умолчанию берется стандартный набор 
        // 0#0#0# - цвет черный, n# - толщина шрифта -  нормальный, 12# - размер шрифта 12, n# - нормальный
        // 0#0#0#n#12#n#

        public static TreeControl_Only CreateTree(string rootKey, string typeTree, string visibleNode, ObservableCollection<TreeFullHierarchyPathToObjectItems> selectionPath)
        {
            var c = new TreeControl_Only();

            c.SelectionPath = selectionPath;
            c._rootKey = rootKey;
            c._typeTree = typeTree;
            c.proxy = new ServiceDataClient();
            
            
            //строим дерево с путем до объекта typeTree == "2"
            if (typeTree == "2")
            {

                //?key=63914303
                //новое!
                //строим первую верхнюю веточку дерева,
                c.proxy.CreateRootOfTreeCompleted += c.CreateRootOfTreeCompleted;
                c.proxy.CreateRootOfTreeAsync("3", GlobalContext.Context, visibleNode);

            }
            //строим обыкновеное дерево typeTree == "1" и rootKey = 3
            else
            {
                if (c.treeView.Items.Count == 0)
                {
                    c.proxy.CreateRootOfTreeCompleted += c.CreateRootOfTreeCompleted;
                    c.proxy.CreateRootOfTreeAsync(rootKey, GlobalContext.Context, visibleNode);
                }
            }
            return c;
        }

        #region Создание первой ветки дерева

        void CreateRootOfTreeCompleted(object sender, CreateRootOfTreeCompletedEventArgs e)
        {
            proxy.CreateRootOfTreeCompleted -= CreateRootOfTreeCompleted;

            if (!e.Result.IsValid)
            {
                Model.Report(" TreeControl.CreateRootOfTreeCompleted = " + e.Result.ErrorMessage);
                return;
            }

            Model.Report(" TreeControl.CreateRootOfTreeCompleted = " + e.Result.ErrorMessage);
            //busyIndicatorTree1.IsBusy = true;
            Model.Indicator(true);

            res = new List<PairItem>(e.Result.PairItemList);
            
            var template = this.Resources["PairTemplate"] as Telerik.Windows.Controls.HierarchicalDataTemplate;
            treeView.ItemTemplate = template;
            
            

            //treeView.ItemContainerStyle = null;

           
            List<PlusPair> list = new List<PlusPair>();

            if (res != null)
            {

                //очищаем дерево!!!!!!!!!????
                if (treeView.ItemsSource != null)
                    treeView.ItemsSource = null;

                treeView.Items.Clear();

                FontWeight fw = FontWeights.Normal;
                int fontSize = 12;
                FontStyle fs = FontStyles.Normal;
                SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(255, Byte.Parse("0"), Byte.Parse("0"), Byte.Parse("0")));
                

                for (int i = 0; i < res.Count; i++)
                {

                    string[] arr = res[i].Texts.Split('#');

                    if (arr.Length == 7)
                    {

                        brush = GetSolidColorBrush(arr);
                        fw = GetFontWeights(arr[3]);
                        fontSize = int.Parse(arr[4]);
                        fs = GetFontStyle(arr[5]);
                        
                    }

                    string[] arrName = res[i].Texts.Split('~');
                    if (arr.Length > 1)
                    {
                        var pp = new PlusPair
                        {
                            Texts = (arrName.Length > 1 ? arrName[1] : res[i].Texts),
                            Key = res[i].Key,
                            Brush = brush,
                            Weight = fw,
                            Style = fs,
                            Size = fontSize,
                            Visibility = Visibility.Visible,
                            Path = "0-" + i.ToString()+"|"
                        };
                        list.Add(pp);
                    }
                    else
                    {
                        var pp = new PlusPair
                        {
                            Texts = (arrName.Length > 1 ? arrName[1] : res[i].Texts),
                            Key = res[i].Key,
                            Brush = brush,
                            Weight = fw,
                            Style = fs,
                            Size = fontSize,
                            Visibility = Visibility.Visible,
                            Path = "0-" + i.ToString() + "|"
                        };
                        list.Add(pp);
                    }
                }
            }

            treeView.IsExpandOnSingleClickEnabled = true;
            //busyIndicatorTree1.IsBusy = false;
        Model.Indicator(false);
            treeView.ItemsSource = list;




          

            //добавляем обработчик клика мыши по ветки дерева
           // treeView.AddHandler(RadTreeViewItem.MouseLeftButtonDownEvent, new MouseButtonEventHandler(treeView_MouseLeftButtonDown), true);

            // если с путем до объекта то вызываем функцию, которая возвращает путь до объекта
            if (_typeTree == "2")
            {
                IsFindTreeOnNavigation = false; //используем поиск по ключу

                GetTreeFullHierarchyPathToObject(SelectionPath);
                //proxy.GetTreeFullHierarchyPathToObjectCompleted += proxy_GetTreeFullHierarchyPathToObjectCompleted;
                //proxy.GetTreeFullHierarchyPathToObjectAsync(_rootKey, GlobalContext.Context);
            }


        }
        #endregion Создание первой ветки дерева

        //клик мыши по ветки дерева
        private void treeView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (arr != null)
            {

                if (_selectPassport)
                {
                    if (arr.Length >= 4)
                    {
                        if (!string.IsNullOrEmpty(arr[3]))
                        {
                            string ttt = arr[1];

                            _keyPassport_onCopy = arr[1];
                            _keyPassport_onDrop = arr[1];
                            _keyPassport_onDropParent = arr[1];
                            _namePassport_onDrop = ((PairItem)treeView.SelectedItem).Texts;
                            _namePassport_onDropParent = ((PairItem)treeView.SelectedItem).Texts;


                            //проверка на доступ к паспорту
                            //ServiceDataClient.ReturnAccessObjCompleted += ReturnAccessObjOnTreeCompleted;
                            //ServiceDataClient.ReturnAccessObjAsync(arr[1]);

                           

                            Model.NeedOpenPassportDetail = new PassportDetailOpenParam()
                                {
                                    PassportKey = arr[1],
                                    IsEditedPassport = 0,
                                    EntityKey = arr[2]
                                };
                            //закоментировано!!! - ключ для внешней программы - внешнего javascript
                            //НЕ АКТУАЛЬНО!!!!!!!!!!!!!
                            // HtmlPage.Window.Invoke("getObjKey", new string[] {arr[1]});

                        }
                        else
                        {
                           
                            Model.NeedOpenPassport = new PassportOpenParam()
                                {
                                    EntityKey = arr[2],
                                    ParentKey = arr[1],
                                    IsShowAllField = true,
                                    IsShowTreeOnFindkey = true
                                };
                        }
                    }
                    else
                    {
                        Model.NeedOpenPassport = new PassportOpenParam()
                            {
                                EntityKey = arr[2],
                                IsShowAllField = true,
                                IsShowTreeOnFindkey = false
                            };

                        e.Handled = false;
                    }
                }
            }
        }

        //если нужно дерево с путем до объекта - то возвращает списое ключей до нужного уровня
        private void GetTreeFullHierarchyPathToObject(ObservableCollection<TreeFullHierarchyPathToObjectItems> selectionPath)
        {
            if (selectionPath.Count > 0)
            {
                _pendingSelectionPath = new List<string>();

                for (int i = 0; i < selectionPath.Count; i++)
                {
                    _pendingSelectionPath.Add(selectionPath[i].ObjTypeKey);
                    _pendingSelectionPath.Add(selectionPath[i].ObjKey);
                }

                List<PlusPair> list = new List<PlusPair>();

                foreach (var item in treeView.Items)
                {
                    list.Add(item as PlusPair);
                }

                CountnextNode = "0";
                var itemsControl = treeView as System.Windows.Controls.ItemsControl;

                if (IsFindTreeOnNavigation)
                {
                    FindpathOnObject(itemsControl, list);
                }
                else
                {
                    ExpandToPendingSelection(itemsControl, list);
                }
            }
            else
            {
                Model.Report("GetTreeFullHierarchyPathToObject = " + "нет данных");
            }
        }

       

        private void ExpandToPendingSelection(System.Windows.Controls.ItemsControl itemsControl, List<PlusPair> list)
        {
            if (_pendingSelectionPath.Count < 1)
            {
                _rootKey = "3";
                _selectPassport = true;
                return;
            }

            for (int i = 0; i < list.Count; i++)
            {

                //обрабатываем ключ list[i].Key т.к. list[i].Key = 100z0z25005 или list[i].Key =101z64064903z25005
                string keyList = ExtractKey(list[i].Key);
                if (keyList == "")
                {
                    Model.Report("TreeControl_Only (должно быть list[i].Key = 1000~25005 или list[i].Key =101~64064903~25005 )   не верные данные ключа = " + list[i].Key);
                    break;
                }

                Debug.WriteLine("+++ ");
                foreach (string s in _pendingSelectionPath)
                {
                    Debug.WriteLine("psp = " + s);
                    //Model.Report("psp = " + s);
                }
                //Model.Report("--- ");
                Debug.WriteLine("--- ");

                Debug.WriteLine("Будем искать " + keyList);
                //Model.Report("Будем искать " + keyList);

                if (_pendingSelectionPath.Contains(keyList))
                {
                    Debug.WriteLine("найден = " + keyList);
                    //Model.Report("найден = " + keyList);
                    int index = _pendingSelectionPath.IndexOf(keyList);

                    _pendingSelectionPath.RemoveAt(index);
                    BringIndexIntoView(itemsControl, i);
                    itemsControl.UpdateLayout();

                    var container = itemsControl.ItemContainerGenerator.ContainerFromIndex(i) as RadTreeViewItem;

                    //Debug.Assert(container!=null);

                    if (container != null)
                    {
                        _itemKey = list[i].Key;
                        //попадаем в LoadOnDemand
                        container.IsExpanded = true;
                        container.UpdateLayout();

                        if (_pendingSelectionPath.Count == 0)
                        {
                            _selectPassport = false;
                            treeView.SelectedItem = container.DataContext as PlusPair;
                            //container.Background = new SolidColorBrush(Colors.Yellow);
                            container.Focus();
                            container.UpdateLayout();
                            _rootKey = "3";
                            _selectPassport = true;


                        }

                        // treeView.SelectedItem = container.DataContext as PlusPair; 

                    }
                    return;
                }
                else
                {
                    Debug.WriteLine("не найден " + keyList);
                    //Model.Report("не найден " + keyList);
                }
            }
        }

        private void BringIndexIntoView(System.Windows.Controls.ItemsControl itemsControl, int index)
        {
            var treeView = itemsControl as RadTreeView;
            if (treeView != null)
            {
                treeView.BringIndexIntoView(index);
            }
            var treeViewItem = itemsControl as RadTreeViewItem;
            if (treeViewItem != null)
            {
                treeViewItem.BringIndexIntoView(index);
            }
        }

        #region LoadOnDemand - по клику на ветку раскрывается следующий уровень дерева

        void treeView_LoadOnDemand(object sender, RadRoutedEventArgs e)
        {

            string keyPassport = "";
            if (Model.NeedOpenPassportDetail !=null)
            {
                keyPassport = Model.NeedOpenPassportDetail.PassportKey;
            }


            Debug.WriteLine("treeView_LoadOnDemand. _itemKey = " + _itemKey);
            //Model.Report("treeView_LoadOnDemand. _itemKey = " + _itemKey);

            proxy = new ServiceDataClient();

            _currentItem = e.OriginalSource as RadTreeViewItem;
            _plusPairCurrentItem = _currentItem.DataContext as PlusPair;

            Model.Indicator(true);
            //busyIndicatorTree1.IsBusy = true;

            if (_rootKey == "3")
            {
                PlusPair c = _currentItem.Item as PlusPair;



                string typekey = GetTypekey(c.Key);
                if (typekey == "100")
                {
                    if (_currentItem.ParentItem != null)
                    {
                        PlusPair c2 = _currentItem.ParentItem.Item as PlusPair;
                        Model.KeyOntree = Getkey(c2.Key);
                    }
                }
                else
                {
                    Model.KeyOntree = Getkey(c.Key);
                }

                {
                   

                    proxy.GetNextNodeCompleted += GetNextNodeCompleted;
                    proxy.GetNextNodeAsync(c.Key, _rootKey, GlobalContext.Context, Model.VisibleNode, keyPassport);
                    Debug.WriteLine("treeView_LoadOnDemand c.Key = " + c.Key);
                    //Model.Report("treeView_LoadOnDemand c.Key = " + c.Key);
                    _currentItem.IsLoadingOnDemand = false;
                }
            }
            else
            {


                proxy.GetNextNodeCompleted += GetNextNodeCompleted;
                if (Model.NeedOpenPassportDetail != null)
                {
                    proxy.GetNextNodeAsync(_itemKey, "3", GlobalContext.Context, Model.VisibleNode, Model.NeedOpenPassportDetail.PassportKey);   
                    
                }
                else
                {
                    proxy.GetNextNodeAsync(_itemKey, "3", GlobalContext.Context, Model.VisibleNode, ""); 
                }
                
            }
        }

        void GetNextNodeCompleted(object sender, GetNextNodeCompletedEventArgs e)
        {
            proxy.GetNextNodeCompleted -= GetNextNodeCompleted;
            
            if (!e.Result.IsValid)
            {
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                //code.SessionEnd.Redirect();
               // Debug.WriteLine("GetNextNodeCompleted. ERROR!!!");
                Model.Report("GetNextNodeCompleted. ERROR!!! = "+ e.Result.ErrorMessage);
                //busyIndicatorTree1.IsBusy = false;
                Model.Indicator(false);
                return;
            }

            Model.Report("GetNextNodeCompleted = " + e.Result.ErrorMessage);

            Debug.WriteLine("GetNextNodeCompleted. _itemKey = " + _plusPairCurrentItem.Key);
            //Model.Report("GetNextNodeCompleted. _itemKey = " + _plusPairCurrentItem.Key);
           // Model.Report("GetNextNodeCompleted. e.Result.Count = " + e.Result.Count);
            Debug.WriteLine("GetNextNodeCompleted. e.Result.Count = " + e.Result.PairItemList.Count);

            //изменение аттрибутов item дерева 
            //(1,2,3 - (3 символа RGB - цвет )
            // 4 -  толщина шрифта (обозначения в базе b - Bold, n - Normal, t-Thin)
            // 5 -  размер шрифта (число - например - 12)
            // 6 -  стиль шрифта (i - Italic, n - Normal) 
            // аттрибуты разделены между собой - #
            // пример -255#0#0#b#12#i#
            // 255#0#0# - цвет красный, b# - толщина шрифта -  толстый, 12# - размер шрифта 12, i# - наклонный
            // по умолчанию берется стандартный набор 
            // 0#0#0# - цвет черный, n# - толщина шрифта -  нормальный, 12# - размер шрифта 12, n# - нормальный
            // 0#0#0#n#12#n#
            SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(255, Byte.Parse("0"), Byte.Parse("0"), Byte.Parse("0")));
            FontWeight fw = FontWeights.Normal;
            int fontSize = 12;
            FontStyle fs = FontStyles.Normal;

            var res = e.Result;

            if (res.PairItemList.Count > 10000)
            {
                //busyIndicatorTree1.IsBusy = false;
                Model.Indicator(false);
                return;
            }

            if (res != null)
            {
                if (_currentItem.Items.Count > 0)
                {
                    //_currentItem.Items.Clear();
                    _plusPairCurrentItem = _currentItem.DataContext as PlusPair;
                    _plusPairCurrentItem.Children.Clear();
                }

                int level = _currentItem.Level + 1;

                for (int i = 0; i < res.PairItemList.Count; i++)
                {
                    string[] arr = res.PairItemList[i].Texts.Split('#');
                    if (arr.Length == 7)
                    {

                        brush = GetSolidColorBrush(arr);
                        fw = GetFontWeights(arr[3]);
                        fontSize = int.Parse(arr[4]);
                        fs = GetFontStyle(arr[5]);

                    }

                    string[] arrName = res.PairItemList[i].Texts.Split('~');
                    if (arrName.Length > 1)
                    {

                        PlusPair child = new PlusPair
                        {

                            Texts =
                                (arrName.Length > 1 ? arrName[1] : res.PairItemList[i].Texts),
                            Key = res.PairItemList[i].Key,
                            Brush = brush,
                            Weight = fw,
                            Style = fs,
                            Size = fontSize,
                            Visibility = Visibility.Collapsed,
                            Path = (_plusPairCurrentItem.Path + level + "-" +i.ToString()+"|")


                        };
                        _plusPairCurrentItem.Children.Add(child);
                        

                    }
                    else
                    {
                        if (_rootKey == "3")
                        {
                            PlusPair child = new PlusPair
                            {
                                Texts =
                                    (arrName.Length > 1 ? arrName[1] : res.PairItemList[i].Texts),
                                Key = res.PairItemList[i].Key,
                                Brush = brush,
                                Weight = fw,
                                Style = fs,
                                Size = fontSize,
                                Visibility = Visibility.Collapsed

                            };
                            
                            _plusPairCurrentItem.Children.Add(child);
                        }
                    }
                }
                if (res.PairItemList.Count == 0)
                {
                    _currentItem.IsLoadOnDemandEnabled = false;
                    _currentItem.IsExpanded = false;

                }
                else
                {
                   

                    _currentItem.IsLoadOnDemandEnabled = true;
                    _currentItem.IsExpanded = true;
                    _currentItem.UpdateLayout();
                  

                }
                CountnextNode = res.PairItemList.Count.ToString();

                Debug.WriteLine("GetNextNodeCompleted " + _plusPairCurrentItem.Key + " Children: " + _plusPairCurrentItem.Children.Count);

                if (_rootKey != "3")
                {
                    foreach (PlusPair pair in _plusPairCurrentItem.Children)
                    {
                        Debug.WriteLine("Child: " + pair.Key);
                    }
                    ExpandToPendingSelection(_currentItem, _plusPairCurrentItem.Children.ToList());
                }
            }
            //busyIndicatorTree1.IsBusy = false;
            if (Model != null)
            {
                Model.Indicator(false);
            }

        }

        public string GetTypekey(string typekey)
        {
            string stypekey = "";

            double ii = typekey.IndexOf("~");
            string[] ArrayKeys = typekey.Split('~');
            if (ii != -1)
            {
                stypekey = ArrayKeys[0];
              
            }

           
            else
            {
                Model.Report("Ошибка TreeControlOnly typekey " + typekey);
                Debugger.Break();
                
            }

            return stypekey;

        }
        public string Getkey(string key)
        {
            string skey = "";

            double ii = key.IndexOf("~");
            string[] ArrayKeys = key.Split('~');
            if (ii != -1)
            {
                skey = ArrayKeys[1];

            }

            else
            {
                Model.Report("Ошибка TreeControlOnly key "+ key);
                Debugger.Break();
            }
            return skey;

        }




        #endregion

        #region обработка клика по ветке дерева - отображаем или списочную форму или паспорт
        private void RadTreeView_Selected(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            _lastSelected = e.OriginalSource as RadTreeViewItem;

            _lastSelected_2 = _lastSelected.ParentItem;



        }
        #endregion

        #region перенос по дереву - "CtrlX(взять объект) - CtrlV(вставить объект)
        private void treeView_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {


            if ((keyPressed + e.Key.ToString()) == "CtrlX")
            {
                _namePassport_onDrop1 = _namePassport_onDrop;
                //MessageBox.Show("The pressed key is: " + keyPressed + e.Key.ToString());
                keyPressed = "";
                _lastSelectedDropKeyPressed = _lastSelected.ParentItem;
                _keyChald = _keyPassport_onDrop;
            }
            else
                if (((keyPressed + e.Key.ToString()) == "CtrlV") && (_keyChald != ""))
                {

                    //MessageBox.Show("The pressed key is: " + keyPressed + e.Key.ToString());
                    keyPressed = "";
                    _keyParent = _keyPassport_onDropParent;
                    _namePassport_onDropParent1 = _namePassport_onDropParent;

                    _popupWindowDelete.Title = "Подтверждение переноса";
                    _popupWindowDelete.titleBox.Text = "Перенести объект: " + _namePassport_onDrop1 + "  на объект: " + _namePassport_onDropParent1 + "? ";
                    _popupWindowDelete.Show();
                    _popupWindowDelete.OKButtonDelete.Click += OKButtonDrop;
                }
                else
                {
                    keyPressed = e.Key.ToString();
                }

        }

        void OKButtonDrop(object sender, RoutedEventArgs e)
        {
            _popupWindowDelete.OKButtonDelete.Click -= OKButtonDelete;

            if (_popupWindowDelete.DialogResult == true)
            {
                proxy = new ServiceDataClient();
                proxy.UpdateParentCompleted += proxy_DropCompleted;
                proxy.UpdateParentAsync(_keyChald, _keyParent, GlobalContext.Context);
            }
        }

        void proxy_DropCompleted(object sender, UpdateParentCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                //code.SessionEnd.Redirect();
            }
            else
            {

                if (e.Result.IsValid)
                {
                    string res_drop = e.Result.KeyParent_OnUpdateTree_result.KeyParent_OnUpdateTree.ToString();
                    if (res_drop == "-1")
                    {
                        Model.Report("Нельзя перенести");
                    }
                    else
                    {

                        if (_lastSelected != null)
                        {
                            _lastSelected.Items.Clear();
                        }
                        if (_lastSelectedDropKeyPressed != null)
                        {
                            _lastSelectedDropKeyPressed.Items.Clear();
                        }

                    }

                }
                else
                {
                    Model.Report("Перенос по дереву err = " + e.Result.ErrorMessage);
                }
            }
        }

        void OKButtonDelete(object sender, RoutedEventArgs e)
        {
            _popupWindowDelete.OKButtonDelete.Click -= OKButtonDelete;

            if (_popupWindowDelete.DialogResult == true)
            {
                //передаем ключ парента
                //PassportModel.DeletePassport("123");

            }
        }


        #endregion
        public TreeControl_Only()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Model != null)
            {
                Model.PropertyChanged -= Model_PropertyChanged;
                Model.PropertyChanged += Model_PropertyChanged;    
            }
            

        }

        void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RadTreeViewItem _parentItem;

            if ("EditPassportSaved".Equals(e.PropertyName))
            {
                //при создании нового паспорта из таблицы и удалении перегружаем ветку дерева на которой стоит курсор
                if (_lastSelected != null)
                {
                    _currentItem = _lastSelected;
                    _plusPairCurrentItem = _lastSelected.DataContext as PlusPair;

                    if (_rootKey == "3")
                    {
                        PlusPair c = _lastSelected.Item as PlusPair;
                        if (c != null)
                        {
                            proxy.GetNextNodeCompleted += GetNextNodeCompleted;
                            proxy.GetNextNodeAsync(c.Key, _rootKey, GlobalContext.Context, Model.VisibleNode, Model.NeedOpenPassportDetail.PassportKey);
                            _currentItem.IsLoadingOnDemand = false;
                        }
                    }
                    else
                    {
                        proxy.GetNextNodeCompleted += GetNextNodeCompleted;
                        proxy.GetNextNodeAsync(_itemKey, "3", GlobalContext.Context, Model.VisibleNode, Model.NeedOpenPassportDetail.PassportKey);
                    }
                }



            }

   
            if ("EditPassportReloadTreeNode".Equals(e.PropertyName))
            {
                //при редактировании паспорта перезаписываем название паспорта в ветке
                if (_lastSelected != null)
                {

                    _currentItem = _lastSelected;
                    _parentItem = _currentItem.ParentItem;

                    PlusPair _plusPairparentItem;

                    if (_parentItem != null)
                    {

                        _plusPairparentItem = _parentItem.Item as PlusPair;
                        proxy.GetNextNodeCompleted += GetNextNodeReloadNodeCompleted;
                        proxy.GetNextNodeAsync(_plusPairparentItem.Key, _rootKey, GlobalContext.Context, Model.VisibleNode, Model.NeedOpenPassportDetail.PassportKey);
                        _currentItem.IsLoadingOnDemand = false;
                    }
                    else
                    {
                        _plusPairparentItem = _currentItem.Item as PlusPair;
                    }

                }
            }

            if ("FindTreeOnkeyPassportIsParent".Equals(e.PropertyName))
            {
                //перегружаем ветку дерева и позицианируемся на итаме
                if (_lastSelected != null)
                {
                    _currentItem = _lastSelected;
                    _plusPairCurrentItem = _lastSelected.DataContext as PlusPair;

                    if (_rootKey == "3")
                    {
                        PlusPair c = _lastSelected.Item as PlusPair;
                        if (c != null)
                        {
                            proxy.GetNextNodeCompleted += GetNextNodeReloadNodeIsParentCompleted;
                            proxy.GetNextNodeAsync(c.Key, _rootKey, GlobalContext.Context, Model.VisibleNode, Model.NeedOpenPassportDetail.PassportKey);
                            _currentItem.IsLoadingOnDemand = false;
                        }
                    }
                    else
                    {
                        proxy.GetNextNodeCompleted += GetNextNodeCompleted;
                        proxy.GetNextNodeAsync(_itemKey, "3", GlobalContext.Context, Model.VisibleNode, Model.NeedOpenPassportDetail.PassportKey);
                    }
                }
            }

            if ("FindNodeTree".Equals(e.PropertyName))
            {
                IsFindTreeOnNavigation = true; //признак , что используем кнопку навигации(вперед-назад)

                proxy.GetTreeFullHierarchyPathToObjectCompleted += proxy_GetTreeFullHierarchyPathToObjectCompleted;

                if (!string.IsNullOrEmpty(Model.KeyOntree))
                {
                    proxy.GetTreeFullHierarchyPathToObjectAsync(Model.KeyOntree, GlobalContext.Context); 
                }
                else
                {
                    proxy.GetTreeFullHierarchyPathToObjectAsync(Model.Path, GlobalContext.Context);    
                }
                

             
               // treeView.GetItemByPath("0.1|1.0|2.0|3.0|4.0|5.1")
            }
            if ("RefreshTree".Equals(e.PropertyName))
            {
               
                treeView.ItemsSource = null;
                proxy.CreateRootOfTreeCompleted += CreateRootOfTreeCompleted;
                proxy.CreateRootOfTreeAsync("3", GlobalContext.Context, Model.VisibleNode);



            }

        }
        //для кнопки навигации
        void proxy_GetTreeFullHierarchyPathToObjectCompleted(object sender, GetTreeFullHierarchyPathToObjectCompletedEventArgs e)
        {
            proxy.GetTreeFullHierarchyPathToObjectCompleted -= proxy_GetTreeFullHierarchyPathToObjectCompleted;

            if (e.Error != null)
            {
                Model.Report("TreeControl.CreateRootOfTreeCompleted = " + e.Error.Message);
                return;
            }

            SelectionPath = new ObservableCollection<TreeFullHierarchyPathToObjectItems>();
            SelectionPath = e.Result.TreeFullHierarchyPathToObjectList;

            //список ключей объектов и ключей паспорта до нужного уровня
            _pendingSelectionPath = new List<string>();

            for (int i = 0; i < SelectionPath.Count; i++)
            {
                _pendingSelectionPath.Add(SelectionPath[i].ObjTypeKey);
                _pendingSelectionPath.Add(SelectionPath[i].ObjKey);
            }

            List<PlusPair> list = new List<PlusPair>();

            foreach (var item in treeView.Items)
            {
                list.Add(item as PlusPair);
            }

            CountnextNode = "0";
            var itemsControl = treeView as System.Windows.Controls.ItemsControl;

            if (IsFindTreeOnNavigation)
            {
                FindpathOnObject(itemsControl, list);
            }
            else
            {
                ExpandToPendingSelection(itemsControl, list);    
            }

        }


        private void FindpathOnObject(System.Windows.Controls.ItemsControl itemsControl, List<PlusPair> list)
        {
            if (_pendingSelectionPath.Count < 1)
            {
                _rootKey = "3";
                _selectPassport = true;
                return;
            }

            List<PlusPair> listNew = new List<PlusPair>();

            for (int i = 0; i < list.Count; i++)
            {


                //---------------------------------------------

                //обрабатываем ключ list[i].Key т.к. list[i].Key = 100z0z25005 или list[i].Key =101z64064903z25005
                string keyList = ExtractKey(list[i].Key);
                if (keyList == "")
                {
                    Model.Report("(должно быть list[i].Key = 100z0z25005 или list[i].Key =101z64064903z25005 ) TreeControl_Only  не верные данные ключа = " + list[i].Key);
                    break;
                }


                Debug.WriteLine("Будем искать " + keyList);
                //Model.Report("Будем искать " + keyList);

                if (_pendingSelectionPath.Contains(keyList))
                {
                    Debug.WriteLine("найден = " + keyList);
                    //Model.Report("найден = " + keyList);
                    int index = _pendingSelectionPath.IndexOf(keyList);

                    //_pendingSelectionPath.RemoveAt(index);

                     

                    foreach (var item in ((PlusPair)(itemsControl.Items[i])).Children)
                    {
                        listNew.Add(item as PlusPair);
                    }

                    if (listNew.Count == 0)
                    {
                        _rootKey = "2";
                        ExpandToPendingSelection(itemsControl, list);  
                        return;
                    }
                    else
                    {
                        list.Clear();
                        list = listNew;
                    }

                    _pendingSelectionPath.RemoveAt(index);

                    var container = itemsControl.ItemContainerGenerator.ContainerFromIndex(i) as RadTreeViewItem;
                    if (container != null)
                    {
                        if ((_pendingSelectionPath.Count == 0))
                        {
                            treeView.SelectedItem = container.DataContext as PlusPair;
                            container.Focus();
                        }
                        else
                        {
                            var itemsControl2 = container as System.Windows.Controls.ItemsControl;
                            if (itemsControl2 == null)
                            {

                                return;
                            }
                            else
                            {
                                //рекурсия
                                FindpathOnObject(itemsControl2, list);
                            }
                        }
                    }






                    //BringIndexIntoView(itemsControl, i);
                    //itemsControl.UpdateLayout();

                    //var container = itemsControl.ItemContainerGenerator.ContainerFromIndex(i) as RadTreeViewItem;

                    ////Debug.Assert(container!=null);

                    //if (container != null)
                    //{
                    //    _itemKey = list[i].Key;
                    //    //попадаем в LoadOnDemand
                    //    container.IsExpanded = true;
                    //    container.UpdateLayout();

                    //    if (_pendingSelectionPath.Count == 0)
                    //    {
                    //        _selectPassport = false;
                    //        treeView.SelectedItem = container.DataContext as PlusPair;
                    //        //container.Background = new SolidColorBrush(Colors.Yellow);
                    //        container.Focus();
                    //        container.UpdateLayout();
                    //        _rootKey = "3";
                    //        _selectPassport = true;


                    //    }






                //----------------------------------














                //обрабатываем ключ list[i].Key т.к. list[i].Key = 100z0z25005 или list[i].Key =101z64064903z25005
               // string pathList = list[i].Path;

               // Debug.WriteLine("Будем искать " + pathList);

               // if (_pendingSelectionPath.Contains(pathList))
               // {
                    //Debug.WriteLine("найден = " + pathList);
                    //int index = _pendingSelectionPath.IndexOf(pathList);

                    //_pendingSelectionPath.RemoveAt(index);

                    //list.Clear();

                    //foreach (var item in ((PlusPair)(itemsControl.Items[i])).Children)
                    //{
                    //    list.Add(item as PlusPair);
                    //}

                   
                    //var container = itemsControl.ItemContainerGenerator.ContainerFromIndex(i) as RadTreeViewItem;
                    //if (container != null)
                    //{
                    //    if ((_pathOnObject.Count == 0))
                    //    {
                    //        treeView.SelectedItem = container.DataContext as PlusPair;
                    //        container.Focus();
                    //    }
                    //    else
                    //    {
                    //        var itemsControl2 = container as System.Windows.Controls.ItemsControl;
                    //        //рекурсия
                    //        FindpathOnObject(itemsControl2, list);
                    //    }
                    //}
                    return;
                }
                else
                {
                    //Debug.WriteLine("не найден " + pathList);
                    //Model.Report("не найден " + keyList);
                }
            }
            if (_pendingSelectionPath.Count > 0)
            {
               // Model.Report("путь не найден в дереве - дерево раскрываем заново " + keyList);
                ExpandToPendingSelection(itemsControl, list);    
            }

        }

        //при клике в таблице(таблица выбрана из дерева) переходим на ноду в дереве
        private void GetNextNodeReloadNodeIsParentCompleted(object sender, GetNextNodeCompletedEventArgs e)
        {
            proxy.GetNextNodeCompleted -= GetNextNodeCompleted;

            if (!e.Result.IsValid)
            {
                Model.Report("GetNextNodeCompleted. ERROR!!! = " + e.Result.ErrorMessage);
                Model.Indicator(false);
                return;
            }

            SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(255, Byte.Parse("0"), Byte.Parse("0"), Byte.Parse("0")));
            FontWeight fw = FontWeights.Normal;
            int fontSize = 12;
            FontStyle fs = FontStyles.Normal;

            var res = e.Result;

            if (res.PairItemList.Count > 10000)
            {


                _pendingSelectionPath = new List<string>();
                _pendingSelectionPath.Add(Model.NeedOpenPassportDetail.PassportKey);

                CountnextNode = "0";

                ExpandToPendingSelection(_currentItem, _plusPairCurrentItem.Children.ToList());


                //Model.Indicator(false);
                return;
            }

            if (res != null)
            {
                if (_currentItem.Items.Count > 0)
                {
                    _plusPairCurrentItem = _currentItem.DataContext as PlusPair;
                    _plusPairCurrentItem.Children.Clear();
                }

                int level = _currentItem.Level + 1;
                for (int i = 0; i < res.PairItemList.Count; i++)
                {
                    string[] arr = res.PairItemList[i].Texts.Split('#');
                    if (arr.Length == 7)
                    {
                        brush = GetSolidColorBrush(arr);
                        fw = GetFontWeights(arr[3]);
                        fontSize = int.Parse(arr[4]);
                        fs = GetFontStyle(arr[5]);
                    }

                    string[] arrName = res.PairItemList[i].Texts.Split('~');
                    if (arrName.Length > 1)
                    {
                        PlusPair child = new PlusPair
                            {
                                Texts =
                                    (arrName.Length > 1 ? arrName[1] : res.PairItemList[i].Texts),
                                Key = res.PairItemList[i].Key,
                                Brush = brush,
                                Weight = fw,
                                Style = fs,
                                Size = fontSize,
                                Visibility = Visibility.Collapsed,
                                Path = (_plusPairCurrentItem.Path + level + "-" + i.ToString() + "|")
                            };
                        _plusPairCurrentItem.Children.Add(child);

                    }
                    else
                    {
                        if (_rootKey == "3")
                        {
                            PlusPair child = new PlusPair
                                {
                                    Texts =
                                        (arrName.Length > 1 ? arrName[1] : res.PairItemList[i].Texts),
                                    Key = res.PairItemList[i].Key,
                                    Brush = brush,
                                    Weight = fw,
                                    Style = fs,
                                    Size = fontSize,
                                    Visibility = Visibility.Collapsed

                                };

                            _plusPairCurrentItem.Children.Add(child);
                        }
                    }
                }
                if (res.PairItemList.Count == 0)
                {
                    _currentItem.IsLoadOnDemandEnabled = false;
                    _currentItem.IsExpanded = false;

                }
                else
                {
                    _currentItem.IsLoadOnDemandEnabled = true;
                    _currentItem.IsExpanded = true;
                    _currentItem.UpdateLayout();
                }
                CountnextNode = res.PairItemList.Count.ToString();

                Debug.WriteLine("GetNextNodeCompleted " + _plusPairCurrentItem.Key + " Children: " +
                                _plusPairCurrentItem.Children.Count);

                _pendingSelectionPath = new List<string>();
                _pendingSelectionPath.Add(Model.NeedOpenPassportDetail.PassportKey);

                CountnextNode = "0";

                ExpandToPendingSelection(_currentItem, _plusPairCurrentItem.Children.ToList());
            }
        }
           

        //при редактировании паспорта перезаписываем название паспорта в ветке
        private void GetNextNodeReloadNodeCompleted(object sender, GetNextNodeCompletedEventArgs e)
        {
            proxy.GetNextNodeCompleted -= GetNextNodeReloadNodeCompleted;
            var res = e.Result;
            string _newText;
            string _oldKey;

            PlusPair c = _currentItem.Item as PlusPair;

            if (res.PairItemList.Count > 10000)
            {
                return;
            }

            if (res != null)
            {

                for (int i = 0; i < res.PairItemList.Count; i++)
                {

                    string[] arrName = res.PairItemList[i].Texts.Split('~');
                    if (arrName.Length > 1)
                    {
                        _newText = (arrName.Length > 1 ? arrName[1] : res.PairItemList[i].Texts);
                        _oldKey = res.PairItemList[i].Key;
                        string[] arr = res.PairItemList[i].Texts.Split('#');

                        if (c.Key.Equals(_oldKey))
                        {
                            c.Texts = _newText;
                            c.Brush = GetSolidColorBrush(arr);
                            c.Weight = GetFontWeights(arr[3]);
  
                        }

                    }
                    else
                    {
                        if (_rootKey == "3")
                        {
                            _newText = (arrName.Length > 1 ? arrName[1] : res.PairItemList[i].Texts);


                        }
                    }
                }
            }
        }


        
        private void ContextMenuClick(object sender, RadRoutedEventArgs e)
        {
            combinedKey = ((PairItem)treeView.SelectedItem).Key;
            string path = ((PlusPair)((PairItem)treeView.SelectedItem)).Path;
            string txt = ((PairItem)treeView.SelectedItem).Texts;
            _keyPassport_onCopy = "0";
            _keyPassport_onDrop = "0";
            _keyPassport_onDropParent = "0";
            //MessageBox.Show(combinedKey+"Test             ");
            //Model.NeedOpenPassportDetail = combinedKey.Split('z')[1];
            arr = combinedKey.Split('~');

            if (_selectPassport)
            {
                if (arr.Length >= 4)
                {
                    if (!string.IsNullOrEmpty(arr[3]))
                    {
                        string ttt = arr[1];

                        _keyPassport_onCopy = arr[1];
                        _keyPassport_onDrop = arr[1];
                        _keyPassport_onDropParent = arr[1];
                        _namePassport_onDrop = ((PairItem)treeView.SelectedItem).Texts;
                        _namePassport_onDropParent = ((PairItem)treeView.SelectedItem).Texts;



                        //проверка на доступ к паспорту
                        //ServiceDataClient.ReturnAccessObjCompleted += ReturnAccessObjOnTreeCompleted;
                        //ServiceDataClient.ReturnAccessObjAsync(arr[1]);
                        
                        Navigation navigation = new Navigation
                           (
                            "NeedOpenPassportDetail",
                              arr[1],
                              0,
                              arr[2],
                              "",
                              false,
                              false,
                              "1",
                              path, 
                              txt,
                              Model.KeyOntree
                           );
                        Model.sBack.Push(navigation);
                         Model.FirePropertyChanged("BackTrue");
                         Model.sForward.Clear();
                         Model.FirePropertyChanged("ForwardFalse");


                         RadTreeViewItem _parentItem;
                         string _parentNameKeyOnAdmin = "";
                         PlusPair _plusPairparentItem;

                         _parentItem = _currentItem.ParentItem;
                         if (_parentItem != null)
                         {
                             _plusPairparentItem = _parentItem.Item as PlusPair;

                             string _parentKey = _plusPairparentItem.Key;
                             string[] arrParent = _parentKey.Split('~');
                             if (arrParent.Length != 0)
                             {
                                 _parentNameKeyOnAdmin = arrParent[1];
                             }
                         }

                        
                        Model.NeedOpenPassportDetail = new PassportDetailOpenParam()
                        {
                            PassportKey = arr[1],
                            IsEditedPassport = 0,
                            EntityKey = arr[2],
                            ParentNameKeyOnAdmin = _parentNameKeyOnAdmin
                        };
                        //закоментировано!!! - ключ для внешней программы - внешнего javascript
                        //НЕ АКТУАЛЬНО!!!!!!!!!!!!!
                        // HtmlPage.Window.Invoke("getObjKey", new string[] {arr[1]});

                    }
                    else
                    {
                       
                       Navigation navigation = new Navigation
                           (
                             "NeedOpenPassport",
                             "",
                             0,
                            arr[2],
                             arr[1],
                             true,
                            true,
                             "1",
                             path,
                             txt,
                             Model.KeyOntree
                           );
                       Model.sBack.Push(navigation);
                       Model.FirePropertyChanged("BackTrue");
                       Model.sForward.Clear();
                       Model.FirePropertyChanged("ForwardFalse");
                        Model.NeedOpenPassport = new PassportOpenParam()
                        {
                            EntityKey = arr[2],
                            ParentKey = arr[1],
                            IsShowAllField = true,
                            IsShowTreeOnFindkey = true
                        };
                    }
                }
                else
                {
                  //Самый верхний уровень дерева
                    Navigation navigation = new Navigation
                          (
                            "NeedOpenPassport",
                            "",
                            0,
                           arr[2],
                            arr[1],
                            true,
                           true,
                            "1",
                            path,
                            txt,
                            Model.KeyOntree
                          );
                    Model.sBack.Push(navigation);
                    Model.FirePropertyChanged("BackTrue");
                    Model.sForward.Clear();
                    Model.FirePropertyChanged("ForwardFalse");

                    Model.NeedOpenPassport = new PassportOpenParam()
                    {
                        EntityKey = arr[2],
                        ParentKey = arr[1],
                        IsShowAllField = true,
                        IsShowTreeOnFindkey = true
                    };



                    e.Handled = false;
                }
            }

           
        }

        private void treeView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //e.
        }

      
    }

}
#region по клику в списочной форме(списочная форма открыта из дерева) показываем ветку на котророй находится паспорт



//private void AddNewNode(string selectedItem)
//{

//    keyPassportOnGrid = selectedItem;
//    //RadTreeViewItem item = _lastSelected;
//    _currentItem = _lastSelected;

//    if (_lastSelected != null && _rootKey != "2")
//    {

//        Pair c = _currentItem.Item as Pair;
//        if (c != null)
//        {
//            proxy = new ServiceDataClient();
//            proxy.GetNextNodeCompleted += GetNextNodeOnKeyPassport;
//            proxy.GetNextNodeAsync(c.Key, _rootKey, GlobalContext.Context);
//        }
//    }
//    else
//    {
//        //treeView.ItemContainerStyle = null;
//        //очищаем дерево!!!!!!!!!????
//        if (treeView.ItemsSource != null)
//            treeView.ItemsSource = null;
//        treeView.Items.Clear();
//        //_rootKey = "2";


//        //proxy = new ServiceDataClient();

//        //proxy.ReturnAllTreeOnObjKeyCompleted += proxy_ReturnAllTreeOnObjKeyCompleted;
//        //proxy.ReturnAllTreeOnObjKeyAsync(selectedItem, GlobalContext.Context);
//            //?key=63914303
//            //новое!
//            //строим первую верхнюю веточку дерева,
//        _typeTree = "2";
//        _rootKey = keyPassportOnGrid;
//            proxy.CreateRootOfTreeCompleted += CreateRootOfTreeCompleted;
//            proxy.CreateRootOfTreeAsync("3", GlobalContext.Context);

//    }
//}

//void GetNextNodeOnKeyPassport(object sender, GetNextNodeCompletedEventArgs e)
//{

//    //proxy.GetNextNodeCompleted -= GetNextNodeCompleted;
//    //treeView.LoadOnDemand -= treeView_LoadOnDemand;
//    //treeView.LoadOnDemand += treeView_LoadOnDemand;
//    //изменение аттрибутов item дерева 
//    //(1,2,3 - (3 символа RGB - цвет )
//    // 4 -  толщина шрифта (обозначения в базе b - Bold, n - Normal, t-Thin)
//    // 5 -  размер шрифта (число - например - 12)
//    // 6 -  стиль шрифта (i - Italic, n - Normal) 
//    // аттрибуты разделены между собой - #
//    // пример -255#0#0#b#12#i#
//    // 255#0#0# - цвет красный, b# - толщина шрифта -  толстый, 12# - размер шрифта 12, i# - наклонный
//    // по умолчанию берется стандартный набор 
//    // 0#0#0# - цвет черный, n# - толщина шрифта -  нормальный, 12# - размер шрифта 12, n# - нормальный
//    // 0#0#0#n#12#n#
//    SolidColorBrush brush = new SolidColorBrush(
//                   Color.FromArgb(
//                       255,
//                       Byte.Parse("0"),
//                       Byte.Parse("0"),
//                       Byte.Parse("0"))
//                   );

//    FontWeight fw = FontWeights.Normal;
//    int fontSize = 12;
//    FontStyle fs = FontStyles.Normal;
//    var res = e.Result;

//    if (res != null)
//    {
//        if (_currentItem.Items.Count > 0)
//        {
//            //_currentItem.Items.Clear();
//        }

//        for (int i = 0; i < res.Count; i++)
//        {
//            string[] arr = res[i].Texts.Split('#');
//            if (arr.Length == 7)
//            {
//                brush = GetSolidColorBrush(arr);
//                fw = GetFontWeights(arr[3]);
//                fontSize = int.Parse(arr[4]);
//                fs = GetFontStyle(arr[5]);
//            }

//            string[] arrName = res[i].Texts.Split('~');
//            if (arrName.Length > 1)
//            {
//                if (_rootKey == "3")
//                {
//                    _currentItem.Items.Add(new PlusPair
//                    {
//                        Texts = (arrName.Length > 1 ? arrName[1] : res[i].Texts),
//                        Key = res[i].Key,
//                        Brush = brush,
//                        Weight = fw,
//                        Style = fs,
//                        Size = fontSize
//                    });
//                }
//                else
//                {
//                    ObjOnTree _ot = _currentItem.Item as ObjOnTree;
//                    string nodeName = (arrName.Length > 1 ? arrName[1] : res[i].Texts);
//                    _currentItem.IsLoadingOnDemand = false;
//                    _ot.Children.Add(new ObjOnTree(res[i].Key, _ot.Key, nodeName));

//                }
//            }
//            else
//            {
//                if (_rootKey == "3")
//                {
//                    _currentItem.Items.Add(new PlusPair
//                    {
//                        Texts = (arrName.Length > 1 ? arrName[1] : res[i].Texts),
//                        Key = res[i].Key,
//                        Brush = brush,
//                        Weight = fw,
//                        Style = fs,
//                        Size = fontSize

//                    });
//                }
//                else
//                {

//                    ObjOnTree _ot = _currentItem.Item as ObjOnTree;
//                    string nodeName = (arrName.Length > 1 ? arrName[1] : res[i].Texts);
//                    _ot.Children.Add(new ObjOnTree(res[i].Key, _ot.Key, nodeName));
//                    _currentItem.IsLoadingOnDemand = false;
//                }

//            }
//            //_currentItem.ItemContainerGenerator.StatusChanged += new EventHandler(ItemContainerGenerator_StatusChanged);
//        }

//        //_currentItem.IsLoadOnDemandEnabled = true;
//        //_currentItem.IsExpanded = true;)
//        if (res.Count == 0)
//        {
//            _currentItem.IsLoadOnDemandEnabled = false;
//            _currentItem.IsExpanded = false;
//        }
//        else
//        {
//            _currentItem.IsLoadOnDemandEnabled = true;
//            _currentItem.IsExpanded = true;
//        }
//    }



//    //treeView.LoadOnDemand -= treeView_LoadOnDemand;
//    //treeView.LoadOnDemand += treeView_LoadOnDemand;


//    _currentItem.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
//    //treeView.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;

//}
//void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
//{
//    RadTreeViewItem currentItem_old;
//    currentItem_old = _currentItem;
//    // if (treeView.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
//    if (_currentItem.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
//    {
//        _currentItem.ItemContainerGenerator.StatusChanged -= ItemContainerGenerator_StatusChanged;
//        for (int i = 0; i < _currentItem.Items.Count; i++)
//        {

//            RadTreeViewItem childItemContainer = _currentItem.ItemContainerGenerator.ContainerFromIndex(i) as RadTreeViewItem;

//            string combinedKey = ((Pair)_currentItem.Items[i]).Key;
//            arr = combinedKey.Split('z');
//            if (arr.Length >= 4)
//                if (!string.IsNullOrEmpty(arr[3]))
//                {
//                    if (keyPassportOnGrid == arr[1])
//                    {
//                        _currentItem = childItemContainer;
//                        Pair c = _currentItem.Item as Pair;
//                        if (c != null)
//                        {
//                            _currentItem.IsSelected = true;


//                            _currentItem.Focus();
//                            _currentItem.FontWeight = FontWeights.Bold;
//                            proxy = new ServiceDataClient();
//                            proxy.GetNextNodeCompleted += GetNextNodeCompleted;
//                            proxy.GetNextNodeAsync(c.Key, _rootKey, GlobalContext.Context);
//                            break;
//                        }
//                        else return;

//                    }
//                }
//        }

//    }
//}

#endregion


  //private void ClearLinknavigation(ObservableCollection<Navigation> navigationList)
  //      {
  //          if (navigationList.Count > 0)
  //          {
  //              foreach (var navigation in navigationList)
  //              {
  //                  navigation.Link = "0";
  //              }     
  //          }
           
  //      }
//void proxy_GetTreeFullHierarchyPathToObjectCompleted(object sender, GetTreeFullHierarchyPathToObjectCompletedEventArgs e)
//{
//    proxy.GetTreeFullHierarchyPathToObjectCompleted -= proxy_GetTreeFullHierarchyPathToObjectCompleted;

//    if (e.Error != null)
//    {
//        Model.Report("TreeControl.CreateRootOfTreeCompleted = " + e.Error.Message);
//        return;
//    }

//    _selectionPath = new ObservableCollection<TreeFullHierarchyPathToObjectItems>();
//    _selectionPath = e.Result.TreeFullHierarchyPathToObjectList;

//    //список ключей объектов и ключей паспорта до нужного уровня
//    //_pendingSelectionPath = new List<string>();

//    //for (int i = 0; i < _selectionPath.Count; i++)
//    //{
//    //    _pendingSelectionPath.Add(_selectionPath[i].ObjTypeKey);
//    //    _pendingSelectionPath.Add(_selectionPath[i].ObjKey);
//    //}

//    //List<PlusPair> list = new List<PlusPair>();

//    //foreach (var item in treeView.Items)
//    //{
//    //    list.Add(item as PlusPair);
//    //}

//    //CountnextNode = "0";
//    //var itemsControl = treeView as System.Windows.Controls.ItemsControl;

//    //if (IsFindTreeOnNavigation)
//    //{
//    //    FindpathOnObject(itemsControl, list);
//    //}
//    //else
//    //{
//    //    ExpandToPendingSelection(itemsControl, list);    
//    //}

//}

//void proxy_ReturnAllTreeOnObjKeyCompleted(object sender, ReturnAllTreeOnObjKeyCompletedEventArgs e)
//{
//    Model.Report("ReturnAllTreeOnObjKey - обработка данных");
//    if (e.Error != null)
//    {
//        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//        //code.SessionEnd.Redirect();
//        Model.Report("ReturnAllTreeOnObjKey = " + e.Error.Message);
//        busyIndicatorTree1.IsBusy = false;
//        return;
//    }
//    // IsShowBusyTree = true;
//    //var res = e.Result;
//    //if (res.IsValid)
//    //{
//    //    if (treeView.ItemsSource != null)
//    //        treeView.ItemsSource = null;
//    //    else

//    //        treeView.Items.Clear();


//    //    var template =
//    //        this.Resources["PairTemplateObjTree"] as Telerik.Windows.Controls.HierarchicalDataTemplate;
//    //    treeView.ItemTemplate = template;

//    //    var templateStyle = this.Resources["myItemContainerStyle"] as System.Windows.Style;
//    //    treeView.ItemContainerStyle = templateStyle;

//    //    Source = new List<ObjOnTree>();
//    //    if (e.Result.DataTreeViewOnObjkeyList.Count > 0)
//    //    {
//    //        Collection<DataTreeViewOnObjkeyItems> ttt = e.Result.DataTreeViewOnObjkeyList;

//    //        FillSource("0", ttt);
//    //        Model.Report("ReturnAllTreeOnObjKey - данные для дерева treeView.ItemsSource");
//    //        Model.Report("ReturnAllTreeOnObjKey - e.Result.DataTreeViewOnObjkeyList.Count = " + e.Result.DataTreeViewOnObjkeyList.Count.ToString());
//    //        treeView.ItemsSource = Source;

//    //        //treeView.IsExpandOnSingleClickEnabled = true;
//    //        //treeView.LoadOnDemand += treeView_LoadOnDemand;
//    //        //treeView.Expanded = true;
//    //    }

//    //    busyIndicatorTree1.IsBusy = false;

//    //}
//    //else
//    //{
//    //    MainModel.Report(" err = " + res.ErrorMessage); // Выводим сообщение об ошибке
//    //}

//    ////IsShowBusyTree = false;
//    //Model.Report("дерево по ключу объекта построено!");
//    //treeView.IsExpandOnSingleClickEnabled = true;
//    //treeView.IsTabStop = true;
//    ////treeView.LoadOnDemand -= treeView_LoadOnDemand;
//    ////treeView.LoadOnDemand += treeView_LoadOnDemand;

//}

//private void FillSource(string parentKey, Collection<DataTreeViewOnObjkeyItems> table)
//{
//    Source.Clear();

//    var item = table[0];

//    ObjOnTree p = CreateObjOnTree(item.Level, item.Key, item.ParentKey, item.Texts, table);
//    Source.Add(p);

//}

//private ObjOnTree CreateObjOnTree(string level, string key, string parentKey, string name, Collection<DataTreeViewOnObjkeyItems> table)
//{
//    ObjOnTree p = new ObjOnTree(key, parentKey, name);


//    foreach (var item in table)
//    {
//        if (item.ParentKey == level)
//        {

//            var pp = CreateObjOnTree(item.Level, item.Key, item.ParentKey, item.Texts, table);
//            p.Children.Add(pp);


//        }
//    }
//    return p;

//}

//расчитываем парент

//string nkey = "0";


//int i = _plusPairparentItem.Key.IndexOf("~");
//string[] ArrayKeys = _plusPairparentItem.Key.Split('~');
//if (i != -1)
//{
//    nkey = ArrayKeys[0];
//}
//else
//{
//    nkey = _plusPairparentItem.Key;
//}


//if (nkey == "100")
//{

//    if (_rootKey == "3")
//    {

//        if (_plusPairparentItem.Key != null)
//        {
//            // _plusPairparentItem.Children.Clear();
//            _currentItem = _parentItem;
//            //proxy.GetNextNodeCompleted += GetNextNodeReloadNodeCompleted;
//            // //
//            //proxy.GetNextNodeAsync(_plusPairparentItem.Key, _rootKey, GlobalContext.Context);
//            //_currentItem.IsLoadingOnDemand = false;


//        }
//    }
//    else
//    {
//        proxy.GetNextNodeCompleted += GetNextNodeCompleted;
//        proxy.GetNextNodeAsync(_itemKey, "3", GlobalContext.Context);
//    }

//}
//else
//{
//    if (nkey == "101")
//    {
//        _plusPairCurrentItem = _lastSelected.DataContext as PlusPair;

//        if (_rootKey == "3")
//        {
//            PlusPair c = _currentItem.Item as PlusPair;
//            if (c != null)
//            {
//                proxy.GetNextNodeCompleted += GetNextNodeCompleted;
//                proxy.GetNextNodeAsync(c.Key, _rootKey, GlobalContext.Context);
//                // _currentItem.IsLoadingOnDemand = false;
//            }
//        }
//        else
//        {
//            proxy.GetNextNodeCompleted += GetNextNodeCompleted;
//            proxy.GetNextNodeAsync(_itemKey, "3", GlobalContext.Context);
//        }
//    }
//}
//срабатывает , если из списочной формы(таблица) выбрали объект, он должен подсветится в дереве
//if (e.PropertyName == "FindTreeOnkeyPassport")
//{
//    //AddNewNode(Model.NeedOpenPassportDetail.PassportKey);
//    AddNewTreeOnkey();
//}

//_pathOnObject = new List<string>();
////вычисляем сохраняем путь к объекту
// string[] arr = (Model.Path).Split('|');

//string str = "";
// for (int ii = 0; ii < arr.Length-1; ii++)
// {
//     str = str + arr[ii]+"|";
//     _pathOnObject.Add(str);

// }

//List<PlusPair> list = new List<PlusPair>();

//if (treeView.Items.Count != 0)
//{
//    foreach (var item in treeView.Items)
//    {
//        list.Add(item as PlusPair);
//    }

//    var itemsControl = treeView as System.Windows.Controls.ItemsControl;
//    FindpathOnObject(itemsControl, list);
//}

//поиск по пути - для общего развития
//RadTreeViewItem targetItem = treeView.GetItemByPath(Model.Path, "|");

//    if (targetItem != null)
//    {
//        targetItem.IsExpanded = true;
//        targetItem.IsSelected = true;
//    }
//        public object Convert(object value, Type targetType, object parameter,
//  System.Globalization.CultureInfo culture)
//{
//    if ((value as List<RadTreeViewItem>).Count > 1) // Node has Children?
//    {
//        if ((parameter as String) == "{box}")
//        {
//            return Visibility.Collapsed;
//        }
//        else ((parameter as String) == "{block}")
//        {
//            return Visibility.Visible;
//        }
//    }
//    else
//    {
//        /*
//         * As above, just with Collapsed and Visible switched
//         */
//    }
//}

//for (int i = 0; i < res.Count; i++)
//{

//    RadTreeViewItem container = _currentItem.ItemContainerGenerator.ContainerFromIndex(i) as RadTreeViewItem;
//    //container.Background = new SolidColorBrush(Colors.Red);
//    double x_2 = i % 2;
//    if (x_2 == 0)
//    {

//        container.Height = 0d;
//        container.Width = 0d;
//        container.Visibility = Visibility.Collapsed;
//       container.UpdateLayout();



//    }
//    else
//    {
//        container.Visibility = Visibility.Visible;
//        container.UpdateLayout();
//    }

//}



