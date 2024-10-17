<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" ViewStateMode="Enabled" AutoEventWireup="true" CodeFile="AccountMapp.aspx.cs" Inherits="BlueLedger.PL.Option.Admin.Interface.AccountMap.AccountMapp" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<asp:Content ID="Header" ContentPlaceHolderID="head" runat="server">
    <style>
        #datatable
        {
            border-collapse: collapse;
        }
        #datatable td
        {
            padding: 8px 10px;
        }
        #datatable thead th
        {
            background-color: #2196f3;
            color: #fff;
            padding: 8px 10px;
            text-align: left;
        }
        #datatable tr:nth-child(even)
        {
            background-color: #f2f2f2;
        }
        
        
        .display-none
        {
            display: none !important;
        }
        
        .display-block
        {
            display: block !important;
        }
        .text-bold
        {
            font-weight: bold;
        }
    </style>
    <style>
        .gridpager, .gridpager td
        {
            color: Green;
            font-weight: bold;
            text-decoration: none;
            border: 0;
            position: relative;
            padding-left: 5px;
            padding-right: 5px;
        }
        
        .gridpager a
        {
            color: black;
            font-weight: normal;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <script type="text/javascript">
        function clearUploadFile() {
            var file = document.getElementById('<%= FileUploadControl.ID %>');

            file.select();
            n = fil.createTextRange();
            n.execCommand('delete');
            file.focus();
        }
    </script>
    <asp:UpdatePanel runat="server" ID="update_panel" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label runat="server" ID="lbl_Test" Font-Size="Medium">Result</asp:Label>
            <table width="100%" border="0" cellpadding="0" cellspacing="0" style="background-color: #4D4D4D; height: auto;">
                <tr>
                    <td align="left" style="padding-left: 10px;">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                        <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.AccountMapp, lbl_Title %>" SkinID="LBL_HD_WHITE"></asp:Label>
                    </td>
                    <td align="right">
                        <dx:ASPxMenu ID="menu_CmdBar" runat="server" AutoPostBack="true" Font-Size="Small" Font-Bold="True" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="5px"
                            VerticalAlign="Middle" Height="16px" OnItemClick="menu_CmdBar_ItemClick">
                            <ItemStyle BackColor="Transparent">
                                <HoverStyle BackColor="Transparent">
                                    <Border BorderStyle="None" />
                                </HoverStyle>
                                <Paddings Padding="2px" />
                                <Border BorderStyle="None" />
                            </ItemStyle>
                            <Items>
                                <dx:MenuItem Name="GetNew" Text="Get new code(s)">
                                    <ItemStyle Height="16px" Width="90px" ForeColor="White" Font-Size="0.75em">
                                        <HoverStyle ForeColor="#C4C4C4">
                                        </HoverStyle>
                                    </ItemStyle>
                                </dx:MenuItem>
                                <dx:MenuItem Name="Import" Text="Import/Export">
                                    <ItemStyle Height="16px" Width="60px" ForeColor="White" Font-Size="0.75em">
                                        <HoverStyle ForeColor="#C4C4C4">
                                        </HoverStyle>
                                    </ItemStyle>
                                </dx:MenuItem>
                                <dx:MenuItem Name="Save" Text="" Visible="false">
                                    <ItemStyle Height="16px" Width="42px">
                                        <HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png" Repeat="NoRepeat" VerticalPosition="center" />
                                        </HoverStyle>
                                        <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/save.png" Repeat="NoRepeat" VerticalPosition="center" />
                                    </ItemStyle>
                                </dx:MenuItem>
                                <dx:MenuItem Name="Print" Text="">
                                    <ItemStyle Height="16px" Width="43px">
                                        <HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-print.png" Repeat="NoRepeat" VerticalPosition="center" />
                                        </HoverStyle>
                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                    </ItemStyle>
                                </dx:MenuItem>
                                <dx:MenuItem Name="Back" Text="" Visible="false">
                                    <ItemStyle Height="16px" Width="43px">
                                        <HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png" Repeat="NoRepeat" VerticalPosition="center" />
                                        </HoverStyle>
                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                    </ItemStyle>
                                </dx:MenuItem>
                            </Items>
                        </dx:ASPxMenu>
                    </td>
                </tr>
            </table>
            <br />
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="View" />
                        <asp:DropDownList runat="server" ID="ddl_View" Width="200" AutoPostBack="true" OnSelectedIndexChanged="ddl_View_SelectedIndexChanged" />
                    </td>
                    <td align="right">
                        <asp:TextBox runat="server" ID="txt_Search" AutoPostBack="true" Width="280" />
                        <asp:Button runat="server" ID="btn_Search" Text="search" OnClick="btn_Search_Click" />
                    </td>
                </tr>
            </table>
            <hr />
            <asp:GridView ID="gv_Data" runat="server" SkinID="GRD_V1" Width="100%" PageSize="25" AllowPaging="true" AllowSorting="true" OnPageIndexChanging="gv_Data_PageIndexChanging"
                OnSorting="gv_Data_Sorting" OnRowDataBound="gv_Data_RowDataBound" OnRowEditing="gv_Data_RowEditing" OnRowCancelingEdit="gv_Data_RowCancelingEdit" OnRowUpdating="gv_Data_RowUpdating">
                <HeaderStyle Height="40px" BackColor="#F4F4F5" Font-Bold="True" ForeColor="#444444" HorizontalAlign="Left" />
                <RowStyle Height="50px" BackColor="White" ForeColor="#333333" BorderColor="#DDDDDD" />
                <PagerStyle Font-Size="Medium" HorizontalAlign="Center" CssClass="gridpager" />
                <Columns>
                    <%-- Location --%>
                    <asp:TemplateField HeaderText="Location" SortExpression="LocationCode">
                        <ItemTemplate>
                            <div>
                                <asp:Label runat="server" ID="LocationCode" Font-Bold="true" />
                            </div>
                            <asp:Label runat="server" ID="LocationName" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div>
                                <asp:Label runat="server" ID="LocationCode" Font-Bold="true" />
                            </div>
                            <asp:Label runat="server" ID="LocationName" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- Category --%>
                    <asp:TemplateField HeaderText="Category" SortExpression="CategoryCode">
                        <ItemTemplate>
                            <div>
                                <asp:Label runat="server" ID="CategoryCode" Font-Bold="true" />
                            </div>
                            <asp:Label runat="server" ID="CategoryName" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div>
                                <asp:Label runat="server" ID="CategoryCode" Font-Bold="true" />
                            </div>
                            <asp:Label runat="server" ID="CategoryName" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- SubCategory --%>
                    <asp:TemplateField HeaderText="Sub-Category" SortExpression="SubCategoryCode">
                        <ItemTemplate>
                            <div>
                                <asp:Label runat="server" ID="SubCategoryCode" Font-Bold="true" />
                            </div>
                            <asp:Label runat="server" ID="SubCategoryName" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div>
                                <asp:Label runat="server" ID="SubCategoryCode" Font-Bold="true" />
                            </div>
                            <asp:Label runat="server" ID="SubCategoryName" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- ItemGroup --%>
                    <asp:TemplateField HeaderText="Item Group" SortExpression="ItemGroupCode">
                        <ItemTemplate>
                            <div>
                                <asp:Label runat="server" ID="ItemGroupCode" Font-Bold="true" />
                            </div>
                            <asp:Label runat="server" ID="ItemGroupName" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div>
                                <asp:Label runat="server" ID="ItemGroupCode" Font-Bold="true" />
                            </div>
                            <asp:Label runat="server" ID="ItemGroupName" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- ---------------------------------------------------------------- --%>
                    <%-- AP : Department Code (A1) --%>
                    <asp:TemplateField HeaderText="Department" ControlStyle-CssClass="AP" SortExpression="DepCode">
                        <ItemTemplate>
                            <div>
                                <asp:Label runat="server" ID="DepCode" Font-Bold="true" />
                            </div>
                            <asp:Label runat="server" ID="DepName" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <dx:ASPxComboBox runat="server" ID="ddl_DepCode" DropDownStyle="DropDown" Width="140" IncrementalFilteringMode="Contains" CallbackPageSize="100" EnableCallbackMode="true" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- AP : Account Code (A2) --%>
                    <asp:TemplateField HeaderText="Account" ControlStyle-CssClass="AP" SortExpression="AccCode">
                        <ItemTemplate>
                            <div>
                                <asp:Label runat="server" ID="AccCode" Font-Bold="true" />
                            </div>
                            <asp:Label runat="server" ID="AccName" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <dx:ASPxComboBox runat="server" ID="ddl_AccCode" DropDownStyle="DropDown" Width="200" IncrementalFilteringMode="Contains" CallbackPageSize="100" EnableCallbackMode="true" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- ---------------------------------------------------------------- --%>
                    <%-- GL : Adjust Code (A1) --%>
                    <asp:TemplateField HeaderText="Movement Type" ControlStyle-CssClass="GL" SortExpression="AdjCode" Visible="false">
                        <ItemTemplate>
                            <div>
                                <asp:Label runat="server" ID="AdjCode" Font-Bold="true" />
                            </div>
                            <asp:Label runat="server" ID="AdjName" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div>
                                <asp:Label runat="server" ID="AdjCode" Font-Bold="true" />
                            </div>
                            <asp:Label runat="server" ID="AdjName" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- GL : Dr. DepCode (A2) --%>
                    <asp:TemplateField HeaderText="Dr. Department" ControlStyle-CssClass="GL" SortExpression="DrDepCode" Visible="false">
                        <ItemTemplate>
                            <div>
                                <asp:Label runat="server" ID="DrDepCode" Font-Bold="true" />
                            </div>
                            <asp:Label runat="server" ID="DrDepName" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <dx:ASPxComboBox runat="server" ID="ddl_DrDepCode" DropDownStyle="DropDown" Width="140" IncrementalFilteringMode="Contains" CallbackPageSize="100" EnableCallbackMode="true" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- GL : Dr. DepCode (A3) --%>
                    <asp:TemplateField HeaderText="Dr. AccCode" ControlStyle-CssClass="GL" SortExpression="DrAccCode" Visible="false">
                        <ItemTemplate>
                            <div>
                                <asp:Label runat="server" ID="DrAccCode" Font-Bold="true" />
                            </div>
                            <asp:Label runat="server" ID="DrAccName" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <dx:ASPxComboBox runat="server" ID="ddl_DrAccCode" DropDownStyle="DropDown" Width="200" IncrementalFilteringMode="Contains" CallbackPageSize="100" EnableCallbackMode="true" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- GL : Cr. DepCode (A4) --%>
                    <asp:TemplateField HeaderText="Cr. DepCode" ControlStyle-CssClass="GL" SortExpression="CrDepCode" Visible="false">
                        <ItemTemplate>
                            <div>
                                <asp:Label runat="server" ID="CrDepCode" Font-Bold="true" />
                            </div>
                            <asp:Label runat="server" ID="CrDepName" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <dx:ASPxComboBox runat="server" ID="ddl_CrDepCode" DropDownStyle="DropDown" Width="140" IncrementalFilteringMode="Contains" CallbackPageSize="100" EnableCallbackMode="true" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- GL : Cr. AccCode (A5) --%>
                    <asp:TemplateField HeaderText="Cr. Account" ControlStyle-CssClass="GL" SortExpression="CrAccCode" Visible="false">
                        <ItemTemplate>
                            <div>
                                <asp:Label runat="server" ID="CrAccCode" Font-Bold="true" />
                            </div>
                            <asp:Label runat="server" ID="CrAccName" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <dx:ASPxComboBox runat="server" ID="ddl_CrAccCode" DropDownStyle="DropDown" Width="200" IncrementalFilteringMode="Contains" CallbackPageSize="100" EnableCallbackMode="true" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- Edit/Save/Cancel --%>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HiddenField runat="server" ID="hf_ID" Value='<%# Eval("ID").ToString() %>' />
                            <asp:ImageButton runat="server" ID="btn_Edit" CommandName="Edit" Width="24" ImageUrl="data:image/png;base64,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" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:HiddenField runat="server" ID="hf_ID" Value='<%# Eval("ID").ToString() %>' />
                            <asp:ImageButton runat="server" ID="btn_SaveItem" CommandName="Update" Width="24" ImageUrl="data:image/png;base64,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" />
                            &nbsp;&nbsp;
                            <asp:ImageButton runat="server" ID="btn_CancelItem" CommandName="Cancel" Width="24" ImageUrl="data:image/png;base64,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" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btn_Search" />
            <asp:PostBackTrigger ControlID="txt_Search" />
            <asp:PostBackTrigger ControlID="menu_CmdBar" />
        </Triggers>
    </asp:UpdatePanel>
    <!-- Popup -->
    <dx:ASPxPopupControl ID="pop_Alert" ClientInstanceName="pop_Alert" runat="server" Width="300px" CloseAction="CloseButton" HeaderText="" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False" ShowPageScrollbarWhenModal="True">
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl8" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center">
                            <asp:Label ID="lbl_Pop_Alert" runat="server" SkinID="LBL_NR" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btn_Pop_Alert_Ok" runat="server" Width="50px" SkinID="BTN_V1" Text="OK" OnClientClick="pop_Alert.Hide();" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_ImportExport" runat="server" HeaderText="Import/Export" Width="430px" ShowCloseButton="true" CloseAction="CloseButton" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                <table width="100%">
                    <tr>
                        <td align="left">
                            <asp:FileUpload ID="FileUploadControl" runat="server" />
                        </td>
                        <td align="right">
                            <asp:Button ID="btn_Import" runat="server" Width="100px" Text="Import" OnClick="btn_Import_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lbl_Import" runat="server" ForeColor="Red" />
                        </td>
                    </tr>
                </table>
                <hr />
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Label ID="lblExport" runat="server" Text="Click 'Export' to save data as file." />
                        </td>
                        <td align="right">
                            <asp:Button ID="btn_Export" runat="server" Text="Export" Width="100px" OnClick="btn_Export_Click" OnClientClick="clearUploadFile()" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <script type="text/javascript" src="https://code.jquery.com/jquery-3.7.1.min.js" integrity="sha256-/JqT3SQfawRcv/BIHPThkBvs0OEvtFFmqPF/lYI/Cxo=" crossorigin="anonymous"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            // Init
            //$('.select2').select2();

            var table = new DataTable('#data', {

            });

            var type = GetUrlParameter('type');

            if (type) {
                $('#ddl_View').val(type);
            } else {
                $("#ddl_View").prop("selectedIndex", 0).val();
            }

            // Event(s)

            $('#ddl_View').on('change', function () {
                console.log(this.value);
                location.href = "AccountMapp.aspx?type=".concat(this.value);
            });


            $('[name="btn_Edit"]').on('click', function () {
                var id = $(this).attr('data-id');

                $('.item-edit').removeClass('display-none');
                $('.item-view').addClass('display-none');


                console.log(id);
            });

            // Method(s)
            function GetUrlParameter(parameter) {
                var queryString = window.location.search;
                var urlParams = new URLSearchParams(queryString);
                return urlParams.get(parameter);
            }
        });


    </script>
</asp:Content>
