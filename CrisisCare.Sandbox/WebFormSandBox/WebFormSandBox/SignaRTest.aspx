<%@ Page Title="POC SignalR" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SignaRTest.aspx.cs" Inherits="WebFormSandBox.SignaRTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>

<script src="Scripts/jquery-3.4.1.min.js"></script>
<script src="Scripts/jquery.signalR-2.4.1.min.js"></script>
<script src="http://www.codeproject.com/signalr/hubs" type="text/javascript"></script>
<script type="text/javascript">

    $(function () {

        var logger = $.connection.logHub;

        logger.client.logMessage = function (msg) {

            $("#logUl").append("<li>" + msg + "</li>");
        };

        $.connection.hub.start();
    });

</script>
<body>

    <form id="form1" runat="server">
        <div>
            <h3>Log Items</h3>
            <asp:ListView ID="logListView" runat="server"
                ItemPlaceholderID="itemPlaceHolder"
                ClientIDMode="Static" EnableViewState="false">
                <LayoutTemplate>
                    <ul id="logUl">
                        <li runat="server"
                            id="itemPlaceHolder"></li>
                    </ul>
                </LayoutTemplate>
                <ItemTemplate>
                    <li><span class="logItem">
                        <%#Container.DataItem.ToString() %></span></li>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </form>
</body>
</html>
