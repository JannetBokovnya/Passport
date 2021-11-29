using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Media.Interfaces;
using Passpot.Business;
using Media.Resources;
using Services.ServiceReference;

namespace Passpot.Controls
{
    public partial class ChildWindowPriv : ChildWindow
    {
        private ServiceDataClient _serviceDataClient;

        private List<DataMGList> SelectMGList;
        private List<DataNitList> SelectNitList;
        //private List<FormParamsPrivItems> FormParams;
        private string result = "";
        private ChildWindowAttantion _popupWindowAttantion;
        private int value = 0;
        private string keyMg;
        private string keyNit;
        private int CountPriv = 0; //количество привязок в таблице контрола добавления привязки
        private string separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        

        private PassportDetailModel Model
        {
            get { return this.DataContext as PassportDetailModel; }
        }

        protected ServiceDataClient ServiceDataClient
        {
            get
            {
                if (_serviceDataClient == null)
                    _serviceDataClient = new ServiceDataClient();
                return _serviceDataClient;
            }
        }

        public NumberModel NumberModel
        {
            get { return DataContext as NumberModel; }
        }

        public ChildWindowPriv()
        {
            InitializeComponent();
            // var numbermodel = new NumberModel(metaData, attrOneContr, value);
            //InitDDL_MG();
            //InitConstraint_Add();


            //подключаем проверку на коректность данных - числа
            FieldMetaDataItem metaData = new FieldMetaDataItem();
            metaData.TYPEVALIDATION = 1;

            var numbermodel_txtXBegin = new NumberModel(metaData, null, "");
            txtXBegin.DataContext = numbermodel_txtXBegin;
            numbermodel_txtXBegin.IsValidate = true;

            var numbermodel_txtYBegin = new NumberModel(metaData, null, "");
            txtYBegin.DataContext = numbermodel_txtYBegin;
            numbermodel_txtYBegin.IsValidate = true;
           

            var numbermodel_txtKmBeginGeod = new NumberModel(metaData, null, "");
            txtKmBeginGeod.DataContext = numbermodel_txtKmBeginGeod;
            numbermodel_txtKmBeginGeod.IsValidate = true;

            var numbermodel_txtKmBeginIst = new NumberModel(metaData, null, "");
            txtKmBeginIst.DataContext = numbermodel_txtKmBeginIst;
            numbermodel_txtKmBeginIst.IsValidate = true;
           

        }

