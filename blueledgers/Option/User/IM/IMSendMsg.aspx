<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IMSendMsg.aspx.cs" Inherits="BlueLedger.PL.IM.IMSendMsg"
    MasterPageFile="~/Master/In/SkinDefault.master" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="cnt_InvoiceList" runat="server" ContentPlaceHolderID="cph_Main">
    <script type="text/javascript" src="../../Scripts/GnxLib.js"></script>
    <script type="text/javascript" language="javascript">

        // Printing for gridview.
        function CallPrint(strid) {
            var printContent = document.getElementById(strid);
            var windowUrl = 'about:blank';
            var uniqueName = new Date();
            var windowName = 'Print' + uniqueName.getTime();
            var printWindow = window.open(windowUrl, windowName, 'left=250,top=250,width=0,height=0');

            printWindow.document.write(printContent.innerHTML);
            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            printWindow.close();
        }

        var textSeparator = ";";
        function OnListBoxSelectionChanged(listBox, args) {
            UpdateText();
        }
        function UpdateText() {
            var selectedItems = checkListBox.GetSelectedItems();
            checkComboBox.SetText(GetSelectedItemsText(selectedItems));
        }
        function SynchronizeListBoxValues(dropDown, args) {
            checkListBox.UnselectAll();
            var texts = dropDown.GetText().split(textSeparator);
            var values = GetValuesByTexts(texts);
            checkListBox.SelectValues(values);
            //            UpdateSelectAllItemState();
            UpdateText();  // for remove non-existing texts
        }
        function GetSelectedItemsText(items) {
            var texts = [];
            for (i = 0; i < items.length; i++)
                if (items[i].index >= 0)
                    texts.push(items[i].text);
            return texts.join(textSeparator);
        }
        function GetValuesByTexts(texts) {
            var actualValues = [];
            var item;
            for (var i = 0; i < texts.length; i++) {
                item = checkListBox.FindItemByText(texts[i]);
                if (item != null)
                    actualValues.push(item.value);
            }
            return actualValues;
        }
        // Select Loop 2.
        var textSeparator2 = ";";
        function OnListBoxSelectionChanged2(listBox2, args2) {
            UpdateText2();
        }
        function UpdateText2() {
            var selectedItems2 = checkListBox2.GetSelectedItems();
            checkComboBox2.SetText(GetSelectedItemsText2(selectedItems2));
        }
        function SynchronizeListBoxValues2(dropDown, args) {
            checkListBox2.UnselectAll();
            var texts = dropDown.GetText().split(textSeparator2);
            var values = GetValuesByTexts2(texts);
            checkListBox2.SelectValues(values);
            //UpdateSelectAllItemState2();
            UpdateText2();  // for remove non-existing texts
        }
        function GetSelectedItemsText2(items) {
            var texts = [];
            for (var i = 0; i < items.length; i++)
                if (items[i].index >= 0)
                    texts.push(items[i].text);
            return texts.join(textSeparator2);
        }
        function GetValuesByTexts2(texts) {
            var actualValues = [];
            var item;
            for (var i = 0; i < texts.length; i++) {
                item = checkListBox.FindItemByText(texts[i]);
                if (item != null)
                    actualValues.push(item.value);
            }
            return actualValues;
        }
    </script>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr style="background-color: #4d4d4d; height: 17px">
            <td align="left">
                <table border="0" cellpadding="1" cellspacing="0">
                    <tr>
                        <td style="padding-left: 10px">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                        </td>
                        <td>
                            <asp:Label ID="lbl_Title" runat="server" Text="Internal Message" SkinID="LBL_HD_WHITE"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
            <td align="right">
                <dx:ASPxMenu ID="menu_CmdBar" runat="server" AutoPostBack="True" Font-Bold="True"
                    BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle"
                    OnItemClick="menu_CmdBar_ItemClick">
                    <ItemStyle BackColor="Transparent">
                        <HoverStyle BackColor="Transparent">
                            <Border BorderStyle="None" />
                        </HoverStyle>
                        <Paddings Padding="2px" />
                        <Border BorderStyle="None" />
                    </ItemStyle>
                    <Items>
                        <dx:MenuItem Text="" Name="Send">
                            <ItemStyle Height="24px" Width="70px">
                                <HoverStyle>
                                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/button/over/send.png" />
                                </HoverStyle>
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/button/send.png" />
                            </ItemStyle>
                        </dx:MenuItem>
                        <dx:MenuItem Text="" Name="Back">
                            <ItemStyle Height="24px" Width="67px">
                                <HoverStyle>
                                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/button/over/back.png" />
                                </HoverStyle>
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/button/back.png" />
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
        </tr>
    </table>
    <%--<table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr style="height: 40px">
            <td style="border-bottom: solid 5px #187EB8;">
                <asp:Label ID="lbl_Title" runat="server" Text="Internal Message" Font-Size="13pt"
                    Font-Bold="true"></asp:Label>
            </td>
            <td align="right" style="border-bottom: solid 5px #187EB8">
                <dx:ASPxMenu ID="menu_CmdBar" runat="server" SkinID="COMMAND_BAR" AutoPostBack="True"
                    OnItemClick="menu_CmdBar_ItemClick" AutoSeparators="RootOnly" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                    CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css" GutterImageSpacing="7px"
                    ShowPopOutImages="True" Height="28px">
                    <ItemStyle DropDownButtonSpacing="12px" PopOutImageSpacing="18px" VerticalAlign="Middle" />
                    <LoadingPanelImage Url="~/App_Themes/Aqua/Web/Loading.gif">
                    </LoadingPanelImage>
                    <Items>
                        <dx:MenuItem Text="Send">
                            <Image Url="~/App_Themes/Default/Images/IM/sent.gif">
                            </Image>
                        </dx:MenuItem>
                        <dx:MenuItem Text="Cancel">
                            <Image Url="~/App_Themes/Default/Images/back.gif">
                            </Image>
                        </dx:MenuItem>
                        <dx:MenuItem Text="Back">
                            <Image Url="~/App_Themes/Default/Images/back.gif">
                            </Image>
                        </dx:MenuItem>
                    </Items>
                    <RootItemSubMenuOffset FirstItemX="-1" FirstItemY="-1" X="-1" Y="-1" />
                    <SubMenuStyle GutterWidth="0px" />
                </dx:ASPxMenu>
            </td>
        </tr>
    </table>--%>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="height: 17px; width: 100%" align="center">
                <asp:Label ID="lbl_Inbox_Nm" runat="server" SkinID="LBL_HD" Text=": Send Message :"></asp:Label>
            </td>
            <%--<td align="right" style="border-bottom: #e5e5e5 1px dotted">
                                <asp:Button ID="btn_ClearAll" runat="server" Text="Clear All" SkinID="BTN_V1" 
                    onclick="btn_ClearAll_Click" />
            </td>--%>
        </tr>
        <tr>
            <td colspan="2">
                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                    <tr style="height: 22px">
                        <td align="right" style="width: 20%">
                            <asp:Label ID="lbl_From_Nm" runat="server" SkinID="LBL_HD" Text="From:"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_From" runat="server" SkinID="LBL_NR" Width="70%"></asp:Label>
                        </td>
                    </tr>
                    <tr style="height: 22px">
                        <td align="right">
                            <asp:Label ID="lbl_To_Nm" runat="server" SkinID="LBL_HD" Text="To:"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <dx:ASPxDropDownEdit ID="ddl_ChooseEmail" runat="server" ClientInstanceName="checkComboBox"
                                SkinID="CheckComboBox" EnableAnimation="false">
                                <DropDownWindowTemplate>
                                    <dx:ASPxListBox Width="100%" ID="listBox" ClientInstanceName="checkListBox" SelectionMode="CheckColumn"
                                        runat="server" SkinID="CheckComboBoxListBox" DataSourceID="ods_Email" ValueField="LoginName"
                                        TextField="LoginName" ValueType="System.String">
                                        <ClientSideEvents SelectedIndexChanged="OnListBoxSelectionChanged" />
                                    </dx:ASPxListBox>
                                </DropDownWindowTemplate>
                                <ClientSideEvents TextChanged="SynchronizeListBoxValues" DropDown="SynchronizeListBoxValues" />
                            </dx:ASPxDropDownEdit>
                        </td>
                    </tr>
                    <tr style="height: 22px">
                        <td align="right">
                            <asp:Label ID="lbl_CC_Nm" runat="server" SkinID="LBL_HD" Text="CC:"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <dx:ASPxDropDownEdit ID="ddl_ChooseEmail2" runat="server" ClientInstanceName="checkComboBox2"
                                SkinID="CheckComboBox2" EnableAnimation="false">
                                <DropDownWindowTemplate>
                                    <dx:ASPxListBox Width="100%" ID="listBox2" ClientInstanceName="checkListBox2" SelectionMode="CheckColumn"
                                        runat="server" SkinID="CheckComboBoxListBox2" DataSourceID="ods_Email" ValueField="LoginName"
                                        TextField="LoginName" ValueType="System.String">
                                        <ClientSideEvents SelectedIndexChanged="OnListBoxSelectionChanged2" />
                                    </dx:ASPxListBox>
                                </DropDownWindowTemplate>
                                <ClientSideEvents TextChanged="SynchronizeListBoxValues2" DropDown="SynchronizeListBoxValues2" />
                            </dx:ASPxDropDownEdit>
                        </td>
                    </tr>
                    <tr style="height: 22px">
                        <td align="right">
                            <asp:Label ID="lbl_Subject_Nm" runat="server" SkinID="LBL_HD" Text="Subject:"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <%--<asp:TextBox ID="txt_Subject" runat="server" SkinID="TXT_NORMAL" Width="70%"></asp:TextBox>--%>
                            <dx:ASPxTextBox ID="txt_Subject" runat="server" Width="70%">
                            </dx:ASPxTextBox>
                        </td>
                    </tr>
                    <tr style="height: 22px">
                        <td align="right">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:CheckBox ID="chk_Importance" runat="server" SkinID="LBL_HD" Text="Importance" />
                            <asp:CheckBox ID="chk_Request" runat="server" SkinID="LBL_HD" Text="Request" />
                        </td>
                    </tr>
                    <tr style="height: 22px">
                        <td align="right" valign="top">
                            <asp:Label ID="lbl_Msg_Nm" runat="server" SkinID="LBL_HD" Text="Message:"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left" valign="top">
                            <dx:ASPxHtmlEditor ID="html_Msg" runat="server" EnableTheming="True">
                                <Settings AllowHtmlView="False" AllowPreview="False" />
                                <Toolbars>
                                    <dx:CustomToolbar>
                                    </dx:CustomToolbar>
                                </Toolbars>
                            </dx:ASPxHtmlEditor>
                        </td>
                    </tr>
                </table>
                <asp:ObjectDataSource ID="ods_Email" runat="server" SelectMethod="GetList" TypeName="Blue.BL.dbo.User">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hf_BuGrpCode" Name="BuGrpCode" PropertyName="Value"
                            Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:HiddenField ID="hf_BuGrpCode" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
