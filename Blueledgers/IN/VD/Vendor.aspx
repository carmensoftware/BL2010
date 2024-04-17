<%@ Page Language="C#" Trace="false" AutoEventWireup="true" CodeFile="Vendor.aspx.cs" Inherits="BlueLedger.PL.IN.Vendor" MasterPageFile="~/Master/In/SkinDefault.master" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxRoundPanel"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Src="~/UserControl/AddressInformation.ascx" TagName="AddressInformation" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxControlToolkit" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="~/UserControl/AdditionalInfo/DispAdditionalInfo.ascx" TagName="DispAdditionalInfo" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Vendor/BankAccount.ascx" TagName="BankAccount" TagPrefix="uc4" %>
<%@ Register Src="~/UserControl/Vendor/ContactPerson.ascx" TagName="ContactPerson" TagPrefix="uc5" %>
<%@ Register Src="DefaultInvoice.ascx" TagName="DefaultInvoice" TagPrefix="uc6" %>
<%@ Register Src="DefaultPayment.ascx" TagName="DefaultPayment" TagPrefix="uc7" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxRatingControl"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList"
    TagPrefix="dx1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx2" %>
<%@ Register Src="~/UserControl/Comment2.ascx" TagName="Comment2" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Attach2.ascx" TagName="Attach2" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/Log2.ascx" TagName="Log2" TagPrefix="uc5" %>
<asp:Content ID="cnt_Vendor" runat="server" ContentPlaceHolderID="cph_Main">
    <script type="text/javascript" language="javascript">

        // Open Edit Additional Field page
        function lnkb_EditMisc_Click() {
            var tableid = 'D47C759E-8D22-42A4-9C61-F20050BB6DB2';
            var name = "lookup";
            var features = "toolbar=no;menubar=no;status=no"

            windowpopup = window.open('/' + appName + '/Setup/Administrator/Application/Field/FieldLookup.aspx?tableid=' + tableid + '&isadditional=true', name, features);
            windowpopup.focus();

            return false;
        }

        function Confirm() {
            if (confirm("Are you sure want to delete this vendor information?") == true) {
                return true;
            }
            else {
                return false;
            }
        }


        // Printing for gridview.
        function CallPrint(infoHeaderID, infoID, additionalInfoHeaderID, additionalInfoID, detailInfoHeaderID, detailInfoID) {
            var informationPrintContentHeader = document.getElementById(infoHeaderID);
            var informationPrintContent = document.getElementById(infoID);
            var additionalInformationPrintContentHeader = document.getElementById(additionalInfoHeaderID);
            var additionalInformationPrintContent = document.getElementById(additionalInfoID);
            var detailInformationPrintContentHeader = document.getElementById(detailInfoHeaderID);
            var detailInformationPrintContent = document.getElementById(detailInfoID);
            var windowUrl = 'about:blank';
            var uniqueName = new Date();
            var windowName = 'Print' + uniqueName.getTime();
            var printWindow = window.open(windowUrl, windowName, 'left=250,top=250,width=0,height=0');

            printWindow.document.write(informationPrintContentHeader.innerHTML + informationPrintContent.innerHTML + additionalInformationPrintContentHeader.innerHTML + additionalInformationPrintContent.innerHTML + detailInformationPrintContentHeader.innerHTML + detailInformationPrintContent.innerHTML);
            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            printWindow.close();

            return false;
        }
        // Delete process confirm message.
        function btn_Delete_Click() {
            if (confirm("Are you sure want to delete this Vendor?!") == true) {
                return true;
            }
            else {
                return false;
            }
        }

        function img_Address_Click(obj) {
            var div = document.getElementById(obj);
            var img = document.getElementById('img' + obj);

            if (div.style.display == 'none') {
                img.src = img.src.replace('show_detail_icon', 'hide_detail_icon');
                div.style.display = ''
            }
            else {
                img.src = img.src.replace('hide_detail_icon', 'show_detail_icon');
                div.style.display = 'none'
            }
        }

        function img_ExpandCollapse_Click(obj, tblID) {
            var tbl_Content = document.getElementById(tblID);

            if (tbl_Content.style.display == 'none') {
                obj.src = obj.src.replace('expand', 'collapse');
                tbl_Content.style.display = ''
            }
            else {
                obj.src = obj.src.replace('collapse', 'expand');
                tbl_Content.style.display = 'none'
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
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr style="background-color: #4d4d4d; height: 17px;">
            <td align="left" style="padding-left: 10px;">
                <%--title bar--%>
                <table border="0" cellpadding="2" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                        </td>
                        <td>
                            <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:In_Vd_Vendor, lbl_Title %>" SkinID="LBL_HD_WHITE"> </asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
            <td align="right">
                <dx:ASPxMenu ID="menu_CmdBar" runat="server" AutoPostBack="True" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle"
                    Height="16px" OnItemClick="menu_CmdBar_ItemClick">
                    <ItemStyle BackColor="Transparent">
                        <HoverStyle BackColor="Transparent">
                            <Border BorderStyle="None" />
                        </HoverStyle>
                        <Paddings Padding="2px" />
                        <Border BorderStyle="None" />
                    </ItemStyle>
                    <Items>
                        <dx:MenuItem Text="" Name="Create" ToolTip="Create">
                            <ItemStyle Height="16px" Width="49px">
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/create.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                            </ItemStyle>
                        </dx:MenuItem>
                        <dx:MenuItem Name="Edit" Text="" ToolTip="Edit">
                            <ItemStyle Height="16px" Width="38px">
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-edit.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/edit.png" Repeat="NoRepeat" VerticalPosition="center" />
                            </ItemStyle>
                        </dx:MenuItem>
                        <dx:MenuItem Name="Delete" Text="" ToolTip="Delete">
                            <ItemStyle Height="16px" Width="47px">
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                            </ItemStyle>
                        </dx:MenuItem>
                        <dx:MenuItem Name="Print" Text="" ToolTip="Print">
                            <ItemStyle Height="16px" Width="43px">
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-print.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                            </ItemStyle>
                        </dx:MenuItem>
                        <dx:MenuItem Name="Back" ToolTip="Back" Text="">
                            <ItemStyle Height="16px" Width="42px">
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
    <div class="printable">
        <!-- Vendor Header -->
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="TABLE_HD">
            <tr>
                <td rowspan="6" style="width: 1%;">
                </td>
                <td>
                    <asp:Label ID="lbl_Vendor_Nm" runat="server" Text="<%$ Resources:In_Vd_Vendor, lbl_Vendor_Nm %>" SkinID="LBL_HD"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbl_Vendor" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbl_AccountVendor" runat="server" SkinID="LBL_HD" Text="<%$ Resources:In_Vd_Vendor, lbl_AccountVendor %>"> </asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbl_SunVendorCode" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbl_IsVoid_Nm" runat="server" Text="<%$ Resources:In_Vd_Vendor, lbl_IsVoid_Nm %>" SkinID="LBL_HD"></asp:Label>
                </td>
                <td>
                    <asp:Image ID="img_Status" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_VendorName_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:In_Vd_Vendor, lbl_VendorName_Nm %>"> </asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbl_VendorName" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbl_Reg_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:In_Vd_Vendor, lbl_Reg_Nm %>"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbl_Reg" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbl_Rating_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:In_Vd_Vendor, lbl_Rating_Nm %>"></asp:Label>
                </td>
                <td align="left">
                    <dx:ASPxRatingControl ID="rc_Rating" runat="server" ReadOnly="True">
                    </dx:ASPxRatingControl>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_TaxID_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:In_Vd_Vendor, lbl_TaxID_Nm %>"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbl_TaxID" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbl_TaxBranchID_Nm" runat="server" SkinID="LBL_HD" Text="Tax Branch ID:"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbl_TaxBranchID" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbl_Category_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:In_Vd_Vendor, lbl_Category_Nm %>"></asp:Label>
                </td>
                <td align="left">
                    <asp:Label ID="lbl_Category" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                    &nbsp;
                    <asp:Label ID="lbl_CategoryName" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_TaxType_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:In_Vd_Vendor, lbl_TaxType_Nm %>"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbl_TaxType" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbl_TaxRateNm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:In_Vd_Vendor, lbl_TaxRateNm %>"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbl_TaxRate" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbl_CreditTerm_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:In_Vd_Vendor, lbl_CreditTerm_Nm %>"> </asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbl_CreditTerm" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                    <asp:Label ID="lbl_CreditTerm_Days" runat="server" SkinID="LBL_HD" Text="<%$ Resources:In_Vd_Vendor, lbl_CreditTerm_Days %>"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                </td>
                <td>
                    <asp:Label ID="lbl_DiscountRate_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:In_Vd_Vendor, lbl_DiscountRate_Nm %>"> </asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbl_DiscountRate" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbl_DiscountTerm_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:In_Vd_Vendor, lbl_DiscountTerm_Nm %>"> </asp:Label>
                </td>
                <td>
                    <asp:Label ID="lbl_DiscountTerm" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                    <asp:Label ID="lbl_DiscountTerm_Days" runat="server" SkinID="LBL_HD" Text="<%$ Resources:In_Vd_Vendor, lbl_CreditTerm_Days %>"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_Description_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:In_Vd_Vendor, lbl_Description_Nm %>"> </asp:Label>
                </td>
                <td colspan="5">
                    <asp:Label ID="lbl_Description" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
            </tr>
        </table>
        <!-- Contact -->
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr style="background-color: #4D4D4D; height: 17px;">
                <td style="padding-left: 10px">
                    <asp:Label ID="lbl_Title2" runat="server" Text="<%$ Resources:In_Vd_Vendor, lbl_Title2 %>" SkinID="LBL_HD_WHITE"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <uc5:ContactPerson ID="ContactPerson" runat="server" Visible="true" GridHeaderCssClass="GL_GRD" />
                </td>
            </tr>
        </table>
        <br />
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr style="background-color: #4D4D4D; height: 17px;">
                <td style="padding-left: 10px">
                    <asp:Label ID="lbl_Title3" runat="server" Text="<%$ Resources:In_Vd_Vendor, lbl_Title3 %>" SkinID="LBL_HD_WHITE"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <uc3:AddressInformation ID="AddressInformation" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    <br />
    <dx:ASPxPopupControl ID="pop_ConfirmDelete" runat="server" Width="300px" CloseAction="CloseButton" HeaderText="<%$ Resources:In_Vd_Vendor, MsgHD %>" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" colspan="2" height="50px">
                            <asp:Label ID="Label60" runat="server" Text="<%$ Resources:In_Vd_Vendor, MsgWarning %>" SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <%--<dx:ASPxButton ID="btn_Yes" runat="server" Text="Yes" OnClick="btn_Yes_Click" 
                                Width="50px">
                            </dx:ASPxButton>--%>
                            <asp:Button ID="btn_Yes" runat="server" Text="<%$ Resources:In_Vd_Vendor, btn_Yes %>" OnClick="btn_Yes_Click" Width="50px" SkinID="BTN_V1" />
                        </td>
                        <td align="left">
                            <%--<dx:ASPxButton ID="btn_No" runat="server" Text="No" OnClick="btn_No_Click" 
                                Width="50px">
                            </dx:ASPxButton>--%>
                            <asp:Button ID="btn_No" runat="server" Text="<%$ Resources:In_Vd_Vendor, btn_No %>" OnClick="btn_No_Click" Width="50px" SkinID="BTN_V1" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_WarningDelete" runat="server" Width="300px" CloseAction="CloseButton" HeaderText="<%$ Resources:In_Vd_Vendor, MsgHD %>" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" height="50px">
                            <asp:Label ID="lbl_WarningDelete" runat="server" Text="<%$ Resources:In_Vd_Vendor, MsgWarning2 %>" SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btn_Ok" runat="server" OnClick="btn_Ok_Click" Text="<%$ Resources:In_Vd_Vendor, btn_Ok %>" Width="50px" SkinID="BTN_V1" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle HorizontalAlign="Left" />
    </dx:ASPxPopupControl>


</asp:Content>
