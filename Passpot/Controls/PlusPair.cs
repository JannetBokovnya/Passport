using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using Services.ServiceReference;

namespace Passpot.Controls
{
    public class PlusPair : PairItem
    {
        protected SolidColorBrush _brush;

        protected FontWeight _weight;

        protected FontStyle _style;

        protected int _fontSize;

        protected string _path;

        public PlusPair()
        {
            this.Children = new ObservableCollection<PlusPair>();
           
        }

        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
                RaisePropertyChanged("Puth");
            }
        }

        public SolidColorBrush Brush
        {
            get
            {
                return _brush;
            }
            set
            {
                _brush = value;
                RaisePropertyChanged("Brush");
            }
        }

        public FontWeight Weight
        {
            get
            {
                return _weight;
            }
            set
            {
                _weight = value;
                RaisePropertyChanged("Weight");
            }
        }
        public FontStyle Style
        {
            get
            {
                return _style;
            }
            set
            {
                _style = value;
                RaisePropertyChanged("Style");
            }
        }

        public int Size
        {
            get
            {
                return _fontSize;
            }
            set
            {
                _fontSize = value;
                RaisePropertyChanged("Size");
            }
        }
        public IList<PlusPair> Children
        {
            get;
            protected set;
        }

        public Visibility Visibility
        {
            get
            {
                return _visibility;
            }
            set
            {
                _visibility = value;
                RaisePropertyChanged("Visible");
            }
        }



        public System.Windows.Visibility _visibility { get; set; }
    }
}
