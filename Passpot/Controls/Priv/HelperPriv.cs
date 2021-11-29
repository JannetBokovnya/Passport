using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Passpot.Business;
using Passpot.Model;
using Services.ServiceReference;

namespace Passpot.Controls.Priv
{
    public class HelperPriv
    {
        
       
        public static List<PrivItems> GetNavigationPrivListDouble(List<NavigationPriv> navigationPrivList, List<PrivItems> navigationPrivListDouble, bool radExpander, string typePriv)
        {
            List<PrivItems> NavigationPrivListDouble = new List<PrivItems>();

            NavigationPrivListDouble = navigationPrivListDouble;
            string kmOrCoor = "1";

            if (radExpander)
            {
                //передаем координаты 1
                kmOrCoor = "1";
            }
            else
            {
                //километраж 2
                kmOrCoor = "2";
            }


            if ((typePriv == "8") || (typePriv == "9"))
            {
                
            }
            else
            {
                kmOrCoor = "3";
            }
                

            for (int ii = 0; ii < navigationPrivList.Count; ii++)
            {
              
                NavigationPrivListDouble.Add(
                    new PrivItems
                    {
                        CNAME = navigationPrivList[ii].CNAME,
                        NKMORT1 = navigationPrivList[ii].NKMORT1,
                        NDISTANCEBEG = navigationPrivList[ii].NDISTANCEBEG,
                        NX1 = navigationPrivList[ii].NX1,
                        NY1 = navigationPrivList[ii].NY1,
                        NKEY = navigationPrivList[ii].NKEY,
                        NH1 = navigationPrivList[ii].NH1,
                        NKMTRUE1 = navigationPrivList[ii].NKMTRUE1,
                        NX2 = navigationPrivList[ii].NX2,
                        NY2 = navigationPrivList[ii].NY2,
                        NKMORT2 = navigationPrivList[ii].NKMORT2,
                        NKMTRUE2 = navigationPrivList[ii].NKMTRUE2,
                        NDISTANCEEND = navigationPrivList[ii].NDISTANCEEND,
                        nMtKey = navigationPrivList[ii].nMtKey.ToString(),
                        nPipeKey = navigationPrivList[ii].nPipeKey.ToString(),
                        NZ2 = navigationPrivList[ii].NZ2,
                        NZ1 = navigationPrivList[ii].NZ1,
                        NH2 = navigationPrivList[ii].NH2,
                        NBUILDTYPE = kmOrCoor,
                        ISEDITED = "1"
                    });
            }

            return NavigationPrivListDouble;
        }

        public static List<PrivItems> GetNavigationPrivListDoubleOnType10Grid(List<NavigationPriv> navigationPrivList)
        {
            List<PrivItems> NavigationPrivListDouble = new List<PrivItems>();

            for (int ii = 0; ii < navigationPrivList.Count; ii++)
            {


                NavigationPrivListDouble.Add(
                    new PrivItems
                    {
                        CNAME = navigationPrivList[ii].CNAME,
                        NKMORT1 = navigationPrivList[ii].NKMORT1,
                        NDISTANCEBEG = navigationPrivList[ii].NDISTANCEBEG,
                        NX1 = navigationPrivList[ii].NX1,
                        NY1 = navigationPrivList[ii].NY1,
                        NKEY = navigationPrivList[ii].NKEY,
                        NH1 = navigationPrivList[ii].NH1,
                        NKMTRUE1 = navigationPrivList[ii].NKMTRUE1,
                        NX2 = navigationPrivList[ii].NX2,
                        NY2 = navigationPrivList[ii].NY2,
                        NKMORT2 = navigationPrivList[ii].NKMORT2,
                        NKMTRUE2 = navigationPrivList[ii].NKMTRUE2,
                        NDISTANCEEND = navigationPrivList[ii].NDISTANCEEND,
                        nMtKey = navigationPrivList[ii].nMtKey.ToString(),
                        nPipeKey = navigationPrivList[ii].nPipeKey.ToString(),
                        NZ2 = navigationPrivList[ii].NZ2,
                        NZ1 = navigationPrivList[ii].NZ1,
                        NH2 = navigationPrivList[ii].NH2,
                        NBUILDTYPE = "1",
                        ISEDITED = navigationPrivList[ii].ISEDITED
                    });
            }
            return NavigationPrivListDouble;
        }

