<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ListPage2.ascx.cs" Inherits="BlueLedger.PL.UserControls.ViewHandler.ListPage2" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<style type="text/css">
	table th,td
	{
		padding-right: 5px;
		padding-left: 5px;
		padding-top: 3px;
		padding-bottom: 2px;		
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
    
        .display-block
        {
            display: block;
        }
        .display-inline-block
        {
            display: inline-block;
        }
    }
</style>
<script type="text/javascript">

    var tmpBackgroundColor;
    var tmpColor;

    function OnGridRowMouseOver(rowObj) {
        tmpBackgroundColor = rowObj.style.backgroundColor;
        tmpColor = rowObj.style.color;

        rowObj.style.backgroundColor = "#4d4d4d";
        rowObj.style.color = "#ffffff";
        rowObj.style.cursor = "pointer";
    }

    function OnGridRowMouseOut(rowObj) {
        rowObj.style.backgroundColor = tmpBackgroundColor;
        rowObj.style.color = tmpColor;
        rowObj.style.cursor = "pointer";
    }

    function OnGridRowClick(buCode, id, vid) {
        window.location = '<%=DetailPageURL%>?BuCode=' + buCode + '&ID=' + id + '&VID=' + vid;
    }

    function OnGridDoubleClick(index, keyFieldName) {
        listPageGrid.GetRowValues(index, keyFieldName, OnGetRowValues);
    }

    function OnGetRowValues(values) {
        window.location = '<%=DetailPageURL%>?BuCode=' + values[0] + '&ID=' + values[1] + '&VID=' + ddl_View.GetValue();
    }


    function MenuItemClick(e) {
        var i = e.item.name;
        if (i == "Print")
            window.print();
    }
