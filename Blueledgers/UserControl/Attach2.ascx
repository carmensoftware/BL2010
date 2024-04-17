<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Attach2.ascx.cs" Inherits="BlueLedger.PL.UserControls.Attach2" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>

<%--<asp:UpdatePanel ID="UdPnDetail" runat="server">--%>
    <%--<ContentTemplate>--%>
        <div>
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr style="background-color: #4D4D4D;">
                    <td style="padding-left: 10px;">
                        <table border="0" cellpadding="2" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_attachment.png" />
                                </td>
                                <td>
                                    <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:UserControl_Attach2, lbl_Title %>"
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
                        <asp:GridView ID="grd_Attach" runat="server" SkinID="GRD_V1" AutoGenerateColumns="False"
                            OnRowCancelingEdit="grd_Attach_RowCancelingEdit" OnRowDataBound="grd_Attach_RowDataBound"
                            OnRowEditing="grd_Attach_RowEditing" OnRowUpdating="grd_Attach_RowUpdating" OnRowDeleting="grd_Attach_RowDeleting"
                            Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="&nbsp;&nbsp;&nbsp;&nbsp;#">
                                    <ItemTemplate>
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:LinkButton ID="lnkb_Edit" runat="server" CausesValidation="False" CommandName="Edit" SkinID="LNKB_NORMAL" Text="Edit"></asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="lnkb_Del" runat="server" CausesValidation="false" CommandName="Delete" SkinID="LNKB_NORMAL" Text="Del" OnClientClick="return confirm('Do you want to delete this file?')"></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <table border="0" cellpadding="1" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:LinkButton ID="lnkb_Update" runat="server" CommandName="Update" SkinID="LNKB_NORMAL"
                                                        Text="Update"></asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="lnkb_Cancel" runat="server" CausesValidation="false" CommandName="Cancel"
                                                        SkinID="LNKB_NORMAL" Text="Cancel"></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </EditItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lbl_FileName" runat="server" SkinID="LBL_BOLD_WHITE" Text="<%$ Resources:UserControl_Attach2, lbl_FileName %>"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkb_FileName" runat="server" SkinID="LNKB_NORMAL" OnClick="lnkb_FileName_Click"></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:FileUpload ID="fu_FileName" runat="server" Font-Names="Trebuchet MS,Tahoma,MS Sans Serif"
                                            Font-Size="9pt" ForeColor="#363636" BorderColor="#DFDFDF" BorderStyle="Solid"
                                            BorderWidth="1px" Width="98%" ondatabinding="fu_FileName_DataBinding"  />
                                    </EditItemTemplate>
                                    <HeaderStyle Width="30%" HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lbl_Description" runat="server" SkinID="LBL_BOLD_WHITE" Text="<%$ Resources:UserControl_Attach2, lbl_Description %>"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Description" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txt_Description" runat="server" SkinID="TXT_NORMAL" Width="98%"></asp:TextBox>
                                    </EditItemTemplate>
                                    <HeaderStyle Width="30%" HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lbl_Public" runat="server" SkinID="LBL_BOLD_WHITE" Text="<%$ Resources:UserControl_Attach2, lbl_Public %>"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk_IsPublic" runat="server" SkinID="CHK_NORMAL" Enabled="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="chk_IsPublic" runat="server" SkinID="CHK_NORMAL" />
                                    </EditItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="7%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lbl_Date" runat="server" SkinID="LBL_BOLD_WHITE" Text="<%$ Resources:UserControl_Attach2, lbl_Date %>"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Date" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="8%" HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lbl_By" runat="server" SkinID="LBL_BOLD_WHITE" Text="<%$ Resources:UserControl_Attach2, lbl_By %>"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_By" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="15%" HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr style="padding: 0px 0px 0px 0px; height: 17px; background-color: #2196f3;">
                                        <td style="padding: 0px 0px 0px 20px; width: 10%;" align="center">
                                            <asp:Label ID="Label1" runat="server" Text="#" SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 30%">
                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:UserControl_Attach2, lbl_FileName %>"
                                                SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 30%">
                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:UserControl_Attach2, lbl_Description %>"
                                                SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 7%">
                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:UserControl_Attach2, lbl_Public %>"
                                                SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 8%">
                                            <asp:Label ID="Label5" runat="server" Text="<%$ Resources:UserControl_Attach2, lbl_Date %>"
                                                SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 15%">
                                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:UserControl_Attach2, lbl_By %>"
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
    <%-- 
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
--%>
    <%--</ContentTemplate>--%>
<%--</asp:UpdatePanel>--%>