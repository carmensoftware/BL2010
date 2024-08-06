<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MoveMentEdit.aspx.cs" Inherits="BlueLedger.PL.IN.MM.MoveMentEdit"
    MasterPageFile="~/Master/In/SkinDefault.master" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
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
                                        <asp:Label ID="lbl_Movement" runat="server" Text="Movement" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right">
                            <table border="0" cellpadding="2" cellspacing="0">
                                <tr>
                                    <td>
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
                                                <dx:MenuItem Name="Commit" Text="">
                                                    <ItemStyle Height="16px" Width="51px">
                                                        <HoverStyle>
                                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-commit.png"
                                                                Repeat="NoRepeat" VerticalPosition="center" />
                                                        </HoverStyle>
                                                        <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/commit.png"
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
                                <%--<tr>
                                    <td>
                                        <dx:ASPxButton ID="btn_Save" runat="server" BackColor="Transparent" OnClick="btn_Save_Click"
                                            ToolTip="Save" Height="16px" Width="42px">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/save.png" Repeat="NoRepeat"
                                                HorizontalPosition="center" VerticalPosition="center" />
                                            <HoverStyle>
                                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png"
                                                    HorizontalPosition="center" VerticalPosition="center" />
                                            </HoverStyle>
                                            <Border BorderStyle="None" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td id="td_Commit" runat="server">
                                        <dx:ASPxButton ID="btn_Commit" runat="server" BackColor="Transparent" OnClick="btn_Commit_Click"
                                            ToolTip="Commit" Height="16px" Width="51px">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/commit.png" Repeat="NoRepeat"
                                                HorizontalPosition="center" VerticalPosition="center" />
                                            <HoverStyle>
                                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/gray-commit.png"
                                                    HorizontalPosition="center" VerticalPosition="center" />
                                            </HoverStyle>
                                            <Border BorderStyle="None" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btn_Back" runat="server" BackColor="Transparent" OnClick="btn_Back_Click"
                                            ToolTip="Back" Height="16px" Width="42px">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat"
                                                HorizontalPosition="center" VerticalPosition="center" />
                                            <HoverStyle>
                                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png"
                                                    HorizontalPosition="center" VerticalPosition="center" />
                                            </HoverStyle>
                                            <Border BorderStyle="None" />
                                        </dx:ASPxButton>
                                    </td>
                                </tr>--%>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table border="0" cellpadding="2" cellspacing="0" width="100%" class="TABLE_HD">
        <tr>
            <td rowspan="4" style="width: 1%;">
            </td>
            <td style="width: 12.5%">
                <asp:Label ID="Label6" runat="server" Text="Ref#:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td style="width: 12.5%">
                <asp:Label ID="lbl_Ref" runat="server" SkinID="LBL_NR"></asp:Label>
            </td>
            <td align="left" style="width: 7.25%">
                &nbsp;
            </td>
            <td style="width: 12.5%">
                <asp:Label ID="lbl_Type_HD" runat="server" Text="Type:" SkinID="LBL_HD"></asp:Label>
                <asp:Label ID="lbl_FromStore_HD" runat="server" Text="From Store:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td style="width: 25%" colspan="2">
                <dx:ASPxComboBox ID="ddl_Type" runat="server" TextFormatString="{0} : {1}" ValueField="AdjId"
                    OnLoad="ddl_Type_Load" Width="95%">
                    <Columns>
                        <dx:ListBoxColumn FieldName="AdjType" Caption="Type" />
                        <dx:ListBoxColumn FieldName="AdjName" Caption="Name" />
                    </Columns>
                </dx:ASPxComboBox>
                <asp:Label ID="lbl_FromStore" runat="server" SkinID="LBL_NR" Visible="false"></asp:Label>
                <asp:Label ID="lbl_FromStoreName" runat="server" SkinID="LBL_NR"></asp:Label>
            </td>
            <td style="width: 17.5%">
                &nbsp;
            </td>
            <td style="width: 10%">
                <asp:Label ID="Label9" runat="server" Text="Status:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td style="width: 5%">
                <asp:Label ID="lbl_Status" runat="server" SkinID="LBL_NR"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label7" runat="server" Text="Date:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lbl_CreatedDate" runat="server" SkinID="LBL_NR"></asp:Label>
            </td>
            <td align="left" style="width: 7.25%">
                &nbsp;
            </td>
            <td>
                <asp:Label ID="lbl_ToStore_HD" runat="server" Text="To Store:" SkinID="LBL_HD"></asp:Label>
                <asp:Label ID="lbl_CommittedDate_HD" runat="server" Text="Commit Date:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="lbl_ToStore" runat="server" SkinID="LBL_NR" Visible="false"></asp:Label>
                <asp:Label ID="lbl_ToStoreName" runat="server" SkinID="LBL_NR"></asp:Label>
                <asp:Label ID="lbl_CommittedDate_SIO" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr id="tr_HLable" runat="server">
            <td id="td_DeliDate_HD" runat="server">
                <asp:Label ID="lbl_DeliveryDate_HD" runat="server" Text="Delivery Date:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td colspan="2" style="overflow: hidden; white-space: nowrap; width: 350px" id="td_DeliDate_lbl">
                <%--Stock In Panel--%>
                <dx:ASPxDateEdit ID="dte_DeliDate" runat="server">
                </dx:ASPxDateEdit>
            </td>
            <td id="td_FromStoreName_HD" runat="server">
                <asp:Label ID="lbl_Commit_HD_TIO" runat="server" Text="Commit Date:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td id="td_FromStoreName" runat="server" colspan="2">
                <asp:Label ID="lbl_CommittedDate" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
            </td>
            <td>
                &nbsp;
            </td>
            <td colspan="3">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="vertical-align: top">
                <asp:Label ID="Label8" runat="server" Text="Description:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td colspan="9">
                <asp:TextBox ID="txt_Desc" runat="server" TextMode="MultiLine" Width="98%" SkinID="TXT_V1"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr style="background-color: #4d4d4d; height: 17px">
            <td style="padding-left: 10px;">
                <asp:Label ID="Label27" runat="server" Text="Movement" SkinID="LBL_HD_WHITE"></asp:Label>
            </td>
            <td align="right">
                <table border="0" cellpadding="2" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <dx:ASPxMenu runat="server" ID="menu_CmdGrd" Font-Bold="True" BackColor="Transparent"
                                Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle" Height="16px"
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
                                </Items>
                                <Paddings Padding="0px" />
                                <SeparatorPaddings Padding="0px" />
                                <SubMenuStyle HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" Font-Size="9pt"
                                    ForeColor="#4D4D4D" />
                                <Border BorderStyle="None"></Border>
                            </dx:ASPxMenu>
                            <%--<dx:ASPxButton ID="btn_Create" runat="server" BackColor="Transparent" ToolTip="Create"
                                OnClick="btn_Create_Click" Width="49px" Height="16px">
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/create.png" HorizontalPosition="center"
                                    VerticalPosition="center" Repeat="NoRepeat" />
                                <HoverStyle>
                                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png"
                                        HorizontalPosition="center" VerticalPosition="center" />
                                </HoverStyle>
                                <Border BorderStyle="None" />
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="btn_Delete" runat="server" BackColor="Transparent" ToolTip="Delete"
                                Width="47px" Height="16px">
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png" Repeat="NoRepeat"
                                    HorizontalPosition="center" VerticalPosition="center" />
                                <HoverStyle>
                                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png"
                                        HorizontalPosition="center" VerticalPosition="center" />
                                </HoverStyle>
                                <Border BorderStyle="None" />
                            </dx:ASPxButton>--%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:GridView ID="grd_MovementDt" runat="server" SkinID="GRD_V1" AutoGenerateColumns="False"
        Width="100%" EmptyDataText="No Data to Display" OnRowDataBound="grd_MovementDt_RowDataBound"
        OnRowCancelingEdit="grd_MovementDt_RowCancelingEdit" OnRowEditing="grd_MovementDt_RowEditing"
        OnRowUpdating="grd_MovementDt_RowUpdating">
        <Columns>
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:CheckBox ID="chk_All" runat="server" Width="10px" onclick="Check(this)" />
                </HeaderTemplate>
                <ItemTemplate>
                    <table class="TABLE_HD" width="100%">
                        <tr style="height: 20px">
                            <td valign="top">
                                <asp:CheckBox ID="chk_Item" runat="server" Width="10px" />
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <%--<EditItemTemplate>
                    <table border="0" cellpadding="1" cellspacing="0">
                        <tr>
                            <td valign="middle">
                                <asp:CheckBox ID="chk_Item_Edit" runat="server" Width="10px" onclick="Check(this)"
                                    Visible="false" />
                            </td>
                        </tr>
                    </table>
                </EditItemTemplate>--%>
                <HeaderStyle HorizontalAlign="Center" Width="3%" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="3%" />
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderStyle Width="4%" HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="4%" />
                <ItemTemplate>
                    <table class="TABLE_HD" width="100%" cellpadding="0" cellspacing="0">
                        <tr style="height: 20px">
                            <td valign="middle" align="center">
                                <asp:LinkButton ID="lnkb_Edit" runat="server" CausesValidation="False" CommandName="Edit"
                                    SkinID="LNKB_NORMAL">Edit</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <EditItemTemplate>
                    <table border="0" cellpadding="2" cellspacing="0">
                        <tr style="height: 20px">
                            <td valign="middle">
                                <asp:LinkButton ID="lnkb_Update" runat="server" CommandName="Update" SkinID="LNKB_NORMAL">Update</asp:LinkButton>
                            </td>
                            <td valign="middle">
                                <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NR" Text="|"></asp:Label>
                            </td>
                            <td valign="middle">
                                <asp:LinkButton ID="lnkb_Cancel" runat="server" CausesValidation="false" CommandName="Cancel"
                                    SkinID="LNKB_NORMAL">Cancel</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </EditItemTemplate>
            </asp:TemplateField>
            <%--<asp:CommandField ShowEditButton="true" HeaderText="#">
                <HeaderStyle BorderStyle="None" Width="80px" HorizontalAlign="Left" />
                <ItemStyle BorderStyle="None" Width="80px" />
            </asp:CommandField>--%>
            <asp:TemplateField>
                <HeaderStyle Width="92%" />
                <ItemStyle Width="92%" />
                <HeaderTemplate>
                    <asp:Panel ID="p_HeaderSO" runat="server">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <%--<td width="9%" align="left">
                                    <asp:Label ID="Label19" runat="server" Text="Store" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td width="18%" align="left">
                                    <asp:Label ID="Label20" runat="server" Text="Name" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td width="8%" align="left">
                                    <asp:Label ID="Label21" runat="server" Text="SKU #" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td width="26%" align="left">
                                    <asp:Label ID="Label22" runat="server" Text="English Name" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td width="26%" align="left">
                                    <asp:Label ID="Label23" runat="server" Text="Local Name" SkinID="LBL_HD_W"></asp:Label>
                                </td>--%>
                                <td align="left" style="width: 20%">
                                    <asp:Label ID="Label32" runat="server" Text="Store Name" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td align="left" style="width: 50%">
                                    <asp:Label ID="Label25" runat="server" Text="Item Description" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td align="left" style="width: 15%">
                                    <asp:Label ID="Label28" runat="server" Text="Unit" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td align="right" style="width: 15%">
                                    <asp:Label ID="Label29" runat="server" Text="Qty" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="p_HeaderSI" runat="server">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <%--<td width="8%" align="left">
                                    <asp:Label ID="Label1" runat="server" Text="Store" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td width="17%" align="left">
                                    <asp:Label ID="Label6" runat="server" Text="Name" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td width="6%" align="left">
                                    <asp:Label ID="Label7" runat="server" Text="SKU #" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td width="20%" align="left">
                                    <asp:Label ID="Label8" runat="server" Text="English Name" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td width="20%" align="left">
                                    <asp:Label ID="Label9" runat="server" Text="Local Name" SkinID="LBL_HD_W"></asp:Label>
                                </td>--%>
                                <td align="left" style="width: 20%">
                                    <asp:Label ID="Label30" runat="server" Text="Store Name" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td width="40%" align="left">
                                    <asp:Label ID="Label1" runat="server" Text="Item Description" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td width="10%" align="left">
                                    <asp:Label ID="Label11" runat="server" Text="Unit" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td width="10%" align="right">
                                    <asp:Label ID="Label12" runat="server" Text="Qty" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td width="18%" align="right">
                                    <asp:Label ID="Label26" runat="server" Text="Unit Cost" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="p_HeaderTIO" runat="server">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td align="left" style="width: 8%">
                                    <asp:Label ID="Label38" runat="server" Text="SKU#" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td align="left" style="width: 35%">
                                    <asp:Label ID="Label39" runat="server" Text="English Name" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td align="left" style="width: 30%">
                                    <asp:Label ID="Label40" runat="server" Text="Local Name" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td align="left" style="width: 7%">
                                    <asp:Label ID="Label41" runat="server" Text="Unit" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td align="right" style="width: 10%">
                                    <%--<asp:Label ID="Label42" runat="server" Text="Qty Allocate" SkinID="LBL_HD_W"></asp:Label>--%>
                                    <asp:Label ID="lbl_QtyTranAllo" runat="server" Text="Qty Allocate" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td align="right" style="width: 10%">
                                    <%--<asp:Label ID="Label18" runat="server" Text="Qty Tr/Out" SkinID="LBL_HD_W"></asp:Label>--%>
                                    <asp:Label ID="lbl_QtyTrIO" runat="server" Text="Qty Tr/Out" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <%--<asp:Panel ID="p_HeaderTI" runat="server">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="background-color: #999999;
                            color: #FFFFFF;">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label14" runat="server" Text="SKU#" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="Label15" runat="server" Text="English Name" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="Label16" runat="server" Text="Local Name" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:Label ID="Label17" runat="server" Text="Unit" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td align="right">
                                    <asp:Label ID="Label29" runat="server" Text="Qty Transfer" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td align="right">
                                    <asp:Label ID="Label37" runat="server" Text="Qty Tr/In" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>--%>
                </HeaderTemplate>
                <ItemTemplate>
                    <%--<asp:Panel ID="p_ItemSO" runat="server">
                        <table border="0" cellpadding="0" cellspacing="3" width="100%">
                            <tr>
                                <td width="8%" align="left">
                                    <asp:Label ID="lbl_LocationCode_SO" runat="server" Width="100%" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td width="20%" align="left">
                                    <asp:Label ID="lbl_LocationName_SO" runat="server" Width="100%" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td width="8%" align="left">
                                    <asp:Label ID="lbl_ProductCode_SO" runat="server" Width="100%" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td width="27%" align="left">
                                    <asp:Label ID="lbl_EnglishName_SO" runat="server" Width="100%" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td width="27%" align="left">
                                    <asp:Label ID="lbl_LocalName_SO" runat="server" Width="100%" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td width="5%" align="left">
                                    <asp:Label ID="lbl_Unit_SO" runat="server" Width="100%" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td width="5%" align="right">
                                    <asp:Label ID="lbl_Qty_SO" runat="server" Width="100%" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>--%>
                    <asp:Panel ID="p_ItemSO" runat="server">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <%--<td align="left" style="width: 9%">
                                    <asp:Label ID="lbl_LocationCode_SO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td align="left" style="width: 18%">
                                    <asp:Label ID="lbl_LocationName_SO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td align="left" style="width: 8%">
                                    <asp:Label ID="lbl_ProductCode_SO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td align="left" style="width: 26%">
                                    <asp:Label ID="lbl_EnglishName_SO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td align="left" style="width: 26%">
                                    <asp:Label ID="lbl_LocalName_SO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>--%>
                                <td align="left" style="width: 20%">
                                    <asp:Label ID="lbl_StoreName_SO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td align="left" style="width: 50%">
                                    <asp:Label ID="lbl_ItemDesc_SO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td align="left" style="width: 15%">
                                    <asp:Label ID="lbl_Unit_SO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td align="right" style="width: 15%">
                                    <asp:Label ID="lbl_Qty_SO" runat="server" SkinID="LBL_NR_GRD" Height="17px"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="p_ItemSI" runat="server">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <%--<td width="8%" align="left">
                                    <asp:Label ID="lbl_LocationCode_SI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td width="17%" align="left">
                                    <asp:Label ID="lbl_LocationName_SI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td width="6%" align="left">
                                    <asp:Label ID="lbl_ProductCode_SI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td width="20%" align="left">
                                    <asp:Label ID="lbl_EnglishName_SI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td width="20%" align="left">
                                    <asp:Label ID="lbl_LocalName_SI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>--%>
                                <td align="left" style="width: 20%">
                                    <asp:Label ID="lbl_StoreName_SI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td align="left" style="width: 40%">
                                    <asp:Label ID="lbl_ItemDesc_SI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td width="10%" align="left">
                                    <asp:Label ID="lbl_Unit_SI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td width="10%" align="right">
                                    <asp:Label ID="lbl_Qty_SI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td width="18%" align="right">
                                    <asp:Label ID="lbl_UnitCost_SI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="p_ItemTIO" runat="server">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td width="8%" align="left">
                                    <asp:Label ID="lbl_ProductCode_TI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td width="35%" align="left">
                                    <asp:Label ID="lbl_EnglishName_TI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td width="30%" align="left">
                                    <asp:Label ID="lbl_LocalName_TI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td width="7%" align="left">
                                    <asp:Label ID="lbl_Unit_TI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td width="10%" align="right">
                                    <asp:Label ID="lbl_QtyTransfer_TI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                    <asp:Label ID="lbl_QtyAllocated_TO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td width="10%" align="right" style="padding-right: 5px">
                                    <asp:Label ID="lbl_QtyTrIn_TI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                    <asp:Label ID="lbl_QtyTrOut_TO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <%-- <asp:Panel ID="p_ItemTO" runat="server">
                        <table border="0" cellpadding="0" cellspacing="1" width="100%">
                            <tr>
                                <td width="50px" align="left">
                                    <asp:Label ID="lbl_ProductCode_TO" runat="server" Width="50px" SkinID="LBL_NR"></asp:Label>
                                </td>
                                <td width="200px" align="left">
                                    <asp:Label ID="lbl_EnglishName_TO" runat="server" Width="200px" SkinID="LBL_NR"></asp:Label>
                                </td>
                                <td width="200px" align="left">
                                    <asp:Label ID="lbl_LocalName_TO" runat="server" Width="200px" SkinID="LBL_NR"></asp:Label>
                                </td>
                                <td width="35px" align="left">
                                    <asp:Label ID="lbl_Unit_TO" runat="server" Width="35px" SkinID="LBL_NR"></asp:Label>
                                </td>
                                <td width="75px" align="right">
                                    <asp:Label ID="lbl_QtyAllocated_TO" runat="server" Width="75px" SkinID="LBL_NR"></asp:Label>
                                </td>
                                <td width="60px" align="right" style="padding-right: 5px">
                                    <asp:Label ID="lbl_QtyTrOut_TO" runat="server" Width="60px" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>--%>
                </ItemTemplate>
                <EditItemTemplate>
                    <dx:ASPxPageControl ID="tp_Information" runat="server" ActiveTabIndex="0" Width="100%"
                        BackColor="WhiteSmoke" Font-Bold="True">
                        <ContentStyle BackColor="WhiteSmoke">
                        </ContentStyle>
                        <ActiveTabStyle BackColor="WhiteSmoke">
                        </ActiveTabStyle>
                        <TabPages>
                            <dx:TabPage Text="Information" Name="Info">
                                <ContentCollection>
                                    <dx:ContentControl ID="ContentControl1" runat="server">
                                        <asp:Panel ID="p_SIO" runat="server">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%" style="padding-left: 10px">
                                                <tr>
                                                    <td style="padding-top: 10px;" align="left" width="50%">
                                                        <asp:Label ID="Label28" runat="server" Text="Store:" SkinID="LBL_HD"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-bottom: 5px; height: 26px;" width="50%">
                                                        <dx:ASPxComboBox ID="ddl_Store" runat="server" Width="90%" TextFormatString="{0} : {1}"
                                                            ValueType="System.String" AutoPostBack="True" OnLoad="ddl_Store_Load" OnSelectedIndexChanged="ddl_Store_SelectedIndexChanged"
                                                            ValueField="LocationCode">
                                                            <Columns>
                                                                <dx:ListBoxColumn Caption="Code" FieldName="LocationCode" />
                                                                <dx:ListBoxColumn Caption="Name" FieldName="LocationName" />
                                                            </Columns>
                                                        </dx:ASPxComboBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%" align="left">
                                                        <asp:Label ID="Label30" runat="server" Text="SKU #:" SkinID="LBL_HD"></asp:Label>
                                                    </td>
                                                    <td style="width: 10%">
                                                        <asp:Label ID="Label31" runat="server" Text="Unit:" SkinID="LBL_HD"></asp:Label>
                                                    </td>
                                                    <td style="width: 20%" align="left">
                                                        <asp:Label ID="Label32" runat="server" Text="Qty:" SkinID="LBL_HD"></asp:Label>
                                                    </td>
                                                    <td width="20%">
                                                        <asp:Label ID="lbl_UnitCost" runat="server" Text="Unit Cost:" SkinID="LBL_HD"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-bottom: 5px; width: 50%;">
                                                        <dx:ASPxComboBox ID="ddl_Product" runat="server" AutoPostBack="True" TextFormatString="{0} : {1} : {2}"
                                                            ValueType="System.String" Width="90%" ValueField="ProductCode" OnLoad="ddl_Product_Load"
                                                            OnSelectedIndexChanged="ddl_Product_SelectedIndexChanged">
                                                            <Columns>
                                                                <dx:ListBoxColumn Caption="Code" FieldName="ProductCode" />
                                                                <dx:ListBoxColumn Caption="Name" FieldName="ProductDesc1" />
                                                                <dx:ListBoxColumn Caption="Desc" FieldName="ProductDesc2" />
                                                            </Columns>
                                                        </dx:ASPxComboBox>
                                                    </td>
                                                    <td style="padding-bottom: 5px; width: 10%;">
                                                        <asp:Label ID="lbl_Unit" runat="server" SkinID="LBL_HD"></asp:Label>
                                                    </td>
                                                    <td style="padding-bottom: 5px; width: 20%;">
                                                        <asp:TextBox ID="txt_Qty" runat="server" Width="90%" SkinID="TXT_NUM_V1">
                                                        </asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                                            TargetControlID="txt_Qty" ValidChars="0123456789.">
                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                    </td>
                                                    <td style="padding-bottom: 5px; padding-right: 10px; width: 20%;">
                                                        <asp:TextBox ID="txt_UnitCost" runat="server" Width="90%" SkinID="TXT_NUM_V1">
                                                        </asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                                            TargetControlID="txt_UnitCost" ValidChars="0123456789.">
                                                        </ajaxToolkit:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="1">
                                                        <asp:Label ID="Label33" runat="server" Text="Debit A/C:" SkinID="LBL_HD"></asp:Label>
                                                    </td>
                                                    <td align="left" width="50%" colspan="2">
                                                        <asp:Label ID="Label34" runat="server" Text="Credit A/C:" SkinID="LBL_HD"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-bottom: 5px;" colspan="1">
                                                        <dx:ASPxComboBox ID="ddl_Debit" runat="server" Width="90%">
                                                        </dx:ASPxComboBox>
                                                    </td>
                                                    <td style="padding-bottom: 5px;" width="50%" colspan="2">
                                                        <dx:ASPxComboBox ID="ddl_Credit" runat="server" Width="90%">
                                                        </dx:ASPxComboBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-bottom: 4px;" colspan="3" align="left">
                                                        <asp:Label ID="Label35" runat="server" Text="Comment:" SkinID="LBL_HD"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-bottom: 10px;" colspan="3">
                                                        <asp:TextBox ID="txt_Comment" runat="server" Width="100%" TextMode="MultiLine" SkinID="TXT_V1">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel ID="p_TIO" runat="server">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%" style="padding-left: 10px">
                                                <tr>
                                                    <td style="padding-top: 10px;" align="left" width="50%">
                                                        <asp:Label ID="Label14" runat="server" Text="SKU #:" SkinID="LBL_HD"></asp:Label>
                                                    </td>
                                                    <td style="width: 30%; padding-top: 10px;" align="left">
                                                        <asp:Label ID="lbl_QtyInOut" runat="server" SkinID="LBL_HD"></asp:Label>
                                                    </td>
                                                    <td width="20%">
                                                        <asp:Label ID="Label19" runat="server" Text="Unit:" SkinID="LBL_HD"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-bottom: 5px; height: 26px;" width="50%">
                                                        <dx:ASPxComboBox ID="ddl_ProductCode" runat="server" Width="95%" OnLoad="ddl_ProductCode_Load"
                                                            OnSelectedIndexChanged="ddl_ProductCode_SelectedIndexChanged" TextFormatString="{0} : {1} : {2}"
                                                            AutoPostBack="True" IncrementalFilteringMode="Contains">
                                                            <Columns>
                                                                <dx:ListBoxColumn Caption="Code" FieldName="ProductCode" />
                                                                <dx:ListBoxColumn Caption="Name" FieldName="ProductDesc1" />
                                                                <dx:ListBoxColumn Caption="Description" FieldName="ProductDesc2" />
                                                            </Columns>
                                                        </dx:ASPxComboBox>
                                                        <asp:Label ID="lbl_ProductCode" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                                    </td>
                                                    <td style="padding-bottom: 5px; width: 30%; height: 26px;">
                                                        <asp:TextBox ID="txt_QtyTrInOut" runat="server" MaxLength="29" SkinID="TXT_NUM_V1">
                                                        </asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server"
                                                        TargetControlID="txt_QtyTrInOut" ValidChars="0123456789.">
                                                    </ajaxToolkit:FilteredTextBoxExtender>
                                                    </td>
                                                    <td width="20%">
                                                        <asp:Label ID="lbl_Unit_TIO" runat="server" SkinID="LBL_HD"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 25%" align="left">
                                                        <asp:Label ID="Label16" runat="server" Text="Debit A/C:" SkinID="LBL_HD"></asp:Label>
                                                    </td>
                                                    <td style="width: 10%" colspan="2">
                                                        <asp:Label ID="Label17" runat="server" Text="Credit A/C:" SkinID="LBL_HD"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-bottom: 5px; width: 50%;">
                                                        <dx:ASPxComboBox ID="ddl_Debit_TIO" runat="server" Width="95%">
                                                        </dx:ASPxComboBox>
                                                    </td>
                                                    <td style="padding-bottom: 5px; width: 10%;" colspan="2">
                                                        <dx:ASPxComboBox ID="ddl_Credit_TIO" runat="server" Width="95%">
                                                        </dx:ASPxComboBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-bottom: 4px;" colspan="4" align="left">
                                                        <asp:Label ID="Label291" runat="server" Text="Comment:" SkinID="LBL_HD"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-bottom: 10px;" colspan="4">
                                                        <asp:TextBox ID="txt_Comment_TIO" runat="server" Width="100%" TextMode="MultiLine"
                                                            SkinID="TXT_V1"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </dx:ContentControl>
                                </ContentCollection>
                            </dx:TabPage>
                        </TabPages>
                        <LoadingPanelStyle BackColor="WhiteSmoke">
                        </LoadingPanelStyle>
                    </dx:ASPxPageControl>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:Panel ID="p_FooterSO" runat="server">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td width="88%">
                                </td>
                                <td width="7%" align="left">
                                    <asp:Label ID="Label13" runat="server" Text="Total Qty:" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td width="5%" align="right">
                                    <asp:Label ID="lbl_SOTotalQty" runat="server" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="p_FooterSI" runat="server">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td align="left" style="width: 20%">
                                    <asp:Label ID="lbl_StoreName_SI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td align="left" style="width: 40%">
                                    <asp:Label ID="lbl_ItemDesc_SI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td align="left" style="width: 10%">
                                    <asp:Label ID="Label7" runat="server" Text="Total Qty:" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td align="right" style="width: 10%">
                                    <asp:Label ID="lbl_SITotalQty" runat="server" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td align="right" style="width: 18%">
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="p_FooterTIO" runat="server">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td width="73%">
                                </td>
                                <td width="7%" align="left">
                                    <asp:Label ID="Label37" runat="server" Text="Total Qty:" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td width="10%" align="right">
                                    <asp:Label ID="lbl_TrfTotalQty" runat="server" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td width="10%">
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </FooterTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <dx:ASPxPopupControl ID="pop_Warning" ClientInstanceName="pop_Warning" runat="server"
        CloseAction="CloseButton" HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Width="300px">
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
                            <asp:Button ID="btn_Warning" SkinID="BTN_V1" runat="server" Text="OK" Width="60px"
                                OnClick="btn_Warning_Click" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_ConfirmSave" ClientInstanceName="pop_ConfirmSave" runat="server"
        CloseAction="CloseButton" HeaderText="Confirm" Modal="True" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Width="300px">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" colspan="2" height="50px">
                            <asp:Label ID="Label36" runat="server" Text="Confirm to Save?" SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Button ID="btn_Confirm_Yes" runat="server" SkinID="BTN_V1" Text="Yes" Width="60px"
                                OnClick="btn_Confirm_Yes_Click" />
                        </td>
                        <td align="left">
                            <asp:Button ID="btn_Confirm_No" runat="server" SkinID="BTN_V1" Text="No" Width="60px"
                                OnClick="btn_Confirm_No_Click" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_ConfrimDelete" ClientInstanceName="pop_ConfrimDelete"
        runat="server" CloseAction="CloseButton" HeaderText="Confirm" Modal="True" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Width="300px">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" colspan="2" height="50px">
                            <asp:Label ID="Label18" runat="server" Text="Confirm to delete the selected record"
                                SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Button ID="btn_ConfirmDelete_Yes" runat="server" SkinID="BTN_V1" Text="Yes"
                                Width="60px" OnClick="btn_ConfirmDelete_Yes_Click" />
                        </td>
                        <td align="left">
                            <asp:Button ID="btn_ConfirmDelete_No" runat="server" SkinID="BTN_V1" Text="No" Width="60px"
                                OnClick="btn_ConfirmDelete_No_Click" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
