<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BuList.aspx.cs" Inherits="BuList" Theme="Default" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <%--<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />--%>
    <meta charset="UTF-8">
    <title>Blueledgers.com</title>
</head>
<body>
    <form id="form1" runat="server">
    <table width="95%" align="center" border="0" cellpadding="0" cellspacing="0">
        <tr style="height: 100px;">
            <td colspan="2">
                <asp:ImageButton ID="imgb_Home" runat="server" ImageUrl="~/App_Themes/Default/Images/login/_LOGO.png" Width="68px" />
            </td>
        </tr>
        <tr style="height: 25px; background-color: #4d4d4d;">
            <td>
                <asp:Label ID="lbl_Title" runat="server" SkinID="BULIST_TITLE" Text="Select Business Unit"></asp:Label>
            </td>
            <td align="right">
                <asp:ImageButton ID="imgbtnBack" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" OnClick="imgbtnBack_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="padding-top: 5px;">
                <asp:GridView ID="grd_BU" runat="server" SkinID="GRD_V1" Width="100%" AutoGenerateColumns="False" EnableModelValidation="True" OnRowDataBound="grd_BU_RowDataBound"
                    OnSelectedIndexChanged="grd_BU_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="BuCode" HeaderText="Buiness Unit">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left"  />
                        </asp:BoundField>
                        <asp:CommandField HeaderText="Description" ShowSelectButton="True" />
                        <asp:TemplateField HeaderText="Display Language">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddl_Lang" runat="server" Width="300px">
                                </asp:DropDownList>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:CommandField SelectText="Go" ShowSelectButton="True">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:CommandField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lbl_MsgError" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
