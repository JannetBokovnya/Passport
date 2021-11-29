<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExportFromUKZ.aspx.cs" Inherits="Modules_Passport_ShablonExport_ExportFromUKZ" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Экспорт УКЗ в формат MS Word</title>
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

        .auto-style24 {
            height: 20px;
        }

        .auto-style27 {
            width: 12%;
            height: 20px;
        }
        .auto-style28 {
            width: 264px;
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
                        <asp:Label ID="Label4" runat="server" Text=" ПАСПОРТ" CssClass="text22bold"
                            Style="letter-spacing: 5px;" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="Label5" runat="server" Text="УСТАНОВКИ КАТОДНОЙ ЗАЩИТЫ" CssClass="text18" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="Label12" runat="server" Text="№ " CssClass="text16bold" />
                        <asp:Label ID="Label13" runat="server" Text="&nbsp &nbsp" />
                        <asp:Label ID="lblNumber" runat="server" Text="" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="Label7" runat="server" Text="ЗАЩИЩАЕМЫЙ ГАЗОПРОВОД " CssClass="text18" />
                    </td>
                    </tr>

                <tr>
                    <td style="height: 440px;">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="height: 50px;" valign="middle" align="center">
                        <asp:Label ID="Label9" runat="server" Text="ОБЩАЯ&nbsp;ЧАСТЬ" CssClass="text16bold" />
                    </td>
                </tr>
               
                <tr>
                    <td>
                        <table width="100%" border="0">
                            <tr>
                                <td class="auto-style5">&nbsp;
                                </td>
                                <td width="160" valign="top" class="auto-style9">1.&nbsp;Номер УКЗ</td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblNumberUKZ" runat="server" Text="" />
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
                                <td width="160" valign="top" class="auto-style9">2.&nbsp;Место установки (Название МГ)</td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblPlaceState" runat="server" Text="" />
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
                                <td width="160" valign="top" class="auto-style9">3.&nbsp;Эксплуатационный километраж (км)</td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblEkcplKm" runat="server" Text="" />
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
                                <td width="160" valign="top" class="auto-style9">4.&nbsp;Диаметр защищаемого трубопровода (м)</td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblDiamTr" runat="server" Text="" />
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
                                <td valign="top" class="auto-style9">5.&nbsp;Изоляция защищаемого трубопровода</td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblIzolTp" runat="server" Text="" />
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
                                <td width="160" valign="top" class="auto-style9">6.&nbsp;Проектная организация
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblproektOrg" runat="server" Text="" />
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
                                <td valign="top" class="auto-style9">7.&nbsp;Дата&nbsp;монтажа
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblDataMont" runat="server" Text="" />
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
                                <td valign="top" class="auto-style9">8.&nbsp;Дата ввода в эксплуатацию</td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblDataEkcpl" runat="server" Text="" />
                                </td>

                            </tr>
                        </table>
                    </td>
                </tr>
                
               
                <tr>
                    <td style="height: 50px;" valign="middle" align="center">
                        <asp:Label ID="Label10" runat="server" Text="КАТОДНАЯ&nbsp;СТАНЦИЯ"
                            CssClass="text16bold" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="0">
                            <tr>
                                <td class="auto-style5">&nbsp;
                                </td>
                                <td class="auto-style9">1.&nbsp;Тип СКЗ
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td class="auto-style24">
                                    <asp:Label ID="lblTypeSkz" runat="server" Text="" />
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
                                <td class="auto-style9">2.&nbsp;Год изготовления
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblYearProduction" runat="server" Text="" />
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
                                <td class="auto-style9">3.&nbsp;Заводской номер  
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
                                <td class="auto-style9">4.&nbsp;Примечание
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblPrim1" runat="server" Text="" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
               
                <tr>
                    <td style="height: 50px;" valign="middle" align="center">
                        <asp:Label ID="Label11" runat="server" Text="ЦЕПЬ КАТОДНОЙ ЗАЩИТЫ"
                            CssClass="text16bold" />
                    </td>
                </tr>
                <tr>
                    <td style="height: 30px;" valign="TOP" align="center">
                        <asp:Label ID="Label14" runat="server" Text="(ДРЕНАЖНАЯ КАТОДНАЯ ЛИНИЯ)"
                            CssClass="text16bold" />
                    </td>
                </tr>
               
                <tr>
                    <td>
                        <table width="100%" border="0">
                            <tr>
                                <td class="auto-style5">&nbsp;
                                </td>
                                <td class="auto-style9">1.&nbsp;Вид прохождения электролинии
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblVidProhElectr" runat="server" Text="" />
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
                                <td class="auto-style9">2.&nbsp;Марка провода/кабеля 
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblMarkaKab" runat="server" Text="" />
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
                                <td class="auto-style9">3.&nbsp;Сечение провода/кабеля
                                    (мм)</td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblSechPr" runat="server" Text="" />
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
                                <td class="auto-style9">4.&nbsp;Длина (м)
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblDlina" runat="server" Text="" />
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
                                <td class="auto-style9">5.&nbsp;Сопротивление
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblCoprotiv" runat="server" Text="" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                 <tr>
                    <td style="height: 50px;" valign="middle" align="center">
                        <asp:Label ID="Label15" runat="server" Text="АНОДНОЕ ЗАЗЕМЛЕНИЕ"
                            CssClass="text16bold" />
                    </td>
                </tr>
                 <tr>
                    <td>
                        <table width="100%" border="0">
                            <tr>
                                <td class="auto-style5">&nbsp;
                                </td>
                                <td class="auto-style9">1.&nbsp;Материал
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblMaterial" runat="server" Text="" />
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
                                <td class="auto-style9">2.&nbsp;Тип анодного заземления
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblTypeAnodZ" runat="server" Text="" />
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
                                <td class="auto-style9">3.&nbsp;Количество электродов (шт.)
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblNumberElectr" runat="server" Text="" />
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
                                <td class="auto-style9">4.&nbsp;Длина анодной конструкции (м)
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblLenghtAnod" runat="server" Text="" />
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
                                <td class="auto-style9">5.&nbsp;Расстояние между электродами (м)
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblDistance" runat="server" Text="" />
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
                                <td class="auto-style9">6.&nbsp;Конструктивная особенность
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblKonstrukt" runat="server" Text="" />
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
                                <td class="auto-style9">7.&nbsp;Тип активатора
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblTypeAktiv" runat="server" Text="" />
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
                                <td class="auto-style9">8.&nbsp;Удельное сопротивление грунта в месте
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblSoprotiv" runat="server" Text="" />
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
                                <td class="auto-style9">&nbsp;&nbsp;&nbsp;расположения АЗ(Ом*м)
                                </td>
                                <td class="auto-style15">&nbsp;
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
                                <td class="auto-style9">9.&nbsp;Дата измерения
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblDateIzmA" runat="server" Text="" />
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
                                <td class="auto-style9">10.&nbsp;Сопротивление растеканию АЗ(Ом)
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblSoprotRast" runat="server" Text="" />
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
                                <td class="auto-style9">11.&nbsp;Дата измерения сопротивления АЗ
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblDataIzmAZ" runat="server" Text="" />
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
                                <td class="auto-style9">12.&nbsp;Глубина
                                </td>
                                <td class="auto-style15">&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lblDepth" runat="server" Text="" />
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
