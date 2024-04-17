<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="VdList.aspx.cs" Inherits="BlueLedger.PL.PC.PL.Vendor.VdList" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%--<%@ Register src="../../../UserControl/ViewHandler/ListPage.ascx" tagname="ListPage" tagprefix="uc1" %>--%>
<%@ Register Src="../../../UserControl/ViewHandler/ListPage2.ascx" TagName="ListPage2"
    TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <table style="width: 100%;" border="0" cellpadding="3" cellspacing="0">
        <tr>
            <td align="left">
                <uc1:ListPage2 ID="ListPage" runat="server" AllowViewCreate="False" DetailPageURL="Vd.aspx"
                    EditPageURL="~/PC/PL/Vendor/VdEdit.aspx" KeyFieldName="PriceLstNo" PageCode="[IN].[vPLVdList]"
                    Title="Price List by Vendor" />
            </td>
        </tr>
    </table>
    <dx:ASPxPopupControl ID="popup_vendor" runat="server" ClientInstanceName="popup_vendor"
        HeaderText="Create By Vendor" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        Modal="True" Width="280px" ShowCloseButton="True">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <table width="100%">
                    <tr align="center">
                        <td>
                            <div style="height: auto; margin-bottom: 10px; margin-top: 10px;" align="center">
                                <%--<dx:ASPxComboBox ID="ddl_Vendor" runat="server" ValueType="System.String" Width="100%"
                                    DataSourceID="ods_Vendor" IncrementalFilteringMode="Contains" TextFormatString="{0} : {1}"
                                    ValueField="VendorCode" TextField="Name" Paddings-PaddingBottom="5px" Paddings-PaddingTop="5px">
                                    <Columns>
                                        <dx:ListBoxColumn Caption="Code" FieldName="VendorCode" Width="100px" />
                                        <dx:ListBoxColumn Caption="Name" FieldName="Name" Width="300px" />
                                    </Columns>
                                </dx:ASPxComboBox>--%>
                                <asp:DropDownList ID="ddl_vendor" runat="server" Width="100%" DataSourceID="ods_Vendor"
                                    AppendDataBoundItems="true" DataValueField="VendorCode" DataTextField="Name"
                                    Style="padding-top: 5px; padding-bottom: 5px;">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RFVComboddl_Vendor" runat="server" ControlToValidate="ddl_Vendor"
                                    ErrorMessage="Vendor name is required." ForeColor="Red"></asp:RequiredFieldValidator>
                                <asp:ObjectDataSource ID="ods_Vendor" runat="server" SelectMethod="GetList" TypeName="Blue.BL.AP.Vendor">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="hf_ConnStr" Name="connStr" PropertyName="Value"
                                            Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </div>
                        </td>
                    </tr>
                    <tr align="right">
                        <td>
                            <asp:Button ID="btn_OK" runat="server" Text="<%$ Resources:PC_PL_Vendor_VdEdit, btn_OK %>"
                                SkinID="BTN_V1" Width="60px" OnClick="btn_OK_Click" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <asp:HiddenField ID="hf_ConnStr" runat="server" />
</asp:Content>
