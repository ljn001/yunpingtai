<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerEdit.aspx.cs" Inherits="SkyLinkCloud.Customer.CustomerEdit" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>客户信息</title>
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
                <f:Panel ID="Panel2" runat="server" ShowBorder="false" ShowHeader="false" Layout="HBox" BoxConfigAlign="StretchMax">
                    <Items>
                        <f:SimpleForm ID="Panel3" BoxFlex="6" MarginRight="10px" runat="server" ShowBorder="false" ShowHeader="false" LabelWidth="140px">
                            <Items>
                                <f:TextBox ID="txtFAppKey" runat="server" Label="AppKey" Readonly="true" />
                                <f:TextBox ID="txtFName" runat="server" Label="客户名称" ShowRedStar="true" Required="true" />
                                <f:TextBox ID="txtFNameShort" runat="server" Label="客户简称" ShowRedStar="true" Required="true" />
                                <f:TextBox ID="txtFTel" runat="server" Label="联系电话" />
                                <f:TextBox ID="txtFCode" runat="server" Label="统一社会信用代码" />
                                <f:TextBox ID="txtFAddress" runat="server" Label="地址" />
                                <f:TriggerBox ID="txtFAppSecret" runat="server" Label="AppSecret" TriggerIconUrl="../res/icon/arrow_refresh.png" OnTriggerClick="txtFAppSecret_TriggerClick" />
                                <f:CheckBox runat="server" ID="cbxFEnabled" Text="是否可用" DisplayType="Switch" Checked="true" SwitchOnText="可用" SwitchOffText="停用" ShowSwitchText="true" CssClass="mycheckbox" />
                                <f:TextBox runat="server" ID="txtFAppKey_HQ" Label="环球AppKey" />
                                <f:TextBox runat="server" ID="txtFAppSecret_HQ" Label="环球AppSecret" />
                                <f:DropDownList runat="server" ID="ddlSysType" Label="系统类型">
                                    <f:ListItem Value="鱼竿系统" Text="鱼竿系统" />
                                    <f:ListItem Value="配件系统" Text="配件系统" />
                                </f:DropDownList>
                                <f:TextBox runat="server" ID="txtDBLink" Label="数据库地址" />
                                <f:TextBox runat="server" ID="txtDBUser" Label="数据库用户" />
                                <f:TextBox runat="server" ID="txtDBPwd" Label="数据库密码" TextMode="Password" />
                                <f:TextBox runat="server" ID="txtDBName" Label="数据库名称" />
                            </Items>
                        </f:SimpleForm>
                        <f:Panel ID="Panel4" runat="server" Title="授权接入平台" Icon="Key" BoxFlex="4" ShowBorder="true" ShowHeader="true" MarginRight="5px" Layout="VBox" BodyPadding="10px" Height="490px">
                            <Items>
                                <f:CheckBoxList runat="server" ID="cblPlatform" DisplayType="Switch" SwitchOnText="启用" SwitchOffText="停用" SwitchTextVisible="true" CssClass="mycheckbox" />
                            </Items>
                        </f:Panel>
                    </Items>
                </f:Panel>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
