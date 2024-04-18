<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Customer.aspx.cs" Inherits="CloudPlatform.Customer.Customer" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>客户管理</title>
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
                        <f:Button runat="server" ID="btnAdd" IconFont="_RoundPlus" Text="新增客户" OnClick="btnAdd_Click" />
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:Grid runat="server" ID="gridCustomer" ShowHeader="false" ShowBorder="false" EnableHeaderMenu="false" OnRowCommand="gridCustomer_RowCommand">
                    <Columns>
                        <f:WindowField ColumnID="myWindowField" Width="50px" TextAlign="Center" WindowID="winEdit" HeaderText="编辑" Icon="Pencil" DataIFrameUrlFields="FAppKey" DataTextFormatString="{0}"
                            DataIFrameUrlFormatString="CustomerEdit.aspx?AppKey={0}" DataWindowTitleField="FNameShort" DataWindowTitleFormatString="编辑 - {0}" />
                        <f:LinkButtonField Width="50px" HeaderText="删除" TextAlign="Center" ConfirmText="确定要删除此用户吗？" ConfirmIcon="Question" ConfirmTarget="Self" CommandName="Delete" Icon="Delete" />
                        <f:BoundField ColumnID="FAppKey" DataField="FAppKey" HeaderText="AppKey" Width="280px" TextAlign="Center" Hidden="true" />
                        <f:BoundField ColumnID="FName" DataField="FName" HeaderText="客户名称" Width="250px" />
                        <f:BoundField ColumnID="FNameShort" DataField="FNameShort" HeaderText="客户简称" Width="150px" />
                        <f:BoundField ColumnID="FTel" DataField="FTel" HeaderText="联系电话" TextAlign="Center" Width="150px" />
                        <f:BoundField ColumnID="FCode" DataField="FCode" HeaderText="统一社会信用代码" TextAlign="Center" Width="180px" />
                        <f:BoundField ColumnID="FAddress" DataField="FAddress" HeaderText="地址" Width="300px" />
                        <f:CheckBoxField DataField="FEnabled" HeaderText="可用" Width="50px" TextAlign="Center" />
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
        <f:Window ID="winEdit" Title="客户管理" Hidden="true" EnableIFrame="true" IFrameUrl="UsersEdit.aspx" ClearIFrameAfterClose="false" runat="server" IsModal="true" Width="800px" Height="700px" EnableClose="true" EnableMaximize="false" EnableResize="false" OnClose="winEdit_Close" />
    </form>
</body>
</html>
