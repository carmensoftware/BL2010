<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>
<%@ Register Src="~/UserControl/Password.ascx" TagName="PasswordDialog" TagPrefix="uc1" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="shortcut icon" href="blueledgers.ico">
    <style type="text/css">
        .text-title
        {
            font-family: 'Segoe UI' , -apple-system, BlinkMacSystemFont, 'Open Sans' , sans-serif;
            font-size: 2em;
            font-weight: 800;
        }
        
        .input
        {
            width: 100%;
            border: 0;
            padding: 0;
            padding: 5px;
            background-color: transparent;
            height: 1.5rem;
            line-height: 2rem;
            border-bottom: 1px solid #eee;
            color: #777;
            transition: all .2s ease-in;
            border-radius: 0.5rem;
        }
        
        .btn
        {
            /* background:  #8E49E8; */
            border: 0;
            font-size: 0.7rem;
            height: 1.5rem;
            line-height: 1.5rem;
            padding: 0 1rem;
            color: white;
            text-transform: uppercase;
            border-radius: .5rem;
            letter-spacing: .2em;
            transition: background .2s;
            background: rgb(64,150,238); /* Old browsers */
            background: -moz-linear-gradient(top, rgba(64,150,238,1) 0%, rgba(64,150,238,1) 100%); /* FF3.6+ */
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,rgba(64,150,238,1)), color-stop(100%,rgba(64,150,238,1))); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(top, rgba(64,150,238,1) 0%,rgba(64,150,238,1) 100%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(top, rgba(64,150,238,1) 0%,rgba(64,150,238,1) 100%); /* Opera 11.10+ */
            background: -ms-linear-gradient(top, rgba(64,150,238,1) 0%,rgba(64,150,238,1) 100%); /* IE10+ */
            background: linear-gradient(to bottom, rgba(64,150,238,1) 0%,rgba(64,150,238,1) 100%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#4096ee', endColorstr='#4096ee',GradientType=0 ); /* IE6-9 */
        }
        
        .btn:focus
        {
            outline: 0;
        }
        
        .btn:hover, .btn:focus
        {
            background: rgb(59,103,158); /* #A678E2; */
        }
        
        .message
        {
            width:480px;
            clear: both;
            position: relative;
            z-index: 50;
            top: 0;
            left: 0;
            right: 0;
            margin-top: 30px;
            background: #fde073;
            color: Red;
            font-weight: bold;
            text-align: center;
            line-height: 2.5;
            overflow: hidden;
            -webkit-box-shadow: 0 0 5px black;
            -moz-box-shadow: 0 0 5px black;
            box-shadow: 0 0 5px black;
        }
        
        /* ------------------------- */
        .logo
        {
            margin-left: 10%;
            width: 80px;
        }
        
        .login
        {
            top: 30%;
            left: 35%;
            position: absolute;
            width: 320px;
            font-size: 1.2em;
        }
        
        .header
        {
            width: 100%;
            border-top: 15px solid #2196f3;
        }
        
        .footer
        {
            clear: both;
            bottom: 0;
            position: absolute;
            width: 100%;
            height: auto;
            text-align: right;
            background-color: transparent;
            color: #908F90;
            font-size: 1em;
        }
        
        .background
        {
            background-color: #2196f3; /*#20B9EB;*/
            background-image: linear-gradient(to top left, rgba(255,255,255,0.25) 50%, rgba(255,255,255,0) 50%), linear-gradient(to top right, rgba(255,255,255,0.25) 50%, rgba(255,255,255,0) 50%);
            background-size: 100% 100%, 600px 100px;
            background-position: left top, left bottom;
            background-repeat: no-repeat;
            border-radius: 1rem;
            height: 480px;
        }
        
        @media only screen and (max-width: 736px)
        {
            .logo
            {
                margin: auto;
                width: 80px;
                padding-bottom: auto;
            }
        
            .login
            {
                left: auto;
                margin: auto;
                position: relative;
                width: 320px;
            }
        
            .footer
            {
                text-align: center;
            }
        }
    </style>
    <script type="text/javascript">
        function closeMessage(id) {
            document.getElementById(id).style.display = 'none';
        }
    </script>
    <title>Blueledgers</title>
</head>
<body>
    <div class="header">
        <div class="logo">
            <img alt="blueledgers" src="App_Themes/Default/Images/login/_logo.png" width="100%">
        </div>
    </div>
    <br />
    <form id="form1" runat="server" class="background">
    <asp:Login ID="LoginControl" runat="server" CssClass="login" OnLoggedIn="LoginControl_LoggedIn" OnAuthenticate="LoginControl_Authenticate">
        <LayoutTemplate>
            <div class="text-title" style="color: White;">
                <center>Login</center>
            </div>
            <br />
            <div style="color: #F0F0F0;">
                <center>Welcome back! Login to access Blueledgers</center>
            </div>
            <br />
            <br />
            <div>
                <asp:TextBox ID="UserName" runat="server" class="input" placeholder=" Username" />
                <%--<asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ErrorMessage="Username is required." ToolTip="Username is required."
                    ValidationGroup="LoginControl">*</asp:RequiredFieldValidator>--%>
            </div>
            <br />
            <div>
                <div>
                    <asp:TextBox ID="Password" runat="server" TextMode="Password" class="input" placeholder=" Password" />
                    <%--<asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" ErrorMessage="Password is required." ToolTip="Password is required."
                        ValidationGroup="LoginControl">*</asp:RequiredFieldValidator>--%>
                </div>
                <br />
                <div>
                    <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Log In" ValidationGroup="LoginControl" class="btn" Width="100%" Height="35" />
                </div>
                <br />
                <div>
                    <center><asp:CheckBox ID="RememberMe1" runat="server" Text="Remember me"></asp:CheckBox></center>
                </div>
                <br />
                <div id="MessageBox" class="message" onclick="closeMessage('MessageBox'); return false;">
                    <asp:Literal ID="FailureText" runat="server" />
                </div>
        </LayoutTemplate>
    </asp:Login>
    <uc1:PasswordDialog ID="dlgPassword" runat="server" />
    </form>
    <div class="footer">
        <div style="padding: 15px;">
            <a target="_blank" href="https://www.carmensoftware.com/">&nbsp;&copy 2020, CARMEN SOFTWARE CO.,LTD. All rights reserved.</a>
        </div>
    </div>
</body>
</html>
