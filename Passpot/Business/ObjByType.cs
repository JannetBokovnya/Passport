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
    public class ObjByType : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private string name;
        private string key;
        private int stadiumCapacity;
        //private ObservableCollection<Player> players;

        public string Name
        {
            get { return this.name; }
            set
            {
                if (value != this.name)
                {
                    this.name = value;
                    this.OnPropertyChanged("Name");
                }
            }
        }

        public string Key
        {
            get { return this.key; }
            set
            {
                if (value != this.key)
                {
                    this.key = value;
                    this.OnPropertyChanged("Key");
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

        public override string ToString()
        {
            return this.Name;
        }

         public ObjByType()
        {

        }

         public ObjByType(string name, string key)
        {
            this.name = name;
            this.key = key;
        }




        public static ObservableCollection<ObjByType> GetObjByTypes()
        {
            ObservableCollection<ObjByType> objByTypes = new ObservableCollection<ObjByType>();
            ObjByType objByType;
            for (int i = 0; i < 1000; i++)
            {
                objByType = new ObjByType("Ar13" + i.ToString(), i.ToString());

                objByTypes.Add(objByType);
            }
            return objByTypes;
        }
    }
}
