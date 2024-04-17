<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VendorEdit.aspx.cs" Inherits="BlueLedger.PL.IN.VendorEdit" MasterPageFile="~/Master/In/SkinDefault.master" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Reference VirtualPath="~/UserControl/AdditionalInfo/EditAdditionalInfo.ascx" %>
<%@ Register Src="~/UserControl/AddressInformation.ascx" TagName="AddressInformation" TagPrefix="uc24" %>
<%@ Register Src="~/UserControl/Vendor/ContactPerson.ascx" TagName="ContactPerson" TagPrefix="uc26" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxRatingControl"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
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
        function confirm_delete() {
            if (confirm("Are you sure you want to delete?") == true) {
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
                img.src = img.src.replace('show', 'hide');
                div.style.display = ''
            }
            else {
                img.src = img.src.replace('hide', 'show');
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
    <asp:UpdatePanel ID="UdPnVendor" runat="server">
        <ContentTemplate>
            <!-- Header Bar -->
            <div>
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
                                    <dx:MenuItem Name="Save" ToolTip="Save" Text="">
                                        <ItemStyle Height="16px" Width="42px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png" Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/save.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
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
                <br />
            </div>
            <!-- Vendor Header -->
            <div>
                <table border="0" cellpadding="3" cellspacing="0" width="100%" class="TABLE_HD">
                    <tr style="vertical-align: baseline;">
                        <td>
                            <asp:Label ID="lbl_Vendor_Nm" runat="server" Text="<%$ Resources:In_Vd_Vendor, lbl_Vendor_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Vendor" runat="server" Style="border: none; background-color: transparent; font-weight: bold;" SkinID="TXT_V1" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lbl_AccountVendor" runat="server" SkinID="LBL_HD" Text="<%$ Resources:In_Vd_Vendor, lbl_AccountVendor %>"> </asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_SunVendorCode" runat="server" Width="200px" SkinID="TXT_V1"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lbl_IsVoid_Nm" runat="server" Text="<%$ Resources:In_Vd_Vendor, lbl_IsVoid_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chk_Status" runat="server" SkinID="CHK_V1" />
                        </td>
                    </tr>
                    <tr style="vertical-align: baseline;">
                        <td>
                            <asp:Label ID="lbl_VendorName_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:In_Vd_Vendor, lbl_VendorName_Nm %>"> </asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_VendorName" runat="server" SkinID="TXT_V1" Width="280px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txt_VendorName" runat="server" ErrorMessage="* Required" ForeColor="Red" Font-Size="Small"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Label ID="lbl_Reg_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:In_Vd_Vendor, lbl_Reg_Nm %>"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Reg" runat="server" Width="200px" SkinID="TXT_V1"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lbl_Rating_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:In_Vd_Vendor, lbl_Rating_Nm %>"></asp:Label>
                        </td>
                        <td>
                            <dx:ASPxRatingControl ID="rc_Rating" runat="server" />
                        </td>
                    </tr>
                    <tr style="vertical-align: baseline;">
                        <td>
                            <asp:Label ID="lbl_TaxID_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:In_Vd_Vendor, lbl_TaxID_Nm %>"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_TaxID" runat="server" Width="280px" SkinID="TXT_V1"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lbl_TaxBranchID_Nm" runat="server" SkinID="LBL_HD" Text="Tax Branch ID:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_TaxBranchID" runat="server" Width="200px" SkinID="TXT_V1"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lbl_Category_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:In_Vd_Vendor, lbl_Category_Nm %>"></asp:Label>
                        </td>
                        <td>
                            <dx:ASPxComboBox ID="cmb_VendorCategory" runat="server" EnableIncrementalFiltering="True" CallbackPageSize="10" IncrementalFilteringMode="Contains" ValueType="System.String"
                                ValueField="VendorCategoryCode" OnItemsRequestedByFilterCondition="cmb_VendorCategory_OnItemsRequestedByFilterCondition" OnItemRequestedByValue="cmb_VendorCategory_OnItemRequestedByValue"
                                TextFormatString="{0}" TextField="Name" Width="200px" Height="19px" SkinID="DDL_V1">
                                <Columns>
                                    <dx:ListBoxColumn FieldName="Name" />
                                </Columns>
                            </dx:ASPxComboBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="cmb_VendorCategory" runat="server" ErrorMessage="* Required" ForeColor="Red"
                                Font-Size="Small"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr style="vertical-align: baseline;">
                        <td>
                            <asp:Label ID="lbl_TaxType_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:In_Vd_Vendor, lbl_TaxType_Nm %>"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_TaxType" runat="server" Width="280px" Height="19px" SkinID="DDL_V1">
                                <asp:ListItem Text="None" Value="N"></asp:ListItem>
                                <asp:ListItem Text="Included" Value="I"></asp:ListItem>
                                <asp:ListItem Text="Add" Value="A"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_TaxRateNm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:In_Vd_Vendor, lbl_TaxRateNm %> "></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_TaxRate" runat="server" Width="200px" SkinID="TXT_NUM_V1"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lbl_CreditTerm_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:In_Vd_Vendor, lbl_CreditTerm_Nm %>"> </asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_CreditTerm" runat="server" Width="200px" SkinID="TXT_V1"></asp:TextBox>
                            <asp:Label ID="lbl_CreditTerm_Days" runat="server" SkinID="LBL_HD" Text="<%$ Resources:In_Vd_Vendor, lbl_CreditTerm_Days %>"></asp:Label>
                        </td>
                    </tr>
                    <tr style="vertical-align: baseline;">
                        <td colspan="2">
                        </td>
                        <td>
                            <asp:Label ID="lbl_DiscountRate_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:In_Vd_Vendor, lbl_DiscountRate_Nm %>"> </asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_DiscountRate" runat="server" Width="200px" SkinID="TXT_NUM_V1"></asp:TextBox>
                            <%--&nbsp;<asp:Label ID="Label14" runat="server" SkinID="LBL_HD" Text="%"></asp:Label>--%>
                            &nbsp;<asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="txt_DiscountRate" ForeColor="Red" Font-Size="Small" ErrorMessage="*Must Be Greater Than Zero"
                                OnServerValidate="ValidateDiscountRate"></asp:CustomValidator>
                        </td>
                        <td>
                            <asp:Label ID="lbl_DiscountTerm_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:In_Vd_Vendor, lbl_DiscountTerm_Nm %>"> </asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_DiscountTerm" runat="server" Width="200px" SkinID="TXT_V1"></asp:TextBox>
                            &nbsp;<asp:Label ID="lbl_DiscountTerm_Days" runat="server" SkinID="LBL_HD" Text="<%$ Resources:In_Vd_Vendor, lbl_CreditTerm_Days %>"></asp:Label>
                        </td>
                    </tr>
                    <tr style="vertical-align: top;">
                        <td>
                            <asp:Label ID="lbl_Description_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:In_Vd_Vendor, lbl_Description_Nm %>"> </asp:Label>
                        </td>
                        <td colspan="7">
                            <asp:TextBox ID="txt_Description" runat="server" SkinID="TXT_V1" TextMode="MultiLine" Rows="3" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <!-- Contact Information -->
            <div>
                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                    <tr style="height: 20px">
                        <td style="height: 20px">
                            <asp:Label ID="lbl_Contact" runat="server" SkinID="LBL_HD" Text="<%$ Resources:In_Vd_Vendor, lbl_Title2 %>"></asp:Label>
                        </td>
                        <td align="right" style="height: 20px">
                        </td>
                    </tr>
                </table>
            </div>
            <!-- Contact Person -->
            <div style="width: 100%;">
                <uc26:ContactPerson ID="ContactPerson" runat="server" Visible="true" gridheadercssclass="GL_GRD" />
            </div>
            <!-- Address -->
            <div>
                <table border="0" cellpadding="1" cellspacing="0" style="width: 100%">
                    <tr style="height: 20px">
                        <td>
                            <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                <tr style="height: 20px">
                                    <td>
                                        <asp:Label ID="lbl_Address" runat="server" SkinID="LBL_HD" Text="<%$ Resources:In_Vd_Vendor, lbl_Title3 %>"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color: #F2F2F2">
                            <uc24:AddressInformation ID="AddressInformation" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <!-- Popup -->
            <dx:ASPxPopupControl ID="pop_AlertTaxRate" runat="server" HeaderText="<%$ Resources:In_Vd_Vendor, MsgHD %>" Modal="True" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Width="225px" CloseAction="CloseButton">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" colspan="2" style="height: 20px">
                                    <asp:Label ID="Label38" runat="server" Text="<%$ Resources:In_Vd_Vendor, MsgWarning3 %>" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <%--<dx:ASPxButton ID="btn_OK" runat="server" OnClick="btn_OK_Click" Text="OK">
                            </dx:ASPxButton>--%>
                                    <asp:Button ID="btn_OK" runat="server" OnClick="btn_OK_Click" Text="<%$ Resources:In_Vd_Vendor, btn_Ok %>" Width="50px" SkinID="BTN_V1" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_Warning" runat="server" HeaderText="<%$ Resources:In_Vd_Vendor, MsgHD %>" Modal="True" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Width="300px" CloseAction="CloseButton">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" colspan="2" style="height: 20px">
                                    <asp:Label ID="lbl_Warning" runat="server" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btn_Warning" runat="server" Text="<%$ Resources:In_Vd_Vendor, btn_Ok %>" Width="50px" SkinID="BTN_V1" OnClick="btn_Warning_Click" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress2" PopupControlID="UpdateProgress2" BackgroundCssClass="POPUP_BG"
                RepositionMode="RepositionOnWindowResizeAndScroll">
            </ajaxToolkit:ModalPopupExtender>
            <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UdPnVendor">
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
