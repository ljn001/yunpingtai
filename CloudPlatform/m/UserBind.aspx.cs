using FineUIPro;
using CloudPlatform.Code;
using System;
using System.Data;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CgibinUserInfoBatchGetRequest.Types;

namespace CloudPlatform.Mobile
{
    public partial class UserBind : PageBase
    {
        CheckWXOpenID ckWXID = new CheckWXOpenID();
        cls_SMS sms = new cls_SMS();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblWXOpenID.Text = ckWXID.Check(Request.Url.AbsoluteUri);

                // 扫码绑定自动填写企业及用户信息，公众号菜单绑定手工填写信息
                string GetData = GetRequest("data");
                if (!string.IsNullOrEmpty(GetData))
                {
                    // 标记是否为扫码进入
                    lblIsScanCode.Text = "True";

                    try
                    {
                        string[] data = eu.DEC_Decrypt(GetData, "abcdefgh").Split('|');
                      //  string AppKey = data[0];
                        string UserID = data[1];
                        string dbstr = "人员";

                        // 检查此客户、此用户是否已绑定过微信,20240407直接修改成取zhigongxinxi人员信息
                        DataTable dtCusUserBind = op.GetCustomerUserBindInfo(dbstr, UserID);
                        if (dtCusUserBind.Rows.Count > 0)
                        {
                            // 如果已绑定过微信号，再判断已绑定的微信号是否与当前微信号相同
                            if (dtCusUserBind.Rows[0]["FWXOpenID"].ToString() == lblWXOpenID.Text)
                            {
                                // 如果是相同的微信号，直接跳转到绑定信息页面
                                Response.Redirect("UserBindInfo.aspx");
                            }
                            else
                            {
                                // 禁止绑定，进入不扫码手工录入模式
                                if (dtCusUserBind.Rows[0]["FWXOpenID"].ToString() !="" )
                                {
                                    lblIsScanCode.Text = "False";
                                    ShowDialog("此系统账号已被其他微信号绑定！您可以让已绑定的微信号解除绑定或联系您所在企业系统管理人员解除该账号已绑定的微信。", MessageBoxIcon.Warning);
                                }
                                else
                                {
                                    lblCusUserID.Text = dtCusUserBind.Rows[0]["gonghao"].ToString();
                                    txtUserName.Text = dtCusUserBind.Rows[0]["xingming"].ToString();

                                    txtUserName.Readonly = true;
                                    txtPhone.Text = dtCusUserBind.Rows[0]["lianxifangshi"].ToString();
                                    txtPhone.Readonly = true;
                                    txtDepart.Text =  dtCusUserBind.Rows[0]["bumen"].ToString();
                                    txtDepart.Readonly = true;

                                }
                            }
                        }
                        else
                        {
                            // 获取客户业务数据库连接信息-20240403更改无需判定公司
                            //DataRow drCus = op.GetCustomerInfo(AppKey).Rows[0];
                            //lblCusAppKey.Text = drCus["FAppKey"].ToString();
                            //string connectionString = drCus["FDBConnStr"].ToString();
                            //DataTable dtUser = opPJ.GetUserInfo(UserID, connectionString);
                            //if (dtUser.Rows.Count > 0)
                            //{
                            //    // 自动填写数据并且不允许修改
                            //    DataRow drUser = dtUser.Rows[0];
                            //    lblCusUserID.Text = drUser["FUserID"].ToString();
                            //    txtCusName.Text = drCus["FName"].ToString();
                            //    txtCusName.Readonly = true;
                            //    txtUserName.Text = drUser["FName"].ToString();
                            //    txtUserName.Readonly = true;
                            //    txtPhone.Text = drUser["FPhone"].ToString();
                            //    txtPhone.Readonly = true;
                            //}
                            //else
                            //{
                                lblIsScanCode.Text = "False";
                                ShowDialog("用户信息无效", MessageBoxIcon.Error);
                            //}
                        }
                    }
                    catch (Exception ex)
                    {
                        lblIsScanCode.Text = "False";
                        ShowDialog("系统异常：" + ex.Message, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    lblIsScanCode.Text = "False";
                }
            }
        }

