<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="User.aspx.cs" Inherits="BlueLedger.PL.Option.Admin.Security.User.User" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <script type="text/javascript" language="javascript">

//        function chkAll(obj, id) {
//            obj.find("input:checkbox").each(function () {
//                if (this != id) { this.checked = id.checked; }
//            });
//        }

        function chkAllUserRole(obj) {
            $('#<%=grd_UserRole1.ClientID%>').find("input:checkbox").each(function () {
                if (this != obj) { this.checked = obj.checked; }
            });
        }

        function chkAllUserStore(obj) {
            $('#<%=grd_UserStore1.ClientID%>').find("input:checkbox").each(function () {
                if (this != obj) { this.checked = obj.checked; }
            });
        }

    </script>
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
        <tr>
            <td align="left">
                <!-- Title & Command Bar  -->
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr style="background-color: #4d4d4d; height: 17px;">
                        <td align="left" style="padding-left: 10px;">
                            <table border="0" cellpadding="2" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, lbl_Title %>"
                                            SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right">
                            <dx:ASPxMenu ID="menu_CmdBar" runat="server" OnItemClick="menu_ItemClick" BackColor="Transparent"
                                Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle" Height="16px">
                                <ItemStyle BackColor="Transparent">
                                    <HoverStyle BackColor="Transparent">
                                        <Border BorderStyle="None" />
                                    </HoverStyle>
                                    <Paddings Padding="2px" />
                                    <Border BorderStyle="None" />
                                </ItemStyle>
                                <Items>
                                    <dx:MenuItem Name="Save" Text="" ToolTip="Save">
                                        <ItemStyle Height="16px" Width="42px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/save.png"
                                                Repeat="NoRepeat" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Text="" Name="Reset" ToolTip="Reset Password">
                                        <ItemStyle Height="16px" Width="79px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-chgpwd.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/chgpwd.png"
                                                Repeat="NoRepeat" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Delete" Text="" ToolTip="Delete">
                                        <ItemStyle Height="16px" Width="47px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png"
                                                Repeat="NoRepeat" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Print" Text="" ToolTip="Print">
                                        <ItemStyle Height="16px" Width="43px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-print.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/print.png"
                                                Repeat="NoRepeat" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem BeginGroup="True" Name="Back" Text="" ToolTip="Back">
                                        <ItemStyle Height="16px" Width="42px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/back.png"
                                                Repeat="NoRepeat" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                </Items>
                            </dx:ASPxMenu>
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="1" cellspacing="0" width="100%" class="TABLE_HD">
                    <tr>
                        <td rowspan="7" style="width: 1%;">
                        </td>
                        <td style="width: 7%">
                            <asp:Label ID="lbl_LoginName_Nm" runat="server"  Text="<%$ Resources:Option_Admin_Security_User_User, lbl_LoginName_Nm %>"
                                SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td style="width: 20%">
                            <asp:TextBox ID="txt_LoginName" Width="250"  runat="server" OnTextChanged="txt_LoginName_TextChanged"></asp:TextBox>
                        </td>
                        <td style="width: 7%">
                            <asp:Label ID="lbl_IsActive_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, lbl_IsActive_Nm %>"
                                SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td style="width: 50%">
                            <asp:CheckBox ID="chk_IsActive" runat="server" Text=" Active" font-size="Small" SkinID="CHK_V1" />
                        </td>
                         <td style="width: 7%">
                        <asp:Label ID="lbl_LastLogin_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, lbl_LastLogin_Nm %>"
                                SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td style="width: 30%">
                            <asp:TextBox ID="txt_LastLogin" Width="150"  runat="server" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_FName_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, lbl_FName_Nm %>"
                                SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_FName" Width="250"  runat="server"></asp:TextBox>
                        </td>
                               <td>
                            <asp:Label ID="lbl_MName_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, lbl_MName_Nm %>"
                                SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_MName" Width="250"  runat="server"></asp:TextBox>
                        </td>
                     </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_LName_Nm"  runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, lbl_LName_Nm %>"
                                SkinID="LBL_HD"></asp:Label>
                        </td>
                         <td>
                            <asp:TextBox ID="txt_LName" Width="250" runat="server"></asp:TextBox>
                        </td>
                         <td>
                            <asp:Label ID="lbl_Mail_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, lbl_Mail_Nm %>"
                                SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td>
                            <%--<asp:HyperLink ID="lnk_Email" runat="server" NavigateUrl="mailto:" Target="_blank">[lnk_Email]</asp:HyperLink>--%>
                            <asp:TextBox ID="txt_Email" Width="250" runat="server" Visible="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_JobTitle_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, lbl_JobTitle_Nm %>"
                                SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_JobTitle" Width="250" runat="server"></asp:TextBox>
                        </td>

                        <td>            
                           <asp:Label ID="lbl_DepartmentCode_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, lbl_DepartmentCode_Nm %>"
                                SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td>
                            <dx:ASPxComboBox ID="cmb_Department" runat="server" DataSourceID="ods_Department" Width="255px"
                                TextField="Department" IncrementalFilteringMode="Contains" TextFormatString="{0} : {1}"
                                ValueField="DepCode" ValueType="System.String" SkinID="DDL_V1">
                                <Columns>
                                     <dx:ListBoxColumn Width="120px" Caption="DepartmentCode" FieldName="DepCode" />
                                    <dx:ListBoxColumn Width="350px" Caption="Department" FieldName="DepName" />
                                </Columns>
                            </dx:ASPxComboBox>
                            <asp:ObjectDataSource ID="ods_Department" runat="server" SelectMethod="GetList" TypeName="Blue.BL.ADMIN.Department">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="hf_ConnStr" Name="connStr" PropertyName="Value"
                                        Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:HiddenField ID="hf_ConnStr" runat="server" />
                        </td>
                        <tr></tr>
                        <tr>
                         <td>
                            <asp:Label ID="lbl_Password_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, lbl_Password_Nm %>"
                                SkinID="LBL_HD" Visible="False"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Pwd0" Width="250" runat="server" Visible="False" TextMode="Password"></asp:TextBox>
                        </td>
                        </tr>
                         <tr>
                         <td>
                            <asp:Label ID="lbl_ConfirmPass" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, lbl_ConfirmPass %>"
                                SkinID="LBL_HD" Visible="False"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_PwdConfirm0" Width="250" runat="server" Visible="False" 
                                TextMode="Password"></asp:TextBox>
                      
                        </td>
                        </tr>
                         </table>
                         
                         
                         <table>
                         <td>
                            <asp:Label ID="lbl_DivisionCode_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, lbl_DivisionCode_Nm %>"
                                Visible="False" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_DivisionCode" Width="250" runat="server" Visible="False"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lbl_SectionCode_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, lbl_SectionCode_Nm %>"
                                Visible="False" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_SectionCode" Width="250" runat="server" Visible="False"></asp:TextBox>
                        </td>
               
                </table>
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr style="background-color: #4d4d4d; height: 17px;">
                        <td align="right">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td valign="top" width="50%">
                            <%--<dx:ASPxGridView ID="grd_UserRole" runat="server" Width="100%" AutoGenerateColumns="False"
                                ClientInstanceName="grd_UserRole" KeyFieldName="RoleName" OnLoad="grd_UserRole_Load">
                                <Styles>
                                    <Header HorizontalAlign="Center">
                                    </Header>
                                </Styles>
                                <SettingsPager Mode="ShowAllRecords" Visible="False">
                                </SettingsPager>
                                <Columns>
                                    <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="50px">
                                        <ClearFilterButton Visible="True">
                                        </ClearFilterButton>
                                        <HeaderTemplate>
                                            <input id="chk_SelAll" type="checkbox" onclick="grd_UserRole.SelectAllRowsOnPage(this.checked);" />
                                        </HeaderTemplate>
                                    </dx:GridViewCommandColumn>
                                    <dx:GridViewDataTextColumn Caption="Role" FieldName="RoleName" VisibleIndex="1">
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                                <Settings ShowFilterRow="True" ShowFooter="True" />
                            </dx:ASPxGridView>--%>
                            <asp:GridView ID="grd_UserRole1" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
                                SkinID="GRD_V1" Width="100%" DataKeyNames="RoleName" 
                                onselectedindexchanged="grd_UserRole1_SelectedIndexChanged" 
                                onrowcreated="grd_UserRole1_RowCreated">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderStyle Width="3%" HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                        <FooterStyle />
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="Chk_AllR" onClick="chkAllUserRole(this);" runat="server" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="Chk_ItemR" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Role" DataField="RoleName">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr style="background-color: #4d4d4d; height: 17px;">
            <td align="right">
                &nbsp;</td>
        </tr>
    </table>
    <table style="width: 100%;">
        <tr>
            <td>
                <asp:GridView ID="grd_UserStore1" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
                    SkinID="GRD_V1" Width="100%" DataKeyNames="LocationCode" 
                    onselectedindexchanged="grd_UserStore1_SelectedIndexChanged">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderStyle Width="3%" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <FooterStyle />
                            <HeaderTemplate>
                                <asp:CheckBox ID="Chk_AllS" onClick="chkAllUserStore(this);" runat="server" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="Chk_ItemS" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Code" DataField="LocationCode">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Location Name" DataField="LocationName">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <dx:ASPxPopupControl ID="pop_ConfrimDelete" runat="server" Width="300px" CloseAction="CloseButton"
        HeaderText="<%$ Resources:Option_Admin_Security_User_User, Msg %>" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" colspan="2" height="50px">
                            <asp:Label ID="lbl_MsgWarning" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, MsgWarning3 %>"
                                SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <%--<dx:ASPxButton ID="btn_ConfrimDelete" runat="server" OnClick="btn_ConfrimDelete_Click"
                                Text="Yes" Width="50px">
                            </dx:ASPxButton>--%>
                            <asp:Button ID="btn_ConfrimDelete" runat="server" OnClick="btn_ConfrimDelete_Click"
                                Text="<%$ Resources:Option_Admin_Security_User_User, btn_Yes %>" Width="50px"
                                SkinID="BTN_V1" />
                        </td>
                        <td align="left">
                            <%--<dx:ASPxButton ID="btn_CancelDelete" runat="server" OnClick="btn_CancelDelete_Click"
                                Text="No" Width="50px">
                            </dx:ASPxButton>--%>
                            <asp:Button ID="btn_CancelDelete" runat="server" OnClick="btn_CancelDelete_Click"
                                Text="<%$ Resources:Option_Admin_Security_User_User, btn_No %>" Width="50px"
                                SkinID="BTN_V1" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle HorizontalAlign="Left" />
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_ResetPwd" runat="server" HeaderText="<%$ Resources:Option_Admin_Security_User_User, Msg3 %>"
        Modal="True" ClientInstanceName="popup_ResetPwd" Height="120px" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" Width="400px" CloseAction="CloseButton" ShowCloseButton="False">
        <ContentStyle HorizontalAlign="Center" VerticalAlign="Middle">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <table border="0" cellpadding="2" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Label ID="Label18" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, MsgWarning4 %>"
                                SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr style="height: 17px">
                        <td align="left">
                            <asp:Label ID="lbl_NewPass_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, lbl_NewPass_Nm %>"
                                SkinID="LBL_NR"></asp:Label>
                        </td>
                        <td>
                            <%--<dx:ASPxTextBox ID="txt_Pwd" runat="server" Password="True" Width="200px">
                            </dx:ASPxTextBox>--%>
                            <asp:TextBox ID="txt_Pwd" runat="server" Password="True" Width="200px" SkinID="TXT_V1"
                                TextMode="Password"></asp:TextBox>
                        </td>
                    </tr>
                    <tr style="height: 17px">
                        <td align="left">
                            <asp:Label ID="lbl_ConfPass_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, lbl_ConfPass_Nm %>"
                                SkinID="LBL_NR"></asp:Label>
                        </td>
                        <td>
                            <%--<dx:ASPxTextBox ID="txt_PwdConfirm" runat="server" Password="True" Width="200px">
                            </dx:ASPxTextBox>--%>
                            <asp:TextBox ID="txt_PwdConfirm" runat="server" Password="True" Width="200px" SkinID="TXT_V1"
                                TextMode="Password"></asp:TextBox>
                        </td>
                    </tr>
                    <tr style="height: 17px">
                        <td align="center" colspan="2">
                            <table border="0">
                                <tr>
                                    <td>
                                        <%--<dx:ASPxButton ID="btn_ResetPwd_Yes" runat="server" OnClick="btn_ResetPwd_Yes_Click"
                                            Text="Yes" Width="75px">
                                        </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_ResetPwd_Yes" runat="server" OnClick="btn_ResetPwd_Yes_Click"
                                            Text="<%$ Resources:Option_Admin_Security_User_User, btn_Yes %>" Width="75px"
                                            SkinID="BTN_V1" />
                                    </td>
                                    <td>
                                        <%--<dx:ASPxButton ID="btn_ResetPwd_Close" runat="server" AutoPostBack="False" Text="Close"
                                            Width="75px">
                                            <ClientSideEvents Click="function(s, e) {
	                                                        popup_ResetPwd.Hide();
                                                        }" />
                                        </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_ResetPwd_Close" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, btn_Close %>"
                                            SkinID="BTN_V1" Width="75px" OnClick="btn_ResetPwd_Close_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_PwdConfirm" runat="server" CloseAction="CloseButton"
        HeaderText="<%$ Resources:Option_Admin_Security_User_User, Msg4 %>" Height="120px"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        ShowCloseButton="False" Width="420px">
        <ContentStyle HorizontalAlign="Left" VerticalAlign="Middle">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <table border="0" cellpadding="5" cellspacing="0" style="width: 100%;">
                    <tr style="height: 30px">
                        <td>
                            <asp:Label ID="lbl_Message" runat="server" SkinID="LBL_NR" Text="<%$ Resources:Option_Admin_Security_User_User, lbl_Message %>">
                            </asp:Label>
                        </td>
                    </tr>
                    <tr style="height: 30px">
                        <td align="center">
                            <%--<dx:ASPxButton ID="btn_PwdConfirm_OK" runat="server" Text="OK" Width="75px" OnClick="btn_PwdConfirm_OK_Click">
                            </dx:ASPxButton>--%>
                            <asp:Button ID="btn_PwdConfirm_OK" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, btn_Ok %>"
                                Width="75px" OnClick="btn_PwdConfirm_OK_Click" SkinID="BTN_V1" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_UpdatedNewPwd" runat="server" Width="400px" HeaderText="<%$ Resources:Option_Admin_Security_User_User, Msg2 %>"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <asp:Label ID="Label21" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, MsgWarning5 %>"
                    SkinID="LBL_NR"></asp:Label>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle HorizontalAlign="Left" />
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_ReachMaxUserNo" runat="server" Width="400px" HeaderText="<%$ Resources:Option_Admin_Security_User_User, Msg2 %>"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, MsgWarning2 %>"
                    SkinID="LBL_NR"></asp:Label>
                &nbsp;
            </dx:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle HorizontalAlign="Left" />
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_SavedUserRole" runat="server" Width="400px" HeaderText="<%$ Resources:Option_Admin_Security_User_User, Msg2 %>"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, MsgWarning6 %>"
                    SkinID="LBL_NR"></asp:Label>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle HorizontalAlign="Left" />
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_SavedUserStore" runat="server" Width="400px" HeaderText="<%$ Resources:Option_Admin_Security_User_User, Msg2 %>"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, MsgWarning7 %>"
                    SkinID="LBL_NR"></asp:Label>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle HorizontalAlign="Left" />
    </dx:ASPxPopupControl>
</asp:Content>
