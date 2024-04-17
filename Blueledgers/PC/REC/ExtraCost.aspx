<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="ExtraCost.aspx.cs" Inherits="BlueLedger.PL.IN.REC.ExtraCost" %>

<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <script type="text/javascript">

        //Check Select All CheckBox.
        function CheckAll(parentChk) {
            var elements = document.getElementsByTagName("input");
            for (i = 0; i < elements.length; i++) {
                if (parentChk.checked == true) {
                    if (IsCheckBox(elements[i])) {
                        elements[i].checked = true;
                    }
                }
                else {
                    elements[i].checked = false;
                }
            }
        }

        function CheckItem(containerID, childID) {
            var elms = document.getElementById(containerID).getElementsByTagName("input");
            for (var i = 0; i < elms.length; i++) {
                if (IsCheckBox(elms[i])) {
                    if (childID.checked == false)
                        elms[i].checked = false;
                }
            }
        }


        function IsCheckBox(chk) {
            if (chk.type == 'checkbox')
                return true;
            else
                return false;
        }


    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--Header Bar--%>
            <div style="margin-top: 10px; width: 100%; height: 24px; background-color: #4D4D4D;">
                <div style="display: inline-block; padding-left: 5px;">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                </div>
                <div style="display: inline-block;">
                    <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_HD_WHITE" Text="Extra Cost Type" />
                </div>
                <div style="display: inline-block; float: right;">
                    <dx:ASPxMenu runat="server" ID="menu_Item" Font-Bold="True" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle"
                        Height="16px" OnItemClick="menu_Item_Click">
                        <ItemStyle BackColor="Transparent">
                            <HoverStyle BackColor="Transparent">
                                <Border BorderStyle="None" />
                            </HoverStyle>
                            <Paddings Padding="2px" />
                            <Border BorderStyle="None" />
                        </ItemStyle>
                        <Items>
                            <dx:MenuItem Name="Create" Text="">
                                <ItemStyle Height="16px" Width="49px">
                                    <HoverStyle>
                                        <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png" Repeat="NoRepeat" VerticalPosition="center" />
                                    </HoverStyle>
                                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/create.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                </ItemStyle>
                            </dx:MenuItem>
                            <dx:MenuItem Name="Delete" Text="">
                                <ItemStyle Height="16px" Width="47px">
                                    <HoverStyle>
                                        <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png" Repeat="NoRepeat" VerticalPosition="center" />
                                    </HoverStyle>
                                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
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
                        </Items>
                    </dx:ASPxMenu>
                </div>
            </div>
            <%-- Body --%>
            <div class="printable" style="clear: both;">
                <asp:GridView ID="grd_ExtCostType" runat="server" AutoGenerateColumns="False" SkinID="GRD_V1" Width="100%" OnRowDataBound="grd_ExtCostType_RowDataBound"
                    OnRowEditing="grd_ExtCostType_RowEditing" OnRowUpdating="grd_ExtCostType_RowUpdating" OnRowCancelingEdit="grd_ExtCostType_RowCancelingEdit">
                    <Columns>
                        <%--using as Primay Index--%>
                        <asp:BoundField DataField="TypeId" Visible="false" />
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <div id="Div_Chk_All">
                                    <asp:CheckBox ID="Chk_All" runat="server" onclick="CheckAll(this)" />
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="Chk_Item" runat="server" SkinID="CHK_V1" onclick="CheckItem('Div_Chk_All',this)" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" Width="10px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkb_Edit" runat="server" CommandName="Edit" SkinID="LNKB_NORMAL">Edit</asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div style="display: inline-block;">
                                    <asp:LinkButton ID="lnkb_Update" runat="server" CommandName="Update" SkinID="LNKB_NORMAL">Update</asp:LinkButton>
                                </div>
                                <div style="display: inline-block;">
                                    <asp:LinkButton ID="lnkb_Cancel" runat="server" CommandName="Cancel" SkinID="LNKB_NORMAL">Cancel</asp:LinkButton>
                                </div>
                            </EditItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" Width="120px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lbl_TypeName" Text="<%#Bind('TypeName')%>"></asp:Label>
                                <asp:HiddenField runat="server" ID="hf_TypeId" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox runat="server" ID="txt_TypeName" Text="<%#Bind('TypeName')%>"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hf_TypeId" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <dx:ASPxPopupControl ID="pop_Create" runat="server" Width="300px" HeaderText="New" CloseAction="CloseButton" ShowCloseButton="true" Modal="True" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <div>
                            <asp:Label runat="server" ID="lbl_TypeName" Text="Name" />
                            <asp:TextBox runat="server" ID="txt_TypeName" Width="280px" />
                        </div>
                        <div>
                            <asp:Label runat="server" ID="lbl_TypeName_Error" ForeColor="Red" />
                        </div>
                        <br />
                        <div style="text-align: right;">
                            <asp:Button runat="server" ID="btn_Save" Text="Save" OnClick="btn_Save_Click" />
                            <asp:Button runat="server" ID="btn_Cancel" Text="Cancel" OnClick="btn_Cancel_Click" />
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle HorizontalAlign="Left" />
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_Alert" runat="server" Width="300px" HeaderText="Warning" CloseAction="CloseButton" ShowCloseButton="true" Modal="True" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                        <div style="text-align: center;">
                            <asp:Label runat="server" ID="lbl_Alert" />
                        </div>
                        <br />
                        <div style="text-align: center;">
                            <asp:Button runat="server" ID="btn_AlertOK" Width="60px" Text="Ok" OnClick="btn_AlertOK_Click" />
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle HorizontalAlign="Left" />
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_Confirmation" runat="server" Width="300px" HeaderText="Confirmation" CloseAction="CloseButton" ShowCloseButton="true" Modal="True"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                        <div style="text-align: center;">
                            <asp:Label runat="server" ID="lbl_Confirm" />
                        </div>
                        <br />
                        <div style="text-align: center;">
                            <asp:Button runat="server" ID="btn_ConfirmYes" Width="60px" Text="Yes" OnClick="btn_ConfirmYes_Click" />
                            <asp:Button runat="server" ID="btn_ConfirmNo" Width="60px" Text="No" OnClick="btn_ConfirmNo_Click" />
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle HorizontalAlign="Left" />
            </dx:ASPxPopupControl>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="menu_Item" EventName="ItemClick" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
