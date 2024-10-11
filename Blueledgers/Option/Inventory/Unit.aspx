<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="Unit.aspx.cs" Inherits="BlueLedger.PL.Option.Inventory.Unit" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Header" runat="server" ContentPlaceHolderID="head">
    <script>
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
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <table width="100%" border="0" cellpadding="0" cellspacing="0" style="background-color: #4D4D4D; height: auto;">
        <tr>
            <td align="left" style="padding-left: 10px;">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:Option.Inventory.Unit, lbl_Title %>" SkinID="LBL_HD_WHITE"></asp:Label>
            </td>
            <td align="right">
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
    <br />
    <table width="100%">
        <tr>
            <td>
                <label>
                    View
                </label>
                <asp:DropDownList runat="server" ID="ddl_View" AutoPostBack="true" OnSelectedIndexChanged="ddl_View_SelectedIndexChanged">
                    <asp:ListItem Value="ALL">All</asp:ListItem>
                    <asp:ListItem Value="ACTIVE">Active</asp:ListItem>
                    <asp:ListItem Value="INACTIVE">Inactive</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td align="right">
                <asp:TextBox runat="server" ID="txt_Search" />
                <asp:Button runat="server" ID="btn_Search" Text="Search" OnClick="btn_Search_Click" />
            </td>
        </tr>
    </table>
    <hr />
    <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <ContentTemplate>
            <div class="printable">
                <asp:GridView ID="gv_Data" runat="server" SkinID="GRD_V1" Width="100%" OnRowDataBound="gv_Data_RowDataBound" OnRowEditing="gv_Data_RowEditing" OnRowUpdating="gv_Data_RowUpdating"
                    OnRowCancelingEdit="gv_Data_RowCancelingEdit">
                    <HeaderStyle Height="40px" BackColor="#F4F4F5" Font-Bold="True" ForeColor="#444444" HorizontalAlign="Left" />
                    <RowStyle Height="50px" BackColor="White" ForeColor="#333333" BorderColor="#DDDDDD" />
                    <Columns>
                        <asp:BoundField HeaderText="Code" DataField="UnitCode" ReadOnly="true" />
                        <asp:BoundField HeaderText="Name" DataField="Name" />
                        <asp:TemplateField HeaderText="Active">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chk_IsActived" onclick="return false;" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox runat="server" ID="chk_IsActived" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:HiddenField runat="server" ID="hf_UnitCode" Value='<%# Eval("UnitCode").ToString() %>' />
                                <asp:Button runat="server" ID="btn_Delete" Text="Delete" OnClick="btn_Delete_Click" />
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button runat="server" ID="btn_Edit" Text="Edit" CommandName="Edit" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:HiddenField runat="server" ID="hf_UnitCode" Value='<%# Eval("UnitCode").ToString() %>' />
                                <asp:Button runat="server" ID="btn_Save" Text="Save" CommandName="Update" />
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button runat="server" ID="btn_Cancel" Text="Cancel" CommandName="Cancel" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <!-- Popup -->
            <dx:ASPxPopupControl ID="pop_Alert" ClientInstanceName="pop_Alert" runat="server" Width="300px" CloseAction="CloseButton" HeaderText="Warning" Modal="True"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False" ShowPageScrollbarWhenModal="True">
                <ContentStyle VerticalAlign="Top">
                </ContentStyle>
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_Pop_Alert" runat="server" SkinID="LBL_NR" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btn_Pop_Alert_Ok" runat="server" Width="50px" SkinID="BTN_V1" Text="OK" OnClientClick="pop_Alert.Hide();" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_New" ClientInstanceName="pop_New" runat="server" Width="240" Modal="True" HeaderText="New" ShowCloseButton="False" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="True" AutoUpdatePosition="true">
                <ContentStyle VerticalAlign="Top">
                </ContentStyle>
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                        <div>
                            <b>Code</b>
                        </div>
                        <div>
                            <asp:TextBox runat="server" ID="txt_UnitCode_New" Width="100" />
                        </div>
                        <br />
                        <div>
                            <b>Name</b>
                        </div>
                        <div>
                            <asp:TextBox runat="server" ID="txt_UnitName_New" Width="100" />
                        </div>
                        <br />
                        <div>
                            <asp:CheckBox runat="server" ID="chk_IsActived_New" Text="Active" />
                        </div>
                        <br />
                        <div style="text-align: right">
                            <asp:Button ID="btn_Save_New" runat="server" Width="50px" SkinID="BTN_V1" Text="Save" OnClick="btn_Save_New_Click" />
                            &nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btn_Cancel" runat="server" Width="50px" SkinID="BTN_V1" Text="Cancel" OnClientClick="pop_New.Hide();" />
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_Confirm_Delete" ClientInstanceName="pop_Confirm_Delete" runat="server" Width="300px" CloseAction="CloseButton" HeaderText="Confirm"
                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False" ShowPageScrollbarWhenModal="True">
                <ContentStyle VerticalAlign="Top">
                </ContentStyle>
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                        <asp:HiddenField runat="server" ID="hf_UnitCode_Delete" />
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_Confirm_Delete" runat="server" SkinID="LBL_NR" Text="Do you want to delete?"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button runat="server" ID="btn_Confirm_Delete_Yes" Text="Yes" OnClick="btn_Confirm_Delete_Yes_Click" />
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Button runat="server" ID="btn_Confirm_Delete_No" Width="50px" SkinID="BTN_V1" Text="No" OnClientClick="pop_Confirm_Delete.Hide();" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <asp:UpdateProgress ID="UpdatePanelProgress" runat="server" AssociatedUpdatePanelID="UpdatePanel">
                <ProgressTemplate>
                    <div class="fix-laouy" style="border-style: solid; border-width: 1px; border-color: #0071BD; background-color: #FFFFFF; width: 120px; height: 60px">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 120px; height: 60px">
                            <tr>
                                <td align="center">
                                    <asp:Image ID="img_Loading1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/ajax-loader.gif" EnableViewState="False" />
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
