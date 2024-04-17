<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="Restore.aspx.cs" Inherits="BlueLedger.PL.Option.Admin.Interface.ACCPAC.Restore" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <table style="width: 100%; border-style: none; border-spacing: 0;">
        <tr style="background-color: #4d4d4d; height: 17px; padding-left: 10px">
            <td style="padding-left: 10px; width: 10px">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
            </td>
            <td style="background-color: #4d4d4d;">
                <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_HD_WHITE" Text="Restore"></asp:Label>
            </td>
            <td style="padding: 0px 10px 0px 0px; background-color: #4D4D4D" align="right">
                <dx:ASPxMenu runat="server" ID="ASPxMenu2" Font-Bold="True" BackColor="Transparent"
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
                        <dx:MenuItem Name="Print" Text="">
                            <ItemStyle Height="16px" Width="46px">
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-print.png"
                                        Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat"
                                    HorizontalPosition="center" VerticalPosition="center" />
                            </ItemStyle>
                        </dx:MenuItem>
                    </Items>
                </dx:ASPxMenu>
            </td>
        </tr>
    </table>
    <table style="width: 100%">
        <tr valign="middle" style="height: 25px">
            <td  style="width: 8%; height: 17px; padding-left: 10px">
                <asp:Label ID="lbl_FromDate" runat="server" Text="From Date:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td style="width: 17%;">
                <dx:ASPxDateEdit ID="txt_FromDate" ClientInstanceName="txt_FromDate" runat="server"
                    Width="100px" DisplayFormatString="dd/MM/yyyy" EditFormat="Custom" EditFormatString="dd/MM/yyyy">
                </dx:ASPxDateEdit>
            </td>
            <td style="width: 8%;">
                <asp:Label ID="lbl_ToDate" runat="server" Text="To Date:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td style="width: 17%;">
                <dx:ASPxDateEdit ID="txt_ToDate" ClientInstanceName="txt_ToDate" runat="server" DisplayFormatString="dd/MM/yyyy"
                    Width="100px" EditFormat="Custom" EditFormatString="dd/MM/yyyy">
                </dx:ASPxDateEdit>
            </td>
            <td style="width: 50%;">
                <asp:Button ID="btn_Restore" runat="server" Text="Restore" SkinID="BTN_V1" Width="60px"
                    OnClick="btn_Restore_Click" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="up_Preview" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="padding-left:10px;">
                <asp:Button ID="btn_Preview" runat="server" Text="Preview" SkinID="BTN_V1" OnClick="btn_Preview_Click" />
            </div>
            <div style="padding-left:10px;">
                <asp:GridView ID="grd_Preview" runat="server" Width="100%" AutoGenerateColumns="false"
                    SkinID="GRD_V1">
                    <HeaderStyle HorizontalAlign="Left" />
                    <Columns>
                        <asp:BoundField DataField="InputDate" DataFormatString="{0:d}" HeaderText="Input Date" />
                        <%--<asp:BoundField DataField="RefNo" HeaderText="Ref#" />--%>
                        <asp:BoundField DataField="Type" HeaderText="Type" />
                        <asp:BoundField DataField="InvoiceDate" DataFormatString="{0:d}" HeaderText="Invoice Date" />
                        <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" />
                        <asp:BoundField DataField="VendorCode" HeaderText="Vendor Code" />
                        <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" />
                        <asp:BoundField DataField="TaxAmount" HeaderText="Tax Amount">
                            <ItemStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount">
                            <ItemStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <dx:ASPxPopupControl ID="pop_Confrim" runat="server" Width="300px" CloseAction="CloseButton"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        ShowCloseButton="False" CssFilePath="" CssPostfix="" SpriteCssFilePath="" HeaderText="">
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" colspan="2" height="50px">
                            <asp:Label ID="lbl_TitleConf" runat="server" Text="Are you sure to restore?" SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Button ID="btn_Confrim" runat="server" Text="Confirm" Width="50px" SkinID="BTN_V1"
                                OnClick="btn_Confrim_Click" />
                        </td>
                        <td align="left">
                            <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" Width="50px" SkinID="BTN_V1"
                                OnClick="btn_Cancel_Click" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
