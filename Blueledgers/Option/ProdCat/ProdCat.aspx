<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="ProdCat.aspx.cs" Inherits="BlueLedger.PL.Option.ProdCat.ProdCat" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <asp:UpdatePanel ID="UdPnDetail" runat="server">
        <ContentTemplate>
            <div>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td align="left">
                            <!-- Title & Command Bar  -->
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr style="background-color: #4d4d4d; height: 17px;">
                                    <td align="left" style="padding-left: 10px;">
                                        <table border="0" cellpadding="2" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:Option.ProdCat.Category, lbl_Title %>"
                                                        SkinID="LBL_HD_WHITE"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="right">
                                        <%--<dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="200px" SkinID="COMMANDBAR">
                                <PanelCollection>
                                    <dx:PanelContent>--%>
                                        <dx:ASPxMenu runat="server" ID="menu_CmdBar" AutoPostBack="True" Font-Bold="True"
                                            BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle"
                                            Height="16px" OnItemClick="menu_CmdBar_ItemClick">
                                            <ItemStyle BackColor="Transparent">
                                                <HoverStyle BackColor="Transparent">
                                                    <Border BorderStyle="None" />
                                                </HoverStyle>
                                                <Paddings Padding="2px" />
                                                <Border BorderStyle="None" />
                                            </ItemStyle>
                                            <Items>
                                                <dx:MenuItem Text="" Name="Create" ToolTip="Create">
                                                    <ItemStyle Height="16px" Width="49px">
                                                        <HoverStyle>
                                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png"
                                                                Repeat="NoRepeat" VerticalPosition="center" />
                                                        </HoverStyle>
                                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/create.png" Repeat="NoRepeat"
                                                            HorizontalPosition="center" VerticalPosition="center" />
                                                    </ItemStyle>
                                                    <Items>
                                                        <dx:MenuItem Text="Create Sub Category" Name="SC">
                                                        </dx:MenuItem>
                                                        <dx:MenuItem Text="Create Item Group" Name="IG">
                                                        </dx:MenuItem>
                                                        <%--<dx:MenuItem Text="Coppy to Property" Name="Coppy" Visible="false">
                                                        </dx:MenuItem>--%>
                                                    </Items>
                                                </dx:MenuItem>
                                                <dx:MenuItem Text="" Name="Edit" ToolTip="Edit">
                                                    <ItemStyle Height="16px" Width="38px">
                                                        <HoverStyle>
                                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-edit.png"
                                                                Repeat="NoRepeat" VerticalPosition="center" />
                                                        </HoverStyle>
                                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/edit.png" Repeat="NoRepeat"
                                                            HorizontalPosition="center" VerticalPosition="center" />
                                                    </ItemStyle>
                                                </dx:MenuItem>
                                                <dx:MenuItem Text="" Name="Delete" ToolTip="Delete">
                                                    <ItemStyle Height="16px" Width="47px">
                                                        <HoverStyle>
                                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png"
                                                                Repeat="NoRepeat" VerticalPosition="center" />
                                                        </HoverStyle>
                                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png" Repeat="NoRepeat"
                                                            HorizontalPosition="center" VerticalPosition="center" />
                                                    </ItemStyle>
                                                </dx:MenuItem>
                                                <dx:MenuItem Name="Back" ToolTip="Back" Text="">
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
                                            <SubMenuStyle HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" Font-Size="9pt"
                                                ForeColor="#4D4D4D" />
                                            <Border BorderStyle="None"></Border>
                                        </dx:ASPxMenu>
                                        <%--</dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>--%>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="Panel1" runat="server">
                                <%--<table border="0" cellpadding="5" cellspacing="0" style="width: 100%;">
            <tr>
                <td align="left">--%>
                                <table border="0" cellpadding="1" cellspacing="0" width="100%" class="TABLE_HD">
                                    <tr>
                                        <td rowspan="5" style="width: 1%;">
                                        </td>
                                        <td style="width: 12%">
                                            <asp:Label ID="lbl_CatCode_Nm" runat="server" Text="<%$ Resources:Option.ProdCat.Category, lbl_CatCode_Nm %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_CatCode" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_CatName_Nm" runat="server" Text="<%$ Resources:Option.ProdCat.Category, lbl_CatName_Nm %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_CatName" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_TaxAccCode_Nm" runat="server" Text="<%$ Resources:Option.ProdCat.Category, lbl_TaxAccCode_Nm %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_TaxAccCode" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_CateType_Nm0" runat="server" SkinID="LBL_HD" Text="Category Type:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_CateType" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_AppLv_Nm0" runat="server" SkinID="LBL_HD" Text="<%$ Resources:Option.ProdCat.Category, lbl_AppLv_Nm %>"
                                                Visible="False"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_AppLv" runat="server" SkinID="LBL_NR_BLUE" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <%--                </td>
            </tr>
        </table>--%>
                            </asp:Panel>
                            <asp:Panel ID="Panel2" runat="server">
                                <%--        <table border="0" cellpadding="5" cellspacing="0" style="width: 100%;">
            <tr>
                <td align="left">--%>
                                <table border="0" cellpadding="1" cellspacing="0" width="100%" class="TABLE_HD">
                                    <tr>
                                        <td rowspan="5" style="width: 1%;">
                                        </td>
                                        <td style="width: 13%">
                                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Option.ProdCat.Category, lbl_CatCode_Nm %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_SCatCode" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_SubCatCode_Nm" runat="server" Text="<%$ Resources:Option.ProdCat.Category, lbl_SubCatCode_Nm %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_SubCatCode" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_SubCatName_Nm" runat="server" Text="<%$ Resources:Option.ProdCat.Category, lbl_SubCatName_Nm %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_SubCatName" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Option.ProdCat.Category, lbl_TaxAccCode_Nm %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_STaxAccCode" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Option.ProdCat.Category, lbl_AppLv_Nm %>"
                                                SkinID="LBL_HD" Visible="False"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_SAppLv" runat="server" SkinID="LBL_NR_BLUE" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <%--                </td>
            </tr>
        </table>--%>
                            </asp:Panel>
                            <asp:Panel ID="Panel3" runat="server">
                                <table border="0" cellpadding="1" cellspacing="0" width="100%" class="TABLE_HD">
                                    <tr>
                                        <td rowspan="6" style="width: 1%;">
                                        </td>
                                        <td style="width: 13%">
                                            <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Option.ProdCat.Category, lbl_CatCode_Nm %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_ICatCode" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Option.ProdCat.Category, lbl_SubCatCode_Nm %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_ISubCatCode" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_IGroupCode_Nm" runat="server" Text="<%$ Resources:Option.ProdCat.Category, lbl_IGroupCode_Nm %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_IGroupCode" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_IGroupName_Nm" runat="server" Text="<%$ Resources:Option.ProdCat.Category, lbl_IGroupCode_Nm %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_IGroupName" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label18" runat="server" Text="<%$ Resources:Option.ProdCat.Category, lbl_TaxAccCode_Nm %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_ITaxAccCode" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label19" runat="server" Text="<%$ Resources:Option.ProdCat.Category, lbl_AppLv_Nm %>"
                                                SkinID="LBL_HD" Visible="False"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_IAppLv" runat="server" SkinID="LBL_NR_BLUE" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <dx:ASPxPopupControl ID="pop_ConfrimDelete" runat="server" CloseAction="CloseButton"
                    HeaderText="<%$ Resources:Option.ProdCat.Category, MsgHD %>" Modal="True" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" colspan="2" height="50px">
                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Option.ProdCat.Category, MsgWarning %>"
                                            SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <%--<dx:ASPxButton ID="btn_ConfrimDelete" runat="server" Text="Yes" OnClick="btn_ConfrimDelete_Click"
                                SkinID="BTN_V1">
                            </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_ConfrimDelete" runat="server" Text="<%$ Resources:Option.ProdCat.Category, btn_Confrim %>"
                                            OnClick="btn_ConfrimDelete_Click" SkinID="BTN_V1" Width="50px" />
                                    </td>
                                    <td align="left">
                                        <%--<dx:ASPxButton ID="btn_CancelDelete" runat="server" Text="No" OnClick="btn_CancelDelete_Click"
                                SkinID="BTN_V1">
                            </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_CancelDelete" runat="server" Text="<%$ Resources:Option.ProdCat.Category, btn_Cancel %>"
                                            OnClick="btn_CancelDelete_Click" SkinID="BTN_V1" Width="50px" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="pop_WarningDelete" runat="server" CloseAction="CloseButton"
                    HeaderText="<%$ Resources:Option.ProdCat.Category, MsgHD %>" Modal="True" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" height="50px">
                                        <asp:Label ID="Label20" runat="server" Text="<%$ Resources:Option.ProdCat.Category, MsgWarning2 %>"
                                            SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <%--<dx:ASPxButton ID="btn_Ok" runat="server" OnClick="btn_Ok_Click" Text="OK" Width="50px"
                                SkinID="BTN_V1">
                            </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_Ok" runat="server" OnClick="btn_Ok_Click" Text="<%$ Resources:Option.ProdCat.Category, btn_SuccessOk %>"
                                            Width="50px" SkinID="BTN_V1" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
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
</asp:Content>