        public static List<PrivItems> GetNavigationPrivListDoubleOne(List<NavigationPriv> navigationPrivList, List<PrivItems> navigationPrivListDouble, bool radExpander)
        {
            List<PrivItems> NavigationPrivListDouble = new List<PrivItems>();

            NavigationPrivListDouble = navigationPrivListDouble;
            string kmOrCoor = "1";

            if (radExpander)
            {
                //передаем координаты 1
                kmOrCoor = "1";
            }
            else
            {
                //километраж 2
                kmOrCoor = "2";
            }

            for (int ii = 0; ii < navigationPrivList.Count; ii++)
            {


                NavigationPrivListDouble.Add(
                    new PrivItems
                    {
                        CNAME = navigationPrivList[ii].CNAME,
                        NKMORT1 = navigationPrivList[ii].NKMORT1,
                        NDISTANCEBEG = navigationPrivList[ii].NDISTANCEBEG,
                        NX1 = navigationPrivList[ii].NX1,
                        NY1 = navigationPrivList[ii].NY1,
                        NKEY = navigationPrivList[ii].NKEY,
                        NH1 = navigationPrivList[ii].NH1,
                        NKMTRUE1 = navigationPrivList[ii].NKMTRUE1,
                        NX2 = navigationPrivList[ii].NX2,
                        NY2 = navigationPrivList[ii].NY2,
                        NKMORT2 = navigationPrivList[ii].NKMORT2,
                        NKMTRUE2 = navigationPrivList[ii].NKMTRUE2,
                        NDISTANCEEND = navigationPrivList[ii].NDISTANCEEND,
                        nMtKey = navigationPrivList[ii].nMtKey.ToString(),
                        nPipeKey = navigationPrivList[ii].nPipeKey.ToString(),
                        NZ2 = navigationPrivList[ii].NZ2,
                        NZ1 = navigationPrivList[ii].NZ1,
                        NH2 = navigationPrivList[ii].NH2,
                        NBUILDTYPE = kmOrCoor,
                        ISEDITED = "1"
                    });
            }

            return NavigationPrivListDouble;
        }

        //наполнение данными структуры для базы(структура xml)
        public static List<SavePriv.SAVEPRIVITEMS> GetPrivItemsNew(List<NavigationPriv> navigationPrivList)
        {

            SavePriv.LISTSAVEPRIV PrivItemsNew = new SavePriv.LISTSAVEPRIV();

            PrivItemsNew.LISTPRIV = new List<SavePriv.SAVEPRIVITEMS>();


            for (int t = 0; t < navigationPrivList.Count; t++)
            {
                SavePriv.SAVEPRIVITEMS privItems = new SavePriv.SAVEPRIVITEMS();

                privItems.CNAME = navigationPrivList[t].CNAME;
                privItems.NDISTANCEBEG = navigationPrivList[t].NDISTANCEBEG;
                privItems.NDISTANCEEND = navigationPrivList[t].NDISTANCEEND;
                privItems.NH1 = navigationPrivList[t].NH1;
                privItems.NH2 = navigationPrivList[t].NH2;
                privItems.NKEY = navigationPrivList[t].NKEY;
                privItems.NKMORT1 = navigationPrivList[t].NKMORT1;
                privItems.NKMORT2 = navigationPrivList[t].NKMORT2;
                privItems.NKMTRUE1 = navigationPrivList[t].NKMTRUE1;
                privItems.NKMTRUE2 = navigationPrivList[t].NKMTRUE2;
                privItems.NX1 = navigationPrivList[t].NX1;
                privItems.NX2 = navigationPrivList[t].NX2;
                privItems.NY1 = navigationPrivList[t].NY1;
                privItems.NY2 = navigationPrivList[t].NY2;
                privItems.NZ2 = navigationPrivList[t].NZ2;
                privItems.NMTKEY = navigationPrivList[t].nMtKey.ToString();
                privItems.NPIPEKEY = navigationPrivList[t].nPipeKey.ToString();
                privItems.NBUILDTYPE = navigationPrivList[t].NBUILDTYPE;
                privItems.ISEDITED = navigationPrivList[t].ISEDITED;

                PrivItemsNew.LISTPRIV.Add(privItems);
            }


            return PrivItemsNew.LISTPRIV;
        }

