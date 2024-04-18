<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PlatformEdit.aspx.cs" Inherits="SkyLinkCloud.Platform.PlatformEdit" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>接入平台信息</title>
    <style>
        .mycheckbox .f-field-checkbox-switch .f-field-checkbox-switch-text {
            min-width: 40px;
        }
    </style>
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
                <f:Panel ID="Panel2" runat="server" ShowBorder="false" ShowHeader="false" BoxConfigAlign="StretchMax">
                    <Items>
                        <f:SimpleForm ID="Panel3" runat="server" ShowBorder="false" ShowHeader="false" LabelAlign="Top" LabelWidth="150px" BodyPadding="10px">
                            <Items>
                                <f:HiddenField runat="server" ID="lblFID" />
                                <f:TextBox ID="txtFName" runat="server" Label="平台名称（单位全称）" ShowRedStar="true" Required="true" />
                                <f:TextBox ID="txtFNameShort" runat="server" Label="平台简称（单位简称）" ShowRedStar="true" Required="true" />
                                <f:DropDownList runat="server" ID="ddlFType" Label="接入类型" AutoPostBack="true" OnSelectedIndexChanged="ddlFType_SelectedIndexChanged">
                                    <f:ListItem Value="API" Text="API接入" />
                                    <f:ListItem Value="DB" Text="数据库接入" />
                                </f:DropDownList>
                                <f:TextBox runat="server" ID="txtFAPIUrl" Label="服务器地址" />
                                <f:TextBox runat="server" ID="txtFAppKey" Label="AppKey" />
                                <f:TextBox runat="server" ID="txtFAppSecret" Label="AppSecret" TextMode="Password" />

                                <f:TextBox runat="server" ID="txtFDBName" Label="服务器名称" Hidden="true" />
                                <f:TextBox runat="server" ID="txtFDBLoginID" Label="用户名" Hidden="true" />
                                <f:TextBox runat="server" ID="txtFDBPwd" Label="密码" TextMode="Password" Hidden="true" />

                                <f:CheckBox runat="server" ID="cbxFEnabled" Text="平台是否可用" DisplayType="Switch" Checked="true" SwitchOnText="可用" SwitchOffText="停用" ShowSwitchText="true" CssClass="mycheckbox" />
                            </Items>
                        </f:SimpleForm>
                    </Items>
                </f:Panel>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
