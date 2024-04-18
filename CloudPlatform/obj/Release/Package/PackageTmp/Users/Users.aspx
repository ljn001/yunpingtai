<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="SkyLinkCloud.Users.Users" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>用户管理</title>
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
                        <f:Button runat="server" ID="btnAdd" IconFont="_RoundPlus" Text="新增用户">
                            <Listeners>
                                <f:Listener Event="click" Handler="onAddClick" />
                            </Listeners>
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:Grid runat="server" ID="gridUsers" ShowHeader="false" ShowBorder="false" EnableHeaderMenu="false" DataKeyNames="FUserID,FUserName" OnRowCommand="gridUsers_RowCommand">
                    <Columns>
                        <f:WindowField ColumnID="myWindowField" Width="50px" TextAlign="Center" WindowID="winEdit" HeaderText="编辑" Icon="Pencil" DataIFrameUrlFields="FUserID" DataTextFormatString="{0}"
                            DataIFrameUrlFormatString="UsersEdit.aspx?ID={0}" DataWindowTitleField="FUserName" DataWindowTitleFormatString="编辑用户 - {0}" />
                        <f:LinkButtonField Width="50px" HeaderText="删除" TextAlign="Center" ConfirmText="确定要删除此用户吗？" ConfirmIcon="Question" ConfirmTarget="Self" CommandName="Delete" Icon="Delete" />
                        <f:BoundField ColumnID="FUserID" DataField="FUserID" HeaderText="账号" Width="150px" TextAlign="Center" />
                        <f:BoundField ColumnID="FUserName" DataField="FUserName" HeaderText="姓名" Width="150px" TextAlign="Center" />
                        <f:CheckBoxField DataField="FEnabled" HeaderText="可用" Width="50px" TextAlign="Center" />
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
        <f:Window ID="winEdit" Title="用户管理" Hidden="true" EnableIFrame="true" IFrameUrl="UsersEdit.aspx" ClearIFrameAfterClose="false" runat="server" IsModal="true" Width="700px" Height="600px" EnableClose="true" EnableMaximize="false" EnableResize="false" OnClose="winEdit_Close" />
    </form>
    <script>
        function onAddClick(event) {
            F(winEdit).show('UsersEdit.aspx','新增用户');
        }
    </script>
</body>
</html>
