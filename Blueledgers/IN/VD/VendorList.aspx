<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="VendorList.aspx.cs" Inherits="BlueLedger.PL.IN.VendorList" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Src="~/UserControl/ViewHandler/ListPage2.ascx" TagName="ListPage2" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<asp:Content ID="cnt_VendorList" runat="server" ContentPlaceHolderID="cph_Main">
    <asp:Panel runat="server" ID="panel_List" Width="100%">
        <uc1:ListPage2 ID="ListPage" runat="server" Title="Vendor Profile" PageCode="[AP].[vVendorProfile]" KeyFieldName="VendorCode" DetailPageURL="Vendor.aspx"
            ListPageCuzURL="VendorView.aspx" EditPageURL="~/IN/VD/VendorEdit.aspx" />
    </asp:Panel>
    <asp:Panel runat="server" ID="panel_Category">
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr style="background-color: #4D4D4D; height: 25px">
                <td style="padding-left: 10px;">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                    <asp:Label ID="lbl_Title" runat="server" Text="Vendor Category" SkinID="LBL_HD_WHITE"></asp:Label>
                </td>
                <td align="right" style="padding-right: 10px;">
                    <asp:LinkButton runat="server" ID="btn_Category_Create" OnClick="btn_Category_Create_Click">
                        <asp:Image ID="Image2" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/create.png" />
                    </asp:LinkButton>
                    <asp:LinkButton runat="server" ID="btn_Category_Back" OnClick="btn_Category_Back_Click">
                        <asp:Image runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" />
                    </asp:LinkButton>
                </td>
            </tr>
        </table>
        <br />
        <asp:HiddenField runat="server" ID="hf_VendorCategoryCode" />
        <asp:GridView runat="server" ID="gv_Category" SkinID="GRD_V1" Width="100%" AutoGenerateColumns="false" DataKeyNames="VendorCategoryCode" OnRowEditing="gv_Category_RowEditing"
            OnRowCancelingEdit="gv_Category_RowCancelingEdit" OnRowDeleting="gv_Category_RowDeleting" OnRowUpdating="gv_Category_RowUpdating">
            <HeaderStyle HorizontalAlign="Left" />
            <Columns>
                <asp:BoundField HeaderText="Code" DataField="VendorCategoryCode" ControlStyle-Width="80" ReadOnly="true" />
                <asp:BoundField HeaderText="Name" DataField="Name" ControlStyle-Width="280" />
                <asp:TemplateField HeaderText="Active">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="chk_IsActive" Width="60" Checked='<%# Eval("IsActive").ToString()=="True" %>' onclick="return false;" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:CheckBox runat="server" ID="chk_IsActive" Width="60" Checked='<%# Eval("IsActive").ToString()=="True" %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="#">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="LinkButton1" Width="60" CommandName="Delete" Text="Delete" />
                        <asp:LinkButton runat="server" ID="LinkButton2" Width="60" CommandName="Edit" Text="Edit" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:LinkButton runat="server" ID="LinkButton1" Width="60" CommandName="Update" Text="Save" />
                        <asp:LinkButton runat="server" ID="LinkButton2" Width="60" CommandName="Cancel" Text="Cancel" />
                    </EditItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
    <dx:ASPxPopupControl ID="pop_Category_Create" ClientInstanceName="pop_Category_Create" runat="server" Width="360" HeaderText="New Category" Modal="True"
        ShowCloseButton="true" CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                <div style="width: 100%; text-align: left;">
                    <asp:Label ID="Label1" runat="server" SkinID="LBL_NR" Text="Code" />
                    <asp:TextBox runat="server" ID="txt_CategoryCode_New" Width="100%" MaxLength="10" />
                </div>
                <br />
                <div style="width: 100%; text-align: left;">
                    <asp:Label ID="Label2" runat="server" SkinID="LBL_NR" Text="Name" />
                    <asp:TextBox runat="server" ID="txt_CategoryName_New" Width="100%" MaxLength="200" />
                </div>
                <br />
                <div style="width: 100%; text-align: left;">
                    <asp:CheckBox runat="server" ID="chk_CategoryIsActive_New" Text="Active" />
                </div>
                <br />
                <div style="width: 100%; text-align: right;">
                    <asp:Button runat="server" ID="Button1" Width="80" Text="Save" OnClick="btn_Pop_Category_Create_Save_Click" />
                    &nbsp;&nbsp;
                    <asp:Button runat="server" ID="Button2" Width="80" Text="Cance" OnClientClick="pop_Category_Create.Hide();" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_Category_ConfirmDelete" ClientInstanceName="pop_Category_ConfirmDelete" runat="server" Width="360" HeaderText="Confirm" Modal="True"
        ShowCloseButton="true" CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="True">
        <HeaderStyle BackColor="#ffffcc" />
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <div style="width: 100%; text-align: center;">
                    <asp:Label ID="lbl_Category_ConfirmDelete" runat="server" SkinID="LBL_NR" />
                </div>
                <br />
                <div style="width: 100%; text-align: center;">
                    <asp:Button runat="server" ID="btn_Category_ConfirmDelete_Yes" Width="80" Text="Yes" OnClick="btn_Category_ConfirmDelete_Yes_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button runat="server" ID="btn_Category_ConfirmDelte_No" Width="80" Text="No" OnClientClick="pop_Category_ConfirmDelete.Hide();" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_Alert" ClientInstanceName="pop_Alert" runat="server" Width="360" HeaderText="<%$ Resources:PC_REC_RecEdit, Warning %>" Modal="True"
        ShowCloseButton="true" CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="True">
        <HeaderStyle BackColor="#ffffcc" />
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl_Alert" runat="server">
                <div style="width: 100%; text-align: center;">
                    <asp:Label ID="lbl_Pop_Alert" runat="server" SkinID="LBL_NR"></asp:Label>
                </div>
                <br />
                <div style="width: 100%; text-align: center;">
                    <button style="width: 100px; padding: 5px;" onclick="pop_Alert.Hide();">
                        Ok
                    </button>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <br />
    <asp:Button runat="server" ID="btn_SyncVendor" Text="Update Vendor from Carmen" OnClick="btn_SyncVendor_Click" Visible="false" />
    <div>
        <asp:Label runat="server" ID="lbl_Error" ForeColor="Red" Text="" />
    </div>
    </
</asp:Content>
