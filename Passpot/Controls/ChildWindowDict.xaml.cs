using System;
using System.Collections.Generic;
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
using Passpot.Model;
using Services.ServiceReference;
using Telerik.Windows.Controls;

namespace Passpot.Controls
{
    public partial class ChildWindowDict : ChildWindow
    {

        private ServiceDataClient _serviceDataClient;
        private string _keyDict;
        public List<OneDictData> OneDictDataList { get; private set; }
        List<DictDataFiltr> _onedictValueData;
        


        public ChildWindowDict()
        {
            //LocalizationManager.DefaultResourceManager = ResourceGrid.ResourceManager;
            InitializeComponent();

            
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




        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }
        public void StubDictData(string keyDict, PassportDetailModel passportModel, List<DictDataFiltr> onedictValueData)
        {

            _keyDict = keyDict;
            _onedictValueData = onedictValueData;
           ServiceDataClient.GetNameNsiOnKeyNSICompleted += OnGetNameNsiOnKeyNSICompleted;
           ServiceDataClient.GetNameNsiOnKeyNSIAsync(keyDict, GlobalContext.Context);
           
               
        }



        void OnGetNameNsiOnKeyNSICompleted(object sender, GetNameNsiOnKeyNSICompletedEventArgs e)
        {
            ServiceDataClient.GetNameNsiOnKeyNSICompleted -= OnGetNameNsiOnKeyNSICompleted;
            var m = DataContext as PassportDetailModel;

            if (e.Result.IsValid)
            {

                textNameDict.Text = ProjectResources.TextNameDict2 + " - " + e.Result.nameNSIonKey_result.NameNSIonkey;
                gridNsi.ItemsSource = _onedictValueData;
                //ServiceDataClient.OneDictValueDataCompleted += OnOneDictValueDataCompleted;
                //ServiceDataClient.OneDictValueDataCompletedAsync(keyDict);
            }
            else
            {
                m.MainModel.Report("Название нси OnGetNameNsiOnKeyNSI err = " + e.Result.ErrorMessage);
            }
        }

        

        private void insertCurrentRecord_Click(object sender, RoutedEventArgs e)
        {
            ServiceDataClient.SaveNsiCompleted += SaveNsiCompleted;
            var m = DataContext as PassportDetailModel;
            ServiceDataClient.SaveNsiAsync("0", textBoxNsiName.Text.ToString(), _keyDict, GlobalContext.Context);
                
            
            //ServiceDataClient
            //    ListNsiOnSave SaveNsi
        }

        void SaveNsiCompleted(object sender, SaveNsiCompletedEventArgs e)
        {
            if (e.Result.IsValid)
            {
                OneDictDataList = new List<OneDictData>(e.Result.DictData_result);
                List<DictDataFiltr> _dictDataFiltr = new List<DictDataFiltr>();
                for (int idl = 0; idl < OneDictDataList.Count; idl++)
                {
                    OneDictData dd = OneDictDataList[idl];
                    _dictDataFiltr.Add(new DictDataFiltr { KEYDICT = dd.KEYVALUEDICT, VALUEDICT = dd.VALUEDICT });
                }


                gridNsi.ItemsSource = _dictDataFiltr;
                

            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        //void OnOneDictValueDataCompleted(object sender, OneDictValueDataCompletedCompletedEventArgs e)
        //{

        //}


    }
}

