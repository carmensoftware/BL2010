<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="Po.aspx.cs" Inherits="BlueLedger.PL.PC.PO.PO" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Src="../StockSummary.ascx" TagName="StockSummary" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Comment2.ascx" TagName="Comment2" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/Attach2.ascx" TagName="Attach2" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Log2.ascx" TagName="Log2" TagPrefix="uc4" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <script type="text/javascript">
        function expandDetailsInGrid(_this) {
            var id = _this.id;
            var imgelem = document.getElementById(_this.id);
            var currowid = id.replace("Img_Btn", "TR_PrDetail") //GETTING THE ID OF SUMMARY ROW

            var rowdetelemid = currowid;
            var rowdetelem = document.getElementById(rowdetelemid);
            if (imgelem.alt == "plus") {
                imgelem.src = "../../App_Themes/Default/Images/master/in/Default/Plus.jpg"
                imgelem.alt = "minus"
                rowdetelem.style.display = 'none';
            }
            else {
                imgelem.src = "../../App_Themes/Default/Images/master/in/Default/Minus.jpg"
                imgelem.alt = "plus"
                rowdetelem.style.display = '';
            }

            return false;
        }
    </script>
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
        <tr>
            <td align="left">
                <!-- Title & Command Bar  -->
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr style="background-color: #4d4d4d; height: 17px;">
                        <td align="left" style="padding-left: 10px;">
                            <table border="0" cellpadding="2" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:PO_Default, lbl_Title %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right" style="padding-right: 10px;">
                            <dx:ASPxMenu runat="server" ID="menu_CmdBar" Font-Bold="True" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle"
                                Height="16px" OnItemClick="menu_CmdBar_ItemClick">
                                <ItemStyle BackColor="Transparent">
                                    <HoverStyle BackColor="Transparent">
                                        <Border BorderStyle="None" />
                                    </HoverStyle>
                                    <Paddings Padding="2px" />
                                    <Border BorderStyle="None" />
                                </ItemStyle>
                                <Items>
                                    <dx:MenuItem Name="Edit" Text="">
                                        <ItemStyle Height="16px" Width="38px">
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/edit.png" Repeat="NoRepeat" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Void" Text="">
                                        <ItemStyle Height="16px" Width="41px">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/void.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="ClosePo" Text="">
                                        <ItemStyle Height="16px" Width="57px">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/closepo.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Print" Text="">
                                        <ItemStyle Height="16px" Width="43px">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Mail" Text="email">
                                        <ItemStyle ForeColor="White" Font-Size="Small" />
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Back" Text="">
                                        <ItemStyle Height="16px" Width="42px">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                </Items>
                                <Paddings Padding="0px" />
                                <SeparatorPaddings Padding="0px" />
                                <SubMenuStyle HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" Font-Size="9pt" ForeColor="#4D4D4D" />
                                <Border BorderStyle="None"></Border>
                            </dx:ASPxMenu>
                        </td>
                    </tr>
                </table>
                <!-- PO Header -->
                <table border="0" cellpadding="2" cellspacing="0" width="100%" class="TABLE_HD">
                    <tr>
                        <td rowspan="7" style="width: 1%;">
                        </td>
                        <td align="left" style="width: 7%;">
                            <!-- Text="PO Ref.#:" -->
                            <asp:Label ID="lbl_Ref_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Ref_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left" style="width: 10%">
                            <asp:Label ID="lbl_PONumber" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                        <td align="left" style="width: 5%">
                            <asp:Label ID="lbl_Buyer_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Buyer_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left" style="width: 20%">
                            <asp:Label ID="lbl_Buyer" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                        <td align="left" style="width: 10%">
                            <asp:Label ID="lbl_Currency_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Currency_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="right" style="width: 10%">
                            <asp:Label ID="lbl_Currency" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            <asp:Label ID="Label1" runat="server" SkinID="LBL_NR_BLUE" Text="@"></asp:Label>
                            <asp:Label ID="lbl_Exchange" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                        <td align="left" style="width: 5%">
                            <asp:Label ID="lbl_Status_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Status_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left" style="width: 10%">
                            <asp:Label ID="lbl_Status" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lbl_PODate_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_PODate_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_PODate" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_vendor_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_vendor_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_VendorCode" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_CreditTerm_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_CreditTerm_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="right">
                            <asp:Label ID="lbl_CreditTerm" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_InvDate_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_InvDate_Nm %>" SkinID="LBL_HD" Visible="False"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_DocDate" runat="server" SkinID="LBL_NR_BLUE" Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lbl_DeliveryDate_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_DeliveryDate_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_DeliveryDate" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                        <td colspan="4" align="left">
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_InvNo_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_InvNo_Nm %>" SkinID="LBL_HD" Visible="False"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_DocNo" runat="server" SkinID="LBL_NR_BLUE" Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lbl_Description_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Description_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td colspan="7" align="left">
                            <asp:Label ID="lbl_Description" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lbl_Remark1_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Remark1_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td colspan="7" align="left">
                            <asp:Label ID="lbl_Remark1" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lbl_Remark2_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Remark2_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td colspan="7" align="left">
                            <asp:Label ID="lbl_Remark2" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lbl_Remark3_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Remark3_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td colspan="7" align="left">
                            <asp:Label ID="lbl_Remark3" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <asp:GridView ID="grd_PoDt" runat="server" DataKeyNames="PoDt" Width="100%" AutoGenerateColumns="False" EnableModelValidation="True" OnRowDataBound="grd_PoDt_RowDataBound"
                                SkinID="GRD_V1.1" CssClass="empty" OnRowEditing="grd_PoDt_RowEditing" OnRowCancelingEdit="grd_PoDt_RowCancelingEdit" ShowFooter="False">
                                <Columns>
                                    <asp:TemplateField>
                                        <%--<HeaderTemplate>
                                            <asp:Label ID="lbl_HD1" runat="server" Text="<%$ Resources:PO_Default, lbl_HD1 %>"
                                                SkinID="LBL_HD_W"></asp:Label>
                                        </HeaderTemplate>--%>
                                        <ItemTemplate>
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr style="height: 17px">
                                                    <%-- <td valign="middle">
                                                        <asp:ImageButton ID="btn_Expand" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/Plus.jpg"
                                                            OnClick="btn_Expand_Click" />                        
                                                    </td>--%>
                                                    <td>
                                                        <asp:ImageButton ID="Img_Btn" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/Plus.jpg" OnClientClick="expandDetailsInGrid(this);return false;" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="10px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lbl_HD" runat="server" Text="#" SkinID="LBL_HD_W"></asp:Label>
                                        </HeaderTemplate>
                                        <HeaderStyle Width="10px" HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="Chk_Item" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" Width="10px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lbl_SKU_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_SKU_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="white-space: nowrap; overflow: hidden; width: 95%">
                                                <asp:Label ID="lbl_SKU" runat="server" SkinID="LBL_NR"></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="35%" />
                                        <ItemStyle HorizontalAlign="Left" Width="35%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txt_QtyOrder" runat="server"></asp:TextBox>
                                        </EditItemTemplate>
                                        <HeaderTemplate>
                                            <asp:Label ID="lbl_QTYOrder_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_QTYOrder_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_QTYOrder" runat="server" SkinID="LBL_NR"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="right" Width="10%" />
                                        <ItemStyle HorizontalAlign="right" Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lbl_RCV_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_RCV_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_RCV" runat="server" SkinID="LBL_NR"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="right" Width="10%" />
                                        <ItemStyle HorizontalAlign="right" Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lbl_FOC_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_FOC_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_FOC" runat="server" SkinID="LBL_NR"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="right" Width="10%" />
                                        <ItemStyle HorizontalAlign="right" Width="10%" />
                                        <FooterStyle HorizontalAlign="left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lbl_Cancel_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Cancel_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Cancel" runat="server" SkinID="LBL_NR"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="right" Width="10%" />
                                        <ItemStyle HorizontalAlign="right" Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lbl_Price_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Price_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Price" runat="server" SkinID="LBL_NR"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="right" Width="12%" />
                                        <ItemStyle HorizontalAlign="right" Width="12%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lbl_Amount_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Amount_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Amount" runat="server" SkinID="LBL_NR"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="right" Width="13%" />
                                        <ItemStyle HorizontalAlign="right" Width="13%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="true" HeaderStyle-Width="0px">
                                        <HeaderStyle Width="0px"></HeaderStyle>
                                        <ItemStyle Width="0px" />
                                        <ItemTemplate>
                                            <tr id="TR_PrDetail" runat="server" style="display: none">
                                                <td colspan="15" style="padding-left: 10px; padding-right: 0px">
                                                    <asp:Panel ID="p_PoEdit" runat="server">
                                                        <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td width="35px">
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                        <tr>
                                                                            <td width="6%">
                                                                                <asp:Label ID="lbl_Receive_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Receive_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td width="7%" align="right">
                                                                                <asp:Label ID="lbl_Receive" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                            </td>
                                                                            <td style="width: 5%; padding-left: 10px;" align="left">
                                                                            </td>
                                                                            <td colspan="2">
                                                                            </td>
                                                                            <td style="width: 5%;">
                                                                            </td>
                                                                            <td colspan="3">
                                                                                <asp:Label ID="lbl_CurrCurrDt_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, TitleCurrency %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                <asp:Label ID="lbl_CurrCurrDt" runat="server" SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td colspan="4">
                                                                                <asp:Label ID="lbl_BaseCurrDt_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, TitleBaseCurrency %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                <asp:Label ID="lbl_BaseCurrDt" runat="server" SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lbl_ConvRate_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_ConvRate_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td align="right">
                                                                                <asp:Label ID="lbl_ConvRate" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                            </td>
                                                                            <td style="padding-left: 10px;">
                                                                            </td>
                                                                            <td colspan="2">
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_CurrNetAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_NetAmt_Nm %>"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="right">
                                                                                <asp:Label ID="lbl_CurrNetAmt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" style="width: 5%;">
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" width="14%">
                                                                                <asp:Label ID="lbl_NetAmt_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_NetAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" width="11%" align="right">
                                                                                <asp:Label ID="lbl_NetAmt" runat="server" SkinID="LBL_NR_1" Text="0.00"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" width="4%" align="center">
                                                                                <asp:Label ID="lbl_NetAC_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_AC_Nm %>"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" width="9%">
                                                                                <asp:Label ID="lbl_NetAC" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lbl_BaseQty_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_BaseQty_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td align="right">
                                                                                <asp:Label ID="lbl_BaseQty" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                            </td>
                                                                            <td style="padding-left: 10px;">
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td colspan="2">
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lbl_CurrDiscAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_DiscAmt_Nm %>"></asp:Label>
                                                                            </td>
                                                                            <td align="right">
                                                                                <asp:Label ID="lbl_CurrDiscAmt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lbl_DiscAmt_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_DiscAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td align="right">
                                                                                <asp:Label ID="lbl_DiscAmt" runat="server" SkinID="LBL_NR_1" Text="0.00"></asp:Label>
                                                                            </td>
                                                                            <td colspan="2">
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2" valign="middle">
                                                                                <asp:CheckBox ID="chk_DiscAdj" runat="server" Enabled="false" SkinID="CHK_V1" Text="<%$ Resources:PO_Default, chk_TexAdj %>" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lbl_Disc_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Disc_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lbl_Disc" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                            </td>
                                                                            <td colspan="2">
                                                                            </td>
                                                                            <td class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_CurrTaxAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_TaxAmt_Nm %>"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="right">
                                                                                <asp:Label ID="lbl_CurrTaxAmt" runat="server"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD">
                                                                            </td>
                                                                            <td class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_TaxAmt_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_TaxAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="right">
                                                                                <asp:Label ID="lbl_TaxAmt" runat="server" SkinID="LBL_NR_1" Text="0.00"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="center">
                                                                                <asp:Label ID="lbl_TaxAC_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_AC_Nm %>"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_TaxAC" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <asp:CheckBox ID="chk_TaxAdj" runat="server" Enabled="false" Text="<%$ Resources:PO_Default, chk_TexAdj %>" SkinID="CHK_V1" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lbl_TaxType_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_TaxType_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lbl_TaxType" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lbl_CurrTotalAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_TotalAmtDt_Nm %>"></asp:Label>
                                                                            </td>
                                                                            <td align="right">
                                                                                <asp:Label ID="lbl_CurrTotalAmt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lbl_Total_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_TotalAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td align="right">
                                                                                <asp:Label ID="lbl_TotalAmt" runat="server" SkinID="LBL_NR_1" Text="0.00"></asp:Label>
                                                                            </td>
                                                                            <td colspan="2">
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2">
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lbl_TaxRate_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_TaxRate_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lbl_TaxRate" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                            </td>
                                                                            <td colspan="3">
                                                                            </td>
                                                                            <td colspan="4">
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <br />
                                                        <%-- Stock Summary and Pr detail --%>
                                                        <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                            <tr style="height: 17px; vertical-align: top">
                                                                <td width="8%">
                                                                    <asp:Label ID="lbl_OnHand_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_OnHand_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                </td>
                                                                <td width="12%">
                                                                    <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                                <td width="8%">
                                                                    <asp:Label ID="lbl_OnOrder_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_OnOrder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                </td>
                                                                <td width="5%">
                                                                    <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                                <td width="4%">
                                                                    <asp:Label ID="lbl_Reorder_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Reorder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                </td>
                                                                <td width="5%">
                                                                    <asp:Label ID="lbl_Reorder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                                <td width="5%">
                                                                    <asp:Label ID="lbl_Restock_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Restock_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                </td>
                                                                <td width="5%">
                                                                    <asp:Label ID="lbl_Restock" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                                <td width="7%">
                                                                    <asp:Label ID="lbl_LastPrice_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_LastPrice_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                </td>
                                                                <td width="9%">
                                                                    <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                                <td width="7%">
                                                                    <asp:Label ID="lbl_LastVendor_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_LastVendor_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                </td>
                                                                <td width="15%">
                                                                    <div style="white-space: nowrap; overflow: hidden; width: 98%">
                                                                        <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <asp:Panel ID="pnl_PROld" runat="server" Visible="false">
                                                            <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                <tr style="height: 17px; vertical-align: top">
                                                                    <td width="8%">
                                                                        <asp:Label ID="lbl_Buyer_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_Buyer_Nm %>"></asp:Label>
                                                                    </td>
                                                                    <td width="12%">
                                                                        <asp:Label ID="lbl_Buyer" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                    </td>
                                                                    <td width="8%">
                                                                        <asp:Label ID="lbl_QtyReq_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_QtyReq_Nm %>"></asp:Label>
                                                                    </td>
                                                                    <td width="5%">
                                                                        <asp:Label ID="lbl_QtyReq" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                    </td>
                                                                    <td width="4%">
                                                                        <asp:Label ID="lbl_Approve_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_Approve_Nm %>"></asp:Label>
                                                                    </td>
                                                                    <td width="5%">
                                                                        <asp:Label ID="lbl_Approve" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                    </td>
                                                                    <td width="5%">
                                                                        <asp:Label ID="lbl_PricePR_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_PricePR_Nm %>"></asp:Label>
                                                                    </td>
                                                                    <td width="5%">
                                                                        <asp:Label ID="lbl_PricePR" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                    </td>
                                                                    <td width="7%">
                                                                        <asp:Label ID="lbl_PRRef_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_PRRef_Nm %>"></asp:Label>
                                                                    </td>
                                                                    <td width="9%">
                                                                        <%-- <asp:Label ID="lbl_PRRef" runat="server" SkinID="LBL_NR_1"></asp:Label>--%>
                                                                        <asp:HyperLink ID="hpl_PRRef" runat="server" SkinID="HYPL_V1"></asp:HyperLink>
                                                                    </td>
                                                                    <td width="7%">
                                                                        <asp:Label ID="lbl_PRDate_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_PRDate_Nm %>"></asp:Label>
                                                                    </td>
                                                                    <td width="15%">
                                                                        <asp:Label ID="lbl_PRDate" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr style="height: 17px; vertical-align: top">
                                                                    <td>
                                                                        <asp:Label ID="lbl_BU_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_BU_Nm %>"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lbl_BU" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lbl_Store_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_Store_Nm %>"></asp:Label>
                                                                    </td>
                                                                    <td colspan="9">
                                                                        <asp:Label ID="lbl_Store" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                        <asp:Panel ID="pnl_PR" runat="server" Visible="true">
                                                            <asp:GridView ID="grd_PR" runat="server" AutoGenerateColumns="False" Width="100%" SkinID="GRD_V1" EnableModelValidation="True" OnRowDataBound="grd_PR_RowDataBound">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <asp:Label ID="lbl_BU_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PO_Default, lbl_BU_Nm %>"></asp:Label>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_BU" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="left" />
                                                                        <ItemStyle HorizontalAlign="left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <asp:Label ID="lbl_PRRef_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PO_Default, lbl_PRRef_Nm %>">
                                                                            </asp:Label>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <%--<asp:Label ID="lbl_PRRef" runat="server" SkinID="LBL_NR_1"></asp:Label>--%>
                                                                            <asp:HyperLink ID="hpl_PRRef" runat="server" SkinID="HYPL_V1"></asp:HyperLink>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="left" />
                                                                        <ItemStyle HorizontalAlign="left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <asp:Label ID="lbl_PRDate_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PO_Default, lbl_PRDate_Nm %>"></asp:Label>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_PRDate" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="left" />
                                                                        <ItemStyle HorizontalAlign="left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <asp:Label ID="lbl_Store_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PO_Default, lbl_Store_Nm %>"></asp:Label>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_Store" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="left" />
                                                                        <ItemStyle HorizontalAlign="left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <asp:Label ID="lbl_Buyer_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PO_Default, lbl_Buyer_Nm %>"></asp:Label>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_Buyer" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="left" />
                                                                        <ItemStyle HorizontalAlign="left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <asp:Label ID="lbl_QtyReq_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PO_Default, lbl_QtyReq_Nm %>"></asp:Label>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_QtyReq" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="left" />
                                                                        <ItemStyle HorizontalAlign="left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <asp:Label ID="lbl_Approve_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PO_Default, lbl_Approve_Nm %>"></asp:Label>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_Approve" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="left" />
                                                                        <ItemStyle HorizontalAlign="left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <asp:Label ID="lbl_PricePR_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PO_Default, lbl_PricePR_Nm %>"></asp:Label>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_PricePR" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="left" />
                                                                        <ItemStyle HorizontalAlign="left" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                        <%--Comment--%>
                                                        <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                                            <tr style="background-color: #DADADA; height: 17px">
                                                                <td>
                                                                    <asp:Label ID="lbl_Comment_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Comment_Nm %>" SkinID="LBL_HD_1"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lbl_Comment" runat="server" SkinID="LBL_NR"></asp:Label>&nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <%--Show Footer--%>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="TABLE_HD" style="background: #B3B2B2;">
                                <tr>
                                    <td rowspan="5" style="width: 3%;">
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lbl_CurrGrandTitle_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, TitleCurrency %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                        <asp:Label ID="lbl_CurrGrandTitle" runat="server" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                    <td rowspan="5" style="width: 5%;">
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lbl_BaseGrandTitle_Nm" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:PC_REC_RecEdit, TitleBaseCurrency %>"></asp:Label>
                                        <asp:Label ID="lbl_BaseGrandTitle" runat="server" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                    <td rowspan="5" style="width: 3%;">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%;">
                                        <asp:Label ID="lbl_CurrTotalPO_Nm" runat="server" SkinID="lbl_HD_White" Text="<%$ Resources:PO_Default, lbl_TotalPO_Nm %>"></asp:Label>
                                    </td>
                                    <td align="right" style="width: 8%;">
                                        <asp:Label ID="lbl_CurrTNet" runat="server" SkinID="lbl_HD_White"></asp:Label>
                                    </td>
                                    <td style="width: 15%">
                                        <asp:Label ID="lbl_TotalPO_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_TotalPO_Nm %>" SkinID="lbl_HD_White"></asp:Label>
                                    </td>
                                    <td align="right" style="width: 10%">
                                        <asp:Label ID="lbl_TNet" runat="server" SkinID="lbl_HD_White" Width="70px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_CurrTotalDisc_Nm" runat="server" SkinID="lbl_HD_White" Text="<%$ Resources:PO_Default, GrandTotalDisc %>"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lbl_CurrTDisc" runat="server" SkinID="lbl_HD_White"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_TDisc_Nm" runat="server" Text="<%$ Resources:PO_Default, GrandTotalDisc %>" SkinID="lbl_HD_White"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lbl_TDisc" runat="server" SkinID="lbl_HD_White" Width="70px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_CurrTotalTax_Nm" runat="server" SkinID="lbl_HD_White" Text="<%$ Resources:PO_Default, lbl_TotalTax_Nm %>"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lbl_CurrTTax" runat="server" SkinID="lbl_HD_White"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_TotalTax_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_TotalTax_Nm %>" SkinID="lbl_HD_White"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lbl_TTax" runat="server" SkinID="lbl_HD_White" Width="70px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_CurrGrandTotal_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_GrandTotal_Nm %>" SkinID="lbl_HD_White"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lbl_CurrTAmount" runat="server" SkinID="lbl_HD_White"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_GrandTotal_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_GrandTotal_Nm %>" SkinID="lbl_HD_White"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lbl_TAmount" runat="server" SkinID="lbl_HD_White" Width="70px"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style="overflow: auto; width: 100%">
                            </div>
                            <%--<asp:ObjectDataSource ID="ods_PriceList" runat="server" SelectMethod="GetList" TypeName="Blue.BL.IN.PriceList"
                                OldValuesParameterFormatString="original_{0}">
                                <SelectParameters>
                                    <asp:SessionParameter Name="ProductCode" SessionField="ProductCode" Type="String" />
                                    <asp:SessionParameter Name="PrDate" SessionField="ReqDate" Type="DateTime" />
                                    <asp:SessionParameter Name="ReqQty" SessionField="ApprQty" Type="Decimal" />
                                    <asp:ControlParameter ControlID="hf_ConnStr" Name="ConnStr" PropertyName="Value"
                                        Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>--%>
                            <asp:HiddenField ID="hf_ConnStr" runat="server" />
                            <asp:HiddenField ID="hd_IndexPo" runat="server" />
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td align="left">
                            <table border="0" cellpadding="1" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Button ID="btn_ProdLog" runat="server" Text="Product Log" Visible="false" SkinID="BTN_V1" OnClick="btn_ProdLog_Click" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_SendMsg" runat="server" Text="Send Message" Visible="false" SkinID="BTN_V1" OnClick="btn_SendMsg_Click" />
                                    </td>
                                    <td>
                                        <asp:Button ID="Button2" runat="server" Text="Receiving Message" Visible="false" SkinID="BTN_V1" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right">
                            <table border="0" cellpadding="1" cellspacing="0">
                                <tr>
                                    <td>
                                        <%-- <asp:Label ID="lbl_BU" runat="server"></asp:Label>
                                                            <asp:HiddenField ID="hf_BuCode" runat="server" />--%>
                                        <asp:Button ID="btn_Approve" runat="server" Text="Close" SkinID="BTN_V1" Visible="false" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_Reject" runat="server" Text="Void" SkinID="BTN_V1" Visible="false" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_SendBack" runat="server" Text="Send Back" SkinID="BTN_V1" Visible="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <dx:ASPxPopupControl ID="pop_ClosePO" runat="server" CloseAction="CloseButton" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        ShowCloseButton="False" HeaderText="Confirm to Close PO?">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                <div>
                    <label>
                        Remark
                    </label>
                </div>
                <br />
                <div>
                    <asp:TextBox ID="txt_pop_ClosePO_Remark" runat="server" Width="460" TextMode="MultiLine" Rows="3" />
                </div>
                <br />
                <br />
                <div style="text-align: right;">
                    <asp:Button ID="btn_YesClosePO" runat="server" Text="<%$ Resources:PO_Default, btn_Confrim %>" SkinID="BTN_V1" OnClick="btn_YesClosePO_Click" />
                    <asp:Button ID="btn_NoClosePO" runat="server" Text="<%$ Resources:PO_Default, btn_Cancel %>" SkinID="BTN_V1" OnClick="btn_NoClosePO_Click"></asp:Button>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_ConfrimDelete" runat="server" CloseAction="CloseButton" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        ShowCloseButton="False" HeaderText="Confirm to void?" AutoUpdatePosition="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                <div>
                    <label>
                        Remark
                    </label>
                </div>
                <br />
                <div>
                    <asp:TextBox ID="txt_pop_ConfirmDelete_Remark" runat="server" Width="460" TextMode="MultiLine" Rows="3" />
                </div>
                <br />
                <%--<div style="text-align: right;">
                    <asp:Label ID="lbl_MsgError1" runat="server" Text="<%$ Resources:PO_Default, lbl_MsgError1 %>"></asp:Label>
                </div>--%>
                <br />
                <div style="text-align: right;">
                    <asp:Button ID="btn_ConfrimDelete" runat="server" Text="<%$ Resources:PO_Default, btn_Confrim %>" SkinID="BTN_V1" OnClick="btn_ConfrimDelete_Click" />
                    <asp:Button ID="btn_CancelDelete" runat="server" Text="<%$ Resources:PO_Default, btn_Cancel %>" SkinID="BTN_V1" OnClick="btn_CancelDelete_Click"></asp:Button>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_NotAllow" runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" HeaderText="" Width="300px"
        CloseAction="CloseButton" Modal="True" ShowCloseButton="False">
        <ContentStyle HorizontalAlign="Center" VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" height="50px">
                            <asp:Label ID="lbl_MsgError2" runat="server" Text="<%$ Resources:PO_Default, lbl_MsgError2 %>"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <%-- <dx:ASPxButton ID="btn_OKNotAllow" runat="server" Text="OK" OnClick="btn_OKNotAllow_Click">
                            </dx:ASPxButton>--%>
                            <asp:Button ID="btn_OKNotAllow" runat="server" Text="<%$ Resources:PO_Default, btn_SuccessOk %>" SkinID="BTN_V1" OnClick="btn_OKNotAllow_Click " />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_ProductLog" runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" HeaderText="History Of Order"
        Width="650px" CloseAction="CloseButton" Modal="True">
        <ContentStyle HorizontalAlign="Center" VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                <table border="0" cellpadding="2" cellspacing="0" width="100%">
                    <tr>
                        <td align="left">
                            <asp:Label ID="HD1" runat="server" Text="Created By" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox1" runat="server" SkinID="TXT_V1" Width="180px" Text="demo@blueledgers.com" Enabled="False"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:Label ID="Label5" runat="server" Text="Created On" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <dx:ASPxDateEdit ID="ASPxDateEdit1" runat="server" Width="100px" Font-Names="Arial,Tahoma,MS Sans serif" Font-Size="8pt" Date="2011-08-22" DisplayFormatString="dd/MM/yyyy"
                                EditFormat="Custom" Enabled="False">
                            </dx:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label9" runat="server" Text="Approved By" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox2" runat="server" SkinID="TXT_V1" Width="180px" Enabled="False"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:Label ID="Label11" runat="server" Text="Approved On" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <dx:ASPxDateEdit ID="ASPxDateEdit2" runat="server" Width="100px" Font-Names="Arial,Tahoma,MS Sans serif" Font-Size="8pt" Enabled="False">
                            </dx:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label12" runat="server" Text="Allocated Buyer" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox3" runat="server" SkinID="TXT_V1" Width="180px" Enabled="False"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:Label ID="Label14" runat="server" Text="Buyer Allocated On" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <dx:ASPxDateEdit ID="ASPxDateEdit3" runat="server" Width="100px" Font-Names="Arial,Tahoma,MS Sans serif" Font-Size="8pt" Enabled="False">
                            </dx:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label15" runat="server" Text="Allocated Vendor" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox4" runat="server" SkinID="TXT_V1" Width="180px" Enabled="False"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:Label ID="Label16" runat="server" Text="Vendor Allocated On" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <dx:ASPxDateEdit ID="ASPxDateEdit4" runat="server" Width="100px" Font-Names="Arial,Tahoma,MS Sans serif" Font-Size="8pt" Enabled="False">
                            </dx:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr style="height: 40px">
                        <td align="left">
                            <asp:Label ID="Label72" runat="server" SkinID="LBL_HD" Text="Vendor Approved By"></asp:Label>
                        </td>
                        <td align="left">
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:Label ID="Label18" runat="server" Text="Vendor Approved On" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label19" runat="server" Text="First Authorized By" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox6" runat="server" SkinID="TXT_V1" Width="180px" Enabled="False"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:Label ID="Label20" runat="server" Text="First Authorized On" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <dx:ASPxDateEdit ID="ASPxDateEdit6" runat="server" Width="100px" Font-Names="Arial,Tahoma,MS Sans serif" Font-Size="8pt" Enabled="False">
                            </dx:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label21" runat="server" Text="Second Authorized By" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox7" runat="server" SkinID="TXT_V1" Width="180px" Enabled="False"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:Label ID="Label23" runat="server" Text="Second Authorized On" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <dx:ASPxDateEdit ID="ASPxDateEdit7" runat="server" Width="100px" Font-Names="Arial,Tahoma,MS Sans serif" Font-Size="8pt" Enabled="False">
                            </dx:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label24" runat="server" Text="Third Authorized By" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox8" runat="server" SkinID="TXT_V1" Width="180px" Enabled="False"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:Label ID="Label25" runat="server" Text="Third Authorized ByOn" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <dx:ASPxDateEdit ID="ASPxDateEdit8" runat="server" Width="100px" Font-Names="Arial,Tahoma,MS Sans serif" Font-Size="8pt" Enabled="False">
                            </dx:ASPxDateEdit>
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_SendMsg" runat="server" HeaderText="Send Message" Width="500px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        CloseAction="CloseButton" Modal="True">
        <ContentStyle HorizontalAlign="Center" VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server" SupportsDisabledAttribute="True">
                <table border="0" cellpadding="3" cellspacing="0" width="100%">
                    <tr>
                        <td align="left" style="width: 44px">
                            <asp:Button ID="btn_To" runat="server" Text="To ..." SkinID="BTN_V1" Width="60px" />
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox10" runat="server" SkinID="TXT_V1" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 44px">
                            <asp:Button ID="btn_CC" runat="server" Text="Cc ..." SkinID="BTN_V1" Width="60px" />
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox9" runat="server" SkinID="TXT_V1" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 44px">
                            <asp:Button ID="Button3" runat="server" Text="Bcc ..." SkinID="BTN_V1" Width="60px" />
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox5" runat="server" SkinID="TXT_V1" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 44px">
                            <asp:Label ID="Label17" runat="server" Text="Subject" SkinID="LBL_HD" Width="60px"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txt_Subject" runat="server" SkinID="TXT_V1" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="Label26" runat="server" Text="Message Text:" SkinID="LBL_HD"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:TextBox ID="txt_Msg" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="2">
                            <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" SkinID="BTN_V1" OnClick="btn_Cancel_Click" />
                            <asp:Button ID="btn_Send" runat="server" Text="Send" SkinID="BTN_V1" OnClick="btn_Send_Click" />
                            <asp:Button ID="btn_Exit" runat="server" Text="Exit" SkinID="BTN_V1" OnClick="btn_Exit_Click" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_SendMail" runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" HeaderText="Send email" Width="480px"
        CloseAction="CloseButton" Modal="True" ShowCloseButton="true" AllowDragging="true">
        <ContentStyle HorizontalAlign="Left" VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl_SendMail" runat="server">
                <table width="100%">
                    <tr>
                        <td style="width: 80px;">
                            <asp:Label ID="lbl_Pop_SendMail_VendorMail_Nm" runat="server" Text="Vendor" />
                        </td>
                        <td>
                            <asp:Label ID="lbl_Pop_SendMail_VendorMail" runat="server" Text="" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_Pop_SendMail_Email" runat="server" Text="email" />
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Pop_SendMail_Email" runat="server" Text="" Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Button ID="btn_Pop_SendMail_Send" runat="server" Text="Send" OnClick="btn_Pop_SendMail_Send_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lbl_Pop_SendMail_Message" runat="server" ForeColor="Red" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
