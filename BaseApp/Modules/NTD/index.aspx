<%@ Page Title="" Language="C#" MasterPageFile="~/System/TopMasterPage.master" AutoEventWireup="true"
    CodeFile="index.aspx.cs" Inherits="NTD" %>    
<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls"
    TagPrefix="asp" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Нормативно-техническая документация</title>
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
             l_value = getIDApp("<%=NTDXAP.ClientID %>");
             SetCtl(l_value); 
             return true;
         }

         function getIDApp(appName) {
             return document.getElementById(appName);
         }
		 
		 function getObjKey(fname) {
             var box = document.getElementById("ctl00_ctl00_ContentPlaceHolder_ContentPlaceHolder_HtmlObjKey");
             box.value = ObjKey;
         }
          
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="Server">
    <div style="height:100%; width:100%; font-size:0px;">
        <input type="hidden" name="ctl00_ctl00_ContentPlaceHolder_ContentPlaceHolder_HtmlObjKey" id="ctl00_ctl00_ContentPlaceHolder_ContentPlaceHolder_HtmlObjKey" value="-1" />
        <asp:Silverlight ID="NTDXAP" runat="server" Source="ClientBin/Passpot.xap" InitParameters="context = 30"  Wmode="opaque"
                        MinimumVersion="2.0.31005.0" width="100%" height="100%" />
    </div
</asp:Content>