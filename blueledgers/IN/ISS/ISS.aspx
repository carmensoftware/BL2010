<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="ISS.aspx.cs" Inherits="BlueLedger.PL.IN.ISS.ISS" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <table style="width: 100%;" border="0" cellpadding="5" cellspacing="0">
        <tr>
            <td align = "left">
                <table border="0" cellpadding="1" cellspacing="0" style="width: 100%;">
                    <tr style="height: 40px;">
                        <td style="border-bottom: solid 5px #187EB8">
                            <asp:Label ID="lbl_Title" runat="server" Text="Issue" Font-Size="13pt"
                                Font-Bold="true"></asp:Label>
                        </td>
                        <td align="right" style="border-bottom: solid 5px #187EB8">
                            <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="200px" SkinID = "COMMANDBAR">
                                <PanelCollection>
                                    <dx:PanelContent>
                                        <dx:ASPxMenu ID="menu_CmdBar" runat="server" SkinID="COMMAND_BAR" 
                                            AutoPostBack="True" OnItemClick="menu_CmdBar_ItemClick">
                                            <Items>
                                                <dx:MenuItem Text = "Edit">
                                                    <Image Url="~/App_Themes/Default/Images/edit.gif"></Image>
                                                </dx:MenuItem>
                                                <dx:MenuItem Text = "Delete">
                                                    <Image Url="~/App_Themes/Default/Images/delete.gif"></Image>
                                                </dx:MenuItem>
                                                <dx:MenuItem Text = "Print">
                                                    <Image Url="~/App_Themes/Default/Images/print.gif"></Image>
                                                </dx:MenuItem>
                                                <dx:MenuItem Text = "Back">
                                                    <Image Url="~/App_Themes/Default/Images/back.gif"></Image>
                                                </dx:MenuItem>
                                            </Items>
                                        </dx:ASPxMenu>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>
                        </td>
                    </tr>
                </table>
                <br />
                <table border="0" cellpadding="1" cellspacing="5">
                    <tr>
                        <td align = "right">
                            <asp:Label ID="Label2" runat="server" Text="Issue No." Font-Bold="true"></asp:Label></td><td>
                                <asp:Label ID="lbl_IssNo" runat="server" Width = "150px"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td align = "right">
                            <asp:Label ID="Label3" runat="server" Text="Issue Date" Font-Bold="true"></asp:Label></td><td>
                                <asp:Label ID="lbl_IssDate" runat="server" Width = "150px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align = "right">
                            <asp:Label ID="Label4" runat="server" Text="Requisition No." Font-Bold="true"></asp:Label></td><td>
                                <asp:Label ID="lbl_ReqNo" runat="server" Width = "150px"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td align = "right">
                            <asp:Label ID="Label5" runat="server" Text="Requisition Date" Font-Bold="true"></asp:Label></td><td>
                                <asp:Label ID="lbl_ReqDate" runat="server" Width ="150px"></asp:Label></td></tr></table>
                    <table border = "0" cellpadding = "1" cellspacing = "5">
                        <tr>
                            <td align = "right">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="Label6" runat="server" Text="Comment" Font-Bold="true"></asp:Label></td><td>
                                <asp:Label ID="lbl_Comment" runat="server" Width = "150px"></asp:Label></td>
                        </tr>
                    </table><br />
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                        <tr>
                            <td>
                                <dx:ASPxGridView ID="ASPxGridView1" runat="server" Width = "100%" 
                                    AutoGenerateColumns="False">
                                    <Columns>
                                        <dx:GridViewDataTextColumn Caption="From Location" VisibleIndex="0">
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="To Location" VisibleIndex="1">
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Product Code" VisibleIndex="2">
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Description 1" VisibleIndex="3">
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Description 2" VisibleIndex="4">
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Unit" VisibleIndex="5">
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Req.Qty." VisibleIndex="6">
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Appr.Qty." VisibleIndex="7">
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Issued Qty." VisibleIndex="8">
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Remain Qty." VisibleIndex="9">
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </dx:GridViewDataTextColumn>
                                    </Columns>
                                </dx:ASPxGridView>
                            </td>
                        </tr>
                    </table>
                    <br />
                <table border="0" cellpadding="1" cellspacing="5">
                    <tr>
                        <td align = "right">
                            <asp:Label ID="Label1" runat="server" Text="Stock On Hand" Font-Bold="true"></asp:Label></td><td>
                                <asp:Label ID="lbl_StockOnHand" runat="server" Width = "150px"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td align = "right">
                            <asp:Label ID="Label7" runat="server" Text="Product Spec. Detail" Font-Bold="true"></asp:Label></td><td>
                                <asp:Label ID="lbl_ProdSpecDetail" runat="server" Width = "150px"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td align = "right">
                            <asp:Label ID="Label8" runat="server" Text="On Order" Font-Bold="true"></asp:Label></td><td>
                                <asp:Label ID="lbl_OnOrder" runat="server" Width = "150px"></asp:Label></td></tr><tr>
                        <td align = "right">
                            <asp:Label ID="Label9" runat="server" Text="Last Price" Font-Bold="true"></asp:Label></td><td>
                                <asp:Label ID="lbl_lastPrice" runat="server" Width = "150px"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td align = "right">
                            <asp:Label ID="Label10" runat="server" Text="Reorder" Font-Bold="true"></asp:Label></td><td>
                                <asp:Label ID="lbl_Reorder" runat="server" Width = "150px"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td align = "right">
                            <asp:Label ID="Label11" runat="server" Text="Rec. Store SOH" Font-Bold="true"></asp:Label></td><td>
                                <asp:Label ID="lbl_RecStoreSOH" runat="server" Width = "150px"></asp:Label></td></tr><tr>
                        <td align = "right">
                            <asp:Label ID="Label12" runat="server" Text="Restock" Font-Bold="true"></asp:Label></td><td>
                                <asp:Label ID="lbl_Restock" runat="server" Width = "150px"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td align = "right">
                            <asp:Label ID="Label14" runat="server" Text="Total Stock On Hand" Font-Bold="true"></asp:Label></td><td>
                                <asp:Label ID="lbl_TotalStockOnHand" runat="server" Width = "150px"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td align = "right">
                            <asp:Label ID="Label15" runat="server" Text="Total On Order" Font-Bold="true"></asp:Label></td><td>
                                <asp:Label ID="lbl_TotalOnOrder" runat="server" Width = "150px"></asp:Label></td></tr></table>
            </td>
        </tr>
    </table>
</asp:Content>
