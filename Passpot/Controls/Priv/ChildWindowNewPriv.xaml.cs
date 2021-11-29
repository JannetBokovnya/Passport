using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Media.Interfaces;
using Media.Resources;
using Passpot.Business;
using Passpot.Controls.Priv;
using Passpot.Model;
using Services.ServiceReference;

using Telerik.Windows;
using Telerik.Windows.Controls;

namespace Passpot.Controls
{
    public partial class ChildWindowNewPriv : ChildWindow
    {
        private ServiceDataClient _serviceDataClient;
        private List<DataMGList> SelectMGList;
        private List<DataNitList> SelectNitList;

        // private ChildWindowAttantion _popupWindowAttantion;
        private int value = 0;
        private string keyMg;
        private string keyNit;
        private string nameNit;
        private int index;
        private int CountPriv = 0; //количество привязок в таблице контрола добавления привязки
        private string separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        private int SelectIndexMg;
        private int SelectIndexPipe;
        private NavigationPriv NavigationPriv;
        //public List<NavigationPriv> NavigationPrivList = new List<NavigationPriv>();
        public SavePriv.LISTSAVEPRIV PrivItemsNew = new SavePriv.LISTSAVEPRIV();
        public List<PrivItems> ListprivItemsNew = new List<PrivItems>();
        private bool RblOnClick = false;
        private bool RbrOnClick = false;
        private string typePige = "0";
        private List<PrivItems> PrivItems = new List<PrivItems>();
        private List<PrivItems> ListPrivNewItems;
        private bool IsEdit = false;
        public string KeyPriv = "";
        private int fewPipe = 0; //если привязка к нескольким газопроводам - нумерация газопроводов
       // private PrivChildModel Model { get { return DataContext as PrivChildModel; } }
        private PrivChildModel Model { get; set; }
        private bool IsNextNewDataPipe = false;
        private bool updateItem = true;
        private string newUpdateItem = "2"; //1 - добавить новый 2 - редактировать 
        List<PrivItems> NavigationPrivListDouble = new List<PrivItems>();
        private PrivInGrid gridType10 = new PrivInGrid();
        private PrivChildModel privChildModel;
        //переменные для создания модели привязки
        private string key;
        private bool isEdited;
        private List<PrivItems> listPriwItems;
        private PassportDetailModel passportDetailModel;
        private string typeLink;
        //private PassportDetailModel Model
        //{
        //    get { return this.DataContext as PassportDetailModel; }
        //}

        protected ServiceDataClient ServiceDataClient
        {
            get
            {
                if (_serviceDataClient == null)
                    _serviceDataClient = new ServiceDataClient();
                return _serviceDataClient;
            }
        }

        public  ChildWindowNewPriv()
        {
            InitializeComponent();


            #region  //подключаем проверку на коректность данных - числа
            FieldMetaDataItem metaData = new FieldMetaDataItem();
            metaData.TYPEVALIDATION = 1;

            var numbermodel_tbKm = new NumberModel(metaData, null, tbKm.Text);
            numbermodel_tbKm.IsValidate = true;
            tbKm.DataContext = numbermodel_tbKm;

            var numbermodel_tbKmA = new NumberModel(metaData, null, tbKmA.Text);
            numbermodel_tbKmA.IsValidate = true;
            tbKmA.DataContext = numbermodel_tbKmA;

            var numbermodel_tbKmB = new NumberModel(metaData, null, tbKmB.Text);
            tbKmB.DataContext = numbermodel_tbKmB;
            numbermodel_tbKmB.IsValidate = true;

            var numbermodel_tbd = new NumberModel(metaData, null, tbd.Text);
            tbd.DataContext = numbermodel_tbd;
            numbermodel_tbd.IsValidate = true;

            var numbermodel_shirotA = new NumberModel(metaData, null, shirotA.Text);
            shirotA.DataContext = numbermodel_shirotA;
            numbermodel_shirotA.IsValidate = true;

            var numbermodel_shirot1 = new NumberModel(metaData, null, shirot1.Text);
            shirotA.DataContext = numbermodel_shirot1;
            numbermodel_shirot1.IsValidate = true;

            var numbermodel_shirotB = new NumberModel(metaData, null, shirotB.Text);
            shirotB.DataContext = numbermodel_shirotB;
            numbermodel_shirotB.IsValidate = true;

            var numbermodel_dolgA = new NumberModel(metaData, null, dolgA.Text);
            dolgA.DataContext = numbermodel_dolgA;
            numbermodel_dolgA.IsValidate = true;

            var numbermodel_dolgB = new NumberModel(metaData, null, dolgB.Text);
            dolgB.DataContext = numbermodel_dolgB;
            numbermodel_dolgB.IsValidate = true;

            var numbermodel_shirot = new NumberModel(metaData, null, shirot.Text);
            shirot.DataContext = numbermodel_shirot;
            numbermodel_shirot.IsValidate = true;

            var numbermodel_dolg = new NumberModel(metaData, null, dolg.Text);
            dolg.DataContext = numbermodel_dolg;
            numbermodel_dolg.IsValidate = true;

            #endregion

        }
       // public PrivChildModel(string key, bool isEdited, List<PrivItems> listPriwItems, PassportDetailModel passportDetailModel, string typeLink)
        public static ChildWindowNewPriv CreateFormpriv(string key, bool isEdited, List<PrivItems> listPriwItems, PassportDetailModel passportDetailModel, string typeLink)
        {
            var c = new ChildWindowNewPriv();

           c.key=key;
           c.isEdited = isEdited;
           c.listPriwItems = listPriwItems;
           c.passportDetailModel = passportDetailModel;
           c.typeLink = typeLink;

            c.privChildModel = new PrivChildModel(key, isEdited, listPriwItems, passportDetailModel, typeLink);
            c.Model = c.privChildModel;
            c.InitConstraint_Add(c.isEdited);
            return c;
        }

        public List<NavigationPriv> GetDataPrivEdit(List<PrivItems> privItems)
        {
            List<NavigationPriv> _navigationPrivEdit = new List<NavigationPriv>();
            int keyMtInt;
            int keyPipeInt;


            for (int ii = 0; ii < PrivItems.Count; ii++)
            {
                int i1 = ii + 1;

                int.TryParse(PrivItems[ii].nMtKey, out keyMtInt);
                int.TryParse(PrivItems[ii].nPipeKey, out keyPipeInt);

                _navigationPrivEdit.Add(new NavigationPriv(i1.ToString(), PrivItems[ii].NKMORT1, PrivItems[ii].NDISTANCEBEG, PrivItems[ii].NX1,
                    PrivItems[ii].NY1, PrivItems[ii].NKEY, PrivItems[ii].NH1, PrivItems[ii].NKMTRUE1, PrivItems[ii].NX2, PrivItems[ii].NY2,
                    PrivItems[ii].NKMORT2, PrivItems[ii].NKMTRUE2, PrivItems[ii].NDISTANCEEND, keyMtInt, keyPipeInt, PrivItems[ii].NZ2,
                    PrivItems[ii].NZ1, PrivItems[ii].NH2, PrivItems[ii].NBUILDTYPE, PrivItems[ii].ISEDITED));

            }

            
            return _navigationPrivEdit;

        }

        public void InitConstraint_Add(bool isEdit)
        {
            IsEdit = isEdit;

            if (Model.PassportDetailModel.TYPEPRIV == "10")
            {
               // CreateWindowNewPriv(Model.NavigationPrivList);
                mainData.RowDefinitions[0].Height = new GridLength(0);
                mainData.RowDefinitions[1].Height = new GridLength(0);
                mainData.RowDefinitions[2].Height = new GridLength(0);
                mainData.RowDefinitions[3].Height = new GridLength(0);
                mainData.RowDefinitions[4].Height = new GridLength(0);
                mainData.RowDefinitions[5].Height = new GridLength(0);
                mainData.RowDefinitions[6].Height = new GridLength(0);

                mainData.RowDefinitions[9].Height = new GridLength(0);
                radExpander.IsExpanded = true;

                expander.RowDefinitions[0].Height = new GridLength(0);
                expander.RowDefinitions[1].Height = new GridLength(0);
                expander.RowDefinitions[2].Height = new GridLength(0);
                expander.RowDefinitions[3].Height = new GridLength(0);
                Priv10 priv10 = new Priv10();
                pictureTypePriv.Children.Add(priv10);
            }
            else
            {
                InitDDL_MG();    
            }
            
        }

        //получаем данные по газопроводу
        private void InitDDL_MG()
        {
            ServiceDataClient.GetDataMGListCompleted += ServiceDataClient_GetDataMGListCompleted;
            ServiceDataClient.GetDataMGListAsync(GlobalContext.Context);

        }

        //получаем данные по газопроводу из базы, строим привязку исходя из ее типа
        void ServiceDataClient_GetDataMGListCompleted(object sender, GetDataMGListCompletedEventArgs e)
        {
            ServiceDataClient.GetDataMGListCompleted -= ServiceDataClient_GetDataMGListCompleted;
            if (e.Result.IsValid)
            {
                SelectMGList = new List<DataMGList>();

                for (int ii = 0; ii < e.Result.DataMGLists.Count; ii++)
                {
                    SelectMGList.Add(new DataMGList { KEYMG = e.Result.DataMGLists[ii].KEYMG, NAMEMG = e.Result.DataMGLists[ii].NAMEMG });
                }

                ddlMG.ItemsSource = SelectMGList;
                keyNit = "";
                CreateWindowNewPriv(Model.NavigationPrivList);
            }
            else
            {
                Model.PassportDetailModel.MainModel.Report("Список MG err = " + e.Result.ErrorMessage);
                
            }
        }

        // строим привязку исходя из ее типа
        private void CreateWindowNewPriv(List<NavigationPriv> NavigationPrivList)
        {
            switch (Model.PassportDetailModel.TYPEPRIV)
            {
                case "1":
                    {

                        CreatePriv1Type(NavigationPrivList);
                        break;
                    }
                case "2":
                    {
                        CreatePriv2Type(NavigationPrivList);
                        break;
                    }
                case "3":
                    {
                        CreatePriv3Type(NavigationPrivList);
                        break;
                    }
                case "4":
                    {

                        CreatePriv4Type(NavigationPrivList);
                        break;
                    }
                case "5":
                    {
                        CreatePriv5Type();
                        break;
                    }

                case "6":
                    {
                        CreatePriv6Type(NavigationPrivList);
                        break;
                    }
                case "7":
                    {
                        CreatePriv7Type(NavigationPrivList);
                        break;
                    }
                case "8":
                    {

                        CreatePriv8Type(NavigationPrivList);
                        break;
                    }
                case "9":
                    {
                        CreatePriv9Type(NavigationPrivList);
                        break;
                    }
                case "10":
                    {
                        CreatePriv10Type(NavigationPrivList);
                        break;
                    }

                default:
                    {
                        break;
                    }
            }

            //if (Model.PassportDetailModel.TYPEPRIV == "7")
            //{
            //    return;
            //}

            if (Model.PassportDetailModel.TYPEPRIV != "10")
            {
                
                if (NavigationPrivList.Count > 0)
                {
                    
                    SelectMgOnFewPipe(Model.NavigationPrivList[0], SelectMGList);
                }
                else
                {
                    IsNextNewDataPipe = false;
                    ddlMG.SelectedIndex = ddlMG.Items.IndexOf(SelectMGList[0]);
                   
                }    
            }
            

        }

        #region Создание привязок разных типов

        private void CreatePriv1Type(List<NavigationPriv> NavigationPrivList)
        {
            LayoutRoot.RowDefinitions[4].Height = new GridLength(0);
            nameTypePrivP1.Text = ProjectResources.Priv1P1;
            nameTypePrivP2.Text = ProjectResources.Priv1P2;
            mainData.RowDefinitions[3].Height = new GridLength(0);
            mainData.RowDefinitions[4].Height = new GridLength(0);
            mainData.RowDefinitions[5].Height = new GridLength(0);
            mainData.RowDefinitions[6].Height = new GridLength(0);
            mainData.RowDefinitions[7].Height = new GridLength(0);
            mainData.RowDefinitions[8].Height = new GridLength(0);


            expander.RowDefinitions[0].Height = new GridLength(0);
            expander.RowDefinitions[1].Height = new GridLength(0);
            expander.RowDefinitions[2].Height = new GridLength(0);
            expander.RowDefinitions[3].Height = new GridLength(0);

            //если редактируем привязку

            if (Model.NavigationPrivList.Count > 0)
            {
                tbKm.setMode(false);
                tbKm.displayWatermark = false;
                tbKm.Text = NavigationPrivList[0].NKMORT1;
                shirot.setMode(false);
                shirot.displayWatermark = false;
                shirot.Text = NavigationPrivList[0].NY1;
                dolg.setMode(false);
                dolg.displayWatermark = false;
                dolg.Text = NavigationPrivList[0].NX1;

                SelectMgOnFewPipe(NavigationPrivList[0], SelectMGList);
            }
            TabInPriv10 = null;
            Priv1 priv1 = new Priv1();
            pictureTypePriv.Children.Add(priv1);
        }

        private void CreatePriv2Type(List<NavigationPriv> NavigationPrivList)
        {
            //не показываем кнопки перехода назад вперед
            LayoutRoot.RowDefinitions[4].Height = new GridLength(0);

            nameTypePrivP1.Text = ProjectResources.Priv1P1;
            nameTypePrivP2.Text = ProjectResources.Priv2P2;

            mainData.RowDefinitions[3].Height = new GridLength(0);
            mainData.RowDefinitions[4].Height = new GridLength(0);
            mainData.RowDefinitions[7].Height = new GridLength(0);
            mainData.RowDefinitions[8].Height = new GridLength(0);

            expander.RowDefinitions[0].Height = new GridLength(0);
            expander.RowDefinitions[1].Height = new GridLength(0);
            expander.RowDefinitions[2].Height = new GridLength(0);
            expander.RowDefinitions[3].Height = new GridLength(0);

            Priv2 priv2 = new Priv2();
            pictureTypePriv.Children.Add(priv2);

            if (NavigationPrivList.Count > 0)
            {
                tbKm.setMode(false);
                tbKm.displayWatermark = false;
                tbKm.Text = NavigationPrivList[0].NKMORT1;
                tbd.setMode(false);
                tbd.displayWatermark = false;

                if (NavigationPrivList[0].NDISTANCEBEG.Contains("-"))
                {
                    tbd.Text = NavigationPrivList[0].NDISTANCEBEG.Substring(1);
                    rbr.IsChecked = true;
                }
                else
                {
                    tbd.Text = NavigationPrivList[0].NDISTANCEBEG;
                    rbl.IsChecked = true;
                }

                shirot.setMode(false);
                shirot.displayWatermark = false;
                shirot.Text = NavigationPrivList[0].NY1;
                dolg.setMode(false);
                dolg.displayWatermark = false;
                dolg.Text = NavigationPrivList[0].NX1;
            }
        }

        private void CreatePriv3Type(List<NavigationPriv> NavigationPrivList)
        {
            LayoutRoot.RowDefinitions[4].Height = new GridLength(0);

            nameTypePrivP1.Text = ProjectResources.Priv3P1;
            nameTypePrivP2.Text = ProjectResources.Priv1P2;

            mainData.RowDefinitions[2].Height = new GridLength(0);
            mainData.RowDefinitions[5].Height = new GridLength(0);
            mainData.RowDefinitions[6].Height = new GridLength(0);
            mainData.RowDefinitions[7].Height = new GridLength(0);
            mainData.RowDefinitions[8].Height = new GridLength(0);

            expander.RowDefinitions[4].Height = new GridLength(0);
            expander.RowDefinitions[5].Height = new GridLength(0);

            if (NavigationPrivList.Count > 0)
            {
                tbKmA.setMode(false);
                tbKmA.displayWatermark = false;
                tbKmA.Text = NavigationPrivList[0].NKMORT1;

                tbKmB.setMode(false);
                tbKmB.displayWatermark = false;
                tbKmB.Text = NavigationPrivList[0].NKMORT2;

                shirotA.setMode(false);
                shirotA.displayWatermark = false;
                shirotA.Text = NavigationPrivList[0].NY1;
                dolgA.setMode(false);
                dolgA.displayWatermark = false;
                dolgA.Text = NavigationPrivList[0].NX1;

                shirotB.setMode(false);
                shirotB.displayWatermark = false;
                shirotB.Text = NavigationPrivList[0].NY2;
                dolgB.setMode(false);
                dolgB.displayWatermark = false;
                dolgB.Text = NavigationPrivList[0].NX2;
            }

            Priv3 priv3 = new Priv3();
            pictureTypePriv.Children.Add(priv3);
        }

        private void CreatePriv4Type(List<NavigationPriv> NavigationPrivList)
        {
            LayoutRoot.RowDefinitions[4].Height = new GridLength(0);
            nameTypePrivP1.Text = ProjectResources.Priv3P1;
            nameTypePrivP2.Text = ProjectResources.Priv2P2;

            mainData.RowDefinitions[2].Height = new GridLength(0);
            expander.RowDefinitions[4].Height = new GridLength(0);
            expander.RowDefinitions[5].Height = new GridLength(0);
            mainData.RowDefinitions[7].Height = new GridLength(0);
            mainData.RowDefinitions[8].Height = new GridLength(0);

            if (NavigationPrivList.Count > 0)
            {
                tbKmA.setMode(false);
                tbKmA.displayWatermark = false;
                tbKmA.Text = NavigationPrivList[0].NKMORT1;

                tbKmB.setMode(false);
                tbKmB.displayWatermark = false;
                tbKmB.Text = NavigationPrivList[0].NKMORT2;

                if (NavigationPrivList[0].NDISTANCEBEG.Contains("-"))
                {
                    // tbd.Text = NavigationPrivList[0].NDISTANCEBEG.Substring(1);
                    rbr.IsChecked = true;
                }
                else
                {
                    //tbd.Text = NavigationPrivList[0].NDISTANCEBEG;
                    rbl.IsChecked = true;

                }

                shirotA.setMode(false);
                shirotA.displayWatermark = false;
                shirotA.Text = NavigationPrivList[0].NY1;
                dolgA.setMode(false);
                dolgA.displayWatermark = false;
                dolgA.Text = NavigationPrivList[0].NX1;

                shirotB.setMode(false);
                shirotB.displayWatermark = false;
                shirotB.Text = NavigationPrivList[0].NY2;
                dolgB.setMode(false);
                dolgB.displayWatermark = false;
                dolgB.Text = NavigationPrivList[0].NX2;
            }



            Priv4 priv4 = new Priv4();
            pictureTypePriv.Children.Add(priv4);
        }

