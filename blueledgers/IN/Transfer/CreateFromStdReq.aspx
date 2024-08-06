<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateFromStdReq.aspx.cs"
    Inherits="BlueLedger.PL.IN.Transfer.CreateFromStdReq" MasterPageFile="~/Master/In/SkinDefault.master" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cph_Main">
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
                                        <asp:Label ID="lbl_StoreReq_Nm" runat="server" Text="<%$ Resources:IN_Transfer_CreateFromStdReq, lbl_StoreReq_Nm %>"
                                            SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right">
                            <table border="0" cellpadding="1" cellspacing="0">
                                <tr>
                                    <td>
                                        <dx:ASPxButton ID="btn_Ok" runat="server" BackColor="Transparent" Height="16px" Width="34px"
                                            ToolTip="OK" OnClick="btn_Ok_Click">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/OK.png" Repeat="NoRepeat"
                                                HorizontalPosition="center" VerticalPosition="center" />
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-OK.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <Border BorderStyle="None" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btn_Back" runat="server" BackColor="Transparent" Height="16px"
                                            Width="42px" ToolTip="Back" OnClick="btn_Back_Click">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat"
                                                HorizontalPosition="center" VerticalPosition="center" />
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <Border BorderStyle="None" />
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="0" cellspacing="3" width="100%" class="TABLE_HD">
                    <tr>
                        <td rowspan="2" style="width: 1%;">
                        </td>
                        <td style="width: 10%;">
                            <asp:Label ID="lbl_ReqFrom_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqFromStdReq, lbl_ReqFrom_Nm %>"
                                SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td colspan="3">
                            <dx:ASPxComboBox ID="ddl_Store" runat="server" OnLoad="ddl_Store_Load" Width="310px"
                                Height="16px" TextFormatString="{0} : {1}" ValueType="System.String" IncrementalFilteringMode="Contains"
                                AutoPostBack="True" OnSelectedIndexChanged="ddl_Store_SelectedIndexChanged">
                                <Columns>
                                    <dx:ListBoxColumn Caption="Code" FieldName="LocationCode" Width="60px" />
                                    <dx:ListBoxColumn Caption="Name" FieldName="LocationName" Width="100px" />
                                </Columns>
                            </dx:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <%--<td style="width: 1%;">
            </td>--%>
                        <td style="width: 10%;">
                            <asp:Label ID="lbl_DeliveryDate_Nm" runat="server" Text="<%$ Resources:IN_Transfer_CreateFromStdReq, lbl_DeliveryDate_Nm %>"
                                SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td style="width: 15%;">
                            <dx:ASPxDateEdit ID="de_DeliveryDate" runat="server">
                            </dx:ASPxDateEdit>
                        </td>
                        <td style="width: 4%;">
                            <asp:Label ID="lbl_Desc_Nm" runat="server" Text="<%$ Resources:IN_Transfer_CreateFromStdReq, lbl_Desc_Nm %>"
                                SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td style="width: 70%;">
                            <asp:TextBox ID="txt_Desc" runat="server" Width="100%" Height="20px" TextMode="MultiLine"
                                SkinID="TXT_V1"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="grd_StoreReqFromStdReq" runat="server" SkinID="GRD_V1" AutoGenerateColumns="False"
                    ShowFooter="True" EmptyDataText="No data to display" OnRowDataBound="grd_StoreReqFromStdReq_RowDataBound"
                    Width="100%" HorizontalAlign="Left" EnableModelValidation="True">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderStyle Width="10%" HorizontalAlign="Left"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lbl_ProductCode_Nm" runat="server" Text="<%$ Resources:IN_Transfer_CreateFromStdReq, lbl_ProductCode_Nm %>"
                                    SkinID="LBL_HD_WHITE"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl_ProductCode" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle Width="30%" HorizontalAlign="Left"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lbl_EnglishName_Nm" runat="server" Text="<%$ Resources:IN_Transfer_CreateFromStdReq, lbl_EnglishName_Nm %>"
                                    SkinID="LBL_HD_WHITE"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl_EnglishName" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle Width="30%" HorizontalAlign="Left"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lbl_LocalName_Nm" runat="server" Text="<%$ Resources:IN_Transfer_CreateFromStdReq, lbl_LocalName_Nm %>"
                                    SkinID="LBL_HD_WHITE"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl_LocalName" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle Width="10%" HorizontalAlign="Right"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lbl_OnHand_Nm" runat="server" Text="<%$ Resources:IN_Transfer_CreateFromStdReq, lbl_OnHand_Nm %>"
                                    SkinID="LBL_HD_WHITE"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Unit">
                            <HeaderStyle Width="10%" HorizontalAlign="Left"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lbl_LocalName_Nm" runat="server" Text="<%$ Resources:IN_Transfer_CreateFromStdReq, lbl_LocalName_Nm %>"
                                    SkinID="LBL_HD_WHITE"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl_Unit" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle Width="10%" HorizontalAlign="Left" />
                            <HeaderTemplate>
                                <asp:Label ID="lbl_QtyReq_Nm" runat="server" Text="<%$ Resources:IN_Transfer_CreateFromStdReq, lbl_QtyReq_Nm %>"
                                    SkinID="LBL_HD_WHITE"></asp:Label>
                            </HeaderTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:TextBox ID="txt_Qty" runat="server" Width="95%" MaxLength="29" SkinID="TXT_NUM_V1"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <dx:ASPxPopupControl ID="pop_Warning" ClientInstanceName="pop_Warning" runat="server"
                    CloseAction="CloseButton" HeaderText="<%$ Resources:IN_Transfer_CreateFromStdReq, Warning %>"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False"
                    AllowResize="True">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" height="50px">
                                        <asp:Label ID="lbl_Warning" runat="server" SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <%--<dx:ASPxButton CausesValidation="false" ID="btn_Warning" runat="server" Text="OK">
                                <ClientSideEvents Click="function(s, e) {
	                                            pop_Warning.Hide();
	                                            return false;

                                            }" />
                            </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_Warning" runat="server" Text="<%$ Resources:IN_Transfer_CreateFromStdReq, btn_Warning %>"
                                            SkinID="BTN_V1" OnClick="btn_Warning_Click" Width="50px" />
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
            <div class="fix-layout" style="border-style: solid; border-width: 1px; border-color: #0071BD; background-color: #FFFFFF;
                width: 120px; height: 60px">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 120px; height: 60px">
                    <tr>
                        <td align="center">
                            <asp:Image ID="img_Loading3" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/ajax-loader.gif"
                                EnableViewState="False" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lbl_Loading3" runat="server" Font-Bold="true" Text="Loading..." EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
