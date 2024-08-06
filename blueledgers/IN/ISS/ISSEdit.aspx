<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="ISSEdit.aspx.cs" Inherits="BlueLedger.PL.IN.ISS.ISSEdit" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <table style="width: 100%;" border="0" cellpadding="5" cellspacing="0">
        <tr>
            <td align="left">
                <table border="0" cellpadding="1" cellspacing="0" style="width: 100%;">
                    <tr style="height: 40px;">
                        <td style="border-bottom: solid 5px #187EB8">
                            <asp:Label ID="lbl_Title" runat="server" Text="Issue" Font-Size="13pt"
                                Font-Bold="true"></asp:Label>
                        </td>
                        <td align="right" style="border-bottom: solid 5px #187EB8">
                            <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="100px" SkinID = "COMMANDBAR">
                                <PanelCollection>
                                    <dx:PanelContent>
                                        <dx:ASPxMenu ID="menu_CmdBar" runat="server" SkinID="COMMAND_BAR" 
                                            AutoPostBack="True" OnItemClick="menu_CmdBar_ItemClick">
                                            <Items>
                                                <dx:MenuItem Text = "Save">
                                                    <Image Url = "~/App_Themes/Default/Images/save.gif"></Image>
                                                </dx:MenuItem>
                                                <dx:MenuItem Text = "Back">
                                                    <Image Url = "~/App_Themes/Default/Images/back.gif"></Image>
                                                </dx:MenuItem>
                                            </Items>
                                        </dx:ASPxMenu>
                                        <%--<table border="0" cellpadding="1" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Image ID="Image2" runat="server" SkinID="IMG_Save" />
                                                </td>
                                                <td>
                                                    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/IN/ISS/ISS.aspx">Save</asp:HyperLink>
                                                </td>
                                                <td>
                                                    <asp:Image ID="Image1" runat="server" SkinID="IMG_Back" />
                                                </td>
                                                <td>
                                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/IN/ISS/ISS.aspx">Back</asp:HyperLink>
                                                </td>
                                            </tr>
                                        </table>--%>
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
                            <asp:TextBox ID="txt_IssNo" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td align = "right">
                            <asp:Label ID="Label3" runat="server" Text="Issue Date" Font-Bold="true"></asp:Label></td><td>
                            <asp:TextBox ID="txt_IssDate" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align = "right">
                            <asp:Label ID="Label4" runat="server" Text="Requisition No." Font-Bold="true"></asp:Label></td><td>
                            <asp:TextBox ID="txt_ReqNo" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td align = "right">
                            <asp:Label ID="Label5" runat="server" Text="Requisition Date" Font-Bold="true"></asp:Label></td><td>
                            <asp:TextBox ID="txt_ReqDate" runat="server"></asp:TextBox></td></tr></table>
                    <table border = "0" cellpadding = "1" cellspacing = "5">
                        <tr>
                            <td align = "right">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="Label6" runat="server" Text="Comment" Font-Bold="true"></asp:Label></td><td>
                            <asp:TextBox ID="txt_Comment" runat="server" Width = "418px"></asp:TextBox></td>
                        </tr>
                    </table><br />
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td>
                            <dx:ASPxGridView ID="ASPxGridView1" runat="server" Width = "100%" 
                                AutoGenerateColumns="False">
                                <Columns>
                                    <dx:GridViewCommandColumn VisibleIndex="0" Width = "80px">
                                        <EditButton Visible="True">
                                        </EditButton>
                                        <DeleteButton Text="Del" Visible="True">
                                        </DeleteButton>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </dx:GridViewCommandColumn>
                                    <dx:GridViewDataTextColumn Caption="From Location" VisibleIndex="1">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="To Location" VisibleIndex="2">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Product Code" VisibleIndex="3">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Description 1" VisibleIndex="4">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Description 2" VisibleIndex="5">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Unit" VisibleIndex="6">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Req.Qty." VisibleIndex="7">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Appr.Qty." VisibleIndex="8">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Issued Qty." VisibleIndex="9">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Remain Qty." VisibleIndex="10">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                            </dx:ASPxGridView>
                        </td>
                    </tr>
                </table><br />
                <table border="0" cellpadding="1" cellspacing="5">
                    <tr>
                        <td align = "right">
                            <asp:Label ID="Label1" runat="server" Text="Stock On Hand" Font-Bold="true"></asp:Label></td><td>
                            <asp:TextBox ID="txt_StockOnHand" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td align = "right">
                            <asp:Label ID="Label7" runat="server" Text="Product Spec. Detail" Font-Bold="true"></asp:Label></td><td>
                            <asp:TextBox ID="txt_ProdSpecDetail" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td align = "right">
                            <asp:Label ID="Label8" runat="server" Text="On Order" Font-Bold="true"></asp:Label></td><td>
                            <asp:TextBox ID="txt_OnOrder" runat="server"></asp:TextBox></td></tr><tr>
                        <td align = "right">
                            <asp:Label ID="Label9" runat="server" Text="Last Price" Font-Bold="true"></asp:Label></td><td>
                            <asp:TextBox ID="txt_LastPrice" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td align = "right">
                            <asp:Label ID="Label10" runat="server" Text="Reorder" Font-Bold="true"></asp:Label></td><td>
                            <asp:TextBox ID="txt_Reorder" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td align = "right">
                            <asp:Label ID="Label11" runat="server" Text="Rec. Store SOH" Font-Bold="true"></asp:Label></td><td>
                            <asp:TextBox ID="txt_RecStroeSOH" runat="server"></asp:TextBox></td></tr><tr>
                        <td align = "right">
                            <asp:Label ID="Label12" runat="server" Text="Restock" Font-Bold="true"></asp:Label></td><td>
                            <asp:TextBox ID="txt_Restock" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td align = "right">
                            <asp:Label ID="Label14" runat="server" Text="Total Stock On Hand" Font-Bold="true"></asp:Label></td><td>
                            <asp:TextBox ID="txt_TotalStockOnHand" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td align = "right">
                            <asp:Label ID="Label15" runat="server" Text="Total On Order" Font-Bold="true"></asp:Label></td><td>
                            <asp:TextBox ID="txt_TotalOnOrder" runat="server"></asp:TextBox></td></tr></table>
                <%--<table width = "100%">
                    <tr>
                        <td align = "Right">
                            <asp:Button ID="Button5" runat="server" Text="New" />
                        </td>
                    </tr>
                </table>--%>
            </td>
        </tr>
    </table>
</asp:Content>
