<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScreenEdit.aspx.cs" Inherits="SkyLinkCloud.Screen.ScreenEdit" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>数据大屏信息</title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" />
        <f:Panel ID="Panel1" runat="server" ShowBorder="false" ShowHeader="false" BodyPadding="10px">
            <Toolbars>
                <f:Toolbar runat="server">
                    <Items>
                        <f:Button runat="server" ID="btnSave" Text="保存" IconFont="_Save" OnClick="btnSave_Click" />
                        <f:ToolbarFill runat="server" />
                        <f:Button runat="server" ID="btnClose" Text="关闭" IconFont="_Close" />
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:SimpleForm ID="Panel3" MarginRight="10px" runat="server" ShowBorder="false" ShowHeader="false" LabelAlign="Top">
                    <Items>
                        <f:TextBox runat="server" ID="txtIMEI" Label="设备IMEI" Readonly="true" />
                        <f:DropDownList runat="server" ID="ddlCus" Label="所属客户" />
                        <f:TextBox runat="server" ID="txtRemark" Label="备注" />
                        <f:RadioButtonList runat="server" ID="rblFEnabled" Label="可用">
                            <f:RadioItem Value="True" Text="是" Selected="true" />
                            <f:RadioItem Value="False" Text="否" />
                        </f:RadioButtonList>
                    </Items>
                </f:SimpleForm>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
