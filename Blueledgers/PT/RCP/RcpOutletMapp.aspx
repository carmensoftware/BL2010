<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RcpOutletMapp.aspx.cs" Inherits="BlueLedger.PL.PT.RCP.RcpOutletMapp"
    MasterPageFile="~/Master/In/SkinDefault.master" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu"
    TagPrefix="dx" %>
<asp:Content ID="Content01" runat="server" ContentPlaceHolderID="cph_Main">
    <asp:UpdatePanel ID="up_rcpMapp" runat="server">
        <ContentTemplate>
            <div class="printable">
                <div class="CMD_BAR">
                    <div class="CMD_BAR_LEFT">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                        <asp:Label ID="lbl_Title" runat="server" Text="Outlet Mapping" SkinID="LBL_HD_WHITE" />
                    </div>
                    <div class="CMD_BAR_RIGHT">
                        <dx:ASPxMenu runat="server" ID="menu_CmdBar" Font-Bold="True" BackColor="Transparent"
                            Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle" 
                            Height="16px" onitemclick="menu_CmdBar_ItemClick">
                            <ItemStyle BackColor="Transparent">
                                <HoverStyle BackColor="Transparent">
                                    <Border BorderStyle="None" />
                                </HoverStyle>
                                <Paddings Padding="2px" />
                                <Border BorderStyle="None" />
                            </ItemStyle>
                            <Items>
                                <dx:MenuItem Name="Print" Text="">
                                    <ItemStyle Height="16px" Width="43px">
                                        <HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-print.png"
                                                Repeat="NoRepeat" VerticalPosition="center" />
                                        </HoverStyle>
                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat"
                                            HorizontalPosition="center" VerticalPosition="center" />
                                    </ItemStyle>
                                </dx:MenuItem>
                                <dx:MenuItem Name="Back" Text="">
                                    <ItemStyle Height="16px" Width="42px">
                                        <HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png"
                                                Repeat="NoRepeat" VerticalPosition="center" />
                                        </HoverStyle>
                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat"
                                            HorizontalPosition="center" VerticalPosition="center" />
                                    </ItemStyle>
                                </dx:MenuItem>
                            </Items>
                            <Paddings Padding="0px" />
                            <SeparatorPaddings Padding="0px" />
                            <SubMenuStyle HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" Font-Size="9pt"
                                ForeColor="#4D4D4D" />
                            <Border BorderStyle="None"></Border>
                        </dx:ASPxMenu>
                    </div>
                </div>
                <asp:GridView ID="grd_OL_Mapp" runat="server" AutoGenerateColumns="false" Width="100%"
                    SkinID="GRD_V1" EmptyDataText="No Data to Display" EnableModelValidation="True"
                    OnRowDataBound="grd_OL_Mapp_RowDataBound" OnRowCancelingEdit="grd_OL_Mapp_RowCancelingEdit"
                    OnRowEditing="grd_OL_Mapp_RowEditing" OnRowUpdating="grd_OL_Mapp_RowUpdating">
                    <Columns>
                        <asp:CommandField HeaderText="#" ShowEditButton="True">
                            <HeaderStyle HorizontalAlign="Center" Width="20%" />
                            <ItemStyle HorizontalAlign="Center" Width="15%" />
                        </asp:CommandField>
                        <asp:TemplateField ItemStyle-Width="30%" HeaderText="Outlet">
                            <ItemTemplate>
                                <asp:Label ID="lbl_OL_Code" runat="server" SkinID="LBL_NR"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="20%" HeaderText="Location">
                            <ItemTemplate>
                                <asp:Label ID="lbl_LocateCode" runat="server" SkinID="LBL_NR"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <cc1:ComboBox ID="comb_LocateCode" runat="server" AutoPostBack="true" DropDownStyle="DropDownList"
                                    AutoCompleteMode="SuggestAppend" OnSelectedIndexChanged="comb_LocateCode_SelectedIndexChanged">
                                </cc1:ComboBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="lbl_LocateDesc" runat="server" SkinID="LBL_NR"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div>
                    <asp:Label ID="lbl_Test" runat="server"></asp:Label>
                    <asp:GridView ID="grd_Test" runat="server" AutoGenerateColumns="true">
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
