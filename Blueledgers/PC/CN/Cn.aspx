<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="Cn.aspx.cs" Inherits="BlueLedger.PL.PC.CN.CN" Title="Credit Note" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
<%@ Register Src="~/UserControl/Comment2.ascx" TagName="Comment2" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Attach2.ascx" TagName="Attach2" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/Log2.ascx" TagName="Log2" TagPrefix="uc3" %>
<%@ Register Src="~/PC/StockSummary.ascx" TagName="StockSummary" TagPrefix="uc4" %>
<%@ Register Src="../StockMovement.ascx" TagName="StockMovement" TagPrefix="uc5" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Src="~/UserControl/TotalSummary.ascx" TagName="TotalSummary" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <script type="text/javascript">
        function expandDetailsInGrid(_this) {
            var id = _this.id;
            var imgelem = document.getElementById(_this.id);
            var currowid = id.replace("Img_Btn", "TR_Summmary")

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
    <style type="text/css">
        .GridViewHeader
        {
            color: Blue;
            border-top-style: solid;
            border-top-width: 1px;
            border-bottom-style: solid;
            border-bottom-width: 1px;
            height: 25px;
        }
        .GridViewRow
        {
            height: 20px;
        }
        .GridViewFooter
        {
            height: 0px;
            border-top-style: solid;
            border-top-width: 1px;
            text-align: right;
        }
    </style>
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
                            <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_Title %>" SkinID="LBL_HD_WHITE"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
            <td align="right" style="padding-right: 10px;">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <dx:ASPxButton ID="btn_Edit" runat="server" OnClick="btn_Edit_Click" BackColor="Transparent" Height="16px" Width="38px" ToolTip="Edit" HorizontalAlign="Right">
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/edit.png" Repeat="NoRepeat" HorizontalPosition="left" VerticalPosition="center" />
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="left" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-edit.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <Border BorderStyle="None" />
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="btn_Void" runat="server" OnClick="btn_Void_Click" BackColor="Transparent" Height="16px" Width="41px" ToolTip="Void" HorizontalAlign="Right">
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/void.png" Repeat="NoRepeat" HorizontalPosition="left" VerticalPosition="center" />
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="left" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-void.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <Border BorderStyle="None" />
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="btn_Print" runat="server" BackColor="Transparent" Height="16px" Width="43px" ToolTip="Print" OnClick="btn_Print_Click" HorizontalAlign="Right">
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat" HorizontalPosition="left" VerticalPosition="center" />
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="left" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-print.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <Border BorderStyle="None" />
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="btn_Back" runat="server" OnClick="btn_Back_Click" BackColor="Transparent" Height="16px" Width="42px" ToolTip="Back" HorizontalAlign="Right">
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat" HorizontalPosition="left" VerticalPosition="center" />
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="left" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <Border BorderStyle="None" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <!-- Credit Note Header -->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="TABLE_HD">
        <tr>
            <td rowspan="3" style="width: 1%;">
            </td>
            <td width="6%">
                <asp:Label ID="lbl_CnNo_Nm" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_CnNo_Nm %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td width="18%">
                <asp:Label ID="lbl_CnNo" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
            </td>
            <td width="3%">
                <asp:Label ID="lbl_CnDate_Nm" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_CnDate_Nm %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td width="16%">
                <asp:Label ID="lbl_CnDate" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
            </td>
            <td width="5%">
                <asp:Label ID="lbl_InvNo_Nm" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_InvNo_Nm %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td width="15%">
                <asp:Label ID="lbl_DocNo" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
            </td>
            <td width="5%">
                <asp:Label ID="lbl_InvDate_Nm" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_InvDate_Nm %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td width="16%">
                <asp:Label ID="lbl_DocDate" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lbl_VendorCode_Nm" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_VendorCode_Nm %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td colspan="3">
                <asp:Label ID="lbl_VendorCode" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                <asp:Label ID="lbl_Colon" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_Colon %>" SkinID="LBL_NR_BLUE" Visible="false"></asp:Label>
                <asp:Label ID="lbl_VendorNm" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lbl_Currency_Nm" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_Currency_Nm %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lbl_Currency" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                <asp:Label ID="lbl_At" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_At %>" SkinID="LBL_NR_BLUE"></asp:Label>
                <asp:Label ID="lbl_ExRateAudit" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lbl_Status_Nm" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_Status_Nm %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lbl_Status" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
            </td>
            <%--<td>
                            <asp:Label ID="lbl_InvoiceNo_Nm" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_InvoiceNo_Nm %>"
                                SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td colspan="3">
                        </td>--%>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lbl_Desc_Nm" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_Desc_Nm %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td colspan="7">
                <asp:Label ID="lbl_Desc" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
            </td>
        </tr>
    </table>
    <!-- Credit Note Detail -->
    <div style="overflow: auto; width: 100%">
        <asp:GridView ID="grd_CnEdit" runat="server" Width="100%" SkinID="GRD_V1" AutoGenerateColumns="False" DataKeyNames="CnDtNo" EnableModelValidation="True"
            OnRowDataBound="grd_CnEdit_RowDataBound" OnRowCommand="grd_CnEdit_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="<%$ Resources:PC_CN_Cn, Charp %>">
                    <HeaderStyle Width="17px" HorizontalAlign="Left" />
                    <ItemStyle Width="17px" VerticalAlign="Middle" />
                    <ItemTemplate>
                        <asp:ImageButton ID="Img_Btn" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/Plus.jpg" OnClientClick="expandDetailsInGrid(this);return false;" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <table border="0" cellpadding="3" cellspacing="0" width="100%" style="table-layout: fixed;">
                            <tr>
                                <td style="width: 80px; text-align: left">
                                    <asp:Label ID="lbl_RecNo_Nm" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_RecNo_Nm %>" SkinID="LBL_HD_W" Width="100%"></asp:Label>
                                </td>
                                <td style="width: 240px; text-align: left">
                                    <asp:Label ID="lbl_Location_Nm" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_Location_Nm %>" SkinID="LBL_HD_W" Width="100%"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_Product_Nm" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_Product_Nm %>" SkinID="LBL_HD_W" Width="100%"></asp:Label>
                                </td>
                                <td style="width: 80px; text-align: left;">
                                    <asp:Label ID="lbl_CnType_Nm" runat="server" Text="Type" SkinID="LBL_HD_W" Width="100%"></asp:Label>
                                </td>
                                <td style="width: 50px; text-align: right;">
                                    <asp:Label ID="lbl_Qty_Nm" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_Qty_Nm %>" SkinID="LBL_HD_W" Width="100%"></asp:Label>
                                </td>
                                <td style="width: 20px; text-align: left;">
                                    <asp:Label ID="lbl_Unit_Nm" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_Unit_Nm %>" SkinID="LBL_HD_W" Width="100%"></asp:Label>
                                </td>
                                <td style="width: 80px; text-align: right;">
                                    <asp:Label ID="lbl_NetAmt_Nm" runat="server" Text="Amount" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td style="width: 80px; text-align: right;">
                                    <asp:Label ID="lbl_TaxAmt_Nm" runat="server" Text="Tax" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td style="width: 80px; text-align: right;">
                                    <asp:Label ID="lbl_TotalAmt_Nm" runat="server" Text="Total" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table border="0" cellpadding="3" cellspacing="0" width="100%" class="TABLE_HD" style="table-layout: fixed;">
                            <tr>
                                <!-- Receiving No -->
                                <td style="width: 80px; text-align: left">
                                    <asp:Label ID="lbl_Rec" runat="server" SkinID="LBL_NR" Width="100%"></asp:Label>
                                </td>
                                <!-- Store -->
                                <td style="width: 240px; text-align: left">
                                    <asp:Label ID="lbl_Location" runat="server" SkinID="LBL_NR" Width="100%"></asp:Label>
                                </td>
                                <!-- Item No -->
                                <td>
                                    <asp:Label ID="lbl_Product" runat="server" SkinID="LBL_NR" Width="100%"></asp:Label>
                                </td>
                                <!-- Type -->
                                <td style="width: 80px; text-align: left;">
                                    <asp:Label ID="lbl_CnType" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <!-- Unit -->
                                <td style="width: 50px; text-align: right;">
                                    <asp:Label ID="lbl_Qty" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td style="width: 20px; text-align: left;">
                                    <asp:Label ID="lbl_Unit" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <!-- Net -->
                                <td style="width: 80px; text-align: right;">
                                    <asp:Label ID="lbl_CurrNet" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <!-- Tax -->
                                <td style="width: 80px; text-align: right;">
                                    <asp:Label ID="lbl_CurrTax" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <!-- Total -->
                                <td style="width: 80px; text-align: right;">
                                    <asp:Label ID="lbl_CurrTotal" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <FooterTemplate>
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 60%;">
                                </td>
                                <td colspan="2" style="width: 20%">
                                    <asp:Label ID="lbl_GrandCurrTotal_Nm" runat="server" SkinID="LBL_HD_W" Font-Underline="True" Text="Total Currency" Width="100%" />
                                </td>
                                <td colspan="2" style="width: 20%">
                                    <asp:Label ID="lbl_GrandTotal_Nm" runat="server" SkinID="LBL_HD_W" Font-Underline="True" Text="Total Base" Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 60%;">
                                </td>
                                <td style="width: 12%">
                                    <asp:Label ID="lbl_GrandCurrTotalNet_Nm" runat="server" SkinID="LBL_HD_W" Text="Net Amount" Width="100%" />
                                </td>
                                <td style="width: 8%; text-align: right; padding-right: 10px;">
                                    <asp:Label ID="lbl_GrandCurrTotalNet" runat="server" SkinID="LBL_HD_W" />
                                </td>
                                <td style="width: 12%">
                                    <asp:Label ID="lbl_GrandTotalNet_Nm" runat="server" SkinID="LBL_HD_W" Text="Net Amount" Width="100%" />
                                </td>
                                <td style="width: 8%; text-align: right;">
                                    <asp:Label ID="lbl_GrandTotalNet" runat="server" SkinID="LBL_HD_W" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_GrandCurrTotalTax_Nm" runat="server" SkinID="LBL_HD_W" Text="Tax Amount" Width="100%" />
                                </td>
                                <td style="text-align: right; padding-right: 10px;">
                                    <asp:Label ID="lbl_GrandCurrTotalTax" runat="server" SkinID="LBL_HD_W" />
                                </td>
                                <td>
                                    <asp:Label ID="lbl_GrandTotalTax_Nm" runat="server" SkinID="LBL_HD_W" Text="Tax Amount" Width="100%" />
                                </td>
                                <td style="text-align: right;">
                                    <asp:Label ID="lbl_GrandTotalTax" runat="server" SkinID="LBL_HD_W" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 60%;">
                                </td>
                                <td>
                                    <asp:Label ID="lbl_GrandCurrTotalAmt_Nm" runat="server" SkinID="LBL_HD_W" Text="Total Amount" Width="100%" />
                                </td>
                                <td style="text-align: right; padding-right: 10px;">
                                    <asp:Label ID="lbl_GrandCurrTotalAmt" runat="server" SkinID="LBL_HD_W" />
                                </td>
                                <td>
                                    <asp:Label ID="lbl_GrandTotalAmt_Nm" runat="server" SkinID="LBL_HD_W" Text="Total Amount" Width="100%" />
                                </td>
                                <td style="text-align: right;">
                                    <asp:Label ID="lbl_GrandTotalAmt" runat="server" SkinID="LBL_HD_W" />
                                </td>
                            </tr>
                        </table>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <tr id="TR_Summmary" runat="server" style="display: none;">
                            <td colspan="9">
                                <table style="width: 100%; table-layout: fixed;">
                                    <tr>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lbl_RecDate_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Receiving Date: " />
                                            <asp:Label ID="lbl_RecDate" runat="server" SkinID="LBL_NR_1" Text="" />
                                        </td>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lbl_RecTaxType_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_CN_Cn, lbl_TaxType_Nm %>" />
                                            <asp:Label ID="lbl_RecTaxType" runat="server" SkinID="LBL_NR_1" Text="" />
                                        </td>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lbl_RecTaxRate_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_CN_Cn, lbl_TaxRate_Nm %>" />
                                            <asp:Label ID="lbl_RecTaxRate" runat="server" SkinID="LBL_NR_1" Text="" />
                                        </td>
                                        <td style="width: 150px;">
                                            <asp:Label ID="lbl_RecPrice_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_CN_Cn, lbl_PriceDt_Nm %>" />
                                            <asp:Label ID="lbl_RecPrice" runat="server" SkinID="LBL_NR_1" Text="" />
                                        </td>
                                        <td style="width: 80px;">
                                            <asp:Label ID="lbl_Currency_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Currency:" />
                                            <asp:Label ID="lbl_Currency" runat="server" SkinID="LBL_NR_1" />
                                        </td>
                                        <td style="width: 80px; text-align: right;">
                                        </td>
                                        <td style="width: 80px;">
                                            <asp:Label ID="lbl_BaseCurrency_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Base:" />
                                            <asp:Label ID="lbl_BaseCurrency" runat="server" SkinID="LBL_NR_1" />
                                        </td>
                                        <td style="width: 80px; text-align: right;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_CurrNetAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Net Amount:" />
                                        </td>
                                        <td style="text-align: right; padding-right: 10px;">
                                            <asp:Label ID="lbl_CurrNetAmt" runat="server" SkinID="LBL_NR_GRD" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_NetAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Net Amount:" />
                                        </td>
                                        <td style="text-align: right; padding-right: 10px;">
                                            <asp:Label ID="lbl_NetAmt" runat="server" SkinID="LBL_NR_GRD" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_CurrTaxAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Tax Amount:" />
                                        </td>
                                        <td style="text-align: right; padding-right: 10px;">
                                            <asp:Label ID="lbl_CurrTaxAmt" runat="server" SkinID="LBL_NR_GRD" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_TaxAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Tax Amount" />
                                        </td>
                                        <td style="text-align: right; padding-right: 10px;">
                                            <asp:Label ID="lbl_TaxAmt" runat="server" SkinID="LBL_NR_GRD" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_CurrTotalAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Total Amount:" />
                                        </td>
                                        <td style="text-align: right; padding-right: 10px;">
                                            <asp:Label ID="lbl_CurrTotalAmt" runat="server" SkinID="LBL_NR_GRD" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_TotalAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Total Amount:" />
                                        </td>
                                        <td style="text-align: right; padding-right: 10px;">
                                            <asp:Label ID="lbl_TotalAmt" runat="server" SkinID="LBL_NR_GRD" />
                                        </td>
                                    </tr>
                                </table>
                                <table style="width: 100%; padding-bottom: 10px;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_RecDesc_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Description: " />
                                            <asp:Label ID="lbl_RecDesc" runat="server" SkinID="LBL_NR_1" Text="" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <uc5:StockMovement ID="uc_StockMovement" runat="server" />
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
        <asp:HiddenField ID="hf_ConnStr" runat="server" />
        <asp:ObjectDataSource ID="obd_CnDt" runat="server" SelectMethod="GetListByCnNo" TypeName="Blue.BL.PC.CN.CnDt">
            <SelectParameters>
                <asp:QueryStringParameter Name="CnNo" QueryStringField="ID" Type="String" />
                <asp:ControlParameter ControlID="hf_ConnStr" Name="connStr" PropertyName="Value" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
    <%--Total Summary --%>
    <div style="display: flex; justify-content: flex-end;">
        <uc:TotalSummary runat="server" ID="TotalSummary" />
    </div>
    <br />
    <dx:ASPxPopupControl ID="pop_ConfirmVoid" ClientInstanceName="pop_ConfirmVoid" runat="server" Width="300px" CloseAction="CloseButton" HeaderText="<%$ Resources:PC_CN_Cn, Confirm %>"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" colspan="2" height="50px">
                            <asp:Label ID="lbl_ConfirmVoid" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_ConfirmVoid %>" SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <%--<dx:ASPxButton ID="btn_ConfirmVoid" CausesValidation="false" runat="server" Text="Yes"
                                            Width="50px" OnClick="btn_ConfirmVoid_Click" SkinID="BTN_N1">
                                        </dx:ASPxButton>--%>
                            <asp:Button ID="btn_ConfirmVoid" runat="server" Text="<%$ Resources:PC_CN_Cn, btn_ConfirmVoid %>" Width="50px" SkinID="BTN_V1" OnClick="btn_ConfirmVoid_Click" />
                        </td>
                        <td align="left">
                            <%--<dx:ASPxButton ID="btn_CancelVoid" CausesValidation="false" runat="server" Text="No"
                                            Width="50px" SkinID="BTN_N1">
                                            <ClientSideEvents Click="function(s, e) {
	                                            pop_ConfirmVoid.Hide();
	                                            return false;
                                            }" />
                                        </dx:ASPxButton>--%>
                            <asp:Button ID="btn_CancelVoid" runat="server" Text="<%$ Resources:PC_CN_Cn, btn_CancelVoid %>" Width="50px" SkinID="BTN_V1" OnClick="btn_CancelVoid_Click" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_ProductLog" runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" HeaderText="<%$ Resources:PC_CN_Cn, HistoryOfOrder %>"
        Width="650px" CloseAction="CloseButton" Modal="True">
        <ContentStyle HorizontalAlign="Center" VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <table border="0" cellpadding="2" cellspacing="0" width="100%">
                    <tr>
                        <td align="left">
                            <asp:Label ID="HD1" runat="server" Text="<%$ Resources:PC_CN_Cn, HD1 %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox1" runat="server" SkinID="TXT_V1" Width="180px" Text="demo@blueledgers.com" Enabled="False"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_CreatedOn" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_CreatedOn %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <dx:ASPxDateEdit ID="ASPxDateEdit1" runat="server" Width="100px" Font-Names="Arial,Tahoma,MS Sans serif" Font-Size="8pt" Date="2011-08-22" DisplayFormatString="dd/MM/yyyy"
                                EditFormat="Custom" Enabled="False">
                            </dx:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lbl_ApprBy" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_ApprBy %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox2" runat="server" SkinID="TXT_V1" Width="180px" Enabled="False"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_ApprOn" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_ApprOn %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <dx:ASPxDateEdit ID="ASPxDateEdit2" runat="server" Width="100px" Font-Names="Arial,Tahoma,MS Sans serif" Font-Size="8pt" Enabled="False">
                            </dx:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lbl_AllocatedBuyer" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_AllocatedBuyer %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox3" runat="server" SkinID="TXT_V1" Width="180px" Enabled="False"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_BuyerAllocatedOn" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_BuyerAllocatedOn %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <dx:ASPxDateEdit ID="ASPxDateEdit3" runat="server" Width="100px" Font-Names="Arial,Tahoma,MS Sans serif" Font-Size="8pt" Enabled="False">
                            </dx:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lbl_AllocatedVendor" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_AllocatedVendor %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox4" runat="server" SkinID="TXT_V1" Width="180px" Enabled="False"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_VendorAllocatedOn" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_VendorAllocatedOn %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <dx:ASPxDateEdit ID="ASPxDateEdit4" runat="server" Width="100px" Font-Names="Arial,Tahoma,MS Sans serif" Font-Size="8pt" Enabled="False">
                            </dx:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr style="height: 40px">
                        <td align="left">
                            <asp:Label ID="lbl_VendorApprBy" runat="server" SkinID="LBL_HD" Text="<%$ Resources:PC_CN_Cn, lbl_VendorApprBy %>"></asp:Label>
                        </td>
                        <td align="left">
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_VendorApprOn" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_VendorApprOn %>" SkinID="LBL_HD"></asp:Label>
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
                            <asp:Label ID="lbl_FirstAuthoBy" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_FirstAuthoBy %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox6" runat="server" SkinID="TXT_V1" Width="180px" Enabled="False"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_FirstAuthoOn" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_FirstAuthoOn %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <dx:ASPxDateEdit ID="ASPxDateEdit6" runat="server" Width="100px" Font-Names="Arial,Tahoma,MS Sans serif" Font-Size="8pt" Enabled="False">
                            </dx:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lbl_SecondAuthoBy" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_SecondAuthoBy %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox7" runat="server" SkinID="TXT_V1" Width="180px" Enabled="False"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_SecondAuthoOn" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_SecondAuthoOn %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <dx:ASPxDateEdit ID="ASPxDateEdit7" runat="server" Width="100px" Font-Names="Arial,Tahoma,MS Sans serif" Font-Size="8pt" Enabled="False">
                            </dx:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lbl_ThirdAuthoBy" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_ThirdAuthoBy %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox8" runat="server" SkinID="TXT_V1" Width="180px" Enabled="False"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_ThirdAuthoByOn" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_ThirdAuthoByOn %>" SkinID="LBL_HD"></asp:Label>
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
    <dx:ASPxPopupControl ID="pop_SendMsg" runat="server" HeaderText="<%$ Resources:PC_CN_Cn, SendMsg %>" Width="500px" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" Modal="True">
        <ContentStyle HorizontalAlign="Center" VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server" SupportsDisabledAttribute="True">
                <table border="0" cellpadding="3" cellspacing="0" width="100%">
                    <tr>
                        <td align="left" style="width: 44px">
                            <asp:Button ID="btn_To" runat="server" Text="<%$ Resources:PC_CN_Cn, btn_To %>" SkinID="BTN_V1" Width="60px" />
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox10" runat="server" SkinID="TXT_V1" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 44px">
                            <asp:Button ID="btn_CC" runat="server" Text="<%$ Resources:PC_CN_Cn, btn_CC %>" SkinID="BTN_V1" Width="60px" />
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox9" runat="server" SkinID="TXT_V1" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 44px">
                            <asp:Button ID="btn_Bcc" runat="server" Text="<%$ Resources:PC_CN_Cn, btn_Bcc %>" SkinID="BTN_V1" Width="60px" />
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox5" runat="server" SkinID="TXT_V1" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 44px">
                            <asp:Label ID="lbl_Subject" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_Subject %>" SkinID="LBL_HD" Width="60px"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txt_Subject" runat="server" SkinID="TXT_V1" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lbl_MsgText" runat="server" Text="<%$ Resources:PC_CN_Cn, lbl_MsgText %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:TextBox ID="txt_Msg" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="2">
                            <asp:Button ID="btn_Cancel" runat="server" Text="<%$ Resources:PC_CN_Cn, btn_Cancel %>" SkinID="BTN_V1" OnClick="btn_Cancel_Click" />
                            <asp:Button ID="btn_Send" runat="server" Text="<%$ Resources:PC_CN_Cn, btn_Send %>" SkinID="BTN_V1" OnClick="btn_Send_Click" />
                            <asp:Button ID="btn_Exit" runat="server" Text="<%$ Resources:PC_CN_Cn, btn_Exit %>" SkinID="BTN_V1" OnClick="btn_Exit_Click" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
