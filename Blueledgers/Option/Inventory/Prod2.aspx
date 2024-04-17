<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="Prod2.aspx.cs" Inherits="BlueLedger.PL.Option.Inventory.Prod2" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Src="~/UserControl/Log2.ascx" TagName="Log2" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        @media print
        {
            body *
            {
                visibility: hidden;
            }
            .printable, .printable *
            {
                visibility: visible;
            }
            .printable
            {
                position: absolute;
                left: 0;
                top: 0;
            }
    </style>
    <style>
        body
        {
            height: fit-content !important;
        }
        .badge
        {
            padding: 5px 8px 5px 8px;
            background-color: Green;
            color: White !important;
            border-radius: 20px;
        }
        .badge-inactive
        {
            padding: 5px 8px 5px 8px;
            background-color: Gray;
            color: White !important;
            border-radius: 20px;
        }
    </style>
    <style>
        .d-flex
        {
            display: flex !important;
            height: fit-content;
        }
        
        .d-flex-wrap
        {
            flex-wrap: wrap;
        }
        .flex-column
        {
            display: flex;
            flex-direction: column;
        }
        .w-100
        {
            width: 100% !important;
        }
        .w-50
        {
            width: 50% !important;
        }
        .p-3
        {
            padding: 10px;
        }
        .ms-3
        {
            margin-left: 10px !important;
        }
        .me-3
        {
            margin-right: 10px !important;
        }
        .mt-3
        {
            margin-top: 10px !important;
        }
        .mb-3
        {
            margin-bottom: 10px !important;
        }
        .v-top
        {
            vertical-align: top !important;
        }
        .border
        {
            border: 1px solid silver !important;
        }
    </style>
    <style>
        .tab
        {
            overflow: hidden;
            border: 1px solid #ccc;
            background-color: #f1f1f1;
        }
        
        /* Style the buttons inside the tab */
        .tab button
        {
            background-color: inherit;
            float: left;
            border: none;
            outline: none;
            cursor: pointer;
            padding: 14px 16px;
            transition: 0.3s;
            font-size: 17px;
        }
        
        /* Change background color of buttons on hover */
        .tab button:hover
        {
            background-color: #ddd;
        }
        
        /* Create an active/current tablink class */
        .tab button.active
        {
            background-color: #ccc;
        }
        
        /* Style the tab content */
        .tabcontent
        {
            display: none;
            padding: 6px 12px;
            border: 1px solid #ccc;
            border-top: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <asp:HiddenField ID="hf_ProductCode" runat="server" />
    <!-- Title Bar-->
    <table border="0" cellpadding="1" cellspacing="0" width="100%">
        <tr style="background-color: #4d4d4d; height: 17px; padding-left: 10px">
            <td style="padding-left: 10px; width: 10px">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
            </td>
            <td align="left">
                <asp:Label ID="lbl_TitleHD" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_TitleHD %>" SkinID="LBL_HD_WHITE" />
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
                        <dx:MenuItem Name="Create" Text="">
                            <ItemStyle Height="16px" Width="49px">
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/create.png" Repeat="NoRepeat" VerticalPosition="center" />
                            </ItemStyle>
                        </dx:MenuItem>
                        <dx:MenuItem Name="Edit" Text="">
                            <ItemStyle Height="16px" Width="38px">
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-edit.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/edit.png" Repeat="NoRepeat" VerticalPosition="center" />
                            </ItemStyle>
                        </dx:MenuItem>
                        <dx:MenuItem Name="Delete" Text="">
                            <ItemStyle Height="16px" Width="41px">
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                            </ItemStyle>
                        </dx:MenuItem>
                        <dx:MenuItem Name="Print" Text="">
                            <ItemStyle Height="16px" Width="42px">
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-print.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
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
                    <Paddings Padding="0px" />
                    <SeparatorPaddings Padding="0px" />
                    <SubMenuStyle HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" Font-Size="9pt" ForeColor="#4D4D4D" />
                    <Border BorderStyle="None"></Border>
                </dx:ASPxMenu>
            </td>
        </tr>
    </table>
    <div class="printable">
        <%--<asp:UpdatePanel ID="UpdatePanel" runat="server">
            <ContentTemplate>
        </ContentTemplate>
            <Triggers>
            <asp:PostBackTrigger ControlID="btn_AssignLocation" />
            <asp:PostBackTrigger ControlID="btn_AssignVendor" />
            </Triggers>
        </asp:UpdatePanel>--%>
        <!-- Product Summary -->
        <table style="width: 100%; height: 30px;">
            <tr>
                <td>
                    <span style="font-size: medium; font-weight: bold;">#</span>
                    <asp:Label ID="lbl_ProductCode" runat="server" Font-Size="Medium" Font-Bold="true" />
                    &nbsp;
                    <asp:Label ID="lbl_Status" runat="server" CssClass="badge-inactive" />
                </td>
                <td align="right">
                    <asp:Label ID="lbl_LastCost_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderLastCost %>" />
                    &nbsp;
                    <asp:Label ID="lbl_LastCost" runat="server" Font-Size="Small" Font-Bold="true" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <br />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_Created" runat="server" Font-Size="Smaller" Font-Italic="true" ForeColor="Gray" />
                </td>
                <td align="right">
                    <asp:Button ID="btn_History" runat="server" Text="Latest Purchase" OnClick="btn_History_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
        </table>
        <!-- Product detail -->
        <table id="table_product" class="w-100" style="vertical-align: top;" cellspacing="10">
            <tr>
                <td style="width: 13%">
                    <asp:Label ID="lbl_Cateogry_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:Option.Inventory.Product, lbl_Cateogry_Nm %>" />
                </td>
                <td style="width: 20%">
                    <asp:Label ID="lbl_Category" runat="server" SkinID="LBL_NR_BLUE" Font-Size="Small" Font-Bold="true" />
                </td>
                <td style="width: 13%">
                    <asp:Label ID="lbl_SubCateogry_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:Option.Inventory.Product, lbl_SubCateogry_Nm %>" />
                </td>
                <td style="width: 20%">
                    <asp:Label ID="lbl_SubCategory" runat="server" SkinID="LBL_NR_BLUE" Font-Size="Small" Font-Bold="true" />
                </td>
                <td style="width: 13%">
                    <asp:Label ID="lbl_ItemGroup_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:Option.Inventory.Product, lbl_ItemGroup_Nm %>" />
                </td>
                <td style="width: 20%">
                    <asp:Label ID="lbl_ItemGroup" runat="server" SkinID="LBL_NR_BLUE" Font-Size="Small" Font-Bold="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_ProductDesc1_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderDescritpion1 %>" />
                </td>
                <td>
                    <asp:Label ID="lbl_ProductDesc1" runat="server" SkinID="LBL_NR_BLUE" Font-Size="Small" />
                </td>
                <td>
                    <asp:Label ID="lbl_ProductDesc2_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderDescription2 %>" />
                </td>
                <td>
                    <asp:Label ID="lbl_ProductDesc2" runat="server" SkinID="LBL_NR_BLUE" Font-Size="Small" />
                </td>
                <td>
                    <asp:Label ID="lbl_BarCode_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderBarCode %>" />
                </td>
                <td>
                    <asp:Label ID="lbl_BarCode" runat="server" SkinID="LBL_NR_BLUE" Font-Size="Small" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_TaxType_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderTAXType %>" />
                </td>
                <td>
                    <asp:Label ID="lbl_TaxType" runat="server" SkinID="LBL_NR_BLUE" Font-Size="Small" />
                </td>
                <td>
                    <asp:Label ID="lbl_TaxRate_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderTaxRate %>" />
                </td>
                <td>
                    <asp:Label ID="lbl_TaxRate" runat="server" SkinID="LBL_NR_BLUE" Font-Size="Small" />
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <hr />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_StandardCost_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderStandardCost %>" />
                </td>
                <td>
                    <asp:Label ID="lbl_StandardCost" runat="server" SkinID="LBL_NR_BLUE" Font-Size="Small" />
                </td>
                <td>
                    <asp:Label ID="lbl_QuantityDeviation_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderQuantityDeviation %>" />
                </td>
                <td>
                    <asp:Label ID="lbl_QuantityDeviation" runat="server" SkinID="LBL_NR_BLUE" Font-Size="Small" />
                </td>
                <td>
                    <asp:Label ID="lbl_PriceDeviation_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderPriceDeviation %>" />
                </td>
                <td>
                    <asp:Label ID="lbl_PriceDeviation" runat="server" SkinID="LBL_NR_BLUE" Font-Size="Small" />
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <hr />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Label ID="lbl_DefaultUnit_Nm" runat="server" Font-Size="Small" Font-Bold="true" Text="Default Unit(s)" />
                </td>
                <td>
                    <asp:CheckBox ID="chk_IsRecipe" runat="server" Font-Size="Small" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderMemberInRecipe %>" Enabled="false" />
                </td>
                <td>
                    <asp:CheckBox ID="chk_SaleItem" runat="server" Font-Size="Small" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderSaleItem %>" Enabled="false" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_InventoryUnit_Nm" runat="server" Width="100" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderInventoryUnit %>" SkinID="LBL_HD" />
                </td>
                <td>
                    <asp:Label ID="lbl_InventoryUnit" runat="server" SkinID="LBL_NR_BLUE" Font-Size="Small" />
                </td>
                <td>
                    <asp:Label ID="lbl_OrderUnit_Nm" runat="server" Width="100" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderOrderUnit %>" SkinID="LBL_HD" />
                </td>
                <td>
                    <asp:Label ID="lbl_OrderUnit" runat="server" SkinID="LBL_NR_BLUE" Font-Size="Small" />
                </td>
                <td>
                    <asp:Label ID="lbl_RecipeUnit_Nm" runat="server" Width="100" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderRecipeUnit %>" SkinID="LBL_HD" />
                </td>
                <td>
                    <asp:Label ID="lbl_RecipeUnit" runat="server" SkinID="LBL_NR_BLUE" Font-Size="Small" />
                </td>
            </tr>
        </table>
    </div>
    <div class="d-flex d-flex-wrap">
        <!-- Order Unit -->
        <div class="border p-3 me-3">
            <h3>
                Order Unit
            </h3>
            <div style="height: 320px; overflow: scroll;">
                <asp:GridView ID="gv_OrderUnit" runat="server" Width="350" Font-Size="Small" AutoGenerateColumns="false" SkinID="GRD_V1" ShowHeaderWhenEmpty="true">
                    <HeaderStyle HorizontalAlign="Left" />
                    <AlternatingRowStyle BackColor="WhiteSmoke" />
                    <Columns>
                        <asp:BoundField HeaderText="Code" DataField="Code" />
                        <asp:TemplateField HeaderText="Rate">
                            <ItemTemplate>
                                <%# ((decimal)Eval("Rate")).ToString("N") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <!-- Recipe Unit -->
        <div class="border p-3 me-3">
            <h3>
                Recipe Unit
            </h3>
            <div style="height: 320px; overflow: scroll;">
                <asp:GridView ID="gv_RecipeUnit" runat="server" Width="350" Font-Size="Small" AutoGenerateColumns="false" SkinID="GRD_V1" ShowHeaderWhenEmpty="true">
                    <HeaderStyle HorizontalAlign="Left" />
                    <AlternatingRowStyle BackColor="WhiteSmoke" />
                    <Columns>
                        <asp:BoundField HeaderText="Code" DataField="Code" />
                        <asp:TemplateField HeaderText="Rate">
                            <ItemTemplate>
                                <%# ((decimal)Eval("Rate")).ToString("N") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <!-- Assign Store/Location -->
        <div class="border p-3 me-3">
            <table class="w-100">
                <tr>
                    <td>
                        <h3>
                            Assign to Store/Location
                        </h3>
                    </td>
                    <td align="right">
                        <asp:Button ID="btn_AssignLocation" runat="server" Font-Size="Small" Text="Assign" OnClick="btn_AssignLocation_Click" />
                    </td>
                </tr>
            </table>
            <div style="height: 320px; overflow: scroll;">
                <asp:GridView ID="gv_Location" runat="server" Width="350" Font-Size="Small" AutoGenerateColumns="false" SkinID="GRD_V1" ShowHeaderWhenEmpty="true">
                    <HeaderStyle HorizontalAlign="Left" />
                    <AlternatingRowStyle BackColor="WhiteSmoke" />
                    <Columns>
                        <asp:BoundField HeaderText="Code" DataField="LocationCode" />
                        <asp:BoundField HeaderText="Name" DataField="LocationName" />
                        <asp:TemplateField HeaderText="Min">
                            <ItemTemplate>
                                <%# ((decimal)Eval("MinQty")).ToString("N") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Max">
                            <ItemTemplate>
                                <%# ((decimal)Eval("MaxQty")).ToString("N") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <!-- Historical Purchase -->
        <!-- Assign Vendor (Obsolete)-->
        <%--<div class="card p-3 me-3">
                <table class="w-100">
                    <tr>
                        <td>
                            <h3>
                                Assign to Vendor
                            </h3>
                        </td>
                        <td align="right">
                            <asp:Button ID="btn_AssignVendor" runat="server" Font-Size="Small" Text="Assign" OnClick="btn_AssignVendor_Click" />
                        </td>
                    </tr>
                </table>
                <div style="height:320px; overflow:scroll;">

                    <asp:GridView ID="gv_Vendor" runat="server" Width="350" Font-Size="Small" AutoGenerateColumns="false" SkinID="GRD_V1" ShowHeaderWhenEmpty="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <AlternatingRowStyle BackColor="WhiteSmoke" />
                        <Columns>
                            <asp:BoundField HeaderText="Code" DataField="VendorCode" />
                            <asp:BoundField HeaderText="Name" DataField="VendorName" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>--%>
    </div>
    <!-- Popup -->
    <dx:ASPxPopupControl runat="server" ID="pop_ConfirmDelete" ClientInstanceName="pop_ConfirmDelete" Width="300px" HeaderText="<%$ Resources:Option.Inventory.Product, btn_Confrim %>"
        Modal="True" CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="true">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl_ConfirmDelete" runat="server">
                <div style="text-align: center;">
                    <asp:Label ID="lbl_ConfirmDelete" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_ConfirmMessage %>" SkinID="LBL_NR" />
                </div>
                <div style="padding-top: 50px; text-align: center;">
                    <asp:Button ID="btn_Pop_ConfirmDelete_Yes" runat="server" Width="50px" SkinID="BTN_V1" Text="<%$ Resources:Option.Inventory.Product, btn_Confrim %>" OnClick="btn_Pop_ConfirmDelete_Yes_Click" />
                    <asp:Button ID="btn_Pop_ConfirmDelete_No" runat="server" Width="50px" SkinID="BTN_V1" Text="<%$ Resources:Option.Inventory.Product, btn_Cancel %>" OnClientClick="pop_ConfirmDelete.Hide();" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl runat="server" ID="pop_AssignLocation" ClientInstanceName="pop_AssignLocation" Width="420" HeaderText="Store/Location" Modal="True"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="true">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl_AssignLocation" runat="server">
                <div class="w-100" style="height: 420px; overflow: scroll;">
                    <asp:GridView ID="gv_AssignLocation" runat="server" SkinID="GRD_V1" Width="100%" Height="200" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
                        <HeaderStyle HorizontalAlign="Left" Height="24" />
                        <RowStyle Height="28" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chk_SelectAll_Location" ClientIDMode="Static" runat="server" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk_Select" runat="server" Checked='<%# Eval("IsChecked").ToString()=="1" %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Code">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_LocationCode" runat="server" Text='<%# Eval("LocationCode") %>' AssociatedControlID="chk_Select" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_LocationName" runat="server" Text='<%# Eval("LocationName") %>' AssociatedControlID="chk_Select" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Min">
                                <ItemTemplate>
                                    <dx:ASPxSpinEdit ID="se_MinQty" runat="server" Width="70" AutoPostBack="False" SpinButtons-ShowIncrementButtons="False" NullText="0" AllowNull="false"
                                        Text='<%# Eval("MinQty") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Max">
                                <ItemTemplate>
                                    <dx:ASPxSpinEdit ID="se_MaxQty" runat="server" Width="70" AutoPostBack="False" SpinButtons-ShowIncrementButtons="False" NullText="0" AllowNull="false"
                                        Text='<%# Eval("MaxQty") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
                <div class="w-100" style="text-align: center;">
                    <asp:Button ID="btn_AssignLocation_Save" runat="server" Width="60" SkinID="BTN_V1" Text="Save" OnClick="btn_AssignLocation_Save_Click" />
                    <asp:Button ID="btn_AssignLocation_Cancel" runat="server" Width="60" SkinID="BTN_V1" Text="Cancel" OnClientClick="pop_AssignLocation.Hide();" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <%--<dx:ASPxPopupControl runat="server" ID="pop_AssignVendor" ClientInstanceName="pop_AssignVendor" Width="420" HeaderText="Vendor" Modal="True" CloseAction="CloseButton"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="true">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl_AssignVendor" runat="server">
                    <div class="w-100" style="height: 420px; overflow: scroll;">
                        <asp:GridView ID="gv_AssignVendor" runat="server" SkinID="GRD_V1" Width="100%" Height="200" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
                            <HeaderStyle HorizontalAlign="Left" Height="24" />
                            <RowStyle Height="28" />
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chk_SelectAll_Vendor" ClientIDMode="Static" runat="server" Width="40" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk_Select" runat="server" Width="40" Checked='<%# Eval("IsChecked").ToString()=="1" %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Code">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_VendorCode" runat="server" Width="60" Text='<%# Eval("VendorCode") %>' AssociatedControlID="chk_Select" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_VendorName" runat="server" Text='<%# Eval("VendorName") %>' AssociatedControlID="chk_Select" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="w-100" style="padding-top: 50px; text-align: center;">
                        <asp:Button ID="btn_AssignVendor_Save" runat="server" Width="50px" SkinID="BTN_V1" Text="Save" OnClick="btn_AssignVendor_Save_Click" />
                        <asp:Button ID="btn_AssignVendor_Cancel" runat="server" Width="50px" SkinID="BTN_V1" Text="Cancel" OnClientClick="pop_AssignVendor.Hide();" />
                    </div>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>--%>
    <dx:ASPxPopupControl ID="pop_History" runat="server" ClientInstanceName="pop_History" Width="920" HeaderText="Top 10 latest purchasing" Modal="True" CloseAction="CloseButton"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl_History" runat="server">
                <div class="w-100">
                    <asp:Label ID="lbl_History_PO" runat="server" Font-Size="Small" Font-Bold="true" Text="Purchase Order (PO)" />
                    <asp:GridView ID="gv_History_PO" runat="server" AutoGenerateColumns="False" SkinID="GRD_V1" Width="100%" ShowHeaderWhenEmpty="True">
                        <HeaderStyle HorizontalAlign="Left" />
                        <Columns>
                            <asp:BoundField HeaderText="No." DataField="RowId" />
                            <asp:BoundField HeaderText="Delivery Date" DataField="DocDate" DataFormatString="{0:d}" HtmlEncode="false" />
                            <asp:BoundField HeaderText="Vendor" DataField="Vendor" />
                            <asp:BoundField HeaderText="Location" DataField="Location" />
                            <asp:BoundField HeaderText="Description" DataField="Description" />
                            <asp:BoundField HeaderText="Status" DataField="DocStatus" ItemStyle-HorizontalAlign="Center"/>
                            <asp:BoundField HeaderText="Document#" DataField="DocNo" />
                            <asp:BoundField HeaderText="Qty" DataField="Qty" />
                            <asp:BoundField HeaderText="Unit" DataField="Unit" ItemStyle-HorizontalAlign="Center"/>
                            <asp:BoundField HeaderText="Price" DataField="Price" ItemStyle-HorizontalAlign="Right"/>
                            <asp:BoundField HeaderText="Currency" DataField="CurrencyCode" ItemStyle-HorizontalAlign="Center"/>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:Label ID="lbl_History_RC" runat="server" Font-Size="Small" Font-Bold="true" Text="Receiving" />
                    <asp:GridView ID="gv_History_RC" runat="server" AutoGenerateColumns="False" SkinID="GRD_V1" Width="100%" ShowHeaderWhenEmpty="True">
                        <HeaderStyle HorizontalAlign="Left" />
                        <Columns>
                            <asp:BoundField HeaderText="No." DataField="RowId" />
                            <asp:BoundField HeaderText="Receiving Date" DataField="DocDate" DataFormatString="{0:d}" HtmlEncode="false" />
                            <asp:BoundField HeaderText="Vendor" DataField="Vendor" />
                            <asp:BoundField HeaderText="Location" DataField="Location" />
                            <asp:BoundField HeaderText="Description" DataField="Description" />
                            <asp:BoundField HeaderText="Status" DataField="DocStatus" ItemStyle-HorizontalAlign="Center"/>
                            <asp:BoundField HeaderText="Document#" DataField="DocNo" />
                            <asp:BoundField HeaderText="Qty" DataField="Qty" />
                            <asp:BoundField HeaderText="Unit" DataField="Unit" ItemStyle-HorizontalAlign="Center"/>
                            <asp:BoundField HeaderText="Price" DataField="Price" ItemStyle-HorizontalAlign="Right"/>
                            <asp:BoundField HeaderText="Currency" DataField="CurrencyCode" ItemStyle-HorizontalAlign="Center" />
                        </Columns>
                    </asp:GridView>
                </div>
                <div style="padding-top: 10px; text-align: center;">
                    <asp:Button ID="btn_Pop_History_Close" runat="server" Text="Close" Width="50px" SkinID="BTN_V1" OnClientClick="pop_History.Hide()" />
                </div>
            </dx:PopupControlContentControl>

        </ContentCollection>
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ID="pop_Alert" ClientInstanceName="pop_Alert" runat="server" Width="320" HeaderText="Alert" ShowHeader="true" CloseAction="CloseButton"
        Modal="True" AutoUpdatePosition="True" AllowDragging="True" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" ShowPageScrollbarWhenModal="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl_Alert" runat="server">
                <div style="width: 100%; text-align: center;">
                    <asp:Label ID="lbl_Alert" runat="server" />
                    <br />
                    <br />
                    <br />
                    <asp:Button ID="btn_Alert_Ok" runat="server" Width="80" Text="OK" OnClientClick="pop_Alert.Hide();" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <script type="text/javascript">
        $(document).ready(function () {

            $('#chk_SelectAll_Location').change(function () {
                var checked = $(this).prop('checked');

                $("[type=checkbox]").prop('checked', checked);

            });

            $('#chk_SelectAll_Vendor').change(function () {
                var checked = $(this).prop('checked');

                $("[type=checkbox]").prop('checked', checked);

            });

        });
    </script>
</asp:Content>
