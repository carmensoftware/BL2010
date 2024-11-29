<%@ Page Title="" Trace="false" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="Pr.aspx.cs" Inherits="BlueLedger.PL.PC.PR.Pr" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dx" %>
<%@ Register Src="../StockSummary.ascx" TagName="StockSummary" TagPrefix="uc0" %>
<%@ Register Src="~/UserControl/ProcessStatus.ascx" TagName="ProcessStatus" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Comment2.ascx" TagName="Comment2" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/Attach2.ascx" TagName="Attach2" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Log2.ascx" TagName="Log2" TagPrefix="uc4" %>
<%@ Register Src="~/UserControl/workflow/ProcessStatusDt.ascx" TagName="ProcessStatusDt" TagPrefix="uc7" %>
<%@ Register Src="~/UserControl/TotalSummary.ascx" TagName="TotalSummary" TagPrefix="uc" %>
<%@ Reference Control="~/UserControl/Comment.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <script type="text/javascript">
        function OnVendorChanged(cmbVendor) {
            cmbVendor.PerformCallback(cmbVendor.GetValue().toString());
        }

        function OnProductChanged(cmbProduct) {
            cmbProduct.PerformCallback(cmbProduct.GetValue().toString());
        }

        var postponedCallbackValue = null;

        function OnCmbVendorIndexChanged(s, e) {
            var item = cmbVendor.GetSelectedItem();
            if (CallbackPanel.InCallback())
                postponedCallbackValue = item.value;
            else
                CallbackPanel.PerformCallback(item.value);
        }

        //Check Select All CheckBox.
        function Check(item) {
            var elements = document.getElementById('ctl00_cph_Main_grd_PRDt1').getElementsByTagName('input');
            for (i = 0; i < elements.length; i++) {
                if (elements[i].type == 'checkbox') {
                    elements[i].checked = item.checked;
                }
            }
        }

        function IsCheckBox(chk) {
            if (chk.type == 'checkbox') return true;
            else return false;
        }

        function lnkb_OnClick() {
            pop_PrDtEdit.Hide();
            return false;
        }

        function OnCheckedChanged() {
            txt_Net.SetEnabled(IsAdj.GetChecked());
            txt_TaxAmt.SetEnabled(IsAdj.GetChecked());
            txt_Amount.SetEnabled(IsAdj.GetChecked());

        }

        var DiscountPercent = 0;
        var DiscountAmt = 0;

        function OnVendorChanged(vendor) {
            CallBackPanel.PerformCallback(vendor.lastSuccessValue);
            //UpdateAllAmount();
        }

        function OnTaxTypeChanged(TaxType) {
            UpdateAllAmount();
        }

        function OnTaxRateNumberChanged() {
            UpdateAllAmount();
        }
        function OnDiscountNumberChanged(Discount) {
            if (Discount.lastChangedValue != "") {
                DiscountPercent = Discount.lastChangedValue;
            }

            // Update discount amount
            DiscountAmt = (txt_ApprQty.lastChangedValue * DiscountPercent) / 100;
            txt_DiscountAmt.SetText(DiscountAmt);

            UpdateAllAmount();
        }

        function UpdateAllAmount() {
            var Price = 0;
            var Amt = 0;
            var Net = 0;
            var TaxAmt = 0;

            if (txt_Price.lastChangedValue != "") {
                Price = txt_Price.lastChangedValue;
            }
            if (txt_DiscountAmt.lastChangedValue != "") {
                DiscountAmt = txt_DiscountAmt.lastChangedValue;
            }

            // Net, Tax, Total
            var calAmt = (Price - DiscountAmt) * txt_ApprQty.lastChangedValue;

            switch (ddl_TaxType.GetValue()) {
                case "A":
                    txt_Net.SetText(calAmt);
                    TaxAmt = (calAmt * txt_TaxRate.lastChangedValue) / 100;
                    txt_TaxAmt.SetText(TaxAmt);
                    Amt = txt_Net.lastChangedValue + txt_TaxAmt.lastChangedValue;
                    txt_Amount.SetText(Amt);
                    break;
                case "I":
                    Net = calAmt - txt_TaxAmt.lastChangedValue;
                    txt_Net.SetText(Net);
                    TaxAmt = (calAmt * txt_TaxRate.lastChangedValue) / (100 + txt_TaxRate.lastChangedValue);
                    txt_TaxAmt.SetText(TaxAmt);
                    Amt = txt_Net.lastChangedValue + txt_TaxAmt.lastChangedValue;
                    txt_Amount.SetText(Amt);
                    break;
                default:
                    txt_Net.SetText(calAmt);
                    txt_TaxAmt.SetText("0");
                    Amt = txt_Net.lastChangedValue + txt_TaxAmt.lastChangedValue;
                    txt_Amount.SetText(Amt);
                    txt_TaxRate.SetText("0");
                    break;
            }
        }

        function OnEndCallback() {
            //UpdateAllAmount();
        }

        function expandDetailsInGrid(_this) {
            var id = _this.id;
            var imgelem = document.getElementById(_this.id);
            var currowid = id.replace("Img_Btn1", "TR_Summmary") //GETTING THE ID OF SUMMARY ROW

            var rowdetelemid = currowid;
            var rowdetelem = document.getElementById(rowdetelemid);
            if (imgelem.alt == "plus") {
                imgelem.src = "../../App_Themes/Default/Images/Plus_1.jpg"
                imgelem.alt = "minus"
                rowdetelem.style.display = 'none';
            }
            else {
                imgelem.src = "../../App_Themes/Default/Images/Minus_1.jpg"
                imgelem.alt = "plus"
                rowdetelem.style.display = '';
            }

            return false;
        }

    </script>
    <asp:HiddenField ID="hf_LoginName" runat="server" />
    <asp:HiddenField ID="hf_ConnStr" runat="server" />
    <asp:HiddenField ID="hf_BuGrpCode" runat="server" />
    <asp:HiddenField ID="hf_ddl_Vendor_Pop" runat="server" />
    <%--Process Status Bar (Top)--%>
    <div>
        <dx:ASPxPanel ID="pnl_WFLegend" runat="server">
            <Paddings Padding="0px" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent2" runat="server">
                    <table border="0" cellpadding="1" cellspacing="0" width="100%">
                        <tr style="height: 17px">
                            <td>
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <tr style="height: 17px">
                                        <td>
                                            <asp:Label ID="lbl_ProcessStatusHdr" runat="server" Text="<%$ Resources:UserControl_ViewHandler_ListPage2, lbl_ProcessStatusHdr %>" SkinID="LBL_NR_BI"
                                                EnableViewState="False"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DataList ID="dl_ProcessStatus" runat="server" RepeatDirection="Horizontal" Font-Names=" Arial" Font-Size="8pt" EnableViewState="False">
                                                <ItemStyle Font-Italic="True" Font-Names="Arial" Font-Size="7pt" />
                                                <ItemTemplate>
                                                    <%#DataBinder.Eval(Container.DataItem,"Step").ToString()%>.<%#DataBinder.Eval(Container.DataItem,"StepDesc").ToString()%>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="right">
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <tr style="height: 35px">
                                        <td>
                                            <asp:Image ID="img_Pending" runat="server" ImageUrl="~/App_Themes/Default/Images/WF/NA.gif" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_Pending" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_Pending %>" SkinID="LBL_NR_I"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Image ID="img_Approve" runat="server" ImageUrl="~/App_Themes/Default/Images/WF/APP.gif" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_Approve" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_Approve %>" SkinID="LBL_NR_I"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Image ID="img_PartialApprove" runat="server" ImageUrl="~/App_Themes/Default/Images/WF/PAR.gif" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_PartialApprove" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_PartialApprove %>" SkinID="LBL_NR_I"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Image ID="img_Reject" runat="server" ImageUrl="~/App_Themes/Default/Images/WF/REJ.gif" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_Reject" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_Reject %>" SkinID="LBL_NR_I"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxPanel>
    </div>
    <%--Action Bar--%>
    <div>
        <table border="0" cellpadding="1" cellspacing="0" width="100%">
            <tr style="padding-left: 10px; background-color: #4d4d4d; height: 17px">
                <td align="left">
                    <table border="0" cellpadding="1" cellspacing="0">
                        <tr>
                            <td style="padding-left: 10px">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                            </td>
                            <td>
                                <asp:Label ID="lbl_PRRequest_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_PRRequest_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                <asp:Label ID="lbl_Process" runat="server" SkinID="LBL_HD_WHITE"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td align="right" style="padding-left: 10px;">
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
                            <dx:MenuItem Name="Commit" Text="">
                                <ItemStyle Height="16px" Width="42px">
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/commit.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </ItemStyle>
                            </dx:MenuItem>
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
                            <dx:MenuItem Name="Print" Text="">
                                <ItemStyle Height="16px" Width="43px">
                                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                </ItemStyle>
                                <Items>
                                    <dx:MenuItem Name="SUMMARY" Text="Summary">
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="DETAIL" Text="Detailed">
                                    </dx:MenuItem>
                                </Items>
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
    </div>
    <%--PR Header--%>
    <table width="100%" class="TABLE_HD" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td class="TD_LINE" style="padding-left: 10px; width: 8.5%">
                <asp:Label ID="lbl_PRNo_Nm0" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_PRNo_Nm0 %>" SkinID="lbl_HD"></asp:Label>
            </td>
            <td class="TD_LINE" style="width: 12.5%">
                <asp:Label ID="lbl_PRNo" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
            </td>
            <td style="width: 12.5%" align="right" style="padding-right: 10px;">
                <asp:Label ID="lbl_PRDate_Nm0" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_PRDate_Nm0 %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td style="width: 12.5%">
                <asp:Label ID="lbl_PRDate" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
            </td>
            <td style="width: 12.5%" align="right" style="padding-right: 10px;">
                <asp:Label ID="lbl_Requestor_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_Requestor_Nm %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td style="width: 12.5%">
                <asp:Label ID="lbl_Requestor" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
            </td>
            <td class="TD_LINE" style="width: 12.5%">
                <asp:Label ID="lbl_Process_Nm0" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_Process_Nm0 %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td class="TD_LINE" style="width: 12.5%;">
                <uc1:ProcessStatus ID="ProcessStatus" runat="server" SkinID="LBL_NR_BLUE" />
            </td>
        </tr>
        <tr>
            <td class="TD_LINE" style="padding-left: 10px; width: 8.5%">
                <asp:Label ID="lbl_PrType_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_PrType_Nm %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td class="TD_LINE" colspan="1">
                <asp:Label ID="lbl_PrType" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
            </td>
            <td class="TD_LINE" colspan="1" align="right" style="padding-right: 10px;">
                <asp:Label ID="lbl_JobCode_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:PC_PR_PrEdit, lbl_JobCode_Nm %>"></asp:Label>
            </td>
            <td class="TD_LINE" colspan="3">
                <asp:Label ID="lbl_jobCode" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lbl_Status_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_Status_Nm %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lbl_Status" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="TD_LINE" style="padding-left: 10px; width: 8.5%">
                <asp:Label ID="lbl_HODRE_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:PC_PR_PrEdit, lbl_HODRE_Nm %>"></asp:Label>
            </td>
            <td class="TD_LINE" colspan="1">
                <asp:Label ID="lbl_HODRE" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
            </td>
            <td class="TD_LINE" colspan="1" align="right" style="padding-right: 10px;">
                <asp:Label ID="lbl_HOD_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:PC_PR_PrEdit, lbl_HOD_Nm %>"></asp:Label>
            </td>
            <td class="TD_LINE" colspan="1">
                <asp:Label ID="lbl_HOD" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="TD_LINE" width="12.5%" style="padding-left: 10px">
                <asp:Label ID="lbl_Desc_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_Desc_Nm %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td class="TD_LINE" style="width: 87.5%" colspan="7">
                <asp:Label ID="lbl_Desc" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
            </td>
        </tr>
    </table>
    <%--PR Detail--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:GridView ID="grd_PRDt1" runat="server" AutoGenerateColumns="False" SkinID="GRD_V1" OnRowDataBound="grd_PRDt1_RowDataBound">
                <Columns>
                    <%--Expand Button--%>
                    <asp:TemplateField HeaderText="#">
                        <ItemTemplate>
                            <asp:ImageButton ID="Img_Btn1" runat="server" ImageUrl="~/App_Themes/Default/Images/Plus_1.jpg" OnClientClick="expandDetailsInGrid(this);return false;" />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="10px" />
                        <ItemStyle HorizontalAlign="Center" Width="10px" />
                    </asp:TemplateField>
                    <%--CheckBox--%>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="Chk_All" runat="server" onclick="Check(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="Chk_Item" runat="server" />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="10px" />
                        <ItemStyle HorizontalAlign="Center" Width="10px" />
                    </asp:TemplateField>
                    <%--Vendor--%>
                    <asp:TemplateField HeaderText="<%$ Resources:PC_PR_Pr, lbl_Vendor_Grd_Nm %>">
                        <ItemTemplate>
                            <div style="white-space: nowrap; overflow: hidden; width: 250px">
                                <asp:Label ID="lbl_VendorCode" runat="server" Width="250px" />
                            </div>
                        </ItemTemplate>
                        <HeaderStyle Width="250px" />
                        <ItemStyle Width="250px" />
                    </asp:TemplateField>
                    <%--Location--%>
                    <asp:TemplateField HeaderText="<%$ Resources:PC_PR_Pr, lbl_LocationCode_Grd_Nm %>">
                        <ItemTemplate>
                            <div style="white-space: nowrap; overflow: hidden; width: 280px">
                                <asp:Label ID="lbl_LocationCode" runat="server" Width="280px"></asp:Label>
                            </div>
                        </ItemTemplate>
                        <HeaderStyle Width="280px" />
                        <ItemStyle Width="280px" />
                    </asp:TemplateField>
                    <%--Product--%>
                    <asp:TemplateField HeaderText="<%$ Resources:PC_PR_Pr, lbl_ItemDesc_Grd_Nm %>">
                        <ItemTemplate>
                            <div style="white-space: nowrap; overflow: hidden; width: 350px">
                                <asp:Label ID="lbl_ProductCode" runat="server" Width="350px"></asp:Label>
                            </div>
                        </ItemTemplate>
                        <HeaderStyle Width="380px" />
                        <ItemStyle Width="380px" />
                    </asp:TemplateField>
                    <%--OrderUnit--%>
                    <asp:TemplateField HeaderText="<%$ Resources:PC_PR_Pr, lbl_Unit_Grd_Nm %>">
                        <ItemTemplate>
                            <asp:Label ID="lbl_OrderUnit" runat="server"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="150px" />
                        <ItemStyle Width="150px" />
                    </asp:TemplateField>
                    <%--Price--%>
                    <asp:TemplateField HeaderText="<%$ Resources:PC_PR_Pr, lbl_Price_Grd_Nm %>">
                        <ItemTemplate>
                            <asp:Label ID="lbl_Price_Dt" runat="server"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Right" Width="150px" />
                        <ItemStyle HorizontalAlign="Right" Width="150px" />
                    </asp:TemplateField>
                    <%--ReqQty--%>
                    <asp:TemplateField HeaderText="<%$ Resources:PC_PR_Pr, lbl_QtyReq_Grd_Nm %>">
                        <ItemTemplate>
                            <asp:Label ID="lbl_ReqQty" runat="server"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Right" Width="150px" />
                        <ItemStyle HorizontalAlign="Right" Width="150px" />
                    </asp:TemplateField>
                    <%--ApproveQty--%>
                    <asp:TemplateField HeaderText="<%$ Resources:PC_PR_Pr, lbl_Apprv_Grd_Nm %>">
                        <ItemTemplate>
                            <asp:Label ID="lbl_ApprQty_HD" runat="server"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Right" Width="150px" />
                        <ItemStyle HorizontalAlign="Right" Width="150px" />
                    </asp:TemplateField>
                    <%--FocQty--%>
                    <asp:TemplateField HeaderText="<%$ Resources:PC_PR_Pr, lbl_FOC_Grd_Nm %>">
                        <ItemTemplate>
                            <asp:Label ID="lbl_FOC" runat="server"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Right" Width="80px" />
                        <ItemStyle HorizontalAlign="Right" Width="80px" />
                    </asp:TemplateField>
                    <%--Total--%>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_Currency_NM" runat="server" Width="20px" Text="Currency" />
                                    </td>
                                    <td style="text-align: right;">
                                        <asp:Label ID="lbl_CurrTotalAmt_NM" runat="server" Width="80px" Text="<%$ Resources:PC_PR_Pr, lbl_Total_Grd_Nm %>" />
                                    </td>
                                    <td style="text-align: right;">
                                        <asp:Label ID="lbl_TotalAmt_NM" runat="server" Width="80px" Text="Base"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table>
                                <tr>
                                    <td style="text-align: right; white-space: nowrap;">
                                        <asp:Label ID="lbl_Currency_HD" runat="server" Width="20px" />
                                    </td>
                                    <td style="text-align: right; white-space: nowrap;">
                                        <asp:Label ID="lbl_CurrTotalAmt_HD" runat="server" Width="80px" />
                                    </td>
                                    <td style="text-align: right; white-space: nowrap;">
                                        <asp:Label ID="lbl_TotalAmt_HD" runat="server" Width="80px" />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                        <FooterTemplate>
                            <table>
                                <tr>
                                    <td style="text-align: right; white-space: nowrap;">
                                        &nbsp;
                                    </td>
                                    <td style="text-align: right; white-space: nowrap;">
                                        <asp:Label ID="lbl_SumCurrTotalAmt_av" runat="server" Width="80px" />
                                    </td>
                                    <td style="text-align: right; white-space: nowrap;">
                                        <asp:Label ID="lbl_SumTotalAmt_av" runat="server" Width="80px" />
                                    </td>
                                </tr>
                            </table>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--ReqDate--%>
                    <%--<asp:BoundField DataField="ReqDate" HeaderText="<%$ Resources:PC_PR_Pr, lbl_DelivDate_Grd_Nm %>" DataFormatString="{0:d}">--%>
                    <asp:BoundField DataField="ReqDate" HeaderText="Delivery on" DataFormatString="{0:d}">
                        <HeaderStyle HorizontalAlign="Left" Width="250px" />
                        <ItemStyle HorizontalAlign="Left" Width="250px" />
                    </asp:BoundField>
                    <%--DeliveryPoint--%>
                    <asp:TemplateField HeaderText="<%$ Resources:PC_PR_Pr, lbl_DelivP_Grd_Nm %>">
                        <HeaderStyle HorizontalAlign="Left" Width="150px" />
                        <ItemStyle Width="150px" />
                        <ItemTemplate>
                            <div style="white-space: nowrap; overflow: hidden; width: 160px">
                                <asp:Label ID="lbl_DeliPointCode" runat="server" Width="160px"></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--ProcessStatus--%>
                    <asp:TemplateField HeaderText="<%$ Resources:PC_PR_Pr, lbl_Process_Grd_Nm %>">
                        <ItemTemplate>
                            <uc7:ProcessStatusDt ID="ProcessStatusDt" runat="server" Module="PC" SubModule="PR" />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" Width="180px" />
                        <ItemStyle HorizontalAlign="Left" Width="180px" />
                    </asp:TemplateField>
                    <%--Summary Panel--%>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <tr id="TR_Summmary" runat="server" style="display: none;">
                                <td colspan="14" style="padding: 10px; width: 100%;">
                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                        <%--Last 3 receiving--%>
                                        <tr>
                                            <td colspan="10">
                                                <asp:Label ID="lbl_LastRC" runat="server" Font-Italic="True" Font-Size="Smaller" />
                                            </td>
                                        </tr>
                                        <%--Title--%>
                                        <tr>
                                            <td style="width: 10%;">
                                            </td>
                                            <td style="width: 10%;">
                                            </td>
                                            <td style="width: 10%;">
                                            </td>
                                            <td style="width: 10%;">
                                            </td>
                                            <td style="width: 10%;">
                                                <asp:Label ID="lbl_CurrCurr_Grd_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, TitleCurrency %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                <asp:Label ID="lbl_CurrCurr_Grd" runat="server" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td align="right" style="width: 10%;">
                                            </td>
                                            <td style="width: 10%;">
                                            </td>
                                            <td style="width: 10%;">
                                                <asp:Label ID="lbl_BaseCurr_Grd_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, TitleBaseCurrency %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                <asp:Label ID="lbl_BaseCurr_Grd" runat="server" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td align="right" style="width: 10%;">
                                            </td>
                                            <td style="width: 10%;">
                                            </td>
                                        </tr>
                                        <%--CurrNetAmt, NetAmt--%>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_CurrNetAmt_Grd_Nm" runat="server" Text="Net. Amount" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="lbl_CurrNetAmt_Grd" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_NetAmt_Grd_Nm" runat="server" Text="Net. Amount" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="lbl_NetAmt_Grd" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <%--DiscRate, CurrDiscAmt, DiscAmt--%>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_Disc_Grd_Nm" runat="server" Text="Disc." SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_Disc_Grd" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_CurrDiscAmt_Grd_Nm" runat="server" Text="Disc. Amount" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="lbl_CurrDiscAmt_Grd" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_DiscAmt_Grd_Nm" runat="server" Text="Disc. Amount" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="lbl_DiscAmt_Grd" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <%--TaxType, CurrTaxAmt, TaxAmt--%>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chk_Adj" runat="server" Text="Adj." />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_TaxType_Grd_Nm" runat="server" Text="Tax Type" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_TaxType_Grd" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_CurrTaxAmt_Grd_Nm" runat="server" Text="Tax. Amount" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="lbl_CurrTaxAmt_Grd" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_TaxAmt_Grd_Nm" runat="server" Text="Tax. Amount" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="lbl_TaxAmt_Grd" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <%--TaxRate, CurrTotalAmt, TotalNetAmt--%>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_TaxRate_Grd_Nm" runat="server" Text="Tax Rate" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_TaxRate_Grd" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_CurrTotalAmt_Grd_Nm" runat="server" Text="Total. Amount" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="lbl_CurrTotalAmt_Grd" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_TotalAmt_Grd_Nm" runat="server" Text="Total. Amount" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="lbl_TotalAmt_Grd" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr style="background-color: #DADADA; height: 17px;">
                                            <td>
                                                <asp:Label ID="lbl_PriceCompare_HD_Nm" runat="server" SkinID="LBL_HD_1" Text="<%$ Resources:PC_PR_Pr, lbl_PriceCompare_HD_Nm %>"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 17px; width: 100%">
                                                <asp:GridView ID="grd_PriceCompare1" runat="server" AutoGenerateColumns="False" Width="100%" EmptyDataText="No Data to Compare" EnableModelValidation="True"
                                                    SkinID="GRD_V1" ShowFooter="True">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="<%$ Resources:PC_PR_Pr, lbl_BU_Compare_Nm %>" DataField="BuCode" ItemStyle-Width="18%">
                                                            <HeaderStyle Width="18%" HorizontalAlign="Left" />
                                                            <ItemStyle Width="18%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="<%$ Resources:PC_PR_Pr, lbl_Ref_Compare_Nm %>" DataField="RefNo" ItemStyle-Width="11%">
                                                            <HeaderStyle Width="11%" HorizontalAlign="Left" />
                                                            <ItemStyle Width="11%" />
                                                        </asp:BoundField>
                                                        <%--<asp:BoundField HeaderText="Vendor" DataField="VendorCode" ItemStyle-Width="300px">
                                                                                <HeaderStyle Width="300px" HorizontalAlign="Left" />
                                                                                <ItemStyle Width="300px" />
                                                                            </asp:BoundField>--%>
                                                        <asp:BoundField HeaderText="<%$ Resources:PC_PR_Pr, lbl_VendorName_Compare_Nm %>" DataField="VendorName" ItemStyle-Width="20%">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle Width="20%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Unit" DataField="OrderUnit" ItemStyle-Width="4%">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle Width="4%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="<%$ Resources:PC_PR_Pr, lbl_Rank_Compare_Nm %>" DataField="VendorRank" ItemStyle-Width="4%">
                                                            <HeaderStyle HorizontalAlign="Right" />
                                                            <ItemStyle Width="4%" HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="<%$ Resources:PC_PR_Pr, lbl_Price_Compare_Nm %>" DataField="Price" ItemStyle-Width="7%">
                                                            <HeaderStyle HorizontalAlign="Right" />
                                                            <ItemStyle Width="7%" HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="<%$ Resources:PC_PR_Pr, lbl_DiscPercent_Compare_Nm %>" DataField="DiscountPercent" ItemStyle-Width="7%">
                                                            <HeaderStyle HorizontalAlign="Right" />
                                                            <ItemStyle Width="7%" HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="<%$ Resources:PC_PR_Pr, lbl_DiscAmt_Compare_Nm %>" DataField="DiscountAmt" ItemStyle-Width="7%">
                                                            <HeaderStyle HorizontalAlign="Right" />
                                                            <ItemStyle Width="7%" HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="<%$ Resources:PC_PR_Pr, lbl_FOC_Compare_Nm %>" DataField="FOC" ItemStyle-Width="7%">
                                                            <HeaderStyle HorizontalAlign="Right" />
                                                            <ItemStyle Width="7%" HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="<%$ Resources:PC_PR_Pr, lbl_QtyFrom_Compare_Nm %>" DataField="QtyFrom" ItemStyle-Width="7%">
                                                            <HeaderStyle HorizontalAlign="Right" />
                                                            <ItemStyle Width="7%" HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="<%$ Resources:PC_PR_Pr, lbl_QtyTo_Compare_Nm %>" DataField="QtyTo" ItemStyle-Width="7%">
                                                            <HeaderStyle HorizontalAlign="Right" />
                                                            <ItemStyle Width="7%" HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                    <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                        <tr style="height: 17px; vertical-align: top">
                                            <td class="TD_LINE_GRD" style="width: 7%;">
                                                <asp:Label ID="lbl_OnHand_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_OnHand_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td class="TD_LINE_GRD" style="width: 7%; overflow: hidden; white-space: nowrap">
                                                <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR_1" Width="90%"></asp:Label>
                                            </td>
                                            <td class="TD_LINE_GRD" style="width: 10%;">
                                                <asp:Label ID="lbl_OnOrder_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_OnOrder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td class="TD_LINE_GRD" style="width: 5%; overflow: hidden; white-space: nowrap">
                                                <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR_1" Width="90%"></asp:Label>
                                            </td>
                                            <td class="TD_LINE_GRD" style="width: 5%;">
                                                <asp:Label ID="lbl_ReOrder_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_ReOrder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td class="TD_LINE_GRD" style="width: 10%; overflow: hidden; white-space: nowrap">
                                                <asp:Label ID="lbl_ReOrder" runat="server" SkinID="LBL_NR_1" Width="90%"></asp:Label>
                                            </td>
                                            <td class="TD_LINE_GRD" style="width: 5%;">
                                                <asp:Label ID="lbl_ReStock_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_ReStock_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td class="TD_LINE_GRD" style="width: 5%; overflow: hidden; white-space: nowrap">
                                                <asp:Label ID="lbl_ReStock" runat="server" SkinID="LBL_NR_1" Width="90%"></asp:Label>
                                            </td>
                                            <td class="TD_LINE_GRD" style="width: 10%;">
                                                <asp:Label ID="lbl_LastPrice_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_LastPrice_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td class="TD_LINE_GRD" style="width: 10%; overflow: hidden; white-space: nowrap">
                                                <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_1" Width="90%"></asp:Label>
                                            </td>
                                            <td class="TD_LINE_GRD" style="width: 10%">
                                                <asp:Label ID="lbl_LastVendor_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_LastVendor_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td class="TD_LINE_GRD" style="width: 8%; overflow: hidden; white-space: nowrap" align="right">
                                                <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1" Width="130px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr style="height: 17px; vertical-align: top">
                                            <td class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_Po_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_Po_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td class="TD_LINE_GRD">
                                                <asp:HyperLink ID="lnk_Po" runat="server" SkinID="HYPL_V1"></asp:HyperLink>
                                            </td>
                                            <td class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_Ref_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_Ref_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_Ref" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_Buyer_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_Buyer_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_Buyer" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_Order_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_Order_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_Order" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_Receive_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_Receive_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_Receive" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_Price_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_Price_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td class="TD_LINE_GRD" align="right">
                                                <asp:Label ID="lbl_Price" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                        <tr style="background-color: #DADADA; height: 17px;">
                                            <td>
                                                <asp:Label ID="lbl_Comment_Detail_Nm" runat="server" SkinID="LBL_HD_1" Text="<%$ Resources:PC_PR_Pr, lbl_Comment_Detail_Nm %>"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr style="height: 17px">
                                            <td>
                                                <asp:Label ID="lbl_Comment_Detail" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <%--Total Summary --%>
    <div style="display: flex; justify-content: flex-end;">
        <uc:TotalSummary runat="server" ID="TotalSummary" />
    </div>
    <%--Approve Bar--%>
    <table id="ApproveBar" runat="server" border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td align="left">
                <asp:Button ID="btn_Split" runat="server" Text="Split & Reject" OnClick="btn_Split_Click" OnClientClick="return confirm('Do you want to split selected item(s) as new PR and reject them?')"
                    SkinID="BTN_V1" />
            </td>
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
                <%--<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">--%>
                <table border="0" cellpadding="1" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Button ID="btn_Appr" runat="server" Text="Approve" OnClick="btn_Appr_Click" SkinID="BTN_V1" />
                        </td>
                        <td>
                            <asp:Button ID="btn_Reject" runat="server" Text="Reject" SkinID="BTN_V1" OnClick="btn_Reject_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btn_SendBack" runat="server" Text="Send Back" SkinID="BTN_V1" OnClick="btn_SendBack_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <div style="text-align: right">
                <asp:CheckBox ID="chk_Approve_NoShowMessage" runat="server" AutoPostBack="true" Text="Don't show approval's result message." OnCheckedChanged="chk_Approve_NoShowMessage_CheckedChanged" />
            </div>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--ObjectDataSource--%>
    <asp:ObjectDataSource ID="ods_Store_Dt" runat="server" SelectMethod="GetList" TypeName="Blue.BL.Option.Inventory.StoreLct">
        <SelectParameters>
            <asp:ControlParameter ControlID="hf_LoginName" Name="LoginName" PropertyName="Value" Type="String" />
            <asp:ControlParameter ControlID="hf_ConnStr" Name="ConnStr" PropertyName="Value" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetList" TypeName="Blue.BL.Option.Inventory.Product" ID="ods_Product_Dt">
        <SelectParameters>
            <asp:ControlParameter ControlID="hf_ConnStr" PropertyName="Value" Name="ConnStr" Type="String"></asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource runat="server" SelectMethod="GetUnitLookup" TypeName="Blue.BL.Option.Inventory.Unit" ID="ods_Unit_Dt">
        <SelectParameters>
            <asp:ControlParameter ControlID="hf_ConnStr" PropertyName="Value" Name="connStr" Type="String"></asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ods_DeliPoint_Dt" runat="server" SelectMethod="GetLookup" TypeName="Blue.BL.Option.Inventory.DeliveryPoint" OldValuesParameterFormatString="original_{0}">
        <SelectParameters>
            <asp:ControlParameter ControlID="hf_ConnStr" Name="connStr" PropertyName="Value" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ods_Vendor_Dt" runat="server" SelectMethod="GetList" TypeName="Blue.BL.AP.Vendor" OldValuesParameterFormatString="original_{0}">
        <SelectParameters>
            <asp:ControlParameter ControlID="hf_ConnStr" Name="connStr" PropertyName="Value" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ods_PriceList" runat="server" SelectMethod="GetListHQ" TypeName="Blue.BL.IN.PriceList" OldValuesParameterFormatString="original_{0}">
        <SelectParameters>
            <asp:SessionParameter Name="dsPriceList" SessionField="dsPriceList" Type="Object" />
            <asp:ControlParameter ControlID="hf_BuGrpCode" Name="BuGrpCode" PropertyName="Value" Type="String" />
            <asp:SessionParameter Name="ProductCode" SessionField="ProductCode" Type="String" />
            <asp:SessionParameter Name="PrDate" SessionField="ReqDate" Type="DateTime" />
            <asp:SessionParameter Name="ReqQty" SessionField="ApprQty" Type="Decimal" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <%-- Options --%>
    <dx:ASPxCallbackPanel ID="CallBackPanel" runat="server" Width="100%" ClientInstanceName="CallBackPanel">
        <PanelCollection>
            <dx:PanelContent runat="server">
                <dx:ASPxPopupControl ID="pop_PrDtEdit" runat="server" CloseAction="CloseButton" HeaderText="Edit Form" Modal="True" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" ClientInstanceName="pop_PrDtEdit" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                    Style="margin-top: 0px">
                    <ContentStyle VerticalAlign="Top">
                        <Paddings Padding="1px" />
                    </ContentStyle>
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                            <table border="0" cellpadding="3" cellspacing="0" width="990">
                                <tr>
                                    <td align="right">
                                        <table border="0" cellpadding="1" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <dx:ASPxButton ID="btn_Update_Pop" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" OnClick="btn_Update_Pop_Click" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                        Text="Update" Width="80px">
                                                    </dx:ASPxButton>
                                                </td>
                                                <td>
                                                    <dx:ASPxButton ID="btn_Cancel_Pop" runat="server" CausesValidation="False" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                        Text="Cancel" Width="80px" AutoPostBack="False">
                                                        <ClientSideEvents Click="function(s, e) {
	                                                        pop_PrDtEdit.Hide();
                                                        }" />
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxPageControl ID="pc_Prdt" runat="server" ActiveTabIndex="0" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                            TabSpacing="3px" Width="100%" OnLoad="pc_Prdt_Load">
                                            <TabPages>
                                                <dx:TabPage Text="Request Information">
                                                    <ContentCollection>
                                                        <dx:ContentControl ID="ContentControl1" runat="server">
                                                            <table border="0" cellpadding="3" cellspacing="0" style="width: 100%;">
                                                                <tr>
                                                                    <td width="10%">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td width="10%">
                                                                        <asp:Label runat="server" Text="Store" Font-Bold="True" Font-Italic="False" ID="Label2"></asp:Label>
                                                                    </td>
                                                                    <td width="30%">
                                                                        <dx:ASPxComboBox ID="ddl_Store_Pop" runat="server" DataSourceID="ods_Store_Pop" IncrementalFilteringMode="Contains" ValueField="LocationCode" ValueType="System.String"
                                                                            Width="300px" AutoPostBack="True" OnSelectedIndexChanged="ddl_Store_Pop_SelectedIndexChanged" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
                                                                            LoadingPanelImagePosition="Top" ShowShadow="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" TextFormatString="{0} - {1}">
                                                                            <Columns>
                                                                                <dx:ListBoxColumn Caption="Code" FieldName="LocationCode" Width="100px" />
                                                                                <dx:ListBoxColumn Caption="Name" FieldName="LocationName" Width="250px" />
                                                                            </Columns>
                                                                            <LoadingPanelImage Url="~/App_Themes/Aqua/Editors/Loading.gif">
                                                                            </LoadingPanelImage>
                                                                            <DropDownButton>
                                                                                <Image>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
                                                                                </Image>
                                                                            </DropDownButton>
                                                                            <ValidationSettings Display="Dynamic">
                                                                                <ErrorFrameStyle ImageSpacing="4px">
                                                                                    <ErrorTextPaddings PaddingLeft="4px" />
                                                                                </ErrorFrameStyle>
                                                                                <RequiredField IsRequired="True" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxComboBox>
                                                                        <asp:ObjectDataSource ID="ods_Store_Pop" runat="server" SelectMethod="GetList" TypeName="Blue.BL.Option.Inventory.StoreLct">
                                                                            <SelectParameters>
                                                                                <asp:ControlParameter ControlID="hf_LoginName" Name="LoginName" PropertyName="Value" Type="String" />
                                                                                <asp:ControlParameter ControlID="hf_ConnStr" Name="ConnStr" PropertyName="Value" Type="String" />
                                                                            </SelectParameters>
                                                                        </asp:ObjectDataSource>
                                                                    </td>
                                                                    <td width="5%">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td width="10%">
                                                                    </td>
                                                                    <td width="35%">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label runat="server" Text="SKU" Font-Bold="True" Font-Italic="False" ID="Label3"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxComboBox runat="server" IncrementalFilteringMode="Contains" ValueType="System.String" DataSourceID="ods_Product_Pop" ValueField="ProductCode" TextFormatString="{0} - {1} - {2}"
                                                                            LoadingPanelImagePosition="Top" ShowShadow="False" Width="300px" AutoPostBack="True" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" CssPostfix="Aqua"
                                                                            CssFilePath="~/App_Themes/Aqua/{0}/styles.css" ClientInstanceName="ddl_Product" ID="ddl_Product_Pop" OnSelectedIndexChanged="ddl_Product_SelectedIndexChanged">
                                                                            <Columns>
                                                                                <dx:ListBoxColumn FieldName="ProductCode" Caption="SKU#"></dx:ListBoxColumn>
                                                                                <dx:ListBoxColumn FieldName="ProductDesc1" Caption="Description"></dx:ListBoxColumn>
                                                                                <dx:ListBoxColumn FieldName="ProductDesc2" Caption="Other Description"></dx:ListBoxColumn>
                                                                            </Columns>
                                                                            <LoadingPanelImage Url="~/App_Themes/Aqua/Editors/Loading.gif">
                                                                            </LoadingPanelImage>
                                                                            <DropDownButton>
                                                                                <Image>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua"></SpriteProperties>
                                                                                </Image>
                                                                            </DropDownButton>
                                                                            <ValidationSettings>
                                                                                <ErrorFrameStyle ImageSpacing="4px">
                                                                                    <ErrorTextPaddings PaddingLeft="4px"></ErrorTextPaddings>
                                                                                </ErrorFrameStyle>
                                                                            </ValidationSettings>
                                                                        </dx:ASPxComboBox>
                                                                        <asp:ObjectDataSource runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetLookUp_LocationCode" TypeName="Blue.BL.Option.Inventory.Product"
                                                                            ID="ods_Product_Pop">
                                                                            <SelectParameters>
                                                                                <asp:ControlParameter ControlID="ddl_Store_Pop" PropertyName="Value" Name="LocateCode" Type="String"></asp:ControlParameter>
                                                                                <asp:ControlParameter ControlID="hf_ConnStr" PropertyName="Value" Name="connStr" Type="String"></asp:ControlParameter>
                                                                            </SelectParameters>
                                                                        </asp:ObjectDataSource>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label runat="server" Text="Unit" Font-Bold="True" ID="Label4"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxComboBox runat="server" IncrementalFilteringMode="Contains" ValueType="System.String" DataSourceID="ods_Unit_Pop" ValueField="UnitCode" TextFormatString="{0}"
                                                                            LoadingPanelImagePosition="Top" ShowShadow="False" Width="100px" Enabled="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" CssPostfix="Aqua"
                                                                            CssFilePath="~/App_Themes/Aqua/{0}/styles.css" ID="ddl_Unit_Pop">
                                                                            <Columns>
                                                                                <dx:ListBoxColumn FieldName="UnitCode" Caption="Code"></dx:ListBoxColumn>
                                                                                <dx:ListBoxColumn FieldName="Name" Caption="Description"></dx:ListBoxColumn>
                                                                            </Columns>
                                                                            <LoadingPanelImage Url="~/App_Themes/Aqua/Editors/Loading.gif">
                                                                            </LoadingPanelImage>
                                                                            <DropDownButton>
                                                                                <Image>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua"></SpriteProperties>
                                                                                </Image>
                                                                            </DropDownButton>
                                                                            <ValidationSettings>
                                                                                <ErrorFrameStyle ImageSpacing="4px">
                                                                                    <ErrorTextPaddings PaddingLeft="4px"></ErrorTextPaddings>
                                                                                </ErrorFrameStyle>
                                                                            </ValidationSettings>
                                                                        </dx:ASPxComboBox>
                                                                        <asp:ObjectDataSource runat="server" SelectMethod="GetUnitLookup" TypeName="Blue.BL.Option.Inventory.Unit" ID="ods_Unit_Pop" OldValuesParameterFormatString="original_{0}">
                                                                            <SelectParameters>
                                                                                <asp:ControlParameter ControlID="hf_ConnStr" PropertyName="Value" Name="connStr" Type="String"></asp:ControlParameter>
                                                                            </SelectParameters>
                                                                        </asp:ObjectDataSource>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label runat="server" Text="Request" Font-Bold="True" Font-Italic="False" ID="Label5"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxSpinEdit runat="server" Number="0" HorizontalAlign="Right" Width="100px" Height="21px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" CssPostfix="Aqua"
                                                                            CssFilePath="~/App_Themes/Aqua/{0}/styles.css" ID="txt_ReqQty">
                                                                            <SpinButtons ShowIncrementButtons="False">
                                                                                <IncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua">
                                                                                    </SpriteProperties>
                                                                                </IncrementImage>
                                                                                <DecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua">
                                                                                    </SpriteProperties>
                                                                                </DecrementImage>
                                                                                <LargeIncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua"></SpriteProperties>
                                                                                </LargeIncrementImage>
                                                                                <LargeDecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua"></SpriteProperties>
                                                                                </LargeDecrementImage>
                                                                            </SpinButtons>
                                                                        </dx:ASPxSpinEdit>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label runat="server" Text="Delivery Date" Font-Bold="True" Font-Italic="False" ID="Label6"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxDateEdit runat="server" EditFormat="Custom" EditFormatString="dd/MM/yyyy" ShowShadow="False" Width="100px" DisplayFormatString="dd/MM/yyyy" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                                            CssPostfix="Aqua" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" ID="txt_ReqDate">
                                                                            <CalendarProperties>
                                                                                <HeaderStyle Spacing="1px"></HeaderStyle>
                                                                                <FooterStyle Spacing="17px"></FooterStyle>
                                                                            </CalendarProperties>
                                                                            <DropDownButton>
                                                                                <Image>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua"></SpriteProperties>
                                                                                </Image>
                                                                            </DropDownButton>
                                                                            <ValidationSettings>
                                                                                <ErrorFrameStyle ImageSpacing="4px">
                                                                                    <ErrorTextPaddings PaddingLeft="4px"></ErrorTextPaddings>
                                                                                </ErrorFrameStyle>
                                                                            </ValidationSettings>
                                                                        </dx:ASPxDateEdit>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label runat="server" Text="Approved" Font-Bold="True" Font-Italic="False" ID="Label7"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxSpinEdit runat="server" Number="0" HorizontalAlign="Right" Width="100px" Height="21px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" CssPostfix="Aqua"
                                                                            CssFilePath="~/App_Themes/Aqua/{0}/styles.css" ID="txt_ApprQty" AutoPostBack="True" ClientInstanceName="txt_ApprQty" OnNumberChanged="txt_ApprQty_NumberChanged">
                                                                            <SpinButtons ShowIncrementButtons="False">
                                                                                <IncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua">
                                                                                    </SpriteProperties>
                                                                                </IncrementImage>
                                                                                <DecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua">
                                                                                    </SpriteProperties>
                                                                                </DecrementImage>
                                                                                <LargeIncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua"></SpriteProperties>
                                                                                </LargeIncrementImage>
                                                                                <LargeDecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua"></SpriteProperties>
                                                                                </LargeDecrementImage>
                                                                            </SpinButtons>
                                                                        </dx:ASPxSpinEdit>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label runat="server" Text="Orderd" Font-Bold="True" Font-Italic="False" ID="Label8"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxSpinEdit runat="server" Number="0" HorizontalAlign="Right" Width="100px" Height="21px" Enabled="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                                            CssPostfix="Aqua" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" ID="txt_OrderQty">
                                                                            <SpinButtons ShowIncrementButtons="False">
                                                                                <IncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua">
                                                                                    </SpriteProperties>
                                                                                </IncrementImage>
                                                                                <DecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua">
                                                                                    </SpriteProperties>
                                                                                </DecrementImage>
                                                                                <LargeIncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua"></SpriteProperties>
                                                                                </LargeIncrementImage>
                                                                                <LargeDecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua"></SpriteProperties>
                                                                                </LargeDecrementImage>
                                                                            </SpinButtons>
                                                                        </dx:ASPxSpinEdit>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label runat="server" Text="Received" Font-Bold="True" Font-Italic="False" ID="Label14"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxSpinEdit runat="server" Number="0" HorizontalAlign="Right" Width="100px" Height="21px" Enabled="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                                            CssPostfix="Aqua" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" ID="txt_RcvQty">
                                                                            <SpinButtons ShowIncrementButtons="False">
                                                                                <IncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua">
                                                                                    </SpriteProperties>
                                                                                </IncrementImage>
                                                                                <DecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua">
                                                                                    </SpriteProperties>
                                                                                </DecrementImage>
                                                                                <LargeIncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua"></SpriteProperties>
                                                                                </LargeIncrementImage>
                                                                                <LargeDecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua"></SpriteProperties>
                                                                                </LargeDecrementImage>
                                                                            </SpinButtons>
                                                                        </dx:ASPxSpinEdit>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label runat="server" Text="Delivery Point" Font-Bold="True" Font-Italic="False" ID="Label15"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxComboBox runat="server" IncrementalFilteringMode="Contains" ValueType="System.String" DataSourceID="ods_DeliPoint_Pop" ValueField="DptCode" TextFormatString="{0} - {1}"
                                                                            LoadingPanelImagePosition="Top" ShowShadow="False" Width="300px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" CssPostfix="Aqua" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                                            ID="ddl_DeliPoint_Pop">
                                                                            <Columns>
                                                                                <dx:ListBoxColumn FieldName="DptCode" Caption="Code"></dx:ListBoxColumn>
                                                                                <dx:ListBoxColumn FieldName="Name" Caption="Description"></dx:ListBoxColumn>
                                                                            </Columns>
                                                                            <LoadingPanelImage Url="~/App_Themes/Aqua/Editors/Loading.gif">
                                                                            </LoadingPanelImage>
                                                                            <DropDownButton>
                                                                                <Image>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua"></SpriteProperties>
                                                                                </Image>
                                                                            </DropDownButton>
                                                                            <ValidationSettings>
                                                                                <ErrorFrameStyle ImageSpacing="4px">
                                                                                    <ErrorTextPaddings PaddingLeft="4px"></ErrorTextPaddings>
                                                                                </ErrorFrameStyle>
                                                                            </ValidationSettings>
                                                                        </dx:ASPxComboBox>
                                                                        <asp:ObjectDataSource ID="ods_DeliPoint_Pop" runat="server" SelectMethod="GetLookup" TypeName="Blue.BL.Option.Inventory.DeliveryPoint" OldValuesParameterFormatString="original_{0}">
                                                                            <SelectParameters>
                                                                                <asp:ControlParameter ControlID="hf_ConnStr" Name="connStr" PropertyName="Value" Type="String" />
                                                                            </SelectParameters>
                                                                        </asp:ObjectDataSource>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td valign="top">
                                                                        <asp:Label runat="server" Text="Comment" Font-Bold="True" Font-Italic="False" ID="Label34"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxMemo runat="server" Height="50px" Width="100%" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" CssPostfix="Aqua" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                                            ID="txt_Comment">
                                                                            <ValidationSettings>
                                                                                <ErrorFrameStyle ImageSpacing="4px">
                                                                                    <ErrorTextPaddings PaddingLeft="4px"></ErrorTextPaddings>
                                                                                </ErrorFrameStyle>
                                                                            </ValidationSettings>
                                                                        </dx:ASPxMemo>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </dx:ContentControl>
                                                    </ContentCollection>
                                                </dx:TabPage>
                                                <dx:TabPage Text="Buyer">
                                                    <ContentCollection>
                                                        <dx:ContentControl ID="ContentControl" runat="server">
                                                            <table border="0" cellpadding="3" cellspacing="0" style="width: 100%;">
                                                                <tr>
                                                                    <td width="10%">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td width="10%">
                                                                        <asp:Label ID="Label11" runat="server" Font-Bold="True" Font-Italic="False" Text="Buyer"></asp:Label>
                                                                    </td>
                                                                    <td width="30%">
                                                                        <dx:ASPxComboBox ID="ddl_Buyer" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" DataSourceID="ods_Buyer" IncrementalFilteringMode="Contains"
                                                                            LoadingPanelImagePosition="Top" ShowShadow="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" TextFormatString="{0} - {1}" ValueField="LoginName"
                                                                            ValueType="System.String" Width="300px" Enabled="False">
                                                                            <Columns>
                                                                                <dx:ListBoxColumn Caption="LoginName" FieldName="LoginName" Width="150px" />
                                                                                <dx:ListBoxColumn Caption="Name" FieldName="Name" Width="200px" />
                                                                            </Columns>
                                                                            <LoadingPanelImage Url="~/App_Themes/Aqua/Editors/Loading.gif">
                                                                            </LoadingPanelImage>
                                                                            <DropDownButton>
                                                                                <Image>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
                                                                                </Image>
                                                                            </DropDownButton>
                                                                            <ValidationSettings Display="Dynamic">
                                                                                <ErrorFrameStyle ImageSpacing="4px">
                                                                                    <ErrorTextPaddings PaddingLeft="4px" />
                                                                                </ErrorFrameStyle>
                                                                                <RequiredField IsRequired="True" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxComboBox>
                                                                        <asp:ObjectDataSource ID="ods_Buyer" runat="server" SelectMethod="GetList" TypeName="Blue.BL.dbo.User" OldValuesParameterFormatString="original_{0}">
                                                                            <SelectParameters>
                                                                                <asp:ControlParameter ControlID="hf_BuGrpCode" Name="BuGrpCode" PropertyName="Value" Type="String" />
                                                                            </SelectParameters>
                                                                        </asp:ObjectDataSource>
                                                                    </td>
                                                                    <td width="5%">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td width="10%">
                                                                    </td>
                                                                    <td width="35%">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </dx:ContentControl>
                                                    </ContentCollection>
                                                </dx:TabPage>
                                                <dx:TabPage Text="Vendor">
                                                    <ContentCollection>
                                                        <dx:ContentControl ID="ContentControl2" runat="server">
                                                            <table border="0" cellpadding="3" cellspacing="0" style="width: 100%;">
                                                                <tr>
                                                                    <td width="10%">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td width="10%">
                                                                        <asp:Label ID="Label12" runat="server" Font-Bold="True" Font-Italic="False" Text="Vendor"></asp:Label>
                                                                    </td>
                                                                    <td width="30%">
                                                                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" />
                                                                        <dx:ASPxComboBox ID="ddl_Vendor" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" ClientInstanceName="ddl_Vendor" CssPostfix="Aqua" LoadingPanelImagePosition="Top"
                                                                            ShowShadow="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" ValueType="System.String" Width="300px" IncrementalFilteringMode="Contains"
                                                                            TextFormatString="{0} - {1}" ValueField="VendorCode" EnableCallbackMode="True" OnItemsRequestedByFilterCondition="ddl_Vendor_OnItemsRequestedByFilterCondition_SQL"
                                                                            OnItemRequestedByValue="ddl_Vendor_OnItemRequestedByValue_SQL">
                                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { OnVendorChanged(s); }" />
                                                                            <ClientSideEvents EndCallback="function(s, e) { OnEndCallback(); }" />
                                                                            <%--OnSelectedIndexChanged="ddl_Vendor_SelectedIndexChanged"--%>
                                                                            <Columns>
                                                                                <dx:ListBoxColumn Caption="Code" FieldName="VendorCode" Width="100px" />
                                                                                <dx:ListBoxColumn Caption="Name" FieldName="VendorName" Width="200px" />
                                                                                <dx:ListBoxColumn Caption="Rank" FieldName="VendorRank" Width="50px" />
                                                                                <dx:ListBoxColumn Caption="Price" FieldName="Price" Width="100px" />
                                                                            </Columns>
                                                                            <LoadingPanelImage Url="~/App_Themes/Aqua/Editors/Loading.gif">
                                                                            </LoadingPanelImage>
                                                                            <DropDownButton>
                                                                                <Image>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
                                                                                </Image>
                                                                            </DropDownButton>
                                                                            <ValidationSettings>
                                                                                <ErrorFrameStyle ImageSpacing="4px">
                                                                                    <ErrorTextPaddings PaddingLeft="4px" />
                                                                                </ErrorFrameStyle>
                                                                            </ValidationSettings>
                                                                        </dx:ASPxComboBox>
                                                                    </td>
                                                                    <td width="5%">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td width="10%">
                                                                        <asp:Label ID="Label18" runat="server" Font-Bold="True" Font-Italic="False" Text="Qoute#"></asp:Label>
                                                                    </td>
                                                                    <td width="35%">
                                                                        <dx:ASPxTextBox ID="txt_RefNo" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                                            Width="100px" Enabled="False">
                                                                            <ValidationSettings>
                                                                                <ErrorFrameStyle ImageSpacing="4px">
                                                                                    <ErrorTextPaddings PaddingLeft="4px" />
                                                                                </ErrorFrameStyle>
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label17" runat="server" Font-Bold="True" Font-Italic="False" Text="Price"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxSpinEdit ID="txt_Price" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" Height="21px" HorizontalAlign="Right" Number="0"
                                                                            SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="100px" DisplayFormatString="#,###.##" ClientInstanceName="txt_Price">
                                                                            <SpinButtons ShowIncrementButtons="False">
                                                                                <IncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua" />
                                                                                </IncrementImage>
                                                                                <DecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua" />
                                                                                </DecrementImage>
                                                                                <LargeIncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua" />
                                                                                </LargeIncrementImage>
                                                                                <LargeDecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua" />
                                                                                </LargeDecrementImage>
                                                                            </SpinButtons>
                                                                        </dx:ASPxSpinEdit>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label16" runat="server" Font-Bold="True" Font-Italic="False" Text="FOC"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxSpinEdit ID="txt_FOCQty" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" Height="21px" HorizontalAlign="Right"
                                                                            Number="0" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="100px">
                                                                            <SpinButtons ShowIncrementButtons="False">
                                                                                <IncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua" />
                                                                                </IncrementImage>
                                                                                <DecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua" />
                                                                                </DecrementImage>
                                                                                <LargeIncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua" />
                                                                                </LargeIncrementImage>
                                                                                <LargeDecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua" />
                                                                                </LargeDecrementImage>
                                                                            </SpinButtons>
                                                                        </dx:ASPxSpinEdit>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label19" runat="server" Font-Bold="True" Text="Discount(%)"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxSpinEdit runat="server" Number="0" HorizontalAlign="Right" Width="100px" Height="21px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" CssPostfix="Aqua"
                                                                                        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" ID="txt_Discount" MaxValue="100" ClientInstanceName="txt_Discount">
                                                                                        <ClientSideEvents NumberChanged="function(s, e) { OnDiscountNumberChanged(s); }" />
                                                                                        <SpinButtons ShowIncrementButtons="False">
                                                                                            <%--OnNumberChanged="txt_Discount_NumberChanged" --%>
                                                                                            <IncrementImage>
                                                                                                <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua">
                                                                                                </SpriteProperties>
                                                                                            </IncrementImage>
                                                                                            <DecrementImage>
                                                                                                <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua">
                                                                                                </SpriteProperties>
                                                                                            </DecrementImage>
                                                                                            <LargeIncrementImage>
                                                                                                <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua"></SpriteProperties>
                                                                                            </LargeIncrementImage>
                                                                                            <LargeDecrementImage>
                                                                                                <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua"></SpriteProperties>
                                                                                            </LargeDecrementImage>
                                                                                        </SpinButtons>
                                                                                    </dx:ASPxSpinEdit>
                                                                                </td>
                                                                                <td>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td width="50px">
                                                                                    <asp:Label runat="server" Text="Amount" Font-Bold="True" Font-Italic="False" ID="Label24"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxSpinEdit runat="server" Number="0" HorizontalAlign="Right" Width="100px" Height="21px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" CssPostfix="Aqua"
                                                                                        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" ID="txt_DiscountAmt" DisplayFormatString="#,###.##" ClientInstanceName="txt_DiscountAmt" ReadOnly="True">
                                                                                        <SpinButtons ShowIncrementButtons="False">
                                                                                            <IncrementImage>
                                                                                                <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua">
                                                                                                </SpriteProperties>
                                                                                            </IncrementImage>
                                                                                            <DecrementImage>
                                                                                                <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua">
                                                                                                </SpriteProperties>
                                                                                            </DecrementImage>
                                                                                            <LargeIncrementImage>
                                                                                                <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua"></SpriteProperties>
                                                                                            </LargeIncrementImage>
                                                                                            <LargeDecrementImage>
                                                                                                <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua"></SpriteProperties>
                                                                                            </LargeDecrementImage>
                                                                                        </SpinButtons>
                                                                                    </dx:ASPxSpinEdit>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label25" runat="server" Font-Bold="True" Font-Italic="False" Text="Vendor SKU#"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="txt_VendorProdCode" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                                            Width="100px" Enabled="False">
                                                                            <ValidationSettings>
                                                                                <ErrorFrameStyle ImageSpacing="4px">
                                                                                    <ErrorTextPaddings PaddingLeft="4px" />
                                                                                </ErrorFrameStyle>
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label29" runat="server" Font-Bold="True" Font-Italic="False" Text="Tax"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxComboBox ID="ddl_TaxType" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" LoadingPanelImagePosition="Top" ShowShadow="False"
                                                                                        SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" ValueType="System.String" Width="100px" OnSelectedIndexChanged="ddl_TaxType_SelectedIndexChanged"
                                                                                        OnCallback="ddl_TaxType_Callback" ClientInstanceName="ddl_TaxType" EnableCallbackMode="True">
                                                                                        <ClientSideEvents SelectedIndexChanged="function(s,e) { OnTaxTypeChanged(s); }" />
                                                                                        <Items>
                                                                                            <dx:ListEditItem Text="None" Value="N" />
                                                                                            <dx:ListEditItem Text="Add" Value="A" />
                                                                                            <dx:ListEditItem Text="Included" Value="I" />
                                                                                        </Items>
                                                                                        <LoadingPanelImage Url="~/App_Themes/Aqua/Editors/Loading.gif">
                                                                                        </LoadingPanelImage>
                                                                                        <DropDownButton>
                                                                                            <Image>
                                                                                                <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
                                                                                            </Image>
                                                                                        </DropDownButton>
                                                                                        <ValidationSettings>
                                                                                            <ErrorFrameStyle ImageSpacing="4px">
                                                                                                <ErrorTextPaddings PaddingLeft="4px" />
                                                                                            </ErrorFrameStyle>
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxComboBox>
                                                                                </td>
                                                                                <td>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td width="50px">
                                                                                    <asp:Label ID="Label30" runat="server" Font-Bold="True" Font-Italic="False" Text="Rate"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxSpinEdit ID="txt_TaxRate" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" Height="21px" HorizontalAlign="Right"
                                                                                        Number="0" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="100px" ClientInstanceName="txt_TaxRate">
                                                                                        <ClientSideEvents NumberChanged="function(s, e) { OnTaxRateNumberChanged(s); }" />
                                                                                        <%-- OnNumberChanged="txt_TaxRate_NumberChanged" --%>
                                                                                        <SpinButtons ShowIncrementButtons="False">
                                                                                            <IncrementImage>
                                                                                                <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua" />
                                                                                            </IncrementImage>
                                                                                            <DecrementImage>
                                                                                                <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua" />
                                                                                            </DecrementImage>
                                                                                            <LargeIncrementImage>
                                                                                                <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua" />
                                                                                            </LargeIncrementImage>
                                                                                            <LargeDecrementImage>
                                                                                                <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua" />
                                                                                            </LargeDecrementImage>
                                                                                        </SpinButtons>
                                                                                    </dx:ASPxSpinEdit>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label31" runat="server" Font-Bold="True" Font-Italic="False" Text="PO#"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="txt_PONo" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                                            Width="100px" Enabled="False">
                                                                            <ValidationSettings>
                                                                                <ErrorFrameStyle ImageSpacing="4px">
                                                                                    <ErrorTextPaddings PaddingLeft="4px" />
                                                                                </ErrorFrameStyle>
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label26" runat="server" Font-Bold="True" Font-Italic="False" Text="Net"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxSpinEdit ID="txt_Net" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" Height="21px" HorizontalAlign="Right" Number="0"
                                                                                        SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="100px" DisplayFormatString="#,###.##" ClientInstanceName="txt_Net">
                                                                                        <ClientSideEvents Init="function(s, e) { OnCheckedChanged(s); }" />
                                                                                        <SpinButtons ShowIncrementButtons="False">
                                                                                            <IncrementImage>
                                                                                                <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua" />
                                                                                            </IncrementImage>
                                                                                            <DecrementImage>
                                                                                                <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua" />
                                                                                            </DecrementImage>
                                                                                            <LargeIncrementImage>
                                                                                                <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua" />
                                                                                            </LargeIncrementImage>
                                                                                            <LargeDecrementImage>
                                                                                                <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua" />
                                                                                            </LargeDecrementImage>
                                                                                        </SpinButtons>
                                                                                    </dx:ASPxSpinEdit>
                                                                                </td>
                                                                                <td>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td width="50px">
                                                                                    <asp:Label ID="Label9" runat="server" Font-Bold="True" Text="Adjust"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxCheckBox ID="chk_IsAdj" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" Font-Bold="True" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                                                        TextSpacing="2px" ClientInstanceName="IsAdj">
                                                                                        <ClientSideEvents CheckedChanged="function(s, e) { OnCheckedChanged(); }" />
                                                                                    </dx:ASPxCheckBox>
                                                                                    <%--OnCheckedChanged="chk_IsAdj_CheckedChanged"--%>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label32" runat="server" Font-Bold="True" Font-Italic="False" Text="Net A/C"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="ASPxTextBox2" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                                            Width="100px" Enabled="False">
                                                                            <ValidationSettings>
                                                                                <ErrorFrameStyle ImageSpacing="4px">
                                                                                    <ErrorTextPaddings PaddingLeft="4px" />
                                                                                </ErrorFrameStyle>
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label27" runat="server" Font-Bold="True" Font-Italic="False" Text="Tax"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxSpinEdit ID="txt_TaxAmt" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" Height="21px" HorizontalAlign="Right"
                                                                            Number="0" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="100px" DisplayFormatString="#,###.##" ClientInstanceName="txt_TaxAmt">
                                                                            <ClientSideEvents Init="function(s, e) { OnCheckedChanged(); }" />
                                                                            <SpinButtons ShowIncrementButtons="False">
                                                                                <IncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua" />
                                                                                </IncrementImage>
                                                                                <DecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua" />
                                                                                </DecrementImage>
                                                                                <LargeIncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua" />
                                                                                </LargeIncrementImage>
                                                                                <LargeDecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua" />
                                                                                </LargeDecrementImage>
                                                                            </SpinButtons>
                                                                        </dx:ASPxSpinEdit>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label33" runat="server" Font-Bold="True" Font-Italic="False" Text="Tax A/C"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="txt_TaxAC" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                                            Width="100px" Enabled="False">
                                                                            <ValidationSettings>
                                                                                <ErrorFrameStyle ImageSpacing="4px">
                                                                                    <ErrorTextPaddings PaddingLeft="4px" />
                                                                                </ErrorFrameStyle>
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label28" runat="server" Font-Bold="True" Font-Italic="False" Text="Amount"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxSpinEdit ID="txt_Amount" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" Height="21px" HorizontalAlign="Right"
                                                                            Number="0" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="100px" DisplayFormatString="#,###.##" ClientInstanceName="txt_Amount">
                                                                            <ClientSideEvents Init="function(s, e) { OnCheckedChanged(); }" />
                                                                            <SpinButtons ShowIncrementButtons="False">
                                                                                <IncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua" />
                                                                                </IncrementImage>
                                                                                <DecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua" />
                                                                                </DecrementImage>
                                                                                <LargeIncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua" />
                                                                                </LargeIncrementImage>
                                                                                <LargeDecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua" PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua" />
                                                                                </LargeDecrementImage>
                                                                            </SpinButtons>
                                                                        </dx:ASPxSpinEdit>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </dx:ContentControl>
                                                    </ContentCollection>
                                                </dx:TabPage>
                                            </TabPages>
                                            <LoadingPanelImage Url="~/App_Themes/Aqua/Web/Loading.gif">
                                            </LoadingPanelImage>
                                            <Paddings Padding="2px" PaddingLeft="5px" PaddingRight="5px" />
                                            <ContentStyle>
                                                <Border BorderColor="#AECAF0" BorderStyle="Solid" BorderWidth="1px" />
                                            </ContentStyle>
                                        </dx:ASPxPageControl>
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
    <%--Popup--%>
    <dx:ASPxPopupControl ID="pop_Alert" runat="server" HeaderText="" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Width="360px" Modal="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupAlertContentControl" runat="server">
                <div style="min-width: 320px;" align="center">
                    <asp:Label ID="lbl_PopupAlert" runat="server" Text="" SkinID="LBL_NR"></asp:Label>
                    <asp:Label ID="lbl_hide_action" runat="server" Style="display: none;"></asp:Label>
                    <asp:Label ID="lbl_hide_value" runat="server" Style="display: none;"></asp:Label>
                    <br />
                    <br />
                    <asp:Button ID="btn_PopupAlert" runat="server" SkinID="BTN_V1" Text="OK" OnClick="btn_PopupAlert_Click" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_ReqVendor" runat="server" HeaderText="<%$ Resources:PC_PR_Pr, lbl_Warning_VendorRequir_Nm %>" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" Width="360px" Modal="True">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <table width="360px">
                    <tr>
                        <td align="center">
                            <asp:Label ID="lbl_VendorRequir_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_VendorRequir_Nm %>" SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_ConfirmApprove" runat="server" HeaderText="Warning" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        Width="360px" Modal="True">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" colspan="2" height="50px">
                            <asp:Label ID="lbl_ConfirmAppr_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_ConfirmAppr_Nm %>" SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <%--<dx:ASPxButton ID="btn_ConfirmApprove" runat="server" OnClick="btn_ConfirmApprove_Click"
                                            Text="Yes" Width="50px" SkinID="BTN_N1">
                                        </dx:ASPxButton>--%>
                            <asp:Button ID="btn_ConfirmApprove" runat="server" OnClick="btn_ConfirmApprove_Click" Text="<%$ Resources:PC_PR_Pr, btn_ConfirmApprove %>" Width="50px"
                                SkinID="BTN_V1" />
                        </td>
                        <td align="left">
                            <%--<dx:ASPxButton ID="btn_CancelApprove" runat="server" OnClick="btn_CancelApprove_Click"
                                            Text="No" Width="50px" SkinID="BTN_N1">
                                        </dx:ASPxButton>--%>
                            <asp:Button ID="btn_CancelApprove" runat="server" OnClick="btn_CancelApprove_Click" Text="<%$ Resources:PC_PR_Pr, btn_CancelApprove %>" Width="50px" SkinID="BTN_V1" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_Approve" runat="server" HeaderText="Information" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Width="360px"
        CloseAction="CloseButton" Modal="True" ShowCloseButton="False">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center">
                            <br />
                            <asp:Label ID="lbl_Approve_Chk" runat="server" SkinID="LBL_NR"></asp:Label>
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <%--<dx:ASPxButton ID="btn_OK_PopApprove" runat="server" OnClick="btn_OK_PopApprove_Click"
                                            Text="OK" SkinID="BTN_N1">
                                        </dx:ASPxButton>--%>
                            <asp:Button ID="btn_OK_PopApprClose" runat="server" OnClick="btn_OK_PopApprClose_Click" Text="<%$ Resources:PC_PR_Pr, btn_OK_PopApprove %>" SkinID="BTN_V1"
                                Width="50px" Visible="false" />
                            <asp:Button ID="btn_OK_PopApprFunction" runat="server" OnClick="btn_OK_PopApprFunction_Click" Text="<%$ Resources:PC_PR_Pr, btn_OK_PopApprove %>" SkinID="BTN_V1"
                                Width="50px" Visible="false" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_ConfirmReject" runat="server" CloseAction="CloseButton" HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Width="300px">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" colspan="2" height="50px">
                            <asp:Label ID="lbl_ConfirmReject_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_ConfirmReject_Nm %>" SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <%--<dx:ASPxButton ID="btn_ConfirmReject" runat="server" OnClick="btn_ConfirmReject_Click"
                                            Text="Yes" Width="50px" SkinID="BTN_N1">
                                        </dx:ASPxButton>--%>
                            <asp:Button ID="btn_ConfirmReject" runat="server" OnClick="btn_ConfirmReject_Click" Text="<%$ Resources:PC_PR_Pr, btn_ConfirmReject %>" Width="50px" SkinID="BTN_V1" />
                        </td>
                        <td align="left">
                            <%--<dx:ASPxButton ID="btn_CancelReject" runat="server" OnClick="btn_CancelReject_Click"
                                            Text="No" Width="50px" SkinID="BTN_N1">
                                        </dx:ASPxButton>--%>
                            <asp:Button ID="btn_CancelReject" runat="server" OnClick="btn_CancelReject_Click" Text="<%$ Resources:PC_PR_Pr, btn_CancelReject %>" Width="50px" SkinID="BTN_V1" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_ConfirmAutoAlloVd" runat="server" HeaderText="Warning" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        Width="360px" Modal="True">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" colspan="2" height="50px">
                            <asp:Label ID="lbl_AutoAllo_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_AutoAllo_Nm %>" SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <%--<dx:ASPxButton ID="btn_ConfirmAutoAlloVd" runat="server" OnClick="btn_ConfirmAutoAlloVd_Click"
                                            Text="Yes" Width="50px" SkinID="BTN_N1">
                                        </dx:ASPxButton>--%>
                            <asp:Button ID="btn_ConfirmAutoAlloVd" runat="server" OnClick="btn_ConfirmAutoAlloVd_Click" Text="<%$ Resources:PC_PR_Pr, btn_ConfirmAutoAlloVd %>" Width="50px"
                                SkinID="BTN_V1" />
                        </td>
                        <td align="left">
                            <%--<dx:ASPxButton ID="btn_CancelAutoAlloVd" runat="server" OnClick="btn_CancelAutoAlloVd_Click"
                                            Text="No" Width="50px" SkinID="BTN_N1">
                                        </dx:ASPxButton>--%>
                            <asp:Button ID="btn_CancelAutoAlloVd" runat="server" OnClick="btn_CancelAutoAlloVd_Click" Text="<%$ Resources:PC_PR_Pr, btn_CancelAutoAlloVd %>" Width="50px"
                                SkinID="BTN_V1" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_AutoAlloVd" runat="server" HeaderText="Information" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        Width="360px" CloseAction="CloseButton" Modal="True" ShowCloseButton="False">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center">
                            <br />
                            <asp:Label ID="lbl_AutoAlloSuccess_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_AutoAlloSuccess_Nm %>" SkinID="LBL_NR"></asp:Label>
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <%--<dx:ASPxButton ID="btn_OK_PopAutoAlloVd" runat="server" OnClick="btn_OK_PopAutoAlloVd_Click"
                                            Text="OK" SkinID="BTN_N1">
                                        </dx:ASPxButton>--%>
                            <asp:Button ID="btn_OK_PopAutoAlloVd" runat="server" OnClick="btn_OK_PopAutoAlloVd_Click" Text="<%$ Resources:PC_PR_Pr, btn_OK_PopAutoAlloVd %>" SkinID="BTN_V1"
                                Width="50px" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_Reject" runat="server" HeaderText="Reject" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Width="480px"
        ClientInstanceName="pop_Reject" CloseAction="CloseButton" Modal="True">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <table border="0" cellpadding="3" cellspacing="1" style="width: 100%;">
                    <tr>
                        <td valign="top">
                        </td>
                        <td>
                            <asp:Label ID="lbl_Comment_POP_Nm" runat="server" Font-Bold="True" Font-Italic="False" Text="<%$ Resources:PC_PR_Pr, lbl_Comment_POP_Nm %>" SkinID="LBL_HD"></asp:Label>
                            <dx:ASPxMemo ID="txt_RejectMessage" runat="server" Height="100px" Width="300px">
                            </dx:ASPxMemo>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <table border="0" cellpadding="1" cellspacing="0">
                                <tr>
                                    <td>
                                        <%--<dx:ASPxButton ID="btn_Reject_OK" runat="server" Text="OK" Width="75px" OnClick="btn_Reject_OK_Click"
                                                        SkinID="BTN_N1">
                                                    </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_Reject_OK" runat="server" Text="<%$ Resources:PC_PR_Pr, btn_Reject_OK %>" Width="75px" OnClick="btn_Reject_OK_Click" SkinID="BTN_V1" />
                                    </td>
                                    <td>
                                        <%--<dx:ASPxButton ID="btn_Reject_Cancel" runat="server" AutoPostBack="False" Text="Cancel"
                                                        Width="75px" OnClick="btn_Reject_Cancel_Click" SkinID="BTN_N1">
                                                        <ClientSideEvents Click="function(s, e) {
	                                                        pop_Reject.Hide();
                                                        }" />
                                                    </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_Reject_Cancel" runat="server" Text="<%$ Resources:PC_PR_Pr, btn_Reject_Cancel %>" Width="75px" OnClick="btn_Reject_Cancel_Click" SkinID="BTN_V1" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_SendBack" runat="server" HeaderText="Send Back" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="pop_SendBack"
        CloseAction="CloseButton" Modal="True">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <table border="0" cellpadding="3" cellspacing="1" style="width: 100%;">
                    <tr>
                        <td>
                            <asp:Label ID="lbl_SendTo_Nm" runat="server" Font-Bold="True" Font-Italic="False" Text="<%$ Resources:PC_PR_Pr, lbl_SendTo_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td>
                            <dx:ASPxComboBox ID="ddl_SendBack" runat="server" DataSourceID="ods_SendBack" TextField="StepDesc" ValueField="Step" ValueType="System.String" Width="300px">
                            </dx:ASPxComboBox>
                            <asp:ObjectDataSource ID="ods_SendBack" runat="server" SelectMethod="GetSendBackStep" TypeName="Blue.BL.APP.WFDt" OldValuesParameterFormatString="original_{0}">
                                <SelectParameters>
                                    <asp:Parameter Name="wfId" Type="Int32" />
                                    <asp:Parameter Name="currenStep" Type="Int32" />
                                    <asp:Parameter Name="prNo" Type="String" />
                                    <asp:ControlParameter ControlID="hf_ConnStr" Name="connStr" PropertyName="Value" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lbl_CommentSendBack_Nm" runat="server" Font-Bold="True" Font-Italic="False" Text="<%$ Resources:PC_PR_Pr, lbl_CommentSendBack_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td>
                            <dx:ASPxMemo ID="txt_SendBackMessage" runat="server" Height="100px" Width="300px">
                            </dx:ASPxMemo>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                            <asp:Label ID="lbl_SendBack_Test" runat="server"></asp:Label>
                        </td>
                        <td>
                            <table border="0" cellpadding="1" cellspacing="0">
                                <tr>
                                    <td>
                                        <%--<dx:ASPxButton ID="btn_SandBack_OK" runat="server" Text="OK" Width="60px" OnClick="btn_SandBack_OK_Click"
                                                        SkinID="BTN_N1">
                                                    </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_SandBack_OK" runat="server" Text="<%$ Resources:PC_PR_Pr, btn_SandBack_OK %>" Width="60px" OnClick="btn_SandBack_OK_Click" SkinID="BTN_V1" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_SendBack_Cancel" runat="server" Text="<%$ Resources:PC_PR_Pr, btn_SendBack_Cancel %>" Width="60px" SkinID="BTN_V1" OnClick="btn_SendBack_Cancel_Click" />
                                        <%--<dx:ASPxButton ID="btn_SendBack_Cancel" runat="server" AutoPostBack="False" Text="Cancel"
                                                        Width="70px" SkinID="BTN_N1">
                                                        <ClientSideEvents Click="function(s, e) {
	                                                        pop_SendBack.Hide();
                                                        }" />
                                                    </dx:ASPxButton>--%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_ConfirmSendback" runat="server" HeaderText="<%$ Resources:PC_PR_Pr, lbl_ConfirmSendBack_HD %>" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" Width="300px" Modal="True">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" colspan="2" height="50px">
                            <%--<asp:Label ID="Label59" runat="server" Text="Send back to Allocate Vendor ?"></asp:Label>--%>
                            <asp:Label ID="lbl_Sendback" runat="server" SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <%--<dx:ASPxButton ID="btn_ConfirmSendback" runat="server" OnClick="btn_ConfirmSendback_Click"
                                            Text="Yes" Width="60px" SkinID="BTN_N1">
                                        </dx:ASPxButton>--%>
                            <asp:Button ID="btn_ConfirmSendback" runat="server" OnClick="btn_ConfirmSendback_Click" Text="<%$ Resources:PC_PR_Pr, btn_ConfirmSendback %>" Width="60px"
                                SkinID="BTN_V1" />
                        </td>
                        <td align="left">
                            <%--<dx:ASPxButton ID="btn_CancelSendback" runat="server" OnClick="btn_CancelSendback_Click"
                                            Text="No" Width="60px" SkinID="BTN_N1">
                                        </dx:ASPxButton>--%>
                            <asp:Button ID="btn_CancelSendback" runat="server" OnClick="btn_CancelSendback_Click" Text="<%$ Resources:PC_PR_Pr, btn_CancelSendback %>" Width="60px"
                                SkinID="BTN_V1" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_ConfirmVoid" ClientInstanceName="pop_ConfirmVoid" runat="server" CloseAction="CloseButton" HeaderText="<%$ Resources:IN_STK_StkInDt, pop_ConfirmVoid %>"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Width="300px">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" colspan="2" height="50px">
                            <asp:Label ID="lbl_ConfirmVoid_Nm" runat="server" Text="<%$ Resources:IN_STK_StkInDt, lbl_ConfirmVoid_Nm %>" SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Button ID="btn_ConfirmVoid" runat="server" Text="<%$ Resources:IN_STK_StkInDt, btn_ConfirmVoid %>" OnClick="btn_ConfirmVoid_Click" SkinID="BTN_V1"
                                Width="60px" />
                        </td>
                        <td align="left">
                            <asp:Button ID="btn_CancelVoid" runat="server" Text="<%$ Resources:IN_STK_StkInDt, btn_CancelVoid %>" OnClick="btn_CancelVoid_Click" SkinID="BTN_V1" Width="60px" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_Void" runat="server" HeaderText="<%$ Resources:IN_STK_StkInDt, pop_Void %>" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        Width="300px" Modal="True" ShowCloseButton="False">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" height="50px">
                            <asp:Label ID="lbl_VoidSuc_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_VoidSuc_Nm %>" SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btn_Void_Success" runat="server" Text="<%$ Resources:PC_PR_Pr, btn_OKApprove_Succ %>" SkinID="BTN_V1" Width="60px" OnClick="btn_Void_Success_Click" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_OKApprove_Succ" runat="server" HeaderText="Warning" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        Width="300px" Modal="True" ShowCloseButton="False">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl6" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" height="50px">
                            <asp:Label ID="lbl_OKApprove_Succ_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_OKApprove_Succ_Nm %>" SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btn_OKApprove_Succ" runat="server" Text="<%$ Resources:PC_PR_Pr, btn_OKApprove_Succ %>" SkinID="BTN_V1" Width="60px" OnClick="btn_OKApprove_Succ_Click" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_ProductLog" runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" HeaderText="<%$ Resources:PC_PR_Pr, lbl_HisOfOrder_Nm %>"
        Width="650px" CloseAction="CloseButton" Modal="True">
        <ContentStyle HorizontalAlign="Center" VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                <table border="0" cellpadding="2" cellspacing="0" width="100%">
                    <tr>
                        <td align="left">
                            <asp:Label ID="HD1" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_CreateBy_Grd_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox1" runat="server" SkinID="TXT_V1" Width="180px" Text="demo@blueledgers.com" Enabled="False"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:Label ID="Label23" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_CreateOn_Grd_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <dx:ASPxDateEdit ID="ASPxDateEdit1" runat="server" Width="100px" Font-Names="Arial,Tahoma,MS Sans serif" Font-Size="8pt" Date="2011-08-22" DisplayFormatString="dd/MM/yyyy"
                                EditFormat="Custom" Enabled="False">
                            </dx:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label35" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_ApproBy_Grd_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox2" runat="server" SkinID="TXT_V1" Width="180px" Enabled="False"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:Label ID="Label39" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_ApprOn_Grd_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <dx:ASPxDateEdit ID="ASPxDateEdit2" runat="server" Width="100px" Font-Names="Arial,Tahoma,MS Sans serif" Font-Size="8pt" Enabled="False">
                            </dx:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label41" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_AllBuyer_Grd_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox3" runat="server" SkinID="TXT_V1" Width="180px" Enabled="False"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:Label ID="Label44" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_BuyAllOn_Grd_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <dx:ASPxDateEdit ID="ASPxDateEdit3" runat="server" Width="100px" Font-Names="Arial,Tahoma,MS Sans serif" Font-Size="8pt" Enabled="False">
                            </dx:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label51" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_AllVendor_Grd_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox4" runat="server" SkinID="TXT_V1" Width="180px" Enabled="False"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:Label ID="Label53" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_VendorAllOn_Grd_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <dx:ASPxDateEdit ID="ASPxDateEdit4" runat="server" Width="100px" Font-Names="Arial,Tahoma,MS Sans serif" Font-Size="8pt" Enabled="False">
                            </dx:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr style="height: 40px">
                        <td align="left">
                            <asp:Label ID="Label72" runat="server" SkinID="LBL_HD" Text="<%$ Resources:PC_PR_Pr, lbl_VendorApprBy_Grd_Nm %>"></asp:Label>
                        </td>
                        <td align="left">
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:Label ID="Label67" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_VendorApprOn_Grd_Nm %>" SkinID="LBL_HD"></asp:Label>
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
                            <asp:Label ID="Label69" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_FirstAuBy_Grd_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox6" runat="server" SkinID="TXT_V1" Width="180px" Enabled="False"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:Label ID="Label20" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_FIrstAuOn_Grd_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <dx:ASPxDateEdit ID="ASPxDateEdit6" runat="server" Width="100px" Font-Names="Arial,Tahoma,MS Sans serif" Font-Size="8pt" Enabled="False">
                            </dx:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label21" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_SecondAuBy_Grd_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox7" runat="server" SkinID="TXT_V1" Width="180px" Enabled="False"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:Label ID="Label71" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_SecondAuOn_Grd_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <dx:ASPxDateEdit ID="ASPxDateEdit7" runat="server" Width="100px" Font-Names="Arial,Tahoma,MS Sans serif" Font-Size="8pt" Enabled="False">
                            </dx:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label72000" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_ThirdAuBy_Grd_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox8" runat="server" SkinID="TXT_V1" Width="180px" Enabled="False"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:Label ID="Label77" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_ThirdAuOn_Grd_Nm %>" SkinID="LBL_HD"></asp:Label>
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
    <dx:ASPxPopupControl ID="pop_SendMsg" runat="server" HeaderText="<%$ Resources:PC_PR_Pr, lbl_SendMsg_HD %>" Width="500px" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" Modal="True">
        <ContentStyle HorizontalAlign="Center" VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" SupportsDisabledAttribute="True">
                <table border="0" cellpadding="3" cellspacing="0" width="100%">
                    <tr>
                        <td align="left" style="width: 44px">
                            <asp:Button ID="btn_To" runat="server" Text="<%$ Resources:PC_PR_Pr, btn_To %>" SkinID="BTN_V1" Width="60px" />
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox10" runat="server" SkinID="TXT_V1" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 44px">
                            <asp:Button ID="btn_CC" runat="server" Text="<%$ Resources:PC_PR_Pr, btn_CC %>" SkinID="BTN_V1" Width="60px" />
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox9" runat="server" SkinID="TXT_V1" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 44px">
                            <asp:Button ID="Button3" runat="server" Text="<%$ Resources:PC_PR_Pr, Button3 %>" SkinID="BTN_V1" Width="60px" />
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBox5" runat="server" SkinID="TXT_V1" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 44px">
                            <asp:Label ID="lbl_Subject_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_Subject_Nm %>" SkinID="LBL_HD" Width="60px"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txt_Subject" runat="server" SkinID="TXT_V1" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lbl_MsgText_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_MsgText_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:TextBox ID="txt_Msg" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="2">
                            <asp:Button ID="btn_Cancel" runat="server" Text="<%$ Resources:PC_PR_Pr, btn_Cancel %>" SkinID="BTN_V1" OnClick="btn_Cancel_Click" />
                            <asp:Button ID="btn_Send" runat="server" Text="<%$ Resources:PC_PR_Pr, btn_Send %>" SkinID="BTN_V1" OnClick="btn_Send_Click" />
                            <asp:Button ID="btn_Exit" runat="server" Text="<%$ Resources:PC_PR_Pr, btn_Exit %>" SkinID="BTN_V1" OnClick="btn_Exit_Click" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
