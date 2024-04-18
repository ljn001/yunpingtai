<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Screen.aspx.cs" Inherits="SkyLinkCloud.Screen.Screen" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>数据大屏</title>
    <link type="text/css" rel="stylesheet" href="/res/css/small.css"/>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" />
        <f:Panel runat="server" ID="Panel1" ShowBorder="false" ShowHeader="false" Layout="Fit">
            <Toolbars>
                <f:Toolbar runat="server">
                    <Items>
                        <f:TriggerBox runat="server" ID="txtSearch" EmptyText="搜索..." TriggerIcon="Search" OnTriggerClick="txtSearch_TriggerClick" />
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:Grid runat="server" ID="gridScreen" ShowHeader="false" ShowBorder="false" EnableHeaderMenu="false" DataKeyNames="FIMEI" OnRowCommand="gridScreen_RowCommand">
                    <Columns>
                        <f:WindowField ColumnID="myWindowField" Width="50px" TextAlign="Center" WindowID="winEdit" DataWindowTitleField="FIMEI" DataWindowTitleFormatString="数据大屏信息" HeaderText="编辑" Icon="Pencil" DataIFrameUrlFields="FIMEI" DataIFrameUrlFormatString="ScreenEdit.aspx?imei={0}" />
                        <f:LinkButtonField Width="50px" HeaderText="删除" TextAlign="Center" ConfirmText="确定要删除此数据大屏吗？" ConfirmIcon="Question" ConfirmTarget="Self" CommandName="Delete" Icon="Delete" />
                        <f:BoundField ColumnID="FCusName" DataField="FCusName" HeaderText="客户名称" Width="250px" />
                        <f:BoundField ColumnID="FAddTime" DataField="FAddTime" HeaderText="添加时间" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" Width="160px" TextAlign="Center" />
                        <f:BoundField ColumnID="FIMEI" DataField="FIMEI" DataToolTipField="FIMEI" HeaderText="IMEI" Width="400px" />
                        <f:CheckBoxField DataField="FEnabled" HeaderText="可用" Width="50px" TextAlign="Center" />
                        <f:BoundField ColumnID="FRemark" DataField="FRemark" HeaderText="备注" Width="150px" />
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
        <f:Window ID="winEdit" Title="数据大屏管理" Hidden="true" EnableIFrame="true" runat="server" IsModal="true" Width="700px" Height="400px" EnableClose="true" EnableMaximize="false" EnableResize="false" OnClose="winEdit_Close" />
    </form>
</body>
</html>
