<%@ Page Title="" Language="C#" MasterPageFile="~/System/TopMasterPage.master" AutoEventWireup="true"
    CodeFile="index.aspx.cs" Inherits="NSI_editor" culture="auto" meta:resourcekey="PageResource2" uiculture="auto" %>    
<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls"
    TagPrefix="asp" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Редактор НСИ</title>
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
             l_value = getIDApp("NSIEditorXAP");
             SetCtl(l_value);
             return true;
         }

         function getIDApp(appName) {
             return document.getElementById(appName).Content.Page;
         }



     </script>
    

  <%--   <script type="text/javascript">
       
         function applicationLoaded() {
             l_value = getIDApp("<%=NSIEditorXAP.ClientID %>");
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
<%--<asp:Content ID="Content" ContentPlaceHolderID="Body" runat="Server">
    <div style="height:100%; width:100%; font-size:0px;">
        <asp:Silverlight ID="NSIEditorXAP" runat="server" Source="ClientBin/Passpot.xap" InitParameters="context = 20"  Windowless="True"  Wmode="opaque"
                                MinimumVersion="2.0.31005.0" width="100%" height="100%" meta:resourcekey="NSIEditorXAPResource2" />
    </div>--%>

    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="Server">
    <div style="height:100%; width:100%; font-size:0px;">
        <input type="hidden"  id="ctl00_ctl00_ContentPlaceHolder_ContentPlaceHolder_HtmlObjKey" value="-1" style="font-size: 0px;" />
        <object id="NSIEditorXAP" data="data:application/x-silverlight-2," type="application/x-silverlight-2" width="100%" height="100%">
            <param name="source" value="ClientBin/Passpot.xap"/>
            <param name="initParams"  value="lang= <asp:Literal id="id" runat="server"/>, context = 30, wmode = opaque" />
            <param name="windowless" value="true" />
        </object> 
    </div>
</asp:Content>