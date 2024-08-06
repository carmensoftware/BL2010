<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="ProdEdit3.aspx.cs" Inherits="BlueLedger.PL.Option.Inventory.ProdEdit3" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>

<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <%--<script type="text/javascript" language="javascript">

        function DropDownHandler(s, e) {
            SynchronizeFocusedRow();
        }

        function RowClickHandler(s, e) {
            ddl_InventoryUnit.SetKeyValue(grd_InventoryUnit.cpKeyValues[e.visibleIndex]);
            ddl_InventoryUnit.SetText(grd_InventoryUnit.cpEmployeeNames[e.visibleIndex]);
            ddl_InventoryUnit.HideDropDown();
        }

        function SynchronizeFocusedRow() {
            var keyValue = ddl_InventoryUnit.GetKeyValue();
            var index = -1;
            if (keyValue != null)
                index = ASPxClientUtils.ArrayIndexOf(grd_InventoryUnit.cpKeyValues, keyValue);
            grd_InventoryUnit.SetFocusedRowIndex(index);
            grd_InventoryUnit.MakeRowVisible(index);
        }

        function DropDownHandler_OrderUnit(s, e) {
            SynchronizeFocusedRow_OrderUnit();
        }

        function RowClickHandler_OrderUnit(s, e) {
            ddl_OrderUnit.SetKeyValue(grd_OrderUnit.cpKeyValues[e.visibleIndex]);
            ddl_OrderUnit.SetText(grd_OrderUnit.cpEmployeeNames[e.visibleIndex]);
            ddl_OrderUnit.HideDropDown();
        }

        function SynchronizeFocusedRow_OrderUnit() {
            var keyValue = ddl_OrderUnit.GetKeyValue();
            var index = -1;
            if (keyValue != null)
                index = ASPxClientUtils.ArrayIndexOf(grd_OrderUnit.cpKeyValues, keyValue);
            grd_OrderUnit.SetFocusedRowIndex(index);
            grd_OrderUnit.MakeRowVisible(index);
        }

        function DropDownHandler_RecipeUnit(s, e) {
            SynchronizeFocusedRow_RecipeUnit();
        }

        function RowClickHandler_RecipeUnit(s, e) {
            ddl_RecipeUnit.SetKeyValue(grd_RecipeUnit.cpKeyValues[e.visibleIndex]);
            ddl_RecipeUnit.SetText(grd_RecipeUnit.cpEmployeeNames[e.visibleIndex]);
            ddl_RecipeUnit.HideDropDown();

        }

        function SynchronizeFocusedRow_RecipeUnit() {
            var keyValue = ddl_RecipeUnit.GetKeyValue();
            var index = -1;
            if (keyValue != null)
                index = ASPxClientUtils.ArrayIndexOf(grd_RecipeUnit.cpKeyValues, keyValue);
            grd_RecipeUnit.SetFocusedRowIndex(index);
            grd_RecipeUnit.MakeRowVisible(index);
        }

        //KeyPress 0 To 9
        function PreTextChange() {
            var keyCode = event.keyCode;
            return (keyCode >= 48 && keyCode <= 57);
        }        

    </script>--%>
    <div align="left">
     <asp:UpdatePanel ID="UdPnlProductCre" runat="server">
                <ContentTemplate>
                    <div>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                        <tr>
                            <td align="left">
                                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                    <tr>
                                        <td align="left">
                                            <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                <tr style="background-color: #4d4d4d; height: 17px; padding-left: 10px">
                                                    <td style="padding-left: 10px; width: 10px">
                                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="Label7" runat="server" Text="Product" SkinID="LBL_HD_WHITE"></asp:Label>
                                                    </td>
                                                    <td align="right" style="padding-right: 10px;">
                                                        <dx:ASPxMenu runat="server" ID="menu_CmdBar" Font-Bold="True" BackColor="Transparent"
                                                            Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle" Height="16px"
                                                            OnItemClick="menu_CmdBar_ItemClick">
                                                            <ItemStyle BackColor="Transparent">
                                                                <HoverStyle BackColor="Transparent">
                                                                    <Border BorderStyle="None" />
                                                                </HoverStyle>
                                                                <Paddings Padding="2px" />
                                                                <Border BorderStyle="None" />
                                                            </ItemStyle>
                                                            <Items>
                                                                <dx:MenuItem Name="Save" Text="">
                                                                    <ItemStyle Height="16px" Width="49px">
                                                                        <HoverStyle>
                                                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png"
                                                                                Repeat="NoRepeat" VerticalPosition="center" />
                                                                        </HoverStyle>
                                                                        <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/save.png"
                                                                            Repeat="NoRepeat" VerticalPosition="center" />
                                                                    </ItemStyle>
                                                                </dx:MenuItem>
                                                                <dx:MenuItem Name="Back" Text="">
                                                                    <ItemStyle Height="16px" Width="42px">
                                                                        <HoverStyle>
                                                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png"
                                                                                Repeat="NoRepeat" VerticalPosition="center" />
                                                                        </HoverStyle>
                                                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat"
                                                                            HorizontalPosition="center" VerticalPosition="center" />
                                                                    </ItemStyle>
                                                                </dx:MenuItem>
                                                            </Items>
                                                            <Paddings Padding="0px" />
                                                            <SeparatorPaddings Padding="0px" />
                                                            <SubMenuStyle HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" Font-Size="9pt"
                                                                ForeColor="#4D4D4D" />
                                                            <Border BorderStyle="None"></Border>
                                                        </dx:ASPxMenu>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                    <tr style="height: 40px;">
                                        <td style="width: 13%">
                                            <asp:Label ID="lbl_Cateogry_Nm" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_Cateogry_Nm %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td style="width: 20%">
                                            <asp:DropDownList ID="ddl_Category" OnSelectedIndexChanged="ddl_Category_OnSelectedIndexChanged"
                                                AutoPostBack="true" runat="server" Width="120px">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="req_Category" ControlToValidate="ddl_Category" runat="server"
                                                ErrorMessage="* " ForeColor="Red" Font-Size="Small"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 13%">
                                            <asp:Label ID="lbl_SubCateogry_Nm" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_SubCateogry_Nm %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td style="width: 20%">
                                            <asp:DropDownList ID="ddl_SubCategory" OnSelectedIndexChanged="ddl_SubCategory_OnSelectedIndexChanged"
                                                AutoPostBack="true" runat="server" Width="120px">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="req_SubCategory" ControlToValidate="ddl_SubCategory"
                                                runat="server" ErrorMessage="* " ForeColor="Red" Font-Size="Small"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 13%">
                                            <asp:Label ID="lbl_ItemGroup_Nm" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_ItemGroup_Nm %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td style="width: 20%">
                                            <asp:DropDownList ID="ddl_ItemGroup" OnSelectedIndexChanged="ddl_ItemGroup_OnSelectedIndexChanged"
                                                AutoPostBack="true" runat="server" Width="120px">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="req_ItemGroup" ControlToValidate="ddl_ItemGroup"
                                                runat="server" ErrorMessage="*" ForeColor="Red" Font-Size="Small"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr style="height: 40px;">
                                        <td>
                                            <asp:Label ID="lbl_HeaderCode" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderCode %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_Code" runat="server" Width="120px" Enabled="false"> </asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_HeaderDescritpion1" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderDescritpion1 %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_Description1" runat="server" Width="120px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="req_Description1" ControlToValidate="txt_Description1"
                                                runat="server" ErrorMessage="*" ForeColor="Red" Font-Size="Small"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_HeaderDescription2" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderDescription2 %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_Descritpion2" runat="server" Width="120px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="height: 40px;">
                                        <td>
                                            <asp:Label ID="lbl_HeaderMemberInRecipe" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderMemberInRecipe %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chk_IsRecipe" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_RecipeConvInventory_Hr" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_RecipeConvInventory_Hr %>"
                                                SkinID="LBL_HD" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr style="height: 40px;">
                                        <td>
                                            <asp:Label ID="lbl_HeaderTAXType" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderTAXType %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td style="width: 100px">
                                            <asp:DropDownList ID="ddl_TAXType" runat="server" Width="120px" AutoPostBack="True"
                                                OnSelectedIndexChanged="ddl_TAXType_OnSelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_HeaderTaxRate" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderTaxRate %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txt_TaxRate" runat="server" Width="120px" SkinID="TXT_NUM_V1"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_HeaderAccountCode" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderAccountCode %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_TaxAccCode" runat="server" Width="120px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="height: 40px;">
                                        <td>
                                            <asp:Label ID="Label39" runat="server" SkinID="LBL_HD" Text="Product Type"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_ProdType" runat="server" Width="120px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_HeaderQuantityDeviation" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderQuantityDeviation %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_QuantityDeviation" runat="server" SkinID="TXT_NUM_V1"></asp:TextBox>
                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                                TargetControlID="txt_QuantityDeviation" ValidChars="0123456789.">
                                            </ajaxToolkit:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_HeaderPriceDeviation" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderPriceDeviation %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_PriceDeviation" runat="server" SkinID="TXT_NUM_V1"></asp:TextBox>
                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender" runat="server"
                                                TargetControlID="txt_PriceDeviation" ValidChars="0123456789.">
                                            </ajaxToolkit:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr style="height: 40px;">
                                        <td>
                                            <asp:Label ID="lbl_HeaderStatus" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderStatus %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chk_Status" runat="server" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_HeaderSaleItem" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderSaleItem %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chk_SaleItem" runat="server" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_HeaderPriceDeviation0" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderPriceDeviation0 %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chk_ReqHQAppr" runat="server" />
                                        </td>
                                    </tr>
                                    <tr style="height: 40px;">
                                        <td>
                                            <asp:Label ID="lbl_HeaderBarCode" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderBarCode %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_BarCode" runat="server" Width="120px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_HeaderStandardCost" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderStandardCost %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_StandardCost" runat="server" SkinID="TXT_NUM_V1"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_HeaderLastCost" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderLastCost %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_LastCost" runat="server" SkinID="TXT_NUM_V1"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="height: 40px;">
                                        <td>
                                            <asp:Label ID="lbl_HeaderInventoryUnit" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderInventoryUnit %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                <tr>
                                                    <td align="left" width="100px">
                                                        <asp:DropDownList ID="ddl_InventoryUnit" runat="server" SkinID="DDL_V1" Width="120px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td width="5px">
                                                        <asp:RequiredFieldValidator ID="req_InventoryUnit" ControlToValidate="ddl_InventoryUnit"
                                                            runat="server" ErrorMessage="*" ForeColor="Red" Font-Size="Small"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_HeaderOrderUnit" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderOrderUnit %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                <tr>
                                                    <td align="left" width="100px">
                                                        <asp:TextBox ID="txt_OrderUnit" runat="server" SkinID="TXT_NUM_V1" Width="90px" Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td width="5px">
                                                        <asp:TextBox ID="txt_InvenConvOrder" runat="server" SkinID="TXT_NUM_V1" Width="70px"
                                                            Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_HeaderRecipeUnit" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderRecipeUnit %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                <tr>
                                                    <td align="left" width="100px">
                                                        <asp:TextBox ID="txt_RecipeUnit" runat="server" SkinID="TXT_NUM_V1" Width="90px"
                                                            Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td width="5px">
                                                        <asp:TextBox ID="txt_RecipeConverseRate" runat="server" SkinID="TXT_NUM_V1" Width="70px"
                                                            Enabled="false"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                   
        </table>
         </div>
                    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UdPgProCre"
                        PopupControlID="UdPgProCre" BackgroundCssClass="POPUP_BG" RepositionMode="RepositionOnWindowResizeAndScroll">
                    </ajaxToolkit:ModalPopupExtender>
                    <asp:UpdateProgress ID="UdPgProCre" runat="server" AssociatedUpdatePanelID="UdPnlProductCre">
                        <ProgressTemplate>
                            <div class="fix-layout" style="border-style: solid; border-width: 1px; border-color: #0071BD; background-color: #FFFFFF;
                                width: 120px; height: 60px">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 120px; height: 60px">
                                    <tr>
                                        <td align="center">
                                            <asp:Image ID="img_Loading1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/ajax-loader.gif"
                                                EnableViewState="False" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbl_Loading1" runat="server" Font-Bold="true" Text="Loading..." EnableViewState="False"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </ContentTemplate>
            </asp:UpdatePanel>
        <asp:HiddenField ID="hf_ConnStr" runat="server" />
    </div>
</asp:Content>
