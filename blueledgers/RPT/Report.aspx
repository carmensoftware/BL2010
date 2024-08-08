<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Report.aspx.cs" Inherits="Report" %>

<%@ Register Assembly="FastReport.Web" Namespace="FastReport.Web" TagPrefix="fr" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="apple-mobile-web-app-status-bar-style" content="black-translucent">
    <meta name="HandheldFriendly" content="true">
    <meta name="MobileOptimized" content="width">
    <title></title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" />
    <style>
        body
        {
            padding: 20px;
            margin-bottom: 40px;
            background-color: #f3f2f4;
        }
        .panel-dialog
        {
            padding-top: 10px;
            padding-bottom: 10px;
            padding-left: 20px;
            padding-right: 20px;
            display: flex !important;
            flex-direction: column !important;
        }
        .dialog-div
        {
            margin-bottom: 10px;
            display: flex;
            flex-direction: column !important;
        }
        .dialog-label
        {
            font-size: 1rem;
            font-weight: bold;
            padding-top: 10px;
            padding-bottom: 5px;
        }
        
        .dialog-select
        {
            width: 100%;
            font-size: 1rem;
        }
        .btn-view
        {
            font-size: 1rem;
            padding: 5px;
            width: 100px;
        }
    </style>
</head>
<body>
    <style type="text/css">
        .nav ul
        {
            width: 200px;
        }
    </style>
    <form id="form1" runat="server">
    <div style="margin-bottom: 14px; text-align: center;">
        <asp:Label runat="server" ID="lbl_Title" Font-Bold="true" Font-Size="X-Large" />
        <br />
        <asp:Label runat="server" ID="lbl_Bu" Font-Size="small" />
    </div>
    <hr />
    <div style="display: flex; align-items: center; flex-direction: column; with: 100%; margin-bottom: 20px;">
        <asp:Panel runat="server" ID="panel_Dialog" ClientIDMode="Static" CssClass="panel-dialog" BackColor="WhiteSmoke" Width="640">
        </asp:Panel>
        <hr />
        <button type="button" id="btn_submit" class="btn-view">
            View
        </button>
    </div>
    <div style="display: flex; justify-content: center; width: 100%;">
        <br />
        <fr:WebReport ID="WebReport1" runat="server" Height="800px" Width="320px" ToolbarStyle="Small" ToolbarIconsStyle="Blue" OnStartReport="WebReport1_StartReport" />
    </div>
    <asp:Label runat="server" ID="lbl_test" />
    </form>
    <script type="text/javascript" src="https://code.jquery.com/jquery-3.7.1.min.js" integrity="sha256-/JqT3SQfawRcv/BIHPThkBvs0OEvtFFmqPF/lYI/Cxo=" crossorigin="anonymous"></script>
    <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            // Init
            $('.dialog-select').select2();

            const query = window.location.search;
            const urlParams = new URLSearchParams(query);
            
            // Event(s)
            $('#btn_submit').on('click', function () { 
                let parameters = [];

                $('.parameter').each(function(){
                    let item = $(this);
                    let tagName = item.prop('tagName');
                    let name = '';
                    let value = '';


                    switch(tagName){
                        case 'TABLE': // DateEdit
                            name = item.find('input').prop('name');
                            value = item.find('input').val().split('/').reverse().join('-');
                            break;
                        case 'SELECT':
                            name = item.prop('name');
                            value = item.val();
                            break;
                        case 'SPAN': // CheckBox
                            name = item.find('input').prop('name');
                            value = item.find('input').prop('checked');
                            break;
                    }

                    parameters.push({
                        name : name,
                        value: value
                   });
                });


                let id = urlParams.get('id');
                let param = btoa(JSON.stringify(parameters));
                let path = window.location.href.split('?')[0]
                location.href = `${path}?id=${id}&parameters=${param}`;

                //console.log(id, param);
             });

            // Method(s)

        });
    </script>
</body>
</html>
