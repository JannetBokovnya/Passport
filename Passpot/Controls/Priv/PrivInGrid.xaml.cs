using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Telerik.Windows.Controls;

namespace Passpot.Controls.Priv
{
    public partial class PrivInGrid : UserControl
    {
        private ServiceDataClient _serviceDataClient;
        protected ServiceDataClient ServiceDataClient
        {
            get
            {
                if (_serviceDataClient == null)
                    _serviceDataClient = new ServiceDataClient();
                return _serviceDataClient;
            }
        }

       // private PrivChildModel Model { get { return DataContext as PrivChildModel; } }

        public bool IsGrid = false;
        private bool _isEdited = false;
        public bool IsvalidCoor = false;
        private List<PrivItems> NavigationPrivListDouble = new List<PrivItems>();
        private PassportDetailModel Model;

        ObservableCollection<NavigationPriv> NavigationPrivIList = new ObservableCollection<NavigationPriv>();
       
        public PrivInGrid()
        {
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(RadGridViewCommands).TypeHandle);
            ICommand beginInsertCommand = RadGridViewCommands.BeginInsert;
            InitializeComponent();
        }
        public static PrivInGrid CreateGrid(List<NavigationPriv> NavigationPrivList, PassportDetailModel Model)
        {
            var c = new PrivInGrid();
            c.Model = Model;
            c.IsGrid = true;
            c.NavigationPrivIList = new ObservableCollection<NavigationPriv>();

            for (int ii = 0; ii < NavigationPrivList.Count; ii++)
            {
                c.NavigationPrivIList.Add(
                    new NavigationPriv
                    (
                        NavigationPrivList[ii].CNAME,
                        NavigationPrivList[ii].NKMORT1,
                        NavigationPrivList[ii].NDISTANCEBEG,
                        NavigationPrivList[ii].NX1,
                        NavigationPrivList[ii].NY1,
                        NavigationPrivList[ii].NKEY,
                        NavigationPrivList[ii].NH1,
                        NavigationPrivList[ii].NKMTRUE1,
                        NavigationPrivList[ii].NX2,
                        NavigationPrivList[ii].NY2,
                        NavigationPrivList[ii].NKMORT2,
                        NavigationPrivList[ii].NKMTRUE2,
                        NavigationPrivList[ii].NDISTANCEEND,
                        0,
                        0,
                        NavigationPrivList[ii].NZ2,
                        NavigationPrivList[ii].NZ1,
                        NavigationPrivList[ii].NH2,
                        "1",
                        "0"
                    ));
            }

            c.Link.ItemsSource = c.NavigationPrivIList;
            return c;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.NavigationPrivIList.Insert(NavigationPrivIList.Count, new NavigationPriv("", "", "", "", "", "", "", "", "", "", "", "", "", 0, 0, "", "", "", "", "1"));
            Link.ItemsSource = null;
            Link.ItemsSource = NavigationPrivIList;
        }

        //возвращает привязку в основную форму
        public List<NavigationPriv> GetNavigationPriv()
        {
         List<NavigationPriv> result = new List<NavigationPriv>();

         foreach (var item in Link.Items)
         {
             result.Add(item as NavigationPriv);
         }
            return result;
        }

        private void Link_RowEditEnded(object sender, GridViewRowEditEndedEventArgs e)
        {

            if (e.OldValues != e.NewData)
            {
                _isEdited = true;
                ((NavigationPriv)(e.NewData)).ISEDITED = "1";
            }
           
        }


        private void PrivInGrid_OnUnloaded(object sender, RoutedEventArgs e)
        {
            //если уходим с формы - проверям правильно ли мы ввели точки
            if (_isEdited)
            {
                List<NavigationPriv> navigationPrivList = new List<NavigationPriv>();
                navigationPrivList = GetNavigationPriv();
                NavigationPrivListDouble = HelperPriv.GetNavigationPrivListDoubleOnType10Grid(navigationPrivList);
                if (NavigationPrivListDouble.Count > 0)
                {
                    string xmlExport = HelperPriv.GetXmlInNewPriv(NavigationPrivListDouble, Model);

                    if (!string.IsNullOrEmpty(xmlExport))
                    {
                        ServiceDataClient.GetCalcPrivJsonCompleted += ServiceDataClient_GetCalcPrivJsonCompleted;
                        ServiceDataClient.GetCalcPrivJsonAsync(Model.PassportKey, xmlExport, GlobalContext.Context);
                    }
                    else
                    {
                        Model.MainModel.Report("ошибка  формирования xml AddAllPriv()");
                    }
                }
            }
        }

        void ServiceDataClient_GetCalcPrivJsonCompleted(object sender, GetCalcPrivJsonCompletedEventArgs e)
        {
            ServiceDataClient.GetCalcPrivJsonCompleted -= ServiceDataClient_GetCalcPrivJsonCompleted;
            if (e.Result.IsValid)
            {
                //если все ок то переходим дальше
                IsvalidCoor = true;
            }
            else
            {
                ChildWindowAttantion _popupWindowAttantion = new ChildWindowAttantion();
                _popupWindowAttantion.titleBox.Text = ProjectResources.PrivTitleBoxErr + e.Result.ErrorMessage;
                _popupWindowAttantion.Show();
                Model.MainModel.Report("ошибка создании привязки " + e.Result.ErrorMessage);

            }
        }

        
    }
}
