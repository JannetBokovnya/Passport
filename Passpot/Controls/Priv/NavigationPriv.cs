using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Passpot.Controls.Priv
{
    public class NavigationPriv
    {
        public string CNAME { get; set; }
        public string NKMORT1 { get; set; }
        public string NDISTANCEBEG { get; set; }
        public string NX1 { get; set; }
        public string NY1 { get; set; }
        public string NKEY { get; set; }
        public string NH1 { get; set; }
        public string NKMTRUE1 { get; set; }
        public string NX2 { get; set; }
        public string NY2 { get; set; }
        public string NKMORT2 { get; set; }
        public string NKMTRUE2 { get; set; }
        public string NDISTANCEEND { get; set; }
        public double nMtKey { get; set; }
        public double nPipeKey { get; set; }
        public string NZ2 { get; set; }
        public string NZ1 { get; set; }
        public string NH2 { get; set; }
        public string NBUILDTYPE { get; set; }
        public string ISEDITED { get; set; }



        //класс навиации для определенных привязок - например 5
        public NavigationPriv(string cNAME, string nKMORT1, string nDISTANCEBEG, string nX1, string nY1, string nKEY, string nH1, string nKMTRUE1,
                               string nX2, string nY2, string nKMORT2, string nKMTRUE2, string nDISTANCEEND, double mG, double pipe, string nz2,
                               string nh2, string nz1, string nbuildType, string isEdited)
        {
            CNAME = cNAME;
            NKMORT1 = nKMORT1;
            NDISTANCEBEG = nDISTANCEBEG;
            NX1=nX1;
            NY1 = nY1;
            NKEY = nKEY;
            NH1 = nH1;
            NKMTRUE1 = nKMTRUE1;
            NX2 = nX2;
            NY2 = nY2;
            NKMORT2 = nKMORT2;
            NKMTRUE2 = nKMTRUE2;
            NDISTANCEEND = nDISTANCEEND;
            nMtKey = mG;
            nPipeKey = pipe;
            NZ2 =nz2;
            NZ1 = nz1;
            NH2 = nh2;
            NBUILDTYPE = nbuildType;
            ISEDITED = isEdited;

        }


        public bool IsEquals(NavigationPriv np, string typeLink)
        {
            switch (typeLink)
            {
                case "1":
                    {
                        bool t = this.nMtKey == np.nMtKey && this.nPipeKey == np.nPipeKey && this.NKMORT1 == np.NKMORT1 && this.NDISTANCEBEG == np.NDISTANCEBEG && 
                            this.NX1 == np.NX1 && this.NY1 == np.NY1;

                        return t;

                    }
                case "2":
                    {
                        bool t = this.nMtKey == np.nMtKey && this.nPipeKey == np.nPipeKey && this.NKMORT1 == np.NKMORT1 && this.NDISTANCEBEG == np.NDISTANCEBEG && 
                            this.NX1 == np.NX1 && this.NY1 == np.NY1;

                        return t;

                    }

                case "3":
                    {
                        bool t = this.nMtKey == np.nMtKey && this.nPipeKey == np.nPipeKey && this.NKMORT1 == np.NKMORT1 && this.NDISTANCEBEG == np.NDISTANCEBEG && 
                            this.NX1 == np.NX1 && this.NY1 == np.NY1 && this.NKMORT2 == np.NKMORT2 && this.NX2 == np.NX2 && this.NY2 == np.NX2;

                        return t;

                    }

                case "5":
                    {
                        bool t = this.nMtKey == np.nMtKey && this.nPipeKey == np.nPipeKey && this.NKMORT1 == np.NKMORT1 && this.NDISTANCEBEG == np.NDISTANCEBEG && this.NX1 == np.NX1 && this.NY1 == np.NY1;

                        return t;
                 
                    }
                case "6":
                    {
                        bool t = this.nMtKey == np.nMtKey && this.nPipeKey == np.nPipeKey && this.NKMORT1 == np.NKMORT1 &&
                                 NKMORT2 == np.NKMORT2 && this.NDISTANCEBEG == np.NDISTANCEBEG && this.NX1 == np.NX1
                                 && this.NY1 == np.NY1 && this.NX2 == np.NX2 && this.NY2 == np.NY2; 
                        //NavigationPriv np = new NavigationPriv(nameNit, tbKmA1, tbd1, dolg1, shirot1, "", "", "", "", "", "", "", "", keyMgDouble, keyPipeDouble, "", "", "", "", "");
                        return t;
                    }
                case "7":
                    {
                        bool t =  this.NX1 == np.NX1 && this.NY1 == np.NY1;

                        return t;

                    }

                case "8":
                    {
                        bool t = this.nMtKey == np.nMtKey && this.nPipeKey == np.nPipeKey && this.NKMORT1 == np.NKMORT1 &&
                                 this.NX1 == np.NX1 && this.NY1 == np.NY1 ;
                        //bool t = this.nMtKey == np.nMtKey && this.nPipeKey == np.nPipeKey && this.NKMORT1 == np.NKMORT1 &&
                        //         NKMORT2 == np.NKMORT2 && this.NDISTANCEBEG == np.NDISTANCEBEG && this.NX1 == np.NX1
                        //         && this.NY1 == np.NY1 && this.NX2 == np.NX2 && this.NY2 == np.NY2;
                        //NavigationPriv np = new NavigationPriv(nameNit, tbKmA1, tbd1, dolg1, shirot1, "", "", "", "", "", "", "", "", keyMgDouble, keyPipeDouble, "", "", "", "", "");
                        return t;
                    }
                case "9":
                    {

                        bool t = this.nMtKey == np.nMtKey && this.nPipeKey == np.nPipeKey && this.NKMORT1 == np.NKMORT1 &&
                                NKMORT2 == np.NKMORT2 && this.NX1 == np.NX1 && this.NY1 == np.NY1 && this.NX2 == np.NX2 && this.NY2 == np.NY2;
                        return t;

                    }
                case "10":
                    {

                        bool t = this.NX1 == np.NX1 && this.NY1 == np.NY1 ;
                        return t;

                    }
                  
                default:
                    {
                       
                        
                        return true; 
                    }
                   
            }

           
        }

       
    }
}
