<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="CopyToProperty.aspx.cs" Inherits="BlueLedger.PL.Option.ProdCat.CopyToProperty" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
        <tr>
            <td align="left">
                <!-- Title & Command Bar  -->
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr style="background-color: #4d4d4d; height: 17px;">
                        <td align="left" style="padding-left: 10px;">
                            <table border="0" cellpadding="2" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_Title" runat="server" Text="Product Category" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right">
                            <dx:ASPxButton ID="btn_Back" runat="server" BackColor="Transparent" Height="16px"
                                Width="42px" ToolTip="Back" OnClick="btn_Back_Click">
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png"
                                    Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png"
                                        Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <Border BorderStyle="None" />
                            </dx:ASPxButton>
                        </td>
                        <%--   </tr>
                </table>
            </td>--%>
                    </tr>
                    <tr>
                        <td align="left" style="padding: 5px">
                            <asp:Label ID="Label2" runat="server" Text="Product Category" Font-Bold="True" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left" style="padding: 5px">
                            <asp:Label ID="Label3" runat="server" Text="Business Unit" Font-Bold="True" SkinID="LBL_HD"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <dx:ASPxTreeList ID="tl_Category" runat="server" AutoGenerateColumns="False" KeyFieldName="CategoryCode"
                                ParentFieldName="ParentNo" OnLoad="tl_Category_Load">
                                <SettingsSelection AllowSelectAll="True" Enabled="True" Recursive="True" />
                                <Columns>
                                    <dx:TreeListTextColumn Caption="Category" FieldName="CategoryName" VisibleIndex="0">
                                    </dx:TreeListTextColumn>
                                </Columns>
                            </dx:ASPxTreeList>
                        </td>
                        <td align="left" valign="top">
                            <dx:ASPxTreeList ID="tl_Bu" runat="server" AutoGenerateColumns="False" KeyFieldName="BuCode"
                                OnLoad="tl_Bu_Load">
                                <SettingsSelection AllowSelectAll="True" Enabled="True" Recursive="True" />
                                <Columns>
                                    <dx:TreeListTextColumn Caption="BU" FieldName="BuName" VisibleIndex="0">
                                    </dx:TreeListTextColumn>
                                </Columns>
                            </dx:ASPxTreeList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="100%" align="right">
                            <%--<dx:ASPxButton ID="btn_Ok" runat="server" Text="OK" OnClick="btn_Ok_Click" SkinID="BTN_N1">
                </dx:ASPxButton>--%>
                            <asp:Button ID="btn_Ok" runat="server" Text="OK" OnClick="btn_Ok_Click" SkinID="BTN_V1"
                                Width="50px" />
                            <asp:HiddenField ID="hf_ConnStr" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <dx:ASPxPopupControl ID="pop_Warning" runat="server" CloseAction="CloseButton" HeaderText="Warning"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        ShowCloseButton="False">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" height="50px">
                            <asp:Label ID="lbl_Warning" runat="server" SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <%--<dx:ASPxButton
    ID="btn_Warning" runat="server" Text="OK" OnClick="btn_Warning_Click" SkinID="BTN_N1">
    </dx:ASPxButton>--%>
                            <asp:Button ID="btn_Warning" runat="server" Text="OK" OnClick="btn_Warning_Click"
                                SkinID="BTN_V1" Width="50px" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_ConfirmCoppy" runat="server" CloseAction="CloseButton"
        Width="300px" ClientInstanceName="pop_ConfirmCoppy" HeaderText="Warning" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" height="50px" colspan="2">
                            <asp:Label ID="Label1" runat="server" Text="Do you want to coppy category to property?"
                                SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <%--<dx:ASPxButton ID="btn_Confirm" runat="server"
    Text="Yes" OnClick="btn_Confirm_Click" SkinID="BTN_N1"> </dx:ASPxButton>--%>
                            <asp:Button ID="btn_Confirm" runat="server" Text="Yes" OnClick="btn_Confirm_Click"
                                SkinID="BTN_V1" Width="50px" />
                        </td>
                        <td align="left">
                            <%--<dx:ASPxButton ID="ASPxButton1" runat="server"
    Text="No" SkinID="BTN_N1"> <ClientSideEvents Click="function(s,e){ pop_ConfirmCoppy.Hide();
    return false; }" /> </dx:ASPxButton>--%>
                            <asp:Button ID="btn_Cancel" runat="server" Text="No" SkinID="BTN_V1" Width="50px"
                                OnClick="btn_Cancel_Click" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
