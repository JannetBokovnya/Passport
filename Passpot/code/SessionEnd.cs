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
using Passpot.Controls;

namespace Passpot.code
{
    public class SessionEnd
    {
        static public void Redirect()
        {
            Passpot.Controls.ChildWindowAttantion _popupWindowAttantion = new ChildWindowAttantion();
            _popupWindowAttantion.titleBox.Text = "Сессия завнершена. Авторизируйтесь повторно.";
            _popupWindowAttantion.Show();
            _popupWindowAttantion.OKButton.Click += new RoutedEventHandler(OKButton_Click);
        }

        static void OKButton_Click(object sender, RoutedEventArgs e)
        {
            string strVal = string.Empty;
            System.Windows.Browser.HtmlPage.Document.QueryString.TryGetValue("flagurl", out strVal);

            if (strVal == null)
            {
                System.Windows.Browser.HtmlPage.Window.Eval("window.location = '" + System.Windows.Browser.HtmlPage.Document.DocumentUri.AbsolutePath + "';");
            }
            else
            {
                System.Windows.Browser.HtmlPage.Window.Eval("window.location = 'login.aspx';");
            }
        }
    }

}
