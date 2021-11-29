using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Resources;
using System.Xml.Linq;
using Media.Interfaces;
using Passpot.Business;
using System.Globalization;
using Passpot.Model;
using Services.ServiceReference;
using Telerik.Windows.Controls;

namespace Passpot
{
    public partial class App : Application
    {

        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;
           
          

         //   Telerik.Windows.Controls.StyleManager.ApplicationTheme = new Telerik.Windows.Controls.Office_BlueTheme();

            InitializeComponent();
        }

        public static string Context;
        private ServiceDataClient _serviceDataClient;

        protected ServiceDataClient ServiceDataClient
        {
            get
            {
                if (_serviceDataClient == null)
                    _serviceDataClient = new ServiceDataClient();
                return _serviceDataClient;
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                string cinfo = "ru-Ru";
                Dictionary<string, string> getparams = e.InitParams as Dictionary<string, string>;

                foreach (KeyValuePair<string, string> pair in getparams)
                {
                    if (pair.Key == "lang")
                    {
                        cinfo = pair.Value;
                        if (cinfo == "en-US")
                        {
                            cinfo = "en";
                        }
                        else
                        {
                            if (cinfo == "ru-RU")
                            {
                                cinfo = "ru-Ru";
                            }
                        }
                       
                    }
                }

                CultureInfo ci = new CultureInfo(cinfo);
                Thread.CurrentThread.CurrentCulture = ci;
                Thread.CurrentThread.CurrentUICulture = ci;
            }
            catch (Exception)
            {
                
                throw;
            }
           

           

           //http://msdn.microsoft.com/ru-RU/library/dd894488(v=vs.95)
           // App.Current.Resources.Add("ColumnString", Passpot.ProjectResources.ResourceManager..Resource1.columnHeader);

           //   this.Resources.Add("fontFamily", SilverlightApp.Resource1.FontFamily);

            //System.Globalization.CultureInfo newCulture = (CultureInfo) System.Threading.Thread.CurrentThread.CurrentCulture.Clone(); 
            ////new System.Globalization.CultureInfo("ru-RU");

            //newCulture.DateTimeFormat.FullDateTimePattern = "dd.mm.yyyy";


            //System.Threading.Thread.CurrentThread.CurrentCulture = newCulture; 

            //создаем MainView (главная форма приложения)
            
            //создаем модель для этого View
            MainModel model = new MainModel();
            model.Report("App - Модель создана, Ver = от 21.07.2016 баг с деревом");

            string v = "";

            StreamResourceInfo info = Application.GetResourceStream(new Uri("version.txt", UriKind.Relative));
            if (info == null)
            {
                model.Version = "файл версии не прочитался";
            }
            else
            {
                StreamReader reader = new StreamReader(info.Stream);

                string line = reader.ReadLine();
                while (line != null)
                {
                    v += line;
                    line = reader.ReadLine();
                }

                reader.Close();
            }

            model.Version = v;
            model.Report("Версия сборки: "+v);

            if (e.InitParams != null)
            {
                foreach (var item in e.InitParams)
                {
                    this.Resources.Add(item.Key, item.Value);

                    if (item.Key == "context")
                    {
                        GlobalContext.Context = item.Value;
                    }

                }
            }


            //отдаем модель View, назначив свойство DataContext 
            //ложим модель в датаконтекст вью
            var mainPage = new MainView();
            mainPage.DataContext = model;
            model.Report("App - передем Model в MainView");
            this.RootVisual = mainPage;
            
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }
        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }
    }
}


         
            ////System.Globalization.CultureInfo newCulture = (CultureInfo) System.Threading.Thread.CurrentThread.CurrentCulture.Clone(); 
            //////new System.Globalization.CultureInfo("ru-RU");

            ////newCulture.DateTimeFormat.FullDateTimePattern = "dd.mm.yyyy";
            

            ////System.Threading.Thread.CurrentThread.CurrentCulture = newCulture; 


            ////создаем MainView (главная форма приложения)
            //var mainPage = new MainView();
            ////создаем модель для этого View
            //MainModel model = new MainModel();

            //if (e.InitParams != null)
            //{
            //    foreach (var item in e.InitParams)
            //    {
            //        this.Resources.Add(item.Key, item.Value);
            //        //if (item.Key == "context")
            //        //{
            //        //    model.NameContext = item.Value;    
            //        //}
                    
            //    }




            //}
            //this.RootVisual = new Page();


            ////отдаем модель View, назначив свойство DataContext 
            ////ложим модель в датаконтекст вью
            //mainPage.DataContext = model;
            //this.RootVisual = mainPage;
