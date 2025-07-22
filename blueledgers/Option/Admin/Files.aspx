<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="Files.aspx.cs" Inherits="BlueLedger.PL.Option.Admin.Files" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .card
        {
            box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
            transition: 0.3s;
            min-width: 320px;
            max-width: 640px;
        }
        
        .card:hover
        {
            box-shadow: 0 8px 16px 0 rgba(0,0,0,0.2);
        }
        .padding-0
        {
            padding: 0 !important;
        }
        .bg-menu-background
        {
            background-color: #4d4d4d !important;
            color: White !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <div class="flex flex-justify-content-between flex-align-items-center bg-menu-background" style="height: 30px;">
        <div class="flex">
            <asp:Image ID="img_Title" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/gear.png" Width="15px" Height="15px" />
            <asp:Label ID="lbl_Title" runat="server" Text="File Management" SkinID="LBL_HD_WHITE"></asp:Label>
        </div>
        <div>
            <asp:Button runat="server" ID="btn_Update" Text="Update" OnClick="btn_Update_Click" />
            &nbsp;&nbsp;&nbsp;
        </div>
    </div>
    <br />
    <div class="card mt-10" style="padding: 10px;">
        <div class="flex flex-justify-content-between">
            <asp:Label runat="server" ID="Label10" Font-Bold="true" Font-Size="Large" Text="Total" />
            <asp:Label runat="server" ID="lbl_Total" Font-Bold="true" Font-Size="Large" Text="Total - " />
        </div>
    </div>
    <hr />
    <div class="card mt-10" style="padding: 10px;">
        <div class="flex flex-justify-content-between">
            <asp:Label runat="server" ID="Label1" Font-Size="Medium" Text="Data" />
            <asp:Label runat="server" ID="lbl_Data" Font-Size="Medium" Text="Data :" />
        </div>
    </div>
    <div class="card mt-10" style="padding: 10px;">
        <div class="flex flex-justify-content-between mb-10">
            <asp:Label runat="server" ID="Label2" Font-Size="Medium" Text="Attachment" />
            <asp:Label runat="server" ID="lbl_Folder" Font-Size="Medium" Text="Data :" />
        </div>
        <div class="flex flex-justify-content-between mb-10">
            <asp:Label runat="server" ID="Label3" Font-Size="Small" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Purchase Request" />
            <asp:Label runat="server" ID="lbl_PR" Font-Size="Small" Text="0" />
        </div>
        <div class="flex flex-justify-content-between mb-10">
            <asp:Label runat="server" ID="Label4" Font-Size="Small" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Purchase Order" />
            <asp:Label runat="server" ID="lbl_PO" Font-Size="Small" Text="0" />
        </div>
        <div class="flex flex-justify-content-between mb-10">
            <asp:Label runat="server" ID="Label5" Font-Size="Small" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Receiving" />
            <asp:Label runat="server" ID="lbl_RC" Font-Size="Small" Text="0" />
        </div>
        <div class="flex flex-justify-content-between mb-10">
            <asp:Label runat="server" ID="Label6" Font-Size="Small" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Credit Note" />
            <asp:Label runat="server" ID="lbl_CN" Font-Size="Small" Text="0" />
        </div>
        <div class="flex flex-justify-content-between mb-10">
            <asp:Label runat="server" ID="Label7" Font-Size="Small" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Stock In" />
            <asp:Label runat="server" ID="lbl_SI" Font-Size="Small" Text="0" />
        </div>
        <div class="flex flex-justify-content-between mb-10">
            <asp:Label runat="server" ID="Label8" Font-Size="Small" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Stock Out" />
            <asp:Label runat="server" ID="lbl_SO" Font-Size="Small" Text="0" />
        </div>
        <div class="flex flex-justify-content-between mb-10">
            <asp:Label runat="server" ID="Label9" Font-Size="Small" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Store Requisition" />
            <asp:Label runat="server" ID="lbl_SR" Font-Size="Small" Text="0" />
        </div>
        <div class="flex flex-justify-content-between mb-10">
            <asp:Label runat="server" ID="Label11" Font-Size="Small" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Others" />
            <asp:Label runat="server" ID="lbl_Others" Font-Size="Small" Text="0" />
        </div>
    </div>
</asp:Content>
