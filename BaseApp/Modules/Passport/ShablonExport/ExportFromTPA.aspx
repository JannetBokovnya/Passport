<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExportFromTPA.aspx.cs" Inherits="Modules_Passport_ShablonExport_ExportFromTPA" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Экспорт ТПА в формат MS Word</title>
    <meta http-equiv="Content-Type" content="text/html;" charset="UTF-8">
     <style type="text/css">
        body {
            font-family: Times New Roman;
            font-size: 14px;
        }

        .tdborderbottom {
            border-bottom: 1px solid #000000;
            padding-left: 20px;
        }

        .text16bold {
            font-size: 16px;
            font-weight: bold;
        }

        .text16 {
            font-size: 16px;
        }

        .text20bold {
            font-size: 20px;
            font-weight: bold;
        }

        .text22bold {
            font-size: 22px;
            font-weight: bold;
        }

        .textsizesmaller {
            font-size: 10px;
        }

        .auto-style1 {
            width: 8%;
        }

        .auto-style5 {
            width: 12%;
        }

        .auto-style9 {
            width: 264px;
        }

        .auto-style15 {
            width: 2%;
        }

        .auto-style16 {
            width: 4%;
        }

        .auto-style20 {
            width: 311px;
        }
        .auto-style21 {
            height: 50px;
            width: 334px;
        }
         .auto-style22 {
             width: 2%;
             height: 20px;
         }
         .auto-style24 {
             height: 20px;
         }
         .auto-style25 {
             width: 323px;
             height: 20px;
         }
         .auto-style26 {
             width: 324px;
             height: 20px;
         }
    </style>
