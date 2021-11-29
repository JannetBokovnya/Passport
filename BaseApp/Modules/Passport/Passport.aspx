<%@ Page Title="" Language="C#" MasterPageFile="~/System/TopMasterPage.master" AutoEventWireup="true"
    CodeFile="Passport.aspx.cs" Inherits="Passport" culture="auto" uiculture="auto" meta:resourcekey="PageResource1"  %> 
    
<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls"
    TagPrefix="asp" %>     

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Паспорт</title>
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
             l_value = getIDApp("PassportXAP");
             SetCtl(l_value);
             //тест для событий
             //receiveAdapter("SHOW_PASSPORT", "1809077302");
             return true;
         }

         function getIDApp(appName) {
             return document.getElementById(appName).Content.Page;
         }
         
          

     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="Server">
    <div style="height:100%; width:100%; font-size:0px;">
        <input type="hidden"  id="ctl00_ctl00_ContentPlaceHolder_ContentPlaceHolder_HtmlObjKey" value="-1" style="font-size: 0px;" />

        <%--<asp:Silverlight ID="PassportXAP" runat="server" Source="ClientBin/Passpot.xap" Windowless="true"
                                MinimumVersion="2.0.31005.0" W width="100%" height="100%" Wmode="opaque" style="font-size: 0px;" /> --%>
       
        <object id="PassportXAP" data="data:application/x-silverlight-2," type="application/x-silverlight-2" width="100%" height="100%">
            <param name="source" value="ClientBin/Passpot.xap"/>
            <param name="initParams"  value="lang= <asp:Literal id="id" runat="server"/>, context = 10, wmode = opaque" />
            <param name="windowless" value="true" />
        </object> 
    </div>
</asp:Content>


<%--http://msdn.microsoft.com/en-us/library/cc838156(v=vs.95).aspx
http://www.telerik.com/forums/background-image-in-drag-drop-area

param name="windowless" value="false" - ставим false, для окна с Drag and Drop телериковской компоненты (перетаскиваем медиаматериалы)--%>