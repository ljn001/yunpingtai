<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserBind.aspx.cs" Inherits="CloudPlatform.Mobile.UserBind" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="initial-scale=1, maximum-scale=1, width=device-width" />
    <title>用户绑定</title>
    <script type="text/javascript" src="/res/js/pxmu.min.js"></script>
    <style>
        #Panel1_SimpleForm1_btnVerifyCode{
            background-color: #f5b418;
            color:#fff;
        }
        #Panel1_SimpleForm1_btnUserBind{
            background-color: #0054a6;
            color:#fff;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AjaxLoadingType="Mask" />
        <f:HiddenField runat="server" ID="lblWXOpenID" />
        <f:HiddenField runat="server" ID="lblIsScanCode" />
        <f:HiddenField runat="server" ID="lblVerifyCode" />
        <f:HiddenField runat="server" ID="lblCusAppKey" />
        <f:HiddenField runat="server" ID="lblCusUserID" />
        <table style="width:100%;padding:10px 20px;">
            <tr>
                <td><img src="../res/images/logo2.png" width="200" style="padding-top:8px;" /></td>
                <td style="text-align:right;"><img src="../res/images/logo1.png" width="60" /></td>
            </tr>
        </table>
        <div style="text-align:center;border-bottom:1px dashed #0054a6;margin:5px 20px;">
            <span style="font-size:22px;font-weight:bold;color:#0054a6;">用户微信绑定</span>
        </div>
        <f:Panel runat="server" ID="Panel1" ShowBorder="false" ShowHeader="false" Layout="Fit" BodyPadding="0 20px">
            <Items>
                <f:Form ID="SimpleForm1" runat="server" ShowBorder="false" ShowHeader="false" Layout="VBox" MessageTarget="None" LabelAlign="Top">
                    <Items>
                        <%--<f:Panel runat="server" ShowBorder="false" ShowHeader="false" Layout="HBox">
                            <Items>
                                <f:TextBox runat="server" ID="txtCusName" Label="企业全称" BoxFlex="1" />
                            </Items>
                        </f:Panel>--%>
                        <f:TextBox runat="server" ID="txtDepart" Label="工厂" />
                        <f:TextBox runat="server" ID="txtUserName" Label="姓名" />
                        <f:NumberBox runat="server" ID="txtPhone" Label="手机号" InputType="number" NoDecimal="true" ShowTrigger="false" RegexPattern="NUMBER" />
                        <f:Button runat="server" ID="btnVerifyCode" Text="发送短信验证码" OnClick="btnVerifyCode_Click" EnableAjaxLoading="false" />
                        <f:NumberBox runat="server" ID="txtVerifyCode" Label="验证码" EmptyText="请输入短信验证码" InputType="number" NoDecimal="true" ShowTrigger="false" RegexPattern="NUMBER" />
                        <f:Button runat="server" ID="btnUserBind" Text="账号绑定" OnClick="btnUserBind_Click" Size="Large" />
                    </Items>
                </f:Form>
            </Items>
        </f:Panel>
    </form>
    <script>
        function timer() {
            var btnVerifyCodeClient = '<%= btnVerifyCode.ClientID %>';
            F(btnVerifyCodeClient).disable();
            let itme = 60;
            F(btnVerifyCodeClient).setText("重发验证码（" + itme + "s）");
            itme--;
            setInterval(() => {
                if (itme > 0 && itme <= 60) {
                    F(btnVerifyCodeClient).setText("重发验证码（" + itme-- + "s）");
                } else {
                    F(btnVerifyCodeClient).enable();
                    F(btnVerifyCodeClient).setText("发送短信验证码");
                }
            }, 1000);
        }
    </script>
</body>
</html>
