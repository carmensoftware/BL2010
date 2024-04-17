<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master/In/SkinDefault.master" CodeFile="Reports.aspx.cs" Inherits="BlueLedger.PL.Report.Reports" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxRoundPanel"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Header" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" />
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.2/themes/base/jquery-ui.css">
    <style>
        .flex-container
        {
            display: flex !important;
        }
        
        .flex-container > div
        {
            margin: 5px;
            padding: 5px;
        }
        
        .flex-direction-column
        {
            flex-direction: column !important;
        }
        
        .width-100
        {
            width: 100% !important;
        }
        
        .width-75
        {
            width: 75% !important;
        }
        
        .width-50
        {
            width: 50% !important;
        }
        
        .width-25
        {
            width: 25% !important;
        }
        
        .margin-bottom
        {
            margin-bottom: 5px;
        }
    </style>
    <style>
        #txt_Search
        {
            background-image: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABUAAAAVCAYAAACpF6WWAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAEnQAABJ0Ad5mH3gAAAACYktHRAD/h4/MvwAAAAl2cEFnAAABKgAAASkAUBZlMQAAACV0RVh0ZGF0ZTpjcmVhdGUAMjAxMy0wNC0xMFQwNjo1OTowNy0wNzowMI5BiVEAAAAldEVYdGRhdGU6bW9kaWZ5ADIwMTMtMDQtMTBUMDY6NTk6MDctMDc6MDD/HDHtAAAAGXRFWHRTb2Z0d2FyZQB3d3cuaW5rc2NhcGUub3Jnm+48GgAAABF0RVh0VGl0bGUAc2VhcmNoLWljb27Cg+x9AAACKklEQVQ4T6WUSavqQBCFK+2sII7gShFXLpUsBBHFf+1KcAQFwaWiolsnnBDn++4p0iHRqPDuByFJd/Wp6qrqVn5+IQP3+52m0ymtVis6Ho885na7KRgMUiKR4O9vmEQHgwGNx2NyOp0khCBFUXgcJo/Hg67XK8ViMcpkMjz+Dl200+nQZrMhh8PBE4gYQgDidrudvzEOm2KxyP9WsCginM1mHKEUS6VSFA6HOWI4G41GPAfx2+1GgUCAVFXVZMwovwY/lUqFPB4PiyFn+XxemzbT6/VovV6z8Ol0olwux+LPCBQFEQKIvhME2WyWbWGHFCD/VghUGVvE1rDlb6TTabbFmuVyqY2aEWgbFALeI5GINvyeUCjEtlgju+IZoRWfkS30CURoxFJUNjMEt9stf38CNjJKIFvNiMBJgTebzcZt843hcMhCELWqPBDxeJwulwtvC/3X7/e1qVfgFD0rC5tMJrUZM8Lr9VI0GmVBRDCfz6nZbHI/Sna7HXW7XZpMJtxSiBIP1lmhH9NqtaqfGKQDTmQREBnSgwfmMqfYYblc1o+2xHShtNttLgSiee4EmMEp3hDBPJzikimVSuRyuTTLJ1GwWCz4pCB3UhiL/X4/Hw50C5zjLSM+n898weCogxdRIzAGxigAdtNqtV6EC4UC+Xy+z6Kf2O/31Gg0TMK4ZBDxf4uCw+FA9XpdF0aaUOg/iQLcHbVaTb/p0Cl/FgXIJ/oYnaCqKv0DC6dltH6Ks84AAAAASUVORK5CYII=');
            background-position: 10px 10px;
            background-repeat: no-repeat;
            width: 25% !important;
            font-size: 0.8rem;
            padding: 12px 20px 12px 40px;
            border: 1px solid #ddd;
        }
        
        #btn_View
        {
            display: block;
            width: 80px;
            height: 22px;
            background: #4E9CAF;
            padding: 5px;
            text-align: center;
            border-radius: 5px;
            color: white;
            font-weight: bold;
            line-height: 22px;
            cursor: pointer;
        }
    </style>
</asp:Content>
<asp:Content ID="Body" ContentPlaceHolderID="cph_Main" runat="Server">
    <!--Title Bar-->
    <div style="width: 100%; padding: 3px 0 3px 10px; margin-bottom: 10px; background-color: #4d4d4d;">
        <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
        <asp:Label ID="lbl_Title" runat="server" Text="Report" SkinID="LBL_HD_WHITE"></asp:Label>
    </div>
    <div class="margin-bottom">
        <input type="search" id="txt_Search" style="width: 300px;" placeholder="Search" />
    </div>
    <div class="flex-container width-100">
        <div style="width: 30%;">
            <table id="table_report" class="width-100">
                <tbody>
                    <%foreach (var item in _reports)%>
                    <%{%>
                    <tr>
                        <td>
                            <div class="flex-container flex-direction-column margin-bottom">
                                <a style="color: Black; text-decoration: none; font-weight: bolder; font-size: 0.8rem;" href="?id=<%=item.Id %>">
                                    <%=item.Name %>
                                </a>
                                <small style="font-style: oblique;">
                                    <%=item.Description %></small>
                            </div>
                        </td>
                    </tr>
                    <%}%>
                </tbody>
            </table>
        </div>
        <div style="width: 70%;">
            <div class="margin-bottom">
                <asp:Label runat="server" ID="lbl_ReportName" Font-Size="Large" Font-Bold="true" />
            </div>
            <br />
            <asp:Panel runat="server" ID="panel_Dialog" ScrollBars="Auto">
            </asp:Panel>
            <br />
            <div>
                <%--<a id="btn_View" href="ViewReport.aspx?id=<%=id%>" target="_blank">View</a>--%>
                <a id="btn_View">View</a>
            </div>
        </div>
    </div>
    <!-- Script(s) -->
    <script src="https://code.jquery.com/jquery-3.6.0.js"></script>
    <script src="https://code.jquery.com/ui/1.13.2/jquery-ui.js"></script>
    <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            // Init
            $('.select2').select2();
            //$('.datepicker').datepicker();
            $('.datepicker').datepicker({ 
                dateFormat: 'dd/mm/yy', 
                changeMonth: true, 
                changeYear: true,  
                //showButtonPanel: true
            });

            var urlParams = new URLSearchParams(window.location.search);
            var id = urlParams.get('id');


            // Event(s)
            $('#txt_Search').on('keyup', function () {
                var filter = $(this).val();
                var tr = $('#table_report tbody tr');

                FilterTable(tr, filter);
            });

            $('#btn_View').on('click', function () {
                
                var data = [];

                $.each($('.parameter'), function(index, value){
                    var item = $(this);
                    
                    data.push({ 
                        Name: item.attr('id'),
                        Value: item.val()
                    });
                });
                console.log(data);

                var encode = btoa(JSON.stringify(data));
                var url = `ViewReport.aspx?id=${id}&value=${encode}`;
                
                window.open(url, '_blank');
            });


            // Method(s)
            function FilterTable(tr, filter) {
                for (var i = 0; i < tr.length; i++) {

                    var td = tr[i].getElementsByTagName("td")[0];

                    if (td) {
                        var txtValue = td.textContent || td.innerText;

                        if (txtValue.toUpperCase().indexOf(filter.toUpperCase()) > -1) {
                            tr[i].style.display = "";
                        } else {
                            tr[i].style.display = "none";
                        }
                    }
                }
            }
        });
    </script>
</asp:Content>