        private void CreatePriv5Type()
        {

            nameTypePrivP1.Text = ProjectResources.Priv1P1;
            nameTypePrivP2.Text = ProjectResources.Priv5P2;

            mainData.RowDefinitions[2].Height = new GridLength(0);
            mainData.RowDefinitions[4].Height = new GridLength(0);
            mainData.RowDefinitions[7].Height = new GridLength(0);
            mainData.RowDefinitions[8].Height = new GridLength(0);

            expander.RowDefinitions[0].Height = new GridLength(0);
            expander.RowDefinitions[1].Height = new GridLength(0);
            expander.RowDefinitions[2].Height = new GridLength(0);
            expander.RowDefinitions[3].Height = new GridLength(0);

            if (Model.NavigationPrivList.Count > 0)
            {
                InitPriv5Type(0);
            }


            Priv5 priv5 = new Priv5();
            pictureTypePriv.Children.Add(priv5);
        }

        private void InitPriv5Type(int indexPrivList)
        {
            tbKmA.setMode(false);
            tbKmA.displayWatermark = false;
            tbKmA.Text = Model.NavigationPrivList[indexPrivList].NKMORT1;
            tbd.setMode(false);
            tbd.displayWatermark = false;


            if (Model.NavigationPrivList[indexPrivList].NDISTANCEBEG.Contains("-"))
            {
                tbd.Text = Model.NavigationPrivList[indexPrivList].NDISTANCEBEG.Substring(1);
                rbr.IsChecked = true;
            }
            else
            {
                tbd.Text = Model.NavigationPrivList[indexPrivList].NDISTANCEBEG;
                rbl.IsChecked = true;
            }

            shirot.setMode(false);
            shirot.displayWatermark = false;
            shirot.Text = Model.NavigationPrivList[indexPrivList].NY1;
            dolg.setMode(false);
            dolg.displayWatermark = false;
            dolg.Text = Model.NavigationPrivList[indexPrivList].NX1;
        }

        private void CreatePriv6Type(List<NavigationPriv> NavigationPrivList)
        {
            nameTypePrivP1.Text = ProjectResources.Priv3P1;
            nameTypePrivP2.Text = ProjectResources.Priv5P2;

            tbmgt.Text = ProjectResources.PtivTitleBoxM + " 1";
            tbpipet.Text = ProjectResources.PrivTitleBoxN + " 1";

            // LayoutRoot.RowDefinitions[4].Height = new GridLength(0);
            mainData.RowDefinitions[2].Height = new GridLength(0);
            mainData.RowDefinitions[7].Height = new GridLength(0);
            mainData.RowDefinitions[8].Height = new GridLength(0);

            expander.RowDefinitions[4].Height = new GridLength(0);
            expander.RowDefinitions[5].Height = new GridLength(0);


            if (Model.NavigationPrivList.Count > 0)
            {
                InitPriv6Type(0);
            }


            Priv6 priv6 = new Priv6();
            pictureTypePriv.Children.Add(priv6);
        }

        private void InitPriv6Type(int indexPrivList)
        {
            tbmgt.Text = ProjectResources.PtivTitleBoxM + (indexPrivList + 1).ToString();
            tbpipet.Text = ProjectResources.PrivTitleBoxN + (indexPrivList + 1).ToString();

            tbKmA.setMode(false);
            tbKmA.displayWatermark = false;
            tbKmA.Text = Model.NavigationPrivList[indexPrivList].NKMORT1;

            tbKmB.setMode(false);
            tbKmB.displayWatermark = false;
            tbKmB.Text = Model.NavigationPrivList[indexPrivList].NKMORT2;

            tbd.setMode(false);
            tbd.displayWatermark = false;


            if (Model.NavigationPrivList[indexPrivList].NDISTANCEBEG.Contains("-"))
            {
                tbd.Text = Model.NavigationPrivList[indexPrivList].NDISTANCEBEG.Substring(1);
                rbr.IsChecked = true;
            }
            else
            {
                tbd.Text = Model.NavigationPrivList[indexPrivList].NDISTANCEBEG;
                rbl.IsChecked = true;
            }

            shirotA.setMode(false);
            shirotA.displayWatermark = false;
            shirotA.Text = Model.NavigationPrivList[indexPrivList].NY1;
            dolgA.setMode(false);
            dolgA.displayWatermark = false;
            dolgA.Text = Model.NavigationPrivList[indexPrivList].NX1;


            shirotB.setMode(false);
            shirotB.displayWatermark = false;
            shirotB.Text = Model.NavigationPrivList[indexPrivList].NY2;
            dolgB.setMode(false);
            dolgB.displayWatermark = false;
            dolgB.Text = Model.NavigationPrivList[indexPrivList].NX2;


        }

        private void CreatePriv7Type(List<NavigationPriv> NavigationPrivList)
        {
            LayoutRoot.RowDefinitions[4].Height = new GridLength(0);
            nameTypePrivP1.Text = ProjectResources.Priv1P1;
            nameTypePrivP2.Text = ProjectResources.Priv7P2;
            tbshirotT1.Text = ProjectResources.PrivShirot1 + ProjectResources.Priv9Gr;
            tbdolgT1.Text = ProjectResources.PrivDolg1 + ProjectResources.Priv9Gr;


            mainData.RowDefinitions[0].Height = new GridLength(0);
            mainData.RowDefinitions[1].Height = new GridLength(0);
            mainData.RowDefinitions[2].Height = new GridLength(0);
            mainData.RowDefinitions[3].Height = new GridLength(0);
            mainData.RowDefinitions[4].Height = new GridLength(0);
            mainData.RowDefinitions[5].Height = new GridLength(0);
            mainData.RowDefinitions[6].Height = new GridLength(0);

            mainData.RowDefinitions[7].Height = new GridLength(0);
            mainData.RowDefinitions[8].Height = new GridLength(0);
            radExpander.IsExpanded = true;

            expander.RowDefinitions[2].Height = new GridLength(0);
            expander.RowDefinitions[3].Height = new GridLength(0);
            expander.RowDefinitions[4].Height = new GridLength(0);
            expander.RowDefinitions[5].Height = new GridLength(0);

           

            if (NavigationPrivList.Count > 0)
            {

                shirotA.setMode(false);
                shirotA.displayWatermark = false;
                shirotA.Text = NavigationPrivList[0].NY1;
                dolgA.setMode(false);
                dolgA.displayWatermark = false;
                dolgA.Text = NavigationPrivList[0].NX1;

            }
           
            Priv7 priv7 = new Priv7();
            pictureTypePriv.Children.Clear();
            pictureTypePriv.Children.Add(priv7);
        }

        private void CreatePriv8Type(List<NavigationPriv> NavigationPrivList)
        {
            nameTypePrivP1.Text = ProjectResources.Priv1P1;
            nameTypePrivP2.Text = ProjectResources.Priv8P2;

            mainData.RowDefinitions[3].Height = new GridLength(0);
            mainData.RowDefinitions[4].Height = new GridLength(0);
            mainData.RowDefinitions[5].Height = new GridLength(0);
            mainData.RowDefinitions[6].Height = new GridLength(0);
            mainData.RowDefinitions[7].Height = new GridLength(0);
            mainData.RowDefinitions[8].Height = new GridLength(0);

            expander.RowDefinitions[0].Height = new GridLength(0);
            expander.RowDefinitions[1].Height = new GridLength(0);
            expander.RowDefinitions[2].Height = new GridLength(0);
            expander.RowDefinitions[3].Height = new GridLength(0);

            if (Model.NavigationPrivList.Count > 0)
            {
                InitPriv8Type(0);
            }
            else
            {
                tbmgt.Text = ProjectResources.PtivTitleBoxM + " 1";
                tbpipet.Text = ProjectResources.PrivTitleBoxN + " 1";
                tbKmT.Text = ProjectResources.PrivKmA;
                tbshirotT.Text = ProjectResources.PrivShirotA;
                tbdolgT.Text = ProjectResources.PrivDolgA;
            }


            Priv8 priv8 = new Priv8();
            pictureTypePriv.Children.Add(priv8);
        }

        private void InitPriv8Type(int indexPrivList)
        {

            if (indexPrivList ==-1)
            {
                tbmgt.Text = ProjectResources.PtivTitleBoxM + " 1";
                tbpipet.Text = ProjectResources.PrivTitleBoxN + " 1";
                tbKmT.Text = ProjectResources.PrivKmA;
                tbshirotT.Text = ProjectResources.PrivShirotA;
                tbdolgT.Text = ProjectResources.PrivDolgA;
            }
            else
            {
                tbmgt.Text = ProjectResources.PtivTitleBoxM + " " + (indexPrivList + 1).ToString();
                tbpipet.Text = ProjectResources.PrivTitleBoxN + " " + (indexPrivList + 1).ToString();

                if (indexPrivList == 0)
                {

                    tbKmT.Text = ProjectResources.PrivKmA;
                    tbKm.setMode(false);
                    tbKm.displayWatermark = false;
                    tbKm.Text = Model.NavigationPrivList[indexPrivList].NKMORT1;

                    tbshirotT.Text = ProjectResources.PrivShirotA;
                    shirot.setMode(false);
                    shirot.displayWatermark = false;
                    shirot.Text = Model.NavigationPrivList[indexPrivList].NY1;

                    tbdolgT.Text = ProjectResources.PrivDolgA;
                    dolg.setMode(false);
                    dolg.displayWatermark = false;
                    dolg.Text = Model.NavigationPrivList[indexPrivList].NX1;
                }
                else
                    if (indexPrivList == -1)
                    {
                        tbmgt.Text = ProjectResources.PtivTitleBoxM + " 1";
                        tbpipet.Text = ProjectResources.PrivTitleBoxN + " 1";
                        tbKmT.Text = ProjectResources.PrivKmA;
                        tbshirotT.Text = ProjectResources.PrivShirotA;
                        tbdolgT.Text = ProjectResources.PrivDolgA;
                    }
                    else
                    {

                        //tbmgt.Text = ProjectResources.PtivTitleBoxM + " " + (indexPrivList + 1).ToString();
                        //tbpipet.Text = ProjectResources.PrivTitleBoxN + " " + (indexPrivList + 1).ToString();

                        tbKmT.Text = ProjectResources.PrivKmB;
                        tbKm.displayWatermark = false;
                        tbKm.setMode(false);
                        tbKm.Text = Model.NavigationPrivList[indexPrivList].NKMORT1;


                        tbshirotT.Text = ProjectResources.PrivShirotB;
                        shirot.setMode(false);
                        shirot.displayWatermark = false;
                        shirot.Text = Model.NavigationPrivList[indexPrivList].NY1;
                        tbdolgT.Text = ProjectResources.PrivDolgB;
                        dolg.setMode(false);
                        dolg.displayWatermark = false;
                        dolg.Text = Model.NavigationPrivList[indexPrivList].NX1;

                    }
            }

        }

        private void CreatePriv9Type(List<NavigationPriv> NavigationPrivList)
        {
            nameTypePrivP1.Text = ProjectResources.Priv3P1;
            nameTypePrivP2.Text = ProjectResources.Priv9P2;

            mainData.RowDefinitions[2].Height = new GridLength(0);
            mainData.RowDefinitions[5].Height = new GridLength(0);
            mainData.RowDefinitions[6].Height = new GridLength(0);
            mainData.RowDefinitions[7].Height = new GridLength(0);
            mainData.RowDefinitions[8].Height = new GridLength(0);

            expander.RowDefinitions[4].Height = new GridLength(0);
            expander.RowDefinitions[5].Height = new GridLength(0);


            if (Model.NavigationPrivList.Count > 0)
            {
                InitPriv9Type(0);
            }
            else
            {
                tbmgt.Text = ProjectResources.PtivTitleBoxM + " " + "1";
                tbpipet.Text = ProjectResources.PrivTitleBoxN + " " + "1";
                tbKmAT.Text = ProjectResources.PrivKmA9 + "1" + ",  " + ProjectResources.PrivKm9m;
                tbKmBT.Text = ProjectResources.PrivKmB9 + "1" + ",  " + ProjectResources.PrivKm9m;
                tbshirotAT.Text = ProjectResources.PrivShirotA1 + "1" + ProjectResources.Priv9Gr;
                tbdolgAT.Text = ProjectResources.PrivDolgA1 + "1" + ProjectResources.Priv9Gr;
                tbshirotBT.Text = ProjectResources.PrivShirotB1 + "1" + ProjectResources.Priv9Gr;
                tbdolgBT.Text = ProjectResources.PrivDolgB1 + "1" + ProjectResources.Priv9Gr;
            }



            Priv9 priv9 = new Priv9();
            pictureTypePriv.Children.Add(priv9);
        }

        private void InitPriv9Type(int indexPrivList)
        {
            tbmgt.Text = ProjectResources.PtivTitleBoxM + " " + (indexPrivList + 1).ToString();
            tbpipet.Text = ProjectResources.PrivTitleBoxN + " " + (indexPrivList + 1).ToString();


            tbKmAT.Text = ProjectResources.PrivKmA9 + (indexPrivList + 1).ToString() + ",  " + ProjectResources.PrivKm9m;
            tbKmA.setMode(false);
            tbKmA.displayWatermark = false;
            tbKmA.Text = Model.NavigationPrivList[indexPrivList].NKMORT1;

            tbKmBT.Text = ProjectResources.PrivKmB9 + (indexPrivList + 1).ToString() + ",  " + ProjectResources.PrivKm9m;
            tbKmB.setMode(false);
            tbKmB.displayWatermark = false;
            tbKmB.Text = Model.NavigationPrivList[indexPrivList].NKMORT2;

            tbshirotAT.Text = ProjectResources.PrivShirotA1 + (indexPrivList + 1).ToString() + ProjectResources.Priv9Gr;
            shirotA.setMode(false);
            shirotA.displayWatermark = false;
            if (Model.NavigationPrivList[indexPrivList].NY1 != null)
            {
                shirotA.Text = Model.NavigationPrivList[indexPrivList].NY1;   
            }
            
            tbdolgAT.Text = ProjectResources.PrivDolgA1 + (indexPrivList + 1).ToString() + ProjectResources.Priv9Gr;
            dolgA.setMode(false);
            dolgA.displayWatermark = false;
            if (Model.NavigationPrivList[indexPrivList].NX1 != null)
            {
                dolgA.Text = Model.NavigationPrivList[indexPrivList].NX1;    
            }
            

            tbshirotBT.Text = ProjectResources.PrivShirotB1 + (indexPrivList + 1).ToString() + ProjectResources.Priv9Gr;
            shirotB.setMode(false);
            shirotB.displayWatermark = false;
            if (Model.NavigationPrivList[indexPrivList].NY2 != null)
            {
                shirotB.Text = Model.NavigationPrivList[indexPrivList].NY2;    
            }
            
            tbdolgBT.Text = ProjectResources.PrivDolgB1 + (indexPrivList + 1).ToString() + ProjectResources.Priv9Gr;
            dolgB.setMode(false);
            dolgB.displayWatermark = false;
            if (Model.NavigationPrivList[indexPrivList].NX2 != null)
            {
                dolgB.Text = Model.NavigationPrivList[indexPrivList].NX2;    
            }
            

        }

        private void CreatePriv10Type(List<NavigationPriv> NavigationPrivList)
        {
            nameTypePrivP1.Text = ProjectResources.Priv10P1;
            nameTypePrivP2.Text = ProjectResources.Priv10P2;

            mainData.RowDefinitions[0].Height = new GridLength(0);
            mainData.RowDefinitions[1].Height = new GridLength(0);
            mainData.RowDefinitions[2].Height = new GridLength(0);
            mainData.RowDefinitions[3].Height = new GridLength(0);
            mainData.RowDefinitions[4].Height = new GridLength(0);
            mainData.RowDefinitions[5].Height = new GridLength(0);
            mainData.RowDefinitions[6].Height = new GridLength(0);

            mainData.RowDefinitions[9].Height = new GridLength(0);
            radExpander.IsExpanded = true;

            expander.RowDefinitions[0].Height = new GridLength(0);
            expander.RowDefinitions[1].Height = new GridLength(0);
            expander.RowDefinitions[2].Height = new GridLength(0);
            expander.RowDefinitions[3].Height = new GridLength(0);

            

            if (Model.NavigationPrivList.Count > 0)
            {
                shirot1.setMode(false);
                shirot1.displayWatermark = false;
                shirot1.Text = "";

                dolg1.setMode(false);
                dolg1.displayWatermark = false;
                dolg1.Text = "";
                InitPriv10Type(0);
            }
            else
            {

                tbshirotT1.Text = ProjectResources.PrivShirot1 + "1" + ProjectResources.Priv9Gr;
                tbdolgT1.Text = ProjectResources.PrivDolg1 + "1" + ProjectResources.Priv9Gr;

            }

            //Priv10 priv10 = new Priv10();
            //pictureTypePriv.Children.Add(priv10);
        }

        private void InitPriv10Type(int indexPrivList)
        {


            tbshirotT1.Text = ProjectResources.PrivShirot1 + (indexPrivList + 1).ToString() + ProjectResources.Priv9Gr;
            shirot1.setMode(false);
            shirot1.displayWatermark = false;
            shirot1.Text = Model.NavigationPrivList[indexPrivList].NY1;
            tbdolgT1.Text = ProjectResources.PrivDolg1 + (indexPrivList + 1).ToString() + ProjectResources.Priv9Gr;
            dolg1.setMode(false);
            dolg1.displayWatermark = false;
            dolg1.Text = Model.NavigationPrivList[indexPrivList].NX1;

        }

        #endregion создание привязок разных типов

        //выбираем мг - если редактируем привязку (записи из массива данных для редактирования NavigationPrivList)
        private void SelectMgOnFewPipe(NavigationPriv NavigationPriv, List<DataMGList> SelectMGList)
        {
            for (int i = 0; i < SelectMGList.Count; i++)
            {
                DataMGList dn = SelectMGList[i];
                if (dn.KEYMG == NavigationPriv.nMtKey.ToString())
                {
                    ddlMG.SelectedIndex = ddlMG.Items.IndexOf(SelectMGList[i]);
                    break;
                }
            }
        }

