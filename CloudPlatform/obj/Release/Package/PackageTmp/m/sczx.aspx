<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sczx.aspx.cs" Inherits="SkyLinkCloud.m.sczx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="initial-scale=1, maximum-scale=1, width=device-width" />
    <title>生产中心</title>
    <link href="css/style_blue.css" rel="stylesheet" id="style" />
    <style>
        a {
            text-decoration: none !important;
        }

        .info{
            width:100%;
            text-align:center;
            font-size:16px;
            color:#0054a6;
            padding: 50px 0px;
        }
    </style>
</head>
<body style="background-color: #f0f3f7;">
    <form id="form1" runat="server">
        <f:PageManager ID="pmMain" runat="server" />
        <f:HiddenField runat="server" ID="lblWXOpenID" />
        <f:HiddenField runat="server" ID="lblDBConnStr" />
        <div>
            <div style="margin:15px;">
                <table style="width:100%; padding: 10px 20px;">
                    <tr>
                        <td><img src="../res/images/logo2.png" width="200" style="padding-top: 8px;" /></td>
                        <td style="text-align: right;"><img src="../res/images/logo1.png" width="60" /></td>
                    </tr>
                </table>
            </div>
            <div class="container mb-3">
                <div class="card">
                    <div class="card-body">
                        <div class="row">
                            <div class="col">
                                <h5 class="mb-1"><asp:Literal runat="server" ID="lblUserName" /></h5>
                                <p class="text-secondary"><asp:Literal runat="server" ID="lblCusName" />　<a href="UserBindInfo.aspx">[切换]</a></p>
                            </div>
                            <div class="col-auto pl-0">
                                <div class="btn btn-40 bg-default-light text-default rounded-circle">
                                    <img src="img/user.png" style="width:90%;" />
                                </div>
                            </div>
                        </div>
                        <div class="progress h-5 mt-3">
                            <div class="progress-bar bg-default" role="progressbar" style="width:100%" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100"></div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="container mb-4">
                <div class="card">
                    <div class="card-body text-center ">
                        <div class="row justify-content-equal no-gutters">
                            <asp:Literal runat="server" ID="lblMenu" />
                            <%--<div class="col-4 col-md-2 mt-1 mb-1">
                                <a href="HBLog">
                                    <div class="icon icon-70 rounded-circle mb-1 bg-default-light text-default">
                                        <img src="img/hbjl.png" style="width:60%;" />
                                    </div>
                                    <p class="text-secondary"><small>汇报记录</small></p>
                                </a>
                            </div>
                            <div class="col-4 col-md-2 mt-1 mb-1">
                                <a href="PlanInfo">
                                    <div class="icon icon-70 rounded-circle mb-1 bg-default-light text-default">
                                        <img src="img/jhcx.png" style="width:72%;" />
                                    </div>
                                    <p class="text-secondary"><small>计划查询</small></p>
                                </a>
                            </div>--%>
                            
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
