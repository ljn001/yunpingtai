<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PlanInfo.aspx.cs" Inherits="CloudPlatform.m.PlanInfo" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="initial-scale=1, maximum-scale=1, width=device-width" />
    <title>计划查询</title>
    <script type="text/javascript" src="/res/js/jweixin-1.2.0.js"></script>
    <script type="text/javascript" src="/res/js/pxmu.min.js"></script>
    <style>
        .ColMargin{
            margin-left:10px;
        }
        .btnScan{
            background-color: #f5b418 !important;
            color:#fff !important;
        }
        .txtQty input{
            font-weight:bold !important;
            color:#0054a6 !important;
            border-color:#0054a6 !important;
        }
        .btnDefault{
            background-color: #0054a5 !important;
            color:#fff !important;
        }
        .tag_g{
            display: inline-block;
            margin:2px;
            padding: 5px;
            border-radius:3px;
            background-color: #67c23a;
            border-color: #67c23a;
            color: #fff;
        }
        .tag_r{
            display: inline-block;
            margin:2px;
            padding: 5px;
            border-radius:3px;
            background-color: #f56c6c;
            border-color: #f56c6c;
            color: #fff;
        }
        .tag_b{
            display: inline-block;
            margin:2px;
            padding: 5px;
            border-radius:3px;
            background-color: #409eff;
            color: #fff;
        }
        .tag_y{
            display: inline-block;
            margin:2px;
            padding: 5px;
            border-radius:3px;
            background-color: #e6a23c;
            border-color: #e6a23c;
            color: #fff;
        }
        .tag_worker{
            display: inline-block;
            border-radius:3px;
            padding: 0 5px;
            background-color: #409eff;
            border-color: #e6a23c;
            color: #fff;
            margin-left:3px;
        }
    </style>
</head>
<body>
    <script type="text/javascript">
        <%= wxJsSdkConfig %>

        function scanPlan() {
            wx.scanQRCode({
                needResult: 1, // 默认为0，扫描结果由微信处理，1则直接返回扫描结果，
                scanType: ["barCode"], // 可以指定扫二维码还是一维码，默认二者都有["qrCode", "barCode"]
                success: function (res) {
                    var result = res.resultStr; // 当needResult 为 1 时，扫码返回的结果
                    F.customEvent('scanPlan|' + result);
                }
            });
        }
    </script>
    <form id="form1" runat="server">
        <f:PageManager ID="pmMain" runat="server" OnCustomEvent="pmMain_CustomEvent" AjaxAspnetControls="lblProc_Container" />
        <f:HiddenField runat="server" ID="lblWXOpenID" />
        <f:HiddenField runat="server" ID="lblDBConnStr" />
        <f:HiddenField runat="server" ID="lblWorkPlanID" />
        <f:HiddenField runat="server" ID="lblMTONo" />
        <table style="width:100%;padding:10px 20px;">
            <tr>
                <td><img src="../res/images/logo2.png" width="200" style="padding-top:8px;" /></td>
                <td style="text-align:right;"><img src="../res/images/logo1.png" width="60" /></td>
            </tr>
        </table>
        <div style="text-align:center;border-bottom:1px dashed #0054a6;margin:5px 20px;">
            <span style="font-size:22px;font-weight:bold;color:#0054a6;">生产计划查询</span>
        </div>
        <f:Panel runat="server" ID="Panel1" ShowBorder="false" ShowHeader="false" Layout="Fit" BodyPadding="20px">
            <Items>
                <f:SimpleForm ID="SimpleForm1" runat="server" ShowBorder="false" ShowHeader="false" Layout="VBox" MessageTarget="None" LabelAlign="Left" LabelWidth="80px">
                    <Items>
                        <f:Panel runat="server" ShowBorder="false" ShowHeader="false" Layout="Column">
                            <Items>
                                <f:TextBox runat="server" ID="txtPlanNo" ColumnWidth="100%" Label="生产计划">
                                    <Listeners>
                                        <f:Listener Event="enter" Handler="onPlanNoEnter" />
                                    </Listeners>
                                </f:TextBox>
                                <f:Button runat="server" ID="btnScan" Text="扫描计划码" Width="100px" CssClass="ColMargin btnScan" OnClick="btnScan_Click">
                                    <Listeners>
                                        <f:Listener Event="click" Handler="scanPlan" />
                                    </Listeners>
                                </f:Button>
                            </Items>
                        </f:Panel>
                        <f:TextBox runat="server" ID="txtBillNo" Label="订单号" Readonly="true" />
                        <f:TextBox runat="server" ID="txtModel" Label="规格型号" Readonly="true" />
                        <f:TextBox runat="server" ID="txtPlanQty" Label="计划数量" Readonly="true" />
                        <f:Panel runat="server" ShowBorder="false"  ShowHeader="false" Layout="Column">
                            <Items>
                                <f:TextBox runat="server" ID="txtFuTu" Label="附图号" Readonly="true" ColumnWidth="100%%" />
                                <f:Button runat="server" ID="btnDownloadFuTu" Text="查看附图" IconFont="Download" CssStyle="margin-left:5px;" OnClick="btnDownloadFuTu_Click" />
                            </Items>
                        </f:Panel>
                        <f:ContentPanel runat="server" ShowBorder="true" ShowHeader="true" Title="工序记录" CssStyle="margin-bottom:8px;">
                            <div id="lblProc_Container" style="padding:5px;">
                                <asp:Literal ID="lblProc" runat="server" />
                            </div>
                        </f:ContentPanel>
                    </Items>
                </f:SimpleForm>
            </Items>
        </f:Panel>
    </form>
    <script>
        function onPlanNoEnter() {
            F.customEvent('scanPlan|' + this.getValue());
        }
    </script>
</body>
</html>
