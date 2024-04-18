<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Platform.aspx.cs" Inherits="SkyLinkCloud.Platform.Platform" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>接入平台管理</title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" />
        <f:Panel runat="server" ID="Panel1" ShowBorder="false" ShowHeader="false" Layout="Fit">
            <Toolbars>
                <f:Toolbar runat="server">
                    <Items>
                        <f:TriggerBox runat="server" ID="txtSearch" EmptyText="搜索..." TriggerIcon="Search" OnTriggerClick="txtSearch_TriggerClick" />
                        <f:ToolbarFill runat="server" />
                        <f:Button runat="server" ID="btnAdd" IconFont="_RoundPlus" Text="新增平台" OnClick="btnAdd_Click" />
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:Grid runat="server" ID="gridPlatform" ShowHeader="false" ShowBorder="false" EnableHeaderMenu="false" OnRowCommand="gridPlatform_RowCommand">
                    <Columns>
                        <f:WindowField ColumnID="myWindowField" Width="50px" TextAlign="Center" WindowID="winEdit" HeaderText="编辑" Icon="Pencil" DataIFrameUrlFields="FID" DataTextFormatString="{0}"
                            DataIFrameUrlFormatString="PlatformEdit.aspx?ID={0}" DataWindowTitleField="FNameShort" DataWindowTitleFormatString="接入平台管理 - {0}" />
                        <f:LinkButtonField Width="50px" HeaderText="删除" TextAlign="Center" ConfirmText="确定要删除此平台吗？" ConfirmIcon="Question" ConfirmTarget="Self" CommandName="Delete" Icon="Delete" />
                        <f:BoundField ColumnID="FID" DataField="FID" HeaderText="ID" Hidden="true" />
                        <f:BoundField ColumnID="FName" DataField="FName" HeaderText="平台名称" Width="250px" />
                        <f:BoundField ColumnID="FNameShort" DataField="FNameShort" HeaderText="平台简称" Width="150px" />
                        <f:BoundField ColumnID="FType" DataField="FType" HeaderText="接入方式" TextAlign="Center" />
                        <f:CheckBoxField DataField="FEnabled" HeaderText="可用" Width="50px" TextAlign="Center" />
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
        <f:Window ID="winEdit" Title="接入平台管理" Hidden="true" EnableIFrame="true" ClearIFrameAfterClose="false" runat="server" IsModal="true" Width="700px" Height="600px" EnableClose="true" EnableMaximize="false" EnableResize="false" OnClose="winEdit_Close" />
    </form>
</body>
</html>
