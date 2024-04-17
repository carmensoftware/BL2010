<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="PrEdit.aspx.cs" Inherits="BlueLedger.PL.PC.PR.PrEdit" %>

<%@ MasterType VirtualPath="~/Master/In/SkinDefault.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Src="../../UserControl/ProcessStatus.ascx" TagName="ProcessStatus" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <style type="text/css">
        .hidden
        {
            visibility: hidden;
            display: none;
            width: 0px;
        }
        .show
        {
            visibility: visible;
            display: table-cell;
            width: auto;
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


        //Select One Row at a Time
        function CheckOtherIsCheckedByGVID(spanChk) {
            var CurrentRdbID = spanChk.id;
            var Chk = spanChk;
            Parent = document.getElementById('<%= grd_Vendor.ClientID %>');
            var items = Parent.getElementsByTagName('input');

            for (i = 0; i < items.length; i++) {
                if (items[i].id != CurrentRdbID && items[i].type == "radio") {
                    if (items[i].checked) {
                        items[i].checked = false;
                    }
                }
            }
        }

        // Expand Gridview
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
    <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <ContentTemplate>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" />
            <!-- Title Bar -->
            <table style="width: 100%; height: 20px; background-color: #4d4d4d; border-collapse: collapse;">
                <tr>
                    <td>
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                        <asp:Label ID="lbl_Title_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Title_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
                        <asp:Label ID="lbl_Process" runat="server" SkinID="LBL_HD_WHITE"></asp:Label>
                    </td>
                    <td align="right">
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
                    </td>
                </tr>
            </table>
            <!-- Header -->
            <table class="TABLE_HD" style="width: 100%; padding-left: 10px; padding-right: 10px;">
                <tr style="height: 30px;">
                    <td style="width: 5%;">
                        <asp:Label ID="lbl_Ref_Nm0" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Ref_Nm0 %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td style="width: 20%">
                        <asp:TextBox ID="txt_Ref" runat="server" Width="80%" Enabled="False" SkinID="txt_V1"></asp:TextBox>
                    </td>
                    <td style="width: 5%">
                        <asp:Label ID="lbl_Date" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Date %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td style="width: 20%">
                        <asp:TextBox ID="txt_PrDate" runat="server" SkinID="TXT_V1" Width="80%" Enabled="false" AutoPostBack="True" OnTextChanged="txt_PrDate_TextChanged"></asp:TextBox>
                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txt_PrDate" Format="dd/MM/yyyy" CssClass="Calen">
                        </cc1:CalendarExtender>
                        <asp:HiddenField ID="hf_PrDate" runat="server" />
                    </td>
                    <td style="width: 5%">
                        <asp:Label ID="lbl_Requestor_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Requestor_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td style="width: 20%">
                        <asp:Label ID="lbl_Requestor" runat="server" SkinID="LBL_NR"></asp:Label>
                    </td>
                    <td style="width: 5%">
                        <asp:Label ID="lbl_Process_Nm0" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Process_Nm0 %>" SkinID="LBL_HD"></asp:Label>
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
                        <%--<asp:DropDownList ID="ddl_PrType" runat="server" Width="80%" AutoPostBack="True" SkinID="DDL_V1" OnSelectedIndexChanged="ddl_PrType_SelectedIndexChanged1" />--%>
                        <dx:ASPxComboBox ID="ddl_PrType" runat="server" Width="80%" ValueField="Code" TextField="Description" />
                        <asp:HiddenField ID="hf_PrType" runat="server" />
                        <asp:HiddenField ID="hf_ProdCateType2" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lbl_JobCode_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:PC_PR_PrEdit, lbl_JobCode_Nm %>"></asp:Label>
                    </td>
                    <td>
                        <dx:ASPxComboBox ID="ddl_JobCode" runat="server" EnableCallbackMode="true" CallbackPageSize="10" ValueField="Code" ValueType="System.String" TextFormatString="{0} : {1}"
                            DropDownStyle="DropDownList" IncrementalFilteringMode="Contains" OnValidation="ddl_JobCode_Validation" OnItemsRequestedByFilterCondition="ddl_JobCode_OnItemsRequestedByFilterCondition_SQL"
                            OnItemRequestedByValue="ddl_JobCode_OnItemRequestedByValue_SQL">
                            <Columns>
                                <dx:ListBoxColumn Caption="Code" FieldName="Code" Width="180px" />
                                <dx:ListBoxColumn Caption="Description" FieldName="Description" Width="360px" />
                            </Columns>
                        </dx:ASPxComboBox>
                        <%--<asp:RequiredFieldValidator ID="Req_JobCode" runat="server" ErrorMessage="*" ValidationGroup="grd_Group_av" ControlToValidate="ddl_JobCode" Display="Dynamic" />--%>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="lbl_Status_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Status_Nm %>" SkinID="LBL_HD" />
                    </td>
                    <td>
                        <asp:Label ID="lbl_Status" runat="server" Width="80%" />
                    </td>
                </tr>
                <tr style="height: 30px">
                    <td width="5%">
                        <asp:Label ID="lbl_Desc_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Desc_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td colspan="7">
                        <asp:TextBox ID="txt_Desc" runat="server" SkinID="txt_V1" Width="100%"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <!-- Detail Options Bar -->
            <table style="width: 100%; background-color: #4d4d4d; height: 28px; border-collapse: collapse;">
                <tr>
                    <td width="130">
                        <asp:Label ID="lbl_ChangeDeliDate_Nm" runat="server" Width="130" ForeColor="White" Text="<%$ Resources:PC_PR_PrEdit, lbl_ChangeDeliDate_Nm %>" />
                    </td>
                    <td width="120">
                        <dx:ASPxDateEdit ID="dte_DeliDate" runat="server" Width="120" />
                    </td>
                    <td>
                        <dx:ASPxMenu runat="server" ID="menu_OKBar" Font-Bold="True" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle"
                            Height="16px" OnItemClick="menu_OKBar_ItemClick">
                            <ItemStyle BackColor="Transparent">
                                <HoverStyle BackColor="Transparent">
                                    <Border BorderStyle="None" />
                                </HoverStyle>
                                <Paddings Padding="2px" />
                                <Border BorderStyle="None" />
                            </ItemStyle>
                            <Items>
                                <dx:MenuItem Name="OK" Text="">
                                    <ItemStyle Height="16px" Width="34px">
                                        <HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-OK.png" Repeat="NoRepeat" VerticalPosition="center" />
                                        </HoverStyle>
                                        <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/OK.png" Repeat="NoRepeat" VerticalPosition="center" />
                                    </ItemStyle>
                                </dx:MenuItem>
                            </Items>
                        </dx:ASPxMenu>
                    </td>
                    <td align="right">
                        <dx:ASPxMenu runat="server" ID="menu_GrdBar" Font-Bold="True" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle"
                            Height="16px" OnItemClick="menu_GrdBar_ItemClick">
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
                    </td>
                </tr>
            </table>
            <!-- Detail -->
            <div id="MainDiv" style="clear: both; overflow: visible; width: 100%;">
                <asp:GridView ID="grd_PrDt1" runat="server" Width="100%" SkinID="GRD_V1" AutoGenerateColumns="False" OnRowDataBound="grd_PrDt1_RowDataBound" OnRowEditing="grd_PrDt1_RowEditing"
                    OnRowUpdating="grd_PrDt1_RowUpdating" OnRowDeleting="grd_PrDt1_RowDeleting" OnRowCancelingEdit="grd_PrDt1_RowCancelingEdit" OnRowCommand="grd_PrDt1_RowCommand">
                    <HeaderStyle HorizontalAlign="Left" />
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
                        <%-- CheckBox --%>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <div style="padding-top: 3px;">
                                    <asp:CheckBox ID="Chk_All" runat="server" onclick="Check(this)" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div style="padding-top: 3px;">
                                    <asp:CheckBox ID="Chk_Item" runat="server" />
                                </div>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="10px" />
                            <ItemStyle HorizontalAlign="Center" Width="10px" />
                        </asp:TemplateField>
                        <%-- BU --%>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbl_BuCode_HD" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_BuCode_HD %>" Width="140" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl_BuCode" runat="server" SkinID="LBL_NR_GRD" Width="140"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <dx:ASPxComboBox ID="ddl_BuCode" runat="server" Width="140px" AutoPostBack="True" IncrementalFilteringMode="Contains" EnableCallbackMode="true" CallbackPageSize="10"
                                    ValueField="BuCode" ValueType="System.String" TextFormatString="{0} : {1}" OnLoad="ddl_BuCode_Load" OnSelectedIndexChanged="ddl_BuCode_SelectedIndexChanged">
                                    <Columns>
                                        <dx:ListBoxColumn Caption="Code" FieldName="BuCode" />
                                        <dx:ListBoxColumn Caption="Name" FieldName="BuName" />
                                    </Columns>
                                </dx:ASPxComboBox>
                            </EditItemTemplate>
                            <HeaderStyle Width="140px" />
                            <ItemStyle Width="140px" />
                        </asp:TemplateField>
                        <%--Detail--%>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Panel ID="p_Issue" runat="server" Width="100%">
                                    <!--Store/Location-->
                                    <div style="display: inline-block;">
                                        <asp:Label ID="lbl_Store_HDG_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Store_HDG_Nm %>" Width="200px" />
                                    </div>
                                    <!--Product-->
                                    <div style="display: inline-block;">
                                        <asp:Label ID="lbl_SKU_HDG_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_SKU_HDG_Nm %>" Width="140px" />
                                    </div>
                                    <!--Description-->
                                    <div style="display: inline-block;">
                                        <asp:Label ID="lbl_DescEn_HDG_Nm" runat="server" Width="220px" Text="Description" />
                                    </div>
                                    <!--Unit-->
                                    <div style="display: inline-block;">
                                        <asp:Label ID="lbl_Unit_HDG_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Unit_HDG_Nm %>" Width="80px" />
                                    </div>
                                    <!--ReqQty-->
                                    <div style="display: inline-block; text-align: right;">
                                        <asp:Label ID="lbl_ReqQty_HDG_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_QtyReq_HDG_Nm %>" Width="80px" />
                                    </div>
                                    <!--ApprQty-->
                                    <div style="display: inline-block; text-align: right;">
                                        <asp:Label ID="lbl_ApprQty_HDG_Nm" runat="server" Text="Qty Appr." Width="80px" />
                                    </div>
                                    <!--DeliveryDate-->
                                    <div style="display: inline-block;">
                                        <asp:Label ID="lbl_DeliDate_HDG_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_DeliDate_HDG_Nm %>" Width="90px" />
                                    </div>
                                    <!--DeliveryPoint-->
                                    <div style="display: inline-block;">
                                        <asp:Label ID="lbl_DeliPoint_HDG_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_DeliPoint_HDG_Nm %>" Width="160px" />
                                    </div>
                                    <!--Currency Code-->
                                    <div style="display: inline-block;">
                                        <asp:Label ID="lbl_CurrCode_HDG_Nm" runat="server" Text="Currency" Width="80px" />
                                    </div>
                                    <!--Currency Rate-->
                                    <div style="display: inline-block; text-align: right;">
                                        <asp:Label ID="lbl_CurrRate_HDG_Nm" runat="server" Text="Curr. Rate" Width="80px" />
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="p_AllocateVendor" runat="server">
                                    <%--<div style="width: 100%;">
                                        <div class="<%= HiddenClassName %>" style="display: inline-block; padding-right: 5px; height: 28px;">
                                            <asp:Label ID="lbl_BU_HD_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_BU_HD_av %>" Width="80px"></asp:Label>
                                        </div>
                                        <div style="display: inline-block; padding-right: 5px; height: 28px;">
                                            <asp:Label ID="lbl_Vendor_HDG_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Vendor_HDG_av %>" Width="220px"></asp:Label>
                                        </div>
                                        <div style="display: inline-block; padding-right: 5px; height: 28px;">
                                            <asp:Label ID="lbl_Store_HDG_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Store_HDG_av %>" Width="200px"></asp:Label>
                                        </div>
                                        <div style="display: inline-block; padding-right: 5px; height: 28px;">
                                            <asp:Label ID="lbl_SKU_HDG_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_SKU_HDG_av %>" Width="100px"></asp:Label>
                                        </div>
                                        <div style="display: inline-block; padding-right: 5px; height: 28px;">
                                            <asp:Label ID="lbl_DescEn_HDG_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_DescEn_HDG_av %>" Width="160px"></asp:Label>
                                        </div>
                                        <div style="display: inline-block; padding-right: 5px; height: 28px;">
                                            <asp:Label ID="lbl_Descll_HDG_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Descll_HDG_av %>" Width="160px"></asp:Label>
                                        </div>
                                        <div style="display: inline-block; padding-right: 5px; height: 28px;">
                                            <asp:Label ID="lbl_Unit_HDG_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Unit_HDG_av %>" Width="80px"></asp:Label>
                                        </div>
                                        <div style="display: inline-block; padding-right: 5px; height: 28px; text-align: right;">
                                            <asp:Label ID="lbl_ReqQty_HDG_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_QtyReq_HDG_av %>" Width="80px"></asp:Label>
                                        </div>
                                        <div style="display: inline-block; padding-right: 5px; height: 28px; text-align: right;">
                                            <asp:Label ID="lbl_ApprQty_HDG_av" runat="server" Text="Qty Appr." Width="80px"></asp:Label>
                                        </div>
                                        <div style="display: inline-block; padding-right: 5px; height: 28px; text-align: right;">
                                            <asp:Label ID="lbl_FOC_HDG_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_FOC_HDG_av %>" Width="80px"></asp:Label>
                                        </div>
                                        <div style="display: inline-block; padding-right: 5px; text-align: right;">
                                            <asp:Label ID="lbl_Price_HDG_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Price_HDG_av %>" Width="100px"></asp:Label>
                                        </div>
                                        <div style="display: inline-block; padding-right: 5px; height: 28px;">
                                            <asp:Label ID="lbl_CurrCode_HDG_av" runat="server" Text="Currency" Width="80px"></asp:Label>
                                        </div>
                                        <div style="display: inline-block; padding-right: 5px; height: 28px; text-align: right;">
                                            <asp:Label ID="lbl_CurrRate_HDG_av" runat="server" Text="Curr. Rate" Width="80px"></asp:Label>
                                        </div>
                                        <div style="display: inline-block; padding-right: 5px; height: 28px;">
                                            <asp:Label ID="lbl_DeliPoint_HDG_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_DeliPotin_HDG_av %>" Width="220px"></asp:Label>
                                        </div>
                                        <div style="display: inline-block; padding-right: 5px; height: 28px;">
                                            <asp:Label ID="lbl_DeliDate_HDG_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_DeliDate_HDG_av %>" Width="100px"></asp:Label>
                                        </div>
                                    </div>--%>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="padding-left: 3px; padding-right: 3px;" class="<%= HiddenClassName %>">
                                                <asp:Label ID="lbl_BU_HD_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_BU_HD_av %>" Width="80px"></asp:Label>
                                            </td>
                                            <td style="padding-left: 3px; padding-right: 3px;">
                                                <asp:Label ID="lbl_Vendor_HDG_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Vendor_HDG_av %>" Width="200px"></asp:Label>
                                            </td>
                                            <td style="padding-left: 3px; padding-right: 3px;">
                                                <asp:Label ID="lbl_Store_HDG_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Store_HDG_av %>" Width="200px"></asp:Label>
                                            </td>
                                            <td style="padding-left: 3px; padding-right: 3px;">
                                                <asp:Label ID="lbl_SKU_HDG_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_SKU_HDG_av %>" Width="100px"></asp:Label>
                                            </td>
                                            <td style="padding-left: 3px; padding-right: 3px;">
                                                <asp:Label ID="lbl_DescEn_HDG_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_DescEn_HDG_av %>" Width="160px"></asp:Label>
                                            </td>
                                            <td style="padding-left: 3px; padding-right: 3px;">
                                                <asp:Label ID="lbl_Descll_HDG_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Descll_HDG_av %>" Width="160px"></asp:Label>
                                            </td>
                                            <td style="padding-left: 3px; padding-right: 3px;">
                                                <asp:Label ID="lbl_DeliDate_HDG_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_DeliDate_HDG_av %>" Width="100px"></asp:Label>
                                            </td>
                                            <td style="padding-left: 3px; padding-right: 3px;">
                                                <asp:Label ID="lbl_DeliPoint_HDG_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_DeliPotin_HDG_av %>" Width="200px"></asp:Label>
                                            </td>
                                            <td style="padding-left: 3px; padding-right: 3px;">
                                                <asp:Label ID="lbl_ReqQty_HDG_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_QtyReq_HDG_av %>" Width="80px"></asp:Label>
                                            </td>
                                            <td style="padding-left: 3px; padding-right: 3px;">
                                                <asp:Label ID="lbl_ApprQty_HDG_av" runat="server" Text="Qty Appr." Width="80px"></asp:Label>
                                            </td>
                                            <td style="padding-left: 3px; padding-right: 3px;">
                                                <asp:Label ID="lbl_FOC_HDG_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_FOC_HDG_av %>" Width="80px"></asp:Label>
                                            </td>
                                            <td style="padding-left: 3px; padding-right: 3px;">
                                                <asp:Label ID="lbl_Unit_HDG_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Unit_HDG_av %>" Width="80px"></asp:Label>
                                            </td>
                                            <td style="padding-left: 3px; padding-right: 3px;">
                                                <asp:Label ID="lbl_Price_HDG_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Price_HDG_av %>" Width="80px"></asp:Label>
                                            </td>
                                            <td style="padding-left: 3px; padding-right: 3px;">
                                                <asp:Label ID="lbl_CurrCode_HDG_av" runat="server" Text="Currency" Width="80px"></asp:Label>
                                            </td>
                                            <td style="padding-left: 3px; padding-right: 3px;">
                                                <asp:Label ID="lbl_CurrRate_HDG_av" runat="server" Text="Curr. Rate" Width="80px"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Panel ID="p_Issue" runat="server" Width="100%">
                                    <!--Store/Location-->
                                    <div style="display: inline-block;">
                                        <asp:Label ID="lbl_LocationCode" runat="server" SkinID="LBL_NR_GRD" Width="200px"></asp:Label>
                                    </div>
                                    <!--Product-->
                                    <div style="display: inline-block;">
                                        <asp:Label ID="lbl_ProductCode" runat="server" SkinID="LBL_NR_GRD" Width="140px"></asp:Label>
                                    </div>
                                    <!--Description-->
                                    <div style="display: inline-block;">
                                        <div>
                                            <asp:Label ID="lbl_DescEn" runat="server" SkinID="LBL_NR_GRD" Width="220px"></asp:Label>
                                        </div>
                                        <div>
                                            <asp:Label ID="lbl_DescLL" runat="server" SkinID="LBL_NR_GRD" Width="220px"></asp:Label>
                                        </div>
                                    </div>
                                    <!--Unit-->
                                    <div style="display: inline-block;">
                                        <asp:Label ID="lbl_Unit" runat="server" SkinID="LBL_NR_GRD" Width="80px"></asp:Label>
                                    </div>
                                    <!--ReqQty-->
                                    <div style="display: inline-block; text-align: right;">
                                        <asp:Label ID="lbl_ReqQty" runat="server" kinID="LBL_NR_GRD" Width="80px"></asp:Label>
                                    </div>
                                    <!--ApprQty-->
                                    <div style="display: inline-block; text-align: right;">
                                        <asp:Label ID="lbl_ApprQty" runat="server" kinID="LBL_NR_GRD" Width="80px"></asp:Label>
                                    </div>
                                    <!--DeliveryDate-->
                                    <div style="display: inline-block;">
                                        <asp:Label ID="lbl_DeliDate" runat="server" SkinID="LBL_NR_GRD" Width="90px"></asp:Label>
                                    </div>
                                    <!--DeliveryPoint-->
                                    <div style="display: inline-block;">
                                        <asp:Label ID="lbl_DePoint" runat="server" SkinID="LBL_NR_GRD" Width="160px"></asp:Label>
                                    </div>
                                    <!--Currency Code-->
                                    <div style="display: inline-block;">
                                        <asp:Label ID="lbl_CurrCode" runat="server" SkinID="LBL_NR_GRD" Width="80px"></asp:Label>
                                    </div>
                                    <!--Currency Rate-->
                                    <div style="display: inline-block; text-align: right;">
                                        <asp:Label ID="lbl_CurrRate" runat="server" SkinID="LBL_NR_GRD" Width="80px"></asp:Label>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="p_AllocateVendor" runat="server">
                                    <%--<div style="width: 100%;">
                                        <div class="<%= HiddenClassName %>" style="display: inline-block; padding-right: 5px; height: 28px;">
                                            <asp:Label ID="lbl_BuCode_av" runat="server" Width="80px"></asp:Label>
                                        </div>
                                        <div style="display: inline-block; padding-right: 5px; height: 28px;">
                                            <asp:Label ID="lbl_Vendor_av" runat="server" Width="220px" SkinID="LBL_NR_GRD"></asp:Label>
                                        </div>
                                        <div style="display: inline-block; padding-right: 5px; height: 28px;">
                                            <asp:Label ID="lbl_LocationCode_av" runat="server" Width="200px" SkinID="LBL_NR_GRD"></asp:Label>
                                        </div>
                                        <div style="display: inline-block; padding-right: 5px; height: 28px;">
                                            <asp:Label ID="lbl_SKU_av" runat="server" Width="100px" SkinID="LBL_NR_GRD"></asp:Label>
                                        </div>
                                        <div style="display: inline-block; padding-right: 5px; height: 28px;">
                                            <asp:Label ID="lbl_DescEN_av" runat="server" Width="160px" SkinID="LBL_NR_GRD"></asp:Label>
                                        </div>
                                        <div style="display: inline-block; padding-right: 5px; height: 28px;">
                                            <asp:Label ID="lbl_DescLL_av" runat="server" Width="160px" SkinID="LBL_NR_GRD"></asp:Label>
                                        </div>
                                        <div style="display: inline-block; padding-right: 5px; height: 28px;">
                                            <asp:Label ID="lbl_OrderUnit_av" runat="server" Width="80px" SkinID="LBL_NR_GRD"></asp:Label>
                                        </div>
                                        <div style="display: inline-block; padding-right: 5px; height: 28px; text-align: right;">
                                            <asp:Label ID="lbl_ReqQty_av" runat="server" Width="80px" SkinID="LBL_NR_GRD"></asp:Label>
                                        </div>
                                        <div style="display: inline-block; padding-right: 5px; height: 28px; text-align: right;">
                                            <asp:Label ID="lbl_ApprQty_av" runat="server" Width="80px" SkinID="LBL_NR_GRD"></asp:Label>
                                        </div>
                                        <div style="display: inline-block; padding-right: 5px; height: 28px; text-align: right;">
                                            <asp:Label ID="lbl_FOC_av" runat="server" Width="80px" SkinID="LBL_NR_GRD"></asp:Label>
                                        </div>
                                        <div style="display: inline-block; padding-right: 5px; height: 28px; text-align: right;">
                                            <asp:Label ID="lbl_Price_av" runat="server" Width="100px" SkinID="LBL_NR_GRD"></asp:Label>
                                        </div>
                                        <div style="display: inline-block; padding-right: 5px; height: 28px;">
                                            <asp:Label ID="lbl_CurrCode_av" runat="server" Width="80px" SkinID="LBL_NR_GRD"></asp:Label>
                                        </div>
                                        <div style="display: inline-block; padding-right: 5px; height: 28px; text-align: right;">
                                            <asp:Label ID="lbl_CurrRate_av" runat="server" Width="80px" SkinID="LBL_NR_GRD"></asp:Label>
                                        </div>
                                        <div style="display: inline-block; padding-right: 5px; height: 28px;">
                                            <asp:Label ID="lbl_DeliPoint_av" runat="server" Width="220px" SkinID="LBL_NR_GRD"></asp:Label>
                                        </div>
                                        <div style="display: inline-block; padding-right: 5px; height: 28px;">
                                            <asp:Label ID="lbl_DeliDate_av" runat="server" Width="100px" SkinID="LBL_NR_GRD"></asp:Label>
                                        </div>
                                    </div>--%>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td class="<%= HiddenClassName %>">
                                                <asp:Label ID="lbl_BuCode_av" runat="server" Width="80px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_Vendor_av" runat="server" Width="200px" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_LocationCode_av" runat="server" Width="200px" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_SKU_av" runat="server" Width="100px" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_DescEN_av" runat="server" Width="160px" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_DescLL_av" runat="server" Width="160px" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_DeliDate_av" runat="server" Width="100px" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_DeliPoint_av" runat="server" Width="200px" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_ReqQty_av" runat="server" Width="80px" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_ApprQty_av" runat="server" Width="80px" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_FOC_av" runat="server" Width="80px" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_OrderUnit_av" runat="server" Width="80px" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_Price_av" runat="server" Width="80px" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_CurrCode_av" runat="server" Width="80px" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_CurrRate_av" runat="server" Width="80px" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Panel ID="p_Issue" runat="server" Width="100%">
                                    <!--Store/Location-->
                                    <div style="display: inline-block;">
                                        <dx:ASPxComboBox ID="ddl_LocationCode" runat="server" Width="200px" AutoPostBack="True" EnableCallbackMode="true" CallbackPageSize="10" IncrementalFilteringMode="Contains"
                                            TextFormatString="{0} : {1}" ValueField="LocationCode" ValueType="System.String" OnItemsRequestedByFilterCondition="ddl_LocationCode_OnItemsRequestedByFilterCondition_SQL"
                                            OnItemRequestedByValue="ddl_LocationCode_OnItemRequestedByValue_SQL" OnSelectedIndexChanged="ddl_LocationCode_SelectedIndexChanged">
                                            <Columns>
                                                <dx:ListBoxColumn Caption="Code" FieldName="LocationCode" Width="100px" />
                                                <dx:ListBoxColumn Caption="Name" FieldName="LocationName" Width="380px" />
                                            </Columns>
                                        </dx:ASPxComboBox>
                                        <asp:RequiredFieldValidator ID="Req_Location" runat="server" ErrorMessage="*" ValidationGroup="grd_Group_av" ControlToValidate="ddl_LocationCode" Display="Dynamic">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <!--Product-->
                                    <div style="display: inline-block;">
                                        <dx:ASPxComboBox ID="ddl_ProductCode" runat="server" Width="140px" AutoPostBack="True" IncrementalFilteringMode="Contains" ValueField="ProductCode" ValueType="System.String"
                                            TextFormatString="{0} : {1} {2}" EnableCallbackMode="true" CallbackPageSize="10" OnSelectedIndexChanged="ddl_ProductCode_SelectedIndexChanged" OnItemsRequestedByFilterCondition="ddl_ProductCode_OnItemsRequestedByFilterCondition_SQL"
                                            OnItemRequestedByValue="ddl_ProductCode_OnItemRequestedByValue_SQL">
                                            <Columns>
                                                <dx:ListBoxColumn Caption="Code" FieldName="ProductCode" Width="100px" />
                                                <dx:ListBoxColumn Caption="Name1" FieldName="ProductDesc1" Width="280px" />
                                                <dx:ListBoxColumn Caption="Name2" FieldName="ProductDesc2" Width="200px" />
                                            </Columns>
                                        </dx:ASPxComboBox>
                                        <asp:RequiredFieldValidator ID="Req_Product" runat="server" ErrorMessage="*" ValidationGroup="grd_Group_av" ControlToValidate="ddl_ProductCode" Display="Dynamic">                                                                    
                                        </asp:RequiredFieldValidator>
                                        <asp:HiddenField ID="hf_ProductCode" runat="server" />
                                    </div>
                                    <!--Description-->
                                    <div style="display: inline-block;">
                                        <div>
                                            <asp:TextBox ID="txt_DescEn" runat="server" Enabled="False" SkinID="txt_V1" Width="220px" Style="background-color: transparent; border-width: 0;"></asp:TextBox>
                                        </div>
                                        <div>
                                            <asp:TextBox ID="txt_DescLL" runat="server" Enabled="False" SkinID="txt_V1" Width="220px" Style="background-color: transparent; border-width: 0;"></asp:TextBox>
                                        </div>
                                    </div>
                                    <!--Unit-->
                                    <div style="display: inline-block;">
                                        <dx:ASPxComboBox ID="ddl_Unit" runat="server" Width="80px" AutoPostBack="false" EnableCallbackMode="true" CallbackPageSize="10" IncrementalFilteringMode="Contains"
                                            ValueField="OrderUnit" ValueType="System.String" TextField="OrderUnit" OnLoad="ddl_Unit_Load">
                                        </dx:ASPxComboBox>
                                        <asp:RequiredFieldValidator ID="Req_Unit" runat="server" ErrorMessage="*" ValidationGroup="grd_Group_av" ControlToValidate="ddl_Unit" Display="Dynamic">                                                                    
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <!--ReqQty-->
                                    <div style="display: inline-block; text-align: right;">
                                        <dx:ASPxSpinEdit ID="txt_QtyRequest" Width="80px" runat="server" SkinID="sk_qty" Enabled="false" />
                                        <asp:RequiredFieldValidator ID="txt_QtyRequestReq" runat="server" ControlToValidate="txt_QtyRequest" ErrorMessage="*" Visible="false"> 
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <!--ApprQty-->
                                    <div style="display: inline-block; text-align: right;">
                                        <dx:ASPxSpinEdit ID="txt_ApprQty" Width="80px" runat="server" SkinID="sk_qty" />
                                        <asp:RequiredFieldValidator ID="txt_ApprQtyReq" runat="server" ControlToValidate="txt_ApprQty" ErrorMessage="*" Visible="false"> 
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <!--DeliveryDate-->
                                    <div style="display: inline-block;">
                                        <dx:ASPxDateEdit ID="dte_Date" runat="server" Width="90px" />
                                    </div>
                                    <!--DeliveryPoint-->
                                    <div style="display: inline-block;">
                                        <dx:ASPxComboBox ID="ddl_DeliPoint" runat="server" Width="160px" ValueField="DptCode"  ValueType="System.String" TextFormatString="{0} : {1}"
											EnableCallbackMode="true" CallbackPageSize="25" DropDownRows="15" OnLoad="ddl_DeliPoint_Load">
                                            <Columns>
                                                <dx:ListBoxColumn Caption="Code" FieldName="DptCode" Width="30px" />
                                                <dx:ListBoxColumn Caption="Name" FieldName="Name" Width="240px" />
                                            </Columns>
                                        </dx:ASPxComboBox>
									
                                        <asp:HiddenField ID="hf_DeliPoint" runat="server" />
                                    </div>
                                    <!--Currency Code-->
                                    <div style="display: inline-block;">
                                        <dx:ASPxComboBox ID="ddl_CurrCode" runat="server" AutoPostBack="true" TextFormatString="{0}" EnableCallbackMode="true" CallbackPageSize="10" ValueType="System.String"
                                            Width="80px" ValueField="CurrencyCode" IncrementalFilteringMode="Contains" OnLoad="ddl_CurrCode_Load">
                                            <Columns>
                                                <dx:ListBoxColumn Caption="Currency" FieldName="CurrencyCode" />
                                            </Columns>
                                        </dx:ASPxComboBox>
                                        <asp:HiddenField ID="hf_CurrCode" runat="server" />
                                    </div>
                                    <!--Currency Rate-->
                                    <div style="display: inline-block; text-align: right;">
                                        <dx:ASPxComboBox ID="comb_CurrRate" runat="server" AutoPostBack="True" TextFormatString="{0:N6}" Width="80px" ValueField="CurrencyRate" IncrementalFilteringMode="Contains">
                                            <Columns>
                                                <dx:ListBoxColumn Caption="Rate" FieldName="CurrencyRate" />
                                                <dx:ListBoxColumn Caption="Input Date" FieldName="InputDate" />
                                            </Columns>
                                        </dx:ASPxComboBox>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="p_AllocateVendor" runat="server">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td class="<%= HiddenClassName %>">
                                                <dx:ASPxComboBox ID="ddl_BuCode_av" runat="server" Width="80px" AutoPostBack="True" IncrementalFilteringMode="Contains" EnableCallbackMode="true" CallbackPageSize="10"
                                                    ValueField="BuCode" ValueType="System.String" TextFormatString="{0} : {1}" OnLoad="ddl_BuCode_av_Load" OnSelectedIndexChanged="ddl_BuCode_av_SelectedIndexChanged">
                                                    <Columns>
                                                        <dx:ListBoxColumn Caption="Code" FieldName="BuCode" />
                                                        <dx:ListBoxColumn Caption="Name" FieldName="BuName" />
                                                    </Columns>
                                                </dx:ASPxComboBox>
                                                <div>
                                                    <asp:RequiredFieldValidator ID="Req_BuCode_av" runat="server" Width="10px" ErrorMessage="*" ValidationGroup="grd_Group_av" ControlToValidate="ddl_BuCode_av"
                                                        Display="Dynamic" />
                                                </div>
                                            </td>
                                            <td>
                                                <dx:ASPxComboBox ID="ddl_Vendor_av" runat="server" Width="200px" EnableCallbackMode="true" CallbackPageSize="100" IncrementalFilteringMode="Contains" ValueField="VendorCode"
                                                    ValueType="System.String" TextFormatString="{0} : {1}" OnLoad="ddl_Vendor_av_Load" OnItemsRequestedByFilterCondition="ddl_Vendor_av_OnItemsRequestedByFilterCondition_SQL"
                                                    OnItemRequestedByValue="ddl_Vendor_av_OnItemRequestedByValue_SQL">
                                                    <Columns>
                                                        <dx:ListBoxColumn Caption="Code" FieldName="VendorCode" Width="70px" />
                                                        <dx:ListBoxColumn Caption="Name" FieldName="Name" Width="300px" />
                                                    </Columns>
                                                </dx:ASPxComboBox>
                                                <div>
                                                    <asp:RequiredFieldValidator ID="Req_Vendor_av" runat="server" Width="10px" ErrorMessage="*" ValidationGroup="grd_Group_av" ControlToValidate="ddl_Vendor_av"
                                                        Display="Dynamic" />
                                                </div>
                                            </td>
                                            <td>
                                                <dx:ASPxComboBox ID="ddl_LocationCode_av" runat="server" Width="200px" AutoPostBack="True" EnableCallbackMode="false" CallbackPageSize="10" IncrementalFilteringMode="Contains"
                                                    ValueField="LocationCode" ValueType="System.String" TextFormatString="{0} : {1}" OnLoad="ddl_LocationCode_av_Load" OnSelectedIndexChanged="ddl_LocationCode_av_SelectedIndexChanged">
                                                    <Columns>
                                                        <dx:ListBoxColumn Caption="Code" FieldName="LocationCode" Width="100px" />
                                                        <dx:ListBoxColumn Caption="Name" FieldName="LocationName" Width="200px" />
                                                    </Columns>
                                                </dx:ASPxComboBox>
                                            </td>
                                            <td colspan="3">
                                                <dx:ASPxComboBox ID="ddl_ProductCode_av" runat="server" Width="420px" AutoPostBack="True" IncrementalFilteringMode="Contains" EnableCallbackMode="true"
                                                    ValueField="ProductCode" ValueType="System.String" TextFormatString="{0} : {1} : {2}" OnSelectedIndexChanged="ddl_ProductCode_av_SelectedIndexChanged">
                                                    <Columns>
                                                        <dx:ListBoxColumn Caption="Code" FieldName="ProductCode" Width="100px" />
                                                        <dx:ListBoxColumn Caption="Name" FieldName="ProductDesc1" Width="200px" />
                                                        <dx:ListBoxColumn Caption="Other Name" FieldName="ProductDesc2" Width="150px" />
                                                    </Columns>
                                                </dx:ASPxComboBox>
                                                <asp:HiddenField ID="hf_ProductCode_av" runat="server" />
                                            </td>
                                            <td>
                                                <dx:ASPxDateEdit ID="dte_DeliDate_av" runat="server" Width="100px" />
                                            </td>
                                            <td>
                                                <dx:ASPxComboBox ID="ddl_DeliPoint_av" runat="server" Width="200px" IncrementalFilteringMode="Contains" ValueField="DptCode" ValueType="System.String"
                                                    TextFormatString="{0} : {1}" OnLoad="ddl_DeliPoint_av_Load">
                                                    <Columns>
                                                        <dx:ListBoxColumn Caption="Code" FieldName="DptCode" Width="50px" />
                                                        <dx:ListBoxColumn Caption="Name" FieldName="Name" Width="200px" />
                                                    </Columns>
                                                </dx:ASPxComboBox>
                                                <asp:HiddenField ID="hf_DeliPoint_av" runat="server" />
                                            </td>
                                            <td>
                                                <dx:ASPxSpinEdit ID="txt_ReqQty_av" Width="80px" runat="server" SkinID="sk_qty" AutoPostBack="true" OnNumberChanged="txt_ReqQty_av_NumberChanged" />
                                                <div>
                                                    <asp:RequiredFieldValidator ID="txt_ReqQty_avReq" runat="server" Width="10px" ControlToValidate="txt_ReqQty_av" ErrorMessage="*" Visible="false"> 
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </td>
                                            <td>
                                                <dx:ASPxSpinEdit ID="txt_ApprQty_av" Width="80px" runat="server" SkinID="sk_qty" OnNumberChanged="txt_ApprQty_av_NumberChanged" />
                                                <div>
                                                    <asp:RequiredFieldValidator ID="txt_ApprQty_avReq" runat="server" Width="10px" ControlToValidate="txt_ReqQty_av" ErrorMessage="*" Visible="false" />
                                                </div>
                                            </td>
                                            <td>
                                                <dx:ASPxSpinEdit ID="txt_FOC_av" Width="80px" runat="server" SkinID="sk_qty" />
                                                <div>
                                                    <asp:RequiredFieldValidator ID="txt_FOC_avReq" runat="server" Width="10px" ControlToValidate="txt_FOC_av" ErrorMessage="*" Visible="false" />
                                                </div>
                                            </td>
                                            <td>
                                                <dx:ASPxComboBox ID="ddl_Unit_av" runat="server" Width="80px" AutoPostBack="True" IncrementalFilteringMode="Contains" ValueField="OrderUnit" ValueType="System.String"
                                                    TextFormatString="{0}" OnLoad="ddl_Unit_av_Load" OnSelectedIndexChanged="ddl_Unit_av_SelectedIndexChanged">
                                                    <Columns>
                                                        <dx:ListBoxColumn Caption="Code" FieldName="OrderUnit" />
                                                    </Columns>
                                                </dx:ASPxComboBox>
                                                <asp:HiddenField ID="hf_UnitCode_av" runat="server" />
                                            </td>
                                            <td>
                                                <dx:ASPxSpinEdit ID="txt_Price_av" runat="server" Width="80px" AutoPostBack="true" HorizontalAlign="Right" NullText="0" Number="0" OnNumberChanged="txt_Price_av_NumberChanged">
                                                    <SpinButtons ShowIncrementButtons="False" />
                                                    <ValidationSettings Display="Dynamic">
                                                        <ErrorFrameStyle ImageSpacing="4px">
                                                            <ErrorTextPaddings PaddingLeft="4px" />
                                                        </ErrorFrameStyle>
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxSpinEdit>
                                                <div>
                                                    <asp:RequiredFieldValidator ID="txt_Price_avReq" runat="server" Width="10px" ControlToValidate="txt_Price_av" ErrorMessage="*" Visible="false" />
                                                </div>
                                            </td>
                                            <td>
                                                <dx:ASPxComboBox ID="ddl_CurrCode_av" runat="server" Width="80px" AutoPostBack="true" IncrementalFilteringMode="Contains" ValueField="CurrencyCode" ValueType="System.String"
                                                    TextFormatString="{0}" OnInit="ddl_CurrCode_av_OnInit" OnSelectedIndexChanged="ddl_CurrCode_av_SelectedIndexChanged">
                                                    <Columns>
                                                        <dx:ListBoxColumn Caption="Code" FieldName="CurrencyCode" />
                                                    </Columns>
                                                </dx:ASPxComboBox>
                                                <div>
                                                    <asp:RequiredFieldValidator ID="Req_CurrCode_av" runat="server" Width="10px" ErrorMessage="*" ValidationGroup="grd_Group_av" ControlToValidate="ddl_CurrCode_av"
                                                        Display="Dynamic" />
                                                </div>
                                            </td>
                                            <td>
                                                <dx:ASPxComboBox ID="ddl_CurrRate_av" runat="server" Width="80px" AutoPostBack="True" DropDownStyle="DropDown" IncrementalFilteringMode="Contains" ValueField="CurrencyRate" TextFormatString="{0:N6}"
                                                    OnLoad="ddl_CurrRate_av_Load" OnSelectedIndexChanged="ddl_CurrRate_av_SelectedIndexChanged">
                                                    <Columns>
                                                        <dx:ListBoxColumn Caption="Rate" FieldName="CurrencyRate" />
                                                        <dx:ListBoxColumn Caption="Input Date" FieldName="InputDate" />
                                                    </Columns>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <%--Summary--%>
                        <asp:TemplateField Visible="true" HeaderStyle-Width="0%">
                            <ItemTemplate>
                                <tr id="TR_Summmary" runat="server" style="display: none">
                                    <td colspan="4" style="padding-left: 30px; padding-top: 10px;">
                                        <asp:Panel ID="p_Issue_Expand" runat="server">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td style="width: 8%;">
                                                    </td>
                                                    <td style="width: 7%;" align="right">
                                                    </td>
                                                    <td style="width: 7%;">
                                                    </td>
                                                    <td style="width: 20%;">
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:Label ID="lbl_CurrCurrDt_Grd_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, TitleCurrency %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                        <asp:Label ID="lbl_CurrCurrDt_Grd" runat="server" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td colspan="4">
                                                        <asp:Label ID="lbl_BaseCurrDt_Grd_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, TitleBaseCurrency %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                        <asp:Label ID="lbl_BaseCurrDt_Grd" runat="server" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td align="right">
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_CurrNetAmt_Grd_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_PR_PrEdit, lbl_NetAmt_Nm %>"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lbl_CurrNetAmt_Grd" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td style="width: 3%;">
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_NetAmt_Grd_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_NetAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lbl_NetAmt_Grd" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td style="width: 3%;">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_DiscPercent_Grd_Nm" runat="server" Text="Disc." SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lbl_DiscPercent_Grd" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_CurrDiscAmt_Grd_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_DiscAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lbl_CurrDiscAmt_Grd" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_DiscAmt_Grd_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_DiscAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lbl_DiscAmt_Grd" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chk_Adj" runat="server" Text="Adj." />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_TaxType_Grd_Nm" runat="server" Text="Tax Type" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lbl_TaxType_Grd" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_CurrTaxAmt_Grd_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_TaxAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lbl_CurrTaxAmt_Grd" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_TaxAmt_Grd_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_TaxAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lbl_TaxAmt_Grd" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_TaxRate_Grd_Nm" runat="server" Text="Tax Rate" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lbl_TaxRate_Grd" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_CurrTotalAmt_Grd_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_TotalAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lbl_CurrTotalAmt_Grd" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_TotalAmt_Grd_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_TotalAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lbl_TotalAmt_Grd" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <%--/* View Mode */--%>
                                        <%--/* Added Currency On: 15/08/2017, By: Fon */--%>
                                        <asp:Panel ID="p_AllocateVendor_Expand" runat="server">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td width="8%">
                                                    </td>
                                                    <td colspan="2">
                                                    </td>
                                                    <td style="width: 20%;">
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:Label ID="lbl_CurrCurrDt_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, TitleCurrency %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                        <asp:Label ID="lbl_CurrCurrDt" runat="server" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:Label ID="lbl_BaseCurrDt_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, TitleBaseCurrency %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                        <asp:Label ID="lbl_BaseCurrDt" runat="server" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td style="width: 7%">
                                                    </td>
                                                    <td style="width: 7%" align="right">
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td width="7%">
                                                        <asp:Label ID="lbl_CurrNetAmt_Grd_Av_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_PR_PrEdit, lbl_NetAmt_Nm %>"></asp:Label>
                                                    </td>
                                                    <td align="right" width="10%">
                                                        <asp:Label ID="lbl_CurrNetAmt_Grd_Av" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td width="5%">
                                                    </td>
                                                    <td style="width: 7%">
                                                        <asp:Label ID="lbl_NetAmt_Grd_Av_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_NetAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right" style="width: 10%">
                                                        <asp:Label ID="lbl_NetAmt_Grd_Av" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td width="3%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_Disc_Grd_Av_Nm" runat="server" Text="Disc." SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lbl_Disc_Grd_Av" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_CurrDiscAmt_Grd_Av_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_PR_PrEdit, lbl_DiscAmt_Nm %>"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lbl_CurrDiscAmt_Grd_Av" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_DiscAmt_Grd_Av_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_DiscAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lbl_DiscAmt_Grd_Av" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chk_Adj_Av" runat="server" Text="Adj. Tax" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_TaxType_Grd_Av_Nm" runat="server" Text="Tax Type" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lbl_TaxType_Grd_Av" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_CurrTaxAmt_Grd_Av_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_PR_PrEdit, lbl_TaxAmt_Nm %>"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lbl_CurrTaxAmt_Grd_Av" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_TaxAmt_Grd_Av_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_TaxAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lbl_TaxAmt_Grd_Av" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_TaxRate_Grd_Av_Nm" runat="server" Text="Tax Rate" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lbl_TaxRate_Grd_Av" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_CurrTotalAmt_Grd_Av_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_TotalAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lbl_CurrTotalAmt_Grd_Av" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_TotalAmt_Grd_Av_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_TotalAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lbl_TotalAmt_Grd_Av" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr style="background-color: #DADADA; height: 17px;">
                                                <td>
                                                    <asp:Label ID="lbl_PriceCompare_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PC_PR_PrEdit, lbl_PriceCompare_Nm %>"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 17px; width: 100%">
                                                    <asp:GridView ID="grd_PriceCompare1" runat="server" AutoGenerateColumns="False" Width="100%" SkinID="GRD_V1" EmptyDataText="No Data to Display" EnableModelValidation="True"
                                                        ShowFooter="True">
                                                        <Columns>
                                                            <asp:BoundField HeaderText="<%$ Resources:PC_PR_PrEdit, lbl_BU_Compare_Nm %>" DataField="BuCode">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Ref#" DataField="RefNo">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="<%$ Resources:PC_PR_PrEdit, lbl_Name_Compare_Nm %>" DataField="VendorName">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Unit" DataField="OrderUnit">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="<%$ Resources:PC_PR_PrEdit, lbl_Rank_Compare_Nm %>" DataField="VendorRank">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="<%$ Resources:PC_PR_PrEdit, lbl_Price_Compare_Nm %>" DataField="Price">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <%--6--%>
                                                            <asp:BoundField HeaderText="<%$ Resources:PC_PR_PrEdit, lbl_DiscPercent_Compare_Nm %>" DataField="DiscountPercent">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="<%$ Resources:PC_PR_PrEdit, lbl_DiscAmt_Compare_Nm %>" DataField="DiscountAmt">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="<%$ Resources:PC_PR_PrEdit, lbl_Foc_Compare_Nm %>" DataField="FOC">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="<%$ Resources:PC_PR_PrEdit, lbl_Min_Compare_Nm %>" DataField="QtyFrom">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="<%$ Resources:PC_PR_PrEdit, lbl_Max_Compare_Nm %>" DataField="QtyTo">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <%--11--%>
                                                            <asp:BoundField DataField="Tax" Visible="false" />
                                                            <asp:BoundField DataField="TaxRate" Visible="false" />
                                                            <asp:BoundField DataField="CurrencyCode" Visible="false" />
                                                            <asp:BoundField DataField="CurrencyRate" Visible="false" />
                                                            <asp:TemplateField HeaderText="">
                                                                <ItemTemplate>
                                                                    <dx:ASPxButton ID="btn_Assign_av" runat="server" Text="Assign" OnClick="btn_Assign_av_Click">
                                                                    </dx:ASPxButton>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                        <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                            <tr style="vertical-align: top">
                                                <td class="TD_LINE_GRD" style="width: 7%;">
                                                    <asp:Label ID="lbl_OnHand_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_OnHand_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" style="width: 5%;">
                                                    <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" style="width: 10%;">
                                                    <asp:Label ID="lbl_OnOrder_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_OnOrder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" style="width: 5%;">
                                                    <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" style="width: 5%;">
                                                    <asp:Label ID="lbl_ReOrder_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_ReOrder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" style="width: 10%;">
                                                    <asp:Label ID="lbl_ReOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" style="width: 5%;">
                                                    <asp:Label ID="lbl_ReStock_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_ReStock_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" style="width: 5%;">
                                                    <asp:Label ID="lbl_ReStock" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" style="width: 10%;">
                                                    <asp:Label ID="lbl_LastPrice_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_LastPrice_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" style="width: 10%;">
                                                    <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" style="width: 10%">
                                                    <asp:Label ID="lbl_LastVendor_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_LastVendor_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" style="width: 8%; white-space: nowrap; overflow: hidden" align="right">
                                                    <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1" Width="130px"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_Po_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Po_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_Po" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_Ref_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Ref_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_Ref" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_Buyer_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Buyer_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_Buyer" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_Order_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Order_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_Order" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_Receive_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Receive_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_Receive" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_Price_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Price_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" align="right">
                                                    <asp:Label ID="lbl_Price" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr style="background-color: #DADADA; height: 17px;">
                                                <td>
                                                    <asp:Label ID="lbl_Comment_Detail_Nm" runat="server" SkinID="LBL_HD_1" Text="<%$ Resources:PC_PR_PrEdit, lbl_Comment_Detail_Nm %>"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr style="height: 17px">
                                                <td>
                                                    <asp:Label ID="lbl_Comment_Detail" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <%--Command Bar--%>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%;">
                                            <tr style="height: 17px;">
                                                <td align="right" style="padding-right: 10px">
                                                    <asp:LinkButton ID="lnkb_Edit" runat="server" CausesValidation="False" CommandName="Edit" SkinID="LNKB_NORMAL">Edit</asp:LinkButton>
                                                    <asp:LinkButton ID="lnkb_Delete" runat="server" CausesValidation="False" CommandName="Delete" SkinID="LNKB_NORMAL">Delete</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <tr id="TR_Summmary" runat="server">
                                    <td colspan="4" style="padding-left: 30px">
                                        <asp:Panel ID="p_Issue_Expand" runat="server">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td style="width: 8%;">
                                                    </td>
                                                    <td style="width: 7%;">
                                                    </td>
                                                    <td style="width: 7%;">
                                                    </td>
                                                    <td style="width: 20%;">
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:Label ID="lbl_CurrCurrDt_Grd_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, TitleCurrency %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                        <asp:Label ID="lbl_CurrCurrDt_Grd" runat="server" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td colspan="4">
                                                        <asp:Label ID="lbl_BaseCurrDt_Grd_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, TitleBaseCurrency %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                        <asp:Label ID="lbl_BaseCurrDt_Grd" runat="server" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_Disc_Grd_Nm" runat="server" Text="Disc." SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lbl_DiscPercent_Grd" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_CurrNetAmt_Grd_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_PR_PrEdit, lbl_NetAmt_Nm %>"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lbl_CurrNetAmt_Grd" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td style="width: 3%;">
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_NetAmt_Grd_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_NetAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lbl_NetAmt_Grd" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td style="width: 3%;">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chk_Adj" runat="server" Text="Adj." Enabled="false" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_TaxType_Grd_Nm" runat="server" Text="Tax Type" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lbl_TaxType_Grd" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_CurrDiscAmt_Grd_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_DiscAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lbl_CurrDiscAmt_Grd" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_DiscAmt_Grd_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_DiscAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lbl_DiscAmt_Grd" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_TaxRate_Grd_Nm" runat="server" Text="Tax Rate" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lbl_TaxRate_Grd" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_CurrTaxAmt_Grd_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_TaxAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lbl_CurrTaxAmt_Grd" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_TaxAmt_Grd_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_TaxAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lbl_TaxAmt_Grd" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_CurrTotalAmt_Grd_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_TotalAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
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
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <%-- /* EditMode */--%>
                                        <asp:Panel ID="p_AllocateVendor_Expand" runat="server">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td width="8%">
                                                    </td>
                                                    <td style="width: 7%;">
                                                        <%-- <asp:Label ID="lbl_CurrRate_Grd_Av_Nm" runat="server" Text="Currency Rate" SkinID="LBL_HD_GRD"></asp:Label>--%>
                                                    </td>
                                                    <td style="width: 7%;">
                                                        <%--<asp:TextBox ID="txt_CurrRate_Grd_Av" runat="server" SkinID="TXT_NUM_V1" Width="50%"
                                                                                        Enabled="false"></asp:TextBox>--%>
                                                    </td>
                                                    <td width="20%">
                                                    </td>
                                                    <td style="width: 7%" colspan="3">
                                                        <asp:Label ID="lbl_CurrCurrDt_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, TitleCurrency %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                        <asp:Label ID="lbl_CurrCurrDt" runat="server" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:Label ID="lbl_BaseCurrDt_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, TitleBaseCurrency %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                        <asp:Label ID="lbl_BaseCurrDt" runat="server" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td style="width: 7%">
                                                        <asp:Label ID="lbl_DiscPercent_Grd_Av_Nm" runat="server" Text="Disc." SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td style="width: 7%">
                                                        <asp:TextBox ID="txt_DiscPercent_Grd_Av" runat="server" AutoPostBack="true" OnTextChanged="txt_DiscPercent_grd_av_TextChanged" SkinID="TXT_NUM_V1" Width="80px"
                                                            MaxLength="6"></asp:TextBox><asp:Label ID="Label1" runat="server" Text=" %"></asp:Label>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txt_DiscPercent_Grd_Av" ValidChars="0123456789.">
                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td width="7%">
                                                        <asp:Label ID="lbl_CurrNetAmt_Grd_Av_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_PR_PrEdit, lbl_NetAmt_Nm %>"></asp:Label>
                                                    </td>
                                                    <td width="10%" align="right">
                                                        <asp:TextBox ID="txt_CurrNetAmt_Grd_Av" runat="server" SkinID="TXT_NUM_V1" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td width="5%">
                                                    </td>
                                                    <td style="width: 7%">
                                                        <asp:Label ID="lbl_NetAmt_Grd_Av_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_NetAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td style="width: 10%" align="right">
                                                        <asp:TextBox ID="txt_NetAmt_Grd_Av" runat="server" SkinID="TXT_NUM_V1" Enabled="false"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_NetAmt_Grd_Av" ValidChars="0123456789.">
                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                    </td>
                                                    <td width="3%">
                                                    </td>
                                                </tr>
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
                                                        <asp:Label ID="lbl_CurrDiscAmt_Grd_Av_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_PR_PrEdit, lbl_DiscAmt_Nm %>"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:TextBox ID="txt_CurrDiscAmt_Grd_Av" runat="server" AutoPostBack="true" SkinID="TXT_NUM_V1" OnTextChanged="txt_CurrDiscAmt_Grd_Av_TextChanged"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_DiscAmt_Grd_Av_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_DiscAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:TextBox ID="txt_DiscAmt_Grd_Av" runat="server" AutoPostBack="true" OnTextChanged="txt_DiscAmt_Grd_av_TextChanged" SkinID="TXT_NUM_V1"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_DiscAmt_Grd_Av" ValidChars="0123456789.">
                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <dx:ASPxCheckBox ID="chk_Adj_Grd_Av" runat="server" AutoPostBack="True" OnCheckedChanged="chk_Adj_Grd_Av_CheckedChanged" Text="Adj. Tax">
                                                        </dx:ASPxCheckBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_TaxType_Grd_Av_Nm" runat="server" Text="Tax Type" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxComboBox ID="ddl_TaxType_Grd_Av" runat="server" AutoPostBack="true" Font-Size="9pt" Font-Names="arial" ForeColor="#4d4d4d" ValueType="System.String"
                                                            Width="80px" SelectedIndex="0" OnSelectedIndexChanged="ddl_TaxType_Grd_Av_SelectedIndexChanged">
                                                            <Items>
                                                                <dx:ListEditItem Text="None" Value="N" Selected="True" />
                                                                <dx:ListEditItem Text="Add" Value="A" />
                                                                <dx:ListEditItem Text="Included" Value="I" />
                                                            </Items>
                                                        </dx:ASPxComboBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_CurrTaxAmt_Grd_Av_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_PR_PrEdit, lbl_TaxAmt_Nm %>"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:TextBox ID="txt_CurrTaxAmt_Grd_Av" runat="server" AutoPostBack="true" SkinID="TXT_NUM_V1" Enabled="false" OnTextChanged="txt_CurrTaxAmt_Grd_Av_TextChanged"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_TaxAmt_Grd_Av_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_TaxAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:TextBox ID="txt_TaxAmt_Grd_Av" runat="server" AutoPostBack="true" SkinID="TXT_NUM_V1"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender" runat="server" TargetControlID="txt_TaxAmt_Grd_Av" ValidChars="0123456789.">
                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_TaxRate_Grd_Av_Nm" runat="server" Text="Tax Rate" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_TaxRate_Grd_av" runat="server" AutoPostBack="true" SkinID="TXT_NUM_V1" Width="80px" MaxLength="6" OnTextChanged="txt_TaxRate_Grd_Av_TextChanged"></asp:TextBox><asp:Label
                                                            ID="Label2" runat="server" Text=" %"></asp:Label>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_TaxRate_Grd_av" ValidChars="0123456789.">
                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_CurrTotalAmt_Grd_Av_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_PR_PrEdit, lbl_TotalAmt_Nm %>"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:TextBox ID="txt_CurrTotalAmt_Grd_Av" runat="server" SkinID="TXT_NUM_V1" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_TotalAmt_Grd_Av_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_TotalAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:TextBox ID="txt_TotalAmt_Grd_Av" runat="server" SkinID="TXT_NUM_V1" Enabled="false"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_TotalAmt_Grd_Av" ValidChars="0123456789.">
                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr style="background-color: #DADADA; height: 17px;">
                                                <td>
                                                    <asp:Label ID="lbl_PriceCompare_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PC_PR_PrEdit, lbl_PriceCompare_Nm %>"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 17px; width: 100%">
                                                    <asp:GridView ID="grd_PriceCompare1" runat="server" AutoGenerateColumns="False" Width="100%" SkinID="GRD_V1" EmptyDataText="No Data to Display" EnableModelValidation="True"
                                                        ShowFooter="True">
                                                        <Columns>
                                                            <asp:BoundField HeaderText="<%$ Resources:PC_PR_PrEdit, lbl_BU_Compare_Nm %>" DataField="BuCode">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Ref#" DataField="RefNo">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="<%$ Resources:PC_PR_PrEdit, lbl_Name_Compare_Nm %>" DataField="VendorName">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Unit" DataField="OrderUnit">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="<%$ Resources:PC_PR_PrEdit, lbl_Rank_Compare_Nm %>" DataField="VendorRank">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="<%$ Resources:PC_PR_PrEdit, lbl_Price_Compare_Nm %>" DataField="Price">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="<%$ Resources:PC_PR_PrEdit, lbl_DiscPercent_Compare_Nm %>" DataField="DiscountPercent">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="<%$ Resources:PC_PR_PrEdit, lbl_DiscAmt_Compare_Nm %>" DataField="DiscountAmt">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="<%$ Resources:PC_PR_PrEdit, lbl_Foc_Compare_Nm %>" DataField="FOC">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="<%$ Resources:PC_PR_PrEdit, lbl_Min_Compare_Nm %>" DataField="QtyFrom">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="<%$ Resources:PC_PR_PrEdit, lbl_Max_Compare_Nm %>" DataField="QtyTo">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="#">
                                                                <ItemTemplate>
                                                                    <dx:ASPxButton ID="btn_Assign_av" runat="server" Text="Assign" OnClick="btn_Assign_av_Click">
                                                                    </dx:ASPxButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                        <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                            <tr style="vertical-align: top">
                                                <td class="TD_LINE_GRD" style="width: 7%;">
                                                    <asp:Label ID="lbl_OnHand_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_OnHand_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" style="width: 5%;">
                                                    <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" style="width: 10%;">
                                                    <asp:Label ID="lbl_OnOrder_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_OnOrder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" style="width: 5%;">
                                                    <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" style="width: 5%;">
                                                    <asp:Label ID="lbl_ReOrder_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_ReOrder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" style="width: 10%;">
                                                    <asp:Label ID="lbl_ReOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" style="width: 5%;">
                                                    <asp:Label ID="lbl_ReStock_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_ReStock_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" style="width: 5%;">
                                                    <asp:Label ID="lbl_ReStock" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" style="width: 10%;">
                                                    <asp:Label ID="lbl_LastPrice_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_LastPrice_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" style="width: 10%;">
                                                    <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" style="width: 10%">
                                                    <asp:Label ID="lbl_LastVendor_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_LastVendor_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" style="width: 8%; overflow: hidden; white-space: nowrap;">
                                                    <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1" Width="100px"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_Po_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Po_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_Po" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_Ref_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Ref_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_Ref" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_Buyer_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Buyer_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_Buyer" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_Order_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Order_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_Order" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_Receive_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Receive_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_Receive" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_Price_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Price_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" align="right">
                                                    <asp:Label ID="lbl_Price" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                            <tr style="background-color: #DADADA; height: 17px;">
                                                <td>
                                                    <asp:Label ID="lbl_Comment_Detail_Nm" runat="server" SkinID="LBL_HD_1" Text="<%$ Resources:PC_PR_PrEdit, lbl_Comment_Detail_Nm %>"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr style="height: 17px">
                                                <td style="width: 100%">
                                                    <asp:TextBox ID="txt_Comment_Detail" runat="server" SkinID="TXT_V1" Width="99%"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr style="height: 17px" align="right">
                                                <td style="padding-right: 10px;">
                                                    <asp:LinkButton ID="lnkb_SaveNew" runat="server" CommandName="SaveNew" SkinID="LNKB_NORMAL" Text="Save & New" CausesValidation="true" ValidationGroup="grd_Group_av"></asp:LinkButton>
                                                    <asp:LinkButton ID="lnkb_Update" runat="server" CommandName="Update" SkinID="LNKB_NORMAL" Text="Save" CausesValidation="true" ValidationGroup="grd_Group_av"></asp:LinkButton>
                                                    <asp:LinkButton ID="lnkb_Cancel" runat="server" CausesValidation="false" CommandName="Cancel" SkinID="LNKB_NORMAL" Text="Cancel"></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                    </td>
                                </tr>
                            </EditItemTemplate>
                            <HeaderStyle Width="0%"></HeaderStyle>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Panel ID="p_Issue_Empty" runat="server">
                            <table border="0" cellpadding="0" cellspacing="0" width="100%" style="display: none;">
                                <tr style="background-color: #11A6DE;">
                                    <td style="width: 10px;">
                                        <asp:ImageButton ID="Img_Create_Issue" runat="server" ImageUrl="~/App_Themes/Default/Images/Plus_1.jpg" OnClick="Img_Create_Issue_Click" ToolTip="Create"
                                            Visible="False" />
                                    </td>
                                    <td style="width: 20px;">
                                        <asp:CheckBox ID="chk_All" runat="server" Width="10px" onclick="Check(this)" />
                                    </td>
                                    <td align="left" style="width: 0px" class="<%= HiddenClassName %>">
                                        <asp:Label ID="lbl_BuCode_HD" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_BuCode_HD %>" Width="100px" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                    <td align="left" style="width: 80px">
                                        <asp:Label ID="lbl_Store_HDG_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Store_HDG_Nm %>" Width="80px" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                    <td align="left" style="width: 60px">
                                        <asp:Label ID="lbl_SKU_HDG_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_SKU_HDG_Nm %>" Width="60px" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                    <td align="left" style="width: 100px">
                                        <asp:Label ID="lbl_DescEn_HDG_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_DescEn_HDG_Nm %>" Width="100px" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                    <td align="left" style="width: 100px">
                                        <asp:Label ID="lbl_Descll_HDG_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Descll_HDG_Nm %>" Width="100px" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                    <td align="right" style="width: 50px">
                                        <asp:Label ID="lbl_ReqQty_HDG_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_QtyReq_HDG_Nm %>" Width="50px" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                    <%--/* Added on: 03/08/2017, By: Fon*/--%>
                                    <td align="right" style="width: 60px">
                                        <asp:Label ID="lbl_ApprQty_HDG_Nm" runat="server" Text="Qty Appr." Width="60px" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                    <%--/* End Added */--%>
                                    <td align="left" style="width: 40px">
                                        <asp:Label ID="lbl_Unit_HDG_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Unit_HDG_Nm %>" Width="40px" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                    <td align="left" style="width: 80px">
                                        <asp:Label ID="lbl_DeliDate_HDG_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_DeliDate_HDG_Nm %>" Width="80px" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                    <td align="left" style="width: 150px">
                                        <asp:Label ID="lbl_DeliPoint_HDG_Nm" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_DeliPoint_HDG_Nm %>" Width="120px" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="Currency" Width="70px" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="p_AlloCate_Empty" runat="server">
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr style="background-color: #11A6DE;">
                                    <td style="width: 10px;">
                                        <asp:ImageButton ID="Img_Create_Vendor" runat="server" ImageUrl="~/App_Themes/Default/Images/Plus_1.jpg" OnClick="Img_Create_Vendor_Click" ToolTip="Create"
                                            Visible="False" />
                                    </td>
                                    <td style="width: 20px;">
                                        <asp:CheckBox ID="chk_All_Av" runat="server" Width="20px" onclick="Check(this)" />
                                    </td>
                                    <td align="left" class="<%= HiddenClassName %>">
                                        <asp:Label ID="lbl_BU_HD_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_BU_HD_av %>" Width="80px" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                    <td align="left" style="width: 80px">
                                        <asp:Label ID="lbl_Vendor_HDG_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Vendor_HDG_av %>" Width="80px" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                    <td align="left" style="width: 80px">
                                        <asp:Label ID="lbl_Store_HDG_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Store_HDG_av %>" Width="80px" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                    <td style="width: 60px" align="left">
                                        <asp:Label ID="lbl_SKU_HDG_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_SKU_HDG_av %>" Width="60px" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                    <td style="width: 100px" align="left">
                                        <asp:Label ID="lbl_DescEn_HDG_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_DescEn_HDG_av %>" Width="100px" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                    <td style="width: 100px" align="left">
                                        <asp:Label ID="lbl_Descll_HDG_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Descll_HDG_av %>" Width="100px" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                    <td style="width: 80px" align="left">
                                        <asp:Label ID="lbl_DeliDate_HDG_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_DeliDate_HDG_av %>" Width="80px" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                    <td style="width: 150px" align="left">
                                        <asp:Label ID="lbl_DeliPoint_HDG_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_DeliPotin_HDG_av %>" Width="150px" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                    <td style="width: 50px" align="right">
                                        <asp:Label ID="lbl_QtyReq_HDG_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_QtyReq_HDG_av %>" Width="50px" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                    <td style="width: 40px" align="right">
                                        <asp:Label ID="lbl_FOC_HDG_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_FOC_HDG_av %>" Width="40px" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                    <td style="width: 40px" align="left">
                                        <asp:Label ID="lbl_Unit_HDG_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Unit_HDG_av %>" Width="40px" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                    <td style="width: 50px" align="right">
                                        <asp:Label ID="lbl_Price_HDG_av" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_Price_HDG_av %>" Width="50px" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                    <%--/* Added on: 03/08/2017, By: Fon*/--%>
                                    <td>
                                        <asp:Label ID="lbl_CurrCode_HDG_av" runat="server" Text="Currency" Width="70px" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_CurrRate_HDG_av" runat="server" Text="Curr. Rate" Width="70px" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
            <!--Allocate Vendor Function Bar-->
            <div style="width: 100%; display: block; padding: 10px;">
                <div style="display: inline-block; padding-left: 5px;">
                    <asp:Button ID="btn_AutoAllocateVd" runat="server" OnClick="btn_ConfirmAutoAlloVd_Click" OnClientClick="return confirm('Confirm to Auto Allocate Vendor ?');"
                        Text="<%$ Resources:PC_PR_PrEdit, btn_AutoAllocateVd %>" SkinID="BTN_V1" />
                </div>
                <div style="display: inline-block; padding-left: 5px;">
                    <asp:Button ID="btn_PriceList" runat="server" Text="<%$ Resources:PC_PR_PrEdit, btn_PriceList %>" SkinID="BTN_V1" Visible="false" />
                </div>
                <div style="display: inline-block;">
                </div>
            </div>
            <!-- Variables -->
            <asp:ObjectDataSource ID="ods_Store_Dt" runat="server" SelectMethod="GetList" TypeName="Blue.BL.Option.Inventory.StoreLct" OldValuesParameterFormatString="original_{0}">
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
            <asp:ObjectDataSource runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetLookup" TypeName="Blue.BL.Option.Inventory.DeliveryPoint"
                ID="ods_DeliPoint_Dt">
                <SelectParameters>
                    <asp:ControlParameter ControlID="hf_ConnStr" PropertyName="Value" Name="connStr" Type="String"></asp:ControlParameter>
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:ObjectDataSource ID="ods_PriceCompare" runat="server" SelectMethod="GetList" TypeName="Blue.BL.IN.PriceList">
                <SelectParameters>
                    <asp:SessionParameter DefaultValue="" Name="ProductCode" SessionField="ProductCode" Type="String" />
                    <asp:SessionParameter Name="PrDate" SessionField="ReqDate" Type="DateTime" />
                    <asp:SessionParameter Name="ReqQty" SessionField="ApprQty" Type="Decimal" />
                    <asp:ControlParameter ControlID="hf_ConnStr" Name="ConnStr" PropertyName="Value" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:HiddenField ID="hf_LoginName" runat="server" />
            <asp:HiddenField ID="hf_ConnStr" runat="server" />
            <dx:ASPxPopupControl ID="pop_AlertBox" runat="server" HeaderText="" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Width="360px"
                Modal="True" ShowPageScrollbarWhenModal="True">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                        <div style="min-width: 320px;" align="center">
                            <asp:Label ID="lbl_Pop_AlertBox" runat="server" Text="" SkinID="LBL_NR" />
                            <br />
                            <br />
                            <asp:Button ID="btn_Pop_AlertBox_OK" runat="server" SkinID="BTN_V1" Text="OK" OnClick="btn_Pop_AlertBox_Ok_Click" />
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_Alert" runat="server" HeaderText="" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Width="360px" Modal="True"
                ShowPageScrollbarWhenModal="True">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupAlertContentControl" runat="server">
                        <div style="min-width: 320px;" align="center">
                            <asp:Label ID="lbl_PopupAlert" runat="server" Text="" SkinID="LBL_NR"></asp:Label>
                            <asp:Label ID="lbl_hide_action" runat="server" Style="display: none;"></asp:Label>
                            <asp:Label ID="lbl_hide_value" runat="server" Style="display: none;"></asp:Label>
                            <asp:Label ID="lbl_PrNo" runat="server" Style="display: none;"></asp:Label>
                            <br />
                            <br />
                            <asp:Button ID="btn_PopupAlert" runat="server" SkinID="BTN_V1" Text="OK" OnClick="btn_PopupAlert_Click" />
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_ConfirmDelete" runat="server" CloseAction="CloseButton" HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Width="300px" ShowPageScrollbarWhenModal="True">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" colspan="2" height="50px">
                                    <asp:Label ID="lbl_ConfirmDelete_POP" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_ConfirmDelete_POP %>" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <%--<dx:ASPxButton ID="btn_ConfirmDelete" runat="server" OnClick="btn_ConfrimDelete_Click"
                                                        Text="Yes" Width="50px" SkinID="BTN_V1">
                                                    </dx:ASPxButton>--%>
                                    <asp:Button ID="btn_ConfirmDelete" runat="server" Text="<%$ Resources:PC_PR_PrEdit, btn_ConfirmDelete %>" OnClick="btn_ConfrimDelete_Click" SkinID="BTN_V1"
                                        Width="50px" />
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_CancelDelete" runat="server" Text="<%$ Resources:PC_PR_PrEdit, btn_CancelDelete %>" OnClick="btn_CancelDelete_Click" Width="50px" SkinID="BTN_V1" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_ConfirmChangeDeliDate" runat="server" HeaderText="Warning" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                Width="360px" Modal="True" ShowPageScrollbarWhenModal="True">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" colspan="2" height="50px">
                                    <asp:Label ID="lbl_ChangeAllDeliDate_POP" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_ChangeAllDeliDate_POP %>" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btn_ConfirmChangeDeliDate" runat="server" OnClick="btn_ConfirmChangeDeliDate_Click" Text="<%$ Resources:PC_PR_PrEdit, btn_ConfirmChangeDeliDate %>"
                                        Width="50px" SkinID="BTN_V1" />
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_CancelChangeDeliDate" runat="server" OnClick="btn_CancelChangeDeliDate_Click" Text="<%$ Resources:PC_PR_PrEdit, btn_CancelChangeDeliDate %>"
                                        Width="50px" SkinID="BTN_V1" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_AlertTaxRate" runat="server" HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                ShowCloseButton="False" Width="300px" CloseAction="CloseButton" ShowPageScrollbarWhenModal="True">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" style="height: 20px">
                                    <asp:Label ID="lbl_TaxRate_POP" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_TaxRate_POP %>" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btn_OK" runat="server" OnClick="btn_OK_Click" Text="<%$ Resources:PC_PR_PrEdit, btn_OK %>" Width="50px" SkinID="BTN_V1" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_AlertApprQty" runat="server" HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                ShowCloseButton="False" Width="300px" CloseAction="CloseButton" ShowPageScrollbarWhenModal="True">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl6" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" style="height: 20px">
                                    <asp:Label ID="lbl_ApprQty_POP" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_ApprQty_POP %>" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btn_OK_ApprQty" runat="server" OnClick="btn_OK_ApprQty_Click" Text="<%$ Resources:PC_PR_PrEdit, btn_OK_ApprQty %>" Width="50px" SkinID="BTN_V1" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_AlertPrice" runat="server" HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                ShowCloseButton="False" Width="300px" CloseAction="CloseButton" ShowPageScrollbarWhenModal="True">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl7" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" style="height: 20px">
                                    <asp:Label ID="lbl_PriceGreaterThan_POP" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_PriceGreaterThan_POP %>" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <%--<dx:ASPxButton ID="btn_Price_OK" runat="server" OnClick="btn_Price_OK_Click" Text="OK"
                                                        Width="50px" SkinID="BTN_N1">
                                                    </dx:ASPxButton>--%>
                                    <asp:Button ID="btn_Price_OK" runat="server" OnClick="btn_Price_OK_Click" Text="<%$ Resources:PC_PR_PrEdit, btn_Price_OK %>" Width="50px" SkinID="BTN_V1" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_Save" runat="server" CloseAction="CloseButton" HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                ShowCloseButton="False" Width="300px" ShowPageScrollbarWhenModal="True">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl8" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" height="50px">
                                    <asp:Label ID="lbl_RecordNotSave_POP" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_RecordNotSave_POP %>" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btn_OK_Save" runat="server" OnClick="btn_OK_Save_Click" Text="<%$ Resources:PC_PR_PrEdit, btn_OK_Save %>" Width="50px" SkinID="BTN_V1" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_AddVendor" runat="server" HeaderText="Warning" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Width="360px"
                Modal="True" ShowPageScrollbarWhenModal="True">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl9" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" colspan="2" height="50px">
                                    <asp:Label ID="lbl_AddVendor_POP" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_AddVendor_POP %>" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <%--<dx:ASPxButton ID="btn_OkAddVendor" runat="server" Text="Yes" Width="50px" OnClick="btn_OkAddVendor_Click"
                                                        SkinID="BTN_N1">
                                                    </dx:ASPxButton>--%>
                                    <asp:Button ID="btn_OkAddVendor" runat="server" Text="<%$ Resources:PC_PR_PrEdit, btn_OkAddVendor %>" Width="50px" OnClick="btn_OkAddVendor_Click" SkinID="BTN_V1" />
                                </td>
                                <td align="left" style="width: 160px">
                                    <%--<dx:ASPxButton ID="btn_CancelAddVendor" runat="server" Text="No" Width="50px" OnClick="btn_CancelAddVendor_Click"
                                                        SkinID="BTN_N1">
                                                    </dx:ASPxButton>--%>
                                    <asp:Button ID="btn_CancelAddVendor" runat="server" Text="<%$ Resources:PC_PR_PrEdit, btn_CancelAddVendor %>" Width="50px" OnClick="btn_CancelAddVendor_Click"
                                        SkinID="BTN_V1" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_ConfirmSaveAddVendor" runat="server" HeaderText="Warning" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                Width="360px" Modal="True" ShowPageScrollbarWhenModal="True">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl10" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" colspan="2" height="50px">
                                    <asp:Label ID="lbl_ConfirmSaveVendor_POP" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_ConfirmSaveVendor_POP %>" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <%--<dx:ASPxButton ID="btn_ConfirmSaveAddVendor" runat="server" Text="Yes" Width="50px"
                                                        OnClick="btn_ConfirmSaveAddVendor_Click" SkinID="BTN_N1">
                                                    </dx:ASPxButton>--%>
                                    <asp:Button ID="btn_ConfirmSaveAddVendor" runat="server" Text="<%$ Resources:PC_PR_PrEdit, btn_ConfirmSaveAddVendor %>" Width="50px" OnClick="btn_ConfirmSaveAddVendor_Click"
                                        SkinID="BTN_V1" />
                                </td>
                                <td align="left" style="width: 160px">
                                    <%-- <dx:ASPxButton ID="btn_CancelSaveAddVendor" runat="server" Text="No" Width="50px"
                                                        OnClick="btn_CancelSaveAddVendor_Click" SkinID="BTN_N1">
                                                    </dx:ASPxButton>--%>
                                    <asp:Button ID="btn_CancelSaveAddVendor" runat="server" Text="<%$ Resources:PC_PR_PrEdit, btn_CancelSaveAddVendor %>" Width="50px" OnClick="btn_CancelSaveAddVendor_Click"
                                        SkinID="BTN_V1" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_AlertProductSame" runat="server" HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                ShowCloseButton="False" Width="300px" CloseAction="CloseButton" ShowPageScrollbarWhenModal="True">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl11" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" style="height: 20px">
                                    <asp:Label ID="lbl_WarningProduct_POP" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_WarningProduct_POP %>" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btn_Warning_OK" runat="server" Text="<%$ Resources:PC_PR_PrEdit, btn_Warning_OK %>" Width="60px" SkinID="BTN_V1" OnClick="btn_Warning_OK_Click" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_AlertProdCateType" runat="server" HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                ShowCloseButton="False" Width="300px" CloseAction="CloseButton" ShowPageScrollbarWhenModal="True">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl12" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" style="height: 20px">
                                    <asp:Label ID="lbl_WarningProductCateType_POP" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_WarningProductCateType_POP %>" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btn_WarningProdCateType_OK" runat="server" Text="<%$ Resources:PC_PR_PrEdit, btn_WarningProdCateType_OK %>" Width="60px" SkinID="BTN_V1"
                                        OnClick="btn_WarningProdCateType_OK_Click" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_ConfirmDelete_Grd" runat="server" CloseAction="CloseButton" HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Width="300px" ShowPageScrollbarWhenModal="True">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl13" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" colspan="2" height="50px">
                                    <asp:Label ID="lbl_ConfirmDelete_POP0" runat="server" Text="<%$ Resources:PC_PR_PrEdit, lbl_ConfirmDelete_POP %>" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <%--<dx:ASPxButton ID="btn_ConfirmDelete" runat="server" OnClick="btn_ConfrimDelete_Click"
                                                        Text="Yes" Width="50px" SkinID="BTN_V1">
                                                    </dx:ASPxButton>--%>
                                    <asp:Button ID="btn_ConfirmDelete_Grd" runat="server" Text="<%$ Resources:PC_PR_PrEdit, btn_ConfirmDelete %>" SkinID="BTN_V1" Width="50px" OnClick="btn_ConfirmDelete_Grd_Click" />
                                </td>
                                <td align="left">
                                    <%--<dx:ASPxButton ID="btn_CancelDelete" runat="server" OnClick="btn_CancelDelete_Click"
                                                        Text="No" Width="50px" SkinID="BTN_N1">
                                                    </dx:ASPxButton>--%>
                                    <asp:Button ID="btn_CancelDelete_Grd" runat="server" Text="<%$ Resources:PC_PR_PrEdit, btn_CancelDelete %>" Width="50px" SkinID="BTN_V1" OnClick="btn_CancelDelete_Grd_Click" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_AlertQty" runat="server" HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                ShowCloseButton="False" Width="300px" CloseAction="CloseButton" AllowResize="True" AutoUpdatePosition="True" ShowPageScrollbarWhenModal="True">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl14" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" style="height: 20px">
                                    <asp:Label ID="lbl_WarningQty" runat="server" Text="Qty Req. greater than 0" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btn_WarningQty" runat="server" Text="OK" Width="60px" SkinID="BTN_V1" OnClick="btn_WarningQty_Click" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_AlertDisc" runat="server" HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                ShowCloseButton="False" Width="300px" CloseAction="CloseButton" ShowPageScrollbarWhenModal="True">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl15" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" style="height: 20px">
                                    <asp:Label ID="lbl_AlertDisc" runat="server" Text="Disc. can not be greater than 100 %" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btn_WarningDisc" runat="server" Text="OK" Width="60px" SkinID="BTN_V1" OnClick="btn_WarningDisc_Click" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_AlertDiscAmt" runat="server" HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                ShowCloseButton="False" Width="300px" CloseAction="CloseButton" ShowPageScrollbarWhenModal="True">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl16" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" style="height: 20px">
                                    <asp:Label ID="lbl_AlertDiscAmt" runat="server" Text="Disc. Amount/Unit can not be greater than Price" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btn_WarningDiscAmt" runat="server" Text="OK" Width="60px" SkinID="BTN_V1" OnClick="btn_WarningDiscAmt_Click" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_AddPriceLst" runat="server" CloseAction="CloseButton" HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Width="300px" ShowPageScrollbarWhenModal="True">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl17" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" colspan="2" height="50px">
                                    <asp:Label ID="lbl_AddPriceLst_Nm" runat="server" Text="Confirm to add price list." SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btn_ConfirmAddPriceLst" runat="server" Text="Yes" SkinID="BTN_V1" Width="50px" OnClick="btn_ConfirmAddPriceLst_Click" />
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_CancelAddPriceLst" runat="server" Text="No" Width="50px" SkinID="BTN_V1" OnClick="btn_CancelAddPriceLst_Click" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_WarningPeriod" runat="server" HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                ShowCloseButton="False" Width="300px" CloseAction="CloseButton" ShowPageScrollbarWhenModal="True">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl18" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" style="height: 20px">
                                    <asp:Label ID="lbl_WarningPeriod" runat="server" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btn_WarningPeriod" runat="server" Text="OK" Width="60px" SkinID="BTN_V1" OnClick="btn_WarningPeriod_Click" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <%--Added on: 10/11/2017, By:Fon--%>
            <dx:ASPxPopupControl ID="pop_Product_Location" runat="server" HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                ShowCloseButton="False" Width="360px" CloseAction="CloseButton" ShowPageScrollbarWhenModal="True">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl20" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td colspan="2" align="center" style="height: 20px">
                                    <asp:Label ID="lbl_Warning_ProductLocate" runat="server" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btn_Yes_popPL" runat="server" Text="Yes" Width="60px" SkinID="BTN_V1" OnClick="btn_Yes_popPL_Click" />
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_No_popPL" runat="server" Text="No" Width="60px" SkinID="BTN_V1" OnClick="btn_No_popPL_Click" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <%--End Added.--%>
            <dx:ASPxPopupControl ID="pop_NewVendor" runat="server" HeaderText="Warning" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" PopupAction="None"
                Modal="True" ShowPageScrollbarWhenModal="True">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl19" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btn_Save" runat="server" Text="<%$ Resources:PC_PR_PrEdit, btn_Save %>" OnClick="btn_Save_Click" SkinID="BTN_V1" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" height="50px">
                                    <asp:GridView ID="grd_Vendor" runat="server" AutoGenerateColumns="False" Width="900px" SkinID="GRD_V1" EmptyDataText="No Data to Display">
                                        <Columns>
                                            <asp:TemplateField HeaderText="#">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk_Vendor" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="<%$ Resources:PC_PR_PrEdit, lbl_Bu_GrdCom_Nm %>" DataField="BuCode" ItemStyle-Width="10px">
                                                <ItemStyle Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="<%$ Resources:PC_PR_PrEdit, lbl_Ref_GrdCom_Nm %>" DataField="RefNo" ItemStyle-Width="80px">
                                                <ItemStyle Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="<%$ Resources:PC_PR_PrEdit, lbl_Vendor_GrdCom_Nm %>" DataField="VendorCode" ItemStyle-Width="50px">
                                                <ItemStyle Width="50px" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="<%$ Resources:PC_PR_PrEdit, lbl_Name_GrdCom_Nm %>" DataField="VendorName" ItemStyle-Width="200px">
                                                <ItemStyle Width="200px" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="<%$ Resources:PC_PR_PrEdit, lbl_Rank_GrdCom_Nm %>" DataField="VendorRank" ItemStyle-Width="50px">
                                                <ItemStyle Width="50px" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="<%$ Resources:PC_PR_PrEdit, lbl_Price_GrdCom_Nm %>" DataField="Price" ItemStyle-Width="70px">
                                                <ItemStyle Width="70px" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="<%$ Resources:PC_PR_PrEdit, lbl_DiscPer_GrdCom_Nm %>" DataField="DiscountPercent" ItemStyle-Width="50px">
                                                <ItemStyle Width="50px" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="<%$ Resources:PC_PR_PrEdit, lbl_DiscAmt_GrdCom_Nm %>" DataField="DiscountAmt" ItemStyle-Width="70px">
                                                <ItemStyle Width="70px" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="<%$ Resources:PC_PR_PrEdit, lbl_Foc_GrdCom_Nm %>" DataField="FOC" ItemStyle-Width="50px">
                                                <ItemStyle Width="50px" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="<%$ Resources:PC_PR_PrEdit, lbl_Min_GrdCom_Nm %>" DataField="QtyFrom" ItemStyle-Width="25px">
                                                <ItemStyle Width="25px" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="<%$ Resources:PC_PR_PrEdit, lbl_Max_GrdCom_Nm %>" DataField="QtyTo" ItemStyle-Width="25px">
                                                <ItemStyle Width="25px" />
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="menu_GrdBar" EventName="ItemClick" />
            <asp:AsyncPostBackTrigger ControlID="menu_GrdBar" EventName="ItemClick" />
        </Triggers>
    </asp:UpdatePanel>
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UdPgDetail" PopupControlID="UdPgDetail" BackgroundCssClass="POPUP_BG"
        RepositionMode="RepositionOnWindowResizeAndScroll">
    </ajaxToolkit:ModalPopupExtender>
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
    <dx:ASPxPopupControl ID="pop_UserRequired" runat="server" HeaderText="Warning" ShowCloseButton="False" Width="480px" Modal="True" ShowPageScrollbarWhenModal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                <div style="text-align: center">
                    <asp:Label runat="server" ID="lbl_UserRequired" />
                    <br />
                    <br />
                    <br />
                    <asp:Button ID="btn_OK_UserRequired" runat="server" Text="OK" Width="60px" SkinID="BTN_V1" OnClick="btn_OK_UserRequired_Click" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
