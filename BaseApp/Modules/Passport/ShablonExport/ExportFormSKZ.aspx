<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExportFormSKZ.aspx.cs" Inherits="ExportFormSKZ" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Экспорт СКЗ в формат MS Word</title>
    <meta http-equiv="Content-Type" content="text/html;" charset="UTF-8">
    <style type="text/css">
        body
        {
            font-family: Times New Roman;
            font-size: 14px;
        }
        .tdborderbottom
        {
            border-bottom: 1px solid #000000;
            padding-left: 20px;
        }
        .textsizelarger
        {
            font-size: 18px;
        }
        .textsizesmaller
        {
            font-size: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="600" border="0">
            <tr>
                <td align="center" style="height: 60px;" valign="top">
                    <asp:Label ID="Label2" runat="server" Text="Паспорт" Style="font-size: 20px; font-weight: bold;"></asp:Label>
                </td>
            </tr>





             <tr>
                <td style="padding-top: 20px;">
                    <asp:Button ID="btnLoadIntoWord" runat="server" Text="Выгрузить в Word" OnClick="btnLoadIntoWord_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
