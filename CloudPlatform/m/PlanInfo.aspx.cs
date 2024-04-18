using FineUIPro;
using CloudPlatform.Code.wx;
using CloudPlatform.Mobile;
using System;
using System.Data;
using SKIT.FlurlHttpClient.Wechat.Api;
using System.Web.Configuration;
using System.Text;
using System.Web;

namespace CloudPlatform.m
{
    public partial class PlanInfo : PageBase
    {
        CheckWXOpenID ckWXID = new CheckWXOpenID();
        WXApi wx = new WXApi();

        public string wxJsSdkConfig { get; set; } //微信JS-SDK配置信息

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 获取用户OpenID
                lblWXOpenID.Text = ckWXID.Check(Request.Url.AbsoluteUri);

                // 获取微信AccessToken
                string AccessToken = wx.GetAccessToken();

                //获取Ticket
                string Ticket = wx.GetTicket(AccessToken);

                if (string.IsNullOrWhiteSpace(Ticket))
                {
                    ShowDialog("无效的Ticket", MessageBoxIcon.Error);
                    return;
                }

                #region 生成调用微信JS-SDK的配置信息
                var options = new WechatApiClientOptions()
                {
                    AppId = WebConfigurationManager.AppSettings["appid"].ToString(),
                    AppSecret = WebConfigurationManager.AppSettings["appsecret"].ToString(),
                    MidasAppKey = "", //米大师相关服务 AppKey，不用则不填
                    ImmeDeliveryAppKey = "", //即时配送相关服务 AppKey，不用则不填
                    ImmeDeliveryAppSecret = "" //即时配送相关服务 AppSecret，不用则不填
                };
                var client = new WechatApiClient(options);

                var paramMap = client.GenerateParametersForJSSDKConfig(Ticket, Request.Url.ToString());

                StringBuilder sb = new StringBuilder();
                sb.Append("wx.config({");
                sb.Append("debug: false,"); //开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印
                sb.Append("appId: '" + paramMap["appId"] + "',"); //必填，公众号的唯一标识
                sb.Append("timestamp: " + paramMap["timestamp"] + ","); //必填，生成签名的时间戳
                sb.Append("nonceStr: '" + paramMap["nonceStr"] + "',"); //必填，生成签名的随机串
                sb.Append("signature: '" + paramMap["signature"] + "',"); //必填，签名
                sb.Append("jsApiList: [\"scanQRCode\"]"); //必填，需要使用的JS接口列表
                sb.Append("});");
                wxJsSdkConfig = sb.ToString();
                #endregion

