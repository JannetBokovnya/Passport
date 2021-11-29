using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Resources;
using Media;
using Media.Interfaces;
using Passpot.Business;
using Passpot.Controls;
using Passpot.components;
using System.Windows.Shapes;
using System.Windows.Browser;
using System.Collections.Generic;
using Media.Resources;
using Services.ServiceReference;
using Telerik.Windows;
using Telerik.Windows.Controls;
using System.Globalization;
using Passpot.Model;
using ListBoxItem = System.Windows.Controls.ListBoxItem;

namespace Passpot
{


    public partial class MainView : UserControl
    {
        private string rootKey = "3";
        private string keyPassportFromOut = "";
        public string visibleShema = "2";
        public string tsdiagram = "";
        public string tsobject = "";
        private ServiceDataClient _serviceDataClient;
        private string returnKey = "1";
        public string keyEntityFromOut = "";
        public string flagurl = "";
        private string keyPressed = "";
        private string keyVisibleLogPressed = "";
        string CR = "\r\n";
        private ChildWindowAttantion _popupWindowAttantion;
        private static string sjason = "";
        public static double _height = 0;
        public FindKeyOnTree findKeyOnTree;
        private RadContextMenu contextMenu;
        private TextBox tb = new TextBox();
        private RadListBox rlb = new RadListBox();
        


       

        protected ServiceDataClient ServiceDataClient
        {
            get
            {
                if (_serviceDataClient == null)
                    _serviceDataClient = new ServiceDataClient();
                return _serviceDataClient;
            }
        }
        public MainModel Model
        {
            get { return DataContext as MainModel; }
        }


        [ScriptableMember()]
        public static void ApplicationLoaded()
        {
            HtmlPage.Window.Invoke("applicationLoaded");

            string yy = ProjectResources.checkDetal;

            var h = CultureInfo.CurrentUICulture;
        }


        [ScriptableMember()]
        public void getCurrentState()
        {
            //TODO: вызвать механизм сохранения состояния

            SaveZakl_Jason();
            SetCurrentState(Model.stringJason);

        }

        /// <summary>
        /// Функция возвращает имя модуля - "PASSPORT". Если открыт паспорт возвращает "PASSPORT_КлючПаспорта"
        /// который используется для сохранения состояния страницы
        /// </summary>
        /// <returns></returns>
        [ScriptableMember ()]
        public string getWindowNameWithKey ()
        {
            //return  (Model.NeedOpenPassportDetail != null) ? "PASSPORT_" + Model.NeedOpenPassportDetail.PassportKey
            //                                               : "PASSPORT";

            return "PASSPORT";
        }

        public  void SetCurrentState(string p_cValue)
        {
            Model.Report("Сохранение состояния  закладки setCurrentState, p_cValue(Jason) =  " + p_cValue);
            HtmlPage.Window.Invoke("setCurrentState", p_cValue);
        }

        [ScriptableMember()]
        public static void sendEvent(string in_EventName, string in_Params, string in_NameAttr)
        {
            
            HtmlPage.Window.Invoke("sendEvent", in_EventName, in_Params, in_NameAttr);

        }

