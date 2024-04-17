<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="VdEdit.aspx.cs" Inherits="BlueLedger.PL.PC.PL.Vendor.VdEdit"
    Title="Price List by Vendor" %>

<%@ Register Src="~/UserControl/ViewHandler/ListPage2.ascx" TagName="ListPage2" TagPrefix="uc2" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Src="~/UserControl/ViewHandler/ListPageLookup.ascx" TagName="ListPageLookup" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <script type="text/javascript">
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
    <asp:UpdatePanel ID="UdPnHdDetail" runat="server">
        <ContentTemplate>
            <div>
                <table border="0" cellpadding="2" cellspacing="0" width="100%">
                    <tr style="background-color: #4D4D4D; height: 17px">
                        <td style="padding-left: 10px; width: 10px">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_PriceVendor_Nm" runat="server" Text="<%$ Resources:PC_PL_Vendor_VdEdit, lbl_PriceVendor_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
                        </td>
                        <td align="right" style="padding-right: 10px;">
                            <div>
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
                                            <ItemStyle Height="16px" Width="49px">
                                                <HoverStyle>
                                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png" Repeat="NoRepeat" VerticalPosition="center" />
                                                </HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/save.png" Repeat="NoRepeat" VerticalPosition="center" />
                                            </ItemStyle>
                                        </dx:MenuItem>
                                        <dx:MenuItem Name="Back" Text="">
                                            <ItemStyle Height="16px" Width="38px">
                                                <HoverStyle>
                                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png" Repeat="NoRepeat" VerticalPosition="center" />
                                                </HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat" VerticalPosition="center" />
                                            </ItemStyle>
                                        </dx:MenuItem>
                                    </Items>
                                    <Paddings Padding="0px" />
                                    <SeparatorPaddings Padding="0px" />
                                    <SubMenuStyle HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" Font-Size="9pt" ForeColor="#4D4D4D" />
                                    <Border BorderStyle="None"></Border>
                                </dx:ASPxMenu>
                            </div>
                        </td>
                    </tr>
                </table>
                <div>
                    <table border="0" cellpadding="2" cellspacing="0" width="100%">
                        <tr>
                            <td align="left" style="height: 17px; vertical-align: middle; width: 8.5%">
                                <asp:Label ID="lbl_Vendor_Nm" runat="server" Text="<%$ Resources:PC_PL_Vendor_VdEdit, lbl_Vendor_Nm %>" SkinID="LBL_HD"></asp:Label>
                                <asp:RequiredFieldValidator ID="RFV_ddlVendor" runat="server" ErrorMessage="*" ControlToValidate="ddl_Vendor" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                            <td style="width: 16.5%">
                                <dx:ASPxComboBox ID="ddl_Vendor" ClientInstanceName="ddl_Vendor" runat="server" DataSourceID="ods_Vendor" Width="100%" EnableCallbackMode="true" CallbackPageSize="100" IncrementalFilteringMode="Contains"
                                    ValueType="System.String" ValueField="VendorCode" TextField="Name" TextFormatString="{0} : {1}">
                                    <Columns>
                                        <dx:ListBoxColumn Caption="Code" FieldName="VendorCode" Width="100px" />
                                        <dx:ListBoxColumn Caption="Name" FieldName="Name" Width="300px" />
                                    </Columns>
                                    <ValidationSettings Display="Dynamic">
                                        <RequiredField IsRequired="True" />
                                    </ValidationSettings>
                                </dx:ASPxComboBox>
                                <asp:ObjectDataSource ID="ods_Vendor" runat="server" SelectMethod="GetList" TypeName="Blue.BL.AP.Vendor">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="hf_ConnStr" Name="connStr" PropertyName="Value" Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </td>
                            <td align="left" style="height: 17px; vertical-align: middle; width: 12.5%">
                                <asp:Label ID="lbl_DateFrom_Nm" runat="server" Text="<%$ Resources:PC_PL_Vendor_VdEdit, lbl_DateFrom_Nm %>" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td style="width: 12.5%">
                                <dx:ASPxDateEdit ID="txt_DateFrom" runat="server" Width="100%" DisplayFormatString="dd/MM/yyyy" EditFormatString="dd/MM/yyyy" EditFormat="Custom">
                                    <ValidationSettings Display="Dynamic">
                                    </ValidationSettings>
                                </dx:ASPxDateEdit>
                            </td>
                            <td align="left" style="height: 17px; vertical-align: middle; width: 12.5%">
                                <asp:Label ID="lbl_DateTo_Nm" runat="server" Text="<%$ Resources:PC_PL_Vendor_VdEdit, lbl_DateTo_Nm %>" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td style="width: 12.5%">
                                <dx:ASPxDateEdit ID="txt_DateTo" runat="server" Width="100%" DisplayFormatString="dd/MM/yyyy" EditFormat="Custom" EditFormatString="dd/MM/yyyy">
                                    <ValidationSettings Display="Dynamic">
                                    </ValidationSettings>
                                </dx:ASPxDateEdit>
                            </td>
                            <td align="left" style="height: 17px; vertical-align: middle; width: 12.5%">
                                <asp:Label ID="lbl_RefNo_Nm" runat="server" Text="<%$ Resources:PC_PL_Vendor_VdEdit, lbl_RefNo_Nm %>" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td style="width: 12.5%">
                                <asp:TextBox ID="txt_RefNo" runat="server" Width="90%" SkinID="TXT_V1"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <table border="0" cellpadding="2" cellspacing="0" width="100%">
                        <tr style="background-color: #4d4d4d" align="right">
                            <td style="padding-left: 10px;" align="left">
                                <asp:Label ID="lbl_PriceList_Nm" runat="server" Text="<%$ Resources:PC_PL_Vendor_VdEdit, lbl_PriceList_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
                            </td>
                            <td align="right" style="padding-right: 10px;">
                                <dx:ASPxMenu runat="server" ID="menu_CmdGrd" Font-Bold="True" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle"
                                    Height="16px" OnItemClick="menu_CmdGrd_ItemClick">
                                    <ItemStyle BackColor="Transparent">
                                        <HoverStyle BackColor="Transparent">
                                            <Border BorderStyle="None" />
                                        </HoverStyle>
                                        <Paddings Padding="2px" />
                                        <Border BorderStyle="None" />
                                    </ItemStyle>
                                    <Paddings Padding="0px" />
                                    <SeparatorPaddings Padding="0px" />
                                    <SubMenuItemStyle Font-Size="1em" ForeColor="Black" Height="34px">
                                        <HoverStyle BackColor="#20B9EB" ForeColor="White">
                                            <Border BorderStyle="None" />
                                        </HoverStyle>
                                        <Paddings PaddingLeft="10px" />
                                    </SubMenuItemStyle>
                                    <SubMenuStyle BackColor="WhiteSmoke" GutterWidth="0px">
                                        <Border BorderStyle="None" />
                                    </SubMenuStyle>
                                    <Items>
                                        <dx:MenuItem Name="Create" Text="">
                                            <ItemStyle Height="16px" Width="42px">
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
                </div>
                <div>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="table-layout: fixed">
                        <tr>
                            <td style="width: 100%">
                                <div style="overflow: auto; width: 100%;">
                                    <asp:GridView ID="grd_PLDt" runat="server" AutoGenerateColumns="False" EmptyDataText="No Data to Display" OnRowDataBound="grd_PLDt_RowDataBound" OnRowCancelingEdit="grd_PLDt_RowCancelingEdit"
                                        OnRowEditing="grd_PLDt_RowEditing" OnRowUpdating="grd_PLDt_RowUpdating" SkinID="GRD_V1" Width="100%" EnableModelValidation="True">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="Chk_All" runat="server" onclick="Check(this)" SkinID="CHK_V1" />
                                                </HeaderTemplate>
                                                <HeaderStyle Width="10px" />
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="Chk_Item" runat="server" SkinID="CHK_V1" />
                                                </ItemTemplate>
                                                <ItemStyle Width="10px" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:CommandField HeaderText="#" ShowEditButton="True">
                                                <HeaderStyle Font-Bold="True" Width="2%" />
                                                <ItemStyle Font-Bold="False" VerticalAlign="Top" Width="2%" HorizontalAlign="Center" />
                                            </asp:CommandField>
                                            <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_VdEdit, lbl_SKU_GRD_Nm %>" ControlStyle-Width="300px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_ProductCode" runat="server" SkinID="LBL_NR" Width="280px"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <dx:ASPxComboBox ID="ddl_ProductCode" runat="server" Width="280px" AutoPostBack="True" ValueField="ProductCode" ValueType="System.String" TextFormatString="{0} : {1} : {2}"
                                                        EnableCallbackMode="true" CallbackPageSize="50" IncrementalFilteringMode="Contains" OnLoad="ddl_ProductCode_Load" OnSelectedIndexChanged="ddl_ProductCode_SelectedIndexChanged">
                                                        <Columns>
                                                            <dx:ListBoxColumn Caption="Code" FieldName="ProductCode" />
                                                            <dx:ListBoxColumn Caption="Name" FieldName="ProductDesc1" />
                                                            <dx:ListBoxColumn Caption="Other Name" FieldName="ProductDesc2" />
                                                        </Columns>
                                                    </dx:ASPxComboBox>
                                                    <asp:HiddenField ID="hf_ProductCode" runat="server" />
                                                </EditItemTemplate>
                                                <ControlStyle Width="280px"></ControlStyle>
                                                <HeaderStyle Width="280px" HorizontalAlign="Left" />
                                                <ItemStyle Width="280px" HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_VdEdit, lbl_Unit_GRD_Nm %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_UnitCode" runat="server" SkinID="LBL_NR" Width="80"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <dx:ASPxComboBox ID="ddl_Unit" runat="server" AutoPostBack="true" Width="80" ValueType="System.String" ValueField="OrderUnit" TextField="OrderUnit" IncrementalFilteringMode="Contains"
                                                        OnLoad="ddl_Unit_Load">
                                                    </dx:ASPxComboBox>
                                                    <asp:HiddenField ID="hf_Unit" runat="server" />
                                                </EditItemTemplate>
                                                <ControlStyle Width="80px" />
                                                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                                                <ItemStyle Width="80px" HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_VdEdit, lbl_Rank_GRD_Nm %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_VendorRank" runat="server" SkinID="LBL_NR" Width="30px"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <dx:ASPxSpinEdit ID="txt_VendorRank" runat="server" Number="0.00" Width="50px" HorizontalAlign="Right">
                                                        <SpinButtons ShowIncrementButtons="False">
                                                        </SpinButtons>
                                                        <ValidationSettings Display="Dynamic">
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                    </dx:ASPxSpinEdit>
                                                </EditItemTemplate>
                                                <ControlStyle Width="50px" />
                                                <HeaderStyle Width="50px" HorizontalAlign="Right" />
                                                <ItemStyle Width="50px" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_VdEdit, lbl_QFrom_GRD_Nm %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_QtyFrom" runat="server" SkinID="LBL_NR" Width="50px"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <dx:ASPxSpinEdit ID="txt_QtyFrom" runat="server" Number="0.00" Width="50px" HorizontalAlign="Right">
                                                        <SpinButtons ShowIncrementButtons="False">
                                                        </SpinButtons>
                                                        <ValidationSettings Display="Dynamic">
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                    </dx:ASPxSpinEdit>
                                                </EditItemTemplate>
                                                <ControlStyle Width="50px" />
                                                <HeaderStyle Width="50px" HorizontalAlign="Right" />
                                                <ItemStyle Width="50px" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_VdEdit, lbl_To_GRD_Nm %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_QtyTo" runat="server" SkinID="LBL_NR" Width="50px"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <dx:ASPxSpinEdit ID="txt_QtyTo" runat="server" Number="0.00" Width="50px" HorizontalAlign="Right">
                                                        <SpinButtons ShowIncrementButtons="False">
                                                        </SpinButtons>
                                                        <ValidationSettings Display="Dynamic">
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                    </dx:ASPxSpinEdit>
                                                </EditItemTemplate>
                                                <ControlStyle Width="50px" />
                                                <HeaderStyle Width="50px" HorizontalAlign="Right" />
                                                <ItemStyle Width="50px" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_VdEdit, lbl_Price_GRD_Nm %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_QuotedPrice" runat="server" SkinID="LBL_NR" Width="50px"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <dx:ASPxSpinEdit ID="txt_QuotedPrice" runat="server" AutoPostBack="True" DisplayFormatString="#,###.##" Number="0.00" OnNumberChanged="txt_QuotedPrice_NumberChanged"
                                                        Width="50px" HorizontalAlign="Right">
                                                        <SpinButtons ShowIncrementButtons="False">
                                                        </SpinButtons>
                                                        <ValidationSettings Display="Dynamic">
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                    </dx:ASPxSpinEdit>
                                                </EditItemTemplate>
                                                <ControlStyle Width="50px" />
                                                <HeaderStyle Width="50px" HorizontalAlign="Right" />
                                                <ItemStyle Width="50px" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_VdEdit, lbl_MarPrice_GRD_Nm %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_MarketPrice" runat="server" SkinID="LBL_NR" Width="90px"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <dx:ASPxSpinEdit ID="txt_MarketPrice" runat="server" DisplayFormatString="#,###.##" Number="0.00" Width="50px" HorizontalAlign="Right">
                                                        <SpinButtons ShowIncrementButtons="False">
                                                        </SpinButtons>
                                                    </dx:ASPxSpinEdit>
                                                </EditItemTemplate>
                                                <ControlStyle Width="90px" />
                                                <HeaderStyle Width="90px" HorizontalAlign="Right" />
                                                <ItemStyle Width="90px" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_VdEdit, lbl_FOC_GRD_Nm %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_FOC" runat="server" SkinID="LBL_NR" Width="50px"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <dx:ASPxSpinEdit ID="txt_FOC" runat="server" HorizontalAlign="Right" Number="0.00" Width="50px">
                                                        <SpinButtons ShowIncrementButtons="False">
                                                        </SpinButtons>
                                                    </dx:ASPxSpinEdit>
                                                </EditItemTemplate>
                                                <ControlStyle Width="50px" />
                                                <HeaderStyle Width="50px" HorizontalAlign="Right" />
                                                <ItemStyle Width="50px" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_VdEdit, lbl_Comment_GRD_Nm %>">
                                                <ItemTemplate>
                                                    <table>
                                                        <tr>
                                                            <td align="left" style="width: 120px; white-space: nowrap; overflow: hidden">
                                                                <asp:Label ID="lbl_Comment" runat="server" SkinID="LBL_NR" Width="100px"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txt_Comment" runat="server" Width="180px" SkinID="TXT_V1"></asp:TextBox>
                                                </EditItemTemplate>
                                                <ControlStyle Width="180px" />
                                                <HeaderStyle Width="180px" HorizontalAlign="Left" />
                                                <ItemStyle Width="180px" HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_VdEdit, lbl_DiscPercent_GRD_Nm %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_DiscPercent" runat="server" SkinID="LBL_V1" Width="50px"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <dx:ASPxSpinEdit ID="txt_DiscPercent" runat="server" AutoPostBack="True" Number="0" Width="50px" HorizontalAlign="Right" OnNumberChanged="txt_DiscPercent_NumberChanged">
                                                        <SpinButtons ShowIncrementButtons="False">
                                                        </SpinButtons>
                                                    </dx:ASPxSpinEdit>
                                                </EditItemTemplate>
                                                <ControlStyle Width="100px" />
                                                <HeaderStyle Width="50px" HorizontalAlign="Right" />
                                                <ItemStyle Width="50px" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_VdEdit, lbl_Amount_GRD_Nm %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_DiscAmt" runat="server" SkinID="LBL_V1" Width="50px"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <dx:ASPxSpinEdit ID="txt_DiscAmt" runat="server" Enabled="true" AutoPostBack="true" HorizontalAlign="Right" OnNumberChanged="txt_DiscAmt_NumberChanged"
                                                        Number="0" Width="50px">
                                                        <SpinButtons ShowIncrementButtons="False">
                                                        </SpinButtons>
                                                    </dx:ASPxSpinEdit>
                                                </EditItemTemplate>
                                                <ControlStyle Width="50px" />
                                                <HeaderStyle Width="50px" HorizontalAlign="Right" />
                                                <ItemStyle Width="50px" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_VdEdit, lbl_TaxType_GRD_Nm %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_TaxType" runat="server" SkinID="LBL_V1" Width="80px"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <dx:ASPxComboBox ID="ddl_TaxType" runat="server" ValueType="System.String" AutoPostBack="true" OnSelectedIndexChanged="ddl_TaxType_SelectedIndexChanged"
                                                        Width="80px">
                                                        <Items>
                                                            <dx:ListEditItem Text="None" Value="N" />
                                                            <dx:ListEditItem Text="Add" Value="A" />
                                                            <dx:ListEditItem Text="Included" Value="I" />
                                                        </Items>
                                                    </dx:ASPxComboBox>
                                                </EditItemTemplate>
                                                <ControlStyle Width="80px" />
                                                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                                                <ItemStyle Width="80px" HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_VdEdit, lbl_Rate_GRD_Nm %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_TaxRate" runat="server" SkinID="LBL_V1" Width="50px"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <dx:ASPxSpinEdit ID="txt_TaxRate" runat="server" AutoPostBack="True" HorizontalAlign="Right" Number="0" OnNumberChanged="txt_TaxRate_NumberChanged" Width="50px">
                                                        <SpinButtons ShowIncrementButtons="False">
                                                        </SpinButtons>
                                                    </dx:ASPxSpinEdit>
                                                </EditItemTemplate>
                                                <ControlStyle Width="50px" />
                                                <HeaderStyle Width="50px" HorizontalAlign="Right" />
                                                <ItemStyle Width="50px" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_VdEdit, lbl_VendorSKU_GRD_Nm %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_VendorProdCode" runat="server" SkinID="LBL_V1" Width="80px"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txt_VendorProdCode" runat="server" Width="80px" SkinID="TXT_V1"></asp:TextBox>
                                                </EditItemTemplate>
                                                <ControlStyle Width="80px" />
                                                <HeaderStyle Width="80px" HorizontalAlign="Left" />
                                                <ItemStyle Width="80px" HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <%--/* Added on: 09/08/2017, By: Fon*/--%>
                                            <asp:TemplateField HeaderText="Currency">
                                                <ItemStyle Width="5%" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_CurrCode" runat="server" SkinID="LBL_NR" Width="70px"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <dx:ASPxComboBox ID="comb_CurrCode" runat="server" Width="70px" AutoPostBack="true" ValueField="CurrencyCode" ValueType="System.String" EnableCallbackMode="true"
                                                        CallbackPageSize="10" IncrementalFilteringMode="Contains" OnInit="comb_CurrCode_Init" OnSelectedIndexChanged="comb_CurrCode_SelectedIndexChanged">
                                                        <Columns>
                                                            <dx:ListBoxColumn Caption="Code" FieldName="CurrencyCode" />
                                                        </Columns>
                                                    </dx:ASPxComboBox>
                                                    <asp:RequiredFieldValidator ID="Req_CurrCode" runat="server" ErrorMessage="*" ControlToValidate="comb_CurrCode" Display="Dynamic"></asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <%--/* End Added*/--%>
                                            <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_VdEdit, lbl_PriceNEt_GRD_Nm %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_NetAmt" runat="server" SkinID="LBL_NR" Width="80px"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <dx:ASPxSpinEdit ID="txt_NetAmt" runat="server" DisplayFormatString="#,###.##" Enabled="False" Number="0" Width="80px">
                                                        <SpinButtons ShowIncrementButtons="False">
                                                        </SpinButtons>
                                                    </dx:ASPxSpinEdit>
                                                </EditItemTemplate>
                                                <ControlStyle Width="80px" />
                                                <HeaderStyle Width="80px" HorizontalAlign="Right" />
                                                <ItemStyle Width="80px" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_VdEdit, lbl_Average_GRD_Nm %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_AvgPrice" runat="server" SkinID="LBL_NR" Width="50px"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <dx:ASPxSpinEdit ID="txt_AvgPrice" runat="server" DisplayFormatString="#,###.##" Number="0" Width="50px">
                                                        <SpinButtons ShowIncrementButtons="False">
                                                        </SpinButtons>
                                                    </dx:ASPxSpinEdit>
                                                </EditItemTemplate>
                                                <ControlStyle Width="50px" />
                                                <HeaderStyle Width="50px" HorizontalAlign="Right" />
                                                <ItemStyle Width="50px" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_VdEdit, lbl_Last_GRD_Nm %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR" Width="50px"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <dx:ASPxSpinEdit ID="txt_LastPrice" runat="server" DisplayFormatString="#,###.##" Number="0" Width="50px">
                                                        <SpinButtons ShowIncrementButtons="False">
                                                        </SpinButtons>
                                                    </dx:ASPxSpinEdit>
                                                </EditItemTemplate>
                                                <ControlStyle Width="50px" />
                                                <HeaderStyle Width="50px" HorizontalAlign="Right" />
                                                <ItemStyle Width="50px" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:HiddenField ID="hf_ConnStr" runat="server" />
                <asp:HiddenField ID="hf_ID" runat="server" />
                <asp:HiddenField ID="hf_ProductCode" runat="server" />
                <dx:ASPxPopupControl ID="pop_ConfirmDelete" runat="server" ClientInstanceName="pop_ConfirmDelete" HeaderText="Warning" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" Modal="True" Width="250px" ShowCloseButton="False">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table width="100%">
                                <tr align="center">
                                    <td colspan="2" style="height: 17px">
                                        <asp:Label ID="lbl_ConfirmDelete_Nm" runat="server" Text="<%$ Resources:PC_PL_Vendor_VdEdit, lbl_ConfirmDelete_Nm %>" SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td>
                                        <asp:Button ID="btn_OK" runat="server" OnClick="btn_OK_Click" Text="<%$ Resources:PC_PL_Vendor_VdEdit, btn_OK %>" SkinID="BTN_V1" Width="60px" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_Cancel" runat="server" OnClick="btn_Cancel_Click" Text="Cancel" SkinID="BTN_V1" Width="60px" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="pop_AlertTaxRate" runat="server" HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                    ShowCloseButton="False" Width="250px">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" style="height: 17px" colspan="2">
                                        <asp:Label ID="lbl_TaxRAteThan_Nm" runat="server" Text="<%$ Resources:PC_PL_Vendor_VdEdit, lbl_TaxRAteThan_Nm %>" SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td>
                                        <asp:Button ID="btn_OK_Tax" runat="server" Text="<%$ Resources:PC_PL_Vendor_VdEdit, btn_OK_Tax %>" OnClick="btn_OK_Tax_Click" Width="60px" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <asp:ObjectDataSource ID="ods_Product" runat="server" SelectMethod="GetList" TypeName="Blue.BL.Option.Inventory.Product">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hf_ConnStr" Name="ConnStr" PropertyName="Value" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:ObjectDataSource ID="ods_Unit" runat="server" SelectMethod="GetList" TypeName="Blue.BL.Option.Inventory.Unit">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hf_ConnStr" Name="connStr" PropertyName="Value" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender4" runat="server" TargetControlID="UpPgHdDetail" PopupControlID="UpPgHdDetail" BackgroundCssClass="POPUP_BG"
                RepositionMode="RepositionOnWindowResizeAndScroll">
            </ajaxToolkit:ModalPopupExtender>
            <%--<asp:Button ID="btnDialog" runat="server" Style="display: none;" />
            <uc1:Dialog ID="dialog" runat="server" />--%>
            <asp:UpdateProgress ID="UpPgHdDetail" runat="server" AssociatedUpdatePanelID="UdPnHdDetail">
                <ProgressTemplate>
                    <div class="fix-layout" style="border-style: solid; border-width: 1px; border-color: #0071BD; background-color: #FFFFFF; width: 120px; height: 60px">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 120px; height: 60px">
                            <tr>
                                <td align="center">
                                    <asp:Image ID="img_Loading4" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/ajax-loader.gif" EnableViewState="False" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_Loading4" runat="server" Font-Bold="true" Text="Loading..." EnableViewState="False"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="menu_CmdBar" EventName="ItemClick" />
            <asp:AsyncPostBackTrigger ControlID="menu_CmdGrd" EventName="ItemClick" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