        public void InitConstraint_Add(int countPriv)
        {
            CountPriv = countPriv;
            InitDDL_MG();

            switch (Model.TYPEPRIV)
            {
                case "1":
                case "8":
                    {
                        gbEnd.Visibility = Visibility.Collapsed;
                        gbBeginkm.Header = "";
                        gbBeginkm.Foreground = new SolidColorBrush(Colors.White);
                        gbEndkm.Visibility = Visibility.Collapsed;
                        tbKm_ist.Visibility = Visibility.Collapsed;
                        txtKmBeginIst.Visibility = Visibility.Collapsed;
                        txtKm_Raspol.Visibility = Visibility.Collapsed;
                        spRaspol.Visibility = Visibility.Collapsed;
                        buCreatePrivt5.Visibility = Visibility.Collapsed;
                        
                        break;
                    }
                case "2":
                    {
                        gbEnd.Visibility = Visibility.Collapsed;
                        gbBeginkm.Header = "";
                        gbEndkm.Visibility = Visibility.Collapsed;
                       // tbKm_ist.Text = "Расстояние, м ";
                        buCreatePrivt5.Visibility = Visibility.Collapsed;
                        break;
                    }
                case "3":
                case "9":
                    {
                        if (CountPriv == 0)
                        {
                            txtKm_Raspol.Visibility = Visibility.Collapsed;
                            spRaspol.Visibility = Visibility.Collapsed;
                            tbKm_ist.Visibility = Visibility.Collapsed;
                            txtKmBeginIst.Visibility = Visibility.Collapsed;
                            buCreatePrivt5.Visibility = Visibility.Collapsed;    
                        }
                        else
                        {
                            //добавляем привязку только по нити.
                            generalTabControlPrivNit.Visibility = Visibility.Collapsed;
                            buCreatePrivt5.Visibility = Visibility.Visible;
                            
                        }
                        
                        break;
                    }
                case "4":
                    {
                        buCreatePrivt5.Visibility = Visibility.Collapsed;
                       // tbKm_ist.Text = "Расстояние, м ";
                        break;
                    }
                case "5":
                    {
                        if (CountPriv == 0)
                        {
                            gbEnd.Visibility = Visibility.Collapsed;
                            gbBeginkm.Header = "";
                            gbEndkm.Visibility = Visibility.Collapsed;
                           // tbKm_ist.Text = "Расстояние, м ";
                            buCreatePrivt5.Visibility = Visibility.Collapsed;
                            break;

                        }
                        else
                        {
                            //добавляем привязку только по нити.
                            generalTabControlPrivNit.Visibility = Visibility.Collapsed;
                            buCreatePrivt5.Visibility = Visibility.Visible;
                            break;
                        }
                        
                    }
                case "6":
                    {
                        if (CountPriv == 0)
                        {
                            buCreatePrivt5.Visibility = Visibility.Collapsed;
                           // tbKm_ist.Text = "Расстояние, м ";
                            break;
                        }
                        else
                        {
                            //добавляем привязку только по нити.
                            generalTabControlPrivNit.Visibility = Visibility.Collapsed;
                            buCreatePrivt5.Visibility = Visibility.Visible;
                            break;
                        }
                        
                    }
                case "7":
                    {
                        MgNit.Visibility = Visibility.Collapsed;
                        tbPrivKmTabItem.Visibility = Visibility.Collapsed;
                        //PrivKm.Visibility = Visibility.Collapsed;
                        tbPrivCoordTabItem.IsSelected = true;
                        gbEnd.Visibility = Visibility.Collapsed;
                        break;
                    }
               
                case "10":
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

        }


        private void InitDDL_MG()
        {
            ServiceDataClient.GetDataMGListCompleted += ServiceDataClient_GetDataMGListCompleted;
            ServiceDataClient.GetDataMGListAsync(GlobalContext.Context);


        }

        void ServiceDataClient_GetDataMGListCompleted(object sender, GetDataMGListCompletedEventArgs e)
        {
            ServiceDataClient.GetDataMGListCompleted -= ServiceDataClient_GetDataMGListCompleted;
            if (e.Result.IsValid)
            {
                SelectMGList = new List<DataMGList>();
                SelectMGList.Add(new DataMGList { KEYMG = "-1", NAMEMG = ProjectResources.PrivSelectMg });

                for (int ii = 0; ii < e.Result.DataMGLists.Count; ii++)
                {
                    SelectMGList.Add(new DataMGList { KEYMG = e.Result.DataMGLists[ii].KEYMG, NAMEMG = e.Result.DataMGLists[ii].NAMEMG }); 
                }
               
                ddlMG.ItemsSource = SelectMGList;
                ddlMG.SelectedIndex = ddlMG.Items.IndexOf(SelectMGList[0]);
                
            }
            else
            {
                Model.MainModel.Report("Список MG err = " + e.Result.ErrorMessage);
            }
        }

        private void ddlMG_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            if ((ddlMG.SelectedItem != null) && (ddlMG.SelectedIndex > 0))
            {
                keyMg = ((DataMGList)ddlMG.SelectedItem).KEYMG;
                ServiceDataClient.GetDataNitListCompleted += ServiceDataClient_GetDataNitListCompleted;
                ServiceDataClient.GetDataNitListAsync(keyMg, GlobalContext.Context);

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
                //ddlNitka.SelectedIndex = ddlNitka.Items.IndexOf(SelectNitList[0]);
            }
            else
            {
                Model.MainModel.Report("Список Nit err = " + e.Result.ErrorMessage);
            }
        }

