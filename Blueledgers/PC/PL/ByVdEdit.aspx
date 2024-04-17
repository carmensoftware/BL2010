<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="ByVdEdit.aspx.cs" Inherits="BlueLedger.PL.PC.PL.ByVdEdit" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <table style="width: 100%;" border="0" cellpadding="5" cellspacing="0">
        <tr>
            <td align="left">
                <table border="0" cellpadding="1" cellspacing="0" style="width: 100%;">
                    <tr style="height: 40px;">
                        <td style="border-bottom: solid 5px #187EB8">
                            <asp:Label ID="Label7" runat="server" Text="Price List by Vendor" Font-Size="13pt"
                                Font-Bold="true"></asp:Label>
                        </td>
                        <td align="right" style="border-bottom: solid 5px #187EB8">
                            <dx:ASPxRoundPanel ID="ASPxRoundPanel" runat="server" SkinID="COMMANDBAR">
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent2" runat="server">
                                        <dx:ASPxMenu ID="menu_CmdBar" runat="server" OnItemClick="menu_CmdBar_ItemClick"
                                            SkinID="COMMAND_BAR">
                                            <Items>
                                                <dx:MenuItem Text="Save">
                                                    <Image Url="~/App_Themes/Default/Images/save.gif">
                                                    </Image>
                                                </dx:MenuItem>
                                                <dx:MenuItem BeginGroup="True" Text="Back">
                                                    <Image Url="~/App_Themes/Default/Images/back.gif">
                                                    </Image>
                                                </dx:MenuItem>
                                                <dx:MenuItem Text="Favotires" Visible="False">
                                                    <Image Url="~/App_Themes/Default/Images/favorites.gif">
                                                    </Image>
                                                </dx:MenuItem>
                                                <dx:MenuItem Visible="False">
                                                    <Template>
                                                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/Images/add_favorites.gif" />
                                                    </Template>
                                                </dx:MenuItem>
                                            </Items>
                                        </dx:ASPxMenu>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>
                        </td>
                    </tr>
                </table>
                <br />
                <table border="0" cellpadding="5" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td align="right" width="15%">
                            <asp:Label ID="lbl_VendorNo_Nm" runat="server" Text="Vendor Code" Font-Bold="True"></asp:Label>
                        </td>
                        <td width="15%">
                            <dx:ASPxComboBox ID="cmb_Vendor" runat="server" Width="200px" OnLoad="cmb_Vendor_Load"
                                IncrementalFilteringMode="Contains" TextField="Name" ValueField="VendorCode"
                                AutoPostBack="True" OnSelectedIndexChanged="cmb_Vendor_SelectedIndexChanged">
                                <Columns>
                                    <dx:ListBoxColumn Caption="Code" FieldName="VendorCode" />
                                    <dx:ListBoxColumn Caption="Name" FieldName="Name" />
                                </Columns>
                            </dx:ASPxComboBox>
                        </td>
                        <td align="right" width="15%">
                            <asp:Label ID="lbl_VendorName_Nm" runat="server" Text="Vendor Name" Font-Bold="True"></asp:Label>
                        </td>
                        <td width="15%">
                            <dx:ASPxTextBox ID="txt_VendorName" runat="server" Width="170px" Enabled="true">
                            </dx:ASPxTextBox>
                        </td>
                        <td align="right" width="15%">
                        </td>
                        <td align="left" width="15%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lbl_DateFrom_Nm" runat="server" Text="Date From" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                            <dx:ASPxDateEdit ID="dte_DateFrom" runat="server" Width="100px" 
                                ondatechanged="dte_DateFrom_DateChanged">
                            </dx:ASPxDateEdit>
                        </td>
                        <td align="right">
                            <asp:Label ID="lbl_DateTo_Nm" runat="server" Text="Date To" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                            <dx:ASPxDateEdit ID="dte_DateTo" runat="server" Width="100px" 
                                ondatechanged="dte_DateTo_DateChanged">
                            </dx:ASPxDateEdit>
                        </td>
                        <td align="right">
                            <asp:Label ID="Label1" runat="server" Text="Reference #" Font-Bold="True"></asp:Label>
                        </td>
                        <td align="left">
                            <dx:ASPxTextBox ID="txt_RefNo" runat="server" Width="200px">
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxPopupControl ID="pop_CheckSaveNew" runat="server" Width="300px" HeaderText="Information"
                    Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                            <asp:Label ID="lbl_CheckSaveNew" runat="server"></asp:Label>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                    <HeaderStyle HorizontalAlign="Left" />
                </dx:ASPxPopupControl>
            </td>
        </tr>
    </table>
</asp:Content>