                // 根据OpenID获取绑定企业信息
                DataTable dtCus = op.GetCustomerUserBindDefault(lblWXOpenID.Text);
                if (dtCus.Rows.Count > 0)
                {
                    DataRow drCus = dtCus.Rows[0];

                    string connectionString = drCus["FDBConnStr"].ToString();
                    lblDBConnStr.Text = eu.DEC_Encrypt(connectionString);

                    // 读取客户企业用户信息
                    DataTable dtPower = opPJ.GetUserPower(drCus["FCusUserID"].ToString(), connectionString);
                    if (dtPower.Rows.Count > 0)
                    {
                        DataRow drPower = dtPower.Rows[0];
                        // 权限校验
                        if (drPower["FState"].ToString() == "启用" && drPower["FPower"].ToString().IndexOf("wxPlanInfo") >= 0)
                        {

                        }
                        else
                        {
                            ShowDialog("无操作权限", MessageBoxIcon.Warning, "window.location='sczx.aspx'", "返回生产中心");
                        }
                    }
                    else
                    {
                        ShowDialog("无操作权限", MessageBoxIcon.Warning, "window.location='sczx.aspx'", "返回生产中心");
                    }
                }
                else
                {
                    // 如果此微信号未绑定任何企业，则直接跳转至绑定页面
                    ShowDialog("此微信尚未绑定任何企业账号，请绑定后再操作！", MessageBoxIcon.Warning, "window.location='UserBind.aspx'", "去绑定");
                }
            }
        }

        protected void pmMain_CustomEvent(object sender, CustomEventArgs e)
        {
            if (e.EventArgument.StartsWith("scanPlan|")) //扫描生产计划码
            {
                string PlanCode = GetWXScanCode(e.EventArgument);

                if (PlanCode.ToUpper().StartsWith("JH") || PlanCode.ToUpper().StartsWith("PL"))
                {
                    lblWorkPlanID.Text = Convert.ToInt32(PlanCode.Substring(2, PlanCode.Length - 2)).ToString();

                    // 读取生产计划详情
                    string DBConnStr = eu.DEC_Decrypt(lblDBConnStr.Text);
                    DataTable dt = opPJ.RptPlanInfo(lblWorkPlanID.Text, DBConnStr);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        txtBillNo.Text = dr["FOrderID"].ToString();
                        txtPlanNo.Text = dr["FPlanCode"].ToString();
                        txtModel.Text = dr["FModel"].ToString();
                        txtPlanQty.Text = dr["FCounts"].ToString();
                        txtFuTu.Text = dr["FFuTu"].ToString();
                        lblMTONo.Text = dr["FMTONo"].ToString();

                        if (!string.IsNullOrWhiteSpace(dr["FFinish"].ToString()))
                        {
                            // 2023-07-29前，只显示完工工序名称
                            //lblProc.Text = dr["FFinish"].ToString().Replace("[", "<span class=\"tag_g\">").Replace("]", "</span>").Replace(",", "");

                            // 显示完工工序（带汇报人及汇报时间）
                            DataTable dtFinishProc = opPJ.RptPlanFinishProcList(lblWorkPlanID.Text, DBConnStr);
                            if (dtFinishProc.Rows.Count > 0)
                            {
                                StringBuilder sbFinishProc = new StringBuilder();
                                foreach (DataRow drFinishProc in dtFinishProc.Rows)
                                {
                                    sbFinishProc.Append("<span class=\"tag_g\">" + drFinishProc["FProcName"] + "<span class=\"tag_worker\">" + drFinishProc["Worker"] + "</span></span>");
                                }

                                lblProc.Text = sbFinishProc.ToString();
                            }
                            
                        }

                        if (!string.IsNullOrEmpty(dr["FNotFinish"].ToString()))
                        {
                            lblProc.Text += dr["FNotFinish"].ToString().Replace("[", "<span class=\"tag_r\">").Replace("]", "</span>").Replace(",", "");
                        }
                    }
                    else
                    {
                        ShowDialog("未查询到生产计划信息");
                    }
                }
                else
                {
                    ShowDialog("无效的生产计划码或不支持的生产计划类型");
                }
            }
            else if(e.EventArgument == "Download_OK") //下载附图
            {
                string FuTuDownloadUrl = "http://61.133.118.6:10088";
                string fileUrl = FuTuDownloadUrl + "/Download.aspx?No=" + HttpUtility.UrlEncode(lblMTONo.Text) + "&Futu=" + HttpUtility.UrlEncode(txtFuTu.Text);
                Response.Redirect(fileUrl);

                // 经测试无效的方法
                //string FileName = Guid.NewGuid().ToString("N");

                //WebClient wc = new WebClient();
                //wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                //// 设置附图文件下载地址
                //string FuTuDownloadUrl = "http://61.133.118.6:10088";

                //string fileUrl = FuTuDownloadUrl + "/Download.aspx?No=" + System.Web.HttpUtility.UrlEncode(lblMTONo.Text) + "&Futu=" + System.Web.HttpUtility.UrlEncode(txtFuTu.Text);
                //wc.DownloadFile(new Uri(fileUrl), Server.MapPath("DownLoadFuTu/" + FileName + ".xls"));

                ////以字符流的形式下载文件
                //FileStream fs = new FileStream(Server.MapPath("DownLoadFuTu/" + FileName + ".xls"), FileMode.Open);
                //byte[] bytes = new byte[(int)fs.Length];
                //fs.Read(bytes, 0, bytes.Length);
                //fs.Close();
                //Response.ContentType = "application/octet-stream";
                ////通知浏览器下载文件而不是打开
                //Response.AddHeader("Content-Disposition", "attachment;  filename=" + HttpUtility.UrlEncode(FileName + ".xls", System.Text.Encoding.UTF8));
                //Response.BinaryWrite(bytes);
                //Response.Flush();
                //Response.End();
            }
        }

        // 查看附图
        protected void btnDownloadFuTu_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(Confirm.GetShowReference("确定下载附图文件吗？",
                    String.Empty,
                    MessageBoxIcon.Question,
                    pmMain.GetCustomEventReference(false, "Download_OK"), // 第一个参数 false 用来指定当前不是AJAX请求
                    pmMain.GetCustomEventReference("Download_Cancel")));
        }

        protected void btnScan_Click(object sender, EventArgs e)
        {

        }
    }
}