        //void ServiceDataClient_BuildSegmentLocationByPipeKMCompleted(object sender, BuildSegmentLocationByPipeKMCompletedEventArgs e)
        //{
        //    if (!e.Result.IsValid)
        //    {
        //        _popupWindowAttantion = new ChildWindowAttantion();
                  
        //        _popupWindowAttantion.titleBox.Text =  ProjectResources.PrivTitleBoxErr + e.Result.ErrorMessage;
        //        _popupWindowAttantion.Show();
        //        Model.MainModel.Report("Ошибка создания привязки = " + e.Result.ErrorMessage);
        //    }
        //    else
        //    {
        //        Model.ReloadGridPriv = "1";
        //        this.DialogResult = true;

        //    }
        //}

        //void ServiceDataClient_AddlinktopipeCompleted(object sender, AddlinktopipeCompletedEventArgs e)
        //{
        //    ServiceDataClient.AddlinktopipeCompleted -= (ServiceDataClient_AddlinktopipeCompleted);
        //    if (!e.Result.IsValid)
        //    {
        //        _popupWindowAttantion = new ChildWindowAttantion();
        //        _popupWindowAttantion.titleBox.Text = ProjectResources.PrivTitleBoxErr + e.Result.ErrorMessage;
        //        _popupWindowAttantion.Show();
        //        Model.MainModel.Report("Ошибка создания привязки = " + e.Result.ErrorMessage);
        //    }
        //    else
        //    {
        //        Model.ReloadGridPriv = "1";
        //        this.DialogResult = true;

        //    }
        //}

       

        private void ddlNitka_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            //if ((ddlNitka.SelectedItem != null) && (ddlNitka.SelectedIndex > 0))
            if ((ddlNitka.SelectedItem != null))
            {
                keyNit = ((DataNitList)ddlNitka.SelectedItem).KEYNIT;

            }
        }

        private decimal ToDecimal(string Value)
        {
            if (Value.Length == 0)
                return 0;
            else
                return Decimal.Parse(Value.Replace(",", "."), NumberStyles.AllowThousands
           | NumberStyles.AllowDecimalPoint | NumberStyles.AllowCurrencySymbol);
        }