        [ScriptableMember()]
        public void receiveEvent(string in_EventName, string in_Params)
        {

            try
            {
               // receiveEvent = SHOW_PASSPORT in_Params{"SenderWindow":"MAP", "Destination":["PASSPORT_1600589702"], "EventParam": {"NOBJKEY":1600589702} }
                Model.Report("receiveEvent = " + in_EventName + " in_Params" + in_Params);
                //in_Params = "{"SenderWindow":"MAP", "Destination":["PASSPORT_1600589702"], "EventParam": {"NOBJKEY":1600589702} }"
                switch (in_EventName)
                {
                    case "SHOW_PASSPORT":
                        {
                            if (!String.IsNullOrEmpty(in_Params))
                            {

                                SenderWindow sw = JsonHelper.JsonDeserialize<SenderWindow>(in_Params);
                                if (!String.IsNullOrEmpty(sw.EventParam.NOBJKEY))
                                {

                                    //добавлено показ дерева до объекта
                                    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                                    Model.NeedOpenPassportDetail = new PassportDetailOpenParam() { PassportKey = sw.EventParam.NOBJKEY, IsVisibleButtonTransit = 1 };
                                    findKeyOnTree = new FindKeyOnTree(sw.EventParam.NOBJKEY);
                                    Model.KeyOntree = sw.EventParam.NOBJKEY;
                                    Model.Report("receiveEvent Model.FirePropertyChanged Событие - FindTreeOnkeyPassport");
                                    Model.FirePropertyChanged("FindTreeOnkeyPassport");



                                }
                                if (!String.IsNullOrEmpty(sw.EventParam.ENTITYKEY))
                                {
                                    //открываем списочную форму

                                    var passporOpenParam = new PassportOpenParam()
                                    {
                                        EntityKey = sw.EventParam.ENTITYKEY,
                                        ParentKey = "-1",
                                        IsShowAllField = true,
                                        IsShowTreeOnFindkey = true
                                    };
                                    ShowPassportView(passporOpenParam);
                                }
                            }

                            break;
                        }
                    case "SHOW_PASSPORT_SGR":
                        {
                            if (!String.IsNullOrEmpty(in_Params))
                            {
                                 SenderWindow sw = JsonHelper.JsonDeserialize<SenderWindow>(in_Params);
                                if (!String.IsNullOrEmpty(sw.EventParam.NOBJKEY))
                                {
                                    rootKey = "-1";
                                    PanelTree.Child = null;
                                    //radpane.IsPinned = false;
                                   // radpane.IsHidden = true;

                                    Model.NeedOpenPassportDetail = new PassportDetailOpenParam() { PassportKey = sw.EventParam.NOBJKEY, IsVisibleButtonTransit = 1 };
                                    Model.FirePropertyChanged("FindTreeOnkeyPassport");
                                }

                            }
                           break; 
                        }
                    case "LOAD_STATE":
                        {
                            if (!String.IsNullOrEmpty(in_Params))
                            {
                               
                                ReadZakl(in_Params);
                            }

                            break;
                        }
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

                Model.Report(ex.Message);
            }
            //receiveMsg.Text = "EventName=" + in_EventName + "; Params=" + in_Params;
        }


        public MainView()
        {
           
            InitializeComponent();

            //локализация для всех гридов телерик - подключаем локализацию
            LocalizationManager.DefaultResourceManager = ProjectResources.ResourceManager;
            //LocalizationManager.DefaultResourceManager = new ResourceManager("Telerik.Windows.Examples.GridView.Localization.Ru-ru", Assembly.GetCallingAssembly());

            HtmlPage.RegisterScriptableObject("Page", this);
            //если вызов из "толстого" клиента(не через эвент) то обрабатываем флаг!!!(он = 0)
            Dictionary<string, string> urlparams =
            HtmlPage.Document.QueryString as Dictionary<string, string>;
            ////показывает или дерево объектов или дерево классификатора 
            //keyPassportFromOut = "64390103";
            ReturnParam(urlparams);
 
        }

        private void ReturnParam(Dictionary<string, string> getparams)
        {
            foreach (KeyValuePair<string, string> pair in getparams)
            {

                if (pair.Key == "tree")
                {
                    rootKey = pair.Value;

                }
               
                if (pair.Key == "key")
                {
                    keyPassportFromOut = pair.Value;
                }

                if (pair.Key == "embed")
                {
                    visibleShema = pair.Value;

                }

                //получение ключей из аркгиса (из схему), получаю ключи и из них получаю ключ паспорта
                if (pair.Key == "tsdiagram")
                {
                    tsdiagram = pair.Value;
                }

                if (pair.Key == "tsobject")
                {
                    tsobject = pair.Value;
                }

                //получение ключа ентити из строки - по нему строится таблица
                if (pair.Key == "keyEntity")
                {
                    keyEntityFromOut = pair.Value;
                }
                //получение флага загрузки(через эвент или через урл)
                if (pair.Key == "flagurl")
                {
                    flagurl = pair.Value;
                }

            }
            //для построения дерева 1-общее дерево, 2- частное дерева для одного объекта

        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Model.PropertyChanged += MainModelPropertyChanged;

            ServiceDataClient.GetDisplayLabelsCompleted += (ServiceDataClient_GetDisplayLabelsCompleted);
            ServiceDataClient.GetDisplayLabelsAsync(GlobalContext.Context);

            Model.Report("MainView.OnLoaded + вызываем ApplicationLoaded()");


            if (flagurl == "")
            {
                Model.Report("MainView.OnLoaded - загружаем LoadOnEvent()");
                LoadOnEvent();
            }

            this.KeyDown -= OnKeyDown;
            this.KeyDown += OnKeyDown;

            RightColumn.Width = new GridLength(350);
            //посылаем сообщение, что форма создана!!!
            ApplicationLoaded();

             Model.FirePropertyChanged("VisibleLog");

        }
        void ServiceDataClient_GetDisplayLabelsCompleted(object sender, GetDisplayLabelsCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                
            }
            else
            {
                if (e.Result.IsValid)
                {
                    for (int i = 0; i < e.Result.DisplayLabelsItemList.Count; i++)
                    {
                        DisplayLabel.Dict.Add(e.Result.DisplayLabelsItemList[i].Сplace,
                                              e.Result.DisplayLabelsItemList[i].Сvalue);
                    }

                    if (DisplayLabel.Dict.ContainsKey("ObjectTreeFrame"))
                    {
                       
                    }
                    
                  
                    Model.FirePropertyChanged("DisplayLabel");

                    if (!string.IsNullOrEmpty(Model.KeyOntree))
                    {
                        Model.Report("событие Model.KeyOntree = " + Model.KeyOntree);
                        if (!string.IsNullOrEmpty(Model.NeedOpenPassportDetail.PassportKey))
                        {
                            Model.Report("событие Model.KeyOntree = " + Model.KeyOntree + "Model.NeedOpenPassportDetail.PassportKey =" + Model.NeedOpenPassportDetail.PassportKey + "вызываем событие FindTreeOnkeyPassport построение дерева по ключу");
                            Model.FirePropertyChanged("FindTreeOnkeyPassport");    
                        }
                    }
                }
                else
                {
                    Model.Report("ServiceDataClient_GetDisplayLabels-"+ e.Result.ErrorMessage);
                }
            }

           
        }



       
        //  25056

