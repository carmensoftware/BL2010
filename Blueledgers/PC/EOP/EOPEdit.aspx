<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="EOPEdit.aspx.cs" Inherits="BlueLedger.PL.PC.EOP.EOPEdit" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <style>
        .flex
        {
            display: flex !important;
        }
        
        .flex-between
        {
            justify-content: space-between !important;
        }
        .flex-center
        {
            justify-content: flex-center !important;
        }
        .flex-end
        {
            justify-content: flex-end !important;
        }
        .width-100
        {
            width: 100% !important;
        }
        .me-3
        {
            margin-right: 10px !important;
        }
    </style>
    <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <ContentTemplate>
            <!-- Main action bar -->
            <table border="0" cellpadding="0" cellspacing="0" width="100%" style="background-color: #4d4d4d;">
                <tr>
                    <td align="left">
                        &nbsp;
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                        <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:PC_EOP_EOPEdit, lbl_Title %>"></asp:Label>
                    </td>
                    <td align="right">
                        <div class="flex flex-end">
                            <dx:ASPxButton ID="btn_Save" runat="server" BackColor="Transparent" Height="16px" Width="42px" ToolTip="Save" OnClick="btn_Save_Click">
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/save.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <Border BorderStyle="None" />
                            </dx:ASPxButton>
                            &nbsp;
                            <dx:ASPxButton ID="btn_Commit" runat="server" Width="51px" Height="16px" BackColor="Transparent" ToolTip="Commit" OnClick="btn_Commit_Click">
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/commit.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                <HoverStyle>
                                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/gray-commit.png" Repeat="NoRepeat" VerticalPosition="center" HorizontalPosition="center" />
                                </HoverStyle>
                                <Border BorderStyle="None" />
                            </dx:ASPxButton>
                            &nbsp; &nbsp;
                            <dx:ASPxButton ID="btn_Delete" runat="server" BackColor="Transparent" Height="16px" Width="47px" ToolTip="Delete" OnClick="btn_Delete_Click">
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <Border BorderStyle="None" />
                            </dx:ASPxButton>
                            &nbsp;
                            <dx:ASPxButton ID="btn_Print" runat="server" BackColor="Transparent" Height="16px" Width="43px" ToolTip="Print" OnClick="btn_Print_Click">
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-print.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <Border BorderStyle="None" />
                            </dx:ASPxButton>
                            &nbsp;
                            <dx:ASPxButton ID="btn_Back" runat="server" BackColor="Transparent" Height="16px" Width="42px" ToolTip="Back" OnClick="btn_Back_Click" CausesValidation="False">
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <Border BorderStyle="None" />
                            </dx:ASPxButton>
                            &nbsp;
                        </div>
                    </td>
                </tr>
            </table>
            <!-- Header -->
            <table class="width-100">
                <tr>
                    <td>
                        <asp:Label ID="lbl_Store_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:PC_EOP_EOPEdit, lbl_Store_Nm %>" />
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Location" runat="server" SkinID="TXT_V1" Width="320" ReadOnly="true" />
                    </td>
                    <td>
                        <asp:Label ID="lbl_Date_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:PC_EOP_EOPEdit, lbl_Date_Nm %>" />
                    </td>
                    <td>
                        <dx:ASPxDateEdit ID="de_Date" runat="server" Width="120px" ValidationSettings-RequiredField-IsRequired="true" />
                    </td>
                    <td>
                        <asp:Label ID="lbl_EndDate_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:PC_EOP_EOPEdit, lbl_EndDate_Nm %>" />
                    </td>
                    <td>
                        <asp:Label ID="lbl_EndDate" runat="server" SkinID="LBL_NR" />
                    </td>
                    <td>
                        <asp:Label ID="lbl_Status_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:PC_EOP_EOPEdit, lbl_Status_Nm %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_Status" runat="server" SkinID="LBL_NR"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_Desc_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:PC_EOP_EOPEdit, lbl_Desc_Nm %>"></asp:Label>
                    </td>
                    </td>
                    <td colspan="7">
                        <asp:TextBox ID="txt_Desc" runat="server" Width="100%" SkinID="TXT_V1"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_Remark_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:PC_EOP_EOPEdit, lbl_Remark_Nm %>"></asp:Label>
                    </td>
                    <td colspan="7">
                        <asp:TextBox ID="txt_Remark" runat="server" Width="100%" SkinID="TXT_V1"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <br />
            <!-- Detail -->
            <div class="flex flex-between">
                <div>
                </div>
                <div class="flex flex-end">
                    <div class="me-3">
                        <asp:CheckBox runat="server" ID="chk_OnlyEmpty" AutoPostBack="true" Text="Show only empty" OnCheckedChanged="chk_OnlyEmpty_CheckedChanged" />
                    </div>
                    <asp:Button runat="server" ID="btn_SetZero" Text="Set empty to zero" OnClick="btn_SetZero_Click" />
                </div>
            </div>
            <br />
            <asp:GridView ID="gv_EopDt" runat="server" SkinID="GRD_V1" Width="100%" AutoGenerateColumns="False" EmptyDataText="No data" OnRowDataBound="gv_EopDt_RowDataBound">
                <HeaderStyle HorizontalAlign="Left" Height="24" />
                <RowStyle Height="30" />
                <Columns>
                    <asp:BoundField HeaderText="Product" DataField="ProductCode" />
                    <asp:BoundField HeaderText="Name #1" DataField="ProductDesc1" />
                    <asp:BoundField HeaderText="Name #2" DataField="ProductDesc2" />
                    <asp:BoundField HeaderText="Unit" DataField="InventoryUnit" />
                    <asp:TemplateField HeaderText="Qty" ItemStyle-Width="80">
                        <ItemTemplate>
                            <dx:ASPxSpinEdit runat="server" ID="se_Qty" Width="100%" AutoPostBack="true" SpinButtons-ShowIncrementButtons="False" OnValueChanged="se_Qty_ValueChanged" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="" ItemStyle-Width="80">
                        <ItemTemplate>
                            &nbsp;
                            <%--<asp:ImageButton runat="server" ID="btn_SaveItem" Width="16" Visible="false" ImageUrl="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAACXBIWXMAAAABAAAAAQE4IvRAAAAAGXRFWHRTb2Z0d2FyZQB3d3cuaW5rc2NhcGUub3Jnm+48GgAAB0FJREFUeJzlm21wVGcVx3/n2ZcspYQ2DSmiY1sNQbJJgVhasMLQmUISKk1SCwKjBcHCF9vUkVFpHSfiS0daoaWjHQaLoA61UYGlTQIpI0hgHCyRDNnUpiO2M2UqSWp4STvTbNx7/AAL2d0k+3Z3s+Dv097n5dzzP/vcO/ee51whzSzYfWeBwwTnqogX5XMoRQg3AzcBN14e9iFwHuUcRjpFectSq0PV0dL80KnudPon6TBasc87E4tlgsxX8KZwHhXoUNVmY+Tlxir/CTv9BBsDUNlYmEvAs8aCVQJT7bIbwZsCv3bJwNZ9VZ19dhhMOQDl9d483NQK8hhwsw0+xcM5lC0Olzzf8KX2c6kYSj4AilT6vF9TkWdRJqTiRAr0qrBh9kn/C3V1WMkYSCoA5T5voajsBL6QzPw0cEyUFU01/tOJTjSJTqjYW1KDyhtkj3iAe1U4WeHzLk10oiPegXV1mJxvlW4S2CzgSfREGSAH5OHCr0zI/erUnoOHD6PxTIrrElhc73X35ZgdqC5LzcfMIOiu7i7Pyta1rQOxx8Zgcb3X3eeWPcBCW7zLHA09XTk1sYIw8j1AkYtu2ca1Jx7ggQkTAzvr6kbWOOI9oLysdLPAWnv9yiilZyYWjP3n77tfH27AsJdAha90Mar16fErw6gu31/T8fJQXUMGoHJPyWdVaAXGp9WxzHHBEZSyhi+3/yuyI/r6UESF33D9iAcYH3RYO9DoPzwqAOU+72qy6yHHJmROxT7vI1Gtgw/K67154pZOID9jfmWWbo86p+ytaTsfaghbASaHJ7h+xQMU9Evw8cENV1ZAZWNhrgY875K5V9rRotctA7eH8glXV0DAs4ZrSLzTuHhs2g/wLTrBr+5/jeK86fFOzQvgfjR0MPgSWGGrh2nEaVw8NfPnLLx9MW6TwyfH3sa3y34SvwHV1aGfBi7l8BRK7HfVfkLiZ028L6w9f8ytiMT9dl9c6SueAaEVYHFNvOUNJx7g0HsNqMafFFI1y+HKJWAW2ONi+nAaF08OI761+xi/PPXThOwp3A8gC3bfWWCMdZY0pcjtICR+9jDiNxyvJWD1J2pWXY5AgXGY4Fz+/8QDyIDlnmtUJWtvfmkUD4Ba4jUIU1JxMl2kWzyAiE5xCkyOK3uYQZzGxfdnbuKeifOi+k50H+VHx59IWTwASpHRLHv2D/3zQ4lv7T5mn3gAId8A4+Id7zJuct3pe1rOxLKPYJzh6hb1iMyZtID6hUd5pfIIT83cRI7D3q2BURAPlwMQEyOG2ul1eBxjAPjipPn8cNYvbAvCKIkHLj0JfhhrkFNceJw3hLVNy7/bliCMpnigzwAx99kDVj9N7/4hqj3VIIyyeIA+I/BBPCNfbH+aQ2caotqn5d/Nj2dvZUzEColFFogH5QNH4bJb5wGlsccqfz37ZyaN/TR35BaF9RXc8Am8t3yeo+83818r5nZcdoi/xCED2hnvaEstnv37k0OuhJJbytgw68WYKyGLxIPRTqOW+hOZk0oQsko8gEqHUXW0QHx76SGSCULWiQfL5Qi0OE6/0vXR5KUFDwMFicy+ek+4jTtyJ4f1Rd4TslA8IG2NVf/YbAAUHXb3dCQurYT1HDrTGNUXWgnj3OOzUDyAHoTLiZCFvpK7LOWNZE0ZMawre5r7PhVdRnAxcIFcd/Q24+iKB6M6o7Gmo+3qxsjekvZUMsMOcfDduzYyZ1Ls9KKtr7TJ8eb+ar8XwvcFdqZiMahBfnbiO7S83zziuCwQDyIvhX5eCYBLBrYCKVVdxgpCVoiHXjeBbaGDKwHYV9XZp+gLqVoPBeGdi2+Htf/7o/eyQTyq8tzgOuOw12Gn0zyH0JPqSYIapPYvS/lb1xEu9PfS1nOcbx5eMuriBbqCA+4tEW3hVPpKVqnyUmT79YAqjxyo8f92cFv0foAiFb6SFuDeTDmWCRSOHKjyz0PCn3qjM0KCirICuJAp59KNwnlnUL4eKR6GKZRsqvGfFvQb6XctM4iweqgKMRihUrSpuuOPqG5Kn1uZQUQ27q/y7x6uf8Sk6P7qjnWgO2z3KkMIuuuek+3rRxozclZY0J4uzxog+m0nyxHhte4uz8pYX5LETIu3rm0d+Hh8fpUq2+1zL80ov+s+m/OQLeXyg4xKha9kI7AuFd/SjIrIM00Ptn9vqDv+UCRcF1C5p7RaRbeTfRVlF1V59ECNP6EC76QKIx74U+lngg5rB8icZObbjcIRNWZl84On3kl0buqfzSHPkGA6zUZ6RWV9U3X7tniXfCQpl8ZU75l+U78EH1e0FshL1V6c/EdVng8OuLccXNKa0hOrbbVB8+q9N3pyzBqUVaBeu+wORsCv6PaPA2w7vKQj5p5mnDbtp9JXPEPVLEeYj1JKEt8nXsZSOCXwulHd1VjT0Wann5CB6rBFrxblD1juuaDFokxVpAghD434fF44j9ILdCL6FiodLkeg5dVFb8e1d5ks/wOfPRsX9RRdmgAAAABJRU5ErkJggg==" />--%>
                            <asp:Button runat="server" ID="btn_SaveItem" Text="save" Visible="false" OnClick="btn_SaveItem_Click" />
                            <asp:HiddenField runat="server" ID="hf_EopId" />
                            <asp:HiddenField runat="server" ID="hf_EopDtId" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <!-- Popup(s) -->
            <dx:ASPxPopupControl ID="pop_New" ClientInstanceName="pop_New" runat="server" Width="360px" CloseAction="CloseButton" HeaderText="Create Physical Count"
                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False" ShowPageScrollbarWhenModal="True">
                <ContentStyle VerticalAlign="Top">
                </ContentStyle>
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl0" runat="server">
                        <div>
                            <div style="margin-bottom: 5px;">
                                <asp:Label runat="server" SkinID="LBL_NR" Text="Location" />
                            </div>
                            <dx:ASPxComboBox runat="server" ID="ddl_Location_New" Width="100%" AutoPostBack="true" EnableCallbackMode="true" CallbackPageSize="50" IncrementalFilteringMode="Contains">
                            </dx:ASPxComboBox>
                        </div>
                        <br />
                        <br />
                        <div class="flex flex-between">
                            <asp:Button ID="btn_New_Create" runat="server" Width="50px" SkinID="BTN_V1" Text="Create" OnClick="btn_New_Create_Click" />
                            <asp:Button ID="btn_New_Cancel" runat="server" Width="50px" SkinID="BTN_V1" Text="Cancel" OnClientClick="location.href='EOPList.aspx'" />
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <!-- Dialog -->
            <dx:ASPxPopupControl ID="pop_Alert" ClientInstanceName="pop_Alert" runat="server" Width="300px" CloseAction="CloseButton" HeaderText="" Modal="True" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" ShowCloseButton="False" ShowPageScrollbarWhenModal="True">
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
            <dx:ASPxPopupControl ID="pop_Confirm_SetZero" ClientInstanceName="pop_Confirm_SetZero" runat="server" Width="300px" CloseAction="CloseButton" HeaderText="Confirmation"
                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False" ShowPageScrollbarWhenModal="True">
                <ContentStyle VerticalAlign="Top">
                </ContentStyle>
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <div style="width: 100%; text-align: center;">
                            <asp:Label ID="Label1" runat="server" SkinID="LBL_NR" Text="All empty items will be set to 0.00?"></asp:Label>
                        </div>
                        <br />
                        <div style="width: 100%; text-align: center;">
                            <asp:Button ID="btn_Yes_Confirm_SetZero" runat="server" Width="50px" SkinID="BTN_V1" Text="Yes" OnClick="btn_Yes_Confirm_SetZero_Click" />
                            &nbsp; &nbsp; &nbsp;
                            <asp:Button ID="btn_No_Confirm_SetZero" runat="server" Width="50px" SkinID="BTN_V1" Text="No" OnClientClick="pop_Confirm_SetZero.Hide();" />
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_Confirm_Delete" ClientInstanceName="pop_Confirm_Delete" runat="server" Width="300px" CloseAction="CloseButton" HeaderText="Confirmation"
                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False" ShowPageScrollbarWhenModal="True">
                <ContentStyle VerticalAlign="Top">
                </ContentStyle>
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                        <div style="width: 100%; text-align: center;">
                            <asp:Label ID="lbl_Confirm_Delete" runat="server" SkinID="LBL_NR"></asp:Label>
                        </div>
                        <br />
                        <div style="width: 100%; text-align: center;">
                            <asp:Button ID="btn_Confirm_Delete_Yes" runat="server" Width="50px" SkinID="BTN_V1" Text="Yes" OnClick="btn_Confirm_Delete_Yes_Click" />
                            &nbsp; &nbsp; &nbsp;
                            <asp:Button ID="Button2" runat="server" Width="50px" SkinID="BTN_V1" Text="No" OnClientClick="pop_Confirm_Delete.Hide();" />
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="UpdatePanel">
        <ProgressTemplate>
            <div class="fix-layout" style="border-style: solid; border-width: 1px; border-color: #0071BD; background-color: #FFFFFF; width: 120px; height: 60px">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 120px; height: 60px">
                    <tr>
                        <td align="center">
                            <asp:Image ID="img_Loading2" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/ajax-loader.gif" EnableViewState="False" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lbl_Loading2" runat="server" Font-Bold="true" Text="Loading..." EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
