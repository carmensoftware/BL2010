<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="UpdateFile.aspx.cs" Inherits="BlueLedger.PL.Report.UpdateFile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" Runat="Server">
    <asp:Button ID="btn_UpdateFile" runat="server" Text="Update Report Files" OnClick="btn_UpdateFile_Click" />
    <asp:Label ID="lbl_Message" runat="server" Text="" />
</asp:Content>