        private void LoadOnEvent()
        {

            Model.Report("MainView  LoadOnEvent - создается");
            if (keyPassportFromOut != "")
            {
                ServiceDataClient.ReturnAccessObjCompleted += ReturnAccessObjCompleted;
                ServiceDataClient.ReturnAccessObjAsync(keyPassportFromOut, GlobalContext.Context);
            }
            else
            {
                if (flagurl == "1")
                {
                    new ButtonOnTollbar() { KeyTollBarButton = "0" };
                    if (keyPassportFromOut != "")
                    {

                        var passportDetailOpenParam = new PassportDetailOpenParam() { PassportKey = keyPassportFromOut };
                        ShowPassportDetailView(passportDetailOpenParam);
                    }
                    if (tsdiagram != "" && tsobject != "")
                    {
                        ServiceDataClient.ReturnKeyObjOnShemaCompleted += OnReturnKeyObjOnShema;
                        ServiceDataClient.ReturnKeyObjOnShemaAsync(tsdiagram, tsobject, GlobalContext.Context);
                    }

                    if (keyEntityFromOut != "")
                    {

                        var passporOpenParam = new PassportOpenParam() { EntityKey = keyEntityFromOut, ParentKey = "-1", IsShowAllField = true, IsShowTreeOnFindkey = true };
                        ShowPassportView(passporOpenParam);

                    }
                    if (flagurl != "1")
                    {

                    }
                }
                else
                {

                    if (keyEntityFromOut != "")
                    {

                        var passporOpenParam = new PassportOpenParam() { EntityKey = keyEntityFromOut, ParentKey = "-1", IsShowAllField = true, IsShowTreeOnFindkey = true };
                        ShowPassportView(passporOpenParam);

                    }
                    new ButtonOnTollbar() { KeyTollBarButton = "1" };
                    if (keyPassportFromOut != "")
                    {
                        if (rootKey == "4")
                        {

                            PanelTree.Child=null;
                            PanelTree.Child = (TreeControl.CreateTree(rootKey, "2", "0", Model)); 

                            //при рефакторинге сделать
                            //var treeOpenParam = new TreeOpenParam() { RootKey = rootKey, TypeTree = "2", VisibleNode = "0"};
                            //ShowTreeView(treeOpenParam);


                        }
                        if (rootKey == "3")
                        {
 
                            PanelTree.Child = null;
                            PanelTree.Child = (TreeControl.CreateTree(keyPassportFromOut, "2", "0", Model));

                            //при рефакторинге сделать
                            //var treeOpenParam = new TreeOpenParam() { RootKey = keyPassportFromOut, TypeTree = "2", VisibleNode = "0" };
                            //ShowTreeView(treeOpenParam);
                           
                           
                        }
                        if (rootKey == "-1")
                        {

                            PanelTree.Child = null;

                        }

                        var passportDetailOpenParam = new PassportDetailOpenParam() { PassportKey = keyPassportFromOut };
                        ShowPassportDetailView(passportDetailOpenParam);

                    }
                    else
                    {
                        if (rootKey == "-1")
                        {
                            //PanelTree.Children.Clear();
                            PanelTree.Child=null;
                           // radpane.IsPinned = false;
                          //  radpane.IsHidden = true;


                        }
                        else
                        {
                            if (rootKey == "3")
                            {

                                PanelTree.Child=null;
                                Model.Report("ModelView.LoadOnEvent TreeControl.CreateTree(rootKey=" + rootKey);
                                PanelTree.Child = (TreeControl.CreateTree(rootKey, "1", "0", Model)); 
                                
                            }
                        }
                    }
                }
            }
        }

