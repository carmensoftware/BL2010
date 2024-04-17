<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ListPage2.ascx.cs" Inherits="BlueLedger.PL.UserControls.ViewHandler.ListPage2" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<style>
    .display-flex
    {
        display: flex !important;
    }
    .space
    {
        margin-right: 5px;
    }
    
    .justify-content-start
    {
        justify-content: flex-start !important;
    }
    .justify-content-end
    {
        justify-content: flex-end !important;
    }
    .justify-content-center
    {
        justify-content: center !important;
    }
    .justify-content-between
    {
        justify-content: space-between !important;
    }
</style>
<style type="text/css">
    table th, td
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
<div class="display-flex justify-content-between" style="background-color: #4D4D4D; width: 100%; margin-bottom: 5px;">
    <div class="display-flex">
        <div style="margin: 2px;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" EnableViewState="False" />
        </div>
        <div style="margin-top: 4px;">
            <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_HD_WHITE" EnableViewState="False"></asp:Label>
        </div>
    </div>
    <div>
        <dx:ASPxMenu runat="server" ID="menu_CmdBar" Font-Bold="true" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle">
            <ClientSideEvents ItemClick="function(s, e) { MenuItemClick(e); }" />
            <ItemStyle BackColor="Transparent">
                <HoverStyle BackColor="Transparent" ForeColor="White">
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
                    <ItemStyle Height="16px" Width="50px" Font-Size="Smaller" ForeColor="White" ImageSpacing="0"></ItemStyle>
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
    </div>
</div>
<div class="printable">
    <!-- View and Search -->
    <div class="display-flex justify-content-between" style="margin-bottom: 5px;">
        <div class="display-flex" style="margin-bottom: 5px; width: 100%">
            <div class="space">
                <asp:Label ID="lbl_View" runat="server" Text="<%$ Resources:UserControl_ViewHandler_ListPage2, lbl_View %>" SkinID="LBL_HD" EnableViewState="False"></asp:Label>
            </div>
            <div class="space">
                <asp:DropDownList ID="ddl_View2" runat="server" SkinID="DDL_V1" AutoPostBack="True" DataTextField="Desc" DataValueField="ViewNo" OnSelectedIndexChanged="ddl_View2_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
            <asp:Button ID="btn_ViewGo2" runat="server" EnableViewState="False" SkinID="BTNView_V1" Text="<%$ Resources:UserControl_ViewHandler_ListPage2, btn_ViewGo2 %>"
                OnClick="btn_ViewGo2_Click" />
            <div style="display: none">
                <asp:Button ID="btn_ViewModify2" runat="server" SkinID="BTNView_V1" EnableViewState="False" Text="<%$ Resources:UserControl_ViewHandler_ListPage2, btn_ViewModify2 %>"
                    OnClick="btn_ViewModify2_Click" />
                <asp:Button ID="btn_ViewCreate2" runat="server" SkinID="BTNView_V1" EnableViewState="False" Text="<%$ Resources:UserControl_ViewHandler_ListPage2, btn_ViewCreate2 %>"
                    OnClick="btn_ViewCreate2_Click" />
            </div>
        </div>
        <div class="display-flex justify-content-end">
            <div class="space">
                <asp:TextBox ID="txt_Search" runat="server" SkinID="TXT_V1" AutoPostBack="True" Width="200"></asp:TextBox>
            </div>
            <div>
                <asp:ImageButton ID="btn_Search" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/search.png" ToolTip="Search" OnClick="btn_Search_Click" />
            </div>
        </div>
    </div>
    <!-- Workflow -->
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
    <!-- Data list -->
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" EnableViewState="false" UpdateMode="Always">
        <ContentTemplate>
            <asp:GridView ID="gv_Data" runat="server" SkinID="GRD_V1" Width="100%" AutoGenerateColumns="False" OnRowCreated="gv_Data_RowCreated" OnRowDataBound="gv_Data_RowDataBound">
                <HeaderStyle ForeColor="White" Font-Size="Small" HorizontalAlign="Left" />
                <RowStyle Font-Size="Small" />
                <Columns>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
    <br />
    <div class="display-flex">
        <asp:Button runat="server" ID="btn_Page_Previous" Style="cursor: pointer;" Text="<<" OnClick="btn_Page_Click" />&nbsp;
        <asp:Button runat="server" ID="btn_P1" Style="cursor: pointer;" Text="1" OnClick="btn_Page_Click" />&nbsp;
        <asp:Button runat="server" ID="btn_P2" Style="cursor: pointer;" Text="2" OnClick="btn_Page_Click" />&nbsp;
        <asp:Button runat="server" ID="btn_P3" Style="cursor: pointer;" Text="3" OnClick="btn_Page_Click" />&nbsp;
        <asp:Button runat="server" ID="btn_P4" Style="cursor: pointer;" Text="4" OnClick="btn_Page_Click" />&nbsp;
        <asp:Button runat="server" ID="btn_P5" Style="cursor: pointer;" Text="5" OnClick="btn_Page_Click" />&nbsp;
        <asp:Button runat="server" ID="btn_P6" Style="cursor: pointer;" Text="6" OnClick="btn_Page_Click" />&nbsp;
        <asp:Button runat="server" ID="btn_P7" Style="cursor: pointer;" Text="7" OnClick="btn_Page_Click" />&nbsp;
        <asp:Button runat="server" ID="btn_P8" Style="cursor: pointer;" Text="8" OnClick="btn_Page_Click" />&nbsp;
        <asp:Button runat="server" ID="btn_P9" Style="cursor: pointer;" Text="9" OnClick="btn_Page_Click" />&nbsp;
        <asp:Button runat="server" ID="btn_P10" Style="cursor: pointer;" Text="10" OnClick="btn_Page_Click" />&nbsp;
        <asp:Button runat="server" ID="btn_Page_Next" Style="cursor: pointer;" Text=">>" OnClick="btn_Page_Click" />&nbsp;
    </div>
</div>
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


    function OnGridRow_Click(buCode, id, vid, page) {
        window.location = '<%=DetailPageURL%>?BuCode=' + buCode + '&ID=' + id + '&VID=' + vid + '&page=' + page;
    }

    function GridCell_Click(buCode, id, vid, page) {
        window.location = '<%=DetailPageURL%>?BuCode=' + buCode + '&ID=' + id + '&VID=' + vid + '&page=' + page;
    }


    function MenuItemClick(e) {
        var i = e.item.name;
        if (i == "Print")
            window.print();
    }
</script>
