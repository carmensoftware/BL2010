<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdjTypeLst.aspx.cs" Inherits="BlueLedger.PL.IN.AdjTypeLst"
    MasterPageFile="~/Master/In/SkinDefault.master" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cph_Main">
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
    <asp:UpdatePanel ID="UdPnDetail" runat="server">
        <ContentTemplate>
            <div>
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr style="background-color: #4D4D4D; height: 17px">
                        <td style="padding-left: 10px; width: 10px">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_AdjustType_Nm" runat="server" Text="<%$ Resources:IN_AdjTypeLst, lbl_AdjustType_Nm %>"
                                SkinID="LBL_HD_WHITE"></asp:Label>
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
                                    <dx:MenuItem Name="Delete" Text="">
                                        <ItemStyle Height="16px" Width="38px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png"
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
                </table>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" style="table-layout: fixed">
                    <tr>
                        <td style="width: 100%">
                            <div style="overflow: auto; width: 100%;">
                                <asp:GridView ID="grd_AdjType1" runat="server" SkinID="GRD_V1" AutoGenerateColumns="False"
                                    EmptyDataText="No Data to Display" Width="100%" EnableModelValidation="True"
                                    OnRowDataBound="grd_AdjType1_RowDataBound" OnRowCancelingEdit="grd_AdjType1_RowCancelingEdit"
                                    OnRowEditing="grd_AdjType1_RowEditing" OnRowUpdating="grd_AdjType1_RowUpdating">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderStyle Width="20px" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="20px" />
                                            <FooterStyle />
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="Chk_All" runat="server" onclick="Check(this)" Width="20px" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Chk_Item" runat="server" Width="20px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:CommandField HeaderText="#" ShowEditButton="True" DeleteText="Del">
                                            <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                        </asp:CommandField>
                                        <asp:TemplateField HeaderText="<%$ Resources:IN_AdjTypeLst, lbl_Code_Grd_Nm %>">
                                            <HeaderStyle Width="50px" HorizontalAlign="Right" />
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txt_Code" runat="server" Enabled="False" SkinID="TXT_V1" Width="50px"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Code" runat="server" Width="50px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:IN_AdjTypeLst, lbl_AdjType_Grd_Nm %>">
                                            <HeaderStyle Width="100px" HorizontalAlign="Left" />
                                            <EditItemTemplate>
                                                <dx:ASPxComboBox ID="ddl_AdjType" runat="server" SelectedIndex="0" ValueType="System.String"
                                                    Width="90%">
                                                    <Items>
                                                        <dx:ListEditItem Text="Issue" Value="Issue" />
                                                        <dx:ListEditItem Text="Stock In" Value="Stock In" />
                                                        <dx:ListEditItem Text="Stock Out" Value="Stock Out" />
                                                        <dx:ListEditItem Text="Transfer" Value="Transfer" />
                                                    </Items>
                                                </dx:ASPxComboBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_AdjType" runat="server" Width="80px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:IN_AdjTypeLst, lbl_AdjCode_Grd_Nm %>">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txt_AdjCode" runat="server" SkinID="TXT_V1" MaxLength="5" Width="70px"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_AdjCode" runat="server" SkinID="LBL_NR" Width="70px"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:IN_AdjTypeLst, lbl_AdjName_Grd_Nm %>">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txt_AdjName" runat="server" SkinID="TXT_V1"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_AdjName" runat="server" SkinID="LBL_NR" Width="170px"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:IN_AdjTypeLst, lbl_Desc_Grd_Nm %>">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txt_Desc" runat="server" SkinID="TXT_V1"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Desc" runat="server" SkinID="LBL_NR" Width="170px"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:IN_AdjTypeLst, lbl_Actived_Grd_Nm %>">
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="chk_Actived" runat="server" SkinID="CHK_V1" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Img_Btn_ChkBox" runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr style="padding: 0px 0px 0px 0px; height: 18px;">
                                                <td style="padding: 0px 0px 0px 10px; width: 100px;">
                                                    <asp:Label ID="lbl_Command" runat="server" Text="#" CssClass="lbl_HD_W"></asp:Label>
                                                </td>
                                                <td style="width: 100px">
                                                    <asp:Label ID="lbl_Date" runat="server" Text="Date" CssClass="lbl_HD_W"></asp:Label>
                                                </td>
                                                <td style="width: 150px">
                                                    <asp:Label ID="lbl_By" runat="server" Text="By" CssClass="lbl_HD_W"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_Comment" runat="server" Text="Comment" CssClass="lbl_HD_W"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
                <dx:ASPxPopupControl ID="pop_ConfrimDelete" runat="server" Width="300px" CloseAction="CloseButton"
                    HeaderText="<%$ Resources:IN_AdjTypeLst, lbl_Warning_HD %>" Modal="True" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" colspan="2" height="50px">
                                        <asp:Label ID="lbl_ConfirmDelete_Nm" runat="server" Text="<%$ Resources:IN_AdjTypeLst, lbl_ConfirmDelete_Nm %>"
                                            SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btn_ConfrimDelete" runat="server" Text="<%$ Resources:IN_AdjTypeLst, btn_ConfrimDelete %>"
                                            SkinID="BTN_V1" OnClick="btn_ConfrimDelete_Click" Width="50px" />
                                    </td>
                                    <td align="left">
                                        <asp:Button ID="btn_CancelDelete" runat="server" Text="<%$ Resources:IN_AdjTypeLst, btn_CancelDelete %>"
                                            Width="50px" SkinID="BTN_V1" OnClick="btn_CancelDelete_Click" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                    <HeaderStyle HorizontalAlign="Left" />
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="pop_Warning" runat="server" Width="300px" CloseAction="CloseButton"
                    HeaderText="<%$ Resources:IN_AdjTypeLst, lbl_Warning_HD %>" Modal="True" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" height="50px">
                                        <asp:Label ID="lbl_Warning" runat="server" SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btn_Ok" runat="server" OnClick="btn_Ok_Click" Text="<%$ Resources:IN_AdjTypeLst, btn_Ok %>"
                                            Width="50px" SkinID="BTN_V1" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
            </div>
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