        void ReturnAccessObjCompleted(object sender, ReturnAccessObjCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                //code.SessionEnd.Redirect();
            }
            else
            {
                ServiceDataClient.ReturnAccessObjCompleted -= ReturnAccessObjCompleted;
                if (e.Result.IsValid)
                {
                    if (e.Result.AccessLevel_OnKey_result.AccessLevel == "0")
                    {
                        //пор просьбе трудящихся убрано
                        //_popupWindowAttantion = new ChildWindowAttantion();
                        //_popupWindowAttantion.titleBox.Text =
                        //    //" Обьекты данного типа не найдены, либо у вас отсутсвуют права на их просмотр. в случае производственной необходимости обратитесь к администртору! ";
                        //    "Паспорта на объекты данного типа в системе отсутствуют";
                        //_popupWindowAttantion.Show();


                        //sendEvent("AccessError","","");
                        //HtmlPage.Window.Navigate(new Uri("`/AccessError.aspx"));
                    }
                    else
                    {
                        if (flagurl == "1")
                        {
                            new ButtonOnTollbar() { KeyTollBarButton = "0" };
                            if (keyPassportFromOut != "")
                            {

                                var passportDetailOpenParam = new PassportDetailOpenParam() { PassportKey = keyPassportFromOut };
                                ShowPassportDetailView(passportDetailOpenParam);
                            }
                            if (tsdiagram != "" && tsobject != "")
                            {
                                ServiceDataClient.ReturnKeyObjOnShemaCompleted += OnReturnKeyObjOnShema;
                                ServiceDataClient.ReturnKeyObjOnShemaAsync(tsdiagram, tsobject, GlobalContext.Context);
                            }

                            if (keyEntityFromOut != "")
                            {

                                var passporOpenParam = new PassportOpenParam()
                                                           {
                                                               EntityKey = keyEntityFromOut,
                                                               ParentKey = "0",
                                                               IsShowAllField = true,
                                                               IsShowTreeOnFindkey = true
                                                           };
                                ShowPassportView(passporOpenParam);

                            }
                            if (flagurl != "1")
                            {
                                new ButtonOnTollbar() { KeyTollBarButton = "0" };
                            }
                        }
                        else
                        {
                            new ButtonOnTollbar() { KeyTollBarButton = "1" };
                            if (keyPassportFromOut != "")
                            {
                                if (rootKey == "4")
                                {
 
                                    PanelTree.Child=null;

                                    //при рефакторинге сделать
                                    //var treeOpenParam = new TreeOpenParam() { RootKey = rootKey, TypeTree = "1", VisibleNode = "0" };
                                    //ShowTreeView(treeOpenParam);

                                    PanelTree.Child = (TreeControl.CreateTree(rootKey, "1", "0", Model));
                                   

                                }
                                if (rootKey == "3")
                                {
 
                                    PanelTree.Child=null;

                                    //при рефакторинге сделать
                                    //var treeOpenParam = new TreeOpenParam() { RootKey = keyPassportFromOut, TypeTree = "2", VisibleNode = "0" };
                                    //ShowTreeView(treeOpenParam);

                                    PanelTree.Child = (TreeControl.CreateTree(keyPassportFromOut, "2","0", Model));
                                    


                                }
                                if (rootKey == "-1")
                                {

                                    PanelTree.Child = null;

                                }


                                var passportDetailOpenParam = new PassportDetailOpenParam() { PassportKey = keyPassportFromOut };
                                ShowPassportDetailView(passportDetailOpenParam);

                            }
                            else
                            {

                                //Открытие основного дерева
                                //!!!!!!!!!!!!!!!!!!!!!
 
                                PanelTree.Child = null;
                                PanelTree.Child = (TreeControl.CreateTree(rootKey, "1", "0", Model));

                                //при рефакторинге сделать
                               // var treeOpenParam = new TreeOpenParam() { RootKey = rootKey, TypeTree = "1", VisibleNode = "0" };
                               // ShowTreeView(treeOpenParam);

                            }
                        }
                    }
                }
                else
                {
                    Model.Report("MainView.ReturnAccessObjCompleted  Ошибка при получении доступа к объекту!!!" +
                                 e.Result.ErrorMessage);
                }
            }
        }

