<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="Copy of UserList.aspx.cs"
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
    <script type="text/javascript">

       
        
    </script>
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
                <asp:TemplateField HeaderText="">
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="btn_View" CommandArgument='<%# Eval("LoginName") %>' ImageUrl="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAApgAAAKYB3X3/OAAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAAGqSURBVEiJtdY/a5NRFAbw34nJIO4FZykoDqZJBRHcFMSCH6SzBcFBJxH8DC7OfgDB6tahmJSCSjfHDnYQQnHw33XICaRJmtyqOXA5L+99zvO859x7z32jlOI0i4gVXMc6uumhh37696WUL6dyzBKIiBYe4QE+YS8J+wnp5ujgKp7jaSnlxxRZKeXEQBv72MHq5PwM/Gpi99Gemp8Ab2KALTQWkY/FNTJmgM2ZAvnlA3RqiWcIdZKjfUIArUxx62/Jx0S2kqs1LvAk6zi3LDiPbXzG3Tnl2sHj0QZawXHlgt5CyfFiwcIfJ7cN7Fam38QzvMLlBdhdbDQND8/e1P6dYaWUn3hYg03O9YbhgenPQ0bE7YjYjoijiHgXEdcqBPrJ7RBrC9K9kuBvhvV/WVHONRw2a3ItpRxExDn8zlffa+IYbqneKJUFdgMX8vltBb6LXsNYrRbYvfS/8LpSoD/KoFMRcCd9v5TytQLfSe66g4YPhgv8EZdwseqg1bYK3MdBBr7BzapWcdZmJy+pMzW7nFheux4DLO/Cmcjkv12ZS7/0ZwqMCf3zb8sfHWAXZtNLxs4AAAAASUVORK5CYII="
                            OnClick="btn_View_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="RowId" HeaderText="#" />
                <%--Login--%>
                <asp:TemplateField HeaderText="Login">
                    <ItemTemplate>
                        <div style="display: flex; align-items: center;">
                            <asp:Image ID="Image2" runat="server" ImageUrl="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAAsQAAALEBxi1JjQAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAALnSURBVEiJnZXbb0xRFMZ/68y0My3tVGlnOm2FiLrfhSZEQlIqExX+ABFvHoSIxOUBIUEfCR69iCASQaIkJOpF44FeBJVGXIrpdDoj1esYPcvDdGp0zjml39te39rfd/Y6a68tTIB4XH0JpUaVeQJ+AIWICO88wqPiYul12i92xLeorhThpMIWINcm7afAQ9PkZHmpNP+TQWen5rnzuKiwGzCcvi4DJsqVRD/7Zs+WYVuDSET9Iy7uAWvGK/wyoXO0GJU+cFtbP3eNsN3vl0iWwYcP6vUU0AiszdyhQMNbuPMGhpKpWH4O1C2C0HzLGr/gJxuCQRmEjBJ4C7g0XhzgZitcb/0jDjCYhBstcKvN8hSryOF8emEAfInostGa/4XuAWhotxQB4H47RPstCGHPt6iuHDNwuTiNxQ9tDYOp9gYjJrR1WVIGwnEAIx5X32grZmE4aRX9G0P2ObWxmBYaCWUzNn1e4ZvYoLzIlvIkoMZQpcouY0kAAgX24sFCWOJ3cFeqDIUyO95twIH1MC0vmyvOh/3rbO8DAAJlbgPU4T9S4YP6rfDkPbzrScWqSmDTnNR9cIKCuhXCzmmQnwuhBRCaKDEbYbcIHXZHUODjd3jdlRoTvaNTxudNjYvFAZg1zUFe6JBYTAsTSpSMTlKg6RPcewNfHIcxVBZB3UKonpk1NhI5SokAhHv0rkIdQHwQLjdBe9RZeDwW+WFvNRT9aYg7wRmyQyA1KgwXL7/2YpxrhO9D/yeeRnE+HN0IZQWYarK6vFSaDYAKv7RGfnC9/unkxSF1+vpG6B7gWvoBGuviBy3s97oZmLx8Cl4Xg4/bOJhejxkc2ymx6grWBn2TNwkWMrCijNWHtklPOpb1Xpy5rdO7TZ696qLK6QJmQoClAdorp7LuYK3Ex3OWONWgu8J91Hf0ELAb2YbA3BLCgSkcPhGSq3bmjjj7QGclYV/fMMuTJqUK5Bp0T/XQ7BnhwpFt8tlp/29i0vUHOIpBwQAAAABJRU5ErkJggg==" />
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
                <%--Login--%>
                <asp:TemplateField HeaderText="Job Title">
                    <ItemTemplate>
                        <%# Eval("JobTitle") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <%--Login--%>
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <asp:Image runat="server" ID="img_Status" Width="24" />
                    </ItemTemplate>
                </asp:TemplateField>
                <%--Login--%>
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!-- Popup -->
            <!-- User Profile -->
            <dx:ASPxPopupControl ID="pop_UserInfo" ClientInstanceName="popup" runat="server" Width="960px" Height="600px" HeaderText="" Modal="True" ShowPageScrollbarWhenModal="true"
                ShowCloseButton="true" CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AutoUpdatePosition="True" AllowDragging="true">
                <HeaderStyle Font-Size="Large" />
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                        <!-- iFrame -->
                        <iframe id="iFrame_UserInfo" runat="server" style="width: 100%; height: 98%" frameborder="0" />
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
        </ContentTemplate>
    </asp:UpdatePanel>
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
<%-- <dx:ASPxPopupControl ID="pop_User" ClientInstanceName="pop_User" runat="server" Width="980px" HeaderText="" Modal="True" ShowPageScrollbarWhenModal="true"
                ShowCloseButton="true" CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                <HeaderStyle Font-Size="Large" />
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <div style="padding: 5px;">
                            <div style="display: flex; justify-content: space-between;">
                                <div>
                                    <asp:Label runat="server" ID="lbl_UserFullName" Font-Size="Large" />
                                </div>
                                <div>
                                    <asp:Button runat="server" ID="btn_Delete" ForeColor="Red" Text="Delete" />
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Button runat="server" ID="btn_ChangePassword" Text="Reset Password" />
                                </div>
                            </div>
                            <hr />
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label0" runat="server" Text="Login name" />
                                        <br />
                                        <asp:TextBox runat="server" ID="txt_LoginName" MaxLength="100" Width="90%" />
                                    </td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chk_IsActived" Text="Active" />
                                    </td>
                                    <td align="right">
                                        <asp:Button runat="server" ID="btn_Edit" Text="Edit User" />
                                        <asp:Button runat="server" ID="btn_SaveUser" Text="Save" />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:Button runat="server" ID="btn_CancelUser" Text="Cancel" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="First name" />
                                        <br />
                                        <asp:TextBox runat="server" ID="txt_FName" MaxLength="100" Width="90%" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="Middle name" />
                                        <br />
                                        <asp:TextBox runat="server" ID="txt_MName" MaxLength="100" Width="90%" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="Last name" />
                                        <br />
                                        <asp:TextBox runat="server" ID="txt_LName" MaxLength="100" Width="90%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Text="Emal" />
                                        <br />
                                        <asp:TextBox runat="server" ID="txt_Email" TextMode="Email" AutoPostBack="true" Width="90%" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" Text="Job Title" />
                                        <br />
                                        <asp:TextBox runat="server" ID="txt_JobTitle" MaxLength="100" Width="90%" />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <br />
                            <div style="display: flex; flex-wrap: wrap;">
                                <!-- BU -->
                                <div style="width: 240px; padding-right: 10px; min-height: 360px; display: flex; flex-direction: column;">
                                    <div style="display: flex; justify-content: space-between; margin-bottom: 5px;">
                                        <div>
                                            <asp:Label ID="Label7" runat="server" Font-Size="Small" Text="Bussiness Unit" />
                                        </div>
                                        <div>
                                            <asp:Button runat="server" ID="btn_AddBu" Text="+" />
                                            <asp:Button runat="server" ID="btn_DelBu" Text="-" />
                                        </div>
                                    </div>
                                    <dx:ASPxListBox ID="list_Bu" runat="server" Width="100%" Height="90%">
                                    </dx:ASPxListBox>
                                </div>
                                <asp:Panel runat="server" ID="panel_Bu" Style="width: 685px;">
                                    <div style="display: flex; justify-content: space-between; padding: 5px;">
                                        <div style="display: flex;">
                                            <asp:Label ID="Label1" runat="server" Text="Department: " />
                                            &nbsp;&nbsp;&nbsp;
                                            <asp:DropDownList runat="server" ID="txt_Department" Width="200" />
                                        </div>
                                        <div>
                                            <asp:Button runat="server" ID="btn_EditBu" Text="Edit Business Unit" />
                                            <asp:Button runat="server" ID="btn_SaveBu" Text="Save" />
                                            &nbsp;&nbsp;&nbsp;
                                            <asp:Button runat="server" ID="btn_CancelBu" Text="Cancel" />
                                        </div>
                                    </div>
                                    <div style="display: flex; justify-content: space-between; padding: 10px; background-color: silver;">
                                        <!-- Role -->
                                        <div style="width: 300px; padding-right: 10px; min-height: 300px; display: flex; flex-direction: column;">
                                            <div style="display: flex; justify-content: space-between; margin-bottom: 5px;">
                                                <div>
                                                    <asp:Label ID="Label8" runat="server" Font-Size="Small" Text="Role" />
                                                </div>
                                                <div>
                                                    <asp:Button runat="server" ID="btn_SelAllRole" Text="All" />
                                                    <asp:Button runat="server" ID="btn_SelNoneRole" Text="None" />
                                                </div>
                                            </div>
                                            <dx:ASPxListBox ID="ASPxListBox1" runat="server" Width="100%" Height="90%">
                                            </dx:ASPxListBox>
                                        </div>
                                        <!-- Location -->
                                        <div style="width: 300px; min-height: 300px; display: flex; flex-direction: column;">
                                            <div style="display: flex; justify-content: space-between; margin-bottom: 5px;">
                                                <div>
                                                    <asp:Label ID="Label9" runat="server" Font-Size="Small" Text="Location" />
                                                </div>
                                                <div>
                                                    <asp:Button runat="server" ID="btn_SelAllLoc" Text="All" />
                                                    <asp:Button runat="server" ID="btn_SelNoneLoc" Text="None" />
                                                </div>
                                            </div>
                                            <dx:ASPxListBox ID="ASPxListBox2" runat="server" Width="100%" Height="90%">
                                            </dx:ASPxListBox>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>--%>
