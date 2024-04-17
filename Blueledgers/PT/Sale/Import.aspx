<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="Import.aspx.cs" Inherits="BlueLedger.PL.PT.Sale.Import" %>

<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .master-info-bar
        {
            font-size: 1rem !important;
        }
        .p-3
        {
            padding: 10px !important;
        }
        .mb
        {
            margin-bottom: 5px !important;
        }
        .mb-3
        {
            margin-bottom: 10px !important;
        }
        .me-3
        {
            margin-right: 10px;
        }
        .text-end
        {
            text-align: right;
        }
        .w-100
        {
            width: 100%;
        }
        .d-flex
        {
            display: flex !important;
            height: fit-content;
        }
        
        .d-flex-wrap
        {
            flex-wrap: wrap;
        }
        .flex-column
        {
            display: flex;
            flex-direction: column;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <asp:HiddenField runat="server" ID="hf_FileName" />
    <asp:HiddenField runat="server" ID="hf_FilePath" />
    <!-- Title Bar -->
    <div class="mb-3" style="background-color: #4d4d4d; width: 100%; padding: 2px; height: 24px;">
        <div style="margin-left: 10px; float: left; margin-top: 5px;">
            <asp:Label ID="lbl_Title" runat="server" Font-Size="Small" Text="Import - Sale" SkinID="LBL_HD_WHITE" />
        </div>
        <div style="margin-right: 10px; float: right;">
        </div>
    </div>
    <!-- Upload file -->
    <asp:Panel ID="panel_File" runat="server" CssClass="card p-3 mb-3" Font-Size="Small">
        <table>
            <tr>
                <td colspan="2">
                    <b>From file</b>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:FileUpload ID="FileUpload" runat="server" Width="300" />
                </td>
                <td>
                    <asp:Button ID="btn_UploadFile" runat="server" Width="60" Text="Upload" OnClick="btn_UploadFile_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <%--<small>Support file *.csv | *.xls | *.xlsx"</small>--%>
                </td>
            </tr>
        </table>
        <br />
        <b>
            <%= hf_FileName.Value.ToString() %>
        </b>
        <hr />
        <div>
            <asp:Button runat="server" ID="btn_Setting" Width="100" Text="Setting" OnClick="btn_Setting_Click" />
            <asp:Button runat="server" ID="btn_Preview" Width="100" Text="Preview" OnClick="btn_Preview_Click" />
            <asp:Button runat="server" ID="btn_Import" Width="100" Text="Import" OnClick="btn_Import_Click" />
        </div>
    </asp:Panel>
    <!--Content-->
    <table class="w-100">
        <tr>
            <td style="width: 80%; vertical-align: top;">
                <div>
                    <asp:CheckBox runat="server" ID="chk_ShowAll" AutoPostBack="true" Text="Show all rows" OnCheckedChanged="chk_ShowAll_CheckedChanged"/>
                </div>
                <br />
                <asp:GridView ID="gv_Data" runat="server" Width="100%">
                    <HeaderStyle BackColor="SkyBlue" />
                    <Columns>
                    </Columns>
                </asp:GridView>
            </td>
            <td style="vertical-align: top;">
                <table>
                    <tr>
                        <td>
                            Start at line
                        </td>
                        <td>
                            <asp:TextBox ID="txt_StartLine" runat="server" Text="1" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <!-- Date -->
                    <tr>
                        <td>
                            <b>Date</b>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddl_Date" Width="100">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <!-- Revenue Center -->
                    <tr>
                        <td>
                            <b>Revenue Center</b>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddl_RevCode" Width="100">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <!-- Outlet -->
                    <tr>
                        <td>
                            <b>Outlet</b>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddl_OutletCode" Width="100">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <!-- Department -->
                    <tr>
                        <td>
                            <b>Department (Optional)</b>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddl_DepCode" Width="100">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <!-- -->
                    <tr>
                        <td>
                            <b>Item</b>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddl_ItemCode" Width="100">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <!-- -->
                    <tr>
                        <td>
                            <b>Qty</b>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddl_Qty" Width="100">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <!-- -->
                    <tr>
                        <td>
                            <b>Price/Unit</b>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddl_Price" Width="100">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <!-- -->
                    <tr>
                        <td>
                            <b>Total</b>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddl_Total" Width="100">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <!-- Popup-->
    <dx:ASPxPopupControl ID="pop_Alert" ClientInstanceName="pop_Alert" runat="server" Width="320" HeaderText="Alert" ShowHeader="true" CloseAction="CloseButton"
        Modal="True" AutoUpdatePosition="True" AllowDragging="True" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" ShowPageScrollbarWhenModal="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl_Alert" runat="server">
                <div style="width: 100%; text-align: center;">
                    <asp:Label ID="lbl_Alert" runat="server" />
                    <br />
                    <br />
                    <br />
                    <asp:Button ID="btn_Alert_Ok" runat="server" Width="80" Text="OK" OnClientClick="pop_Alert.Hide();" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
