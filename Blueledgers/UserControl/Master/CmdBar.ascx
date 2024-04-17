<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CmdBar.ascx.cs" Inherits="BlueLedger.PL.UserControls.Master.CmdBar" %>
<div class="CmdBar">
    <ul id="ul_control">
        <li><span><a href="javascript:ShowSubMenu('ul_sub_tool')">
                      <img src='<%= ResolveUrl("~/App_Themes/Default/Images/master/pt/default/wrenchplus.png") %>'
                           alt="" /></a><i>Tools</i></span>
            <ul id="ul_sub_tool">
                <li onmouseover=" javascript:SubMenuItemMouserOver(this); " onmouseout=" javascript:SubMenuItemMouserOut(this); ">
                    <span><a href='<%= ResolveUrl("~/Option/Admin/Bu/Bu.aspx") %>'>Business Profile</a></span></li>
            </ul>
        </li>
        <li><span><a href="javascript:ShowSubMenu('ul_sub_info')">
                      <img src='<%= ResolveUrl("~/App_Themes/Default/Images/master/pt/default/info_icon.png") %>'
                           alt="" /></a><i>About</i></span>
            <ul id="ul_sub_info">
                <li onmouseover=" javascript:SubMenuItemMouserOver(this); " onmouseout=" javascript:SubMenuItemMouserOut(this); ">
                    <span><a href='<%= ResolveUrl("~/Help/Default.aspx") %>'>Help & Training</a></span></li>
                <li onmouseover=" javascript:SubMenuItemMouserOver(this); " onmouseout=" javascript:SubMenuItemMouserOut(this); ">
                    <span><a href='<%= ResolveUrl("~/Help/Default.aspx") %>'>About Blueledgers.com</a></span></li>
            </ul>
        </li>
        <li><span><a href="javascript:ShowSubMenu('ul_sub_option')">
                      <img src='<%= ResolveUrl("~/App_Themes/Default/Images/master/pt/default/cog_icon.png") %>'
                           alt="" /></a><i>Options</i></span>
            <ul id="ul_sub_option">
                <li onmouseover=" javascript:SubMenuItemMouserOver(this); " onmouseout=" javascript:SubMenuItemMouserOut(this); ">
                    <span><a href='<%= ResolveUrl("~/Option/User/Default.aspx") %>'>User Profile</a></span></li>
                <li onmouseover=" javascript:SubMenuItemMouserOver(this); " onmouseout=" javascript:SubMenuItemMouserOut(this); ">
                    <span><a href='<%= ResolveUrl("~/Login.aspx") %>'>Log Out</a></span></li>
            </ul>
        </li>
    </ul>
</div>