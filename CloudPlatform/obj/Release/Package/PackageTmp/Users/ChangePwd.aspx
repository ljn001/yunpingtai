<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePwd.aspx.cs" Inherits="SkyLinkCloud.Users.ChangePwd" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>修改密码</title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" />
        <f:Panel runat="server" ID="Panel1" ShowBorder="false" ShowHeader="false">
            <Items>
                <f:SimpleForm runat="server" ShowBorder="false" ShowHeader="false" BodyPadding="20px">
                    <Toolbars>
                        <f:Toolbar runat="server">
                            <Items>
                                <f:Button runat="server" ID="btnSave" IconFont="_Save" Text="保存" OnClick="btnSave_Click" />
                                <f:ToolbarFill runat="server" />
                                <f:Button runat="server" ID="btnClose" IconFont="_Close" Text="关闭" />
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Items>
                        <f:TextBox runat="server" ID="txtPwdOld" Label="原始密码" TextMode="Password" />
                        <f:TextBox runat="server" ID="txtPwdNew" Label="新密码" TextMode="Password" />
                        <f:TextBox runat="server" ID="txtPwdRe" Label="重复新密码" TextMode="Password" />
                    </Items>
                </f:SimpleForm>
            </Items>
        </f:Panel>
    </form>
</body>
</html>