        //сохранение привязки по километражу
        private void buCreatePrivKm_Click(object sender, RoutedEventArgs e)
        {
            switch (Model.TYPEPRIV)
            {
                case "1":
                case "8":
                    {
                       
                            if ((ddlNitka.SelectedItem == "-1") ||
                            (ddlNitka.SelectedItem == null) ||
                            (txtKmBeginGeod.Text == ""))
                            {
                                _popupWindowAttantion = new ChildWindowAttantion();
                                _popupWindowAttantion.titleBox.Text = ProjectResources.PrivTitleBoxErrField;
                                _popupWindowAttantion.Show();
                            }
                            else
                            {

                                string _km = "";
                                _km = txtKmBeginGeod.Text.Replace(",", separator);
                                _km = txtKmBeginGeod.Text.Replace(".", separator);

                                ServiceDataClient.BuildPointLocationByPipeKMCompleted +=
                                    ServiceDataClient_BuildPointLocationByPipeKMCompleted;
                                ServiceDataClient.BuildPointLocationByPipeKMAsync(Model.PassportKey, Model.EntityKey, keyNit,
                                                                                  _km, "0", GlobalContext.Context);
                                this.DialogResult = true;
                            }
                       
                        //else
                        //{
                        //    ServiceDataClient.AddlinktopipeCompleted += ServiceDataClient_AddlinktopipeCompleted;
                        //    ServiceDataClient.AddlinktopipeAsync(Model.PassportKey, keyNit);
                        //}
                        

                        break;
                    }
                case "2":
                    {

                        if ((ddlNitka.SelectedItem == "-1") ||
                            (ddlNitka.SelectedItem == null) ||
                            (txtKmBeginGeod.Text == "") ||
                            (txtKmBeginIst.Text == "") ||
                            ((rbLeft.IsChecked == false) && (rbRight.IsChecked == false)))
                        {
                            _popupWindowAttantion = new ChildWindowAttantion();
                            _popupWindowAttantion.titleBox.Text = ProjectResources.PrivTitleBoxErrField;
                            _popupWindowAttantion.Show();
                        }
                        else
                        {

                            string _km = "";
                            _km = txtKmBeginGeod.Text.Replace(",", separator);
                            _km = txtKmBeginGeod.Text.Replace(".", separator);
                            string _txtKmBeginIst = "";
                            _txtKmBeginIst = txtKmBeginIst.Text.Replace(",", separator);
                            _txtKmBeginIst = txtKmBeginIst.Text.Replace(".", separator);
                            if (rbLeft.IsChecked == false)
                            {
                                _txtKmBeginIst = "-" + _txtKmBeginIst;
                            }
                            ServiceDataClient.BuildPointLocationByPipeKMCompleted +=
                                ServiceDataClient_BuildPointLocationByPipeKMCompleted;
                            ServiceDataClient.BuildPointLocationByPipeKMAsync(Model.PassportKey, Model.EntityKey, keyNit,
                                                                              _km, _txtKmBeginIst, GlobalContext.Context);
                            this.DialogResult = true;

                        }
                        break;
                    }
                case "3":
                case "9":
                    {
                        if (CountPriv == 0)
                        {
                            if ((ddlNitka.SelectedItem == "-1") ||
                            (ddlNitka.SelectedItem == null) ||
                            (txtKmBeginGeod.Text == "") ||
                            (txtDistanceBegin.Text == ""))
                            {
                                _popupWindowAttantion = new ChildWindowAttantion();
                                _popupWindowAttantion.titleBox.Text = ProjectResources.PrivTitleBoxErrField;
                                _popupWindowAttantion.Show();
                            }
                            else
                            {

                                string _km = "";
                                string _km2 = "";
                                _km = txtKmBeginGeod.Text.Replace(",", separator);
                                _km = txtKmBeginGeod.Text.Replace(".", separator);
                                _km2 = txtDistanceBegin.Text.Replace(",", separator);
                                _km2 = txtDistanceBegin.Text.Replace(".", separator);

                                ServiceDataClient.BuildSegmentLocationByPipeKMCompleted +=
                                    ServiceDataClient_BuildSegmentLocationByPipeKMCompleted;
                                ServiceDataClient.BuildSegmentLocationByPipeKMAsync(Model.PassportKey, Model.EntityKey,
                                                                                    keyNit, _km, _km2, "0", "0", GlobalContext.Context);
                                ServiceDataClient.BuildPointLocationByPipeKMAsync(Model.PassportKey, Model.EntityKey, keyNit,
                                                                                  _km, "0", GlobalContext.Context);
                                this.DialogResult = true;
                            }
                        }
                        else
                        {
                            ServiceDataClient.AddlinktopipeCompleted += ServiceDataClient_AddlinktopipeCompleted;
                            ServiceDataClient.AddlinktopipeAsync(Model.PassportKey, keyNit, GlobalContext.Context);
                        }
                        

                        break;
                    }
                case "4":
                    {
                        if ((ddlNitka.SelectedItem == "-1") ||
                            (ddlNitka.SelectedItem == null) ||
                            (txtKmBeginGeod.Text == "") ||
                            (txtKmBeginIst.Text == "") ||
                            ((rbLeft.IsChecked == false) && (rbRight.IsChecked == false)))
                        {
                            _popupWindowAttantion = new ChildWindowAttantion();
                            _popupWindowAttantion.titleBox.Text = ProjectResources.PrivTitleBoxErrField;
                            _popupWindowAttantion.Show();
                        }
                        else
                        {

                            string _km = "";
                            string _km2 = "";
                            string _txtKmBeginIst = "";
                            _km = txtKmBeginGeod.Text.Replace(",", separator);
                            _km = txtKmBeginGeod.Text.Replace(".", separator);
                            _km2 = txtDistanceBegin.Text.Replace(",", separator);
                            _km2 = txtDistanceBegin.Text.Replace(".", separator);

                            _txtKmBeginIst = txtKmBeginIst.Text.Replace(",",separator);
                            _txtKmBeginIst = txtKmBeginIst.Text.Replace(".", separator);
                            if (rbLeft.IsChecked == false)
                            {
                                _txtKmBeginIst = "-" + _txtKmBeginIst;
                            }
                            ServiceDataClient.BuildSegmentLocationByPipeKMCompleted +=
                                ServiceDataClient_BuildSegmentLocationByPipeKMCompleted;
                            ServiceDataClient.BuildSegmentLocationByPipeKMAsync(Model.PassportKey, Model.EntityKey,
                                                                                keyNit, _km, _km2, _txtKmBeginIst,
                                                                                _txtKmBeginIst, GlobalContext.Context);
                            //ServiceDataClient.BuildPointLocationByPipeKMAsync(Model.PassportKey, Model.EntityKey, keyNit, _km, "0");
                            
                        }

                        break;
                    }

                case "5":
                    {

                        if (CountPriv == 0)
                        {
                            if ((ddlNitka.SelectedItem == "-1") ||
                                (ddlNitka.SelectedItem == null) ||
                                (txtKmBeginGeod.Text == "") ||
                                (txtKmBeginIst.Text == "") ||
                                ((rbLeft.IsChecked == false) && (rbRight.IsChecked == false)))
                            {
                                _popupWindowAttantion = new ChildWindowAttantion();
                                _popupWindowAttantion.titleBox.Text = ProjectResources.PrivTitleBoxErrField;
                                _popupWindowAttantion.Show();
                            }
                            else
                            {

                                string _km = "";
                                _km = txtKmBeginGeod.Text.Replace(",", separator);
                                _km = txtKmBeginGeod.Text.Replace(".", separator);
                                string _txtKmBeginIst = "";
                                _txtKmBeginIst = txtKmBeginIst.Text.Replace(",", separator);
                                _txtKmBeginIst = txtKmBeginIst.Text.Replace(".", separator);
                                if (rbLeft.IsChecked == false)
                                {
                                    _txtKmBeginIst = "-" + _txtKmBeginIst;
                                }
                                ServiceDataClient.BuildPointLocationByPipeKMCompleted +=
                                    ServiceDataClient_BuildPointLocationByPipeKMCompleted;
                                ServiceDataClient.BuildPointLocationByPipeKMAsync(Model.PassportKey, Model.EntityKey,
                                                                                  keyNit, _km, _txtKmBeginIst, GlobalContext.Context);
                                this.DialogResult = true;

                            }
                            break;
                        }
                        else
                        {
                            ServiceDataClient.AddlinktopipeCompleted +=ServiceDataClient_AddlinktopipeCompleted;
                            ServiceDataClient.AddlinktopipeAsync(Model.PassportKey, keyNit, GlobalContext.Context);
                        }

                        break;

                    }
                case "6":
                    {
                        if (CountPriv == 0)
                        {
                            if ((ddlNitka.SelectedItem == "-1") ||
                           (ddlNitka.SelectedItem == null) ||
                           (txtKmBeginGeod.Text == "") ||
                           (txtKmBeginIst.Text == "") ||
                           ((rbLeft.IsChecked == false) && (rbRight.IsChecked == false)))
                            {
                                _popupWindowAttantion = new ChildWindowAttantion();
                                _popupWindowAttantion.titleBox.Text = ProjectResources.PrivTitleBoxErrField;
                                _popupWindowAttantion.Show();
                            }
                            else
                            {

                                string _km = "";
                                string _km2 = "";
                                string _txtKmBeginIst = "";
                                _km = txtKmBeginGeod.Text.Replace(",", separator);
                                _km = txtKmBeginGeod.Text.Replace(".", separator);
                                _km2 = txtDistanceBegin.Text.Replace(",", separator);
                                _km2 = txtDistanceBegin.Text.Replace(".", separator);

                                _txtKmBeginIst = txtKmBeginIst.Text.Replace(",", separator);
                                _txtKmBeginIst = txtKmBeginIst.Text.Replace(".", separator);
                                if (rbLeft.IsChecked == false)
                                {
                                    _txtKmBeginIst = "-" + _txtKmBeginIst;
                                }
                                ServiceDataClient.BuildSegmentLocationByPipeKMCompleted +=
                                    ServiceDataClient_BuildSegmentLocationByPipeKMCompleted;
                                ServiceDataClient.BuildSegmentLocationByPipeKMAsync(Model.PassportKey, Model.EntityKey,
                                                                                    keyNit, _km, _km2, _txtKmBeginIst,
                                                                                    _txtKmBeginIst, GlobalContext.Context);
                                //ServiceDataClient.BuildPointLocationByPipeKMAsync(Model.PassportKey, Model.EntityKey, keyNit, _km, "0");

                            }
                        }
                        else
                        {
                            ServiceDataClient.AddlinktopipeCompleted += ServiceDataClient_AddlinktopipeCompleted;
                            ServiceDataClient.AddlinktopipeAsync(Model.PassportKey, keyNit, GlobalContext.Context); 
                        }
                        break;
                    }
            }

        }


