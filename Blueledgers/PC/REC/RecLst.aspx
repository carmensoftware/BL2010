<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="RecLst.aspx.cs" Inherits="BlueLedger.PL.IN.REC.RecLst" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Src="../../UserControl/ViewHandler/ListPage2.ascx" TagName="ListPage2" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <uc1:ListPage2 ID="ListPage2" runat="server" DetailPageURL="Rec.aspx" KeyFieldName="RecNo" ListPageCuzURL="RecView.aspx" PageCode="[PC].[vRECList]" Title="Receiving" />
    <asp:UpdatePanel ID="upd_CreatePObyPR" runat="server">
        <ContentTemplate>
            <dx:ASPxPopupControl ID="pop_SelectVendor" runat="server" ClientInstanceName="pop_ML" CloseAction="CloseButton" Modal="True" HeaderText="<%$ Resources:PC_REC_RecLst, PurchaseOrderList %>"
                Height="460px" Width="780px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AutoUpdatePosition="True">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <asp:HiddenField ID="hf_Vendor" runat="server" />
                        <div style="display: block; padding: 3px;">
                            <div style="display: inline-block; vertical-align: top; padding: 3px;">
                                <asp:Label ID="lbl_VendorHd_Nm" runat="server" Text="<%$ Resources:PC_REC_RecLst, lbl_VendorHd_Nm %>" SkinID="LBL_HD" />
                            </div>
                            <div style="display: inline-block;">
                                <dx:ASPxComboBox ID="ddl_Vendor" runat="server" Width="420px" TextField="VendorName" ValueField="VendorCode" AutoPostBack="True" TextFormatString="{0} : {1}"
                                    IncrementalFilteringMode="Contains" EnableCallbackMode="true" CallbackPageSize="10" OnLoad="ddl_Vendor_Load" OnSelectedIndexChanged="ddl_Vendor_SelectedIndexChanged">
                                    <Columns>
                                        <dx:ListBoxColumn Caption="Code" FieldName="VendorCode" Width="200px" />
                                        <dx:ListBoxColumn Caption="Name" FieldName="VendorName" Width="380px" />
                                    </Columns>
                                </dx:ASPxComboBox>
                            </div>
                            <div style="display: inline-block;">
                                <%--/* Added on: 17/08/2017, By: Fon */--%>
                                <dx:ASPxComboBox ID="comb_CurrCode" runat="server" Width="100px" TextField="CurrencyCode" ValueField="CurrencyCode" AutoPostBack="true" IncrementalFilteringMode="Contains"
                                    EnableCallbackMode="true" CallbackPageSize="10" OnInit="comb_CurrCode_Init" OnSelectedIndexChanged="comb_CurrCode_SelectedIndexChanged">
                                    <Columns>
                                        <dx:ListBoxColumn Caption="Currency" FieldName="CurrencyCode" Width="100px" />
                                    </Columns>
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                        <div style="clear: both; padding: 3px;">
                            <div style="height: 390px; overflow: auto">
                                <asp:GridView ID="grd_PoList" runat="server" AutoGenerateColumns="False" DataKeyNames="PoNo" SkinID="GRD_V1" Width="100%" OnRowDataBound="grd_PoList_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecLst, lbl_Charp %>">
                                            <HeaderStyle Width="50px" />
                                            <ItemStyle Width="50px" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chk_Item" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecLst, lbl_Vendor_Nm %>">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Vendor" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecLst, lbl_DeliveryDate_Nm %>">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_DeliveryDate" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecLst, lbl_RefNo_Nm %>">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_PoNo" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecLst, lbl_Status_Nm %>">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Status" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <div style="clear: both; padding: 3px;">
                            <div style="float: right; padding: 3px;">
                                <asp:Button ID="btn_Ok_SelectVendor" runat="server" Text="<%$ Resources:PC_REC_RecLst, btn_Ok %>" OnClick="btn_Ok_SelectVendor_Click" SkinID="BTN_V1" Width="50px" />
                            </div>
                        </div>
                        <div style="clear: both;">
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_Cancel" runat="server" ClientInstanceName="pop_Cancel" CloseAction="CloseButton" Modal="True" HeaderText="<%$ Resources:PC_REC_RecLst, PurchaseOrderListCancel %>"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="460px" Width="780px">
                <ContentStyle VerticalAlign="Top">
                </ContentStyle>
                <FooterStyle />
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                        <table border="0" cellpadding="1" cellspacing="0" width="780px">
                            <tr>
                                <td valign="top">
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td width="60px">
                                                            <asp:Label ID="lbl_VendorCancelHd_Nm" runat="server" Text="<%$ Resources:PC_REC_RecLst, lbl_VendorCancelHd_Nm %>" Font-Bold="False" SkinID="LBL_HD"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxComboBox ID="ddl_VendorCancel" runat="server" Width="200px" LoadingPanelImagePosition="Top" ShowShadow="False" OnLoad="ddl_VendorCancel_Load" TextField="VendorName"
                                                                TextFormatString="{0} : {1}" ValueField="VendorCode" AutoPostBack="True" OnValueChanged="ddl_VendorCancel_ValueChanged" IncrementalFilteringMode="Contains">
                                                                <Columns>
                                                                    <dx:ListBoxColumn Caption="Code" FieldName="VendorCode" />
                                                                    <dx:ListBoxColumn Caption="Name" FieldName="VendorName" />
                                                                </Columns>
                                                                <%--<LoadingPanelImage Url="~/App_Themes/Aqua/Editors/Loading.gif">
                                                                    </LoadingPanelImage--%>
                                                                <DropDownButton>
                                                                    <Image>
                                                                        <SpriteProperties HottrackedCssClass="" PressedCssClass="" />
                                                                    </Image>
                                                                </DropDownButton>
                                                                <ValidationSettings>
                                                                    <ErrorFrameStyle ImageSpacing="4px">
                                                                        <ErrorTextPaddings PaddingLeft="4px" />
                                                                    </ErrorFrameStyle>
                                                                </ValidationSettings>
                                                            </dx:ASPxComboBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%--<dx:ASPxGridView ID="grd_CancelItemList" ClientInstanceName="grd_CancelItemList"
                                                        runat="server" AutoGenerateColumns="False" KeyFieldName="PoNo" OnLoad="grd_CancelItemList_OnLoad"
                                                        SkinID="Default2" Width="100%" SettingsBehavior-AllowMultiSelection="false">
                                                        <Styles>
                                                            <Header HorizontalAlign="Center">
                                                            </Header>
                                                        </Styles>
                                                        <SettingsPager AlwaysShowPager="True" PageSize="10">
                                                        </SettingsPager>
                                                        <SettingsEditing Mode="Inline" NewItemRowPosition="Bottom" />
                                                        <Columns>
                                                            <dx:GridViewCommandColumn VisibleIndex="1" ShowSelectCheckbox="true" Width="50px">
                                                                <ClearFilterButton Visible="True">
                                                                </ClearFilterButton>
                                                            </dx:GridViewCommandColumn>
                                                            <dx:GridViewDataTextColumn Caption="Vendor Name" FieldName="VendorName" VisibleIndex="2">
                                                                <DataItemTemplate>
                                                                    <dx:ASPxLabel ID="lbl_Vendor" runat="server" Text='<%#Eval("VendorCode") %>'>
                                                                    </dx:ASPxLabel>
                                                                    :
                                                                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text='<%#Eval("VendorName") %>'>
                                                                    </dx:ASPxLabel>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataDateColumn Caption="Delivery Date" FieldName="DeliveryDate" VisibleIndex="3">
                                                                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy">
                                                                </PropertiesDateEdit>
                                                            </dx:GridViewDataDateColumn>
                                                            <dx:GridViewDataTextColumn Caption="Po No." FieldName="PoNo" VisibleIndex="4">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Qty Order" FieldName="OrdQty" VisibleIndex="5"
                                                                Width="80px">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Qty Receive" FieldName="RcvQty" VisibleIndex="6"
                                                                Width="80px">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Status" FieldName="DocStatus" VisibleIndex="8">
                                                            </dx:GridViewDataTextColumn>
                                                        </Columns>
                                                        <Settings ShowFilterRow="True" />
                                                        <SettingsBehavior />
                                                    </dx:ASPxGridView>--%>
                                                <div style="height: 390px; overflow: auto;">
                                                    <asp:GridView ID="grd_CancelItemList" runat="server" AutoGenerateColumns="False" DataKeyNames="PoNo" SkinID="GRD_V1" Width="100%" OnRowDataBound="grd_CancelItemList_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecLst, lbl_CharpCancel %>">
                                                                <HeaderStyle Width="50px" />
                                                                <ItemStyle Width="50px" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chk_Item" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecLst, lbl_VendorCancel_Nm %>">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_Vendor" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecLst, lbl_DeliveryDateCancel_Nm %>">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_DeliveryDate" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecLst, lbl_PoNo_Nm %>">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_PoNo" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecLst, lbl_QtyOrd_Nm %>">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_QtyOrd" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecLst, lbl_QtyRec_Nm %>">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_QtyRec" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecLst, lbl_StatusCancel_Nm %>">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_Status" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 9px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <%--<dx:ASPxButton ID="btn_Cancel" runat="server" Text="OK" OnClick="btn_Cancel_Click">
                                                    </dx:ASPxButton>--%>
                                                <asp:HiddenField ID="hf_VendorCancel" runat="server" />
                                                <asp:Button ID="btn_Cancel" runat="server" Text="<%$ Resources:PC_REC_RecLst, btn_Cancel %>" OnClick="btn_Cancel_Click" SkinID="BTN_V1" Width="50px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_HQ" runat="server" ClientInstanceName="pop_HQ" CloseAction="CloseButton" Modal="True" HeaderText="<%$ Resources:PC_REC_RecLst, PurchaseOrderListFromHQ %>"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="460px" Width="780px">
                <ContentStyle VerticalAlign="Top">
                </ContentStyle>
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                        <table border="0" cellpadding="1" cellspacing="0" width="780px">
                            <tr>
                                <td valign="top">
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td style="width: 60px;">
                                                            <asp:Label ID="lbl_VendorHdHq_Nm" runat="server" Text="<%$ Resources:PC_REC_RecLst, lbl_VendorHdHq_Nm %>" SkinID="LBL_HD"></asp:Label>
                                                        </td>
                                                        <td style="width: 200px;">
                                                            <dx:ASPxComboBox ID="ddl_VendorHQ" runat="server" Width="200px" LoadingPanelImagePosition="Top" ShowShadow="False" TextField="VendorName" TextFormatString="{0} : {1}"
                                                                ValueField="VendorCode" AutoPostBack="True" OnLoad="ddl_VendorHQ_Load" OnValueChanged="ddl_VendorHQ_ValueChanged" IncrementalFilteringMode="Contains">
                                                                <Columns>
                                                                    <dx:ListBoxColumn Caption="Code" FieldName="VendorCode" />
                                                                    <dx:ListBoxColumn Caption="Name" FieldName="VendorName" />
                                                                </Columns>
                                                                <LoadingPanelImage Url="">
                                                                </LoadingPanelImage>
                                                                <DropDownButton>
                                                                    <Image>
                                                                        <SpriteProperties HottrackedCssClass="" PressedCssClass="" />
                                                                    </Image>
                                                                </DropDownButton>
                                                                <ValidationSettings>
                                                                    <ErrorFrameStyle ImageSpacing="4px">
                                                                        <ErrorTextPaddings PaddingLeft="4px" />
                                                                        <ErrorTextPaddings PaddingLeft="4px"></ErrorTextPaddings>
                                                                    </ErrorFrameStyle>
                                                                </ValidationSettings>
                                                            </dx:ASPxComboBox>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxComboBox ID="comb_CurrCodeHQ" runat="server" AutoPostBack="true" Width="100px" IncrementalFilteringMode="Contains" ValueField="CurrencyCode" TextField="CurrencyCode"
                                                                OnInit="comb_CurrCodeHQ_Init" OnSelectedIndexChanged="comb_CurrCodeHQ_SelectedIndexChanged">
                                                                <Columns>
                                                                    <dx:ListBoxColumn Caption="Currency" FieldName="CurrencyCode" Width="100px" />
                                                                </Columns>
                                                            </dx:ASPxComboBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%--<dx:ASPxGridView ID="grd_HQItemList" ClientInstanceName="grd_HQItemList" runat="server"
                                                        AutoGenerateColumns="False" KeyFieldName="PoNo" OnLoad="grd_HQItemList_OnLoad"
                                                        SkinID="Default2" Width="100%" SettingsBehavior-AllowMultiSelection="false">
                                                        <Styles>
                                                            <Header HorizontalAlign="Center">
                                                            </Header>
                                                        </Styles>
                                                        <SettingsPager AlwaysShowPager="True" PageSize="10">
                                                        </SettingsPager>
                                                        <SettingsEditing Mode="Inline" NewItemRowPosition="Bottom" />
                                                        <Columns>
                                                            <dx:GridViewCommandColumn VisibleIndex="1" ShowSelectCheckbox="true" Width="50px">
                                                                <ClearFilterButton Visible="True">
                                                                </ClearFilterButton>
                                                            </dx:GridViewCommandColumn>
                                                            <dx:GridViewDataTextColumn Caption="Vendor Name" FieldName="VendorName" VisibleIndex="2">
                                                                <DataItemTemplate>
                                                                    <dx:ASPxLabel ID="lbl_Vendor" runat="server" Text='<%#Eval("VendorCode") %>'>
                                                                    </dx:ASPxLabel>
                                                                    :
                                                                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text='<%#Eval("VendorName") %>'>
                                                                    </dx:ASPxLabel>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataDateColumn Caption="Delivery Date" FieldName="DeliveryDate" VisibleIndex="3">
                                                                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy">
                                                                </PropertiesDateEdit>
                                                            </dx:GridViewDataDateColumn>
                                                            <dx:GridViewDataTextColumn Caption="Po No." FieldName="PoNo" VisibleIndex="4">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Status" FieldName="DocStatus" VisibleIndex="8">
                                                            </dx:GridViewDataTextColumn>
                                                        </Columns>
                                                        <Settings ShowFilterRow="True" />
                                                        <SettingsBehavior />
                                                    </dx:ASPxGridView>--%>
                                                <div style="height: 390px; overflow: auto">
                                                    <asp:GridView ID="grd_HQItemList" runat="server" AutoGenerateColumns="False" DataKeyNames="PoNo" SkinID="GRD_V1" Width="100%" OnRowDataBound="grd_HQItemList_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecLst, lbl_CharpHq %>">
                                                                <HeaderStyle Width="50px" />
                                                                <ItemStyle Width="50px" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chk_Item" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecLst, lbl_VendorHq_Nm %>">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_Vendor" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecLst, lbl_DeliveryDateHq_Nm %>">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_DeliveryDate" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecLst, lbl_RefNoHq_Nm %>">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_PoNo" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecLst, lbl_StatusHq_Nm %>">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_Status" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 9px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <%--<dx:ASPxButton ID="btn_HQ" runat="server" Text="OK" OnClick="btn_HQ_Click">
                                                    </dx:ASPxButton>--%>
                                                <asp:HiddenField ID="hf_VendorHQ" runat="server" />
                                                <asp:Button ID="btn_HQ" runat="server" Text="<%$ Resources:PC_REC_RecLst, btn_HQ %>" OnClick="btn_HQ_Click" SkinID="BTN_V1" Width="50px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_CancelHQ" runat="server" ClientInstanceName="pop_CancelHQ" CloseAction="CloseButton" Modal="True" HeaderText="<%$ Resources:PC_REC_RecLst, PurchaseOrderListFromHQCancel %>"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="460px" Width="780px">
                <ContentStyle VerticalAlign="Top">
                </ContentStyle>
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                        <table border="0" cellpadding="1" cellspacing="0" width="780px">
                            <tr>
                                <td valign="top">
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td width="60px">
                                                            <asp:Label ID="lbl_VendorCancelHdHq_Nm" runat="server" Text="<%$ Resources:PC_REC_RecLst, lbl_VendorCancelHdHq_Nm %>" SkinID="LBL_HD"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxComboBox ID="ddl_VendorHQCancel" runat="server" Width="200px" LoadingPanelImagePosition="Top" ShowShadow="False" TextField="VendorName" TextFormatString="{0} : {1}"
                                                                ValueField="VendorCode" AutoPostBack="True" OnLoad="ddl_VendorHQCancel_Load" OnValueChanged="ddl_VendorHQCancel_ValueChanged" IncrementalFilteringMode="Contains">
                                                                <Columns>
                                                                    <dx:ListBoxColumn Caption="Code" FieldName="VendorCode" />
                                                                    <dx:ListBoxColumn Caption="Name" FieldName="VendorName" />
                                                                </Columns>
                                                                <%-- <LoadingPanelImage Url="~/App_Themes/Aqua/Editors/Loading.gif">
                                                                    </LoadingPanelImage>--%>
                                                                <DropDownButton>
                                                                    <Image>
                                                                        <SpriteProperties HottrackedCssClass="" PressedCssClass="" />
                                                                    </Image>
                                                                </DropDownButton>
                                                                <ValidationSettings>
                                                                    <ErrorFrameStyle ImageSpacing="4px">
                                                                        <ErrorTextPaddings PaddingLeft="4px" />
                                                                    </ErrorFrameStyle>
                                                                </ValidationSettings>
                                                            </dx:ASPxComboBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%--<dx:ASPxGridView ID="grd_CancelHQ" ClientInstanceName="grd_CancelHQ" runat="server"
                                                        AutoGenerateColumns="False" KeyFieldName="PoNo" Width="100%" SkinID="Default2"
                                                        SettingsBehavior-AllowMultiSelection="false" OnLoad="grd_CancelHQ_Load">
                                                        <Styles>
                                                            <Header HorizontalAlign="Center">
                                                            </Header>
                                                        </Styles>
                                                        <SettingsPager AlwaysShowPager="True" PageSize="10">
                                                        </SettingsPager>
                                                        <SettingsEditing Mode="Inline" NewItemRowPosition="Bottom" />
                                                        <Columns>
                                                            <dx:GridViewCommandColumn VisibleIndex="1" ShowSelectCheckbox="true" Width="50px">
                                                                <ClearFilterButton Visible="True">
                                                                </ClearFilterButton>
                                                            </dx:GridViewCommandColumn>
                                                            <dx:GridViewDataTextColumn Caption="Vendor Name" FieldName="VendorName" VisibleIndex="2">
                                                                <DataItemTemplate>
                                                                    <dx:ASPxLabel ID="lbl_Vendor" runat="server" Text='<%#Eval("VendorCode") %>'>
                                                                    </dx:ASPxLabel>
                                                                    :
                                                                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text='<%#Eval("VendorName") %>'>
                                                                    </dx:ASPxLabel>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataDateColumn Caption="Delivery Date" FieldName="DeliveryDate" VisibleIndex="3">
                                                                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy">
                                                                </PropertiesDateEdit>
                                                            </dx:GridViewDataDateColumn>
                                                            <dx:GridViewDataTextColumn Caption="Po No." FieldName="PoNo" VisibleIndex="4">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Qty Order" FieldName="OrdQty" VisibleIndex="5"
                                                                Width="80px">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Qty Receive" FieldName="RcvQty" VisibleIndex="6"
                                                                Width="80px">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Status" FieldName="DocStatus" VisibleIndex="8">
                                                            </dx:GridViewDataTextColumn>
                                                        </Columns>
                                                        <Settings ShowFilterRow="True" />
                                                        <SettingsBehavior />
                                                    </dx:ASPxGridView>--%>
                                                <div style="height: 390px; overflow: auto">
                                                    <asp:GridView ID="grd_CancelHQ" runat="server" AutoGenerateColumns="False" DataKeyNames="PoNo" SkinID="GRD_V1" Width="100%" OnRowDataBound="grd_CancelHQ_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecLst, lbl_CharpCancelHq %>">
                                                                <HeaderStyle Width="50px" />
                                                                <ItemStyle Width="50px" />
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chk_Item" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecLst, lbl_VendorCancelHq_Nm %>">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_Vendor" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecLst, lbl_DeliveryDateCancelHq_Nm %>">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_DeliveryDate" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecLst, lbl_PoNoHq_Nm %>">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_PoNo" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecLst, lbl_QtyOrdHq_Nm %>">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_QtyOrd" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecLst, lbl_QtyRecHq_Nm %>">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_QtyRec" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecLst, lbl_StatusCancelHq_Nm %>">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_Status" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 9px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <%--<dx:ASPxButton ID="btn_CancelHQ" runat="server" Text="OK" OnClick="btn_CancelHQ_Click">
                                                    </dx:ASPxButton>--%>
                                                <asp:HiddenField ID="hf_VendorHQCancel" runat="server" />
                                                <asp:Button ID="btn_CancelHQ" runat="server" Text="<%$ Resources:PC_REC_RecLst, btn_CancelHQ %>" OnClick="btn_CancelHQ_Click" SkinID="BTN_V1" Width="50px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_SelectLocation" runat="server" ClientInstanceName="pop_SelectLocation" HeaderText="<%$ Resources:PC_REC_RecLst, StoreLocationList %>"
                CloseAction="None" ShowCloseButton="false" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="150px" Width="400px">
                <ContentStyle VerticalAlign="Top">
                </ContentStyle>
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                            <tr>
                                <td>
                                    <table width="100%">
                                        <tr>
                                            <td width="100px">
                                                <asp:Label ID="lbl_Location_Nm" runat="server" Text="<%$ Resources:PC_REC_RecLst, lbl_Location_Nm %>" SkinID="LBL_HD"></asp:Label>
                                            </td>
                                            <td>
                                                <dx:ASPxComboBox ID="ddl_Location" runat="server" Width="280px" ValueField="LocationCode" TextFormatString="{0} : {1}" IncrementalFilteringMode="Contains">
                                                    <Columns>
                                                        <dx:ListBoxColumn Caption="Code" FieldName="LocationCode" />
                                                        <dx:ListBoxColumn Caption="Name" FieldName="LocationName" />
                                                    </Columns>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btn_ConfirmLocation" runat="server" Text="<%$ Resources:PC_REC_RecLst, btn_ConfirmLocation %>" OnClick="btn_ConfirmLocation_Click" SkinID="BTN_V1"
                                        Width="50px" />&nbsp
                                    <asp:Button ID="btn_CancelLocation" runat="server" Text="CANCEL" OnClick="btn_CancelLocation_Click" SkinID="BTN_V1" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_LocationHQList" runat="server" ClientInstanceName="pop_LocationHQList" CloseAction="CloseButton" Modal="True" HeaderText="<%$ Resources:PC_REC_RecLst, StoreLocationListHQ %>"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="150px" Width="400px">
                <ContentStyle VerticalAlign="Top">
                </ContentStyle>
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl6" runat="server">
                        <table border="0" cellpadding="1" cellspacing="0" width="400px">
                            <tr>
                                <td valign="top">
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td width="100px">
                                                            <asp:Label ID="lbl_LocationHq_Nm" runat="server" Text="<%$ Resources:PC_REC_RecLst, lbl_LocationHq_Nm %>" SkinID="LBL_HD"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxComboBox ID="ddl_LocationHQ" runat="server" Width="280px" LoadingPanelImagePosition="Top" ShowShadow="False" ValueField="LocationCode" TextFormatString="{0} : {1}"
                                                                IncrementalFilteringMode="Contains">
                                                                <Columns>
                                                                    <dx:ListBoxColumn Caption="Code" FieldName="LocationCode" />
                                                                    <dx:ListBoxColumn Caption="Name" FieldName="LocationName" />
                                                                </Columns>
                                                                <LoadingPanelImage Url="">
                                                                </LoadingPanelImage>
                                                                <DropDownButton>
                                                                    <Image>
                                                                        <SpriteProperties HottrackedCssClass="" PressedCssClass="" />
                                                                    </Image>
                                                                </DropDownButton>
                                                                <ValidationSettings>
                                                                    <ErrorFrameStyle ImageSpacing="4px">
                                                                        <ErrorTextPaddings PaddingLeft="4px" />
                                                                    </ErrorFrameStyle>
                                                                </ValidationSettings>
                                                            </dx:ASPxComboBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <%--<dx:ASPxButton ID="btn_ConfirmLocationHQ" runat="server" Text="OK" OnClick="btn_ConfirmLocationHQ_Click">
                                                    </dx:ASPxButton>--%>
                                                <asp:Button ID="btn_ConfirmLocationHQ" runat="server" Text="<%$ Resources:PC_REC_RecLst, btn_ConfirmLocationHQ %>" OnClick="btn_ConfirmLocationHQ_Click"
                                                    Width="50px" SkinID="BTN_V1" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_Warning" ClientInstanceName="pop_Warning" runat="server" Width="300px" CloseAction="CloseButton" HeaderText="<%$ Resources:PC_REC_RecLst, Warning %>"
                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                <ContentStyle VerticalAlign="Top">
                </ContentStyle>
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl7" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" height="50px">
                                    <asp:Label ID="lbl_Warning" runat="server" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <%--<dx:ASPxButton CausesValidation="false" ID="btn_Warning" runat="server" Text="OK"
                                            Width="50px" OnClick="btn_Warning_Click">
                                        </dx:ASPxButton>--%>
                                    <asp:Button ID="btn_Warning" runat="server" Text="<%$ Resources:PC_REC_RecLst, btn_Warning %>" Width="50px" OnClick="btn_Warning_Click" SkinID="BTN_V1" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_WarningPo" ClientInstanceName="pop_Warning" runat="server" Width="300px" CloseAction="CloseButton" HeaderText="<%$ Resources:PC_REC_RecLst, Warning %>"
                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                <ContentStyle VerticalAlign="Top">
                </ContentStyle>
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl8" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" height="50px">
                                    <asp:Label ID="lbl_WarningPo" runat="server" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <%--<dx:ASPxButton CausesValidation="false" ID="btn_Warning" runat="server" Text="OK"
                                            Width="50px" OnClick="btn_Warning_Click">
                                        </dx:ASPxButton>--%>
                                    <asp:Button ID="btn_WarningPo" runat="server" Text="<%$ Resources:PC_REC_RecLst, btn_Warning %>" Width="50px" SkinID="BTN_V1" OnClick="btn_WarningPo_Click" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_VendorMapping" ClientInstanceName="pop_VendorMapping" runat="server" Width="460px" CloseAction="CloseButton" HeaderText="Vendor Mapping"
                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="True">
                <ContentStyle VerticalAlign="Top">
                </ContentStyle>
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl9" runat="server">
                        <p>
                            <asp:Label ID="lbl_VendorMapping_Msg" runat="server" Text="Vendor(HQ) is not assign to vendor of this BU. Please assign vendor code for (HQ) " />
                            <asp:Label ID="lbl_VendorMapping_Text" runat="server" Text="" />
                        </p>
                        <div>
                            <asp:Label ID="lbl_VendorMapping" runat="server" Text="<%$ Resources:PC_REC_RecLst, lbl_VendorHdHq_Nm %>" SkinID="LBL_HD" />
                            <dx:ASPxComboBox ID="ddl_VendorLocal" runat="server" Width="380px" DropDownStyle="DropDown" AutoPostBack="true" TextFormatString="{0} : {1}" ValueField="VendorCode"
                                OnInit="ddl_VendorLocal_Init">
                                <Columns>
                                    <dx:ListBoxColumn Caption="Code" FieldName="VendorCode" />
                                    <dx:ListBoxColumn Caption="Name" FieldName="Name" />
                                    <%--<dx:ListBoxColumn Caption="Name" FieldName="VendorName" />--%>
                                </Columns>
                            </dx:ASPxComboBox>
                        </div>
                        <br />
                        <div style="text-align: center;">
                            <asp:Button ID="BtnOkVendorMapping" runat="server" Text="Ok" Width="65px" SkinID="BTN_V1" OnClick="BtnOkVendorMapping_Click" />
                            &nbsp;&nbsp;&nbsp;
                            <asp:Button ID="BtnCancelVendorMapping" runat="server" Text="Cancel" Width="65px" SkinID="BTN_V1" OnClick="BtnCancelVendorMapping_Click" />
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
