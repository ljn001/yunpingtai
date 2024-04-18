<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UsersEdit.aspx.cs" Inherits="CloudPlatform.Users.UsersEdit" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>编辑用户</title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" />
        <f:Panel ID="Panel1" runat="server" ShowBorder="false" ShowHeader="false" BodyPadding="10px">
            <Toolbars>
                <f:Toolbar runat="server">
                    <Items>
                        <f:Button runat="server" ID="btnSave" Text="保存" IconFont="_Save" OnClick="btnSave_Click" ValidateForms="frm1" />
                        <f:ToolbarFill runat="server" />
                        <f:Button runat="server" ID="btnClose" Text="关闭" IconFont="_Close" />
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:Panel ID="Panel2" runat="server" ShowBorder="false" ShowHeader="false" Layout="HBox" BoxConfigAlign="StretchMax">
                    <Items>
                        <f:SimpleForm ID="Panel3" BoxFlex="6" MarginRight="10px" runat="server" ShowBorder="false" ShowHeader="false" LabelAlign="Top">
                            <Items>
                                <f:TextBox ID="txtFUserID" runat="server" Label="账号" ShowRedStar="true" Required="true" />
                                <f:TextBox ID="txtFUserName" runat="server" Label="姓名" ShowRedStar="true" Required="true" />
                                <f:TextBox ID="txtFPassword" runat="server" Label="密码" ShowRedStar="true" Required="true" />
                                <f:RadioButtonList runat="server" ID="rblFEnabled" Label="可用">
                                    <f:RadioItem Value="True" Text="是" Selected="true" />
                                    <f:RadioItem Value="False" Text="否" />
                                </f:RadioButtonList>
                            </Items>
                        </f:SimpleForm>
                        <f:Panel ID="Panel4" runat="server" BoxFlex="4" ShowBorder="false" ShowHeader="false" MarginRight="5px" Layout="VBox">
                            <Items>
                                <f:Tree ID="treePower" IsFluid="true" CssClass="blockpanel" EnableCollapse="false" ShowHeader="true" Title="用户权限" runat="server" EnableCheckBox="true" CascadeCheck="true" Height="490px">
                                    <Nodes>
                                        <f:TreeNode Text="平台管理" Expanded="true">
                                            <f:TreeNode Text="客户管理" AttributeDataTag="平台管理|/res/icon/user_orange.png|/Customer/Customer.aspx" />
                                        </f:TreeNode>
                                        <f:TreeNode Text="基础资料" Expanded="true">
                                            <f:TreeNode Text="用户管理" AttributeDataTag="基础资料|/res/icon/user.png|/Users/Users.aspx" />
                                            <f:TreeNode Text="接入平台" AttributeDataTag="基础资料|/res/icon/world_link.png|/Platform/Platform.aspx" />
                                        </f:TreeNode>
                                    </Nodes>
                                    <Listeners>
                                        <f:Listener Event="nodecheck" Handler="onTreeNodeCheck" />
                                    </Listeners>
                                </f:Tree>
                            </Items>
                        </f:Panel>
                    </Items>
                </f:Panel>
            </Items>
        </f:Panel>
    </form>
    <script>
        function onTreeNodeCheck(event, nodeId, checked) {
            if (checked) {
                // 第二个参数true：递归更新全部子节点
                this.checkNode(nodeId, true);
            } else {
                this.uncheckNode(nodeId, true);
            }
        }
    </script>
</body>
</html>
