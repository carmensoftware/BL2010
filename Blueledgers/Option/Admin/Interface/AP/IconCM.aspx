<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="IconCM.aspx.cs" Inherits="BlueLedger.PL.Option.Admin.Interface.AP.IconCM" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.1.3/dist/css/bootstrap.min.css" integrity="sha384-MCw98/SFnGE8fJT3GXwEOngsV7Zt27NXFoaoApmYm81iuXoPkFOJwJ8ERdknLPMO"
        crossorigin="anonymous">
    <style>
        .title-bar
        {
            background-color: Black;
            color: White;
            width: 100%;
            height: 26px;
        }
        
        .mb-3
        {
            margin-bottom: 1rem !important;
        }
    </style>
    <style>
        /* Style the tab */
        .tab
        {
            overflow: hidden;
            border: 1px solid #ccc;
            background-color: #f1f1f1;
        }
        
        /* Style the buttons inside the tab */
        .tab a
        {
            background-color: inherit;
            float: left;
            border: none;
            outline: none;
            cursor: pointer;
            padding: 14px 16px;
            transition: 0.3s;
            font-size: 17px;
        }
        
        /* Change background color of buttons on hover */
        .tab a:hover
        {
            background-color: #ddd;
        }
        
        /* Create an active/current tablink class */
        .tab a.active
        {
            background-color: #ccc;
        }
        
        
        /* Style the tab content */
        .tabcontent
        {
            display: none;
            padding: 6px 12px;
            border: 1px solid #ccc;
            border: none;
        }
    </style>
    <script src="../../../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="server">
    <!-- Title Bar  -->
    <div class="title-bar mb-3">
        <div style="display: inline-block; margin-top: 3px; margin-right: 2px; margin-left: 2px;">
            <asp:Image ID="image_Title" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
        </div>
        <div style="display: inline-block;">
            <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_HD_WHITE" Text="Interface to IconCM" />
        </div>
    </div>
    <div class="mb-3">
        <div style="display: inline-block; vertical-align: top; padding-top: 3px;">
            <asp:Label ID="lbl_FromDate_Nm" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.ExportPosting, lbl_FromDate_Nm %>" SkinID="LBL_HD" />
        </div>
        <div style="display: inline-block; vertical-align: top;">
            <dx:ASPxDateEdit ID="date_DateFrom" runat="server" DisplayFormatString="dd/MM/yyyy" EditFormat="Custom" EditFormatString="dd/MM/yyyy" />
            <asp:RequiredFieldValidator ID="dateFrom" runat="server" ControlToValidate="date_DateFrom" ErrorMessage="*" ForeColor="Red" Font-Bold="true" Font-Size="Larger" />
        </div>
        <div style="display: inline-block; vertical-align: top;">
            <asp:Button ID="btn_Preview" runat="server" Text="Preview" Width="60px" SkinID="BTN_V1" OnClick="btn_Preview_Click" />
        </div>
    </div>
    <div class="mb-3">
        <div style="display: inline-block; vertical-align: top;">
            <asp:Button ID="btn_ExportHeader" runat="server" Text="Download Header..." Width="120px" SkinID="BTN_V1" Visible="false" OnClick="btn_ExportHeader_Click" />
        </div>
        <div style="display: inline-block; vertical-align: top;">
            <asp:Button ID="btn_ExportDetail" runat="server" Text="Download Detail..." Width="120px" SkinID="BTN_V1" Visible="false" OnClick="btn_ExportDetail_Click" />
        </div>
    </div>
    <div class="tab">
        <a class="tablink active">Header </a>
        <a class="tablink">Detail </a>
    </div>
    <div id="Header" class="tabcontent">
        <asp:GridView runat="server" ID="gvHeader" AutoGenerateColumns="true" Width="100%">
        </asp:GridView>
    </div>
    <div id="Detail" class="tabcontent">
        <asp:GridView runat="server" ID="gvDetail" AutoGenerateColumns="true" Width="100%">
        </asp:GridView>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            // Init
            $('#Header').show();

            // Event(s)
            $('.tablink').click(function () {
                var id = $(this).text();

                $('.tablink').each(function () {
                    $(this).removeClass('active');
                });

                $('.tabcontent').each(function () {
                    var item = $(this);
                    item.hide();
                });

                var item = $('#' + id);
                item.show();
                $(this).addClass('active');
            });
        });
    
    </script>
</asp:Content>
