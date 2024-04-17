<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="AccountMapp2.aspx.cs" Inherits="BlueLedger.PL.Option.Admin.Interface.AccountMap.AccountMapp2" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .mt
        {
            margin-top: 8px !important;
        }
        .mb
        {
            margin-bottom: 8px !important;
        }
        .ms
        {
            margin-left: 8px !important;
        }
        .me
        {
            margin-right: 8px !important;
        }
        .w-100
        {
            width: 100% !important;
        }
    </style>
    <style>
        .pagination
        {
            display: inline-block;
        }
        
        .pagination a
        {
            color: black;
            float: left;
            padding: 8px 16px;
            text-decoration: none;
        }
        
        .pagination a.active
        {
            background-color: #4CAF50;
            color: white;
            border-radius: 5px;
        }
        
        .pagination a:hover:not(.active)
        {
            background-color: #ddd;
            border-radius: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <asp:Label runat="server" ID="lbl_test" Text="Test" Font-Size="Larger" Font-Bold="true" />
    <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <ContentTemplate>
            <!-- Title & Menu bar -->
            <div style="background-color: #4D4D4D; padding: 5px 10px 5px 10px; font-size: 1em;">
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.AccountMapp, lbl_Title %>" SkinID="LBL_HD_WHITE"></asp:Label>
                        </td>
                        <td align="right">
                            <asp:Button runat="server" ID="btn_Edit" Text="Edit" OnClick="btn_Edit_Click" />
                            <asp:Button runat="server" ID="btn_Save" Text="Save" OnClick="btn_Save_Click" />
                            <asp:Button runat="server" ID="btn_Cancel" Text="Cancel" OnClick="btn_Cancel_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <!-- View  & Search-->
            <table class="w-100 mt mb" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lbl_View" Font-Bold="true" Text="View" />
                        <asp:DropDownList runat="server" ID="ddl_View" AutoPostBack="true" Width="240" OnSelectedIndexChanged="ddl_View_SelectedIndexChanged" />
                    </td>
                    <td align="right">
                        <asp:TextBox runat="server" ID="txt_Search" Width="200" />
                        <asp:Button runat="server" ID="btn_Search" Text="search" />
                    </td>
                </tr>
            </table>
            <!-- Data -->
            <asp:GridView runat="server" ID="gv_Data" SkinID="GRD_V1" AutoGenerateColumns="false" Width="100%" OnRowDataBound="gv_Data_RowDataBound">
                <HeaderStyle HorizontalAlign="Left" />
                <Columns>
                    <%--Location--%>
                    <asp:TemplateField HeaderText="Location">
                        <ItemTemplate>
                            <div>
                                <%# Eval("LocationCode") %>
                            </div>
                            <small>
                                <%# Eval("LocationName") %>
                            </small>
                            <asp:HiddenField runat="server" ID="hf_Id" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div>
                                <%# Eval("LocationCode") %>
                            </div>
                            <small>
                                <%# Eval("LocationName") %>
                            </small>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--Category--%>
                    <asp:TemplateField HeaderText="Category">
                        <ItemTemplate>
                            <div>
                                <%# Eval("CategoryCode")%>
                            </div>
                            <small>
                                <%# Eval("CategoryName")%>
                            </small>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div>
                                <%# Eval("CategoryCode")%>
                            </div>
                            <small>
                                <%# Eval("CategoryName")%>
                            </small>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--SubCategory--%>
                    <asp:TemplateField HeaderText="Sub-Category">
                        <ItemTemplate>
                            <div>
                                <%# Eval("SubCategoryCode")%>
                            </div>
                            <small>
                                <%# Eval("SubCategoryName")%></small>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div>
                                <%# Eval("SubCategoryCode")%>
                            </div>
                            <small>
                                <%# Eval("SubCategoryName")%>
                            </small>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--ItemGroup--%>
                    <asp:TemplateField HeaderText="Item Group">
                        <ItemTemplate>
                            <div>
                                <%# Eval("ItemGroupCode")%>
                            </div>
                            <small>
                                <%# Eval("ItemGroupName")%>
                            </small>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div>
                                <%# Eval("ItemGroupCode")%>
                            </div>
                            <small>
                                <%# Eval("ItemGroupName")%>
                            </small>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--A1--%>
                    <asp:TemplateField HeaderText="A1">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_A1" />
                            <asp:TextBox runat="server" ID="txt_A1" Visible="false" />
                            <dx:ASPxComboBox runat="server" ID="ddl_A1">
                            </dx:ASPxComboBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--A2--%>
                    <asp:TemplateField HeaderText="A2">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_A2" />
                            <asp:TextBox runat="server" ID="txt_A2" Visible="false" />
                            <dx:ASPxComboBox runat="server" ID="ddl_A2">
                            </dx:ASPxComboBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--A3--%>
                    <asp:TemplateField HeaderText="A3">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_A3" />
                            <asp:TextBox runat="server" ID="txt_A3" Visible="false" />
                            <dx:ASPxComboBox runat="server" ID="ddl_A3">
                            </dx:ASPxComboBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--A4--%>
                    <asp:TemplateField HeaderText="A4">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_A4" />
                            <asp:TextBox runat="server" ID="txt_A4" Visible="false" />
                            <dx:ASPxComboBox runat="server" ID="ddl_A4">
                            </dx:ASPxComboBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--A5--%>
                    <asp:TemplateField HeaderText="A5">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_A5" />
                            <asp:TextBox runat="server" ID="txt_A5" Visible="false" />
                            <dx:ASPxComboBox runat="server" ID="ddl_A5">
                            </dx:ASPxComboBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--A6--%>
                    <asp:TemplateField HeaderText="A6">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_A6" />
                            <asp:TextBox runat="server" ID="txt_A6" Visible="false" />
                            <dx:ASPxComboBox runat="server" ID="ddl_A6">
                            </dx:ASPxComboBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--A7--%>
                    <asp:TemplateField HeaderText="A7">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_A7" />
                            <asp:TextBox runat="server" ID="txt_A7" Visible="false" />
                            <dx:ASPxComboBox runat="server" ID="ddl_A7">
                            </dx:ASPxComboBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--A8--%>
                    <asp:TemplateField HeaderText="A8">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_A8" />
                            <asp:TextBox runat="server" ID="txt_A8" Visible="false" />
                            <dx:ASPxComboBox runat="server" ID="ddl_A8">
                            </dx:ASPxComboBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--A9--%>
                    <asp:TemplateField HeaderText="A9">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_A9" />
                            <asp:TextBox runat="server" ID="txt_A9" Visible="false" />
                            <dx:ASPxComboBox runat="server" ID="ddl_A9">
                            </dx:ASPxComboBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <table class="w-100 mt">
                <tr>
                    <td>
                        <div class="pagination w-100">
                            <asp:Button runat="server" ID="btn_page_prev" Font-Size="Small" Text="&laquo;" OnClick="btn_page_prev_Click" />
                            <asp:Button runat="server" ID="btn_page_p1" Font-Size="Small" OnClick="btn_page_p_Click" />
                            <asp:Button runat="server" ID="btn_page_p2" Font-Size="Small" OnClick="btn_page_p_Click" />
                            <asp:Button runat="server" ID="btn_page_p3" Font-Size="Small" OnClick="btn_page_p_Click" />
                            <asp:Button runat="server" ID="btn_page_p4" Font-Size="Small" OnClick="btn_page_p_Click" />
                            <asp:Button runat="server" ID="btn_page_p5" Font-Size="Small" OnClick="btn_page_p_Click" />
                            <asp:Button runat="server" ID="btn_page_p6" Font-Size="Small" OnClick="btn_page_p_Click" />
                            <asp:Button runat="server" ID="btn_page_p7" Font-Size="Small" OnClick="btn_page_p_Click" />
                            <asp:Button runat="server" ID="btn_page_p8" Font-Size="Small" OnClick="btn_page_p_Click" />
                            <asp:Button runat="server" ID="btn_page_p9" Font-Size="Small" OnClick="btn_page_p_Click" />
                            <asp:Button runat="server" ID="btn_page_p10" Font-Size="Small" OnClick="btn_page_p_Click" />
                            <asp:Button runat="server" ID="btn_page_next" Font-Size="Small" Text="&raquo;" OnClick="btn_page_next_Click" />
                            <%--<asp:LinkButton runat="server" ID="link_prev" OnClick="link_prev_Click">&laquo;</asp:LinkButton>
                            <asp:LinkButton runat="server" ID="link_p1" OnClick="link_p_Click">1</asp:LinkButton>
                            <asp:LinkButton runat="server" ID="link_p2" OnClick="link_p_Click">2</asp:LinkButton>
                            <asp:LinkButton runat="server" ID="link_p3" OnClick="link_p_Click">3</asp:LinkButton>
                            <asp:LinkButton runat="server" ID="link_p4" OnClick="link_p_Click">4</asp:LinkButton>
                            <asp:LinkButton runat="server" ID="link_p5" OnClick="link_p_Click">5</asp:LinkButton>
                            <asp:LinkButton runat="server" ID="link_p6" OnClick="link_p_Click">6</asp:LinkButton>
                            <asp:LinkButton runat="server" ID="link_p7" OnClick="link_p_Click">7</asp:LinkButton>
                            <asp:LinkButton runat="server" ID="link_p8" OnClick="link_p_Click">8</asp:LinkButton>
                            <asp:LinkButton runat="server" ID="link_p9" OnClick="link_p_Click">9</asp:LinkButton>
                            <asp:LinkButton runat="server" ID="link_p10" OnClick="link_p_Click">10</asp:LinkButton>
                            <asp:LinkButton runat="server" ID="link_next" OnClick="link_next_Click">&raquo;</asp:LinkButton>--%>
                        </div>
                    </td>
                    <td align="right">
                        <asp:DropDownList runat="server" ID="ddl_PageSize" AutoPostBack="true" OnSelectedIndexChanged="ddl_PageSize_SelectedIndexChanged">
                            <%--<asp:ListItem Value="2">2</asp:ListItem>--%>
                            <asp:ListItem Value="10">10</asp:ListItem>
                            <asp:ListItem Value="25">25</asp:ListItem>
                            <asp:ListItem Value="50">50</asp:ListItem>
                            <asp:ListItem Value="100">100</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btn_Edit" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
