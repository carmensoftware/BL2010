<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="ExportRestore.aspx.cs" Inherits="BlueLedger.PL.Option.Admin.Interface.Sun.ExportRestore" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <style type="text/css">
        .normalrow
        {
            border-style: none;
            cursor: pointer;
        }
        .hightlighrow
        {
            border-style: solid;
            border-color: #4d4d4d;
            border-width: 1px;
        }
    </style>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td align="left">
                <table border="0" cellpadding="2" cellspacing="0" width="100%">
                    <tr style="background-color: #4d4d4d; height: 17px; padding-left: 10px">
                        <td style="padding-left: 10px; width: 10px">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                        </td>
                        <td style="background-color: #4d4d4d;">
                            <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:Option.Admin.Interface.Sun.ExportRestore, lbl_Title %>"></asp:Label>
                        </td>
                        <td style="padding: 0px 10px 0px 0px; background-color: #4D4D4D" align="right">
                            <dx:ASPxMenu runat="server" ID="ASPxMenu2" Font-Bold="True" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle"
                                Height="16px" OnItemClick="menu_CmdBar_ItemClick">
                                <ItemStyle BackColor="Transparent">
                                    <HoverStyle BackColor="Transparent">
                                        <Border BorderStyle="None" />
                                    </HoverStyle>
                                    <Paddings Padding="2px" />
                                    <Border BorderStyle="None" />
                                </ItemStyle>
                                <Items>
                                    <%--<dx:MenuItem Name="Create" Text="">
                                        <ItemStyle Height="16px" Width="49px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/create.png"
                                                Repeat="NoRepeat" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Delete" Text="">
                                        <ItemStyle Height="16px" Width="38px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png"
                                                Repeat="NoRepeat" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>--%>
                                    <%--<dx:MenuItem Name="Print" Text="">
                                        <ItemStyle Height="16px" Width="46px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-print.png" Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>--%>
                                </Items>
                            </dx:ASPxMenu>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table border="0" cellpadding="1" cellspacing="0">
        <tr valign="middle" style="height: 25px">
            <td style="height: 17px; padding-left: 10px">
                <asp:Label ID="lbl_FromDate_Nm" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.ExportRestore, lbl_FromDate_Nm %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td>
                <dx:ASPxDateEdit ID="txt_FromDate" ClientInstanceName="txt_FromDate" runat="server" DisplayFormatString="dd/MM/yyyy" EditFormat="Custom" EditFormatString="dd/MM/yyyy">
                </dx:ASPxDateEdit>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="fromDate" runat="server" ControlToValidate="txt_FromDate" ErrorMessage="*" ForeColor="Red" Font-Bold="true" Font-Size="Larger"></asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:Label ID="lbl_ToDate_Nm" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.ExportRestore, lbl_ToDate_Nm %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td>
                <dx:ASPxDateEdit ID="txt_ToDate" ClientInstanceName="txt_ToDate" runat="server" DisplayFormatString="dd/MM/yyyy" EditFormat="Custom" EditFormatString="dd/MM/yyyy">
                </dx:ASPxDateEdit>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="toDate" runat="server" ControlToValidate="txt_ToDate" ErrorMessage="*" ForeColor="Red" Font-Bold="true" Font-Size="Larger"></asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:Button ID="btn_Preview" runat="server" OnClick="btn_Preview_Click" Text="<%$ Resources:Option.Admin.Interface.Sun.ExportRestore, btn_Preview %>" SkinID="BTN_V1" />
            </td>
            <td>
                <asp:Button ID="btn_Restore" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.ExportRestore, btn_Restore %>" OnClick="btn_Restore_Click" SkinID="BTN_V1" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="divPv" runat="server">
                <asp:GridView ID="grd_Preview2" runat="server" AutoGenerateColumns="False" EnableModelValidation="True" SkinID="GRD_V1" Width="100%">
                    <Columns>
                        <asp:BoundField HeaderText="<%$ Resources:Option.Admin.Interface.Sun.ExportPosting, DocDate %>" DataField="DocDate" DataFormatString="{0:d}">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="<%$ Resources:Option.Admin.Interface.Sun.ExportPosting, DocNo %>" DataField="DocNo">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="<%$ Resources:Option.Admin.Interface.Sun.ExportPosting, Doctype %>" DataField="RecordType">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="<%$ Resources:Option.Admin.Interface.Sun.ExportPosting, InvoiceDate %>" DataField="InvoiceDate" DataFormatString="{0:d}">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="<%$ Resources:Option.Admin.Interface.Sun.ExportPosting, InvoiceNo %>" DataField="InvoiceNo">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="<%$ Resources:Option.Admin.Interface.Sun.ExportPosting, VendorCode %>" DataField="VendorCode">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Description" DataField="Description">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Account No." DataField="TheAccountNumber">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Dr./Cr." DataField="DBCR">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="<%$ Resources:Option.Admin.Interface.Sun.ExportPosting, RecordAmount %>" DataField="RecordAmount" DataFormatString="{0:###,###.00}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ExportStatus" HeaderText="Exported">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <dx:ASPxPopupControl runat="server" ID="pop_Confrim" Width="360px" HeaderText="Confirmation" ShowCloseButton="False" CloseAction="CloseButton" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" colspan="2" height="50px">
                            <asp:Label ID="lbl_TitleConfirm" runat="server" width="360" SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Button ID="btn_Confrim" runat="server" SkinID="BTN_V1" Width="50px" Text="<%$ Resources:Option.Admin.Interface.Sun.ExportRestore, btn_Yes %>" OnClick="btn_Confrim_Click" />
                        </td>
                        <td align="left">
                            <asp:Button ID="btn_Cancel" runat="server" SkinID="BTN_V1" Width="50px" Text="<%$ Resources:Option.Admin.Interface.Sun.ExportRestore, btn_No %>" OnClick="btn_Cancel_Click" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
