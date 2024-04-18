using FineUIPro;
using CloudPlatform.Mobile;
using System;
using System.Data;

namespace CloudPlatform.m
{
    public partial class HBLog : PageBase
    {
        CheckWXOpenID ckWXID = new CheckWXOpenID();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblWXOpenID.Text = ckWXID.Check(Request.Url.AbsoluteUri);

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
                        if (drPower["FState"].ToString() == "启用" && drPower["FPower"].ToString().IndexOf("wxHuiBaoLog") >= 0)
                        {
                            lvHBLog.DataSource = opPJ.GetHuiBaoLog(drCus["FCusUserID"].ToString(), DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"), connectionString);
                            lvHBLog.DataBind();
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
            if (e.EventArgument.StartsWith("delete|")) //删除汇报记录
            {
                string[] PDInfo = e.EventArgument.Split('|');
                string PlanNo = PDInfo[1]; //计划单号（辅助工序为“[辅助工序]”）
                string PDID = PDInfo[2];

                bool ok;
                if (PlanNo == "[辅助工序]")
                {
                    ok = opPJ.DeleteProductionData_FuZhu(PDID, eu.DEC_Decrypt(lblDBConnStr.Text));
                }
                else
                {
                    ok = opPJ.DeleteProductionData(PDID, eu.DEC_Decrypt(lblDBConnStr.Text));
                }

                if (ok)
                {
                    Alert.Show("工序汇报已删除", "成功", MessageBoxIcon.Success, "javascript:location.reload();");
                }
                else
                {
                    Alert.Show("工序汇报删除失败", MessageBoxIcon.Error);
                }
            }
        }
    }
}