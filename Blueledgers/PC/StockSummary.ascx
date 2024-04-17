<%@ Control Language="C#" AutoEventWireup="true" CodeFile="StockSummary.ascx.cs"
    Inherits="BlueLedger.PL.PC.StockSummary" %>
<asp:Panel ID="p_Stock" runat="server">
    <table border="0" cellpadding="2" cellspacing="0" width="100%">
        <%-- <tr style="background-color: #DADADA; height: 17px">
            <td>
                <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:PC_StockSummary, lbl_Title %>"
                    SkinID="LBL_HD_1"></asp:Label>
            </td>
        </tr>--%>
        <tr>
            <td>
                <%-- <table border="0" cellpadding="1" cellspacing="0" width="100%">
                    <tr style="height: 17px; vertical-align: top">
                        <td class="TD_LINE_GRD" width="25%">
                            <asp:Label ID="lbl_OnHand_Nm" runat="server" Text="<%$ Resources:PC_StockSummary, lbl_OnHand_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                        </td>
                        <td class="TD_LINE_GRD" width="25%">
                            <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR_1"></asp:Label>
                        </td>
                        <td class="TD_LINE_GRD" width="25%">
                            <asp:Label ID="lbl_OnOrder_Nm" runat="server" Text="<%$ Resources:PC_StockSummary, lbl_OnOrder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                        </td>
                        <td class="TD_LINE_GRD" width="25%">
                            <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                        </td>
                    </tr>
                    <tr style="height: 17px; vertical-align: top">
                        <td class="TD_LINE_GRD">
                            <asp:Label ID="lbl_Reorder_Nm" runat="server" Text="<%$ Resources:PC_StockSummary, lbl_Reorder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                        </td>
                        <td class="TD_LINE_GRD">
                            <asp:Label ID="lbl_Reorder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                        </td>
                        <td class="TD_LINE_GRD">
                            <asp:Label ID="lbl_Restock_Nm" runat="server" Text="<%$ Resources:PC_StockSummary, lbl_Restock_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                        </td>
                        <td class="TD_LINE_GRD">
                            <asp:Label ID="lbl_Restock" runat="server" SkinID="LBL_NR_1"></asp:Label>
                        </td>
                    </tr>
                    <tr style="height: 17px; vertical-align: top">
                        <td class="TD_LINE_GRD">
                            <asp:Label ID="lbl_LastPrice_Nm" runat="server" Text="<%$ Resources:PC_StockSummary, lbl_LastPrice_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                        </td>
                        <td class="TD_LINE_GRD">
                            <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_1"></asp:Label>
                        </td>
                        <td class="TD_LINE_GRD">
                            <asp:Label ID="lbl_LastVendor_Nm" runat="server" Text="<%$ Resources:PC_StockSummary, lbl_LastVendor_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                        </td>
                        <td class="TD_LINE_GRD">
                            <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1"></asp:Label>
                        </td>
                    </tr>
                </table>--%>
                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                    <tr style="height: 17px; vertical-align: top">
                        <td class="TD_LINE_GRD" width="8%">
                            <asp:Label ID="lbl_OnHand_Nm" runat="server" Text="<%$ Resources:PC_StockSummary, lbl_OnHand_Nm %>"
                                SkinID="LBL_HD_GRD"></asp:Label>
                        </td>
                        <td class="TD_LINE_GRD" width="8%">
                            <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR_1"></asp:Label>
                        </td>
                        <td class="TD_LINE_GRD" width="8%">
                            <asp:Label ID="lbl_OnOrder_Nm" runat="server" Text="<%$ Resources:PC_StockSummary, lbl_OnOrder_Nm %>"
                                SkinID="LBL_HD_GRD"></asp:Label>
                        </td>
                        <td class="TD_LINE_GRD" width="8%">
                            <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                        </td>
                        <td class="TD_LINE_GRD" width="8%">
                            <asp:Label ID="lbl_Reorder_Nm" runat="server" Text="<%$ Resources:PC_StockSummary, lbl_Reorder_Nm %>"
                                SkinID="LBL_HD_GRD"></asp:Label>
                        </td>
                        <td class="TD_LINE_GRD" width="10%">
                            <asp:Label ID="lbl_Reorder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                        </td>
                        <td class="TD_LINE_GRD" width="8%">
                            <asp:Label ID="lbl_Restock_Nm" runat="server" Text="<%$ Resources:PC_StockSummary, lbl_Restock_Nm %>"
                                SkinID="LBL_HD_GRD"></asp:Label>
                        </td>
                        <td class="TD_LINE_GRD" width="10%">
                            <asp:Label ID="lbl_Restock" runat="server" SkinID="LBL_NR_1"></asp:Label>
                        </td>
                        <td class="TD_LINE_GRD" width="8%">
                            <asp:Label ID="lbl_LastPrice_Nm" runat="server" Text="<%$ Resources:PC_StockSummary, lbl_LastPrice_Nm %>"
                                SkinID="LBL_HD_GRD"></asp:Label>
                        </td>
                        <td class="TD_LINE_GRD" width="10%">
                            <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_1"></asp:Label>
                        </td>
                        <td class="TD_LINE_GRD" width="8%">
                            <asp:Label ID="lbl_LastVendor_Nm" runat="server" Text="<%$ Resources:PC_StockSummary, lbl_LastVendor_Nm %>"
                                SkinID="LBL_HD_GRD"></asp:Label>
                        </td>
                        <td class="TD_LINE_GRD" width="10%">
                            <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
<%--<dx:ASPxRoundPanel ID="rp_StockSum" runat="server" Width="100%" Font-Bold="True"
    HeaderText="Stock Summary" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
    GroupBoxCaptionOffsetY="-28px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
    <PanelCollection>
        <dx:PanelContent>
            <table width="100%">
                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel13" runat="server" Text="On Hand" Font-Bold="true">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxLabel ID="lbl_OnHand" runat="server">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel14" runat="server" Text="On Order" Font-Bold="true">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxLabel ID="lbl_OnOrder" runat="server">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel15" runat="server" Text="Reorder" Font-Bold="true">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxLabel ID="lbl_ReOrder" runat="server">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel18" runat="server" Text="Restock" Font-Bold="true">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxLabel ID="lbl_Restock" runat="server">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel16" runat="server" Text="Last Price" Font-Bold="true">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxLabel ID="lbl_LastPrice" runat="server">
                        </dx:ASPxLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel17" runat="server" Text="Last Vendor" Font-Bold="true">
                        </dx:ASPxLabel>
                    </td>
                    <td>
                        <dx:ASPxLabel ID="lbl_LastVendor" runat="server">
                        </dx:ASPxLabel>
                    </td>
                </tr>
            </table>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxRoundPanel>--%>