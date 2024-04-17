<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="UserEdit_old.aspx.cs" Inherits="BlueLedger.PL.Option.Admin.Security.User.UserEdit" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
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
                                        <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, lbl_Title %>" SkinID="LBL_HD_WHITE"></asp:Label>
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
                                    <dx:MenuItem Name="Save" ToolTip="Save" Text="">
                                        <ItemStyle Height="16px" Width="42px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/save.png"
                                                Repeat="NoRepeat" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Back" ToolTip="Back" Text="">
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
                        <td rowspan="6" style="width: 1%;">
                        </td>
                        <td style="width: 7%">
                            <asp:Label ID="lbl_LoginName_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, lbl_LoginName_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td style="width: 30%">                            
                            <asp:TextBox ID="txt_LoginName" runat="server" Width="200px" SkinID="TXT_V1"></asp:TextBox>
                        </td>
                        <td style="width: 13%">
                            <asp:Label ID="lbl_IsActive_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, lbl_IsActive_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td style="width: 50%">
                            <asp:CheckBox ID="chk_IsActive" runat="server" SkinID="CHK_V1" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_FName_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, lbl_FName_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_FName" runat="server" Width="200px" SkinID="TXT_V1"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lbl_LastLogin_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, lbl_LastLogin_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_LastLogin" runat="server" Width="200px" ReadOnly="True"
                            SkinID="TXT_V1"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_MName_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, lbl_MName_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_MName" runat="server" Width="200px" SkinID="TXT_V1"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lbl_DepartmentCode_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, lbl_DepartmentCode_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td>
                            <dx:ASPxComboBox ID="cmb_Department" runat="server" DataSourceID="ods_Department"
                                TextField="Department" IncrementalFilteringMode="Contains" TextFormatString="{0}-{1}"
                                ValueField="DepCode" ValueType="System.String" SkinID="DDL_V1">
                                <Columns>
                                    <dx:ListBoxColumn Caption="DepartmentCode" FieldName="DepCode" />
                                    <dx:ListBoxColumn Caption="Department" FieldName="DepName" />
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
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_LName_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, lbl_LName_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_LName" runat="server" Width="200px" SkinID="TXT_V1"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lbl_Password_Nm" runat="server" 
                                Text="<%$ Resources:Option_Admin_Security_User_User, lbl_Password_Nm %>" 
                                SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Pwd" runat="server" Width="200px" Password="True" 
                                SkinID="TXT_V1" TextMode="Password"></asp:TextBox>                           
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_Mail_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, lbl_Mail_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Email" runat="server" Width="200px" SkinID="TXT_V1"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lbl_ConfirmPass" runat="server" 
                                Text="<%$ Resources:Option_Admin_Security_User_User, lbl_ConfirmPass %>" 
                                SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_PwdConfirm" runat="server" Width="200px" Password="True" 
                                SkinID="TXT_V1" TextMode="Password"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_JobTitle_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, lbl_JobTitle_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_JobTitle" runat="server" Width="200px" SkinID="TXT_V1"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lbl_DivisionCode_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, lbl_DivisionCode_Nm %>" Visible="false" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_SectionCode_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, lbl_SectionCode_Nm %>" SkinID="LBL_HD" Visible="false"></asp:Label>
                        </td>
                    </tr>
                </table>
                <dx:ASPxPopupControl ID="pop_ShowPwd" runat="server" CloseAction="CloseButton" HeaderText="<%$ Resources:Option_Admin_Security_User_User, Msg5 %>"
                    Height="120px" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                    ShowCloseButton="False" Width="360px">
                    <ContentStyle VerticalAlign="Top">
                    </ContentStyle>
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" style="width: 100%;">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_NewPassword" runat="server" SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">                                       
                                        <asp:Button ID="btn_ShowPassword_OK" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, btn_Ok %>" Width="75px" 
                                            OnClick="btn_ShowPassword_OK_Click" SkinID="BTN_V1" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="pop_PwdConfirm" runat="server" CloseAction="CloseButton"
                    HeaderText="<%$ Resources:Option_Admin_Security_User_User, Msg4 %>" Height="120px" Modal="True" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Width="420px">
                    <ContentStyle VerticalAlign="Top">
                    </ContentStyle>
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" style="width: 100%;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_Message" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, lbl_Message %>" SkinID="LBL_NR">
                                        </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <%--<dx:ASPxButton ID="btn_PwdConfirm_OK" runat="server" Text="OK" Width="75px" OnClick="btn_PwdConfirm_OK_Click">
                                        </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_PwdConfirm_OK" runat="server" Text="<%$ Resources:Option_Admin_Security_User_User, btn_Ok %>" Width="75px" 
                                            OnClick="btn_PwdConfirm_OK_Click" SkinID="BTN_V1" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <br />
            </td>
        </tr>
    </table>
</asp:Content>
