<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="ExportPosting.aspx.cs" Inherits="BlueLedger.PL.Option.Admin.Interface.Sun.ExportPosting" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <script type="text/javascript">
        function PrintPage() {
            var printContent = document.getElementById('<%= printArea.ClientID %>');
            var printWindow = window.open('', '', 'left=50000,top=50000,width=0,height=0');
            printWindow.document.write(printContent.innerHTML);
            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            printWindow.close();
        }
    </script>
    <div style="display: block; background-color: #4d4d4d; padding-left: 5px; padding-right: 5px; height: 24px; width: 100%">
        <div style="display: inline-block;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
        </div>
        <div style="display: inline-block;">
            <asp:Label ID="Label1" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:Option.Admin.Interface.Sun.ExportPosting, lbl_Title %>"></asp:Label>
        </div>
        <div style="display: inline-block; float: right;">
            <asp:LinkButton ID="lnkPrint" runat="server" ForeColor="White" Font-Bold="true" ToolTip="Click to Print All Records" OnClientClick="PrintPage();"><img src="../../../../App_Themes/Default/Images/master/icon/print.png" alt="Print" /></asp:LinkButton>
        </div>
    </div>
    <div style="clear: both;" />
    <div style="display: block; padding-top: 5px;">
        <div style="display: inline-block; vertical-align: top; padding-top: 3px;">
            <asp:Label ID="lbl_FromDate_Nm" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.ExportPosting, lbl_FromDate_Nm %>" SkinID="LBL_HD" />
        </div>
        <div style="display: inline-block; vertical-align: top;">
            <dx:ASPxDateEdit ID="txt_FromDate" ClientInstanceName="txt_FromDate" runat="server" DisplayFormatString="dd/MM/yyyy" EditFormat="Custom" EditFormatString="dd/MM/yyyy" />
            <asp:RequiredFieldValidator ID="fromDate" runat="server" ControlToValidate="txt_FromDate" ErrorMessage="*" ForeColor="Red" Font-Bold="true" Font-Size="Larger" />
        </div>
        <div style="display: inline-block; vertical-align: top; padding-top: 3px;">
            <asp:Label ID="lbl_ToDate_Nm" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.ExportPosting, lbl_ToDate_Nm %>" SkinID="LBL_HD"></asp:Label>
        </div>
        <div style="display: inline-block; vertical-align: top;">
            <dx:ASPxDateEdit ID="txt_ToDate" ClientInstanceName="txt_ToDate" runat="server" DisplayFormatString="dd/MM/yyyy" EditFormat="Custom" EditFormatString="dd/MM/yyyy" />
            <asp:RequiredFieldValidator ID="toDate" runat="server" ControlToValidate="txt_ToDate" ErrorMessage="*" ForeColor="Red" Font-Bold="true" Font-Size="Larger" />
        </div>
        <div style="display: inline-block; vertical-align: top;">
            <asp:Button ID="btn_Preview" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.ExportPosting, btn_Preview %>" Width="60px" SkinID="BTN_V1"
                OnClick="btn_Preview_Click" />
        </div>
        <div style="display: inline-block; vertical-align: top;">
            <asp:Button ID="btn_Export" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.ExportPosting, btn_Export %>" Width="60px" SkinID="BTN_V1" OnClick="btn_Export_Click"
                OnClientClick="return confirm('Confirm export?');" />
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="printArea" runat="server">
                <asp:GridView ID="grd_Preview2" runat="server" AutoGenerateColumns="False" EnableModelValidation="True" Width="100%" SkinID="GRD_V1" OnRowDataBound="grd_Preview2_RowDataBound"    >
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
                    </Columns>
                </asp:GridView>
            </div>
            <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="UpdatePanel">
                <ProgressTemplate>
                    <div class="fix-layout" style="border-style: solid; border-width: 1px; border-color: #0071BD; background-color: #FFFFFF; width: 120px; height: 60px">
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
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
