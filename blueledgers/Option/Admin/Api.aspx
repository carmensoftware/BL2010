<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="Api.aspx.cs" Inherits="BlueLedger.PL.Option.Admin.Api" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--Flex--%>
    <style>
        .flex
        {
            display: flex !important;
        }
        
        .flex-justify-content-start
        {
            justify-content: flex-start;
        }
        .flex-justify-content-end
        {
            justify-content: flex-end;
        }
        .flex-justify-content-center
        {
            justify-content: center;
        }
        .flex-justify-content-between
        {
            justify-content: space-between;
        }
        .flex-row
        {
            flex-flow: row !important;
        }
        .flex-columm
        {
            flex-flow: column !important;
        }
        
        .flex-wrap
        {
            flex-wrap: wrap !important;
        }
        .mt-10
        {
            margin-top: 10px;
        }
        .mt-20
        {
            margin-top: 20px;
        }
        .mt-30
        {
            margin-top: 30px;
        }
        .mb-10
        {
            margin-bottom: 10px;
        }
        .mb-20
        {
            margin-bottom: 20px;
        }
        .mb-30
        {
            margin-bottom: 30px;
        }
        .width-100
        {
            width: 100% !important;
        }
        
        .bg-menu-background
        {
            background-color: #4d4d4d !important;
            color: White !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <asp:GridView ID="gv" runat="server" Font-Size="Medium">
        <Columns>
        </Columns>
    </asp:GridView>
    <br />
    <h3>
        Carmen API</h3>
    <table style="width: 480px;">
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Host" />
            </td>
            <td>
                <div class="flex">
                    <asp:TextBox runat="server" ID="txt_CarmenHost" Width="100%" />
                    &nbsp;
                    <button id="btn_OpenHost" onclick="OpenLink()">
                        Open
                    </button>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" Text="Admin Token" />
            </td>
            <td>
                <asp:TextBox runat="server" ID="txt_CarmenAdminToken" Width="100%" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label3" runat="server" Text="Username" />
            </td>
            <td>
                <asp:TextBox runat="server" ID="txt_CarmenUsername" Width="100%" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label4" runat="server" Text="Password" />
            </td>
            <td>
                <asp:TextBox runat="server" ID="txt_CarmenPassword" TextMode="Password" Width="100%" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Button runat="server" ID="btn_Get" Width="80" Text="Get" OnClick="btn_Get_Click" />
            </td>
        </tr>
    </table>
    <br />
    <asp:TextBox runat="server" ID="txt_Result" TextMode="MultiLine" Rows="10" Width="100%" ReadOnly="True" />
    
    <script type="text/javascript">
        function OpenLink() {
            var url = document.getElementById('<%= txt_CarmenHost.ClientID %>').value;

            var link = document.createElement('a');
            link.href = url;
            link.target = '_blank';
            link.click();
        }
    
    </script>
</asp:Content>
