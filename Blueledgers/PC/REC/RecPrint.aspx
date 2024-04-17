<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="RecPrint.aspx.cs"
    Inherits="BlueLedger.PL.IN.REC.RECPrint" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <table style="width: 100%;" border="0" cellpadding="5" cellspacing="0">
        <tr>
            <td align="left">
                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                    <tr style="height: 40px;">
                        <td style="border-bottom: solid 5px #187EB8">
                            <asp:Label ID="Label1" runat="server" Text="Receving" Font-Size="13pt" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <table border="0" cellpadding="3" cellspacing="0" width="100%">
                    <tr>
                        <td align="right" style="width: 13%">
                            <asp:Label ID="lbl_RecNo_Nm" runat="server" Text="Ref.#" Font-Bold="True"></asp:Label>
                        </td>
                        <td style="width: 20%">
                            <asp:Label ID="lbl_RecNo" runat="server"></asp:Label>
                        </td>
                        <td align="right" style="width: 13%">
                            <asp:Label ID="lbl_RecDate_Nm" runat="server" Text="Date" Font-Bold="True"></asp:Label>
                        </td>
                        <td style="width: 20%">
                            <asp:Label ID="lbl_RecDate" runat="server"></asp:Label>
                        </td>
                        <td align="right" style="width: 14%">
                            <asp:Label ID="lbl_Status_Nm" runat="server" Text="Status" Font-Bold="True"></asp:Label>
                        </td>
                        <td style="width: 20%">
                            <asp:Label ID="lbl_DocStatus" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lbl_InvNo_Nm" runat="server" Text="Doc.#" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_InvNo" runat="server"></asp:Label>
                        </td>
                        <td align="right">
                            <asp:Label ID="lbl_InvDate_Nm" runat="server" Text="Doc. Date" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_InvDate" runat="server"></asp:Label>
                        </td>
                        <td align="right">
                            <asp:Label ID="lbl_DeliPoing_Nm" runat="server" Text="Delivery Point" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_DeliPoint" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lbl_VendorCode_Nm" runat="server" Text="Vendor" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_VendorCode" runat="server"></asp:Label>
                            &nbsp;-
                            <asp:Label ID="lbl_VendorNm" runat="server"></asp:Label>
                        </td>
                        <td align="right">
                            <asp:Label ID="lbl_Currency_Nm" runat="server" Text="Currency" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_Currency" runat="server"></asp:Label>
                            &nbsp;@
                            <asp:Label ID="lbl_ExRateAudit" runat="server"></asp:Label>
                        </td>
                        <td align="right">
                            <asp:Label ID="lbl_DeliPoing_Nm0" runat="server" Text="Cash/Consignment" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chk_CashConsign" runat="server" Enabled="False" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lbl_Desc_Nm" runat="server" Text="Descritpion" Font-Bold="True"></asp:Label>
                        </td>
                        <td colspan="5">
                            <asp:Label ID="lbl_Desc" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <dx:ASPxGridView ID="grd_RecEdit" runat="server" AutoGenerateColumns="False" Width="100%"
                    KeyFieldName="RecDtNo" Font-Size="8pt">
                    <Templates>
                        <DetailRow>
                            <table border="0" cellpadding="3" cellspacing="0" style="width: 100%;">
                                <tr>
                                    <td align="right" width="10%">
                                        Local Description
                                    </td>
                                    <td width="23">
                                        &nbsp;
                                    </td>
                                    <td align="right" width="10%">
                                        PO#
                                    </td>
                                    <td width="23%">
                                        <asp:Label ID="Label6" runat="server" Text='<%# Eval("PoNo") %>'></asp:Label>
                                    </td>
                                    <td align="right" width="10%">
                                        PR#
                                    </td>
                                    <td width="24%">
                                        <asp:Label ID="Label7" runat="server" Text='<%# Eval("PrNo") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        Vendor SKU
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td align="right">
                                        Net Dr.A/C
                                    </td>
                                    <td>
                                        <asp:Label ID="Label8" runat="server" Text='<%# Eval("NetDrAccNm") %>'></asp:Label>
                                    </td>
                                    <td align="right">
                                        Tax Dr. A/C
                                    </td>
                                    <td>
                                        <asp:Label ID="Label9" runat="server" Text='<%# Eval("TaxDrAccNm") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        Contact
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td align="right">
                                        Telephone
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td align="right">
                                        Fax
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </DetailRow>
                    </Templates>
                    <TotalSummary>
                        <dx:ASPxSummaryItem DisplayFormat="#,###.##" FieldName="NetAmt" 
                            SummaryType="Sum" />
                        <dx:ASPxSummaryItem DisplayFormat="#,###.##" FieldName="TaxAmt" 
                            SummaryType="Sum" />
                        <dx:ASPxSummaryItem DisplayFormat="#,###.##" FieldName="TotalAmt" 
                            ShowInColumn="Amount" SummaryType="Sum" />
                    </TotalSummary>
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="No." VisibleIndex="0" Width="15px" FieldName="RecDtNo">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn VisibleIndex="1" Caption="Store" Width="100px" FieldName="LocationName">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn VisibleIndex="2" FieldName="ProductCode" Caption="SKU#"
                            Width="40px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn VisibleIndex="3" FieldName="ProductName" Caption="Description"
                            Width="150px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn VisibleIndex="4" Caption="Ord" Width="100px" FieldName="UnitName">
                            <DataItemTemplate>
                                <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                    <tr>
                                        <td width="50%" style="border-top: none; border-left: none; border-right: solid 1px #9F9F9F;
                                            border-bottom: none">
                                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("UnitName") %>'></asp:Label>
                                        </td>
                                        <td width="50%">
                                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("OrderQty") %>'></asp:Label>
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
                                    <tr>
                                        <td colspan="2" align="center" style="border-top: none; border-left: none; border-right: none;
                                            border-bottom: solid 1px #9F9F9F;">
                                            Order
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="50%" align="center" style="border-top: none; border-left: none; border-right: solid 1px #9F9F9F;
                                            border-bottom: none">
                                            Unit
                                        </td>
                                        <td width="50%" align="center">
                                            Qty.
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn VisibleIndex="6" FieldName="RecQty" Caption="Rcv.Qty."
                            Width="50px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn VisibleIndex="7" Caption="FOC" Width="40px" FieldName="FOCQty">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn VisibleIndex="8" FieldName="Price" Caption="Price"
                            Width="50px">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn VisibleIndex="9" Caption="Tax">
                            <DataItemTemplate>
                                <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                    <tr>
                                        <td width="33%" style="border-top: none; border-left: none; border-right: solid 1px #9F9F9F;
                                            border-bottom: none;">
                                            <asp:Label ID="Label4" runat="server" Text='<%# Eval("TaxType") %>'></asp:Label>
                                        </td>
                                        <td width="33%" style="border-top: none; border-left: none; border-right: solid 1px #9F9F9F;
                                            border-bottom: none;">
                                            <asp:Label ID="Label5" runat="server" Text='<%# Eval("TaxRate") %>'></asp:Label>
                                        </td>
                                        <td width="34%" style="border-top: none; border-left: none; border-right: solid 1px #9F9F9F;
                                            border-bottom: none;">
                                            <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Eval("TaxAdj") %>' Enabled="false" />
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
                            <FooterCellStyle HorizontalAlign="Right">
                            </FooterCellStyle>
                            <HeaderTemplate>
                                <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                    <tr>
                                        <td align="center" colspan="3" style="border-top: none; border-left: none; border-right: none;
                                            border-bottom: solid 1px #9F9F9F;">
                                            Tax
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" width="33%" style="border-top: none; border-left: none; border-right: solid 1px #9F9F9F;
                                            border-bottom: none;">
                                            Type
                                        </td>
                                        <td align="center" width="33%" style="border-top: none; border-left: none; border-right: solid 1px #9F9F9F;
                                            border-bottom: none;">
                                            Rate
                                        </td>
                                        <td align="center" width="34%">
                                            Adj.
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <FooterTemplate>
                                Total
                            </FooterTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn VisibleIndex="9" FieldName="NetAmt" Caption="Net" 
                            Width="60px">
                            <Settings GroupInterval="Value" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn VisibleIndex="10" FieldName="TaxAmt" Caption="Tax" Width="50px">
                            <Settings GroupInterval="Value" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn VisibleIndex="11" FieldName="TotalAmt" Caption="Amount"
                            Width="80px">
                            <Settings GroupInterval="Value" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Status" VisibleIndex="12" Width="50px" FieldName="Status">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn VisibleIndex="13" Caption="Net Dr. Acc#" Width="100px"
                            FieldName="NetDrAccNm" Visible="False">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn VisibleIndex="15" FieldName="TaxDrAccNm" Caption="Tax Dr. Acc#"
                            Width="100px" Visible="False">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn VisibleIndex="12" FieldName="PoNo" Caption="PO#" Width="50px"
                            Visible="False">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn VisibleIndex="12" FieldName="PrNo" Caption="PR#" Width="50px"
                            Visible="False">
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <Settings ShowFooter="True" />
                    <SettingsDetail ShowDetailRow="True" />
                </dx:ASPxGridView>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
