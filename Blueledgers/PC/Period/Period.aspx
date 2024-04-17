<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="Period.aspx.cs" Inherits="BlueLedger.PL.PC.Period" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <asp:UpdatePanel ID="UdPnDetail" runat="server">
        <ContentTemplate>
            <div>
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr style="background-color: #4d4d4d; height: 17px;">
                        <td align="left" style="padding-left: 10px;">
                            <%--title bar--%>
                            <table border="0" cellpadding="2" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:PC_Period_Period, lbl_Title %>"
                                            SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <%--<dx:ASPxButton ID="bnt_Apply" runat="server" Text="Apply" Width="80px" OnClick="bnt_Apply_Click">
                </dx:ASPxButton>--%>
                            <asp:Button ID="bnt_Apply" runat="server" Text="Apply" Width="80px" OnClick="bnt_Apply_Click"
                                SkinID="BTN_V1" />
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="3" cellspacing="0" width="100%">
                    <!-- Formate Guideline Row -->
                    <tr>
                        <td style="width: 1%">
                        </td>
                        <td style="width: 9%">
                            <asp:Label ID="lbl_Year_Nm" runat="server" Font-Bold="True" Text="<%$ Resources:PC_Period_Period, lbl_Year_Nm %>"
                                SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td style="width: 15%">
                            <dx:ASPxComboBox ID="ddl_Year" runat="server" CssFilePath="" CssPostfix="" LoadingPanelImagePosition="Top"
                                ShowShadow="False" SpriteCssFilePath="" ValueType="System.String" Width="100px"
                                AutoPostBack="True" OnSelectedIndexChanged="ddl_Year_SelectedIndexChanged" TextField="Year"
                                ValueField="Year">
                                <LoadingPanelImage Url="">
                                </LoadingPanelImage>
                                <DropDownButton>
                                    <Image>
                                        <SpriteProperties HottrackedCssClass="" PressedCssClass="" />
                                    </Image>
                                </DropDownButton>
                                <ValidationSettings>
                                    <ErrorFrameStyle ImageSpacing="4px">
                                        <ErrorTextPaddings PaddingLeft="4px" />
                                    </ErrorFrameStyle>
                                </ValidationSettings>
                            </dx:ASPxComboBox>
                        </td>
                        <td style="width: 9%">
                        </td>
                        <td style="width: 15%">
                        </td>
                        <td style="width: 9%">
                        </td>
                        <td style="width: 15%">
                        </td>
                        <td style="width: 9%">
                        </td>
                        <td style="width: 15%">
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                    <tr>
                        <td width="1%">
                        </td>
                        <td width="92%">
                            <asp:GridView ID="grd_Period" runat="server" Width="100%" AutoGenerateColumns="False"
                                ShowFooter="True" OnRowDataBound="grd_Period_RowDataBound" EnableModelValidation="True"
                                SkinID="GRD_V1">
                                <Columns>
                                    <asp:BoundField DataField="PeriodNumber" HeaderText="<%$ Resources:PC_Period_Period, lbl_Period_Grd_Nm %>">
                                        <HeaderStyle Width="10%" HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="<%$ Resources:PC_Period_Period, lbl_StartDAte_Grd_Nm %>">
                                        <ItemTemplate>
                                            <dx:ASPxDateEdit ID="txt_StartDate" runat="server" CssFilePath="" CssPostfix="" ShowShadow="False"
                                                SpriteCssFilePath="" Width="100px">
                                                <ValidationSettings>
                                                    <ErrorFrameStyle ImageSpacing="4px">
                                                        <ErrorTextPaddings PaddingLeft="4px" />
                                                    </ErrorFrameStyle>
                                                </ValidationSettings>
                                                <DropDownButton>
                                                    <Image>
                                                        <SpriteProperties HottrackedCssClass="" PressedCssClass="" />
                                                    </Image>
                                                </DropDownButton>
                                                <CalendarProperties>
                                                    <HeaderStyle Spacing="1px" />
                                                    <FooterStyle Spacing="17px" />
                                                </CalendarProperties>
                                            </dx:ASPxDateEdit>
                                        </ItemTemplate>
                                        <HeaderStyle Width="40%" HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:PC_Period_Period, lbl_EndDate_Grd_Nm %>">
                                        <ItemTemplate>
                                            <dx:ASPxDateEdit ID="txt_EndDate" runat="server" CssFilePath="" CssPostfix="" ShowShadow="False"
                                                SpriteCssFilePath="" Width="100px">
                                                <ValidationSettings>
                                                    <ErrorFrameStyle ImageSpacing="4px">
                                                        <ErrorTextPaddings PaddingLeft="4px" />
                                                    </ErrorFrameStyle>
                                                </ValidationSettings>
                                                <DropDownButton>
                                                    <Image>
                                                        <SpriteProperties HottrackedCssClass="" PressedCssClass="" />
                                                    </Image>
                                                </DropDownButton>
                                                <CalendarProperties>
                                                    <HeaderStyle Spacing="1px" />
                                                    <FooterStyle Spacing="17px" />
                                                </CalendarProperties>
                                            </dx:ASPxDateEdit>
                                        </ItemTemplate>
                                        <HeaderStyle Width="40%" HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:PC_Period_Period, lbl_Status_Grd_Nm %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Status" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="10%" HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <dx:ASPxPopupControl ID="pop_CreatedSuccess" runat="server" CssFilePath="" CssPostfix=""
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" SpriteCssFilePath=""
                    HeaderText="" Width="300px" CloseAction="CloseButton" Modal="True" ShowCloseButton="False">
                    <ContentStyle HorizontalAlign="Center" VerticalAlign="Top">
                    </ContentStyle>
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" colspan="2" height="50px">
                                        <asp:Label ID="lbl_PeriodCreate_Nm" runat="server" Text="<%$ Resources:PC_Period_Period, lbl_PeriodCreate_Nm %>"
                                            SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <%--<dx:ASPxButton ID="btn_OKNotAllow0" runat="server" CssFilePath="" CssPostfix="" SpriteCssFilePath=""
                                Text="OK" OnClick="btn_OKNotAllow_Click">
                            </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_OKNotAllow0" runat="server" Text="<%$ Resources:PC_Period_Period, btn_OKNotAllow0 %>"
                                            OnClick="btn_OKNotAllow_Click" SkinID="BTN_V1" Width="50px" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpPgDetail"
        PopupControlID="UpPgDetail" BackgroundCssClass="POPUP_BG" RepositionMode="RepositionOnWindowResizeAndScroll">
    </ajaxToolkit:ModalPopupExtender>
    <asp:UpdateProgress ID="UpPgDetail" runat="server" AssociatedUpdatePanelID="UdPnDetail">
        <ProgressTemplate>
            <div class="fix-layout" style="border-style: solid; border-width: 1px; border-color: #0071BD;
                background-color: #FFFFFF; width: 120px; height: 60px">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 120px; height: 60px">
                    <tr>
                        <td align="center">
                            <asp:Image ID="img_Loading1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/ajax-loader.gif"
                                EnableViewState="False" />
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
</asp:Content>