        void OnReturnKeyObjOnShema(object sender, ReturnKeyObjOnShemaCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                //code.SessionEnd.Redirect();
            }
            else
            {
                ServiceDataClient.ReturnKeyObjOnShemaCompleted -= OnReturnKeyObjOnShema;

                if (e.Result.IsValid)
                {

                    if (e.Result.KeyonShema_result.KeyonShema_arcgis == "")
                    {
                        Model.Report("Ключ паспорта со схемы не получен!!!");
                    }
                    else
                    {
                        var passportDetailOpenParam = new PassportDetailOpenParam() { PassportKey = e.Result.KeyonShema_result.KeyonShema_arcgis };
                        ShowPassportDetailView(passportDetailOpenParam);
                    }

                }
                else
                {
                    Model.Report("MainView.OnReturnKeyObjOnShema  Ключ паспорта со схемы не получен, ошибка!!!" +
                                 e.Result.ErrorMessage);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Обработчик сообщений PropertyChanged(когда произойдет событие PropertyChanged,
        /// то обработчик будет выполнен)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(e.PropertyName));
            }

            if (e.PropertyName == "NeedOpenPassportDetail")
            {
                //открываем форму PassportDetailView и передаем в качестве аргумента значение свойства
                //модели NeedOpenPassportDetail в котором содержится ключ паспорта
                ShowPassportDetailView(Model.NeedOpenPassportDetail);
            }
            else if (e.PropertyName == "NeedOpenPassport")
            {
                ShowPassportView(Model.NeedOpenPassport);
            }
            else if (e.PropertyName == "ClosePassportDetal")
            {
                contentHolder2.Child = null;
                contentHolder.Child = null;
               // Model.FirePropertyChanged("ModelInited");
               // ShowPassportView(Model.NeedOpenPassport);
               
            }
            if (e.PropertyName == "VisibleLog")
            {
               string t = passportForm.RowDefinitions[1].Height.ToString();
               if (passportForm.RowDefinitions[1].Height.ToString() == "0")
               {
                   passportForm.RowDefinitions[1].Height = new GridLength(50);
               }
               else
               {
                   passportForm.RowDefinitions[1].Height = new GridLength(0);
               }
                
            }

           
        }

        private void ShowTreeView(TreeOpenParam treeOpenParam)
        {
            var treeControlView = new TreeControlView();
            var treeModel = new TreeModel(Model);
            //treeModel.InitModel(treeOpenParam);
            treeControlView.DataContext = treeModel;

            PanelTree.Child = treeControlView;
            treeModel.InitModel(treeOpenParam);

        }



        private void ShowPassportView(PassportOpenParam passportOpenParam)
        {
            contentHolder2.Child = null;

            var passportView = new PassportView();
            var passportModel = new PassportModel(Model);
            passportModel.visiblebutonShema = visibleShema;
            passportView.DataContext = passportModel;

            contentHolder.Child = passportView;
            //spGridDataTabItem.Children.Add(passportView);
            Model.Indicator(true);
            passportModel.InitModel(passportOpenParam);

            //(passportOpenParam.EntityKey);

        }

        private void ShowPassportDetailView(PassportDetailOpenParam passportDetailOpenParam)
        {
            var passportDetailView = new PassportDetailView();
            var detailModel = new PassportDetailModel(Model);
            detailModel.visiblebutonShema = visibleShema;
            detailModel.VisibleChildTabItem = rootKey;
            passportDetailView.DataContext = detailModel;
            contentHolder2.Child = passportDetailView;
            Model.Indicator(true);
            //инициируем модель просмотра паспорта
            detailModel.InitModel(passportDetailOpenParam);
        }

       

        private void reportListBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if ((keyPressed + e.Key.ToString()) == "CtrlC")
            {
                string s = string.Empty;
                foreach (var _item in reportListBox.SelectedItems)
                {
                    s = s + _item.ToString() + CR;
                    
                }

                System.Windows.Clipboard.SetText(s);
                keyPressed = "";
               

            }
            else
            {
                keyPressed = e.Key.ToString();
            }


        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if ((keyVisibleLogPressed + e.Key.ToString()) == "CtrlM")
            {
                string s = string.Empty;


                Model.FirePropertyChanged("VisibleLog");
                keyVisibleLogPressed = "";

                
            }
            else
            {
                keyVisibleLogPressed = e.Key.ToString();
            }
        }

