<%@ Page Title="Job Code" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="JobCode.aspx.cs" Inherits="BlueLedger.PL.Option.Inventory.JobCode" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <!--Menu Bar-->
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr style="background-color: #4d4d4d; height: 17px;">
            <td style="padding-left: 10px; padding-top: 2px; vertical-align: baseline;">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_HD_WHITE" Text="Job Code" />
            </td>
            <td align="right" style="padding-right: 10px;">
                <dx:ASPxMenu runat="server" ID="menu_ItemClick" Font-Bold="True" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle"
                    Height="16px" OnItemClick="menu_ItemClick_Click">
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
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/create.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                            </ItemStyle>
                        </dx:MenuItem>
                        <%--<dx:MenuItem Name="Delete" Text="">
                            <ItemStyle Height="16px" Width="47px">
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                            </ItemStyle>
                        </dx:MenuItem>--%>
                        <dx:MenuItem Name="Print" Text="">
                            <ItemStyle Height="16px" Width="43px">
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-print.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                            </ItemStyle>
                        </dx:MenuItem>
                    </Items>
                </dx:ASPxMenu>
            </td>
        </tr>
    </table>
    <!-- -->
    <br />
    <asp:GridView ID="gvJobcode" runat="server" Width="100%" AutoGenerateColumns="false" SkinID="GRD_V1" AllowPaging="true" PageSize="50" OnPageIndexChanging="gvJobcode_PageIndexChanging"
        OnRowCommand="gvJobcode_RowCommand" OnRowEditing="gvJobcode_RowEditing" OnRowUpdating="gvJobcode_RowUpdating" OnRowCancelingEdit="gvJobcode_RowCancelingEdit">
        <Columns>
            <asp:TemplateField HeaderStyle-Width="160">
                <HeaderStyle HorizontalAlign="Center" />
                <HeaderTemplate>
                    <asp:Label runat="server" Text="#" />
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Left" />
                <ItemTemplate>
                    <asp:LinkButton ID="btn_Edit" runat="server" CausesValidation="False" CommandName="Edit" CommandArgument="<%# Container.DataItemIndex %>" SkinID="LNKB_NORMAL">Edit</asp:LinkButton>
                    <asp:LinkButton ID="btn_Del" runat="server" CausesValidation="False" CommandName="DeleteItem" CommandArgument="<%# Container.DataItemIndex %>" SkinID="LNKB_NORMAL">Delete</asp:LinkButton>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:LinkButton ID="btn_Save" runat="server" CausesValidation="False" CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" SkinID="LNKB_NORMAL">Save</asp:LinkButton>
                    <asp:LinkButton ID="btn_Cance" runat="server" CausesValidation="False" CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" SkinID="LNKB_NORMAL">Cancel</asp:LinkButton>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:BoundField ItemStyle-Width="100" DataField="Code" HeaderText="Code" HeaderStyle-HorizontalAlign="Left" ReadOnly="true" />
            <asp:BoundField ItemStyle-Width="240" DataField="Description" HeaderText="Description" HeaderStyle-HorizontalAlign="Left" />
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Left" />
                <HeaderTemplate>
                    <asp:Label ID="lbl_IsActive" runat="server" Text="Active" />
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Left" />
                <ItemTemplate>
                    <asp:CheckBox ID="chk_IsActive" runat="server" Checked='<%# Eval("IsActive") %>' onClick="return false;" />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:CheckBox ID="chk_IsActive" runat="server" Checked='<%# Eval("IsActive") %>' />
                </EditItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <!--Popup-->
    <dx:ASPxPopupControl ID="pop_Create" runat="server" ClientInstanceName="pop_Create" Width="380px" Modal="True" HeaderText="New" ShowCloseButton="False"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                <asp:HiddenField runat="server" ID="HiddenField1" />
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lbl_Code" Text="Code" />
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txt_Code"  Width="260" MaxLength="20" onkeyup="this.value=this.value.toUpperCase()"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lbl_Desc" Text="Description" />
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txt_Desc" Width="260" />
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <table width="100%" cellpadding="5">
                    <tr>
                        <td align="right">
                            <asp:Button ID="btn_SaveNew" runat="server" Text="Save" SkinID="BTN_V1" Width="50px" OnClick="btn_SaveNew_Click" />
                        </td>
                        <td align="left">
                            <asp:Button ID="btn_CancelNew" runat="server" Text="Cancel" SkinID="BTN_V1" Width="50px" OnClientClick="pop_Create.Hide();" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_ConfirmDelete" runat="server" ClientInstanceName="pop_ConfirmDelete" Width="300px" Modal="True" HeaderText="Confirm" ShowCloseButton="False"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <asp:HiddenField runat="server" ID="hf_DeleteCode" />
                <asp:HiddenField runat="server" ID="hf_DeleteDesc" />
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Label ID="lbl_ConfirmDelete" runat="server" SkinID="LBL_NR" Text="" />
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Button ID="btn_ConfirmDelete" runat="server" Text="Yes" SkinID="BTN_V1" Width="50px" OnClick="btn_ConfirmDelete_Click" />
                        </td>
                        <td align="left">
                            <asp:Button ID="btn_CancelDelete" runat="server" Text="No" SkinID="BTN_V1" Width="50px" OnClientClick="pop_ConfirmDelete.Hide();" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_Alert" runat="server" ClientInstanceName="pop_Alert" Width="300px" Modal="True" HeaderText="Alert" ShowCloseButton="true" CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter">
        <ContentStyle HorizontalAlign="Center" />
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                <asp:Label ID="lbl_Alert" runat="server" SkinID="LBL_NR" Text="" />
                <br />
                <br />
                <br />
                <asp:Button runat="server" SkinID="BTN_V1" Width="50px" Text="Ok"  OnClientClick="pop_Alert.Hide();" />
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
