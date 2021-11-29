<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExportFormZRA.aspx.cs" Inherits="ExportFormZRA" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Экспорт ЗРА в формат MS Word</title>
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
        .text16bold
        {
            font-size: 16px;
            font-weight: bold;
        }
        .text22bold
        {
            font-size: 22px;
            font-weight: bold;
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
                <td align="center" style="height: 150px;" valign="bottom">
                    <asp:Label ID="Label1" runat="server" Text="Открытое Акционерное Общество &laquo;ГАЗПРОМ&raquo;"
                        CssClass="text16bold" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Label ID="Label2" runat="server" Text="ООО &laquo;СУРГУТГАЗПРОМ&raquo;" CssClass="text16bold" />
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" border="0">
                        <tr>
                            <td style="width: 30%;">
                                &nbsp;
                            </td>
                            <td class="tdborderbottom" style="width: 25%;">
                                <asp:Label ID="lblLpuMgName" runat="server" Text="" />
                            </td>
                            <td style="width: 25%;">
                                <asp:Label ID="Label3" runat="server" Text="ЛПУМГ (МТ)" CssClass="text16bold" />
                            </td>
                            <td style="width: 20%;">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" style="height: 100px;" valign="bottom">
                    <asp:Label ID="Label4" runat="server" Text="Эксплуатационный" CssClass="text16bold"
                        Style="letter-spacing: 5px;" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Label ID="Label6" runat="server" Text="ПАСПОРТ" CssClass="text22bold" Style="letter-spacing: 5px;" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Label ID="Label5" runat="server" Text="на установленную запорную арматуру" CssClass="text16bold" />
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" border="0">
                        <tr>
                            <td width="45%" align="right">
                                <asp:Label ID="Label7" runat="server" Text="Зав. №" CssClass="text16bold" />
                            </td>
                            <td class="tdborderbottom">
                                <asp:Label ID="lblFactoryNuber" runat="server" Text="" />
                            </td>
                            <td width="37%">
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
                            <td width="37%" align="right">
                                Dy
                            </td>
                            <td class="tdborderbottom" style="width: 50px;">
                                <asp:Label ID="lblDy" runat="server" Text="" />
                            </td>
                            <td style="width: 40px;" align="right">
                                Ру
                            </td>
                            <td class="tdborderbottom" style="width: 50px;">
                                <asp:Label ID="lblRu" runat="server" Text="" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 500px;">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="height: 50px;" valign="bottom">
                    <asp:Label ID="Label9" runat="server" Text="I.&nbsp;Общая&nbsp;характеристика" CssClass="text16bold" />
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" border="0">
                        <tr>
                            <td width="160" valign="top">
                                1.&nbsp;Род&nbsp;запорной&nbsp;арматуры
                            </td>
                            <td class="tdborderbottom">
                                <asp:Label ID="lblZraType" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td valign="top" align="center">
                                <asp:Label ID="Label8" runat="server" Text="(полное наименование крана, задвижки, вентиля)"
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
                            <td width="260" valign="top">
                                2.&nbsp;Способ&nbsp;присоединения&nbsp;к&nbsp;трубопроводу
                            </td>
                            <td class="tdborderbottom">
                                <asp:Label ID="lblSposobPrisoedineniya" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td valign="top" align="center">
                                <asp:Label ID="Label11" runat="server" Text="(вварной, фланцевый, ...)" CssClass="textsizesmaller" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" border="0">
                        <tr>
                            <td width="160" valign="top">
                                3.&nbsp;Тип&nbsp;запорного&nbsp;органа
                            </td>
                            <td class="tdborderbottom">
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
                            <td width="430" valign="top">
                                4.&nbsp;ГОСТ&nbsp;или&nbsp;ТУ&nbsp;по&nbsp;которому&nbsp;изготовлена&nbsp;арматура&nbsp;или&nbsp;№&nbsp;контракта
                            </td>
                            <td class="tdborderbottom">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="tdborderbottom" align="center">
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
                            <td width="150" valign="top">
                                5.&nbsp;Завод-изготовитель
                            </td>
                            <td class="tdborderbottom" width="150">
                                <asp:Label ID="lblFactoryVender" runat="server" Text="" />
                            </td>
                            <td width="60" align="right" valign="top">
                                Страна
                            </td>
                            <td class="tdborderbottom">
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
                            <td width="110" valign="top">
                                6.&nbsp;Заводской&nbsp;№
                            </td>
                            <td class="tdborderbottom" width="100">
                                <asp:Label ID="lblFactoryNumber" runat="server" Text="" />
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
                            <td width="120" valign="top">
                                7.&nbsp;Дата&nbsp;выпуска,&nbsp;г
                            </td>
                            <td class="tdborderbottom" width="100">
                                <asp:Label ID="lblDateOfCreating" runat="server" Text="" />
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
                            <td width="180" valign="top">
                                8.&nbsp;Диаметр&nbsp;условный&nbsp;Dy,&nbsp;мм
                            </td>
                            <td class="tdborderbottom" width="100">
                                <asp:Label ID="lblDiamUslovnyi" runat="server" Text="" />
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
                            <td width="200" valign="top">
                                9.&nbsp;Рабочее&nbsp;давление&nbsp;Py,&nbsp;кг/см<sup>2</sup>
                            </td>
                            <td class="tdborderbottom" width="100">
                                <asp:Label ID="lblDavlRab" runat="server" Text="" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 50px;" valign="bottom">
                    <asp:Label ID="Label10" runat="server" Text="II.&nbsp;Дополнительные&nbsp;устройства"
                        CssClass="text16bold" />
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" border="0">
                        <tr>
                            <td width="80" valign="top">
                                1.&nbsp;Обвязка
                            </td>
                            <td class="tdborderbottom">
                                <asp:Label ID="lblObvyazka" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td valign="top" align="center">
                                <asp:Label ID="Label13" runat="server" Text="(ручной байпасный кран и т.д.)" CssClass="textsizesmaller" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" border="0">
                        <tr>
                            <td width="80" valign="top">
                                2.&nbsp;Привод
                            </td>
                            <td class="tdborderbottom">
                                <asp:Label ID="lblPrivod" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td valign="top" align="center">
                                <asp:Label ID="Label14" runat="server" Text="(пневматический, пневмогидравлический, электрический, ручной)"
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
                            <td width="100" valign="top">
                                3.&nbsp;Управление
                            </td>
                            <td class="tdborderbottom">
                                <asp:Label ID="lblUpravleniye" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td valign="top" align="center">
                                <asp:Label ID="Label15" runat="server" Text="(дистанционное, местное)" CssClass="textsizesmaller" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" border="0">
                        <tr>
                            <td width="70" valign="top">
                                4.&nbsp;Прочее
                            </td>
                            <td class="tdborderbottom">
                                <asp:Label ID="lblProchee" runat="server" Text="" />
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
                <td style="height: 120px;" valign="bottom">
                    <table width="100%" border="0">
                        <tr>
                            <td width="160" valign="top">
                                <asp:Label ID="Label12" runat="server" Text="III.&nbsp;Место&nbsp;установки" CssClass="text16bold" />
                            </td>
                            <td class="tdborderbottom">
                                <asp:Label ID="lblPlaceOfInstallation" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="tdborderbottom">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" valign="top" align="center">
                                <asp:Label ID="Label17" runat="server" Text="(километр, метр на магистрали, свече, перемычке, дюкере, отводе,<br />цех, система, агрегат, аппарат и т.д.)"
                                    CssClass="textsizesmaller" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td valign="bottom">
                    <table width="100%" border="0">
                        <tr>
                            <td width="160" valign="top">
                                <asp:Label ID="Label16" runat="server" Text="IV.&nbsp;Дата&nbsp;установки" CssClass="text16bold" />
                            </td>
                            <td class="tdborderbottom">
                                <asp:Label ID="lblDateOnInstallation" runat="server" Text="" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td valign="bottom">
                    <table width="100%" border="0">
                        <tr>
                            <td width="330" valign="top">
                                <asp:Label ID="Label18" runat="server" Text="V.&nbsp;Присвоенный&nbsp;на&nbsp;месте&nbsp;установки&nbsp;номер"
                                    CssClass="text16bold" />
                            </td>
                            <td class="tdborderbottom">
                                <asp:Label ID="lblNumberOfInstallation" runat="server" Text="" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td valign="bottom">
                    <table width="100%" border="0">
                        <tr>
                            <td width="270" valign="top">
                                <asp:Label ID="Label19" runat="server" Text="VI.&nbsp;Условное&nbsp;обозначение&nbsp;на&nbsp;схеме"
                                    CssClass="text16bold" />
                            </td>
                            <td class="tdborderbottom">
                                <asp:Label ID="lblOboznNaScheme" runat="server" Text="" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" border="0">
                        <tr>
                            <td valign="top">
                                <asp:Label ID="Label20" runat="server" Text="VII.&nbsp;Эскиз&nbsp;места&nbsp;установки&nbsp;(включая&nbsp;обвязку)"
                                    CssClass="text16bold" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td valign="top" style="height: 250px;">
                    <table width="100%" border="0">
                        <tr>
                            <td valign="top">
                                <asp:Label ID="Label21" runat="server" Text="VII.&nbsp;Схема&nbsp;управления&nbsp;краном"
                                    CssClass="text16bold" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" border="0">
                        <tr>
                            <td width="160" valign="top" align="right">
                                Паспорт&nbsp;составил&nbsp;
                            </td>
                            <td width="180" class="tdborderbottom">
                                <asp:Label ID="lblPost" runat="server" Text="" />
                            </td>
                            <td width="20">
                                &nbsp;
                            </td>
                            <td class="tdborderbottom">
                                <asp:Label ID="lblFIO" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td valign="top" align="center">
                                <asp:Label ID="Label23" runat="server" Text="(должность)" CssClass="textsizesmaller" />
                            </td>
                            <td>
                            </td>
                            <td valign="top" align="center">
                                <asp:Label ID="Label24" runat="server" Text="(Ф.И.О.)" CssClass="textsizesmaller" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" border="0">
                        <tr>
                            <td width="160" valign="top" align="right">
                                Дата&nbsp;заполнения&nbsp;
                            </td>
                            <td width="180" class="tdborderbottom" style="padding-left: 0px;" align="center">
                                <asp:Label ID="lblDateOfFilling" runat="server" Text="" />
                            </td>
                            <td width="20">
                                г.
                            </td>
                            <td class="tdborderbottom">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td valign="top" align="center">
                                &nbsp;
                            </td>
                            <td>
                            </td>
                            <td valign="top" align="center">
                                <asp:Label ID="Label28" runat="server" Text="(подпись)" Style="font-style: italic;"
                                    CssClass="textsizesmaller" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 70px;">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td valign="bottom" style="height: 90px;">
                    <table width="100%" border="0">
                        <tr>
                            <td >
                                Учет&nbsp;работ,&nbsp;производимых&nbsp;на
                            </td>
                            <td class="tdborderbottom" colspan="6" width="380">
                                <asp:Label ID="lblObjectName" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7" align="center" valign="top">
                                <asp:Label ID="Label22" runat="server" Text="(кран,&nbsp;задвижка,&nbsp;вентиль)"
                                    CssClass="textsizesmaller" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <table>
                                    <tr>
                                        <td width="20">
                                            Dy
                                        </td>
                                        <td class="tdborderbottom" style="padding-left: 0px;" align="center" width="100">
                                            <asp:Label ID="lblDu" runat="server" Text="" />
                                        </td>
                                        <td width="20" align="right">
                                            Ру
                                        </td>
                                        <td class="tdborderbottom" style="padding-left: 0px;" align="center" width="100">
                                            <asp:Label ID="lblRy" runat="server" Text="" />
                                        </td>
                                        <td width="20" align="right">
                                            №
                                        </td>
                                        <td class="tdborderbottom" style="padding-left: 0px;" align="center" width="100">
                                            <asp:Label ID="lblNum" runat="server" Text="" />
                                        </td>
                                        <td width="140">
                                            во&nbsp;время&nbsp;эксплуатации
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 5px;">
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
                            <asp:BoundField HeaderText="Описание&nbsp;проведенных&nbsp;работ (при&nbsp;ревизии,&nbsp;осмотре, ремонте,&nbsp;опробовании)"
                                DataField="WORK_DESCRIPTION">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="WORK_ORGANIZATION" HeaderText="Кем выполнялись работы (организация, исполнитель)">
                                <ItemStyle Width="100px" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="WORK_SIGHN" HeaderText="Подпись лица, ответственного за содержание крана и принявшего его после ремонта">
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
