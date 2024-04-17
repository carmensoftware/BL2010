<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="PrintBatch.aspx.cs" Inherits="BlueLedger.PL.PC.PO.PrintBatch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .card
        {
            box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
            transition: 0.3s;
            width: 40%;
            background-color: #FFF;
        }
        
        .card:hover
        {
            box-shadow: 0 8px 16px 0 rgba(0,0,0,0.2);
        }
        
        .container
        {
            padding: 2px 16px;
        }
        
        .mb
        {
            margin-bottom: 10px !important;
        }
        
        .badge
        {
            font-size: 1rem;
            font-weight: bold;
            padding: 5px 15px 5px 15px;
            background-color: transparent;
            color: Black !important;
        }
        
        .badge-select
        {
            font-size: 1rem;
            font-weight: bold;
            padding: 5px 15px 5px 15px;
            background-color: #0040ff;
            color: White !important;
            border-radius: 20px;
        }
        
        .report-panel
        {
            height: auto;
            width: auto; /*padding: 10px 0 10px 15px;
            height: auto;
            border-bottom: 1px solid Gray; */
        }
        .report-item
        {
            color: Black !important;
            line-height: 3rem;
            font-size: 1rem;
        }
        .dialog-control
        {
            margin-bottom: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <!-- Title Bar -->
    <div class="mb" style="padding: 3px; width: 100%; background-color: #4d4d4d;">
        <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
        <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_HD_WHITE" Text="Purchase Order" />
    </div>
</asp:Content>
