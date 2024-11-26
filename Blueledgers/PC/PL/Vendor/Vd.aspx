<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="Vd.aspx.cs" Inherits="BlueLedger.PL.PC.PL.Vendor.Vd"
    Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<asp:Content ID="Content" ContentPlaceHolderID="cph_Main" runat="Server">
    <style type="text/css">
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
    <table border="0" cellpadding="1" cellspacing="0" width="100%">
        <tr>
            <td align="left">
                <table border="0" cellpadding="2" cellspacing="0" width="100%">
                    <tr style="background-color: #4d4d4d; height: 17px; padding-left: 10px">
                        <td style="padding-left: 10px; width: 10px">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_PriceByVendor_Nm" runat="server" Text="<%$ Resources:PC_PL_Vendor_Vd, lbl_PriceByVendor_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
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
                                    <dx:MenuItem Name="Export" Text="Export">
                                        <ItemStyle Height="16px" Width="49px" ForeColor="White">
                                        </ItemStyle>
                                    </dx:MenuItem>
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
                                        <ItemStyle Height="16px" Width="43px">
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
                    <table border="0" cellpadding="2" cellspacing="0" width="100%">
                        <tr align="left" style="height: 17px;">
                            <td width="5%" style="padding-left: 10px; height: 17px">
                                <asp:Label ID="lbl_Vendor_Nm" runat="server" Text="<%$ Resources:PC_PL_Vendor_Vd, lbl_Vendor_Nm %>" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td width="10%" style="height: 17px">
                                <asp:Label ID="lbl_Vendor" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td width="5%" style="height: 17px">
                                <asp:Label ID="lbl_VendorName_Nm" runat="server" Text="<%$ Resources:PC_PL_Vendor_Vd, lbl_VendorName_Nm %>" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td width="20%" style="height: 17px">
                                <asp:Label ID="lbl_VendorName" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td width="10%" style="height: 17px">
                                <asp:Label ID="lbl_DateFrom_Nm" runat="server" Text="<%$ Resources:PC_PL_Vendor_Vd, lbl_DateFrom_Nm %>" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td width="10%" style="height: 17px">
                                <asp:Label ID="lbl_DateFrom" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td width="10%" style="height: 17px">
                                <asp:Label ID="lbl_DateTo_Nm" runat="server" Text="<%$ Resources:PC_PL_Vendor_Vd, lbl_DateTo_Nm %>" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td width="10%" style="height: 17px">
                                <asp:Label ID="lbl_DateTo" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td width="10%" style="height: 17px">
                                <asp:Label ID="lbl_RefNo_Nm" runat="server" Text="<%$ Resources:PC_PL_Vendor_Vd, lbl_RefNo_Nm %>" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td width="10%" style="height: 17px">
                                <asp:Label ID="lbl_RefNo" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="table-layout: fixed">
                        <tr>
                            <td style="width: 100%">
                                <div style="overflow: auto; width: 100%;">
                                    <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="grd_PLDt" runat="server" AutoGenerateColumns="False" EnableModelValidation="True" SkinID="GRD_V1" Width="100%" OnLoad="grd_PLDt_Load"
                                                    OnRowDataBound="grd_PLDt_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_Vd, lbl_SKU_GRD_Nm %>">
                                                            <ItemTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td style="width: 80px; white-space: nowrap; overflow: hidden">
                                                                            <asp:Label ID="lbl_ProductCode" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_Vd, lbl_Desc_GRD_Nm %>">
                                                            <ItemTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td style="width: 200px; white-space: nowrap; overflow: hidden">
                                                                            <asp:Label ID="lbl_ProductName" runat="server" SkinID="LBL_NR" Width="190px"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="200px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_Vd, lbl_Unit_GRD_Nm %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_Unit" runat="server" SkinID="LBL_NR"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="50px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_Vd, lbl_Rank_GRD_Nm %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_Rank" runat="server" SkinID="LBL_NR" Width="25px"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Right" Width="25px" />
                                                            <ItemStyle HorizontalAlign="Right" Width="25px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_Vd, lbl_QFrom_GRD_Nm %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_QtyFrom" runat="server" SkinID="LBL_NR" Width="70px"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Right" Width="70px" />
                                                            <ItemStyle HorizontalAlign="Right" Width="70px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_Vd, lbl_To_GRD_Nm %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_To" runat="server" SkinID="LBL_NR" Width="70px"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Right" Width="70px" />
                                                            <ItemStyle HorizontalAlign="Right" Width="70px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_Vd, lbl_Price_GRD_Nm %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_Price" runat="server" SkinID="LBL_NR" Width="50px"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Right" Width="50px" />
                                                            <ItemStyle HorizontalAlign="Right" Width="50px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Currency">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_CurrCode" runat="server" SkinID="LBL_NR" Width="70px"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_Vd, lbl_MarPrice_GRD_Nm %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_MarketPrice" runat="server" SkinID="LBL_NR" Width="80px"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Right" Width="100px" />
                                                            <ItemStyle HorizontalAlign="Right" Width="100px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_Vd, lbl_FOC_GRD_Nm %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_Foc" runat="server" SkinID="LBL_NR"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Right" Width="25px" />
                                                            <ItemStyle HorizontalAlign="Right" Width="25px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_Vd, lbl_Comment_GRD_Nm %>">
                                                            <ItemTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td style="width: 150px; white-space: nowrap; overflow: hidden">
                                                                            <asp:Label ID="lbl_Comment" runat="server" SkinID="LBL_NR" Width="130px"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_Vd, lbl_DiscPercent_GRD_Nm %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_Disc" runat="server" SkinID="LBL_NR" Width="75px"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_Vd, lbl_Amount_GRD_Nm %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_DiscAmt" runat="server" SkinID="LBL_NR" Width="50px"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Right" Width="50px" />
                                                            <ItemStyle HorizontalAlign="Right" Width="50px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_Vd, lbl_TaxType_GRD_Nm %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_TaxType" runat="server" SkinID="LBL_NR" Width="75px"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="75px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_Vd, lbl_Rate_GRD_Nm %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_TaxRate" runat="server" SkinID="LBL_NR" Width="20px"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Right" Width="20px" />
                                                            <ItemStyle HorizontalAlign="Right" Width="20px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_Vd, lbl_VendorSKU_GRD_Nm %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_VendorSKU" runat="server" SkinID="LBL_NR" Width="80px"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_Vd, lbl_PriceNEt_GRD_Nm %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_PriceNet" runat="server" SkinID="LBL_NR" Width="60px"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                                            <ItemStyle HorizontalAlign="Right" Width="60px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_Vd, lbl_Average_GRD_Nm %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_Avg" runat="server" SkinID="LBL_NR" Width="60px"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                                            <ItemStyle HorizontalAlign="Right" Width="60px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:PC_PL_Vendor_Vd, lbl_Last_GRD_Nm %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_Last" runat="server" SkinID="LBL_NR" Width="60px"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Right" Width="60px" />
                                                            <ItemStyle HorizontalAlign="Right" Width="60px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <% /*
                <dx:ASPxPopupControl ID="pop_PLDtEdit" runat="server" ClientInstanceName="pop_PLDtEdit"
                    CloseAction="CloseButton" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
                    HeaderText="Price List Detail" Modal="True" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                    Width="800px" ShowCloseButton="False">
                    <ContentStyle VerticalAlign="Top">
                        <Paddings Padding="1px" />
                    </ContentStyle>
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                            <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                <tr>
                                    <td align="right">
                                        <table border="0" cellpadding="1" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <dx:ASPxButton ID="btn_Update_PLDt" runat="server" Text="Update" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                        CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="80px"
                                                        OnClick="btn_Update_PLDt_Click">
                                                    </dx:ASPxButton>
                                                </td>
                                                <td>
                                                    <dx:ASPxButton ID="btn_Cancel_PLDt" runat="server" Text="Cancel" CausesValidation="False"
                                                        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                        Width="80px" OnClick="btn_Cancel_PLDt_Click">
                                                        <ClientSideEvents Click="function(s, e) {
	                                                        pop_PLDtEdit.Hide();
                                                        }" />
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxPageControl ID="pc_PLDt" runat="server" ActiveTabIndex="0" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                            CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" TabSpacing="3px"
                                            Width="100%">
                                            <TabPages>
                                                <dx:TabPage Text="Product">
                                                    <ContentCollection>
                                                        <dx:ContentControl ID="ContentControl1" runat="server">
                                                            <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td width="5%">
                                                                    </td>
                                                                    <td width="15%">
                                                                        <asp:Label ID="Label16" runat="server" Font-Bold="True" Text="SKU#"></asp:Label>
                                                                    </td>
                                                                    <td width="30%">
                                                                        <dx:ASPxComboBox ID="ddl_Product" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                                            CssPostfix="Aqua" DataSourceID="ods_Product" LoadingPanelImagePosition="Top"
                                                                            ShowShadow="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" TextFormatString="{0} - {1}"
                                                                            ValueField="ProductCode" ValueType="System.String" Width="200px" IncrementalFilteringMode="Contains"
                                                                            AutoPostBack="True" OnSelectedIndexChanged="ddl_Product_SelectedIndexChanged">
                                                                            <Columns>
                                                                                <dx:ListBoxColumn Caption="Code" FieldName="ProductCode" Width="100px" />
                                                                                <dx:ListBoxColumn Caption="Name" FieldName="ProductDesc1" Width="200px" />
                                                                                <dx:ListBoxColumn Caption="Other Name" FieldName="ProductDesc2" Width="200px" />
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
                                                                    </td>
                                                                    <td width="5%">
                                                                    </td>
                                                                    <td width="15%">
                                                                        <asp:Label ID="Label21" runat="server" Font-Bold="True" Text="Unit"></asp:Label>
                                                                    </td>
                                                                    <td width="30%">
                                                                        <dx:ASPxComboBox ID="ddl_OrderUnit" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                                            CssPostfix="Aqua" DataSourceID="ods_Unit" Enabled="False" LoadingPanelImagePosition="Top"
                                                                            ShowShadow="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" ValueType="System.String"
                                                                            Width="100px">
                                                                            <Columns>
                                                                                <dx:ListBoxColumn Caption="Code" FieldName="UnitCode" />
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
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label22" runat="server" Font-Bold="True" Text="Quantity From"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxSpinEdit ID="txt_QtyFrom" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                                            CssPostfix="Aqua" HorizontalAlign="Right" Number="0" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                                            Width="100px">
                                                                            <SpinButtons ShowIncrementButtons="False">
                                                                                <IncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua" />
                                                                                </IncrementImage>
                                                                                <DecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua" />
                                                                                </DecrementImage>
                                                                                <LargeIncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua" />
                                                                                </LargeIncrementImage>
                                                                                <LargeDecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua" />
                                                                                </LargeDecrementImage>
                                                                            </SpinButtons>
                                                                            <ValidationSettings Display="Dynamic">
                                                                                <RequiredField IsRequired="True" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxSpinEdit>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label23" runat="server" Font-Bold="True" Text="Quantity To"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxSpinEdit ID="txt_QtyTo" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                                            CssPostfix="Aqua" HorizontalAlign="Right" Number="0" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                                            Width="100px">
                                                                            <SpinButtons ShowIncrementButtons="False">
                                                                                <IncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua" />
                                                                                </IncrementImage>
                                                                                <DecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua" />
                                                                                </DecrementImage>
                                                                                <LargeIncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua" />
                                                                                </LargeIncrementImage>
                                                                                <LargeDecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua" />
                                                                                </LargeDecrementImage>
                                                                            </SpinButtons>
                                                                            <ValidationSettings Display="Dynamic">
                                                                                <RequiredField IsRequired="True" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxSpinEdit>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label24" runat="server" Font-Bold="True" Text="Vendor Rank"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxSpinEdit ID="txt_VendorRank" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                                            CssPostfix="Aqua" HorizontalAlign="Right" Number="0" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                                            Width="100px">
                                                                            <SpinButtons ShowIncrementButtons="False">
                                                                                <IncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua" />
                                                                                </IncrementImage>
                                                                                <DecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua" />
                                                                                </DecrementImage>
                                                                                <LargeIncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua" />
                                                                                </LargeIncrementImage>
                                                                                <LargeDecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua" />
                                                                                </LargeDecrementImage>
                                                                            </SpinButtons>
                                                                            <ValidationSettings Display="Dynamic">
                                                                                <RequiredField IsRequired="True" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxSpinEdit>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label25" runat="server" Font-Bold="True" Text="Quoted Price"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxSpinEdit ID="txt_QuotedPrice" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                                            CssPostfix="Aqua" HorizontalAlign="Right" Number="0" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                                            Width="100px" DisplayFormatString="#,###.##" AutoPostBack="True" OnNumberChanged="txt_QuotedPrice_NumberChanged">
                                                                            <SpinButtons ShowIncrementButtons="False">
                                                                                <IncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua" />
                                                                                </IncrementImage>
                                                                                <DecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua" />
                                                                                </DecrementImage>
                                                                                <LargeIncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua" />
                                                                                </LargeIncrementImage>
                                                                                <LargeDecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua" />
                                                                                </LargeDecrementImage>
                                                                            </SpinButtons>
                                                                            <ValidationSettings Display="Dynamic">
                                                                                <RequiredField IsRequired="True" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxSpinEdit>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label26" runat="server" Font-Bold="True" Text="Market Price"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxSpinEdit ID="txt_MarketPrice" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                                            CssPostfix="Aqua" DisplayFormatString="#,###.##" HorizontalAlign="Right" Number="0"
                                                                            SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="100px">
                                                                            <SpinButtons ShowIncrementButtons="False">
                                                                                <IncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua" />
                                                                                </IncrementImage>
                                                                                <DecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua" />
                                                                                </DecrementImage>
                                                                                <LargeIncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua" />
                                                                                </LargeIncrementImage>
                                                                                <LargeDecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua" />
                                                                                </LargeDecrementImage>
                                                                            </SpinButtons>
                                                                        </dx:ASPxSpinEdit>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label27" runat="server" Font-Bold="True" Text="FOC"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxSpinEdit ID="txt_FOC" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                                            CssPostfix="Aqua" HorizontalAlign="Right" Number="0" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                                            Width="100px">
                                                                            <SpinButtons ShowIncrementButtons="False">
                                                                                <IncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua" />
                                                                                </IncrementImage>
                                                                                <DecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua" />
                                                                                </DecrementImage>
                                                                                <LargeIncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua" />
                                                                                </LargeIncrementImage>
                                                                                <LargeDecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua" />
                                                                                </LargeDecrementImage>
                                                                            </SpinButtons>
                                                                        </dx:ASPxSpinEdit>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                    </td>
                                                                    <td valign="top">
                                                                        <asp:Label ID="Label17" runat="server" Font-Bold="True" Text="Comment"></asp:Label>
                                                                    </td>
                                                                    <td colspan="4">
                                                                        <dx:ASPxMemo ID="txt_Comment" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                                            CssPostfix="Aqua" Height="50px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                                            Width="100%">
                                                                            <ValidationSettings>
                                                                                <ErrorFrameStyle ImageSpacing="4px">
                                                                                    <ErrorTextPaddings PaddingLeft="4px" />
                                                                                </ErrorFrameStyle>
                                                                            </ValidationSettings>
                                                                        </dx:ASPxMemo>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </dx:ContentControl>
                                                    </ContentCollection>
                                                </dx:TabPage>
                                                <dx:TabPage Text="Discount & Tax">
                                                    <ContentCollection>
                                                        <dx:ContentControl ID="ContentControl2" runat="server">
                                                            <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td width="5%">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td width="20%">
                                                                        <asp:Label ID="Label13" runat="server" Font-Bold="True" Text="Discount(%)"></asp:Label>
                                                                    </td>
                                                                    <td width="30%">
                                                                        <dx:ASPxSpinEdit ID="txt_DiscPercent" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                                            CssPostfix="Aqua" HorizontalAlign="Right" Number="0" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                                            Width="100px" OnNumberChanged="txt_DiscPercent_NumberChanged" AutoPostBack="True">
                                                                            <SpinButtons ShowIncrementButtons="False">
                                                                                <IncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua" />
                                                                                </IncrementImage>
                                                                                <DecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua" />
                                                                                </DecrementImage>
                                                                                <LargeIncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua" />
                                                                                </LargeIncrementImage>
                                                                                <LargeDecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua" />
                                                                                </LargeDecrementImage>
                                                                            </SpinButtons>
                                                                        </dx:ASPxSpinEdit>
                                                                    </td>
                                                                    <td width="5%">
                                                                    </td>
                                                                    <td width="20%">
                                                                        <asp:Label ID="Label28" runat="server" Font-Bold="True" Text="Discount Amount"></asp:Label>
                                                                    </td>
                                                                    <td width="35%">
                                                                        <dx:ASPxSpinEdit ID="txt_DiscAmt" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                                            CssPostfix="Aqua" HorizontalAlign="Right" Number="0" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                                            Width="100px" Enabled="False">
                                                                            <SpinButtons ShowIncrementButtons="False">
                                                                                <IncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua" />
                                                                                </IncrementImage>
                                                                                <DecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua" />
                                                                                </DecrementImage>
                                                                                <LargeIncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua" />
                                                                                </LargeIncrementImage>
                                                                                <LargeDecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua" />
                                                                                </LargeDecrementImage>
                                                                            </SpinButtons>
                                                                        </dx:ASPxSpinEdit>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label29" runat="server" Font-Bold="True" Text="Tax Type"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxComboBox ID="ddl_TaxType" runat="server" ValueType="System.String" Width="100px"
                                                                            CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" LoadingPanelImagePosition="Top"
                                                                            ShowShadow="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" AutoPostBack="True"
                                                                            OnSelectedIndexChanged="ddl_TaxType_SelectedIndexChanged">
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
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label30" runat="server" Font-Bold="True" Text="Tax Rate"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxSpinEdit ID="txt_TaxRate" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                                            CssPostfix="Aqua" HorizontalAlign="Right" Number="0" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                                            Width="100px" AutoPostBack="True" OnNumberChanged="txt_TaxRate_NumberChanged">
                                                                            <SpinButtons ShowIncrementButtons="False">
                                                                                <IncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua" />
                                                                                </IncrementImage>
                                                                                <DecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua" />
                                                                                </DecrementImage>
                                                                                <LargeIncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua" />
                                                                                </LargeIncrementImage>
                                                                                <LargeDecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua" />
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
                                                                        <asp:Label ID="Label31" runat="server" Font-Bold="True" Text="Vendor SKU#"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxTextBox ID="txt_VendorProdCode" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                                            CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="100px">
                                                                            <ValidationSettings>
                                                                                <ErrorFrameStyle ImageSpacing="4px">
                                                                                    <ErrorTextPaddings PaddingLeft="4px" />
                                                                                </ErrorFrameStyle>
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label32" runat="server" Font-Bold="True" Text="Average Price"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxSpinEdit ID="txt_AvgPrice" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                                            CssPostfix="Aqua" DisplayFormatString="#,###.##" HorizontalAlign="Right" Number="0"
                                                                            SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="100px">
                                                                            <SpinButtons ShowIncrementButtons="False">
                                                                                <IncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua" />
                                                                                </IncrementImage>
                                                                                <DecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua" />
                                                                                </DecrementImage>
                                                                                <LargeIncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua" />
                                                                                </LargeIncrementImage>
                                                                                <LargeDecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua" />
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
                                                                        <asp:Label ID="Label33" runat="server" Font-Bold="True" Text="Net  Amount"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxSpinEdit ID="txt_NetAmt" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                                            CssPostfix="Aqua" DisplayFormatString="#,###.##" HorizontalAlign="Right" Number="0"
                                                                            SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="100px" Enabled="False">
                                                                            <SpinButtons ShowIncrementButtons="False">
                                                                                <IncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua" />
                                                                                </IncrementImage>
                                                                                <DecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua" />
                                                                                </DecrementImage>
                                                                                <LargeIncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua" />
                                                                                </LargeIncrementImage>
                                                                                <LargeDecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua" />
                                                                                </LargeDecrementImage>
                                                                            </SpinButtons>
                                                                        </dx:ASPxSpinEdit>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label34" runat="server" Font-Bold="True" Text="Last Price"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <dx:ASPxSpinEdit ID="txt_LastPrice" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                                            CssPostfix="Aqua" DisplayFormatString="#,###.##" HorizontalAlign="Right" Number="0"
                                                                            SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="100px">
                                                                            <SpinButtons ShowIncrementButtons="False">
                                                                                <IncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua" />
                                                                                </IncrementImage>
                                                                                <DecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua" />
                                                                                </DecrementImage>
                                                                                <LargeIncrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua" />
                                                                                </LargeIncrementImage>
                                                                                <LargeDecrementImage>
                                                                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua"
                                                                                        PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua" />
                                                                                </LargeDecrementImage>
                                                                            </SpinButtons>
                                                                        </dx:ASPxSpinEdit>
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
                <dx:ASPxPopupControl ID="pop_ConfirmDelete" runat="server" ClientInstanceName="pop_ConfirmDelete"
                    CloseAction="CloseButton" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
                    HeaderText="Warning" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                    SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                    <ContentStyle HorizontalAlign="Center" VerticalAlign="Top">
                    </ContentStyle>
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <asp:Label ID="Label35" runat="server" Text="Confirm Delete?"></asp:Label>
                            <br />
                            <br />
                            <dx:ASPxButton ID="btn_OK" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                CssPostfix="Aqua" OnClick="btn_OK_Click" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                Text="OK">
                            </dx:ASPxButton>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="pop_ConfirmDeletePL" runat="server" ClientInstanceName="pop_ConfirmDeletePL"
                    CloseAction="CloseButton" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua"
                    HeaderText="Warning" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                    SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                    <ContentStyle HorizontalAlign="Center" VerticalAlign="Top">
                    </ContentStyle>
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <asp:Label ID="Label36" runat="server" Text="Confirm Delete?"></asp:Label>
                            <br />
                            <br />
                            <dx:ASPxButton ID="btn_OK_Delete_PR" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                CssPostfix="Aqua" OnClick="btn_OK_Delete_PR_Click" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                Text="OK">
                            </dx:ASPxButton>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                */ %>
                <asp:HiddenField ID="hf_ID" runat="server" />
                <asp:HiddenField ID="hf_ConnStr" runat="server" />
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
                <dx:ASPxPopupControl ID="pop_ConfirmDeletePL" runat="server" Width="250px" CloseAction="CloseButton" HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" colspan="2" height="50px">
                                        <asp:Label ID="lbl_ConfirmDelete_Nm" runat="server" Text="<%$ Resources:PC_PL_Vendor_Vd, lbl_ConfirmDelete_Nm %>" SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <%--<dx:ASPxButton ID="btn_ConfirmDeletePL" runat="server" Text="Yes" Width="60px" OnClick="btn_ConfirmDeletePL_Click"
                                            SkinID="BTN_V1">
                                        </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_ConfirmDeletePL" runat="server" Text="<%$ Resources:PC_PL_Vendor_Vd, btn_ConfirmDeletePL %>" Width="60px" OnClick="btn_ConfirmDeletePL_Click"
                                            SkinID="BTN_V1" />
                                    </td>
                                    <td align="left">
                                        <%--<dx:ASPxButton ID="btn_CancelDeletePL" runat="server" Text="No" Width="60px" OnClick="btn_CancelDeletePL_Click"
                                            SkinID="BTN_V1">
                                        </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_CancelDeletePL" runat="server" Text="<%$ Resources:PC_PL_Vendor_Vd, btn_CancelDeletePL %>" Width="60px" OnClick="btn_CancelDeletePL_Click"
                                            SkinID="BTN_V1" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="pop_CannotDelete" runat="server" Width="250px" CloseAction="CloseButton" HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" colspan="2" height="50px">
                                        <asp:Label ID="lbl_CannotDelete_Nm" runat="server" Text="<%$ Resources:PC_PL_Vendor_Vd, lbl_CannotDelete_Nm %>" SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <%--<dx:ASPxButton ID="btn_YES" runat="server" Text="Yes" Width="60px" OnClick="btn_YES_Click"
                                            SkinID="BTN_V1">
                                        </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_YES" runat="server" Text="<%$ Resources:PC_PL_Vendor_Vd, btn_YES %>" Width="60px" OnClick="btn_YES_Click" SkinID="BTN_V1" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
            </td>
        </tr>
    </table>
</asp:Content>
