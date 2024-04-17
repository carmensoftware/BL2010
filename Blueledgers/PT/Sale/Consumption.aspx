<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Consumption.aspx.cs" Inherits="BlueLedger.PL.PT.Sale.Consumption" MasterPageFile="~/Master/In/SkinDefault.master" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Src="~/UserControl/Spinner.ascx" TagName="Spinner" TagPrefix="uc" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <style>
        .mb-3
        {
            margin-bottom: 10px !important;
        }
        .me-3
        {
            margin-right: 10px;
        }
        .text-end
        {
            text-align: right;
        }
        .w-100
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cph_Main">
    <!-- Title Bar -->
    <div class="mb-3" style="background-color: #4d4d4d; width: 100%; padding: 2px; height: 32px;">
        <div style="margin-left: 10px; float: left; margin-top: 5px;">
            <asp:Label ID="lbl_Title" runat="server" Font-Size="Small" Text="Receipe Details" SkinID="LBL_HD_WHITE" />
        </div>
        <div style="margin-right: 10px; margin-top: 2px; float: right;">
            <dx:ASPxMenu runat="server" ID="menu_CmdBar" Font-Size="Small" Font-Bold="True" BackColor="Transparent" ItemSpacing="5px" VerticalAlign="Middle" OnItemClick="menu_CmdBar_ItemClick">
                <Border BorderStyle="None" />
                <ItemStyle BackColor="WhiteSmoke" ForeColor="Black" Font-Size="Small">
                    <HoverStyle BackColor="Blue" ForeColor="White">
                        <Border BorderStyle="None" />
                    </HoverStyle>
                    <Paddings PaddingLeft="10" PaddingRight="10" />
                    <Border BorderStyle="None" />
                </ItemStyle>
                <Items>
                    <dx:MenuItem Name="Issue" Text="Post to Issue">
                        <ItemStyle BackColor="DarkGray" ForeColor="White" />
                    </dx:MenuItem>
                    <dx:MenuItem Name="Back" Text="Back" />
                </Items>
            </dx:ASPxMenu>
        </div>
    </div>
    <!--Content-->
    <table width="100%">
        <tr>
            <td style="width: 20%; vertical-align: top;">
                <div class="mb-3">
                    <b>Location(s)</b>
                </div>
                <asp:ListBox ID="listbox_Location" runat="server" AutoPostBack="true" Width="90%" Rows="30" OnSelectedIndexChanged="listbox_Location_SelectedIndexChanged">
                </asp:ListBox>
            </td>
            <td style="vertical-align: top;">
                <div class="mb-3">
                    <b>
                        <%= (listbox_Location.SelectedIndex < 0?"":listbox_Location.SelectedItem.Text) %></b>
                </div>
                <asp:GridView ID="gv_Items" runat="server" SkinID="GRD_V2" Width="100%" HeaderStyle-HorizontalAlign="Left">
                    <Columns>
                        <asp:BoundField DataField="ProductCode" HeaderText="Code" />
                        <asp:BoundField DataField="ProductDesc1" HeaderText="Name1" />
                        <asp:BoundField DataField="ProductDesc2" HeaderText="Name2" />
                        <asp:BoundField DataField="Unit" HeaderText="Unit" />
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <div style="text-align: right;">
                                    Quantity
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# FormatQty(Eval("Qty")) %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <!--Popup-->
    <dx:ASPxPopupControl ID="pop_Alert" ClientInstanceName="pop_Alert" runat="server" Width="320" HeaderText="Alert" ShowHeader="true" CloseAction="CloseButton"
        Modal="True" AutoUpdatePosition="True" AllowDragging="True" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" ShowPageScrollbarWhenModal="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl_Alert" runat="server">
                <div style="width: 100%; text-align: center;">
                    <asp:Label ID="lbl_Alert" runat="server" />
                    <br />
                    <br />
                    <br />
                    <asp:Button ID="btn_Alert_Ok" runat="server" Width="80" Text="OK" OnClientClick="pop_Alert.Hide();" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_MissingCode" ClientInstanceName="pop_MissingCode" runat="server" Width="320" HeaderText="Missing code" ShowHeader="true" CloseAction="CloseButton"
        Modal="True" AutoUpdatePosition="True" AllowDragging="True" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" ShowPageScrollbarWhenModal="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <div style="width: 100%; text-align: center;">
                    <asp:Label ID="lbl_MissingCode_Text" runat="server" Font-Bold="true" />
                    <br />
                    <br />
                    <asp:ListBox ID="list_MissingCode" runat="server" Width="100%" Rows="25"></asp:ListBox>
                    <br />
                    <br />
                    <asp:Button runat="server" Width="80" Text="OK" OnClientClick="pop_MissingCode.Hide();" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_Issue" ClientInstanceName="pop_Issue" runat="server" Width="320" HeaderText="Issue" ShowHeader="true" CloseAction="CloseButton"
        Modal="True" AutoUpdatePosition="True" AllowDragging="True" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" ShowPageScrollbarWhenModal="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                <div style="width: 100%; text-align: center;">
                    <div class="mb-3">
                        <b>Type</b>&nbsp;&nbsp;
                        <asp:DropDownList ID="ddl_IssueType" runat="server" AutoPostBack="true" Width="220">
                        </asp:DropDownList>
                    </div>
                    <br />
                    <br />
                    <asp:Button ID="btn_Issue_Create" runat="server" Width="80" Text="Create Issue" OnClick="btn_Issue_Create_Click" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btn_ConfirmDelete_No" runat="server" Width="80" Text="No" OnClientClick="pop_Issue.Hide();" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
