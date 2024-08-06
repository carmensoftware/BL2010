<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TrfDt.aspx.cs" Inherits="BlueLedger.PL.IN.Transfer.TrfDt"
    MasterPageFile="~/Master/In/SkinDefault.master" %>

<%--<%@ Register Src="~/UserControl/WFControl.ascx" TagName="WFControl" TagPrefix="uc1" %>--%>
<%@ Register Src="~/UserControl/Comment2.ascx" TagName="Comment2" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/Attach2.ascx" TagName="Attach2" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Log2.ascx" TagName="Log2" TagPrefix="uc4" %>
<%@ Register Src="~/UserControl/ProcessStatus.ascx" TagName="ProcessStatus" TagPrefix="uc5" %>
<%@ Register Src="~/PC/StockSummary.ascx" TagName="StockSummary" TagPrefix="uc6" %>
<%@ Register Src="~/PC/StockMovement.ascx" TagName="StockMovement" TagPrefix="uc7" %>
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cph_Main">
    <script type="text/javascript" language="javascript">

        //Check Select All CheckBox
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

        function expandDetailsInGrid(_this) {
            var id = _this.id;
            var imgelem = document.getElementById(_this.id);
            var currowid = id.replace("Img_Btn", "TR_Summmary") //GETTING THE ID OF SUMMARY ROW

            var rowdetelemid = currowid;
            var rowdetelem = document.getElementById(rowdetelemid);
            if (imgelem.alt == "plus") {
                imgelem.src = "../../App_Themes/Default/Images/master/in/Default/Plus.jpg"
                imgelem.alt = "minus"
                rowdetelem.style.display = 'none';
            }
            else {
                imgelem.src = "../../App_Themes/Default/Images/master/in/Default/Minus.jpg"
                imgelem.alt = "plus"
                rowdetelem.style.display = '';
            }

            return false;
        }
    </script>
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
    <%--<asp:Panel ID="p_HeaderAllocated" runat="server" Visible="false">
                        <table border="0" cellpadding="3" cellspacing="0" width="100%">
                            <tr>
                                <td align="right" style="width: 8%; padding-right: 5px;">
                                    <asp:Label ID="lbl_QtyAllocate_Allocate_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_QtyAllocate_Allocate_Nm %>"
                                        SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td align="right" style="width: 8%; padding-right: 5px;">
                                    <asp:Label ID="lbl_Approve_Allocate_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_Approve_Allocate_Nm %>"
                                        SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td align="left" style="width: 3%; padding-right: 5px;">
                                    <asp:Label ID="lbl_Unit_Allocate_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_Unit_Allocate_Nm %>"
                                        SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td align="left" style="width: 30%; padding-right: 5px;">
                                    <asp:Label ID="lbl_Product_Allocate_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_Product_Allocate_Nm %>"
                                        SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td align="left" style="width: 9%; padding-right: 5px;">
                                    <asp:Label ID="lbl_ReqDate_Allocate_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_ReqDate_Allocate_Nm %>"
                                        SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td align="left" style="width: 18%">
                                    <asp:Label ID="lbl_StoreName_Allocate_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_StoreName_Allocate_Nm %>"
                                        SkinID="LBL_HD_W"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>--%>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr style="background-color: #4d4d4d; height: 17px;">
            <td align="left" style="padding-left: 10px;">
                <%--<td align="left" style="width: 10%">
                                    <asp:Label ID="lbl_TransferTo_Issue" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>--%>
                <table border="0" cellpadding="2" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                        </td>
                        <td>
                            <asp:Label ID="lbl_StoreReq_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_StoreReq_Nm %>"
                                SkinID="LBL_HD_WHITE"></asp:Label>
                            <%--<td align="left" style="width: 7%">
                                    <asp:Label ID="lbl_ProductCode_Issue" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>--%>
                        </td>
                    </tr>
                </table>
            </td>
            <td align="right">
                <table border="0" cellpadding="1" cellspacing="0">
                    <tr>
                        <td>
                            <dx:ASPxButton ID="btn_Create" runat="server" Height="16px" Width="49px" OnClick="btn_Create_Click"
                                ToolTip="Create" BackColor="Transparent">
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/create.png" Repeat="NoRepeat"
                                    HorizontalPosition="center" VerticalPosition="center" />
                                <HoverStyle>
                                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png"
                                        Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                </HoverStyle>
                                <Border BorderStyle="None" />
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="btn_Edit" runat="server" OnClick="btn_Edit_Click" BackColor="Transparent"
                                Height="16px" Width="38px" ToolTip="Edit">
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/edit.png" Repeat="NoRepeat"
                                    HorizontalPosition="center" VerticalPosition="center" />
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-edit.png"
                                        Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <Border BorderStyle="None" />
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="btn_Void" runat="server" OnClick="btn_Void_Click" BackColor="Transparent"
                                Height="16px" Width="41px" ToolTip="Void">
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/void.png" Repeat="NoRepeat"
                                    HorizontalPosition="center" VerticalPosition="center" />
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-void.png"
                                        Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <Border BorderStyle="None" />
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="btn_Print" runat="server" BackColor="Transparent" Height="16px"
                                Width="43px" ToolTip="Print" OnClick="btn_Print_Click">
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat"
                                    HorizontalPosition="center" VerticalPosition="center" />
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-print.png"
                                        Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <Border BorderStyle="None" />
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="btn_Back" runat="server" OnClick="btn_Back_Click" BackColor="Transparent"
                                Height="16px" Width="42px" ToolTip="Back">
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat"
                                    HorizontalPosition="center" VerticalPosition="center" />
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png"
                                        Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <Border BorderStyle="None" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div class="printable">
        <table border="0" cellpadding="1" cellspacing="0" width="100%" class="TABLE_HD">
            <tr>
                <td rowspan="3" style="width: 1%;">
                </td>
                <td style="width: 7%;">
                    <asp:Label ID="lbl_Ref_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt,lbl_Ref_Nm %>"
                        SkinID="LBL_HD"></asp:Label>
                </td>
                <td style="width: 11%;">
                    <asp:Label ID="lbl_Ref" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
                <td style="width: 8.5%;">
                    <asp:Label ID="lbl_ReqFrom_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt,lbl_ReqFrom_Nm %>"
                        SkinID="LBL_HD"></asp:Label>
                </td>
                <td style="width: 30%;">
                    <%--<td align="left" style="width: 15%">
                                    <asp:Label ID="lbl_LocalName_Issue" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>--%>
                    <asp:Label ID="lbl_StoreName" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
                <td style="width: 5%;">
                    <%--<asp:Panel ID="p_ItemApproval" runat="server" Visible="false">
                        <table border="0" cellpadding="3" cellspacing="0" width="100%">
                            <tr>
                                <td align="right" style="width: 8%; padding-right: 5px;">
                                    <asp:Label ID="lbl_QtyAppr_Appr" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td align="right" style="width: 8%; padding-right: 5px;">
                                    <asp:Label ID="lbl_QtyRequested_Appr" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td align="left" style="width: 3%; padding-right: 5px;">
                                    <asp:Label ID="lbl_Unit_Appr" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td align="left" style="width: 30%; padding-right: 5px;">
                                    <asp:Label ID="lbl_EnglishName_Appr" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td align="left" style="width: 9%; padding-right: 5px;">
                                    <asp:Label ID="lbl_DeliveryDate_Appr" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td align="left" style="width: 18%">
                                    <asp:Label ID="lbl_StoreName_Appr" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="p_ItemAllocated" runat="server" Visible="false">
                        <table border="0" cellpadding="3" cellspacing="0" width="100%">
                            <tr>
                                <td align="right" style="width: 8%; padding-right: 5px;">
                                    <asp:Label ID="lbl_QtyAllocated" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td align="right" style="width: 8%; padding-right: 5px;">
                                    <asp:Label ID="lbl_QtyAppr_Allocated" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td align="left" style="width: 3%; padding-right: 5px;">
                                    <asp:Label ID="lbl_Unit_Allocated" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td align="left" style="width: 30%; padding-right: 5px;">
                                    <asp:Label ID="lbl_EnglishName_Allocated" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td align="left" style="width: 9%; padding-right: 5px;">
                                    <asp:Label ID="lbl_DeliveryDate_Allocated" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td align="left" style="width: 18%">
                                    <asp:Label ID="lbl_StoreName_Allocated" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>--%>
                </td>
                <td style="width: 38%;">
                    <%--style="display: none"--%>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_Date_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_Date_Nm %>"
                        SkinID="LBL_HD"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbl_Date" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbl_Status_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_Status_Nm %>"
                        SkinID="LBL_HD"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbl_Status" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
                <%--<asp:Panel ID="p_DetailRows" runat="server" Visible="false" BackColor="Transparent">--%>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_Desc_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_Desc_Nm %>"
                        SkinID="LBL_HD"></asp:Label>
                </td>
                <td colspan="5">
                    <asp:Label ID="lbl_Desc" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:GridView ID="grd_TrfDt" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
            Width="100%" OnRowDataBound="grd_TrfDt_RowDataBound" SkinID="GRD_V1" EmptyDataText="No Date to Display"
            OnRowCommand="grd_TrfDt_RowCommand">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr style="height: 16px">
                                <td valign="bottom">
                                    <asp:ImageButton ID="Img_Btn" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/Plus.jpg"
                                        OnClientClick="expandDetailsInGrid(this);return false;" />
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <HeaderStyle Width="10px" />
                    <ItemStyle HorizontalAlign="Left" Width="10px" VerticalAlign="Top" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox ID="chk_All" runat="server" onclick="Check(this)" SkinID="CHK_V1" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr style="height: 16px">
                                <td valign="bottom">
                                    <asp:CheckBox ID="chk_Item" runat="server" SkinID="CHK_V1" />
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" Width="10px" />
                    <ItemStyle HorizontalAlign="Left" Width="10px" VerticalAlign="Top" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Panel ID="p_HeaderIssue" runat="server">
                            <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                <tr>
                                    <%--<td align="left" style="width: 10%">
                                    <asp:Label ID="Label11" runat="server" Text="Transfer To" SkinID="LBL_HD_W"></asp:Label>
                                </td>--%>
                                    <td align="left" style="width: 18%; padding-right: 5px;">
                                        <asp:Label ID="lbl_StoreName_Issue_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_StoreName_Issue_Nm %>"
                                            SkinID="LBL_HD_W"></asp:Label>
                                    </td>
                                    <%--<td align="left" style="width: 7%">
                                    <asp:Label ID="Label13" runat="server" Text="SKU#" SkinID="LBL_HD_W"></asp:Label>
                                </td>--%>
                                    <td align="left" style="width: 30%; padding-right: 5px;">
                                        <asp:Label ID="lbl_Product_Issue_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_Product_Issue_Nm %>"
                                            SkinID="LBL_HD_W"></asp:Label>
                                    </td>
                                    <%--<td align="left" style="width: 15%">
                                    <asp:Label ID="Label15" runat="server" Text="Local Name" SkinID="LBL_HD_W"></asp:Label>
                                </td>--%>
                                    <td align="left" style="width: 3%; padding-right: 5px;">
                                        <asp:Label ID="lbl_Unit_Issue_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_Unit_Issue_Nm %>"
                                            SkinID="LBL_HD_W"></asp:Label>
                                    </td>
                                    <td align="right" style="width: 9%; padding-right: 5px;">
                                        <asp:Label ID="lbl_QtyReq_Issue_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_QtyReq_Issue_Nm %>"
                                            SkinID="LBL_HD_W"></asp:Label>
                                    </td>
                                    <td align="left" style="width: 9%">
                                        <asp:Label ID="lbl_ReqDate_Issue_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_ReqDate_Issue_Nm %>"
                                            SkinID="LBL_HD_W"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <%--<asp:Panel ID="p_HeaderApproval" runat="server" Visible="false">
                        <table border="0" cellpadding="3" cellspacing="0" width="100%">
                            <tr>
                                <td align="right" style="width: 8%; padding-right: 5px;">
                                    <asp:Label ID="lbl_Approve_Appr_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_Approve_Appr_Nm %>"
                                        SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td align="right" style="width: 8%; padding-right: 5px;">
                                    <asp:Label ID="lbl_QtyReq_Appr_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_QtyReq_Appr_Nm %>"
                                        SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td align="left" style="width: 3%; padding-right: 5px;">
                                    <asp:Label ID="lbl_Unit_Appr_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_Unit_Appr_Nm %>"
                                        SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td align="left" style="width: 30%; padding-right: 5px;">
                                    <asp:Label ID="lbl_Product_Appr_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_Product_Appr_Nm %>"
                                        SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td align="left" style="width: 9%; padding-right: 5px;">
                                    <asp:Label ID="lbl_ReqDate_Appr_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_ReqDate_Appr_Nm %>"
                                        SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td align="left" style="width: 18%;">
                                    <asp:Label ID="lbl_StoreName_Appr_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_StoreName_Appr_Nm %>"
                                        SkinID="LBL_HD_W"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>--%>
                        <%--<asp:Panel ID="p_HeaderAllocated" runat="server" Visible="false">
                        <table border="0" cellpadding="3" cellspacing="0" width="100%">
                            <tr>
                                <td align="right" style="width: 8%; padding-right: 5px;">
                                    <asp:Label ID="lbl_QtyAllocate_Allocate_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_QtyAllocate_Allocate_Nm %>"
                                        SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td align="right" style="width: 8%; padding-right: 5px;">
                                    <asp:Label ID="lbl_Approve_Allocate_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_Approve_Allocate_Nm %>"
                                        SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td align="left" style="width: 3%; padding-right: 5px;">
                                    <asp:Label ID="lbl_Unit_Allocate_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_Unit_Allocate_Nm %>"
                                        SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td align="left" style="width: 30%; padding-right: 5px;">
                                    <asp:Label ID="lbl_Product_Allocate_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_Product_Allocate_Nm %>"
                                        SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td align="left" style="width: 9%; padding-right: 5px;">
                                    <asp:Label ID="lbl_ReqDate_Allocate_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_ReqDate_Allocate_Nm %>"
                                        SkinID="LBL_HD_W"></asp:Label>
                                </td>
                                <td align="left" style="width: 18%">
                                    <asp:Label ID="lbl_StoreName_Allocate_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_StoreName_Allocate_Nm %>"
                                        SkinID="LBL_HD_W"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>--%>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Panel ID="p_ItemIssue" runat="server">
                            <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                <tr>
                                    <%--<td align="left" style="width: 10%">
                                    <asp:Label ID="lbl_TransferTo_Issue" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>--%>
                                    <td align="left" style="width: 18%; padding-right: 5px;">
                                        <asp:Label ID="lbl_StoreName_Issue" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                    </td>
                                    <%--<td align="left" style="width: 7%">
                                    <asp:Label ID="lbl_ProductCode_Issue" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>--%>
                                    <td align="left" style="width: 30%; padding-right: 5px;">
                                        <asp:Label ID="lbl_EnglishName_Issue" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                    </td>
                                    <%--<td align="left" style="width: 15%">
                                    <asp:Label ID="lbl_LocalName_Issue" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>--%>
                                    <td align="left" style="width: 3%; padding-right: 5px;">
                                        <asp:Label ID="lbl_Unit_Issue" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                    </td>
                                    <td align="right" style="width: 9%; padding-right: 5px;">
                                        <asp:Label ID="lbl_QtyRequest_Issue" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                    </td>
                                    <td align="left" style="width: 9%">
                                        <asp:Label ID="lbl_DeliveryDate_Issue" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <%--<asp:Panel ID="p_ItemApproval" runat="server" Visible="false">
                        <table border="0" cellpadding="3" cellspacing="0" width="100%">
                            <tr>
                                <td align="right" style="width: 8%; padding-right: 5px;">
                                    <asp:Label ID="lbl_QtyAppr_Appr" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td align="right" style="width: 8%; padding-right: 5px;">
                                    <asp:Label ID="lbl_QtyRequested_Appr" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td align="left" style="width: 3%; padding-right: 5px;">
                                    <asp:Label ID="lbl_Unit_Appr" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td align="left" style="width: 30%; padding-right: 5px;">
                                    <asp:Label ID="lbl_EnglishName_Appr" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td align="left" style="width: 9%; padding-right: 5px;">
                                    <asp:Label ID="lbl_DeliveryDate_Appr" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td align="left" style="width: 18%">
                                    <asp:Label ID="lbl_StoreName_Appr" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="p_ItemAllocated" runat="server" Visible="false">
                        <table border="0" cellpadding="3" cellspacing="0" width="100%">
                            <tr>
                                <td align="right" style="width: 8%; padding-right: 5px;">
                                    <asp:Label ID="lbl_QtyAllocated" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td align="right" style="width: 8%; padding-right: 5px;">
                                    <asp:Label ID="lbl_QtyAppr_Allocated" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td align="left" style="width: 3%; padding-right: 5px;">
                                    <asp:Label ID="lbl_Unit_Allocated" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td align="left" style="width: 30%; padding-right: 5px;">
                                    <asp:Label ID="lbl_EnglishName_Allocated" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td align="left" style="width: 9%; padding-right: 5px;">
                                    <asp:Label ID="lbl_DeliveryDate_Allocated" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td align="left" style="width: 18%">
                                    <asp:Label ID="lbl_StoreName_Allocated" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>--%>
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:BoundField DataField="ApprStatus" HeaderText="<%$ Resources:IN_Transfer_TrfDt, ProcessStatus %>">
                <HeaderStyle HorizontalAlign="Left" Width="100px" />
                <ItemStyle HorizontalAlign="Left" Width="100px" />
            </asp:BoundField>--%>
                <asp:TemplateField>
                    <ItemTemplate>
                        <%--style="display: none"--%>
                        <tr id="TR_Summmary" runat="server" style="display: none">
                            <td colspan="5" style="padding-left: 10px; padding-right: 0px">
                                <%--<asp:Panel ID="p_DetailRows" runat="server" Visible="false" BackColor="Transparent">--%>
                                <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                    <tr style="vertical-align: top;">
                                        <td style="width: 100%">
                                            <table border="0" cellpadding="2" cellspacing="0" style="width: 100%;">
                                                <%--<tr style="background-color: #DADADA; height: 17px;">
                                                <td>
                                                    <asp:Label ID="lbl_TransDt" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_TransDt %>"></asp:Label>
                                                </td>
                                            </tr>--%>
                                                <tr>
                                                    <td>
                                                        <table id="chk" border="0" cellpadding="1" cellspacing="6" width="100%">
                                                            <tr style="height: 17px; vertical-align: top">
                                                                <td style="width: 7%" class="TD_LINE_GRD">
                                                                    <asp:Label ID="lbl_OnHand_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_OnHand_Nm %>"
                                                                        SkinID="LBL_HD_GRD"></asp:Label>
                                                                </td>
                                                                <td align="right" style="width: 8%" class="TD_LINE_GRD">
                                                                    <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                                <td style="width: 9%; padding-left: 5px;" class="TD_LINE_GRD">
                                                                    <asp:Label ID="lbl_OnOrder_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_OnOrder_Nm %>"
                                                                        SkinID="LBL_HD_GRD"></asp:Label>
                                                                </td>
                                                                <td align="right" style="width: 8%" class="TD_LINE_GRD">
                                                                    <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                                <td style="width: 8%; padding-left: 5px;" class="TD_LINE_GRD">
                                                                    <asp:Label ID="lbl_Reorder_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_Reorder_Nm %>"
                                                                        SkinID="LBL_HD_GRD"></asp:Label>
                                                                </td>
                                                                <td align="right" style="width: 8%" class="TD_LINE_GRD">
                                                                    <asp:Label ID="lbl_Reorder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                                <td style="width: 8%; padding-left: 5px" class="TD_LINE_GRD">
                                                                    <asp:Label ID="lbl_Restock_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_Restock_Nm %>"
                                                                        SkinID="LBL_HD_GRD"></asp:Label>
                                                                </td>
                                                                <td align="right" style="width: 8%" class="TD_LINE_GRD">
                                                                    <asp:Label ID="lbl_Restock" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                                <td style="width: 8%; padding-left: 5px;" class="TD_LINE_GRD">
                                                                    <asp:Label ID="lbl_LastPrice_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_LastPrice_Nm %>"
                                                                        SkinID="LBL_HD_GRD"></asp:Label>
                                                                </td>
                                                                <td align="right" style="width: 8%" class="TD_LINE_GRD">
                                                                    <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                                <td style="width: 8%; padding-left: 5px; white-space: nowrap;" class="TD_LINE_GRD">
                                                                    <asp:Label ID="lbl_LastVendor_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_LastVendor_Nm %>"
                                                                        SkinID="LBL_HD_GRD"></asp:Label>
                                                                </td>
                                                                <td style="width: 12%; white-space: nowrap; overflow: hidden" class="TD_LINE_GRD">
                                                                    <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 17px; vertical-align: top">
                                                                <td class="TD_LINE_GRD">
                                                                    <asp:Label ID="lbl_Category_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_Category_Nm %>"></asp:Label>
                                                                </td>
                                                                <td class="TD_LINE_GRD">
                                                                    <asp:Label ID="lbl_Category" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                                <td style="padding-left: 5px;" class="TD_LINE_GRD">
                                                                    <asp:Label ID="lbl_SubCate_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_SubCate_Nm %>"></asp:Label>
                                                                </td>
                                                                <td class="TD_LINE_GRD">
                                                                    <asp:Label ID="lbl_SubCate" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                                <td style="padding-left: 5px;" class="TD_LINE_GRD">
                                                                    <asp:Label ID="lbl_ItemGroup_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_ItemGroup_Nm %>"></asp:Label>
                                                                </td>
                                                                <td class="TD_LINE_GRD">
                                                                    <asp:Label ID="lbl_ItemGroup" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                                <td style="padding-left: 5px;" class="TD_LINE_GRD">
                                                                    <asp:Label ID="lbl_BarCode_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_BarCode_Nm %>"></asp:Label>
                                                                </td>
                                                                <td class="TD_LINE_GRD">
                                                                    <asp:Label ID="lbl_BarCode" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                                <td colspan="4" class="TD_LINE_GRD">
                                                                </td>
                                                                <%--<td>
                                                                <asp:Label ID="lbl_ProductCode_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_ProductCode_Nm %>"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lbl_ProductCode" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lbl_BaseUnit_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_BaseUnit_Nm %>"
                                                                    Visible="false"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lbl_BaseUnit" runat="server" SkinID="LBL_NR_1" Visible="false"></asp:Label>
                                                            </td>--%>
                                                            </tr>
                                                            <%--<tr style="height: 17px; vertical-align: top">
                                                            
                                                            <td class="TD_LINE_GRD">
                                                                <asp:Label ID="lbl_EnglishName_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_EnglishName_Nm %>"></asp:Label>
                                                            </td>
                                                            <td class="TD_LINE_GRD" colspan="3">
                                                                <asp:Label ID="lbl_EnglishName" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 17px; vertical-align: top">
                                                            
                                                            <td class="TD_LINE_GRD">
                                                                <asp:Label ID="lbl_LocalName_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_LocalName_Nm %>"></asp:Label>
                                                            </td>
                                                            <td class="TD_LINE_GRD" colspan="3">
                                                                <asp:Label ID="lbl_LocalName" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                            </td>
                                                        </tr>--%>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <%--<td style="vertical-align: top; width: 30%;">
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr style="height: 17px; vertical-align: top">
                                                <td>--%>
                                        <%--<uc6:StockSummary ID="uc_StockSummary" runat="server" />--%>
                                        <%--/td>
                                            </tr>
                                            <tr style="visibility: hidden">
                                                <td>--%><%--Account Details--%><%-- <table border="0" cellpadding="0" cellspacing="0" class="TABLE_HD" width="100%">
                                                        <tr style="background-color: #DADADA; height: 17px; vertical-align: top;">
                                                            <td colspan="2">
                                                                <asp:Label ID="lbl_AccDt" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_AccDt %>"
                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 17px; vertical-align: top">
                                                            <td class="TD_LINE_GRD" style="width: 20%">
                                                                <asp:Label ID="lbl_NetAcc_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_NetAcc_Nm %>"></asp:Label>
                                                            </td>
                                                            <td class="TD_LINE_GRD" style="width: 80%;">
                                                                <asp:Label ID="lbl_NetAcc" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 17px; vertical-align: top">
                                                            <td class="TD_LINE_GRD" style="width: 20%">
                                                                <asp:Label ID="lbl_TaxAcc_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_TaxAcc_Nm %>"></asp:Label>
                                                            </td>
                                                            <td class="TD_LINE_GRD" style="width: 80%;">
                                                                <asp:Label ID="lbl_TaxAcc" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>--%></tr>
                                    <tr>
                                        <td>
                                            <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                                <tr style="background-color: #DADADA; height: 17px;">
                                                    <td>
                                                        <asp:Label ID="lbl_Comment_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_Comment_Nm %>"
                                                            SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%--<asp:TextBox ID="txt_Comment" runat="server" Width="100%" TextMode="MultiLine" BorderStyle="None"
                                                Height="60px" ReadOnly="True" SkinID="TXT_V1" BackColor="Transparent"></asp:TextBox>--%>
                                                        <asp:Label ID="lbl_Comment" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <uc7:StockMovement ID="uc_StockMovement" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <%--<table border="0" cellpadding="3" cellspacing="0" width="100%">
                            <tr style="background-color: #999999; color: #ffffff;">
                                <td colspan="4">
                                    <asp:Label ID="Label21" runat="server" Text="Stock Summary" SkinID="LBL_HD"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 13%">
                                    <asp:Label ID="Label22" runat="server" Text="On Hand" SkinID="LBL_HD"></asp:Label>
                                </td>
                                <td style="width: 13%">
                                    <asp:Label ID="Label24" runat="server" Text="Reorder" SkinID="LBL_HD"></asp:Label>
                                </td>
                                <td style="width: 13%">
                                    <asp:Label ID="Label39" runat="server" Text="Avg.Price" SkinID="LBL_HD"></asp:Label>
                                </td>
                                <td style="width: 13%">
                                    <asp:Label ID="Label19" runat="server" Text="Last Price" SkinID="LBL_HD"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 13%">
                                    <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR"></asp:Label>
                                </td>
                                <td style="width: 13%">
                                    <asp:Label ID="lbl_Reorder" runat="server" SkinID="LBL_NR"></asp:Label>
                                </td>
                                <td style="width: 13%">
                                    <asp:Label ID="lbl_AvgPrice" runat="server" SkinID="LBL_NR"></asp:Label>
                                </td>
                                <td style="width: 13%">
                                    <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 13%">
                                    <asp:Label ID="Label23" runat="server" Text="On Order" SkinID="LBL_HD"></asp:Label>
                                </td>
                                <td style="width: 13%">
                                    <asp:Label ID="Label25" runat="server" Text="Restock" SkinID="LBL_HD"></asp:Label>
                                </td>
                                <td style="width: 13%">
                                    <asp:Label ID="Label26" runat="server" Text="Last Vendor" SkinID="LBL_HD"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 13%">
                                    <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR"></asp:Label>
                                </td>
                                <td style="width: 13%">
                                    <asp:Label ID="lbl_Restock" runat="server" SkinID="LBL_NR"></asp:Label>
                                </td>
                                <td style="width: 13%">
                                    <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                        </table>--%>
                                <%--</asp:Panel>--%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <%--<tr style="background-color: #DADADA; height: 17px;">
                                                <td>
                                                    <asp:Label ID="lbl_TransDt" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_TransDt %>"></asp:Label>
                                                </td>
                                            </tr>--%>
    <dx:ASPxPopupControl ID="pop_ConfirmVoid" ClientInstanceName="pop_ConfirmVoid" runat="server"
        CloseAction="CloseButton" HeaderText="<%$ Resources:IN_Transfer_TrfDt, Confirm %>"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        ShowCloseButton="False">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" colspan="2" height="50px">
                            <asp:Label ID="lbl_PopConfirmVoid" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_PopConfirmVoid %>"
                                SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <%--<dx:ASPxButton ID="btn_ConfirmVoid" CausesValidation="false" runat="server" Text="Yes"
                                OnClick="btn_ConfirmVoid_Click" SkinID="BTN_N1">
                            </dx:ASPxButton>--%>
                            <asp:Button ID="btn_ConfirmVoid" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, btn_ConfirmVoid %>"
                                OnClick="btn_ConfirmVoid_Click" SkinID="BTN_V1" Width="60px" />
                        </td>
                        <td align="left">
                            <%--<dx:ASPxButton ID="btn_CancelVoid" CausesValidation="false" runat="server" Text="No" SkinID="BTN_N1">
                                <ClientSideEvents Click="function(s, e) {
	                                            pop_ConfirmVoid.Hide();
	                                            return false;
                                            }" />
                            </dx:ASPxButton>--%>
                            <asp:Button ID="btn_CancelVoid" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, btn_CancelVoid %>"
                                SkinID="BTN_V1" OnClick="btn_CancelVoid_Click" Width="60px" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_VoidSeccess" runat="server" CloseAction="CloseButton"
        HeaderText="<%$ Resources:IN_Transfer_TrfDt, Information %>" Modal="True" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" colspan="2" height="50px">
                            <asp:Label ID="lbl_PopVoidSeccess" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, lbl_PopVoidSeccess %>"
                                SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btn_Ok" runat="server" Text="<%$ Resources:IN_Transfer_TrfDt, btn_Ok %>"
                                SkinID="BTN_V1" Width="50px" OnClick="btn_Ok_Click" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