        //выбираем нитку - если редактируем привязку (записи из массива данных для редактирования NavigationPrivList)
        private void SelectPipeOnFewPipe(NavigationPriv NavigationPriv, List<DataNitList> SelectNitList)
        {
            for (int i = 0; i < SelectNitList.Count; i++)
            {
                DataNitList dn = SelectNitList[i];
                if (dn.KEYNIT == NavigationPriv.nPipeKey.ToString())
                {
                    ddlNitka.SelectedIndex = ddlNitka.Items.IndexOf(SelectNitList[i]);
                    break;
                }
            }
        }

        private void ddlMG_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            keyNit = "";
            if ((ddlMG.SelectedItem != null))
            {
                keyMg = ((DataMGList)ddlMG.SelectedItem).KEYMG;
                ServiceDataClient.GetDataNitListCompleted += ServiceDataClient_GetDataNitListCompleted;
                ServiceDataClient.GetDataNitListAsync(keyMg, GlobalContext.Context);
                //SelectIndexMg = ddlMG.SelectedIndex;
                int keyMgInt;
                int.TryParse(keyMg, out keyMgInt);
                SelectIndexMg = keyMgInt;
            }
            else
            {
                ddlNitka.ItemsSource = null;
            }

        }

        void ServiceDataClient_GetDataNitListCompleted(object sender, GetDataNitListCompletedEventArgs e)
        {
            ServiceDataClient.GetDataNitListCompleted -= ServiceDataClient_GetDataNitListCompleted;
            if (e.Result.IsValid)
            {
                SelectNitList = new List<DataNitList>(e.Result.DataNitLists);
                ddlNitka.ItemsSource = SelectNitList;
                if (!IsNextNewDataPipe)
                {
                    int index = Model.CurrentIndex;
                    if (index != -1)
                    {
                        SelectPipeOnFewPipe(Model.NavigationPrivList[index], SelectNitList);
                    }
                }
            }
            else
            {
                Model.PassportDetailModel.MainModel.Report("Список Nit err = " + e.Result.ErrorMessage);
                //Model.MainModel.Report();
            }
        }

