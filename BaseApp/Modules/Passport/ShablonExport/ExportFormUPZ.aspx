<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExportFormUPZ.aspx.cs" Inherits="ExportForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Экспорт УПЗ в формат MS Word</title>
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
            <%--<tr>
                <td style="height: 40px;">
                    &nbsp;
                </td>
            </tr>--%>
            <tr>
                <td>
                    <table width="100%" border="0">
                        <tr>
                            <td width="290">
                                <asp:Label ID="Label1" runat="server" Text="Установка&nbsp;протекторной&nbsp;защиты&nbsp;№:"
                                    CssClass="textsizelarger"></asp:Label>
                            </td>
                            <td class="tdborderbottom">
                                <asp:Label ID="lblUpzNumber" runat="server" Text="" CssClass="textsizelarger" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" border="0">
                        <tr>
                            <td width="70%">
                                &nbsp;
                            </td>
                            <td class="tdborderbottom" align="center">
                                <asp:Label ID="lblDayEndMonth" runat="server" Text="" />
                            </td>
                           <%-- <td width="15">
                                20
                            </td>
                            <td class="tdborderbottom" width="20" style="padding-left: 0px;" align="center">
                                <asp:Label ID="lblbYear" runat="server" Text="" />
                            </td>
                            <td width="10">
                                г.
                            </td>--%>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 60px;" valign="bottom">
                    <table width="100%" border="0">
                        <tr>
                            <td width="110">
                                Место&nbsp;установки:
                            </td>
                            <td class="tdborderbottom">
                                <asp:Label ID="lblPlaceOfImplementation" runat="server" Text="" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" border="0">
                        <tr>
                            <td width="180">
                                Дата&nbsp;ввода&nbsp;в&nbsp;эксплуатацию:
                            </td>
                            <td class="tdborderbottom">
                                <asp:Label ID="lvlDateOfEnterInExpluatation" runat="server" Text="" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" border="0">
                        <tr>
                            <td width="80">
                                Газопровод:
                            </td>
                            <td class="tdborderbottom">
                                <asp:Label ID="lblMgName" runat="server" Text="" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" border="0">
                        <tr>
                            <td width="90">
                                Зона&nbsp;защиты:
                            </td>
                            <td class="tdborderbottom" width="80">
                                <asp:Label ID="lblZoneOfProtection" runat="server" Text="" />
                            </td>
                            <td width="15">
                                км.
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" border="0">
                        <tr>
                            <td width="150">
                                Проектная&nbsp;организация:
                            </td>
                            <td class="tdborderbottom">
                                <asp:Label ID="lblProjectOrganization" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td valign="top" align="center">
                                <asp:Label ID="Label3" runat="server" Text="Диаметр, тип изоляции, введен в эксплуатацию, дата"
                                    CssClass="textsizesmaller" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" border="0">
                        <tr>
                            <td width="180">
                                Число&nbsp;протекторов&nbsp;в&nbsp;группе:
                            </td>
                            <td class="tdborderbottom">
                                <asp:Label ID="lblCountOfProtectorsInGroup" runat="server" Text="" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" border="0">
                        <tr>
                            <td width="270">
                                Сечение&nbsp;и&nbsp;марка&nbsp;соединительных&nbsp;проводов:
                            </td>
                            <td class="tdborderbottom">
                                <asp:Label ID="lblSechAndMarkaSoedProvodov" runat="server" Text="" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" border="0">
                        <tr>
                            <td width="280">
                                Расстояние&nbsp;от&nbsp;протекторов&nbsp;до&nbsp;сооружения,&nbsp;м:
                            </td>
                            <td class="tdborderbottom">
                                <asp:Label ID="lblDistanceFromProtectorsToBuiding" runat="server" Text="" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" border="0">
                        <tr>
                            <td width="220">
                                Расстояние&nbsp;между&nbsp;протекторами,&nbsp;м:
                            </td>
                            <td class="tdborderbottom">
                                <asp:Label ID="lblDistanceBetweenProtectors" runat="server" Text="" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" border="0">
                        <tr>
                            <td width="220">
                                Глубина&nbsp;заложения&nbsp;протекторов,&nbsp;м:
                            </td>
                            <td class="tdborderbottom">
                                <asp:Label ID="lblDeepOfProtectors" runat="server" Text="" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="padding-left: 30px; height: 80px;">
                    ПАРАМЕТРЫ&nbsp;протекторной&nbsp;устанвки
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" border="0">
                        <tr>
                            <td width="280">
                                Споротивление&nbsp;цепи&nbsp;протектор-газопровод
                            </td>
                            <td class="tdborderbottom">
                                <asp:Label ID="lblThreadSoprotivleniye" runat="server" Text="" />
                            </td>
                            <td width="70">
                                Ом.&nbsp;Ток
                            </td>
                            <td class="tdborderbottom">
                                <asp:Label ID="lblThreadTok" runat="server" Text="" />
                            </td>
                            <td width="10">
                                А.
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" border="0">
                        <tr>
                            <td width="270">
                                Разность&nbsp;потенциалов&nbsp;газопровод-земля,&nbsp;В:
                            </td>
                            <td class="tdborderbottom">
                                <asp:Label ID="lblRaznostPotenstialov" runat="server" Text="" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" border="0">
                        <tr>
                            <td width="380">
                                Удельное&nbsp;сопротивление&nbsp;грунта&nbsp;в&nbsp;зоне&nbsp;установки&nbsp;протектора:
                            </td>
                            <td class="tdborderbottom">
                                <asp:Label ID="lblUdelnoyeSoprotivlenye" runat="server" Text="" />
                            </td>
                            <td width="10">
                                Ом.
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 50px;" valign="bottom">
                    <table width="100%" border="0">
                        <tr>
                            <td width="90" style="padding-left: 20px;" valign="top">
                                Примечание:
                            </td>
                            <td style="padding-left: 20px;">
                                <asp:Label ID="lblComment" runat="server" Text="" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 200px;" valign="bottom">
                    <table width="100%" border="0">
                        <tr>
                            <td width="70">
                                Составил:
                            </td>
                            <td class="tdborderbottom">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="tdborderbottom">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td valign="bottom" style="height: 90px;">
                    <table width="100%" border="0">
                        <tr>
                            <td width="180">
                                Учет&nbsp;работ,&nbsp;производимых&nbsp;на
                            </td>
                            <td class="tdborderbottom">
                                <asp:Label ID="lblNazvaniyeUchastka" runat="server" Text="" />
                            </td>
                            <td width="10">
                                №
                            </td>
                            <td class="tdborderbottom" style="padding-left:0px;" align="center" width="30">
                                <asp:Label ID="lblNomerUchastka" runat="server" Text="" />
                            </td>
                            <td width="140">
                                во&nbsp;время&nbsp;эксплуатации
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height:5px;">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="grvWorks" runat="server" AutoGenerateColumns="False" Width="100%">
                        <Columns>
                            <asp:BoundField HeaderText="№ п/п" DataField="WORK_NUMBER">
                            <ItemStyle Width="30px" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Дата" DataField="WORK_DATE">
                            <ItemStyle Width="100px" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Описание проведенных работ" 
                                DataField="WORK_DESCRIPTION" >
                            <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="WORK_ORGANIZATION" 
                                HeaderText="Кем выполнялись работы (организация, исполнитель)">
                            <ItemStyle Width="100px" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="WORK_SIGHN" 
                                HeaderText="Подпись лица, ответственного за исполнение">
                            <ItemStyle Width="100px" HorizontalAlign="Center" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
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
