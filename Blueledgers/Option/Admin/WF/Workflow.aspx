<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="Workflow.aspx.cs" Inherits="BlueLedger.PL.Option.Admin.WF.Workflow" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <style type="text/css">
        .block
        {
            clear: both;
            padding: 0;
            margin: 5px;
        }
        .block-left
        {
            float: left;
            padding: 5px;
            position: relative;
        }
        .block-right
        {
            float: right;
            padding: 5px;
        }
        .vertical-middle
        {
            margin-top: 3px;
        }
        .full-width
        {
            width: 100%;
        }
        .border-min
        {
            border: 1px solid #C0C0C0;
        }
    </style>
    <script type="text/javascript">
    </script>
    <!-- Title & Command Bar  -->
    <div class="CMD_BAR">
        <div class="CMD_BAR_LEFT">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
            <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:Option_Admin_Security_WF_WF, lbl_Title %>" SkinID="LBL_HD_WHITE"></asp:Label>
        </div>
        <div class="CMD_BAR_RIGHT">
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!-- Work-Flow Selection-->
            <div class="block">
                <div class="block-left vertical-middle">
                    <asp:Label ID="lbl_Module_Nm" runat="server" CssClass="text-middle" Text="<%$ Resources:Option_Admin_Security_WF_WF, lbl_Module_Nm %>" SkinID="LBL_HD" />
                </div>
                <div class="block-left">
                    <dx:ASPxComboBox ID="ddl_Module" runat="server" TextField="Desc" ValueField="WFId" AutoPostBack="True" Width="240px" OnSelectedIndexChanged="ddl_Module_SelectedIndexChanged" />
                </div>
            </div>
            <asp:Panel ID="panel_WF" runat="server" CssClass="block full-width">
                <asp:GridView ID="grd_WFDt" runat="server" AutoGenerateColumns="False" ShowHeader="false" GridLines="None" OnRowDataBound="grd_WFDt_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <fieldset>
                                    <legend>
                                        <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:Option_Admin_Security_WF_WF, lbl_Process1 %>" SkinID="LBL_HD"></asp:Label>
                                        <asp:Label ID="lbl_Step" runat="server" SkinID="LBL_HD"></asp:Label>
                                    </legend>
                                    <div class="block">
                                        <div class="block-left">
                                            <asp:Label ID="lbl_Desc_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_WF_WF, lbl_Desc_Nm %>" SkinID="LBL_HD"></asp:Label>
                                        </div>
                                        <div class="block-left">
                                            <asp:TextBox ID="txt_WfStepDesc" runat="server" Width="200px" SkinID="TXT_V1" />
                                        </div>
                                    </div>
                                    <br class="block" />
                                    <div id="detail" class="block">
                                    </div>
                                </fieldset>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
