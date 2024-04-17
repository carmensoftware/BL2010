<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="PM.aspx.cs" Inherits="BlueLedger.PL.IN.PM.PM" %>

<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="cph_Main">
    <table border="0" cellpadding="1" cellspacing="0" width="100%">
        <tr style="padding-left: 10px; background-color: #4d4d4d; height: 17px">
            <td align="left">
                <table border="0" cellpadding="1" cellspacing="0">
                    <tr>
                        <td style="padding-left: 10px">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                        </td>
                        <td>
                            <asp:Label ID="lbl_PRRequest_Nm" runat="server" Text="Product Movement" SkinID="LBL_HD_WHITE"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
            <td align="right" style="padding-left: 10px;">
                <dx:ASPxMenu runat="server" ID="menu_CmdBar" Font-Bold="True" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle"
                    Height="16px" OnItemClick="menu_CmdBar_ItemClick">
                    <ItemStyle BackColor="Transparent">
                        <HoverStyle BackColor="Transparent">
                            <Border BorderStyle="None" />
                        </HoverStyle>
                        <Paddings Padding="2px" />
                        <Border BorderStyle="None" />
                    </ItemStyle>
                    <Items>
                        <dx:MenuItem Name="Print" Text="">
                            <ItemStyle Height="16px" Width="43px">
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-print.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                            </ItemStyle>
                        </dx:MenuItem>
                    </Items>
                    <Paddings Padding="0px" />
                    <SeparatorPaddings Padding="0px" />
                    <SubMenuStyle HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" Font-Size="9pt" ForeColor="#4D4D4D" />
                    <Border BorderStyle="None"></Border>
                </dx:ASPxMenu>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="width: 100%;">
                <tr>
                    <td style="width: 260; background-color: #0071BD; vertical-align: top;">
                        <div style="background-color: #0071BD; width: 20%; min-width: 240px;">
                            <!-- BU -->
                            <div style="padding: 5px 0 0 10px;">
                                <asp:Label ID="lbl_BU" runat="server" SkinID="LBL_HD_WHITE" Text="Business Unit:" />
                            </div>
                            <div style="padding: 5px 0 0 10px;">
                                <asp:DropDownList ID="ddl_Bu" runat="server" SkinID="DDL_V1" Width="220" AutoPostBack="false" Enabled="false" OnSelectedIndexChanged="ddl_Bu_SelectedIndexChanged" />
                            </div>
                            <!-- Date -->
                            <div style="padding: 5px 0 0 10px;">
                                <div style="float: left; width: 110px;">
                                    <asp:Label ID="lbl_DateFrom" runat="server" SkinID="LBL_HD_WHITE" Text="Date From:" Width="100"></asp:Label>
                                </div>
                                <div style="float: left; width: 110px;">
                                    <asp:Label ID="lbl_DateTo" runat="server" SkinID="LBL_HD_WHITE" Text="Date To:" Width="100"></asp:Label>
                                </div>
                            </div>
                            <div style="clear: both; padding: 5px 0 0 10px;">
                                <div style="float: left; width: 110px;">
                                    <asp:TextBox ID="txt_DateFrom" runat="server" SkinID="TXT_V1" Width="100" Enabled="false" ToolTip="dd/MM/yyyy"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_DateFrom" runat="server" Format="dd/MM/yyyy">
                                    </ajaxToolkit:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="rfv_DateFrom" runat="server" ControlToValidate="txt_DateFrom" ErrorMessage="*"></asp:RequiredFieldValidator>
                                </div>
                                <div style="float: left; width: 110px;">
                                    <asp:TextBox ID="txt_DateTo" runat="server" SkinID="TXT_V1" Width="100" AutoPostBack="True" ToolTip="dd/MM/yyyy" OnTextChanged="txt_DateTo_TextChanged"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfv_DateTo" runat="server" ControlToValidate="txt_DateTo" ErrorMessage="*"></asp:RequiredFieldValidator>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_DateTo" runat="server" Format="dd/MM/yyyy">
                                    </ajaxToolkit:CalendarExtender>
                                </div>
                            </div>
                            <!-- Location -->
                            <div style="clear: both; padding: 5px 0 0 10px;">
                                <asp:Label ID="lbl_Location" runat="server" SkinID="LBL_HD_WHITE" Text="Store/Location:" Width="220"></asp:Label>
                            </div>
                            <div style="padding: 5px 0 0 10px;">
                                <asp:DropDownList ID="ddl_Location" runat="server" SkinID="DDL_V1" Width="220" AutoPostBack="true" OnSelectedIndexChanged="ddl_SubCate_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <!-- Category -->
                            <div style="padding: 5px 0 0 10px;">
                                <asp:Label ID="lbl_Category" runat="server" SkinID="LBL_HD_WHITE" Text="Category:"></asp:Label>
                            </div>
                            <div style="padding: 5px 0 0 10px;">
                                <asp:DropDownList ID="ddl_Category" runat="server" SkinID="DDL_V1" Width="220" AutoPostBack="True" OnSelectedIndexChanged="ddl_Category_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <!-- Sub-Category -->
                            <div style="padding: 5px 0 0 10px;">
                                <asp:Label ID="lbl_SubCate" runat="server" SkinID="LBL_HD_WHITE">Sub Category Search:</asp:Label>
                            </div>
                            <div style="padding: 5px 0 0 10px;">
                                <asp:DropDownList ID="ddl_SubCate" runat="server" SkinID="DDL_V1" Width="220" AutoPostBack="true" OnSelectedIndexChanged="ddl_SubCate_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <!-- ItemGroup -->
                            <div style="padding: 5px 0 0 10px;">
                                <asp:Label ID="lbl_ItemGroup" runat="server" SkinID="LBL_HD_WHITE">Itemn Group Search:</asp:Label>
                            </div>
                            <div style="padding: 5px 0 0 10px;">
                                <asp:DropDownList ID="ddl_ItemGroup" runat="server" SkinID="DDL_V1" Width="220" AutoPostBack="true" OnSelectedIndexChanged="ddl_ItemGroup_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <!-- Product -->
                            <div style="padding: 5px 0 0 10px;">
                                <asp:Label ID="lbl_Product" runat="server" SkinID="LBL_HD_WHITE" Text="Product Search:"></asp:Label>
                            </div>
                            <div style="padding: 5px 0 0 10px;">
                                <asp:DropDownList ID="ddl_Product" runat="server" SkinID="DDL_V1" Width="220">
                                </asp:DropDownList>
                            </div>
                            <!-- Options -->
                            <div style="padding: 5px 0 0 10px;">
                                <asp:CheckBox ID="chk_ZeroValue" runat="server" SkinID="CHK_NORMAL_WHITE" Text="Display zero values" AutoPostBack="True" OnCheckedChanged="chk_ZeroValue_CheckedChanged" />
                            </div>
                            <br />
                            <!-- Button -->
                            <div style="padding: 5px 0 0 10px;">
                                <asp:Button ID="btn_Go" runat="server" Text="Go" Width="60" OnClick="btn_Go_Click" />
                            </div>
                        </div>
                    </td>
                    <td style="width: 100%; vertical-align: top;">
                        <div style="width: 80%;">
                            <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                <tr>
                                    <td style="width: 15%; padding-left: 20px">
                                        <asp:Label ID="lbl_ItemDesc_Title" runat="server" SkinID="LBL_NR" Text="Item Description:"></asp:Label>
                                    </td>
                                    <td style="width: 85%;">
                                        <asp:Label ID="lbl_ItemDesc" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 20px">
                                        <asp:Label ID="lbl_BuName_Title" runat="server" SkinID="LBL_NR" Text="Business Unit:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_BuName" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <%--<asp:GridView ID="grd_Test" runat="server" AutoGenerateColumns="true" Width="100%">
                            </asp:GridView>--%>
                            <asp:Repeater ID="rpt_Store" runat="server" OnItemDataBound="rpt_Store_ItemDataBound">
                                <HeaderTemplate>
                                    <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td style="padding-left: 20px">
                                            <asp:Label ID="lbl_Store" runat="server" SkinID="LBL_HD"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 20px">
                                            <asp:GridView ID="grd_ProdMove" runat="server" SkinID="GRD_V1" AutoGenerateColumns="False" Width="100%" OnRowDataBound="grd_ProdMove_RowDataBound">
                                                <Columns>
                                                    <asp:BoundField DataField="Date" HeaderText="Date" DataFormatString="{0:d}">
                                                        <HeaderStyle HorizontalAlign="Left" Width="8%" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="RefNo" HeaderText="Ref#">
                                                        <HeaderStyle HorizontalAlign="Left" Width="14%" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Type" HeaderText="Type">
                                                        <HeaderStyle HorizontalAlign="Left" Width="8%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="BwfQty" HeaderText="B/F Qty">
                                                        <HeaderStyle HorizontalAlign="Right" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="In" HeaderText="In">
                                                        <HeaderStyle HorizontalAlign="Right" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Out" HeaderText="Out">
                                                        <HeaderStyle HorizontalAlign="Right" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="C/F Qty">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_CFQty" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Right" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Cost" HeaderText="Cost/Unit">
                                                        <HeaderStyle HorizontalAlign="Right" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Amount" HeaderText="Amount">
                                                        <HeaderStyle HorizontalAlign="Right" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Balance">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_Balance" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Right" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                        <tr class="grdHeaderRow_V1">
                                                            <td align="left" style="width: 8%">
                                                                Date
                                                            </td>
                                                            <td align="left" style="width: 14%">
                                                                Ref#
                                                            </td>
                                                            <td align="left" style="width: 8%">
                                                                Type
                                                            </td>
                                                            <td align="right" style="width: 10%">
                                                                B/F Qty
                                                            </td>
                                                            <td align="right" style="width: 10%">
                                                                In
                                                            </td>
                                                            <td align="right" style="width: 10%">
                                                                Out
                                                            </td>
                                                            <td align="right" style="width: 10%">
                                                                C/F Qty
                                                            </td>
                                                            <td align="right" style="width: 10%">
                                                                Cost/Unit
                                                            </td>
                                                            <td align="right" style="width: 10%">
                                                                Amount
                                                            </td>
                                                            <td align="right" style="width: 10%">
                                                                Balance
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                            <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                <tr style="height: 17px">
                                                    <td align="right" width="40%">
                                                        <asp:Label ID="lbl_TotalQty_Sum_Title" runat="server" SkinID="LBL_HD" Text="Total Qty:"></asp:Label>
                                                    </td>
                                                    <td align="right" width="10%">
                                                        <asp:Label ID="lbl_InQty" runat="server" SkinID="LBL_HD"></asp:Label>
                                                    </td>
                                                    <td align="right" width="10%">
                                                        <asp:Label ID="lbl_OutQty" runat="server" SkinID="LBL_HD"></asp:Label>
                                                    </td>
                                                    <td align="right" width="10%">
                                                        <asp:Label ID="lbl_CFQty" runat="server" SkinID="LBL_HD"></asp:Label>
                                                    </td>
                                                    <td align="right" width="10%">
                                                        <asp:Label ID="lbl_Cost" runat="server" SkinID="LBL_HD"></asp:Label>
                                                    </td>
                                                    <td align="right" width="20%">
                                                        <asp:Label ID="lbl_Total" runat="server" SkinID="LBL_HD"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpPgDetail" PopupControlID="UpPgDetail" BackgroundCssClass="POPUP_BG"
        RepositionMode="RepositionOnWindowResizeAndScroll">
    </ajaxToolkit:ModalPopupExtender>
    <asp:UpdateProgress ID="UpPgDetail" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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
</asp:Content>