</head>
<body>
     <form id="form1" runat="server">
        <div>
            <table width="600" border="0">
                <tr>
                    <td align="center" style="height: 40px;" valign="bottom">
                        <asp:Label ID="Label1" runat="server" Text="ТОО &laquo;Азиатский Газопровод&raquo;"
                            CssClass="text16bold" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <table width="100%" border="0">
                            <tr>

                                <td class="auto-style1" align="center">
                                    <asp:Label ID="Label3" runat="server" Text="РЭУ" CssClass="text16bold" />
                                     <asp:Label ID="Label8" runat="server" Text=" &nbsp &nbsp &nbsp  " CssClass="text16bold" />
                                    <asp:Label ID="Label6" runat="server" Text="Tapas" CssClass="text16bold" />
                                   <%-- <asp:Label ID="lblLpuMgName" runat="server" Text="" />--%>
                                </td>



                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center" style="height: 350px;" valign="bottom">
                        <asp:Label ID="Label4" runat="server" Text="ЭКСПЛУАТАЦИОННЫЙ ПАСПОРТ" CssClass="text22bold"
                            Style="letter-spacing: 5px;" />
                    </td>
                </tr>

                <tr>
                    <td align="center">
                        <asp:Label ID="Label5" runat="server" Text="НА УСТАНОВЛЕННУЮ ТРУБОПРОВОДНУЮ АРМАТУРУ" CssClass="text18" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="0">
                            <tr>

                                <td align="center">
                                    <asp:Label ID="Label7" runat="server" Text="Технологический номер " CssClass="text16bold" />
                                    <asp:Label ID="Label11" runat="server" Text="&nbsp &nbsp" />
                                    <asp:Label ID="lblFactoryNuber" runat="server" Text="" />
                                </td>
                                <td></td>

                            </tr>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td style="height: 450px;">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="height: 50px;" valign="middle">
                        <asp:Label ID="Label9" runat="server" Text="I.&nbsp;Общая&nbsp;характеристика" CssClass="text16bold" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="0">
                            <tr>
                                <td class="auto-style5">&nbsp;
                                </td>
                                <td width="160" valign="top" class="auto-style9">1.&nbsp;Тип&nbsp;трубопроводной&nbsp;арматуры
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblZraType" runat="server" Text="" />
                                </td>
                            </tr>

                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="0">
                            <tr>
                                <td class="auto-style5">&nbsp;
                                </td>
                                <td width="160" valign="top" class="auto-style9">2.&nbsp;Способ&nbsp;присоединения&nbsp;
                                к</td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblSposobPrisoedineniya" runat="server" Text="" />
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style5">&nbsp;
                                </td>
                                <td valign="top" class="auto-style9">&nbsp&nbsp&nbsp&nbsp;трубопроводу
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="0">
                            <tr>
                                <td class="auto-style5">&nbsp;
                                </td>
                                <td width="160" valign="top" class="auto-style9">3.&nbsp;Род&nbsp;запорного&nbsp;органа
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblZapornOrganType" runat="server" Text="" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="0">
                            <tr>
                                <td class="auto-style5">&nbsp;
                                </td>
                                <td width="160" valign="top" class="auto-style9">4.&nbsp;Стандарт изготовления ТПА
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblGostZra" runat="server" Text="" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="0">
                            <tr>
                                <td class="auto-style5">&nbsp;
                                </td>
                                <td valign="top" class="auto-style9">5.&nbsp;Завод-изготовитель
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblFactoryVender" runat="server" Text="" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="0">
                            <tr>
                                <td class="auto-style5">&nbsp;
                                </td>
                                <td width="160" valign="top" class="auto-style9">6.&nbsp;Страна
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblCountry" runat="server" Text="" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td>
                        <table width="100%" border="0">
                            <tr>
                                <td class="auto-style5">&nbsp;
                                </td>
                                <td valign="top" class="auto-style9">7.&nbsp;Заводской&nbsp;номер
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblFactoryNumber" runat="server" Text="" />
                                </td>

                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="0">
                            <tr>
                                <td class="auto-style5">&nbsp;
                                </td>
                                <td valign="top" class="auto-style9">8.&nbsp;Условный&nbsp;диаметр&nbsp;на&nbsp;выходе(мм)
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblDiamUslovnyi" runat="server" Text="" />
                                </td>

                            </tr>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td>
                        <table width="100%" border="0">
                            <tr>
                                <td class="auto-style5">&nbsp;
                                </td>
                                <td class="auto-style9" valign="top">9.&nbsp;Рабочее&nbsp;давление&nbsp;(МПа)
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblDavlRab" runat="server" Text="" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td style="height: 50px;" valign="middle">
                        <asp:Label ID="Label10" runat="server" Text="II.&nbsp;Дополнительные&nbsp;устройства"
                            CssClass="text16bold" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" height="50" border="0">
                            <tr>
                                <td class="auto-style16">&nbsp;
                                </td>
                                <td>1.&nbsp;Обвязка ТПА
                                </td>
                            </tr>

                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="0">
                            <tr>
                                <td class="auto-style22">&nbsp;
                                </td>
                                <td class="auto-style25">1.1&nbsp;Название ТПА обвязки 
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td class="auto-style24">
                                    <asp:Label ID="lblNameTPA1" runat="server" Text="" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="0">
                            <tr>
                                <td class="auto-style5">&nbsp;
                                </td>
                                <td class="auto-style9">1.1.1&nbsp;Тип ТПА 
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblTypeTPA" runat="server" Text="" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="0">
                            <tr>
                                <td class="auto-style5">&nbsp;
                                </td>
                                <td class="auto-style9">1.1.2&nbsp;Условный диаметр на выходе(мм)  
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblDiam" runat="server" Text="" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="0">
                            <tr>
                                <td class="auto-style5">&nbsp;
                                </td>
                                <td class="auto-style9">1.1.3&nbsp;Назначение ТПА  
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblNAznTPA" runat="server" Text="" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="0">
                            <tr>
                                <td class="auto-style5">&nbsp;
                                </td>
                                <td class="auto-style9">1.1.4&nbsp;Способ управления привода  
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblSposobUpr" runat="server" Text="" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="0">
                            <tr>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td class="auto-style26">1.2&nbsp;Название ТПА обвязки 
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblNameTPA2" runat="server" Text="" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="0">
                            <tr>
                                <td class="auto-style5">&nbsp;
                                </td>
                                <td class="auto-style9">1.2.1&nbsp;Тип ТПА 
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblTypeTPA2" runat="server" Text="" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="0">
                            <tr>
                                <td class="auto-style5">&nbsp;
                                </td>
                                <td class="auto-style9">1.2.2&nbsp;Условный диаметр на выходе(мм)  
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblDiam2" runat="server" Text="" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table  width="100%" border="0">
                            <tr>
                                <td class="auto-style5">&nbsp;
                                </td>
                                <td class="auto-style9">1.2.3&nbsp;Назначение ТПА  
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblNaznTPA2" runat="server" Text="" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="0">
                            <tr>
                                <td class="auto-style5">&nbsp;
                                </td>
                                <td class="auto-style9">1.2.4&nbsp;Способ управления привода  
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblSposobUpr2" runat="server" Text="" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="0">
                            <tr>
                                <td class="auto-style16">&nbsp;
                                </td>
                                <td class="auto-style20">2.&nbsp;Способ управления привода  
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblSposobUprPriv3" runat="server" Text="" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="0">
                            <tr>
                                <td class="auto-style16">&nbsp;
                                </td>
                                <td class="auto-style20">3.&nbsp;Вид управления приводом  
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblVidUprPriv" runat="server" Text="" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="0">
                            <tr>
                                <td class="auto-style16">&nbsp;
                                </td>
                                <td class="auto-style20">4.&nbsp;Примечание  
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblPrim" runat="server" Text="" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table border="0">
                            <td valign="middle" class="auto-style21">
                                <asp:Label ID="Label2" runat="server" Text="III.&nbsp;Место&nbsp;установки"
                                    CssClass="text16bold" />
                            </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblPlaseState" runat="server" Text="" />
                                </td>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table border="0">
                            <tr>
                                <td class="auto-style16">&nbsp;
                                </td>
                                <td class="auto-style20">1.&nbsp;Дата ввода в экусплуатацию  
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lbldateEkcpl" runat="server" Text="" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="padding-top: 20px;">
                        <asp:Button ID="btnLoadIntoWord" runat="server" Text="Выгрузить в Word"
                            OnClick="btnLoadIntoWord_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
