<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CloudPlatform.Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=edge" />
    <title>智能制造云平台</title>
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="static/css/bootstrap.min.css" />
    <link href="static/css/css.css" rel="stylesheet " />
    <link rel="stylesheet" href="static/css/style.css " />
</head>
<body>
    <f:PageManager ID="PageManager1" runat="server" AjaxLoadingType="Default" AjaxAspnetControls="txtUserID,txtPassword" />
    <form id="form1" runat="server">
        <div id="preloader" class="preloader">
            <div class='inner'>
                <div class='line1'></div>
                <div class='line2'></div>
                <div class='line3'></div>
            </div>
        </div>
        <section class="fxt-template-animation fxt-template-layout34" data-bg-image="img/elements/bg1.png">
            <div class="fxt-shape">
                <div class="fxt-transformX-L-50 fxt-transition-delay-1">
                    <img src="static/picture/shape1.png" alt="Shape" />
                </div>
            </div>
            <div class="container">
                <div class="row">
                    <div class="col-lg-8">
                        <div class="fxt-column-wrap justify-content-between">
                            <div class="fxt-animated-img">
                                <div class="fxt-transformX-L-50 fxt-transition-delay-10">
                                    <img src="static/picture/bg34-1.png" alt="Image" />
                                </div>
                            </div>
                            <div class="fxt-transformX-L-50 fxt-transition-delay-3">
                                <div href="" class="fxt-logo">
                                    <img src="res/images/Logo1.png" width="90" alt="Logo" />
                                    <img src="res/images/Logo2.png" width="320" alt="Logo" style="margin:-3px 0 0 5px;" />
                                </div>
                            </div>
                            <div class="fxt-transformX-L-50 fxt-transition-delay-5">
                                <div class="fxt-middle-content">
                                    <h1 class="fxt-main-title">欢迎</h1>
                                    <div class="fxt-switcher-description1" style="font-size:16px;">实现多数据源信息的整合，建立多种数据分析模型，通过可灵活配置的业务驱动模型实现制造业的智能化管理。</div>
                                </div>
                            </div>
                            <div class="fxt-transformX-L-50 fxt-transition-delay-7">
                                <div class="fxt-qr-code">
                                    <img src="res/images/WXQRCode.jpg" alt="QR" width="200" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-4">
                        <div class="fxt-column-wrap justify-content-center">
                            <div class="fxt-form">
                                <div>
                                    <div class="form-group">
                                        <div style="margin:10px 3px;">用户名：</div>
                                        <asp:TextBox runat="server" ID="txtUserID" class="form-control" placeholder="请填写用户名" required="required" />
                                    </div>
                                    <div class="form-group">
                                        <div style="margin:10px 3px;">密　码：</div>
                                        <asp:TextBox runat="server" ID="txtPassword" class="form-control" TextMode="Password" placeholder="请填写密码" required="required" />
                                    </div>
                                    <div class="form-group" style="margin-top:50px;">
                                        <asp:Button runat="server" ID="btnLogin" Text="用户登录" OnClick="btnLogin_Click" class="fxt-btn-fill" UseSubmitBehavior="false" style="background-color:#0054a6;" />
                                    </div>
                                </div>
                            </div>
                            <div class="fxt-style-line" style="margin-top:30px;">
                                <span>版权所有 & 技术支持</span>
                            </div>
                            <div class="fxt-socials" style="text-align:center;color:#808080;">
                                山东环球渔具股份有限公司
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </form>
    <!-- Bootstrap js -->
    <script src="static/js/bootstrap.min.js"></script>
    <!-- Imagesloaded js -->
    <script src="static/js/imagesloaded.pkgd.min.js"></script>
    <!-- Validator js -->
    <script src="static/js/validator.min.js"></script>
    <!-- Custom Js -->
    <script src="static/js/main.js"></script>
</body>
</html>
