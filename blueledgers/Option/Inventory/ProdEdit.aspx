<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="ProdEdit.aspx.cs" Inherits="BlueLedger.PL.Option.Inventory.ProdEdit" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Poup" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx2" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <script type="text/javascript" >

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
    </script>
    <div align="left">
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
                                    AutoPostBack="true" runat="server" Width="120px" Enabled="false">
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
                                    AutoPostBack="true" runat="server" Width="120px" Enabled="false">
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
                                    AutoPostBack="true" runat="server" Width="120px" Enabled="false">
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
                                <asp:TextBox ID="txt_Code" runat="server" Width="120px" Enabled="false">
</asp:TextBox>
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
                        <%--2012/01/09 Receive issue that set visible = false at recipe item, recipe unit, conversion--%>
                        <tr style="height: 40px; display: none">
                            <td>
                                <asp:Label ID="lbl_HeaderMemberInRecipe" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderMemberInRecipe %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="chk_IsRecipe" runat="server" AutoPostBack="true" OnCheckedChanged="chk_IsRecipe_OnCheckedChanged" />
                            </td>
                            <td>
                                <asp:Label ID="lbl_HeaderRecipeUnit" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderRecipeUnit %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td>
                                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                    <tr>
                                        <td align="left" width="100px">
                                            <dx:ASPxComboBox ID="cmb_RecipeUnit" runat="server" EnableIncrementalFiltering="True"
                                                CallbackPageSize="10" IncrementalFilteringMode="Contains" ValueType="System.String"
                                                ValueField="UnitCode" TextFormatString="{0}" TextField="Name" Width="120px" Enabled="false">
                                                <Columns>
                                                    <dx:ListBoxColumn FieldName="UnitCode" />
                                                </Columns>
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td width="5px">
                                            <asp:LinkButton ID="lnkb_RecipeUnit" runat="server" Text="Lookup" CausesValidation="false"></asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="req_RecipeUnit" ControlToValidate="cmb_RecipeUnit"
                                                runat="server" ErrorMessage="*" ForeColor="Red" Font-Size="Small" Enabled="false"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <asp:Label ID="lbl_RecipeConvInventory_Hr" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_RecipeConvInventory_Hr %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td>
                                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                    <tr>
                                        <td align="left" width="100px">
                                            <dx:ASPxSpinEdit ID="txt_RecipeConverseRate" runat="server" Width="120px" NumberType="Integer"
                                                Enabled="false" MaxValue="99999999999999999">
                                                <SpinButtons ShowIncrementButtons="False">
                                                </SpinButtons>
                                            </dx:ASPxSpinEdit>
                                        </td>
                                        <td width="5px">
                                            <asp:RequiredFieldValidator ID="req_RecipeConverseRate" ControlToValidate="txt_RecipeConverseRate"
                                                runat="server" ErrorMessage="*" ForeColor="Red" Font-Size="Small" Enabled="false"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:RegularExpressionValidator ID="reg_RecipeConverseRate" ControlToValidate="txt_RecipeConverseRate"
                                                ValidationExpression="(^\d*\.?\d*[1-9]+\d*$)|(^[1-9]+\d*\.\d*$)" ValidationGroup="check"
                                                runat="server" ErrorMessage="*Must &gt; 0" Font-Size="Small" Enabled="false"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                </table>
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
                                    <asp:ListItem Text="None" Value="N"></asp:ListItem>
                                    <asp:ListItem Text="Included" Value="I"></asp:ListItem>
                                    <asp:ListItem Text="Add" Value="A"></asp:ListItem>
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
                                            <%--<dx:ASPxSpinEdit ID="txt_TaxRate" runat="server" Number="0.00" DecimalPlaces="2"
                                                Width="120px" HorizontalAlign="Right">
                                                <SpinButtons ShowIncrementButtons="False">
                                                </SpinButtons>
                                            </dx:ASPxSpinEdit>--%>
                                            <asp:TextBox ID="txt_TaxRate" runat="server" Width="120px" SkinID="TXT_NUM_V1"></asp:TextBox>
                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server"
                                                TargetControlID="txt_TaxRate" ValidChars="0123456789.">
                                            </ajaxToolkit:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:RegularExpressionValidator ID="reg_TaxRate" ControlToValidate="txt_TaxRate"
                                                ValidationExpression="(^\d*\.?\d*[1-9]+\d*$)|(^[1-9]+\d*\.\d*$)" ValidationGroup="check"
                                                runat="server" ErrorMessage="*Must &gt; 0" Font-Size="Small" Enabled="false"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <asp:Label ID="lbl_HeaderAccountCode" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderAccountCode %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td>
                                <dx:ASPxSpinEdit ID="txt_TaxAccCode" runat="server" Width="120px" NumberType="Integer">
                                    <SpinButtons ShowIncrementButtons="False">
                                    </SpinButtons>
                                </dx:ASPxSpinEdit>
                                <%--<asp:TextBox ID="txt_TaxAccCode" runat="server" Width="150px"></asp:TextBox>--%>
                            </td>
                        </tr>
                        <tr style="height: 40px;">
                            <td>
                                <asp:Label ID="lbl_ApprovalLevel_Nm" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_ApprovalLevel_Nm %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td>
                                <%--<asp:TextBox ID="txt_ApprovalLevel" runat="server" Width="150px"></asp:TextBox>--%>
                                <dx:ASPxComboBox ID="ddl_ApprovalLevel" Width="120px" runat="server" DataSourceID="ods_ApprLv"
                                    ValueType="System.String" TextFormatString="{0} : {1}">
                                    <Columns>
                                        <dx:ListBoxColumn FieldName="ApprLvCode" Caption="Code"></dx:ListBoxColumn>
                                        <dx:ListBoxColumn FieldName="Name" Caption="Description"></dx:ListBoxColumn>
                                    </Columns>
                                </dx:ASPxComboBox>
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txt_ApprovalLevel"
                                    runat="server" ErrorMessage="* Required" ForeColor="Red" Font-Size="Small"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" ControlToValidate="txt_ApprovalLevel"
                                    ValidationExpression="(^\d*\.?\d*[1-9]+\d*$)|(^[1-9]+\d*\.\d*$)" ValidationGroup="check"
                                    runat="server" ErrorMessage="*Must Greater Than Zero" Font-Size="Small"></asp:RegularExpressionValidator>--%>
                            </td>
                            <td>
                                <asp:Label ID="lbl_HeaderQuantityDeviation" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderQuantityDeviation %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td>
                                <%--<dx:ASPxSpinEdit ID="txt_QuantityDeviation" runat="server" Number="0.00" DecimalPlaces="2"
                                    Width="120px" HorizontalAlign="Right">
                                    <SpinButtons ShowIncrementButtons="False">
                                    </SpinButtons>
                                </dx:ASPxSpinEdit>--%>
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
                                <%--<dx:ASPxSpinEdit ID="txt_PriceDeviation" runat="server" Number="0.00" DecimalPlaces="2"
                                    Width="120px" HorizontalAlign="Right">
                                    <SpinButtons ShowIncrementButtons="False">
                                    </SpinButtons>
                                </dx:ASPxSpinEdit>--%>
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
                                <%--<dx:ASPxSpinEdit ID="txt_StandardCost" runat="server" Number="0.00" DecimalPlaces="2"
                                    Width="120px" HorizontalAlign="Right">
                                    <SpinButtons ShowIncrementButtons="False">
                                    </SpinButtons>
                                </dx:ASPxSpinEdit>--%>
                                <asp:TextBox ID="txt_StandardCost" runat="server" SkinID="TXT_NUM_V1"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                    TargetControlID="txt_StandardCost" ValidChars="0123456789.">
                                </ajaxToolkit:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_HeaderLastCost" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_HeaderLastCost %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td>
                                <%--<dx:ASPxSpinEdit ID="txt_LastCost" runat="server" Number="0.00" DecimalPlaces="2"
                                    Width="120px" HorizontalAlign="Right">
                                    <SpinButtons ShowIncrementButtons="False">
                                    </SpinButtons>
                                </dx:ASPxSpinEdit>--%>
                                <asp:TextBox ID="txt_LastCost" runat="server" SkinID="TXT_NUM_V1"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server"
                                    TargetControlID="txt_StandardCost" ValidChars="0123456789.">
                                </ajaxToolkit:FilteredTextBoxExtender>
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
                                            <dx:ASPxComboBox ID="cmb_InventoryUnit" runat="server" EnableIncrementalFiltering="True"
                                                CallbackPageSize="10" IncrementalFilteringMode="Contains" ValueType="System.String"
                                                ValueField="UnitCode" TextFormatString="{0}" TextField="Name" Width="120px">
                                                <Columns>
                                                    <dx:ListBoxColumn FieldName="UnitCode" />
                                                </Columns>
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td width="5px">
                                            <asp:LinkButton ID="lnkb_InventoryUnit" runat="server" Text="Lookup" CausesValidation="false"
                                                Visible="False"></asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="req_InventoryUnit" ControlToValidate="cmb_InventoryUnit"
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
                                            <dx:ASPxComboBox ID="cmb_OrderUnit" runat="server" EnableIncrementalFiltering="True"
                                                CallbackPageSize="10" IncrementalFilteringMode="Contains" ValueType="System.String"
                                                ValueField="UnitCode" TextFormatString="{0}" TextField="Name" Width="120px" >
                                                <Columns>
                                                    <dx:ListBoxColumn FieldName="UnitCode" />
                                                </Columns>
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td width="5px">
                                            <asp:LinkButton ID="lnkb_Orderunit" runat="server" Text="Lookup" CausesValidation="false"
                                                Visible="False"></asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="req_OrderUnit" ControlToValidate="cmb_OrderUnit"
                                                runat="server" ErrorMessage="*" ForeColor="Red" Font-Size="Small"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <asp:Label ID="lbl_InvenConvOrder_Hr" runat="server" Text="<%$ Resources:Option.Inventory.Product, lbl_InvenConvOrder_Hr %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td>
                                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                    <tr>
                                        <td align="left" width="100px">
                                            <asp:TextBox ID="txt_InvenConvOrder" runat="server" SkinID="TXT_NUM_V1" ></asp:TextBox>
                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server"
                                                TargetControlID="txt_InvenConvOrder" ValidChars="0123456789.">
                                            </ajaxToolkit:FilteredTextBoxExtender>
                                        </td>
                                        <td width="5px">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="txt_InvenConvOrder"
                                                runat="server" ErrorMessage="*" ForeColor="Red" Font-Size="Small"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <%--<asp:RegularExpressionValidator ID="reg_InvenConvOrder" ControlToValidate="txt_InvenConvOrder"
                                                ValidationExpression="(^\d*\.?\d*[1-9]+\d*$)|(^[1-9]+\d*\.\d*$)" ValidationGroup="check"
                                                runat="server" ErrorMessage="*Must &gt; 0" Font-Size="Small"></asp:RegularExpressionValidator>--%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <%--<table border="0" cellpadding="2" cellspacing="0" width="100%">
                        <tr align="left" style="height: 17px; vertical-align: top">
                            <td style="padding-left: 10px; width: 12%;">
                                <asp:Label ID="lbl_Cateogry_Nm" runat="server" Text="Category" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE" style="width: 15%;">
                                <asp:DropDownList ID="ddl_Category" OnSelectedIndexChanged="ddl_Category_OnSelectedIndexChanged"
                                    AutoPostBack="true" runat="server" Width="100px" Enabled="false" SkinID="DDL_V1">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="req_Category" ControlToValidate="ddl_Category" runat="server"
                                    ErrorMessage="*" ForeColor="Red" Font-Size="Small"></asp:RequiredFieldValidator>
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
                            <td class="TD_LINE" align="right" style="width: 13%;">
                                <dx:ASPxSpinEdit ID="txt_LastCost" runat="server" Number="0.00" DecimalPlaces="2"
                                    Width="90px" Font-Names="Arail,Tahoma,MS Sans Serif" Font-Size="8pt" HorizontalAlign="Right">
                                    <SpinButtons ShowIncrementButtons="False">
                                    </SpinButtons>
                                </dx:ASPxSpinEdit>
                            </td>
                            <td class="TD_LINE" style="width: 10%;">
                                <asp:Label ID="lbl_HeaderStatus" runat="server" Text="Status" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE" style="width: 15%;">
                                <asp:CheckBox ID="chk_Status" runat="server" SkinID="CHK_V1" />
                            </td>
                        </tr>
                        <tr align="left" style="height: 17px; vertical-align: top">
                            <td style="padding-left: 10px">
                                <asp:Label ID="lbl_SubCateogry_Nm" runat="server" Text="Sub Category" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:DropDownList ID="ddl_SubCategory" OnSelectedIndexChanged="ddl_SubCategory_OnSelectedIndexChanged"
                                    AutoPostBack="true" runat="server" Width="100px" Enabled="false" SkinID="DDL_V1">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="req_SubCategory" ControlToValidate="ddl_SubCategory"
                                    runat="server" ErrorMessage="*" ForeColor="Red" Font-Size="Small"></asp:RequiredFieldValidator>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_HeaderOrderUnit" runat="server" Text="Order Unit" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr style="height: 17px; vertical-align: top">
                                        <td align="left">
                                            <dx:ASPxComboBox ID="cmb_OrderUnit" runat="server" EnableIncrementalFiltering="True"
                                                CallbackPageSize="10" IncrementalFilteringMode="Contains" ValueType="System.String"
                                                ValueField="UnitCode" TextFormatString="{0}" TextField="Name" Width="100%" Font-Names="Arail,Tahoma,MS Sans Serif"
                                                Font-Size="8pt">
                                                <Columns>
                                                    <dx:ListBoxColumn FieldName="UnitCode" />
                                                </Columns>
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td>
                                            &nbsp;<asp:LinkButton ID="lnkb_Orderunit" runat="server" Text="Lookup" CausesValidation="false"></asp:LinkButton>
                                            <Poup:ModalPopupExtender ID="Mdlp_OrderunitPopup" runat="server" TargetControlID="lnkb_Orderunit"
                                                PopupControlID="pnlLookupOrderunit" BackgroundCssClass="POPUP_DISABLED" DropShadow="false">
                                            </Poup:ModalPopupExtender>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="req_OrderUnit" ControlToValidate="cmb_OrderUnit"
                                                runat="server" ErrorMessage="*" ForeColor="Red" Font-Size="Small"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <asp:Label ID="lbl_HeaderStandardCost" runat="server" Text="Standard Cost" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE" align="right">
                                <dx:ASPxSpinEdit ID="txt_StandardCost" runat="server" Number="0.00" DecimalPlaces="2"
                                    Width="90px" Font-Names="Arail,Tahoma,MS Sans Serif" Font-Size="8pt" HorizontalAlign="Right">
                                    <SpinButtons ShowIncrementButtons="False">
                                    </SpinButtons>
                                </dx:ASPxSpinEdit>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_ApprovalLevel_Nm" runat="server" Text="Approval Level" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <dx:ASPxComboBox ID="ddl_ApprovalLevel" Width="100px" runat="server" DataSourceID="ods_ApprLv"
                                    Font-Names="Arail,Tahoma,MS Sans Serif" ValueType="System.String" TextFormatString="{0} : {1}"
                                    Font-Size="8pt">
                                    <Columns>
                                        <dx:ListBoxColumn FieldName="ApprLvCode" Caption="Code" Width="10px"></dx:ListBoxColumn>
                                        <dx:ListBoxColumn FieldName="Name" Caption="Description" Width="90px"></dx:ListBoxColumn>
                                    </Columns>
                                </dx:ASPxComboBox>
                            </td>
                        </tr>
                        <tr align="left" style="height: 17px; vertical-align: top">
                            <td style="padding-left: 10px" class="TD_LINE">
                                <asp:Label ID="lbl_ItemGroup_Nm" runat="server" Text="Item Group" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:DropDownList ID="ddl_ItemGroup" OnSelectedIndexChanged="ddl_ItemGroup_OnSelectedIndexChanged"
                                    AutoPostBack="true" runat="server" Width="100px" Enabled="false" SkinID="DDL_V1">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="req_ItemGroup" ControlToValidate="ddl_ItemGroup"
                                    runat="server" ErrorMessage="*" ForeColor="Red" Font-Size="Small"></asp:RequiredFieldValidator>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_InvenConvOrder_Hr" runat="server" Text="Unit Conversion (I)" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE" align="right">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr style="height: 17px; vertical-align: top">
                                        <td align="left">
                                            <dx:ASPxSpinEdit ID="txt_InvenConvOrder" runat="server" NumberType="Integer" Width="90px"
                                                Font-Names="Arail,Tahoma,MS Sans Serif" Font-Size="8pt" HorizontalAlign="Right">
                                                <SpinButtons ShowIncrementButtons="False">
                                                </SpinButtons>
                                            </dx:ASPxSpinEdit>
                                        </td>
                                        <td>
                                            <asp:RegularExpressionValidator ID="reg_InvenConvOrder" ControlToValidate="txt_InvenConvOrder"
                                                ValidationExpression="(^\d*\.?\d*[1-9]+\d*$)|(^[1-9]+\d*\.\d*$)" ValidationGroup="check"
                                                runat="server" ErrorMessage="**" Font-Size="Small"></asp:RegularExpressionValidator>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="txt_InvenConvOrder"
                                                runat="server" ErrorMessage="*" ForeColor="Red" Font-Size="Small"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="Label1" runat="server" Text="Average Cost" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE" align="right">
                                <asp:Label ID="lbl_AverageCost" runat="server" SkinID="LBL_NR_BLUE_NUM"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lbl_HeaderSaleItem" runat="server" Text="Sale Item" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:CheckBox ID="chk_SaleItem" runat="server" />
                            </td>
                        </tr>
                        <tr align="left" style="height: 17px; vertical-align: top">
                            <td style="padding-left: 10px;" class="TD_LINE">
                                <asp:Label ID="lbl_HeaderCode" runat="server" Text="SKU" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:TextBox ID="txt_Code" runat="server" Width="150px" Enabled="false" SkinID="TXT_V1"></asp:TextBox>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_HeaderInventoryUnit" runat="server" Text="Inventory Unit" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td align="left" style="vertical-align: top">
                                            <dx:ASPxComboBox ID="cmb_InventoryUnit" runat="server" EnableIncrementalFiltering="True"
                                                CallbackPageSize="10" IncrementalFilteringMode="Contains" ValueType="System.String"
                                                ValueField="UnitCode" TextFormatString="{0}" TextField="Name" Width="100%" Font-Names="Arail,Tahoma,MS Sans Serif"
                                                Font-Size="8pt">
                                                <Columns>
                                                    <dx:ListBoxColumn FieldName="UnitCode" />
                                                </Columns>
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td >
                                            &nbsp;<asp:LinkButton ID="lnkb_InventoryUnit" runat="server" Text="Lookup" CausesValidation="false"></asp:LinkButton>
                                            <Poup:ModalPopupExtender ID="Mdlp_InventoryUnitPopup" runat="server" TargetControlID="lnkb_InventoryUnit"
                                                PopupControlID="pnlLookupInventoryUnit" BackgroundCssClass="POPUP_DISABLED" DropShadow="false">
                                            </Poup:ModalPopupExtender>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="req_InventoryUnit" ControlToValidate="cmb_InventoryUnit"
                                                runat="server" ErrorMessage="*" ForeColor="Red" Font-Size="Small"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_HeaderPriceDeviation" runat="server" Text="Price Deviation(%)"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE" align="right">
                                <dx:ASPxSpinEdit ID="txt_PriceDeviation" runat="server" Number="0.00" DecimalPlaces="2"
                                    Width="90px" Font-Names="Arail,Tahoma,MS Sans Serif" Font-Size="8pt" HorizontalAlign="Right">
                                    <SpinButtons ShowIncrementButtons="False">
                                    </SpinButtons>
                                </dx:ASPxSpinEdit>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_HeaderTAXType" runat="server" Text="TAX Type" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:DropDownList ID="ddl_TAXType" runat="server" Width="100px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddl_TAXType_OnSelectedIndexChanged" SkinID="DDL_V1">
                                    <asp:ListItem Text="None" Value="N"></asp:ListItem>
                                    <asp:ListItem Text="Included" Value="I"></asp:ListItem>
                                    <asp:ListItem Text="Add" Value="A"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr align="left" style="height: 17px; vertical-align: top">
                            <td style="padding-left: 10px" class="TD_LINE">
                                <asp:Label ID="lbl_HeaderDescritpion1" runat="server" Text="English Description"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txt_Description1" runat="server" SkinID="TXT_V1"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="req_Description1" ControlToValidate="txt_Description1"
                                                runat="server" ErrorMessage="*" ForeColor="Red" Font-Size="Small"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_RecipeConvInventory_Hr" runat="server" Text="Unit Conversion(R)"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE" align="right">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr style="height: 17px; vertical-align: top">
                                        <td align="left">
                                            <dx:ASPxSpinEdit ID="txt_RecipeConverseRate" runat="server" NumberType="Integer"
                                                Width="90px" Font-Names="Arail,Tahoma,MS Sans Serif" Font-Size="8pt" HorizontalAlign="Right"
                                                Enabled="false">
                                                <SpinButtons ShowIncrementButtons="False">
                                                </SpinButtons>
                                            </dx:ASPxSpinEdit>
                                        </td>
                                        <td>
                                            <asp:RegularExpressionValidator ID="reg_RecipeConverseRate" ControlToValidate="txt_RecipeConverseRate"
                                                ValidationExpression="(^\d*\.?\d*[1-9]+\d*$)|(^[1-9]+\d*\.\d*$)" ValidationGroup="check"
                                                runat="server" ErrorMessage="**" Font-Size="Small" Enabled="false"></asp:RegularExpressionValidator>--%>
                    <%--<asp:TextBox ID="txt_RecipeConverseRate" runat="server" Width="150px" Enabled="false"></asp:TextBox>--%>
                    <%--</td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="req_RecipeConverseRate" ControlToValidate="txt_RecipeConverseRate"
                                                runat="server" ErrorMessage="*" ForeColor="Red" Font-Size="Small" Enabled="false"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_HeaderQuantityDeviation" runat="server" Text="Quantity Deviation(%)"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE" align="right">
                                <dx:ASPxSpinEdit ID="txt_QuantityDeviation" runat="server" Number="0.00" DecimalPlaces="2"
                                    Width="90px" Font-Names="Arail,Tahoma,MS Sans Serif" Font-Size="8pt" HorizontalAlign="Right">
                                    <SpinButtons ShowIncrementButtons="False">
                                    </SpinButtons>
                                </dx:ASPxSpinEdit>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_HeaderAccountCode" runat="server" Text="Tax Account" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE" align="left">
                                <dx:ASPxSpinEdit ID="txt_TaxAccCode" runat="server" Width="90px" HorizontalAlign="left"
                                    NumberType="Integer" Font-Names="Arail,Tahoma,MS Sans Serif" Font-Size="8pt">
                                    <SpinButtons ShowIncrementButtons="False">
                                    </SpinButtons>
                                </dx:ASPxSpinEdit>
                            </td>
                        </tr>
                        <tr align="left" style="height: 17px; vertical-align: top">
                            <td style="padding-left: 10px" class="TD_LINE">
                                <asp:Label ID="lbl_HeaderDescription2" runat="server" Text="Local Description" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:TextBox ID="txt_Descritpion2" runat="server" Width="150px" SkinID="TXT_V1"></asp:TextBox>
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_HeaderRecipeUnit" runat="server" Text="Recipe Unit" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:CheckBox ID="chk_IsRecipe" runat="server" AutoPostBack="true" OnCheckedChanged="chk_IsRecipe_OnCheckedChanged" />
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_HeaderPriceDeviation0" runat="server" Text="Central Purchase"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE">
                                <asp:CheckBox ID="chk_ReqHQAppr" runat="server" />
                            </td>
                            <td class="TD_LINE">
                                <asp:Label ID="lbl_HeaderTaxRate" runat="server" Text="Tax Rate(%)" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE" align="right">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr style="height: 17px; vertical-align: top">
                                        <td>
                                            <dx:ASPxSpinEdit ID="txt_TaxRate" runat="server" Number="0.00" DecimalPlaces="2"
                                                Width="90px" Font-Names="Arail,Tahoma,MS Sans Serif" Font-Size="8pt" HorizontalAlign="Right">
                                                <SpinButtons ShowIncrementButtons="False">
                                                </SpinButtons>
                                            </dx:ASPxSpinEdit>
                                        </td>
                                        <td>
                                            <asp:RegularExpressionValidator ID="reg_TaxRate" ControlToValidate="txt_TaxRate"
                                                ValidationExpression="(^\d*\.?\d*[1-9]+\d*$)|(^[1-9]+\d*\.\d*$)" ValidationGroup="check"
                                                runat="server" ErrorMessage="**" Font-Size="Small" Enabled="false"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr align="left" style="height: 17px; vertical-align: top">
                            <td style="padding-left: 10px" class="TD_LINE">
                                <asp:Label ID="lbl_HeaderBarCode" runat="server" Text="Bar Code" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE" colspan="7">
                                <asp:TextBox ID="txt_BarCode" runat="server" Width="150px" SkinID="TXT_V1"></asp:TextBox>
                            </td>--%>
                    <%--<td>
                                <asp:Label ID="lbl_HeaderMemberInRecipe" runat="server" Text="Recipe Item" SkinID="LBL_HD"
                                    Visible="false"></asp:Label>
                            </td>
                            <td>
                                <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                    <tr style="height: 17px; vertical-align: top">
                                        <td align="left" width="155px">
                                            <dx:ASPxComboBox ID="cmb_RecipeUnit" runat="server" EnableIncrementalFiltering="True"
                                                CallbackPageSize="10" IncrementalFilteringMode="Contains" ValueType="System.String"
                                                ValueField="UnitCode" TextFormatString="{0}" TextField="Name" Width="100%" Enabled="false">
                                                <Columns>
                                                    <dx:ListBoxColumn FieldName="UnitCode" />
                                                </Columns>
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td width="50px">
                                            <asp:LinkButton ID="lnkb_RecipeUnit" runat="server" Text="Lookup" CausesValidation="false"></asp:LinkButton>
                                            <Poup:ModalPopupExtender ID="Mdlp_RecipeUnitPopup" runat="server" TargetControlID="lnkb_RecipeUnit"
                                                PopupControlID="pnlLookupRecipeUnit" BackgroundCssClass="POPUP_DISABLED" DropShadow="false">
                                            </Poup:ModalPopupExtender>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="req_RecipeUnit" ControlToValidate="cmb_RecipeUnit"
                                                runat="server" ErrorMessage="* Required" ForeColor="Red" Font-Size="Small" Enabled="false"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                            </td>--%>
                    <%--  </tr>
                        <tr align="right">
                            <td colspan="8">
                                <asp:Label ID="Label8" runat="server" Text="* Required" SkinID="LBL_NR_RED"></asp:Label>
                                &nbsp;
                                <asp:Label ID="Label9" runat="server" Text="** Must Greater Than Zero" SkinID="LBL_NR_RED"></asp:Label>
                            </td>
                        </tr>
                    </table>--%>
                </td>
            </tr>
        </table>
        <asp:Panel Style="cursor: move; display: none;" ID="pnlLookupRecipeUnit" runat="server"
            Width="500px">
            <table border="0" cellpadding="2" cellspacing="0" width="100%">
                <tr>
                    <td align="left">
                        <table border="0" cellpadding="2" cellspacing="0" width="100%">
                            <tr style="height: 17px">
                                <td style="background-color: #DEDEDE">
                                    <asp:Label ID="lbl_Title" runat="server" Font-Bold="True" Font-Size="13pt" Text="Unit"></asp:Label>
                                </td>
                                <td align="right" style="background-color: #DEDEDE">
                                    <dx:ASPxRoundPanel ID="ASPxRoundPanel" runat="server" SkinID="COMMANDBAR">
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent1" runat="server">
                                                <dx:ASPxMenu ID="ASPxMenu1" runat="server" SkinID="COMMAND_BAR" OnItemClick="menu_ItemClick">
                                                    <Items>
                                                        <dx:MenuItem Name="Create" Text="Create">
                                                            <Image Url="~/App_Themes/Default/Images/create.gif">
                                                            </Image>
                                                        </dx:MenuItem>
                                                        <dx:MenuItem Text="Delete">
                                                            <Image Url="~/App_Themes/Default/Images/delete.gif">
                                                            </Image>
                                                        </dx:MenuItem>
                                                        <dx:MenuItem Text="Close">
                                                            <Image Url="~/App_Themes/Default/Images/print.gif">
                                                            </Image>
                                                        </dx:MenuItem>
                                                    </Items>
                                                </dx:ASPxMenu>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dx:ASPxRoundPanel>
                                </td>
                            </tr>
                        </table>
                        <dx:ASPxGridView ID="grd_Unit" runat="server" AutoGenerateColumns="False" KeyFieldName="UnitCode"
                            ClientInstanceName="grd_Unit" Width="100%" OnLoad="grd_Unit_OnLoad" OnRowUpdating="grd_Unit_RowUpdating"
                            OnInitNewRow="grd_Unit_InitNewRow" OnRowInserting="grd_Unit_RowInserting">
                            <Styles>
                                <Header HorizontalAlign="Center">
                                </Header>
                            </Styles>
                            <SettingsPager AlwaysShowPager="True" PageSize="10">
                            </SettingsPager>
                            <SettingsEditing Mode="Inline" NewItemRowPosition="Bottom" />
                            <Columns>
                                <dx:GridViewCommandColumn VisibleIndex="0" ShowSelectCheckbox="True" Width="50px">
                                    <ClearFilterButton Visible="True">
                                    </ClearFilterButton>
                                    <HeaderTemplate>
                                        <input id="chk_SelAll" type="checkbox" onclick="grd_Unit.SelectAllRowsOnPage(this.checked);" />
                                    </HeaderTemplate>
                                </dx:GridViewCommandColumn>
                                <dx:GridViewCommandColumn Caption="&amp;nbsp;" VisibleIndex="1" Width="100px">
                                    <EditButton Text="Edit" Visible="True">
                                    </EditButton>
                                </dx:GridViewCommandColumn>
                                <dx:GridViewDataTextColumn FieldName="UnitCode" VisibleIndex="2" Caption="Code">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="Name" VisibleIndex="3" Caption="Description">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataCheckColumn FieldName="IsActived" VisibleIndex="4" Caption="Actived">
                                </dx:GridViewDataCheckColumn>
                            </Columns>
                            <Settings ShowFilterRow="True" />
                        </dx:ASPxGridView>
                    </td>
                </tr>
            </table>
            <dx:ASPxPopupControl ID="pop_ConfrimDelete" runat="server" Width="300px" CloseAction="CloseButton"
                HeaderText="Confirm" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                ClientInstanceName="pop_ConfrimDelete" AllowDragging="True" EnableAnimation="False"
                EnableViewState="false" ShowFooter="false">
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                        <table border="0" cellpadding="5" cellspacing="2" width="100%">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="Label3" runat="server" Text="Are you sure to delete this selected row?"></asp:Label>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr align="center">
                                <td align="right">
                                    <dx:ASPxButton ID="btn_ConfrimDelete" runat="server" OnClick="btn_ConfrimDelete_Click"
                                        Text="Yes" CausesValidation="false">
                                    </dx:ASPxButton>
                                </td>
                                <td align="left">
                                    <dx:ASPxButton ID="btn_ConfrimDeleteNo" Width="50px" runat="server" OnClick="btn_ConfrimDeleteNo_Click"
                                        Text="No" CausesValidation="false">
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle HorizontalAlign="Left" />
            </dx:ASPxPopupControl>
        </asp:Panel>
        <Poup:DragPanelExtender ID="dpe_RecipeUnitPopup" runat="server" TargetControlID="pnlLookupRecipeUnit">
        </Poup:DragPanelExtender>
        <asp:Panel Style="cursor: move; display: none;" ID="pnlLookupInventoryUnit" runat="server"
            Width="500px">
            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                <tr>
                    <td align="left">
                        <table border="0" cellpadding="1" cellspacing="0" width="100%">
                            <tr style="height: 40px">
                                <td style="background-color: #DEDEDE">
                                    <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="13pt" Text="Unit"></asp:Label>
                                </td>
                                <td align="right" style="background-color: #DEDEDE">
                                    <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" SkinID="COMMANDBAR">
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent2" runat="server">
                                                <dx:ASPxMenu ID="menuInventoryUnit" runat="server" SkinID="COMMAND_BAR" OnItemClick="menuInventoryUnit_ItemClick">
                                                    <Items>
                                                        <dx:MenuItem Name="Create" Text="Create">
                                                            <Image Url="~/App_Themes/Default/Images/create.gif">
                                                            </Image>
                                                        </dx:MenuItem>
                                                        <dx:MenuItem Text="Delete">
                                                            <Image Url="~/App_Themes/Default/Images/delete.gif">
                                                            </Image>
                                                        </dx:MenuItem>
                                                        <dx:MenuItem Text="Close">
                                                            <Image Url="~/App_Themes/Default/Images/print.gif">
                                                            </Image>
                                                        </dx:MenuItem>
                                                    </Items>
                                                </dx:ASPxMenu>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dx:ASPxRoundPanel>
                                </td>
                            </tr>
                        </table>
                        <dx:ASPxGridView ID="grd_InventoryUnit" runat="server" AutoGenerateColumns="False"
                            KeyFieldName="UnitCode" ClientInstanceName="grd_InventoryUnit" Width="100%" OnLoad="grd_InventoryUnit_OnLoad"
                            OnRowUpdating="grd_InventoryUnit_RowUpdating" OnInitNewRow="grd_InventoryUnit_InitNewRow"
                            OnRowInserting="grd_InventoryUnit_RowInserting">
                            <Styles>
                                <Header HorizontalAlign="Center">
                                </Header>
                            </Styles>
                            <SettingsPager AlwaysShowPager="True" PageSize="10">
                            </SettingsPager>
                            <SettingsEditing Mode="Inline" NewItemRowPosition="Bottom" />
                            <Columns>
                                <dx:GridViewCommandColumn VisibleIndex="0" ShowSelectCheckbox="True" Width="50px">
                                    <ClearFilterButton Visible="True">
                                    </ClearFilterButton>
                                    <HeaderTemplate>
                                        <input id="chk_SelAll" type="checkbox" onclick="grd_InventoryUnit.SelectAllRowsOnPage(this.checked);" />
                                    </HeaderTemplate>
                                </dx:GridViewCommandColumn>
                                <dx:GridViewCommandColumn Caption="&amp;nbsp;" VisibleIndex="1" Width="100px">
                                    <EditButton Text="Edit" Visible="True">
                                    </EditButton>
                                </dx:GridViewCommandColumn>
                                <dx:GridViewDataTextColumn FieldName="UnitCode" VisibleIndex="2" Caption="Code">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="Name" VisibleIndex="3" Caption="Description">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataCheckColumn FieldName="IsActived" VisibleIndex="4" Caption="Actived">
                                </dx:GridViewDataCheckColumn>
                            </Columns>
                            <Settings ShowFilterRow="True" />
                        </dx:ASPxGridView>
                    </td>
                </tr>
            </table>
            <dx:ASPxPopupControl ID="pop_ConfrimDeleteInventoryUnit" runat="server" Width="300px"
                CloseAction="CloseButton" HeaderText="Confirm" Modal="True" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" ClientInstanceName="pop_ConfrimDeleteInventoryUnit"
                AllowDragging="True" EnableAnimation="False" EnableViewState="false" ShowFooter="false">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <table border="0" cellpadding="5" cellspacing="2" width="100%">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="Label5" runat="server" Text="Are you sure to delete this selected row?"></asp:Label>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr align="center">
                                <td align="right">
                                    <dx:ASPxButton ID="btn_ConfrimDeleteInventoryUnit" runat="server" OnClick="btn_ConfrimDeleteInventoryUnit_Click"
                                        Text="Yes" CausesValidation="false">
                                    </dx:ASPxButton>
                                </td>
                                <td align="left">
                                    <dx:ASPxButton ID="btn_ConfrimDeleteInventoryUnitNo" Width="50px" runat="server"
                                        OnClick="btn_ConfrimDeleteInventoryUnitNo_Click" Text="No" CausesValidation="false">
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle HorizontalAlign="Left" />
            </dx:ASPxPopupControl>
        </asp:Panel>
        <Poup:DragPanelExtender ID="dpe_InventoryUnitPopup" runat="server" TargetControlID="pnlLookupInventoryUnit">
        </Poup:DragPanelExtender>
        <asp:Panel Style="cursor: move; display: none;" ID="pnlLookupOrderunit" runat="server"
            Width="500px">
            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                <tr>
                    <td align="left">
                        <table border="0" cellpadding="1" cellspacing="0" width="100%">
                            <tr style="height: 40px">
                                <td style="background-color: #DEDEDE">
                                    <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Size="13pt" Text="Unit"></asp:Label>
                                </td>
                                <td align="right" style="background-color: #DEDEDE">
                                    <dx:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" SkinID="COMMANDBAR">
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent3" runat="server">
                                                <dx:ASPxMenu ID="menuOrderunit" runat="server" SkinID="COMMAND_BAR" OnItemClick="menuOrderunit_ItemClick">
                                                    <Items>
                                                        <dx:MenuItem Name="Create" Text="Create">
                                                            <Image Url="~/App_Themes/Default/Images/create.gif">
                                                            </Image>
                                                        </dx:MenuItem>
                                                        <dx:MenuItem Text="Delete">
                                                            <Image Url="~/App_Themes/Default/Images/delete.gif">
                                                            </Image>
                                                        </dx:MenuItem>
                                                        <dx:MenuItem Text="Close">
                                                            <Image Url="~/App_Themes/Default/Images/print.gif">
                                                            </Image>
                                                        </dx:MenuItem>
                                                    </Items>
                                                </dx:ASPxMenu>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dx:ASPxRoundPanel>
                                </td>
                            </tr>
                        </table>
                        <dx:ASPxGridView ID="grd_Orderunit" runat="server" AutoGenerateColumns="False" KeyFieldName="UnitCode"
                            ClientInstanceName="grd_Orderunit" Width="100%" OnLoad="grd_Orderunit_OnLoad"
                            OnRowUpdating="grd_Orderunit_RowUpdating" OnInitNewRow="grd_Orderunit_InitNewRow"
                            OnRowInserting="grd_Orderunit_RowInserting">
                            <Styles>
                                <Header HorizontalAlign="Center">
                                </Header>
                            </Styles>
                            <SettingsPager AlwaysShowPager="True" PageSize="10">
                            </SettingsPager>
                            <SettingsEditing Mode="Inline" NewItemRowPosition="Bottom" />
                            <Columns>
                                <dx:GridViewCommandColumn VisibleIndex="0" ShowSelectCheckbox="True" Width="50px">
                                    <ClearFilterButton Visible="True">
                                    </ClearFilterButton>
                                    <HeaderTemplate>
                                        <input id="chk_SelAll" type="checkbox" onclick="grd_Orderunit.SelectAllRowsOnPage(this.checked);" />
                                    </HeaderTemplate>
                                </dx:GridViewCommandColumn>
                                <dx:GridViewCommandColumn Caption="&amp;nbsp;" VisibleIndex="1" Width="100px">
                                    <EditButton Text="Edit" Visible="True">
                                    </EditButton>
                                </dx:GridViewCommandColumn>
                                <dx:GridViewDataTextColumn FieldName="UnitCode" VisibleIndex="2" Caption="Code">
                                    <EditFormSettings Visible="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="Name" VisibleIndex="3" Caption="Description">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataCheckColumn FieldName="IsActived" VisibleIndex="4" Caption="Actived">
                                </dx:GridViewDataCheckColumn>
                            </Columns>
                            <Settings ShowFilterRow="True" />
                        </dx:ASPxGridView>
                    </td>
                </tr>
            </table>
            <dx:ASPxPopupControl ID="pop_ConfrimDeleteOrderunit" runat="server" Width="300px"
                CloseAction="CloseButton" HeaderText="<%$ Resources:Option.Inventory.Product, btn_Confrim %>"
                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                ClientInstanceName="pop_ConfrimDeleteOrderunit" AllowDragging="True" EnableAnimation="False"
                EnableViewState="false" ShowFooter="false">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                        <table border="0" cellpadding="5" cellspacing="2" width="100%">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Option.Inventory.Product, MsgWarning3 %>"></asp:Label>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr align="center">
                                <td align="right">
                                    <dx:ASPxButton ID="btn_ConfrimDeleteOrderunit" runat="server" OnClick="btn_ConfrimDeleteOrderunit_Click"
                                        Text="<%$ Resources:Option.Inventory.Product, btn_Confrim %>" CausesValidation="false">
                                    </dx:ASPxButton>
                                </td>
                                <td align="left">
                                    <dx:ASPxButton ID="btn_OrderunitNo" Width="50px" runat="server" OnClick="btn_OrderunitNo_Click"
                                        Text="<%$ Resources:Option.Inventory.Product, btn_Cancel %>" CausesValidation="false">
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle HorizontalAlign="Left" />
            </dx:ASPxPopupControl>
        </asp:Panel>
        <Poup:DragPanelExtender ID="dpe_OrderunitPopup" runat="server" TargetControlID="pnlLookupOrderunit">
        </Poup:DragPanelExtender>
        <asp:ObjectDataSource ID="ods_ApprLv" runat="server" SelectMethod="GetLookUp" TypeName="Blue.BL.Option.Inventory.ApprLv">
            <SelectParameters>
                <asp:ControlParameter ControlID="hf_ConnStr" Name="connStr" PropertyName="Value"
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:HiddenField ID="hf_ConnStr" runat="server" />
        <dx:ASPxPopupControl ID="pop_AlertTaxRate" runat="server" HeaderText="<%$ Resources:Option.Inventory.Product, MsgHD %>"
            Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            ShowCloseButton="False" Width="238px" CloseAction="CloseButton">
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <table border="0" cellpadding="5" cellspacing="0" width="100%">
                        <tr>
                            <td align="center" style="height: 20px">
                                <asp:Label ID="Label38" runat="server" Text="<%$ Resources:Option.Inventory.Product, MsgWarning2 %>"
                                    SkinID="LBL_NR"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <%--<dx:ASPxButton ID="btn_OK" runat="server" OnClick="btn_OK_Click" Text="OK" SkinID="BTN_N1">
                                </dx:ASPxButton>--%>
                                <asp:Button ID="btn_OK" runat="server" OnClick="btn_OK_Click" Text="<%$ Resources:Option.Inventory.Product, btn_SuccessOk %>"
                                    SkinID="BTN_V1" Width="50px" />
                            </td>
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td>
                    <asp:GridView ID="grd_ProdUnit" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
                        ShowFooter="True" SkinID="GRD_V1" Width="100%" Visible="False">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkb_New" runat="server" SkinID="LNKB_GRD_HEADER" OnClick="lnkb_New_Click">+</asp:LinkButton>
                                </HeaderTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Unit">
                                <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                <ItemTemplate>
                                    <asp:Label ID="lbl_OrderUnit" runat="server" Text='<%# Eval("OrderUnit") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description">
                                <HeaderStyle HorizontalAlign="Left" Width="300px" />
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Description" runat="server" Text='<%# Eval("OrderUnit") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rate">
                                <HeaderStyle HorizontalAlign="Right" Width="80px" />
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Rate" runat="server" Text='<%# Eval("Rate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="&nbsp;"></asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                <tr class="grdHeaderRow_V1">
                                    <td align="center" style="width: 50px">
                                        <asp:LinkButton ID="lnkb_New" runat="server" SkinID="LNKB_GRD_HEADER" OnClick="lnkb_New_Click">+</asp:LinkButton>
                                    </td>
                                    <td align="left" style="width: 150px">
                                        Unit
                                    </td>
                                    <td align="left" style="width: 300px">
                                        Description
                                    </td>
                                    <td align="right" style="width: 80px">
                                        Rate
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                    </asp:GridView>
                    <dx2:ASPxPopupControl ID="pop_ProdUnit" runat="server" CloseAction="CloseButton"
                        HeaderText="Product Conversion Rate" Height="420px" Modal="True" PopupHorizontalAlign="WindowCenter"
                        PopupVerticalAlign="WindowCenter" Width="360px">
                        <ContentStyle>
                            <Paddings Padding="5px" />
                        </ContentStyle>
                        <ContentCollection>
                            <dx:PopupControlContentControl runat="server">
                                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" Text="Product" ID="lbl_FromProduct"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" AutoPostBack="True" SkinID="DDL_V1" Width="275px"
                                                ID="ddl_Product" OnSelectedIndexChanged="ddl_Product_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div style="width: 360px; height: 330px; overflow-y: scroll; overflow-x: hidden">
                                                <asp:GridView runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
                                                    SkinID="GRD_V1" Width="100%" ID="grd_UnitList" OnRowDataBound="grd_UnitList_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chk_Selected" runat="server" />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" Width="20px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Unit">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_Unit" runat="server" Text='<%# Bind("OrderUnit") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="80px"></HeaderStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Description">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_Description0" runat="server" Text='<%# Bind("OrderUnit") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="150px"></HeaderStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Rate">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txt_Rate" runat="server" Width="95%" Text='<%# Bind("Rate") %>'></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Right" Width="50px"></HeaderStyle>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                            <tr class="grdHeaderRow_V1">
                                                                <td align="left" style="width: 20px">
                                                                    &nbsp;
                                                                </td>
                                                                <td align="left" style="width: 80px">
                                                                    Unit
                                                                </td>
                                                                <td align="left" style="width: 150px">
                                                                    Description
                                                                </td>
                                                                <td align="right" style="width: 50px">
                                                                    Rate
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" colspan="2">
                                            <asp:Button runat="server" Text="OK" SkinID="BTN_V1" Width="60px" ID="btn_OK_ProdUnit"
                                                OnClick="btn_OK_ProdUnit_Click"></asp:Button>
                                            <asp:Button runat="server" Text="Cancel" SkinID="BTN_V1" Width="60px" 
                                                ID="btn_Cancel_ProdUnit" OnClick="btn_Cancel_ProdUnit_Click">
                                            </asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx2:ASPxPopupControl>
                    <dx2:ASPxPopupControl ID="pop_Error" runat="server" CloseAction="CloseButton" HeaderText="Information"
                        Modal="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                        Width="320px">
                        <ContentStyle HorizontalAlign="Center">
                        </ContentStyle>
                        <ContentCollection>
                            <dx2:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
                                <asp:Label ID="lbl_Error" runat="server"></asp:Label>
                            </dx2:PopupControlContentControl>
                        </ContentCollection>
                    </dx2:ASPxPopupControl>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
