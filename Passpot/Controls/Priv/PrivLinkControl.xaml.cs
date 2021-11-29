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
using Media.Interfaces;
using Media.Resources;
using Passpot.Business;
using Passpot.Model;
using Services.ServiceReference;

namespace Passpot.Controls.Priv
{
    public partial class PrivLinkControl : UserControl
    {
        private PassportDetailModel _passportModel;
        public bool klickButton = false;
        public string NKEY = "";
        public string KeyLink { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private ServiceDataClient _serviceDataClient;
        public DataListRelationObjItems newItems;
        PrivItems _privItemsList;
        private ChildWindowDelete _popupWindowDeleteChild = new ChildWindowDelete();


        private ServiceDataClient ServiceDataClient
        {
            get
            {
                if (_serviceDataClient == null)
                    _serviceDataClient = new ServiceDataClient();
                return _serviceDataClient;
            }
        }


        private PassportDetailModel Model
        {
            get { return DataContext as PassportDetailModel; }
        }

        public static PrivLinkControl CreateLinkControlEdit(PrivItems privItemsList,
                                                            PassportDetailModel passportModel,
                                                            FieldMetaDataItem metaData,
                                                            bool noLinks, bool visiblebuAdd, bool isEdit)
        {
            var c = new PrivLinkControl();

            c.button_delPriv.DataContext = privItemsList.NKEY;

            if (isEdit)
            {
                c.delColumn.Width = new GridLength(32);
                c.button_delPriv.Visibility = Visibility.Visible;

            }
            else
            {
                c.delColumn.Width = new GridLength(0);
                c.button_delPriv.Visibility = Visibility.Collapsed;
            }

            c._privItemsList = privItemsList;
            c.CreatePriv(c._privItemsList, passportModel);



            return c;
        }

        public void CreatePriv(PrivItems privItemsList, PassportDetailModel passportModel)
        {


            button_delPriv.Tag = privItemsList.NKEY;
            switch (passportModel.TYPEPRIV)
            {
                case "1":
                    {

                        tbmain.Text = ConvertToString(_privItemsList.NKMORT1,2) + " m    " + _privItemsList.CNAME;
                        tbrest.Text = "E " + ConvertToString(_privItemsList.NX1,7) + "˚   N " + ConvertToString(_privItemsList.NY1,7) + "˚ ";
                        rowPriv4.Height = new GridLength(0);
                        break;
                    }
                case "2":
                    {

                        string typedistan = "";
                        if (_privItemsList.NDISTANCEBEG.Contains("-"))
                        {

                            typedistan = " справа на " + ConvertToString(_privItemsList.NDISTANCEBEG.Substring(1),2);
                        }
                        else
                        {
                            typedistan = " слева на " + ConvertToString(_privItemsList.NDISTANCEBEG,2);
                        }
                        tbmain.Text = ConvertToString(_privItemsList.NKMORT1,2) + " m   " + _privItemsList.CNAME;
                        tbrest.Text = "E " + ConvertToString(_privItemsList.NX1,7) + "˚   N " + ConvertToString(_privItemsList.NY1,7) + "˚ " +
                                      typedistan +
                                      "м  ";
                        rowPriv4.Height = new GridLength(0);
                        break;
                    }

                case "3":
                    {
                        tbmain.Text = ConvertToString(_privItemsList.NKMORT1,2) + " - " + ConvertToString(_privItemsList.NKMORT2,2) + " м   " +
                                      _privItemsList.CNAME;
                        tbrest.Text = "E " + ConvertToString(_privItemsList.NX1,7) + "˚   N " + ConvertToString(_privItemsList.NY1,7) + "˚ " +
                                      " -  E " + ConvertToString(_privItemsList.NX2,7) + "˚   N " + ConvertToString(_privItemsList.NY2,7) + "˚ ";
                        rowPriv4.Height = new GridLength(0);
                        break;
                    }
                case "4":
                    {
                        string typedistan = "";
                        if (_privItemsList.NDISTANCEBEG.Contains("-"))
                        {

                            typedistan = " справа на " + ConvertToString(_privItemsList.NDISTANCEBEG.Substring(1),2);
                        }
                        else
                        {
                            typedistan = " слева на " + ConvertToString(_privItemsList.NDISTANCEBEG,2);
                        }

                        tbmain.Text = ConvertToString(_privItemsList.NKMORT1,2) + " - " + ConvertToString(_privItemsList.NKMORT2,2) + " м   " +
                                      _privItemsList.CNAME;
                        tbrest.Text = "E " + ConvertToString(_privItemsList.NX1,7) + "˚   N " + ConvertToString(_privItemsList.NY1,7) + "˚ " +
                                      " -  E " + ConvertToString(_privItemsList.NX2,7) + "˚   N " + ConvertToString(_privItemsList.NY2,7) + "˚ ";
                        tbrest2.Text = typedistan;
                        break;
                    }

                case "5":
                    {
                        string typedistan = "";

                        if (_privItemsList.NDISTANCEBEG != null)
                        {
                            if (_privItemsList.NDISTANCEBEG.Contains("-"))
                            {

                                typedistan = " справа на " + ConvertToString(_privItemsList.NDISTANCEBEG.Substring(1),2);
                            }
                            else
                            {
                                typedistan = " слева на " + ConvertToString(_privItemsList.NDISTANCEBEG,2);
                            }
                        }
                        else
                        {
                            typedistan = " справа на 0" ;
                        }

                        
                        tbmain.Text = ConvertToString(_privItemsList.NKMORT1,2) + " m   " + _privItemsList.CNAME;
                        tbrest.Text = "E " + ConvertToString(_privItemsList.NX1,7) + "˚   N " + ConvertToString(_privItemsList.NY1,7) + "˚ " +
                                      typedistan + "м  от " + _privItemsList.CNAME;
                        rowPriv4.Height = new GridLength(0);
                        break;
                    }


                case "6":
                    {

                        tbmain.Text = ConvertToString(_privItemsList.NKMORT1,2) + " m  - " + ConvertToString(_privItemsList.NKMORT2,2) + "    " +
                                      _privItemsList.CNAME;
                        tbrest.Text = "E " + ConvertToString(_privItemsList.NX1,7) + "˚   N " + ConvertToString(_privItemsList.NY1,7) + "˚ - " +
                                      " E " + ConvertToString(_privItemsList.NX2,7) + "˚  N " + ConvertToString(_privItemsList.NY2,7) + "˚";
                        rowPriv4.Height = new GridLength(0);
                        break;
                    }
                case "7":
                    {

                        tbmain.Text = "E " + ConvertToString(_privItemsList.NX1,7) + "˚   N " + ConvertToString(_privItemsList.NY1,7) + "˚ ";
                        rowPriv4.Height = new GridLength(0);

                        break;
                    }
                case "8":
                    {
                        tbmain.Text = ConvertToString(_privItemsList.NKMORT1,2) + " m  " + _privItemsList.CNAME;
                        tbrest.Text = "E " + ConvertToString(_privItemsList.NX1,7) + "˚   N " + ConvertToString(_privItemsList.NY1,7) + "˚ ";
                        rowPriv4.Height = new GridLength(0);
                        break;
                    }
                case "9":
                    {
                        tbmain.Text = ConvertToString(_privItemsList.NKMORT1,2) + " - " + ConvertToString(_privItemsList.NKMORT2,2) + " м   " +
                                      _privItemsList.CNAME;
                        tbrest.Text = "E " + ConvertToString(_privItemsList.NX1,7) + "˚   N " + ConvertToString(_privItemsList.NY1,7) + "˚ " +
                                      " -  E " + ConvertToString(_privItemsList.NX2,7) + "˚   N " + ConvertToString(_privItemsList.NY2,7) + "˚ ";
                        rowPriv4.Height = new GridLength(0);
                        break;
                    }

                case "10":
                    {

                        tbrest.Text = "E " + ConvertToString(_privItemsList.NX1,7) + "˚   N " + ConvertToString(_privItemsList.NY1,7) + "˚ " ;
                        rowPriv4.Height = new GridLength(0);
                        break;
                    }
            }
            
            //текст привязки для экселя
            passportModel.TextPrivExel = tbmain.Text + tbrest.Text;


        }
        public PrivLinkControl()
        {
            InitializeComponent();
        }

        private string ConvertToString(string data, int znak)
        {
            string result = "";

            if (!string.IsNullOrEmpty(data))
            {
                string separator = string.Empty;
                separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                string checkValue = "";
                checkValue = (data).Replace(".", separator);
                checkValue = checkValue.Replace(",", separator);
                double d = Convert.ToDouble(checkValue);
                d = Math.Round(d, znak);
                result = d.ToString();

            }

            return result;

        }

      


        #region  удалить привязку
        //удалить привязку
        private void Button_delPriv_OnClick(object sender, RoutedEventArgs e)
        {
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
                //удаляем по новому алгоритму - перестраиваем класс и при сохранении всего паспорта передаем его.

                klickButton = true;
                NKEY = button_delPriv.Tag.ToString();
                Model.FirePropertyChanged("KeyPrivDelete");


                //ServiceDataClient.DeleteLinkToPipeByKeyCompleted += ServiceDataClient_DeleteLinkToPipeByKeyCompleted;
                //ServiceDataClient.DeleteLinkToPipeByKeyAsync(button_delPriv.Tag.ToString(), GlobalContext.Context);

            }
        }

        //void ServiceDataClient_DeleteLinkToPipeByKeyCompleted(object sender, DeleteLinkToPipeByKeyCompletedEventArgs e)
        //{
        //    ServiceDataClient.DeleteLinkToPipeByKeyCompleted -= ServiceDataClient_DeleteLinkToPipeByKeyCompleted;
        //    if (e.Result.IsValid)
        //    {
        //        klickButton = true;
        //        Model.FirePropertyChanged("KeyPrivDelete"); 

        //    }
        //    else
        //    {
        //        _passportModel.MainModel.Report(" Удаление привязки err = " + e.Result.ErrorMessage);
        //    }
        //}




        #endregion
    }
}