        private void ddlNitka_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            if ((ddlNitka.SelectedItem != null))
            {
                keyNit = ((DataNitList)ddlNitka.SelectedItem).KEYNIT;
                nameNit = ((DataNitList)ddlNitka.SelectedItem).NAMENIT;
                int keyNitInt;
                int.TryParse(keyNit, out keyNitInt);
                SelectIndexPipe = keyNitInt;
            }
        }

        private void RadExpander_OnExpanded(object sender, RadRoutedEventArgs e)
        {
            switch (Model.PassportDetailModel.TYPEPRIV)
            {
                case "1":
                    {
                        tbKmT.Foreground = new SolidColorBrush(Colors.DarkGray);
                        tbKm.IsEnabled = false;
                        break;
                    }
                case "2":
                    {
                        tbKmT.Foreground = rbT.Foreground = tbdT.Foreground = new SolidColorBrush(Colors.DarkGray);
                        tbKm.IsEnabled = tbd.IsEnabled = false;
                        rbl.IsEnabled = rbr.IsEnabled = false;

                        break;
                    }
                case "3":
                    {
                        tbKmAT.Foreground = tbKmBT.Foreground = new SolidColorBrush(Colors.DarkGray);
                        tbKmA.IsEnabled = tbKmB.IsEnabled = false;

                        break;
                    }
                case "4":
                    {
                        tbKmAT.Foreground = tbKmBT.Foreground = rbT.Foreground = tbdT.Foreground = new SolidColorBrush(Colors.DarkGray);
                        tbKmA.IsEnabled = tbKmB.IsEnabled = tbd.IsEnabled = false;
                        rbl.IsEnabled = rbr.IsEnabled = false;

                        break;
                    }

                case "5":
                    {
                        tbKmAT.Foreground = rbT.Foreground = tbdT.Foreground = new SolidColorBrush(Colors.DarkGray);
                        tbKmA.IsEnabled = tbd.IsEnabled = false;
                        rbl.IsEnabled = rbr.IsEnabled = false;

                        break;
                    }

                case "6":
                    {
                        tbKmAT.Foreground = tbKmBT.Foreground = tbdT.Foreground = rbT.Foreground = new SolidColorBrush(Colors.DarkGray);
                        tbKmA.IsEnabled = tbKmB.IsEnabled = tbd.IsEnabled = false;
                        rbl.IsEnabled = rbr.IsEnabled = false;

                        break;
                    }

                case "8":
                    {
                        tbKmT.Foreground = new SolidColorBrush(Colors.DarkGray);
                        tbKm.IsEnabled = false;

                        break;
                    }
                case "9":
                    {
                        tbKmAT.Foreground = tbKmBT.Foreground = new SolidColorBrush(Colors.DarkGray);
                        tbKmA.IsEnabled = tbKmB.IsEnabled = false;
                        tbKmBT.Foreground = tbKmBT.Foreground = new SolidColorBrush(Colors.DarkGray);
                        tbKmB.IsEnabled = tbKmB.IsEnabled = false;

                        break;
                    }

                default: break;
            }

        }

        private void RadExpander_OnCollapsed(object sender, RadRoutedEventArgs e)
        {
            switch (Model.PassportDetailModel.TYPEPRIV)
            {
                case "1":
                    {
                        tbKm.Foreground = tbKmT.Foreground = new SolidColorBrush(Colors.Black);
                        tbKm.IsEnabled = true;
                        break;
                    }
                case "2":
                    {
                        tbKm.Foreground = tbKmT.Foreground = rbT.Foreground = tbdT.Foreground = new SolidColorBrush(Colors.Black);
                        tbKm.IsEnabled = tbd.IsEnabled = true;
                        rbl.IsEnabled = rbr.IsEnabled = true;
                        break;
                    }
                case "3":
                    {
                        tbKmAT.Foreground = tbKmBT.Foreground = new SolidColorBrush(Colors.Black);
                        tbKmA.IsEnabled = tbKmB.IsEnabled = true;

                        break;
                    }

                case "4":
                    {
                        tbKmAT.Foreground = tbKmBT.Foreground = rbT.Foreground = tbdT.Foreground = new SolidColorBrush(Colors.Black);
                        tbKmA.IsEnabled = tbKmB.IsEnabled = tbd.IsEnabled = true;
                        rbl.IsEnabled = rbr.IsEnabled = true;

                        break;
                    }
                case "5":
                    {
                        tbKmAT.Foreground = rbT.Foreground = tbdT.Foreground = new SolidColorBrush(Colors.Black);
                        tbKmA.IsEnabled = tbd.IsEnabled = true;
                        rbl.IsEnabled = rbr.IsEnabled = true;

                        break;
                    }
                case "6":
                    {
                        tbKmAT.Foreground = tbKmBT.Foreground = tbdT.Foreground = rbT.Foreground = new SolidColorBrush(Colors.Black);
                        tbKmA.IsEnabled = tbKmB.IsEnabled = tbd.IsEnabled = true;
                        rbl.IsEnabled = rbr.IsEnabled = true;
                        break;
                    }

                case "8":
                    {
                        tbKmT.Foreground = new SolidColorBrush(Colors.Black);
                        tbKm.IsEnabled = true;

                        break;
                    }
                case "9":
                    {
                        tbKmAT.Foreground = tbKmBT.Foreground = new SolidColorBrush(Colors.Black);
                        tbKmA.IsEnabled = tbKmB.IsEnabled = true;
                        tbKmBT.Foreground = tbKmBT.Foreground = new SolidColorBrush(Colors.Black);
                        tbKmB.IsEnabled = tbKmB.IsEnabled = true;

                        break;
                    }
                default: break;
            }
        }

        private void AddPriv_OnClick(object sender, RoutedEventArgs e)
        {
            //новая привязка - 
            AddAllPriv();
        }

        //кнопка добавить привязку
        private void AddAllPriv()
        {

            int index = Model.CurrentIndex;

            List<PrivItems> tt = new List<PrivItems>();
            NavigationPrivListDouble = new List<PrivItems>();

            var allDataForm = new AllDataForm(tbKmA, tbKmB, tbKm, tbd, dolg, dolgA, dolgB, shirot, shirotA, shirotB, rbr, radExpander, keyMg, keyNit, shirot1, dolg1);

            switch (Model.PassportDetailModel.TYPEPRIV)
            {
                case "1":
                    {
                        #region добавление привязки 1 типа

                        NavigationPriv np = new NavigationPriv("", allDataForm.tbKm1, allDataForm.tbd1, allDataForm.dolg1, allDataForm.shirot1, "", "", "", "", "", "", "", "", allDataForm.keyMgDouble, allDataForm.keyPipeDouble, "", "", "", "", "");

                        //если новая привязка то просто добавляем
                        if (Model.NavigationPrivList.Count != 0)
                        {
                            //проверяем редактировалис ли данные
                            if (Model.DataHasChanged(np))
                            {
                                //редактирование
                                if (!string.IsNullOrEmpty(keyNit))
                                {
                                    NavigationPrivListDouble.Add(
                                   new PrivItems
                                   {
                                       CNAME = "",
                                       NKMORT1 = allDataForm.tbKm1,
                                       NDISTANCEBEG = allDataForm.tbd1,
                                       NX1 = allDataForm.dolg1,
                                       NY1 = allDataForm.shirot1,
                                       NKEY = "",
                                       NH1 = "",
                                       NKMTRUE1 = "",
                                       NX2 = "",
                                       NY2 = "",
                                       NKMORT2 = "",
                                       NKMTRUE2 = "",
                                       NDISTANCEEND = "",
                                       nMtKey = keyMg,
                                       nPipeKey = keyNit,
                                       NZ2 = "",
                                       NZ1 = "",
                                       NH2 = "",
                                       NBUILDTYPE = allDataForm.kmOrCoor,
                                       ISEDITED = "1"
                                   });
                                }
                            }
                            else
                            {
                                //если не было изменений то просто выходим
                                this.DialogResult = true;
                                return;
                            }
                        }
                        else
                        {
                            //создаем новую привязку
                            List<NavigationPriv> lnp = new List<NavigationPriv>();
                            lnp.Add(np);
                            NavigationPrivListDouble = HelperPriv.GetNavigationPrivListDoubleOne(lnp, NavigationPrivListDouble, radExpander.IsExpanded);
                        }
                        #endregion
                        break;
                    }
                case "2":
                    {
                        #region добавить привязку 2 типа
                        NavigationPriv np = new NavigationPriv("", allDataForm.tbKm1, allDataForm.tbd1, allDataForm.dolg1, allDataForm.shirot1, "", "", "", "", "", "", "", "", allDataForm.keyMgDouble, allDataForm.keyPipeDouble, "", "", "", "", "");

                        //если новая привязка то просто добавляем
                        if (Model.NavigationPrivList.Count != 0)
                        {
                            if (Model.DataHasChanged(np))
                            {
                                //редактирование
                                if (!string.IsNullOrEmpty(keyNit))
                                {
                                    //передаем координаты
                                    NavigationPrivListDouble.Add(
                                   new PrivItems
                                   {
                                       CNAME = "",
                                       NKMORT1 = allDataForm.tbKm1,
                                       NDISTANCEBEG = allDataForm.tbd1,
                                       NX1 = allDataForm.dolg1,
                                       NY1 = allDataForm.shirot1,
                                       NKEY = "",
                                       NH1 = "",
                                       NKMTRUE1 = "",
                                       NX2 = "",
                                       NY2 = "",
                                       NKMORT2 = "",
                                       NKMTRUE2 = "",
                                       NDISTANCEEND = "",
                                       nMtKey = keyMg,
                                       nPipeKey = keyNit,
                                       NZ2 = "",
                                       NZ1 = "",
                                       NH2 = "",
                                       NBUILDTYPE = allDataForm.kmOrCoor,
                                       ISEDITED = "1"

                                   });
                                }
                            }
                            else
                            {
                                //если не было изменений то просто выходим
                                this.DialogResult = true;
                                return;
                            }
                        }
                        //если создание нового
                        else
                        {
                            //создаем новую привязку
                            List<NavigationPriv> lnp = new List<NavigationPriv>();
                            lnp.Add(np);
                            NavigationPrivListDouble = HelperPriv.GetNavigationPrivListDoubleOne(lnp, NavigationPrivListDouble, radExpander.IsExpanded);
                        }
                        #endregion
                        break;
                    }
                case "3":
                    {
                        #region кнопка добавить привязка 3

                        NavigationPriv np = new NavigationPriv("", allDataForm.tbKmA1, "", allDataForm.dolgA1, allDataForm.shirotA1, "", "", "", allDataForm.dolgB1, allDataForm.shirotB1, allDataForm.tbKmB1, "", "", allDataForm.keyMgDouble, allDataForm.keyPipeDouble, "", "", "", "", "");
                        //если новая привязка то просто добавляем
                        if (Model.NavigationPrivList.Count != 0)
                        {
                            //проверяем редактировалис ли данные
                            if (Model.DataHasChanged(np))
                            {
                                //редактирование
                                if (!string.IsNullOrEmpty(keyNit))
                                {

                                    NavigationPrivListDouble.Add(
                                    new PrivItems
                                    {
                                        CNAME = "",
                                        NKMORT1 = allDataForm.tbKmA1,
                                        NDISTANCEBEG = "",
                                        NX1 = allDataForm.dolgA1,
                                        NY1 = allDataForm.shirotA1,
                                        NKEY = "",
                                        NH1 = "",
                                        NKMTRUE1 = "",
                                        NX2 = allDataForm.dolgB1,
                                        NY2 = allDataForm.shirotB1,
                                        NKMORT2 = allDataForm.tbKmB1,
                                        NKMTRUE2 = "",
                                        NDISTANCEEND = "",
                                        nMtKey = keyMg,
                                        nPipeKey = keyNit,
                                        NZ2 = "",
                                        NZ1 = "",
                                        NH2 = "",
                                        NBUILDTYPE = allDataForm.kmOrCoor,
                                        ISEDITED = "1"
                                    });
                                }
                            }
                            else
                            {
                                //если не было изменений то просто выходим
                                this.DialogResult = true;
                                return;
                            }
                        }
                        //если создание нового
                        else
                        {
                            //создаем новую привязку
                            List<NavigationPriv> lnp = new List<NavigationPriv>();
                            lnp.Add(np);
                            NavigationPrivListDouble = HelperPriv.GetNavigationPrivListDoubleOne(lnp, NavigationPrivListDouble, radExpander.IsExpanded);
                        }

                        #endregion
                        break;
                    }

                case "4":
                    {
                        #region добавить привязку 4 типа
                        NavigationPriv np = new NavigationPriv("", allDataForm.tbKmA1, "", allDataForm.dolgA1, allDataForm.shirotA1, "", "", "", allDataForm.dolgB1, allDataForm.shirotB1, allDataForm.tbKmB1, "", "", allDataForm.keyMgDouble, allDataForm.keyPipeDouble, "", "", "", "", "");

                        //если новая привязка то просто добавляем
                        if (Model.NavigationPrivList.Count != 0)
                        {
                            //проверяем редактировалис ли данные
                            if (Model.DataHasChanged(np))
                            {
                                //редактирование
                                if (!string.IsNullOrEmpty(keyNit))
                                {
                                    NavigationPrivListDouble.Add(
                                        new PrivItems
                                        {
                                            CNAME = nameNit,
                                            NKMORT1 = allDataForm.tbKm1,
                                            NDISTANCEBEG = allDataForm.tbd1,
                                            NX1 = allDataForm.dolg1,
                                            NY1 = allDataForm.shirot1,
                                            NKEY = "",
                                            NH1 = "",
                                            NKMTRUE1 = "",
                                            NX2 = "",
                                            NY2 = "",
                                            NKMORT2 = "",
                                            NKMTRUE2 = "",
                                            NDISTANCEEND = "",
                                            nMtKey = keyMg,
                                            nPipeKey = keyNit,
                                            NZ2 = "",
                                            NZ1 = "",
                                            NH2 = "",
                                            NBUILDTYPE = "1",
                                            ISEDITED = "1"
                                        });
                                    //перестраиваем массив с данными(данные должны придти из базы)
                                    // Model.LoadData(Model.PassportDetailModel.PassportKey, 0, NavigationPrivListDouble);
                                }

                            }
                            else
                            {
                                //надо сделать перерасчет привязки в базе

                                NavigationPrivListDouble = HelperPriv.GetNavigationPrivListDouble(Model.NavigationPrivList, NavigationPrivListDouble, radExpander.IsExpanded, Model.PassportDetailModel.TYPEPRIV);

                                //сохраняем массив, КОТОРЫЙ ПРИШЕЛ ИЗ БАЗЫ

                                Model.LoadData(Model.PassportDetailModel.PassportKey, 0, NavigationPrivListDouble);
                            }
                        }
                        else
                        {

                            // NavigationPriv np = new NavigationPriv(nameNit, tbKm1, tbd1, dolg1, shirot1, "", "", "", "", "", "", "", "", keyMgDouble, keyPipeDouble, "", "");
                            List<NavigationPriv> lnp = new List<NavigationPriv>();
                            lnp.Add(np);
                            NavigationPrivListDouble = HelperPriv.GetNavigationPrivListDoubleOne(lnp, NavigationPrivListDouble, radExpander.IsExpanded);

                            //сохраняем массив, КОТОРЫЙ ПРИШЕЛ ИЗ БАЗЫ

                            Model.LoadData(Model.PassportDetailModel.PassportKey, 0, NavigationPrivListDouble);

                            //Model.LoadData(Model.PassportDetailModel.PassportKey, 0, NavigationPrivListDouble);
                        }

                        PrivItemsNew.LISTPRIV = new List<SavePriv.SAVEPRIVITEMS>();
                        PrivItemsNew.LISTPRIV = HelperPriv.GetPrivItemsNew(Model.NavigationPrivList);

                        #endregion

                        break;
                    }
                case "5":
                    {

                        if ((string.IsNullOrEmpty(keyNit)) && (Model.NavigationPrivList.Count() == 0))
                        {
                            this.DialogResult = true;
                            return;

                        }


                        #region добавить привязку 5 типа
                        //если есть данные (но перешли на след страницу и ничего не добавили)
                        if ((string.IsNullOrEmpty(keyNit)) && (Model.NavigationPrivList.Count() > 0))
                        {
                            NavigationPrivListDouble = HelperPriv.GetNavigationPrivListDoubleOne(Model.NavigationPrivList, NavigationPrivListDouble, radExpander.IsExpanded);

                            List<PrivItems> outPrivxmlBaza = NavigationPrivListDouble;
                            ListprivItemsNew = outPrivxmlBaza;

                            //событие - перезаписать привязку в паспорте(если изменилась)
                            Model.PassportDetailModel.FirePropertyChanged("KeyPrivAdd");
                            this.DialogResult = true;
                            return;
                        }
                        //проверяем редактировалис ли данные
                        else
                        {
                            NavigationPriv np = new NavigationPriv("", allDataForm.tbKmA1, allDataForm.tbd1, allDataForm.dolg1, allDataForm.shirot1, "", "", "", "", "", "", "", "", allDataForm.keyMgDouble, allDataForm.keyPipeDouble, "", "", "", "", "");
                            //если есть данные и не сохраненные только на одной странице
                            if (Model.NavigationPrivList.Count == 0)
                            {
                                //создаем новую привязку
                                List<NavigationPriv> lnp = new List<NavigationPriv>();
                                lnp.Add(np);
                                NavigationPrivListDouble = HelperPriv.GetNavigationPrivListDoubleOne(lnp, NavigationPrivListDouble, radExpander.IsExpanded);
                            }
                            else
                            {
                                //проверяем редактировались ли данные
                                if (Model.DataHasChanged(np))
                                {
                                    // newUpdateItem = "2";
                                    NavigationPrivListDouble = HelperPriv.GetNavigationPrivList(newUpdateItem, Model.NavigationPrivList, np, index, radExpander.IsExpanded, Model.PassportDetailModel.TYPEPRIV);
                                }
                                else
                                {
                                    NavigationPrivListDouble = HelperPriv.GetNavigationPrivListDoubleOne(Model.NavigationPrivList, NavigationPrivListDouble, radExpander.IsExpanded);
                                    //переменная паблик - которая видна "наверху"
                                    List<PrivItems> outPrivxmlBaza = NavigationPrivListDouble;
                                    ListprivItemsNew = outPrivxmlBaza;

                                    //событие - перезаписать привязку в паспорте(если изменилась)
                                    Model.PassportDetailModel.FirePropertyChanged("KeyPrivAdd");
                                    this.DialogResult = true;
                                    return;
                                }
                            }


                        }


                        #endregion
                        newUpdateItem = "2";
                        break;
                    }
                case "6":
                    {
                        //проверка на не заданную нитку или нет данных
                        if ((string.IsNullOrEmpty(keyNit)) && (Model.NavigationPrivList.Count() == 0))
                        {
                            this.DialogResult = true;
                            return;

                        }

                        #region добавить привязку 6 типа
                        //если есть данные (но перешли на след страницу и ничего не добавили)
                        if ((string.IsNullOrEmpty(keyNit)) && (Model.NavigationPrivList.Count() > 0))
                        {
                            NavigationPrivListDouble = HelperPriv.GetNavigationPrivListDoubleOne(Model.NavigationPrivList, NavigationPrivListDouble, radExpander.IsExpanded);

                            List<PrivItems> outPrivxmlBaza = NavigationPrivListDouble;
                            ListprivItemsNew = outPrivxmlBaza;

                            //событие - перезаписать привязку в паспорте(если изменилась)
                            Model.PassportDetailModel.FirePropertyChanged("KeyPrivAdd");
                            this.DialogResult = true;
                            return;
                        }
                        //проверяем редактировалис ли данные
                        else
                        {
                            NavigationPriv np = new NavigationPriv("", allDataForm.tbKmA1, allDataForm.tbd1, allDataForm.dolgA1, allDataForm.shirotA1, "", "", "", allDataForm.dolgB1, allDataForm.shirotB1, allDataForm.tbKmB1, "", "", allDataForm.keyMgDouble, allDataForm.keyPipeDouble, "", "", "", "", "");
                            //если есть данные и не сохраненные только на одной странице
                            if (Model.NavigationPrivList.Count == 0)
                            {
                                //создаем новую привязку
                                List<NavigationPriv> lnp = new List<NavigationPriv>();
                                lnp.Add(np);
                                NavigationPrivListDouble = HelperPriv.GetNavigationPrivListDoubleOne(lnp, NavigationPrivListDouble, radExpander.IsExpanded);
                            }
                            else
                            {
                                //проверяем редактировались ли данные
                                if (Model.DataHasChanged(np))
                                {
                                    // newUpdateItem = "2";
                                    NavigationPrivListDouble = HelperPriv.GetNavigationPrivList(newUpdateItem, Model.NavigationPrivList, np, index, radExpander.IsExpanded, Model.PassportDetailModel.TYPEPRIV);
                                }
                                else
                                {
                                    NavigationPrivListDouble = HelperPriv.GetNavigationPrivListDoubleOne(Model.NavigationPrivList, NavigationPrivListDouble, radExpander.IsExpanded);
                                    //переменная паблик - которая видна "наверху"
                                    List<PrivItems> outPrivxmlBaza = NavigationPrivListDouble;
                                    ListprivItemsNew = outPrivxmlBaza;

                                    //событие - перезаписать привязку в паспорте(если изменилась)
                                    Model.PassportDetailModel.FirePropertyChanged("KeyPrivAdd");
                                    this.DialogResult = true;
                                    return;
                                }
                            }


                        }


                        #endregion
                        newUpdateItem = "2";
                        break;

                        #region Old Code
                        //проверяем редактировалис ли данные
                        //#region добавить привязку 6 типа
                        //NavigationPriv np = new NavigationPriv("", allDataForm.tbKmA1, allDataForm.tbd1, allDataForm.dolgA1, allDataForm.shirotA1, "", "", "", allDataForm.dolgB1, allDataForm.shirotB1, allDataForm.tbKmB1, "", "", allDataForm.keyMgDouble, allDataForm.keyPipeDouble, "", "", "", "", "");

                        //if ((!string.IsNullOrEmpty(keyNit)) && (Model.NavigationPrivList.Count() > 0))
                        //{
                        //    NavigationPrivListDouble = HelperPriv.GetNavigationPrivList(newUpdateItem, Model.NavigationPrivList, np, index, radExpander.IsExpanded);
                        //}
                        //else
                        //{
                        //    if (!string.IsNullOrEmpty(keyNit))
                        //    {
                        //        //создаем новую привязку
                        //        List<NavigationPriv> lnp = new List<NavigationPriv>();
                        //        lnp.Add(np);
                        //        NavigationPrivListDouble = HelperPriv.GetNavigationPrivListDoubleOne(lnp, NavigationPrivListDouble, radExpander.IsExpanded);
                        //    }
                        //    else
                        //    {
                        //        if (Model.NavigationPrivList.Count() > 0)
                        //        {
                        //            NavigationPrivListDouble = HelperPriv.GetNavigationPrivListDoubleOne(Model.NavigationPrivList, NavigationPrivListDouble, radExpander.IsExpanded);
                        //        }
                        //        else
                        //        {
                        //            this.DialogResult = true;
                        //        }
                        //    }
                        //}
                        //#endregion
                        //newUpdateItem = "2";
                        //break;
                        #endregion
                    }

                case "7":
                    {
                        #region добавление привязки 7 типа

                        NavigationPriv np = new NavigationPriv("", "", "", allDataForm.dolgA1, allDataForm.shirotA1, "", "", "", "", "", "", "", "", 0, 0, "", "", "", "", "");

                        //если ничего не ввели
                        if (!Model.GetData(allDataForm, false))
                        {
                            this.DialogResult = true;
                            return;
                        }


                        //если новая привязка то просто добавляем
                        if (Model.NavigationPrivList.Count != 0)
                        {
                            //проверяем редактировалис ли данные
                            if (Model.DataHasChanged(np))
                            {
                                NavigationPrivListDouble.Add(
                               new PrivItems
                               {
                                   CNAME = "",
                                   NKMORT1 = "",
                                   NDISTANCEBEG = "",
                                   NX1 = allDataForm.dolgA1,
                                   NY1 = allDataForm.shirotA1,
                                   NKEY = "",
                                   NH1 = "",
                                   NKMTRUE1 = "",
                                   NX2 = "",
                                   NY2 = "",
                                   NKMORT2 = "",
                                   NKMTRUE2 = "",
                                   NDISTANCEEND = "",
                                   nMtKey = "",
                                   nPipeKey = "",
                                   NZ2 = "",
                                   NZ1 = "",
                                   NH2 = "",
                                   NBUILDTYPE = "1",
                                   ISEDITED = "1"
                               });
                            }
                            else
                            {
                                //если не было изменений то просто выходим
                                this.DialogResult = true;
                                return;
                            }
                        }
                        else
                        {
                            //создаем новую привязку
                            List<NavigationPriv> lnp = new List<NavigationPriv>();
                            lnp.Add(np);
                            NavigationPrivListDouble = HelperPriv.GetNavigationPrivListDoubleOne(lnp, NavigationPrivListDouble, true);
                        }
                        #endregion
                        break;
                    }
                case "8":
                    {
                        if ((string.IsNullOrEmpty(keyNit)) && (Model.NavigationPrivList.Count() == 0))
                        {
                            this.DialogResult = true;
                            return;

                        }
                        #region добавить привязку 8 типа

                        //если есть данные (но перешли на след страницу и ничего не добавили)
                        if ((string.IsNullOrEmpty(keyNit)) && (Model.NavigationPrivList.Count() > 0))
                        {
                            NavigationPrivListDouble = HelperPriv.GetNavigationPrivListDoubleOne(Model.NavigationPrivList, NavigationPrivListDouble, radExpander.IsExpanded);

                            List<PrivItems> outPrivxmlBaza = NavigationPrivListDouble;
                            ListprivItemsNew = outPrivxmlBaza;

                            //событие - перезаписать привязку в паспорте(если изменилась)
                            Model.PassportDetailModel.FirePropertyChanged("KeyPrivAdd");
                            this.DialogResult = true;
                            return;
                        }
                        //проверяем редактировалис ли данные
                        else
                        {
                   //проверка на непустые данные                        
                            if (!Model.GetData(allDataForm, radExpander.IsExpanded))
                            {
                                //если данные пустые но есть предыдущие
                                if ((Model.NavigationPrivList.Count() > 0))
                                {
                                    NavigationPrivListDouble = HelperPriv.GetNavigationPrivListDoubleOne(Model.NavigationPrivList, NavigationPrivListDouble, radExpander.IsExpanded);

                                    List<PrivItems> outPrivxmlBaza = NavigationPrivListDouble;
                                    ListprivItemsNew = outPrivxmlBaza;

                                    //событие - перезаписать привязку в паспорте(если изменилась)
                                    Model.PassportDetailModel.FirePropertyChanged("KeyPrivAdd");
                                }
                                this.DialogResult = true;
                                return;
                            }

                            NavigationPriv np = np = new NavigationPriv("", allDataForm.tbKm1, "", allDataForm.dolg1, allDataForm.shirot1, "", "", "", "", "",
                                                             "", "", "", allDataForm.keyMgDouble, allDataForm.keyPipeDouble, "", "", "", "",
                                                             "");

                            //если есть данные и не сохраненные только на одной странице
                            if (Model.NavigationPrivList.Count == 0)
                            {
                                //создаем новую привязку
                                List<NavigationPriv> lnp = new List<NavigationPriv>();
                                lnp.Add(np);
                                NavigationPrivListDouble = HelperPriv.GetNavigationPrivListDoubleOne(lnp, NavigationPrivListDouble, radExpander.IsExpanded);
                            }
                            else
                            {
                                //проверяем редактировались ли данные
                                if (Model.DataHasChanged(np))
                                {
                                    // newUpdateItem = "2";
                                    NavigationPrivListDouble = HelperPriv.GetNavigationPrivList(newUpdateItem, Model.NavigationPrivList, np, index, radExpander.IsExpanded, Model.PassportDetailModel.TYPEPRIV);
                                }
                                else
                                {
                                    NavigationPrivListDouble = HelperPriv.GetNavigationPrivListDoubleOne(Model.NavigationPrivList, NavigationPrivListDouble, radExpander.IsExpanded);
                                    //переменная паблик - которая видна "наверху"
                                    List<PrivItems> outPrivxmlBaza = NavigationPrivListDouble;
                                    ListprivItemsNew = outPrivxmlBaza;

                                    //событие - перезаписать привязку в паспорте(если изменилась)
                                    Model.PassportDetailModel.FirePropertyChanged("KeyPrivAdd");
                                    this.DialogResult = true;
                                    return;
                                }
                            }
                        }


                        #endregion
                        newUpdateItem = "2";
                        break;
                    }
                case "9":
                    {
                        if ((string.IsNullOrEmpty(keyNit)) && (Model.NavigationPrivList.Count() == 0))
                        {
                            this.DialogResult = true;
                            return;

                        }


                        #region добавить привязку 9 типа
                        //если есть данные (но перешли на след страницу и ничего не добавили)
                        if ((string.IsNullOrEmpty(keyNit)) && (Model.NavigationPrivList.Count() > 0))
                        {
                            NavigationPrivListDouble = HelperPriv.GetNavigationPrivListDoubleOne(Model.NavigationPrivList, NavigationPrivListDouble, radExpander.IsExpanded);

                            List<PrivItems> outPrivxmlBaza = NavigationPrivListDouble;
                            ListprivItemsNew = outPrivxmlBaza;

                            //событие - перезаписать привязку в паспорте(если изменилась)
                            Model.PassportDetailModel.FirePropertyChanged("KeyPrivAdd");
                            this.DialogResult = true;
                            return;
                        }
                        //проверяем редактировалис ли данные
                        else
                        {

                            NavigationPriv np = new NavigationPriv("", allDataForm.tbKmA1, "", allDataForm.dolgA1, allDataForm.shirotA1, "", "", "",
                                allDataForm.dolgB1, allDataForm.shirotB1, allDataForm.tbKmB1, "", "", allDataForm.keyMgDouble, allDataForm.keyPipeDouble, "", "", "", "", "");
                            //если есть данные и не сохраненные только на одной странице
                            if (Model.NavigationPrivList.Count == 0)
                            {
                                //создаем новую привязку
                                List<NavigationPriv> lnp = new List<NavigationPriv>();
                                lnp.Add(np);
                                NavigationPrivListDouble = HelperPriv.GetNavigationPrivListDoubleOne(lnp, NavigationPrivListDouble, radExpander.IsExpanded);
                            }
                            else
                            {
                                //проверяем редактировались ли данные
                                if (Model.DataHasChanged(np))
                                {
                                    // newUpdateItem = "2";
                                    NavigationPrivListDouble = HelperPriv.GetNavigationPrivList(newUpdateItem, Model.NavigationPrivList, np, index, radExpander.IsExpanded, Model.PassportDetailModel.TYPEPRIV);
                                }
                                else
                                {
                                    NavigationPrivListDouble = HelperPriv.GetNavigationPrivListDoubleOne(Model.NavigationPrivList, NavigationPrivListDouble, radExpander.IsExpanded);
                                    //переменная паблик - которая видна "наверху"
                                    List<PrivItems> outPrivxmlBaza = NavigationPrivListDouble;
                                    ListprivItemsNew = outPrivxmlBaza;

                                    //событие - перезаписать привязку в паспорте(если изменилась)
                                    Model.PassportDetailModel.FirePropertyChanged("KeyPrivAdd");
                                    this.DialogResult = true;
                                    return;
                                }
                            }


                        }


                        #endregion
                        break;
                    }

                case "10":
                    {

                        if (TabInPriv10.SelectedItem is RadTabItem)
                        {
                            var tab = TabInPriv10.SelectedItem as RadTabItem;

                            if (tab.Name.Equals("tabItemGrid", StringComparison.OrdinalIgnoreCase))
                            {
                                List<NavigationPriv> navigationPrivList = new List<NavigationPriv>();
                               navigationPrivList = gridType10.GetNavigationPriv();
                               NavigationPrivListDouble = HelperPriv.GetNavigationPrivListDoubleOnType10Grid(navigationPrivList);
                               if (NavigationPrivListDouble.Count > 0)
                               {
                                   string xmlExport = HelperPriv.GetXmlInNewPriv(NavigationPrivListDouble, passportDetailModel);

                                   if (!string.IsNullOrEmpty(xmlExport))
                                   {
                                       ServiceDataClient.GetCalcPrivJsonCompleted += ServiceDataClient_GetCalcPrivJsonCompleted;
                                       ServiceDataClient.GetCalcPrivJsonAsync(Model.PassportDetailModel.PassportKey, xmlExport, GlobalContext.Context);
                                   }
                                   else
                                   {
                                       Model.PassportDetailModel.MainModel.Report("ошибка  формирования xml AddAllPriv()");
                                   }
                               }
                               else
                               {
                                   this.DialogResult = true;
                               }

                                return;
                            }
                        }
                       
                            #region добавить привязку 10 типа
                            //если есть данные (но перешли на след страницу и ничего не добавили)
                            if ((string.IsNullOrEmpty(allDataForm.dolg1_1)) && (string.IsNullOrEmpty(allDataForm.dolg1_1)))
                            {


                                if (Model.NavigationPrivList.Count() > 0)
                                {
                                    NavigationPrivListDouble = HelperPriv.GetNavigationPrivListDoubleOne(Model.NavigationPrivList, NavigationPrivListDouble, radExpander.IsExpanded);

                                    List<PrivItems> outPrivxmlBaza = NavigationPrivListDouble;
                                    ListprivItemsNew = outPrivxmlBaza;

                                    //событие - перезаписать привязку в паспорте(если изменилась)
                                    Model.PassportDetailModel.FirePropertyChanged("KeyPrivAdd");
                                    this.DialogResult = true;
                                    return;
                                }
                                else
                                {
                                    this.DialogResult = true;
                                    return;
                                }
                            }
                            //проверяем редактировалис ли данные
                            else
                            {
                                NavigationPriv np = new NavigationPriv("", "", "", allDataForm.dolg1_1, allDataForm.shirot1_1, "", "", "",
                                                                       "", "", "", "", "", 0, 0, "", "", "", "", "");
                                //если есть данные и не сохраненные только на одной странице
                                if (Model.NavigationPrivList.Count == 0)
                                {
                                    //создаем новую привязку
                                    List<NavigationPriv> lnp = new List<NavigationPriv>();
                                    lnp.Add(np);
                                    NavigationPrivListDouble = HelperPriv.GetNavigationPrivListDoubleOne(lnp, NavigationPrivListDouble, radExpander.IsExpanded);
                                }
                                else
                                {
                                    //проверяем редактировались ли данные
                                    if (Model.DataHasChanged(np))
                                    {
                                        // newUpdateItem = "2";
                                        NavigationPrivListDouble = HelperPriv.GetNavigationPrivList(newUpdateItem, Model.NavigationPrivList, np, index, radExpander.IsExpanded, Model.PassportDetailModel.TYPEPRIV);
                                    }
                                    else
                                    {
                                        //всегда будем проверять при сохранении
                                        NavigationPrivListDouble = HelperPriv.GetNavigationPrivListDoubleOne(Model.NavigationPrivList, NavigationPrivListDouble, radExpander.IsExpanded);

                                        //переменная паблик - которая видна "наверху"
                                        //List<PrivItems> outPrivxmlBaza = NavigationPrivListDouble;
                                        //ListprivItemsNew = outPrivxmlBaza;

                                        ////событие - перезаписать привязку в паспорте(если изменилась)
                                        //Model.PassportDetailModel.FirePropertyChanged("KeyPrivAdd");
                                        //this.DialogResult = true;
                                        //return;
                                    }
                                }

                            }



                            #endregion      
                       
                      
                        break;
                    }

            }

            ///////////////////////////////////////////////////

            if (NavigationPrivListDouble.Count > 0)
            {
                string xmlExport = HelperPriv.GetXmlInNewPriv(NavigationPrivListDouble, passportDetailModel);

                if (!string.IsNullOrEmpty(xmlExport))
                {
                    ServiceDataClient.GetCalcPrivJsonCompleted += ServiceDataClient_GetCalcPrivJsonCompleted;
                    ServiceDataClient.GetCalcPrivJsonAsync(Model.PassportDetailModel.PassportKey, xmlExport, GlobalContext.Context);
                }
                else
                {
                    Model.PassportDetailModel.MainModel.Report("ошибка  формирования xml AddAllPriv()");
                }
            }
            else
            {
                this.DialogResult = true;
            }

        }

        //используется при нажатии кнопки AddPriv
        void ServiceDataClient_GetCalcPrivJsonCompleted(object sender, GetCalcPrivJsonCompletedEventArgs e)
        {
            ServiceDataClient.GetCalcPrivJsonCompleted -= ServiceDataClient_GetCalcPrivJsonCompleted;
            if (e.Result.IsValid)
            {
                string xmlToList = e.Result.CalcPrivOnEditJson_result.CalcPrivOnEditJsonOnKeyPassport;

                //конвертируем обратно из джейсона в структуру привязки
                List<PrivItems> outPrivxmlBaza = HelperPriv.GetXmlOut(xmlToList, passportDetailModel);
                //загружаем модель новыми данными
                Model.LoadData(Model.PassportDetailModel.PassportKey, 0, outPrivxmlBaza);

                //переменная паблик - которая видна "наверху"
                ListprivItemsNew = outPrivxmlBaza;

                //событие - перезаписать привязку в паспорте(если изменилась)
                Model.PassportDetailModel.FirePropertyChanged("KeyPrivAdd");
                this.DialogResult = true;
            }
            else
            {
                ChildWindowAttantion _popupWindowAttantion = new ChildWindowAttantion();
                _popupWindowAttantion.titleBox.Text = ProjectResources.PrivTitleBoxErr + e.Result.ErrorMessage;
                _popupWindowAttantion.Show();
                Model.PassportDetailModel.MainModel.Report("ошибка создании привязки " + e.Result.ErrorMessage);
            }
        }

        private void Rbl_OnClick(object sender, RoutedEventArgs e)
        {
            RblOnClick = true;
            RbrOnClick = false;
        }

        private void Rbr_OnClick(object sender, RoutedEventArgs e)
        {
            RblOnClick = false;
            RbrOnClick = true;
        }

        //вперед
        private void Next_OnClick(object sender, RoutedEventArgs e)
        {
           NavigationPrivListDouble = new List<PrivItems>();

            //Данные контролов
            index = Model.CurrentIndex;
            NavigationPriv np = new NavigationPriv("", "", "", "", "", "", "", "", "", "", "", "", "", 0, 0, "", "", "", "", "");

            var allDataFormForwardEnd = new AllDataForm(tbKmA, tbKmB, tbKm, tbd, dolg, dolgA, dolgB, shirot, shirotA, shirotB, rbr, radExpander, keyMg, keyNit, shirot1, dolg1);

            switch (Model.PassportDetailModel.TYPEPRIV)
            {
                case "5":
                    {
                        np = new NavigationPriv("", allDataFormForwardEnd.tbKmA1, allDataFormForwardEnd.tbd1, allDataFormForwardEnd.dolg1,
                                                                allDataFormForwardEnd.shirot1, "", "", "", "", "", "", "", "",
                                                                allDataFormForwardEnd.keyMgDouble, allDataFormForwardEnd.keyPipeDouble, "", "", "", "", "");
                        break;
                    }
                case "6":
                    {
                        np = new NavigationPriv("", allDataFormForwardEnd.tbKmA1, allDataFormForwardEnd.tbd1, allDataFormForwardEnd.dolgA1, allDataFormForwardEnd.shirotA1,
                                                               "", "", "", allDataFormForwardEnd.dolgB1, allDataFormForwardEnd.shirotB1, allDataFormForwardEnd.tbKmB1, "", "",
                                                               allDataFormForwardEnd.keyMgDouble, allDataFormForwardEnd.keyPipeDouble, "", "", "", "", "");
                        break;
                    }
                case "8":
                    {
                        np = new NavigationPriv("", allDataFormForwardEnd.tbKm1, "", allDataFormForwardEnd.dolg1, allDataFormForwardEnd.shirot1, "", "", "", "", "",
                                                             "", "", "", allDataFormForwardEnd.keyMgDouble, allDataFormForwardEnd.keyPipeDouble, "", "", "", "",
                                                             "");
                        break;
                    }
                case "9":
                    {
                        np = new NavigationPriv("", allDataFormForwardEnd.tbKmA1, "", allDataFormForwardEnd.dolgA1, allDataFormForwardEnd.shirotA1, "", "", "",
                              allDataFormForwardEnd.dolgB1, allDataFormForwardEnd.shirotB1, allDataFormForwardEnd.tbKmB1, "", "",
                              allDataFormForwardEnd.keyMgDouble, allDataFormForwardEnd.keyPipeDouble, "", "", "", "", "");
                        break;
                    }
                case "10":
                    {
                        np = new NavigationPriv("", "", "", allDataFormForwardEnd.dolg1_1, allDataFormForwardEnd.shirot1_1, "", "", "",
                              "", "", "", "", "", 0, 0, "", "", "", "", "");
                        break;
                    }
            }
            if (Model.PassportDetailModel.TYPEPRIV != "10")
            {
                // newUpdateItem = "2";
                if (string.IsNullOrEmpty(keyNit) | (!Model.GetData(allDataFormForwardEnd, radExpander.IsExpanded)))
                {
                    return;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(allDataFormForwardEnd.dolg1_1) &&
                    string.IsNullOrEmpty(allDataFormForwardEnd.shirot1_1))
                {
                    return;
                }
            }


            //создали новую привязку и кликнули вперед
            if (index == -1)
            {
                //создаем новую привязку
                List<NavigationPriv> lnp = new List<NavigationPriv>();
                lnp.Add(np);
                NavigationPrivListDouble = HelperPriv.GetNavigationPrivListDoubleOne(lnp, NavigationPrivListDouble, radExpander.IsExpanded);

                //запрос в базу - можно ли создать привязку

                string xmlExport = HelperPriv.GetXmlInNewPriv(NavigationPrivListDouble, passportDetailModel);

                if (!string.IsNullOrEmpty(xmlExport))
                {
                    ServiceDataClient.GetCalcPrivJsonCompleted += ServiceDataClient_GetCalcPrivXml2Completed;
                    ServiceDataClient.GetCalcPrivJsonAsync(Model.PassportDetailModel.PassportKey, xmlExport, GlobalContext.Context);
                }
                else
                {
                    Model.PassportDetailModel.MainModel.Report("ошибка  формирования xml Next_OnClick");
                }
            }
            else
            {
                //проверяем изменялась ли привязка
                if (Model.DataHasChanged(np))
                {
                    //если есть данные
                    if (Model.NavigationPrivList.Count() > 0)
                    {
                        NavigationPrivListDouble = HelperPriv.GetNavigationPrivList(newUpdateItem, Model.NavigationPrivList, np, index, radExpander.IsExpanded, Model.PassportDetailModel.TYPEPRIV);
                    }

                    //запрос в базу - можно ли создать привязку

                    string xmlExport = HelperPriv.GetXmlInNewPriv(NavigationPrivListDouble, passportDetailModel);

                    if (!string.IsNullOrEmpty(xmlExport))
                    {
                        ServiceDataClient.GetCalcPrivJsonCompleted += ServiceDataClient_GetCalcPrivXml2Completed;
                        ServiceDataClient.GetCalcPrivJsonAsync(Model.PassportDetailModel.PassportKey, xmlExport, GlobalContext.Context);
                    }
                    else
                    {
                        Model.PassportDetailModel.MainModel.Report("ошибка  формирования xml Next_OnClick");
                    }

                }
                //если не редактировалось
                else
                {
                    if (Model.CurrentIndex == Model.NavigationPrivList.Count - 1)
                    {
                        switch (Model.PassportDetailModel.TYPEPRIV)
                        {
                            case "5":
                                {
                                    //если добавить новые данные
                                    tbKmA.Text = "";
                                    tbd.Text = "";
                                    dolg.Text = "";
                                    shirot.Text = "";

                                    break;
                                }
                            case "6":
                                {

                                    tbmgt.Text = ProjectResources.PtivTitleBoxM + (Model.NavigationPrivList.Count + 1).ToString();
                                    tbpipet.Text = ProjectResources.PrivTitleBoxN + (Model.NavigationPrivList.Count + 1).ToString();
                                    //если добавить новые данные
                                    tbKmA.Text = "";
                                    tbd.Text = "";
                                    dolgA.Text = "";
                                    shirotA.Text = "";

                                    tbKmB.Text = "";
                                    dolgB.Text = "";
                                    shirotB.Text = "";

                                    break;
                                }
                            case "8":
                                {
                                    int indexNext = Model.CurrentIndex;

                                    //у нас должно быть только 2 привязке - т.к. перемычка
                                    if (indexNext >= 1)
                                    {
                                        return;
                                    }
                                    tbmgt.Text = ProjectResources.PtivTitleBoxM + " " + (indexNext + 2).ToString();
                                    tbpipet.Text = ProjectResources.PrivTitleBoxN + " " + (indexNext + 2).ToString();



                                    mainData.RowDefinitions[2].Height = new GridLength(35);
                                    mainData.RowDefinitions[3].Height = new GridLength(0);
                                    mainData.RowDefinitions[4].Height = new GridLength(0);
                                    mainData.RowDefinitions[5].Height = new GridLength(0);
                                    mainData.RowDefinitions[6].Height = new GridLength(0);

                                    expander.RowDefinitions[0].Height = new GridLength(0);
                                    expander.RowDefinitions[1].Height = new GridLength(0);
                                    expander.RowDefinitions[2].Height = new GridLength(35);

                                    expander.RowDefinitions[3].Height = new GridLength(0);
                                    expander.RowDefinitions[4].Height = new GridLength(35);
                                    expander.RowDefinitions[5].Height = new GridLength(35);

                                    tbKmT.Text = ProjectResources.PrivKmB;
                                    tbKm.displayWatermark = false;
                                    tbKm.setMode(false);
                                    tbKm.Text = "";

                                    tbshirotT.Text = ProjectResources.PrivShirotB;
                                    shirot.setMode(false);
                                    shirot.displayWatermark = false;
                                    shirot.Text = "";
                                    tbdolgT.Text = ProjectResources.PrivDolgB;
                                    dolg.setMode(false);
                                    dolg.displayWatermark = false;
                                    dolg.Text = "";

                                    IsNextNewDataPipe = true;
                                    ddlMG.ItemsSource = null;
                                    ddlMG.ItemsSource = SelectMGList;
                                    ddlMG.SelectedIndex = ddlMG.Items.IndexOf(SelectMGList[0]);
                                    newUpdateItem = "1";
                                    break;
                                }
                            case "9":
                                {
                                    int indexNext = Model.CurrentIndex;
                                    indexNext = indexNext + 2;

                                    tbmgt.Text = ProjectResources.PtivTitleBoxM + " " + indexNext.ToString();
                                    tbpipet.Text = ProjectResources.PrivTitleBoxN + " " + indexNext.ToString();


                                    tbKmAT.Text = ProjectResources.PrivKmA9 + indexNext.ToString() + ",  " + ProjectResources.PrivKm9m;
                                    tbKmA.setMode(false);
                                    tbKmA.displayWatermark = false;
                                    tbKmA.Text = "";

                                    tbKmBT.Text = ProjectResources.PrivKmB9 + indexNext.ToString() + ",  " + ProjectResources.PrivKm9m;
                                    tbKmB.setMode(false);
                                    tbKmB.displayWatermark = false;
                                    tbKmB.Text = "";

                                    tbshirotAT.Text = ProjectResources.PrivShirotA1 + indexNext.ToString() + ProjectResources.Priv9Gr;
                                    shirotA.setMode(false);
                                    shirotA.displayWatermark = false;
                                    shirotA.Text = "";

                                    tbdolgAT.Text = ProjectResources.PrivDolgA1 + indexNext.ToString() + ProjectResources.Priv9Gr;
                                    dolgA.setMode(false);
                                    dolgA.displayWatermark = false;
                                    dolgA.Text = "";

                                    tbshirotBT.Text = ProjectResources.PrivShirotB1 + indexNext.ToString() + ProjectResources.Priv9Gr;
                                    shirotB.setMode(false);
                                    shirotB.displayWatermark = false;
                                    shirotB.Text = "";

                                    tbdolgBT.Text = ProjectResources.PrivDolgB1 + indexNext.ToString() + ProjectResources.Priv9Gr;
                                    dolgB.setMode(false);
                                    dolgB.displayWatermark = false;
                                    dolgB.Text = "";

                                    IsNextNewDataPipe = true;
                                    ddlMG.ItemsSource = null;
                                    ddlMG.ItemsSource = SelectMGList;
                                    ddlMG.SelectedIndex = ddlMG.Items.IndexOf(SelectMGList[0]);

                                    newUpdateItem = "1";
                                    break;
                                }
                            case "10":
                                {
                                    int indexNext = Model.CurrentIndex;
                                    indexNext = indexNext + 2;

                                    tbshirotT1.Text = ProjectResources.PrivShirot1 + indexNext.ToString() + ProjectResources.Priv9Gr;
                                    shirot1.setMode(false);
                                    shirot1.displayWatermark = false;
                                    shirot1.Text = "";

                                    tbdolgT1.Text = ProjectResources.PrivDolg1 + indexNext.ToString() + ProjectResources.Priv9Gr;
                                    dolg1.setMode(false);
                                    dolg1.displayWatermark = false;
                                    dolg1.Text = "";

                                    IsNextNewDataPipe = true;
                                    newUpdateItem = "1";

                                    break;
                                }
                        }
                        if ((Model.PassportDetailModel.TYPEPRIV == "8") || (Model.PassportDetailModel.TYPEPRIV == "9") || (Model.PassportDetailModel.TYPEPRIV == "10"))
                        {
                        }
                        else
                        {
                            mainData.RowDefinitions[2].Height = new GridLength(0);
                            mainData.RowDefinitions[3].Height = new GridLength(0);
                            mainData.RowDefinitions[4].Height = new GridLength(0);
                            mainData.RowDefinitions[5].Height = new GridLength(0);
                            mainData.RowDefinitions[6].Height = new GridLength(0);
                            radExpander.Visibility = Visibility.Collapsed;
                            //mainData.RowDefinitions[7].Height = new GridLength(0);
                            IsNextNewDataPipe = true;
                            ddlMG.ItemsSource = null;
                            ddlMG.ItemsSource = SelectMGList;
                            ddlMG.SelectedIndex = ddlMG.Items.IndexOf(SelectMGList[0]);
                            newUpdateItem = "1";
                        }


                    }
                    else
                    {
                        Model.GoNextZak();
                        int indexNext = Model.CurrentIndex;
                        switch (Model.PassportDetailModel.TYPEPRIV)
                        {
                            case "5":
                                {
                                    InitPriv5Type(indexNext);
                                    break;
                                }
                            case "6":
                                {
                                    InitPriv6Type(indexNext);
                                    break;
                                }
                            case "8":
                                {
                                    InitPriv8Type(indexNext);
                                    break;
                                }
                            case "9":
                                {
                                    InitPriv9Type(indexNext);
                                    break;
                                }
                            case "10":
                                {
                                    InitPriv10Type(indexNext);
                                    break;
                                }
                        }
                        if (Model.PassportDetailModel.TYPEPRIV != "10")
                        {
                            SelectMgOnFewPipe(Model.NavigationPrivList[indexNext], SelectMGList);
                            SelectPipeOnFewPipe(Model.NavigationPrivList[indexNext], SelectNitList);
                        }


                    }

                }
            }
        }

        //кнопка next
        void ServiceDataClient_GetCalcPrivXml2Completed(object sender, GetCalcPrivJsonCompletedEventArgs e)
        {
            ServiceDataClient.GetCalcPrivJsonCompleted -= ServiceDataClient_GetCalcPrivXml2Completed;
            if (e.Result.IsValid)
            {
                int index = Model.CurrentIndex;

                string xmlToList = e.Result.CalcPrivOnEditJson_result.CalcPrivOnEditJsonOnKeyPassport;

                //конвертируем обратно из xml в структуру привязки
                List<PrivItems> outPrivxmlBaza = HelperPriv.GetXmlOut(xmlToList, passportDetailModel);
                if (Model.CurrentIndex == -1)
                {
                    index = index + 1;
                }
                //загружаем модель новыми данными
                //соответственно перестраивать привязку
                Model.LoadData(Model.PassportDetailModel.PassportKey, index + 1, outPrivxmlBaza);

                if (Model.NavigationPrivList.Count - 1 == Model.CurrentIndex)
                {
                    //загружаем новую страницу с данными

                    switch (Model.PassportDetailModel.TYPEPRIV)
                    {
                        case "5":
                            {

                                tbKmA.Text = "";
                                tbd.Text = "";
                                dolg.Text = "";
                                shirot.Text = "";

                                mainData.RowDefinitions[2].Height = new GridLength(0);
                                mainData.RowDefinitions[3].Height = new GridLength(0);
                                mainData.RowDefinitions[4].Height = new GridLength(0);
                                mainData.RowDefinitions[5].Height = new GridLength(0);
                                mainData.RowDefinitions[6].Height = new GridLength(0);
                                mainData.RowDefinitions[7].Height = new GridLength(0);
                                IsNextNewDataPipe = true;
                                ddlMG.ItemsSource = null;
                                ddlMG.ItemsSource = SelectMGList;
                                ddlMG.SelectedIndex = ddlMG.Items.IndexOf(SelectMGList[0]);
                                //создаем новую привязку
                                newUpdateItem = "1";
                                break;
                            }
                        case "6":
                            {
                                tbmgt.Text = ProjectResources.PtivTitleBoxM + " " + (Model.NavigationPrivList.Count + 1).ToString();
                                tbpipet.Text = ProjectResources.PrivTitleBoxN + " " + (Model.NavigationPrivList.Count + 1).ToString();

                                tbKmA.Text = "";
                                tbKmB.Text = "";
                                tbd.Text = "";
                                dolgA.Text = "";
                                shirotA.Text = "";
                                dolgB.Text = "";
                                shirotB.Text = "";

                                mainData.RowDefinitions[2].Height = new GridLength(0);
                                mainData.RowDefinitions[3].Height = new GridLength(0);
                                mainData.RowDefinitions[4].Height = new GridLength(0);
                                mainData.RowDefinitions[5].Height = new GridLength(0);
                                mainData.RowDefinitions[6].Height = new GridLength(0);
                                mainData.RowDefinitions[7].Height = new GridLength(0);
                                IsNextNewDataPipe = true;
                                ddlMG.ItemsSource = null;
                                ddlMG.ItemsSource = SelectMGList;
                                ddlMG.SelectedIndex = ddlMG.Items.IndexOf(SelectMGList[0]);
                                //создаем новую привязку
                                newUpdateItem = "1";
                                break;
                            }

                        case "8":
                            {
                                int indexNext = Model.CurrentIndex;

                                //у нас должно быть только 2 привязке - т.к. перемычка

                                if ((Model.CurrentIndex == 1) & (Model.NavigationPrivList.Count == 2))
                                {

                                    mainData.RowDefinitions[2].Height = new GridLength(35);
                                    mainData.RowDefinitions[3].Height = new GridLength(0);
                                    mainData.RowDefinitions[4].Height = new GridLength(0);
                                    mainData.RowDefinitions[5].Height = new GridLength(0);
                                    mainData.RowDefinitions[6].Height = new GridLength(0);

                                    expander.RowDefinitions[0].Height = new GridLength(0);
                                    expander.RowDefinitions[1].Height = new GridLength(0);
                                    expander.RowDefinitions[2].Height = new GridLength(35);

                                    expander.RowDefinitions[3].Height = new GridLength(0);
                                    expander.RowDefinitions[4].Height = new GridLength(35);
                                    expander.RowDefinitions[5].Height = new GridLength(35);


                                    InitPriv8Type(Model.CurrentIndex);


                                    return;
                                }
                                //if (indexNext >= 1)
                                //{
                                //    return;
                                //}

                                if (indexNext == 0)
                                {
                                    indexNext = indexNext + 2;
                                }
                                else
                                {
                                    indexNext = indexNext + 1;
                                }


                                tbmgt.Text = ProjectResources.PtivTitleBoxM + " " + indexNext.ToString();
                                tbpipet.Text = ProjectResources.PrivTitleBoxN + " " + indexNext.ToString();

                                //mainData.RowDefinitions[3].Height = new GridLength(0);
                                //mainData.RowDefinitions[4].Height = new GridLength(0);
                                //mainData.RowDefinitions[5].Height = new GridLength(0);
                                //mainData.RowDefinitions[6].Height = new GridLength(0);

                                //expander.RowDefinitions[0].Height = new GridLength(0);
                                //expander.RowDefinitions[1].Height = new GridLength(0);
                                //expander.RowDefinitions[2].Height = new GridLength(0);
                                //expander.RowDefinitions[3].Height = new GridLength(0);



                                tbKmT.Text = ProjectResources.PrivKmB;
                                tbKm.displayWatermark = false;
                                tbKm.Text = "";

                                tbshirotT.Text = ProjectResources.PrivShirotB;
                                shirot.setMode(false);
                                shirot.displayWatermark = false;
                                shirot.Text = "";
                                tbdolgT.Text = ProjectResources.PrivDolgB;
                                dolg.setMode(false);
                                dolg.displayWatermark = false;
                                dolg.Text = "";

                                IsNextNewDataPipe = true;
                                ddlMG.ItemsSource = null;
                                ddlMG.ItemsSource = SelectMGList;
                                ddlMG.SelectedIndex = ddlMG.Items.IndexOf(SelectMGList[0]);

                                newUpdateItem = "1";


                                // var allDataFormForwardEnd = new AllDataForm(tbKmA, tbKmB, tbKm, tbd, dolg, dolgA, dolgB, shirot, shirotA, shirotB, rbr, radExpander, keyMg, keyNit, shirot1, dolg1);

           
                                //// np = new NavigationPriv("", allDataFormForwardEnd.tbKm1, "", allDataFormForwardEnd.dolg1, allDataFormForwardEnd.shirot1, "", "", "", "", "",
                                //                             "", "", "", allDataFormForwardEnd.keyMgDouble, allDataFormForwardEnd.keyPipeDouble, "", "", "", "",
                                //  //                           "");




                                break;
                            }
                        case "9":
                            {
                                int indexNext = Model.CurrentIndex;
                                indexNext = indexNext + 2;

                                tbmgt.Text = ProjectResources.PtivTitleBoxM + " " + indexNext.ToString();
                                tbpipet.Text = ProjectResources.PrivTitleBoxN + " " + indexNext.ToString();


                                tbKmAT.Text = ProjectResources.PrivKmA9 + indexNext.ToString() + ",  " + ProjectResources.PrivKm9m;
                                tbKmA.setMode(false);
                                tbKmA.displayWatermark = false;
                                tbKmA.Text = "";

                                tbKmBT.Text = ProjectResources.PrivKmB9 + indexNext.ToString() + ",  " + ProjectResources.PrivKm9m;
                                tbKmB.setMode(false);
                                tbKmB.displayWatermark = false;
                                tbKmB.Text = "";

                                tbshirotAT.Text = ProjectResources.PrivShirotA1 + indexNext.ToString() + ProjectResources.Priv9Gr;
                                shirotA.setMode(false);
                                shirotA.displayWatermark = false;
                                shirotA.Text = "";

                                tbdolgAT.Text = ProjectResources.PrivDolgA1 + indexNext.ToString() + ProjectResources.Priv9Gr;
                                dolgA.setMode(false);
                                dolgA.displayWatermark = false;
                                dolgA.Text = "";

                                tbshirotBT.Text = ProjectResources.PrivShirotB1 + indexNext.ToString() + ProjectResources.Priv9Gr;
                                shirotB.setMode(false);
                                shirotB.displayWatermark = false;
                                shirotB.Text = "";

                                tbdolgBT.Text = ProjectResources.PrivDolgB1 + indexNext.ToString() + ProjectResources.Priv9Gr;
                                dolgB.setMode(false);
                                dolgB.displayWatermark = false;
                                dolgB.Text = "";

                                IsNextNewDataPipe = true;
                                ddlMG.ItemsSource = null;
                                ddlMG.ItemsSource = SelectMGList;
                                ddlMG.SelectedIndex = ddlMG.Items.IndexOf(SelectMGList[0]);

                                newUpdateItem = "1";

                                break;
                            }
                        case "10":
                            {
                                
                                    int indexNext = Model.CurrentIndex;
                                    indexNext = indexNext + 2;

                                    tbshirotT1.Text = ProjectResources.PrivShirot1 + indexNext.ToString() + ProjectResources.Priv9Gr;
                                    shirot1.setMode(false);
                                    shirot1.displayWatermark = false;
                                    shirot1.Text = "";

                                    tbdolgT1.Text = ProjectResources.PrivDolg1 + indexNext.ToString() + ProjectResources.Priv9Gr;
                                    dolg1.setMode(false);
                                    dolg1.displayWatermark = false;
                                    dolg1.Text = "";

                                    IsNextNewDataPipe = true;
                                    newUpdateItem = "1";
                                    break;
                                
                                
                            }
                    }


                }
                else
                {

                    switch (Model.PassportDetailModel.TYPEPRIV)
                    {
                        case "5":
                            {
                                mainData.RowDefinitions[2].Height = new GridLength(0);
                                mainData.RowDefinitions[4].Height = new GridLength(0);

                                expander.RowDefinitions[0].Height = new GridLength(0);
                                expander.RowDefinitions[1].Height = new GridLength(0);
                                expander.RowDefinitions[2].Height = new GridLength(0);
                                expander.RowDefinitions[3].Height = new GridLength(0);
                                newUpdateItem = "2";

                                Model.GoCurrentZak();
                                int indexNext = Model.CurrentIndex;
                                InitPriv5Type(indexNext);
                                SelectMgOnFewPipe(Model.NavigationPrivList[indexNext], SelectMGList);
                                SelectPipeOnFewPipe(Model.NavigationPrivList[indexNext], SelectNitList);
                                break;
                            }
                        case "6":
                            {
                                mainData.RowDefinitions[2].Height = new GridLength(0);
                                mainData.RowDefinitions[4].Height = new GridLength(0);

                                expander.RowDefinitions[0].Height = new GridLength(0);
                                expander.RowDefinitions[1].Height = new GridLength(0);
                                expander.RowDefinitions[2].Height = new GridLength(0);
                                expander.RowDefinitions[3].Height = new GridLength(0);
                                newUpdateItem = "2";

                                Model.GoCurrentZak();
                                int indexNext = Model.CurrentIndex;
                                InitPriv6Type(indexNext);
                                SelectMgOnFewPipe(Model.NavigationPrivList[indexNext], SelectMGList);
                                SelectPipeOnFewPipe(Model.NavigationPrivList[indexNext], SelectNitList);
                                break;
                            }
                        case "8":
                            {
                                Model.GoCurrentZak();
                                int indexNext = Model.CurrentIndex;
                                InitPriv8Type(indexNext);
                                SelectMgOnFewPipe(Model.NavigationPrivList[indexNext], SelectMGList);
                                SelectPipeOnFewPipe(Model.NavigationPrivList[indexNext], SelectNitList);
                                break;
                            }
                        case "9":
                            {
                                newUpdateItem = "2";

                                Model.GoCurrentZak();
                                int indexNext = Model.CurrentIndex;
                                InitPriv9Type(indexNext);
                                SelectMgOnFewPipe(Model.NavigationPrivList[indexNext], SelectMGList);
                                SelectPipeOnFewPipe(Model.NavigationPrivList[indexNext], SelectNitList);
                                break;
                            }
                        case "10":
                            {
                                newUpdateItem = "2";

                                Model.GoCurrentZak();
                                int indexNext = Model.CurrentIndex;
                                InitPriv10Type(indexNext);
                                break;
                            }
                    }

                }

            }
            else
            {
                if (Model.PassportDetailModel.TYPEPRIV != "10")
                {
                    IsNextNewDataPipe = true;
                    ddlMG.ItemsSource = null;
                    ddlMG.ItemsSource = SelectMGList;
                    ddlMG.SelectedIndex = ddlMG.Items.IndexOf(SelectMGList[0]);    
                }
                
                ChildWindowAttantion _popupWindowAttantion = new ChildWindowAttantion();
                _popupWindowAttantion.titleBox.Text = ProjectResources.PrivTitleBoxErr + e.Result.ErrorMessage;
                _popupWindowAttantion.Show();
                Model.PassportDetailModel.MainModel.Report("ошибка создании привязки " + e.Result.ErrorMessage);
            }
        }


        //назад
        private void Back_OnClick(object sender, RoutedEventArgs e)
        {
            //Данные контролов
            index = Model.CurrentIndex;
            NavigationPriv np = new NavigationPriv("", "", "", "", "", "", "", "", "", "", "", "", "", 0, 0, "", "", "", "", "");

            var allDataFormForwardEnd = new AllDataForm(tbKmA, tbKmB, tbKm, tbd, dolg, dolgA, dolgB, shirot, shirotA, shirotB, rbr, radExpander, keyMg, keyNit, shirot1, dolg1);

            switch (Model.PassportDetailModel.TYPEPRIV)
            {
                case "5":
                    {
                        np = new NavigationPriv("", allDataFormForwardEnd.tbKmA1, allDataFormForwardEnd.tbd1, allDataFormForwardEnd.dolg1,
                                                                allDataFormForwardEnd.shirot1, "", "", "", "", "", "", "", "",
                                                                allDataFormForwardEnd.keyMgDouble, allDataFormForwardEnd.keyPipeDouble, "", "", "", "", "");
                        break;
                    }
                case "6":
                    {
                        np = new NavigationPriv("", allDataFormForwardEnd.tbKmA1, allDataFormForwardEnd.tbd1, allDataFormForwardEnd.dolgA1, allDataFormForwardEnd.shirotA1,
                                                               "", "", "", allDataFormForwardEnd.dolgB1, allDataFormForwardEnd.shirotB1, allDataFormForwardEnd.tbKmB1, "", "",
                                                               allDataFormForwardEnd.keyMgDouble, allDataFormForwardEnd.keyPipeDouble, "", "", "", "", "");
                        break;
                    }
                case "8":
                    {
                        np = new NavigationPriv("", allDataFormForwardEnd.tbKm1, "", allDataFormForwardEnd.dolg1, allDataFormForwardEnd.shirot1, "", "", "", "", "",
                                                             "", "", "", allDataFormForwardEnd.keyMgDouble, allDataFormForwardEnd.keyPipeDouble, "", "", "", "",
                                                             "");
                        break;
                    }
                case "9":
                    {
                        np = new NavigationPriv("", allDataFormForwardEnd.tbKmA1, "", allDataFormForwardEnd.dolgA1, allDataFormForwardEnd.shirotA1, "", "", "",
                              allDataFormForwardEnd.dolgB1, allDataFormForwardEnd.shirotB1, allDataFormForwardEnd.tbKmB1, "", "",
                              allDataFormForwardEnd.keyMgDouble, allDataFormForwardEnd.keyPipeDouble, "", "", "", "", "");
                        break;
                    }
                case "10":
                    {
                        np = new NavigationPriv("", "", "", allDataFormForwardEnd.dolg1_1, allDataFormForwardEnd.shirot1_1, "", "", "",
                              "", "", "", "", "", 0, 0, "", "", "", "", "");
                        break;
                    }
            }

            #region privazka  Back
          

            #region //10 тип если перешли на страницу добавления нового значения привязки, но не добавили и перешли назад(т.е. нить не определена)
            if (Model.PassportDetailModel.TYPEPRIV == "10")
            {
                
                if (string.IsNullOrEmpty(allDataFormForwardEnd.dolg1_1) && string.IsNullOrEmpty(allDataFormForwardEnd.shirot1_1))
                {
                    if (Model.CurrentIndex != 0)
                    {
                        newUpdateItem = "2";
                        int indexNext = Model.CurrentIndex;
                        InitPriv10Type(indexNext);    
                    }
                    
                    return;
                }
               
            }
            else
            {
                #region //если перешли на страницу добавления привязки, но не добавили и перешли назад(т.е. нить не определена)
                if (string.IsNullOrEmpty(keyNit))
                {

                    if ((Model.CurrentIndex > 0))
                    {

                        newUpdateItem = "2";
                        int indexNext = Model.CurrentIndex;

                        switch (Model.PassportDetailModel.TYPEPRIV)
                        {
                            case "5":
                            {
                                mainData.RowDefinitions[3].Height = new GridLength(35);
                                mainData.RowDefinitions[5].Height = new GridLength(35);
                                mainData.RowDefinitions[6].Height = new GridLength(35);
                                mainData.RowDefinitions[9].Height = new GridLength(35);

                                InitPriv5Type(indexNext);
                                break;
                            }
                            case "6":
                            {
                                mainData.RowDefinitions[3].Height = new GridLength(35);
                                mainData.RowDefinitions[5].Height = new GridLength(35);
                                mainData.RowDefinitions[6].Height = new GridLength(35);
                                mainData.RowDefinitions[9].Height = new GridLength(35);
                                InitPriv6Type(indexNext);
                                break;
                            }
                            case "8":
                            {

                                mainData.RowDefinitions[2].Height = new GridLength(35);
                                mainData.RowDefinitions[3].Height = new GridLength(0);
                                mainData.RowDefinitions[4].Height = new GridLength(0);
                                mainData.RowDefinitions[5].Height = new GridLength(0);
                                mainData.RowDefinitions[6].Height = new GridLength(0);

                                expander.RowDefinitions[0].Height = new GridLength(0);
                                expander.RowDefinitions[1].Height = new GridLength(0);
                                expander.RowDefinitions[2].Height = new GridLength(35);

                                expander.RowDefinitions[3].Height = new GridLength(0);
                                expander.RowDefinitions[4].Height = new GridLength(35);
                                expander.RowDefinitions[5].Height = new GridLength(35);


                                InitPriv8Type(indexNext);


                                break;

                            }
                            case "9":
                            {
                                InitPriv9Type(indexNext);
                                break;
                            }
                        }
                        if (indexNext != -1)
                        {
                            SelectMgOnFewPipe(Model.NavigationPrivList[indexNext], SelectMGList);
                            SelectPipeOnFewPipe(Model.NavigationPrivList[indexNext], SelectNitList);
                        }

                    }
                    else
                    {

                        newUpdateItem = "2";
                        int indexNext = Model.CurrentIndex;

                        if (indexNext != -1)
                        {


                            switch (Model.PassportDetailModel.TYPEPRIV)
                            {
                                case "5":
                                {
                                    mainData.RowDefinitions[3].Height = new GridLength(35);
                                    mainData.RowDefinitions[5].Height = new GridLength(35);
                                    mainData.RowDefinitions[6].Height = new GridLength(35);
                                    mainData.RowDefinitions[9].Height = new GridLength(35);
                                    InitPriv5Type(indexNext);
                                    break;
                                }
                                case "6":
                                {
                                    mainData.RowDefinitions[3].Height = new GridLength(35);
                                    mainData.RowDefinitions[4].Height = new GridLength(35);
                                    mainData.RowDefinitions[5].Height = new GridLength(35);
                                    mainData.RowDefinitions[6].Height = new GridLength(35);
                                    mainData.RowDefinitions[9].Height = new GridLength(35);
                                    InitPriv6Type(indexNext);
                                    break;
                                }
                                case "8":
                                {
                                    mainData.RowDefinitions[2].Height = new GridLength(35);
                                    mainData.RowDefinitions[3].Height = new GridLength(0);
                                    mainData.RowDefinitions[4].Height = new GridLength(0);
                                    mainData.RowDefinitions[5].Height = new GridLength(0);
                                    mainData.RowDefinitions[6].Height = new GridLength(0);

                                    expander.RowDefinitions[0].Height = new GridLength(0);
                                    expander.RowDefinitions[1].Height = new GridLength(0);
                                    expander.RowDefinitions[2].Height = new GridLength(35);

                                    expander.RowDefinitions[3].Height = new GridLength(0);
                                    expander.RowDefinitions[4].Height = new GridLength(35);
                                    expander.RowDefinitions[5].Height = new GridLength(35);

                                    InitPriv8Type(indexNext);
                                    break;

                                }
                                case "9":
                                {
                                    InitPriv9Type(indexNext);
                                    break;
                                }

                            }
                            SelectMgOnFewPipe(Model.NavigationPrivList[indexNext], SelectMGList);
                            SelectPipeOnFewPipe(Model.NavigationPrivList[indexNext], SelectNitList);
                        }
                    }
                    return;
                }

                #endregion
            }
            #endregion
            //создали новую привязку и кликнули назад(запрос в базу нужно только если вперед или назад)
            if (index == -1)
            {
                return;
            }
            else
            {
                //проверяем изменялась ли привязка
                if (Model.DataHasChanged(np))
                {

                    List<PrivItems> NavigationPrivListDouble = new List<PrivItems>();

                    //если есть данные
                    if (Model.NavigationPrivList.Count() > 0)
                    {
                        NavigationPrivListDouble = HelperPriv.GetNavigationPrivList(newUpdateItem, Model.NavigationPrivList, np, index, radExpander.IsExpanded, Model.PassportDetailModel.TYPEPRIV);
                        newUpdateItem = "2";

                    }

                    //запрос в базу - можно ли создать привязку

                    string xmlExport = HelperPriv.GetXmlInNewPriv(NavigationPrivListDouble, passportDetailModel);

                    if (!string.IsNullOrEmpty(xmlExport))
                    {
                        ServiceDataClient.GetCalcPrivJsonCompleted += ServiceDataClient_GetCalcPrivXmlBackCompleted;
                        ServiceDataClient.GetCalcPrivJsonAsync(Model.PassportDetailModel.PassportKey, xmlExport, GlobalContext.Context);
                    }
                    else
                    {
                        Model.PassportDetailModel.MainModel.Report("ошибка  формирования xml Next_OnClick");
                    }


                }
                //если не редактировалось
                else
                {
                    if (Model.CurrentIndex != 0)
                    {
                        Model.GoPrevZak();
                    }

                    newUpdateItem = "2";
                    int indexNext = Model.CurrentIndex;

                    switch (Model.PassportDetailModel.TYPEPRIV)
                    {
                        case "5":
                            {
                                mainData.RowDefinitions[3].Height = new GridLength(35);
                                mainData.RowDefinitions[5].Height = new GridLength(35);
                                mainData.RowDefinitions[6].Height = new GridLength(35);
                                mainData.RowDefinitions[9].Height = new GridLength(35);
                                InitPriv5Type(indexNext);
                                break;
                            }
                        case "6":
                            {
                                mainData.RowDefinitions[3].Height = new GridLength(35);
                                mainData.RowDefinitions[5].Height = new GridLength(35);
                                mainData.RowDefinitions[6].Height = new GridLength(35);
                                mainData.RowDefinitions[9].Height = new GridLength(35);
                                InitPriv6Type(indexNext);
                                break;
                            }
                        case "8":
                            {

                                mainData.RowDefinitions[2].Height = new GridLength(35);
                                mainData.RowDefinitions[3].Height = new GridLength(0);
                                mainData.RowDefinitions[4].Height = new GridLength(0);
                                mainData.RowDefinitions[5].Height = new GridLength(0);
                                mainData.RowDefinitions[6].Height = new GridLength(0);

                                expander.RowDefinitions[0].Height = new GridLength(0);
                                expander.RowDefinitions[1].Height = new GridLength(0);
                                expander.RowDefinitions[2].Height = new GridLength(35);

                                expander.RowDefinitions[3].Height = new GridLength(0);
                                expander.RowDefinitions[4].Height = new GridLength(35);
                                expander.RowDefinitions[5].Height = new GridLength(35);

                                InitPriv8Type(indexNext);
                                break;

                            }
                        case "9":
                            {
                                InitPriv9Type(indexNext);
                                break;
                            }
                        case "10":
                            {
                                InitPriv10Type(indexNext);
                                break;
                            }
                    }
                    if (Model.PassportDetailModel.TYPEPRIV != "10")
                    {
                        SelectMgOnFewPipe(Model.NavigationPrivList[indexNext], SelectMGList);
                        SelectPipeOnFewPipe(Model.NavigationPrivList[indexNext], SelectNitList);    
                    }
                    

                }
            }
            #endregion

        }
        //back
        void ServiceDataClient_GetCalcPrivXmlBackCompleted(object sender, GetCalcPrivJsonCompletedEventArgs e)
        {
            ServiceDataClient.GetCalcPrivJsonCompleted -= ServiceDataClient_GetCalcPrivXml2Completed;
            if (e.Result.IsValid)
            {
                int index = Model.CurrentIndex;

                string xmlToList = e.Result.CalcPrivOnEditJson_result.CalcPrivOnEditJsonOnKeyPassport;

                //конвертируем обратно из xml в структуру привязки
                List<PrivItems> outPrivxmlBaza = HelperPriv.GetXmlOut(xmlToList, passportDetailModel);
                if (Model.CurrentIndex == -1)
                {
                    index = index + 1;
                }
                //загружаем модель новыми данными
                //соответственно перестраивать привязку
                Model.LoadData(Model.PassportDetailModel.PassportKey, index + 1, outPrivxmlBaza);

                if (Model.NavigationPrivList.Count - 1 == Model.CurrentIndex)
                {
                    //загружаем новую страницу с данными

                    if (Model.CurrentIndex != 0)
                    {
                        Model.GoPrevZak();
                    }
                    newUpdateItem = "2";
                    int indexNext = Model.CurrentIndex;

                    switch (Model.PassportDetailModel.TYPEPRIV)
                    {
                        case "5":
                            {
                                mainData.RowDefinitions[3].Height = new GridLength(35);
                                mainData.RowDefinitions[5].Height = new GridLength(35);
                                mainData.RowDefinitions[6].Height = new GridLength(35);
                                mainData.RowDefinitions[9].Height = new GridLength(35);
                                InitPriv5Type(indexNext);
                                break;
                            }
                        case "6":
                            {
                                mainData.RowDefinitions[3].Height = new GridLength(35);
                                mainData.RowDefinitions[5].Height = new GridLength(35);
                                mainData.RowDefinitions[6].Height = new GridLength(35);
                                mainData.RowDefinitions[9].Height = new GridLength(35);
                                InitPriv6Type(indexNext);
                                break;
                            }
                        case "8":
                            {

                                mainData.RowDefinitions[2].Height = new GridLength(35);
                                mainData.RowDefinitions[3].Height = new GridLength(0);
                                mainData.RowDefinitions[4].Height = new GridLength(0);
                                mainData.RowDefinitions[5].Height = new GridLength(0);
                                mainData.RowDefinitions[6].Height = new GridLength(0);

                                expander.RowDefinitions[0].Height = new GridLength(0);
                                expander.RowDefinitions[1].Height = new GridLength(0);
                                expander.RowDefinitions[2].Height = new GridLength(35);

                                expander.RowDefinitions[3].Height = new GridLength(0);
                                expander.RowDefinitions[4].Height = new GridLength(35);
                                expander.RowDefinitions[5].Height = new GridLength(35);

                                InitPriv8Type(indexNext);
                                break;

                            }
                        case "9":
                            {
                                InitPriv9Type(indexNext);
                                break;
                            }
                        case "10":
                            {
                                InitPriv10Type(indexNext);
                                break;
                            }
                    }

                    SelectMgOnFewPipe(Model.NavigationPrivList[indexNext], SelectMGList);
                    SelectPipeOnFewPipe(Model.NavigationPrivList[indexNext], SelectNitList);


                }
                else
                {

                    switch (Model.PassportDetailModel.TYPEPRIV)
                    {
                        case "5":
                            {
                                mainData.RowDefinitions[2].Height = new GridLength(0);
                                mainData.RowDefinitions[4].Height = new GridLength(0);

                                expander.RowDefinitions[0].Height = new GridLength(0);
                                expander.RowDefinitions[1].Height = new GridLength(0);
                                expander.RowDefinitions[2].Height = new GridLength(0);
                                expander.RowDefinitions[3].Height = new GridLength(0);
                                newUpdateItem = "2";

                                Model.GoCurrentZak();
                                int indexNext = Model.CurrentIndex;
                                InitPriv5Type(indexNext);
                                SelectMgOnFewPipe(Model.NavigationPrivList[indexNext], SelectMGList);
                                SelectPipeOnFewPipe(Model.NavigationPrivList[indexNext], SelectNitList);
                                break;
                            }
                        case "6":
                            {
                                mainData.RowDefinitions[2].Height = new GridLength(0);
                                mainData.RowDefinitions[4].Height = new GridLength(0);

                                expander.RowDefinitions[0].Height = new GridLength(0);
                                expander.RowDefinitions[1].Height = new GridLength(0);
                                expander.RowDefinitions[2].Height = new GridLength(0);
                                expander.RowDefinitions[3].Height = new GridLength(0);
                                newUpdateItem = "2";

                                Model.GoCurrentZak();
                                int indexNext = Model.CurrentIndex;
                                InitPriv6Type(indexNext);
                                SelectMgOnFewPipe(Model.NavigationPrivList[indexNext], SelectMGList);
                                SelectPipeOnFewPipe(Model.NavigationPrivList[indexNext], SelectNitList);
                                break;
                            }
                        case "8":
                            {
                                Model.GoCurrentZak();
                                int indexNext = Model.CurrentIndex;
                                InitPriv8Type(indexNext);
                                SelectMgOnFewPipe(Model.NavigationPrivList[indexNext], SelectMGList);
                                SelectPipeOnFewPipe(Model.NavigationPrivList[indexNext], SelectNitList);
                                break;
                            }
                        case "9":
                            {
                                Model.GoCurrentZak();
                                int indexNext = Model.CurrentIndex;
                                InitPriv9Type(indexNext);
                                SelectMgOnFewPipe(Model.NavigationPrivList[indexNext], SelectMGList);
                                SelectPipeOnFewPipe(Model.NavigationPrivList[indexNext], SelectNitList);
                                break;
                            }
                        case "10":
                            {
                                Model.GoCurrentZak();
                                int indexNext = Model.CurrentIndex;
                                InitPriv10Type(indexNext);
                                break;
                            }
                    }

                }

            }
            else
            {
                if (Model.PassportDetailModel.TYPEPRIV != "10")
                {
                    IsNextNewDataPipe = true;
                    ddlMG.ItemsSource = null;
                    ddlMG.ItemsSource = SelectMGList;
                    ddlMG.SelectedIndex = ddlMG.Items.IndexOf(SelectMGList[0]);
                }

                ChildWindowAttantion _popupWindowAttantion = new ChildWindowAttantion();
                _popupWindowAttantion.titleBox.Text = ProjectResources.PrivTitleBoxErr + e.Result.ErrorMessage;
                _popupWindowAttantion.Show();
                Model.PassportDetailModel.MainModel.Report("ошибка создании привязки " + e.Result.ErrorMessage);
            }
        }



        //переход на последнюю запись
        private void ForwardEnd_OnClick(object sender, RoutedEventArgs e)
        {
            //Данные контролов
            index = Model.CurrentIndex;
            NavigationPriv np = new NavigationPriv("", "", "", "", "", "", "", "", "", "", "", "", "", 0, 0, "", "", "", "", "");

            var allDataFormForwardEnd = new AllDataForm(tbKmA, tbKmB, tbKm, tbd, dolg, dolgA, dolgB, shirot, shirotA, shirotB, rbr, radExpander, keyMg, keyNit, shirot1, dolg1);

            switch (Model.PassportDetailModel.TYPEPRIV)
            {
                case "5":
                    {
                        np = new NavigationPriv(nameNit, allDataFormForwardEnd.tbKmA1, allDataFormForwardEnd.tbd1, allDataFormForwardEnd.dolg1,
                                                                allDataFormForwardEnd.shirot1, "", "", "", "", "", "", "", "",
                                                                allDataFormForwardEnd.keyMgDouble, allDataFormForwardEnd.keyPipeDouble, "", "", "", "", "");
                        break;
                    }
                case "6":
                    {
                        np = new NavigationPriv(nameNit, allDataFormForwardEnd.tbKmA1, allDataFormForwardEnd.tbd1, allDataFormForwardEnd.dolg1,
                                                               allDataFormForwardEnd.shirot1, "", "", "", "", "", "", "", "",
                                                               allDataFormForwardEnd.keyMgDouble, allDataFormForwardEnd.keyPipeDouble, "", "", "", "", "");
                        break;
                    }
                case "8":
                    {
                        np = new NavigationPriv("", allDataFormForwardEnd.tbKm1, "", allDataFormForwardEnd.dolg1, allDataFormForwardEnd.shirot1, "", "", "", "", "",
                                                             "", "", "", allDataFormForwardEnd.keyMgDouble, allDataFormForwardEnd.keyPipeDouble, "", "", "", "",
                                                             "");
                        break;
                    }
                case "9":
                    {
                        np = new NavigationPriv("", allDataFormForwardEnd.tbKmA1, "", allDataFormForwardEnd.dolgA1, allDataFormForwardEnd.shirotA1, "", "", "",
                              allDataFormForwardEnd.dolgB1, allDataFormForwardEnd.shirotB1, allDataFormForwardEnd.tbKmB1, "", "",
                              allDataFormForwardEnd.keyMgDouble, allDataFormForwardEnd.keyPipeDouble, "", "", "", "", "");
                        break;
                    }
                case "10":
                    {
                        np = new NavigationPriv("", "", "", allDataFormForwardEnd.dolg1_1, allDataFormForwardEnd.shirot1_1, "", "", "",
                              "", "", "", "", "", 0, 0, "", "", "", "", "");
                        break;
                    }
            }

            if (Model.PassportDetailModel.TYPEPRIV != "10")
            {
               
                if (string.IsNullOrEmpty(keyNit))
                {
                    return;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(allDataFormForwardEnd.dolg1_1) &&
                    string.IsNullOrEmpty(allDataFormForwardEnd.shirot1_1))
                {
                    return;
                }
            }

            //проверяем изменялась ли привязка

            if (Model.DataHasChanged(np))
            {

                List<PrivItems> NavigationPrivListDouble = new List<PrivItems>();

                //если есть данные
                if (Model.NavigationPrivList.Count() > 0)
                {
                    NavigationPrivListDouble = HelperPriv.GetNavigationPrivList(newUpdateItem, Model.NavigationPrivList, np, index, radExpander.IsExpanded, Model.PassportDetailModel.TYPEPRIV);
                    newUpdateItem = "2";

                }
                else
                {
                    //создаем новую привязку
                    List<NavigationPriv> lnp = new List<NavigationPriv>();
                    lnp.Add(np);
                    NavigationPrivListDouble = HelperPriv.GetNavigationPrivListDoubleOne(lnp, NavigationPrivListDouble, radExpander.IsExpanded);
                    newUpdateItem = "1";

                }
                //запрос в базу - можно ли создать привязку

                string xmlExport = HelperPriv.GetXmlInNewPriv(NavigationPrivListDouble, passportDetailModel);

                if (!string.IsNullOrEmpty(xmlExport))
                {
                    ServiceDataClient.GetCalcPrivJsonCompleted += ServiceDataClient_GetCalcPrivXml2Completed;
                    ServiceDataClient.GetCalcPrivJsonAsync(Model.PassportDetailModel.PassportKey, xmlExport, GlobalContext.Context);
                }
                else
                {
                    Model.PassportDetailModel.MainModel.Report("ошибка  формирования xml Next_OnClick");
                }

            }
            //если не редактировалось
            else
            {
                if (Model.CurrentIndex != Model.NavigationPrivList.Count - 1)
                {
                    Model.GoLastZak();
                    int indexNext = Model.MaxIndex;
                    switch (Model.PassportDetailModel.TYPEPRIV)
                    {
                        case "5":
                            {
                                InitPriv5Type(indexNext);
                                break;
                            }
                        case "6":
                            {
                                InitPriv6Type(indexNext);
                                break;
                            }
                        case "8":
                            {
                                InitPriv8Type(indexNext);
                                break;
                            }
                        case "9":
                            {
                                InitPriv9Type(indexNext);
                                break;
                            }
                        case "10":
                            {
                                InitPriv10Type(indexNext);
                                break;
                            }
                    }

                    if (Model.PassportDetailModel.TYPEPRIV != "10")
                    {
                        SelectMgOnFewPipe(Model.NavigationPrivList[indexNext], SelectMGList);
                        SelectPipeOnFewPipe(Model.NavigationPrivList[indexNext], SelectNitList);     
                    }
                   
                }

            }



        }
        //переход на первый итем
        private void BackEnd_OnClick(object sender, RoutedEventArgs e)
        {

            index = Model.CurrentIndex;
            NavigationPriv np = new NavigationPriv("", "", "", "", "", "", "", "", "", "", "", "", "", 0, 0, "", "", "", "", "");

            var allDataFormForwardEnd = new AllDataForm(tbKmA, tbKmB, tbKm, tbd, dolg, dolgA, dolgB, shirot, shirotA, shirotB, rbr, radExpander, keyMg, keyNit, shirot1, dolg1);

            switch (Model.PassportDetailModel.TYPEPRIV)
            {
                case "5":
                    {
                        np = new NavigationPriv(nameNit, allDataFormForwardEnd.tbKmA1, allDataFormForwardEnd.tbd1, allDataFormForwardEnd.dolg1,
                                                                allDataFormForwardEnd.shirot1, "", "", "", "", "", "", "", "",
                                                                allDataFormForwardEnd.keyMgDouble, allDataFormForwardEnd.keyPipeDouble, "", "", "", "", "");
                        break;
                    }
                case "6":
                    {
                        np = new NavigationPriv(nameNit, allDataFormForwardEnd.tbKmA1, allDataFormForwardEnd.tbd1, allDataFormForwardEnd.dolg1,
                                                               allDataFormForwardEnd.shirot1, "", "", "", "", "", "", "", "",
                                                               allDataFormForwardEnd.keyMgDouble, allDataFormForwardEnd.keyPipeDouble, "", "", "", "", "");
                        break;
                    }
                case "8":
                    {
                        np = new NavigationPriv("", allDataFormForwardEnd.tbKm1, "", allDataFormForwardEnd.dolg1, allDataFormForwardEnd.shirot1, "", "", "", "", "",
                                                             "", "", "", allDataFormForwardEnd.keyMgDouble, allDataFormForwardEnd.keyPipeDouble, "", "", "", "",
                                                             "");
                        break;
                    }
                case "9":
                    {
                        np = new NavigationPriv("", allDataFormForwardEnd.tbKmA1, "", allDataFormForwardEnd.dolgA1, allDataFormForwardEnd.shirotA1, "", "", "",
                              allDataFormForwardEnd.dolgB1, allDataFormForwardEnd.shirotB1, allDataFormForwardEnd.tbKmB1, "", "",
                              allDataFormForwardEnd.keyMgDouble, allDataFormForwardEnd.keyPipeDouble, "", "", "", "", "");
                        break;
                    }
                case "10":
                    {
                        np = new NavigationPriv("", "", "", allDataFormForwardEnd.dolg1_1, allDataFormForwardEnd.shirot1_1, "", "", "",
                              "", "", "", "", "", 0, 0, "", "", "", "", "");
                        break;
                    }
            }

            if (Model.PassportDetailModel.TYPEPRIV != "10")
            {

                if (string.IsNullOrEmpty(keyNit))
                {
                    if (Model.CurrentIndex > 0)
                    {
                        int indexNext = Model.CurrentIndex;
                        switch (Model.PassportDetailModel.TYPEPRIV)
                        {
                            case "5":
                                {
                                    mainData.RowDefinitions[3].Height = new GridLength(35);
                                    mainData.RowDefinitions[5].Height = new GridLength(35);
                                    mainData.RowDefinitions[6].Height = new GridLength(35);
                                    mainData.RowDefinitions[9].Height = new GridLength(35);
                                    InitPriv5Type(indexNext);
                                    break;
                                }
                            case "6":
                                {
                                    mainData.RowDefinitions[3].Height = new GridLength(35);
                                    mainData.RowDefinitions[5].Height = new GridLength(35);
                                    mainData.RowDefinitions[6].Height = new GridLength(35);
                                    mainData.RowDefinitions[9].Height = new GridLength(35);
                                    InitPriv6Type(indexNext);
                                    break;
                                }
                            case "8":
                                {

                                    mainData.RowDefinitions[2].Height = new GridLength(35);
                                    mainData.RowDefinitions[3].Height = new GridLength(0);
                                    mainData.RowDefinitions[4].Height = new GridLength(0);
                                    mainData.RowDefinitions[5].Height = new GridLength(0);
                                    mainData.RowDefinitions[6].Height = new GridLength(0);

                                    expander.RowDefinitions[0].Height = new GridLength(0);
                                    expander.RowDefinitions[1].Height = new GridLength(0);
                                    expander.RowDefinitions[2].Height = new GridLength(35);

                                    expander.RowDefinitions[3].Height = new GridLength(0);
                                    expander.RowDefinitions[4].Height = new GridLength(35);
                                    expander.RowDefinitions[5].Height = new GridLength(35);

                                    InitPriv8Type(indexNext);
                                    break;

                                }
                            case "9":
                                {
                                    InitPriv9Type(indexNext);
                                    break;
                                }


                            //mainData.RowDefinitions[3].Height = new GridLength(35);
                            //mainData.RowDefinitions[5].Height = new GridLength(35);
                            //mainData.RowDefinitions[6].Height = new GridLength(35);
                            //radExpander.Visibility = Visibility.Visible;
                            ////mainData.RowDefinitions[7].Height = new GridLength(35);

                            ////Model.GoPrevZak();
                            //int indexNext = Model.CurrentIndex;
                            //InitPriv5Type(indexNext);
                            //SelectMgOnFewPipe(Model.NavigationPrivList[indexNext], SelectMGList);
                            //SelectPipeOnFewPipe(Model.NavigationPrivList[indexNext], SelectNitList);

                        }
                    }
                    return;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(allDataFormForwardEnd.dolg1_1) &&
                    string.IsNullOrEmpty(allDataFormForwardEnd.shirot1_1))
                {
                    int indexNext = Model.CurrentIndex;
                    if (Model.CurrentIndex > 0)
                    {
                        InitPriv10Type(indexNext);
                       
                    }
                    return;
                   
                }
            }

                //проверяем изменялась ли привязка
                if (Model.DataHasChanged(np))
                {

                    List<PrivItems> NavigationPrivListDouble = new List<PrivItems>();

                    //если есть данные
                    if (Model.NavigationPrivList.Count() > 0)
                    {
                        NavigationPrivListDouble = HelperPriv.GetNavigationPrivList(newUpdateItem, Model.NavigationPrivList, np, index, radExpander.IsExpanded, Model.PassportDetailModel.TYPEPRIV);
                        newUpdateItem = "2";

                    }

                    //запрос в базу - можно ли создать привязку

                    string xmlExport = HelperPriv.GetXmlInNewPriv(NavigationPrivListDouble, passportDetailModel);

                    if (!string.IsNullOrEmpty(xmlExport))
                    {
                        ServiceDataClient.GetCalcPrivJsonCompleted += ServiceDataClient_GetCalcPrivXml2Completed;
                        ServiceDataClient.GetCalcPrivJsonAsync(Model.PassportDetailModel.PassportKey, xmlExport, GlobalContext.Context);
                    }
                    else
                    {
                        Model.PassportDetailModel.MainModel.Report("ошибка  формирования xml Next_OnClick");
                    }
                }
                //если не редактировалось
                else
                {
                    if (Model.CurrentIndex != 0)
                    {
                        Model.GoFistZak();
                        int indexNext = Model.FistIndex;
                        switch (Model.PassportDetailModel.TYPEPRIV)
                        {
                            case "5":
                                {
                                    InitPriv5Type(indexNext);
                                    break;
                                }
                            case "6":
                                {
                                    InitPriv6Type(indexNext);
                                    break;
                                }
                            case "8":
                                {
                                    InitPriv8Type(indexNext);
                                    break;
                                }
                            case "9":
                                {
                                    InitPriv9Type(indexNext);
                                    break;
                                }
                            case "10":
                                {
                                    InitPriv10Type(indexNext);
                                    break;
                                }
                        }

                        if (Model.PassportDetailModel.TYPEPRIV != "10")
                        {
                            SelectMgOnFewPipe(Model.NavigationPrivList[indexNext], SelectMGList);
                            SelectPipeOnFewPipe(Model.NavigationPrivList[indexNext], SelectNitList);    
                        }
                        
                    }
                }
        }

        //из xml в структуру привязки...
        //private List<PrivItems> GetXmlOut(string xmlString)
        //{

        //    List<PrivItems> result = new List<PrivItems>();
        //    SavePriv.LISTSAVEPRIV listsavpriv = new SavePriv.LISTSAVEPRIV();
        //    listsavpriv.LISTPRIV = new List<SavePriv.SAVEPRIVITEMS>();
        //    try
        //    {
        //        //listsavpriv = JsonHelper.JsonDeserialize<SavePriv.ListSavePriv>(jsonString);
        //        listsavpriv = XmlHelper.Deserialize<SavePriv.LISTSAVEPRIV>(xmlString);
        //        string nkey = "";

        //        for (int ii = 0; ii < listsavpriv.LISTPRIV.Count; ii++)
        //        {
        //            if (string.IsNullOrEmpty(listsavpriv.LISTPRIV[ii].NKEY))
        //            {
        //                nkey = ii.ToString();
        //            }
        //            else
        //            {
        //                nkey = listsavpriv.LISTPRIV[ii].NKEY;
        //            }
        //            result.Add(
        //                   new PrivItems
        //                   {

        //                       CNAME = listsavpriv.LISTPRIV[ii].CNAME,
        //                       NKMORT1 = listsavpriv.LISTPRIV[ii].NKMORT1,
        //                       NDISTANCEBEG = listsavpriv.LISTPRIV[ii].NDISTANCEBEG,
        //                       NX1 = listsavpriv.LISTPRIV[ii].NX1,
        //                       NY1 = listsavpriv.LISTPRIV[ii].NY1,
        //                       NZ1 = listsavpriv.LISTPRIV[ii].NZ1,
        //                       NKEY = nkey,
        //                       NH1 = listsavpriv.LISTPRIV[ii].NH1,
        //                       NKMTRUE1 = listsavpriv.LISTPRIV[ii].NKMTRUE1,
        //                       NX2 = listsavpriv.LISTPRIV[ii].NX2,
        //                       NY2 = listsavpriv.LISTPRIV[ii].NY2,
        //                       NZ2 = listsavpriv.LISTPRIV[ii].NZ2,
        //                       NH2 = listsavpriv.LISTPRIV[ii].NH2,
        //                       NKMORT2 = listsavpriv.LISTPRIV[ii].NKMORT2,
        //                       NKMTRUE2 = listsavpriv.LISTPRIV[ii].NKMTRUE2,
        //                       NDISTANCEEND = listsavpriv.LISTPRIV[ii].NDISTANCEEND,
        //                       nMtKey = listsavpriv.LISTPRIV[ii].NMTKEY,
        //                       nPipeKey = listsavpriv.LISTPRIV[ii].NPIPEKEY,
        //                       NBUILDTYPE = listsavpriv.LISTPRIV[ii].NBUILDTYPE,
        //                       ISEDITED = listsavpriv.LISTPRIV[ii].ISEDITED

        //                   });

        //        }



        //        //result = JsonHelper.JsonDeserialize<List<PrivItems>>(jsonString);
        //    }
        //    catch (Exception ex)
        //    {

        //        Model.PassportDetailModel.MainModel.Report("ошибка XmlDeserialize jsonString =  " + xmlString + ex.Message);
        //    }
        //    return result;
        //}
        ////
        //private string GetXmlInNewPriv(List<PrivItems> ListprivItems)
        //{
        //    string xmls = "";

        //    SavePriv.LISTSAVEPRIV listsavpriv = new SavePriv.LISTSAVEPRIV();
        //    listsavpriv.LISTPRIV = new List<SavePriv.SAVEPRIVITEMS>();

        //    try
        //    {


        //        if (ListprivItems.Count > 0)
        //        {
        //            for (int ii = 0; ii < ListprivItems.Count; ii++)
        //            {
        //                SavePriv.SAVEPRIVITEMS privItems = new SavePriv.SAVEPRIVITEMS();

        //                privItems.CNAME = ListprivItems[ii].CNAME;
        //                privItems.NDISTANCEBEG = ListprivItems[ii].NDISTANCEBEG;
        //                privItems.NDISTANCEEND = ListprivItems[ii].NDISTANCEEND;
        //                privItems.NH1 = ListprivItems[ii].NH1;
        //                privItems.NH2 = ListprivItems[ii].NH2;
        //                privItems.NKEY = ListprivItems[ii].NKEY;
        //                privItems.NKMORT1 = ListprivItems[ii].NKMORT1;
        //                privItems.NKMORT2 = ListprivItems[ii].NKMORT2;
        //                privItems.NKMTRUE1 = ListprivItems[ii].NKMTRUE1;
        //                privItems.NKMTRUE2 = ListprivItems[ii].NKMTRUE2;
        //                privItems.NX1 = ListprivItems[ii].NX1;
        //                privItems.NX2 = ListprivItems[ii].NX2;
        //                privItems.NY1 = ListprivItems[ii].NY1;
        //                privItems.NY2 = ListprivItems[ii].NY2;
        //                privItems.NZ1 = ListprivItems[ii].NZ1;
        //                privItems.NZ2 = ListprivItems[ii].NZ2;
        //                privItems.NMTKEY = ListprivItems[ii].nMtKey;
        //                privItems.NPIPEKEY = ListprivItems[ii].nPipeKey;
        //                privItems.NBUILDTYPE = ListprivItems[ii].NBUILDTYPE;
        //                privItems.ISEDITED = ListprivItems[ii].ISEDITED;

        //                listsavpriv.LISTPRIV.Add(privItems);
        //            }
        //        }

        //        // strJson = JsonHelper.JsonSerializer<SavePriv.ListSavePriv>(listsavpriv);
        //        xmls = XmlHelper.InternalSerializer<SavePriv.LISTSAVEPRIV>(listsavpriv);
        //    }
        //    catch (Exception ex)
        //    {

        //        Model.PassportDetailModel.MainModel.Report("ошибка InternalSerializer jsonString =  " + ex.Message);
        //    }
        //    return xmls;
        //}

        private void TabInPriv10_OnSelectionChanged(object sender, RadSelectionChangedEventArgs e)
        {
            if (Model.PassportDetailModel.TYPEPRIV == "10")
            {
                if (TabInPriv10 != null)
                {
                    if (TabInPriv10.SelectedItem is RadTabItem)
                    {
                        var tab = TabInPriv10.SelectedItem as RadTabItem;

                        if (tab.Name.Equals("tabItemGrid", StringComparison.OrdinalIgnoreCase))
                        {
                            List<NavigationPriv> navigationPrivList = GetNavigationPrivInGrid();
                            LayoutRoot.RowDefinitions[4].Height = new GridLength(0);
                            TreeHolder.Children.Clear();
                            //gridType10 = PrivInGrid.CreateGrid(Model.NavigationPrivList, Model.PassportDetailModel);
                            gridType10 = PrivInGrid.CreateGrid(navigationPrivList, Model.PassportDetailModel);
                            TreeHolder.Children.Add(gridType10);
                        }
                        if (tab.Name.Equals("tabItemCoor", StringComparison.OrdinalIgnoreCase))
                        {
                            List<NavigationPriv> navigationPrivList = new List<NavigationPriv>();
                            if (gridType10.IsGrid)
                            {

                                navigationPrivList = gridType10.GetNavigationPriv();
                                NavigationPrivListDouble = new List<PrivItems>();
                                NavigationPrivListDouble =
                                    HelperPriv.GetNavigationPrivListDoubleOnType10Grid(navigationPrivList);
                                Model = null;
                                Model = new PrivChildModel(key, isEdited, NavigationPrivListDouble, passportDetailModel,
                                                           typeLink);

                                //tabItemCoor.IsSelected = false;
                                //tabItemGrid.IsSelected = true;

                            }
                            LayoutRoot.RowDefinitions[4].Height = new GridLength(40);
                            CreateWindowNewPriv(Model.NavigationPrivList);


                        }
                    }
                }
           
                
            }
        }

        //для 10 типа привязки - если на вкладке координаты набрали новую привязку и нажали перейти  в таблицу
        private List<NavigationPriv> GetNavigationPrivInGrid()
        {
            List<NavigationPriv> result = new List<NavigationPriv>();

            NavigationPrivListDouble = new List<PrivItems>();

            //Данные контролов
            index = Model.CurrentIndex;
            NavigationPriv np = new NavigationPriv("", "", "", "", "", "", "", "", "", "", "", "", "", 0, 0, "", "", "", "", "");

            var allDataFormForwardEnd = new AllDataForm(tbKmA, tbKmB, tbKm, tbd, dolg, dolgA, dolgB, shirot, shirotA, shirotB, rbr, radExpander, keyMg, keyNit, shirot1, dolg1);

            np = new NavigationPriv("", "", "", allDataFormForwardEnd.dolg1_1, allDataFormForwardEnd.shirot1_1, "", "", "",
                              "", "", "", "", "", 0, 0, "", "", "", "", "");


               if (string.IsNullOrEmpty(allDataFormForwardEnd.dolg1_1) &&
                    string.IsNullOrEmpty(allDataFormForwardEnd.shirot1_1))
                {
                    if (Model.NavigationPrivList.Count() > 0)
                    {
                        result = Model.NavigationPrivList;
                    }
                    return result;
                }

            //создали новую привязку и кликнули вперед
            if (index == -1)
            {
                //создаем новую привязку
                List<NavigationPriv> lnp = new List<NavigationPriv>();
                lnp.Add(np);
                result = lnp;

            }
            else
            {
                //проверяем изменялась ли привязка
                if (Model.DataHasChanged(np))
                {
                    //если есть данные
                    if (Model.NavigationPrivList.Count() > 0)
                    {
                      
                        result = HelperPriv.GetNavigationPrivListInGrid10Type(newUpdateItem,Model.NavigationPrivList, np, index,radExpander.IsExpanded);
                    }

                   

                }
                    //если не редактировалось
                else
                {
                    result = Model.NavigationPrivList;
                }
            }

            return result;
        }
        private void ChildWindowNewPriv_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            
        }
    }
}

