<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="ProdCatLst.aspx.cs" Inherits="BlueLedger.PL.Option.ProdCat.ProdCatLst" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <style type="text/css">
        .marginTB
        {
            margin-bottom: 1px;
            margin-top: 1px;
        }
        .vertical-line-right
        {
            border-right: 1px solid #A6A6A6;
            height: 100%;
        }
        .font-style
        {
            font-size: 12px;
            font-family: Arial;
        }
        
        /*For Toggle*/
        .switch
        {
            position: relative;
            display: inline-block;
            width: 30px;
            height: 17px;
        }
        
        .switch input
        {
            display: none;
        }
        
        .slider
        {
            position: absolute;
            cursor: pointer;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: #ccc;
            -webkit-transition: .4s;
            transition: .4s;
        }
        
        .slider:before
        {
            position: absolute;
            content: "";
            height: 16px;
            width: 16px;
            left: 1px;
            bottom: 1px;
            background-color: white;
            -webkit-transition: .4s;
            transition: .4s;
        }
        
        input:checked + .slider
        {
            /*background-color: #2196F3;*/
            background-color: #20B9EB;
        }
        
        input:focus + .slider
        {
            box-shadow: 0 0 1px #2196F3;
        }
        
        input:checked + .slider:before
        {
            -webkit-transform: translateX(16px);
            -ms-transform: translateX(16px);
            transform: translateX(12px); /*move on X = 10 px*/
        }
        
        /* Rounded sliders*/
        .slider.round
        {
            border-radius: 34px; /*0px = 90' for every coner*/
        }
        
        .slider.round:before
        {
            border-radius: 50%;
        }
        
        .HideControl
        {
            display: none;
        }
        
        @media print
        {
            body *
            {
                visibility: hidden;
            }
            .printable, .printable *
            {
                visibility: visible;
            }
            .printable
            {
                position: absolute;
                left: 0;
                top: 0;
            }
    </style>
    <script type="text/javascript">
        function fromChild() {
            document.getElementById("<%=btnUploadHide.ClientID %>").click();
        }
    </script>
    <asp:UpdatePanel ID="UdPnHdDetail" runat="server">
        <ContentTemplate>
            <div style="background-color: #4D4D4D; width: 100%;">
                <table width="100%" border="0">
                    <tr>
                        <td>
                            <div style="float: left; margin-left: 10px;">
                                <asp:Image ID="img01" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                                <asp:Label ID="lblPageName" runat="server" Text="Product Category" ForeColor="White"></asp:Label>
                            </div>
                            <div style="float: right;">
                                <dx:ASPxMenu ID="menu_CmdBar" runat="server" Font-Bold="True" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle"
                                    Height="16px" AutoPostBack="true" OnItemClick="menu_CmdBar_ItemClick">
                                    <ItemStyle BackColor="Transparent">
                                        <HoverStyle BackColor="Transparent">
                                            <Border BorderStyle="None" />
                                        </HoverStyle>
                                        <Paddings Padding="2px" />
                                        <Border BorderStyle="None" />
                                    </ItemStyle>
                                    <Items>
                                        <dx:MenuItem Name="Import" Text="Import">
                                            <ItemStyle Height="16px" Width="20px" ForeColor="White" Font-Size="8.7px" Font-Names="Tahoma" Paddings-PaddingBottom="0px"></ItemStyle>
                                        </dx:MenuItem>
                                        <dx:MenuItem Name="AddLevel01" Text="" ToolTip="Add Category" Visible="false">
                                            <ItemStyle Height="16px" Width="43px">
                                                <HoverStyle>
                                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png" Repeat="NoRepeat" VerticalPosition="center" />
                                                </HoverStyle>
                                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/create.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                            </ItemStyle>
                                        </dx:MenuItem>
                                        <dx:MenuItem Name="AddLevel02" Text="" ToolTip="Add Sub-Category" Visible="false">
                                            <ItemStyle Height="16px" Width="43px">
                                                <HoverStyle>
                                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png" Repeat="NoRepeat" VerticalPosition="center" />
                                                </HoverStyle>
                                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/create.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                            </ItemStyle>
                                        </dx:MenuItem>
                                        <dx:MenuItem Name="AddLevel03" Text="" ToolTip="Add Item Group" Visible="false">
                                            <ItemStyle Height="16px" Width="43px">
                                                <HoverStyle>
                                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png" Repeat="NoRepeat" VerticalPosition="center" />
                                                </HoverStyle>
                                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/create.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                            </ItemStyle>
                                        </dx:MenuItem>
                                        <dx:MenuItem Name="Edit" Text="" Visible="false">
                                            <ItemStyle Height="16px" Width="43px">
                                                <HoverStyle>
                                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-edit.png" Repeat="NoRepeat" VerticalPosition="center" />
                                                </HoverStyle>
                                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/edit.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                            </ItemStyle>
                                        </dx:MenuItem>
                                        <%--<dx:MenuItem Name="Delete" Text="" Visible="false">
                                            <ItemStyle Height="16px" Width="43px">
                                                <HoverStyle>
                                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png"
                                                        Repeat="NoRepeat" VerticalPosition="center" />
                                                </HoverStyle>
                                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png" Repeat="NoRepeat"
                                                    HorizontalPosition="center" VerticalPosition="center" />
                                            </ItemStyle>
                                        </dx:MenuItem>--%>
                                        <dx:MenuItem Name="Print" Text="">
                                            <ItemStyle Height="16px" Width="43px">
                                                <HoverStyle>
                                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-print.png" Repeat="NoRepeat" VerticalPosition="center" />
                                                </HoverStyle>
                                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                            </ItemStyle>
                                        </dx:MenuItem>
                                    </Items>
                                </dx:ASPxMenu>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="width: 100%;">
                <div style="float: left; width: 50%; padding-left: 10px; padding-top: 2px;">
                    <asp:LinkButton ID="btnUploadHide" runat="server" Text="hide" CssClass="HideControl" OnClick="btnUploadHide_Click"></asp:LinkButton>
                </div>
            </div>
            <asp:Panel ID="pnSearch" runat="server" DefaultButton="btnS" Width="100%">
                <div style="float: right;">
                    <asp:TextBox ID="txtSearch" runat="server" Width="200px"></asp:TextBox>
                    <asp:ImageButton ID="btnS" AlternateText="Search" runat="server" OnClick="btnS_Click" ImageUrl="~/App_Themes/Default/Images/master/in/Default/search.png" />
                </div>
            </asp:Panel>
            <div class="printable">
                <table style="width: 100%; padding-top: 15px;">
                    <tr>
                        <td style="width: 70%;" class="vertical-line-right" valign="top">
                            <div style="float: none; margin-left: 50px; width: 90%;">
                                <asp:TreeView runat="server" ID="tview" OnSelectedNodeChanged="tview_SelectedNodeChanged" ForeColor="#4D4D4D" CssClass="font-style">
                                    <SelectedNodeStyle BackColor="#EBEBEB" />
                                </asp:TreeView>
                            </div>
                        </td>
                        <td style="width: 30%" valign="top">
                            <div class="font-style" style="margin-left: 50px; width: 90%;">
                                <asp:Label ID="lblLevelDesc" runat="server" Text="" Width="170px" Font-Bold="true" Font-Size="14px"></asp:Label>
                                <asp:Label ID="lblLevelNo" runat="server" Visible="false"></asp:Label>
                                <br />
                                <table>
                                    <tr>
                                        <td style="width: 45%;">
                                            <asp:Label ID="lbl00" runat="server" Text="Parent: " Visible="false"></asp:Label>
                                        </td>
                                        <td style="width: 55%">
                                            <asp:TextBox ID="txtParent" runat="server" CssClass="marginTB" Enabled="false" Visible="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl01" runat="server" Text="Code: "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCategoryCode" runat="server" CssClass="marginTB" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl02" runat="server" Text="Name: "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCategoryName" runat="server" CssClass="marginTB" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl05" runat="server" Text="Tax Account: "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAccCode" runat="server" CssClass="marginTB" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl03" runat="server" Width="100px" Text="Category Type: "></asp:Label>
                                        </td>
                                        <td>
                                            <%--<asp:TextBox ID="txtCategoryType" runat="server" CssClass="marginTB" Visible="false"
                                    Enabled="false"></asp:TextBox>--%>
                                            <asp:DropDownList ID="ddlCategoryType" runat="server" CssClass="marginTB" Width="150px" AppendDataBoundItems="true" Enabled="false" OnInit="ddlCategoryType_Init">
                                                <asp:ListItem Selected="True" Value="-1" Text=""></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_PriceDeviation" runat="server" Width="100px" Text="Price Deviation" />
                                        </td>
                                        <td>
                                            <dx:ASPxSpinEdit ID="se_PriceDeviation" runat="server" Number="0" Width="150px" SpinButtons-ShowIncrementButtons="False" Enabled="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_QtyDeviation" runat="server" Width="100px" Text="Quantity Deviation" />
                                        </td>
                                        <td>
                                            <dx:ASPxSpinEdit ID="se_QtyDeviation" runat="server" Number="0" Width="150px" SpinButtons-ShowIncrementButtons="False" Enabled="false" />
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 10%;">
                                            <asp:CheckBox ID="cbTActive" runat="server" Enabled="false" />
                                            <%--  <div>
                                                
                                                <label class="switch">
                                                   <input type="checkbox" id="cbTActive" runat="server" disabled="disabled" />
                                                    <div class="slider round">
                                                    </div>
                                                </label>
                                            </div>--%>
                                        </td>
                                        <td style="width: 90%;">
                                            <label>
                                                Active</label>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <asp:Label ID="lbl06" runat="server" Width="100px" Text="AuthRules: " Visible="false"></asp:Label>
                                <asp:CheckBox ID="cbAuthRules" runat="server" CssClass="marginTB" Enabled="false" Visible="false" />
                                <br />
                                <asp:Label ID="lbl07" runat="server" Width="100px" Text="ApprovalLevel: " Visible="false"></asp:Label>
                                <asp:TextBox ID="txtApp" runat="server" CssClass="marginTB" Enabled="false" Visible="false"></asp:TextBox>
                                <br />
                                <div>
                                    <asp:Button ID="btnSave" Text="Save" runat="server" OnClick="btnSave_Click" Visible="false" />
                                    <asp:Button ID="btnSaveEdit" Text="Save" runat="server" OnClick="btnSaveEdit_Click" Visible="false" />
                                    <asp:Button ID="btnCancel" Text="Cancel" runat="server" OnClick="btnCancel_Click" Visible="false" />
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="width: 100%;">
                <asp:Label ID="lbl_Test" runat="server"></asp:Label>
                <asp:GridView ID="gv_Test" runat="server" AutoGenerateColumns="true">
                </asp:GridView>
            </div>
            <dx:ASPxPopupControl ID="pop_Warning" ClientInstanceName="pop_Warning" runat="server" Width="300px" CloseAction="CloseButton" HeaderText="Warning" Modal="True"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                <ContentStyle VerticalAlign="Top">
                </ContentStyle>
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl8" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_Warning" runat="server" SkinID="LBL_NR" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btn_Warning" runat="server" Text="OK" Width="50px" SkinID="BTN_V1" OnClick="btn_Warning_Click" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="ASPxPopupFile" runat="server" CloseAction="CloseButton" HeaderText="Import" Width="430px" Modal="True" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" ShowCloseButton="true">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                        <div align="center">
                            <iframe id="iFrame_SetApproval" runat="server" src="ProdCatLstUpload.aspx" style="width: 99%; height: 99%" frameborder="0" />
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
