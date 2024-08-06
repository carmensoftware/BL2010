<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserManage.aspx.cs" Inherits="Option_LocalConfig_User_UserManage" %>

<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="switch.css">
    <title></title>
    <style type="text/css">
        .Background
        {
            filter: alpha(opacity=90);
            opacity: 0.8;
        }
        .Popup
        {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 1px;
            padding-left: 1px;
            width: 300px;
            height: 240px;
        }
        .Popup2
        {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 1px;
            padding-left: 1px;
            width: 300px;
            height: 140px;
        }
        .lbl
        {
            font-size: 16px;
            font-style: italic;
            font-weight: bold;
        }
        
        .style1
        {
            width: 15px;
        }
    </style>
    <script language="javascript">
        function windowsclose() {
            //             window.close();
            alert('TEST');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" method="get">
    <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </cc1:ToolkitScriptManager>
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <contenttemplate>
        <%--BEGIN: PopUp Reset Password--%>
            <cc1:ModalPopupExtender ID="mp3" runat="server" PopupControlID="Panl3" TargetControlID="HiddenField3"
            CancelControlID="BtnCancelReset" BackgroundCssClass="Background">
        </cc1:ModalPopupExtender>
            <asp:HiddenField ID="HiddenField3" runat="server" /> 
            <asp:Panel ID="Panl3" runat="server" style="display:none" CssClass="Popup2">
            <table width="300px">
                <tr height="15px">
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                    <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="Password" />
                </td>
                    <td width="2px">&nbsp;</td>
                    <td>
                     <dx:ASPxTextBox ID="txtResetPass" runat="server" Font-Names="Tahoma" Text="" 
                        Width="200px" Password="True" />
                </td>
                </tr>
                <tr>
                    <td><dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="Re Password"></dx:ASPxLabel></td>
                    <td>&nbsp;</td>
                    <td><dx:ASPxTextBox ID="txtResetPass2" runat="server" Font-Names="Tahoma" Text="" 
                            Width="200px" Password="True"> </dx:ASPxTextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td><asp:Label ID="lblMesageRepass" runat="server" Text="" /></td>
                </tr>
            </table>
            <center>
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="BtnSavePassword" runat="server" Text="confirm" 
                                onclick="BtnSavePasswords_Click" />
                        </td>
                        <td width="10px">
                        </td>
                        <td>
                            <asp:Button ID="BtnCancelReset" runat="server" Text="cancel" />
                        </td>
                    </tr>
                </table>
            </center>
            </asp:Panel>
        <%-- END: PopUp Reset Password--%>

        <%-- BEGIN: PopUp Delete User Confirm--%>
            <cc1:ModalPopupExtender ID="mp1" runat="server" PopupControlID="Panl1" TargetControlID="btnRefresh"
            CancelControlID="BtnCancelDelete" BackgroundCssClass="Background">
        </cc1:ModalPopupExtender>
            <asp:HiddenField ID="btnRefresh" runat="server" /> 
            <asp:Panel ID="Panl1" runat="server" align="center" style = "display:none" CssClass="Popup2">
                <br /><br />
                <table width="300px">
        <tr>
            <td><center><asp:Label ID="lblDelete" runat="server" Text="Do You Want To Delete User"></asp:Label>
            <asp:Label ID="lblDeletes" runat="server" Text="UserName" Font-Bold="True"></asp:Label>
            </center></td>
        </tr>
         </table>
                <br /> <br /> <br />
                <table width="300px">
        <tr>
            <td><center><asp:Button ID="BtnConfirmDelete" runat="server" Text="Confirm" Width="60px" 
                    onclick="BtnConfirmDelete_Click" /></center></td>
            <td></td>
            <td><center><asp:Button ID="BtnCancelDelete" runat="server" Text="Cancel"  Width="60px"  /></center></td>
       </tr>
        </table>
            </asp:Panel>
        <%-- END: PopUp Delete User Confirm --%>

        <%--PopUp Insert BuUser Confirm--%>
            <cc1:ModalPopupExtender ID="mp2" runat="server" PopupControlID="Panel1" TargetControlID="HiddenField1"
            CancelControlID="BtnCancelBu" BackgroundCssClass="Background">
        </cc1:ModalPopupExtender>
            <asp:HiddenField ID="HiddenField1" runat="server" /> 
            <asp:Panel ID="Panel1" runat="server" align="center" style = "display:none" CssClass="Popup">
                <br /><br />
                <table width="300px">
        <tr>
             <td><center style="height: 20px">
            <dx:ASPxLabel ID="LabelBuCode" runat="server" Font-Names="Tahoma" Text="Business Unit">
            </dx:ASPxLabel> 
            </td>
        </tr>
        <tr>
            <td><center style="height: 120px">
                   <dx:ASPxListBox ID="ListBuIn" runat="server"  Width='80px' Height='100px' Font-Size="X-Small">
                   </dx:ASPxListBox>
            </center></td>
        </tr>
            <table>
                <tr>
                    <td>
                        <center>
                            <asp:Button ID="BtnSelectBu" runat="server" onclick="BtnSelectBu_Click" 
                                Text="Confirm" Width="60px" />
                        </center>
                    </td>
                    <td width="20px">
                    </td>
                    <td>
                        <center>
                            <asp:Button ID="BtnCancelBu" runat="server" Text="Cancel" Width="60px" />
                        </center>
                    </td>
                </tr>
            </table>
            </tr>
        </table>
            </asp:Panel>
      <%--END: PopUp Insert BuUser Confirm--%>

<%----------------------------------------------------- BEGIN: USER Profile ------------------------------------- --%>
            <br />
            <table align="left" border="0" cellpadding="0" cellspacing="0" style="margin-right:10px;width:230px;" >
                <tr>
                    <td>
                        <table border="0" cellpadding="0" cellspacing="0"  width="230px">
                            <tr width="230px">
                                <td width="20px"><dx:ASPxLabel  ID="ASPxLabel8" runat="server" Text="Status" 
                                    Font-Bold="True" Font-Names="Tahoma" />
                                </td>
                                <td class="switch">
                                    <input ID="ActiveUser" runat="server" class="switch-input" name="view" 
                                        type="radio" value="ActiveUser">
                                    <label runat="server"  class="switch-label switch-label-off" for="ActiveUser">
                                        Active</label>
                                    <input ID="InActiveUser" runat="server" class="switch-input" name="view" 
                                        type="radio" value="InActiveUser">
                                    <label runat="server" class="switch-label switch-label-on" for="InActiveUser">
                                        Inactive</label> <span class="switch-selection"></span>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>                    
                    <td height="10px">
                    </td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Bold="True" 
                            Font-Names="Tahoma" Text="Login Name" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxTextBox ID="TxtLoginName" runat="server" Font-Bold="False" 
                            Font-Names="Tahoma" Width="230px" />
                    </td>
                </tr>
                <tr>
                    <td height="5px">
                    </td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Bold="True" 
                            Font-Names="Tahoma" Text="Password" />
                    </td>
                </tr>
                <tr>
                    <td height="20px">
                        <dx:ASPxTextBox ID="txtPassword" runat="server" Font-Names="Tahoma" 
                            Width="230px" Password="True" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxButton ID="btnResetPassword" runat="server"  Text="Reset Password" 
                             Width="150px" onclick="ResetPassword_Click" />
                    </td>
                </tr>
                <tr>
                    <td height="15px">
                    </td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel3" runat="server" Font-Bold="True" 
                            Font-Names="Tahoma" Text="First Name ">
                        </dx:ASPxLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxTextBox ID="TxtFirstName" runat="server" Font-Names="Tahoma" 
                            Width="230px" />
                    </td>
                </tr>
                <tr>
                    <td height="10px">
                    </td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel4" runat="server" Font-Bold="True" 
                            Font-Names="Tahoma" Text="Middle Name" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxTextBox ID="TxtMidName" runat="server" Font-Names="Tahoma" 
                            Width="230px" />
                    </td>
                </tr>
                <tr>
                    <td height="10px">
                    </td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel5" runat="server" Font-Bold="True" 
                            Font-Names="Tahoma" Text="Last Name" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxTextBox ID="TxtLastname" runat="server" Font-Names="Tahoma" 
                            Width="230px" />
                    </td>
                </tr>
                <tr>
                    <td height="10px">
                    </td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel6" runat="server" Font-Bold="True" 
                            Font-Names="Tahoma" Text="Email" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxTextBox ID="TxtEmail" runat="server" Font-Names="Tahoma" Width="230px" />
                    </td>
                </tr>
                <tr>
                    <td height="10px">
                    </td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel7" runat="server" Font-Bold="True" 
                            Font-Names="Tahoma" Text="Job Title" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxTextBox ID="TxtJobTitle" runat="server" Font-Names="Tahoma" 
                            Width="230px" />
                    </td>
                </tr>
                <tr>
                    <td height="40px">
                    </td>
                </tr>
                <tr>
                    <td>
                        <table border="0" cellspacing="0" colspacing="0" width="230px">
                            <tr>
                                <td width="100px">
                                    <dx:ASPxButton ID="BtnSave" runat="server" Text="Edit"
                                        onclick="BtnSave_Click1" />
                                </td>
                                <td width="20px"></td>
                                 <td>
                                    <dx:ASPxButton ID="BtnSaveFinal" runat="server" Text="Save"
                                        OnClick="BtnSaveFinal_Click" />
                                </td>
                                <td>
                                    <dx:ASPxButton ID="BtnCancel" runat="server" Text="Cancel"
                                        OnClick="ASPxButton1_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <dx:ASPxLabel ID="LblMessage" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
<%----------------------------------------------------- END: USER Profile ------------------------------------- --%>
        <%------------------------------------------------------------ ROLE AND LOCATION INSERT-------------------------------%>
    

            <table cellpadding="0" style="font-size:small;font-family:Tahoma;">
                <tr>
                    <td width="100px">
                        <table>
                            <tr>
                                <td width="38%"><asp:Label ID="Selected" runat="server" Text="BU" /></td>
                                <td style="text-align:center;"> 
                                    <asp:Button ID="BtnIn" runat="server" Text="+" 
                                        Width="24px" Height="18px" onclick="BtnIn_Click" />
                                    <asp:Button ID="BtnOut" runat="server" Text="-" 
                                        Width="24px" Height="18px" onclick="BtnOut_Click"/>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="5">&nbsp;</td>            
                    <td width="190px">
                        <asp:Label ID="Label1" runat="server" Text="Role"/>
                    </td>
                    <td width="5">&nbsp;</td>    
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Location" />
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top;">              
                        <dx:ASPxListBox ID="ListBuOn" runat="server" AutoPostBack="true" 
                            Font-Size="X-Small" Height="390px" Width="100px" 
                            OnSelectedIndexChanged="ListBuOn_SelectedIndexChanged" >
                        </dx:ASPxListBox>
                    </td>
                    <td></td>   
                    <td style="vertical-align: top;">              
                        <dx:ASPxListBox ID="ListRoleName" runat="server" SelectionMode="CheckColumn"
                            Font-Size="X-Small" Height="390px" Width="190px">
                            <ItemStyle BackColor="White" ForeColor="Black">
                            <SelectedStyle BackColor="White" ForeColor="Black">
                            </SelectedStyle>
                            </ItemStyle>
                        </dx:ASPxListBox>
                    </td>
                    <td></td>   
                    <td style="vertical-align: top;">   
                        <dx:ASPxListBox ID="ListLocation" runat="server" SelectionMode="CheckColumn" 
                            Font-Size="X-Small" Height="390px" Width="380px">
                            <ItemStyle BackColor="White" ForeColor="Black">
                            <SelectedStyle BackColor="White" ForeColor="Black">
                            </SelectedStyle>
                            </ItemStyle>
                        </dx:ASPxListBox>
                    </td>
                </tr>
            </table>
<%--------------------------------------------- ROLE AND LOCATION INSERT---------------------------------------------%>
        </contenttemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
