using System.ComponentModel;
using System.Windows;
using Services.ServiceReference;


namespace Passpot.Model
{

    public class ModelBase : INotifyPropertyChanged
    {
        #region Private fields

        private ServiceDataClient _serviceDataClient;
        //private Visibility _isShowBusy = Visibility.Collapsed;
        private bool _isShowBusy = false;
        


        #endregion Private fields


        public ModelBase()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #region Public properties

        protected ServiceDataClient ServiceDataClient
        {
            get
            {
                if (_serviceDataClient == null)
                    _serviceDataClient = new ServiceDataClient();
                return _serviceDataClient;
            }

            //get { return _serviceDataClient ?? (_serviceDataClient = new ServiceDataClient()); }
        }

        public bool IsShowBusy
        {
            get { return _isShowBusy; }
            protected set
            {
                _isShowBusy = value;
                FirePropertyChanged("IsShowBusy");
               
            }
        }

 
        #endregion

        public void FirePropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
