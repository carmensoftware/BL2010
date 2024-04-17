<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="RoleView.aspx.cs" Inherits="BlueLedger.PL.Option.Admin.Security.Role.RoleView" %>

<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <style type="text/css">
        .box
        {
            width: 100%;
            border: 0px solid white;
            box-shadow: 0.5px 1px 2.5px 1px rgba(0,0,0,0.1);
            transition: 0.3s;
        }
    </style>
    <!-- Toolbar -->
    <div class="CMD_BAR">
        <div class="CMD_BAR_LEFT">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
            <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:Option_Admin_Security_Role_Role, lbl_Title %>" SkinID="LBL_HD_WHITE"></asp:Label>
        </div>
        <div class="CMD_BAR_RIGHT">
            <dx:ASPxMenu ID="menu_CmdBar0" runat="server" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle" Height="16px"
                OnItemClick="menu_ItemClick">
                <ItemStyle BackColor="Transparent">
                    <HoverStyle BackColor="Transparent">
                        <Border BorderStyle="None" />
                    </HoverStyle>
                    <Paddings Padding="2px" />
                    <Border BorderStyle="None" />
                </ItemStyle>
                <Items>
                    <dx:MenuItem Name="Save" ToolTip="Save" Text="">
                        <ItemStyle Height="16px" Width="38px" HoverStyle-BackColor="Gray" Cursor="pointer">
                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/save.png" Repeat="NoRepeat" VerticalPosition="center" />
                        </ItemStyle>
                    </dx:MenuItem>
                    <dx:MenuItem Name="Delete" ToolTip="Delete" Text="">
                        <ItemStyle Height="16px" Width="38px" HoverStyle-BackColor="Gray" Cursor="pointer">
                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png" Repeat="NoRepeat" VerticalPosition="center" />
                        </ItemStyle>
                    </dx:MenuItem>
                    <dx:MenuItem Name="Print" ToolTip="Print" Text="">
                        <ItemStyle Height="16px" Width="38px" HoverStyle-BackColor="Gray" Cursor="pointer">
                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat" VerticalPosition="center" />
                        </ItemStyle>
                    </dx:MenuItem>
                    <dx:MenuItem Name="Back" ToolTip="Back" Text="">
                        <ItemStyle Height="16px" Width="38px" HoverStyle-BackColor="Gray" Cursor="pointer">
                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat" VerticalPosition="center" />
                        </ItemStyle>
                    </dx:MenuItem>
                </Items>
            </dx:ASPxMenu>
        </div>
    </div>
    <!-- Content -->
    <div class="printable">
        <div class="container">
            <div class="row">
                <div class="col-1">
                    <asp:Label ID="lbl_Role_Nm" runat="server" Text="Role" SkinID="LBL_HD"></asp:Label>
                </div>
                <div class="col-4">
                    <%--<asp:TextBox ID="txt_RoleName" runat="server" class="half-width"></asp:TextBox>--%>
                    <asp:TextBox ID="txt_RoleDesc" runat="server" class="half-width"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-1">
                    <asp:Label ID="Label2" runat="server" Text="Status:" SkinID="LBL_HD"></asp:Label>
                </div>
                <div class="col-4">
                    <asp:CheckBox ID="chk_IsActive" runat="server" Text="Active" />
                </div>
                <div class="col-6" align="right">
                    <asp:Button ID="btn_RoleType" runat="server" Text="Set PR Type" OnClick="btn_RoleType_Click" />
                </div>
            </div>
            <br />
            <hr />
            <div>
                <asp:Label ID="lbl_test" runat="server" SkinID="LBL_HD" Font-Size="Larger"></asp:Label>
                <asp:GridView ID="gv_Test01" runat="server" AutoGenerateColumns="true">
                </asp:GridView>
                <asp:GridView ID="gv_Test02" runat="server" AutoGenerateColumns="true">
                </asp:GridView>
            </div>
            <div class="row">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 70%;">
                                    <asp:GridView ID="gv_App" runat="server" AutoGenerateColumns="False" ShowHeader="false" GridLines="None" Width="90%" OnRowDataBound="gv_App_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderStyle Width="100%" />
                                                <ItemStyle Width="100%" />
                                                <ItemTemplate>
                                                    <fieldset id="fieldRow" runat="server" class="box">
                                                        <legend></legend>
                                                        <p>
                                                            <asp:Label ID="lbl_Module" runat="server" SkinID="LBL_HD" Font-Size="Larger" />
                                                        </p>
                                                        <p>
                                                            <asp:GridView ID="gv_Module" runat="server" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White" AutoGenerateColumns="false" OnRowDataBound="gv_Module_RowDataBound">
                                                                <Columns>
                                                                    <%--<asp:BoundField DataField="ID" HeaderText="ID" Visible="false" />--%>
                                                                    <asp:TemplateField>
                                                                        <ItemStyle Width="200" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_ID" runat="server" Style="display: none;"></asp:Label>
                                                                            <asp:Label ID="lbl_Parent" runat="server" Style="display: none;"></asp:Label>
                                                                            <asp:Label ID="lbl_Desc" runat="server"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <%-- <asp:BoundField DataField="Desc" HeaderText="Name" Visible="true" ItemStyle-Width="200" />--%>
                                                                    <%--<asp:BoundField DataField="Parent" HeaderText="Parent" Visible="false" />--%>
                                                                    <asp:TemplateField HeaderText="View" ItemStyle-Width="50">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkView" runat="server" AutoPostBack="true" OnCheckedChanged="chkView_CheckedChanged" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Create/Edit" ItemStyle-Width="50">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkEdit" runat="server" AutoPostBack="true" OnCheckedChanged="chkEdit_CheckedChanged" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Delete" ItemStyle-Width="50">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkDelete" runat="server" AutoPostBack="true" OnCheckedChanged="chkDelete_CheckedChanged" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <tr>
                                                                                <td colspan="999">
                                                                                    <asp:Panel ID="pn_SubModule" runat="server" Style="padding: 10px;">
                                                                                        <asp:GridView ID="gv_SubModule" runat="server" ShowHeader="true" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White" AutoGenerateColumns="false"
                                                                                            OnRowDataBound="gv_SubModule_RowDataBound">
                                                                                            <Columns>
                                                                                                <%--<asp:BoundField DataField="ID" HeaderText="ID" Visible="false" />--%>
                                                                                                <asp:TemplateField>
                                                                                                    <ItemStyle Width="187" />
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lbl_ID" runat="server" Style="display: none;"></asp:Label>
                                                                                                        <asp:Label ID="lbl_Parent" runat="server" Style="display: none;"></asp:Label>
                                                                                                        <asp:Label ID="lbl_Desc" runat="server"></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <%-- <asp:BoundField DataField="Desc" HeaderText="Name" Visible="true" ItemStyle-Width="187" />--%>
                                                                                                <%-- <asp:BoundField DataField="Parent" HeaderText="Parent" Visible="false" />--%>
                                                                                                <asp:TemplateField HeaderText="View" ItemStyle-Width="50">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:CheckBox ID="chkView" runat="server" AutoPostBack="true" OnCheckedChanged="chkView_CheckedChanged" />
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Create/Edit" ItemStyle-Width="50">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:CheckBox ID="chkEdit" runat="server" AutoPostBack="true" OnCheckedChanged="chkEdit_CheckedChanged" />
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Delete" ItemStyle-Width="50">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:CheckBox ID="chkDelete" runat="server" AutoPostBack="true" OnCheckedChanged="chkDelete_CheckedChanged" />
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                            </Columns>
                                                                                        </asp:GridView>
                                                                                    </asp:Panel>
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </p>
                                                    </fieldset>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td style="vertical-align: top;">
                                    <asp:GridView ID="gv_ProdType" runat="server" Width="100%" AutoGenerateColumns="false"  HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White" GridLines="None" BorderStyle="None"
                                    OnRowDataBound="gv_ProdType_RowDataBound">
                                        <%--<HeaderStyle Height="40px" BackColor="#F4F4F5" Font-Bold="True" ForeColor="#444444" HorizontalAlign="Left" />
                                        <RowStyle Height="40px" BackColor="White" ForeColor="#333333" BorderColor="#DDDDDD" />--%>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cb_ProdType" runat="server" />
                                                    <asp:Label ID="lbl_TypeCode" runat="server" Style="display: none;"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Assigned PR Type" DataField="Description" HeaderStyle-HorizontalAlign="Left" />
                                        </Columns>
                                    </asp:GridView>
                                    <%--<asp:GridView ID="gv_ProdType" runat="server" AutoGenerateColumns="false" Width="100%" BackColor="White" BorderColor="#DDDDDD" GridLines="Horizontal" Font-Names="Arial"
                                        Font-Size="12px" OnRowDataBound="gv_ProdType_RowDataBound">
                                        <HeaderStyle Height="40px" BackColor="#F4F4F5" Font-Bold="True" ForeColor="#444444" HorizontalAlign="Left" />
                                        <RowStyle Height="40px" BackColor="White" ForeColor="#333333" BorderColor="#DDDDDD" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemStyle Width="20%" HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cb_ProdType" runat="server" />
                                                    <asp:Label ID="lbl_TypeCode" runat="server" Style="display: none;"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="PR Type Avaliable" DataField="Description" />
                                        </Columns>
                                    </asp:GridView>--%>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="row">
            </div>
        </div>
    </div>
    <!-- Popup Panel -->
    <dx:ASPxPopupControl ID="pop_ConfrimDelete" runat="server" Width="300px" CloseAction="CloseButton" HeaderText="<%$ Resources:Option_Admin_Security_Role_Role, Msg %>"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" colspan="2" height="50px">
                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Option_Admin_Security_Role_Role, MsgWarning %>" SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <%--<dx:ASPxButton ID="btn_ConfrimDelete" runat="server" OnClick="btn_ConfrimDelete_Click"
                                Text="Yes" Width="50px">
                            </dx:ASPxButton>--%>
                            <asp:Button ID="btn_ConfrimDelete" runat="server" OnClick="btn_ConfrimDelete_Click" Text="<%$ Resources:Option_Admin_Security_Role_Role, btn_Yes %>" Width="50px"
                                SkinID="BTN_V1" />
                        </td>
                        <td align="left">
                            <%--<dx:ASPxButton ID="btn_CancelDelete" runat="server" OnClick="btn_CancelDelete_Click"
                                Text="No" Width="50px">
                            </dx:ASPxButton>--%>
                            <asp:Button ID="btn_CancelDelete" runat="server" OnClick="btn_CancelDelete_Click" Text="<%$ Resources:Option_Admin_Security_Role_Role, btn_No %>" Width="50px"
                                SkinID="BTN_V1" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle HorizontalAlign="Left" />
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_RolePermissionSaveSuccess" runat="server" Width="300px" HeaderText="<%$ Resources:Option_Admin_Security_Role_Role, Msg2 %>"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                <div align="center">
                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Option_Admin_Security_Role_Role, MsgWarning2 %>" SkinID="LBL_NR"></asp:Label>
                    <br />
                    <br />
                    <asp:Button ID="btn_OK_SaveSuccess" runat="server" Text="OK" OnClick="btn_OK_SaveSuccess_Click" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle HorizontalAlign="Left" />
    </dx:ASPxPopupControl>
    <%--HeaderText="<%$ Resources:Option_Admin_Security_Role_Role, ProdTypeHeader %>"--%>
    <dx:ASPxPopupControl ID="pop_SetProdType" runat="server" Width="380px" Height="600px" HeaderText="Avaliable" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ContentCollection>
            <dx:PopupControlContentControl>
                <div>
                    <%--<asp:GridView ID="gv_ProdType" runat="server" AutoGenerateColumns="false" Width="100%" BackColor="White" BorderColor="#DDDDDD" GridLines="Horizontal" Font-Names="Arial"
                        Font-Size="12px" OnRowDataBound="gv_ProdType_RowDataBound">
                        <HeaderStyle Height="40px" BackColor="#F4F4F5" Font-Bold="True" ForeColor="#444444" HorizontalAlign="Left" />
                        <RowStyle Height="40px" BackColor="White" ForeColor="#333333" BorderColor="#DDDDDD" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle Width="20%" HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="cb_ProdType" runat="server" />
                                    <asp:Label ID="lbl_TypeCode" runat="server" Style="display: none;"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="PR Type Avaliable" DataField="Description" />
                        </Columns>
                    </asp:GridView>--%>
                </div>
                <div>
                    <asp:Button ID="btn_OKSetProdType" runat="server" Text="OK" OnClick="btn_OKSetProdType_Click" />&nbsp
                    <asp:Button ID="btn_CancelProdType" runat="server" Text="Cancel" OnClick="btn_CancelProdType_Click" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_Message" runat="server" HeaderText="" Modal="True" PopupHorizontalAlign="WindowCenter" Width="300px" PopupVerticalAlign="WindowCenter">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <div align="center">
                    <asp:Label ID="pop_lbl_message" runat="server"></asp:Label>
                    <br />
                    <br />
                    <asp:Button ID="pop_btn_OK" runat="server" Text="OK" OnClick="pop_btn_OK_ToCloseClick" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle HorizontalAlign="Left" />
    </dx:ASPxPopupControl>
</asp:Content>
