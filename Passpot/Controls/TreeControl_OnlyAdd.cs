using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Passpot.Controls
{
    public partial class TreeControl_Only : UserControl
    {
        private FontWeight GetFontWeights(string s)
        {
            FontWeight fw = FontWeights.Normal;
            switch (s)
            {
                case "b":
                    fw = FontWeights.Bold;
                    break;
                case "n":
                    fw = FontWeights.Normal;
                    break;
                case "t":
                    fw = FontWeights.Thin;
                    break;
            }
            return fw;
        }

        private FontStyle GetFontStyle(string s)
        {
            FontStyle fs = FontStyles.Normal;
            switch (s)
            {
                case "n":
                    fs = FontStyles.Normal;
                    break;
                case "i":
                    fs = FontStyles.Italic;
                    break;

            }
            return fs;
        }

        private SolidColorBrush GetSolidColorBrush(string[] arr)
        {
            SolidColorBrush brush = new SolidColorBrush(
                        Color.FromArgb(
                            255,
                            Byte.Parse("0"),
                            Byte.Parse("0"),
                            Byte.Parse("0"))
                        );


            brush = new SolidColorBrush(
                                   Color.FromArgb(
                                       255,
                                       Byte.Parse(arr[0]),
                                       Byte.Parse(arr[1]),
                                       Byte.Parse(arr[2]))
                                   );
            return brush;
        }

        private string ExtractKey(string fullKey)
        {
            string nvalue1 = "";
            string nvalue2 = "";
            string nvalue0 = "";

            double ii = fullKey.IndexOf("~");
            string[] ArrayKeys = fullKey.Split('~');
            if (ii != -1)
            {
                nvalue0 = ArrayKeys[0];
                nvalue1 = ArrayKeys[1];
                nvalue2 = ArrayKeys[2];
            }

            string keyList = "";


            if (nvalue0 == "100")
            {
                keyList = nvalue2;
            }
            else if (nvalue0 == "101")
            {
                keyList = nvalue1;
            }
            else
            {
                //Debugger.Break();
                keyList = "";
            }
            return keyList;
        }
    }
}
