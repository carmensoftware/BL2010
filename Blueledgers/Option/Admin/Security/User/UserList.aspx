<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="UserList.aspx.cs"
    Inherits="BlueLedger.PL.Option.Admin.Security.User.UserList" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Namespace="DevExpress.Web.ASPxMenu" Assembly="DevExpress.Web.v10.1" TagPrefix="dx" %>
<%@ Register Namespace="DevExpress.Web.ASPxPopupControl" Assembly="DevExpress.Web.v10.1" TagPrefix="dx" %>
<%@ Register Namespace="DevExpress.Web.ASPxEditors" Assembly="DevExpress.Web.ASPxEditors.v10.1" TagPrefix="dx" %>
<asp:Content ID="Content_Header" runat="server" ContentPlaceHolderID="head">
</asp:Content>
<asp:Content ID="Content_Body" ContentPlaceHolderID="cph_Main" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!-- Title Bar -->
            <div class="flex flex-justify-content-between mb-10" style="background-color: #4d4d4d; color: White; height: 24px;">
                <div class="flex flex-align-items-center">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                    &nbsp;&nbsp;
                    <asp:Label runat="server" ID="lbl_Title" Font-Bold="true" Text="Users" />
                </div>
                <div class="flex flex-align-items-center">
                    <asp:ImageButton runat="server" ID="btn_Create" AlternateText="Add" ImageUrl="~/App_Themes/Default/Images/master/icon/create.png" OnClick="btn_Create_Click" />
                    &nbsp;&nbsp;
                    <asp:ImageButton runat="server" ID="btn_Print" AlternateText="Print" ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" OnClick="btn_Print_Click" />
                </div>
            </div>
            <!-- Menu Bar -->
            <div class="flex flex-justify-content-between" style="flex-wrap: wrap;">
                <div class="flex flex-align-items-center mb-10">
                    <asp:Label ID="Label12" runat="server" Text="Business unit: " />
                    &nbsp;&nbsp;&nbsp;
                    <asp:DropDownList runat="server" ID="ddl_Bu" Width="280" AutoPostBack="true" OnSelectedIndexChanged="ddl_Bu_SelectedIndexChanged">
                    </asp:DropDownList>
                    &nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lbl_Status" runat="server" Text="Status: " />
                    &nbsp;&nbsp;&nbsp;
                    <asp:DropDownList runat="server" ID="ddl_Status" Width="80" AutoPostBack="true" OnSelectedIndexChanged="ddl_Status_SelectedIndexChanged">
                        <asp:ListItem Value="" Text="All" />
                        <asp:ListItem Value="1" Text="Active" />
                        <asp:ListItem Value="0" Text="Inactive" />
                    </asp:DropDownList>
                </div>
                <div class="mb-10">
                    <input type="text" id="txt_Search" class="input" style="width: 350px" onkeyup="filterTable()" placeholder="search .." title="Type in a name">
                </div>
            </div>
            <br />
            <!-- User license -->
            <div class="flex flex-align-items-center width-100 mb-10">
                <asp:Label runat="server" ID="lbl_UserCount" Text="0" />
            </div>
            <div style="width: 100%; padding: 5px; border: 0px solid silver;">
            </div>
            <!-- User List -->
            <div style="overflow: auto; height: 760px; background-color: Gray;">
                <asp:GridView runat="server" ID="gv_User" ClientIDMode="Static" SkinID="GRD_V1" Width="100%" EmptyDataText="No user" AutoGenerateColumns="False" GridLines="Horizontal"
                    Font-Size="Small" OnRowDataBound="gv_User_RowDataBound" OnSelectedIndexChanged="gv_User_SelectedIndexChanged">
                    <HeaderStyle HorizontalAlign="Left" Height="40px" />
                    <RowStyle Height="50px" BackColor="White" ForeColor="#333333" BorderColor="#DDDDDD" />
                    <Columns>
                        <%--ID--%>
                        <asp:BoundField DataField="RowId" HeaderText="#" />
                        <%--Login--%>
                        <asp:TemplateField HeaderText="Login">
                            <ItemTemplate>
                                <div style="display: flex; align-items: center;">
                                    <asp:Image ID="Image2" runat="server" ImageUrl="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAAsQAAALEBxi1JjQAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAALnSURBVEiJnZXbb0xRFMZ/68y0My3tVGlnOm2FiLrfhSZEQlIqExX+ABFvHoSIxOUBIUEfCR69iCASQaIkJOpF44FeBJVGXIrpdDoj1esYPcvDdGp0zjml39te39rfd/Y6a68tTIB4XH0JpUaVeQJ+AIWICO88wqPiYul12i92xLeorhThpMIWINcm7afAQ9PkZHmpNP+TQWen5rnzuKiwGzCcvi4DJsqVRD/7Zs+WYVuDSET9Iy7uAWvGK/wyoXO0GJU+cFtbP3eNsN3vl0iWwYcP6vUU0AiszdyhQMNbuPMGhpKpWH4O1C2C0HzLGr/gJxuCQRmEjBJ4C7g0XhzgZitcb/0jDjCYhBstcKvN8hSryOF8emEAfInostGa/4XuAWhotxQB4H47RPstCGHPt6iuHDNwuTiNxQ9tDYOp9gYjJrR1WVIGwnEAIx5X32grZmE4aRX9G0P2ObWxmBYaCWUzNn1e4ZvYoLzIlvIkoMZQpcouY0kAAgX24sFCWOJ3cFeqDIUyO95twIH1MC0vmyvOh/3rbO8DAAJlbgPU4T9S4YP6rfDkPbzrScWqSmDTnNR9cIKCuhXCzmmQnwuhBRCaKDEbYbcIHXZHUODjd3jdlRoTvaNTxudNjYvFAZg1zUFe6JBYTAsTSpSMTlKg6RPcewNfHIcxVBZB3UKonpk1NhI5SokAhHv0rkIdQHwQLjdBe9RZeDwW+WFvNRT9aYg7wRmyQyA1KgwXL7/2YpxrhO9D/yeeRnE+HN0IZQWYarK6vFSaDYAKv7RGfnC9/unkxSF1+vpG6B7gWvoBGuviBy3s97oZmLx8Cl4Xg4/bOJhejxkc2ymx6grWBn2TNwkWMrCijNWHtklPOpb1Xpy5rdO7TZ696qLK6QJmQoClAdorp7LuYK3Ex3OWONWgu8J91Hf0ELAb2YbA3BLCgSkcPhGSq3bmjjj7QGclYV/fMMuTJqUK5Bp0T/XQ7BnhwpFt8tlp/29i0vUHOIpBwQAAAABJRU5ErkJggg==" />
                                    &nbsp;&nbsp;
                                    <asp:Label ID="Label100" runat="server" Font-Bold="true" Font-Size="Small" Text='<%# Eval("LoginName") %>' />
                                </div>
                                <asp:HiddenField runat="server" ID="hf_LoginName" />
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
            <dx:ASPxPopupControl ID="pop_NewUser" ClientInstanceName="pop_NewUser" runat="server" Width="320" Modal="True" HeaderText="New user" ShowCloseButton="true"
                CloseAction="CloseButton" ShowPageScrollbarWhenModal="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl6" runat="server">
                        <div>
                            <asp:Label runat="server" Font-Bold="true" Text="Username" />
                        </div>
                        <div>
                            <asp:TextBox runat="server" ID="txt_NewUsername" Width="95%" Font-Size="Small" />
                        </div>
                        <br />
                        <div>
                            <asp:Label ID="Label9" runat="server" Font-Bold="true" Text="Password" />
                        </div>
                        <div>
                            <asp:TextBox runat="server" ID="txt_NewPassword" Width="95%" Font-Size="Small" TextMode="Password" />
                        </div>
                        <br />
                        <br />
                        <div class="flex flex-justify-content-center mb-10">
                            <asp:Button ID="Button4" runat="server" Text="Create" Width="60" OnClick="btn_NewUserCreate_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="Button5" runat="server" Text="Cancel" Width="60" OnClientClick="pop_NewUser.Hide()" />
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_User" ClientInstanceName="pop_User" runat="server" Width="960px" Height="600px" HeaderText="Profile" Modal="True" ShowCloseButton="true"
                CloseAction="CloseButton" ShowPageScrollbarWhenModal="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AutoUpdatePosition="True">
                <HeaderStyle Font-Size="Small" />
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <div class="flex flex-justify-content-between mb-10" style="padding: 5px;">
                                    <div class="flex">
                                        <asp:Image runat="server" ImageUrl="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAAsQAAALEBxi1JjQAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAALnSURBVEiJnZXbb0xRFMZ/68y0My3tVGlnOm2FiLrfhSZEQlIqExX+ABFvHoSIxOUBIUEfCR69iCASQaIkJOpF44FeBJVGXIrpdDoj1esYPcvDdGp0zjml39te39rfd/Y6a68tTIB4XH0JpUaVeQJ+AIWICO88wqPiYul12i92xLeorhThpMIWINcm7afAQ9PkZHmpNP+TQWen5rnzuKiwGzCcvi4DJsqVRD/7Zs+WYVuDSET9Iy7uAWvGK/wyoXO0GJU+cFtbP3eNsN3vl0iWwYcP6vUU0AiszdyhQMNbuPMGhpKpWH4O1C2C0HzLGr/gJxuCQRmEjBJ4C7g0XhzgZitcb/0jDjCYhBstcKvN8hSryOF8emEAfInostGa/4XuAWhotxQB4H47RPstCGHPt6iuHDNwuTiNxQ9tDYOp9gYjJrR1WVIGwnEAIx5X32grZmE4aRX9G0P2ObWxmBYaCWUzNn1e4ZvYoLzIlvIkoMZQpcouY0kAAgX24sFCWOJ3cFeqDIUyO95twIH1MC0vmyvOh/3rbO8DAAJlbgPU4T9S4YP6rfDkPbzrScWqSmDTnNR9cIKCuhXCzmmQnwuhBRCaKDEbYbcIHXZHUODjd3jdlRoTvaNTxudNjYvFAZg1zUFe6JBYTAsTSpSMTlKg6RPcewNfHIcxVBZB3UKonpk1NhI5SokAhHv0rkIdQHwQLjdBe9RZeDwW+WFvNRT9aYg7wRmyQyA1KgwXL7/2YpxrhO9D/yeeRnE+HN0IZQWYarK6vFSaDYAKv7RGfnC9/unkxSF1+vpG6B7gWvoBGuviBy3s97oZmLx8Cl4Xg4/bOJhejxkc2ymx6grWBn2TNwkWMrCijNWHtklPOpb1Xpy5rdO7TZ696qLK6QJmQoClAdorp7LuYK3Ex3OWONWgu8J91Hf0ELAb2YbA3BLCgSkcPhGSq3bmjjj7QGclYV/fMMuTJqUK5Bp0T/XQ7BnhwpFt8tlp/29i0vUHOIpBwQAAAABJRU5ErkJggg==" />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:Label runat="server" ID="lbl_LoginName" Font-Bold="True" Font-Size="Large" />
                                    </div>
                                    <div class="flex flex-justify-content-end" style="width: 100%;">
                                        <asp:Button runat="server" ID="btn_UserSave" Text="Save" OnClick="btn_UserSave_Click" Visible="false" />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:Button runat="server" ID="btn_UserCancel" Text="Cancel" OnClick="btn_UserCancel_Click" Visible="false" />
                                        <asp:Button runat="server" ID="btn_UserDel" BorderColor="Red" BackColor="Red" ForeColor="White" Text="Delete" OnClick="btn_UserDel_Click" />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:Button runat="server" ID="btn_ChangePassword" BorderColor="Green" BackColor="Green" ForeColor="White" Text="Change password ..." OnClick="btn_ChangePassword_Click" />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:Button runat="server" ID="btn_UserEdit" Text="Edit" OnClick="btn_UserEdit_Click" />
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
                                                <asp:Button runat="server" ID="btn_BuAdd" Text="Add" Width="60" OnClick="btn_BuAdd_Click" />
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
                                                <asp:Button runat="server" ID="btn_BuDel" ForeColor="Red" Text="Delete" Width="60" OnClick="btn_BuDel_Click" Visible="false" />
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
                                <asp:PostBackTrigger ControlID="btn_UserDel" />
                                <asp:PostBackTrigger ControlID="btn_ChangePassword" />
                                <asp:PostBackTrigger ControlID="btn_BuDel" />
                                <asp:PostBackTrigger ControlID="btn_BuAdd" />
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
            <dx:ASPxPopupControl ID="pop_UserConfirmDelete" ClientInstanceName="pop_UserConfirmDelete" runat="server" Width="540" Modal="True" HeaderText="Confirmation"
                ShowCloseButton="true" CloseAction="CloseButton" ShowPageScrollbarWhenModal="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
                        <div class="flex flex-justify-content-center mb-10">
                            <asp:Label runat="server" ID="lbl_UserConfirmDelete" Font-Size="Small" />
                        </div>
                        <div style="text-align: center; width: 100%;">
                            <asp:Label runat="server" ID="Label11" Font-Size="Smaller" ForeColor="Red" Text="*All roles and locations will be removed from all business units." />
                        </div>
                        <br />
                        <br />
                        <div class="flex flex-justify-content-center mb-10">
                            <asp:Button ID="Button1" runat="server" Text="Yes" Width="60" OnClick="btn_UserConfirmDelete_Yes_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="Button3" runat="server" Text="No" Width="60" OnClientClick="pop_UserConfirmDelete.Hide()" />
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
                                    <asp:TextBox ID="txt_ChangePassword" runat="server" Text="" Width="180px" TextMode="Password" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Confirm Password" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_ChangePasswordConfirm" runat="server" Text="" Width="180px" TextMode="Password" />
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