        public void SaveZakl_Jason()
        {
            SaveVkladka sv = new SaveVkladka();


            if (Model.NeedOpenPassport != null)
            {
                sv.EntityKey = Model.NeedOpenPassport.EntityKey;
                sv.ParentKey = Model.NeedOpenPassport.ParentKey;
                sv.PassportKey = "0";
               // sv.EntityName = "";
               // sv.PassportName = "";
                string jsonString = JsonHelper.JsonSerializer<SaveVkladka>(sv);
                Model.stringJason = jsonString;
                Model.Report(jsonString);
                sjason = jsonString;
            }
            else
            {
                if (Model.NeedOpenPassportDetail != null)
                {
                    sv.PassportKey = Model.NeedOpenPassportDetail.PassportKey;
                    sv.EntityKey = Model.NeedOpenPassportDetail.EntityKey;
                  //  sv.EntityName = "Название объекта";
                    sv.ParentKey = "0";
                 //   sv.PassportName = "Название паспорта";
                    string jsonString = JsonHelper.JsonSerializer<SaveVkladka>(sv);
                    Model.stringJason = jsonString;
                    Model.Report(jsonString);
                }
            }
            
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (RightColumn.Width.Value == 0)
            {
                GridSplitter.CollapseButton.Content = "4";
            }
            else
            {
                GridSplitter.CollapseButton.Content = "3";
            }
        }


        private void ReadZakl(string strJason)
        {
            if (strJason != "")
            {
                //SaveVkladka sv = JsonHelper.JsonDeserialize<SaveVkladka>(strJason);
                SenderWindow sw = JsonHelper.JsonDeserialize<SenderWindow>(strJason);
                if (sw.EventParam.PassportKey == "0")
                {
                    Model.NeedOpenPassport = new PassportOpenParam()
                    {
                        EntityKey = sw.EventParam.ENTITYKEY,
                        ParentKey = sw.EventParam.ParentKey,
                        IsShowAllField = true,
                        IsShowTreeOnFindkey = true
                    };
                }
                else
                {
                    Model.NeedOpenPassportDetail = new PassportDetailOpenParam() { PassportKey = sw.EventParam.PassportKey, IsEditedPassport = 0 };
                    PanelTree.Child = (TreeControl.CreateTree(sw.EventParam.PassportKey, "2","0", Model));
                   // Model.    .FirePropertyChanged("FindTreeOnkeyPassport");
                }
            }
        }

  
        private void GridSplitter_OnCollapseButtonClicked(object sender, EventArgs e)
        {
            if (RightColumn.Width.Value == 0)
            {
                RightColumn.Width = new GridLength(350);
            }
            else if (sender != this)
            {
                RightColumn.Width = new GridLength(0);
            }
        }

        private void MainView_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            e.Handled = true;

            if (rlb != null)
            {
                gridAll.Children.Remove(rlb);
            }
            
            tb = new TextBox();
            Point p = e.GetPosition(this);

             rlb.Items.Clear();
            string[] arrVersion = Model.Version.Split(';');
             for (int i = 0; i < arrVersion.Length - 1; i++)
             {
                 rlb.Items.Add(arrVersion[i]);      
             }
            rlb.VerticalAlignment = VerticalAlignment.Stretch;
            rlb.HorizontalAlignment = HorizontalAlignment.Stretch;
            double top = ((ActualHeight - p.Y) - (25*(arrVersion.Length-1)));
            double right = ((ActualWidth - p.X) - 280);
            rlb.Margin = new Thickness(p.X, p.Y, right, top);
            //tb.TextWidth(text1);
            gridAll.Children.Add(rlb);
            
        }


        private void MainView_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            gridAll.Children.Remove(rlb);
        }

        private void MainView_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            gridAll.Children.Remove(rlb);
        }
    }

}

