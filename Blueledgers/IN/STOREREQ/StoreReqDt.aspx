<%@ Page AutoEventWireup="true" CodeFile="StoreReqDt.aspx.cs" Inherits="BlueLedger.PL.IN.STOREREQ.StoreReqDt" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Src="~/UserControl/WFControl.ascx" TagName="WFControl" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Comment2.ascx" TagName="Comment2" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/Attach2.ascx" TagName="Attach2" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Log2.ascx" TagName="Log2" TagPrefix="uc4" %>
<%@ Register Src="~/UserControl/ProcessStatus.ascx" TagName="ProcessStatus" TagPrefix="uc5" %>
<%@ Register Src="~/PC/StockSummary.ascx" TagName="StockSummary" TagPrefix="uc6" %>
<%@ Register Src="~/PC/StockMovement.ascx" TagName="StockMovement" TagPrefix="uc7" %>
<%@ Register Src="~/UserControl/workflow/ProcessStatusDt.ascx" TagName="ProcessStatusDt" TagPrefix="uc8" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cph_Main">
    <script type="text/javascript">
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
    <asp:Label runat="server" ID="lbl_Test" Font-Size="XX-Large" Text="TEST" Visible="false" />
    <dx:ASPxPanel ID="pnl_WFLegend" runat="server">
        <PanelCollection>
            <dx:PanelContent ID="PanelContent2" runat="server">
                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                    <tr style="height: 17px">
                        <td>
                            <table border="0" cellpadding="1" cellspacing="0">
                                <tr style="height: 17px;">
                                    <td>
                                        <asp:Label ID="lbl_ProcessStatus" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_ProcessStatus %>" SkinID="LBL_NR_BI"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DataList ID="dl_ProcessStatus" runat="server" RepeatDirection="Horizontal" EnableViewState="False">
                                            <ItemStyle Font-Italic="True" />
                                            <ItemTemplate>
                                                <%#DataBinder.Eval(Container.DataItem,"Step").ToString()%>.<%#DataBinder.Eval(Container.DataItem,"StepDesc").ToString()%>&nbsp;&nbsp;
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
                                        <asp:Image ID="img_Pending" runat="server" ImageUrl="~/App_Themes/Default/Images/WF/NA.gif" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_Pending" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_Pending %>" SkinID="LBL_NR_I"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_Approve" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_Approve %>" SkinID="LBL_NR_I"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Image ID="img_PartialApprove" runat="server" ImageUrl="~/App_Themes/Default/Images/WF/PAR.gif" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_PartialApprove" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_PartialApprove %>" SkinID="LBL_NR_I"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Image ID="img_Reject" runat="server" ImageUrl="~/App_Themes/Default/Images/WF/REJ.gif" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_Reject" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_Reject %>" SkinID="LBL_NR_I"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxPanel>
    <div style="background-color: #4d4d4d; height: 28px; width: 100%;">
        <div style="display: inline-block; padding-top: 5px;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
            <asp:Label ID="lbl_StoreReq_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_StoreReq_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
            <asp:Label ID="lbl_Process" runat="server" SkinID="LBL_HD_WHITE"></asp:Label>
        </div>
        <div style="display: inline-block; float: right;">
            <table>
                <tr>
                    <td>
                        <dx:ASPxButton ID="btn_Create" runat="server" Height="16px" Width="49px" OnClick="btn_Create_Click" ToolTip="Create" BackColor="Transparent">
                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/create.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                            <HoverStyle>
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                            </HoverStyle>
                            <Border BorderStyle="None" />
                        </dx:ASPxButton>
                    </td>
                    <td>
                        <dx:ASPxButton ID="btn_Edit" runat="server" OnClick="btn_Edit_Click" BackColor="Transparent" Height="16px" Width="38px" ToolTip="Edit">
                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/edit.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                            <HoverStyle>
                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-edit.png" Repeat="NoRepeat" VerticalPosition="center" />
                            </HoverStyle>
                            <Border BorderStyle="None" />
                        </dx:ASPxButton>
                    </td>
                    <td>
                        <dx:ASPxButton ID="btn_Void" runat="server" OnClick="btn_Void_Click" BackColor="Transparent" Height="16px" Width="41px" ToolTip="Void">
                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/void.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                            <HoverStyle>
                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-void.png" Repeat="NoRepeat" VerticalPosition="center" />
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
                        <dx:ASPxButton ID="btn_Back" runat="server" OnClick="btn_Back_Click" BackColor="Transparent" Height="16px" Width="42px" ToolTip="Back">
                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                            <HoverStyle>
                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png" Repeat="NoRepeat" VerticalPosition="center" />
                            </HoverStyle>
                            <Border BorderStyle="None" />
                        </dx:ASPxButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <br style="clear: both;" />
    <div class="printable">
        <table class="TABLE_HD" width="100%">
            <tr>
                <td style="width: 90px;">
                    <asp:Label ID="lbl_Ref_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt,lbl_Ref_Nm %>" SkinID="LBL_HD"></asp:Label>
                </td>
                <td style="width: 15%;">
                    <asp:Label ID="lbl_Ref" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
                <td style="width: 40px;">
                    <asp:Label ID="lbl_Date_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_Date_Nm %>" SkinID="LBL_HD"></asp:Label>
                </td>
                <td style="width: 80px;">
                    <asp:Label ID="lbl_Date" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
                <td style="width: 60px;">
                    <asp:Label ID="lbl_ReqFrom_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt,lbl_ReqFrom_Nm %>" SkinID="LBL_HD"></asp:Label>
                </td>
                <td style="width: 30%;">
                    <asp:Label ID="lbl_RequestTo" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                    <asp:Label ID="lbl_StoreName" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
                <td style="width: 60px">
                    <asp:Label ID="lbl_Process_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt,lbl_Process_Nm %>" SkinID="LBL_HD"></asp:Label>
                </td>
                <td style="width: 10%;">
                    <uc5:ProcessStatus ID="ProcessStatus" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_Move_Type %>" SkinID="LBL_HD"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:Label ID="lbl_Move_Type" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbl_Project_Ref_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_Project_Ref %>" SkinID="LBL_HD"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbl_Project_Ref" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbl_Status_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_Status_Nm %>" SkinID="LBL_HD"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbl_Status" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_Desc_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_Desc_Nm %>" SkinID="LBL_HD"></asp:Label>
                </td>
                <td colspan="7">
                    <asp:Label ID="lbl_Desc" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:GridView ID="grd_StoreReqDt" runat="server" AutoGenerateColumns="False" Width="100%" SkinID="GRD_V1" EmptyDataText="No Data to Display" OnRowDataBound="grd_StoreReqDt_RowDataBound"
                    OnRowCommand="grd_StoreReqDt_RowCommand">
                    <Columns>
                        <%--Expand Button--%>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="Img_Btn" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/Plus.jpg" OnClientClick="expandDetailsInGrid(this); return false;" />
                            </ItemTemplate>
                            <HeaderStyle Width="10px" />
                            <ItemStyle HorizontalAlign="Left" Width="10px" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <%--Check Box--%>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chk_All" runat="server" onclick="Check(this)" SkinID="CHK_V1" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chk_Item" runat="server" SkinID="CHK_V1" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="10px" />
                            <ItemStyle HorizontalAlign="Left" Width="10px" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <%-- Main Information --%>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <table width="100%">
                                    <tr>
                                        <td align="left" style="width: 16%; padding-right: 5px;">
                                            <asp:Label ID="lbl_StoreName_Issue_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_StoreName_Issue_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 30%; padding-right: 5px;">
                                            <asp:Label ID="lbl_Product_Issue_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_Product_Issue_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 4%; padding-right: 5px;">
                                            <asp:Label ID="lbl_Unit_Issue_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_Unit_Issue_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                        </td>
                                        <td align="right" style="width: 9%; padding-right: 5px;">
                                            <asp:Label ID="lbl_QtyReq_Issue_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_QtyReq_Issue_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                        </td>
                                        <td align="right" style="width: 9%; padding-right: 5px;">
                                            <asp:Label ID="lbl_Approve_Appr_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_Approve_Appr_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                        </td>
                                        <td align="right" style="width: 9%; padding-right: 5px;">
                                            <asp:Label ID="lbl_Allocate_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_QtyAllocate_Allocate_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                        <td align="right" style="width: 9%; padding-right: 5px;">
                                            <asp:Label ID="lbl_UnitCost_Nm" runat="server" Text="Cost/Unit" SkinID="LBL_HD_W"></asp:Label>
                                        </td>
                                        <td align="right" style="width: 9%; padding-right: 5px;">
                                            <asp:Label ID="lbl_Total_Nm" runat="server" Text="Total" SkinID="LBL_HD_W"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 5%">
                                            <asp:Label ID="lbl_ReqDate_Issue_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_ReqDate_Issue_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <table width="100%">
                                    <tr>
                                        <td align="left" style="width: 16%; padding-right: 5px;">
                                            <asp:Label ID="lbl_StoreName_Issue" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 30%; padding-right: 5px;">
                                            <asp:Label ID="lbl_EnglishName_Issue" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 4%; padding-right: 5px;">
                                            <asp:Label ID="lbl_Unit_Issue" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                        </td>
                                        <td align="right" style="width: 9%; padding-right: 5px;">
                                            <asp:Label ID="lbl_QtyRequest_Issue" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                        </td>
                                        <td align="right" style="width: 9%; padding-right: 5px;">
                                            <asp:Label ID="lbl_QtyAppr_Appr" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                        </td>
                                        <td align="right" style="width: 9%; padding-right: 5px;">
                                            <asp:Label ID="lbl_QtyAllocated" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                        </td>
                                        <td align="right" style="width: 9%; padding-right: 5px;">
                                            <asp:Label ID="lbl_UnitCost" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                        </td>
                                        <td align="right" style="width: 9%; padding-right: 5px;">
                                            <asp:Label ID="lbl_Total" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 5%">
                                            <asp:Label ID="lbl_DeliveryDate_Issue" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <FooterTemplate>
                                <table width="100%">
                                    <tr>
                                        <td align="left" style="width: 16%; padding-right: 5px;">
                                        </td>
                                        <td align="left" style="width: 30%; padding-right: 5px;">
                                        </td>
                                        <td align="left" style="width: 4%; padding-right: 5px;">
                                        </td>
                                        <td align="right" style="width: 9%; padding-right: 5px;">
                                        </td>
                                        <td align="right" style="width: 9%; padding-right: 5px;">
                                        </td>
                                        <td align="right" style="width: 9%; padding-right: 5px;">
                                        </td>
                                        <td align="right" style="width: 9%; padding-right: 5px;">
                                        </td>
                                        <td align="right" style="width: 9%; padding-right: 5px;">
                                            <asp:Label ID="lbl_Total" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 5%">
                                        </td>
                                    </tr>
                                </table>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <%--Workflow Status Bar--%>
                        <asp:TemplateField HeaderText="<%$ Resources:IN_STOREREQ_StoreReqDt, ProcessStatus %>">
                            <ItemTemplate>
                                <uc8:ProcessStatusDt ID="ProcessStatusDt" runat="server" Module="IN" SubModule="SR" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="60px" />
                            <ItemStyle HorizontalAlign="Left" Width="60px" />
                        </asp:TemplateField>
                        <%--Footer Information--%>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <tr id="TR_Summmary" runat="server" style="display: none">
                                    <td colspan="9" style="padding-bottom: 20px;">
                                        <div style="width: 100%;">
                                            <table id="chk" border="0" cellpadding="1" cellspacing="6" width="100%">
                                                <tr style="height: 17px; vertical-align: top">
                                                    <td style="width: 10%" class="TD_LINE_GRD">
                                                        <asp:Label ID="lbl_OnHand_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_OnHand_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td style="width: 10%" align="right" class="TD_LINE_GRD">
                                                        <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td style="width: 10%; padding-left: 5px;" class="TD_LINE_GRD">
                                                        <asp:Label ID="lbl_OnHand_Req_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_OnHand_Req_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td style="width: 10%" align="right" class="TD_LINE_GRD">
                                                        <asp:Label ID="lbl_OnHand_Req" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td style="width: 10%;" class="TD_LINE_GRD">
                                                        <asp:Label ID="lbl_OnOrder_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_OnOrder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td style="width: 10%" align="right" class="TD_LINE_GRD">
                                                        <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td style="width: 10%;" class="TD_LINE_GRD">
                                                        <asp:Label ID="lbl_LastPrice_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_LastPrice_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td style="width: 10%" align="right" class="TD_LINE_GRD">
                                                        <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td style="width: 10%;" class="TD_LINE_GRD">
                                                        <asp:Label ID="lbl_BarCode_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_BarCode_Nm %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 10%;" class="TD_LINE_GRD">
                                                        <asp:Label ID="lbl_BarCode" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr style="height: 17px; vertical-align: top">
                                                    <td class="TD_LINE_GRD">
                                                        <asp:Label ID="lbl_Category_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_Category_Nm %>"></asp:Label>
                                                    </td>
                                                    <td class="TD_LINE_GRD">
                                                        <asp:Label ID="lbl_Category" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td style="padding-left: 5px;" class="TD_LINE_GRD">
                                                        <asp:Label ID="lbl_SubCate_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_SubCate_Nm %>"></asp:Label>
                                                    </td>
                                                    <td class="TD_LINE_GRD">
                                                        <asp:Label ID="lbl_SubCate" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td style="padding-left: 5px;" class="TD_LINE_GRD">
                                                        <asp:Label ID="lbl_ItemGroup_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_ItemGroup_Nm %>"></asp:Label>
                                                    </td>
                                                    <td class="TD_LINE_GRD">
                                                        <asp:Label ID="lbl_ItemGroup" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                    <td style="padding-left: 5px;" class="TD_LINE_GRD">
                                                        <asp:Label ID="lbl_LastVendor_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_LastVendor_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                    <td colspan="3" class="TD_LINE_GRD">
                                                        <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                                <tr style="background-color: #DADADA; height: 17px;">
                                                    <td>
                                                        <asp:Label ID="lbl_Comment_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_Comment_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_Comment" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <uc7:StockMovement ID="uc_StockMovement" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div style="width: 100%;">
                    <div style="display: inline-block; float: left; width: 60%;">
                        <asp:Label ID="lbl_ApproveMessage" runat="server" Text="" />
                    </div>
                    <div style="display: inline-block; float: right;">
                        <asp:Panel ID="panel_WFControl" runat="server">
                            <div style="text-align: left; width: 205px;">
                                <uc1:WFControl ID="WFControlPanel" runat="server" />
                            </div>
                        </asp:Panel>
                    </div>
                </div>
                <br style="clear: both;" />
                <dx:ASPxPopupControl ID="pop_ConfirmVoid" ClientInstanceName="pop_ConfirmVoid" runat="server" CloseAction="CloseButton" HeaderText="<%$ Resources:IN_STOREREQ_StoreReqDt, Confirm %>"
                    Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" colspan="2" height="50px">
                                        <asp:Label ID="lbl_PopConfirmVoid" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_PopConfirmVoid %>" SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btn_ConfirmVoid" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, btn_ConfirmVoid %>" OnClick="btn_ConfirmVoid_Click" SkinID="BTN_V1"
                                            Width="60px" />
                                    </td>
                                    <td align="left">
                                        <asp:Button ID="btn_CancelVoid" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, btn_CancelVoid %>" SkinID="BTN_V1" OnClick="btn_CancelVoid_Click"
                                            Width="60px" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="pop_VoidSeccess" runat="server" CloseAction="CloseButton" HeaderText="<%$ Resources:IN_STOREREQ_StoreReqDt, Information %>" Modal="True"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" colspan="2" height="50px">
                                        <asp:Label ID="lbl_PopVoidSeccess" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_PopVoidSeccess %>" SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btn_Ok" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, btn_Ok %>" SkinID="BTN_V1" Width="50px" OnClick="btn_Ok_Click" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
