<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountMappUpload.aspx.cs"
    Inherits="Option_Admin_Interface_AccountMap_AccountMappUpload" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <script type="text/javascript">
            function FileUploadControl_Change(FileUploadControl) {
                if (FileUploadControl.value != '') {
                    document.getElementById("<%=btnUpload.ClientID %>").disabled = false;
                }
            }
            function callbtnUploadParent() {
                parent.uploadFromChild();
            }
            function callbtnExportOfParent() {
                parent.exportFromChild();
            }
        </script>
        <div style="width: 380px;">
            <div style="border-bottom: 1px solid #C9C9C9; padding-bottom: 50px;">
                <table width="100%">
                    <tr class="popUpline">
                        <td align="left">
                            <asp:Label ID="lbltxt01" Text="Click 'Export' to get data." runat="server"></asp:Label>
                        </td>
                        <td align="right">
                            <asp:Button ID="btnExport" runat="server" Text="Export" Width="100px" OnClick="btnExport_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="padding-top: 10px;">
                <table width="100%">
                    <tr>
                        <td align="left">
                            <asp:FileUpload ID="FileUploadControl" runat="server" />
                        </td>
                        <td align="right">
                            <asp:Button ID="btnUpload" runat="server" Enabled="False" OnClick="btnUpload_Click"
                                Text="Import" Width="100px" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
