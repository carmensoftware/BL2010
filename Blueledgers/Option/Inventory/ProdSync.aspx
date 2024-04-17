<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="ProdSync.aspx.cs" Inherits="BlueLedger.PL.Option.Inventory.ProdSync" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <table border="0" cellpadding="3" cellspacing="0" width="100%">
        <tr>
            <td align="left">
                <table border="0" cellpadding="3" cellspacing="0" style="width: 100%;">
                    <tr style="height: 35px;">
                        <td style="background-image: url(<%= Page.ResolveUrl("~")%>/App_Themes/Default/Images/master/pc/blue/bg_title.png)">
                            <asp:Label ID="lbl_Title" runat="server" Text="Product - Create From HQ" Font-Size="13pt"
                                Font-Bold="true" ForeColor="White"></asp:Label>
                        </td>
                        <td align="right" style="background-image: url(<%= Page.ResolveUrl("~")%>/App_Themes/Default/Images/master/pc/blue/bg_title.png)">
                            <table border="0" cellpadding="1" cellspacing="0">
                                <tr>
                                    <td>
                                        <%--<dx:ASPxButton ID="btn_Back" runat="server" Text="Back" 
                                            OnClick="btn_Back_Click">
                                        </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_Save" runat="server" Text="Save" 
                                            OnClick="btn_Save_Click" Width="50px"  SkinID="BTN_V1"/>
                                    </td>
                                    <td>
                                        <%--<dx:ASPxTreeList ID="tl_Product" runat="server" AutoGenerateColumns="False" KeyFieldName="CategoryCode"
                    OnLoad="tl_Product_Load" ParentFieldName="ParentNo" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                    CssPostfix="Aqua" Width="100%">
                    <Styles CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
                        <CustomizationWindowContent VerticalAlign="Top">
                        </CustomizationWindowContent>
                    </Styles>
                    <Images SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                        <LoadingPanel Url="~/App_Themes/Aqua/TreeList/Loading.gif">
                        </LoadingPanel>
                    </Images>
                    <SettingsLoadingPanel ImagePosition="Top" />
                    <SettingsSelection AllowSelectAll="True" Enabled="True" Recursive="True" />
                    <Columns>
                        <dx:TreeListTextColumn Caption="Assign Item" FieldName="CategoryName" VisibleIndex="0">
                        </dx:TreeListTextColumn>
                    </Columns>
                    <ImagesEditors>
                        <DropDownEditDropDown>
                            <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
                        </DropDownEditDropDown>
                        <SpinEditIncrement>
                            <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua"
                                PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua" />
                        </SpinEditIncrement>
                        <SpinEditDecrement>
                            <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua"
                                PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua" />
                        </SpinEditDecrement>
                        <SpinEditLargeIncrement>
                            <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua"
                                PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua" />
                        </SpinEditLargeIncrement>
                        <SpinEditLargeDecrement>
                            <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua"
                                PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua" />
                        </SpinEditLargeDecrement>
                    </ImagesEditors>
                </dx:ASPxTreeList>--%>
                                        <asp:Button ID="btn_Back" runat="server" Text="Back" 
                                            OnClick="btn_Back_Click" SkinID="BTN_V1"  Width="50px"/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <br />
                <%--<dx:ASPxGridView ID="grd_Product" runat="server" AutoGenerateColumns="False" 
                    KeyFieldName="ProductCode" OnLoad="grd_Product_Load" SkinID="Default2">
                    <SettingsPager Mode="ShowAllRecords">
                    </SettingsPager>
                    <Columns>
                        <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0">
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn Caption="SKU#" FieldName="ProductCode" VisibleIndex="1"
                            Width="20%">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Description" FieldName="ProductDesc1" VisibleIndex="2"
                            Width="40%">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Other Description" FieldName="ProductDesc2" VisibleIndex="3"
                            Width="40%">
                        </dx:GridViewDataTextColumn>
                    </Columns>
                </dx:ASPxGridView>--%>
                <asp:GridView ID="grd_Product" runat="server" AutoGenerateColumns="False" 
                    EnableModelValidation="True" SkinID="GRD_V1" Width="100%" 
                    onload="grd_Product_Load" onrowdatabound="grd_Product_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:CheckBox ID="chk_Item" runat="server" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="5%" />
                            <ItemStyle Width="5%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SKU #">
                        <ItemTemplate>
                            <asp:Label ID="lbl_ProductCode" runat="server" SkinID="LBL_NR"></asp:Label>
                        </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                        <ItemTemplate>
                            <asp:Label ID="lbl_Desc" runat="server" SkinID="LBL_NR"></asp:Label>
                        </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="40%" />
                            <ItemStyle HorizontalAlign="Left" Width="40%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Other Description">
                        <ItemTemplate>
                            <asp:Label ID="lbl_OthDesc" runat="server" SkinID="LBL_NR"></asp:Label>
                        </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="40%" />
                            <ItemStyle HorizontalAlign="Left" Width="40%" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <%--<dx:ASPxTreeList ID="tl_Product" runat="server" AutoGenerateColumns="False" KeyFieldName="CategoryCode"
                    OnLoad="tl_Product_Load" ParentFieldName="ParentNo" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                    CssPostfix="Aqua" Width="100%">
                    <Styles CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
                        <CustomizationWindowContent VerticalAlign="Top">
                        </CustomizationWindowContent>
                    </Styles>
                    <Images SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                        <LoadingPanel Url="~/App_Themes/Aqua/TreeList/Loading.gif">
                        </LoadingPanel>
                    </Images>
                    <SettingsLoadingPanel ImagePosition="Top" />
                    <SettingsSelection AllowSelectAll="True" Enabled="True" Recursive="True" />
                    <Columns>
                        <dx:TreeListTextColumn Caption="Assign Item" FieldName="CategoryName" VisibleIndex="0">
                        </dx:TreeListTextColumn>
                    </Columns>
                    <ImagesEditors>
                        <DropDownEditDropDown>
                            <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
                        </DropDownEditDropDown>
                        <SpinEditIncrement>
                            <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua"
                                PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua" />
                        </SpinEditIncrement>
                        <SpinEditDecrement>
                            <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua"
                                PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua" />
                        </SpinEditDecrement>
                        <SpinEditLargeIncrement>
                            <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua"
                                PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua" />
                        </SpinEditLargeIncrement>
                        <SpinEditLargeDecrement>
                            <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua"
                                PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua" />
                        </SpinEditLargeDecrement>
                    </ImagesEditors>
                </dx:ASPxTreeList>--%>
                <dx:ASPxPopupControl ID="pop_Message" runat="server" Modal="True" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" HeaderText="Information">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <asp:Label ID="lbl_Message" runat="server"></asp:Label>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
            </td>
        </tr>
    </table>
</asp:Content>