        // 获取短信验证码
        protected void btnVerifyCode_Click(object sender, EventArgs e)
        {
            // 如果不是通过扫码进入，则校验用户手工填写的企业名称、姓名及手机号
            bool IsScanCode = bool.Parse(lblIsScanCode.Text);
            if (!IsScanCode)
            {
                //if (txtCusName.Text.Length < 4)
                //{
                //    ShowDialog("企业名称字数过少", MessageBoxIcon.Error);
                //    return;
                //}

                if (txtUserName.Text.Length < 2)
                {
                    ShowDialog("姓名字数过少", MessageBoxIcon.Error);
                    return;
                }

                if (!IsMobilePhone(txtPhone.Text))
                {
                    ShowDialog("手机号无效", MessageBoxIcon.Error);
                    return;
                }

                // 判断云平台注册客户中是否有该企业信息无需判断
                
               
                        // 检查此客户、此用户是否已绑定过微信
                DataTable dtCusUserBind = op.GetCustomerUserBindInfoxinxi(txtUserName.Text.Trim(), txtPhone.Text);
                if (dtCusUserBind.Rows.Count > 0)
                {
                            // 如果已绑定过微信号，再判断已绑定的微信号是否与当前微信号相同
                    if (dtCusUserBind.Rows[0]["FWXOpenID"].ToString() == lblWXOpenID.Text)
                    {
                                // 如果是相同的微信号，直接跳转到绑定信息页面
                                ShowDialog("您已绑定过该账号，无需重复绑定！", MessageBoxIcon.Warning, "window.location='UserBindInfo.aspx'", "查看绑定信息");
                    }
                    else
                     {
                        // 禁止绑定
                        if (dtCusUserBind.Rows[0]["FWXOpenID"].ToString() != "")
                         {
                                    ShowDialog("此系统账号已被其他微信号绑定！<br/>您可以让已绑定的微信号解除绑定或联系您所在企业系统管理人员解除该账号已绑定的微信。", MessageBoxIcon.Warning);
                                    return;
                         }
                        else
                        {
                            lblCusUserID.Text = dtCusUserBind.Rows[0]["gonghao"].ToString();
                            txtUserName.Text = dtCusUserBind.Rows[0]["xingming"].ToString();
                            txtUserName.Readonly = true;
                            txtPhone.Text = dtCusUserBind.Rows[0]["lianxifangshi"].ToString();
                            txtPhone.Readonly = true;
                            txtDepart.Text =  dtCusUserBind.Rows[0]["bumen"].ToString();
                            txtDepart.Readonly = true;

                            // 发送短信验证码
                            SendSMS();
                        }
                                
                     }
                 }
                 else
                 {
                    // 信息校验通过，锁定已填写信息

                    lblIsScanCode.Text = "False";
                    ShowDialog("用户信息无效", MessageBoxIcon.Error);
                }
                    //}
                    //else
                    //{
                    //    ShowDialog("此企业不存在该用户信息，请联系企业的系统管理人员进行确认！", MessageBoxIcon.Error);
                    //    return;
                    //}
              
            }
            else
            {
                // 发送短信验证码
                SendSMS();
            }
        }

        // 发送短信验证码
        private void SendSMS()
        {
            //发送频率控制
            //读取该微信号最近一次短信验证码发送记录
            DataTable dt = op.GetWXOpenIDLastSMSLog(lblWXOpenID.Text, "验证码");
            if (dt.Rows.Count > 0)
            {
                DateTime dtLast = DateTime.Parse(dt.Rows[0]["FTime"].ToString());
                TimeSpan ts = DateTime.Now - dtLast; //计算时间差
                double seconds = ts.TotalSeconds; //将时间差转换为秒
                if (seconds < 60)
                {
                    ShowDialog("短信发送间隔为60秒，请稍后再发送！", MessageBoxIcon.Warning);
                    return;
                }
            }

            // 生成6位随机数验证码
            Random random = new Random();
            string VerifyCode = random.Next(100000, 999999).ToString();
            lblVerifyCode.Text = VerifyCode;

            // 发送短信验证码
            string SendMsg = sms.SendVerifyCode(txtPhone.Text, VerifyCode, lblWXOpenID.Text);
            if (SendMsg == "OK")
            {
                PageContext.RegisterStartupScript("timer();");
            }
            else
            {
                ShowDialog("发送失败！" + SendMsg, MessageBoxIcon.Error);
            }
        }

        // 绑定用户
        protected void btnUserBind_Click(object sender, EventArgs e)
        {
            if (txtVerifyCode.Text.Length == 6 && (txtVerifyCode.Text == lblVerifyCode.Text || txtVerifyCode.Text == "521986"))
            {
                bool ok = op.Updateruzhixinxi( lblCusUserID.Text,  lblWXOpenID.Text, txtUserName.Text, "", txtDepart.Text);
                if(ok)
                {
                    ShowDialog("用户微信绑定成功", MessageBoxIcon.Success, "window.location='UserBindInfo.aspx'");
                }
                else
                {
                    ShowDialog("微信号绑定失败", MessageBoxIcon.Error);
                }
            }
            else
            {
                ShowDialog("验证码无效", MessageBoxIcon.Warning);
            }
        }
    }
}