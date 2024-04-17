<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="BlueLedger.PL.Option.User.Default"
    Title="Home - blueledgers.com" %>

<%@ MasterType VirtualPath="~/Master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTabControl"
    TagPrefix="dx" %>
<%@ Register Src="Inbox.ascx" TagName="Inbox" TagPrefix="uc1" %>
<%@ Register Src="Delete.ascx" TagName="Delete" TagPrefix="uc2" %>
<%@ Register Src="Sent.ascx" TagName="Sent" TagPrefix="uc3" %>
<%@ Register Src="Personal.ascx" TagName="Personal" TagPrefix="uc4" %>
<%@ Register Src="~/UserControl/Password.ascx" TagName="PasswordDialog" TagPrefix="uc5" %>
<%@ Register Src="HomePage.ascx" TagName="HomePage" TagPrefix="uc6" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <style>
        .content-main-bar
        {
            width: 100%;
            background-color: #4d4d4d;
            height: 25px;
            line-height: 2;
        }
        .content-main-bar div
        {
            float: left;
            margin-left: 10px;
            vertical-align: middle;
        }
        .content-main-bar img
        {
            vertical-align: middle;
        }
        
        .profile
        {
            width: 100%;
            height: 120px;
            display: inline-block;
        }
        .figure
        {
            float: left;
            margin-left: 20px;
            width: 96px;
            height: 96px;
            border-radius: 50%;
            overflow: hidden;
        }
    </style>
    <style>
        .nav-bar
        {
            font-family: Verdana,sans-serif;
            font-family: Arial, Tahoma, Segoe UI;
            font-size: 1rem;
            line-height: 1.5;
            width: 100%;
        }
        
        
        a:active, a:hover
        {
            outline: 0;
        }
        
        .tabpage
        {
            display: none;
        }
        
        .navbar
        {
            list-style-type: none;
            margin: 0;
            padding: 0;
            overflow: hidden;
        }
        .navbar li
        {
            float: left;
        }
        .navbar li a
        {
            display: block;
            padding: 8px 16px;
        }
        .navbar li a:hover
        {
            color: #000;
            background-color: #ccc;
        }
        .tablink-active
        {
            color: #fff !important;
            /*background-color: #20B9EB !important;*/
            background-color: #2196f3 !important;
        }
        
        .tab-container:after
        {
            content: "";
            display: table;
            clear: both;
        }
        .tab-line
        {
            width: 100%;
            height: 5px;
            /*background-color: #20B9EB;*/
            background-color: #2196f3 !important;
        }
        .tab-border
        {
            /*border: 1px solid #CCC !important;*/
            border: 1px solid #2196f3 !important;
            padding: 12px;
        }
    </style>
    <style>
        .block
        {
            display: inline-block;
            margin: 10px;
        }
        .text-gray
        {
            color: Gray;
        }
    </style>
    <br />
    <!-- Title & Command Bar  -->
    <div class="content-main-bar">
        <div>
            <img src="../../App_Themes/Default/Images/master/icon/icon_home.png" alt="icon" />
        </div>
        <div>
            <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:Option.User.Default, lbl_Title %>" SkinID="LBL_HD_WHITE" />
        </div>
    </div>
    <br />
    <div>
        <div class="block">
            <img src="../../App_Themes/Default/Images/user-nophoto.png" alt="Image" width="96px" />
        </div>
        <div class="block">
            <h2>
                <%= string.Format("{0} {1} {2}", LoginInfo.FName, LoginInfo.MName, LoginInfo.LName)%>
            </h2>
            <h4>
                Login name: <span><%= LoginInfo.LoginName %></span>
            </h4>
            <h4>
                Last login: <span><%= LoginInfo.LastLogin.ToShortDateString() %></span>
            </h4>
            <div>
                <asp:Button ID="btn_ChangePassword" runat="server" Text="Change Password ..." OnClick="btn_ChangePassword_Click" />
            </div>
        </div>
    </div>
    <div class="nav-bar">
        <ul class="navbar">
            <%--<li><a href="#" class="tablink tablink-active" onclick="openPage(event,'Message')">Message</a></li>--%>
            <li><a href="#" class="tablink tablink-active" onclick="openPage(event,'Profile')">Profile</a></li>
            <li><a href="#" class="tablink" onclick="openPage(event,'RecentActivity')">Recent activity</a></li>
        </ul>
        <%--<div style="height: 5px; width: 100%; background-color: #20B9EB;" />--%>
        <div class="tab-line" />
    </div>
    <div id="Profile" class="tab-container tab-border tabpage" style="display: block;">
        <%--<b><u>Change Password</u></b>
        <uc5:ChangePwd ID="ChangePwd" runat="server" />
        <hr />
        --%>
        <b><u>Signature</u></b>
        <uc4:Personal ID="Personal" runat="server" />
    </div>
    <div id="RecentActivity" class="tab-container tab-border tabpage">
        &nbsp;
    </div>
    <div id="HomePage" class="tab-container tab-border" style="display: none;">
        <uc6:HomePage ID="HomePage1" runat="server" />
    </div>
    <script type="text/javascript">
        function openPage(evt, id) {
            var i, x, tablinks;
            x = document.getElementsByClassName('tabpage');
            for (i = 0; i < x.length; i++) {
                x[i].style.display = "none";
            }
            document.getElementById(id).style.display = "block";

            tablinks = document.getElementsByClassName("tablink");
            for (i = 0; i < x.length; i++) {
                tablinks[i].classList.remove("tablink-active");
            }
            evt.currentTarget.classList.add("tablink-active");
        }
    </script>
    <uc5:PasswordDialog ID="dlgPassword" runat="server" />
</asp:Content>
