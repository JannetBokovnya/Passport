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
using Services.ServiceReference;
using Telerik.Windows.Controls;

namespace Passpot.Controls.Priv
{
    public class AllDataForm
    {

        public string tbKmA1 = "";
        public string tbKmB1 = "";
        public string tbKm1 = "";
        public string tbd1 = "";
        public string dolg1 = "";
        public string dolg1_1 = "";
        public string dolgA1 = "";
        public string dolgB1 = "";
        public string shirot1 = "";
        public string shirot1_1 = "";
        public string shirotA1 = "";
        public string shirotB1 = "";
        public string kmOrCoor = "1";
        public double keyMgDouble = 0d;
        public double keyPipeDouble = 0d;

        public AllDataForm(WatermarkTextBox tbKmA, WatermarkTextBox tbKmB, WatermarkTextBox tbKm, WatermarkTextBox tbd, WatermarkTextBox dolg, WatermarkTextBox dolgA, WatermarkTextBox dolgB,
                              WatermarkTextBox shirot, WatermarkTextBox shirotA, WatermarkTextBox shirotB, RadioButton rbr, RadExpander radExpander, string keyMg, string keyNit,
                              WatermarkTextBox shirot11, WatermarkTextBox dolg11)
        {
            if (tbKm.Text != tbKm.Watermark)
            {
                tbKm1 = tbKm.Text.Replace(" ", ""); 
            }
            if (tbKmA.Text != tbKmA.Watermark)
            {
                tbKmA1 = tbKmA.Text.Replace(" ", ""); 
            }
            if (tbKmB.Text != tbKmB.Watermark)
            {
                tbKmB1 = tbKmB.Text.Replace(" ", ""); ;
            }
            if (tbd.Text != tbd.Watermark)
            {
                tbd1 = tbd.Text.Replace(" ", ""); ;
            }
            if (dolg.Text != dolg.Watermark)
            {
                dolg1 = dolg.Text.Replace(" ", ""); ;
            }
            if (dolg11.Text != dolg11.Watermark)
            {
                dolg1_1 = dolg11.Text.Replace(" ", ""); ;
            }
            if (dolgA.Text != dolgA.Watermark)
            {
                dolgA1 = dolgA.Text.Replace(" ", ""); ;
            }
            if (dolgB.Text != dolgB.Watermark)
            {
                dolgB1 = dolgB.Text.Replace(" ", ""); ;
            }
            if (shirot.Text != shirot.Watermark)
            {
                shirot1 = shirot.Text.Replace(" ", ""); ;
            }
            if (shirot11.Text != shirot11.Watermark)
            {
                shirot1_1 = shirot11.Text.Replace(" ", ""); ;
            }
            if (shirotA.Text != shirotA.Watermark)
            {
                shirotA1 = shirotA.Text.Replace(" ", ""); ;
            }
            if (shirotB.Text != shirotB.Watermark)
            {
                shirotB1 = shirotB.Text.Replace(" ", ""); ;
            }
            if (rbr.IsChecked == true)
            {
                tbd1 = "-" + tbd1;
            }

            if (radExpander.IsExpanded)
            {
                //передаем координаты 1
                kmOrCoor = "1";
            }
            else
            {
                //километраж 2
                kmOrCoor = "2";
            }
           
            double.TryParse(keyMg, out keyMgDouble);
            double.TryParse(keyNit, out keyPipeDouble);

        }

        

    }
}