        // добавление привязки по координатам
        private void buCreatePrivKoord_Click(object sender, RoutedEventArgs e)
        {
            switch (Model.TYPEPRIV)
            {
                case "1":
                case "2":
                case "8":
                    {
                        
                            if ((ddlNitka.SelectedItem == "-1") ||
                             (ddlNitka.SelectedItem == null) ||
                             (txtXBegin.Text == "") ||
                             (txtYBegin.Text == ""))
                            {
                                _popupWindowAttantion = new ChildWindowAttantion();
                                _popupWindowAttantion.titleBox.Text = ProjectResources.PrivTitleBoxErrField;
                                _popupWindowAttantion.Show();
                            }
                            else
                            {
                                string txtXBegin_s = txtXBegin.Text.Replace(",", separator);
                                txtXBegin_s = txtXBegin.Text.Replace(".", separator);
                                string txtYBegin_s = txtYBegin.Text.Replace(",", separator);
                                txtYBegin_s = txtYBegin.Text.Replace(".", separator);


                                ServiceDataClient.BuildPointLocationByPipeXYCompleted +=
                                    ServiceDataClient_BuildPointLocationByPipeXYCompleted;
                                ServiceDataClient.BuildPointLocationByPipeXYAsync(Model.PassportKey, Model.EntityKey, keyNit, txtXBegin_s, txtYBegin_s, GlobalContext.Context);

                            } 
                        
                        //else
                        //{
                        //    ServiceDataClient.AddlinktopipeCompleted += ServiceDataClient_AddlinktopipeCompleted;
                        //    ServiceDataClient.AddlinktopipeAsync(Model.PassportKey, keyNit);
                        //}
                        
                        break;
                    }
                case "3":
                case "4":
                case "9":
                    {
                        if (CountPriv == 0)
                        {
                            if ((ddlNitka.SelectedItem == "-1") ||
                           (ddlNitka.SelectedItem == null) ||
                           (txtXBegin.Text == "") ||
                           (txtYBegin.Text == "") ||
                           (txtXEnd.Text == "") ||
                           (txtYEnd.Text == ""))
                            {
                                _popupWindowAttantion = new ChildWindowAttantion();
                                _popupWindowAttantion.titleBox.Text = ProjectResources.PrivTitleBoxErrField;
                                _popupWindowAttantion.Show();
                            }
                            else
                            {
                                string txtXBegin_s = txtXBegin.Text.Replace(",", separator);
                                txtXBegin_s = txtXBegin.Text.Replace(".", separator);
                                string txtYBegin_s = txtYBegin.Text.Replace(",", separator);
                                txtYBegin_s = txtYBegin.Text.Replace(".", separator);
                                string txtYEnd_s = txtYEnd.Text.Replace(",", separator);
                                txtYEnd_s = txtYEnd.Text.Replace(".", separator);
                                string txtXEnd_s = txtXEnd.Text.Replace(",", separator);
                                txtXEnd_s = txtXEnd.Text.Replace(".", separator);

                                ServiceDataClient.BuildSegmentLocationByPipeXYCompleted += ServiceDataClient_BuildSegmentLocationByPipeXYCompleted;
                                ServiceDataClient.BuildSegmentLocationByPipeXYAsync(Model.PassportKey, Model.EntityKey, keyNit, txtXBegin_s, txtYBegin_s, txtXEnd_s, txtYEnd_s, GlobalContext.Context);

                            }
                        }
                        else
                        {
                            ServiceDataClient.AddlinktopipeCompleted += ServiceDataClient_AddlinktopipeCompleted;
                            ServiceDataClient.AddlinktopipeAsync(Model.PassportKey, keyNit, GlobalContext.Context);
                        }
                       
                        break;
                    }
                
                case "5":
                    {
                         if (CountPriv == 0)
                         {
                             if ((ddlNitka.SelectedItem == "-1") ||
                            (ddlNitka.SelectedItem == null) ||
                            (txtXBegin.Text == "") ||
                            (txtYBegin.Text == ""))
                             {
                                 _popupWindowAttantion = new ChildWindowAttantion();
                                 _popupWindowAttantion.titleBox.Text = ProjectResources.PrivTitleBoxErrField;
                                 _popupWindowAttantion.Show();
                             }
                             else
                             {
                                 string txtXBegin_s = txtXBegin.Text.Replace(",", separator);
                                 txtXBegin_s = txtXBegin.Text.Replace(".", separator);
                                 string txtYBegin_s = txtYBegin.Text.Replace(",", separator);
                                 txtYBegin_s = txtYBegin.Text.Replace(".", separator);



                                 ServiceDataClient.BuildPointLocationByPipeXYCompleted +=
                                     ServiceDataClient_BuildPointLocationByPipeXYCompleted;
                                 ServiceDataClient.BuildPointLocationByPipeXYAsync(Model.PassportKey, Model.EntityKey, keyNit, txtXBegin_s, txtYBegin_s, GlobalContext.Context);

                                 //this.DialogResult = true;

                             }
                             break;
                         }
                         else
                         {
                             ServiceDataClient.AddlinktopipeCompleted += ServiceDataClient_AddlinktopipeCompleted;
                             ServiceDataClient.AddlinktopipeAsync(Model.PassportKey, keyNit, GlobalContext.Context);
                         }
                        break;
                    }
                case "6":
                    {
                        if (CountPriv == 0)
                        {
                            if ((ddlNitka.SelectedItem == "-1") ||
                            (ddlNitka.SelectedItem == null) ||
                            (txtXBegin.Text == "") ||
                            (txtYBegin.Text == "") ||
                            (txtXEnd.Text == "") ||
                            (txtYEnd.Text == ""))
                            {
                                _popupWindowAttantion = new ChildWindowAttantion();
                                _popupWindowAttantion.titleBox.Text = ProjectResources.PrivTitleBoxErrField;
                                _popupWindowAttantion.Show();
                            }
                            else
                            {
                                string txtXBegin_s = txtXBegin.Text.Replace(",", separator);
                                txtXBegin_s = txtXBegin.Text.Replace(".", separator);
                                string txtYBegin_s = txtYBegin.Text.Replace(",", separator);
                                txtYBegin_s = txtYBegin.Text.Replace(".", separator);
                                string txtYEnd_s = txtYEnd.Text.Replace(",", separator);
                                txtYEnd_s = txtYEnd.Text.Replace(".", separator);
                                string txtXEnd_s = txtXEnd.Text.Replace(",", separator);
                                txtXEnd_s = txtXEnd.Text.Replace(".", separator);

                                ServiceDataClient.BuildSegmentLocationByPipeXYCompleted += ServiceDataClient_BuildSegmentLocationByPipeXYCompleted;
                                ServiceDataClient.BuildSegmentLocationByPipeXYAsync(Model.PassportKey, Model.EntityKey, keyNit, txtXBegin_s, txtYBegin_s, txtXEnd_s, txtYEnd_s, GlobalContext.Context);

                            }
                        }
                        else
                        {
                            ServiceDataClient.AddlinktopipeCompleted += ServiceDataClient_AddlinktopipeCompleted;
                            ServiceDataClient.AddlinktopipeAsync(Model.PassportKey, keyNit, GlobalContext.Context);
                        }
                        break;
                    }
                case "7":
                    {
                        if ((txtXBegin.Text == "") ||
                            (txtYBegin.Text == ""))
                        {
                            _popupWindowAttantion = new ChildWindowAttantion();
                            _popupWindowAttantion.titleBox.Text = ProjectResources.PrivTitleBoxErrField;
                            _popupWindowAttantion.Show();
                        }
                        else
                        {
                            string txtXBegin_s = txtXBegin.Text.Replace(",", separator);
                            txtXBegin_s = txtXBegin.Text.Replace(".", separator);
                            string txtYBegin_s = txtYBegin.Text.Replace(",", separator);
                            txtYBegin_s = txtYBegin.Text.Replace(".", separator);


                            ServiceDataClient.BuildPointLocationByPipeXYCompleted +=
                                ServiceDataClient_BuildPointLocationByPipeXYCompleted;
                            ServiceDataClient.BuildPointLocationByPipeXYAsync(Model.PassportKey, Model.EntityKey, "0", txtXBegin_s, txtYBegin_s, GlobalContext.Context);

                        } 
                        break;
                    }

            }
        }

