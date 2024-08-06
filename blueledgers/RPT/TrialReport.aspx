<%@ Page Title="" Trace="false" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master"
    AutoEventWireup="true" CodeFile="TrialReport.aspx.cs" Inherits="BlueLedger.PL.RPT.TrialReport" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="FastReport.Web, Version=2017.2.1.0, Culture=neutral, PublicKeyToken=db7e5ce63278458c" Namespace="FastReport.Web" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <asp:UpdatePanel ID="up_01" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 100%;">
                <asp:Label ID="lbl_Test" runat="server"></asp:Label>
                <asp:GridView ID="gv_Test" runat="server" AutoGenerateColumns="false">
                </asp:GridView>
                </br>
            </div>
            <div style="width: 100%;">
                <asp:GridView ID="gv_Criteria" runat="server" AutoGenerateColumns="false" Width="100%">
                    <Columns>
                        <asp:TemplateField></asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div style="width: 100%;">
                <dx:ASPxComboBox ID="cbb_v" runat="server" DropDownStyle="DropDownList" IncrementalFilteringMode="Contains"
                    SelectedIndex="0" CallbackPageSize="15" EnableCallbackMode="true" ValueField="ProductCode"
                    OnInit="cbb_v_Init">
                    <ValidationSettings SetFocusOnError="true">
                    </ValidationSettings>
                </dx:ASPxComboBox>
                <asp:SqlDataSource ID="SqlDataSource01" runat="server"></asp:SqlDataSource>
                <asp:Button ID="btn_OK" runat="server" Text="CallReport" OnClick="btn_OK_Click" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
