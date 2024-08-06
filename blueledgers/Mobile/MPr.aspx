<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MPr.aspx.cs" Inherits="BlueLedger.PL.Mobile.PrDt" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <style type="text/css">
        body
        {
            font-size: xx-large;
            font-family: Arial;
            background-color: #dcdcdc;
        }
        .header
        {
            width: 100%;
            position: fixed;
            top: 0px;
            left: 0px;
            background-color: #FFF;
            opacity: 1.0;
            z-index: 1;
        }
        .cbScaleR
        {
            width: 50px;
            height: 50px;
        }
        
        .circle
        {
            width: 105px;
            height: 105px;
            -webkit-border-radius: 50px;
            -moz-border-radius: 50px;
            border-radius: 50px;
            background: #4D4D4D;
            padding-top: 3px;
            padding-left: 1px;
            opacity: 1.0;
            box-shadow: 10px 10px 15px #808080;
            -moz-box-shadow: 5px 5px 6px #808080;
            -webkit-box-shadow: 5px 5px 15px #808080; /* col. 3 = box blur*/
            -moz-border-radius: 100px;
            -webkit-border-radius: 100px;
        }
        
        #triangle-up
        {
            margin-top: 20px;
            width: 0;
            height: 0;
            border-left: 25px solid transparent;
            border-right: 25px solid transparent;
            border-bottom: 50px solid #FFFFFF;
        }
        #triangle-down
        {
            margin-top: 30px;
            margin-left: 25px;
            width: 0;
            height: 0;
            border-left: 25px solid transparent;
            border-right: 25px solid transparent;
            border-top: 50px solid #FFFFFF;
        }
        #detailCard
        {
            width: 100%;
            height: 60%;
            transition: margin-bottom .5s;
            position: relative;
        }
        
        .floatMenuIcon
        {
            right: 5%;
            bottom: 3%;
            position: fixed;
        }
        .lessFloatMenuIcon
        {
            right: 5%;
            bottom: 250px;
            position: fixed;
        }
        
        .footerEdit
        {
            width: 100%;
            height: 0;
            bottom: 0;
            position: fixed;
            background-color: #4D4D4D;
            z-index: 1;
            transition: 0.3s;
        }
        .footerFull
        {
            width: 100%;
            height: 0;
            bottom: 0;
            position: fixed;
            background-color: #4D4D4D;
            vertical-align: middle;
            opacity: 1.0;
            z-index: 1;
            overflow-x: hidden;
            transition: 0.3s;
        }
    </style>
    <script type="text/javascript">
        function ShowfullMenu() {
            document.getElementById('<%=btnFloatMenu.ClientID%>').click();
        }
        function HideFullMenu() {
            document.getElementById('<%= btnLessFullMenu.ClientID%>').click();
        }
        function countCBR() {
            var cbR = document.getElementsByClassName("cbScaleR");
            var totalCount = cbR.length;
            var checkCount = 0;

            for (var i = 0; i < cbR.length; i++) {
                if (cbR[i].checked) {
                    checkCount++;
                }
            }
            document.getElementById('<%=lblSelected.ClientID%>').innerHTML = '(' + checkCount + '/' + totalCount + ')';
            return checkCount;
        }

        function selectAll() {
            var count = countCBR();
            var cbR = document.getElementsByClassName("cbScaleR");

            if (count < cbR.length) {
                for (var i = 0; i < cbR.length; i++) {
                    cbR[i].checked = true;
                }
            } else {
                for (var i = 0; i < cbR.length; i++) {
                    cbR[i].checked = false;
                }
            }
            countCBR();
        }
    </script>
    <asp:ScriptManager ID="script01" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <ContentTemplate>
            <div style="height: 100%; width: 100%;">
                <div class="header">
                    <asp:Label ID="lblMsg" runat="server"></asp:Label>
                    <table width="100%" style="height: 100%; padding-top: 15px; padding-left: 15px;">
                        <tr>
                            <td style="width: 10%;">
                                <asp:Label ID="lbl_txtPRref" runat="server">PR Ref#</asp:Label>
                            </td>
                            <td style="width: 20%;">
                                <asp:Label ID="lbl_PRref" runat="server"></asp:Label>
                            </td>
                            <td style="width: 10%;">
                                <asp:Label ID="lbl_txtProcess" runat="server">Process</asp:Label>
                            </td>
                            <td style="width: 15%;">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%;">
                                <asp:Label ID="lbl_txtPRtype" runat="server">PR Type</asp:Label>
                            </td>
                            <td style="width: 20%;">
                                <asp:Label ID="lbl_Type" runat="server"></asp:Label>
                            </td>
                            <td style="width: 10%;">
                                <asp:Label ID="lbl_txtStatus" runat="server">Status</asp:Label>
                            </td>
                            <td style="width: 15%;">
                                <asp:Label ID="lbl_status" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%;">
                                <asp:Label ID="lbl_txtJobCode" runat="server">JobCode</asp:Label>
                            </td>
                            <td style="width: 20%;">
                                <asp:Label ID="lbl_jobCode" runat="server"></asp:Label>
                            </td>
                            <td style="width: 10%;">
                                <asp:Label ID="lbl_txtDate" runat="server">Date</asp:Label>
                            </td>
                            <td style="width: 15%;">
                                <asp:Label ID="lbl_Date" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:LinkButton ID="lbExpandHead" runat="server" Font-Size="XX-Large" Font-Underline="false"
                                    ForeColor="#7B7B7B" OnClick="lbExpandHead_Click">more</asp:LinkButton>
                                <asp:Panel ID="pnExpandHeader" runat="server" Style="display: none;">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 10%;">
                                                <asp:Label ID="lbl_txtReq" runat="server">Requestor</asp:Label>
                                            </td>
                                            <td style="width: 15%;">
                                                <asp:Label ID="lbl_Req" runat="server"></asp:Label>
                                            </td>
                                            <td style="width: 10%;">
                                                <asp:Label ID="lbl_txtPermit" runat="server">Permit By</asp:Label>
                                            </td>
                                            <td style="width: 15%;">
                                                <asp:Label ID="lbl_permit" runat="server"></asp:Label>
                                            </td>
                                            <td style="width: 10%;">
                                                <asp:Label ID="lbl_txtApp" runat="server">Approve By</asp:Label>
                                            </td>
                                            <td style="width: 10%;">
                                                <asp:Label ID="lbl_approve" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 10%;">
                                                <asp:Label ID="lbl_txtDesc" runat="server">Description</asp:Label>
                                            </td>
                                            <td colspan="5">
                                                <asp:Label ID="lbl_Desc" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:LinkButton ID="lbLessHead" runat="server" Font-Size="XX-Large" Font-Underline="false"
                                        ForeColor="#7B7B7B" OnClick="lbExpandHead_Click"></asp:LinkButton>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </div>
                <%-- Detail Card--%>
                <div id="detailCard" runat="server" style="height: 60%;">
                    <asp:Repeater ID="rptDetail" runat="server">
                        <HeaderTemplate>
                            <table align="left" width="97%" style="height: 100%; position: relative; margin-top: 20%;
                                margin-left: 2%; padding-top: 20%; margin-bottom: 250px; background-color: #FFF;
                                border-collapse: collapse; border-style: hidden;">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr style="height: 20%; background-color: #FFF;">
                                <td style="width: 7%; height: 100%;">
                                    <%--<asp:CheckBox ID="cbPRDt" runat="server" class="cbScaleR" Style="display: none;"
                                    OnCheckedChanged="cbPRDt_CheckedChanged" />--%>
                                    <input type="checkbox" id="cbPRDt" runat="server" class="cbScaleR" style="display: none;"
                                        onclick="countCBR()" />
                                </td>
                                <td colspan="2">
                                    <asp:Label ID="lbltxtB01" runat="server" Text='<%# GetFieldHeader(1) %>'>Block 01</asp:Label>
                                    <asp:Label ID="lblValueB01" runat="server" Text='<%# GetFieldValue(1) %>' Font-Bold="true"></asp:Label>
                                    <asp:Label ID="lbltxtB02" runat="server" Text='<%# GetFieldHeader(2) %>'>Block 02</asp:Label>
                                    <asp:Label ID="lblValueB02" runat="server" Text='<%# GetFieldValue(2) %>' Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr style="height: 20%; background-color: #FFF">
                                <td style="width: 7%; height: 100%;">
                                </td>
                                <td style="width: 45%; height: 100%;">
                                    <asp:Label ID="lbltxtB03" runat="server" Text='<%# GetFieldHeader(3) %>'>Block 03</asp:Label>
                                    <asp:Label ID="lblValueB03" runat="server" Text='<%# GetFieldValue(3) %>'></asp:Label>
                                </td>
                                <td style="width: 45%; height: 100%;">
                                    <asp:Label ID="lbltxtB04" runat="server" Text='<%# GetFieldHeader(4) %>'>Block 04</asp:Label>
                                    <asp:Label ID="lblValueB04" runat="server" Text='<%# GetFieldValue(4) %>'></asp:Label>
                                </td>
                            </tr>
                            <tr style="height: 20%; background-color: #FFF">
                                <td style="width: 7%; height: 100%;">
                                </td>
                                <td style="width: 45%; height: 100%;">
                                    <asp:Label ID="lbltxtB05" runat="server" Text='<%# GetFieldHeader(5) %>'>Block 05</asp:Label>
                                    <asp:Label ID="lblValueB05" runat="server" Text='<%# GetFieldValue(5) %>'></asp:Label>
                                </td>
                                <td style="width: 45%; height: 100%;">
                                    <asp:Label ID="lbltxtB06" runat="server" Text='<%# GetFieldHeader(6) %>'>Block 06</asp:Label>
                                    <asp:Label ID="lblValueB06" runat="server" Text='<%# GetFieldValue(6) %>'></asp:Label>
                                </td>
                            </tr>
                            <tr style="height: 20%; background-color: #FFF">
                                <td style="width: 7%; height: 100%;">
                                </td>
                                <td style="width: 45%; height: 100%;">
                                    <asp:Label ID="lbltxtB07" runat="server" Text='<%# GetFieldHeader(7) %>'>Block 07</asp:Label>
                                    <asp:Label ID="lblValueB07" runat="server" Text='<%# GetFieldValue(7) %>'></asp:Label>
                                </td>
                                <td style="width: 45%; height: 100%;">
                                    <asp:Label ID="lbltxtB08" runat="server" Text='<%# GetFieldHeader(8) %>'>Block 08</asp:Label>
                                    <asp:Label ID="lblValueB08" runat="server" Text='<%# GetFieldValue(8) %>'></asp:Label>
                                </td>
                            </tr>
                            <tr style="height: 20%; background-color: #FFF">
                                <td style="width: 7%; height: 100%;">
                                </td>
                                <td colspan="2">
                                    <div style="width: 96%;">
                                        <asp:LinkButton ID="lbExpand" runat="server" Font-Size="XX-Large" Font-Underline="false"
                                            ForeColor="#7B7B7B" OnClick="lbExpend_OnClick">more</asp:LinkButton>
                                        <%--<asp:Label ID="lblExpDesc" runat="server">More</asp:Label>--%>
                                        <asp:Panel ID="pnExpand" runat="server" Style="display: none; padding-left: 3%;">
                                            <asp:Label ID="lblExtend01" runat="server">Label in Hidden DIV.</asp:Label>
                                            <br />
                                            <asp:LinkButton ID="lbLess" runat="server" Font-Size="XX-Large" Font-Underline="false"
                                                ForeColor="#7B7B7B" OnClick="lbExpend_OnClick"></asp:LinkButton>
                                        </asp:Panel>
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <SeparatorTemplate>
                            <tr style="height: 30%">
                                <td colspan="3">
                                    <hr style="width: 100%; background-color: transparent; border: 15px solid #DCDCDC;
                                        border-collapse: collapse; left: 0px;" />
                                </td>
                            </tr>
                        </SeparatorTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
                <div style="bottom: 5%; right: 20%; position: fixed; z-index: -1;">
                    <asp:Label ID="lbl_countDetail" runat="server"></asp:Label>
                </div>
                <asp:Button ID="btnFloatMenu" runat="server" OnClick="divFloatMenu_Click" Style="display: none;" />
                <div id="divFloatMenu" runat="server" class="floatMenuIcon" align="center">
                    <div class="circle" onclick="ShowfullMenu()">
                        <div id="triangle-up">
                        </div>
                    </div>
                </div>
                <div id="divFullMenu" runat="server" class="footerFull">
                    <table width="100%">
                        <tr align="center" style="height: 75px; width: 100%; vertical-align: top;">
                            <td style="width: 33%; padding-top: 3%;">
                                <table width="100%">
                                    <tr>
                                        <td align="right" style="width: 40%;">
                                            <%--<asp:CheckBox ID="cbAll" runat="server" AutoPostBack="true" CssClass="cbScale" OnCheckedChanged="cbAll_CheckedChanged" />--%>
                                            <input type="checkbox" id="cbMenu" runat="server" style="width: 50px; height: 50px;"
                                                onclick="selectAll()" />
                                        </td>
                                        <td align="left" style="width: 60%;">
                                            <asp:LinkButton ID="lbtn_Select" runat="server" Font-Underline="false" ForeColor="White"
                                                OnClick="lbtn_Select_Click">Select All</asp:LinkButton><br />
                                            <asp:Label ID="lblSelected" runat="server" Style="color: #FFF; font-size: x-large;"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 33%; padding-top: 3%;">
                                <asp:ImageButton ID="imgBtnEdit" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/MobileEdit.png"
                                    Width="50px" Height="50px" OnClick="Edit_Click" />
                                <asp:LinkButton ID="lbtn_Edit" runat="server" Font-Underline="false" ForeColor="White"
                                    OnClick="Edit_Click">Edit</asp:LinkButton>
                                <%--OnClientClick="EditClick()"--%>
                                <%--<linkbutton id="lbtn_Edit" font-underline="false" onclientclick="EditClick()">Edit</linkbutton>--%>
                            </td>
                            <td style="width: 33%; padding-top: 3%;">
                                <%-- <asp:LinkButton ID="LinkButton1" runat="server" Font-Underline="false" ForeColor="White">Button 03</asp:LinkButton>--%>
                            </td>
                        </tr>
                        <tr align="center" style="height: 75px; width: 100%;">
                            <td style="width: 33%;">
                                <asp:ImageButton ID="imgBtn_App" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/MobileApprove.png"
                                    Width="50px" Height="50px" />
                                <asp:LinkButton ID="lbtn_App" runat="server" Font-Underline="false" ForeColor="White">Approve</asp:LinkButton>
                            </td>
                            <td style="width: 33%;">
                                <asp:ImageButton ID="imgbtn_Reject" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/MobileReject.png"
                                    Width="50px" Height="50px" />
                                <asp:LinkButton ID="lbtn_reject" runat="server" Font-Underline="false" ForeColor="White">Reject</asp:LinkButton>
                            </td>
                            <td style="width: 33%;">
                                <asp:ImageButton ID="ingBtnSB" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/MobileSendBack.png"
                                    Width="50px" Height="50px" />
                                <asp:LinkButton ID="lbtn_SendBack" runat="server" Font-Underline="false" ForeColor="White">Send Back</asp:LinkButton>
                            </td>
                        </tr>
                        <tr align="center" style="height: 50px;">
                            <td colspan="3">
                                <asp:LinkButton ID="lbtn_destop" runat="server" Font-Size="X-Large" ForeColor="White"
                                    Text="Desktop Version"></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:Button ID="btnLessFullMenu" runat="server" OnClick="divLessFullMenu_Click" Style="display: none;" />
                <div id="divLessFullMenu" runat="server" class="lessFloatMenuIcon" style="display: none;"
                    onclick="HideFullMenu()">
                    <div class="circle">
                        <div id="triangle-down">
                        </div>
                    </div>
                </div>
                <div id="divEditMode" runat="server" class="footerEdit">
                    <table width="100%" style="height: 100%; vertical-align: middle;">
                        <tr align="center" style="height: 75px;">
                            <td style="width: 50%;">
                                <asp:LinkButton ID="lbtn_save" runat="server" Font-Underline="false" ForeColor="White"
                                    OnClick="lbtn_save_Click">Save</asp:LinkButton>
                            </td>
                            <td style="width: 50%;">
                                <asp:LinkButton ID="lbtn_cancel" runat="server" Font-Underline="false" ForeColor="White"
                                    OnClick="lbtn_cancel_Click">Cancel</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
