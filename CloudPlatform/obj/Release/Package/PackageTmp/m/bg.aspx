<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="bg.aspx.cs" Inherits="SkyLinkCloud.m.bg" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="initial-scale=1, maximum-scale=1, width=device-width" />
    <title>工序汇报</title>
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

        function scanProcedure() {
            var txtBillNoClientID = '<%= txtBillNo.ClientID %>';
            var billno = F(txtBillNoClientID).getValue();
            if (billno.trim().length > 0) {
                wx.scanQRCode({
                    needResult: 1, // 默认为0，扫描结果由微信处理，1则直接返回扫描结果，
                    scanType: ["barCode"], // 可以指定扫二维码还是一维码，默认二者都有["qrCode", "barCode"]
                    success: function (res) {
                        var result = res.resultStr; // 当needResult 为 1 时，扫码返回的结果
                        F.customEvent('scanProcedure|' + result);
                    }
                });
            }
            else {
                pxmu.toast('请先指定生产计划');
            }
        }
    </script>
    <form id="form1" runat="server">
        <f:PageManager ID="pmMain" runat="server" OnCustomEvent="pmMain_CustomEvent" ValidateForms="SimpleForm1" />
        <f:HiddenField runat="server" ID="lblWXOpenID" />
        <f:HiddenField runat="server" ID="lblWorkPlanID" />
        <f:HiddenField runat="server" ID="lblDBConnStr" />
        <f:HiddenField runat="server" ID="lblUserID" />
        <f:HiddenField runat="server" ID="lblUserProcIDList" />
        <f:HiddenField runat="server" ID="lblMobileProcList" />
        <table style="width:100%;padding:10px 20px;">
            <tr>
                <td><img src="../res/images/logo2.png" width="200" style="padding-top:8px;" /></td>
                <td style="text-align:right;"><img src="../res/images/logo1.png" width="60" /></td>
            </tr>
        </table>
        <div style="text-align:center;border-bottom:1px dashed #0054a6;margin:5px 20px;">
            <span style="font-size:22px;font-weight:bold;color:#0054a6;">工序汇报</span>
        </div>
        <f:Panel runat="server" ID="Panel1" ShowBorder="false" ShowHeader="false" Layout="Fit" BodyPadding="20px">
            <Items>
                <f:SimpleForm ID="SimpleForm1" runat="server" ShowBorder="false" ShowHeader="false" Layout="VBox" MessageTarget="None" LabelAlign="Left" LabelWidth="80px">
                    <Items>
                        <f:RadioButtonList runat="server" ID="rblReportType" Label="汇报类型" AutoPostBack="true" OnSelectedIndexChanged="rblReportType_SelectedIndexChanged">
                            <f:RadioItem Value="Start" Text="开工" />
                            <f:RadioItem Value="End" Text="完工" Selected="true" />
                            <f:RadioItem Value="FuZhu" Text="辅助" Enabled="false" />
                        </f:RadioButtonList>
                        <f:DropDownList runat="server" ID="ddlFCus" Label="企业名称" AutoPostBack="true" OnSelectedIndexChanged="ddlFCus_SelectedIndexChanged" />
                        <f:Panel runat="server" ShowBorder="false" ShowHeader="false" Layout="Column">
                            <Items>
                                <f:TextBox runat="server" ID="txtUserName" Label="员工姓名" ColumnWidth="50%" Readonly="true" />
                                <f:TextBox runat="server" ID="txtUserID" Label="员工工号" ColumnWidth="50%" CssClass="ColMargin" Readonly="true" />
                            </Items>
                        </f:Panel>
                        <f:Panel runat="server" ID="pnlWorkPlan" ShowBorder="false" ShowHeader="false" Layout="Column">
                            <Items>
                                <f:TextBox runat="server" ID="txtPlanNo" ColumnWidth="100%" Label="生产计划">
                                    <Listeners>
                                        <f:Listener Event="enter" Handler="onPlanNoEnter" />
                                    </Listeners>
                                </f:TextBox>
                                <f:Button runat="server" ID="btnScan" Text="扫描计划码" Width="100px" CssClass="ColMargin btnScan">
                                    <Listeners>
                                        <f:Listener Event="click" Handler="scanPlan" />
                                    </Listeners>
                                </f:Button>
                            </Items>
                        </f:Panel>
                        <f:TextBox runat="server" ID="txtBillNo" Label="订单号" Readonly="true" />
                        <f:TextBox runat="server" ID="txtModel" Label="规格型号" Readonly="true" />
                        <f:TextBox runat="server" ID="txtPlanQty" Label="计划数量" Readonly="true" CssClass="txtQty" />
                        <f:Panel runat="server" ShowBorder="false" ShowHeader="false" Layout="Column">
                            <Items>
                                <f:DropDownList runat="server" ID="ddlProcedure" ColumnWidth="100%" Label="生产工序" AutoPostBack="true" OnSelectedIndexChanged="ddlProcedure_SelectedIndexChanged" />
                                <f:Button runat="server" ID="btnScanProcedure" Text="扫描工序码" Width="100px" CssClass="ColMargin btnScan">
                                    <Listeners>
                                        <f:Listener Event="click" Handler="scanProcedure" />
                                    </Listeners>
                                </f:Button>
                            </Items>
                        </f:Panel>
                        <f:NumberBox runat="server" ID="txtSCQty" Label="生产数量" InputType="number" NoNegative="true" CssClass="txtQty" />
                        <f:Panel runat="server" ID="pnlNGQty" ShowBorder="false" ShowHeader="false" Layout="Column">
                            <Items>
                                <f:NumberBox runat="server" ID="txtBadQty" Label="不良数量" Text="0" ColumnWidth="50%" InputType="number" NoNegative="true" />
                                <f:NumberBox runat="server" ID="txtLossQty" Label="损耗数量" Text="0" ColumnWidth="50%" InputType="number" CssClass="ColMargin" NoNegative="true" />
                            </Items>
                        </f:Panel>
                        <f:TextBox runat="server" ID="txtRemark" Label="备注" />
                        <f:Button runat="server" ID="btnSubmit" Text="提交汇报" OnClick="btnSubmit_Click" Size="Large" CssClass="btnDefault" />
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