        public static List<PrivItems> GetNavigationPrivListOnAddPrivButton()
        {
            List<PrivItems> result = new List<PrivItems>();
            return result;
        }

        public static List<PrivItems> GetNavigationPrivListOnPipe(List<PrivItems> listPrivItems, string keyMg, string keyNit)
        {
            List<PrivItems> result = new List<PrivItems>();
            listPrivItems.Add(
                                   new PrivItems
                                   {
                                       CNAME = "",
                                       NKMORT1 = "",
                                       NDISTANCEBEG = "",
                                       NX1 = "",
                                       NY1 = "",
                                       NKEY = "",
                                       NH1 = "",
                                       NKMTRUE1 = "",
                                       NX2 = "",
                                       NY2 = "",
                                       NKMORT2 = "",
                                       NKMTRUE2 = "",
                                       NDISTANCEEND = "",
                                       nMtKey = keyMg,
                                       nPipeKey = keyNit,
                                       NZ2 = "",
                                       NZ1 = "",
                                       NH2 = "",
                                       NBUILDTYPE = "3",
                                       ISEDITED = "1"
                                       
                                   });

            result = listPrivItems;
            return result;

        }

        //в зависимости от редактирования или добавления нового формируем массив для передачи в базу(расчет)
        public static List<PrivItems> GetNavigationPrivList(string newUpdateItem, List<NavigationPriv> navigationPrivList, NavigationPriv np, int index, bool radExpander, string typePriv)
        {
            List<PrivItems> NavigationPrivList= new List<PrivItems>();
            //создаем новый

            string kmOrCoor = "1";

            if (radExpander)
            {
                //передаем координаты 1
                kmOrCoor = "1";
            }
            else
            {
                //километраж 2
                kmOrCoor = "2";
            }

            if (newUpdateItem == "1")
            {
                for (int ii = 0; ii < navigationPrivList.Count; ii++)
                {
                    
                    NavigationPrivList.Add(
                                     new PrivItems
                                     {
                                         CNAME = "",
                                         NKMORT1 = navigationPrivList[ii].NKMORT1,
                                         NDISTANCEBEG = navigationPrivList[ii].NDISTANCEBEG,
                                         NX1 = navigationPrivList[ii].NX1,
                                         NY1 = navigationPrivList[ii].NY1,
                                         NKEY = navigationPrivList[ii].NKEY,
                                         NH1 = navigationPrivList[ii].NH1,
                                         NKMTRUE1 = navigationPrivList[ii].NKMTRUE1,
                                         NX2 = navigationPrivList[ii].NX2,
                                         NY2 = navigationPrivList[ii].NY2,
                                         NKMORT2 = navigationPrivList[ii].NKMORT2,
                                         NKMTRUE2 = navigationPrivList[ii].NKMTRUE2,
                                         NDISTANCEEND = navigationPrivList[ii].NDISTANCEEND,
                                         nMtKey = navigationPrivList[ii].nMtKey.ToString(),
                                         nPipeKey = navigationPrivList[ii].nPipeKey.ToString(),
                                         NZ2 = navigationPrivList[ii].NZ2,
                                         NZ1 = navigationPrivList[ii].NZ1,
                                         NH2 = navigationPrivList[ii].NH2,
                                         NBUILDTYPE = navigationPrivList[ii].NBUILDTYPE,
                                         ISEDITED = "0"
                                     });
                }

                List<NavigationPriv> lnp = new List<NavigationPriv>();
                lnp.Add(np);
                NavigationPrivList = HelperPriv.GetNavigationPrivListDouble(lnp, NavigationPrivList, radExpander, typePriv);

                //немного путанно но пока не знаю как с этим бороться - изначально ставит редактировать, но если новый
                // то ставим один и сразу 2
                
            }
                //редактирование
            else
            {
                for (int ii = 0; ii < navigationPrivList.Count; ii++)
                {
                    if (index == ii)
                    {
                        List<NavigationPriv> lnp = new List<NavigationPriv>();
                        lnp.Add(np);

                       
                            NavigationPrivList.Add(
                                         new PrivItems
                                         {
                                             CNAME = "",
                                             NKMORT1 = lnp[0].NKMORT1,
                                             NDISTANCEBEG = lnp[0].NDISTANCEBEG,
                                             NX1 = lnp[0].NX1,
                                             NY1 = lnp[0].NY1,
                                             NKEY = lnp[0].NKEY,
                                             NH1 = lnp[0].NH1,
                                             NKMTRUE1 = lnp[0].NKMTRUE1,
                                             NX2 = lnp[0].NX2,
                                             NY2 = lnp[0].NY2,
                                             NKMORT2 = lnp[0].NKMORT2,
                                             NKMTRUE2 = lnp[0].NKMTRUE2,
                                             NDISTANCEEND = lnp[0].NDISTANCEEND,
                                             nMtKey = lnp[0].nMtKey.ToString(),
                                             nPipeKey = lnp[0].nPipeKey.ToString(),
                                             NZ2 = lnp[0].NZ2,
                                             NZ1 = lnp[0].NZ1,
                                             NH2 = lnp[0].NH2,
                                             NBUILDTYPE = kmOrCoor,
                                             ISEDITED = "1"
                                         });
                    }
                    else
                    {
                        
                        NavigationPrivList.Add(
                                         new PrivItems
                                         {
                                             CNAME = "",
                                             NKMORT1 = navigationPrivList[ii].NKMORT1,
                                             NDISTANCEBEG = navigationPrivList[ii].NDISTANCEBEG,
                                             NX1 = navigationPrivList[ii].NX1,
                                             NY1 = navigationPrivList[ii].NY1,
                                             NKEY = navigationPrivList[ii].NKEY,
                                             NH1 = navigationPrivList[ii].NH1,
                                             NKMTRUE1 = navigationPrivList[ii].NKMTRUE1,
                                             NX2 = navigationPrivList[ii].NX2,
                                             NY2 = navigationPrivList[ii].NY2,
                                             NKMORT2 = navigationPrivList[ii].NKMORT2,
                                             NKMTRUE2 = navigationPrivList[ii].NKMTRUE2,
                                             NDISTANCEEND = navigationPrivList[ii].NDISTANCEEND,
                                             nMtKey = navigationPrivList[ii].nMtKey.ToString(),
                                             nPipeKey = navigationPrivList[ii].nPipeKey.ToString(),
                                             NZ2 = navigationPrivList[ii].NZ2,
                                             NZ1 = navigationPrivList[ii].NZ1,
                                             NH2 = navigationPrivList[ii].NH2,
                                            // NBUILDTYPE = navigationPrivList[ii].NBUILDTYPE,
                                             NBUILDTYPE = "1",
                                             ISEDITED = "0"
                                         });
                    }

                }
            }

            return NavigationPrivList;
        }

