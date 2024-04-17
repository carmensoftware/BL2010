<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="CurrencyExchange.aspx.cs" Inherits="BlueLedger.PL.Option.CurrencyExchange"
    Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <style type="text/css">
        .normalrow
        {
            border-style: none;
            cursor: pointer;
        }
        .hightlighrow
        {
            border-style: solid;
            border-color: #4d4d4d;
            border-width: 1px;
        }
        
        @media print
        {
            body *
            {
                visibility: hidden;
            }
            .printable, .printable *
            {
                visibility: visible;
            }
            .printable
            {
                position: absolute;
                left: 0;
                top: 0;
            }
        }
    </style>
    <script type="text/javascript" language="javascript">
        //Check Select All CheckBox.
        function Check(parentChk) {
            var elements = document.getElementsByTagName("input");
            for (i = 0; i < elements.length; i++) {
                if (parentChk.checked == true) {
                    if (IsCheckBox(elements[i])) {
                        elements[i].checked = true;
                    }
                }
                else {
                    elements[i].checked = false;
                }
            }
        }

        function IsCheckBox(chk) {
            if (chk.type == 'checkbox') return true;
            else return false;
        }
    </script>
    <style type="text/css">
        
    </style>
    <asp:UpdatePanel ID="UdPnDetail" runat="server">
        <ContentTemplate>
            <div class="printable">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td align="left">
                            <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                <tr style="background-color: #4d4d4d; height: 17px; padding-left: 10px">
                                    <td style="padding-left: 10px; width: 10px">
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:Option.Currency.Currency, lbl_Title %>"></asp:Label>
                                    </td>
                                    <td style="padding: 0px 10px 0px 0px; background-color: #4D4D4D" align="right">
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
                                                <%--<dx:MenuItem Name="Create" Text="">
                                                    <ItemStyle Height="16px" Width="49px">
                                                        <HoverStyle>
                                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png"
                                                                Repeat="NoRepeat" VerticalPosition="center" />
                                                        </HoverStyle>
                                                        <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/create.png"
                                                            Repeat="NoRepeat" VerticalPosition="center" />
                                                    </ItemStyle>
                                                </dx:MenuItem>--%>
                                                <%-- <dx:MenuItem Name="Delete" Text="">
                                                    <ItemStyle Height="16px" Width="47px">
                                                        <HoverStyle>
                                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png"
                                                                Repeat="NoRepeat" VerticalPosition="center" />
                                                        </HoverStyle>
                                                        <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png"
                                                            Repeat="NoRepeat" VerticalPosition="center" />
                                                    </ItemStyle>
                                                </dx:MenuItem>--%>
                                                <dx:MenuItem Name="Edit" Text="">
                                                    <ItemStyle Height="16px" Width="43px">
                                                        <HoverStyle>
                                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-edit.png"
                                                                Repeat="NoRepeat" VerticalPosition="center" />
                                                        </HoverStyle>
                                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/edit.png" Repeat="NoRepeat"
                                                            HorizontalPosition="center" VerticalPosition="center" />
                                                    </ItemStyle>
                                                </dx:MenuItem>
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
                                            </Items>
                                            <Paddings Padding="0px" />
                                            <SeparatorPaddings Padding="0px" />
                                            <SubMenuStyle HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" Font-Size="9pt"
                                                ForeColor="#4D4D4D" />
                                            <Border BorderStyle="None"></Border>
                                        </dx:ASPxMenu>
                                    </td>
                                </tr>
                            </table>
                            <div>
                                <asp:GridView ID="gv_CurrView" runat="server" Width="100%" ShowHeader="false" GridLines="None"
                                    AutoGenerateColumns="false" SkinID="GRD_V1" OnRowDataBound="gv_CurrView_RowDataBound">
                                    <Columns>
                                        <asp:BoundField ItemStyle-Width="5%" />
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemTemplate>
                                                <table width="100%">
                                                    <tr style="font-size: small">
                                                        <td>
                                                            <asp:Label ID="lbl_CurrCode" runat="server" SkinID="LBL_HD_GRD" Width="5%"></asp:Label>
                                                            <asp:Label ID="lbl_CurrDesc" runat="server" SkinID="LBL_HD_GRD"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" style="width: 100%;">
                                                            <asp:GridView ID="gv_Bottom_5" runat="server" AutoGenerateColumns="false" ShowHeader="true"
                                                                GridLines="None" Width="95%" OnRowDataBound="gv_Bottom_5_RowDataBound">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Input Date">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_Time" runat="server"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Rate">
                                                                        <HeaderStyle HorizontalAlign="Right" />
                                                                        <ItemStyle HorizontalAlign="Right" Width="30%" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_Rate" runat="server"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField ItemStyle-Width="20%" />
                                                                    <%--<asp:TemplateField HeaderText="Updated By">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl_By" runat="server"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>--%>
                                                                </Columns>
                                                            </asp:GridView>
                                                            <div align="right" style="width: 90%;">
                                                                <asp:LinkButton ID="lnk_MoreD" runat="server" OnClick="lnk_MoreD_OnClick" Text="More"
                                                                    Style="text-decoration: none; color: #A8A8A8; padding-right: 35px; font-size: small;">
                                                                </asp:LinkButton>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpPgDetail"
                PopupControlID="UpPgDetail" BackgroundCssClass="POPUP_BG" RepositionMode="RepositionOnWindowResizeAndScroll">
            </ajaxToolkit:ModalPopupExtender>
            <asp:UpdateProgress ID="UpPgDetail" runat="server" AssociatedUpdatePanelID="UdPnDetail">
                <ProgressTemplate>
                    <div class="fix-layout" style="border-style: solid; border-width: 1px; border-color: #0071BD;
                        background-color: #FFFFFF; width: 120px; height: 60px">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 120px; height: 60px">
                            <tr>
                                <td align="center">
                                    <asp:Image ID="img_Loading2" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/ajax-loader.gif"
                                        EnableViewState="False" />
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
            <dx:ASPxPopupControl ID="pop_Details" runat="server" ClientInstanceName="pop_Details"
                HeaderText="History" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                Height="250px" Width="380px" CloseAction="CloseButton" ShowCloseButton="true">
                <ContentStyle HorizontalAlign="Left">
                </ContentStyle>
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <asp:Label ID="pop_lbl_Code" runat="server" Font-Size="Small" Font-Bold="true"></asp:Label>
                        <asp:Label ID="pop_lbl_Desc" runat="server" Font-Size="Small"></asp:Label>
                        <div style="vertical-align: top; height: 30vh; overflow-y: scroll; padding-top: 10px;
                            padding-bottom: 10px;">
                            <table width="100%">
                                <tr>
                                    <td style="width: 100%;">
                                        <asp:GridView ID="pop_gv_EachDetails" runat="server" Width="98%" AutoGenerateColumns="false"
                                            GridLines="None" SkinID="GRD_V1">
                                            <Columns>
                                                <asp:BoundField HeaderText="Date" DataField="InputDate" DataFormatString="{0:d}">
                                                    <ItemStyle Width="30%" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Rate" DataField="CurrencyRate">
                                                    <HeaderStyle HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" Width="20%" />
                                                </asp:BoundField>
                                                <asp:BoundField ItemStyle-Width="10%" />
                                                <%--<asp:BoundField HeaderText="Updated By" DataField="UpdatedBy">
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:BoundField>--%>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="menu_CmdBar" EventName="ItemClick" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
