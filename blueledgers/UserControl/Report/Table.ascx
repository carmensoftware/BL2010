<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Table.ascx.cs" Inherits="BlueLedger.PL.UserControls.Report.Table" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1.Export, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView.Export" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>
<table class="GENERAL_BG1" border="0" cellpadding="0" cellspacing="0" width="1024">
    <tr>
        <td width="15" height="15">
        </td>
        <td width="994" height="15">
        </td>
        <td width="15" height="15">
        </td>
    </tr>
    <tr>
        <td width="15" height="15">
        </td>
        <td width="994" height="15">
            <table class="GENERAL_BG2" border="0" cellpadding="0" cellspacing="0" width="994">
                <tr>
                    <td width="15" height="15">
                    </td>
                    <td width="964" height="15">
                    </td>
                    <td width="15" height="15">
                    </td>
                </tr>
                <tr>
                    <td width="15" height="15">
                    </td>
                    <td width="964" height="15" align="right">
                        <table border="0" cellpadding="0" cellspacing="0" width="290">
                            <tr>
                                <td width="60" align="right">
                                    <asp:Label ID="lbl_SelectExtraReport_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="Save as :"></asp:Label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="ddl_SelectTypeReport" runat="server">
                                        <Items>
                                            <dxe:ListEditItem Text="PDF Format" Value="Pdf" />
                                            <dxe:ListEditItem Text="RTF Format" Value="Rtf" />
                                            <dxe:ListEditItem Text="XLS Format" Value="Xls" />
                                        </Items>
                                    </dxe:ASPxComboBox>
                                </td>
                                <td>
                                    <asp:Button ID="btn_Ok" runat="server" Text="Ok" OnClick="btn_Ok_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="15" height="15">
                    </td>
                </tr>
                <tr>
                    <td width="15" height="15">
                    </td>
                    <td width="964" height="15">
                    </td>
                    <td width="15" height="15">
                    </td>
                </tr>
                <tr>
                    <td width="15" height="15">
                    </td>
                    <td width="964" height="15">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td colspan="4" height="5px">
                                    <cc1:ASPxGridViewExporter ID="grdexp_Table" runat="server" GridViewID="grd_Table">
                                    </cc1:ASPxGridViewExporter>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <dxwgv:ASPxGridView ID="grd_Table" runat="server" Width="100%">
                                    </dxwgv:ASPxGridView>
                                    private void Page_Setting() {</td>
                            </tr>
                        </table>
                    </td>
                    <td width="15" height="15">
                    </td>
                </tr>
                <tr>
                    <td width="15" height="15">
                    </td>
                    <td width="994" height="15">
                    </td>
                    <td width="15" height="15">
                    </td>
                </tr>
            </table>
        </td>
        <td width="15" height="15">
        </td>
    </tr>
    <tr>
        <td width="15" height="15">
        </td>
        <td width="994" height="15">
        </td>
        <td width="15" height="15">
        </td>
    </tr>
</table>
