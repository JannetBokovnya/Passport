using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Media.Interfaces;
using Passpot.Business;
using Passpot.Model;
using Services.ServiceReference;

namespace Passpot.Controls.Priv
{
    public class PrivChildModel : ModelBase
    {
        #region Ctor

        public PrivChildModel(string key, bool isEdited, List<PrivItems> listPriwItems, PassportDetailModel passportDetailModel, string typeLink)
        {
            IsEdited = isEdited;
            TypeLink = typeLink;
            NavigationPrivList = new List<NavigationPriv>();
            LoadData(key, 0, listPriwItems);
            PassportDetailModel = passportDetailModel;
        }

        #endregion Ctor

        public IMainModel MainModel { get; private set; }
        public PassportDetailModel PassportDetailModel { get; private set; }
        //public event PropertyChangedEventHandler PropertyChanged;

        public void LoadData(string key, int index, List<PrivItems> listPriwItems)
        {
            NavigationPrivList.Clear();

            for (int ii = 0; ii < listPriwItems.Count; ii++)
            {
                double mtkey = 0d;
                Double.TryParse(listPriwItems[ii].nMtKey, out mtkey);

                double pipekey = 0d;
                Double.TryParse(listPriwItems[ii].nPipeKey, out pipekey);

                NavigationPrivList.Add(
                  new NavigationPriv
                  (
                     listPriwItems[ii].CNAME,
                     listPriwItems[ii].NKMORT1,
                     listPriwItems[ii].NDISTANCEBEG,
                     listPriwItems[ii].NX1,
                     listPriwItems[ii].NY1,
                     listPriwItems[ii].NKEY,
                     listPriwItems[ii].NH1,
                     listPriwItems[ii].NKMTRUE1,
                     listPriwItems[ii].NX2,
                     listPriwItems[ii].NY2,
                     listPriwItems[ii].NKMORT2,
                     listPriwItems[ii].NKMTRUE2,
                     listPriwItems[ii].NDISTANCEEND,
                     mtkey,
                     pipekey,
                     listPriwItems[ii].NZ2,
                     listPriwItems[ii].NZ1,
                     listPriwItems[ii].NH2,
                     listPriwItems[ii].NBUILDTYPE,
                     listPriwItems[ii].ISEDITED
                  ));
            }


            if (index < NavigationPrivList.Count)
            {
                CurrentNavigationPriv = NavigationPrivList[index];
            }
            else
            {
                CurrentNavigationPriv = NavigationPrivList.FirstOrDefault();
            }
        }

       
        public bool IsEdited { get; private set; }
        public string TypeLink { get; private set; }
        public List<NavigationPriv> NavigationPrivList { get; private set; }

        public NavigationPriv CurrentNavigationPriv { get; private set; }

        public bool DataHasChanged(NavigationPriv fromControls)
        {
            return !CurrentNavigationPriv.IsEquals(fromControls, TypeLink);
        }

        //public bool DataIsNotNullChang(AllDataForm allDataForm, bool isExpanded)
        //{
        //    return CurrentNavigationPriv.GetData(allDataForm, TypeLink, isExpanded);
        //}
        public int CurrentIndex
        {
            get { return NavigationPrivList.IndexOf(CurrentNavigationPriv); }
        }

        public int MaxIndex
        {
            get { return (NavigationPrivList.Count - 1); }
        }

        public int FistIndex
        {
            get { return 0; }
        }


        public void GoNextZak()
        {
            CurrentNavigationPriv = NavigationPrivList[CurrentIndex + 1];
        }

        public void GoLastZak()
        {
            CurrentNavigationPriv = NavigationPrivList[MaxIndex];
        }

        public void GoFistZak()
        {
            CurrentNavigationPriv = NavigationPrivList[FistIndex];
        }

        public void GoPrevZak()
        {
            CurrentNavigationPriv = NavigationPrivList[CurrentIndex - 1];
        }

        public void GoCurrentZak()
        {
            CurrentNavigationPriv = NavigationPrivList[CurrentIndex];
        }
        internal bool перерасчет_привязки_в_базе()
        {
            return true;
        }

        public bool GetData(AllDataForm allDataForm, bool isExpanded)
        {
            bool result = true;
            switch (TypeLink)
            {
                case "8":
                    {
                        if (isExpanded)
                        {
                            if ((string.IsNullOrEmpty(allDataForm.dolg1)) | (string.IsNullOrEmpty(allDataForm.shirot1)))
                            {
                                result = false;
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(allDataForm.tbKm1))
                            {
                                result = false;
                            }
                        }
                        break;
                    }
                case "7":
                    {
                        if ((string.IsNullOrEmpty(allDataForm.shirotA1)) | (string.IsNullOrEmpty(allDataForm.dolgA1)))
                        {
                            result = false;
                        }
                        break;
                    }
            }


            return result;
        }

    }
}
