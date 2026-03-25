<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Test.aspx.cs" Inherits="Test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        DocNo:
        <asp:TextBox ID="txt_DocNo" runat="server" Text="PR26030084" />
        <asp:TextBox ID="txt_WfId" runat="server" Text="1" />
        <asp:TextBox ID="txt_WfStep" runat="server" Text="2" />
        <asp:TextBox ID="txt_ConnStr" runat="server" Width="500" Text="Data Source=192.168.136.1; Initial Catalog=CafeDelMarPhuket_PK; User ID=bl; Password=kikuCH4Ya6HUpr295AthajLpa6LrL3o2" />
    </div>
    <br />
    <div>
        <asp:TextBox ID="txt_Approvals" runat="server" Width="100%" Text="SUPPORT,RoleID_6,RoleID_5,RoleID_3,#ABM,#BAR,#BEACH,#CA,#COST,#DOF,#DOME,#DOPS,#EN,#EXCHEF,#EXSOUS,#FO,#HK,#HOST,#HRM,#IT,#LS,#OPM,#OPM2,#PM,#PM_BAR,#PM_DOFB,#PM_ERDCHEF,#PM_EXCHEF,#PM_HRA,#POOL,#RETAIL,#SECURE,#STORE,#support@carmen" />
    </div>
    <br />
    <div>
        <asp:Button runat="server" ID="btn_Get" Text="Get" OnClick="btn_Get_Click" />
    </div>
    <br />
    <div>
        <asp:TextBox ID="txt_Emails" runat="server" Width="100%" Text="" />
    </div>
    </form>
</body>
</html>
