<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="Prod.aspx.cs" Inherits="BlueLedger.PL.Option.Inventory.Prod" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Src="../../UserControl/IN/PROD/ProdUnit.ascx" TagName="ProdUnit" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <script type="text/javascript" language="javascript">
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
    </script>
    <div align="left">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
            <tr>
                <td align="left">
                    <table border="0" cellpadding="1" cellspacing="0" width="100%">
                        <tr style="background-color: #4d4d4d; height: 17px; padding-left: 10px">
                            <td style="padding-left: 10px; width: 10px">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                            </td>
                            <td align="left">
                                <asp:Label ID="lbl_TitleHD" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_TitleHD %>"
                                    SkinID="LBL_HD_WHITE"></asp:Label>
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
                                        <dx:MenuItem Name="Create" Text="">
                                            <ItemStyle Height="16px" Width="49px">
                                                <HoverStyle>
                                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png"
                                                        Repeat="NoRepeat" VerticalPosition="center" />
                                                </HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/create.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </ItemStyle>
                                        </dx:MenuItem>
                                        <dx:MenuItem Name="Edit" Text="">
                                            <ItemStyle Height="16px" Width="38px">
                                                <HoverStyle>
                                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-edit.png"
                                                        Repeat="NoRepeat" VerticalPosition="center" />
                                                </HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/edit.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </ItemStyle>
                                        </dx:MenuItem>
                                        <dx:MenuItem Name="Delete" Text="">
                                            <ItemStyle Height="16px" Width="41px">
                                                <HoverStyle>
                                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png"
                                                        Repeat="NoRepeat" VerticalPosition="center" />
                                                </HoverStyle>
                                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png" Repeat="NoRepeat"
                                                    HorizontalPosition="center" VerticalPosition="center" />
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
            <tr>
                <td align="center">
                    <table border="0" cellpadding="5" cellspacing="2" width="100%">
                        <tr>
                            <td align="right" style="width: 13%">
                                <asp:Label ID="lbl_Cateogry_Nm" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_Cateogry_Nm %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td align="left" style="width: 20%">
                                <asp:Label ID="lbl_Cateogry" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td align="right" style="width: 13%">
                                <asp:Label ID="lbl_SubCateogry_Nm" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_SubCateogry_Nm %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td align="left" style="width: 20%">
                                <asp:Label ID="lbl_SubCategory" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td align="right" style="width: 13%">
                                <asp:Label ID="lbl_ItemGroup_Nm" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_ItemGroup_Nm %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td align="left" style="width: 20%">
                                <asp:Label ID="lbl_ItemGroup" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 13%">
                                <asp:Label ID="lbl_HeaderCode" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderCode %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td align="left" style="width: 20%">
                                <asp:Label ID="lbl_Code" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td align="right" style="width: 13%">
                                <asp:Label ID="lbl_HeaderDescritpion1" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderDescritpion1 %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td align="left" style="width: 20%">
                                <asp:Label ID="lbl_Description1" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td align="right" style="width: 13%">
                                <asp:Label ID="lbl_HeaderDescription2" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderDescription2 %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td align="left" style="width: 20%">
                                <asp:Label ID="lbl_Description2" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                        </tr>
                        <%--2012/01/09 Receive issue that set visible = false at recipe item, recipe unit, conversion--%>
                        <%--<tr>
                            <td align="right" style="width: 13%">
                                <asp:Label ID="lbl_HeaderMemberInRecipe" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderMemberInRecipe %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td align="left" style="width: 20%">
                                <asp:Label ID="lbl_MemberInRecipe" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td align="right" style="width: 13%">
                                <asp:Label ID="lbl_HeaderRecipeUnit" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderRecipeUnit %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td align="left" style="width: 20%">
                                <asp:Label ID="lbl_RecipeUnit" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td align="right" style="width: 13%">
                                <asp:Label ID="lbl_RecipeConvInventory_Hr" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_RecipeConvInventory_Hr %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td align="left" style="width: 20%">
                                <asp:Label ID="lbl_RecipeConverseRate" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                        </tr>--%>
                        <tr>
                            <td align="right" style="width: 13%">
                                <asp:Label ID="lbl_HeaderTAXType" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderTAXType %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td align="left" style="width: 20%">
                                <asp:Label ID="lbl_TAXType" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td align="right" style="width: 13%">
                                <asp:Label ID="lbl_HeaderTaxRate" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderTaxRate %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td align="left" style="width: 20%">
                                <asp:Label ID="lbl_TaxRate" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td align="right" style="width: 13%">
                                <asp:Label ID="lbl_HeaderAccountCode" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderAccountCode %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td align="left" style="width: 20%">
                                <asp:Label ID="lbl_AccountCode" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 13%">
                                <asp:Label ID="lbl_ApprovalLevel_Nm" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_ApprovalLevel_Nm %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td align="left" style="width: 20%">
                                <asp:Label ID="lbl_ApprovalLevel" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td align="right" style="width: 13%">
                                <asp:Label ID="lbl_HeaderQuantityDeviation" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderQuantityDeviation %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td align="left" style="width: 20%">
                                <asp:Label ID="lbl_QuantityDeviation" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td align="right" style="width: 13%">
                                <asp:Label ID="lbl_HeaderPriceDeviation" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderPriceDeviation %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td align="left" style="width: 20%">
                                <asp:Label ID="lbl_PriceDeviation" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 13%">
                                <asp:Label ID="lbl_HeaderStatus" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderStatus %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td align="left" style="width: 20%">
                                <asp:Label ID="lbl_Status" runat="server" Text="Status" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td align="right" style="width: 13%">
                                <asp:Label ID="lbl_HeaderSaleItem" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderSaleItem %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td align="left" style="width: 20%">
                                <asp:Label ID="lbl_SaleItem" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td align="right" style="width: 13%">
                                <asp:Label ID="lbl_HeaderPriceDeviation0" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderPriceDeviation0 %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td align="left" style="width: 20%">
                                <asp:Label ID="lbl_HeaderPriceDeviationStatus" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 13%">
                                <asp:Label ID="lbl_HeaderBarCode" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderBarCode %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td align="left" style="width: 20%">
                                <asp:Label ID="lbl_BarCode" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td align="right" style="width: 13%">
                                <asp:Label ID="lbl_HeaderStandardCost" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderStandardCost %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td align="left" style="width: 20%">
                                <asp:Label ID="lbl_StandardCost" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td align="right" style="width: 13%">
                                <asp:Label ID="lbl_HeaderLastCost" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderLastCost %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td align="left" style="width: 20%">
                                <asp:Label ID="lbl_LastCost" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 13%">
                                <asp:Label ID="lbl_HeaderInventoryUnit" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderInventoryUnit %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td align="left" style="width: 20%">
                                <asp:Label ID="lbl_InventoryUnit" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td align="right" style="width: 13%">
                                <asp:Label ID="lbl_HeaderOrderUnit" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderOrderUnit %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td align="left" style="width: 20%">
                                <asp:Label ID="lbl_OrderUnit" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td align="right" style="width: 13%">
                                <asp:Label ID="lbl_InvenConvOrder_Hr" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_InvenConvOrder_Hr %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td align="left" style="width: 20%">
                                <asp:Label ID="lbl_OrderConverseRate" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <%-- New version 2011-09-17
                        <table border="0" cellpadding="2" cellspacing="0" width="100%">
                        <tr align="left" style="height: 17px; vertical-align: top">
                            <td style="padding-left: 10px; width: 12%;">
                                <asp:Label ID="lbl_Cateogry_Nm" runat="server" Text="Category" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE" style="width: 15%;">
                                <asp:Label ID="lbl_Cateogry" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td class="TD_LINE" style="width: 10%;">
                                <asp:Label ID="lbl_BaseUnit_Nm" runat="server" Text="Base Unit" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE" style="width: 13%;">
                                <asp:Label ID="lbl_BaseUnit" runat="server" Text="Kg" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td class="TD_LINE" style="width: 12%;">
                                <asp:Label ID="lbl_HeaderLastCost" runat="server" Text="Last Cost" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE" style="width: 13%;">
                                <asp:Label ID="lbl_LastCost" runat="server" SkinID="LBL_NR_BLUE_NUM"></asp:Label>
                            </td>
                            <td class="TD_LINE" style="width: 10%;">
                                <asp:Label ID="lbl_HeaderStatus" runat="server" Text="Status" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE" style="width: 15%;">
                                <asp:Label ID="lbl_Status" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                        </tr>
                        <tr align="left" style="height: 17px; vertical-align: top">
                            <td style="padding-left: 10px">
                                <asp:Label ID="lbl_SubCateogry_Nm" runat="server" Text="Sub Category" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_SubCategory" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_HeaderOrderUnit" runat="server" Text="Order Unit" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_OrderUnit" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lbl_HeaderStandardCost" runat="server" Text="Standard Cost" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_StandardCost" runat="server" SkinID="LBL_NR_BLUE_NUM"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_ApprovalLevel_Nm" runat="server" Text="Approval Level" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_ApprovalLevel" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                        </tr>
                        <tr align="left" style="height: 17px; vertical-align: top">
                            <td style="padding-left: 10px" class="TD_LINE">
                                <asp:Label ID="lbl_ItemGroup_Nm" runat="server" Text="Item Group" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_ItemGroup" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_InvenConvOrder_Hr" runat="server" Text="Unit Conversion (I)" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE" align="right">
                                <asp:Label ID="lbl_OrderConverseRate" runat="server" SkinID="LBL_NR_BLUE_NUM"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="Label2" runat="server" Text="Average Cost" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE" align="right">
                                <asp:Label ID="lbl_AverageCost" runat="server" SkinID="LBL_NR_BLUE_NUM"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lbl_HeaderSaleItem" runat="server" Text="Sale Item" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_SaleItem" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                        </tr>
                        <tr align="left" style="height: 17px; vertical-align: top">
                            <td style="padding-left: 10px;" class="TD_LINE">
                                <asp:Label ID="lbl_HeaderCode" runat="server" Text="SKU" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_Code" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_HeaderInventoryUnit" runat="server" Text="Inventory Unit" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_InventoryUnit" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_HeaderPriceDeviation" runat="server" Text="Price Deviation(%)"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE" align="right">
                                <asp:Label ID="lbl_PriceDeviation" runat="server" SkinID="LBL_NR_BLUE_NUM"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_HeaderTAXType" runat="server" Text="TAX Type" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_TAXType" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                        </tr>
                        <tr align="left" style="height: 17px; vertical-align: top">
                            <td style="padding-left: 10px" class="TD_LINE">
                                <asp:Label ID="lbl_HeaderDescritpion1" runat="server" Text="English Description"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_Description1" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_RecipeConvInventory_Hr" runat="server" Text="Unit Conversion(R)"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE" align="right">
                                <asp:Label ID="lbl_RecipeConverseRate" runat="server" SkinID="LBL_NR_BLUE_NUM"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_HeaderQuantityDeviation" runat="server" Text="Quantity Deviation(%)"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE" align="right">
                                <asp:Label ID="lbl_QuantityDeviation" runat="server" SkinID="LBL_NR_BLUE_NUM"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_HeaderAccountCode" runat="server" Text="Tax Account" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE" align="left">
                                <asp:Label ID="lbl_AccountCode" runat="server" SkinID="LBL_NR_BLUE_NUM"></asp:Label>
                            </td>
                        </tr>
                        <tr align="left" style="height: 17px; vertical-align: top">
                            <td style="padding-left: 10px" class="TD_LINE">
                                <asp:Label ID="lbl_HeaderDescription2" runat="server" Text="Local Description" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_Description2" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_HeaderRecipeUnit" runat="server" Text="Recipe Unit" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_RecipeUnit" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_HeaderPriceDeviation0" runat="server" Text="Central Purchase"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_HeaderPriceDeviationStatus" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_HeaderTaxRate" runat="server" Text="Tax Rate(%)" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE" align="right">
                                <asp:Label ID="lbl_TaxRate" runat="server" SkinID="LBL_NR_BLUE_NUM"></asp:Label>
                            </td>
                        </tr>
                        <tr align="left" style="height: 17px; vertical-align: top">
                            <td style="padding-left: 10px" class="TD_LINE">
                                <asp:Label ID="lbl_HeaderBarCode" runat="server" Text="Bar Code" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_BarCode" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="lbl_HeaderMemberInRecipe" runat="server" Text="Recipe Item" SkinID="LBL_HD"
                                    Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lbl_MemberInRecipe" runat="server" SkinID="LBL_NR_BLUE" Visible="false"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>--%>
                </td>
            </tr>
        </table>
        <uc1:ProdUnit ID="ProdUnit" runat="server" />
        <br />
        <table border="0" cellpadding="5" cellspacing="0" width="100%">
            <tr>
                <td>
                    <table border="0" cellpadding="1" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Button ID="btn_AssignStore" runat="server" Text="<%$ Resources:Option.Inventory.Product, btn_AssignStore %>"
                                    SkinID="BTN_V1" OnClick="btn_AssignStore_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btn_AssignBu" runat="server" Text="<%$ Resources:Option.Inventory.Product, btn_AssignBu %>"
                                    OnClick="btn_AssignBu_Click" SkinID="BTN_V1" />
                            </td>
                            <td>
                                <asp:Button ID="btn_AssignVendor" runat="server" Text="<%$ Resources:Option.Inventory.Product, btn_AssignVendor %>"
                                    OnClick="btn_AssignVendor_Click" SkinID="BTN_V1" />
                            </td>
                            <td align="right" style="width: 60%">
                                <asp:Label ID="lbl_MsgNoAssign" runat="server" Style="font-weight: bold;" SkinID="LBL_NR_RED"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div>
        <%--<asp:Panel Style="cursor: move; overflow: auto; display: none; height: 300px;" ID="pnlLookupStore"
            runat="server" Width="500px" Visible="true">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td align="left">
                        <table border="0" cellpadding="1" cellspacing="0" width="100%">
                            <tr style="height: 40px">
                                <td style="background-color: #DEDEDE">
                                    <asp:Label ID="Label4" runat="server" Text="Assign Store" SkinID="LBL_HD"></asp:Label>
                                </td>
                                <td align="right" style="background-color: #DEDEDE">                                    
                                    <dx:ASPxMenu runat="server" ID="menuStore" Font-Bold="True" BackColor="Transparent"
                                        Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle" Height="16px"
                                        OnItemClick="menuStore_ItemClick">
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
                                                    <HoverStyle BackColor="White">
                                                        <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png"
                                                            Repeat="NoRepeat" VerticalPosition="center" />
                                                    </HoverStyle>
                                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/save.png"
                                                        Repeat="NoRepeat" VerticalPosition="center" />
                                                </ItemStyle>
                                            </dx:MenuItem>
                                            <dx:MenuItem Name="Close" Text="">
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
                        <asp:GridView ID="grd_Store" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
                            SkinID="GRD_V1" Width="100%" OnRowDataBound="grd_Store_RowDataBound" DataKeyNames="LocationCode"
                            OnLoad="grd_Store_OnLoad">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chk_SelAll" runat="server" onclick="Check(this)" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk_Item" runat="server" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="5%" />
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Store">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_LocationCode" runat="server" SkinID="LBL_NR"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="20%" />
                                    <ItemStyle Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_LocationName" runat="server" SkinID="LBL_NR"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="75%" />
                                    <ItemStyle Width="75%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <Poup:ModalPopupExtender ID="Mdlp_StorePopup" runat="server" TargetControlID="btn_AssignStore"
            PopupControlID="pnlLookupStore" BackgroundCssClass="POPUP_DISABLED" DropShadow="false">
        </Poup:ModalPopupExtender>
        <Poup:DragPanelExtender ID="dpe_StorePopup" runat="server" TargetControlID="pnlLookupStore">
        </Poup:DragPanelExtender>--%>
    </div>
    <div>
        <dx:ASPxPopupControl ID="pop_ConfrimDelete" runat="server" Width="300px" CloseAction="CloseButton"
            HeaderText="<%$ Resources:Option.Inventory.Product, btn_Confrim %>" Modal="True"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="pop_ConfrimDelete"
            AllowDragging="True" EnableAnimation="False" EnableViewState="false" ShowFooter="false">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" Width="300px">
                    <table border="0" cellpadding="5" cellspacing="2" width="100%">
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Label ID="lbl_ConfirmMessage" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_ConfirmMessage %>"
                                    SkinID="LBL_NR"></asp:Label>
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr align="center">
                            <td align="right">
                                <asp:Button ID="btn_Yes" runat="server" Text="<%$ Resources:Option.Inventory.Product, btn_Confrim %>"
                                    OnClick="btn_Yes_Click" Width="50px" SkinID="BTN_V1" />
                            </td>
                            <td align="left">
                                <asp:Button ID="btn_No" runat="server" Text="<%$ Resources:Option.Inventory.Product, btn_Cancel %>"
                                    OnClick="btn_No_Click" Width="50px" SkinID="BTN_V1" />
                            </td>
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle HorizontalAlign="Center" />
            <FooterStyle HorizontalAlign="Center" />
        </dx:ASPxPopupControl>
        <dx:ASPxPopupControl ID="pop_WarningDelete" runat="server" Width="300px" CloseAction="CloseButton"
            HeaderText="<%$ Resources:Option.Inventory.Product, MsgHD %>" Modal="True" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                    <table border="0" cellpadding="5" cellspacing="0" width="100%">
                        <tr>
                            <td align="center" height="50px">
                                <asp:Label ID="MsgWarning" runat="server" Text="<%$ Resources:Option.Inventory.Product, MsgWarning %>"
                                    SkinID="LBL_NR"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btn_Ok" runat="server" OnClick="btn_Ok_Click" Text="<%$ Resources:Option.Inventory.Product, btn_SuccessOk %>"
                                    Width="50px" SkinID="BTN_V1" />
                            </td>
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle HorizontalAlign="Left" />
        </dx:ASPxPopupControl>
        <dx:ASPxPopupControl ID="pop_WarningReplace" runat="server" Width="300px" CloseAction="CloseButton"
            HeaderText="<%$ Resources:Option.Inventory.Product, btn_Confrim %>" Modal="True"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="pop_WarningReplace"
            AllowDragging="True" EnableAnimation="False" EnableViewState="false" ShowFooter="false">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl7" runat="server" Width="300px">
                    <table border="0" cellpadding="5" cellspacing="2" width="100%">
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Label ID="lbl_Warning" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_WarningMsg %>"
                                    SkinID="LBL_NR"></asp:Label>
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr align="center">
                            <td align="right">
                                <asp:Button ID="btn_WarningYes" runat="server" Text="<%$ Resources:Option.Inventory.Product, btn_Confrim %>"
                                    Width="70px" SkinID="BTN_V1" OnClick="btn_WarningYes_Click" />
                            </td>
                            <td align="left">
                                <asp:Button ID="btn_WarningNo" runat="server" Text="<%$ Resources:Option.Inventory.Product, btn_Cancel %>"
                                    Width="70px" SkinID="BTN_V1" OnClick="btn_WarningNo_Click" />
                            </td>
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle HorizontalAlign="Center" />
            <FooterStyle HorizontalAlign="Center" />
        </dx:ASPxPopupControl>
        <dx:ASPxPopupControl ID="pop_BuList" runat="server" HeaderText="<%$ Resources:Option.Inventory.Product, MsgHD2 %>"
            PopupHorizontalAlign="WindowCenter" CloseAction="CloseButton" Modal="True" PopupVerticalAlign="WindowCenter"
            Height="420px" Width="360px">
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <table style="width: 100%; height: 100%" border="0" cellpadding="1" cellspacing="0">
                        <tr>
                            <td valign="top">
                                <asp:GridView ID="grd_BuList" runat="server" AutoGenerateColumns="False" DataKeyNames="BuCode"
                                    SkinID="GRD_V1" OnRowDataBound="grd_BuList_RowDataBound" AllowPaging="True" Width="100%"
                                    Height="100%" PageSize="15">
                                    <Columns>
                                        <asp:TemplateField HeaderText="#">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chk_Item" runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" Width="5%" />
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Option.Inventory.Product, lbl_BuCode %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_BuCode" runat="server" SkinID="LBL_NR"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Option.Inventory.Product, lbl_BuName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_BuName" runat="server" SkinID="LBL_NR"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" Width="75%" />
                                            <ItemStyle HorizontalAlign="Left" Width="75%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:GridView ID="grd_BUandLocate" runat="server" AutoGenerateColumns="False" DataKeyNames="BuCode"
                                    SkinID="GRD_V1" OnRowDataBound="grd_BuList_RowDataBound" AllowPaging="True" Width="100%"
                                    Height="100%" PageSize="15" Visible="false">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Panel ID="Panel1" runat="server">
                                                    <table border="1" cellpadding="0" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td style="width: 5%">
                                                                &nbsp;
                                                            </td>
                                                            <td style="width: 5%">
                                                                &nbsp;
                                                            </td>
                                                            <td style="width: 15%" align="left">
                                                                <asp:Label ID="lbl_BuCode" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_BuCode %>"></asp:Label>
                                                            </td>
                                                            <td style="width: 65%" align="left">
                                                                <asp:Label ID="lbl_BuName" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_BuName %>"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <table border="1" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td style="width: 5%">
                                                            <asp:CheckBox ID="chk_Item" runat="server" />
                                                        </td>
                                                        <td style="width: 5%">
                                                            <asp:ImageButton ID="btn_Expand" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/Plus.jpg"
                                                                OnClick="btn_Expand_Click" Style="height: 11px" />
                                                        </td>
                                                        <td style="width: 15%" align="left">
                                                            <asp:Label ID="lbl_BuCode" runat="server" SkinID="LBL_NR"></asp:Label>
                                                        </td>
                                                        <td style="width: 65%" align="left">
                                                            <asp:Label ID="lbl_BuName" runat="server" SkinID="LBL_NR"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:Panel ID="p_StoreLocation" runat="server" Visible="false">
                                                                <asp:GridView ID="grd_StoreLocation" runat="server" AutoGenerateColumns="False" DataKeyNames="LocationCode"
                                                                    SkinID="GRD_V1" AllowPaging="True" Width="100%" Height="100%" OnRowDataBound="grd_StoreLocation_RowDataBound"
                                                                    PageSize="10" GridLines="Horizontal">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="#">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chk_Item" runat="server" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Left" Width="5%" />
                                                                            <ItemStyle Width="5%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Option.Inventory.Product, lbl_BuCode %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbl_LocationCode" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="<%$ Resources:Option.Inventory.Product, lbl_BuName %>">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbl_LocationName" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Left" Width="75%" />
                                                                            <ItemStyle HorizontalAlign="Left" Width="75%" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btn_Pop_BuList_OK" runat="server" OnClick="btn_Pop_BuList_OK_Click"
                                                Text="<%$ Resources:Option.Inventory.Product, btn_SuccessOk %>" SkinID="BTN_V1"
                                                Width="75px" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btn_Pop_BuList_Cancel" runat="server" OnClick="btn_Pop_BuList_Cancel_Click"
                                                Text="<%$ Resources:Option.Inventory.Product, btn_Cancel %>" SkinID="BTN_V1"
                                                Width="75px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
        <dx:ASPxPopupControl ID="pop_Message" runat="server" Modal="True" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" HeaderText="<%$ Resources:Option.Inventory.Product, MsgHD3 %>">
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <asp:Label ID="lbl_Message" runat="server" SkinID="LBL_NR"></asp:Label>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
        <dx:ASPxPopupControl ID="pop_AlertMinMax" runat="server" Width="300px" CloseAction="None"
            HeaderText="<%$ Resources:Option.Inventory.Product, MsgHD3 %>" Modal="True" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" ClientInstanceName="pop_ConfrimDelete" AllowDragging="True"
            EnableAnimation="False" EnableViewState="false" ShowFooter="false">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server" Width="300px">
                    <table border="0" cellpadding="5" cellspacing="2" width="100%">
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Label ID="lbl_AlertMinMax" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_AlertMinMax %>"
                                    SkinID="LBL_NR"></asp:Label>
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr align="center">
                            <td align="center">
                                <asp:Button ID="btn_AlertOK" runat="server" Text="OK" Width="50px" SkinID="BTN_V1"
                                    OnClick="btn_AlertOK_Click" />
                            </td>
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle HorizontalAlign="Center" />
            <FooterStyle HorizontalAlign="Center" />
        </dx:ASPxPopupControl>
        <dx:ASPxPopupControl ID="pop_AlertMinMaxEmpty" runat="server" Width="300px" CloseAction="None"
            HeaderText="<%$ Resources:Option.Inventory.Product, MsgHD3 %>" Modal="True" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" ClientInstanceName="pop_ConfrimDelete" AllowDragging="True"
            EnableAnimation="False" EnableViewState="false" ShowFooter="false">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server" Width="300px">
                    <table border="0" cellpadding="5" cellspacing="2" width="100%">
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Label ID="lbl_AlertMinMaxEmpty" runat="server" SkinID="LBL_NR"></asp:Label>
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr align="center">
                            <td align="center">
                                <asp:Button ID="btn_AlertMinMaxEmpty" runat="server" Text="<%$ Resources:Option.Inventory.Product, btn_AlertMinMaxEmpty %>"
                                    Width="50px" SkinID="BTN_V1" OnClick="btn_AlertMinMaxEmpty_Click" />
                            </td>
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle HorizontalAlign="Center" />
            <FooterStyle HorizontalAlign="Center" />
        </dx:ASPxPopupControl>
        <dx:ASPxPopupControl ID="pop_AlertMinMaxSave" runat="server" Width="300px" CloseAction="None"
            HeaderText="<%$ Resources:Option.Inventory.Product, MsgHD3 %>" Modal="True" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" ClientInstanceName="pop_ConfrimDelete" AllowDragging="True"
            EnableAnimation="False" EnableViewState="false" ShowFooter="false">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl6" runat="server" Width="300px">
                    <table border="0" cellpadding="5" cellspacing="2" width="100%">
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Label ID="lbl_AlertMinMaxSave" runat="server" SkinID="LBL_NR" Text="<%$ Resources:Option.Inventory.Product, lbl_AlertMinMaxSave %>"></asp:Label>
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr align="center">
                            <td align="center">
                                <asp:Button ID="btn_AlertMinMaxSave" runat="server" Text="<%$ Resources:Option.Inventory.Product, btn_AlertMinMaxSave %>"
                                    Width="50px" SkinID="BTN_V1" OnClick="btn_AlertMinMaxSave_Click" />
                            </td>
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle HorizontalAlign="Center" />
            <FooterStyle HorizontalAlign="Center" />
        </dx:ASPxPopupControl>
        <dx:ASPxPopupControl ID="pop_AssignProduct" runat="server" HeaderText="<%$ Resources:Option.Inventory.Product, pop_AssignProduct %>"
            PopupHorizontalAlign="WindowCenter" CloseAction="CloseButton" Modal="True" PopupVerticalAlign="WindowCenter"
            Height="360px" Width="750px">
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <table style="width: 100%;" border="0" cellpadding="1" cellspacing="0">
                        <tr>
                            <td>
                                <asp:GridView ID="grd_AssignProduct" runat="server" AutoGenerateColumns="False" DataKeyNames="VendorCode"
                                    SkinID="GRD_V1" OnRowDataBound="grd_AssignProduct_RowDataBound" AllowPaging="True"
                                    Width="100%">
                                    <%--OnPageIndexChanging="grd_AssignProduct_PageIndexChanging"--%>
                                    <Columns>
                                        <asp:TemplateField HeaderText="#">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chk_Item" runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" Width="5%" />
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Option.Inventory.Product, lbl_BuCode %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_VendorCode" runat="server" SkinID="LBL_NR"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Option.Inventory.Product, lbl_BuName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_VendorName" runat="server" SkinID="LBL_NR"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" Width="75%" />
                                            <ItemStyle HorizontalAlign="Left" Width="75%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btn_Pop_Vendor_OK" runat="server" OnClick="btn_Pop_Vendor_OK_Click"
                                                Text="<%$ Resources:Option.Inventory.Product, btn_SuccessOk %>" Width="75px"
                                                SkinID="BTN_V1" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btn_Pop_Vendor_Cancel" runat="server" OnClick="btn_Pop_Vendor_Cancel_Click"
                                                Text="<%$ Resources:Option.Inventory.Product, btn_Cancel %>" Width="75px" SkinID="BTN_V1" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
        <dx:ASPxPopupControl ID="pop_AssignStore" runat="server" HeaderText="<%$ Resources:Option.Inventory.Product, pop_AssignStore %>"
            PopupHorizontalAlign="WindowCenter" CloseAction="CloseButton" Modal="True" PopupVerticalAlign="WindowCenter"
            Height="360px" Width="750px">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                    <table style="width: 100%;" border="0" cellpadding="0" cellspacing="0">
                        <tr style="background-color: #4d4d4d; height: 17px; padding-left: 10px">
                            <td align="right">
                                <dx:ASPxMenu runat="server" ID="ASPxMenu1" Font-Bold="True" BackColor="Transparent"
                                    Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle" Height="16px"
                                    OnItemClick="menuStore_ItemClick" Visible="false">
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
                                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png"
                                                        Repeat="NoRepeat" VerticalPosition="center" />
                                                </HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/save.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </ItemStyle>
                                        </dx:MenuItem>
                                        <dx:MenuItem Name="Close" Text="">
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
                        <tr>
                            <td>
                             <%--OnLoad="grd_Store_OnLoad"--%>
                                <div style="height: 340px; width: 750px; overflow: auto;">
                                    <asp:GridView ID="grd_Store" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
                                        SkinID="GRD_V1" Width="100%" OnRowDataBound="grd_Store_RowDataBound" DataKeyNames="LocationCode" >
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chk_SelAll" runat="server" onclick="Check(this)" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk_Item" runat="server" AutoPostBack="true" OnCheckedChanged="chk_Item_CheckedChanged" />
                                                </ItemTemplate>
                                                <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                                <ItemStyle Width="5%" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Option.Inventory.Product, lbl_LocationCode %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_LocationCode" runat="server" SkinID="LBL_NR"></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="20%" HorizontalAlign="Left" />
                                                <ItemStyle Width="20%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Option.Inventory.Product, lbl_LocationName %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_LocationName" runat="server" SkinID="LBL_NR"></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="45%" HorizontalAlign="Left" />
                                                <ItemStyle Width="45%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Option.Inventory.Product, lbl_MinQty %>">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_MinQty" runat="server" SkinID="TXT_NUM_V1" MaxLength="7" Width="50%"
                                                        Enabled="false"></asp:TextBox>
                                                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender" runat="server"
                                                        TargetControlID="txt_MinQty" ValidChars="0123456789.">
                                                    </ajaxToolkit:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Right" Width="15%" />
                                                <ItemStyle Width="15%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Option.Inventory.Product, lbl_MaxQty %>">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txt_MaxQty" runat="server" SkinID="TXT_NUM_V1" MaxLength="7" Width="50%"
                                                        Enabled="false"></asp:TextBox>
                                                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                                        TargetControlID="txt_MaxQty" ValidChars="0123456789.">
                                                    </ajaxToolkit:FilteredTextBoxExtender>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Right" Width="15%" />
                                                <ItemStyle Width="15%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 5px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <table cellpadding="1" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btn_Save" runat="server" Text="<%$ Resources:Option.Inventory.Product, btn_Save %>"
                                                Width="60px" SkinID="BTN_V1" OnClick="btn_Save_Click" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btn_Clear" runat="server" Text="<%$ Resources:Option.Inventory.Product, btn_Clear %>"
                                                Width="60px" SkinID="BTN_V1" OnClick="btn_Clear_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
    </div>
</asp:Content>
