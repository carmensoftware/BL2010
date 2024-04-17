<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Comment2.ascx.cs" Inherits="BlueLedger.PL.UserControls.Comment2" %>

<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>

<%--<asp:UpdatePanel ID="UdPnDetail" runat="server">  <ContentTemplate>--%>
        <div>
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr style="background-color: #4D4D4D;">
                    <td style="padding-left: 10px;">
                        <table border="0" cellpadding="2" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_comment.png" />
                                </td>
                                <td>
                                    <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:UserControl_Comment2, lbl_Title %>"
                                        SkinID="LBL_HD_WHITE"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="right" style="padding-right: 10px;">
                        <dx:ASPxMenu runat="server" ID="menu_CmdBar" Font-Bold="True" BackColor="Transparent"
                            Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle" Height="16px"
                            OnItemClick="menu_CmdBar_ItemClick">
                            <ItemStyle BackColor="Transparent">
                                <HoverStyle BackColor="Transparent">
                                    <Border BorderStyle="None" />
                                </HoverStyle>
                                <Paddings Padding="2px" />
                                <Border BorderStyle="None" />
                            </ItemStyle>
                            <Items>
                                <dx:MenuItem Name="Create" Text="">
                                    <ItemStyle Height="16px" Width="49px">
                                        <HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png"
                                                Repeat="NoRepeat" VerticalPosition="center" />
                                        </HoverStyle>
                                        <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/create.png"
                                            Repeat="NoRepeat" VerticalPosition="center" />
                                    </ItemStyle>
                                </dx:MenuItem>
                            </Items>
                            <Paddings Padding="0px" />
                            <SeparatorPaddings Padding="0px" />
                            <SubMenuStyle HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" Font-Size="9pt"
                                ForeColor="#4D4D4D" />
                            <Border BorderStyle="None"></Border>
                        </dx:ASPxMenu>
                    </td>
                </tr>
                <tr style="height: 2px">
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="grd_Comment" runat="server" AutoGenerateColumns="False" SkinID="GRD_V1"
                            OnRowCancelingEdit="grd_Comment_RowCancelingEdit" OnRowDataBound="grd_Comment_RowDataBound"
                            OnRowDeleting="grd_Comment_RowDeleting" OnRowEditing="grd_Comment_RowEditing"
                            OnRowUpdating="grd_Comment_RowUpdating" EmptyDataText="No Data to Display" Width="100%"
                            EnableModelValidation="True">
                            <Columns>
                                <asp:CommandField HeaderText="&nbsp;&nbsp;&nbsp;&nbsp;#" ShowEditButton="True" DeleteText="Del"
                                    ShowDeleteButton="True">
                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:CommandField>
                                <asp:TemplateField HeaderText="<%$ Resources:UserControl_Comment2, colDate %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Date" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:UserControl_Comment2, colBy %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_By" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:UserControl_Comment2, colComment %>">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txt_Comment" runat="server" SkinID="TXT_V1" Width="100%" Height="17px"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Comment" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="60%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr style="padding: 0 0 0 0; height: 17px; background-color: #2196f3;">
                                        <td style="padding: 0 0 0 20px; width: 10%;" align="center">
                                            <asp:Label ID="lbl_Command" runat="server" Text="#" SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 15%">
                                            <asp:Label ID="lbl_Date" runat="server" Text="<%$ Resources:UserControl_Comment2, colDate %>"
                                                SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 15%">
                                            <asp:Label ID="lbl_By" runat="server" Text="<%$ Resources:UserControl_Comment2, colBy %>"
                                                SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 60%">
                                            <asp:Label ID="lbl_Comment" runat="server" Text="<%$ Resources:UserControl_Comment2, colComment %>"
                                                SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
