<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="PoPrint.aspx.cs"
    Inherits="BlueLedger.PL.PC.PO.PoPrint" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .art-footer .art-footer-text p
        {
            margin: 0;
        }
        .style1
        {
            font-size: large;
        }
        .style2
        {
            font-size: xx-small;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <table style="width: 100%;" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td align="left">
                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                    <tr style="height: 40px;">
                        <td rowspan="2" style="border-bottom: solid 5px #187EB8" width="25%">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/genex2010_logo.png"
                                Height="56px" Width="190px" />
                        </td>
                        <td align="center" width="50%">
                            <asp:Label ID="lbl_Title" runat="server" Text="Purchase Order" Font-Size="13pt" Font-Bold="true"
                                Style="font-size: x-large"></asp:Label>
                        </td>
                        <td align="right" rowspan="2" style="border-bottom: solid 5px #187EB8" width="25%">
                            <%--<span class="style2">891/24-25 Thanapat 360 ; Rama III Rd.,</span><br 
                                class="style2" />
                            <span class="style2">(Wongwean-Utsahakam) Bangpongpang,
                            </span>
                            <br class="style2" />
                            <span class="style2">Yannawa, Bangkok 10120 Thailand</span><br class="style2" />
                            <span class="style2">T: +662 284 1234 F: +662 284 3944</span>--%>
                        </td>
                    </tr>
                    <tr style="height: 25px;">
                        <td style="border-bottom: solid 5px #187EB8" align="center">
                            <span class="style1">Carmen Software Co.,Ltd.</span>
                        </td>
                    </tr>
                </table>
                <br />
                <table border="0" cellpadding="5" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td align="right" width="10%">
                            <asp:Label ID="Label1" runat="server" Text="Ref.#" Font-Bold="True"></asp:Label>
                        </td>
                        <td width="20%">
                            <asp:Label ID="lbl_PONumber" runat="server" Text="PO Number"></asp:Label>
                        </td>
                        <td align="right" width="10%">
                            <asp:Label ID="Label3" runat="server" Text="Date" Font-Bold="True"></asp:Label>
                        </td>
                        <td width="20%">
                            <asp:Label ID="lbl_PODate" runat="server" Text="PO Date"></asp:Label>
                        </td>
                        <td align="right" width="10%">
                            <asp:Label ID="Label71" runat="server" Text="Status" Font-Bold="True"></asp:Label>
                        </td>
                        <td width="20%">
                            <asp:Label ID="lbl_Status" runat="server" Text="Status"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="Label4" runat="server" Text="Vendor" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_VendorCode" runat="server" Text="Vendor Code"></asp:Label>
                        </td>
                        <td align="right">
                            <asp:Label ID="Label10" runat="server" Text="Name" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_VendorName" runat="server" Text="Vendor Name"></asp:Label>
                        </td>
                        <td align="right">
                            <asp:Label ID="Label9" runat="server" Text="Buyer" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_Buyer" runat="server" Text="Buyer/Purchasing"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="Label5" runat="server" Text="Store" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_Location" runat="server" Text="Location"></asp:Label>
                        </td>
                        <td align="right">
                            <asp:Label ID="Label11" runat="server" Text="Delivery Point" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_DeliveryPoint" runat="server" Text="Delivery Point"></asp:Label>
                        </td>
                        <td align="right">
                            <asp:Label ID="Label7" runat="server" Text="Delivery Date" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_DeliveryDate" runat="server" Text="Delivery Date"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="Label6" runat="server" Text="Currency" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_Currency" runat="server" Text="Currency"></asp:Label>
                        </td>
                        <td align="right">
                            <asp:Label ID="Label12" runat="server" Text="Ex. Rate" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_ExChangeRate" runat="server" Text="Exchange Rate"></asp:Label>
                        </td>
                        <td align="right">
                            <asp:Label ID="Label13" runat="server" Text="Credit Term" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_CreditTerm" runat="server" Text="Credit Term"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="Label8" runat="server" Text="Description" Font-Bold="True"></asp:Label>
                        </td>
                        <td colspan="5">
                            <asp:Label ID="lbl_Description" runat="server" Text="Description"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <dx:ASPxGridView ID="grd_PoDt" runat="server" AutoGenerateColumns="False" Width="100%"
                    Font-Size="8pt" KeyFieldName="PoNo">
                    <Templates>
                        <DetailRow>
                            <table border="1" cellpadding="0" cellspacing="0" width="800px">
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label66" runat="server" Text="Local Description"></asp:Label>
                                    </td>
                                    <td align="left">
                                        &nbsp;
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label38" runat="server" Font-Bold="False" Text="Vendor Prod. Code"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:HyperLink ID="HyperLink11" runat="server">V0001</asp:HyperLink>
                                        <asp:HyperLink ID="HyperLink12" runat="server" NavigateUrl="javascript:alert('more informatio')">...</asp:HyperLink>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label39" runat="server" Font-Bold="False" Text="Price Source"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="Label40" runat="server" Font-Bold="False" Text="0.00"></asp:Label>
                                    </td>
                                    <td align="right">
                                        Debit A/C
                                    </td>
                                    <td align="left">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label41" runat="server" Font-Bold="False" Text="Vendor"></asp:Label>
                                    </td>
                                    <td align="left">
                                        &nbsp;
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label42" runat="server" Font-Bold="False" Text="Telephone"></asp:Label>
                                    </td>
                                    <td align="left">
                                        &nbsp;
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label43" runat="server" Font-Bold="False" Text="Contact"></asp:Label>
                                    </td>
                                    <td align="left">
                                        &nbsp;
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label32" runat="server" Font-Bold="False" Text="Fax"></asp:Label>
                                    </td>
                                    <td align="left">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label44" runat="server" Font-Bold="False" Text="On Hand"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lbl_onhand" runat="server" Font-Bold="False" Text="0.00"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label46" runat="server" Font-Bold="False" Text="On Order"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lbl_order" runat="server" Font-Bold="False" Text="0.00"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label48" runat="server" Font-Bold="False" Text="ReOrder"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lbl_reorder" runat="server" Font-Bold="False" Text="0.00"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label50" runat="server" Font-Bold="False" Text="ReStock"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lbl_restock" runat="server" Font-Bold="False" Text="0.00"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label52" runat="server" Font-Bold="False" Text="Total On Hand"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lbl_totalonhand" runat="server" Font-Bold="False" Text="0.00"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label54" runat="server" Font-Bold="False" Text="Total On Order"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lbl_totalorder" runat="server" Font-Bold="False" Text="0.00"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label56" runat="server" Font-Bold="False" Text="Last Price"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lbl_lastprice" runat="server" Font-Bold="False" Text="0.00"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label58" runat="server" Font-Bold="False" Text="Last Vendor"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lbl_lastvendor" runat="server" Font-Bold="False" Text="0.00"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </DetailRow>
                    </Templates>
                    <Columns>
                        <dx:GridViewDataTextColumn VisibleIndex="0" Caption="Store" FieldName="locationname">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Product" VisibleIndex="1" FieldName="ProductDesc1">
                            <DataItemTemplate>
                                <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                    <tr align="left">
                                        <td style="border-top: none; border-left: none; border-right: none; border-bottom: none;
                                            width: 30%">
                                            <asp:Label ID="Label73" runat="server" Text='<%# Eval("ProductCode") %>'></asp:Label>
                                        </td>
                                        <td style="border-top: none; border-left: solid 1px #cfcfcf; border-right: none;
                                            border-bottom: none; width: 70%">
                                            <asp:Label ID="Label72" runat="server" Text='<%# Eval("ProductDesc1") %>'></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </DataItemTemplate>
                            <HeaderStyle>
                                <Paddings Padding="0px" />
                            </HeaderStyle>
                            <CellStyle>
                                <Paddings Padding="0px" />
                            </CellStyle>
                            <HeaderTemplate>
                                <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                    <tr align="center">
                                        <td align="center" colspan="2" style="border-top: none; border-left: none; border-right: none;
                                            border-bottom: solid 1px #9f9f9f;">
                                            <asp:Label ID="lbl_Product_Hr" runat="server" Text="Product"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td style="width: 30%">
                                            <asp:Label ID="Label67" runat="server" Text="Code"></asp:Label>
                                        </td>
                                        <td style="border-top: none; border-left: solid 1px #9f9f9f; border-right: none;
                                            border-bottom: none; width: 70%">
                                            <asp:Label ID="Label68" runat="server" Text="Name"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Unit" VisibleIndex="2" Width="40px" 
                            FieldName="UnitName">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Quantity" VisibleIndex="3">
                            <DataItemTemplate>
                                <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                    <tr align="left">
                                        <td style="border-top: none; border-left: none; border-right: none; border-bottom: none;
                                            width: 50%">
                                            <asp:Label ID="Label69" runat="server"></asp:Label>
                                        </td>
                                        <td style="border-top: none; border-left: solid 1px #cfcfcf; border-right: none;
                                            border-bottom: none; width: 50%">
                                            <asp:Label ID="Label14" runat="server" Text="&nbsp;"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </DataItemTemplate>
                            <HeaderStyle>
                                <Paddings Padding="0px" />
                            </HeaderStyle>
                            <CellStyle>
                                <Paddings Padding="0px" />
                            </CellStyle>
                            <HeaderTemplate>
                                <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                    <tr align="center">
                                        <td colspan="2" style="border-top: none; border-left: none; border-right: none; border-bottom: solid 1px #9f9f9f;">
                                            <asp:Label ID="lbl_AppQty_Hr" runat="server" Text="Quantity"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td align="left" style="border-top: none; border-left: solid 1px #9f9f9f; border-right: none;
                                            border-bottom: none; width: 50%">
                                            <asp:Label ID="Label70" runat="server" Text="Ord."></asp:Label>
                                        </td>
                                        <td align="left" style="border-top: none; border-left: solid 1px #9f9f9f; border-right: none;
                                            border-bottom: none; width: 50%">
                                            <asp:Label ID="Label15" runat="server" Text="FOC"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataSpinEditColumn Caption="Price" VisibleIndex="4" Width="40px">
                            <PropertiesSpinEdit DisplayFormatString="g">
                            </PropertiesSpinEdit>
                        </dx:GridViewDataSpinEditColumn>
                        <dx:GridViewDataTextColumn Caption="Discount<br>Amount" VisibleIndex="5" Width="40px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Net" VisibleIndex="6" Width="40px" FieldName="NetAmt">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Tax" FieldName="TaxAmt" VisibleIndex="7" Width="30px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn VisibleIndex="8" Caption="Amount" FieldName="TotalAmt"
                            Width="40px">
                            <Settings GroupInterval="DisplayText" />
                            <DataItemTemplate>
                                <asp:Label ID="lbl_TotalAmt0" runat="server" Text='<%# Eval("TotalAmt") %>'></asp:Label>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataHyperLinkColumn VisibleIndex="9" FieldName="PONo." Caption="Pr#"
                            Width="40px">
                            <PropertiesHyperLinkEdit>
                            </PropertiesHyperLinkEdit>
                            <DataItemTemplate>
                                <asp:HyperLink ID="hpl_PoNo" runat="server"></asp:HyperLink>
                            </DataItemTemplate>
                        </dx:GridViewDataHyperLinkColumn>
                        <dx:GridViewDataTextColumn VisibleIndex="10" Width="50px" Caption="Comment">
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <Settings ShowFooter="True" ShowGroupFooter="VisibleIfExpanded" ShowGroupedColumns="True" />
                    <Styles>
                        <Header HorizontalAlign="Center">
                        </Header>
                    </Styles>
                    <SettingsPager AlwaysShowPager="True" Visible="False">
                    </SettingsPager>
                    <TotalSummary>
                        <dx:ASPxSummaryItem FieldName="NetAmt" SummaryType="Sum" DisplayFormat="n" />
                        <dx:ASPxSummaryItem FieldName="TaxAmt" SummaryType="Sum" DisplayFormat="n" />
                        <dx:ASPxSummaryItem FieldName="TotalAmt" SummaryType="Sum" DisplayFormat="n" />
                    </TotalSummary>
                </dx:ASPxGridView>
                <br />
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <table border="0" cellpadding="5" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Image ID="Image2" runat="server" ImageUrl="~/App_Themes/Default/Images/IMG00564-20100904-0251.jpg" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        Mr.Thet Naung Soe
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        Purchasing Manager
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right">
                            <table border="0" cellpadding="5" cellspacing="0">
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="lblTotalAmt" runat="server" Text="Total Net Amount" Font-Bold="True">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxLabel ID="lbl_NetAmt" runat="server" Text="">
                                        </dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="lbl_TotalTaxAmt" runat="server" Text="Total Tax Amount" Font-Bold="True">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxLabel ID="lbl_TaxAmt" runat="server" Text="">
                                        </dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Discount" Font-Bold="True">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxLabel ID="lbl_Discount" runat="server" Text="">
                                        </dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxLabel ID="ASPxLabel4" runat="server" Text="Grand Total Amount" Font-Bold="True">
                                        </dx:ASPxLabel>
                                    </td>
                                    <td>
                                        <dx:ASPxLabel ID="lbl_TotalAmt" runat="server" Text="">
                                        </dx:ASPxLabel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
