<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="PrPrint.aspx.cs"
    Inherits="BlueLedger.PL.PC.PR.PrPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%@ register assembly="DevExpress.Web.ASPxGridView.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ register assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
        <tr>
            <td align="left">
                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                    <tr style="height: 40px;">
                        <td style="border-bottom: solid 5px #187EB8">
                            <asp:Label ID="lbl_Title" runat="server" Text="Purchase Request" Font-Size="13pt"
                                Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="right" style="width: 10%">
                            <asp:Label ID="lbl_PRNo_Nm" runat="server" Font-Bold="True" Text="Ref.#"></asp:Label>
                        </td>
                        <td style="width: 20%">
                            <asp:Label ID="lbl_PRNo" runat="server"></asp:Label>
                        </td>
                        <td align="right" style="width: 10%">
                            <asp:Label ID="lbl_PRDate_Nm" runat="server" Font-Bold="True" Text="Date"></asp:Label>
                        </td>
                        <td style="width: 20%">
                            <asp:Label ID="lbl_PRDate" runat="server"></asp:Label>
                            <%-- <DataItemTemplate>
                                <asp:Label ID="lbl_NetAmt" runat="server" Text='<%# Eval("NetAmt") %>'></asp:Label>
                                <table border="0" cellpadding="3" cellspacing="0">
                                    <tr style="height: 25px">
                                        <td style="width: 80px;">
                                            <asp:Label ID="lbl_Code" runat="server" Text='<%# Eval("ProductCode") %>'></asp:Label>
                                        </td>
                                        <td style="border-top: none; border-left: solid 1px #cfcfcf; border-right: none;
                                            border-bottom: none; width: 161px">
                                            <asp:Label ID="lbl_Desc3" runat="server" Text='<%# Eval("ProDesc") %>'></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </DataItemTemplate>--%>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lbl_Location_Nm" runat="server" Font-Bold="True" Text="Store"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_Location" runat="server"></asp:Label>
                        </td>
                        <td align="right">
                            <asp:Label ID="lbl_DeliPoing_Nm" runat="server" Font-Bold="True" Text="Delivery Point"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_DeliPoint" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="Label7" runat="server" Font-Bold="true" Text="Buyer"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="right">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lbl_Desc_Nm" runat="server" Font-Bold="True" Text="Description"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:Label ID="lbl_Desc" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <dx:ASPxGridView ID="grd_PrDt" runat="server" AutoGenerateColumns="False" Width="100%"
                    KeyFieldName="PRDtNo" OnDetailRowExpandedChanged="grd_PrDt_DetailRowExpandedChanged"
                    OnHtmlRowCreated="grd_PrDt_HtmlRowCreated" Font-Size="8pt">
                    <Templates>
                        <DetailRow>
                            <table border="1" cellpadding="0" cellspacing="0" width="800px">
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label5" runat="server" Text="Local Description"></asp:Label>
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
                        <dx:GridViewCommandColumn VisibleIndex="0" Width="20px">
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn VisibleIndex="1" Caption="Store">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Product" VisibleIndex="2" Width="200px">
                            <%-- <DataItemTemplate>
                                <asp:Label ID="lbl_NetAmt" runat="server" Text='<%# Eval("NetAmt") %>'></asp:Label>
                                <table border="0" cellpadding="3" cellspacing="0">
                                    <tr style="height: 25px">
                                        <td style="width: 80px;">
                                            <asp:Label ID="lbl_Code" runat="server" Text='<%# Eval("ProductCode") %>'></asp:Label>
                                        </td>
                                        <td style="border-top: none; border-left: solid 1px #cfcfcf; border-right: none;
                                            border-bottom: none; width: 161px">
                                            <asp:Label ID="lbl_Desc3" runat="server" Text='<%# Eval("ProDesc") %>'></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </DataItemTemplate>--%>
                            <DataItemTemplate>
                                <table border="0" cellpadding="3" cellspacing="0">
                                    <tr style="height: 25px">
                                        <td style="width: 80px;">
                                            <asp:Label ID="lbl_Code" runat="server" Text='<%# Eval("ProductCode") %>'></asp:Label>
                                        </td>
                                        <td style="border-top: none; border-left: solid 1px #cfcfcf; border-right: none;
                                            border-bottom: none; width: 161px">
                                            <asp:Label ID="lbl_Desc3" runat="server" Text='<%# Eval("ProDesc") %>'></asp:Label>
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
                                <table border="0" cellpadding="3" cellspacing="0">
                                    <tr align="center" style="height: 20px">
                                        <td align="center" colspan="2" style="border-top: none; border-left: none; border-right: none;
                                            border-bottom: solid 1px #9f9f9f;">
                                            <asp:Label ID="lbl_Product_Hr" runat="server" Text="Product"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr align="center" style="height: 20px">
                                        <td style="width: 93px">
                                            <asp:Label ID="Label1" runat="server" Text="Code"></asp:Label>
                                        </td>
                                        <td style="border-top: none; border-left: solid 1px #9f9f9f; border-right: none;
                                            border-bottom: none; width: 160px">
                                            <asp:Label ID="Label2" runat="server" Text="Name"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Unit" VisibleIndex="3" FieldName="OrderUnit"
                            Width="40px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Quantity" VisibleIndex="4" Width="200px">
                            <DataItemTemplate>
                                <table border="0" cellpadding="4" cellspacing="0">
                                    <tr align="left" style="height: 25px">
                                        <td align="right" style="width: 40px;">
                                            <asp:Label ID="lbl_Desc0" runat="server" Text='<%# Eval("ReqQty") %>'></asp:Label>
                                        </td>
                                        <td style="border-top: none; border-left: solid 1px #cfcfcf; border-right: none;
                                            border-bottom: none; width: 50px">
                                            <asp:Label ID="lbl_Appr" runat="server" Text='<%# Eval("ApprQty") %>'></asp:Label>
                                        </td>
                                        <td style="border-top: none; border-left: solid 1px #cfcfcf; border-right: none;
                                            border-bottom: none; width: 50px">
                                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("OrderQty") %>'></asp:Label>
                                        </td>
                                        <td style="border-top: none; border-left: solid 1px #cfcfcf; border-right: none;
                                            border-bottom: none; width: 50px">
                                            <asp:Label ID="Label8" runat="server" Text='<%# Eval("FOCQty") %>'></asp:Label>
                                        </td>
                                        <td style="border-top: none; border-left: solid 1px #cfcfcf; border-right: none;
                                            border-bottom: none; width: 50px">
                                            <asp:Label ID="lbl_Code0" runat="server" Text="&nbsp;"></asp:Label>
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
                                <table border="0" cellpadding="3" cellspacing="0">
                                    <tr align="center" style="height: 20px">
                                        <td colspan="5" style="border-top: none; border-left: none; border-right: none; border-bottom: solid 1px #9f9f9f;">
                                            <asp:Label ID="lbl_AppQty_Hr" runat="server" Text="Quantity"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr align="center" style="height: 20px">
                                        <td align="right" style="width: 50px">
                                            <asp:Label ID="Label64" runat="server" Text="Req."></asp:Label>
                                        </td>
                                        <td align="left" style="border-top: none; border-left: solid 1px #9f9f9f; border-right: none;
                                            border-bottom: none; width: 50px">
                                            <asp:Label ID="Label36" runat="server" Text="Appr."></asp:Label>
                                        </td>
                                        <td align="left" style="border-top: none; border-left: solid 1px #9f9f9f; border-right: none;
                                            border-bottom: none; width: 50px">
                                            <asp:Label ID="Label4" runat="server" Text="Ord."></asp:Label>
                                        </td>
                                        <td align="left" style="border-top: none; border-left: solid 1px #9f9f9f; border-right: none;
                                            border-bottom: none; width: 50px">
                                            <asp:Label ID="Label6" runat="server" Text="FOC."></asp:Label>
                                        </td>
                                        <td align="left" style="border-top: none; border-left: solid 1px #9f9f9f; border-right: none;
                                            border-bottom: none; width: 50px">
                                            <asp:Label ID="Label65" runat="server" Text="Rcv."> </asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataSpinEditColumn Caption="Price" FieldName="Price" VisibleIndex="5"
                            Width="40px">
                            <PropertiesSpinEdit DisplayFormatString="g">
                            </PropertiesSpinEdit>
                        </dx:GridViewDataSpinEditColumn>
                        <dx:GridViewDataTextColumn Caption="Discount%" VisibleIndex="6" Width="45px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Discount<br>Amount" VisibleIndex="6" Width="40px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn VisibleIndex="6" Width="100px">
                            <HeaderStyle>
                                <Paddings Padding="0px" />
                            </HeaderStyle>
                            <HeaderTemplate>
                                <table border="0" cellpadding="3" cellspacing="0">
                                    <tr align="center" style="height: 20px">
                                        <td align="center" colspan="3" style="border-top: none; border-left: none; border-right: none;
                                            border-bottom: solid 1px #9f9f9f;">
                                            <asp:Label ID="lbl_Tax_Hr" runat="server" Text="Tax"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr align="center" style="height: 20px">
                                        <td style="width: 40px">
                                            <asp:Label ID="lbl_Type_Hr" runat="server" Text="Type"></asp:Label>
                                        </td>
                                        <td style="border-top: none; border-left: solid 1px #9f9f9f; border-right: none;
                                            border-bottom: none; width: 40px">
                                            <asp:Label ID="lbl_Rate_Hr" runat="server" Text="Rate"></asp:Label>
                                        </td>
                                        <td style="border-top: none; border-left: solid 1px #9f9f9f; border-right: none;
                                            border-bottom: none; width: 30px">
                                            <asp:Label ID="lbl_Adj_Hr" runat="server" Text="Adj."></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <CellStyle>
                                <Paddings Padding="0px" />
                            </CellStyle>
                            <DataItemTemplate>
                                <table border="0" cellpadding="4" cellspacing="0">
                                    <tr align="left" style="height: 25px">
                                        <td align="right" style="width: 40px;">
                                            <asp:Label ID="lbl_Type" runat="server"></asp:Label>
                                        </td>
                                        <td style="border-top: none; border-left: solid 1px #cfcfcf; border-right: none;
                                            border-bottom: none; width: 50px">
                                            <asp:Label ID="lbl_Rate" runat="server"></asp:Label>
                                        </td>
                                        <td style="border-top: none; border-left: solid 1px #cfcfcf; border-right: none;
                                            border-bottom: none; width: 50px">
                                            <dx:ASPxCheckBox ID="chk_Adj" runat="server">
                                            </dx:ASPxCheckBox>
                                        </td>
                                    </tr>
                                </table>
                            </DataItemTemplate>
                            <FooterTemplate>
                                Total
                            </FooterTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Net" FieldName="NetAmt" VisibleIndex="10" Width="40px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Tax" FieldName="TaxAmt" VisibleIndex="11" Width="30px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="TotalAmt" VisibleIndex="12" Width="40px" Caption="Amount">
                            <Settings GroupInterval="DisplayText" />
                            <DataItemTemplate>
                                <asp:Label ID="lbl_TotalAmt" runat="server" Text='<%# Eval("TotalAmt") %>'></asp:Label>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Buyer" VisibleIndex="13" Width="40px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataHyperLinkColumn VisibleIndex="14" FieldName="VendorCode" Caption="Vendor"
                            Width="80px">
                            <PropertiesHyperLinkEdit>
                            </PropertiesHyperLinkEdit>
                            <DataItemTemplate>
                                <asp:HyperLink ID="hpl_Vendor" runat="server"></asp:HyperLink>
                            </DataItemTemplate>
                            <CellStyle Wrap="False">
                            </CellStyle>
                        </dx:GridViewDataHyperLinkColumn>
                        <dx:GridViewDataHyperLinkColumn VisibleIndex="15" FieldName="PONo." Width="40px">
                            <PropertiesHyperLinkEdit>
                            </PropertiesHyperLinkEdit>
                            <DataItemTemplate>
                                <asp:HyperLink ID="hpl_PoNo" runat="server"></asp:HyperLink>
                            </DataItemTemplate>
                        </dx:GridViewDataHyperLinkColumn>
                        <dx:GridViewDataDateColumn Caption="Require<br>Date" VisibleIndex="16" Width="30px">
                        </dx:GridViewDataDateColumn>
                        <dx:GridViewDataTextColumn VisibleIndex="17" Caption="Delivery<br>Point" Width="50px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Comment" VisibleIndex="18" Width="50px">
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <Settings ShowFooter="True" ShowGroupFooter="VisibleIfExpanded"
                        ShowGroupedColumns="True" />
                    <Styles>
                        <Header HorizontalAlign="Center">
                        </Header>
                    </Styles>
                    <SettingsPager Visible="False">
                    </SettingsPager>
                    <TotalSummary>
                        <dx:ASPxSummaryItem FieldName="NetAmt" SummaryType="Sum" DisplayFormat="n" />
                        <dx:ASPxSummaryItem FieldName="TaxAmt" SummaryType="Sum" DisplayFormat="n" />
                        <dx:ASPxSummaryItem FieldName="TotalAmt" SummaryType="Sum" DisplayFormat="n" />
                    </TotalSummary>
                </dx:ASPxGridView>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
