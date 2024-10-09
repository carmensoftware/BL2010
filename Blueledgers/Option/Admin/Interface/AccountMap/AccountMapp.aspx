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
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
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
                        <dx:MenuItem Name="GetNew" Text="Scan for new code">
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
                        <dx:MenuItem Name="Edit" Text="">
                            <%-- 2016-10-05 Add Edit Button --%>
                            <ItemStyle Height="16px" Width="43px">
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-edit.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/edit.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
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
    <div>
        <label>
            View Type</label>
        <select id="ddl_View" style="width: 200px;">
            <% foreach (System.Data.DataRow dr in _dtDataView.Rows)
               { %>
            <option value='<%= dr["ID"].ToString() %>'>
                <%= dr["ViewName"].ToString() %></option>
            <% } %>
        </select>
    </div>
    <hr />
    <asp:UpdatePanel runat="server" ID="update_panel" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:GridView ID="gv_AP" runat="server" SkinID="GRD_V1" Width="100%" AllowPaging="true" AllowSorting="true" OnPageIndexChanging="gv_AP_PageIndexChanging"
                OnSorting="gv_AP_Sorting" OnRowDataBound="gv_AP_RowDataBound" OnRowEditing="gv_AP_RowEditing" OnRowCancelingEdit="gv_AP_RowCancelingEdit" OnRowUpdating="gv_AP_RowUpdating">
                <HeaderStyle Height="40px" BackColor="#F4F4F5" Font-Bold="True" ForeColor="#444444" HorizontalAlign="Left" />
                <RowStyle Height="50px" BackColor="White" ForeColor="#333333" BorderColor="#DDDDDD" />
                <PagerStyle BackColor="#4D4D4D" ForeColor="White" HorizontalAlign="Center" BorderColor="#DDDDDD" Font-Size="Medium" CssClass="GridPager" />
                <Columns>
                    <%-- Location --%>
                    <asp:TemplateField HeaderText="Location">
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
                    <asp:TemplateField HeaderText="Category">
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
                    <asp:TemplateField HeaderText="Sub-Category">
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
                    <asp:TemplateField HeaderText="Item Group">
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
                    <%-- Department Code (A1) --%>
                    <asp:TemplateField HeaderText="Department">
                        <ItemTemplate>
                            <div>
                                <asp:Label runat="server" ID="DepCode" Font-Bold="true" />
                            </div>
                            <asp:Label runat="server" ID="DepName" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <% if (1 == 1)
                               { %>
                            <asp:DropDownList runat="server" ID="ddl_DepCode" Width="120" />
                            <%}
                               else
                               { %>
                            <asp:TextBox runat="server" ID="txt_DepCode" Width="120" />
                            <%} %>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- Account Code (A2) --%>
                    <asp:TemplateField HeaderText="Account">
                        <ItemTemplate>
                            <div>
                                <asp:Label runat="server" ID="AccCode" Font-Bold="true" />
                            </div>
                            <asp:Label runat="server" ID="AccName" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <% if (1 == 1)
                               { %>
                            <asp:DropDownList runat="server" ID="ddl_AccCode" Width="200" />
                            <%}
                               else
                               { %>
                            <asp:TextBox runat="server" ID="txt_AccCode" Width="200" />
                            <%} %>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- Edit/Save/Cancel --%>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HiddenField runat="server" ID="hf_ID" Value='<%# Eval("ID").ToString() %>' />
                            <asp:LinkButton runat="server" ID="btn_Edit" SkinID="LNKB_NORMAL" CommandName="Edit" Text="Edit" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:HiddenField runat="server" ID="hf_ID" Value='<%# Eval("ID").ToString() %>' />
                            <asp:LinkButton runat="server" ID="btn_SaveItem" BackColor="Green" ForeColor="White" SkinID="LNKB_NORMAL" CommandName="Update" Text="Save" />
                            &nbsp;&nbsp;
                            <asp:LinkButton runat="server" ID="btn_CancelItem" BackColor="Silver" ForeColor="White" SkinID="LNKB_NORMAL" CommandName="Cancel" Text="Cancel" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
    <dx:ASPxPopupControl ID="pop_Alert" ClientInstanceName="pop_Alert" runat="server" Width="300px" CloseAction="CloseButton" HeaderText="Warning" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
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
                            <asp:Button ID="btn_Pop_Alert_Ok" runat="server" Width="50px" SkinID="BTN_V1" Text="OK" OnClientClick="pop_Alert.hide();" />
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