        public static List<NavigationPriv> GetNavigationPrivListInGrid10Type(string newUpdateItem,
                                                                       List<NavigationPriv> navigationPrivList,
                                                                       NavigationPriv np, int index, bool radExpander)
        {
            List<NavigationPriv>result = new List<NavigationPriv>();

            if (newUpdateItem == "1")
            {
                for (int ii = 0; ii < navigationPrivList.Count; ii++)
                {

                    result.Add(
                                     new NavigationPriv
                                     (
                                         "",
                                         navigationPrivList[ii].NKMORT1,
                                         navigationPrivList[ii].NDISTANCEBEG,
                                         navigationPrivList[ii].NX1,
                                         navigationPrivList[ii].NY1,
                                         navigationPrivList[ii].NKEY,
                                         navigationPrivList[ii].NH1,
                                         navigationPrivList[ii].NKMTRUE1,
                                         navigationPrivList[ii].NX2,
                                         navigationPrivList[ii].NY2,
                                         navigationPrivList[ii].NKMORT2,
                                         navigationPrivList[ii].NKMTRUE2,
                                         navigationPrivList[ii].NDISTANCEEND,
                                         0,
                                         0,
                                         navigationPrivList[ii].NZ2,
                                         navigationPrivList[ii].NZ1,
                                         navigationPrivList[ii].NH2,
                                         "2",
                                         "0"
                                     ));
                }

                List<NavigationPriv> lnp = new List<NavigationPriv>();
                lnp.Add(np);

                result.Add(
                                    new NavigationPriv
                                    (
                                        "",
                                        lnp[0].NKMORT1,
                                        lnp[0].NDISTANCEBEG,
                                        lnp[0].NX1,
                                        lnp[0].NY1,
                                        lnp[0].NKEY,
                                        lnp[0].NH1,
                                        lnp[0].NKMTRUE1,
                                        lnp[0].NX2,
                                        lnp[0].NY2,
                                        lnp[0].NKMORT2,
                                        lnp[0].NKMTRUE2,
                                        lnp[0].NDISTANCEEND,
                                        0,
                                        0,
                                        lnp[0].NZ2,
                                        lnp[0].NZ1,
                                        lnp[0].NH2,
                                        "2",
                                        "1"
                                    ));
                

              

            }
            //редактирование
            else
            {
                for (int ii = 0; ii < navigationPrivList.Count; ii++)
                {
                    if (index == ii)
                    {
                        List<NavigationPriv> lnp = new List<NavigationPriv>();
                        lnp.Add(np);


                        result.Add(
                                     new NavigationPriv
                                     (
                                         "",
                                         lnp[0].NKMORT1,
                                         lnp[0].NDISTANCEBEG,
                                         lnp[0].NX1,
                                         lnp[0].NY1,
                                         lnp[0].NKEY,
                                         lnp[0].NH1,
                                         lnp[0].NKMTRUE1,
                                         lnp[0].NX2,
                                         lnp[0].NY2,
                                         lnp[0].NKMORT2,
                                         lnp[0].NKMTRUE2,
                                         lnp[0].NDISTANCEEND,
                                         0,
                                         0,
                                         lnp[0].NZ2,
                                         lnp[0].NZ1,
                                         lnp[0].NH2,
                                         "2",
                                         "1"
                                     ));
                    }
                    else
                    {

                        result.Add(
                                        new NavigationPriv
                                     (
                                             "",
                                             navigationPrivList[ii].NKMORT1,
                                             navigationPrivList[ii].NDISTANCEBEG,
                                             navigationPrivList[ii].NX1,
                                             navigationPrivList[ii].NY1,
                                             navigationPrivList[ii].NKEY,
                                             navigationPrivList[ii].NH1,
                                             navigationPrivList[ii].NKMTRUE1,
                                             navigationPrivList[ii].NX2,
                                             navigationPrivList[ii].NY2,
                                             navigationPrivList[ii].NKMORT2,
                                             navigationPrivList[ii].NKMTRUE2,
                                             navigationPrivList[ii].NDISTANCEEND,
                                             0,
                                             0,
                                             navigationPrivList[ii].NZ2,
                                             navigationPrivList[ii].NZ1,
                                             navigationPrivList[ii].NH2,
                                             "2",
                                             "0"
                                         ));
                    }

                }
            }

            return result;
        }
        //из xml в структуру привязки...
        public static List<PrivItems> GetXmlOut(string xmlString, PassportDetailModel Model)
        {

            List<PrivItems> result = new List<PrivItems>();
            SavePriv.LISTSAVEPRIV listsavpriv = new SavePriv.LISTSAVEPRIV();
            listsavpriv.LISTPRIV = new List<SavePriv.SAVEPRIVITEMS>();
            try
            {
                //listsavpriv = JsonHelper.JsonDeserialize<SavePriv.ListSavePriv>(jsonString);
                listsavpriv = XmlHelper.Deserialize<SavePriv.LISTSAVEPRIV>(xmlString);
                string nkey = "";

                for (int ii = 0; ii < listsavpriv.LISTPRIV.Count; ii++)
                {
                    if (string.IsNullOrEmpty(listsavpriv.LISTPRIV[ii].NKEY))
                    {
                        nkey = ii.ToString();
                    }
                    else
                    {
                        nkey = listsavpriv.LISTPRIV[ii].NKEY;
                    }
                    result.Add(
                           new PrivItems
                           {

                               CNAME = listsavpriv.LISTPRIV[ii].CNAME,
                               NKMORT1 = listsavpriv.LISTPRIV[ii].NKMORT1,
                               NDISTANCEBEG = listsavpriv.LISTPRIV[ii].NDISTANCEBEG,
                               NX1 = listsavpriv.LISTPRIV[ii].NX1,
                               NY1 = listsavpriv.LISTPRIV[ii].NY1,
                               NZ1 = listsavpriv.LISTPRIV[ii].NZ1,
                               NKEY = nkey,
                               NH1 = listsavpriv.LISTPRIV[ii].NH1,
                               NKMTRUE1 = listsavpriv.LISTPRIV[ii].NKMTRUE1,
                               NX2 = listsavpriv.LISTPRIV[ii].NX2,
                               NY2 = listsavpriv.LISTPRIV[ii].NY2,
                               NZ2 = listsavpriv.LISTPRIV[ii].NZ2,
                               NH2 = listsavpriv.LISTPRIV[ii].NH2,
                               NKMORT2 = listsavpriv.LISTPRIV[ii].NKMORT2,
                               NKMTRUE2 = listsavpriv.LISTPRIV[ii].NKMTRUE2,
                               NDISTANCEEND = listsavpriv.LISTPRIV[ii].NDISTANCEEND,
                               nMtKey = listsavpriv.LISTPRIV[ii].NMTKEY,
                               nPipeKey = listsavpriv.LISTPRIV[ii].NPIPEKEY,
                               NBUILDTYPE = listsavpriv.LISTPRIV[ii].NBUILDTYPE,
                               ISEDITED = listsavpriv.LISTPRIV[ii].ISEDITED

                           });

                }



                //result = JsonHelper.JsonDeserialize<List<PrivItems>>(jsonString);
            }
            catch (Exception ex)
            {

                Model.MainModel.Report("ошибка XmlDeserialize jsonString =  " + xmlString + ex.Message);
            }
            return result;
        }
        //
        public static string GetXmlInNewPriv(List<PrivItems> ListprivItems, PassportDetailModel Model)
        {
            string xmls = "";

            SavePriv.LISTSAVEPRIV listsavpriv = new SavePriv.LISTSAVEPRIV();
            listsavpriv.LISTPRIV = new List<SavePriv.SAVEPRIVITEMS>();

            try
            {


                if (ListprivItems.Count > 0)
                {
                    for (int ii = 0; ii < ListprivItems.Count; ii++)
                    {
                        SavePriv.SAVEPRIVITEMS privItems = new SavePriv.SAVEPRIVITEMS();

                        privItems.CNAME = ListprivItems[ii].CNAME;
                        privItems.NDISTANCEBEG = ListprivItems[ii].NDISTANCEBEG;
                        privItems.NDISTANCEEND = ListprivItems[ii].NDISTANCEEND;
                        privItems.NH1 = ListprivItems[ii].NH1;
                        privItems.NH2 = ListprivItems[ii].NH2;
                        privItems.NKEY = ListprivItems[ii].NKEY;
                        privItems.NKMORT1 = ListprivItems[ii].NKMORT1;
                        privItems.NKMORT2 = ListprivItems[ii].NKMORT2;
                        privItems.NKMTRUE1 = ListprivItems[ii].NKMTRUE1;
                        privItems.NKMTRUE2 = ListprivItems[ii].NKMTRUE2;
                        privItems.NX1 = ListprivItems[ii].NX1;
                        privItems.NX2 = ListprivItems[ii].NX2;
                        privItems.NY1 = ListprivItems[ii].NY1;
                        privItems.NY2 = ListprivItems[ii].NY2;
                        privItems.NZ1 = ListprivItems[ii].NZ1;
                        privItems.NZ2 = ListprivItems[ii].NZ2;
                        privItems.NMTKEY = ListprivItems[ii].nMtKey;
                        privItems.NPIPEKEY = ListprivItems[ii].nPipeKey;
                        privItems.NBUILDTYPE = ListprivItems[ii].NBUILDTYPE;
                        privItems.ISEDITED = ListprivItems[ii].ISEDITED;

                        listsavpriv.LISTPRIV.Add(privItems);
                    }
                }

                // strJson = JsonHelper.JsonSerializer<SavePriv.ListSavePriv>(listsavpriv);
                xmls = XmlHelper.InternalSerializer<SavePriv.LISTSAVEPRIV>(listsavpriv);
            }
            catch (Exception ex)
            {

                Model.MainModel.Report("ошибка InternalSerializer jsonString =  " + ex.Message);
            }
            return xmls;
        }
    }
}

