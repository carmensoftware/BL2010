<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="StoreEdit2.aspx.cs" Inherits="BlueLedger.PL.Option.Store.StoreEdit2" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <table border="0" cellpadding="2" cellspacing="0" style="width: 100%;">
        <tr style="background-color: #4d4d4d; height: 17px; padding-left: 10px">
            <td style="padding-left: 10px; width: 10px">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
            </td>
            <td align="left">
                <asp:Label ID="lbl_StoreLocation_Nm" runat="server" Text="<%$ Resources:Option.Inventory.Store.StoreEdit, lbl_StoreLocation_Nm %>"
                    SkinID="LBL_HD_WHITE"></asp:Label>
            </td>
            <td align="right" style="padding-right: 10px;">
                <dx:ASPxMenu runat="server" ID="menu_CmdBar" Font-Bold="True" BackColor="Transparent"
                    Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle" Height="16px"
                    OnItemClick="menu_CmdBar_ItemClick">
                    <ItemStyle BackColor="Transparent">
                        <HoverStyle BackColor="Transparent">
                            <Border BorderStyle="None" />
                        </HoverStyle>
                        <Paddings Padding="2px" />
                        <Border BorderStyle="None" />
                    </ItemStyle>
                    <Items>
                        <dx:MenuItem Name="Save" Text="">
                            <ItemStyle Height="16px" Width="42px">
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png"
                                        Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/save.png"
                                    Repeat="NoRepeat" VerticalPosition="center" />
                            </ItemStyle>
                        </dx:MenuItem>
                        <dx:MenuItem Name="Back" Text="">
                            <ItemStyle Height="16px" Width="42px">
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png"
                                        Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat"
                                    HorizontalPosition="center" VerticalPosition="center" />
                            </ItemStyle>
                        </dx:MenuItem>
                    </Items>
                    <Paddings Padding="0px" />
                    <SeparatorPaddings Padding="0px" />
                    <SubMenuStyle HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" Font-Size="9pt"
                        ForeColor="#4D4D4D" />
                    <Border BorderStyle="None"></Border>
                </dx:ASPxMenu>
            </td>
        </tr>
    </table>
    <table border="0" cellpadding="2" cellspacing="0" width="100%">
        <tr>
            <td align="left" style="width: 10%; padding-left: 10px">
                <asp:Label ID="lbl_Code_Nm" runat="server" Text="<%$ Resources:Option.Inventory.Store.StoreEdit, lbl_Code_Nm %>"
                    Font-Bold="True" SkinID="LBL_HD"></asp:Label>
            </td>
            <td align="left" style="width: 20%">
                <asp:TextBox ID="txt_Code" runat="server" AutoCompleteType="Disabled" MaxLength="20"
                    Width="150px" SkinID="TXT_V1"></asp:TextBox>
            </td>
            <td align="left" style="width: 10%">
                <asp:Label ID="lbl_Store_Nm" runat="server" Text="<%$ Resources:Option.Inventory.Store.StoreEdit, lbl_Store_Nm %>"
                    Font-Bold="True" SkinID="LBL_HD"></asp:Label>
            </td>
            <td align="left" style="width: 20%">
                <asp:TextBox ID="txt_Store" runat="server" Width="200px" SkinID="TXT_V1"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left" style="width: 10%; padding-left: 10px">
                <asp:Label ID="lbl_EOP_Nm" runat="server" Text="<%$ Resources:Option.Inventory.Store.StoreEdit, lbl_EOP_Nm %>"
                    Font-Bold="True" SkinID="LBL_HD"></asp:Label>
            </td>
            <td align="left" style="width: 20%">
                <dx:ASPxComboBox ID="ddl_Eop" runat="server" ValueType="System.Int32" OnLoad="ddl_Eop_Load"
                    Width="170px">
                    <Items>
                        <dx:ListEditItem Text="Enter Counted Stock" Value="1" />
                        <dx:ListEditItem Text="Default Zero" Value="2" />
                        <dx:ListEditItem Text="Default System" Value="3" />
                    </Items>
                </dx:ASPxComboBox>
            </td>
            <td align="left" style="width: 10%">
                <asp:Label ID="lbl_DelPoint_Nm" runat="server" Text="<%$ Resources:Option.Inventory.Store.StoreEdit, lbl_DelPoint_Nm %>"
                    Font-Bold="True" SkinID="LBL_HD"></asp:Label>
            </td>
            <td align="left" style="width: 20%">
                <dx:ASPxComboBox ID="ddl_DelPoint" runat="server" IncrementalFilteringMode="Contains"
                    ValueType="System.String" TextField="Name" ValueField="DptCode" OnLoad="ddl_DelPoint_Load"
                    Width="200px">
                </dx:ASPxComboBox>
            </td>
            <td align="left" style="width: 10%">
                <asp:Label ID="lbl_AccCode_Nm" runat="server" Text="<%$ Resources:Option.Inventory.Store.StoreEdit, lbl_AccCode_Nm %>"
                    Font-Bold="True" Visible="False" SkinID="LBL_HD"></asp:Label>
            </td>
            <td align="left" style="width: 20%">
                <asp:TextBox ID="txt_AccCode" runat="server" Width="200px" Visible="False" SkinID="TXT_V1">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left" style="width: 10%; padding-left: 10px">
                <asp:Label ID="lbl_IsActive" runat="server" Text="<%$ Resources:Option.Inventory.Store.StoreEdit, lbl_IsActive %>"
                    Font-Bold="True" SkinID="LBL_HD"></asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="chk_IsActive" runat="server" Checked="true" SkinID="CHK_V1" />
            </td>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Store Group" Font-Bold="True" SkinID="LBL_HD"></asp:Label>
            </td>
            <td>
                <dx:ASPxComboBox ID="ddl_StoreGrp" runat="server" IncrementalFilteringMode="Contains"
                    ValueType="System.String" TextField="Code" ValueField="Code" OnLoad="ddl_StoreGrp_Load"
                    Width="200px">
                </dx:ASPxComboBox>
            </td>
        </tr>
    </table>
    <br />
    <dx:ASPxPopupControl ID="pop_CheckSaveNew" runat="server" Width="400px" HeaderText="<%$ Resources:Option.Inventory.Store.StoreEdit, pop_CheckSaveNew %>"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <table>
                    <tr align="center">
                        <td>
                            <asp:Label ID="lbl_CheckSaveNew" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle HorizontalAlign="Left" />
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_Warning" runat="server" Width="300px" CloseAction="CloseButton"
        HeaderText="<%$ Resources:Option.Inventory.Store.StoreEdit, pop_Warning %>" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="left" height="50px" colspan="2">
                            <asp:Label ID="lbl_Warning" runat="server" SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btn_Ok" runat="server" Text="<%$ Resources:Option.Inventory.Store.StoreEdit, btn_Ok %>"
                                SkinID="BTN_V1" Width="50px" OnClick="btn_Ok_Click" />
                        </td>
                        <td align="center">
                            <asp:Button ID="btn_No" runat="server" Text="<%$ Resources:Option.Inventory.Store.StoreEdit, btn_No %>"
                                SkinID="BTN_V1" Width="50px" OnClick="btn_No_Click" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
