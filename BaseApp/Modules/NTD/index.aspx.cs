using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using App_Code.Passport_API;

public partial class NTD : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    { 
        SetModuleName("NTD");
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
}