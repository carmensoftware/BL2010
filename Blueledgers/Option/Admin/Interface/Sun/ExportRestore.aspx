<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="ExportRestore.aspx.cs" Inherits="BlueLedger.PL.Option.Admin.Interface.Sun.ExportRestore" %>
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Header" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
    </style>
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

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <div class="flex flex-justify-content-between flex-align-items-center mb-10" style="background-color: #2196f3;">
        <div class="flex flex-align-items-center">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
            <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_HD_WHITE" Text="Restore Export"></asp:Label>
        </div>
        <div>
            <asp:LinkButton ID="lnkPrint" runat="server" ForeColor="White" Font-Bold="true" ToolTip="Click to Print All Records" OnClientClick="PrintPage();"><img src="../../../../App_Themes/Default/Images/master/icon/print.png" alt="Print" /></asp:LinkButton>
        </div>
    </div>
    <div class="flex flex-justify-content-between flex-align-items-center mb-10">
        <div class="flex flex-align-items-center">
            <asp:Label ID="lbl_FromDate_Nm" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.ExportPosting, lbl_FromDate_Nm %>" SkinID="LBL_HD" />
            &nbsp;&nbsp;
            <dx:ASPxDateEdit ID="de_FromDate" ClientInstanceName="txt_FromDate" runat="server" DisplayFormatString="dd/MM/yyyy" EditFormat="Custom" EditFormatString="dd/MM/yyyy" />
            &nbsp;&nbsp;
            <asp:Label ID="lbl_ToDate_Nm" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.ExportPosting, lbl_ToDate_Nm %>" SkinID="LBL_HD"></asp:Label>
            &nbsp;&nbsp;
            <dx:ASPxDateEdit ID="de_ToDate" ClientInstanceName="txt_ToDate" runat="server" DisplayFormatString="dd/MM/yyyy" EditFormat="Custom" EditFormatString="dd/MM/yyyy" />
            &nbsp;&nbsp;
            <asp:Button ID="btn_Preview" runat="server" Width="60px" SkinID="BTN_V1" Text="Preview" OnClick="btn_Preview_Click" />
        </div>
        <div>
            <asp:Button ID="btn_Restore" runat="server" Width="60px" SkinID="BTN_V1" Text="Restore" OnClick="btn_Restore_Click" OnClientClick="return confirm('Do you want to restore all items?')" />
        </div>
    </div>
    <hr />
    <div id="printArea" runat="server">
        <asp:GridView ID="gv_Data" runat="server" AutoGenerateColumns="False" Width="100%" GridLines="None" AllowPaging="true" PageSize="100" OnRowDataBound="gv_Data_RowDataBound"
            OnPageIndexChanging="gv_Data_PageIndexChanging">
            <HeaderStyle HorizontalAlign="Left" Height="32" Font-Size="Small" BackColor="#2196f3" ForeColor="White" BorderStyle="Solid" BorderColor="#2196f3" />
            <RowStyle Height="24" />
            <PagerStyle Font-Size="Medium" HorizontalAlign="Center" />
            <Columns>
                <asp:BoundField HeaderText="Doc. Date" DataField="DocDate" DataFormatString="{0:d}"></asp:BoundField>
                <asp:BoundField HeaderText="Doc. No." DataField="DocNo"></asp:BoundField>
                <asp:BoundField HeaderText="Doc. Type" DataField="DocType"></asp:BoundField>
                <asp:BoundField HeaderText="Invoice Date" DataField="InvoiceDate" DataFormatString="{0:d}"></asp:BoundField>
                <asp:BoundField HeaderText="Invoice No." DataField="InvoiceNo"></asp:BoundField>
                <asp:BoundField HeaderText="Vendor" DataField="VendorCode"></asp:BoundField>
                <asp:BoundField HeaderText="Description" DataField="Description"></asp:BoundField>
                <asp:BoundField HeaderText="Dr./Cr." DataField="DrCr"></asp:BoundField>
                <asp:BoundField HeaderText="Amount" DataField="Amount" DataFormatString="{0:###,###.00}">
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        &nbsp;&nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:BoundField HeaderText="Sun Account No" DataField="SunAccountNo"></asp:BoundField>--%>
                <asp:TemplateField HeaderText="Sun Account No.">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lbl_SunAccountNo" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="A1">
                    <HeaderTemplate>
                        <asp:Label runat="server" ID="lbl_A1_Header" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lbl_A1" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="A2">
                    <HeaderTemplate>
                        <asp:Label runat="server" ID="lbl_A2_Header" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lbl_A2" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Exported">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:Image runat="server" ID="img_ExportStatus" ImageUrl="~/App_Themes/Default/Images/check.png" />
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:TemplateField HeaderText="Remark">
                            <ItemTemplate>
                                &nbsp;&nbsp;
                                <asp:Label runat="server" ID="lbl_Remark" />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
            </Columns>
        </asp:GridView>
    </div>
    <!-- Popup(s)-->
    <dx:ASPxPopupControl ID="pop_Alert" ClientInstanceName="pop_Alert" runat="server" Width="420" HeaderText="<%$ Resources:PC_REC_RecEdit, Warning %>" Modal="True"
        ShowCloseButton="true" CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="True">
        <HeaderStyle BackColor="#ffffcc" />
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl_Alert" runat="server">
                <div class="flex flex-justify-content-center mt-20 mb-20 width-100">
                    <asp:Label ID="lbl_Alert" runat="server" SkinID="LBL_NR" Font-Size="Small"></asp:Label>
                </div>
                <div class="flex flex-justify-content-center mt-20 width-100">
                    <button style="width: 100px; padding: 5px;" onclick="pop_Alert.Hide();">
                        Ok
                    </button>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    
</asp:Content>
