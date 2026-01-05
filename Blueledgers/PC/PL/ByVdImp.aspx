<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="ByVdImp.aspx.cs" Inherits="BlueLedger.PL.PC.PL.ByVdImp" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr style="background-color: #4D4D4D; height: 17px">
            <td style="padding-left: 10px; width: 10px">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
            </td>
            <td align="left">
                <asp:Label ID="lbl_ImportPrice_Nm" runat="server" Text="<%$ Resources:PC_PL_ByVdImp, lbl_ImportPrice_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
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
            </td>
        </tr>
    </table>
    <!-- Header -->
    <table border="0" cellpadding="2" cellspacing="0" width="100%">
        <tr>
            <!--Vendor-->
            <td>
                <asp:Label ID="lbl_Vendor_Nm" runat="server" Text="<%$ Resources:PC_PL_ByVdImp, lbl_Vendor_Nm %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td colspan="3">
             <dx:ASPxComboBox ID="ddl_Vendor" runat="server" Width="100%" IncrementalFilteringMode="Contains" EnableCallbackMode="true" CallbackPageSize="25">
                    <ValidationSettings Display="Dynamic">
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxComboBox>
               <%-- <dx:ASPxComboBox ID="ddl_Vendor" runat="server" Width="100%" ValueType="System.String" DataSourceID="ods_Vendor" IncrementalFilteringMode="Contains" ValueField="VendorCode"
                    TextField="Name" TextFormatString="{0} : {1}" EnableCallbackMode="true" CallbackPageSize="30">
                    <Columns>
                        <dx:ListBoxColumn Caption="Code" FieldName="VendorCode" Width="100px" />
                        <dx:ListBoxColumn Caption="Name" FieldName="Name" Width="300px" />
                    </Columns>
                    <ValidationSettings Display="Dynamic">
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxComboBox>
                <asp:ObjectDataSource ID="ods_Vendor" runat="server" SelectMethod="GetList" TypeName="Blue.BL.AP.Vendor" OldValuesParameterFormatString="original_{0}">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hf_ConnStr" Name="connStr" PropertyName="Value" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>--%>
            </td>
            <!--Currency-->
            <td>
                <asp:Label ID="lbl_Curr" runat="server" SkinID="LBL_HD">Currency:</asp:Label>
            </td>
            <td>
                <dx:ASPxComboBox ID="ddl_CurrCode" runat="server" Width="100%" AutoPostBack="true">
                </dx:ASPxComboBox>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <!--Date from-->
            <td>
                <asp:Label ID="lbl_DateFrom_Nm" runat="server" Text="<%$ Resources:PC_PL_ByVdImp, lbl_DateFrom_Nm %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td>
                <dx:ASPxDateEdit ID="txt_DateFrom" runat="server" CssFilePath="" CssPostfix="" ShowShadow="False" SpriteCssFilePath="" Width="120px" DisplayFormatString="dd/MM/yyyy"
                    EditFormatString="dd/MM/yyyy">
                    <ValidationSettings Display="Dynamic">
                        <ErrorFrameStyle ImageSpacing="4px">
                            <ErrorTextPaddings PaddingLeft="4px" />
                        </ErrorFrameStyle>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                    <DropDownButton>
                        <Image>
                            <SpriteProperties HottrackedCssClass="" PressedCssClass="" />
                        </Image>
                    </DropDownButton>
                    <CalendarProperties>
                        <HeaderStyle Spacing="1px" />
                        <FooterStyle Spacing="17px" />
                    </CalendarProperties>
                </dx:ASPxDateEdit>
            </td>
            <!--Date to-->
            <td>
                <asp:Label ID="lbl_DateTo_Nm" runat="server" Text="<%$ Resources:PC_PL_ByVdImp, lbl_DateTo_Nm %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td>
                <dx:ASPxDateEdit ID="txt_DateTo" runat="server" CssFilePath="" CssPostfix="" ShowShadow="False" SpriteCssFilePath="" Width="120px" DisplayFormatString="dd/MM/yyyy"
                    EditFormat="Custom" EditFormatString="dd/MM/yyyy">
                    <ValidationSettings Display="Dynamic">
                        <ErrorFrameStyle ImageSpacing="4px">
                            <ErrorTextPaddings PaddingLeft="4px" />
                        </ErrorFrameStyle>
                    </ValidationSettings>
                    <DropDownButton>
                        <Image>
                            <SpriteProperties HottrackedCssClass="" PressedCssClass="" />
                        </Image>
                    </DropDownButton>
                    <CalendarProperties>
                        <HeaderStyle Spacing="1px" />
                        <FooterStyle Spacing="17px" />
                    </CalendarProperties>
                </dx:ASPxDateEdit>
            </td>
            <!--RefNo-->
            <td style="width: 10%">
                <asp:Label ID="lbl_Refer_Nm" runat="server" Text="<%$ Resources:PC_PL_ByVdImp, lbl_Refer_Nm %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td style="width: 10%">
                <asp:TextBox ID="txt_RefNo" runat="server" Width="200px" SkinID="TXT_V1"></asp:TextBox>
            </td>
            <!-- Vendor Rank -->
            <td>
                <asp:Label ID="lbl_VRank_Nm" runat="server" Text="Ranking (1-5)" SkinID="LBL_HD" />
            </td>
            <td>
                <dx:ASPxSpinEdit ID="txt_Vrank" runat="server" SkinID="TXT_V1" MinValue="1" MaxValue="5" NullText="1" Number="1" Width="60" />
            </td>
        </tr>
    </table>
    <!-- Import File -->
    <div style="padding: 10px; margin-top: 5px; margin-bottom: 5px; background-color: WhiteSmoke;">
        <div style="margin-bottom: 5px;">
            <asp:Label ID="lbl_ImportFile_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:PC_PL_ByVdImp, lbl_ImportFile_Nm %>"></asp:Label>
        </div>
        <div style="margin-bottom: 5px; display: flex;">
            <div>
                <asp:FileUpload ID="FileUpload1" runat="server" SkinID="BTN_V1" Width="360px" />
            </div>
            <div style="margin-left: 5px;">
                <asp:Button ID="btn_Upload" runat="server" Width="100" Text="Upload" OnClick="btn_UploadFile_Click" />
            </div>
        </div>
        <div style="margin-bottom: 5px;">
            <asp:Label ID="lbl_Upload_Message" runat="server" SkinID="LBL_NR"></asp:Label>
        </div>
    </div>
    <!-- Details -->
    <br />
    <asp:Label runat="server" ID="lbl_Test" Font-Size="Large" />
    <table width="100%" cellpadding="0" cellspacing="0" border="0" style="table-layout: fixed">
        <tr>
            <td style="width: 100%">
                <div style="overflow: auto; width: 100%;">
                    <asp:GridView ID="GridView1" runat="server" EnableModelValidation="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                        <HeaderStyle CssClass="grdHeaderRow_V1" />
                        <RowStyle CssClass="grdDataRow_V1" Width="50px" Wrap="True" />
                        <AlternatingRowStyle CssClass="grdAlternatingRow_V1" />
                        <EmptyDataRowStyle CssClass="grdHeaderRow_V1" HorizontalAlign="Center" />
                        <FooterStyle CssClass="grdFooterRow_V1" HorizontalAlign="right" />
                        <PagerStyle CssClass="grdPager_V1" HorizontalAlign="Right" />
                    </asp:GridView>
                </div>
            </td>
        </tr>
    </table>
    <!-- Hidden -->
    <asp:HiddenField runat="server" ID="hf_PriceListNo" />
    <!-- PopUp -->
    <dx:ASPxPopupControl runat="server" ID="pop_Alert" ClientInstanceName="pop_Alert" Style="z-index: 99999 !important;" HeaderText="Alert" Width="300px" Modal="True"
        CloseAction="CloseButton" ShowCloseButton="true" ShowPageScrollbarWhenModal="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl7" runat="server">
                <div style="display: flex; justify-content: center;">
                    <asp:Label ID="lbl_Alert" runat="server" Font-Size="Small" Text="" />
                </div>
                <br />
                <br />
                <div style="display: flex; justify-content: center;">
                    <asp:Button runat="server" Width="100" Text="Ok" OnClientClick="pop_Alert.Hide()" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_NewOrderUnit" runat="server" HeaderText="Invalid Order Unit" Modal="True" CloseAction="CloseButton" ShowPageScrollbarWhenModal="true"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl6" runat="server">
                <asp:GridView runat="server" ID="grd_UnitProd" SkinID="GRD_V1" AutoGenerateColumns="False" Width="800px">
                    <Columns>
                        <asp:TemplateField HeaderText="ProductCode">
                            <ItemTemplate>
                                <asp:Label ID="lbl_ProductCode" runat="server" Text='<%# Bind("ProductCode") %>' ToolTip='<%# Bind("ProductCode") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="80px"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <div style="white-space: nowrap; overflow: hidden; width: 150px">
                                    <asp:Label ID="lbl_DescEn" runat="server" Text='<%# Bind("ProductDesc1") %>' ToolTip='<%# Bind("ProductDesc1") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="150px"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Local Description">
                            <ItemTemplate>
                                <div style="white-space: nowrap; overflow: hidden; width: 150px">
                                    <asp:Label ID="lbl_DescTH" runat="server" Text='<%# Bind("ProductDesc2") %>' ToolTip='<%# Bind("ProductDesc2") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="150px"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="InventoryUnit">
                            <ItemTemplate>
                                <asp:Label ID="lbl_InventUnit" runat="server" Text='<%# Bind("InventoryUnit") %>' ToolTip='<%# Bind("InventoryUnit") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="150px"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="OrderUnit">
                            <ItemTemplate>
                                <asp:Label ID="lbl_Unit" runat="server" Text='<%# Bind("OrderUnit") %>' ToolTip='<%# Bind("OrderUnit") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="150px"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rate">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_Rate" runat="server" Width="95%" Text='<%# Bind("Rate") %>' SkinID="TXT_NUM_V1"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="Req_Rate" runat="server" ErrorMessage="*" ControlToValidate="txt_Rate" ValidationGroup="grp_UnitProd" Display="Dynamic"></asp:RequiredFieldValidator>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txt_Rate" ValidChars="0123456789.">
                                </ajaxToolkit:FilteredTextBoxExtender>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Right" Width="50px"></HeaderStyle>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <table border="0" cellpadding="1" cellspacing="0" width="100%">
                            <tr class="grdHeaderRow_V1">
                                <td align="left" style="width: 80px">
                                    ProductCode
                                </td>
                                <td align="left" style="width: 150px">
                                    Description1
                                </td>
                                <td align="left" style="width: 150px">
                                    Description2
                                </td>
                                <td align="left" style="width: 150px">
                                    InventoryUnit
                                </td>
                                <td align="left" style="width: 150px">
                                    OrderUnit
                                </td>
                                <td align="right" style="width: 50px">
                                    Rate
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                </asp:GridView>
                <br />
                <div style="text-align: right;">
                    <asp:Button runat="server" ID="btn_Save_NewOrderUnit" SkinID="BTN_V1" Width="100" CausesValidation="true" ValidationGroup="grp_UnitProd" Text="Save" OnClick="btn_Save_NewOrderUnit_Click" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl runat="server" ID="pop_NewOrReplace" ClientInstanceName="pop_NewOrReplace" Width="420px" HeaderText="Confirmation" Modal="true" ShowPageScrollbarWhenModal="true"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ContentStyle HorizontalAlign="Center" VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lbl_NewOrReplace" runat="server" Text="" SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:HyperLink ID="link_ExistPriceList" runat="server" Style="padding: 10px;" BackColor="WhiteSmoke" Font-Bold="true" ForeColor="Blue" Width="100" NavigateUrl=""
                                Target="_blank" Text="View"></asp:HyperLink>
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Button ID="btn_ConfirmReplace" runat="server" Style="padding: 5px;" Width="100px" ForeColor="Black" BackColor="Yellow" Text="Replace" OnClick="btn_ConfirmReplace_Click" />
                        </td>
                        <td align="center">
                            <asp:Button ID="btn_ConfirmNew" runat="server" Style="padding: 5px;" Width="100px" ForeColor="White" BackColor="DarkBlue" Text="Create as new" OnClick="btn_ConfirmNew_Click" />
                        </td>
                        <td align="left">
                            <asp:Button ID="btn_NewOrReplace_Cancel" runat="server" Style="padding: 5px;" Width="100px" SkinID="BTN_V1" Text="Cancel" OnClientClick="pop_NewOrReplace.Hide()" />
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl runat="server" ID="pop_Saved" ClientInstanceName="pop_Saved" Style="z-index: 99999 !important;" HeaderText="Saved" Width="420px" Modal="True"
        CloseAction="CloseButton" ShowCloseButton="false" ShowPageScrollbarWhenModal="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl8" runat="server">
                <div style="display: flex; justify-content: center;">
                    <asp:Label ID="lbl_Saved" runat="server" Font-Size="Small" Text="Price list saved.<br/><br/>Do you want to import another file or view this price list?" />
                </div>
                <br />
                <br />
                <div style="display: flex; justify-content: space-between;">
                    <asp:Button ID="btn_Saved_Import" runat="server" Width="100" Text="Import" OnClick="btn_Saved_Import_Click" />
                    <asp:Button ID="btn_Saved_View" runat="server" Width="100" Text="View" OnClick="btn_Saved_View_Click" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <!-- -->
</asp:Content>
