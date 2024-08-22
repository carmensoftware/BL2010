<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="PeriodEnd.aspx.cs" Inherits="BlueLedger.PL.PC.PeriodEnd" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <!-- Main Bar -->
    <div style="background-color: #4d4d4d; height: 17px;">
        <div style="display: inline-block; margin-left: 10px;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
        </div>
        <div style="display: inline-block;">
            <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:PC_Period_PeriodEnd, lbl_Title %>"></asp:Label>
        </div>
    </div>
    <div style="width: 100%;">
        <div style="margin: 10px; text-align: right;">
            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:PC_Period_PeriodEnd, lbl_Message1 %>" SkinID="LBL_NR" />
            <asp:Label ID="lbl_EndDate" runat="server" SkinID="LBL_NR" />
            <asp:Button ID="btn_ClosePeriod" runat="server" Style="margin-left: 10px;" Text="Closed Period" OnClick="btn_ClosePeriod_Click" />
        </div>
        <div style="margin: 10px;">
            <b>Pending Receiving</b>
            <asp:GridView ID="gvRec" runat="server" Width="100%" AutoGenerateColumns="false" GridLines="Horizontal" Font-Size="Small" AllowPaging="true" PageSize="20"
                OnPageIndexChanging="gvRec_PageIndexChanging">
                <HeaderStyle BorderStyle="None" BackColor="Gray" ForeColor="White" HorizontalAlign="Left" />
                <Columns>
                    <asp:TemplateField HeaderText="Receiving No.">
                        <ItemTemplate>
                            <a href="../../PC/REC/Rec.aspx?BuCode=<%# LoginInfo.BuInfo.BuCode %>&ID=<%# Eval("RecNo").ToString()%>" target="_blank">
                                <%# Eval("RecNo").ToString()%>
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Date">
                        <ItemTemplate>
                            <%# DateTime.Parse(Eval("RecDate").ToString()).ToShortDateString() %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField></asp:TemplateField>
                    <asp:BoundField DataField="Vendor" HeaderText="Vendor" />
                    <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No." />
                    <asp:TemplateField HeaderText="Invoice Date">
                        <ItemTemplate>
                            <%# Eval("InvoiceDate") == DBNull.Value ? "" : DateTime.Parse(Eval("InvoiceDate").ToString()).ToShortDateString() %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Description" HeaderText="Description" />
                    <asp:BoundField DataField="DocStatus" HeaderText="Status" />
                    <asp:BoundField DataField="CurrencyCode" HeaderText="Currency" />
                    <asp:TemplateField HeaderText="Total">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <%# FormatNumber(Convert.ToDecimal(Eval("CurrTotalAmt"))) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total (Base)">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <%# FormatNumber(Convert.ToDecimal(Eval("TotalAmt"))) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div style="padding: 10px;">
            <b>Pending Closing Balance (EOP)</b>
            <asp:GridView ID="gvEop" runat="server" Width="100%" AutoGenerateColumns="false" GridLines="Horizontal" Font-Size="Small" AllowPaging="true" PageSize="20"
                OnPageIndexChanging="gvEop_PageIndexChanging">
                <HeaderStyle BorderStyle="None" BackColor="Gray" ForeColor="White" HorizontalAlign="Left" />
                <Columns>
                    <asp:TemplateField HeaderText="Location">
                        <ItemTemplate>
                            <a href="../../PC/EOP/EOPList.aspx" target="_blank">
                                <%# Eval("LocationCode").ToString()%>
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Name">
                        <ItemTemplate>
                            <a href="../../PC/EOP/EOPList.aspx" target="_blank">
                                <%# Eval("LocationName").ToString()%>
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Description" DataField="Description" />
                    <asp:BoundField HeaderText="Remark" DataField="Remark" />
                    <asp:BoundField HeaderText="Status" DataField="DocStatus" />
                    <asp:TemplateField></asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <dx:ASPxPopupControl ID="pop_Warning" runat="server" Width="320px" ShowCloseButton="False" CloseAction="CloseButton" HeaderText="Warning" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AutoUpdatePosition="True">
        <ContentStyle HorizontalAlign="Center" />
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <asp:Label ID="lbl_Warning" runat="server" />
                <br />
                <br />
                <asp:Button ID="btn_Warning" runat="server" Width="60px" Text="OK" OnClick="btn_Warning_Click" />
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
