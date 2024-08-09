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
        }
    </style>
    <%--Flex--%>
    <style>
        .flex
        {
            display: flex !important;
        }
        
        .flex-justify-content-start
        {
            justify-content: flex-start;
        }
        .flex-justify-content-end
        {
            justify-content: flex-end;
        }
        .flex-justify-content-center
        {
            justify-content: center;
        }
        .flex-justify-content-between
        {
            justify-content: space-between;
        }
        .flex-row
        {
            flex-flow: row !important;
        }
        .flex-columm
        {
            flex-flow: column !important;
        }
        
        .mt-10
        {
            margin-top: 10px;
        }
        .mt-20
        {
            margin-top: 20px;
        }
        .mt-30
        {
            margin-top: 30px;
        }
        .mb-10
        {
            margin-bottom: 10px;
        }
        .mb-20
        {
            margin-bottom: 20px;
        }
        .mb-30
        {
            margin-bottom: 30px;
        }
    </style>
    <%--Dialog Controls--%>
    <style>
        .dialog-div
        {
            margin-bottom: 10px;
            display: flex;
            flex-direction: column !important;
        }
        .dialog-label
        {
            font-size: 0.8rem;
            font-weight: bold;
            margin-top: 10px;
            margin-bottom: 5px;
        }
        
        .dialog-select
        {
            width: 100%;
            font-size: 1rem;
        }
    </style>
    <%--Dialog Panel--%>
    <style>
        .accordion
        {
            width: 100%;
            padding: 10px 0;
            margin-bottom: 0;
            background-color: #04AA6D; /* Green */
            border: none;
            color: white;
            text-align: center;
            text-decoration: none;
            display: inline-block;
            font-size: 1rem;
            cursor: pointer;
        }
        
        .active, .accordion:hover
        {
            background-color: #ccc;
        }
        
        .panel
        {
            overflow: hidden;
            max-height: 0;
            transition: max-height 0.2s ease-out;
        }
        
        .panel-parameter
        {
            background-color: #F5F5F5;
            width: auto;
            height: auto;
            padding: 0 20px;
            margin-bottom: 10px;
            display: flex !important;
            flex-direction: column !important;
        }
        
        .btn-view
        {
            width: 100px;
            margin: 0 0 20px 20px;
            font-size: 0.8rem;
            padding: 5px;
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
    <div style="text-align: center;">
        <asp:Label runat="server" ID="lbl_Title" Font-Bold="true" Font-Size="X-Large" />
        <br />
        <asp:Label runat="server" ID="lbl_Bu" Font-Size="small" />
    </div>
    <hr />
    <br />
    <div class="flex flex-justify-content-center">
        <asp:Panel runat="server" ID="panel_Dialog" CssClass="" Width="640">
            <button type="button" id='btn_Dialog' class="accordion">
                Dialog
            </button>
            <div id="dialog" class="panel ">
                <asp:Panel runat="server" ID="panel_Parameters" ClientIDMode="Static" CssClass="panel-parameter">
                </asp:Panel>
                <div class="flex flex-justify-content-center">
                    <button type="button" id="btn_View" class="btn-view">
                        View
                    </button>
                </div>
            </div>
        </asp:Panel>
    </div>
    <br />
    <div class="flex flex-justify-content-center">
        <fr:WebReport ID="WebReport1" runat="server" Height="800px" Width="320px" ToolbarStyle="Small" ToolbarIconsStyle="Blue" OnStartReport="WebReport1_StartReport" />
    </div>
    </form>
    <script type="text/javascript" src="https://code.jquery.com/jquery-3.7.1.min.js" integrity="sha256-/JqT3SQfawRcv/BIHPThkBvs0OEvtFFmqPF/lYI/Cxo=" crossorigin="anonymous"></script>
    <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            // Init
            $('.dialog-select').select2();


            const query = window.location.search;
            const urlParams = new URLSearchParams(query);


            if (!urlParams.get('parameters')){
                showDialog();
            }
            
            // Event(s)
            $('#btn_Dialog').on('click', function(){
                $(this).toggleClass('active');
                showDialog();
            });


            $('#btn_View').on('click', function () { 
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
                //console.log(parameters);

                let id = urlParams.get('id');
                let param = btoa(JSON.stringify(parameters));
                let path = window.location.href.split('?')[0]
                location.href = `${path}?id=${id}&parameters=${param}`;

                //console.log(id, param);
            });

            // Method(s)
            function showDialog(){
                let panel = document.getElementById('dialog');

                if (panel.style.maxHeight){
                    panel.style.maxHeight = null;
                }else{
                    let height = panel.scrollHeight + 100;
                    panel.style.maxHeight = height + "px";
                }

            }


        });
    </script>
</body>
</html>