</script>
<div style="width: 100%; margin: auto;">
    <table style="width: 100%; border-spacing: 0; margin: auto">
        <tr>
            <td style="padding: 0px 0px 0px 10px; background-color: #4D4D4D; height: 20px;">
                <table border="0" cellpadding="2" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" EnableViewState="False" />
                        </td>
                        <td>
                            <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_HD_WHITE" EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="padding: 0px 10px 0px 0px; background-color: #4D4D4D;" align="right">
                <dx:ASPxMenu runat="server" ID="menu_CmdBar" Font-Bold="true" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle">
                    <ClientSideEvents ItemClick="function(s, e) { MenuItemClick(e); }" />
                    <ItemStyle BackColor="Transparent">
                        <HoverStyle BackColor="Transparent">
                            <Border BorderStyle="None" />
                        </HoverStyle>
                        <Paddings Padding="2px" />
                        <Border BorderStyle="None" />
                    </ItemStyle>
                    <Paddings Padding="0px" />
                    <SeparatorPaddings Padding="0px" />
                    <SubMenuItemStyle Font-Size="1em" ForeColor="Black" Height="34px">
                        <HoverStyle BackColor="#20B9EB" ForeColor="White">
                            <Border BorderStyle="None" />
                        </HoverStyle>
                        <Paddings PaddingLeft="10px" />
                    </SubMenuItemStyle>
                    <SubMenuStyle BackColor="WhiteSmoke" GutterWidth="0px">
                        <Border BorderStyle="None" />
                    </SubMenuStyle>
                    <Items>
                        <dx:MenuItem Name="Create" Text="">
                            <ItemStyle Height="16px" Width="49px">
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/create.png" Repeat="NoRepeat" VerticalPosition="center" />
                            </ItemStyle>
                        </dx:MenuItem>
                        <dx:MenuItem Name="Delete" Text="">
                            <ItemStyle Height="16px" Width="47px">
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                            </ItemStyle>
                        </dx:MenuItem>
                        <dx:MenuItem Name="Void" Text="">
                            <ItemStyle Height="16px" Width="41px">
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-void.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/void.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                            </ItemStyle>
                        </dx:MenuItem>
                        <dx:MenuItem Name="ClosePO" Text="">
                            <ItemStyle Height="16px" Width="57px">
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-closepo.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/closepo.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                            </ItemStyle>
                        </dx:MenuItem>
                        <dx:MenuItem Name="Export" Text="Export...">
                            <ItemStyle Height="16px" Width="50px" Font-Size="Smaller" ForeColor="White" ImageSpacing="0">
                                <%--<HoverStyle>
                                   <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/save.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />--%>
                            </ItemStyle>
                        </dx:MenuItem>
                        <dx:MenuItem Name="Print" Text="">
                            <ItemStyle Height="16px" Width="43px">
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-print.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                            </ItemStyle>
                        </dx:MenuItem>
                        <dx:MenuItem Name="Favorites" Visible="False" Text="">
                            <ItemStyle Height="26px" Width="29px">
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/favorites.gif" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                            </ItemStyle>
                        </dx:MenuItem>
                    </Items>
                </dx:ASPxMenu>
            </td>
        </tr>
    </table>
    <div class="printable">
        <table style="margin: auto; width: 100%">
            <tr valign="top">
                <td style="padding: 0px 0px 0px 10px;">
                    <table border="0" cellpadding="2" cellspacing="0">
                        <tr valign="middle" style="height: 25px">
                            <td>
                                <asp:Label ID="lbl_View" runat="server" Text="<%$ Resources:UserControl_ViewHandler_ListPage2, lbl_View %>" SkinID="LBL_HD" EnableViewState="False"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_View2" runat="server" AutoPostBack="True" DataTextField="Desc" DataValueField="ViewNo" OnSelectedIndexChanged="ddl_View2_SelectedIndexChanged"
                                    SkinID="DDL_V1">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btn_ViewGo2" runat="server" OnClick="btn_ViewGo2_Click" Text="<%$ Resources:UserControl_ViewHandler_ListPage2, btn_ViewGo2 %>" SkinID="BTNView_V1"
                                    EnableViewState="False" />
                            </td>
                            <td>
                                |
                            </td>
                            <td>
                                <asp:Button ID="btn_ViewModify2" runat="server" OnClick="btn_ViewModify2_Click" Text="<%$ Resources:UserControl_ViewHandler_ListPage2, btn_ViewModify2 %>"
                                    SkinID="BTNView_V1" EnableViewState="False" />
                            </td>
                            <td>
                                |
                            </td>
                            <td>
                                <asp:Button ID="btn_ViewCreate2" runat="server" OnClick="btn_ViewCreate2_Click" Text="<%$ Resources:UserControl_ViewHandler_ListPage2, btn_ViewCreate2 %>"
                                    SkinID="BTNView_V1" EnableViewState="False" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td align="right">
                    <asp:Panel ID="p_SearchBasic" runat="server" EnableViewState="False">
                        <table border="0" cellpadding="1" cellspacing="0" width="20%">
                            <tr valign="middle">
                                <td align="center">
                                    <asp:TextBox ID="txt_FullTextSearch" runat="server" SkinID="TXT_V1" Height="12px" Width="330px" AutoPostBack="True"></asp:TextBox>
                                </td>
                                <td style="width: 16px">
                                    <asp:ImageButton ID="btn_Search" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/search.png" ToolTip="Search" OnClick="btn_Search_Click"
                                        EnableViewState="False" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <dx:ASPxPanel ID="pnl_WFLegend" runat="server" EnableViewState="False">
            <Paddings Padding="0px" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent2" runat="server" EnableViewState="False">
                    <table style="margin: auto; width: 100%; font-size: 7pt;">
                        <tr style="height: 25px">
                            <td>
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_ProcessStatusHdr" runat="server" Text="<%$ Resources:UserControl_ViewHandler_ListPage2, lbl_ProcessStatusHdr %>" SkinID="LBL_NR_BI"
                                                EnableViewState="False"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DataList ID="dl_ProcessStatus" runat="server" RepeatDirection="Horizontal" Font-Names=" Arial" Font-Size="8pt" EnableViewState="False">
                                                <ItemStyle Font-Italic="True" Font-Names="Arial" Font-Size="7pt" />
                                                <ItemTemplate>
                                                    <%#DataBinder.Eval(Container.DataItem,"Step").ToString()%>.<%#DataBinder.Eval(Container.DataItem,"StepDesc").ToString()%>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="right">
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <tr style="height: 35px">
                                        <td>
                                            <asp:Image ID="img_Pending" runat="server" ImageUrl="~/App_Themes/Default/Images/WF/NA.gif" EnableViewState="False" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_Pending" runat="server" Text="<%$ Resources:UserControl_ViewHandler_ListPage2, lbl_Pending %>" SkinID="LBL_NR_I" EnableViewState="False"></asp:Label>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:Image ID="img_Approve" runat="server" ImageUrl="~/App_Themes/Default/Images/WF/APP.gif" EnableViewState="False" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_Approve" runat="server" Text="<%$ Resources:UserControl_ViewHandler_ListPage2, lbl_Approve %>" SkinID="LBL_NR_I" EnableViewState="False"></asp:Label>&nbsp;
                                        </td>
                                        <td>
                                            <asp:Image ID="img_PartialApprove" runat="server" ImageUrl="~/App_Themes/Default/Images/WF/PAR.gif" EnableViewState="False" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_PartialApprove" runat="server" Text="<%$ Resources:UserControl_ViewHandler_ListPage2, lbl_PartialApprove %>" SkinID="LBL_NR_I" EnableViewState="False"></asp:Label>&nbsp;
                                        </td>
                                        <td>
                                            <asp:Image ID="img_Reject" runat="server" ImageUrl="~/App_Themes/Default/Images/WF/REJ.gif" EnableViewState="False" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_Reject" runat="server" Text="<%$ Resources:UserControl_ViewHandler_ListPage2, lbl_Reject %>" SkinID="LBL_NR_I" EnableViewState="False"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxPanel>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" EnableViewState="false">
            <ContentTemplate>
                <asp:GridView ID="grd_BU" runat="server" AutoGenerateColumns="False" Width="100%" SkinID="GRD_V1" ShowHeader="False" ShowFooter="false" BorderStyle="Solid"
                    BorderWidth="1px" BorderColor="#999999" GridLines="Horizontal" CellPadding="3" CellSpacing="0" EnableViewState="False" OnRowDataBound="grd_BU_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <div>
                                    <asp:ImageButton ID="btn_Expand" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/Plus.jpg" OnClick="btn_Expand_Click" />
                                </div>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="25px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <div style="padding-bottom: 5px;">
                                    <asp:Label ID="lbl_BU" runat="server" Font-Bold="true" />
                                    <asp:Label ID="lbl_Message" runat="server" Visible="False" ForeColor="Red" EnableViewState="False">[This page found items more than 1000 items please use search for find another]</asp:Label>
                                    <asp:HiddenField ID="hf_BuCode" runat="server" />
                                </div>
                                <div>
                                    <asp:GridView ID="grd_Trans" runat="server" SkinID="GRD_V1" Width="100%" AutoGenerateColumns="False" AllowPaging="True" PageSize="25" ShowFooter="False"
                                        AllowSorting="False" OnRowDataBound="grd_Trans_RowDataBound" OnSorting="grd_Trans_Sorting" OnRowCreated="grd_Trans_RowCreated" OnPageIndexChanging="grd_Trans_PageIndexChanging">
                                        <PagerSettings Mode="NumericFirstLast" />
                                        <HeaderStyle ForeColor="White" Height="20px" HorizontalAlign="Left" />
                                    </asp:GridView>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress2" PopupControlID="UpdateProgress2" BackgroundCssClass="POPUP_BG"
        RepositionMode="RepositionOnWindowResizeAndScroll">
    </ajaxToolkit:ModalPopupExtender>
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
        <ProgressTemplate>
            <div style="border-style: solid; border-width: 1px; border-color: #0071BD; background-color: #FFFFFF; width: 120px; height: 60px">
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
</div>
