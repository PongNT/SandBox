<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebFormSandBox._Default" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <style type="text/css">
        .modalBackground {
            background-color: Black;
            filter: alpha (opacity•90);
            opacity: 0.8;
            z-index: 1000 !important;
        }

        .modalPopup {
            background-color: #FFFFFF;
            border: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 10px;
            padding-left: 10px;
            width: 300px;
            height: 140px;
        }

        .testAread {
            background: #eeeeee; 
            padding: 10px;
            margin:30px 0px;
        }

    </style>

    <div class="jumbotron">
        <h1>Sandbox</h1>
        <p class="lead">Miscelaneous testing</p>
        <p><a href="http://www.asp.net" class="btn btn-primary btn-lg">ASP.NET Web Forms online doc &raquo;</a></p>
        <p>
            <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkId=301948">Getting started &raquo;</a>
        </p>
    </div>

    <div class="testAread" id="dvLanguageResourcesTests">
        <h2>Language resource tests</h2>
        <div class="row">
            <div class="col-md-4">
                Eval from Markup: 
                <label id="lblTestLanguageFromMarkup" runat="server"><%=Resources.Language.test%></label>
            </div>
            <div class="col-md-4">
                Eval from code: 
                <label id="lblTestLanguageFromCode" runat="server" />
            </div>
        </div>
    </div>

    <div class="testAread" id="dvComboTests">

        <div class="row">
            <div class="col-md-4">
                <h2>Combo test</h2>
            </div>
            <div class="col-md-4">
                <h2>Treeview test</h2>
            </div>
            <div class="col-md-4">
                <h2>RadDropDownTree test</h2>
            </div>
        </div>

        <div class="row">

            <div class="col-md-4">
                <asp:Button runat="server" />
                <asp:Button ID="MyButton" OnClick="MyButton_Click" runat="server" />
                <asp:DropDownList ID="DropDownList1"
                    OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged"
                    AutoPostBack="true" Width="250px" runat="server">
                    <asp:ListItem Selected="True" Value="White"> White </asp:ListItem>
                    <asp:ListItem Value="Silver"> Silver </asp:ListItem>
                    <asp:ListItem Value="DarkGray"> Dark Gray </asp:ListItem>
                    <asp:ListItem Value="Khaki"> Khaki </asp:ListItem>
                    <asp:ListItem Value="DarkKhaki"> Dark Khaki </asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="DropDownList2"
                    OnSelectedIndexChanged="DropDownList2_SelectedIndexChanged" AutoPostBack="true" Width="250px" runat="server">
                    <asp:ListItem Selected="True" Value="White"> White </asp:ListItem>
                    <asp:ListItem Value="Silver"> Silver </asp:ListItem>
                    <asp:ListItem Value="DarkGray"> Dark Gray </asp:ListItem>
                    <asp:ListItem Value="Khaki"> Khaki </asp:ListItem>
                    <asp:ListItem Value="DarkKhaki"> Dark Khaki </asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="col-md-4">
                <div class="treeViewWrapper">
                    <telerik:RadTreeView RenderMode="Lightweight" runat="Server" ID="RadTreeView1" CheckBoxes="true" OnNodeCheck="RadTreeView1_NodeCheck">
                        <Nodes>
                            <telerik:RadTreeNode runat="server" Text="Island" Expanded="true" AllowDrag="false"
                                AllowDrop="false">
                                <Nodes>
                                    <telerik:RadTreeNode runat="server" Text="Zanzibar" AllowDrag="false">
                                        <Nodes>
                                            <telerik:RadTreeNode runat="server" Text="Weekend Package" AllowDrop="false" Value="1999">
                                            </telerik:RadTreeNode>
                                            <telerik:RadTreeNode runat="server" Text="1 Week Package" AllowDrop="false" Value="2999">
                                            </telerik:RadTreeNode>
                                            <telerik:RadTreeNode runat="server" Text="2 Week Package" AllowDrop="false" Value="3999">
                                            </telerik:RadTreeNode>
                                        </Nodes>
                                    </telerik:RadTreeNode>
                                    <telerik:RadTreeNode runat="server" Text="Mauritius" AllowDrag="false">
                                        <Nodes>
                                            <telerik:RadTreeNode runat="server" Text="Weekend Package" AllowDrop="false" Value="2999">
                                            </telerik:RadTreeNode>
                                            <telerik:RadTreeNode runat="server" Text="1 Week Package" AllowDrop="false" Value="3999">
                                            </telerik:RadTreeNode>
                                            <telerik:RadTreeNode runat="server" Text="2 Week Package" AllowDrop="false" Value="4999">
                                            </telerik:RadTreeNode>
                                        </Nodes>
                                    </telerik:RadTreeNode>
                                    <telerik:RadTreeNode runat="server" Text="Maldives" Expanded="true" AllowDrag="false">
                                        <Nodes>
                                            <telerik:RadTreeNode runat="server" Text="Weekend Package" AllowDrop="false" Value="3999">
                                            </telerik:RadTreeNode>
                                            <telerik:RadTreeNode runat="server" Text="1 Week Package" AllowDrop="false" Value="4999">
                                            </telerik:RadTreeNode>
                                            <telerik:RadTreeNode runat="server" Text="2 Week Package" AllowDrop="false" Value="5999">
                                            </telerik:RadTreeNode>
                                        </Nodes>
                                    </telerik:RadTreeNode>
                                </Nodes>
                            </telerik:RadTreeNode>
                        </Nodes>
                    </telerik:RadTreeView>
                </div>
            </div>
            <div class="col-md-4">
                <div class="treeViewWrapper">
                    <telerik:RadDropDownTree RenderMode="Lightweight" ID="DDTV_Test" runat="server" AutoPostBack="true" Width="300px"
                        CheckBoxes="None"
                        OnEntryAdded="RadDropDownTree1_EntryAdded" OnEntryRemoved="RadDropDownTree1_EntryRemoved"
                        DefaultMessage="Please select">
                        <DropDownSettings OpenDropDownOnLoad="false" CloseDropDownOnSelection="true" />

                    </telerik:RadDropDownTree>
                </div>
            </div>

        </div>

        <asp:Panel ID="Pnl_Popup" runat="server" CssClass="modalPopup">

            <telerik:RadDropDownTree RenderMode="Lightweight" ID="DDTV_InPopup" runat="server" AutoPostBack="true" Width="300px"
                CheckBoxes="None"
                OnEntryAdded="RadDropDownTree1_EntryAdded" OnEntryRemoved="RadDropDownTree1_EntryRemoved"
                DefaultMessage="Please select">
                <DropDownSettings OpenDropDownOnLoad="false" CloseDropDownOnSelection="true" />

            </telerik:RadDropDownTree>

            <asp:Button ID="Btn_OK" runat="server" Text="OK" />

        </asp:Panel>

    </div>

    <asp:Button ID="Btn_OpenDlg" runat="server" Text="Open Dialog" />

    <ajaxToolkit:ModalPopupExtender ID="MPE_Test" runat="server"
        BackgroundCssClass="modalBackground" OkControlID="Btn_OK" PopupControlID="Pnl_Popup" TargetControlID="Btn_OpenDlg">
    </ajaxToolkit:ModalPopupExtender>
</asp:Content>
