<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="SPOLst.aspx.cs" Inherits="BlueLedger.PL.IN.SPO.SPOLst" %>
    
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
                            <asp:Label ID="lbl_Title" runat="server" Text="Spoil/Damage" Font-Size="13pt"
                                Font-Bold="true"></asp:Label>
                        </td>
                        <td align="right" style="border-bottom: solid 5px #187EB8">
                            <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="100px" SkinID = "COMMANDBAR">
                                <PanelCollection>
                                    <dx:PanelContent>
                                        <dx:ASPxMenu ID="menu_CmdBar" runat="server" SkinID="COMMAND_BAR" 
                                            AutoPostBack="True" OnItemClick="menu_CmdBar_ItemClick">
                                            <Items>
                                                <dx:MenuItem Text = "Create">
                                                    <Image Url="~/App_Themes/Default/Images/create.gif">
                                                    </Image>
                                                </dx:MenuItem>
                                                <dx:MenuItem Text = "Print">
                                                    <Image Url="~/App_Themes/Default/Images/print.gif">
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
                <br />
                <table border="0" cellpadding="1" cellspacing="5">
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="From Date" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_FromDate" runat="server"></asp:TextBox>
                        </td>
                        <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="To Date" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_ToDate" runat="server"></asp:TextBox>
                        </td>
                        <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td>
                            <asp:Button ID="bnt_Go" runat="server" Text="Go" />
                        </td>
                    </tr>
                </table>
                <br />
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td align = "center">
                            <dx:ASPxGridView ID="ASPxGridView1" runat="server" Width = "100%" 
                                AutoGenerateColumns="False">
                                <Columns>
                                    <dx:GridViewCommandColumn VisibleIndex="0">
                                        <EditButton Visible="True">
                                        </EditButton>
                                        <DeleteButton Text="Del" Visible="True">
                                        </DeleteButton>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </dx:GridViewCommandColumn>
                                    <dx:GridViewDataTextColumn Caption="Date" VisibleIndex="1">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Spoil/Damage#" VisibleIndex="2">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Location" VisibleIndex="3">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Doc.No." VisibleIndex="4">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Status" VisibleIndex="5">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Locked" VisibleIndex="6">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Loced By" VisibleIndex="7">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                            </dx:ASPxGridView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
