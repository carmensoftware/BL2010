<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="Rec.aspx.cs" Inherits="BlueLedger.PL.IN.REC.REC"
    Title="Receiving" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
<%@ Register Src="~/PC/StockMovement.ascx" TagName="StockMovement" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Comment2.ascx" TagName="Comment2" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/Attach2.ascx" TagName="Attach2" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Log2.ascx" TagName="Log2" TagPrefix="uc4" %>
<%@ Register Src="~/PC/StockSummary.ascx" TagName="StockSummary" TagPrefix="uc5" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Src="~/UserControl/TotalSummary.ascx" TagName="TotalSummary" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="server">
    <script type="text/javascript">
        function expandDetailsInGrid(_this) {
            var id = _this.id;
            var imgelem = document.getElementById(_this.id);
            var currowid = id.replace("Img_Btn", "TR_Summmary") //GETTING THE ID OF SUMMARY ROW

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
    <table style="width: 100%;" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td align="left">
                <!-- Title & Command Bar -->
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr style="background-color: #4D4D4D; height: 17px">
                        <td style="padding: 0px 0px 0px 10px;">
                            <table border="0" cellpadding="2" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_Title %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right" style="padding-right: 10px;">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <dx:ASPxButton ID="btn_EditInvoice" runat="server" OnClick="btn_EditInvoice_Click" BackColor="Transparent" Height="16px" Width="38px" ToolTip="Edit" HorizontalAlign="Right">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/edit.png" Repeat="NoRepeat" HorizontalPosition="left" VerticalPosition="center" />
                                            <Border BorderStyle="None" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btn_Edit" runat="server" OnClick="btn_Edit_Click" BackColor="Transparent" Height="16px" Width="38px" ToolTip="Edit" HorizontalAlign="Right">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/edit.png" Repeat="NoRepeat" HorizontalPosition="left" VerticalPosition="center" />
                                            <Border BorderStyle="None" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btn_Void" runat="server" OnClick="btn_Void_Click" BackColor="Transparent" Height="16px" Width="41px" ToolTip="Void" HorizontalAlign="Right">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/void.png" Repeat="NoRepeat" HorizontalPosition="left" VerticalPosition="center" />
                                            <Border BorderStyle="None" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btn_Print" runat="server" OnClick="btn_Print_Click" BackColor="Transparent" Height="16px" Width="43px" ToolTip="Print" AutoPostBack="False"
                                            HorizontalAlign="Right">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat" HorizontalPosition="left" VerticalPosition="center" />
                                            <Border BorderStyle="None" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btn_Back" runat="server" OnClick="btn_Back_Click" BackColor="Transparent" Height="16px" Width="42px" ToolTip="Back" HorizontalAlign="Right">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat" HorizontalPosition="left" VerticalPosition="center" />
                                            <Border BorderStyle="None" />
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <!-- Rcv. Header -->
                <table border="0" cellpadding="0" cellspacing="0" width="100%" class="TABLE_HD">
                    <tr>
                        <td style="width: 7%">
                            <asp:Label ID="lbl_RecNo_Nm" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_RecNo_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td style="width: 13%">
                            <asp:Label ID="lbl_RecNo" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                        <td style="width: 6%">
                            <asp:Label ID="lbl_RecDate_Nm" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_RecDate_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td style="width: 12%">
                            <asp:Label ID="lbl_RecDate" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                        <td style="width: 11%">
                            <asp:Label ID="lbl_Receiver_Nm" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_Receiver_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td style="width: 15%; white-space: nowrap; overflow: hidden">
                            <asp:Label ID="lbl_Receiver" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                        <td style="width: 8%">
                            <asp:Label ID="lbl_DeliPoing_Nm" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_DeliPoing_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td style="width: 15%;">
                            <asp:Label ID="lbl_DeliPoint" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                        <td style="width: 5%">
                            <asp:Label ID="lbl_Status_Nm" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_Status_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td style="width: 7%">
                            <asp:Label ID="lbl_DocStatus" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_InvNo_Nm1" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_InvNo_Nm1 %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_InvDate" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_InvNo_Nm0" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_InvNo_Nm0 %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_InvNo" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_VendorCode_Nm" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_VendorCode_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:Label ID="lbl_VendorCode" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            <asp:Label ID="lbl_VendorNm" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_CommitDate_Nm" runat="server" SkinID="LBL_HD">Committed Date:</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_CommitDate" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_Desc_Nm" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_Desc_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:Label ID="lbl_Desc" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_DeliPoing_Nm0" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_DeliPoing_Nm0 %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chk_IsCashConsign" Enabled="false" />
                        </td>
                        <td>
                            <asp:Label ID="lbl_Currency_Nm" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_Currency_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:Label ID="lbl_Currency" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            <asp:Label ID="lbl_At" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_At %>" SkinID="LBL_NR_BLUE"></asp:Label>
                            <asp:Label ID="lbl_ExRateAudit" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lbl_TotalExtraCost_Nm" runat="server" Text="Extra Cost" SkinID="LBL_HD" />
                        </td>
                        <td colspan="1">
                            <asp:Label ID="lbl_TotalExtraCost" runat="server" SkinID="LBL_NR_BLUE" />
                            &nbsp;
                            <asp:Label ID="lbl_ExtraCostBy" runat="server" SkinID="LBL_NR_BLUE" />
                        </td>
                        <td colspan="8">
                            <asp:Button ID="btn_ExtraCostDetail" runat="server" Text="Detail" Width="100px" OnClick="btn_ExtraCostDetail_Click" />
                        </td>
                    </tr>
                </table>
                <!-- Rcv. Detail -->
                <div style="overflow: auto; width: 100%">
                    <asp:GridView ID="grd_RecEdit" runat="server" Width="100%" SkinID="GRD_V1" AutoGenerateColumns="False" DataKeyNames="RecNo,RecDtNo" EmptyDataText="No Data to Display"
                        EnableModelValidation="True" OnRowCommand="grd_RecEdit_RowCommand" OnRowDataBound="grd_RecEdit_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="#">
                                <HeaderStyle Width="17px" HorizontalAlign="Left" />
                                <ItemStyle Width="17px" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr style="height: 17px">
                                            <td valign="middle">
                                                <%--CommandName="ShowDetail"--%>
                                                <asp:ImageButton ID="Img_Btn" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/Plus.jpg" OnClientClick="expandDetailsInGrid(this);return false;" />
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <table border="0" cellpadding="3" cellspacing="0" width="100%" class="TABLE_HD">
                                        <tr>
                                            <td width="160px" align="left" style="padding-right: 5px; margin: auto;">
                                                <%--300px--%>
                                                <asp:Label ID="lbl_Store_Nm" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_Store_Nm %>" SkinID="LBL_HD_W" Width="160px" />
                                            </td>
                                            <td width="18%" align="left" style="padding-right: 5px; margin: auto;">
                                                <%--300px--%>
                                                <asp:Label ID="lbl_Product_Nm" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_Product_Nm %>" SkinID="LBL_HD_W"></asp:Label><%--Width="300px"--%>
                                            </td>
                                            <td width="7%" align="right" style="padding-right: 5px;">
                                                <%--70px--%>
                                                <asp:Label ID="lbl_Ord_Nm" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_Ord_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td width="5%">
                                                <%--50px--%>
                                                <asp:Label ID="lbl_Unit_Nm" runat="server" Text="" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td width="7%" align="right" style="padding-right: 5px; margin: auto;">
                                                <%--70px--%>
                                                <asp:Label ID="lbl_Rcv_Nm" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_Rcv_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td width="5%">
                                                <%--50px--%>
                                            </td>
                                            <td width="7%" align="right" style="padding-right: 5px; margin: auto;">
                                                <%--70px--%>
                                                <asp:Label ID="lbl_Foc_Nm" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_Foc_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td width="8%" align="right" style="padding-right: 5px; margin: auto;">
                                                <%--70px--%>
                                                <asp:Label ID="lbl_Price_Nm" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_Price_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td width="8%" align="right" style="padding-right: 5px; margin: auto;">
                                                <%--70px--%>
                                                <asp:Label ID="lbl_ExtraCost_Nm" runat="server" Text="Extra Cost" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td width="8%" align="right" style="padding-right: 5px; margin: auto;">
                                                <%--70px--%>
                                                <asp:Label ID="lbl_Total_Nm" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_Total_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td width="9%" align="left" style="margin: auto;">
                                                <%--60px--%>
                                            </td>
                                            <td width="6%" align="left" style="margin: auto;">
                                                <%--50px--%>
                                                <asp:Label ID="lbl_Status_Nm_Grid" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_Status_Nm_Grid %>" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td width="100px">
                                                <asp:Label ID="lbl_ExpiryDate_Nm" runat="server" Text="Expiry Date" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%--<asp:Panel ID="p_Item" runat="server" Width="100%">--%>
                                    <table border="0" cellpadding="3" cellspacing="0" width="100%" class="TABLE_HD">
                                        <tr>
                                            <%--<td width="25%" align="left" style="padding-right: 5px; overflow: hidden; margin: auto; white-space: nowrap;">--%>
                                            <td width="160px" align="left" style="padding-right: 5px;">
                                                <%--  width="300px"--%>
                                                <asp:Label ID="lbl_Location" runat="server" SkinID="LBL_NR" Width="160px" />
                                            </td>
                                            <td width="18%" align="left" style="padding-right: 5px; overflow: hidden; margin: auto; white-space: nowrap;">
                                                <%--  width="300px"--%>
                                                <asp:Label ID="lbl_ProductCode" runat="server" Width="150px" SkinID="LBL_NR">                                                                
                                                </asp:Label>
                                            </td>
                                            <td width="7%" align="right" style="padding-right: 5px; margin: auto;">
                                                <%-- width="70px"--%>
                                                <asp:Label ID="lbl_OrderQty" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td width="5%" align="left" style="margin: auto;">
                                                <%-- width="50px"--%>
                                                <asp:Label ID="lbl_Unit" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td width="7%" align="right" style="padding-right: 5px; margin: auto;">
                                                <%-- width="70px"--%>
                                                <asp:Label ID="lbl_RecQty" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td width="5%" align="left" style="margin: auto;">
                                                <%-- width="50px"--%>
                                                <asp:Label ID="lbl_RcvUnit" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td width="7%" align="right" style="padding-right: 5px; margin: auto;">
                                                <%-- width="70px"--%>
                                                <asp:Label ID="lbl_FocQty" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td width="8%" align="right" style="padding-right: 5px; margin: auto;">
                                                <%-- width="70px"--%>
                                                <asp:Label ID="lbl_Price" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td width="8%" align="right" style="padding-right: 5px; margin: auto;">
                                                <%-- width="70px"--%>
                                                <asp:Label ID="lbl_ExtraCost" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td width="8%" align="right" style="padding-right: 5px; margin: auto;">
                                                <%-- width="70px"--%>
                                                <asp:Label ID="lbl_TotalAmt" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td width="9%" align="left" style="margin: auto;">
                                                <%-- width="60px"--%>
                                                <asp:Label ID="lbl_TaxType_Row" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td width="6%" style="margin: auto;">
                                                <%-- width="50px"--%>
                                                <asp:Label ID="lbl_Status" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td width="100px">
                                                <asp:Label ID="lbl_ExpiryDate" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <%--</asp:Panel>--%>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <table border="0" cellpadding="3" cellspacing="0" width="100%" class="TABLE_HD">
                                        <tr>
                                            <td colspan="3">
                                                <asp:Label ID="lbl_CurrGrandTitle_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, TitleCurrency %>" SkinID="LBL_HD_W"></asp:Label>
                                                <asp:Label ID="lbl_CurrGrandTitle" runat="server" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:Label ID="lbl_BaseGrandTitle_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, TitleBaseCurrency %>" SkinID="LBL_HD_W"></asp:Label>
                                                <asp:Label ID="lbl_BaseGrandTitle" runat="server" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 15%; white-space: nowrap;">
                                                <asp:Label ID="lbl_CurrGrandTotalNet_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_TotalRec_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td width="8%" align="right">
                                                <asp:Label ID="lbl_CurrGrandTotalNet" runat="server" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td width="7%">
                                            </td>
                                            <td align="left" style="width: 15%; white-space: nowrap; margin: auto;">
                                                <asp:Label ID="lbl_TotalRec_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_TotalRec_Nm %>" SkinID="LBL_HD_W"></asp:Label><%--Width="100px"--%>
                                            </td>
                                            <td width="8%" align="right" style="margin: auto; padding-right: 5px;">
                                                <%--75px--%>
                                                <asp:Label ID="lbl_TotalRec" runat="server" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td width="3%" style="margin: auto;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="lbl_CurrGrandTotalTax_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_TotalTax_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="lbl_CurrGrandTotalTax" runat="server" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td align="left" style="white-space: nowrap; margin: auto;">
                                                <%--width: 100px;--%>
                                                <asp:Label ID="lbl_TotalTax_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_TotalTax_Nm %>" SkinID="LBL_HD_W"></asp:Label><%-- Width="100px"--%>
                                            </td>
                                            <td align="right" style="margin: auto; padding-right: 5px;">
                                                <asp:Label ID="lbl_TotalTax" runat="server" SkinID="LBL_HD_W"></asp:Label><%-- Width="75px"--%>
                                            </td>
                                            <td style="margin: auto;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="padding-right: 5px; white-space: nowrap;">
                                                <asp:Label ID="lbl_CurrGrandTotalAmt_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_GrandTotal %>" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="lbl_CurrGrandTotalAmt" runat="server" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td align="left" style="white-space: nowrap; margin: auto;">
                                                <%-- width="100px"--%>
                                                <asp:Label ID="lbl_GrandTotal" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_GrandTotal %>" SkinID="LBL_HD_W"></asp:Label><%-- Width="100px"--%>
                                            </td>
                                            <td align="right" style="margin: auto; padding-right: 5px;">
                                                <asp:Label ID="lbl_TotalAmt" runat="server" SkinID="LBL_HD_W"></asp:Label><%--Width="75px"--%>
                                            </td>
                                            <td style="margin: auto;">
                                            </td>
                                        </tr>
                                    </table>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Width="0%">
                                <ItemTemplate>
                                    <%--style="display: none"--%>
                                    <tr id="TR_Summmary" runat="server" style="display: none;">
                                        <td colspan="5" style="padding-left: 10px; padding-right: 0px">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="TABLE_HD">
                                                <tr>
                                                    <td style="vertical-align: top; width: 100%">
                                                        <%--Transaction Details--%>
                                                        <table border="0" cellpadding="3" cellspacing="0" class="TABLE_HD" width="100%">
                                                            <tr>
                                                                <td style="width: 6%; margin: auto;">
                                                                    <asp:Label ID="lbl_Receive_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_Rec, lbl_Receive_Nm %>"></asp:Label>
                                                                </td>
                                                                <td align="right" style="width: 6%; margin: auto;">
                                                                    <asp:Label ID="lbl_Receive" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                                <td style="width: 5%; margin: auto;">
                                                                    <asp:Label ID="lbl_RcvUnit_Expand" runat="server" SkinID="LBL_NR_1"></asp:Label>
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
                                                                    <asp:Label ID="lbl_ConvertRate_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_ConvertRate_Nm %>"></asp:Label>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Label ID="lbl_ConvertRate" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                                <td style="padding-left: 10px;">
                                                                </td>
                                                                <td colspan="2">
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lbl_CurrNetAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_NetAmt_Nm %>"></asp:Label>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Label ID="lbl_CurrNetAmt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                                <td style="width: 5%;">
                                                                </td>
                                                                <td style="width: 20%;">
                                                                    <asp:Label ID="lbl_NetAmtDt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_Rec, lbl_NetAmtDt_Nm %>"></asp:Label>
                                                                </td>
                                                                <td align="right" style="width: 10%">
                                                                    <asp:Label ID="lbl_NetAmtDt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                                <%--<td align="center" style="width: 7%;">
                                                                                <asp:Label ID="lbl_NetAcc_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_Rec, lbl_NetAcc_Nm %>"></asp:Label>
                                                                            </td>
                                                                            <td style="width: 10%;">
                                                                                <asp:Label ID="lbl_NetAcc" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>--%>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lbl_BaseQty_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_BaseQty_Nm %>"></asp:Label>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Label ID="lbl_BaseQty" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                                <td style="padding-left: 10px;">
                                                                    <asp:Label ID="lbl_BaseUnit" runat="server" SkinID="LBL_NR_1"></asp:Label>
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
                                                                    <asp:Label ID="lbl_DiscAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_Rec, lbl_DiscAmt_Nm %>"></asp:Label>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Label ID="lbl_DiscAmt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                                <td colspan="2">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:CheckBox ID="chk_DiscAdj" runat="server" SkinID="CHK_V1" Text=" Adj. Discount" Enabled="false" />
                                                                </td>
                                                                <td style="width: 8%;">
                                                                    <asp:Label ID="lbl_Disc_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_Rec, lbl_Disc_Nm %>"></asp:Label>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Label ID="lbl_Disc" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                    <asp:Label ID="lbl_DiscPercent" runat="server" SkinID="LBL_NR_1" Text="<%$ Resources:PC_REC_RecEdit, lbl_Percent %>"></asp:Label>
                                                                </td>
                                                                <td colspan="2">
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lbl_CurrTaxAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_TaxAmt_Nm %>"></asp:Label>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Label ID="lbl_CurrTaxAmt" runat="server"></asp:Label>
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lbl_TaxAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_Rec, lbl_TaxAmt_Nm %>"></asp:Label>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Label ID="lbl_TaxAmtDt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                                <%--<td align="center">
                                                                                <asp:Label ID="lbl_TaxAcc_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_Rec, lbl_TaxAcc_Nm %>"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lbl_TaxAcc" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>--%>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:CheckBox ID="chk_TaxAdj" runat="server" SkinID="CHK_V1" Text=" Adj. Tax" Enabled="false" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lbl_TaxType_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_Rec, lbl_TaxType_Nm %>"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lbl_TaxType" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                                <td colspan="2">
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lbl_CurrTotalAmtDt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_TotalAmtDt_Nm %>"></asp:Label>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Label ID="lbl_CurrTotalAmtDt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lbl_TotalAmtDt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_Rec, lbl_TotalAmtDt_Nm %>"></asp:Label>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Label ID="lbl_TotalAmtDt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                                <td colspan="2">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lbl_TaxRate_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_Rec, lbl_TaxRate_Nm %>"></asp:Label>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Label ID="lbl_TaxRate" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                    <asp:Label ID="lbl_Percent" runat="server" SkinID="LBL_NR_1" Text="<%$ Resources:PC_REC_RecEdit, lbl_Percent %>"></asp:Label>
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>
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
                                            <table border="0" cellpadding="3" cellspacing="0" class="TABLE_HD" width="100%">
                                                <tr>
                                                    <td style="width: 6%; margin: auto;" class="TD_LINE_GRD">
                                                        <asp:Label ID="lbl_OnHand_Nm" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_OnHand_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right" style="width: 6%; margin: auto;" class="TD_LINE_GRD">
                                                        <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td style="width: 6%; margin: auto;" class="TD_LINE_GRD">
                                                        <asp:Label ID="lbl_OnOrder_Nm" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_OnOrder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right" style="width: 6%; margin: auto; padding-left: 5px;" class="TD_LINE_GRD">
                                                        <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td style="width: 6%; margin: auto;" class="TD_LINE_GRD">
                                                        <asp:Label ID="lbl_Reorder_Nm" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_Reorder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right" style="width: 6%; margin: auto;" class="TD_LINE_GRD">
                                                        <asp:Label ID="lbl_Reorder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td style="width: 6%; padding-left: 5px; margin: auto;" class="TD_LINE_GRD">
                                                        <asp:Label ID="lbl_Restock_Nm" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_Restock_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right" style="width: 6%; margin: auto;" class="TD_LINE_GRD">
                                                        <asp:Label ID="lbl_Restock" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td style="width: 6%; white-space: nowrap; margin: auto;" class="TD_LINE_GRD">
                                                        <asp:Label ID="lbl_LastPrice_Nm" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_LastPrice_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right" style="width: 6%; margin: auto;" class="TD_LINE_GRD">
                                                        <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td style="width: 10%; padding-left: 5px; margin: auto;" class="TD_LINE_GRD">
                                                        <asp:Label ID="lbl_LastVendor_Nm" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_LastVendor_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td style="width: 30%; margin: auto; overflow: hidden;" class="TD_LINE_GRD">
                                                        <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1" Width="200px"></asp:Label>
                                                    </td>
                                                    <%-- Width="300px" white-space: nowrap;--%>
                                                </tr>
                                                <tr>
                                                    <td class="TD_LINE_GRD">
                                                        <asp:Label ID="lbl_PoRef_Nm" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_PoRef_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td class="TD_LINE_GRD" colspan="3">
                                                        <asp:HyperLink ID="hpl_PoRef" runat="server" SkinID="HYPL_V1"></asp:HyperLink>
                                                    </td>
                                                    <td class="TD_LINE_GRD">
                                                        <asp:Label ID="lbl_PrRef_Nm" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_PrRef_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td class="TD_LINE_GRD" colspan="3">
                                                        <asp:Label ID="lbl_PrRef" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td class="TD_LINE_GRD" colspan="4">
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:Panel ID="pnl_PR" runat="server" Visible="true" Width="100%">
                                                <asp:GridView ID="grd_PR" runat="server" AutoGenerateColumns="False" Width="100%" SkinID="GRD_V1" EnableModelValidation="True" OnRowDataBound="grd_PR_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lbl_BU_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PC_REC_Rec, lbl_BU_Nm %>"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_BU" runat="server" SkinID="LBL_NR"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="left" />
                                                            <ItemStyle HorizontalAlign="left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lbl_PRRef_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PC_REC_Rec, lbl_PRRef_Nm %>">
                                                                </asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="hpl_PRRef" runat="server" SkinID="HYPL_V1"></asp:HyperLink>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="left" />
                                                            <ItemStyle HorizontalAlign="left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lbl_PRDate_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PC_REC_Rec, lbl_PRDate_Nm %>"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_PRDate" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="left" />
                                                            <ItemStyle HorizontalAlign="left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lbl_Store_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PC_REC_Rec, lbl_Store_Nm %>"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_Store" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="left" />
                                                            <ItemStyle HorizontalAlign="left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lbl_Buyer_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PC_REC_Rec, lbl_Buyer_Nm %>"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_Buyer" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="left" />
                                                            <ItemStyle HorizontalAlign="left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lbl_QtyReq_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PC_REC_Rec, lbl_QtyReq_Nm %>"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_QtyReq" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="left" />
                                                            <ItemStyle HorizontalAlign="left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lbl_Approve_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PC_REC_Rec, lbl_Approve_Nm %>"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_Approve" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="left" />
                                                            <ItemStyle HorizontalAlign="left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lbl_PricePR_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PC_REC_Rec, lbl_PricePR_Nm %>"></asp:Label>
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
                                            <table class="TABLE_HD" width="100%">
                                                <tr style="background-color: #DADADA; height: 17px; margin: auto;">
                                                    <td>
                                                        <asp:Label ID="lbl_DtrComment_Nm" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_DtrComment_Nm %>" SkinID="LBL_HD_1"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_DtrComment" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="TABLE_HD">
                                                <tr>
                                                    <td style="margin: auto;">
                                                        <uc1:StockMovement ID="uc_StockMovement" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <HeaderStyle Width="0%"></HeaderStyle>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <%--Total Summary --%>
                    <div style="display: flex; justify-content: flex-end;">
                        <uc:TotalSummary runat="server" ID="TotalSummary" />
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td align="left">
                <table border="0" cellpadding="1" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Button ID="btn_ProdLog" runat="server" Text="<%$ Resources:PC_REC_Rec, btn_ProdLog %>" Visible="false" SkinID="BTN_V1" OnClick="btn_ProdLog_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btn_SendMsg" runat="server" Text="<%$ Resources:PC_REC_Rec, btn_SendMsg %>" Visible="false" SkinID="BTN_V1" OnClick="btn_SendMsg_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btn_RecMsg" runat="server" Text="<%$ Resources:PC_REC_Rec, btn_RecMsg %>" Visible="false" SkinID="BTN_V1" />
                        </td>
                    </tr>
                </table>
                <br />
            </td>
        </tr>
    </table>
    <dx:ASPxPopupControl ID="pop_ConfrimVoid" ClientInstanceName="pop_ConfrimVoid" runat="server" Width="300px" CloseAction="CloseButton" HeaderText="<%$ Resources:PC_REC_Rec, Confirm %>"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" colspan="2" height="50px">
                            <asp:Label ID="lbl_ConfirmVoid" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_ConfirmVoid %>" SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Button ID="btn_ConfrimVoid" runat="server" Text="<%$ Resources:PC_REC_Rec, btn_ConfrimVoid %>" SkinID="BTN_V1" Width="50px" OnClick="btn_ConfrimVoid_Click" />
                        </td>
                        <td align="left">
                            <asp:Button ID="btn_CancelDelete" runat="server" Text="<%$ Resources:PC_REC_Rec, btn_CancelDelete %>" SkinID="BTN_V1" Width="50px" OnClick="btn_CancelDelete_Click" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_Warning" ClientInstanceName="pop_Warning" runat="server" Width="300px" CloseAction="CloseButton" HeaderText="<%$ Resources:PC_REC_Rec, Warning %>"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" height="50px">
                            <asp:Label ID="lbl_Warning" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_Warning %>" SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btn_Warning" runat="server" Text="<%$ Resources:PC_REC_Rec, btn_Warning %>" Width="50px" OnClick="btn_Warning_Click" SkinID="BTN_V1" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_ProductLog" runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" HeaderText="<%$ Resources:PC_REC_Rec, HistoryOfOrder %>"
        Width="650px" CloseAction="CloseButton" Modal="True">
        <ContentStyle HorizontalAlign="Center" VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <table border="0" cellpadding="2" cellspacing="0" width="100%">
                    <tr>
                        <td align="left">
                            <asp:Label ID="HD1" runat="server" Text="<%$ Resources:PC_REC_Rec, HD1 %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox1" runat="server" SkinID="TXT_V1" Width="180px" Text="demo@blueledgers.com" Enabled="False"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_CreatedOn" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_CreatedOn %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <dx:ASPxDateEdit ID="ASPxDateEdit1" runat="server" Width="100px" Font-Names="Arial,Tahoma,MS Sans serif" Font-Size="8pt" Date="2011-08-22" DisplayFormatString="dd/MM/yyyy"
                                EditFormat="Custom" Enabled="False">
                            </dx:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lbl_ApprBy" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_ApprBy %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox2" runat="server" SkinID="TXT_V1" Width="180px" Enabled="False"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_ApprOn" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_ApprOn %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <dx:ASPxDateEdit ID="ASPxDateEdit2" runat="server" Width="100px" Font-Names="Arial,Tahoma,MS Sans serif" Font-Size="8pt" Enabled="False">
                            </dx:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lbl_AllocatedBuyer" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_AllocatedBuyer %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox3" runat="server" SkinID="TXT_V1" Width="180px" Enabled="False"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_BuyerAllocatedOn" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_BuyerAllocatedOn %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <dx:ASPxDateEdit ID="ASPxDateEdit3" runat="server" Width="100px" Font-Names="Arial,Tahoma,MS Sans serif" Font-Size="8pt" Enabled="False">
                            </dx:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lbl_AllocatedVendor" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_AllocatedVendor %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox4" runat="server" SkinID="TXT_V1" Width="180px" Enabled="False"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_VendorAllocatedOn" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_VendorAllocatedOn %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <dx:ASPxDateEdit ID="ASPxDateEdit4" runat="server" Width="100px" Font-Names="Arial,Tahoma,MS Sans serif" Font-Size="8pt" Enabled="False">
                            </dx:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr style="height: 40px">
                        <td align="left">
                            <asp:Label ID="lbl_VendorApprBy" runat="server" SkinID="LBL_HD" Text="<%$ Resources:PC_REC_Rec, lbl_VendorApprBy %>"></asp:Label>
                        </td>
                        <td align="left">
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_VendorApprOn" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_VendorApprOn %>" SkinID="LBL_HD"></asp:Label>
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
                            <asp:Label ID="lbl_FirstAuthoBy" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_FirstAuthoBy %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox6" runat="server" SkinID="TXT_V1" Width="180px" Enabled="False"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_FirstAuthoOn" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_FirstAuthoOn %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <dx:ASPxDateEdit ID="ASPxDateEdit6" runat="server" Width="100px" Font-Names="Arial,Tahoma,MS Sans serif" Font-Size="8pt" Enabled="False">
                            </dx:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lbl_SecondAuthoBy" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_SecondAuthoBy %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox7" runat="server" SkinID="TXT_V1" Width="180px" Enabled="False"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_SecondAuthoOn" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_SecondAuthoOn %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <dx:ASPxDateEdit ID="ASPxDateEdit7" runat="server" Width="100px" Font-Names="Arial,Tahoma,MS Sans serif" Font-Size="8pt" Enabled="False">
                            </dx:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lbl_ThirdAuthoBy" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_ThirdAuthoBy %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox8" runat="server" SkinID="TXT_V1" Width="180px" Enabled="False"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_ThirdAuthoByOn" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_ThirdAuthoByOn %>" SkinID="LBL_HD"></asp:Label>
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
            <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server" SupportsDisabledAttribute="True">
                <table border="0" cellpadding="3" cellspacing="0" width="100%">
                    <tr>
                        <td align="left" style="width: 44px">
                            <asp:Button ID="btn_To" runat="server" Text="<%$ Resources:PC_REC_Rec, btn_To %>" SkinID="BTN_V1" Width="60px" />
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox10" runat="server" SkinID="TXT_V1" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 44px">
                            <asp:Button ID="btn_CC" runat="server" Text="<%$ Resources:PC_REC_Rec, btn_CC %>" SkinID="BTN_V1" Width="60px" />
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox9" runat="server" SkinID="TXT_V1" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 44px">
                            <asp:Button ID="btn_Bcc" runat="server" Text="<%$ Resources:PC_REC_Rec, btn_Bcc %>" SkinID="BTN_V1" Width="60px" />
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox5" runat="server" SkinID="TXT_V1" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 44px">
                            <asp:Label ID="lbl_Subject" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_Subject %>" SkinID="LBL_HD" Width="60px"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txt_Subject" runat="server" SkinID="TXT_V1" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lbl_MsgText" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_MsgText %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:TextBox ID="txt_Msg" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="2">
                            <asp:Button ID="btn_Cancel" runat="server" Text="<%$ Resources:PC_REC_Rec, btn_Cancel %>" SkinID="BTN_V1" OnClick="btn_Cancel_Click" />
                            <asp:Button ID="btn_Send" runat="server" Text="<%$ Resources:PC_REC_Rec, btn_Send %>" SkinID="BTN_V1" OnClick="btn_Send_Click" />
                            <asp:Button ID="btn_Exit" runat="server" Text="<%$ Resources:PC_REC_Rec, btn_Exit %>" SkinID="BTN_V1" OnClick="btn_Exit_Click" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_EditInvoice" runat="server" HeaderText="Invoice" Width="320px" CloseAction="CloseButton" Modal="True" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" AutoUpdatePosition="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server" SupportsDisabledAttribute="True">
                <div style="width: 100%;">
                    <div style="display: inline-block; width: 100px; vertical-align: top; padding-top: 2px;">
                        <asp:Label ID="lbl_InvNoEdit" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_InvNo_Nm1 %>" SkinID="LBL_HD"></asp:Label>
                    </div>
                    <div style="display: inline-block;">
                        <dx:ASPxDateEdit ID="de_InvDateEdit" runat="server" ShowShadow="False" Width="120px" DisplayFormatString="dd/MM/yyyy" EditFormat="Custom" EditFormatString="dd/MM/yyyy">
                            <ValidationSettings Display="Dynamic">
                                <ErrorFrameStyle>
                                    <ErrorTextPaddings PaddingLeft="4px" />
                                </ErrorFrameStyle>
                            </ValidationSettings>
                            <CalendarProperties>
                                <HeaderStyle Spacing="1px" />
                                <FooterStyle Spacing="17px" />
                            </CalendarProperties>
                        </dx:ASPxDateEdit>
                    </div>
                </div>
                <div style="width: 100%;">
                    <div style="display: inline-block; width: 100px;">
                        <asp:Label ID="lbl_InvDateEdit" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_InvNo_Nm0 %>" SkinID="LBL_HD"></asp:Label>
                    </div>
                    <div style="display: inline-block">
                        <asp:TextBox ID="txt_InvNoEdit" runat="server" MaxLength="30" Width="120px" SkinID="TXT_V1"></asp:TextBox>
                    </div>
                </div>
                <hr />
                <div style="width: 100%; text-align: right; padding-top: 10px; padding-right: 10px;">
                    <asp:Button runat="server" ID="btn_pop_EditInvoice_Save" Text="Save" OnClick="btn_pop_EditInvoice_Save_Click" />
                    <asp:Button runat="server" ID="btn_pop_EditInvoice_Cancel" Text="Cancel" OnClick="btn_pop_EditInvoice_Cancel_Click" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_ExtraCostDetail" ClientInstanceName="pop_ExtraCostDetail" runat="server" Width="400px" CloseAction="CloseButton" HeaderText="Extra Cost Detail"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="true" AllowDragging="true" AutoUpdatePosition="true"
        ShowPageScrollbarWhenModal="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl_ExtraCostDetail" runat="server">
                <div>
                    <asp:GridView ID="grd_ExtraCost" runat="server" Width="100%" SkinID="GRD_V1" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField DataField="TypeName" HeaderText="Item" ConvertEmptyStringToNull="true" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="Amount" HeaderText="Amount" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                        </Columns>
                    </asp:GridView>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <asp:HiddenField ID="hf_ConnStr" runat="server" />
</asp:Content>
