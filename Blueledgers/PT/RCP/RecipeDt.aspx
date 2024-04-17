<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RecipeDt.aspx.cs" Inherits="BlueLedger.PL.PT.RCP.RecipeDt" MasterPageFile="~/Master/In/SkinDefault.master" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Src="~/PC/StockSummary.ascx" TagName="StockSummary" TagPrefix="uc1" %>
<%@ Register Src="~/PC/StockMovement.ascx" TagName="StockMovement" TagPrefix="uc5" %>
<%@ Register Src="~/UserControl/Comment2.ascx" TagName="Comment2" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/Attach2.ascx" TagName="Attach2" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Log2.ascx" TagName="Log2" TagPrefix="uc4" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cph_Main">
    <script type="text/javascript">
        function expandDetailsInGrid(_this) {
            var id = _this.id;
            var imgelem = document.getElementById(_this.id);
            var currowid = id.replace("Img_Btn1", "TR_Summmary") //GETTING THE ID OF SUMMARY ROW

            var rowdetelemid = currowid;
            var rowdetelem = document.getElementById(rowdetelemid);
            if (imgelem.alt == "plus") {
                imgelem.src = "../../App_Themes/Default/Images/Plus_1.jpg"
                imgelem.alt = "minus"
                rowdetelem.style.display = 'none';
            }
            else {
                imgelem.src = "../../App_Themes/Default/Images/Minus_1.jpg"
                imgelem.alt = "plus"
                rowdetelem.style.display = '';
            }

            return false;

        }
    </script>
    <style type="text/css">
        table.Header
        {
            width: 100%;
        }
        table.Header tr
        {
            width: 100%;
        }
        table.Header td
        {
            padding-left: 10px;
            vertical-align: top;
        }
        .recipe_image
        {
            border: 1px solid silver;
            width: 400px;
            height: 100%;
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
    </style>
    <!-- Hidden Field(s) -->
    <div>
        <asp:HiddenField ID="hf_ConnStr" runat="server" />
        <asp:HiddenField runat="server" ID="hf_DefaultAmtDigit" />
        <asp:HiddenField runat="server" ID="hf_DefaultSvcRate" />
        <asp:HiddenField runat="server" ID="hf_DefaultTaxRate" />
    </div>
    <!-- MENU BAR -->
    <div class="CMD_BAR">
        <div class="CMD_BAR_LEFT">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
            <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:PT_RCP_Recipe, lbl_Title %>" SkinID="LBL_HD_WHITE" />
        </div>
        <div class="CMD_BAR_RIGHT">
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
                    <dx:MenuItem Name="Update" Text="Update Cost">
                        <ItemStyle Height="16px" Width="20px" ForeColor="White" Font-Size="8.7px" Font-Names="Tahoma" Paddings-PaddingBottom="0px"></ItemStyle>
                    </dx:MenuItem>
                    <dx:MenuItem Name="Create" Text="">
                        <ItemStyle Height="16px" Width="49px">
                            <HoverStyle>
                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png" Repeat="NoRepeat" VerticalPosition="center" />
                            </HoverStyle>
                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/create.png" Repeat="NoRepeat" VerticalPosition="center" />
                        </ItemStyle>
                    </dx:MenuItem>
                    <dx:MenuItem Name="Edit" Text="">
                        <ItemStyle Height="16px" Width="38px">
                            <HoverStyle>
                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-edit.png" Repeat="NoRepeat" VerticalPosition="center" />
                            </HoverStyle>
                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/edit.png" Repeat="NoRepeat" VerticalPosition="center" />
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
                    <dx:MenuItem Name="Print" Text="">
                        <ItemStyle Height="16px" Width="43px">
                            <HoverStyle>
                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-print.png" Repeat="NoRepeat" VerticalPosition="center" />
                            </HoverStyle>
                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                        </ItemStyle>
                    </dx:MenuItem>
                    <dx:MenuItem Name="Back" Text="">
                        <ItemStyle Height="16px" Width="42px">
                            <HoverStyle>
                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png" Repeat="NoRepeat" VerticalPosition="center" />
                            </HoverStyle>
                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                        </ItemStyle>
                    </dx:MenuItem>
                </Items>
                <Paddings Padding="0px" />
                <SeparatorPaddings Padding="0px" />
                <SubMenuStyle HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" Font-Size="9pt" ForeColor="#4D4D4D" />
                <Border BorderStyle="None"></Border>
            </dx:ASPxMenu>
        </div>
    </div>
    <!-- Alert -->
    <div id="VoidMessage" runat="server" class="message_box_void" style="display: none;">
        <strong>VOID</strong> (<span><asp:Label ID="lbl_VoidComment_Alert" runat="server" /></span>)
    </div>
    <div class="printable">
        <!-- Header -->
        <table class="Header">
            <!--Row 1-->
            <tr>
                <!--Image-->
                <td rowspan="8" class="recipe_image" style="width: 320px;">
                    <asp:Image ID="img01" runat="server" Width="200px" />
                </td>
                <!-- Recipe Code -->
                <td style="width: 100px;">
                    <asp:Label ID="lbl01" runat="server" SkinID="LBL_HD">Recipe Code:</asp:Label>
                </td>
                <td style="width: 220px;">
                    <asp:Label ID="lbl_RcpCode" runat="server" SkinID="LBL_NR_BLUE" />
                </td>
                <!-- Preparation -->
                <td colspan="2">
                    <asp:Label ID="lbl12" runat="server" SkinID="LBL_HD">Preparation:</asp:Label>
                </td>
                <!--Status-->
                <td style="width: 80px;">
                    <asp:Label ID="lbl04" runat="server" SkinID="LBL_HD">Status:</asp:Label>
                </td>
                <td style="width: 200px;">
                    <asp:Label ID="lbl_RcpStatus" runat="server" SkinID="LBL_NR_BLUE" />
                </td>
            </tr>
            <!--Row 2-->
            <tr>
                <!--Image-->
                <!-- Description1 -->
                <td>
                    <asp:Label ID="lbl02" runat="server" SkinID="LBL_HD">Description1:</asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbl_RcpDesc1" runat="server" SkinID="LBL_NR_BLUE" />
                </td>
                <!-- Prepartion -->
                <td colspan="2" rowspan="6">
                    <asp:TextBox ID="txt_Preparation" runat="server" ReadOnly="true" Width="100%" Rows="8" TextMode="MultiLine" SkinID="TXT_V1" />
                </td>
                <!-- Summary -->
                <td colspan="2" rowspan="8" style="width: 360px;">
                    <div style="border: 1px solid #C9C9C9; width: 100%; height: 100%;">
                        <table class="TABLE_HD" style="border-spacing: 10px; height: 100%;">
                            <tr>
                                <td style="width: 100px;">
                                    <asp:Label ID="lbl03" runat="server" SkinID="LBL_HD">Total Cost:</asp:Label>
                                </td>
                                <td style="width: 150px; text-align: right;">
                                    <asp:Label ID="lblTotalCost" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px;">
                                    <asp:Label ID="lbl07" runat="server" SkinID="LBL_HD">Total Mix(%):</asp:Label>
                                </td>
                                <td style="width: 150px; text-align: right;">
                                    <asp:Label ID="lblTotalMix" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px;">
                                    <asp:Label ID="lbl10" runat="server" SkinID="LBL_HD">Cost of TotalMix:</asp:Label>
                                </td>
                                <td style="width: 150px; text-align: right;">
                                    <asp:Label ID="lblCostOfTotalMix" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px;">
                                    <asp:Label ID="lbl13" runat="server" SkinID="LBL_HD">Net Price:</asp:Label>
                                </td>
                                <td style="width: 150px; text-align: right;">
                                    <asp:Label ID="lblNetPrice" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px;">
                                    <asp:Label ID="lbl14" runat="server" SkinID="LBL_HD">Gross Price:</asp:Label>
                                </td>
                                <td style="width: 150px; text-align: right;">
                                    <asp:Label ID="lblGrossPrice" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px;">
                                    <asp:Label ID="lbl18" runat="server" SkinID="LBL_HD">Net Cost(%):</asp:Label>
                                </td>
                                <td style="width: 150px; text-align: right;">
                                    <asp:Label ID="lblNetCost" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px;">
                                    <asp:Label ID="lbl19" runat="server" SkinID="LBL_HD">Gross Cost(%):</asp:Label>
                                </td>
                                <td style="width: 150px; text-align: right;">
                                    <asp:Label ID="lblGrossCost" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                                </td>
                            </tr>
                            <%--Service Charge & Tax Rate--%>
                            <tr>
                                <td colspan="2">
                                    <asp:Label runat="server" ID="lbl_TaxSvcRate" ForeColor="#101010" Font-Size="0.9em" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <!--Row 3-->
            <tr>
                <!-- Image -->
                <!-- Description2 -->
                <td>
                    <asp:Label ID="lbl06" runat="server" SkinID="LBL_HD">Description2:</asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbl_RcpDesc2" runat="server" SkinID="LBL_NR_BLUE" />
                </td>
                <!-- Preparation -->
            </tr>
            <!--Row 4-->
            <tr>
                <!-- Image -->
                <!-- Category -->
                <td>
                    <asp:Label ID="lbl05" runat="server" SkinID="LBL_HD">Category:</asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbl_RcpCateCode" runat="server" SkinID="LBL_NR_BLUE" />
                </td>
                <!-- Preparation -->
            </tr>
            <!--Row 5-->
            <tr>
                <!-- Image -->
                <!-- Locaiton -->
                <td>
                    <asp:Label ID="lbl20" runat="server" SkinID="LBL_HD">Location:</asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbl_RcpLocateCode" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
                <!-- Preparation -->
            </tr>
            <!--Row 6-->
            <tr>
                <!-- Image -->
                <!-- Unit -->
                <td>
                    <asp:Label ID="lbl08" runat="server" SkinID="LBL_HD">Unit of Sale:</asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbl_RcpUnit" runat="server" SkinID="LBL_NR_BLUE" />
                </td>
                <!-- Preparation -->
            </tr>
            <!--Row 7-->
            <tr>
                <!-- Image -->
                <!-- Portion Size  -->
                <td>
                    <asp:Label ID="lbl09" runat="server" SkinID="LBL_HD">Portion Size:</asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbl_PortionSize" runat="server" SkinID="LBL_NR_BLUE" />
                </td>
                <!-- Preparation -->
            </tr>
            <!--Row 8-->
            <tr>
                <!-- Image -->
                <!-- Cost of Portion  -->
                <td>
                    <asp:Label ID="lbl11" runat="server" SkinID="LBL_HD">Cost of Portion:</asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblCostOfPortion" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
                <!-- Preparation Time -->
                <td colspan="2">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <asp:Label ID="lbl15" runat="server" SkinID="LBL_HD" Width="100%">Preparation Time:</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lbl_PrepTime" runat="server" SkinID="LBL_NR_BLUE" />
                            </td>
                            <td>
                                <asp:Label ID="lbl16" runat="server" SkinID="LBL_HD" Width="100%">Total Time:</asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lbl_TotalTime" runat="server" SkinID="LBL_NR_BLUE" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <!-- Row 9 -->
            <tr>
                <!-- Image Load Button -->
                <td>
                </td>
                <!-- Remark -->
                <td>
                    <asp:Label ID="lbl17" runat="server" SkinID="LBL_HD">Remark:</asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txt_Note" runat="server" Width="100%" ReadOnly="True" TextMode="MultiLine" SkinID="LBL_NR_BLUE" />
                </td>
            </tr>
        </table>
        <!-- DETAIL -->
        <asp:GridView ID="grd_RecipeDt" runat="server" AutoGenerateColumns="False" SkinID="GRD_V1" Width="100%" OnLoad="grd_RecipeDt_Load" OnRowDataBound="grd_RecipeDt_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="<%$ Resources:PT_RCP_Recipe, lbl_RcpDtItemType %>">
                    <ItemStyle VerticalAlign="Top" />
                    <ItemTemplate>
                        <div style="width: 65px; overflow: hidden; white-space: nowrap">
                            <asp:Label ID="lbl_ItemType" runat="server" Width="100%" />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:PT_RCP_Recipe, lbl_RcpDtItem %>">
                    <ItemStyle VerticalAlign="Top" />
                    <ItemTemplate>
                        <div style="width: 100px; overflow: hidden; white-space: nowrap">
                            <asp:Label ID="lbl_ItemCode" runat="server" Width="100%" />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemStyle VerticalAlign="Top" />
                    <HeaderTemplate>
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 400px;">
                                    <asp:Label ID="lbl_ItemDesc_Nm" runat="server" Text="<%$ Resources:PT_RCP_Recipe, lbl_RcpDtItemDesc1 %>" />
                                </td>
                                <td style="width: 200px; text-align: right;">
                                    <asp:Label ID="lbl_RcpDtQty_Nm" runat="server" Text="<%$ Resources:PT_RCP_Recipe, lbl_RcpDtQty %>" />
                                </td>
                                <td style="width: 200px;">
                                    <asp:Label ID="lbl_RcpUnit_Nm" runat="server" Text="<%$ Resources:PT_RCP_Recipe, lbl_RcpDtUnit %>" />
                                </td>
                                <td style="width: 200px; text-align: right;">
                                    <asp:Label ID="lbl_BaseCost_Nm" runat="server" Text="<%$ Resources:PT_RCP_Recipe, lbl_RcpDtBaseCost %>" />
                                </td>
                                <td style="width: 200px; text-align: right;">
                                    <asp:Label ID="lbl_WastageRate_Nm" runat="server" Text="<%$ Resources:PT_RCP_Recipe, lbl_RcpDtWastageRate %>" />
                                </td>
                                <td style="width: 200px; text-align: right;">
                                    <asp:Label ID="lbl_NetCost_Nm" runat="server" Text="<%$ Resources:PT_RCP_Recipe, lbl_RcpDtNetCost %>" />
                                </td>
                                <td style="width: 200px; text-align: right;">
                                    <asp:Label ID="lbl_WastageCost" runat="server" Text="<%$ Resources:PT_RCP_Recipe, lbl_RcpDtWastageCost %>" />
                                </td>
                                <td style="width: 200px; text-align: right;">
                                    <asp:Label ID="lbl_TotalCost_Nm" runat="server" Text="<%$ Resources:PT_RCP_Recipe, lbl_RcpDtTotalCost %>" />
                                </td>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 400px;">
                                    <asp:Label ID="lbl_ItemDesc1" runat="server" Width="100%" />
                                </td>
                                <td style="width: 200px; text-align: right;">
                                    <asp:Label ID="lbl_RcpDtQty" runat="server" />
                                </td>
                                <td style="width: 200px;">
                                    <asp:Label ID="lbl_RcpUnit" runat="server" />
                                </td>
                                <td style="width: 200px; text-align: right;">
                                    <asp:Label ID="lbl_BaseCost" runat="server" />
                                </td>
                                <td style="width: 200px; text-align: right;">
                                    <asp:Label ID="lbl_WastageRate" runat="server" />
                                </td>
                                <td style="width: 200px; text-align: right;">
                                    <asp:Label ID="lbl_NetCost" runat="server" />
                                </td>
                                <td style="width: 200px; text-align: right;">
                                    <asp:Label ID="lbl_WastageCost" runat="server" />
                                </td>
                                <td style="width: 200px; text-align: right;">
                                    <asp:Label ID="lbl_TotalCost" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <%--Description 2--%>
                                <td>
                                    <asp:Label runat="server" ID="lbl_ItemDesc2" />
                                </td>
                                <!-- Base Unit & Unit Rate-->
                                <td colspan="5">
                                    <asp:Label runat="server" ID="lbl_BaseUnit_Nm" Text="<%$Resources:PT_RCP_Recipe, lbl_RcpDtBaseUnit %>" SkinID="LBL_HD_1" />
                                    &nbsp;
                                    <asp:Label runat="server" ID="lbl_BaseUnit" />
                                    &nbsp;(
                                    <asp:Label ID="lbl_UnitRate_Nm" runat="server" Text="<%$Resources:PT_RCP_Recipe, lbl_RcpDtUnitRate %>" SkinID="LBL_HD_1" />
                                    &nbsp;
                                    <asp:Label ID="lbl_UnitRate" runat="server" Text="0.00" />
                                    )
                                </td>
                            </tr>
                            <%-- <tr>
                                <td>
                                    <asp:Label ID="lbl_ItemDesc2" runat="server" ForeColor="DimGray" Width="100%" />
                                </td>
                                <td style="text-align: right;">
                                    <asp:Label ID="lbl_UnitRate_Nm" runat="server" ForeColor="DimGray" Font-Bold="true" Text="<%$ Resources:PT_RCP_Recipe, lbl_RcpDtUnitRate %>" />
                                </td>
                                <td>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_UnitRate" runat="server" ForeColor="DimGray" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_BaseUnit" runat="server" ForeColor="DimGray" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>--%>
                        </table>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <dx:ASPxPopupControl ID="pop_ConfirmVoid" ClientInstanceName="pop_ConfirmVoid" runat="server" CloseAction="CloseButton" HeaderText="<%$ Resources:PT_RCP_Recipe, pop_Confirmation %>"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Width="300px" HeaderStyle-BackColor="Yellow">
        <HeaderStyle BackColor="Yellow"></HeaderStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr style="height: 50px;">
                        <td align="center" colspan="2">
                            <asp:Label ID="lbl_ConfirmVoid_Nm" runat="server" Text="<%$ Resources:PT_RCP_Recipe, lbl_Void_Message %>" SkinID="LBL_NR" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lbl_VoidComment" runat="server" Text="Reason to void:" SkinID="LBL_NR" />
                            &nbsp;
                            <asp:TextBox ID="txt_VoidComment" runat="server" Width="100%" TextMode="MultiLine" AutoPostBack="True" />
                        </td>
                    </tr>
                    <tr style="height: 50px;">
                        <td align="right">
                            <asp:Button ID="btn_ConfirmVoid" runat="server" Text="<%$ Resources:PT_RCP_Recipe, btn_Yes %>" OnClick="btn_ConfirmVoid_Click" SkinID="BTN_V1" Width="60px" />
                        </td>
                        <td align="left">
                            <asp:Button ID="btn_CancelVoid" runat="server" Text="<%$ Resources:PT_RCP_Recipe, btn_No %>" OnClick="btn_CancelVoid_Click" SkinID="BTN_V1" Width="60px" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_Information" runat="server" HeaderText="<%$ Resources:PT_RCP_Recipe, pop_Information %>" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" Width="300px" Modal="True" HeaderStyle-BackColor="Blue" HeaderStyle-ForeColor="White">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center">
                            <asp:Label ID="lbl_Information" runat="server" SkinID="LBL_NR" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btn_Info_OK" runat="server" Text="<%$ Resources:PT_RCP_Recipe, btn_Ok %>" SkinID="BTN_V1" Width="60px" OnClick="btn_Void_Info_OK_Click" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
