using System;
using System.Globalization;
using System.Threading;
using System.Web;
//using App_Code.Passport_API;

public partial class NSI_editor : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    { 
        SetModuleName("DBEDITOR");
        if (HttpContext.Current.Session["lang"] != null)
            id.Text = HttpContext.Current.Session["lang"].ToString();
    }

    public void SetModuleName(string p_cModuleName)
    {
        System_TopMasterPage master = (System_TopMasterPage)this.Master;
        master.SetModuleName(p_cModuleName);
    }


    //Перегружаем "Культуру" для данной страницы (этот метод вызвается самым первым, раньше всех других)
    protected override void InitializeCulture()
    {
        if (Session["lang"] != null)//Если выполнен переход на страницу Логина (нажата кнопка "Выйти")
        {
            String selectedLanguage = Session["lang"].ToString();
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