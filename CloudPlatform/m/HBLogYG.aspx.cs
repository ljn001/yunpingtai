using FineUIPro;
using CloudPlatform.Mobile;
using System;
using System.Data;

namespace CloudPlatform.m
{
    public partial class HBLogYG : PageBase
    {
        CheckWXOpenID ckWXID = new CheckWXOpenID();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblWXOpenID.Text = ckWXID.Check(Request.Url.AbsoluteUri);

                // 根据OpenID获取绑定企业信息
                DataTable dtCus = op.GetCustomerUserBind(lblWXOpenID.Text);
                if (dtCus.Rows.Count > 0)
                {
                    DataRow drCus = dtCus.Rows[0];
                    DataTable dtCusdb = op.GetCustomerUserBindDB(dtCus.Rows[0]["bumendaihao"].ToString());

                    if (dtCusdb.Rows.Count > 0)
                    {
                        string connectionString = dtCusdb.Rows[0]["FDBConnStr"].ToString();

                        //     string connectionString = drCus["FDBConnStr"].ToString();
                        lblDBConnStr.Text = eu.DEC_Encrypt(connectionString);
                        // 读取客户企业用户信息
                        DataTable dtCusUser = opYG.GetUserInfo(drCus["FCusUserID"].ToString(), connectionString);
                    if (dtCusUser.Rows.Count > 0)
                    {
                     

                      
                            DataRow drPower = dtCusUser.Rows[0];
                            // 权限校验
                            if (drPower["FIsLeave"].ToString() == "0")
                            {
                                lvHBLog.DataSource = opYG.GetHuiBaoLog(drCus["FStaffNo"].ToString(), DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd"), eu.DEC_Decrypt(lblDBConnStr.Text));
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
                string FID = PDInfo[1]; //计划单号（辅助工序为“[辅助工序]”）
                

                bool ok;
               
                ok = opYG.DeleteProductionData(FID, eu.DEC_Decrypt(lblDBConnStr.Text));
                

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