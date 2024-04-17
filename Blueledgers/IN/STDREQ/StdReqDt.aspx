<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StdReqDt.aspx.cs" Inherits="BlueLedger.PL.IN.STDREQ.StdReqDt"
    MasterPageFile="~/Master/In/SkinDefault.master" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cph_Main">
    <style type="text/css">
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
    </style>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr style="background-color: #4d4d4d; height: 17px; padding-left: 10px">
            <td style="padding-left: 10px; width: 10px">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
            </td>
            <td align="left">
                <asp:Label ID="lbl_StdReq_Nm" runat="server" Text="<%$ Resources:IN_STDREQ_StdReqDt, lbl_StdReq_Nm %>"
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
                        <dx:MenuItem Name="Edit" Text="">
                            <ItemStyle Height="16px" Width="38px">
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-edit.png"
                                        Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/edit.png"
                                    Repeat="NoRepeat" VerticalPosition="center" />
                            </ItemStyle>
                        </dx:MenuItem>
                        <dx:MenuItem Name="Delete" Text="">
                            <ItemStyle Height="16px" Width="47px">
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png"
                                        Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png" Repeat="NoRepeat"
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
                        <dx:MenuItem Name="Copy" Text="">
                            <ItemStyle Height="16px" Width="45px">
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-copy.png"
                                        Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/copy.png" Repeat="NoRepeat"
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
            </td>
        </tr>
    </table>
    <div class="printable">
        <table border="0" cellpadding="1" cellspacing="0" width="100%" class="TABLE_HD">
            <tr>
                <td class="TD_LINE" align="left" style="padding-left: 10px; width: 12.5%">
                    <asp:Label ID="lbl_Ref_Nm" runat="server" Text="<%$ Resources:IN_STDREQ_StdReqDt, lbl_Ref_Nm %>"
                        SkinID="LBL_HD"></asp:Label>
                </td>
                <td class="TD_LINE" align="left" style="width: 12.5%">
                    <asp:Label ID="lbl_Ref" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
                <td class="TD_LINE" align="left" style="width: 10%">
                    <asp:Label ID="lbl_Status_Nm" runat="server" Text="<%$ Resources:IN_STDREQ_StdReqDt, lbl_Status_Nm %>"
                        SkinID="LBL_HD"></asp:Label>
                </td>
                <td class="TD_LINE" align="left" style="width: 10%">
                    <asp:CheckBox ID="chk_Status" runat="server" Text="Active" ReadOnly="True" SkinID="CHK_V1"
                        Enabled="False" />
                </td>
                <td class="TD_LINE" align="left" style="width: 10%">
                    <asp:Label ID="lbl_FromStore_Nm" runat="server" Text="<%$ Resources:IN_STDREQ_StdReqDt,lbl_ToStore_nm %>"
                        SkinID="LBL_HD"></asp:Label>
                </td>
                <td class="TD_LINE" align="left" style="width: 10%">
                    <asp:Label ID="lbl_Store" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
                <td class="TD_LINE" align="left" style="width: 10%">
                    <asp:Label ID="lbl_Name_Nm" runat="server" Text="<%$ Resources:IN_STDREQ_StdReqDt, lbl_Name_Nm %>"
                        SkinID="LBL_HD"></asp:Label>
                </td>
                <td class="TD_LINE" align="left" style="width: 25%">
                    <asp:Label ID="lbl_StoreName" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
            </tr>
            <tr align="left" style="height: 18; vertical-align: top">
                <td class="TD_LINE" align="left" style="width: 12.5%; padding-left: 10px">
                    <asp:Label ID="lbl_Desc_Nm" runat="server" Text="<%$ Resources:IN_STDREQ_StdReqDt, lbl_Desc_Nm %>"
                        SkinID="LBL_HD"></asp:Label>
                </td>
                <td colspan="3" style="width: 81.5%;">
                    <asp:Label ID="lbl_Desc" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:GridView ID="grd_StdReqDt1" runat="server" AutoGenerateColumns="False" EmptyDataText="No Data to Display"
            EnableModelValidation="True" OnRowDataBound="grd_StdReqDt1_RowDataBound" SkinID="GRD_V1"
            Width="100%">
            <Columns>
                <asp:TemplateField HeaderText="<%$ Resources:IN_STDREQ_StdReqDt, lbl_SKU_Nm %>">
                    <ItemTemplate>
                        <asp:Label ID="lbl_ProductCode" runat="server" SkinID="LBL_NR" Width="100%"></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:IN_STDREQ_StdReqDt, lbl_EnglishName_Nm %>">
                    <ItemTemplate>
                        <asp:Label ID="lbl_ProductDesc1" runat="server" SkinID="LBL_NR" Width="100%"></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="35%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:IN_STDREQ_StdReqDt, lbl_LocalName_Nm %>">
                    <ItemTemplate>
                        <asp:Label ID="lbl_ProductDesc2" runat="server" SkinID="LBL_NR" Width="100%"></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="35%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:IN_STDREQ_StdReqDt, lbl_Unit_Nm %>">
                    <ItemTemplate>
                        <asp:Label ID="lbl_OrderUnit" runat="server" SkinID="LBL_NR" Width="100%"></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <table width="100%">
        <tr>
            <td style="padding-left: 10px; padding-top: 3px;">
                <asp:LinkButton ID="lnk_Requisition" runat="server" ForeColor="#0082c8" Font-Bold="True"
                    OnClick="lnk_Requisition_Click" Font-Size="8pt" Font-Names="Arial" Text="<%$ Resources:IN_STDREQ_StdReqDt, lnk_Requisition %>"></asp:LinkButton>
            </td>
        </tr>
    </table>
    <asp:ObjectDataSource ID="ods_StdTemplate" runat="server" SelectMethod="GetListByRefId"
        TypeName="Blue.BL.IN.StandardRequisitionDetail">
        <SelectParameters>
            <asp:QueryStringParameter Name="RefId" QueryStringField="ID" Type="String" />
            <asp:ControlParameter ControlID="hf_ConnStr" Name="connStr" PropertyName="Value"
                Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:HiddenField ID="hf_ConnStr" runat="server" />
    <dx:ASPxPopupControl ID="pop_ConfirmDelete" runat="server" Width="300px" CloseAction="CloseButton"
        HeaderText="<%$ Resources:IN_STDREQ_StdReqDt, lbl_Warning_HD_POP %>" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" colspan="2" height="50px">
                            <asp:Label ID="lbl_ConfirmDeleteTeplate_OP_Nm" runat="server" Text="<%$ Resources:IN_STDREQ_StdReqDt, lbl_ConfirmDeleteTeplate_OP_Nm %>"
                                SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Button ID="btn_ConfirmDelete_pop" runat="server" Text="<%$ Resources:IN_STDREQ_StdReqDt, btn_ConfirmDelete_pop %>"
                                SkinID="BTN_V1" Width="60px" OnClick="btn_ConfirmDelete_pop_Click" />
                        </td>
                        <td align="left">
                            <asp:Button ID="btn_CancelDelete_pop" runat="server" OnClick="btn_CancelDelete_pop_Click"
                                SkinID="BTN_V1" Text="<%$ Resources:IN_STDREQ_StdReqDt, btn_CancelDelete_pop %>"
                                Width="60px" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_Delete" runat="server" HeaderText="<%$ Resources:IN_STDREQ_StdReqDt, lbl_Information_HD_POP %>"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Width="300px"
        Modal="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" height="50px">
                            <asp:Label ID="lbl_DeleteSuc_Nm" runat="server" Text="<%$ Resources:IN_STDREQ_StdReqDt, lbl_DeleteSuc_Nm %>"
                                SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btn_Delete_Success" runat="server" Text="<%$ Resources:IN_STDREQ_StdReqDt, btn_Delete_Success %>"
                                SkinID="BTN_V1" Width="60px" OnClick="btn_Delete_Success_Click" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
