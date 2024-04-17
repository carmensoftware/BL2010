<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StdReqEdit.aspx.cs" Inherits="BlueLedger.PL.IN.STDREQ.StdReqEdit"
    MasterPageFile="~/Master/In/SkinDefault.master" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cph_Main">
    <asp:UpdatePanel ID="UdPnDetail" runat="server">
        <ContentTemplate>
            <div>
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr style="background-color: #4D4D4D; height: 17px">
                        <td style="padding-left: 10px; width: 10px">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_StdReq_Nm" runat="server" Text="<%$ Resources:IN_STDREQ_StdReqEdit, lbl_StdReq_Nm %>"
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
                                    <dx:MenuItem Name="Save" Text="">
                                        <ItemStyle Height="16px" Width="42px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/save.png"
                                                Repeat="NoRepeat" VerticalPosition="center" />
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
                        </td>
                    </tr>
                </table>
                <div>
                    <table border="0" cellpadding="1" cellspacing="0" width="100%">
                        <tr id="tr_Header1" runat="server">
                            <td align="left" class="TD_LINE" style="padding-left: 10px; width: 15%">
                                <asp:Label ID="lbl_Ref_Hd" runat="server" Text="<%$ Resources:IN_STDREQ_StdReqEdit, lbl_Ref_Hd %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE" align="left" style="width: 30%">
                                <asp:Label ID="lbl_RefID" runat="server" SkinID="LBL_NR"></asp:Label>
                            </td>
                            <td class="TD_LINE" align="left" style="width: 15%">
                                <asp:Label ID="lbl_Status_Nm" runat="server" Text="<%$ Resources:IN_STDREQ_StdReqEdit, lbl_Status_Nm %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE" align="left" style="width: 40%">
                                <asp:CheckBox ID="chk_Status" runat="server" Text="Active" Height="17px" SkinID="CHK_V1" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="TD_LINE" style="padding-left: 10px; width: 15%">
                                <asp:Label ID="lbl_Store_Nm" runat="server" Text="<%$ Resources:IN_STDREQ_StdReqEdit, lbl_StoreTo_Nm %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE" align="left" style="width: 30%">
                                <dx:ASPxComboBox ID="ddl_StoreLocation" runat="server" ValueType="System.String"
                                    TextFormatString="{0} : {1}" ValueField="LocationCode" IncrementalFilteringMode="Contains"
                                    OnLoad="ddl_StoreLocation_Load" Width="100%">
                                    <Columns>
                                        <dx:ListBoxColumn FieldName="LocationCode" Caption="Code" Width="100px" />
                                        <dx:ListBoxColumn FieldName="LocationName" Caption="Name" Width="200px" />
                                    </Columns>
                                    <ValidationSettings>
                                        <RequiredField IsRequired="True" />
                                    </ValidationSettings>
                                </dx:ASPxComboBox>
                            </td>
                            <td class="TD_LINE" align="left" style="width: 15%">
                                <asp:Label ID="lbl_Desc_Nm" runat="server" Text="<%$ Resources:IN_STDREQ_StdReqEdit, lbl_Desc_Nm %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE" align="left" style="width: 40%; padding-right: 10px">
                                <asp:TextBox ID="txt_Desc" runat="server" Width="100%" TextMode="MultiLine" SkinID="TXT_V1"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                    <tr style="background-color: #4d4d4d; height: 17px">
                        <td style="padding-left: 10px;" align="left">
                            <asp:Label ID="lbl_Assign_Nm" runat="server" Text="<%$ Resources:IN_STDREQ_StdReqEdit, lbl_Assign_Nm %>"
                                SkinID="LBL_HD_WHITE"></asp:Label>
                        </td>
                        <td align="right" style="padding-right: 10px;">
                            <dx:ASPxMenu runat="server" ID="menu_CmdGrd" Font-Bold="True" BackColor="Transparent"
                                Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle" Height="17px"
                                OnItemClick="menu_CmdGrd_ItemClick">
                                <ItemStyle BackColor="Transparent">
                                    <HoverStyle BackColor="Transparent">
                                        <Border BorderStyle="None" />
                                    </HoverStyle>
                                    <Paddings Padding="2px" />
                                    <Border BorderStyle="None" />
                                </ItemStyle>
                                <Items>
                                    <dx:MenuItem Name="Create" Text="">
                                        <ItemStyle Height="16px" Width="42px">
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
                </table>
                <div>
                    <dx:ASPxTreeList ID="tl_StdReqEdit" runat="server" AutoGenerateColumns="False" Width="100%"
                        KeyFieldName="CategoryCode" ParentFieldName="ParentNo" OnLoad="tl_StdReqEdit_Load"
                        SkinID="GRD_V1">
                        <SettingsLoadingPanel ImagePosition="Top" />
                        <SettingsSelection AllowSelectAll="True" Enabled="True" Recursive="True" />
                        <Columns>
                            <dx:TreeListTextColumn Caption="<%$ Resources:IN_STDREQ_StdReqEdit, lbl_Assign_Nm %>"
                                VisibleIndex="0" FieldName="CategoryName" Width="200px">
                                <CellStyle HorizontalAlign="Left">
                                </CellStyle>
                            </dx:TreeListTextColumn>
                            <dx:TreeListTextColumn Caption="<%$ Resources:IN_STDREQ_StdReqEdit, lbl_Product_Nm %>"
                                VisibleIndex="1" FieldName="ProductDesc2" Width="200px">
                                <CellStyle HorizontalAlign="Left">
                                </CellStyle>
                            </dx:TreeListTextColumn>
                        </Columns>
                    </dx:ASPxTreeList>
                </div>
                <asp:ObjectDataSource ID="ods_StoreLocation" runat="server" SelectMethod="GetList"
                    TypeName="Blue.BL.Option.Inventory.StoreLct" OldValuesParameterFormatString="original_{0}">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hf_LoginName" Name="LoginName" PropertyName="Value"
                            Type="String" />
                        <asp:ControlParameter ControlID="hf_ConnStr" Name="connStr" PropertyName="Value"
                            Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:HiddenField ID="hf_ConnStr" runat="server" />
                <asp:HiddenField ID="hf_LoginName" runat="server" />
                <asp:HiddenField ID="hf_TreeView" runat="server" />
                <dx:ASPxPopupControl ID="pop_ChkDetail" runat="server" HeaderText="<%$ Resources:IN_STDREQ_StdReqEdit, lbl_Warning_HD %>"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Width="300px"
                    Modal="True">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" height="50px">
                                        <asp:Label ID="lbl_ClickCreateBtn_Nm" runat="server" Text="<%$ Resources:IN_STDREQ_StdReqEdit, lbl_ClickCreateBtn_Nm %>"
                                            SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btn_ChkDetail_OK" runat="server" Text="<%$ Resources:IN_STDREQ_StdReqEdit, btn_ChkDetail_OK %>"
                                            OnClick="btn_ChkDetail_OK_Click" SkinID="BTN_V1" Width="70px" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="pop_ChkStore" runat="server" HeaderText="<%$ Resources:IN_STDREQ_StdReqEdit, lbl_Warning_HD %>"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Width="300px"
                    Modal="True">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" height="50px">
                                        <asp:Label ID="lbl_ClickSelectStore_Nm" runat="server" Text="<%$ Resources:IN_STDREQ_StdReqEdit, lbl_ClickSelectStore_Nm %>"
                                            SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btn_ChkStore_OK" runat="server" Text="<%$ Resources:IN_STDREQ_StdReqEdit, btn_ChkStore_OK %>"
                                            OnClick="btn_ChkStore_OK_Click" SkinID="BTN_V1" Width="70px" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="pop_Save" runat="server" HeaderText="<%$ Resources:IN_STDREQ_StdReqEdit, lbl_Information_HD %>"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Width="300px"
                    Modal="True">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" height="50px">
                                        <asp:Label ID="lbl_SvaeSuc_Nm" runat="server" Text="<%$ Resources:IN_STDREQ_StdReqEdit, lbl_SvaeSuc_Nm %>"
                                            SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btn_Save_Success" runat="server" Text="<%$ Resources:IN_STDREQ_StdReqEdit, btn_Save_Success %>"
                                            SkinID="BTN_V1" Width="70px" OnClick="btn_Save_Success_Click" />
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
