<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="PrEdit.aspx.cs" Inherits="BlueLedger.PL.PC.PR.PrEdit" %>

<%@ MasterType VirtualPath="~/Master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Src="../../UserControl/ProcessStatus.ascx" TagName="ProcessStatus" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .flex
        {
            display: flex !important;
        }
        
        .flex-justify-content-start
        {
            justify-content: flex-start;
        }
        .flex-justify-content-end
        {
            justify-content: flex-end;
        }
        .flex-justify-content-center
        {
            justify-content: center;
        }
        .flex-justify-content-between
        {
            justify-content: space-between;
        }
        .flex-row
        {
            flex-flow: row !important;
        }
        .flex-columm
        {
            flex-flow: column !important;
        }
        
        .flex-wrap
        {
            flex-wrap: wrap !important;
        }
        .ms-10
        {
            margin-left: 10px;
        }
        .ms-20
        {
            margin-left: 20px;
        }
        .ms-30
        {
            margin-left: 30px;
        }
        .me-10
        {
            margin-right: 10px;
        }
        .me-20
        {
            margin-right: 20px;
        }
        .me-30
        {
            margin-right: 30px;
        }
        .mt-10
        {
            margin-top: 10px;
        }
        .mt-20
        {
            margin-top: 20px;
        }
        .mt-30
        {
            margin-top: 30px;
        }
        .mb-10
        {
            margin-bottom: 10px;
        }
        .mb-20
        {
            margin-bottom: 20px;
        }
        .mb-30
        {
            margin-bottom: 30px;
        }
        .width-100
        {
            width: 100% !important;
        }
        
        .bg-menu-background
        {
            background-color: #4d4d4d !important;
            color: White !important;
        }
        .bg-sub
        {
            background-color: #DADADA !important;
            color: White !important;
        }
    </style>
    <script type="text/javascript">
        function GoTop() {
            window.scrollTo(0, 0);
        }

        function OnCheckedChanged() {
            txt_Net.SetEnabled(IsAdj.GetChecked());
            txt_TaxAmt.SetEnabled(IsAdj.GetChecked());
            txt_Amount.SetEnabled(IsAdj.GetChecked());
        }

        var DiscountPercent = 0;
        var DiscountAmt = 0;


        //Check Select All CheckBox.
        function Check(parentChk) {
            var elements = document.getElementsByTagName("input");
            for (i = 0; i < elements.length; i++) {
                if (parentChk.checked == true) {
                    if (IsCheckBox(elements[i])) {
                        elements[i].checked = true;
                    }
                }
                else {
                    elements[i].checked = false;
                }
            }
        }

        function IsCheckBox(chk) {
            if (chk.type == 'checkbox') return true;
            else return false;
        }

        // Expand Gridview
        function expandDetailsInGrid(_this) {
            var id = _this.id;
            var imgelem = document.getElementById(_this.id);
            var currowid = id.replace("Img_Btn1", "TR_Summary")

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hf_ConnStr" runat="server" />
            <!-- Menu Main -->
            <div class="flex flex-justify-content-between width-100 bg-menu-background" style="height: 26px;">
                <div class="flex flex-align-items-center ms-10">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                    &nbsp;&nbsp;
                    <asp:Label ID="lbl_Title_Nm" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:PC_PR_PrEdit, lbl_Title_Nm %>" />
                    &nbsp;&nbsp;
                    <asp:Label ID="lbl_ViewName" runat="server" SkinID="LBL_HD_WHITE" />
                </div>
                <div class="flex flex-justify-content-end">
                    <dx:ASPxMenu runat="server" ID="menu_Header" Font-Bold="True" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle"
                        Height="16px" OnItemClick="menu_Header_ItemClick">
                        <ItemStyle BackColor="Transparent">
                            <HoverStyle BackColor="Transparent">
                                <Border BorderStyle="None" />
                            </HoverStyle>
                            <Paddings Padding="2px" />
                            <Border BorderStyle="None" />
                        </ItemStyle>
                        <Items>
                            <dx:MenuItem Name="Save" Text="">
                                <ItemStyle Height="16px" Width="42px">
                                    <HoverStyle>
                                        <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png" Repeat="NoRepeat" VerticalPosition="center" />
                                    </HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/save.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </ItemStyle>
                            </dx:MenuItem>
                            <dx:MenuItem Name="Commit" Text="" Visible="false">
                                <ItemStyle Height="16px" Width="42px">
                                    <HoverStyle>
                                        <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-commit.png" Repeat="NoRepeat" VerticalPosition="center" />
                                    </HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/commit.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </ItemStyle>
                            </dx:MenuItem>
                            <dx:MenuItem Name="Back" Text="">
                                <ItemStyle Height="16px" Width="42px">
                                    <HoverStyle>
                                        <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png" Repeat="NoRepeat" VerticalPosition="center" />
                                    </HoverStyle>
                                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                </ItemStyle>
                            </dx:MenuItem>
                        </Items>
                    </dx:ASPxMenu>
                </div>
            </div>
            <!-- Header -->
            <table class="width-100">
                <tr style="height: 30px;">
                    <td style="width: 5%;">
                        <asp:Label ID="lbl_PrNo_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Ref_Nm0 %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td style="width: 20%">
                        <asp:Label ID="lbl_PrNo" runat="server" Width="80%" SkinID="txt_V1" />
                    </td>
                    <td style="width: 5%">
                        <asp:Label ID="lbl_Date" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Date %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td style="width: 20%">
                        <dx:ASPxDateEdit runat="server" ID="date_PrDate" SkinID="TXT_V1" Width="80%" AutoPostBack="true" Enabled="false" />
                    </td>
                    <td style="width: 5%">
                        <asp:Label ID="lbl_Department_Nm" runat="server" Text="Department" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td style="width: 20%">
                        <asp:Label ID="lbl_Department" runat="server" Text="Department" SkinID="LBL_NR"></asp:Label>
                    </td>
                    <td style="width: 5%">
                        <asp:Label ID="lbl_ProcessStatus_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Process_Nm0 %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td style="width: 20%">
                        <uc1:ProcessStatus ID="ProcessStatus" runat="server" Font-Size="9pt" Font-Names="arial" ForeColor="#4d4d4d" />
                    </td>
                </tr>
                <tr style="height: 30px">
                    <td>
                        <asp:Label ID="lbl_PrType_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_PrType_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td>
                        <dx:ASPxComboBox ID="ddl_PrType" runat="server" Width="80%" />
                    </td>
                    <td>
                        <asp:Label ID="lbl_JobCode_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:PC_PR_PrEdit, lbl_JobCode_Nm %>"></asp:Label>
                    </td>
                    <td>
                        <dx:ASPxComboBox ID="ddl_JobCode" runat="server" Width="80%" EnableCallbackMode="true" CallbackPageSize="25">
                        </dx:ASPxComboBox>
                    </td>
                    <td>
                        <asp:Label ID="lbl_Requestor_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Requestor_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_Requestor" runat="server" SkinID="LBL_NR"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_Status_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Status_Nm %>" SkinID="LBL_HD" />
                    </td>
                    <td>
                        <asp:Label ID="lbl_Status" runat="server" Width="80%" />
                    </td>
                </tr>
                <tr style="height: 30px">
                    <td>
                        <asp:Label ID="lbl_Desc_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Desc_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td colspan="7">
                        <asp:TextBox ID="txt_Desc" runat="server" SkinID="txt_V1" Width="100%"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <!-- Menu Detail -->
            <div class="flex flex-justify-content-between width-100 bg-menu-background" style="height: 26px;">
                <div class="flex flex-align-items-center ms-10">
                    <asp:Label ID="lbl_ChangeDeliDate_Nm" runat="server" Width="130" ForeColor="White" Text="<%$ Resources:PC_PR_PrEdit, lbl_ChangeDeliDate_Nm %>" />
                    &nbsp;&nbsp;
                    <dx:ASPxDateEdit ID="date_DeliveryDate" runat="server" Width="120" />
                    &nbsp;&nbsp;
                    <asp:Button runat="server" ID="btn_Apply_DeliveryDate" Width="60" Text="Apply" OnClick="btn_Apply_DeliveryDate_Click" />
                </div>
                <div class="flex flex-justify-content-end">
                    <dx:ASPxMenu runat="server" ID="menu_Detail" Font-Bold="True" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle"
                        Height="16px" OnItemClick="menu_Detail_ItemClick">
                        <ItemStyle BackColor="Transparent">
                            <HoverStyle BackColor="Transparent">
                                <Border BorderStyle="None" />
                            </HoverStyle>
                            <Paddings Padding="2px" />
                            <Border BorderStyle="None" />
                        </ItemStyle>
                        <Items>
                            <dx:MenuItem Name="Create" Text="" Visible="true">
                                <ItemStyle Height="16px" Width="49px">
                                    <HoverStyle>
                                        <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png" Repeat="NoRepeat" VerticalPosition="center" />
                                    </HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/create.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </ItemStyle>
                            </dx:MenuItem>
                            <dx:MenuItem Name="Delete" Text="">
                                <ItemStyle Height="16px" Width="47px">
                                    <HoverStyle>
                                        <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png" Repeat="NoRepeat" VerticalPosition="center" />
                                    </HoverStyle>
                                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                </ItemStyle>
                            </dx:MenuItem>
                        </Items>
                        <Paddings Padding="0px" />
                        <SeparatorPaddings Padding="0px" />
                        <SubMenuStyle HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" Font-Size="9pt" ForeColor="#4D4D4D" />
                        <Border BorderStyle="None"></Border>
                    </dx:ASPxMenu>
                </div>
            </div>
            <!-- Detail -->
            <asp:GridView ID="gv_PrDt" runat="server" Width="100%" SkinID="GRD_V1" AutoGenerateColumns="False" OnRowDataBound="gv_PrDt_RowDataBound" OnRowCommand="gv_PrDt_RowCommand"
                OnRowEditing="gv_PrDt_RowEditing" OnRowUpdating="gv_PrDt_RowUpdating" OnRowCancelingEdit="gv_PrDt_RowCancelingEdit" OnRowDeleting="gv_PrDt_RowDeleting">
                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" Height="24" />
                <RowStyle HorizontalAlign="Left" VerticalAlign="Top" Height="40" />
                <Columns>
                    <%-- Expand Button --%>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:ImageButton ID="Img_Btn0" runat="server" ImageUrl="~/App_Themes/Default/Images/Plus_1.jpg" CommandName="Create" ToolTip="Create" Visible="False" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:ImageButton ID="Img_Btn1" runat="server" ImageUrl="~/App_Themes/Default/Images/Plus_1.jpg" OnClientClick="expandDetailsInGrid(this);return false;" />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10px" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10px" />
                    </asp:TemplateField>
                    <%-- <asp:TemplateField>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10px" />
                        <HeaderTemplate>
                            <asp:ImageButton ID="Img_Btn0" runat="server" ImageUrl="~/App_Themes/Default/Images/Plus_1.jpg" CommandName="Create" ToolTip="Create" Visible="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:ImageButton ID="Img_Btn1" runat="server" ImageUrl="~/App_Themes/Default/Images/Plus_1.jpg" OnClientClick="expandDetailsInGrid(this); return false;" />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <%-- CheckBox --%>
                    <asp:TemplateField>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10px" />
                        <HeaderTemplate>
                            <asp:CheckBox ID="Chk_All" runat="server" onclick="Check(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="Chk_Item" runat="server" />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="10px" />
                        <ItemStyle HorizontalAlign="Center" Width="10px" />
                    </asp:TemplateField>
                    <%-- Vendor --%>
                    <asp:TemplateField HeaderText="Vendor">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_Vendor" />
                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- Location --%>
                    <asp:TemplateField HeaderText="Location">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_Location" />
                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- Product --%>
                    <asp:TemplateField HeaderText="Product">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_Product" />
                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- Unit --%>
                    <asp:TemplateField HeaderText="Unit">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_Unit" />
                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- Price --%>
                    <asp:TemplateField HeaderText="Price">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_Price" />
                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- Request --%>
                    <asp:TemplateField HeaderText="Request Qty.">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_ReqQty" />
                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- Approve --%>
                    <asp:TemplateField HeaderText="Approve Qty.">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_AppQty" />
                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- FOC --%>
                    <asp:TemplateField HeaderText="FOC Qty.">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_FocQty" />
                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- Total Currency --%>
                    <asp:TemplateField HeaderText="Total">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_CurrTotalAmt" />
                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- Total Base --%>
                    <asp:TemplateField HeaderText="Total (Base)">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_TotalAmt" />
                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- Delivery --%>
                    <asp:TemplateField HeaderText="Delivery">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_Delivery" />
                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- Process --%>
                    <asp:TemplateField HeaderText="Process">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_Process" />
                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- -------------------- --%>
                    <%-- Expandable Data --%>
                    <%-- Summary --%>
                    <%-- -------------------- --%>
                    <asp:TemplateField HeaderText="" Visible="true" HeaderStyle-Width="0%">
                        <ItemTemplate>
                            <tr id="TR_Summary" runat="server" style="display: none;">
                                <td id="TD_Summary" class="td-summary">
                                    <div style="width: 100%; padding: 5px; margin-top: 5px; margin-bottom: 5px; border: 1px solid silver;">
                                        <div class="flex" style="flex-wrap: wrap;">
                                            <div>
                                            </div>
                                            <div>
                                            </div>
                                        </div>
                                        <!-- Comment -->
                                        <div class="width-100 bg-sub ps-10">
                                            <asp:Label ID="lbl_Comment_Dt_Nm" runat="server" SkinID="LBL_HD_1" Text="<%$ Resources:PC_PR_PrEdit, lbl_Comment_Detail_Nm %>" />
                                        </div>
                                        <div class="mb-10">
                                            <asp:Label ID="lbl_Comment_Dt" runat="server" SkinID="LBL_NR_1" />
                                        </div>
                                        <hr style="margin-right: 0px;" />
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <!-- Popup(s) -->
            <dx:ASPxPopupControl runat="server" ID="pop_Alert" ClientInstanceName="pop_Alert" Width="400" HeaderText="Warning" ShowCloseButton="true" Modal="True"
                ShowPageScrollbarWhenModal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl_Alert" runat="server">
                        <div class="flex flex-justify-content-center">
                            <asp:Label runat="server" ID="lbl_Alert" />
                        </div>
                        <br />
                        <br />
                        <div class="flex flex-justify-content-center">
                            <asp:Button runat="server" ID="btn_Alert_Ok" Width="80" Text="Ok" OnClientClick="pop_Alert.Hide()" />
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="menu_Header" EventName="ItemClick" />
            <asp:AsyncPostBackTrigger ControlID="menu_Detail" EventName="ItemClick" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UdPgDetail" runat="server" AssociatedUpdatePanelID="UpdatePanel">
        <ProgressTemplate>
            <div class="fix-layout" style="border-style: solid; border-width: 1px; border-color: #0071BD; background-color: #FFFFFF; width: 120px; height: 60px">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 120px; height: 60px">
                    <tr>
                        <td align="center">
                            <asp:Image ID="img_Loading2" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/ajax-loader.gif" EnableViewState="False" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lbl_Loading2" runat="server" Font-Bold="true" Text="Loading..." EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <script type="text/javascript">
        $(document).ready(function () {
            // Init
            var col_span = $('#'.concat('<%= gv_PrDt.ClientID %>')).children('tbody').children('tr').children('td').length;

            $('.td-summary').attr('colspan', col_span);

            // Event(s)


            // Method(s)
        });
    </script>
</asp:Content>
