<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="ADJEdit.aspx.cs" Inherits="BlueLedger.PL.IN.ADJ.ADJEdit" %>
    
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
            <td align="left">
                <table border="0" cellpadding="1" cellspacing="0" style="width: 100%;">
                    <tr style="height: 40px;">
                        <td style="border-bottom: solid 5px #187EB8">
                            <asp:Label ID="lbl_Title" runat="server" Text="Adjustment" Font-Size="13pt"
                                Font-Bold="true">
                            </asp:Label>
                        </td>
                        <td align="right" style="border-bottom: solid 5px #187EB8">
                            <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="100px" SkinID = "COMMANDBAR">
                                <PanelCollection>
                                    <dx:PanelContent>
                                        <dx:ASPxMenu ID="menu_CmdBar" runat="server" SkinID="COMMAND_BAR" 
                                            AutoPostBack="True" OnItemClick="menu_CmdBar_ItemClick">
                                            <Items>
                                                <dx:MenuItem Text = "Save">
                                                    <Image Url = "~/App_Themes/Default/Images/save.gif">
                                                    </Image>
                                                </dx:MenuItem>
                                                <dx:MenuItem Text = "Back">
                                                    <Image Url = "~/App_Themes/Default/Images/back.gif">
                                                    </Image>
                                                </dx:MenuItem>
                                            </Items>
                                        </dx:ASPxMenu>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="1" cellspacing="5">
                    <tr>
                        <td align = "right">
                            <asp:Label ID="Label2" runat="server" Text="Adjustment#" Font-Bold="true"></asp:Label></td><td>
                            <asp:TextBox ID="txt_Adj" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td align = "right">
                            <asp:Label ID="Label3" runat="server" Text="Date" Font-Bold="true"></asp:Label></td><td>
                            <asp:TextBox ID="txt_Date" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td align ="right">
                            <asp:CheckBox ID="CheckBox1" runat="server" />
                        </td>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Commited" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align = "right">
                            <asp:Label ID="Label4" runat="server" Text="Location" Font-Bold="true"></asp:Label></td><td>
                            <asp:DropDownList ID="DropDownList1" runat="server" Width = "155px">
                            </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td align = "right">
                            <asp:Label ID="Label5" runat="server" Text="Doc.No" Font-Bold="true"></asp:Label></td><td>
                                <asp:TextBox ID="txt_DocNo" runat="server"></asp:TextBox></td></tr></table>
                    <table border = "0" cellpadding = "1" cellspacing = "5">
                        <tr>
                            <td align = "right">&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="Label6" runat="server" Text="Comment" Font-Bold="true"></asp:Label></td><td>
                            <asp:TextBox ID="txt_Comment" runat="server" Width = "370px"></asp:TextBox></td>
                        </tr>
                    </table><br />
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                        <tr>
                            <td>
                                <dx:ASPxGridView ID="ASPxGridView1" runat="server" Width = "100%" 
                                    AutoGenerateColumns="False">
                                    <Columns>
                                        <dx:GridViewCommandColumn VisibleIndex="0" Width = "80px">
                                            <editbutton visible="True">
                                            </editbutton>
                                            <deletebutton visible="True">
                                            </deletebutton>
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </dx:GridViewCommandColumn>
                                        <dx:GridViewDataTextColumn Caption="Product Code" VisibleIndex="1">
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Description 1" VisibleIndex="2">
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Description 2" VisibleIndex="3">
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Unit" VisibleIndex="4">
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Qty." VisibleIndex="5">
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Remark" VisibleIndex="6">
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </dx:GridViewDataTextColumn>
                                    </Columns>
                                </dx:ASPxGridView>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table width = "100%">
                    <tr>
                        <td align = "right">
                            <asp:Button ID="Button5" runat="server" Text="New" />
                        </td>
                    </tr>
                </table>
                <br />
               <table border="0" cellpadding="1" cellspacing="5">
                    <tr>
                        <td align ="right" >
                            <asp:Label ID="Label7" runat="server" Text="Product Spec. Detail" Font-Bold="true"></asp:Label></td><td>
                                <asp:TextBox ID="txt_ProdSpecDetail" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td align = "right">
                            <asp:Label ID="Label8" runat="server" Text="QOH" Font-Bold="true"></asp:Label></td><td>
                                <asp:TextBox ID="txt_QOH" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td align = "right">
                            <asp:Label ID="Label9" runat="server" Text="Total On Hand" Font-Bold="true"></asp:Label></td><td>
                                <asp:TextBox ID="txt_TotalOnHand" runat="server"></asp:TextBox></td></tr><tr>
                        <td align = "right">
                            <asp:Label ID="Label10" runat="server" Text="On Order" Font-Bold="true"></asp:Label></td><td>
                                <asp:TextBox ID="txt_OnOrder" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td align = "right">
                            <asp:Label ID="Label11" runat="server" Text="Total On Order" Font-Bold="true"></asp:Label></td><td>
                                <asp:TextBox ID="txt_TotalOnOrder" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td align = "right">
                            <asp:Label ID="Label12" runat="server" Text="Reorder" Font-Bold="true"></asp:Label></td><td>
                                <asp:TextBox ID="txt_Reorder" runat="server"></asp:TextBox></td></tr><tr>
                        <td align = "right">
                            <asp:Label ID="Label14" runat="server" Text="Restock" Font-Bold="true"></asp:Label></td><td>
                                <asp:TextBox ID="txt_Restock" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        </tr></table><br />
            </td>
        </tr>
    </table>
</asp:Content>
