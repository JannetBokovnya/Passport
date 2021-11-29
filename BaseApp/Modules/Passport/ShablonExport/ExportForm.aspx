<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExportForm.aspx.cs" Inherits="ExportForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Экспорт в формат MS Word</title>
    <meta http-equiv="Content-Type" content="text/html;" charset="UTF-8">
    
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table width="400" border="1">
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Идентификатор Паспорта: " />
            </td>
            <td>
                <asp:Label ID="Label2" runat="server" Text="" />
            </td>
            <td>
                <asp:Label ID="Label3" runat="server" Text="" />
            </td>
        </tr>
        <tr>
            <td colspan="3" style="padding-top:20px;">
                <asp:Button ID="btnLoadIntoWord" runat="server" Text="Выгрузить в Word" 
                    onclick="btnLoadIntoWord_Click" />
            </td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
