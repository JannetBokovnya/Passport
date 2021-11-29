<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExportPasportToXML.aspx.cs"
    Inherits="Modules_Passport_ExportPasportToXML" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Выгрузка Паспорта в формат XML</title>
    <style type="text/css">
        .legendtext
        {
            /*color: #38848e;*/
            color: #5c5c5c;
            font-size: 11px;
            font-weight: bold;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server" style="font-family: Arial; font-size: 12px;">
    <div style="padding-top: 30px; padding-left: 20px;" runat="server" id="divMain" visible="false">
        <fieldset style="width: 335px; height: 43px;">
            <legend class="legendtext">Выбор XML-файла для загрузки в БД</legend>
            
            <table width="300" border="0">
                <tr>
                    <td >
                        <asp:FileUpload ID="FileField" runat="server" />
                    </td>
                    <td style="padding-left: 5px;">
                        <asp:Button ID="bntLoadXmlInDb" runat="server" Text="Загрузить" 
                            OnClick="bntLoadXmlInDb_Click" Height="23px" Width="87px" />
                    </td>
                </tr>
            </table>
        </fieldset>
        <br />
        <asp:Label Style="margin-left: 10px;" ID="lblResultMessage" runat="server" Text=""
            EnableViewState="false"></asp:Label>
        <br />
    </div>
    </form>
</body>
</html>
