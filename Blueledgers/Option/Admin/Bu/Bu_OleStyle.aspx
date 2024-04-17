<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="Bu_OleStyle.aspx.cs" Inherits="BlueLedger.PL.Option.Admin.Bu.Bu" %>
    
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
    <table border="0" cellpadding="2" cellspacing="0" width="100%">
        <tr>
            <td align="left">
                <!-- Title -->
                <table border="0" cellpadding="3" cellspacing="0" width="100%">
                    <tr style="height: 35px">
                        <td style="background-image: url(<%= Page.ResolveUrl("~")%>/App_Themes/Default/Images/master/pc/blue/bg_title.png)">
                            <asp:Label ID="lbl_Title" runat="server" Text="Company Information" Font-Size="13pt"
                                Font-Bold="True" ForeColor="White"></asp:Label>
                        </td>
                        <td align="right" style="background-image: url(<%= Page.ResolveUrl("~")%>/App_Themes/Default/Images/master/pc/blue/bg_title.png)">
                            <table border="0" cellpadding="1" cellspacing="0">
                                <tr>
                                    <td>
                                        <dx:ASPxButton ID="btn_Save" runat="server" Text="Save" Width="75px" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                            CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                                            <Image Url="~/App_Themes/Default/Images/save.gif">
                                            </Image>
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btn_Cancel" runat="server" Text="Cancel" Width="75px" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                            CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                                            <Image Url="~/App_Themes/Default/Images/cancel.gif">
                                            </Image>
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <br />
                <dx:ASPxNavBar ID="ASPxNavBar" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                    CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="100%">
                    <LoadingPanelImage Url="~/App_Themes/Aqua/Web/nbLoading.gif">
                    </LoadingPanelImage>
                    <Groups>
                        <%-- General Information --%>
                        <%-- Address --%>
                        <%-- Delivery Terms --%>
                        <%-- Regional and Language --%>
                        <dx:NavBarGroup Text="General Information" Expanded="False">
                            <ContentTemplate>
                                <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                    <tr>
                                        <td width="4%">
                                            &nbsp;
                                        </td>
                                        <td width="10%">
                                            <asp:Label ID="Label1" runat="server" Text="Code" Font-Bold="True"></asp:Label>
                                        </td>
                                        <td width="22%">
                                            <dx:ASPxTextBox ID="txt_BuCode" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                CssPostfix="Aqua" Enabled="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                Width="150px">
                                                <ValidationSettings>
                                                    <ErrorFrameStyle ImageSpacing="4px">
                                                        <ErrorTextPaddings PaddingLeft="4px" />
                                                    </ErrorFrameStyle>
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td width="10%">
                                            <asp:Label ID="Label2" runat="server" Text="Name" Font-Bold="True"></asp:Label>
                                        </td>
                                        <td width="22%">
                                            <dx:ASPxTextBox ID="txt_BuName" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="200px">
                                                <ValidationSettings>
                                                    <ErrorFrameStyle ImageSpacing="4px">
                                                        <ErrorTextPaddings PaddingLeft="4px" />
                                                    </ErrorFrameStyle>
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td width="10%">
                                            <asp:Label ID="Label3" runat="server" Text="Billing Name" Font-Bold="True"></asp:Label>
                                        </td>
                                        <td width="22%">
                                            <dx:ASPxTextBox ID="txt_BuNameBilling" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="200px">
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
                        <dx:NavBarGroup Text="Address">
                            <ContentTemplate>
                                <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                    <tr>
                                        <td width="4%">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Text="Street" Font-Bold="True"></asp:Label>
                                        </td>
                                        <td colspan="5">
                                            <dx:ASPxTextBox ID="txt_Address" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="585px">
                                                <ValidationSettings>
                                                    <ErrorFrameStyle ImageSpacing="4px">
                                                        <ErrorTextPaddings PaddingLeft="4px" />
                                                    </ErrorFrameStyle>
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:Label ID="Label5" runat="server" Font-Bold="True" Text="Post Code"></asp:Label>
                                        </td>
                                        <td>
                                            <dx:ASPxTextBox ID="txt_PostCode" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="150px">
                                                <ValidationSettings>
                                                    <ErrorFrameStyle ImageSpacing="4px">
                                                        <ErrorTextPaddings PaddingLeft="4px" />
                                                    </ErrorFrameStyle>
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label6" runat="server" Font-Bold="True" Text="City"></asp:Label>
                                        </td>
                                        <td>
                                            <dx:ASPxComboBox ID="ddl_City" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                CssPostfix="Aqua" LoadingPanelImagePosition="Top" ShowShadow="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                ValueType="System.String" Width="150px" AutoPostBack="True">
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
                                        <td>
                                            <asp:Label ID="Label7" runat="server" Font-Bold="True" Text="Country"></asp:Label>
                                        </td>
                                        <td>
                                            <dx:ASPxComboBox ID="ddl_Country" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                CssPostfix="Aqua" LoadingPanelImagePosition="Top" ShowShadow="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                ValueType="System.String" Width="150px" OnSelectedIndexChanged="ddl_Country_SelectedIndexChanged"
                                                AutoPostBack="True">
                                                <Columns>
                                                    <dx:ListBoxColumn Caption="Code" FieldName="Code" Width="80px" />
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
                                            &nbsp;
                                        </td>
                                        <td width="10%">
                                            <asp:Label ID="Label8" runat="server" Text="Phone" Font-Bold="True"></asp:Label>
                                        </td>
                                        <td width="22%">
                                            <dx:ASPxTextBox ID="txt_Phone" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="150px">
                                                <ValidationSettings>
                                                    <ErrorFrameStyle ImageSpacing="4px">
                                                        <ErrorTextPaddings PaddingLeft="4px" />
                                                    </ErrorFrameStyle>
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td width="10%">
                                            <asp:Label ID="Label9" runat="server" Text="Fax" Font-Bold="True"></asp:Label>
                                        </td>
                                        <td width="22%">
                                            <dx:ASPxTextBox ID="txt_Fax" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="150px">
                                                <ValidationSettings>
                                                    <ErrorFrameStyle ImageSpacing="4px">
                                                        <ErrorTextPaddings PaddingLeft="4px" />
                                                    </ErrorFrameStyle>
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td width="10%">
                                            <asp:Label ID="Label10" runat="server" Font-Bold="True" Text="Email"></asp:Label>
                                        </td>
                                        <td width="22%">
                                            <dx:ASPxTextBox ID="txt_Email" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="150px">
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
                        <dx:NavBarGroup Text="Delivery Terms">
                            <ContentTemplate>
                                <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                    <tr>
                                        <td width="4%">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <dx:ASPxMemo ID="txt_DeliveryTerm" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                CssPostfix="Aqua" Height="150px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                Width="100%">
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
                        <dx:NavBarGroup Text="Regional and Language">
                            <ContentTemplate>
                                <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                    CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" TabSpacing="3px"
                                    Width="100%">
                                    <LoadingPanelImage Url="~/App_Themes/Aqua/Web/Loading.gif">
                                    </LoadingPanelImage>
                                    <ContentStyle>
                                        <Border BorderColor="#AECAF0" BorderStyle="Solid" BorderWidth="1px" />
                                    </ContentStyle>
                                    <TabPages>
                                        <dx:TabPage Text="Regional Options">
                                            <ContentCollection>
                                                <dx:ContentControl runat="server">
                                                    <table border="0" cellpadding="3" cellspacing="0" width="50%">
                                                        <tr>
                                                            <td>
                                                                <fieldset>
                                                                    <legend>
                                                                        <asp:Label ID="Label11" runat="server" Text="Standards and formats"></asp:Label></legend>
                                                                    <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="Label12" runat="server" Text="This option affects how some programs format numbers, currencies, dates and time.<br><br>Select an item to match its preferences, or click Customize to choose your own formats:"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <table border="0" cellpadding="3" cellspacing="0">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <dx:ASPxComboBox ID="ddl_FmtCode" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                                                                CssPostfix="Aqua" LoadingPanelImagePosition="Top" ShowShadow="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                                                                Width="400px">
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
                                                                                            <dx:ASPxButton ID="bnt_Custz" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                                                                CssPostfix="Aqua" OnClick="bnt_Custz_Click" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                                                                Text="Customize...">
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
                                                                                        <td colspan="2">
                                                                                            <asp:Label ID="Label13" runat="server" Text="Samples"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td width="20%">
                                                                                            <asp:Label ID="Label14" runat="server" Text="Number:"></asp:Label>
                                                                                        </td>
                                                                                        <td width="80%">
                                                                                            <dx:ASPxTextBox ID="txt_Number" runat="server" Width="385px" Enabled="false" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                                                                CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                                                                                                <ValidationSettings>
                                                                                                    <ErrorFrameStyle ImageSpacing="4px">
                                                                                                        <ErrorTextPaddings PaddingLeft="4px" />
                                                                                                    </ErrorFrameStyle>
                                                                                                </ValidationSettings>
                                                                                            </dx:ASPxTextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="Label15" runat="server" Text="Currency:"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <dx:ASPxTextBox ID="txt_Currency" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                                                                CssPostfix="Aqua" Enabled="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                                                                Width="385px">
                                                                                                <ValidationSettings>
                                                                                                    <ErrorFrameStyle ImageSpacing="4px">
                                                                                                        <ErrorTextPaddings PaddingLeft="4px" />
                                                                                                    </ErrorFrameStyle>
                                                                                                </ValidationSettings>
                                                                                            </dx:ASPxTextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="Label16" runat="server" Text="Time:"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <dx:ASPxTextBox ID="txt_Time" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                                                                CssPostfix="Aqua" Enabled="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                                                                Width="385px">
                                                                                                <ValidationSettings>
                                                                                                    <ErrorFrameStyle ImageSpacing="4px">
                                                                                                        <ErrorTextPaddings PaddingLeft="4px" />
                                                                                                    </ErrorFrameStyle>
                                                                                                </ValidationSettings>
                                                                                            </dx:ASPxTextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="Label17" runat="server" Text="Short date:"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <dx:ASPxTextBox ID="txt_SDate" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                                                                CssPostfix="Aqua" Enabled="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                                                                Width="385px">
                                                                                                <ValidationSettings>
                                                                                                    <ErrorFrameStyle ImageSpacing="4px">
                                                                                                        <ErrorTextPaddings PaddingLeft="4px" />
                                                                                                    </ErrorFrameStyle>
                                                                                                </ValidationSettings>
                                                                                            </dx:ASPxTextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="Label18" runat="server" Text="Long date:"></asp:Label>
                                                                                        </td>
                                                                                        <td>
                                                                                            <dx:ASPxTextBox ID="txt_LDate" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                                                                CssPostfix="Aqua" Enabled="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                                                                Width="385px">
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
                                        <dx:TabPage Text="Language">
                                            <ContentCollection>
                                                <dx:ContentControl runat="server">
                                                    <table border="0" cellpadding="1" cellspacing="0" width="50%">
                                                        <tr>
                                                            <td>
                                                                <fieldset>
                                                                    <legend>
                                                                        <asp:Label ID="Label19" runat="server" Text="Display Language"></asp:Label></legend>
                                                                    <table border="0" cellpadding="3" cellspacing="0" style="width: 100%;">
                                                                        <tr>
                                                                            <td width="20%">
                                                                                <asp:Label ID="Label20" runat="server" Text="Default"></asp:Label>
                                                                            </td>
                                                                            <td width="80%">
                                                                                <dx:ASPxComboBox ID="ddl_DefaultLang" runat="server" Width="200px" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                                                    CssPostfix="Aqua" LoadingPanelImagePosition="Top" ShowShadow="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
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
                                                                                <asp:Label ID="Label21" runat="server" Text="Optional"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <dx:ASPxComboBox ID="ddl_OptionalLang" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                                                    CssPostfix="Aqua" LoadingPanelImagePosition="Top" ShowShadow="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                                                    Width="200px">
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
                                    <Paddings Padding="2px" PaddingLeft="5px" PaddingRight="5px" />
                                </dx:ASPxPageControl>
                            </ContentTemplate>
                        </dx:NavBarGroup>
                    </Groups>
                </dx:ASPxNavBar>
            </td>
        </tr>
    </table>
    <dx:ASPxPopupControl ID="pop_FormatCustz" runat="server" CloseAction="CloseButton"
        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" HeaderText="Customize Regional Options"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="480px">
        <ContentStyle HorizontalAlign="Left" VerticalAlign="Top">
            <Paddings Padding="3px" />
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxPageControl ID="ASPxPageControl2" runat="server" ActiveTabIndex="1" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                    CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" TabSpacing="3px"
                    Width="100%">
                    <LoadingPanelImage Url="~/App_Themes/Aqua/Web/Loading.gif">
                    </LoadingPanelImage>
                    <TabPages>
                        <dx:TabPage Text="Numbers">
                            <ContentCollection>
                                <dx:ContentControl runat="server">
                                    <fieldset>
                                        <legend>
                                            <asp:Label ID="Label22" runat="server" Text="Sample"></asp:Label></legend>
                                        <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label23" runat="server" Text="Positive:"></asp:Label>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txt_NumPos" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                        CssPostfix="Aqua" Enabled="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                        Width="150px">
                                                        <ValidationSettings>
                                                            <ErrorFrameStyle ImageSpacing="4px">
                                                                <ErrorTextPaddings PaddingLeft="4px" />
                                                            </ErrorFrameStyle>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label24" runat="server" Text="Negative:"></asp:Label>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txt_NumNeg" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                        CssPostfix="Aqua" Enabled="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                        Width="150px">
                                                        <ValidationSettings>
                                                            <ErrorFrameStyle ImageSpacing="4px">
                                                                <ErrorTextPaddings PaddingLeft="4px" />
                                                            </ErrorFrameStyle>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <table border="0" cellpadding="3" cellspacing="0" style="width: 100%;">
                                        <tr>
                                            <td width="40%">
                                                <asp:Label ID="Label25" runat="server" Text="Decimal symbol:"></asp:Label>
                                            </td>
                                            <td width="60%">
                                                <dx:ASPxTextBox ID="txt_FmtNumDec" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                    CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="250px">
                                                    <ValidationSettings>
                                                        <ErrorFrameStyle ImageSpacing="4px">
                                                            <ErrorTextPaddings PaddingLeft="4px" />
                                                        </ErrorFrameStyle>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label26" runat="server" Text="No. of digits after decimal:"></asp:Label>
                                            </td>
                                            <td>
                                                <dx:ASPxComboBox ID="ddl_FmtNumDecNo" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                    CssPostfix="Aqua" LoadingPanelImagePosition="Top" ShowShadow="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                    ValueType="System.String" Width="250px">
                                                    <LoadingPanelImage Url="~/App_Themes/Aqua/Editors/Loading.gif">
                                                    </LoadingPanelImage>
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
                                                <asp:Label ID="Label27" runat="server" Text="Digit grouping symbol:"></asp:Label>
                                            </td>
                                            <td>
                                                <dx:ASPxTextBox ID="txt_FmtNumDecGrp" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                    CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="250px">
                                                    <ValidationSettings>
                                                        <ErrorFrameStyle ImageSpacing="4px">
                                                            <ErrorTextPaddings PaddingLeft="4px" />
                                                        </ErrorFrameStyle>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label29" runat="server" Text="Negitive sign symbol:"></asp:Label>
                                            </td>
                                            <td>
                                                <dx:ASPxTextBox ID="txt_FmtNumNeg" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                    CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="250px">
                                                    <ValidationSettings>
                                                        <ErrorFrameStyle ImageSpacing="4px">
                                                            <ErrorTextPaddings PaddingLeft="4px" />
                                                        </ErrorFrameStyle>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Currency">
                            <ContentCollection>
                                <dx:ContentControl runat="server">
                                    <fieldset>
                                        <legend>
                                            <asp:Label ID="Label28" runat="server" Text="Sample"></asp:Label></legend>
                                        <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label30" runat="server" Text="Positive:"></asp:Label>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txt_CurPos" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                        CssPostfix="Aqua" Enabled="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                        Width="150px">
                                                        <ValidationSettings>
                                                            <ErrorFrameStyle ImageSpacing="4px">
                                                                <ErrorTextPaddings PaddingLeft="4px" />
                                                            </ErrorFrameStyle>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label31" runat="server" Text="Negative:"></asp:Label>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txt_CurNeg" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                        CssPostfix="Aqua" Enabled="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                        Width="150px">
                                                        <ValidationSettings>
                                                            <ErrorFrameStyle ImageSpacing="4px">
                                                                <ErrorTextPaddings PaddingLeft="4px" />
                                                            </ErrorFrameStyle>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <table border="0" cellpadding="3" cellspacing="0" style="width: 100%;">
                                        <tr>
                                            <td width="40%">
                                                <asp:Label ID="Label32" runat="server" Text="Currency symbol:"></asp:Label>
                                            </td>
                                            <td width="60%">
                                                <dx:ASPxTextBox ID="txt_FmtCurrency" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                    CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="250px">
                                                    <ValidationSettings>
                                                        <ErrorFrameStyle ImageSpacing="4px">
                                                            <ErrorTextPaddings PaddingLeft="4px" />
                                                        </ErrorFrameStyle>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label36" runat="server" Text="Decimal symbol:"></asp:Label>
                                            </td>
                                            <td>
                                                <dx:ASPxTextBox ID="txt_FmtCurrencyDec" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                    CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="250px">
                                                    <ValidationSettings>
                                                        <ErrorFrameStyle ImageSpacing="4px">
                                                            <ErrorTextPaddings PaddingLeft="4px" />
                                                        </ErrorFrameStyle>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label33" runat="server" Text="No. of digits after decimal:"></asp:Label>
                                            </td>
                                            <td>
                                                <dx:ASPxComboBox ID="txt_FmtCurrencyDecNo" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                    CssPostfix="Aqua" LoadingPanelImagePosition="Top" ShowShadow="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                    ValueType="System.String" Width="250px">
                                                    <LoadingPanelImage Url="~/App_Themes/Aqua/Editors/Loading.gif">
                                                    </LoadingPanelImage>
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
                                                <asp:Label ID="Label34" runat="server" Text="Digit grouping symbol:"></asp:Label>
                                            </td>
                                            <td>
                                                <dx:ASPxTextBox ID="txt_FmtCurrencyDecgrp" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                    CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="250px">
                                                    <ValidationSettings>
                                                        <ErrorFrameStyle ImageSpacing="4px">
                                                            <ErrorTextPaddings PaddingLeft="4px" />
                                                        </ErrorFrameStyle>
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Date and Time">
                            <ContentCollection>
                                <dx:ContentControl runat="server">
                                    <fieldset>
                                        <legend>
                                            <asp:Label ID="Label38" runat="server" Text="Date"></asp:Label></legend>
                                        <table border="0" cellpadding="3" cellspacing="0" style="width: 100%;">
                                            <tr>
                                                <td width="40%">
                                                    <asp:Label ID="Label35" runat="server" Text="Short Date"></asp:Label>
                                                </td>
                                                <td width="60%">
                                                    <dx:ASPxTextBox ID="txt_FmtSDate" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                        CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="200px">
                                                        <ValidationSettings>
                                                            <ErrorFrameStyle ImageSpacing="4px">
                                                                <ErrorTextPaddings PaddingLeft="4px" />
                                                            </ErrorFrameStyle>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label41" runat="server" Text="Long Date"></asp:Label>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txt_FmtLDate" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                        CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="200px">
                                                        <ValidationSettings>
                                                            <ErrorFrameStyle ImageSpacing="4px">
                                                                <ErrorTextPaddings PaddingLeft="4px" />
                                                            </ErrorFrameStyle>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <fieldset>
                                        <legend>
                                            <asp:Label ID="Label39" runat="server" Text="Time"></asp:Label></legend></legend>
                                        <table border="0" cellpadding="3" cellspacing="0" style="width: 100%;">
                                            <tr>
                                                <td width="40%">
                                                    <asp:Label ID="Label40" runat="server" Text="Time Zone"></asp:Label>
                                                </td>
                                                <td width="60%">
                                                    <dx:ASPxDropDownEdit ID="ddl_UTCCode" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                        CssPostfix="Aqua" ShowShadow="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                                        Width="250px">
                                                        <DropDownButton>
                                                            <Image>
                                                                <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
                                                            </Image>
                                                        </DropDownButton>
                                                        <ButtonEditEllipsisImage Height="3px" Url="~/App_Themes/Aqua/Editors/edtEllipsis.png"
                                                            UrlDisabled="~/App_Themes/Aqua/Editors/edtEllipsisDisabled.png" UrlHottracked="~/App_Themes/Aqua/Editors/edtEllipsisHottracked.png"
                                                            UrlPressed="~/App_Themes/Aqua/Editors/edtEllipsisPressed.png">
                                                        </ButtonEditEllipsisImage>
                                                        <ValidationSettings>
                                                            <ErrorFrameStyle ImageSpacing="4px">
                                                                <ErrorTextPaddings PaddingLeft="4px" />
                                                            </ErrorFrameStyle>
                                                        </ValidationSettings>
                                                    </dx:ASPxDropDownEdit>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label37" runat="server" Text="Time Format"></asp:Label>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txt_FmtSTime" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                                        CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" Width="200px">
                                                        <ValidationSettings>
                                                            <ErrorFrameStyle ImageSpacing="4px">
                                                                <ErrorTextPaddings PaddingLeft="4px" />
                                                            </ErrorFrameStyle>
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                    </TabPages>
                    <Paddings Padding="2px" PaddingLeft="5px" PaddingRight="5px" />
                    <ContentStyle>
                        <Border BorderColor="#AECAF0" BorderStyle="Solid" BorderWidth="1px" />
                    </ContentStyle>
                </dx:ASPxPageControl>
                <table border="0" cellpadding="1" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td align="right">
                            <table border="0" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td>
                                        <dx:ASPxButton ID="btn_PopOK" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                            CssPostfix="Aqua" OnClick="btn_PopOK_Click" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                            Text="OK" Width="70px">
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btn_PopCancel" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                            CssPostfix="Aqua" OnClick="btn_PopCancel_Click" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                                            Text="Cancel" Width="70px">
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle HorizontalAlign="Left" />
    </dx:ASPxPopupControl>
</asp:Content>
