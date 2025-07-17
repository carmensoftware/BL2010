<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="ExportPosting.aspx.cs" Inherits="BlueLedger.PL.Option.Admin.Interface.Sun.ExportPosting" %>

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
    <div class="flex flex-justify-content-between flex-align-items-center mb-10" style="background-color: #4d4d4d;">
        <div class="flex flex-align-items-center">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
            <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:Option.Admin.Interface.Sun.ExportPosting, lbl_Title %>"></asp:Label>
        </div>
        <div>
            <asp:LinkButton ID="lnkPrint" runat="server" ForeColor="White" Font-Bold="true" ToolTip="Click to Print All Records" OnClientClick="PrintPage();"><img src="../../../../App_Themes/Default/Images/master/icon/print.png" alt="Print" /></asp:LinkButton>
        </div>
    </div>
    <div class="flex flex-justify-content-between flex-align-items-center mb-10">
        <div class="flex flex-align-items-center">
            <asp:Label ID="Label2" runat="server" Text="View" SkinID="LBL_HD" />
            &nbsp;&nbsp;
            <asp:DropDownList ID="ddl_View" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_View_SelectedIndexChanged">
                <asp:ListItem Value="" Text="All" />
                <asp:ListItem Value="N" Text="No Exported" />
                <asp:ListItem Value="E" Text="Exported" />
            </asp:DropDownList>
            &nbsp;&nbsp;
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
            <asp:Button ID="btn_Config" runat="server" Width="80px" SkinID="BTN_V1" Text="Configuration" OnClick="btn_Config_Click" />
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="btn_Export" runat="server" Width="60px" SkinID="BTN_V1" Text="Export" OnClick="btn_Export_Click" OnClientClick="return confirm('Do you want to export?')" />
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
                <%--Column=0--%>
                <asp:BoundField HeaderText="Doc. Date" DataField="DocDate" DataFormatString="{0:d}"></asp:BoundField>
                <%--Column=1--%>
                <asp:BoundField HeaderText="Doc. No." DataField="DocNo"></asp:BoundField>
                <%--Column=2--%>
                <asp:BoundField HeaderText="Doc. Type" DataField="DocType"></asp:BoundField>
                <%--Column=3--%>
                <asp:BoundField HeaderText="Invoice Date" DataField="InvoiceDate" DataFormatString="{0:d}"></asp:BoundField>
                <%--Column=4--%>
                <asp:BoundField HeaderText="Invoice No." DataField="InvoiceNo"></asp:BoundField>
                <%--Column=5--%>
                <asp:BoundField HeaderText="Vendor" DataField="VendorCode"></asp:BoundField>
                <%--Column=6--%>
                <asp:BoundField HeaderText="Description" DataField="Description"></asp:BoundField>
                <%--Column=7--%>
                <asp:BoundField HeaderText="Dr./Cr." DataField="DrCr"></asp:BoundField>
                <%--Column=8--%>
                <asp:BoundField HeaderText="Amount" DataField="Amount" DataFormatString="{0:###,###.00}">
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <%--Column=9--%>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        &nbsp;&nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <%--Column=10--%>
                <%--<asp:BoundField HeaderText="Sun Account No" DataField="SunAccountNo"></asp:BoundField>--%>
                <asp:TemplateField HeaderText="Sun Account No.">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lbl_SunAccountNo" />
                    </ItemTemplate>
                </asp:TemplateField>
                <%--Column=11--%>
                <asp:TemplateField HeaderText="A1">
                    <HeaderTemplate>
                        <asp:Label runat="server" ID="lbl_A1_Header" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lbl_A1" />
                    </ItemTemplate>
                </asp:TemplateField>
                <%--Column=12--%>
                <asp:TemplateField HeaderText="A2">
                    <HeaderTemplate>
                        <asp:Label runat="server" ID="lbl_A2_Header" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lbl_A2" />
                    </ItemTemplate>
                </asp:TemplateField>
                <%--Column=13--%>
                <asp:TemplateField HeaderText="Exported">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:Image runat="server" ID="img_ExportStatus" ImageUrl="~/App_Themes/Default/Images/check.png" />
                    </ItemTemplate>
                </asp:TemplateField>
                <%--Column=14--%>
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
    <dx:ASPxPopupControl ID="pop_Config" ClientInstanceName="pop_Config" runat="server" Width="420" HeaderText="Configuration" Modal="True" ShowCloseButton="true"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="True">
        <HeaderStyle BackColor="#ffffcc" />
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <div class="mb-10">
                    <asp:Label runat="server" Text="Version" />
                </div>
                <div class="mb-20">
                    <asp:DropDownList runat="server" ID="ddl_Config_Version" Width="120">
                        <asp:ListItem Value="42601" Text="42601" />
                        <asp:ListItem Value="610" Text="610" />
                        <asp:ListItem Value="620" Text="620" />
                    </asp:DropDownList>
                </div>
                <div class="mb-10">
                    <asp:Label ID="Label4" runat="server" Text="Journal Type" />
                </div>
                <div class="mb-20">
                    <asp:TextBox runat="server" ID="txt_Config_JournalType" Width="120" Text="MCINV" />
                    <div>
                        <small>*default = MCINV</small>
                    </div>
                </div>
                <div class="mb-10">
                    <asp:Label ID="Label3" runat="server" Text="Tax Account" />
                </div>
                <div class="mb-20">
                    <asp:DropDownList runat="server" ID="ddl_Config_TaxAccountType" Width="120">
                        <asp:ListItem Value="CATEGORY" Text="Category" />
                        <asp:ListItem Value="SUBCATEGORY" Text="Sub-Category" />
                        <asp:ListItem Value="ITEMGROUP" Text="Item Group" />
                        <asp:ListItem Value="PRODUCT" Text="Product" />
                        <asp:ListItem Value="" Text="Fix code" />
                    </asp:DropDownList>
                    &nbsp;
                    <asp:TextBox runat="server" ID="txt_Config_TaxAccountCode" Width="120" Text="" />
                </div>
                <%--<div class="mb-10">
                    <asp:Label ID="Label101" runat="server" Text="Use A1" />
                </div>
                <div class="mb-20">
                    <asp:DropDownList runat="server" ID="ddl_UseA1" Width="120">
                        <asp:ListItem Value="true" Text="Yes" />
                        <asp:ListItem Value="false" Text="No" />
                    </asp:DropDownList>
                </div>

                <div class="mb-10">
                    <asp:Label ID="Label102" runat="server" Text="Use A2" />
                </div>
                <div class="mb-20">
                    <asp:DropDownList runat="server" ID="ddl_UseA2" Width="120">
                        <asp:ListItem Value="true" Text="Yes" />
                        <asp:ListItem Value="false" Text="No" />
                    </asp:DropDownList>
                </div>

                <div class="mb-10">
                    <asp:Label ID="Label103" runat="server" Text="Use A3" />
                </div>
                <div class="mb-20">
                    <asp:DropDownList runat="server" ID="ddl_UseA3" Width="120">
                        <asp:ListItem Value="true" Text="Yes" />
                        <asp:ListItem Value="false" Text="No" />
                    </asp:DropDownList>
                </div>--%>
                <div class="mb-10">
                    <asp:Label ID="Label103" runat="server" Text="Use Committed Date" />
                </div>
                <div class="mb-20">
                    <asp:DropDownList runat="server" ID="ddl_Config_UseCommitDate" Width="120">
                        <asp:ListItem Value="true" Text="Yes" />
                        <asp:ListItem Value="false" Text="No" />
                    </asp:DropDownList>
                </div>
                <div class="mb-10">
                    <asp:Label ID="Label5" runat="server" Text="Only once export" />
                </div>
                <div class="mb-20">
                    <asp:DropDownList runat="server" ID="ddl_Config_SingleExport" Width="120">
                        <asp:ListItem Value="true" Text="Yes" />
                        <asp:ListItem Value="false" Text="No" />
                    </asp:DropDownList>
                </div>
                <div class="flex flex-justify-content-center mt-20 width-100">
                    <asp:Button runat="server" ID="btn_SaveConfig" Text="Save" OnClick="btn_SaveConfig_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button runat="server" ID="btn_CancelConfig" Text="Cancel" OnClientClick="pop_Config.Hide()" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
