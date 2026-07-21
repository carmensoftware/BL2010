<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ResetPassword.aspx.cs" Inherits="Option_Admin_Security_ResetPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reset Password</title>
    <style>
        body
        {
            font-family: 'Segoe UI' , Tahoma, Geneva, Verdana, sans-serif;
            background-color: #f4f7f6;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
            margin: 0;
        }
        .reset-container
        {
            background-color: #fff;
            padding: 40px;
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
            width: 350px;
        }
        h1, h2
        {
            text-align: center;
            color: #333;
            margin-bottom: 20px;
        }
        .form-group
        {
            margin-bottom: 20px;
        }
        label
        {
            display: block;
            margin-bottom: 8px;
            color: #666;
            font-size: 14px;
        }
        input[type="password"], input[type="text"]
        {
            width: 100%;
            padding: 10px;
            border: 1px solid #ddd;
            border-radius: 4px;
            box-sizing: border-box;
        }
        button
        {
            width: 100%;
            padding: 12px;
            background-color: #2c3e50;
            color: white;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 16px;
        }
        button:hover
        {
            background-color: #1a252f;
        }
        .message
        {
            text-align: center;
            margin-top: 15px;
            font-size: 14px;
            color: #d9534f;
        }
    </style>
</head>
<body>
    <form id="form2" runat="server">
    <div class="reset-container">
        <h1>
            Password is expired.
        </h1>
        <h2>
            Please change password.
        </h2>
        <div class="form-group">
            <label for="txtEmail">
                Username
            </label>
            <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="txtOldPassword">
                Old Password</label>
            <asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password"></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="txtNewPassword">
                New Password</label>
            <asp:TextBox ID="TextBox1" runat="server" TextMode="Password"></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="txtConfirmPassword">
                Confirm Password</label>
            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
        </div>
        <br />
        <asp:Button ID="btnReset" runat="server" Width="100" Text="Reset" OnClick="btnReset_Click" />
        <asp:Label ID="lblMessage" runat="server" CssClass="message"></asp:Label>
    </div>
    </form>
</body>
</html>
