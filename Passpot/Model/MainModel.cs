using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Media;
using Media.Interfaces;
using Media.Resources;
using Passpot.Business;
using Passpot.Model;

namespace Passpot.Model
{
    public class MainModel : ModelBase, IMainModel
    {
       

        private PassportOpenParam _needOpenPassport;
        private TreeOpenParam _needOpenTree;
        private PassportDetailOpenParam _needOpenDetailPassport;
        private string _stringJason;
        private bool _isClosePassportDetail;
        private string _getSelectTree;
        private Stack<Navigation> _sBack;
        private Stack<Navigation> _sForward;
        private string _version;
       
        
        
        

        public MainModel()
        {
            Reports = new ObservableCollection<string>();
            Pr =  ProjectResources.GridControlMessageTitle;
            sBack = new Stack<Navigation>();
            sForward = new Stack<Navigation>();
            KeyOntree = "";
            Version = "";
            VisibleNode = "0";

        }

        public PassportOpenParam NeedOpenPassport
        {
            get
            {
                return _needOpenPassport;
            }
            set
            {
                _needOpenPassport = value;
                FirePropertyChanged("NeedOpenPassport");
            }
        }

        public TreeOpenParam NeedOpenTree
        {
            get
            {
                return _needOpenTree;
            }
            set
            {
                _needOpenTree = value;
                FirePropertyChanged("TreeModelInited");
            }
        }



        public string stringJason
        {
            get { return _stringJason; }
            set { _stringJason = value; }
        }

        public string GetSelectTree
        {
            get { return _getSelectTree; }
            set
            {
                _getSelectTree = value;

            }
        }
        
        /// <summary>
        /// Свойство, в котором содержится ключ пасспорта для которого нужно открыть окно PassportDetail.
        /// Для того что бы открыть окно необходимо присвоить этому свойству ключ паспорта
        /// </summary>
        public PassportDetailOpenParam NeedOpenPassportDetail
        {
            get
            {
                return _needOpenDetailPassport;
            }
            set
            {
                _needOpenDetailPassport = value;
                //посылаем всем "подписчекам" сообщение, о том что изменилось свойство NeedOpenPassportDetail
                FirePropertyChanged("NeedOpenPassportDetail");
            }
        }




        public ObservableCollection<string> Reports { get; private set; }

        public string Pr { get; private set; }

        public string KeyOntree { get;  set; }

        public string VisibleNode { get; set; }

        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        //public Stack<Navigation> sBack 
        //{
        //    get
        //    {
        //        return _sBack;
        //    }
        //    set
        //    {
        //        _sBack = value;
                
        //    } 
        //} //стек для кнопки вперед-назад
        public Stack<Navigation> sForward  { get;  set; }
        public Stack<Navigation> sBack  { get;  set; }
        

        public string Path { get; set; } //путь поиска для дерева(нужно для кнопки)
        

        public void Report(string message)
        {
            Reports.Add(string.Format("{0}, {1}", DateTime.Now.ToLongTimeString(), message));
        }

        public void Indicator(bool show)
        {
            IsShowBusy = show;
        }

        internal void Report()
        {
            throw new NotImplementedException();
        }

        

       
    }
}
