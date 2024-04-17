<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="MLtoPr.aspx.cs" Inherits="BlueLedger.PL.PC.PR.MLtoPr" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">--%>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <table border="0" cellpadding="1" cellspacing="0" style="width: 100%;">
        <tr>
            <td align="left">
                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                    <tr style="background-color: #4d4d4d; height: 17px">
                        <td style="padding-left: 10px;width:10px">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                        </td>
                        <td>
                            <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:PC_PR_MLtoPr, lbl_Title %>" SkinID="LBL_HD_WHITE"></asp:Label>
                        </td>
                        <td align="right" style="padding-left: 10px;">
                            <dx:ASPxMenu runat="server" ID="menu_CmdBar" Font-Bold="True" BackColor="Transparent"
                                Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle" Height="16px"
                                OnItemClick="menu_CmdBar_ItemClick">
                                <ItemStyle BackColor="Transparent">
                                    <HoverStyle BackColor="Transparent">
                                        <Border BorderStyle="None" />
                                    </HoverStyle>
                                    <Paddings Padding="2px" />
                                    <Border BorderStyle="None" />
                                </ItemStyle>
                                <Items>
                                    <dx:MenuItem Name="Save" Text="">
                                        <ItemStyle Height="16px" Width="42px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/save.png"
                                                Repeat="NoRepeat" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Back" Text="">
                                        <ItemStyle Height="16px" Width="42px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat"
                                                HorizontalPosition="center" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                </Items>
                                <Paddings Padding="0px" />
                                <SeparatorPaddings Padding="0px" />
                                <SubMenuStyle HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" Font-Size="9pt"
                                    ForeColor="#4D4D4D" />
                                <Border BorderStyle="None"></Border>
                            </dx:ASPxMenu>
                        </td>
                        <%--<td width="14px">
                            <dx:ASPxButton ID="btn_Save" runat="server" OnClick="btn_Save_Click" BackColor="Transparent">
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/in/Default/icon_active_save.png"
                                    HorizontalPosition="center" Repeat="NoRepeat" />
                                <HoverStyle BackColor="White">
                                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/in/Default/icon_over_save.png"
                                        HorizontalPosition="center" Repeat="NoRepeat" />
                                </HoverStyle>
                                <Border BorderStyle="None" />
                            </dx:ASPxButton>
                        </td>
                        <td width="14px">
                            <dx:ASPxButton ID="btn_Back" runat="server" OnClick="btn_Back_Click" BackColor="Transparent">
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/in/Default/icon_active_back.png"
                                    HorizontalPosition="center" Repeat="NoRepeat" />
                                <HoverStyle BackColor="White">
                                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/in/Default/icon_over_back.png"
                                        HorizontalPosition="center" Repeat="NoRepeat" />
                                </HoverStyle>
                                <Border BorderStyle="None" />
                            </dx:ASPxButton>
                        </td>--%>
                    </tr>
                </table>
                <%--<table border="0" cellpadding="3" cellspacing="0" width="100%">
                    <tr style="height: 35px;">
                        <td style="background-image: url(<%= Page.ResolveUrl("~")%>/App_Themes/Default/Images/master/pc/blue/bg_title.png)">
                            <asp:Label ID="lbl_Title" runat="server" Text="Purchase Request" Font-Size="13pt"
                                Font-Bold="true" ForeColor="White"></asp:Label>
                        </td>
                        <td align="right" style="background-image: url(<%= Page.ResolveUrl("~")%>/App_Themes/Default/Images/master/pc/blue/bg_title.png)">
                            <table border="0" cellpadding="1" cellspacing="0">
                                <tr>
                                    <td>
                                        <dx:ASPxButton ID="btn_Save" runat="server" OnClick="btn_Save_Click"
                                            Text="OK">
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btn_Back" runat="server" OnClick="btn_Back_Click"
                                            Text="Back" CausesValidation="False">
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>--%>
                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                    <tr>
                        <td style="padding-left:10px; width:20%">
                            <asp:Label ID="lbl_Deli_Nm" runat="server" Font-Bold="True" Text="<%$ Resources:PC_PR_MLtoPr, lbl_Deli_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td>
                            <dx:ASPxDateEdit ID="txt_DeliveryDate" runat="server" DisplayFormatString="dd/MM/yyyy"
                                EditFormat="Custom" EditFormatString="dd/MM/yyyy">
                                <ValidationSettings Display="Dynamic">
                                    <RequiredField IsRequired="True" />
                                </ValidationSettings>
                            </dx:ASPxDateEdit>
                        </td>
                        <td style="width:10%">
                            &nbsp;
                        </td>
                        <td style="width:15%">
                            <asp:Label ID="lbl_Desc_Nm" runat="server" Font-Bold="True" Text="<%$ Resources:PC_PR_MLtoPr, lbl_Desc_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td style="width:55%">
                            <asp:Label ID="lbl_Desc" runat="server" SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="grd_TemplateDetail" runat="server" AutoGenerateColumns="False"
                    Width="100%" SkinID="GRD_V1" BackColor="WhiteSmoke" 
                    EnableModelValidation="True">
                    <%--Old SkinID="Aqua"--%>
                    <Columns>
                        <%--CssClass="gvHeaderRow_Aqua" <ItemStyle CssClass="gvDataRow_Aqua" />--%>
                        <asp:BoundField DataField="ProductCode" HeaderText="<%$ Resources:PC_PR_MLtoPr, lbl_SKU_Nm %>">
                            <HeaderStyle Width="100px" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Name" HeaderText="<%$ Resources:PC_PR_MLtoPr, lbl_Name_Nm %>">
                            <HeaderStyle Width="400px" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LocalName" HeaderText="<%$ Resources:PC_PR_MLtoPr, lbl_LocalName_Nm %>">
                            <HeaderStyle Width="400px" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UnitCode" HeaderText="<%$ Resources:PC_PR_MLtoPr, lbl_Unit_Nm %>">
                            <HeaderStyle Width="100px" HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="<%$ Resources:PC_PR_MLtoPr, lbl_ReqQty_Nm %>">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_ReqQty" runat="server" BorderWidth="1px" Width="95%" 
                                    SkinID="TXT_NUM_V1"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender" runat="server"
                                    TargetControlID="txt_ReqQty" ValidChars="0123456789.">
                                </ajaxToolkit:FilteredTextBoxExtender>
                            </ItemTemplate>
                            <HeaderStyle Width="120px" HorizontalAlign="Right"/>
                            <ItemStyle HorizontalAlign="Center"/>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
