<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="ReStock.aspx.cs" Inherits="BlueLedger.PL.IN.RE.ReStock" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cph_Main">
    <div style="width: 100%;">
        <table style="width: 100%; border: none;" border="0">
            <tr style="padding-left: 10px; background-color: #4d4d4d; height: 17px">
                <td align="left" style="width: 25%;">
                    <table border="0" cellpadding="1" cellspacing="0">
                        <tr>
                            <td style="padding-left: 10px">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                            </td>
                            <td>
                                <asp:Label ID="lbl_PRRequest_Nm" runat="server" Text="Product Restock" SkinID="LBL_HD_WHITE"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td align="right" style="width: 75%; padding-right: 10px;">
                    <dx:ASPxMenu runat="server" ID="menu_CmdBar" Font-Bold="True" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle"
                        Height="16px" OnItemClick="menu_CmdBar_ItemClick">
                        <ItemStyle BackColor="Transparent" ForeColor="White">
                            <HoverStyle BackColor="Transparent" ForeColor="Gray">
                                <Border BorderStyle="None" />
                                <Border BorderStyle="None"></Border>
                            </HoverStyle>
                            <Paddings Padding="2px" />
                            <Border BorderStyle="None" />
                            <Paddings Padding="2px"></Paddings>
                            <Border BorderStyle="None"></Border>
                        </ItemStyle>
                        <Items>
                            <dx:MenuItem Name="Generate PR" Text="Generate PR">
                                <ItemStyle Height="16px" Width="43px"></ItemStyle>
                            </dx:MenuItem>
                        </Items>
                        <Paddings Padding="0px" />
                        <SeparatorPaddings Padding="0px" />
                        <SubMenuStyle HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" Font-Size="9pt" ForeColor="#4D4D4D" />
                        <SubMenuStyle HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" Font-Size="9pt" ForeColor="#4D4D4D"></SubMenuStyle>
                        <Border BorderStyle="None"></Border>
                    </dx:ASPxMenu>
                </td>
            </tr>
        </table>
        <dx:ASPxPopupControl ID="pop_Alert" ClientInstanceName="pop_Alert" runat="server" CloseAction="CloseButton" HeaderText="Alert" Modal="True" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" ShowCloseButton="True" Width="300px">
            <HeaderStyle HorizontalAlign="Left" />
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                    <asp:Label ID="alert_Text" runat="server" SkinID="LBL_NR" Width="100%" Style="text-align: center" />
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
    </div>
    <%--<asp:Content ID="Content1" ContentPlaceHolderID="cph_Left" runat="Server">--%>
    <div align="center" style="width: 25%; float: left;">
        <asp:UpdatePanel ID="UdPnLeftRst" runat="server">
            <ContentTemplate>
                <div style="background-color: #0071BD;">
                    <div>
                        <%-- border="0" cellpadding="3" cellspacing="0" width="100%"--%>
                        <table style="border: 0px; width: 100%;">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_Stock" runat="server" SkinID="LBL_HD_WHITE" Text="Store Location:"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddl_Store" runat="server" SkinID="DDL_V1" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddl_Store_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_Category" runat="server" SkinID="LBL_HD_WHITE" Text="Category:"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddl_Category" runat="server" SkinID="DDL_V1" Width="100%">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_Status" runat="server" SkinID="LBL_HD_WHITE" Text="Status:"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButtonList ID="rbl_Status" runat="server" ForeColor="White">
                                        <asp:ListItem Selected="True" Value="0">Show All</asp:ListItem>
                                        <asp:ListItem Value="1">Under Par</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div>
                        <table border="0" cellpadding="3" cellspacing="0" width="100%">
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btn_Go" runat="server" Text="Go" OnClick="btn_Go_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpPgLeftRst" PopupControlID="UpPgLeftRst" BackgroundCssClass="POPUP_BG"
                    RepositionMode="RepositionOnWindowResizeAndScroll">
                </ajaxToolkit:ModalPopupExtender>
                <asp:UpdateProgress ID="UpPgLeftRst" runat="server" AssociatedUpdatePanelID="UdPnLeftRst">
                    <ProgressTemplate>
                        <div class="fix-layout" style="border-style: solid; border-width: 1px; border-color: #0071BD; background-color: #FFFFFF; width: 120px; height: 60px">
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 120px; height: 60px">
                                <tr>
                                    <td align="center">
                                        <asp:Image ID="img_Loading1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/ajax-loader.gif" EnableViewState="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_Loading1" runat="server" Font-Bold="true" Text="Loading..." EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <%--</asp:Content>--%>
    <div style="width: 74%; float: right;">
        <asp:UpdatePanel ID="UdPnDetail" runat="server">
            <ContentTemplate>
                <div>
                    <div>
                        <asp:GridView ID="grd_ProductRestock" runat="server" AutoGenerateColumns="False" CellPadding="3" EnableModelValidation="True" SkinID="GRD_V1" Width="100%">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chk_SelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="chk_SelectAll_CheckedChanged" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk_Select" runat="server" AutoPostBack="True" OnCheckedChanged="chk_Select_CheckedChanged" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="ProductName" HeaderText="Item Description">
                                    <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="OnHand" HeaderText="On Hand">
                                    <HeaderStyle HorizontalAlign="Right" Width="10%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="InventoryUnit">
                                    <HeaderStyle HorizontalAlign="Left" Width="5%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MinQty" HeaderText="Min">
                                    <HeaderStyle HorizontalAlign="Right" Width="10%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MaxQty" HeaderText="Max">
                                    <HeaderStyle HorizontalAlign="Right" Width="10%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Restock">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Restock" runat="server" onkeypress="txt_Restock_KeyPress()" SkinID="TXT_NUM_V1" Text='<%# Bind("Restock") %>'></asp:TextBox>
                                        <script language="javascript" type="text/javascript">

                                            function txt_Restock_KeyPress() {
                                                if ((event.keyCode < 48 || event.keyCode > 57) && event.keyCode != 46) {
                                                    event.keyCode = null;
                                                }
                                            }
                                        </script>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Right" Width="10%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Status" HeaderText="Status">
                                    <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="UpPgDetail" PopupControlID="UpPgDetail" BackgroundCssClass="POPUP_BG"
                    RepositionMode="RepositionOnWindowResizeAndScroll">
                </ajaxToolkit:ModalPopupExtender>
                <asp:UpdateProgress ID="UpPgDetail" runat="server" AssociatedUpdatePanelID="UdPnDetail">
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
    </div>
    <%--</asp:Content>--%>
    <div style="width: 100%;">
        <asp:Label ID="lbl_Test" runat="server"></asp:Label>
        <asp:GridView ID="gv_Test" runat="server" AutoGenerateColumns="true">
        </asp:GridView>
    </div>
</asp:Content>