        void ServiceDataClient_BuildSegmentLocationByPipeXYCompleted(object sender, BuildSegmentLocationByPipeXYCompletedEventArgs e)
        {
            if (e.Result.IsValid)
            {
                
                Model.ReloadGridPriv = "1";
                this.DialogResult = true;


            }
            else
            {
                Model.MainModel.Report("Ошибка добавления привязки - " + e.Result.ErrorMessage);
                _popupWindowAttantion = new ChildWindowAttantion();
                _popupWindowAttantion.titleBox.Text = ProjectResources.PrivTitleBoxErr + e.Result.ErrorMessage;
                _popupWindowAttantion.Show();

            }
        }

        void ServiceDataClient_BuildPointLocationByPipeXYCompleted(object sender, BuildPointLocationByPipeXYCompletedEventArgs e)
        {
            if (e.Result.IsValid)
            {
                Model.ReloadGridPriv = "1";
                this.DialogResult = true;

            }
            else
            {
                Model.MainModel.Report("Ошибка добавления привязки - " + e.Result.ErrorMessage);
                _popupWindowAttantion = new ChildWindowAttantion();
                _popupWindowAttantion.titleBox.Text = ProjectResources.PrivTitleBoxErr + e.Result.ErrorMessage;
                _popupWindowAttantion.Show();

            }
        }

        void ServiceDataClient_BuildPointLocationByPipeKMCompleted(object sender, BuildPointLocationByPipeKMCompletedEventArgs e)
        {
            if (e.Result.IsValid)
            {
                Model.ReloadGridPriv = "1";
                this.DialogResult = true;


            }
            else
            {
                Model.MainModel.Report("Ошибка добавления привязки - " + e.Result.ErrorMessage);
                _popupWindowAttantion = new ChildWindowAttantion();
                _popupWindowAttantion.titleBox.Text = ProjectResources.PrivTitleBoxErr + e.Result.ErrorMessage;
                _popupWindowAttantion.Show();
                

            }
        }

        private void txtXBegin_BindingValidationError(object sender, ValidationErrorEventArgs e)
        {
            txtXBegin.Focus();
        }

        public void CreatePriv()
        {
            this.DialogResult = true;
        }

        private void buCreatePrivt5_Click(object sender, RoutedEventArgs e)
        {
            ServiceDataClient.AddlinktopipeCompleted += ServiceDataClient_AddlinktopipeCompleted;
            ServiceDataClient.AddlinktopipeAsync(Model.PassportKey, keyNit, GlobalContext.Context);
        }
    }
}

