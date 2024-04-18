<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HBLogYG.aspx.cs" Inherits="CloudPlatform.m.HBLogYG" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="initial-scale=1, maximum-scale=1, width=device-width" />
    <title>汇报记录</title>
    <link href="css/style_blue.css" rel="stylesheet" id="style" />
    <script type="text/javascript" src="/res/js/pxmu.min.js"></script>
</head>
<body class="f-body f-widget-content f-mobi f-webkit f-safari f-ios f-iphone f-theme-pure_blue">
    <form id="form1" runat="server">
        <f:PageManager ID="pmMain" runat="server" OnCustomEvent="pmMain_CustomEvent" />
        <f:HiddenField runat="server" ID="lblWXOpenID" />
        <f:HiddenField runat="server" ID="lblDBConnStr" />
        <main class="flex-shrink-0 main" style="padding-top:10px;">
            <div class="main-container" style="background-color:#70a1ff;">
                <div class="container mt-2">
                     <asp:ListView runat="server" ID="lvHBLog">
                        <LayoutTemplate>
				            <div style="padding-top:5px;">
					            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
				            </div>
			            </LayoutTemplate>
                        <EmptyDataTemplate>
                            <div style="text-align:center;width:100%;margin:20px 0">
                                <span style="color:#fff;font-weight:bold;">您7日内未提交任何生产汇报</span>
                            </div>
                        </EmptyDataTemplate>
			            <ItemTemplate>
                            <div class="card border-0 mb-2">
                                <div class="card-header">
                                    <div class="row align-items-center">
                                        <div class="col align-self-center  pr-0">
                                            <h6 class="mb-0"><%# Eval("FProcName") %></h6>
                                            <p class="text-secondary"><%# DateTime.Parse(Eval("FReportDT").ToString()).ToString("yyyy-MM-dd") %></p>
                                        </div>
                                        <div class="col-auto align-self-center pl-0">
                                            <a class="btn btn-sm btn-danger rounded" style="color:#fff" onclick="OnDeleteClick('<%# Eval("FID") %>');">删除汇报</a>
                                        </div>
                                    </div>
                                </div>
                                <div class="card-body bg-warning-light" style="padding:5px;">
                                    <h1 class="text-center mb-0"><%# Eval("FQTY") %></h1>
                                </div>
                                <div class="card-footer  bg-warning text-white">
                                    <div class="row align-items-center">
                                        <div class="col">
                                            <p class="mb-0"><%# Eval("DrawSeriesNo") %></p>
                                            <p class="mb-1">图纸</p>
                                        </div>
                                           <div class="col">
                                            <p class="mb-0"><%# Eval("BlankName") %></p>
                                            <p class="mb-1">素材</p>
                                        </div>
                                        <div class="col-auto align-self-center text-right">
                                            <p class="mb-0"><%# Eval("orderNumber") %></p>
                                            <p class="mb-1">订单号</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </div>
        </main>
    </form>
    <script>
        function OnDeleteClick(id) {
            pxmu.diaglog({
                title: { text: '操作确认' },
                content: { text: '<div style="width:100%;text-align:center;"><img src="/res/images/svg/Question.svg" width="70" /><div style="margin-top:20px">是否确认删除此汇报？</div></div>' },
                line: { solid: 1, color: '#d4d4d4' },
                btn: {
                    left: { text: '取消', bg: '#fff', solidcolor: '#fff', color: '#464646' },
                    right: { text: '确定', bg: '#fff', solidcolor: '#fff', color: '#0054a6' }
                },
                congif: {
                    btncount: false, animation: 'slideup', anclose: false
                }}).then(function (res) {
                    if (res.btn == 'right') {
                        F.customEvent('delete|'  + id);
                    }
                    else {

                    }
                });
        }
    </script>
</body>
</html>
