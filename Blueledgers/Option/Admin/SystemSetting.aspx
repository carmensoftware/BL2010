<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="SystemSetting.aspx.cs" Inherits="BlueLedger.PL.Option.Admin.SystemSetting" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <div class="CMD_BAR">
        <div class="CMD_BAR_LEFT">
            <asp:Image ID="img_TitleIcon" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/gear.png" Width="15px" Height="15px" />
            <asp:Label ID="lbl_Title" runat="server" Text="System Setting" SkinID="LBL_HD_WHITE"></asp:Label>
        </div>
    </div>
    <br />
    <div style="margin: 0 auto; display: block; width: 90%;">
        <%--Company Profile--%>
        <div style="display: inline-block; width: 300px; vertical-align: top; padding: 10px;">
            <div style="font-size: 1.5em; font-weight: bold;">
                <asp:LinkButton ID="btn_CompanyProfile" runat="server" Text="Company Profile" OnClick="btn_CompanyProfile_Click" ForeColor="Black" />
            </div>
            <div>
                Company name, billing, address and logo
            </div>
        </div>
        <%--Web Server--%>
        <div style="display: inline-block; width: 300px; vertical-align: top; padding: 10px;">
            <div style="font-size: 1.5em; font-weight: bold;">
                <asp:LinkButton ID="btn_WebServer" runat="server" Text="Web Server (URL)" OnClick="btn_WebServer_Click" ForeColor="Black" />
            </div>
            <div>
                Domain and sub-domain for access via web browser.
            </div>
        </div>
        <%--Mail Server--%>
        <div style="display: inline-block; width: 300px; vertical-align: top; padding: 10px;">
            <div style="font-size: 1.5em; font-weight: bold;">
                <asp:LinkButton ID="btn_MailServer" runat="server" Text="Mail Server Configuration" OnClick="btn_MailServer_Click" ForeColor="Black" />
            </div>
            <div>
                SMTP for Purchase Request, Purchase Order and others.
            </div>
        </div>
        <%--Interface--%>
        <div style="display: inline-block; width: 300px; vertical-align: top; padding: 10px;">
            <div style="font-size: 1.5em; font-weight: bold;">
                <asp:LinkButton ID="btn_InterfaceAccount" runat="server" Text="Interface to Accounting System" OnClick="btn_InterfaceAccount_Click" ForeColor="Black" />
            </div>
            <div>
                Export Receiving, Credit Note, Store Requisition to Accounting System.
            </div>
        </div>
        <%--Password Policy--%>
        <div style="display: inline-block; width: 300px; vertical-align: top; padding: 10px;">
            <div style="font-size: 1.5em; font-weight: bold;">
                <asp:LinkButton ID="btn_Password" runat="server" Text="Password Policy" OnClick="btn_Password_Click" ForeColor="Black" />
            </div>
            <div>
                Set password complexity and period of expiry.
            </div>
        </div>
         <%--File Management--%>
        <div style="display: inline-block; width: 300px; vertical-align: top; padding: 10px;">
            <div style="font-size: 1.5em; font-weight: bold;">
                <asp:LinkButton ID="btn_FileManage" runat="server" Text="File Management" OnClick="btn_FileManage_Click" ForeColor="Black" />
            </div>
            <div>
                Managing the attachment's files.
            </div>
        </div>
        <%--Other--%>
        <div style="display: inline-block; width: 300px; vertical-align: top; padding: 10px;">
            <div style="font-size: 1.5em; font-weight: bold;">
                <asp:LinkButton ID="btn_System" runat="server" Text="System" OnClick="btn_System_Click" ForeColor="Black" />
            </div>
            <div>
                Set the system default values.
            </div>
        </div>
    </div>
    <!-- Pop-up -->
    <dx:ASPxPopupControl ID="pop_Alert" runat="server" HeaderText="Warning" Modal="True" Width="480" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        ShowCloseButton="True" CloseAction="CloseButton">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl20" runat="server">
                <div style="text-align: center;">
                    <asp:Label ID="lbl_Alert" runat="server" />
                </div>
                <br />
                <div style="text-align: center;">
                    <asp:Button ID="btn_Alert_Ok" runat="server" Text="OK" Width="60px" SkinID="BTN_V1" OnClick="btn_Alert_Ok_Click" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <!-- Company Profile -->
    <dx:ASPxPopupControl ID="pop_CompanyProfile" runat="server" HeaderText="" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        ShowCloseButton="True" CloseAction="CloseButton">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <div style="width: 680px;">
                    <div class="container">
                        <div>
                            <div style="display: inline-block;">
                                <h3>
                                    <b>Company Profile</b>
                                    <asp:Label ID="lbl_BuCode" runat="server" Text="Code" />
                                </h3>
                            </div>
                            <div style="display: inline-block; float: right; margin-top: 5px; margin-right: 40px;">
                                <asp:Button ID="btn_EditBu" runat="server" Text="Edit" OnClick="btn_EditBu_Click" />
                            </div>
                        </div>
                        <p>
                            <table>
                                <tr>
                                    <td width="120">
                                        <asp:Label ID="lbl_BuName" runat="server" Text="Name" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_BuName" runat="server" Width="480" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_NameBilling" runat="server" Text="Name of Billing" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_NameBilling" runat="server" Width="480" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_Address" runat="server" Text="Address" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_Address" runat="server" Width="480" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_PostCode" runat="server" Text="Post Code" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_PostCode" runat="server" Width="480" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_Phone" runat="server" Text="Phone" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_Phone" runat="server" Width="480" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_Fax" runat="server" Text="Fax" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_Fax" runat="server" Width="480" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_Email" runat="server" Text="Email" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_Email" runat="server" Width="480" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_TaxId" runat="server" Text="Tax ID" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_TaxId" runat="server" Width="480" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_BuLogo" runat="server" Text="Company Logo" />
                                    </td>
                                    <td>
                                        <asp:Image ID="img_BuLogo" runat="server" Width="160" Height="100"></asp:Image>
                                        <br />
                                        <asp:FileUpload ID="FileUpload1" runat="server" ClientIDMode="Static" onchange="this.form.submit()" ForeColor="White" Visible="false" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <div style="text-align: right; margin-right: 40px;">
                                <asp:Button ID="btn_SaveBu" runat="server" Text="Save" OnClick="btn_SaveBu_Click" Visible="false" />
                                <asp:Button ID="btn_CancelBu" runat="server" Text="Cancel" OnClick="btn_CancelBu_Click" Visible="false" />
                            </div>
                        </p>
                    </div>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <!-- Mail Server -->
    <dx:ASPxPopupControl ID="pop_MailServer" runat="server" HeaderText="" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        ShowCloseButton="True" CloseAction="CloseButton">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                <div style="width: 480px;">
                    <div class="container">
                        <div>
                            <div style="display: inline-block;">
                                <h3>
                                    <b>Mail Server</b>
                                </h3>
                            </div>
                            <div style="display: inline-block; float: right; margin-top: 5px; margin-right: 40px;">
                                <asp:Button ID="btn_EditSMTP" runat="server" Text="Edit" OnClick="btn_EditSMTP_Click" />
                            </div>
                        </div>
                        <table>
                            <tr>
                                <td width="120">
                                    <asp:Label ID="lbl_ServerName" runat="server" Text="SMTP Server:" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_ServerName" runat="server" Width="300" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_Port" runat="server" Text="Port:" />
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <dx:ASPxSpinEdit ID="txt_Port" runat="server" Width="80" NumberType="Integer" SpinButtons-ShowIncrementButtons="true" Number="587" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_PortDefault" runat="server" Text="Default: 587" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_SSL" runat="server" Text="SSL" />
                                </td>
                                <td>
                                    <dx:ASPxCheckBox ID="check_SSL" runat="server" AutoPostBack="false" OnCheckedChanged="check_SSL_CheckedChanged" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_Authen" runat="server" Text="Authentication" />
                                </td>
                                <td>
                                    <dx:ASPxCheckBox ID="check_Authen" runat="server" AutoPostBack="false" OnCheckedChanged="check_Authen_CheckedChanged" Checked="true" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table>
                            <tr>
                                <td colspan="2">
                                    <b>Notification</b>
                                </td>
                            </tr>
                            <tr>
                                <td width="120">
                                    <asp:Label ID="lbl_SenderName" runat="server" Text="Sender Name:" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Name" runat="server" Width="300" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_Username" runat="server" Text="User Name:" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Username" runat="server" Width="300" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_Password" runat="server" Text="Password" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Password" runat="server" TextMode="Password" Width="300" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btn_TestSendMail" runat="server" Text="Test sending PR/SR" OnClick="btn_TestSendMail_Click" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_TestReceiver" runat="server" Width="300" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_TestReceiver" runat="server" ForeColor="Blue" Width="300" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table>
                            <tr>
                                <td colspan="2">
                                    <b>Purchase Order</b>
                                </td>
                            </tr>
                            <tr>
                                <td width="120">
                                    <asp:Label ID="Label9" runat="server" Text="Sender Name:" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_PoSenderName" runat="server" Width="300" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label10" runat="server" Text="User Name:" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_PoUsername" runat="server" Width="300" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label11" runat="server" Text="Password" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_PoPassword" runat="server" TextMode="Password" Width="300" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Subject" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_PoSubject" runat="server" Width="300" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="Message" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_PoMessageBody" runat="server" Width="300" TextMode="MultiLine" Rows="5" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label14" runat="server" Text="Cc:" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_PoCc" runat="server" Width="300" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label17" runat="server" Text="Cc:" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chk_PoCcHod" runat="server" Text="Head of Department" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btn_TestMailPo" runat="server" Text="Test sending PO" OnClick="btn_TestMailPo_Click" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_TestMailPo" runat="server" Width="300" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="Label26" runat="server" ForeColor="Blue" Width="300" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div style="text-align: right; margin-right: 40px;">
                            <asp:Button ID="btn_SaveSMTP" runat="server" Text="Save" OnClick="btn_SaveSMTP_Click" />
                            <asp:Button ID="btn_CancelSMTP" runat="server" Text="Cancel" OnClick="btn_CancelSMTP_Click" />
                        </div>
                    </div>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <!-- Account Interface -->
    <dx:ASPxPopupControl ID="pop_AccountInterface" runat="server" HeaderText="" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        ShowCloseButton="True" CloseAction="CloseButton">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                <div style="width: 580px;">
                    <div class="container">
                        <div>
                            <div style="display: inline-block;">
                                <h3>
                                    <b>Account Interface</b>
                                </h3>
                            </div>
                            <div style="display: inline-block; float: right; margin-top: 5px; margin-right: 40px;">
                                <asp:Button ID="btn_InterfaceEdit" runat="server" Text="Edit" OnClick="btn_InterfaceEdit_Click" />
                            </div>
                        </div>
                        <br />
                        <asp:Panel ID="pn_Interface" runat="server">
                            <table>
                                <tr>
                                    <td width="120">
                                        <asp:Label ID="Label3" runat="server" Text="API Server:" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_InterfaceServer" runat="server" Width="400" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="120">
                                        <asp:Label ID="Label4" runat="server" Text="Authorize Token:" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_InterfaceToken" runat="server" Width="400" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="120">
                                        <asp:Label ID="Label7" runat="server" Text="Account Code" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_InterfaceAccCode" runat="server" Width="400" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="120">
                                        <asp:Label ID="Label8" runat="server" Text="Department" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_InterfaceDepCode" runat="server" Width="400" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="120">
                                        <asp:Label ID="Label6" runat="server" Text="Vendor" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_InterfaceVendor" runat="server" Width="400" />
                                    </td>
                                </tr>
                            </table>
                            <%--<table>
                                <tr>
                                    <td width="120">
                                        <asp:Label ID="Label3" runat="server" Text="Server:" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_InterfaceServer" runat="server" Width="300" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="Port:" />
                                    </td>
                                    <td>
                                        <dx:ASPxSpinEdit ID="txt_InterfacePort" runat="server" Width="80" NumberType="Integer" SpinButtons-ShowIncrementButtons="true" Number="3306" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" Text="User Name:" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_InterfaceUserName" runat="server" Width="300" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label7" runat="server" Text="Password" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_InterfacePassword" runat="server" TextMode="Password" Width="300" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label8" runat="server" Text="Database:" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_InterfaceDatabase" runat="server" Width="300" />
                                    </td>
                                </tr>
                            </table>--%>
                        </asp:Panel>
                        <br />
                        <div style="text-align: right; margin-right: 40px;">
                            <asp:Button ID="btn_InterfaceSave" runat="server" Text="Save" OnClick="btn_InterfaceSave_Click" />
                            <asp:Button ID="btn_InterfaceCancel" runat="server" Text="Cancel" OnClick="btn_InterfaceCancel_Click" />
                        </div>
                    </div>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <!-- Password -->
    <dx:ASPxPopupControl ID="pop_Password" runat="server" HeaderText="" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        ShowCloseButton="True" CloseAction="CloseButton">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                <div style="width: 480px;">
                    <div class="container">
                        <div>
                            <div style="display: inline-block;">
                                <h3>
                                    <b>Password Policy</b>
                                </h3>
                            </div>
                            <div style="display: inline-block; float: right; margin-top: 5px; margin-right: 40px;">
                                <asp:Button ID="btn_PasswordPolicyEdit" runat="server" Text="Edit" OnClick="btn_PasswordPolicyEdit_Click" />
                            </div>
                        </div>
                        <br />
                        <asp:Panel ID="Panel_PasswordPolicy" runat="server">
                            <table>
                                <tr>
                                    <td width="120">
                                        <asp:Label ID="Label5" runat="server" Text="Minimum password length:" />
                                    </td>
                                    <td>
                                        <dx:ASPxSpinEdit ID="txt_PasswordLength" runat="server" Width="80" NumberType="Integer" SpinButtons-ShowIncrementButtons="true" Number="8" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label12" runat="server" Text="Enable complexity" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chk_ComplexityPassword" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label13" runat="server" Text="Password Expired after day(s)" />
                                    </td>
                                    <td>
                                        <dx:ASPxSpinEdit ID="txt_ExpireDays" runat="server" Width="80" NumberType="Integer" SpinButtons-ShowIncrementButtons="true" Number="8" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <div style="text-align: right; margin-right: 40px;">
                            <asp:Button ID="btn_PasswordPolicySave" runat="server" Text="Save" OnClick="btn_PasswordPolicySave_Click" />
                            <asp:Button ID="btn_PasswordPolicyCancel" runat="server" Text="Cancel" OnClick="btn_PasswordPolicyCancel_Click" />
                        </div>
                    </div>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <!-- Web Server -->
    <dx:ASPxPopupControl ID="pop_WebServer" runat="server" HeaderText="" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        ShowCloseButton="True" CloseAction="CloseButton">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
                <div style="width: 480px;">
                    <div class="container">
                        <div>
                            <div style="display: inline-block;">
                                <h3>
                                    <b>Web Server Configuration</b>
                                </h3>
                            </div>
                            <div style="display: inline-block; float: right; margin-top: 5px; margin-right: 40px;">
                                <asp:Button ID="btn_WebServerEdit" runat="server" Text="Edit" OnClick="btn_WebServerEdit_Click" />
                            </div>
                        </div>
                        <br />
                        <asp:Panel ID="Panel_WebServer" runat="server">
                            <table>
                                <tr>
                                    <td width="120">
                                        <asp:Label ID="Label15" runat="server" Text="Domain/IP" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_Domain" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label16" runat="server" Text="Sub-Domain" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_SubDomain" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        URL:
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_URL" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <div style="text-align: right; margin-right: 40px;">
                            <asp:Button ID="btn_WebServerSave" runat="server" Text="Save" OnClick="btn_WebServerSave_Click" />
                            <asp:Button ID="btn_WebServerCancel" runat="server" Text="Cancel" OnClick="btn_WebServerCancel_Click" />
                        </div>
                    </div>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <!-- System -->
    <dx:ASPxPopupControl ID="pop_System" runat="server" HeaderText="" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="True"
        CloseAction="CloseButton">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl6" runat="server">
                <div style="width: 320px;">
                    <div class="container">
                        <div>
                            <div style="display: inline-block;">
                                <h3>
                                    <b>System Parameters</b>
                                    <asp:Label ID="Label18" runat="server" Text="Code" />
                                </h3>
                            </div>
                            <div style="display: inline-block; float: right; margin-top: 5px; margin-right: 40px;">
                                <asp:Button ID="btn_EditSystem" runat="server" Text="Edit" OnClick="btn_EditSystem_Click" />
                            </div>
                        </div>
                        <p>
                            <table>
                                <tr>
                                    <td width="140">
                                        <asp:Label ID="Label19" runat="server" Text="Currency" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_SystemCurrency" runat="server" Width="120" ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label20" runat="server" Text="Tax Rate" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_SystemTaxRate" runat="server" Width="120" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label21" runat="server" Text="Service Rate" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_SystemSvcRate" runat="server" Width="120" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label22" runat="server" Text="Decimal Digit for Amount" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_SystemDigitAmt" runat="server" Width="120" ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label23" runat="server" Text="Decimal Digit for Qty" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_SystemDigitQty" runat="server" Width="120" ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label24" runat="server" Text="Cost System" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_SystemCost" runat="server" Width="120" ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label25" runat="server" Text="Enable edit the commit document" />
                                    </td>
                                    <td>
                                        <dx:ASPxCheckBox ID="chk_EnableEditCommit" runat="server" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                        </p>
                        <div style="text-align: right; margin-right: 40px;">
                            <asp:Button ID="btn_SaveSystem" runat="server" Text="Save" OnClick="btn_SaveSystem_Click" Visible="false" />
                            <asp:Button ID="btn_CancelSystem" runat="server" Text="Cancel" OnClick="btn_CancelSystem_Click" Visible="false" />
                        </div>
                    </div>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
