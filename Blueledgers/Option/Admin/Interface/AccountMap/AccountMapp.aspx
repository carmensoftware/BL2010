<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="AccountMapp.aspx.cs" Inherits="BlueLedger.PL.Option.Admin.Interface.AccountMap.AccountMapp" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
<style>
     .GridPager a, .GridPager span
    {
        margin: 2px;
        display: block;
        text-decoration: none;
    }
    .GridPager a
    {
        background-color: transparent;
    }
    .GridPager span
    {
        padding: 3px;
        background-color: #fff;
        color: #000;
    }
</style>
    <asp:UpdatePanel ID="updView" runat="server">
        <ContentTemplate>
            <div style="background-color: #4D4D4D; height: auto;">
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left" style="padding-left: 10px;">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                            <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.AccountMapp, lbl_Title %>" SkinID="LBL_HD_WHITE"></asp:Label>
                        </td>
                        <td align="right">
                            <dx:ASPxMenu ID="menu_CmdBar" runat="server" AutoPostBack="true" Font-Size="Small" Font-Bold="True" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="5px"
                                VerticalAlign="Middle" Height="16px" OnItemClick="menu_CmdBar_ItemClick">
                                <ItemStyle BackColor="Transparent">
                                    <HoverStyle BackColor="Transparent">
                                        <Border BorderStyle="None" />
                                    </HoverStyle>
                                    <Paddings Padding="2px" />
                                    <Border BorderStyle="None" />
                                </ItemStyle>
                                <Items>
                                    <dx:MenuItem Name="GetNew" Text="Scan for new code">
                                        <ItemStyle Height="16px" Width="90px" ForeColor="White" Font-Size="0.75em">
                                            <HoverStyle ForeColor="#C4C4C4">
                                            </HoverStyle>
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Import" Text="Import/Export">
                                        <ItemStyle Height="16px" Width="60px" ForeColor="White" Font-Size="0.75em">
                                            <HoverStyle ForeColor="#C4C4C4">
                                            </HoverStyle>
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Edit" Text="">
                                        <%-- 2016-10-05 Add Edit Button --%>
                                        <ItemStyle Height="16px" Width="43px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-edit.png" Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/edit.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Save" Text="" Visible="false">
                                        <ItemStyle Height="16px" Width="42px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png" Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/save.png" Repeat="NoRepeat" VerticalPosition="center" />
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
                                    <dx:MenuItem Name="Back" Text="" Visible="false">
                                        <ItemStyle Height="16px" Width="43px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png" Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                </Items>
                            </dx:ASPxMenu>
                        </td>
                    </tr>
                </table>
            </div>
            <table style="margin-top: 10px; width: 100%;">
                <tr>
                    <td>
                        <asp:Label ID="lbl1" Text="View Name: " runat="server"></asp:Label>
                        <asp:DropDownList ID="ddl_View" runat="server" AutoPostBack="true" Width="200px" OnSelectedIndexChanged="ddl_View_SelectedIndexChanged" />
                        <%--<asp:Button ID="btn_CreateView" runat="server" Text="Create" OnClick="btn_CreateView_Click" />--%>
                        <%--<asp:Button ID="btn_EditView" runat="server" Text="Edit" OnClick="btn_EditView_Click" />--%>
                    </td>
                    <td align="right">
                        <asp:TextBox ID="txt_Search" runat="server" Width="200px" AutoPostBack="true"></asp:TextBox>
                        <asp:Button ID="btn_Search" runat="server" Text="Search" OnClick="btn_Search_Click" />
                    </td>
                </tr>
            </table>
            <hr />
            <div class="printable">
                <asp:GridView ID="gv_AccMap" runat="server" Width="100%" AutoGenerateColumns="true" AllowPaging="True" AllowSorting="true" PageSize="40" GridLines="Horizontal"
                    OnRowDataBound="gv_AccMap_OnRowDataBound" OnPageIndexChanging="gv_AccMap_PageIndexChanging" OnSorting="gv_AccMap_Sorting">
                    <HeaderStyle Height="40px" BackColor="#F4F4F5" Font-Bold="True" ForeColor="#444444" HorizontalAlign="Left" />
                    <%--<RowStyle BackColor="White" BorderColor="#DDDDDD" />--%>
                    <RowStyle Height="50px" BackColor="White" ForeColor="#333333" BorderColor="#DDDDDD" />
                    <PagerStyle BackColor="#4D4D4D" ForeColor="White" HorizontalAlign="Center" BorderColor="#DDDDDD" Font-Size="Medium" CssClass="GridPager" />
                    <Columns>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
        <Triggers>
        <asp:PostBackTrigger ControlID="ddl_View" />
        </Triggers>
    </asp:UpdatePanel>
    <dx:ASPxPopupControl ID="pop_Alert" ClientInstanceName="pop_Alert" runat="server" Width="300px" CloseAction="CloseButton" HeaderText="Warning" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl8" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center">
                            <asp:Label ID="lbl_Pop_Alert" runat="server" SkinID="LBL_NR" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btn_Pop_Alert_Ok" runat="server" Width="50px" SkinID="BTN_V1" Text="OK" OnClientClick="pop_Alert.hide();" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <script type="text/javascript">
//        var ddl_View = document.getElementById("<%=ddl_View.ClientID%>");
//        var txt_Search = document.getElementById("<%=txt_Search.ClientID%>");

//        var id = ddl_View.value;
//        var text = txt_Search.value;

//        ddl_View.addEventListener("change", function () {
//            window.location.href = `AccountMapp.aspx?id=${id}&search=${text}`;
//        });




    </script>
</asp:Content>
