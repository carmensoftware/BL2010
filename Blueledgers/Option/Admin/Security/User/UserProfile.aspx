<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserProfile.aspx.cs" Inherits="Option_Admin_Security_User_UserProfile" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
    <style>
        .onoffswitch
        {
            position: relative;
            width: 90px;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
        }
        .onoffswitch-checkbox
        {
            display: none;
        }
        .onoffswitch-label
        {
            display: block;
            overflow: hidden;
            cursor: pointer;
            border: 2px solid #999999;
            border-radius: 0px;
        }
        .onoffswitch-inner
        {
            display: block;
            width: 200%;
            margin-left: -100%;
            transition: margin 0.3s ease-in 0s;
        }
        .onoffswitch-inner:before, .onoffswitch-inner:after
        {
            display: block;
            float: left;
            width: 50%;
            height: 20px;
            padding: 0;
            line-height: 20px;
            font-size: 10px;
            color: white; /* font-family: Trebuchet, Arial, sans-serif; */
            font-weight: bold;
            box-sizing: border-box;
            border: 2px solid transparent;
            background-clip: padding-box;
        }
        .onoffswitch-inner:before
        {
            content: "Active";
            padding-left: 10px;
            background-color: #2E8DEF;
            color: #FFFFFF;
        }
        .onoffswitch-inner:after
        {
            content: "Inactive";
            padding-right: 10px;
            background-color: #CCCCCC;
            color: #333333;
            text-align: right;
        }
        .onoffswitch-switch
        {
            display: block;
            width: 25px;
            margin: 0px;
            background: #000000;
            position: absolute;
            top: 0;
            bottom: 0;
            right: 65px;
            transition: all 0.3s ease-in 0s;
        }
        .onoffswitch-checkbox:checked + .onoffswitch-label .onoffswitch-inner
        {
            margin-left: 0;
        }
        .onoffswitch-checkbox:checked + .onoffswitch-label .onoffswitch-switch
        {
            right: 0px;
        }
    </style>
    <style type="text/css">
        body
        {
            font-size: 14px;
        }
        
        .Background
        {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }
        
        .Popup
        {
            background-color: White;
            border-width: 3px; /* border-style: solid; */ /* border-color: black; */
            padding-top: 1px;
            padding-left: 1px;
        }
        
        .TableNoBorder
        {
            border: 0;
            border-collapse: collapse;
            border-spacing: 0;
            padding: 0;
        }
        
        .popup-header
        {
            padding: 9px 15px;
            border-bottom: 1px solid #eee;
        }
        
        .popup-content
        {
            padding: 10px 10px 10px 10px;
            margin: 15px;
        }
        
        .popup-footer
        {
            /*background-color: #f5f5f5;*/
            padding: 14px 15px 15px;
            margin-bottom: 0;
            border-top: 1px solid #ddd;
            -webkit-border-radius: 0 0 6px 6px;
            -moz-border-radius: 0 0 6px 6px;
            border-radius: 0 0 6px 6px;
            -webkit-box-shadow: inset 0 1px 0 #ffffff;
            -moz-box-shadow: inset 0 1px 0 #ffffff;
            box-shadow: inset 0 1px 0 #ffffff;
        }
    </style>
    <script type="text/javascript">
        function UserDeleteConfirmation() {
            var userName = document.getElementById('TextLoginName').value;

            return confirm('Are you sure you want to delete this user?\n"' + userName + '"');
        }
    </script>
    <script type="text/javascript">
        function GetSelectedItem() {
            var list = document.getElementById('ListPopBuAdd').value;


        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </cc1:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <br />
            <div class="popup-header">
                <asp:Label ID="LabelUserName" runat="server" Font-Italic="true" Font-Size="Medium" />
                <div style="float: right">
                    <asp:Button ID="BtnEdit" runat="server" Text="Edit" OnClick="BtnEdit_Click" />
                    <asp:Button ID="BtnChangePassword" runat="server" Text="Change Password" OnClick="BtnChangePassword_Click" />
                    <asp:Button ID="BtnDelUser" runat="server" Text="Delete" OnClick="BtnDelUser_Click" />
                    <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" />
                    <asp:Button ID="BtnCancel" runat="server" Text="Cancel" OnClick="BtnCancel_Click" />
                </div>
                <br style="clear: both" />
            </div>
            <br />
            <div style="padding-left: 10px;">
                <!--User Information-->
                <div style="width: 240px; float: left;">
                    <table style="width: 100%; padding-right: 5px; line-height: 20px; font-size: 1.1em;">
                        <%--Status--%>
                        <tr>
                            <td>
                                <div class="onoffswitch">
                                    <input id="ActiveUser" runat="server" type="checkbox" name="ActiveUser" class="onoffswitch-checkbox" checked>
                                    <label class="onoffswitch-label" for="ActiveUser">
                                        <span class="onoffswitch-inner"></span><span class="onoffswitch-switch"></span>
                                    </label>
                                </div>
                                <br />
                            </td>
                        </tr>
                        <%--Login Name--%>
                        <tr>
                            <td>
                                <asp:Label ID="LabelLoginName" runat="server" Font-Bold="True" Text="Login Name" />
                                <asp:TextBox ID="TextLoginName" runat="server" Width="100%" AutoPostBack="True" OnTextChanged="TextLoginName_TextChanged" /><br />
                                <asp:Label ID="lbl_errLogin" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <%--Password--%>
                        <tr>
                            <td>
                                <asp:Label ID="LabelPassword" runat="server" Font-Bold="True" Text="Password" />
                                <asp:TextBox ID="TextPassword" runat="server" Width="100%" TextMode="Password" />
                            </td>
                        </tr>
                        <%--Confirm Password--%>
                        <tr>
                            <td>
                                <asp:Label ID="LabelPasswordConfirm" runat="server" Font-Bold="True" Text="Confirm Password" />
                                <asp:TextBox ID="TextPasswordConfirm" runat="server" Width="100%" TextMode="Password" />
                            </td>
                        </tr>
                        <%--First Name--%>
                        <tr>
                            <td>
                                <asp:Label ID="LabelFirstName" runat="server" Font-Bold="True" Text="First Name " />
                                <asp:TextBox ID="TextFirstName" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <%--Middle Name--%>
                        <tr>
                            <td>
                                <asp:Label ID="LabelMidName" runat="server" Font-Bold="True" Text="Middle Name" />
                                <asp:TextBox ID="TextMidName" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <%--Last Name--%>
                        <tr>
                            <td>
                                <asp:Label ID="LabelLastName" runat="server" Font-Bold="True" Text="Last Name" />
                                <asp:TextBox ID="TextLastname" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <%--Email--%>
                        <tr>
                            <td>
                                <asp:Label ID="LabelEmail" runat="server" Font-Bold="True" Text="Email" />
                                <asp:TextBox ID="TextEmail" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <%--Job Title--%>
                        <tr>
                            <td>
                                <asp:Label ID="LabelJobTitle" runat="server" Font-Bold="True" Text="Job Title" />
                                <asp:TextBox ID="TextJobTitle" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <%--Department--%>
                        <tr>
                            <td>
                                <asp:Label ID="LabelBusinessUnit" runat="server" Font-Bold="True" Text="Primary BU" Style="display: none;" />
                                <asp:DropDownList ID="DdlBusinessUnit" runat="server" Width="100%" AutoPostBack="True" Style="display: none;" />
                                <asp:Label ID="LabelDepartment" runat="server" Font-Bold="True" Text="Department" />
                                <asp:DropDownList ID="DdlDepartment" runat="server" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="DdlDepartment_SelectedIndexChanged" />
                            </td>
                        </tr>
                    </table>
                </div>
                <!--BU-->
                <div style="width: 120px; float: left; padding-left: 5px;">
                    <div style="height: 20px; padding-left: 5px;">
                        <asp:Label ID="LabelBU" runat="server" Text="BU" />
                        <div style="float: right">
                            <asp:Button ID="BtnBUAdd" runat="server" Text="+" Width="24px" Height="18px" OnClick="BtnBUAdd_Click" />
                            <asp:Button ID="BtnBUDel" runat="server" Text="-" Width="24px" Height="18px" OnClick="BtnBUDel_Click" />
                        </div>
                    </div>
                    <dx:ASPxListBox ID="ListBU" runat="server" Width="100%" Height="420px" AutoPostBack="true" OnSelectedIndexChanged="ListBU_SelectedIndexChanged">
                    </dx:ASPxListBox>
                </div>
                <!--Role-->
                <div style="width: 200px; float: left; padding-left: 5px;">
                    <div style="height: 20px; padding-left: 5px;">
                        <asp:Label ID="LabelRole" runat="server" Text="Role" />
                        <div style="float: right">
                            <asp:Button ID="BtnRoleSelAll" runat="server" Text="All" Width="32px" Height="18px" Font-Size="X-Small" OnClick="BtnRoleSelAll_Click" />
                            <asp:Button ID="BtnRoleSelNone" runat="server" Text="None" Width="36px" Height="18px" Font-Size="X-Small" OnClick="BtnRoleSelNone_Click" />
                        </div>
                    </div>
                    <dx:ASPxListBox ID="ListRole" runat="server" Width="100%" Height="420px" SelectionMode="CheckColumn">
                        <ItemStyle BackColor="White" ForeColor="Black">
                            <SelectedStyle BackColor="White" ForeColor="Black">
                            </SelectedStyle>
                        </ItemStyle>
                    </dx:ASPxListBox>
                </div>
                <!--Location-->
                <div style="width: 280px; float: left; padding-left: 5px;">
                    <div style="height: 20px; padding-left: 5px;">
                        <asp:Label ID="Label2" runat="server" Text="Location" />
                        <div style="float: right">
                            <asp:Button ID="BtnLocationSelAll" runat="server" Text="All" Width="32px" Height="18px" Font-Size="X-Small" OnClick="BtnLocationSelAll_Click" />
                            <asp:Button ID="BtnLocationSelNone" runat="server" Text="None" Width="36px" Height="18px" Font-Size="X-Small" OnClick="BtnLocationSelNone_Click" />
                        </div>
                    </div>
                    <dx:ASPxListBox ID="ListLocation" runat="server" Width="100%" Height="420px" SelectionMode="CheckColumn">
                        <ItemStyle BackColor="White" ForeColor="Black">
                            <SelectedStyle BackColor="White" ForeColor="Black">
                            </SelectedStyle>
                        </ItemStyle>
                    </dx:ASPxListBox>
                </div>
                <!--User Setting-->
            </div>
            <%--Popup Change Password--%>
            <cc1:ModalPopupExtender ID="Pop_ChangePassword" runat="server" BehaviorID="Pop_ChangePassword" PopupControlID="Panel_ChangePassword" TargetControlID="HiddenField_ChangePassword"
                CancelControlID="BtnPopChangePasswordCancel" BackgroundCssClass="Background">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="Panel_ChangePassword" runat="server" CssClass="Popup" Width="320px" Height="220px">
                <div class="popup-header">
                    <h3>
                        Change Password
                    </h3>
                </div>
                <br />
                <table style="width: 90%; margin: 0 auto;">
                    <tr>
                        <td>
                            <asp:Label ID="LabelChangePassword" runat="server" Text="New Password" />
                        </td>
                        <td>
                            <asp:TextBox ID="TextChangePassword" runat="server" Text="" Width="180px" TextMode="Password" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LabelChangePasswordConfirm" runat="server" Text="Confirm Password" />
                        </td>
                        <td>
                            <asp:TextBox ID="TextChangePasswordConfirm" runat="server" Text="" Width="180px" TextMode="Password" />
                        </td>
                    </tr>
                </table>
                <br />
                <div style="text-align: center; color: Red;">
                    <asp:Label ID="LabelChangePasswordAlert" runat="server" Text="" />
                </div>
                <div class="popup-footer">
                    <div style="float: right; margin-right: 10px;">
                        <asp:Button ID="BtnPopChangePasswordSave" runat="server" Text="Save" OnClick="BtnPopChangePasswordSave_Click" />
                        <asp:Button ID="BtnPopChangePasswordCancel" runat="server" Text="Cancel" />
                    </div>
                    <br style="clear: both;" />
                </div>
            </asp:Panel>
            <asp:HiddenField ID="HiddenField_ChangePassword" runat="server" />
            <%--Popup Delete User --%>
            <cc1:ModalPopupExtender ID="Pop_DelUserConfirm" runat="server" BehaviorID="Pop_DelUserConfirm" PopupControlID="Panel_DelUserConfirm" TargetControlID="HiddenField_DelUserConfirm"
                CancelControlID="BtnPopDelUserConfirmNo" BackgroundCssClass="Background">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="Panel_DelUserConfirm" runat="server" Style="display: block" CssClass="Popup" Width="360px" Height="150px">
                <div class="popup-header" style="text-align: center">
                    <h4>
                        <asp:Label ID="LabelPopDelUserConfirm" runat="server" Text="All configurations such as Business Unit, Role and Location of this user will be deleted.<br/><br/>Do you want to delete this user? " />
                    </h4>
                </div>
                <div class="popup-footer">
                    <div style="float: right">
                        <asp:Button ID="BtnPopDelUserConfirmYes" runat="server" Text="Yes" Width="60px" OnClick="BtnPopDelUserConfirmYes_Click" />
                        <asp:Button ID="BtnPopDelUserConfirmNo" runat="server" Text="No" Width="60px" />
                    </div>
                    <br style="clear: both" />
                </div>
            </asp:Panel>
            <asp:HiddenField ID="HiddenField_DelUserConfirm" runat="server" />
            <%--Popup Add BU --%>
            <cc1:ModalPopupExtender ID="Pop_BuAdd" runat="server" BehaviorID="Pop_BuAdd" PopupControlID="Panel_BuAdd" TargetControlID="HiddenField_BuAdd" CancelControlID="BtnPopBuAddCancel"
                BackgroundCssClass="Background">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="Panel_BuAdd" runat="server" align="center" CssClass="Popup" Width="460px" Height="400px">
                <div style="margin: 0 auto;">
                    <p>
                        <asp:Label ID="LabelPopBuAddTitle" runat="server" Text="Available Business Unit" Font-Size="14px" />
                    </p>
                    <dx:ASPxListBox ID="ListPopBuAdd" runat="server" Width='420px' Height='290px' SelectionMode="CheckColumn">
                    </dx:ASPxListBox>
                </div>
                <br />
                <div>
                    <asp:Button ID="BtnPopBuAddOk" runat="server" Text="Ok" Width="60px" OnClick="BtnPopBuAddOk_Click" />
                    <asp:Button ID="BtnPopBuAddCancel" runat="server" Text="Cancel" Width="60px" />
                </div>
            </asp:Panel>
            <asp:HiddenField ID="HiddenField_BuAdd" runat="server" />
            <%--Popup Delete User --%>
            <cc1:ModalPopupExtender ID="Pop_BuDel" runat="server" BehaviorID="Pop_BuDel" PopupControlID="Panel_BuDel" TargetControlID="HiddenField_BuDel" CancelControlID="BtnPopBuDelNo"
                BackgroundCssClass="Background">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="Panel_BuDel" runat="server" Style="display: block" CssClass="Popup" Width="360px" Height="150px">
                <div class="popup-header" style="text-align: center">
                    <h4>
                        <asp:Label ID="Label1" runat="server" Text="All setting of Role and Location will be deleted.<br/><br/>Do you want to delete this Business Unit? " />
                    </h4>
                </div>
                <div class="popup-footer">
                    <div style="float: right">
                        <asp:Button ID="BtnPopBuDelYes" runat="server" Text="Yes" Width="60px" OnClick="BtnPopBuDelYes_Click" />
                        <asp:Button ID="BtnPopBuDelNo" runat="server" Text="No" Width="60px" />
                    </div>
                    <br style="clear: both" />
                </div>
            </asp:Panel>
            <asp:HiddenField ID="HiddenField_BuDel" runat="server" />
            <%--Popup MessageBox --%>
            <cc1:ModalPopupExtender ID="Pop_MessageBox" runat="server" BehaviorID="Pop_MessageBox" PopupControlID="Panel_MessageBox" TargetControlID="HiddenField_MessageBox"
                CancelControlID="BtnPopMessageBoxOk" BackgroundCssClass="Background">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="Panel_MessageBox" runat="server" Style="display: block" CssClass="Popup" Width="320px" Height="120px" align="center">
                <div class="popup-header" style="text-align: center">
                    <h4>
                        <asp:Label ID="LabelPopMessageBox" runat="server" />
                    </h4>
                </div>
                <p>
                    <asp:Button ID="BtnPopMessageBoxOk" runat="server" Text="Ok" Width="60px" />
                </p>
            </asp:Panel>
            <asp:HiddenField ID="HiddenField_MessageBox" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
