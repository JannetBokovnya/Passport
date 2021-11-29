using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using App_Code.Passport_API;

public partial class Passport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (HttpContext.Current.Session["lang"] != null)
                id.Text = HttpContext.Current.Session["lang"].ToString();
            //PassportXAP.InitParameters = HttpContext.Current.Session["lang"].ToString();
            SetModuleName("PASSPORT");
        }
       
    }

    

    public void SetModuleName(string p_cModuleName)
    { 
        System_TopMasterPage master = (System_TopMasterPage)this.Master;
        master.SetModuleName(p_cModuleName);
    }

    public void moduleEvent(string moduleEvents, string p_cModuleName, string p_cValue)
    {
        throw new NotImplementedException();
    }

    //Перегружаем "Культуру" для данной страницы (этот метод вызвается самым первым, раньше всех других)
    protected override void InitializeCulture()
    {
        //Выставляем "Культуру" для данной страницы, в зависимости выбранного ранее пользователем (берем из сессионной переменной Session["lang"])
        if (HttpContext.Current.Session["lang"] != null)
        {
            String selectedLanguage = HttpContext.Current.Session["lang"].ToString();
            UICulture = selectedLanguage;
            Culture = selectedLanguage;

            Thread.CurrentThread.CurrentCulture =
                CultureInfo.CreateSpecificCulture(selectedLanguage);
            Thread.CurrentThread.CurrentUICulture = new
                CultureInfo(selectedLanguage);

        }
        base.InitializeCulture();
    }
}