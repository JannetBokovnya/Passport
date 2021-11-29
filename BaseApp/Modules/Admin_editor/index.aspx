<%@ Page Title="" Language="C#" MasterPageFile="~/System/TopMasterPage.master" AutoEventWireup="true"
    CodeFile="index.aspx.cs" Inherits="Admin_editor" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>    
<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls"
    TagPrefix="asp" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Администратор доступа</title>
    <style type="text/css">
        html, body
        {
            height: 100%;
            overflow: auto;
        }
        body
        {
            padding: 0;
            margin: 0;
        }
        #silverlightControlHost
        {
            height: 100%;
            text-align: center;
        }
    </style>

     <script type="text/javascript">
       
         function applicationLoaded() {
             l_value = getIDApp("AdminEditorXAP");
             SetCtl(l_value); 
             return true;
         }

         function getIDApp(appName) {
             return document.getElementById(appName);
         }
		 
		 function getObjKey(fname) {
         //    var box = document.getElementById("ctl00_ctl00_ContentPlaceHolder_ContentPlaceHolder_HtmlObjKey");
         //    box.value = ObjKey;
         }
          
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="Server">
    <div style="height:100%; width:100%; font-size:0px;">
        <%--<asp:Silverlight ID="AdminEditorXAP" runat="server" Source="ClientBin/Passpot.xap" InitParameters="context = 40"  Windowless="True"
                                MinimumVersion="2.0.31005.0" width="100%" height="100%" Wmode="opaque" meta:resourcekey="AdminEditorXAPResource1" />--%>
        <object id="AdminEditorXAP" data="data:application/x-silverlight-2," type="application/x-silverlight-2" width="100%" height="100%">
            <param name="source" value="ClientBin/Passpot.xap"/>
            <param name="initParams"  value="lang= <asp:Literal id="id" runat="server"/>, context = 40, wmode = opaque" />
            <param name="windowless" value="true" />
        </object> 
    </div>
</asp:Content>