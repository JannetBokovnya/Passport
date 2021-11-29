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
using System.Collections.ObjectModel;
using Media.Interfaces;
using Media.Resources;
using Services.ServiceReference;
using Telerik.Windows.Controls;
using System.Windows.Data;
using Passpot.Business;
using Passpot.Model;

namespace Passpot.Controls
{



    public partial class ComboBoxControl : UserControl, IControlValueChanged
    {
        // входные данные паспорта: поле, значение
       //public Dictionary<string, string> _dictDataFiltr;
        private string _oldValue;
        private string key = string.Empty;
        private string keyDict;
        private FieldMetaDataItem _metaData;
        private ChildWindowDict _popupWindowDict;
        private PassportDetailModel _passportModel;
        private ServiceDataClient _serviceDataClient;
        private List<OneDictData> OneDictDataList;
        public List<DictDataFiltr> _dictDataFiltr;
        
        

        public ComboBoxControl()
        {
            InitializeComponent();
            //не нужно добавлять словари из паспорта
            //buttonAdd.Click += Button_Click;
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


        //public static ComboBoxControl CreateControl(FieldMetaDataItem metaData, Collection<DictData> dictValueData, string value)
        //var _dictDataFiltr = new Dictionary<string, string>();
        // Collection<DictDataFiltr> _dictDataFiltr = new Collection<DictDataFiltr>();

        public static ComboBoxControl CreateControl(FieldMetaDataItem metaData, List<DictData> dictValueData, PassportDetailModel passportModel, string value)
        {
            
            var c = new ComboBoxControl();
            c._dictDataFiltr = new List<DictDataFiltr>();

            c._passportModel = passportModel;
            c._oldValue = value;
            c._metaData = metaData;

            c.comboBox.Items.Clear();
            c.comboBox.ItemsSource = null;
            c.titleBox.Text = metaData.TITUL;

            switch (metaData.BASIC_FLD)
            {
                case 1:
                    c.titleBox.FontWeight = FontWeights.Bold;
                    break;
                case 0:

                    break;

            }


            switch (metaData.MANDATORYVALUE_FLD)
            {
                case 1:
                    c.titleBox.Foreground = new SolidColorBrush(Color.FromArgb(255, Byte.Parse("220"), Byte.Parse("70"), Byte.Parse("74")));

                    break;
                case 0:

                    break;
            }



            c.keyDict = metaData.KEYDICT; 
            for (int i = 0; i < dictValueData.Count; i++)
            {
                DictData dd = dictValueData[i];
                //if (dd.FLDNAME == metaData.FLDNAME)
                if (dd.KEYDICT == metaData.KEYDICT)
                {
                    c._dictDataFiltr.Add(new DictDataFiltr { KEYDICT = dd.VALUEKEYDICT, VALUEDICT = dd.VALUEDICT });
                }
            }

            //добавление строки неопределено
            //c._dictDataFiltr.Add(new DictDataFiltr { KEYDICT = "", VALUEDICT = ProjectResources.DictDataFiltr });

            
            c.comboBox.ItemsSource = c._dictDataFiltr;
            c.comboBox.DisplayMemberPath = "VALUEDICT";
          
            for (int ii = 0; ii < c._dictDataFiltr.Count; ii++)
            {
                DictDataFiltr ddf = c._dictDataFiltr[ii];
                if (ddf.KEYDICT.ToString() == value)
                {
                    c.comboBox.SelectedIndex = c.comboBox.Items.IndexOf(c._dictDataFiltr[ii]);
                    break;
                }
            }

            if (metaData.IS_EDITED == 0)
            {
                c.IsEnabled = false;
            }
            else
            {
                c.IsEnabled = true;
            }

                return c;
        }

        private void comboBox_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (comboBox.SelectedItem != null)
            {
                key = ((DictDataFiltr)comboBox.SelectedItem).KEYDICT.ToString();
            }
            else
            {
                key = "";
            }
            

        }

        #region IControlValueChanged Members

        bool IControlValueChanged.HasChanges
        {
            get
            {
                
                return key.ToString() != _oldValue; 
            }
        }

        string IControlValueChanged.NewValue
        {
            get 
            {
                //if (key == "")
                //    return key = "0";
                //else
                    return key.ToString(); 
            }
        }

        FieldMetaDataItem IControlValueChanged.MetaData
        {
            get { return _metaData; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

           // _passportModel.StartOnNameNSI(keyDict);


            _popupWindowDict = new ChildWindowDict();
            _popupWindowDict.StubDictData(keyDict, _passportModel, _dictDataFiltr);
            _popupWindowDict.Show();
            _popupWindowDict.btnOk.Click += ButtonOkClickInsertNSI;
            
            
        }

        void ButtonOkClickInsertNSI(object sender, RoutedEventArgs e)
        {
            _popupWindowDict.btnOk.Click -= ButtonOkClickInsertNSI;
            ServiceDataClient.OneDictValueDataCompleted += OneDictValueDataCompleted;
            ServiceDataClient.OneDictValueDataAsync(keyDict, GlobalContext.Context);
            
        }

        void OneDictValueDataCompleted(object sender, OneDictValueDataCompletedEventArgs e)
        {

            if (e.Result.IsValid)
            {
                OneDictDataList = new List<OneDictData>(e.Result.DictData_result);
                List<DictDataFiltr> _dictDataFiltr2 = new List<DictDataFiltr>();
                for (int idl = 0; idl < OneDictDataList.Count; idl++)
                {
                    OneDictData dd = OneDictDataList[idl];
                    _dictDataFiltr2.Add(new DictDataFiltr { KEYDICT = dd.KEYVALUEDICT, VALUEDICT = dd.VALUEDICT });
                }

                _dictDataFiltr2.Add(new DictDataFiltr { KEYDICT = "", VALUEDICT = ProjectResources.DictDataFiltr });
              
                comboBox.ItemsSource = _dictDataFiltr2;
                comboBox.DisplayMemberPath = "VALUEDICT";

                for (int ii = 0; ii < _dictDataFiltr2.Count; ii++)
                {
                    DictDataFiltr ddf2 = _dictDataFiltr2[ii];
                    if (ddf2.KEYDICT.ToString() == _oldValue)
                    {
                        comboBox.SelectedIndex = comboBox.Items.IndexOf(_dictDataFiltr2[ii]);
                        break;
                    }
                }


                
            }
            else
            {
                //MainModel.Report("Словари DictValueDataCompleted err = " + e.Result.ErrorMessage);
            }
            
            
            
            


        }

        //public void OnGetAddNSI(string nameNSI)
        //{
        //    var m = DataContext as PassportDetailModel;
        //    try
        //        {
             
        //            _popupWindowDict = new ChildWindowDict();
        //            _popupWindowDict.StubDictData(keyDict);
        //            _popupWindowDict.Show();



        //        }
        //        catch (Exception ex)
        //        {
        //            m.MainModel.Report(ex.Message);
        //        }

        //}

        #endregion
    }
}
