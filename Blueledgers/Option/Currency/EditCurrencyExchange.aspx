<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="EditCurrencyExchange.aspx.cs" Inherits="BlueLedger.PL.Option.EditCurrencyExchange"
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
        
        .calendarBG
        {
            background-image: url(<%= Page.ResolveUrl("~/App_Themes/Default/Images/master/pt/default/calendar.png") %>);
            background-repeat: no-repeat;
            background-position: 180px 0px;
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
        function onlyNumerics(evt) {
            // Why I cannot use regular validator inside gridview ?!
            // for trans-browser compatibility
            var resultStatus = false;
            var e = event || evt;
            var charCode = e.which || e.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                if (charCode == 46) {
                    // Code 46: point '.'
                    return true;
                }
                return false;
            }
            return true;
        }

        // ? Try, chrome or firefox = not work.
        //        function onMousebtClick(e) {
        //            switch (event.button) {
        //                case 1:
        //                    alert("You Clicked Left.");
        //                    break;
        //                case 2:
        //                    alert("You Clicked Right.");
        //                    break;
        //            }
        //        }
        //        document.onmousedown = onMousebtClick
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
                                                <dx:MenuItem Name="Save" Text="">
                                                    <ItemStyle Height="16px" Width="43px">
                                                        <HoverStyle>
                                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png"
                                                                Repeat="NoRepeat" VerticalPosition="center" />
                                                        </HoverStyle>
                                                        <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/save.png"
                                                            Repeat="NoRepeat" VerticalPosition="center" />
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
                                                <dx:MenuItem Name="Back" Text="">
                                                    <ItemStyle Height="16px" Width="43px">
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
                                    </td>
                                </tr>
                            </table>
                            <div align="left" style="width: 100%; padding-top: 2vh; padding-bottom: 1vh;">
                                <asp:Label ID="lbl_Titel01" runat="server" Text="Input Date" Font-Bold="true"></asp:Label>
                                <asp:TextBox ID="txt_HeaderDate" runat="server" onfocus="this.blur();" Style="width: 200px;
                                    margin-left: 1vw; margin-right: 1vw;"></asp:TextBox><%--CssClass="calendarBG"--%>
                                <ajaxToolkit:CalendarExtender ID="cal_HeaderDate" runat="server" TargetControlID="txt_HeaderDate"
                                    Format="dd/MM/yyyy">
                                </ajaxToolkit:CalendarExtender>
                                <asp:Button ID="btn_Go" runat="server" Text="Go" OnClick="btn_Go_Click" />
                            </div>
                            <div>
                                <asp:GridView ID="gv_CurrEdit" runat="server" Width="100%" ShowHeader="true" SkinID="GRD_V1"
                                    GridLines="Horizontal" OnRowDataBound="gv_CurrEdit_RowDataBound">
                                    <Columns>
                                        <asp:BoundField HeaderStyle-Width="10%" />
                                        <asp:TemplateField HeaderText="Currency">
                                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_CurrCode" runat="server" Style="padding-left: 20px;"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle Width="30%" />
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_CurrDesc" runat="server" Style="padding-left: 5px;"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle />
                                            <ItemStyle />
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_Date" runat="server" AutoPostBack="true" Enabled="false" Style="width: 15vw;
                                                    display: none;" onfocus="this.blur();" OnTextChanged="txt_Date_OnTextChanged"></asp:TextBox>
                                                <%--<ajaxToolkit:CalendarExtender ID="cal_InputDate" runat="server" TargetControlID="txt_Date"
                                                    Format="dd/MM/yyyy">
                                                </ajaxToolkit:CalendarExtender>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rate">
                                            <HeaderStyle HorizontalAlign="Right" />
                                            <ItemStyle HorizontalAlign="Center" Width="30%" />
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_rate" runat="server" AutoPostBack="true" Style="width: 100%;
                                                    text-align: right;" onkeypress="return onlyNumerics(this);" OnTextChanged="txt_rate_OnTextChanged"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-Width="10%" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <asp:Label ID="lbl_Test" runat="server"></asp:Label>
                <asp:GridView ID="gv_Test" runat="server">
                </asp:GridView>
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
            <dx:ASPxPopupControl ID="pop_Warning" runat="server" ClientInstanceName="pop_Warning"
                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                Height="60px" Width="240px" CloseAction="None" ShowCloseButton="False">
                <ContentStyle HorizontalAlign="Center" VerticalAlign="Middle">
                </ContentStyle>
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <asp:Label ID="lbl_Result" runat="server" SkinID="LBL_NR"></asp:Label>
                        <asp:Label ID="poplbl_Hide" runat="server" Style="display: none;"></asp:Label>
                        <br />
                        <br />
                        <asp:Button ID="btn_Result" runat="server" Text="OK" SkinID="BTN_V1" OnClick="btn_Result_Click" />
                        &nbsp<asp:Button ID="btn_No" runat="server" Text="NO" SkinID="BTN_V1" Visible="false"
                            OnClick="btn_No_Click" />
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="menu_CmdBar" EventName="ItemClick" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
