<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RecipeEditUpload.aspx.cs"
    Inherits="PT_RCP_" %>

<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../../PC/StockMovement.ascx" TagName="StockMovement" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript">
        function FileUploadControl_Change(FileUploadControl) {
            if (FileUploadControl.value != '') {
                document.getElementById("<%=btnUpload.ClientID %>").disabled = false;
            }
        }

        function callParentbtnUpload_Hide() {
            parent.btnUploadFromChild();
        }
    </script>
    <div style="width: 380px;">
        <div style="border-bottom: 1px solid #C9C9C9; padding-bottom: 50px;">
            <table width="100%">
                <tr class="popUpline">
                    <td align="left" colspan="2">
                        <asp:Label ID="lbltxt01" runat="server">Select a photo from your computer.</asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding-top: 10px;">
            <table width="100%">
                <tr>
                    <td align="left">
                        <asp:FileUpload ID="fileUploadImg" runat="server" />
                    </td>
                    <td align="right">
                        <asp:Button ID="btnUpload" runat="server" Enabled="False" OnClick="btnUpload_Click"
                            Text="Upload" Width="100px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
