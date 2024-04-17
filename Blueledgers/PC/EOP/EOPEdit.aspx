<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="EOPEdit.aspx.cs" Inherits="BlueLedger.PL.PC.EOP.EOPEdit" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dx" %>
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
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
                                                    <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:PC_EOP_EOPEdit, lbl_Title %>"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="right">
                                        <table border="0" cellpadding="2" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <dx:ASPxButton ID="btn_Save" runat="server" BackColor="Transparent" Height="16px" Width="42px" ToolTip="Save" OnClick="btn_Save_Click">
                                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/save.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                                        <HoverStyle>
                                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png" Repeat="NoRepeat" VerticalPosition="center" />
                                                        </HoverStyle>
                                                        <Border BorderStyle="None" />
                                                    </dx:ASPxButton>
                                                </td>
                                                <td>
                                                    <dx:ASPxButton ID="btn_Commit" runat="server" Width="51px" Height="16px" BackColor="Transparent" ToolTip="Commit" OnClick="btn_Commit_Click">
                                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/commit.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                                        <HoverStyle>
                                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/gray-commit.png" Repeat="NoRepeat" VerticalPosition="center" HorizontalPosition="center" />
                                                        </HoverStyle>
                                                        <Border BorderStyle="None" />
                                                    </dx:ASPxButton>
                                                </td>
                                                <td>
                                                    <dx:ASPxButton ID="btn_Print" runat="server" BackColor="Transparent" Height="16px" Width="43px" ToolTip="Print" OnClick="btn_Print_Click">
                                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                                        <HoverStyle>
                                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-print.png" Repeat="NoRepeat" VerticalPosition="center" />
                                                        </HoverStyle>
                                                        <Border BorderStyle="None" />
                                                    </dx:ASPxButton>
                                                </td>
                                                <td>
                                                    <dx:ASPxButton ID="btn_Delete" runat="server" BackColor="Transparent" Height="16px" Width="47px" ToolTip="Delete" OnClick="btn_Delete_Click">
                                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                                        <HoverStyle>
                                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png" Repeat="NoRepeat" VerticalPosition="center" />
                                                        </HoverStyle>
                                                        <Border BorderStyle="None" />
                                                    </dx:ASPxButton>
                                                </td>
                                                <td>
                                                    <dx:ASPxButton ID="btn_Back" runat="server" BackColor="Transparent" Height="16px" Width="42px" ToolTip="Back" OnClick="btn_Back_Click" CausesValidation="False">
                                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                                        <HoverStyle>
                                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png" Repeat="NoRepeat" VerticalPosition="center" />
                                                        </HoverStyle>
                                                        <Border BorderStyle="None" />
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <div>
                                <table border="0" cellpadding="2" cellspacing="0" width="100%" class="TABLE_HD">
                                    <tr>
                                        <td rowspan="3" style="width: 1%;">
                                        </td>
                                        <td style="width: 7%;">
                                            <asp:Label ID="lbl_Store_Nm" runat="server" Text="<%$ Resources:PC_EOP_EOPEdit, lbl_Store_Nm %>" SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td style="width: 40%;">
                                            <dx:ASPxComboBox ID="ddl_Store" runat="server" Width="98%" ValueType="System.String" AutoPostBack="True" IncrementalFilteringMode="Contains" OnSelectedIndexChanged="ddl_Store_SelectedIndexChanged"
                                                TextField="LocationName" ValueField="LocationCode" OnLoad="ddl_Store_Load" TextFormatString="{0} : {1}">
                                                <Columns>
                                                    <dx:ListBoxColumn Caption="Code" FieldName="LocationCode" Width="100px" />
                                                    <dx:ListBoxColumn Caption="Name" FieldName="LocationName" Width="250px" />
                                                </Columns>
                                                <ValidationSettings>
                                                    <RequiredField IsRequired="True" />
                                                </ValidationSettings>
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td style="width: 3%;">
                                            <asp:Label ID="lbl_Date_Nm" runat="server" Text="<%$ Resources:PC_EOP_EOPEdit, lbl_Date_Nm %>" SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td style="width: 8%;">
                                            <dx:ASPxDateEdit ID="txt_Date" runat="server" Width="100px">
                                                <ValidationSettings>
                                                    <RequiredField IsRequired="True" />
                                                </ValidationSettings>
                                            </dx:ASPxDateEdit>
                                        </td>
                                        <td style="width: 10%;">
                                            <asp:Label ID="lbl_EndDate_Nm" runat="server" Text="<%$ Resources:PC_EOP_EOPEdit, lbl_EndDate_Nm %>" SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_EndDate" runat="server" SkinID="LBL_NR"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_Desc_Nm" runat="server" Text="<%$ Resources:PC_EOP_EOPEdit, lbl_Desc_Nm %>" SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txt_Desc" runat="server" Width="96%" SkinID="TXT_V1"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_Status_Nm" runat="server" Text="<%$ Resources:PC_EOP_EOPEdit, lbl_Status_Nm %>" SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_Status" runat="server" SkinID="LBL_NR"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_Remark_Nm" runat="server" Text="<%$ Resources:PC_EOP_EOPEdit, lbl_Remark_Nm %>" SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td colspan="5">
                                            <asp:TextBox ID="txt_Remark" runat="server" Width="98%" SkinID="TXT_V1"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <div>
                                            <asp:Button ID="btn_SetZero" runat="server" Text="Set empty to zero" Style="float: right;" OnClick="btn_SetZero_Click" />
                                        </div>
                                        <div>
                                            <asp:GridView ID="grd_Product" runat="server" SkinID="GRD_V1" AutoGenerateColumns="False" ShowFooter="True" Width="100%" EmptyDataText="No data to display"
                                                OnRowDataBound="grd_Product_RowDataBound" OnPageIndexChanging="grd_Product_PageIndexChanging" PageSize="50">
                                                <PagerSettings Mode="NumericFirstLast" />
                                                <Columns>
                                                    <asp:BoundField HeaderText="<%$ Resources:PC_EOP_EOPEdit, ProductCode %>" DataField="ProductCode">
                                                        <HeaderStyle Width="10%" HorizontalAlign="Left"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="<%$ Resources:PC_EOP_EOPEdit, Descen %>" DataField="ProductDesc1">
                                                        <HeaderStyle Width="35%" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="<%$ Resources:PC_EOP_EOPEdit, Descll %>" DataField="ProductDesc2">
                                                        <HeaderStyle Width="35%" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="<%$ Resources:PC_EOP_EOPEdit, Unit %>" DataField="InventoryUnit">
                                                        <HeaderStyle Width="10%" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="<%$ Resources:PC_EOP_EOPEdit, Qty %>">
                                                        <ItemTemplate>
                                                            <%--<asp:TextBox ID="txt_Qty" runat="server" SkinID="TXT_NUM_V1" Width="96%"></asp:TextBox>--%>
                                                            <dx:ASPxSpinEdit ID="txt_Qty" runat="server" SkinID="TXT_NUM_V1" Width="96%" HorizontalAlign="Right">
                                                                <SpinButtons ShowIncrementButtons="false" />
                                                            </dx:ASPxSpinEdit>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="10%" HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr style="background-color: #11A6DE; height: 17px;">
                                                            <td style="width: 10%" align="left">
                                                                <asp:Label ID="lbl_ProductCode_Nm" runat="server" Text="<%$ Resources:PC_EOP_EOPEdit, lbl_ProductCode_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                                            </td>
                                                            <td style="width: 35%" align="left">
                                                                <asp:Label ID="lbl_Descen_Nm" runat="server" Text="<%$ Resources:PC_EOP_EOPEdit, lbl_Descen_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                                            </td>
                                                            <td style="width: 35%" align="left">
                                                                <asp:Label ID="lbl_Descll_Nm" runat="server" Text="<%$ Resources:PC_EOP_EOPEdit, lbl_Descll_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                                            </td>
                                                            <td style="width: 10%" align="left">
                                                                <asp:Label ID="lbl_Unit_Nm" runat="server" Text="<%$ Resources:PC_EOP_EOPEdit, lbl_Unit_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                                            </td>
                                                            <td style="width: 10%" align="right">
                                                                <asp:Label ID="lbl_Qty_Nm" runat="server" Text="<%$ Resources:PC_EOP_EOPEdit, lbl_Qty_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </EmptyDataTemplate>
                                                <%--<HeaderStyle HorizontalAlign="Left" />--%>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 9px">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btn_UncountItem" runat="server" SkinID="BTN_V1" Text="<%$ Resources:PC_EOP_EOPEdit, btn_UncountItem %>" OnClick="btn_UncountItem_Click"
                                            Width="120px" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <dx:ASPxPopupControl ID="pop_UpdateSuccess" runat="server" Height="120px" CloseAction="CloseButton" HeaderText="<%$ Resources:PC_EOP_EOPEdit, Information %>"
                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Width="240px">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" style="width: 100%;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_UpdateSuccess" runat="server" Text="<%$ Resources:PC_EOP_EOPEdit, lbl_UpdateSuccess %>" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btn_UpdateSuccess_OK" runat="server" OnClick="btn_UpdateSuccess_OK_Click" Text="<%$ Resources:PC_EOP_EOPEdit, btn_UpdateSuccess_OK %>"
                                        Width="50px" SkinID="BTN_V1" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_SaveConfirm" runat="server" Height="120px" Width="240px" CloseAction="CloseButton" HeaderText="Confirmation" Modal="True"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" style="width: 100%;">
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Label ID="lbl_SaveConfirm" runat="server" Text="<%$ Resources:PC_EOP_EOPEdit, lbl_SaveConfirm %>" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btn_SaveConfirm_OK" runat="server" Text="<%$ Resources:PC_EOP_EOPEdit, btn_SaveConfirm_OK %>" SkinID="BTN_V1" Width="50px" OnClick="btn_SaveConfirm_OK_Click" />
                                </td>
                                <td align="center">
                                    <asp:Button ID="btn_SaveConfirm_Cancel" runat="server" Text="<%$ Resources:PC_EOP_EOPEdit, btn_Delete_Cancel %>" Width="50px" SkinID="BTN_V1" OnClick="btn_SaveConfirm_Cancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_DeleteConfirm" runat="server" Height="120px" CloseAction="CloseButton" HeaderText="Confirmation" Modal="True" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" Width="300px" ClientInstanceName="pop_DeleteConfirm">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" style="width: 100%;">
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Label ID="lbl_DeleteConfirm" runat="server" Text="<%$ Resources:PC_EOP_EOPEdit, lbl_ConfirmDel %>" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btn_Delete_Ok" runat="server" Text="<%$ Resources:PC_EOP_EOPEdit, btn_Delete_Ok %>" Width="50px" SkinID="BTN_V1" OnClick="btn_Delete_Ok_Click" />
                                </td>
                                <td align="center">
                                    <asp:Button ID="btn_Delete_Cancel" runat="server" Text="<%$ Resources:PC_EOP_EOPEdit, btn_Delete_Cancel %>" Width="50px" SkinID="BTN_V1" OnClick="btn_Delete_Cancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_CommitConfirm" runat="server" Height="120px" CloseAction="CloseButton" HeaderText="Confirmation" Modal="True" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" Width="240px" ClientInstanceName="pop_CommitConfirm">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" style="width: 100%;">
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Label ID="lbl_CommitConfirm" runat="server" Text="<%$ Resources:PC_EOP_EOPEdit, lbl_CommitConfirm %>" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btn_CommitConfirm_Yes" runat="server" Text="<%$ Resources:PC_EOP_EOPEdit, btn_Yes %>" Width="50px" SkinID="BTN_V1" OnClick="btn_CommitConfirm_Yes_Click" />
                                </td>
                                <td align="center">
                                    <asp:Button ID="btn_CommitConfirm_No" runat="server" Text="<%$ Resources:PC_EOP_EOPEdit, btn_No %>" Width="50px" SkinID="BTN_V1" OnClick="btn_CommitConfirm_No_Click" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_Alert" runat="server" Height="120px" CloseAction="CloseButton" HeaderText="<%$ Resources:PC_EOP_EOPEdit, Warning %>" Modal="True"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Width="300px" ClientInstanceName="pop_Alert">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" style="width: 100%;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_Alert" runat="server" SkinID="LBL_NR" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <br />
                                    <asp:Button ID="btn_Alert_OK" runat="server" SkinID="BTN_V1" Width="50px" Text="OK" OnClick="btn_Alert_OK_Click" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UdPgDetail" PopupControlID="UdPgDetail" BackgroundCssClass="POPUP_BG"
                RepositionMode="RepositionOnWindowResizeAndScroll">
            </ajaxToolkit:ModalPopupExtender>
            <asp:UpdateProgress ID="UdPgDetail" runat="server" AssociatedUpdatePanelID="UdPnDetail">
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
</asp:Content>
