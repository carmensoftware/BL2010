<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="ProdEdit2.aspx.cs"
    Inherits="BlueLedger.PL.Option.Inventory.ProdEdit2" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1" Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
        .gv td, th
        {
            padding: 5px;
            line-height: 24px;
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="server">
    <asp:HiddenField runat="server" ID="hf_PriceDeviation" />
    <asp:HiddenField runat="server" ID="hf_QtyDeviation" />
    <!-- Title Bar and Menu -->
    <table style="width: 100%; height: 20px; background-color: #4d4d4d;">
        <tr>
            <td>
                <asp:Image ID="img_Title" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_HD_WHITE" Text="Product"></asp:Label>
            </td>
            <td align="right">
                <dx:ASPxMenu runat="server" ID="menu_CmdBar" Font-Size="Small" Font-Bold="True" BackColor="Transparent" OnItemClick="menu_CmdBar_ItemClick">
                    <Border BorderStyle="None" />
                    <ItemStyle BackColor="Transparent" ImageSpacing="2px" VerticalAlign="Middle">
                        <Border BorderStyle="None" />
                        <HoverStyle BackColor="Transparent">
                            <Border BorderStyle="None" />
                        </HoverStyle>
                        <Paddings Padding="2px" />
                    </ItemStyle>
                    <Items>
                        <dx:MenuItem Name="SetActive" Text="">
                            <ItemStyle ForeColor="White" />
                        </dx:MenuItem>
                        <dx:MenuItem Name="Save" Text="">
                            <ItemStyle Width="49px">
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/save.png" Repeat="NoRepeat" VerticalPosition="center" />
                            </ItemStyle>
                        </dx:MenuItem>
                        <dx:MenuItem Name="Back" Text="">
                            <ItemStyle Width="42px">
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
    <br />
    <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <ContentTemplate>
            <!--------------------------------------------------- -->
            <asp:HiddenField ID="hf_ProductCode" runat="server" />
            <asp:HiddenField ID="hf_InventoryConvOrder" runat="server" />
            <asp:HiddenField ID="hf_RecipeConvInvent" runat="server" />
            <asp:HiddenField ID="hf_IsActive" runat="server" />
            <!-- Product summary -->
            <table style="width: 100%; height: 30px;">
                <tr>
                    <td>
                        <span style="font-size: medium; font-weight: bold;">#</span>
                        <asp:Label ID="lbl_ProductCode" runat="server" Font-Size="Medium" Font-Bold="true" />
                        &nbsp;
                        <asp:Label ID="lbl_Status" runat="server" CssClass="badge-inactive" />
                    </td>
                    <td align="right">
                        <asp:Label ID="lbl_LastCost_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderLastCost %>"></asp:Label>
                        &nbsp;
                        <asp:Label ID="lbl_LastCost" runat="server" Font-Size="Small" Font-Bold="true" />
                    </td>
                </tr>
            </table>
            <!-- Product detail -->
            <table id="table_product" class="w-100" style="vertical-align: top;" cellspacing="10">
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td style="width: 13%">
                        <asp:Label ID="lbl_Cateogry_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:Option.Inventory.Product, lbl_Cateogry_Nm %>" />
                    </td>
                    <td style="width: 20%">
                        <asp:Label ID="lbl_Category" runat="server" Font-Size="Small" Font-Bold="true" />
                        <asp:DropDownList ID="ddl_Category" runat="server" Width="90%" Font-Size="Small" AutoPostBack="true" DataValueField="Code" DataTextField="Name" OnSelectedIndexChanged="ddl_Category_OnSelectedIndexChanged"
                            Visible="false">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="req_Category" ControlToValidate="ddl_Category" runat="server" ErrorMessage="* " ForeColor="Red" Font-Size="Small"></asp:RequiredFieldValidator>
                    </td>
                    <td style="width: 13%">
                        <asp:Label ID="lbl_SubCateogry_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:Option.Inventory.Product, lbl_SubCateogry_Nm %>" />
                    </td>
                    <td style="width: 20%">
                        <asp:Label ID="lbl_SubCategory" runat="server" Font-Size="Small" Font-Bold="true" />
                        <asp:DropDownList ID="ddl_SubCategory" runat="server" Width="90%" Font-Size="Small" AutoPostBack="true" DataValueField="Code" DataTextField="Name" OnSelectedIndexChanged="ddl_SubCategory_OnSelectedIndexChanged"
                            Visible="false">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="req_SubCategory" ControlToValidate="ddl_SubCategory" runat="server" ErrorMessage="* " ForeColor="Red" Font-Size="Small"></asp:RequiredFieldValidator>
                    </td>
                    <td style="width: 13%">
                        <asp:Label ID="lbl_ItemGroup_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:Option.Inventory.Product, lbl_ItemGroup_Nm %>" />
                    </td>
                    <td style="width: 20%">
                        <asp:Label ID="lbl_ItemGroup" runat="server" Font-Size="Small" Font-Bold="true" />
                        <asp:DropDownList ID="ddl_ItemGroup" runat="server" Width="90%" Font-Size="Small" AutoPostBack="true" DataValueField="Code" DataTextField="Name" OnSelectedIndexChanged="ddl_ItemGroup_OnSelectedIndexChanged"
                            Visible="false">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="req_ItemGroup" ControlToValidate="ddl_ItemGroup" runat="server" ErrorMessage="*" ForeColor="Red" Font-Size="Small"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_ProductDesc1" runat="server" SkinID="LBL_HD" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderDescritpion1 %>"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_ProductDesc1" runat="server" Width="90%" Font-Size="Small" AutoPostBack="True" />
                        <%--<asp:RequiredFieldValidator ID="req_Description1" ControlToValidate="txt_ProductDesc1" runat="server" ErrorMessage="*" ForeColor="Red" Font-Size="Small"></asp:RequiredFieldValidator>--%>
                    </td>
                    <td>
                        <asp:Label ID="lbl_ProductDesc2" runat="server" SkinID="LBL_HD" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderDescription2 %>"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_ProductDesc2" runat="server" Width="90%" Font-Size="Small" AutoPostBack="True" />
                    </td>
                    <td>
                        <asp:Label ID="lbl_BarCode_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderBarCode %>"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_BarCode" runat="server" Width="90%" Font-Size="Small" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_TaxType_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderTAXType %>"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_TaxType" runat="server" Width="90%" Font-Size="Small" AutoPostBack="True" OnSelectedIndexChanged="ddl_TaxType_OnSelectedIndexChanged">
                            <asp:ListItem Text="None" Value="N"></asp:ListItem>
                            <asp:ListItem Text="Included" Value="I"></asp:ListItem>
                            <asp:ListItem Text="Add" Value="A"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbl_TaxRate_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderTaxRate %>"></asp:Label>
                    </td>
                    <td>
                        <dx:ASPxSpinEdit ID="se_TaxRate" runat="server" Width="90%" Font-Size="Small" AutoPostBack="True" SpinButtons-ShowIncrementButtons="False" NullText="0"
                            AllowNull="false" DecimalPlaces="2" DisplayFormatString="#,0.00" />
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
                        <asp:Label ID="lbl_StandardCost_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderStandardCost %>"></asp:Label>
                    </td>
                    <td>
                        <dx:ASPxSpinEdit ID="se_StandardCost" runat="server" Width="90%" Font-Size="Small" AutoPostBack="True" SpinButtons-ShowIncrementButtons="False" NullText="0"
                            DisplayFormatString="#,0.####" />
                    </td>
                    <td>
                        <asp:Label ID="lbl_QuantityDeviation_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderQuantityDeviation %>"></asp:Label>
                    </td>
                    <td>
                        <dx:ASPxSpinEdit ID="se_QtyDev" runat="server"  Width="90%" Font-Size="Small" AutoPostBack="True" SpinButtons-ShowIncrementButtons="False" NullText="0"
                            DisplayFormatString="#,0.###" />
                    </td>
                    <td>
                        <asp:Label ID="lbl_PriceDeviation_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderPriceDeviation %>"></asp:Label>
                    </td>
                    <td>
                        <dx:ASPxSpinEdit ID="se_PriceDev" runat="server" Width="90%" Font-Size="Small" AutoPostBack="True" SpinButtons-ShowIncrementButtons="False" NullText="0"
                            DisplayFormatString="#,0.####" />
                    </td>
                </tr>
            </table>
            <br />
            <!-- Default Units -->
            <div class="d-flex flex-column border mb-3 p-3" style="width: 98%;">
                <asp:Label ID="Label1" runat="server" Font-Size="Small" Font-Bold="true" Text="Default Unit" />
                <div class="d-flex d-flex-wrap mt-3">
                    <div class="d-flex p-3 ">
                        <asp:Label ID="lbl_InventoryUnit_Nm" runat="server" Width="100" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderInventoryUnit %>" SkinID="LBL_HD"></asp:Label>
                        <asp:TextBox ID="txt_InventoryUnit" runat="server" SkinID="TXT_V1" Width="110" ReadOnly="true" />
                        <dx:ASPxComboBox ID="ddl_InventoryUnit" runat="server" Width="120" ValueType="System.String" ValueField="Code" TextFormatString="{0}" EnableCallbackMode="true"
                            CallbackPageSize="25" EnableIncrementalFiltering="True" IncrementalFilteringMode="Contains" OnLoad="ddl_InventoryUnit_Load">
                            <Columns>
                                <dx:ListBoxColumn FieldName="Code" />
                                <dx:ListBoxColumn FieldName="Name" />
                            </Columns>
                        </dx:ASPxComboBox>
                    </div>
                    <div class="d-flex p-3 ">
                        <asp:Label ID="lbl_OrderUnit_Nm" runat="server" Width="100" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderOrderUnit %>" SkinID="LBL_HD"></asp:Label>
                        <div class="flex-column">
                            <asp:TextBox ID="txt_OrderUnit" runat="server" SkinID="TXT_V1" Width="110" ReadOnly="true"></asp:TextBox>
                            <asp:Label ID="lbl_OrderUnitDesc" runat="server" CssClass="ms-3" Font-Size="Smaller" Font-Italic="true" />
                        </div>
                    </div>
                    <div class="d-flex p-3 ">
                        <asp:Label ID="lbl_RecipeUnit_Nm" runat="server" Width="100" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderRecipeUnit %>" SkinID="LBL_HD"></asp:Label>
                        <div class="flex-column">
                            <asp:TextBox ID="txt_RecipeUnit" runat="server" SkinID="TXT_V1" Width="110" ReadOnly="true"></asp:TextBox>
                            <asp:Label ID="lbl_RecipeUnitDesc" runat="server" CssClass="ms-3" Font-Size="Smaller" />
                        </div>
                    </div>
                    <div class="d-flex p-3 ">
                        <asp:CheckBox ID="chk_IsRecipe" runat="server" Width="110" Font-Size="Small" AutoPostBack="true" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderMemberInRecipe %>" />
                    </div>
                    <div class="d-flex p-3 ">
                        <asp:CheckBox ID="chk_SaleItem" runat="server" Width="110" Font-Size="Small" AutoPostBack="true" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderSaleItem %>" />
                    </div>
                </div>
            </div>
            <!-- Order Unit && Recipe Unit -->
            <div class="d-flex d-flex-wrap">
                <!-- Order Unit -->
                <div class="border p-3 me-3">
                    <asp:Label ID="Label2" runat="server" Font-Size="Small" Font-Bold="true" Text="Order Unit" />
                    <asp:GridView ID="gv_OrderUnit" runat="server" Width="460" CssClass="gv" SkinID="GRD_V1" ShowHeaderWhenEmpty="true" OnRowDataBound="gv_OrderUnit_RowDataBound">
                        <HeaderStyle HorizontalAlign="Left" />
                        <Columns>
                            <asp:TemplateField HeaderText="Code">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_UnitCode" runat="server" Text='<%# Eval("Code") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rate">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="lbl_UnitRate" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="Rate">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="lbl_InvUnit" runat="server" Font-Size="Smaller" Font-Italic="true" />
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                                <HeaderTemplate>
                                    <asp:Button ID="btn_NewOrderUnit" runat="server" Text="New" OnClick="btn_NewOrderUnit_Click" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Button ID="btn_DeleteOrderUnit" runat="server" Text="Delete" OnClick="btn_DeleteOrderUnit_Click" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btn_EditOrderUnit" runat="server" Text="Edit" OnClick="btn_EditOrderUnit_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <!-- Recipe Unit -->
                <div class="border p-3">
                    <asp:Label ID="Label3" runat="server" Font-Size="Small" Font-Bold="true" Text="Recipe Unit" />
                    <asp:GridView ID="gv_RecipeUnit" runat="server" Width="460" CssClass="gv" SkinID="GRD_V1" ShowHeaderWhenEmpty="true" OnRowDataBound="gv_RecipeUnit_RowDataBound">
                        <HeaderStyle HorizontalAlign="Left" />
                        <Columns>
                            <asp:TemplateField HeaderText="Code">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_UnitCode" runat="server" Text='<%# Eval("Code") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rate">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="lbl_UnitRate" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                                <HeaderTemplate>
                                    <asp:Button ID="btn_NewRecipeUnit" runat="server" Text="New" OnClick="btn_NewRecipeUnit_Click" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Button ID="btn_DeleteRecipeUnit" runat="server" Text="Delete" OnClick="btn_DeleteRecipeUnit_Click" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btn_EditRecipeUnit" runat="server" Text="Edit" OnClick="btn_EditRecipeUnit_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <!-- Popup -->
            <dx:ASPxPopupControl ID="pop_Alert" ClientInstanceName="pop_Alert" runat="server" Width="360" Modal="true" ShowCloseButton="true" CloseAction="CloseButton"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="true">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl_Alert" runat="server">
                        <div class="w-100" style="text-align: center">
                            <asp:Label ID="lbl_Alert" runat="server" Width="100%" />
                        </div>
                        <br />
                        <div style="text-align: center;">
                            <button style="width: 60px;" onclick="pop_Alert.Hide();">
                                Ok</button>
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_Document" ClientInstanceName="pop_Document" runat="server" Width="480" Modal="true" ShowCloseButton="true" CloseAction="CloseButton"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="true">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl_Document" runat="server">
                        <div class="w-100 mb-3">
                            <asp:Label ID="lbl_Document" runat="server" />
                        </div>
                        <asp:GridView ID="gv_Document" runat="server" CssClass="w-100" AutoGenerateColumns="false" GridLines="None">
                            <HeaderStyle HorizontalAlign="Left" BackColor="LightPink" />
                            <AlternatingRowStyle BackColor="WhiteSmoke" />
                            <RowStyle Wrap="true" />
                            <Columns>
                                <asp:BoundField HeaderText="Document#" DataField="DocNo" />
                                <asp:BoundField HeaderText="Date" DataField="DocDate" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField HeaderText="Description" DataField="Description" />
                                <asp:BoundField HeaderText="Status" DataField="DocStatus" />
                                <asp:BoundField HeaderText="Type" DataField="DocType" />
                            </Columns>
                        </asp:GridView>
                        <br />
                        <hr />
                        <div class="w-100 mb-3">
                            Quantity remain
                        </div>
                        <asp:GridView ID="gv_LocatonOnhand" runat="server" CssClass="w-100" AutoGenerateColumns="false" GridLines="None">
                            <HeaderStyle HorizontalAlign="Left" BackColor="LightPink" />
                            <AlternatingRowStyle BackColor="WhiteSmoke" />
                            <RowStyle Wrap="true" />
                            <Columns>
                                <asp:BoundField HeaderText="Location" DataField="LocationCode" />
                                <asp:BoundField HeaderText="Name" DataField="LocationName" />
                                <asp:TemplateField>
                                    <ItemStyle HorizontalAlign="Right" />
                                    <ItemTemplate>
                                        <%# ((decimal)Eval("Qty")).ToString("N") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:BoundField HeaderText="Onhand" DataField="Qty" />--%>
                            </Columns>
                        </asp:GridView>
                        <br />
                        <hr />
                        <br />
                        <div style="text-align: center;">
                            <button style="width: 60px;" onclick="pop_Document.Hide();">
                                Ok</button>
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_OrderUnit" ClientInstanceName="pop_OrderUnit" runat="server" Width="240" Modal="true" ShowCloseButton="true" CloseAction="CloseButton"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="true">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl_OrderUnit" runat="server">
                        <div class="d-flex flex-column">
                            <asp:Label ID="lbl_OrderUnitCode_Nm" runat="server" Font-Bold="true" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderOrderUnit %>" />
                            <asp:TextBox ID="txt_OrderUnitCode" runat="server" ReadOnly="true" />
                            <asp:DropDownList ID="ddl_OrderUnitCode" runat="server" Font-Size="Small" SkinID="DDL_V1">
                            </asp:DropDownList>
                            <br />
                            <asp:Label ID="lbl_OrderUnitRate_Nm" runat="server" Font-Bold="true" Text="Rate" />
                            <dx:ASPxSpinEdit ID="se_OrderUnitRate" ClientInstanceName="se_OrderUnitRate" runat="server" Height="18" MinValue="0" MaxValue="100000" />
                            <div class="mt-3">
                                <asp:CheckBox ID="chk_OrderUnitIsDefault" runat="server" Text="Set as default" />
                            </div>
                        </div>
                        <br />
                        <small>The unit rate is the number of quantity of inventory units converted to be one order unit.</small>
                        <hr />
                        <br />
                        <div style="width: 100%; text-align: center;">
                            <asp:Button ID="btn_SaveOrderUnit" runat="server" CssClass="me-3" Text="Save" OnClick="btn_SaveOrderUnit_Click" />
                            <asp:Button ID="btn_CancelOrderUnit" runat="server" CssClass="ms-3" Text="Cancel" OnClientClick="pop_OrderUnit.Hide();" />
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_RecipeUnit" ClientInstanceName="pop_RecipeUnit" runat="server" Width="240" Modal="true" ShowCloseButton="true" CloseAction="CloseButton"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="true">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <div class="d-flex flex-column">
                            <asp:Label ID="lbl_RecipeUnitCode" runat="server" Font-Bold="true" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderOrderUnit %>" />
                            <asp:TextBox ID="txt_RecipeUnitCode" runat="server" ReadOnly="true" />
                            <asp:DropDownList ID="ddl_RecipeUnitCode" runat="server" Font-Size="Small" SkinID="DDL_V1">
                            </asp:DropDownList>
                            <br />
                            <asp:Label ID="lbl_RecipeUnitRate" runat="server" Font-Bold="true" Text="Rate" />
                            <dx:ASPxSpinEdit ID="se_RecipeUnitRate" runat="server" Height="18" MinValue="0" MaxValue="100000" />
                            <div class="mt-3">
                                <asp:CheckBox ID="chk_RecipeUnitIsDefault" runat="server" Text="Set as default" />
                            </div>
                        </div>
                        <br />
                        <small>The unit rate is the number of quantity of recipe units converted to be one inventory unit.</small>
                        <hr />
                        <br />
                        <div style="width: 100%; text-align: center;">
                            <asp:Button ID="btn_SaveRecipeUnit" runat="server" CssClass="me-3" Text="Save" OnClick="btn_SaveRecipeUnit_Click" />
                            <asp:Button ID="btn_CancelRecipeUnit" runat="server" CssClass="ms-3" Text="Cancel" OnClientClick="pop_RecipeUnit.Hide();" />
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ddl_Category" />
            <asp:PostBackTrigger ControlID="ddl_SubCategory" />
            <asp:PostBackTrigger ControlID="ddl_ItemGroup" />
            <%--<asp:AsyncPostBackTrigger ControlID="imb_CreateOrder" EventName="Click" />--%>
            <%--<asp:AsyncPostBackTrigger ControlID="imb_DeleteOrder" EventName="Click" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
