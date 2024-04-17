<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" EnableEventValidation="false"
    CodeFile="DepartmentList.aspx.cs" Inherits="BlueLedger.PL.Option.Admin.Security.Department.DepartmentList" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <script type="text/javascript">

        //Check Select All CheckBox.
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
            if (chk.type == 'checkbox')
                return true;
            else
                return false;
        }

    </script>
    <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hf_ConnStr" runat="server" />
            <!-- Title & Menu  -->
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr style="background-color: #4d4d4d; height: 17px;">
                    <td style="padding-left: 10px; width: 20px;">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                    </td>
                    <td>
                        <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:Option_Admin_Security_Dep_Dep, lbl_Title %>" SkinID="LBL_HD_WHITE"></asp:Label>
                    </td>
                    <td align="right">
                        <dx:ASPxMenu ID="menu_CmdBar" runat="server" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle" Height="16px" OnItemClick="menu_CmdBar_ItemClick">
                            <ItemStyle BackColor="Transparent">
                                <HoverStyle BackColor="Transparent">
                                    <Border BorderStyle="None" />
                                </HoverStyle>
                                <Paddings Padding="2px" />
                                <Border BorderStyle="None" />
                            </ItemStyle>
                            <Items>
                                <dx:MenuItem Name="Create" ToolTip="Create" Text="">
                                    <ItemStyle Height="16px" Width="49px">
                                        <HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png" Repeat="NoRepeat" VerticalPosition="center" />
                                        </HoverStyle>
                                        <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/create.png" Repeat="NoRepeat" VerticalPosition="center" />
                                    </ItemStyle>
                                </dx:MenuItem>
                                <dx:MenuItem Name="Delete" ToolTip="Delete" Text="">
                                    <ItemStyle Height="16px" Width="47px">
                                        <HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png" Repeat="NoRepeat" VerticalPosition="center" />
                                        </HoverStyle>
                                        <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png" Repeat="NoRepeat" VerticalPosition="center" />
                                    </ItemStyle>
                                </dx:MenuItem>
                                <%--<dx:MenuItem Name="Print" ToolTip="Print" Text="">
                                        <ItemStyle Height="16px" Width="43px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-print.png" Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>--%>
                            </Items>
                        </dx:ASPxMenu>
                    </td>
                </tr>
            </table>
            <br />
            <%--Data List--%>
            <asp:GridView runat="server" ID="gvDepartment" Width="100%" AutoGenerateColumns="False" GridLines="Horizontal" BackColor="Transparent" BorderColor="LightGray"
                OnRowDataBound="gvDepartment_RowDataBound">
                <HeaderStyle Height="40px" BackColor="Gray" HorizontalAlign="Left" Font-Bold="True" />
                <RowStyle Height="40px" BackColor="Transparent" />
                <Columns>
                    <%--Checkbox--%>
                    <asp:TemplateField>
                        <HeaderStyle Width="40" />
                        <HeaderTemplate>
                            <asp:CheckBox ID="chk_All" runat="server" SkinID="CHK_V1" onclick="Check(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chk_Item" runat="server" SkinID="CHK_V1" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--Edit Button--%>
                    <asp:TemplateField>
                        <HeaderStyle Width="80" />
                        <ItemTemplate>
                            <asp:LinkButton ID="btn_Edit" runat="server" Text="Edit" OnClick="btn_Edit_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--Code--%>
                    <asp:TemplateField HeaderText="<%$ Resources:Option_Admin_Security_Dep_Dep, Code %>">
                        <HeaderStyle Width="100" />
                        <ItemTemplate>
                            <asp:Label ID="lbl_DepCode" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--Name--%>
                    <asp:TemplateField HeaderText="<%$ Resources:Option_Admin_Security_Dep_Dep, Name %>">
                        <HeaderStyle Width="280" />
                        <ItemTemplate>
                            <asp:Label ID="lbl_DepName" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--HOD--%>
                    <asp:TemplateField HeaderText="<%$ Resources:Option_Admin_Security_Dep_Dep, HeadOfDep %>">
                        <ItemTemplate>
                            <asp:Label ID="lbl_HOD" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--Account--%>
                    <asp:TemplateField HeaderText="Account Code">
                        <HeaderStyle Width="110" />
                        <ItemTemplate>
                            <asp:Label ID="lbl_AccCode" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--Active--%>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="<%$ Resources:Option_Admin_Security_Dep_Dep, IsActive %>">
                        <HeaderStyle HorizontalAlign="Center" Width="60" />
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:Image ID="imgActive" runat="server" Width="16px" Height="16px" />
                            <asp:HiddenField runat="server" ID="hf_IsActive" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <%--Popup--%>
            <dx:ASPxPopupControl runat="server" ID="pop_Department" ClientInstanceName="pop_Department" Width="320" Modal="True" ShowCloseButton="False" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="True">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 30%;">
                                    <asp:Label ID="lbl_DepCode_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_Dep_Dep, Code %>" />
                                </td>
                                <td style="width: 70%;">
                                    <asp:TextBox ID="txt_DepCode" runat="server" Width="100%" Style="text-transform: uppercase" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_DepName_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_Dep_Dep, Name %>" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_DepName" runat="server" Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_AccCode_Nm" runat="server" Text="Account Code" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_AccCode" runat="server" Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_Active" runat="server" Text="<%$ Resources:Option_Admin_Security_Dep_Dep, IsActive %>" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="check_Active" runat="server" Width="100%" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Label ID="lbl_HOD" runat="server" Text="<%$ Resources:Option_Admin_Security_Dep_Dep, HeadOfDep %>" />
                        <dx:ASPxListBox runat="server" ID="list_HOD" SelectionMode="CheckColumn" Height="210px" Width="100%" ValueField="LoginName" TextField="LoginName">
                        </dx:ASPxListBox>
                        <div style="padding-top: 10px;">
                            <asp:Button ID="btn_AddUser" runat="server" Text="Add" OnClick="btn_AddUser_Click" />
                            <asp:Button ID="btn_DelUser" runat="server" Text="Delete" OnClick="btn_DelUser_Click" />
                        </div>
                        <br />
                        <div style="width: 100%; text-align: right;">
                            <asp:Button ID="btn_SaveDep" runat="server" Text="Save" OnClick="btn_SaveDep_Click" />
                            <asp:Button ID="btn_CancelDep" runat="server" Text="Cancel" OnClientClick="pop_Department.Hide();" />
                            <%--<asp:Button ID="btn_CancelDep" runat="server" Text="Cancel" OnClick="btn_CancelDep_Click" />--%>
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl runat="server" ID="pop_UserList" ClientInstanceName="pop_UserList" HeaderText="User List" Width="320" Modal="True" ShowCloseButton="True"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AllowDragging="True" ShowPageScrollbarWhenModal="True">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                        <dx:ASPxListBox ID="list_SelectUser" runat="server" SelectionMode="CheckColumn" Height="420" Width="100%" ValueField="LoginName" TextField="LoginName">
                        </dx:ASPxListBox>
                        <br />
                        <asp:Button ID="btn_Select_UserList" runat="server" Text="Select" OnClick="btn_Select_UserList_Click" />
                        <asp:Button ID="btn_Cancel_UserList" runat="server" Text="Cancel" OnClick="btn_Cancel_UserList_Click" />
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl runat="server" ID="pop_Alert" ClientInstanceName="pop_Alert" HeaderText="Alert" Width="320" Modal="True" ShowCloseButton="True" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" AllowDragging="True">
                <ContentCollection>
                    <dx:PopupControlContentControl>
                        <div style="text-align: center;">
                            <asp:Label runat="server" ID="lbl_Alert" Width="100%" />
                            <br />
                            <br />
                            <asp:Button ID="btn_AlertOK" runat="server" Text="OK" Width="60" OnClientClick="pop_Alert.Hide();" />
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl runat="server" ID="pop_Confirm" ClientInstanceName="pop_Confirm" HeaderText="Confirmation" Width="320" Modal="True" ShowCloseButton="True"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AllowDragging="True">
                <ContentCollection>
                    <dx:PopupControlContentControl>
                        <div style="text-align: center;">
                            <asp:Label runat="server" ID="lbl_Confirm" Width="100%" />
                            <br />
                            <br />
                            <asp:Button ID="btn_ConfirmYes" runat="server" Text="Yes" Width="60" OnClick="btn_ConfirmYes_Click" />
                            <asp:Button ID="btn_ConfirmNo" runat="server" Text="No" Width="60" OnClientClick="pop_Confirm.Hide();" />
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="menu_CmdBar" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="UpdatePanel">
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
</asp:Content>
