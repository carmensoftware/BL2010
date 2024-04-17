<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="Bu.aspx.cs" Inherits="BlueLedger.PL.Option.Admin.Bu.Bu" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxNavBar" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <table border="0" cellpadding="1" cellspacing="0" width="100%">
        <tr>
            <td align="left">
                <!-- Title -->
                <table class="tableSave" border="0" cellpadding="0" cellspacing="0" width="100%"> <%--New Theme not have 'width="100%"'. Now use Old Version Theme--%>
                    <tr style="height: 28px">
                        <td>
                            <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_Title %>" Font-Size="13pt"
                                Font-Bold="True" ForeColor="Black"></asp:Label><%--New Theme ForeColor="White". Now use Old Version Theme--%>
                        </td>
                        <td align="right">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="save">
                                        <dx:ASPxButton ID="btn_Save" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, btn_Save %>" Paddings-PaddingLeft="10px"
                                            Paddings-PaddingTop="2px" Font-Bold="true" Font-Names="Arial" BackColor="White"
                                            Border-BorderStyle="none" BackgroundImage-ImageUrl="~/App_Themes/Default/Images/master/pt/default/save.jpg"
                                            Width="70px" Height="25px" onclick="btn_Save_Click">
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btn_Cancel" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, btn_Cancel %>" Paddings-PaddingLeft="13px"
                                            Paddings-PaddingTop="2px" Font-Bold="true" Font-Names="Arial" BackColor="White"
                                            Border-BorderStyle="none" BackgroundImage-ImageUrl="~/App_Themes/Default/Images/master/pt/default/cencel.jpg"
                                            Width="70px" Height="25px" onclick="btn_Cancel_Click">
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table border="0" width="100%">
                    <dx:ASPxNavBar ID="ASPxNavBar" runat="server" Width="100%">
                        <Groups>
                            <%-- General Information --%>
                            <%-- Address --%>
                            <%-- Delivery Terms --%>
                            <%-- Regional and Language --%>
                            <dx:NavBarGroup Text="<%$ Resources:Option_Admin_Bu_Bu, GeneralInfo %>" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true"
                                HeaderStyle-Font-Names="Arial" HeaderStyle-BackColor="#8f8f8f" ContentStyle-Border-BorderStyle="None">
                                <HeaderStyle BackColor="#8F8F8F" Font-Bold="True" Font-Names="Arial" ForeColor="White">
                                </HeaderStyle>
                                <ContentTemplate>
                                    <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                        <tr>
                                            <td width="15%">
                                                <asp:Label ID="lbl_BuCode" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_BuCode %>" Font-Bold="True" Font-Names="Arial"
                                                    ForeColor="Black"></asp:Label>
                                            </td>
                                            <td width="23%">
                                                <dx:ASPxTextBox ID="txt_BuCode" runat="server" BackColor="#f2f2f2" Width="100%">
                                                    <ValidationSettings>
                                                        <ErrorFrameStyle ImageSpacing="4px">
                                                            <ErrorTextPaddings PaddingLeft="4px" />
                                                        </ErrorFrameStyle>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="15%">
                                                <asp:Label ID="lbl_BuName" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_BuName %>" Font-Bold="True" Font-Names="Arial"
                                                    ForeColor="Black"></asp:Label>
                                            </td>
                                            <td width="23%">
                                                <dx:ASPxTextBox ID="txt_BuName" runat="server" BackColor="#f2f2f2" Width="100%">
                                                    <ValidationSettings>
                                                        <ErrorFrameStyle ImageSpacing="4px">
                                                            <ErrorTextPaddings PaddingLeft="4px" />
                                                        </ErrorFrameStyle>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="15%">
                                                <asp:Label ID="lbl_BillName" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_BillName %>" Font-Bold="True" Font-Names="Arial"
                                                    ForeColor="Black"></asp:Label>
                                            </td>
                                            <td width="23%">
                                                <dx:ASPxTextBox ID="txt_BuNameBilling" runat="server" BackColor="#f2f2f2" Width="100%">
                                                    <ValidationSettings>
                                                        <ErrorFrameStyle ImageSpacing="4px">
                                                            <ErrorTextPaddings PaddingLeft="4px" />
                                                        </ErrorFrameStyle>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="15%">
                                                <asp:Label ID="lbl_CalcType" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_CalcType %>" Font-Bold="True" Font-Names="Arial"
                                                    ForeColor="Black"></asp:Label>
                                            </td>
                                            <td width="23%">
                                                <dx:ASPxComboBox ID="ddl_CalcType" ForeColor="Black" Font-Names="Arial" runat="server"
                                                    BackColor="#f2f2f2" Width="100%" ValueType="System.Int32" TextFormatString="{0} : {1}">
                                                    <Items>
                                                        <dx:ListEditItem Text="AVERAGE" Value="0" />
                                                        <dx:ListEditItem Text="FIFO" Value="1" />
                                                    </Items>
                                                </dx:ASPxComboBox>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </dx:NavBarGroup>
                            <dx:NavBarGroup Text="<%$ Resources:Option_Admin_Bu_Bu, Address %>" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true"
                                HeaderStyle-Font-Names="Arial" HeaderStyle-BackColor="#8f8f8f" ContentStyle-Border-BorderStyle="None">
                                <HeaderStyle BackColor="#8F8F8F" Font-Bold="True" Font-Names="Arial" ForeColor="White">
                                </HeaderStyle>
                                <ContentTemplate>
                                    <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                        <tr>
                                            <td width="15%">
                                                <asp:Label ID="lbl_Address" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_Address %>" Font-Bold="True" Font-Names="Arial"
                                                    ForeColor="Black"></asp:Label>
                                            </td>
                                            <td colspan="5">
                                                <dx:ASPxTextBox ID="txt_Address" runat="server" BackColor="#f2f2f2" ForeColor="Black"
                                                    Width="100%">
                                                    <ValidationSettings>
                                                        <ErrorFrameStyle ImageSpacing="4px">
                                                            <ErrorTextPaddings PaddingLeft="4px" />
                                                        </ErrorFrameStyle>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="15%">
                                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_PostCode %>" Font-Bold="True" Font-Names="Arial"
                                                    ForeColor="Black"></asp:Label>
                                            </td>
                                            <td width="23%">
                                                <dx:ASPxTextBox ID="txt_PostCode" ForeColor="Black" Font-Names="Arial" runat="server"
                                                    BackColor="#f2f2f2" Width="100%">
                                                    <ValidationSettings>
                                                        <ErrorFrameStyle ImageSpacing="4px">
                                                            <ErrorTextPaddings PaddingLeft="4px" />
                                                        </ErrorFrameStyle>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td width="5%" align="center">
                                                <asp:Label ID="lbl_City" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_City %>" Font-Bold="True" Font-Names="Arial"
                                                    ForeColor="Black"></asp:Label>
                                            </td>
                                            <td width="23%">
                                                <dx:ASPxComboBox ID="ddl_City" ForeColor="Black" Font-Names="Arial" runat="server"
                                                    BackColor="#f2f2f2" Width="100%" ValueType="System.String" AutoPostBack="True" TextFormatString="{0} : {1}" IncrementalFilteringMode="Contains">
                                                    <LoadingPanelImage Url="~/App_Themes/Aqua/Editors/Loading.gif">
                                                    </LoadingPanelImage>
                                                    <DropDownButton>
                                                        <Image>
                                                            <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
                                                        </Image>
                                                    </DropDownButton>
                                                    <Columns>
                                                        <dx:ListBoxColumn Caption="Code" FieldName="Code" />
                                                        <dx:ListBoxColumn Caption="Name" FieldName="Name" />
                                                    </Columns>
                                                    <ValidationSettings>
                                                        <ErrorFrameStyle ImageSpacing="4px">
                                                            <ErrorTextPaddings PaddingLeft="4px" />
                                                        </ErrorFrameStyle>
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </td>
                                            <td width="10%" align="center">
                                                <asp:Label ID="lbl_Country" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_Country %>" Font-Bold="True" Font-Names="Arial"
                                                    ForeColor="Black"></asp:Label>
                                            </td>
                                            <td width="23%">
                                                <dx:ASPxComboBox ID="ddl_Country" ForeColor="Black" Font-Names="Arial" runat="server"
                                                    BackColor="#f2f2f2" Width="100%" LoadingPanelImagePosition="Top" ShowShadow="False"
                                                    ValueType="System.String" OnSelectedIndexChanged="ddl_Country_SelectedIndexChanged"
                                                    OnLoad="ddl_Country_OnLoad" AutoPostBack="True" TextFormatString="{0} : {1}" IncrementalFilteringMode="Contains">
                                                    <Columns>
                                                        <dx:ListBoxColumn Caption="Code" FieldName="CountryCode" Width="80px" />
                                                        <dx:ListBoxColumn Caption="Name" FieldName="Name" Width="180px" />
                                                    </Columns>
                                                    <LoadingPanelImage Url="~/App_Themes/Aqua/Editors/Loading.gif">
                                                    </LoadingPanelImage>
                                                    <DropDownButton>
                                                        <Image>
                                                            <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
                                                        </Image>
                                                    </DropDownButton>
                                                    <ValidationSettings>
                                                        <ErrorFrameStyle ImageSpacing="4px">
                                                            <ErrorTextPaddings PaddingLeft="4px" />
                                                        </ErrorFrameStyle>
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="15%">
                                                <asp:Label ID="lbl_Phone" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_Phone %>" Font-Bold="True" Font-Names="Arial"
                                                    ForeColor="Black"></asp:Label>
                                            </td>
                                            <td width="23%">
                                                <dx:ASPxTextBox ID="txt_Phone" ForeColor="Black" Font-Names="Arial" runat="server"
                                                    BackColor="#f2f2f2" Width="100%">
                                                    <ValidationSettings>
                                                        <ErrorFrameStyle ImageSpacing="4px">
                                                            <ErrorTextPaddings PaddingLeft="4px" />
                                                        </ErrorFrameStyle>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td width="5%" align="center">
                                                <asp:Label ID="lbl_Fax" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_Fax %>" Font-Bold="True" Font-Names="Arial"
                                                    ForeColor="Black"></asp:Label>
                                            </td>
                                            <td width="23%">
                                                <dx:ASPxTextBox ID="txt_Fax" ForeColor="Black" Font-Names="Arial" runat="server"
                                                    BackColor="#f2f2f2" Width="100%">
                                                    <ValidationSettings>
                                                        <ErrorFrameStyle ImageSpacing="4px">
                                                            <ErrorTextPaddings PaddingLeft="4px" />
                                                        </ErrorFrameStyle>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                            <td width="10%" align="center">
                                                <asp:Label ID="lbl_Email" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_Email %>" Font-Bold="True" Font-Names="Arial"
                                                    ForeColor="Black"></asp:Label>
                                            </td>
                                            <td width="23%">
                                                <dx:ASPxTextBox ID="txt_Email" ForeColor="Black" Font-Names="Arial" runat="server"
                                                    BackColor="#f2f2f2" Width="100%">
                                                    <ValidationSettings>
                                                        <ErrorFrameStyle ImageSpacing="4px">
                                                            <ErrorTextPaddings PaddingLeft="4px" />
                                                        </ErrorFrameStyle>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </dx:NavBarGroup>
                            <dx:NavBarGroup Text="<%$ Resources:Option_Admin_Bu_Bu, DeliTerms %>" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true"
                                HeaderStyle-Font-Names="Arial" HeaderStyle-BackColor="#8f8f8f" ContentStyle-Border-BorderStyle="None">
                                <HeaderStyle BackColor="#8F8F8F" Font-Bold="True" Font-Names="Arial" ForeColor="White">
                                </HeaderStyle>
                                <ContentTemplate>
                                    <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                        <tr>
                                            <td>
                                                <dx:ASPxMemo ID="txt_DeliveryTerm" ForeColor="Black" Font-Names="Arial" runat="server"
                                                    BackColor="#f2f2f2" Height="150px" Width="100%">
                                                    <ValidationSettings>
                                                        <ErrorFrameStyle ImageSpacing="4px">
                                                            <ErrorTextPaddings PaddingLeft="4px" />
                                                        </ErrorFrameStyle>
                                                    </ValidationSettings>
                                                </dx:ASPxMemo>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </dx:NavBarGroup>
                            <dx:NavBarGroup Text="<%$ Resources:Option_Admin_Bu_Bu, RegionalLang %>" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true"
                                HeaderStyle-Font-Names="Arial" HeaderStyle-BackColor="#8f8f8f" ContentStyle-Border-BorderStyle="None">
                                <HeaderStyle BackColor="#8F8F8F" Font-Bold="True" Font-Names="Arial" ForeColor="White">
                                </HeaderStyle>
                                <ContentTemplate>
                                    <dx:ASPxPageControl ID="pc_Regional" runat="server" ActiveTabIndex="0" TabSpacing="3px"
                                        Width="100%">
                                        <ContentStyle BackColor="#f2f2f2">
                                            <Border BorderColor="#99999e" BorderStyle="Solid" BorderWidth="1px" />
                                        </ContentStyle>
                                        <TabPages>
                                            <dx:TabPage Name="tp_Option" Text="<%$ Resources:Option_Admin_Bu_Bu, tp_Option %>" TabStyle-BackColor="#909090" ActiveTabStyle-BackColor="#F2F2F2"
                                                TabStyle-ForeColor="White" ActiveTabStyle-ForeColor="Black">
                                                <ContentCollection>
                                                    <dx:ContentControl runat="server">
                                                        <table border="0" cellpadding="3" cellspacing="0" width="100%" style="background-color: White">
                                                            <tr>
                                                                <td>
                                                                    <fieldset>
                                                                        <legend>
                                                                            <asp:Label ID="lbl_StdFmt" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_StdFmt %>" Font-Names="Arial"></asp:Label></legend>
                                                                        <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                            <tr>
                                                                                <td style="padding-left: 9px;">
                                                                                    <asp:Label ID="lbl_Desc" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_Desc %>"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                                                                        <tr>
                                                                                            <td width="100%" style="padding-left: 9px">
                                                                                                <dx:ASPxComboBox ID="ddl_FmtCode" runat="server" LoadingPanelImagePosition="Top"
                                                                                                    BackColor="#f2f2f2" ShowShadow="False" Width="100%">
                                                                                                    <LoadingPanelImage Url="~/App_Themes/Aqua/Editors/Loading.gif">
                                                                                                    </LoadingPanelImage>
                                                                                                    <DropDownButton>
                                                                                                        <Image>
                                                                                                            <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
                                                                                                        </Image>
                                                                                                    </DropDownButton>
                                                                                                    <ValidationSettings>
                                                                                                        <ErrorFrameStyle ImageSpacing="4px">
                                                                                                            <ErrorTextPaddings PaddingLeft="4px" />
                                                                                                        </ErrorFrameStyle>
                                                                                                    </ValidationSettings>
                                                                                                </dx:ASPxComboBox>
                                                                                            </td>
                                                                                            <td>
                                                                                                <dx:ASPxButton ID="bnt_Custz" runat="server" BackgroundImage-ImageUrl="~/App_Themes/Default/Images/master/pt/default/Bt.jpg"
                                                                                                    OnClick="bnt_Custz_Click" Text="<%$ Resources:Option_Admin_Bu_Bu, bnt_Custz %>">
                                                                                                </dx:ASPxButton>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                                                                        <tr>
                                                                                            <td colspan="2" style="padding-left: 9px">
                                                                                                <asp:Label ID="lbl_Samples" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_Samples %>"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td width="20%" style="padding-left: 9px">
                                                                                                <asp:Label ID="lbl_Number" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_Number %>"></asp:Label>
                                                                                            </td>
                                                                                            <td width="80%">
                                                                                                <dx:ASPxTextBox ID="txt_Number" runat="server" Width="100%" BackColor="#f2f2f2">
                                                                                                    <ValidationSettings>
                                                                                                        <ErrorFrameStyle ImageSpacing="4px">
                                                                                                            <ErrorTextPaddings PaddingLeft="4px" />
                                                                                                        </ErrorFrameStyle>
                                                                                                    </ValidationSettings>
                                                                                                </dx:ASPxTextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="padding-left: 9px">
                                                                                                <asp:Label ID="lbl_Currency" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_Currency %>"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <dx:ASPxTextBox ID="txt_Currency" runat="server" Enabled="False" Width="100%" BackColor="#f2f2f2">
                                                                                                    <ValidationSettings>
                                                                                                        <ErrorFrameStyle ImageSpacing="4px">
                                                                                                            <ErrorTextPaddings PaddingLeft="4px" />
                                                                                                        </ErrorFrameStyle>
                                                                                                    </ValidationSettings>
                                                                                                </dx:ASPxTextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="padding-left: 9px">
                                                                                                <asp:Label ID="lbl_Time" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_Time %>"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <dx:ASPxTextBox ID="txt_Time" runat="server" Enabled="False" Width="100%" BackColor="#f2f2f2">
                                                                                                    <ValidationSettings>
                                                                                                        <ErrorFrameStyle ImageSpacing="4px">
                                                                                                            <ErrorTextPaddings PaddingLeft="4px" />
                                                                                                        </ErrorFrameStyle>
                                                                                                    </ValidationSettings>
                                                                                                </dx:ASPxTextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="padding-left: 9px">
                                                                                                <asp:Label ID="lbl_SDate" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_SDate %>"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <dx:ASPxTextBox ID="txt_SDate" runat="server" Enabled="False" Width="100%" BackColor="#f2f2f2">
                                                                                                    <ValidationSettings>
                                                                                                        <ErrorFrameStyle ImageSpacing="4px">
                                                                                                            <ErrorTextPaddings PaddingLeft="4px" />
                                                                                                        </ErrorFrameStyle>
                                                                                                    </ValidationSettings>
                                                                                                </dx:ASPxTextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="padding-left: 9px">
                                                                                                <asp:Label ID="lbl_LDate" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_LDate %>"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <dx:ASPxTextBox ID="txt_LDate" runat="server" Enabled="False" Width="100%" BackColor="#f2f2f2">
                                                                                                    <ValidationSettings>
                                                                                                        <ErrorFrameStyle ImageSpacing="4px">
                                                                                                            <ErrorTextPaddings PaddingLeft="4px" />
                                                                                                        </ErrorFrameStyle>
                                                                                                    </ValidationSettings>
                                                                                                </dx:ASPxTextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </fieldset>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </dx:ContentControl>
                                                </ContentCollection>
                                            </dx:TabPage>
                                            <dx:TabPage Text="<%$ Resources:Option_Admin_Bu_Bu, Language %>" TabStyle-BackColor="#909090" ActiveTabStyle-BackColor="#F2F2F2"
                                                TabStyle-ForeColor="White" ActiveTabStyle-ForeColor="Black">
                                                <ContentCollection>
                                                    <dx:ContentControl runat="server">
                                                        <table border="0" cellpadding="1" cellspacing="0" width="50%" style="background-color: White">
                                                            <tr>
                                                                <td>
                                                                    <fieldset>
                                                                        <legend>
                                                                            <asp:Label ID="lbl_DisplayLang" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_DisplayLang %>"></asp:Label></legend>
                                                                        <table border="0" cellpadding="3" cellspacing="0" style="width: 100%;">
                                                                            <tr>
                                                                                <td width="20%">
                                                                                    <asp:Label ID="lbl_DefaultLang" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_DefaultLang %>"></asp:Label>
                                                                                </td>
                                                                                <td width="80%">
                                                                                    <dx:ASPxComboBox ID="ddl_DefaultLang" runat="server" Width="100%" LoadingPanelImagePosition="Top" AutoPostBack="true"
                                                                                        BackColor="#f2f2f2" ShowShadow="False" TextFormatString="{0} : {1}" OnLoad="ddl_DefaultLang_OnLoad">
                                                                                        <Columns>
                                                                                            <dx:ListBoxColumn Caption="Code" FieldName="LangCode" Width="80px" />
                                                                                            <dx:ListBoxColumn Caption="Name" FieldName="Name" Width="180px" />
                                                                                        </Columns>
                                                                                        <LoadingPanelImage Url="~/App_Themes/Aqua/Editors/Loading.gif">
                                                                                        </LoadingPanelImage>
                                                                                        <DropDownButton>
                                                                                            <Image>
                                                                                                <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
                                                                                            </Image>
                                                                                        </DropDownButton>
                                                                                        <ValidationSettings>
                                                                                            <ErrorFrameStyle ImageSpacing="4px">
                                                                                                <ErrorTextPaddings PaddingLeft="4px" />
                                                                                            </ErrorFrameStyle>
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxComboBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lbl_OptionalLang" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_OptionalLang %>"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <dx:ASPxComboBox ID="ddl_OptionalLang" runat="server" LoadingPanelImagePosition="Top" AutoPostBack="true"
                                                                                        ShowShadow="False" Width="100%" BackColor="#f2f2f2" TextFormatString="{0} : {1}" OnLoad="ddl_OptionalLang_OnLoad">
                                                                                        <Columns>
                                                                                            <dx:ListBoxColumn Caption="Code" FieldName="LangCode" Width="80px" />
                                                                                            <dx:ListBoxColumn Caption="Name" FieldName="Name" Width="180px" />
                                                                                        </Columns>
                                                                                        <LoadingPanelImage Url="~/App_Themes/Aqua/Editors/Loading.gif">
                                                                                        </LoadingPanelImage>
                                                                                        <DropDownButton>
                                                                                            <Image>
                                                                                                <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
                                                                                            </Image>
                                                                                        </DropDownButton>
                                                                                        <ValidationSettings>
                                                                                            <ErrorFrameStyle ImageSpacing="4px">
                                                                                                <ErrorTextPaddings PaddingLeft="4px" />
                                                                                            </ErrorFrameStyle>
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxComboBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </fieldset>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </dx:ContentControl>
                                                </ContentCollection>
                                            </dx:TabPage>
                                        </TabPages>
                                    </dx:ASPxPageControl>
                                </ContentTemplate>
                            </dx:NavBarGroup>
                        </Groups>
                        <Paddings PaddingLeft="0px" PaddingRight="0px" PaddingTop="0px" />
                    </dx:ASPxNavBar>
                </table>
            </td>
        </tr>
    </table>
    <dx:ASPxPopupControl ID="pop_FormatCustz" runat="server" CloseAction="CloseButton"
        HeaderText="<%$ Resources:Option_Admin_Bu_Bu, pop_FormatCustz %>" Modal="True" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true"
        HeaderStyle-Font-Names="Arial" HeaderStyle-BackColor="#8f8f8f" 
        ContentStyle-Border-BorderStyle="None" Width="420px"
        >
        <ContentStyle BackColor="#f2f2f2">
            <Border BorderStyle="None"></Border>
        </ContentStyle>
        <HeaderStyle BackColor="#8F8F8F" Font-Bold="True" Font-Names="Arial" ForeColor="White">
        </HeaderStyle>
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxPageControl ID="ASPxPageControl2" runat="server" ActiveTabIndex="1" 
                    BackColor="#f2f2f2" Width="410px" >
                    <TabPages>
                        <dx:TabPage Text="<%$ Resources:Option_Admin_Bu_Bu, tp_Numbers %>" TabStyle-BackColor="#909090" ActiveTabStyle-BackColor="#F2F2F2"
                            TabStyle-ForeColor="White" ActiveTabStyle-ForeColor="Black">
                            <ActiveTabStyle BackColor="#F2F2F2" ForeColor="Black">
                            </ActiveTabStyle>
                            <TabStyle BackColor="#909090" ForeColor="#F2F2F2">
                            </TabStyle>
                            <ContentCollection>
                                <dx:ContentControl runat="server" Width="100%">
                                    <fieldset>
                                        <legend>
                                            <asp:Label ID="lbl_Sample" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_Sample %>"></asp:Label></legend>
                                        <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                            <tr>
                                                <td width="15%">
                                                    <asp:Label ID="lbl_Positive" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_Positive %>"></asp:Label>
                                                </td>
                                                <td width="35%">
                                                    <dx:ASPxTextBox ID="txt_NumPos" runat="server" Enabled="False" Width="100%" BackColor="#f2f2f2">
                                                    </dx:ASPxTextBox>
                                                </td>
                                                <td width="15%">
                                                    <asp:Label ID="lbl_Negative" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_Negative %>"></asp:Label>
                                                </td>
                                                <td width="35%">
                                                    <dx:ASPxTextBox ID="txt_NumNeg" runat="server" Enabled="False" Width="100%" BackColor="#f2f2f2">
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <table border="0" cellpadding="3" cellspacing="0" style="width: 100%;">
                                        <tr>
                                            <td width="40%">
                                                <asp:Label ID="lbl_DecSymbol" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_DecSymbol %>"></asp:Label>
                                            </td>
                                            <td width="60%">
                                                <dx:ASPxTextBox ID="txt_FmtNumDec" runat="server" Width="100%" 
                                                    BackColor="#f2f2f2" OnTextChanged="txt_FmtNumDec_TextChanged" 
                                                    AutoPostBack="True">
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="40%">
                                                <asp:Label ID="lbl_DigitDecNo" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_DigitDecNo %>"></asp:Label>
                                            </td>
                                            <td width="60%">
                                                <dx:ASPxComboBox ID="ddl_FmtNumDecNo" runat="server" ValueType="System.String" BackColor="#f2f2f2"
                                                    Width="100%" AutoPostBack="True"  
                                                    OnValueChanged="ddl_FmtNumDecNo_ValueChanged">
                                                    <Items>
                                                        <dx:ListEditItem Text="0" Value="0" />
                                                        <dx:ListEditItem Text="1" Value="1" />
                                                        <dx:ListEditItem Text="2" Value="2" />
                                                        <dx:ListEditItem Text="3" Value="3" />
                                                        <dx:ListEditItem Text="4" Value="4" />
                                                        <dx:ListEditItem Text="5" Value="5" />
                                                        <dx:ListEditItem Text="6" Value="6" />
                                                        <dx:ListEditItem Text="7" Value="7" />
                                                        <dx:ListEditItem Text="8" Value="8" />
                                                        <dx:ListEditItem Text="9" Value="9" />
                                                    </Items>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="40%">
                                                <asp:Label ID="lbl_GrpSymbol" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_GrpSymbol %>"></asp:Label>
                                            </td>
                                            <td width="60%">
                                                <dx:ASPxTextBox ID="txt_FmtNumDecGrp" runat="server" Width="100%" 
                                                    BackColor="#f2f2f2" AutoPostBack="True" 
                                                    OnTextChanged="txt_FmtNumDecGrp_TextChanged">
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="40%">
                                                <asp:Label ID="lbl_NegSymbol" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_NegSymbol %>"></asp:Label>
                                            </td>
                                            <td width="60%">
                                                <dx:ASPxTextBox ID="txt_FmtNumNeg" runat="server" Width="100%" 
                                                    BackColor="#f2f2f2" AutoPostBack="True" 
                                                    OnTextChanged="txt_FmtNumNeg_TextChanged">
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="<%$ Resources:Option_Admin_Bu_Bu, tp_Currency %>" TabStyle-BackColor="#909090" ActiveTabStyle-BackColor="#F2F2F2"
                            TabStyle-ForeColor="White" ActiveTabStyle-ForeColor="Black">
                            <ActiveTabStyle BackColor="#F2F2F2" ForeColor="Black">
                            </ActiveTabStyle>
                            <TabStyle BackColor="#909090" ForeColor="White">
                            </TabStyle>
                            <ContentCollection>
                                <dx:ContentControl runat="server" Width="100%">
                                    <fieldset>
                                        <legend>
                                            <asp:Label ID="Label28" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_Sample %>"></asp:Label></legend>
                                        <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                            <tr>
                                                <td width="15%">
                                                    <asp:Label ID="Label30" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_Positive %>"></asp:Label>
                                                </td>
                                                <td width="35%">
                                                    <dx:ASPxTextBox ID="txt_CurPos" runat="server" Enabled="False" Width="100%" BackColor="#f2f2f2">
                                                    </dx:ASPxTextBox>
                                                </td>
                                                <td width="15%">
                                                    <asp:Label ID="Label31" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_Negative %>"></asp:Label>
                                                </td>
                                                <td width="35%">
                                                    <dx:ASPxTextBox ID="txt_CurNeg" runat="server" Enabled="False" Width="100%" BackColor="#f2f2f2">
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <table border="0" cellpadding="3" cellspacing="0" style="width: 100%;">
                                        <tr>
                                            <td width="40%">
                                                <asp:Label ID="Label32" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_CurSymbol %>"></asp:Label>
                                            </td>
                                            <td width="60%">
                                                <dx:ASPxTextBox ID="txt_FmtCurrency" runat="server" Width="100%" 
                                                    BackColor="#f2f2f2" AutoPostBack="True" 
                                                    OnTextChanged="txt_FmtCurrency_TextChanged">
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="40%">
                                                <asp:Label ID="Label36" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_DecSymbol %>"></asp:Label>
                                            </td>
                                            <td width="60%">
                                                <dx:ASPxTextBox ID="txt_FmtCurrencyDec" runat="server" Width="100%" 
                                                    BackColor="#f2f2f2" AutoPostBack="True" 
                                                    OnTextChanged="txt_FmtCurrencyDec_TextChanged">
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="40%">
                                                <asp:Label ID="Label33" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_DigitDecNo %>"></asp:Label>
                                            </td>
                                            <td width="60%">
                                                <dx:ASPxComboBox ID="txt_FmtCurrencyDecNo" runat="server" ValueType="System.String"
                                                    Width="100%" BackColor="#f2f2f2" AutoPostBack="True" 
                                                    OnValueChanged="txt_FmtCurrencyDecNo_ValueChanged">
                                                    <Items>
                                                        <dx:ListEditItem Text="0" Value="0" />
                                                        <dx:ListEditItem Text="1" Value="1" />
                                                        <dx:ListEditItem Text="2" Value="2" />
                                                        <dx:ListEditItem Text="3" Value="3" />
                                                        <dx:ListEditItem Text="4" Value="4" />
                                                        <dx:ListEditItem Text="5" Value="5" />
                                                        <dx:ListEditItem Text="6" Value="6" />
                                                        <dx:ListEditItem Text="7" Value="7" />
                                                        <dx:ListEditItem Text="8" Value="8" />
                                                        <dx:ListEditItem Text="9" Value="9" />
                                                    </Items>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="40%">
                                                <asp:Label ID="Label34" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_GrpSymbol %>"></asp:Label>
                                            </td>
                                            <td width="60%">
                                                <dx:ASPxTextBox ID="txt_FmtCurrencyDecgrp" runat="server" Width="100%" 
                                                    BackColor="#f2f2f2" AutoPostBack="True" 
                                                    OnTextChanged="txt_FmtCurrencyDecgrp_TextChanged">
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="<%$ Resources:Option_Admin_Bu_Bu, tp_DateTime %>" TabStyle-BackColor="#909090" ActiveTabStyle-BackColor="#F2F2F2"
                            TabStyle-ForeColor="White" ActiveTabStyle-ForeColor="Black">
                            <ActiveTabStyle BackColor="#F2F2F2" ForeColor="Black">
                            </ActiveTabStyle>
                            <TabStyle BackColor="#909090" ForeColor="White">
                            </TabStyle>
                            <ContentCollection>
                                <dx:ContentControl runat="server" Width="100%">
                                    <fieldset>
                                        <legend>
                                            <asp:Label ID="lbl_Date" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_Date %>"></asp:Label></legend>
                                        <table border="0" cellpadding="3" cellspacing="0" style="width: 100%;">
                                            <tr>
                                                <td width="30%">
                                                    <asp:Label ID="lbl_ShortDate" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_ShortDate %>"></asp:Label>
                                                </td>
                                                <td width="70%">
                                                    <dx:ASPxTextBox ID="txt_FmtSDate" runat="server" Width="100%" BackColor="#f2f2f2">
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="30%">
                                                    <asp:Label ID="lbl_LongDate" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_LongDate %>"></asp:Label>
                                                </td>
                                                <td width="70%">
                                                    <dx:ASPxTextBox ID="txt_FmtLDate" runat="server" Width="100%" BackColor="#f2f2f2">
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <fieldset>
                                        <legend>
                                            <asp:Label ID="lbl_PopTime" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_PopTime %>"></asp:Label></legend></legend>
                                        <table border="0" cellpadding="3" cellspacing="0" style="width: 100%;">
                                            <tr>
                                                <td width="30%">
                                                    <asp:Label ID="Label40" runat="server" Text="Time Zone"></asp:Label>
                                                </td>
                                                <td width="70%">
                                                    <dx:ASPxComboBox ID="ddl_UTCCode" Width="100%" BackColor="#f2f2f2" 
                                                        runat="server" TextFormatString="{0} : {1}" OnLoad="ddl_UTCCode_Load">
                                                        <Columns>
                                                            <dx:ListBoxColumn Caption="Code" FieldName="UTCCode" Width="80px" />
                                                            <dx:ListBoxColumn Caption="Name" FieldName="Name" />
                                                        </Columns>
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="30%">
                                                    <asp:Label ID="lbl_TimeFmt" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, lbl_TimeFmt %>"></asp:Label>
                                                </td>
                                                <td width="70%">
                                                    <dx:ASPxTextBox ID="txt_FmtSTime" runat="server" Width="100%" BackColor="#f2f2f2">
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                    </TabPages>
                </dx:ASPxPageControl>
                <table border="0" cellpadding="1" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td align="right">
                            <table border="0" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td>
                                        <dx:ASPxButton ID="btn_PopOK" runat="server" OnClick="btn_PopOK_Click" Text="<%$ Resources:Option_Admin_Bu_Bu, btn_PopOK %>"
                                            Width="70px">
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btn_PopCancel" runat="server" OnClick="btn_PopCancel_Click" Text="<%$ Resources:Option_Admin_Bu_Bu, btn_Cancel %>"
                                            Width="70px" BackColor="#f2f2f2">
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_Warning" ClientInstanceName="pop_Warning" runat="server"
        Width="300px" CloseAction="CloseButton" HeaderText="<%$ Resources:Option_Admin_Bu_Bu, pop_Warning %>"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        ShowCloseButton="False">
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" height="50px">
                            <asp:Label ID="lbl_Warning" runat="server" SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btn_Warning" runat="server" Text="<%$ Resources:Option_Admin_Bu_Bu, btn_PopOK %>"
                                SkinID="BTN_V1" Width="50px" OnClick="btn_Warning_Click" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
