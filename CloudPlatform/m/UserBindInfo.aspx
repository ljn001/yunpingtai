<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserBindInfo.aspx.cs" Inherits="CloudPlatform.Mobile.UserBindInfo" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="initial-scale=1, maximum-scale=1, width=device-width" />
    <title>已绑定账号</title>
    <script type="text/javascript" src="/res/js/pxmu.min.js"></script>
    <style>
        .card{
            border:1px solid #0054a6;
            border-radius:5px;
            padding:10px;
            background-image:linear-gradient(135deg,#eef2f3,#8e9eab);
            margin-bottom:10px;
        }
        .card_empty{
            border:1px solid #ffa502;
            border-radius:5px;
            padding:10px;
            margin-top:30px;
            text-align:center;
            background-color:#ffc947;
            color:#e74c3c;
            font-size:16px;
        }
        .tdBtn .f-btn .f-btn-text{
            line-height:25px;
        }
        .tag_Green{
            margin: 0 0 0 5px;
            padding: 1px 10px 2px 10px;
            border-radius:3px;
            background-color: #67c23a;
            border-color: #67c23a;
            color: #fff;
            font-size:12px;
            display:inline-block;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" />
        <f:HiddenField runat="server" ID="lblWXOpenID" />
        <div style="padding:20px;">
            <table style="width:100%;">
                <tr>
                    <td><img src="../res/images/logo2.png" width="200" style="padding-top:8px;" /></td>
                    <td style="text-align:right;"><img src="../res/images/logo1.png" width="60" /></td>
                </tr>
            </table>
            <div style="width:100%;text-align:center;margin:20px 0 0 0;">
                <span style="font-size:22px;font-weight:bold;color:#0054a6;">已绑定账号信息</span>
                <hr style="height:1px;border:none;border-top:1px dashed #0054a6;" />
            </div>
            <asp:ListView runat="server" ID="lvBindList">
                <LayoutTemplate>
				    <div style="padding-top:5px;">
					    <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
				    </div>
			    </LayoutTemplate>
			    <ItemTemplate>
                    <div class="card">
                        <div style="display:table-cell;vertical-align:middle;">
                            <span style="font-weight:bold;font-size:16px;"><%# Eval("bumendaihao") %>（<%# Eval("bumen") %>）</span>
                           <%-- <%# !string.IsNullOrEmpty(Eval("FDefault").ToString()) ? (bool.Parse(Eval("FDefault").ToString()) ? "<span class=\"tag_Green\">默认</span>" : null) : null %>--%>
                        </div>
                        <hr style="height:1px;border:none;border-top:1px dashed #c7ddf2;" />
                        <table style="width:100%">
                            <tr>
                                <td style="line-height:26px;">
                                    账号：<%# Eval("gonghao") %>（<%# Eval("xingming") %>）
                                    <br />
                                    手机：<%# Eval("lianxifangshi") %>
                                    <br />
                                    绑定时间：<%# DateTime.Parse(Eval("wxdate").ToString()).ToString("yyyy-MM-dd HH:mm") %>
                                </td>
                                <td class="tdBtn" style="width:80px;text-align:right;">
                                  <%--  <f:Button runat="server" ID="btnDefault" Text="设为默认" OnClick="btnDefault_Click" ConfirmText="确认将此单位设为默认吗？" ConfirmTarget="Top" AttributeDataTag='<%# eu.DEC_Encrypt(Eval("FID").ToString()) %>' CssStyle="margin-bottom:5px;" />--%>
                                    <f:Button runat="server" ID="btnDeleteBind" Text="解除绑定" OnClick="btnDeleteBind_Click" ConfirmText="确认对此账号解除绑定吗？" ConfirmTarget="Top" AttributeDataTag='<%# eu.DEC_Encrypt(Eval("hetongbianhao").ToString()) %>' />
                                </td>
                            </tr>
                        </table>
                    </div>
                </ItemTemplate>
                <EmptyDataTemplate>
                    <div class="card_empty">
                        当前微信号尚未绑定任何系统账号！
                    </div>
                </EmptyDataTemplate>
            </asp:ListView>
        </div>
    </form>
</body>
</html>
