<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="EOP.aspx.cs" Inherits="BlueLedger.PL.PC.EOP.EOP" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Src="~/UserControl/Comment2.ascx" TagName="Comment2" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Attach2.ascx" TagName="Attach2" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/Log2.ascx" TagName="Log2" TagPrefix="uc3" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%--<%@ Reference Control="~/UserControl/Comment.ascx" %>--%>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
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
                                        <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:PC_EOP_EOP, lbl_Title %>"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right">
                            <table border="0" cellpadding="1" cellspacing="0">
                                <tr>
                                    <td>
                                        <dx:ASPxButton ID="btn_UpdateProduct" runat="server" Text="Update Product" OnClick="btn_UpdateProduct_Click" />
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btn_Import" runat="server" Text="Import" OnClick="btn_Import_Click" />
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btn_Export" runat="server" Text="Export" OnClick="btn_Export_Click" />
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btn_Edit" runat="server" BackColor="Transparent" Height="16px" Width="38px" ToolTip="Edit" OnClick="btn_Edit_Click">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/edit.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-edit.png" Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <Border BorderStyle="None" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btn_Commit" runat="server" Width="51px" Height="16px" BackColor="Transparent" ToolTip="Commit" OnClick="btn_Commit_Click">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/commit.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                            <HoverStyle>
                                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/gray-commit.png" Repeat="NoRepeat" VerticalPosition="center" HorizontalPosition="center" />
                                            </HoverStyle>
                                            <Border BorderStyle="None" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btn_Print" runat="server" BackColor="Transparent" Height="16px" Width="43px" ToolTip="Print" OnClick="btn_Print_Click">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-print.png" Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <Border BorderStyle="None" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btn_Back" runat="server" BackColor="Transparent" Height="16px" Width="42px" ToolTip="Back" OnClick="btn_Back_Click" CausesValidation="False">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png" Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <Border BorderStyle="None" />
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" class="TABLE_HD">
                    <tr>
                        <td rowspan="3" style="width: 1%;">
                        </td>
                        <td style="width: 7%;">
                            <asp:Label ID="lbl_Store_Nm" runat="server" Text="<%$ Resources:PC_EOP_EOP, lbl_Store_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td style="width: 25%;">
                            <asp:Label ID="lbl_Store" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                        <td style="width: 3%;">
                            <asp:Label ID="lbl_Date_Nm" runat="server" Text="<%$ Resources:PC_EOP_EOP, lbl_Date_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td style="width: 7%;">
                            <asp:Label ID="lbl_Date" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                        <td style="width: 9%;">
                            <asp:Label ID="lbl_EndDate_Nm" runat="server" Text="<%$ Resources:PC_EOP_EOP, lbl_EndDate_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_EndDate" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_Desc_Nm" runat="server" Text="<%$ Resources:PC_EOP_EOP, lbl_Desc_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:Label ID="lbl_Desc" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_Status_Nm" runat="server" Text="<%$ Resources:PC_EOP_EOP, lbl_Status_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_Status" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_Remark_Nm" runat="server" Text="<%$ Resources:PC_EOP_EOP, lbl_Remark_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td colspan="5">
                            <asp:Label ID="lbl_Remark" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="grd_Product" runat="server" SkinID="GRD_V1" AutoGenerateColumns="False" ShowFooter="True" Width="100%" EmptyDataText="No data to display"
                    OnRowDataBound="grd_Product_RowDataBound" OnPageIndexChanging="grd_Product_PageIndexChanging">
                    <Columns>
                        <asp:BoundField HeaderText="<%$ Resources:PC_EOP_EOP, ProductCode %>" DataField="ProductCode">
                            <HeaderStyle Width="10%" HorizontalAlign="Left"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="<%$ Resources:PC_EOP_EOP, Descen %>" DataField="ProductDesc1">
                            <HeaderStyle Width="25%" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="<%$ Resources:PC_EOP_EOP, Descll %>" DataField="ProductDesc2">
                            <HeaderStyle Width="25%" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="<%$ Resources:PC_EOP_EOP, Unit %>" DataField="InventoryUnit">
                            <HeaderStyle Width="5%" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="<%$ Resources:PC_EOP_EOP, Qty %>" DataField="Qty">
                            <HeaderStyle Width="10%" HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Price" DataField="AvgPrice">
                            <HeaderStyle Width="10%" HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Total" DataField="Total">
                            <HeaderStyle Width="10%" HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                    </Columns>
                    <%--<HeaderStyle HorizontalAlign="Left" />--%>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <dx:ASPxPopupControl ID="pop_CommitConfirm" runat="server" ClientInstanceName="popupControl" Height="120px" CloseAction="CloseButton" HeaderText="Confirmation"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Width="240px">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Label ID="lbl_CommitConfirm" runat="server" Text="Do you want to commit?" SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <br />
                        </td>
                    </tr>
                    <tr align="center">
                        <td>
                            <dx:ASPxButton ID="btnOK" runat="server" AutoPostBack="False" Text="Yes" Width="80px" OnClick="btn_CommitConfirm_OK_Click">
                                <ClientSideEvents Click="function(s, e) {
	                                                        popupControl.Hide();
                                                            popupControlProcess.Show();
                                                            // client-side processing is here
                                                            e.processOnServer = true; // do some processing at the server side
                                                          }" />
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="btnCancel" runat="server" AutoPostBack="False" ClientInstanceName="btnCancel" Text="No" Width="80px">
                                <ClientSideEvents Click="function(s, e) {
	                                                            popupControl.Hide();
                                                            }" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_UpdateSuccess" runat="server" Height="120px" CloseAction="CloseButton" HeaderText="<%$ Resources:PC_EOP_EOPEdit, Information %>"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Width="240px">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td align="center">
                            <asp:Label ID="lbl_UpdateSuccess" runat="server" Text="<%$ Resources:PC_EOP_EOPEdit, lbl_UpdateSuccess %>" SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr align="center">
                        <td>
                            <%--<dx:ASPxButton ID="btn_UpdateSuccess_OK" runat="server" OnClick="btn_UpdateSuccess_OK_Click"
                                        Text="OK" Width="50px" SkinID="BTN_N1">
                                    </dx:ASPxButton>--%>
                            <asp:Button ID="btn_UpdateSuccess_OK" runat="server" OnClick="btn_UpdateSuccess_OK_Click" Text="<%$ Resources:PC_EOP_EOPEdit, btn_UpdateSuccess_OK %>"
                                Width="50px" SkinID="BTN_V1" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_Processing" runat="server" ClientInstanceName="popupControlProcess" Width="340px" Height="160px" Modal="True" CloseAction="None"
        HeaderText="<%$ Resources:PC_EOP_EOPEdit, Information %>" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" style="width: 100%;">
                    <tr align="center">
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Committing on progress ..." SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr align="center">
                        <td>
                            <%--<div class="fix-layout" style="border-style: solid; border-width: 1px; border-color: #0071BD;
                                background-color: #FFFFFF; width: 120px; height: 60px">--%>
                            <div style="width: 120px; height: 60px">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 120px; height: 60px">
                                    <tr>
                                        <td align="center">
                                            <asp:Image ID="img_Loading2" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/ajax-loader.gif" EnableViewState="False" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbl_Loading2" runat="server" Font-Bold="true" Text="Loading..." EnableViewState="False"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_Information" runat="server" Height="120px" CloseAction="CloseButton" HeaderText="Information" Modal="True" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" Width="240px" ShowCloseButton="False">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" style="width: 100%;">
                    <tr align="center">
                        <td>
                            <asp:Label ID="Label_Information" runat="server" Text="" SkinID="LBL_NR" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr align="center">
                        <td>
                            <asp:Button ID="btn_Information_OK" runat="server" OnClick="btn_Information_OK_Click" Text="OK" Width="50px" SkinID="BTN_V1" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_ConfirmAdjustIn" runat="server" HeaderText="Confirmation" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        ShowCloseButton="false" CloseAction="CloseButton">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
                <div style="text-align: center; width: 480px;">
                    <asp:Label ID="lbl_ConfirmAdjustIn" runat="server" Text="Some products have physical count more than On-hand. Do you want to commit?" />
                    <div>
                        <br />
                    </div>
                    <div style="text-align: center;">
                        <asp:Button ID="btn_ConfirmAdjustIn_Yes" runat="server" Text="Yes" OnClick="btn_ConfirmAdjustIn_Yes_Click" />
                        &nbsp;&nbsp;
                        <asp:Button ID="btn_ConfirmAdjustIn_No" runat="server" Text="No" OnClick="btn_ConfirmAdjustIn_No_Click" />
                    </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
