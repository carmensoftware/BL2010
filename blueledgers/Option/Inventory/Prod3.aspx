<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="Prod3.aspx.cs" Inherits="BlueLedger.PL.Option.Inventory.Prod3" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Src="ProdUnit.ascx" TagName="ProdUnit" TagPrefix="uc1" %>
<%@ Register Src="ProdLoc.ascx" TagName="ProdLoc" TagPrefix="uc2" %>
<%@ Register Src="ProdVendor.ascx" TagName="ProdVendor" TagPrefix="uc3" %>
<%@ Register Src="ProdBu.ascx" TagName="ProdBu" TagPrefix="uc4" %>
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
    <asp:UpdatePanel ID="UdPnDetail" runat="server">
        <ContentTemplate>
        <div>
            <asp:HiddenField ID="hf_ConnStr" runat="server" />
            <div align="left">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td align="left">
                            <!-- Menu -------------------------------------------------------------------->
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
                            <!-- Product Information ----------------------------------------------------->
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
                                <tr>
                                    <td align="right" style="width: 13%">
                                        <asp:Label ID="lbl_HeaderMemberInRecipe" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderMemberInRecipe %>"
                                            SkinID="LBL_HD"></asp:Label>
                                    </td>
                                    <td align="left" style="width: 20%">
                                        <asp:Label ID="lbl_MemberInRecipe" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                                    </td>
                                    <td align="right" style="width: 13%">
                                        &nbsp;
                                    </td>
                                    <td align="left" style="width: 20%">
                                        &nbsp;
                                    </td>
                                    <td align="right" style="width: 13%">
                                        <asp:Label ID="lbl_InvenConvOrder_Hr" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_InvenConvOrder_Hr %>"
                                            SkinID="LBL_HD" Visible="false"></asp:Label>
                                    </td>
                                    <td align="left" style="width: 20%">
                                        &nbsp;
                                    </td>
                                </tr>
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
                                        <asp:Label ID="Label39" runat="server" SkinID="LBL_HD" Text="Product Type"></asp:Label>
                                    </td>
                                    <td align="left" style="width: 20%">
                                        <asp:Label ID="lbl_ProdType" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
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
                                        <asp:Label ID="lbl_ReqHQAppr" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
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
                                        <asp:Label ID="lbl_OrderConverseRate" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                                    </td>
                                    <td align="right" style="width: 13%">
                                        <asp:Label ID="lbl_HeaderRecipeUnit" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderRecipeUnit %>"
                                            SkinID="LBL_HD"></asp:Label>
                                    </td>
                                    <td align="left" style="width: 20%">
                                        <asp:Label ID="lbl_RecipeUnit" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                                        <asp:Label ID="lbl_RecipeConverseRate" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" valign="top">
                            <!-- Unit Information -------------------------------------------------------->
                            <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                <tr>
                                    <td style="width: 50%" valign="top">
                                        <div>
                                            <uc1:ProdUnit ID="ProdUnitOrder" runat="server" />
                                        </div>
                                    </td>
                                    <td style="width: 50%" valign="top">
                                        <div>
                                            <uc1:ProdUnit ID="ProdUnitRecipe" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <!-- Assign Product to Store ------------------------------------------------->
                            <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <uc2:ProdLoc ID="ProdLoc" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <!-- Assign Product to Vendor ------------------------------------------------->
                            <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <uc3:ProdVendor ID="ProdVendor" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <!-- Assign Product to BU, Store ---------------------------------------------->
                            <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <uc4:ProdBu ID="ProdBu" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UdPgDetail"
        PopupControlID="UdPgDetail" BackgroundCssClass="POPUP_BG" RepositionMode="RepositionOnWindowResizeAndScroll">
    </ajaxToolkit:ModalPopupExtender>
    <asp:UpdateProgress ID="UdPgDetail" runat="server" AssociatedUpdatePanelID="UdPnDetail">
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
</asp:Content>