//if (newUpdateItem == "1")
//{
//    for (int ii = 0; ii < Model.NavigationPrivList.Count; ii++)
//    {

//            NavigationPrivListDouble.Add(
//                             new PrivItems
//                             {
//                                 CNAME = Model.NavigationPrivList[ii].CNAME,
//                                 NKMORT1 = Model.NavigationPrivList[ii].NKMORT1,
//                                 NDISTANCEBEG = Model.NavigationPrivList[ii].NDISTANCEBEG,
//                                 NX1 = Model.NavigationPrivList[ii].NX1,
//                                 NY1 = Model.NavigationPrivList[ii].NY1,
//                                 NKEY = Model.NavigationPrivList[ii].NKEY,
//                                 NH1 = Model.NavigationPrivList[ii].NH1,
//                                 NKMTRUE1 = Model.NavigationPrivList[ii].NKMTRUE1,
//                                 NX2 = Model.NavigationPrivList[ii].NX2,
//                                 NY2 = Model.NavigationPrivList[ii].NY2,
//                                 NKMORT2 = Model.NavigationPrivList[ii].NKMORT2,
//                                 NKMTRUE2 = Model.NavigationPrivList[ii].NKMTRUE2,
//                                 NDISTANCEEND = Model.NavigationPrivList[ii].NDISTANCEEND,
//                                 nMtKey = Model.NavigationPrivList[ii].nMtKey.ToString(),
//                                 nPipeKey = Model.NavigationPrivList[ii].nPipeKey.ToString(),
//                                 NZ2 = Model.NavigationPrivList[ii].NZ2,
//                                 NZ1 = Model.NavigationPrivList[ii].NZ1,
//                                 NH2 = Model.NavigationPrivList[ii].NH2
//                             });
//        }

