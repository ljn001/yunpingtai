<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="SkyLinkCloud.m.Error" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="initial-scale=1, maximum-scale=1, width=device-width" />
    <title>信息</title>
    <link href="css/style_blue.css" rel="stylesheet" id="style" />
</head>
<body class="body-scroll d-flex flex-column h-100 menu-overlay">
    <form id="form1" runat="server">
        <main class="flex-shrink-0 main">
            <div class="main-container h-100">
                <div class="container h-100">
                    <div class="row h-100">
                        <div class="col-12 col-md-6 col-lg-4 align-self-center text-center my-3 mx-auto">
                            <div class="icon icon-120 bg-danger-light text-danger rounded-circle mb-3">
                                <img src="img/info.png" style="width:60%;" />
                            </div>
                            <%--<h2 class="display-2">提示</h2>--%>
                            <h5 class="text-secondary mb-4 text-uppercase">提示信息</h5>
                            <p class="text-secondary">
                                <asp:Literal runat="server" ID="lblInfo" />
                            </p>
                            <br />
                            <a href="sczx" class="btn btn-default rounded">返回首页</a>
                        </div>
                    </div>
                </div>
            </div>
        </main>
    </form>
</body>
</html>
