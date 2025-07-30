<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="UserList.aspx.cs"
    Inherits="BlueLedger.PL.Option.Admin.Security.User.UserList" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Namespace="DevExpress.Web.ASPxMenu" Assembly="DevExpress.Web.v10.1" TagPrefix="dx" %>
<%@ Register Namespace="DevExpress.Web.ASPxPanel" Assembly="DevExpress.Web.v10.1" TagPrefix="dx" %>
<%@ Register Namespace="DevExpress.Web.ASPxPopupControl" Assembly="DevExpress.Web.v10.1" TagPrefix="dx" %>
<%@ Register Namespace="DevExpress.Web.ASPxEditors" Assembly="DevExpress.Web.ASPxEditors.v10.1" TagPrefix="dx" %>
<%@ Register Namespace="DevExpress.Web.ASPxTabControl" Assembly="DevExpress.Web.v10.1" TagPrefix="dx" %>
<%@ Register Namespace="DevExpress.Web.ASPxClasses" Assembly="DevExpress.Web.v10.1" TagPrefix="dx" %>
<asp:Content ID="Header" runat="server" ContentPlaceHolderID="head">
    <style>
        .button
        {
            padding: 5px 10px 5px 10px;
        }
        .list
        {
            list-style-type: none;
            padding: 0;
            margin: 0;
        }
        
        .list li a
        {
            border: 1px solid #ddd;
            margin-top: -1px; /* Prevent double borders */
            background-color: #f6f6f6;
            padding: 12px;
            text-decoration: none;
            font-size: 1rem;
            color: black;
            display: block;
        }
        
        .list li a:hover:not(.header)
        {
            background-color: #eee;
        }
    </style>
    <style>
        /* Style the tab */
        .tab
        {
            overflow: hidden;
            border: 1px solid #ccc;
            background-color: #f1f1f1;
        }
        
        /* Style the buttons inside the tab */
        .tab button
        {
            background-color: inherit;
            float: left;
            border: none;
            outline: none;
            cursor: pointer;
            padding: 14px 16px;
            transition: 0.3s;
            font-size: 17px;
        }
        
        /* Change background color of buttons on hover */
        .tab button:hover
        {
            background-color: #ddd;
        }
        
        /* Create an active/current tablink class */
        .tab button.active
        {
            background-color: #ccc;
        }
        
        /* Style the tab content */
        .tabcontent
        {
            display: none;
            padding: 6px 12px;
            border: 1px solid #ccc;
            border-top: none;
        }
        
        .card
        {
            box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
            transition: 0.3s;
            width: 100%;
        }
        
        .card:hover
        {
            box-shadow: 0 8px 16px 0 rgba(0,0,0,0.2);
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <!-- Title & Command Bar -->
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr style="background-color: #4d4d4d; height: 17px;">
            <td align="left" style="padding-left: 10px;">
                <table border="0" cellpadding="2" cellspacing="0">
                    <tr style="color: White;">
                        <td>
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblTitle" Text="Users" />
                        </td>
                    </tr>
                </table>
            </td>
            <td align="right">
                <asp:ImageButton runat="server" ID="btn_Create" AlternateText="Add" ImageUrl="~/App_Themes/Default/Images/master/icon/create.png" OnClick="btn_Create_Click" />
                <asp:ImageButton runat="server" ID="btn_Print" AlternateText="Print" ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" OnClick="btn_Print_Click" />
            </td>
        </tr>
    </table>
    <br />
    <div style="display: flex; justify-content: space-between;">
        <div style="justify-content: start;">
            <% var bu = Request.QueryString["bu"] == null ? "all" : Request.QueryString["bu"].ToString(); %>
            <% var status = Request.QueryString["status"] == null ? "all" : Request.QueryString["status"].ToString(); %>
            <asp:Label runat="server" Text="Business unit: " />
            <select id="sel_bu" style="width: 280px;">
                <option value="">All</option>
                <%
                    foreach (System.Data.DataRow dr in _dtBusinessUnit.Rows)
                    {
                        var buCode = dr["BuCode"].ToString();
                %>
                <option value='<%= buCode %>' <%= bu==buCode?"selected":"" %>>
                    <%= dr["BuName"] %>
                </option>
                <% } %>
            </select>
            &nbsp;&nbsp;&nbsp;
            <asp:Label ID="Label10" runat="server" Text="Status: " />
            <select id="sel_status" class="input" style="width: 80px;">
                <option value="all">All</option>
                <option value="active" <%= status=="active"?"selected":"" %>>Active</option>
                <option value="inactive" <%= status=="inactive"?"selected":"" %>>Inactive</option>
            </select>
        </div>
        <div>
            <input type="text" id="txt_Search" class="input" style="width: 420px" onkeyup="filterTable()" placeholder="search .." title="Type in a name">
        </div>
    </div>
    <br />
    <div style="width: 100%; padding: 5px; border: 0px solid silver;">
        <asp:Label runat="server" ID="lbl_UserCount" Text="0" />
    </div>
    <div style="overflow: auto; height: 760px; background-color: Gray;">
        <asp:GridView runat="server" ID="gv_User" ClientIDMode="Static" SkinID="GRD_V1" Width="100%" EmptyDataText="No user" AutoGenerateColumns="False" GridLines="Horizontal"
            Font-Size="Small" OnRowDataBound="gv_User_RowDataBound">
            <HeaderStyle HorizontalAlign="Left" Height="40px" />
            <RowStyle Height="50px" BackColor="White" ForeColor="#333333" BorderColor="#DDDDDD" />
            <Columns>
                <asp:BoundField DataField="RowId" HeaderText="#" />
                <%--Login--%>
                <asp:TemplateField HeaderText="Login">
                    <ItemTemplate>
                        <div style="display: flex; align-items: center;">
                            <asp:ImageButton ID="btn_View" CommandArgument='<%# Eval("LoginName") %>' runat="server" ImageUrl="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAAsQAAALEBxi1JjQAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAALnSURBVEiJnZXbb0xRFMZ/68y0My3tVGlnOm2FiLrfhSZEQlIqExX+ABFvHoSIxOUBIUEfCR69iCASQaIkJOpF44FeBJVGXIrpdDoj1esYPcvDdGp0zjml39te39rfd/Y6a68tTIB4XH0JpUaVeQJ+AIWICO88wqPiYul12i92xLeorhThpMIWINcm7afAQ9PkZHmpNP+TQWen5rnzuKiwGzCcvi4DJsqVRD/7Zs+WYVuDSET9Iy7uAWvGK/wyoXO0GJU+cFtbP3eNsN3vl0iWwYcP6vUU0AiszdyhQMNbuPMGhpKpWH4O1C2C0HzLGr/gJxuCQRmEjBJ4C7g0XhzgZitcb/0jDjCYhBstcKvN8hSryOF8emEAfInostGa/4XuAWhotxQB4H47RPstCGHPt6iuHDNwuTiNxQ9tDYOp9gYjJrR1WVIGwnEAIx5X32grZmE4aRX9G0P2ObWxmBYaCWUzNn1e4ZvYoLzIlvIkoMZQpcouY0kAAgX24sFCWOJ3cFeqDIUyO95twIH1MC0vmyvOh/3rbO8DAAJlbgPU4T9S4YP6rfDkPbzrScWqSmDTnNR9cIKCuhXCzmmQnwuhBRCaKDEbYbcIHXZHUODjd3jdlRoTvaNTxudNjYvFAZg1zUFe6JBYTAsTSpSMTlKg6RPcewNfHIcxVBZB3UKonpk1NhI5SokAhHv0rkIdQHwQLjdBe9RZeDwW+WFvNRT9aYg7wRmyQyA1KgwXL7/2YpxrhO9D/yeeRnE+HN0IZQWYarK6vFSaDYAKv7RGfnC9/unkxSF1+vpG6B7gWvoBGuviBy3s97oZmLx8Cl4Xg4/bOJhejxkc2ymx6grWBn2TNwkWMrCijNWHtklPOpb1Xpy5rdO7TZ696qLK6QJmQoClAdorp7LuYK3Ex3OWONWgu8J91Hf0ELAb2YbA3BLCgSkcPhGSq3bmjjj7QGclYV/fMMuTJqUK5Bp0T/XQ7BnhwpFt8tlp/29i0vUHOIpBwQAAAABJRU5ErkJggg=="
                                OnClick="btn_View_Click" />
                            &nbsp;&nbsp; <b>
                                <%# Eval("LoginName") %>
                            </b>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <%--Profile--%>
                <asp:TemplateField HeaderText="Profile">
                    <ItemTemplate>
                        <b>
                            <%# Eval("FullName") %></b>
                        <br />
                        <%# Eval("Email") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <%--Job Title--%>
                <asp:TemplateField HeaderText="Job Title">
                    <ItemTemplate>
                        <%# Eval("JobTitle") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <%--Status--%>
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <asp:Image runat="server" ID="img_Status" Width="24" />
                    </ItemTemplate>
                </asp:TemplateField>
                <%--Last Login--%>
                <asp:TemplateField HeaderText="Last Login">
                    <ItemTemplate>
                        <%# string.IsNullOrEmpty(Eval("LastLogin").ToString()) ? "" : Convert.ToDateTime(Eval("LastLogin")).ToString("dd/MM/yyyy HH:mm") %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <asp:Panel runat="server" ID="panel_User" Style="position: relative; bottom: 0; width: 100%; height: 200; background-color: gray; border: 1 solid silver;"
        Visible="false">
    </asp:Panel>
    <asp:Button runat="server" ID="btn_Support" Text="support@carmen" Font-Size="Small" Visible="false" />
    <!--Hidden Field-->
    <asp:HiddenField runat="server" ID="hf_LoginName" />
    <!-- Popup -->
    <dx:ASPxPopupControl ID="pop_Alert" ClientInstanceName="pop_Alert" runat="server" Width="420" Modal="True" HeaderText="Warning" ShowCloseButton="true"
        CloseAction="CloseButton" ShowPageScrollbarWhenModal="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <HeaderStyle BackColor="Yellow" />
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl" runat="server">
                <div style="text-align: center; width: 100%;">
                    <asp:Label runat="server" ID="lbl_Alert" Font-Size="Small" />
                </div>
                <br />
                <br />
                <div class="flex flex-justify-content-center mb-10">
                    <asp:Button ID="btn_AlertOk" runat="server" Text="Ok" Width="80" OnClientClick="pop_Alert.Hide()" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_User" ClientInstanceName="pop_User" runat="server" Width="960px" Height="600px" HeaderText="Profile" Modal="True" ShowCloseButton="true" CloseAction="CloseButton"
        ShowPageScrollbarWhenModal="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AutoUpdatePosition="True">
        <HeaderStyle Font-Size="Small" />
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="flex flex-justify-content-between mb-10" style="padding: 5px;">
                            <div class="flex">
                                <asp:Image runat="server" ImageUrl="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAAsQAAALEBxi1JjQAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAALnSURBVEiJnZXbb0xRFMZ/68y0My3tVGlnOm2FiLrfhSZEQlIqExX+ABFvHoSIxOUBIUEfCR69iCASQaIkJOpF44FeBJVGXIrpdDoj1esYPcvDdGp0zjml39te39rfd/Y6a68tTIB4XH0JpUaVeQJ+AIWICO88wqPiYul12i92xLeorhThpMIWINcm7afAQ9PkZHmpNP+TQWen5rnzuKiwGzCcvi4DJsqVRD/7Zs+WYVuDSET9Iy7uAWvGK/wyoXO0GJU+cFtbP3eNsN3vl0iWwYcP6vUU0AiszdyhQMNbuPMGhpKpWH4O1C2C0HzLGr/gJxuCQRmEjBJ4C7g0XhzgZitcb/0jDjCYhBstcKvN8hSryOF8emEAfInostGa/4XuAWhotxQB4H47RPstCGHPt6iuHDNwuTiNxQ9tDYOp9gYjJrR1WVIGwnEAIx5X32grZmE4aRX9G0P2ObWxmBYaCWUzNn1e4ZvYoLzIlvIkoMZQpcouY0kAAgX24sFCWOJ3cFeqDIUyO95twIH1MC0vmyvOh/3rbO8DAAJlbgPU4T9S4YP6rfDkPbzrScWqSmDTnNR9cIKCuhXCzmmQnwuhBRCaKDEbYbcIHXZHUODjd3jdlRoTvaNTxudNjYvFAZg1zUFe6JBYTAsTSpSMTlKg6RPcewNfHIcxVBZB3UKonpk1NhI5SokAhHv0rkIdQHwQLjdBe9RZeDwW+WFvNRT9aYg7wRmyQyA1KgwXL7/2YpxrhO9D/yeeRnE+HN0IZQWYarK6vFSaDYAKv7RGfnC9/unkxSF1+vpG6B7gWvoBGuviBy3s97oZmLx8Cl4Xg4/bOJhejxkc2ymx6grWBn2TNwkWMrCijNWHtklPOpb1Xpy5rdO7TZ696qLK6QJmQoClAdorp7LuYK3Ex3OWONWgu8J91Hf0ELAb2YbA3BLCgSkcPhGSq3bmjjj7QGclYV/fMMuTJqUK5Bp0T/XQ7BnhwpFt8tlp/29i0vUHOIpBwQAAAABJRU5ErkJggg==" />
                                &nbsp;&nbsp;&nbsp;
                                <asp:Label runat="server" ID="lbl_LoginName" Font-Bold="True" Font-Size="Large" />
                            </div>
                            <div class="flex flex-justify-content-end" style="width: 100%;">
                                <asp:Button runat="server" ID="btn_SaveUser" Text="Save" OnClick="btn_SaveUser_Click" Visible="false" />
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button runat="server" ID="btn_CacelUser" Text="Cancel" OnClick="btn_CancelUser_Click" Visible="false" />
                                <asp:Button runat="server" ID="btn_DelUser" BorderColor="Red" BackColor="Red" ForeColor="White" Text="Delete" OnClick="btn_DelUser_Click" />
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button runat="server" ID="btn_ChangePassword" BorderColor="Green" BackColor="Green" ForeColor="White" Text="Change password ..." OnClick="btn_ChangePassword_Click" />
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button runat="server" ID="btn_EditUser" Text="Edit" OnClick="btn_EditUser_Click" />
                            </div>
                        </div>
                        <hr />
                        <table style="width: 100%;">
                            <tr>
                                <td>
                                    <div>
                                        <asp:Label runat="server" ID="Label1" Font-Bold="True" Text="First name" />
                                    </div>
                                    <asp:TextBox runat="server" ID="txt_FirstName" Font-Size="Medium" Width="90%" />
                                </td>
                                <td>
                                    <div>
                                        <asp:Label runat="server" ID="Label2" Font-Bold="True" Text="Middle name" />
                                    </div>
                                    <asp:TextBox runat="server" ID="txt_MidName" Font-Size="Medium" Width="90%" />
                                </td>
                                <td>
                                    <div>
                                        <asp:Label runat="server" ID="Label3" Font-Bold="True" Text="Last name" />
                                    </div>
                                    <asp:TextBox runat="server" ID="txt_LastName" Font-Size="Medium" Width="90%" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div>
                                        <asp:Label runat="server" ID="Label4" Font-Bold="True" Text="Email" />
                                    </div>
                                    <asp:TextBox runat="server" ID="txt_Email" Font-Size="Medium" Width="90%" />
                                </td>
                                <td>
                                    <div>
                                        <asp:Label runat="server" ID="Label5" Font-Bold="True" Text="Job Title" />
                                    </div>
                                    <asp:TextBox runat="server" ID="txt_JobTitle" Font-Size="Medium" Width="90%" />
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" ID="chk_IsActive" Font-Size="Medium" Text="Active" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div class="flex">
                            <!--BU-->
                            <div style="width: 300px;">
                                <div>
                                    <asp:Label ID="lbl_BusinessUnit" runat="server" Font-Bold="true" Font-Size="Medium" Text="Business Unit(s)" />
                                    <br />
                                    <br />
                                    <div class="flex flex-justify-content-between mb-10">
                                        <asp:Button ID="btn_AddBu" runat="server" Text="Add" Width="60" OnClick="btn_AddBu_Click" />
                                    </div>
                                </div>
                                <dx:ASPxListBox ID="list_Bu" runat="server" Width="100%" Height="445px" AutoPostBack="true" OnSelectedIndexChanged="list_Bu_SelectedIndexChanged">
                                </dx:ASPxListBox>
                            </div>
                            <asp:Panel runat="server" ID="panel_BuUser" Width="100%" Style="padding: 5px; margin: 5px;" BackColor="Silver" Visible="false">
                                <div class="mb-10">
                                    <asp:Label runat="server" ID="lbl_SelectedBu" Font-Bold="True" Font-Size="Large" />
                                </div>
                                <div class="flex flex-justify-content-between mb-10">
                                    <div class="flex">
                                        <asp:Label runat="server" ID="Label6" Font-Bold="True" Text="Department" />&nbsp;&nbsp;
                                        <asp:DropDownList runat="server" ID="ddl_Department" Font-Size="Small" Width="240" />
                                    </div>
                                    <div class="flex">
                                        <asp:Button ID="btn_DelBu" runat="server" ForeColor="Red" Text="Delete" Width="60" OnClick="btn_DelBu_Click" Visible="false" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button runat="server" ID="btn_BuEdit" Text="Edit" OnClick="btn_BuEdit_Click" Visible="false" />
                                        <asp:Button runat="server" ID="btn_BuSave" Text="Save" OnClick="btn_BuSave_Click" Visible="false" />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:Button runat="server" ID="btn_BuCancel" Text="Cancel" OnClick="btn_BuCancel_Click" Visible="false" />
                                    </div>
                                </div>
                                <hr />
                                <div class="flex" style="width: 100%;">
                                    <!--Role-->
                                    <div style="width: 40%; padding-left: 5px;">
                                        <div style="height: 20px;">
                                            <asp:Label ID="LabelRole" runat="server" Text="Role" />
                                            <div style="float: right">
                                                <asp:Button ID="btn_RoleSelAll" runat="server" Text="All" Width="32px" Height="18px" Font-Size="X-Small" OnClick="btn_RoleSelAll_Click" />
                                                <asp:Button ID="btn_RoleSelNone" runat="server" Text="None" Width="36px" Height="18px" Font-Size="X-Small" OnClick="btn_RoleSelNone_Click" />
                                            </div>
                                        </div>
                                        <dx:ASPxListBox ID="list_Role" runat="server" Width="100%" Height="420px" SelectionMode="CheckColumn">
                                            <ItemStyle BackColor="White" ForeColor="Black">
                                                <SelectedStyle BackColor="White" ForeColor="Black">
                                                </SelectedStyle>
                                            </ItemStyle>
                                        </dx:ASPxListBox>
                                    </div>
                                    <!--Location-->
                                    <div style="width: 60%; padding-left: 5px;">
                                        <div style="height: 20px; padding-left: 5px;">
                                            <asp:Label ID="Label7" runat="server" Text="Location" />
                                            <div style="float: right">
                                                <asp:Button ID="btn_LocationSelAll" runat="server" Text="All" Width="32px" Height="18px" Font-Size="X-Small" OnClick="btn_LocationSelAll_Click" />
                                                <asp:Button ID="btn_LocationSelNone" runat="server" Text="None" Width="36px" Height="18px" Font-Size="X-Small" OnClick="btn_LocationSelNone_Click" />
                                            </div>
                                        </div>
                                        <dx:ASPxListBox ID="list_Location" runat="server" Width="100%" Height="420px" SelectionMode="CheckColumn">
                                            <ItemStyle BackColor="White" ForeColor="Black">
                                                <SelectedStyle BackColor="White" ForeColor="Black">
                                                </SelectedStyle>
                                            </ItemStyle>
                                        </dx:ASPxListBox>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btn_ChangePassword" />
                        <asp:PostBackTrigger ControlID="btn_DelBu" />
                        <asp:PostBackTrigger ControlID="btn_AddBu" />
                    </Triggers>
                </asp:UpdatePanel>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_BuConfirmDelete" ClientInstanceName="pop_BuConfirmDelete" runat="server" Width="540" Modal="True" HeaderText="Confirmation"
        ShowCloseButton="true" CloseAction="CloseButton" ShowPageScrollbarWhenModal="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                <div class="flex flex-justify-content-center mb-10">
                    <asp:Label runat="server" ID="lbl_BuConfirmDelete" Font-Size="Small" />
                </div>
                <div style="text-align: center; width: 100%;">
                    <asp:Label runat="server" ID="Label8" Font-Size="Smaller" ForeColor="Red" Text="*All roles and locations will be removed from this business unit." />
                </div>
                <br />
                <br />
                <div class="flex flex-justify-content-center mb-10">
                    <asp:Button ID="btn_BuConfirm_Yes" runat="server" Text="Yes" Width="60" OnClick="btn_BuConfirm_Yes_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btn_BuConfirm_No" runat="server" Text="No" Width="60" OnClientClick="pop_BuConfirmDelete.Hide()" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_AddBu" ClientInstanceName="pop_AddBu" runat="server" Width="540" Modal="True" HeaderText="Business Unit" ShowCloseButton="true"
        CloseAction="CloseButton" ShowPageScrollbarWhenModal="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                <dx:ASPxListBox ID="list_AddBu" runat="server" Width="100%" Height="420" SelectionMode="CheckColumn">
                    <ItemStyle BackColor="White" ForeColor="Black">
                        <SelectedStyle BackColor="White" ForeColor="Black">
                        </SelectedStyle>
                    </ItemStyle>
                </dx:ASPxListBox>
                <br />
                <br />
                <div class="flex flex-justify-content-center mb-10">
                    <asp:Button ID="btn_AddBuSeleted" runat="server" Text="Add" Width="60" OnClick="btn_AddBuSeleted_Click" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_ChangePassword" ClientInstanceName="pop_ChangePassword" runat="server" Width="500" Modal="True" HeaderText="Business Unit"
        ShowCloseButton="true" CloseAction="CloseButton" ShowPageScrollbarWhenModal="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                <div class="popup-header">
                    <h3>
                        Change Password
                    </h3>
                </div>
                <br />
                <table style="width: 90%; margin: 0 auto;">
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="New Password" />
                        </td>
                        <td>
                            <asp:TextBox ID="txt_NewPassword" runat="server" Text="" Width="180px" TextMode="Password" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="Confirm Password" />
                        </td>
                        <td>
                            <asp:TextBox ID="txt_NewPasswordConfirm" runat="server" Text="" Width="180px" TextMode="Password" />
                        </td>
                    </tr>
                </table>
                <br />
                <hr />
                <asp:Label runat="server" ID="lbl_PwdLength" Font-Size="Smaller" />
                <br />
                <asp:Label runat="server" ID="lbl_PwdComplexity" Font-Size="Smaller" />
                <hr />
                <br />
                <div class="flex flex-justify-content-center mb-10">
                    <asp:Button ID="btn_ChangePassword_Yes" runat="server" Text="Yes" Width="60" OnClick="btn_ChangePassword_Yes_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="Button2" runat="server" Text="No" Width="60" OnClientClick="pop_ChangePassword.Hide()" />
                </div>
                <asp:HiddenField runat="server" ID="hf_PwdLength" />
                <asp:HiddenField runat="server" ID="hf_PwdComplexity" />
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <!-- Script -->
    <script type="text/javascript">


        function filterTable() {
            var input, filter, table, tr, td, i, txtValue;
            input = document.getElementById("txt_Search");
            filter = input.value.toUpperCase();
            table = document.getElementById("gv_User");
            tr = table.getElementsByTagName("tr");

            for (i = 0; i < tr.length; i++) {
                td = tr[i].getElementsByTagName("td");
                for (var j = 0; j < td.length; j++) {
                    txtValue = td[j].textContent || td[j].innerText;
                    if (txtValue.toUpperCase().indexOf(filter) > -1) {
                        tr[i].style.display = "";
                        break;
                    } else {
                        tr[i].style.display = "none";
                    }
                }
            }
        }

        $(document).ready(function () {

            $('#sel_bu').on('change', function () {
                var bu = $(this).val();
                var status = '<%= Request.QueryString["status"] %>';

                location.href = 'userlist.aspx?status='.concat(status).concat('&bu=').concat(bu);
            });

            $('#sel_status').on('change', function () {
                var status = $(this).val();
                var bu = '<%= Request.QueryString["bu"] %>';

                location.href = 'userlist.aspx?status='.concat(status).concat('&bu=').concat(bu);
            });
        });
        
    </script>
</asp:Content>