//    List<NavigationPriv> lnp = new List<NavigationPriv>();
//    lnp.Add(np);
//    NavigationPrivListDouble = HelperPriv.GetNavigationPrivListDouble(lnp, NavigationPrivListDouble);

//    //немного путанно но пока не знаю как с этим бороться - изначально ставит редактировать, но если новый
//    // то ставим один и сразу 2
//    newUpdateItem = "2";
//}
//else
//{
//    for (int ii = 0; ii < Model.NavigationPrivList.Count; ii++)
//    {
//        if (index == ii)
//        {
//            List<NavigationPriv> lnp = new List<NavigationPriv>();
//            lnp.Add(np);
//            NavigationPrivListDouble = HelperPriv.GetNavigationPrivListDouble(lnp, NavigationPrivListDouble);
//        }
//        else
//        {
//            NavigationPrivListDouble.Add(
//                             new PrivItems
//                                  {
//                                      CNAME = Model.NavigationPrivList[ii].CNAME,
//                                      NKMORT1 = Model.NavigationPrivList[ii].NKMORT1,
//                                      NDISTANCEBEG = Model.NavigationPrivList[ii].NDISTANCEBEG,
//                                      NX1 = Model.NavigationPrivList[ii].NX1,
//                                      NY1 = Model.NavigationPrivList[ii].NY1,
//                                      NKEY = Model.NavigationPrivList[ii].NKEY,
//                                      NH1 = Model.NavigationPrivList[ii].NH1,
//                                      NKMTRUE1 = Model.NavigationPrivList[ii].NKMTRUE1,
//                                      NX2 = Model.NavigationPrivList[ii].NX2,
//                                      NY2 = Model.NavigationPrivList[ii].NY2,
//                                      NKMORT2 = Model.NavigationPrivList[ii].NKMORT2,
//                                      NKMTRUE2 = Model.NavigationPrivList[ii].NKMTRUE2,
//                                      NDISTANCEEND = Model.NavigationPrivList[ii].NDISTANCEEND,
//                                      nMtKey = Model.NavigationPrivList[ii].nMtKey.ToString(),
//                                      nPipeKey = Model.NavigationPrivList[ii].nPipeKey.ToString(),
//                                      NZ2 = Model.NavigationPrivList[ii].NZ2,
//                                      NZ1 = Model.NavigationPrivList[ii].NZ1,
//                                      NH2 = Model.NavigationPrivList[ii].NH2
//                                  });
//        }

//    }
//}
