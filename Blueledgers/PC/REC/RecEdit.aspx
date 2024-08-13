<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="RecEdit.aspx.cs" Inherits="BlueLedger.PL.IN.REC.RECEdit" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1" Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Src="../StockMovement.ascx" TagName="StockMovement" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <style type="text/css">
        th, td
        {
            padding: 0 0 0 3px;
        }
    </style>
    <!-- Title / Action buttons -->
    <div class="flex flex-justify-content-between title-bar mb-10">
        <div class="ms-10">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
            <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:PC_REC_RecEdit, lbl_Title %>" />
        </div>
        <div class="flex me-10">
            <dx:ASPxButton ID="btn_Save" runat="server" CssClass="ms-10" BackColor="Transparent" Height="16px" Width="42px" ToolTip="Save" HorizontalAlign="Right"
                OnClick="btn_Save_Click">
                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/save.png" Repeat="NoRepeat" HorizontalPosition="left" VerticalPosition="center" />
                <HoverStyle>
                    <BackgroundImage HorizontalPosition="left" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png" Repeat="NoRepeat" VerticalPosition="center" />
                </HoverStyle>
                <Border BorderStyle="None" />
            </dx:ASPxButton>
            <dx:ASPxButton ID="btn_Commit" runat="server" CssClass="ms-10" Width="51px" Height="16px" BackColor="Transparent" ForeColor="White" ToolTip="Commit" HorizontalAlign="Right"
                OnClick="btn_Commit_Click">
                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/commit.png" Repeat="NoRepeat" HorizontalPosition="left" VerticalPosition="center" />
                <HoverStyle>
                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/gray-commit.png" Repeat="NoRepeat" VerticalPosition="center" HorizontalPosition="left" />
                </HoverStyle>
                <Border BorderStyle="None" />
            </dx:ASPxButton>
            <dx:ASPxButton ID="btn_Back" runat="server" CssClass="ms-10" BackColor="Transparent" Height="16px" Width="42px" ToolTip="Back" HorizontalAlign="Right"
                OnClick="btn_Back_Click">
                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat" HorizontalPosition="left" VerticalPosition="center" />
                <HoverStyle>
                    <BackgroundImage HorizontalPosition="left" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png" Repeat="NoRepeat" VerticalPosition="center" />
                </HoverStyle>
                <Border BorderStyle="None" />
            </dx:ASPxButton>
        </div>
    </div>
    <!-- Header -->
    <table class="table width-100 mb-10">
        <tr>
            <td style="width: 80px;">
                <asp:Label ID="lbl_Type_Nm" runat="server" SkinID="LBL_HD">Type</asp:Label>
            </td>
            <td style="width: 100px;">
                <asp:Label ID="lbl_Type" runat="server" SkinID="LBL_HD">From Purchase Order</asp:Label>
            </td>
            <td style="width: 80px;">
            </td>
            <td style="width: 160px;">
            </td>
            <td style="width: 80px;">
                <asp:Label ID="lbl_Receiver_Nm" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_Receiver_Nm %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td style="width: 120px;">
                <asp:Label ID="lbl_Receiver" runat="server" SkinID="LBL_NR"></asp:Label>
            </td>
            <td style="width: 120px;">
                <asp:Label ID="lbl_CommitDate_Nm" runat="server" SkinID="LBL_HD">Committed Date:</asp:Label>
            </td>
            <td>
                <asp:Label ID="lbl_CommitDate" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
            </td>
            <td style="width: 60px;">
                <asp:Label ID="lbl_Status_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_Status_Nm %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td style="width: 120px;">
                <asp:Label ID="lbl_DocStatus" runat="server" SkinID="LBL_NR" Text="STATUS"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lbl_RecNo_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_RecNo_Nm %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lbl_RecNo" runat="server" SkinID="LBL_NR"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lbl_RecDate_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_RecDate_Nm %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td>
                <dx:ASPxDateEdit ID="de_RecDate" runat="server" Width="170" DisplayFormatString="dd/MM/yyyy" EditFormatString="dd/MM/yyyy" ShowShadow="False">
                    <ValidationSettings Display="Dynamic">
                        <ErrorFrameStyle ImageSpacing="4px">
                            <ErrorTextPaddings PaddingLeft="4px" />
                        </ErrorFrameStyle>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxDateEdit>
            </td>
            <td>
                <asp:Label ID="lbl_VendorCode_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_VendorCode_Nm %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td>
                <dx:ASPxComboBox ID="ddl_Vendor" runat="server" Width="340" ValueField="Code" TextField="Name" IncrementalFilteringMode="Contains">
                </dx:ASPxComboBox>
            </td>
            <td>
                <asp:Label ID="lbl_DeliPoing_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_DeliPoing_Nm %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td>
                <dx:ASPxComboBox ID="ddl_DeliPoint" runat="server" Width="98%" ValueField="Code" TextField="Name" IncrementalFilteringMode="Contains">
                </dx:ASPxComboBox>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lbl_InvDate_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_InvNo_Nm1 %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td>
                <dx:ASPxDateEdit ID="de_InvDate" runat="server" Width="140" DisplayFormatString="dd/MM/yyyy" EditFormatString="dd/MM/yyyy" ShowShadow="False">
                    <ValidationSettings Display="Dynamic">
                        <ErrorFrameStyle>
                            <ErrorTextPaddings PaddingLeft="4px" />
                        </ErrorFrameStyle>
                    </ValidationSettings>
                </dx:ASPxDateEdit>
            </td>
            <td>
                <asp:Label ID="lbl_InvNo_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_InvNo_Nm0 %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txt_InvNo" runat="server" Width="160" SkinID="TXT_V1" MaxLength="30"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="lbl_CashConsign_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_DeliPoing_Nm0 %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="chk_CashConsign" runat="server" Enabled="true" SkinID="CHK_V1" />
            </td>
            <td>
                <asp:Label ID="lbl_Currency_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_Currency_Nm %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td colspan="3" class="flex">
                <dx:ASPxComboBox ID="ddl_Currency" runat="server" ValueField="Code" TextField="Name" IncrementalFilteringMode="Contains" OnSelectedIndexChanged="ddl_Currency_SelectedIndexChanged">
                </dx:ASPxComboBox>
                <asp:Label runat="server" Font-Size="Large" Text="@" />
                <dx:ASPxSpinEdit runat="server" ID="se_CurrencyRate" MinValue="0" NullText="1.00" SpinButtons-ShowIncrementButtons="False" />
            </td>
            <%--<td>
                    </td>
                    <td>
                    </td>--%>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lbl_Desc_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_Desc_Nm %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td colspan="9">
                <asp:TextBox ID="txt_Desc" runat="server" Width="98%" SkinID="TXT_V1"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lbl_TotalExtraCost" runat="server" Text="Extra Cost" SkinID="LBL_HD" />
            </td>
            <td>
                <dx:ASPxSpinEdit ID="se_TotalExtraCost" runat="server" DisplayFormatString=",0.00" NullText="0" SpinButtons-ShowIncrementButtons="False" DecimalPlaces="2"
                    ReadOnly="true" />
            </td>
            <td>
                <asp:Button ID="btn_AllocateExtraCost" runat="server" Text="Allocate" OnClick="btn_AllocateExtraCost_Click" />
            </td>
            <td>
                <asp:RadioButton ID="rdb_ExtraCostByQty" runat="server" Text="Quantity" GroupName="Group_ExtraCost" />
                <asp:RadioButton ID="rdb_ExtraCostByAmt" runat="server" Text="Amount" GroupName="Group_ExtraCost" />
            </td>
            <td colspan="6">
                <asp:Button ID="btn_ExtraCostDetail" runat="server" Text="Detail" Width="100px" OnClick="btn_ExtraCostDetail_Click" />
            </td>
        </tr>
    </table>
    <!-- PO Selection -->
    <div class="flex flex-justify-content-between mb-10" style="background-color: #f5f5f5; padding: 10px;">
        <div>
        </div>
        <div>
            <asp:Button ID="btn_AddPo" runat="server" Text="<%$ Resources:PC_REC_RecEdit, btn_AddPo %>" SkinID="BTN_V1" Width="80px" OnClick="btn_AddPo_Click" />
            <asp:Button ID="btn_AddItem" runat="server" Text="<%$ Resources:PC_REC_RecEdit, btn_AddItem %>" SkinID="BTN_V1" Width="80px" OnClick="btn_AddItem_Click" />
        </div>
    </div>
    <!-- Details -->
    <asp:UpdatePanel ID="UpdatePanel_Detail" runat="server">
        <ContentTemplate>
            <asp:GridView ID="gv_Detail" runat="server" Width="100%" SkinID="GRD_V1" AutoGenerateColumns="False" EmptyDataText="No Data">
                <Columns>
                    <asp:TemplateField>
                        <HeaderStyle Width="17px" HorizontalAlign="Left" />
                        <ItemStyle Width="17px" VerticalAlign="Top" HorizontalAlign="Left" />
                        <ItemTemplate>
                            <%--CommandName="ShowDetail"--%>
                            <asp:ImageButton ID="Img_Btn" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/Plus.jpg" OnClientClick="expandDetailsInGrid(this);return false;" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <%--CommandName="ShowDetail"--%>
                            <asp:ImageButton ID="Img_Btn" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/Plus.jpg" OnClientClick="expandDetailsInGrid(this);return false;" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            
                        </HeaderTemplate>
                        <ItemTemplate>
                            
                        </ItemTemplate>
                        <EditItemTemplate>
                            
                        </EditItemTemplate>
                        <FooterTemplate>
                            <table border="0" cellpadding="3" cellspacing="0" width="100%" style="margin: auto;">
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
                                    <td align="left" style="width: 15%; white-space: nowrap;">
                                        <%-- Width="100px" 100px--%>
                                        <asp:Label ID="lbl_GrandTotalNet_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_TotalRec_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                    </td>
                                    <td width="8%" align="right">
                                        <%--70pxWidth="8%"--%>
                                        <asp:Label ID="lbl_GrandTotalNet" runat="server" SkinID="LBL_HD_W"></asp:Label>
                                    </td>
                                    <td width="3%">
                                    </td>
                                    <%--65px--%>
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
                                    <td align="left">
                                        <asp:Label ID="lbl_GrandTotalTax_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_TotalTax_Nm %>" SkinID="LBL_HD_W"></asp:Label><%-- Width="100px"--%>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lbl_GrandTotalTax" runat="server" SkinID="LBL_HD_W"></asp:Label><%-- Width="70px"--%>
                                    </td>
                                    <td>
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
                                    <td align="left" style="padding-right: 5px; white-space: nowrap;">
                                        <asp:Label ID="lbl_GrandTotalAmt_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_GrandTotal %>" SkinID="LBL_HD_W"></asp:Label><%-- Width="100px"--%>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lbl_GrandTotalAmt" runat="server" SkinID="LBL_HD_W"></asp:Label><%-- Width="70px"--%>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="0%">
                        <ItemTemplate>
                            <tr id="TR_Summmary" runat="server" style="display: none; margin: auto;">
                                <td colspan="5" style="padding-left: 10px; padding-right: 0px">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="margin: auto;">
                                        <tr>
                                            <td style="vertical-align: top; width: 100%">
                                                <%--Transaction Details--%>
                                                <table border="0" cellpadding="0" cellspacing="0" class="TABLE_HD" width="100%" style="margin: auto;">
                                                    <tr>
                                                        <td style="width: 6%">
                                                            <asp:Label ID="lbl_Receive_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_Receive_Nm %>"></asp:Label>
                                                        </td>
                                                        <td style="width: 7%;" align="right">
                                                            <asp:Label ID="lbl_Receive" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                        <td style="width: 5%; padding-left: 10px;" align="left">
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
                                                        <td colspan="2">
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
                                                        <td>
                                                        </td>
                                                        <td colspan="2">
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td class="TD_LINE_GRD" style="width: 10%;">
                                                            <asp:Label ID="lbl_CurrNetAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_NetAmt_Nm %>"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD" align="right" style="width: 15%;">
                                                            <asp:Label ID="lbl_CurrNetAmt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD" style="width: 5%;">
                                                        </td>
                                                        <td class="TD_LINE_GRD" style="width: 10%;">
                                                            <asp:Label ID="lbl_NetAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_NetAmt_Nm %>"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD" align="right" style="width: 15%;">
                                                            <asp:Label ID="lbl_NetAmt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                        <%-- <td class="TD_LINE_GRD" align="center" style="width: 7%;">
                                                                            <asp:Label ID="lbl_NetAcc_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_NetAcc_Nm %>"></asp:Label>
                                                                        </td>
                                                                        <td class="TD_LINE_GRD" style="width: 10%;">
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
                                                            <asp:Label ID="lbl_DiscAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_DiscAmt_Nm %>"></asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="lbl_DiscAmt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                        <%-- <td colspan="2">
                                                                        </td>--%>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:CheckBox ID="chk_DiscAdj" runat="server" SkinID="CHK_V1" Text=" Adj. Discount" Enabled="false" Height="15px" />
                                                        </td>
                                                        <td style="width: 8%;">
                                                            <asp:Label ID="lbl_Disc_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_Disc_Nm %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_Disc" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                            <asp:Label ID="lbl_DiscPercent" runat="server" SkinID="LBL_NR_1" Text="<%$ Resources:PC_REC_RecEdit, lbl_Percent %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
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
                                                            <asp:Label ID="lbl_TaxAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_TaxAmt_Nm %>"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD" align="right">
                                                            <asp:Label ID="lbl_TaxAmt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                        <%--<td class="TD_LINE_GRD" align="center">
                                                                            <asp:Label ID="lbl_TaxAcc_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_TaxAcc_Nm %>"></asp:Label>
                                                                        </td>
                                                                        <td class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_TaxAcc" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>--%>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:CheckBox ID="chk_TaxAdj" runat="server" SkinID="CHK_V1" Text=" Adj. Tax" Enabled="false" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_TaxType_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_TaxType_Nm %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_TaxType" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
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
                                                            <asp:Label ID="lbl_TotalAmtDt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_TotalAmtDt_Nm %>"></asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="lbl_TotalAmtDt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_TaxRate_Edit_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_TaxRate_Edit_Nm %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_TaxRate" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                            <asp:Label ID="lbl_Percent" runat="server" SkinID="LBL_NR_1" Text="<%$ Resources:PC_REC_RecEdit, lbl_Percent %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td colspan="3">
                                                        </td>
                                                        <td colspan="2">
                                                        </td>
                                                    </tr>
                                                    <tr align="right">
                                                        <td colspan="10">
                                                        </td>
                                                        <td align="right">
                                                            <table border="0" cellpadding="2" cellspacing="0" style="margin: auto;">
                                                                <tr>
                                                                    <td valign="bottom">
                                                                        <asp:LinkButton ID="lnkb_Edit" runat="server" CausesValidation="False" CommandName="Edit" SkinID="LNKB_NORMAL" Text="Edit"></asp:LinkButton>
                                                                    </td>
                                                                    <td valign="bottom">
                                                                        <asp:LinkButton ID="lnkb_Del" runat="server" CausesValidation="false" CommandName="Delete" SkinID="LNKB_NORMAL" Text="Delete"></asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <table border="0" cellpadding="3" cellspacing="0" class="TABLE_HD" width="100%" style="margin: auto;">
                                        <tr>
                                            <td style="width: 6%" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_OnHand_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_OnHand_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td style="width: 6%" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td style="width: 6%" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_OnOrder_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_OnOrder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td style="width: 6%; padding-left: 5px;" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td style="width: 6%" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_Reorder_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_Reorder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td style="width: 6%" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_Reorder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td style="width: 6%; padding-left: 5px;" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_Restock_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_Restock_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td style="width: 6%" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_Restock" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td style="width: 6%; white-space: nowrap;" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_LastPrice_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_LastPrice_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td style="width: 6%" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td style="width: 10%; white-space: nowrap; padding-left: 5px;" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_LastVendor_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_LastVendor_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td style="width: 30%; white-space: nowrap; overflow: hidden" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1" Width="200px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_PoRef_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_PoRef_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td class="TD_LINE_GRD" colspan="3">
                                                <asp:Label ID="lbl_PoRef" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_PrRef_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_PrRef_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td class="TD_LINE_GRD" colspan="3">
                                                <asp:Label ID="lbl_PrRef" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td class="TD_LINE_GRD" colspan="4">
                                            </td>
                                        </tr>
                                    </table>
                                    <%--Comment--%>
                                    <table class="TABLE_HD" width="100%" style="margin: auto;">
                                        <tr style="background-color: #DADADA; height: 17px">
                                            <td>
                                                <asp:Label ID="lbl_DtrComment_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_DtrComment_Nm %>" SkinID="LBL_HD_1"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_DtrComment" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="TABLE_HD" style="margin: auto;">
                                        <tr>
                                            <td>
                                                <uc2:StockMovement ID="uc_StockMovement" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EditItemTemplate>
                            
                        </EditItemTemplate>
                        <HeaderStyle Width="0%"></HeaderStyle>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
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
</asp:Content>
