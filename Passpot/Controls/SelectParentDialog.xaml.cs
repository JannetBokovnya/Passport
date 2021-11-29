using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using Passpot.Business;

using System.Collections.ObjectModel;
using Passpot.Model;
using Services.ServiceReference;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;


namespace Passpot.Controls
{
    /// <summary>
    /// Sample ChildWindow for demonstration purposes.
    /// </summary>
    public partial class SelectParentDialog : ChildWindow
    {

        //private List<DataParentList> _fieldParentDataList;
        //private List<DataOneParent> _fieldOneDataParent;
        private string _keyParent;
        private string _keyPassportParent;


        /// <summary>
        /// Initializes a DemoChildWindow.
        /// </summary>
        public SelectParentDialog()
        {
            //LocalizationManager.DefaultResourceManager = ResourceGrid.ResourceManager;
            InitializeComponent();
            // optionsStack.DataContext = this;
            SelectedParentKey = string.Empty;
        }

        public PassportModel Model
        {
            get { return DataContext as PassportModel; }
        }

        public void InitPage(string passportKey)
        {
            string aa = Model.PassportKey;

           // Model.StartGetDataParent(Model.PassportKey);

        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Model.PropertyChanged -= Model_PropertyChanged;
            Model.PropertyChanged += Model_PropertyChanged;
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //SelectOneParentList
            if (e.PropertyName.Equals("SelectParentList"))
            {
                //_fieldParentDataList = Model.SelectParentList;
                //comboBoxParent.ItemsSource = _fieldParentDataList;
                //comboBoxParent.DisplayMemberPath = "NAMEPARENT";
                //comboBoxParent.SelectedIndex = 0;
                //StubParentData();

                return;
            }
            if (e.PropertyName.Equals("SelectOneParentList"))
            {

                //_fieldOneDataParent = Model.SelectOneParentList;
                //grid.ItemsSource = _fieldOneDataParent;
                //_fieldParentDataList = Model.SelectParentList;
                //comboBoxParent.ItemsSource = _fieldParentDataList;
                //comboBoxParent.DisplayMemberPath = "NAMEPARENT";
                //comboBoxParent.SelectedIndex = 0;
                //StubParentData();

                return;
            }

        }

        private void StubParentData()
        {
            //Model.StartGetDataParent(Model.PassportKey);
            //_fieldParentDataList = new List<DataParentList>();
            //_fieldParentDataList = Model.StartGetDataParent(Model.PassportKey);
            //_fieldParentDataList.Add(new DataParentList { KEYPARENT = "13", NAMEPARENT = "Запорная арматура" });
            //_fieldParentDataList.Add(new DataParentList { KEYPARENT = "14", NAMEPARENT = "Компресорная станция" });
            //_fieldParentDataList.Add(new DataParentList { KEYPARENT = "15", NAMEPARENT = "ЛПУ" });
            //comboBoxParent.ItemsSource = null;
            //comboBoxParent.ItemsSource = _fieldParentDataList;
            //comboBoxParent.DisplayMemberPath = "NAMEPARENT";
            //comboBoxParent.SelectedIndex = 0;
        }

        public string SelectedParentKey { get; private set; }

        /// <summary>
        /// Handles the Click event of the OK button.
        /// </summary>
        /// <param name="sender">OK Button.</param>
        /// <param name="e">Event arguments.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used by event defined in Xaml.")]
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedParentKey = _keyPassportParent;// "устанавливает выбранный";
            this.DialogResult = true;
        }

        /// <summary>
        /// Handles the Click event of the Cancel button.
        /// </summary>
        /// <param name="sender">Cancel button.</param>
        /// <param name="e">Event arguments.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used by event defined in Xaml.")]
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ComboBoxParent_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (comboBoxParent.SelectedItem != null)
            {
                //_keyParent = ((DataParentList)comboBoxParent.SelectedItem).KEYPARENT;

                //не актуально!!!!! для добавления нового паспорта не используем выбор парента!!!!
                //Model.StartGetDataOneParent(_keyParent);
                //_keyParent = ((DataParentList)comboBoxParent.SelectedItem).KEYPARENT;

                //_fieldOneDataParent = new List<DataOneParent>();

                //_fieldOneDataParent.Add(new DataOneParent { KEYONEPARENT = "17", NAMEONEPARENT = "Название 1" });
                //_fieldOneDataParent.Add(new DataOneParent { KEYONEPARENT = "18", NAMEONEPARENT = "Название 2" });
                //_fieldOneDataParent.Add(new DataOneParent { KEYONEPARENT = "19", NAMEONEPARENT = "Название 3" });

                //grid.ItemsSource = _fieldOneDataParent;
            }

        }

        private void grid_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            //_keyPassportParent = ((DataOneParent)grid.SelectedItem).KEYONEPARENT;

        }


    }
}
