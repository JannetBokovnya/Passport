using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Media;
using Media.Interfaces;
using Media.Resources;
using Passpot.Business;
using Passpot.Model;
using Services.ServiceReference;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace Passpot.Controls
{
    public partial class TextBlockControl : UserControl, IControlValueChanged
    {
        private ServiceDataClient _serviceDataClient;
        private PassportDetailModel _passportModel;
        private FieldMetaDataItem _metaData;
        private bool _editPassport;
        public FindKeyOnTree findKeyOnTree;
        private ChildWindowLinkObj _linkTableWindow;
        private List<DataConnectList> _selectConnectList;
        public DataListRelationObjItems newItems;
        public string _keyPassportRelation = "";
        public string _namePassport;
        private ChildWindowDelete _popupWindowDelete;
        private ChildWindowAttantion _popupWindowAttantion;
        private List<DataListRelationObjItems> _selectRelationObjGridList = new List<DataListRelationObjItems>();
        private List<DataListRelationObjItems> old_selectRelationObjGridList = new List<DataListRelationObjItems>();
        private static int countrelationObj;

        public event PropertyChangedEventHandler PropertyChanged;

        private ServiceDataClient ServiceDataClient
        {
            get
            {
                if (_serviceDataClient == null)
                    _serviceDataClient = new ServiceDataClient();
                return _serviceDataClient;
            }
        }

        public void FirePropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }

        private PassportDetailModel Model
        {
            get { return DataContext as PassportDetailModel; }
        }


        public TextBlockControl()
        {
            InitializeComponent();
           
        }


        public static TextBlockControl CreateTextBlockControl(FieldMetaDataItem metaData, PassportDetailModel passportModel, bool editpassport)
         {
             var c = new TextBlockControl();
             c._metaData = metaData;
             c._passportModel = passportModel;
             c.titleTextBlock.Text = metaData.TITUL;
             c._editPassport = editpassport;
             
             switch (metaData.BASIC_FLD)
             {
                 case 1:
                     c.titleTextBlock.FontWeight = FontWeights.Bold;
                     break;
                 case 0:

                     break;

             }

             if ((!c._editPassport) || (c._metaData.IS_EDITED ==0))
             {
                 c.button_addRelation.Visibility = Visibility.Collapsed;
             }

             switch (metaData.MANDATORYVALUE_FLD)
             {
                 case 1:
                     c.titleTextBlock.Foreground = new SolidColorBrush(Colors.Red);

                     break;
                 case 0:

                     break;
             }

             if (c._passportModel.PassportDetailOpenParams.PassportKey != "0")
             {


                 c.StartOnGetRelationObj(c._passportModel.PassportDetailOpenParams.PassportKey,
                                                        c._passportModel.PassportDetailOpenParams.EntityKey,
                                                        c._metaData.FLDKEY.ToString(), c._metaData.FLDNAME, 1, 1);


             }
             else
             {
                 //c.addRelation.IsEnabled = false;
                 //c.button_dell.IsEnabled = false;
             }

             

            return c;
         }
       
        public void StartOnGetRelationObj(string keyObj, string keyEntity, string fldKey, string fldName, int typeVisible, int typeControl)
        {
            busyIndicatorTextBlock.IsBusy = true;
            //получаем данные для таблицы связей(название поля - обязательно!!!!!!!!)
            ServiceDataClient.GetDataRelationObjCompleted += GetDataRelationObjCompletedGrid;
            ServiceDataClient.GetDataRelationObjAsync(_passportModel.PassportDetailOpenParams.PassportKey, _metaData.FLDNAME.ToString(), GlobalContext.Context);
        }

        void GetDataRelationObjCompletedGrid(object sender, GetDataRelationObjCompletedEventArgs e)
        {
            ServiceDataClient.GetDataRelationObjCompleted -= GetDataRelationObjCompletedGrid;
            if (e.Result.IsValid)
            {

                _selectRelationObjGridList = new List<DataListRelationObjItems>(e.Result.DataListRelationObjList);

                old_selectRelationObjGridList = new List<DataListRelationObjItems>(e.Result.DataListRelationObjList);


                if ((_editPassport) && (_metaData.IS_EDITED == 1))
                {
                    //Если паспорт редактируем
                    //здесь создаем контролы

                    //если нет данных то только кнопка добавить
                    if (_selectRelationObjGridList.Count == 0)
                    {
                          List<DataListRelationObjItems> tt = new List<DataListRelationObjItems>();
                          tt.Add(new DataListRelationObjItems { KeyObj = "", NameEntity = "", NameObj =""});
                       

                        LinkControlEdit tc = LinkControlEdit.CreateLinkControlEdit(tt[0], _passportModel, _metaData, _selectRelationObjGridList, true, true, false);
                        spTextBlock.Children.Add(tc);
                        
                    }
                    else
                    {
                       // spTextBlockButton.Visibility = Visibility.Collapsed;
                        for (int ii = 0; ii < _selectRelationObjGridList.Count; ii++)
                        {
                            LinkControlEdit tc;
                            if (ii == 0)
                            {
                                tc = LinkControlEdit.CreateLinkControlEdit(_selectRelationObjGridList[ii], _passportModel, _metaData, _selectRelationObjGridList, false, true, false);    
                            }
                            else
                            {
                                 tc = LinkControlEdit.CreateLinkControlEdit(_selectRelationObjGridList[ii], _passportModel, _metaData, _selectRelationObjGridList, false, false, false);
                            }
                            
                            spTextBlock.Children.Add(tc);
                        }      
                    }
                  
                }
                else
                {
                    //здесь создаем контролы - просмотр
                    for (int ii = 0; ii < _selectRelationObjGridList.Count; ii++)
                    {
                        TextBlock tc = CreateTextBlock(_selectRelationObjGridList[ii]);
                        // lbLinks.Items.Add(tc);
                        spTextBlock.Children.Add(tc);
                    }
                    
                }

                //создаем кнопку, если значений больше 4, остальные невидимые
                if (_selectRelationObjGridList.Count >= 1)
                {
                    for (int t1 = 0; t1 < spTextBlock.Children.Count(); t1++)
                    {
                        if (t1 > 1)
                        {
                            spTextBlock.Children[t1].Visibility = Visibility.Collapsed;
                            
                        }
                    }
                    //создаем кнопку если больше 4 (для тестировании больше 2)
                    if (_selectRelationObjGridList.Count > 2)
                    {
                        TextBlock tc2 = CreateTextBlockVisibleAlllink();
                        if (!_editPassport)
                        {
                            tc2.Margin = new Thickness(5, 5, 0, 0);
                        }
                        else
                        {
                            tc2.Margin = new Thickness(39,5,0,0);
                        }
                        spTextBlock.Children.Add(tc2);
                    }

                }

                //наполняем лист для вывода в ексель горизонтальных связей 

                string _nameObj = "";

                for (int i = 0; i < _selectRelationObjGridList.Count; i++)
                {
                    DataListRelationObjItems dd = _selectRelationObjGridList[i];
                    _nameObj = _nameObj + dd.NameObj + ";";


                }
                _passportModel.ListRelationObjList.Add(new ListRelationObj(_metaData.FLDNAME, _nameObj));


            }
            else
            {
                _passportModel.MainModel.Report("Список связей GetDataRelationObj err = " + e.Result.ErrorMessage);
                Model.MainModel.Report("Список связей GetDataRelationObj err = " + e.Result.ErrorMessage);
               
            }

           // _passportModel.MainModel.Indicator(false);
            busyIndicatorTextBlock.IsBusy = false;
        }

      

        //создание связей - контрол текст блок
        private  TextBlock CreateTextBlock(DataListRelationObjItems relationList)
        {
            TextBlock tb = new TextBlock();
            tb.Name = relationList.KeyObj;
            tb.Text = relationList.NameObj.TrimStart();
            tb.TextDecorations = TextDecorations.Underline;
            tb.Cursor = Cursors.Hand;
            tb.TextTrimming = TextTrimming.WordEllipsis;
            tb.HorizontalAlignment = HorizontalAlignment.Stretch;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.FontFamily = new FontFamily("Tahoma");
            tb.Margin = new  Thickness(4,5,0,0);
            tb.Height = 23;
            tb.MouseLeftButtonDown += (tb_MouseLeftButtonDown);
            tb.Foreground = new SolidColorBrush(Color.FromArgb(
                                      255,
                                      Byte.Parse("0"),
                                      Byte.Parse("150"),
                                      Byte.Parse("219")));


            return tb;
        }

        //отдельный контрол - отобразить все связи

        private TextBlock CreateTextBlockVisibleAlllink()
        {
            TextBlock tb = new TextBlock();
            tb.Text = ProjectResources.LinkAll;
            tb.TextDecorations = TextDecorations.Underline;
            tb.Cursor = Cursors.Hand;
            tb.TextTrimming = TextTrimming.WordEllipsis;
            tb.HorizontalAlignment = HorizontalAlignment.Stretch;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.FontFamily = new FontFamily("Tahoma");
            tb.Margin = new Thickness(4,5,0,0);
            tb.Height = 23;
            tb.Tag = "1";
            tb.MouseLeftButtonDown += tb_MouseLeftButtonDownVisibleAlllink;
            tb.Foreground = new SolidColorBrush(Color.FromArgb(
                                      255,
                                      Byte.Parse("76"),
                                      Byte.Parse("147"),
                                      Byte.Parse("77")));


            return tb;
        }


         void tb_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var tt = sender as TextBlock;
            _passportModel.MainModel.NeedOpenPassportDetail = new PassportDetailOpenParam() { PassportKey = tt.Name };
            _passportModel.MainModel.FirePropertyChanged("FindTreeOnkeyPassport");
           
        }

        //нажатие на кнопки отобразить все связи
        private void tb_MouseLeftButtonDownVisibleAlllink(object sender, MouseButtonEventArgs e)
        {
            var tb = sender as TextBlock;
            if (tb.Tag == "1")
            {
                for (int i = 0; i < spTextBlock.Children.Count(); i++)
                {
                    spTextBlock.Children[i].Visibility = Visibility.Visible;
                }
                tb.Text = ProjectResources.LinkAllHide;
                tb.Tag = "0";
            }
            else
            {
                for (int i = 0; i < spTextBlock.Children.Count()-1; i++)
                {

                    if (i > 1)
                    {
                        spTextBlock.Children[i].Visibility = Visibility.Collapsed;
                    }

                }

                tb.Text = ProjectResources.LinkAll; 
                tb.Tag = "1";
            }

        }


      //кнопка добавить связь
        private void Button_addRelation_OnClick(object sender, RoutedEventArgs e)
        {
            ServiceDataClient.GetDataConnectCompleted += OnGetDataConnectCompleted;
            ServiceDataClient.GetDataConnectAsync(_passportModel.PassportDetailOpenParams.EntityKey, _metaData.FLDNAME, GlobalContext.Context);
        }

        void OnGetDataConnectCompleted(object sender, GetDataConnectCompletedEventArgs e)
        {
            ServiceDataClient.GetDataConnectCompleted -= OnGetDataConnectCompleted;
            if (e.Result.IsValid)
            {

                string allKeyRelation = "";

                if (_selectRelationObjGridList.Count > 0)
                {
                    for (int ii = 0; ii < _selectRelationObjGridList.Count; ii++)
                    {
                        allKeyRelation = allKeyRelation + _selectRelationObjGridList[ii].KeyObj + ", ";
                    }
                    allKeyRelation = allKeyRelation.Substring(0, allKeyRelation.Length - 2);
                }
                else
                {
                    allKeyRelation = "0";
                }



                _selectConnectList = new List<DataConnectList>(e.Result.DataConnectLists);
                var m = DataContext as PassportDetailModel;

                // _linkTableWindow = new PopUpChildWindow();
                _linkTableWindow = new ChildWindowLinkObj();
                _linkTableWindow.Title = ProjectResources.ChildWindowLinkObjTitle; //"Связи объекта ";
                _linkTableWindow.DataContext = m;
                _linkTableWindow.StubLinkData(_selectConnectList, _metaData.FLDNAME, allKeyRelation);
                _linkTableWindow.Show();
                _linkTableWindow.btnOk.Click += ButtonOkClick;

            }
            else
            {
                _passportModel.MainModel.Report("Список связей OnGetDataConnect err = " + e.Result.ErrorMessage);
            }
        }
  
        public void ButtonOkClick(object sender, RoutedEventArgs e)
        {


            _linkTableWindow.btnOk.Click -= ButtonOkClick;

            var t = _linkTableWindow.LinkOnGridItem;

            //if (_noLinks)
            //{
            //    _selectRelationObjGridList.Clear();
            //}
            //_selectRelationObjGridList.Add(new DataListRelationObjItems { KeyObj = t.ObjKey, NameEntity = t.EntityKey, NameObj = t.ObjName });
            newItems = null;
            newItems = new DataListRelationObjItems();
            newItems.KeyObj = t.ObjKey;
            newItems.NameObj = t.ObjName;
            newItems.NameEntity = t.EntityKey;

            KeyLinkAdd();

        }

        private void KeyLinkAdd()
        {
            if (_selectRelationObjGridList.Count == 0)
            {
                foreach (var child in spTextBlock.Children)
                {
                    var lceAdd = child as LinkControlEdit;
                    if (lceAdd != null && lceAdd.newItems != null)
                    {
                        _selectRelationObjGridList.Add(new DataListRelationObjItems { KeyObj = lceAdd.newItems.KeyObj, NameEntity = lceAdd.newItems.NameEntity, NameObj = lceAdd.newItems.NameObj });
                        LinkControlEdit tc = LinkControlEdit.CreateLinkControlEdit(_selectRelationObjGridList[0], _passportModel, _metaData, _selectRelationObjGridList, false, true, false);

                        spTextBlock.Children.Clear();
                        spTextBlock.Children.Add(tc);
                        return;
                    }
                    else
                    {


                        _selectRelationObjGridList.Add(new DataListRelationObjItems { KeyObj = newItems.KeyObj, NameEntity = newItems.NameEntity, NameObj = newItems.NameObj });
                        LinkControlEdit tc = LinkControlEdit.CreateLinkControlEdit(_selectRelationObjGridList[0], _passportModel, _metaData, _selectRelationObjGridList, false, true, false);
                        spTextBlock.Children.Clear();
                        spTextBlock.Children.Add(tc);


                        //_selectRelationObjGridList.Add(new DataListRelationObjItems { KeyObj = lceAdd.newItems.KeyObj, NameEntity = lceAdd.newItems.NameEntity, NameObj = lceAdd.newItems.NameObj });
                        //LinkControlEdit tc = LinkControlEdit.CreateLinkControlEdit(_selectRelationObjGridList[0], _passportModel, _metaData, _selectRelationObjGridList, false, true, false);

                        //spTextBlock.Children.Add(tc);
                        return;   
                    }
                }
            }
            else
            {

                //сначало убиваем кнопку - отобразить все
                foreach (var child2 in spTextBlock.Children)
                {
                    var tt = child2 as TextBlock;
                    if (tt != null)
                    {
                        spTextBlock.Children.Remove(tt);
                        break;
                    }
                }
                //добавляем 
                
                        _selectRelationObjGridList.Add(new DataListRelationObjItems { KeyObj = newItems.KeyObj, NameEntity = newItems.NameEntity, NameObj = newItems.NameObj });
                        int item = _selectRelationObjGridList.Count;
                        LinkControlEdit tc = LinkControlEdit.CreateLinkControlEdit(_selectRelationObjGridList[item - 1], _passportModel, _metaData, _selectRelationObjGridList, false, false, false);
                        spTextBlock.Children.Add(tc);

                        //создаем кнопку, если значений больше 4, остальные невидимые
                        if (_selectRelationObjGridList.Count >= 1)
                        {
                            for (int t1 = 0; t1 < spTextBlock.Children.Count(); t1++)
                            {
                                if (t1 > 1)
                                {
                                    spTextBlock.Children[t1].Visibility = Visibility.Collapsed;

                                }
                            }
                            //создаем кнопку если больше 4 (для тестировании больше 2)
                            if (_selectRelationObjGridList.Count > 2)
                            {
                                TextBlock tc2 = CreateTextBlockVisibleAlllink();
                                if (!_editPassport)
                                {
                                    //tc2.Margin = new Thickness(26, 4, 4, 4);
                                    tc2.Margin = new Thickness(26, 4, 0, 0);
                                }
                                else
                                {
                                    tc2.Margin = new Thickness(4,4,0,0);
                                }
                                spTextBlock.Children.Add(tc2);
                            }

                        }
            }

            //var child = spTextBlock.Children;
        }

        private void TextBlockControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            Model.PropertyChanged -= Model_PropertyChanged;
            Model.PropertyChanged += Model_PropertyChanged;
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //событие на удаление связи
            if (e.PropertyName.Equals("KeyLinkDelete"))
            {
               //удаление связи
                if (spTextBlock.Children.Count > 0)
                {

                    foreach (var child in spTextBlock.Children)
                    {
                        var lce = child as LinkControlEdit;
                           if (lce != null && lce.klickButton)
                           {
                               string _keyLinkOnDelete = lce.KeyLink;
                               //удаляем из spTextBlock
                               spTextBlock.Children.Remove(lce);
                               
                               //удаляем из коллекции
                               for (int ii = 0; ii < _selectRelationObjGridList.Count; ii++)
                               {
                                   if (_selectRelationObjGridList[ii].KeyObj == _keyLinkOnDelete)
                                   {
                                       _selectRelationObjGridList.RemoveAt(ii);

                                       if (_selectRelationObjGridList.Count > 2)
                                       {
                                           
                                       }
                                       else
                                       {
                                           //скрыть кнопку отобразить все
                                           for (int t1 = 0; t1 < spTextBlock.Children.Count(); t1++)
                                           {
                                               var tb = spTextBlock.Children[t1] as TextBlock;
                                               if (tb != null)
                                               {
                                                   spTextBlock.Children[t1].Visibility = Visibility.Collapsed;
                                               }
                                               else
                                               {
                                                   
                                                   spTextBlock.Children[t1].Visibility = Visibility.Visible;
                                               }
                                           }
                                       }


                                       return;
                                   }
                               }
                              
                           }
                    }
                   // foreach (LinkControlEdit LinkControlEdit in spTextBlock.Children)
                   

                }
            }
            //событие на добавление связи
            if (e.PropertyName.Equals("KeyLinkAdd"))
            {
                if (_selectRelationObjGridList.Count == 0)
                {
                    foreach (var child in spTextBlock.Children)
                    {
                        var lceAdd = child as LinkControlEdit;
                        if (lceAdd != null && lceAdd.newItems != null)
                        {
                            _selectRelationObjGridList.Add(new DataListRelationObjItems { KeyObj = lceAdd.newItems.KeyObj, NameEntity = lceAdd.newItems.NameEntity, NameObj = lceAdd.newItems.NameObj });
                            LinkControlEdit tc = LinkControlEdit.CreateLinkControlEdit(_selectRelationObjGridList[0], _passportModel, _metaData, _selectRelationObjGridList, false, true, false);

                            spTextBlock.Children.Clear();
                            spTextBlock.Children.Add(tc);
                            return;
                        }
                    }
                }
                else
                {

                    //сначало убиваем кнопку - отобразить все
                    foreach (var child2 in spTextBlock.Children)
                    {
                        var tt = child2 as TextBlock;
                        if (tt != null)
                        {
                            spTextBlock.Children.Remove(tt);
                            break;
                        }
                    }
                    //добавляем 
                    foreach (var child in spTextBlock.Children)
                    {

                        var lceAdd = child as LinkControlEdit;
                        if (lceAdd != null && lceAdd.newItems != null)
                        {
                            _selectRelationObjGridList.Add(new DataListRelationObjItems { KeyObj = lceAdd.newItems.KeyObj, NameEntity = lceAdd.newItems.NameEntity, NameObj = lceAdd.newItems.NameObj });
                            int item = _selectRelationObjGridList.Count;
                            LinkControlEdit tc = LinkControlEdit.CreateLinkControlEdit(_selectRelationObjGridList[item-1], _passportModel, _metaData, _selectRelationObjGridList, false, false, false);
                            spTextBlock.Children.Add(tc);
                           
                            //создаем кнопку, если значений больше 4, остальные невидимые
                            if (_selectRelationObjGridList.Count >= 1)
                            {
                                for (int t1 = 0; t1 < spTextBlock.Children.Count(); t1++)
                                {
                                    if (t1 > 1)
                                    {
                                        spTextBlock.Children[t1].Visibility = Visibility.Collapsed;

                                    }
                                }
                                //создаем кнопку если больше 4 (для тестировании больше 2)
                                if (_selectRelationObjGridList.Count > 2)
                                {
                                    TextBlock tc2 = CreateTextBlockVisibleAlllink();
                                    if (!_editPassport)
                                    {
                                        tc2.Margin = new Thickness(26, 4, 0, 0);
                                    }
                                    else
                                    {
                                        tc2.Margin = new Thickness(4,4,0,0);
                                    }
                                    spTextBlock.Children.Add(tc2);
                                }

                            }
                            
                            return;
                        }
                        
                    }
                    
                }
                    
                    //var child = spTextBlock.Children;
                   

               

            }
        }

        private string KeyLink()
        {
            string keyLink = "";
            if (_selectRelationObjGridList.Count > 0)
            {
                for (int ii = 0; ii < _selectRelationObjGridList.Count; ii++)
                {
                    keyLink = keyLink + _selectRelationObjGridList[ii].KeyObj + ",";
                }

                keyLink = keyLink.Substring(0, keyLink.Length - 1);
            }



            return keyLink;
        }


        #region IControlValueChanged Members

        public bool HasChanges
        {

            get
            {
                //bool nn = NeyKey();
                //return nn;
                return (_selectRelationObjGridList != null);
            }
        }

        public string NewValue
        {

            get
            {
                string keyLink = KeyLink();
                return keyLink;
            }
        }


        #endregion

        #region IControlValueChanged Members

        public FieldMetaDataItem MetaData
        {
            get { return _metaData; }
        }

        #endregion

    }
}
