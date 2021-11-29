using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Passpot.Business
{
    public class MyViewModelObjByType : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<ObjByType> objByTypes;
        //private ObservableCollection<Player> players;
        private object selectedItem;

        public ObservableCollection<ObjByType> ObjByTypes
        {
            get
            {
                if (this.objByTypes == null)
                {
                    this.objByTypes = ObjByType.GetObjByTypes();
                }

                return this.objByTypes;
            }
        }
        public object SelectedItem
        {
            get { return this.selectedItem; }
            set
            {
                if (value != this.selectedItem)
                {
                    this.selectedItem = value;
                    this.OnPropertyChanged("SelectedItem");
                }
            }
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }
}
