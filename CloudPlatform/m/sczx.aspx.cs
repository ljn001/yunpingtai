using System;
using System.Data;
using System.Text;
using FineUIPro;
using CloudPlatform.Mobile;

namespace CloudPlatform.m
{
    public partial class sczx : PageBase
    {
        CheckWXOpenID ckWXID = new CheckWXOpenID();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 获取用户OpenID
                lblWXOpenID.Text = ckWXID.Check(Request.Url.AbsoluteUri);

                // 根据OpenID获取绑定企业信息
                DataTable dtCus = op.GetCustomerUserBindDefault(lblWXOpenID.Text);
                if (dtCus.Rows.Count > 0)
                {
                    DataRow drCus = dtCus.Rows[0];

                    lblUserName.Text = drCus["xingming"].ToString();
                    lblCusName.Text = drCus["bumen"].ToString();

                    // 获取客户公司数据库连接地址
                    
                    DataTable dtCusdb = op.GetCustomerUserBindDB(drCus["bumendaihao"].ToString());
                   

                    if (dtCusdb.Rows.Count > 0)
                    {
                        DataRow drCusdb = dtCusdb.Rows[0];
                        string connectionString = drCusdb["FDBConnStr"].ToString();
                        lblDBConnStr.Text = eu.DEC_Encrypt(connectionString);

                        // 获取用户权限（生产系统）
                        DataTable dtPower = null;
                        if (drCus["bumendaihao"].ToString() == "009")
                            dtPower = opYS.GetUserPower(drCus["gonghao"].ToString(), connectionString);
                        else
                            dtPower = opPJ.GetUserPower(drCus["gonghao"].ToString(), connectionString);
                        if (dtPower.Rows.Count > 0 && dtPower.Rows[0]["FState"].ToString() == "启用")
                        {
                            string strPower = dtPower.Rows[0]["FPower"].ToString();

                            StringBuilder sb = new StringBuilder();

                            // 根据权限拼接用户可用菜单项
                            // 汇报记录
                            if (strPower.IndexOf("wxHuiBaoys") >= 0)
                            {
                                sb.Append("<div class=\"col-4 col-md-2 mt-1 mb-1\">");
                                sb.Append("<a href=\"bgys\">");
                                sb.Append("<div class=\"icon icon-70 rounded-circle mb-1 bg-default-light text-default\">");
                                sb.Append("<img src=\"img/hbjl.png\" style=\"width:60%;\" />");
                                sb.Append("</div>");
                                sb.Append("<p class=\"text-secondary\"><small>印刷厂报工</small></p>");
                                sb.Append("</a>");
                                sb.Append("</div>");
                            }
                            if (strPower.IndexOf("wxHuiBaoLogys") >= 0)
                            {
                                sb.Append("<div class=\"col-4 col-md-2 mt-1 mb-1\">");
                                sb.Append("<a href=\"HBLogys\">");
                                sb.Append("<div class=\"icon icon-70 rounded-circle mb-1 bg-default-light text-default\">");
                                sb.Append("<img src=\"img/hbjl.png\" style=\"width:60%;\" />");
                                sb.Append("</div>");
                                sb.Append("<p class=\"text-secondary\"><small>印刷汇报记录</small></p>");
                                sb.Append("</a>");
                                sb.Append("</div>");
                            }

                            // 计划查询
                            if (strPower.IndexOf("wxPlanInfoys") >= 0)
                            {
                                sb.Append("<div class=\"col-4 col-md-2 mt-1 mb-1\">");
                                sb.Append("<a href=\"PlanInfoys\">");
                                sb.Append("<div class=\"icon icon-70 rounded-circle mb-1 bg-default-light text-default\">");
                                sb.Append("<img src=\"img/jhcx.png\" style=\"width:72%;\" />");
                                sb.Append("</div>");
                                sb.Append("<p class=\"text-secondary\"><small>印刷计划查询</small></p>");
                                sb.Append("</a>");
                                sb.Append("</div>");
                            }
                            //渔竿工厂 
                            if (strPower.IndexOf("wxHuiBaoyg") >= 0)
                            {
                                sb.Append("<div class=\"col-4 col-md-2 mt-1 mb-1\">");
                                sb.Append("<a href=\"bgyg\">");
                                sb.Append("<div class=\"icon icon-70 rounded-circle mb-1 bg-default-light text-default\">");
                                sb.Append("<img src=\"img/hbjl.png\" style=\"width:60%;\" />");
                                sb.Append("</div>");
                                sb.Append("<p class=\"text-secondary\"><small>工序报工</small></p>");
                                sb.Append("</a>");
                                sb.Append("</div>");
                            }
                            if (strPower.IndexOf("wxHuiBaoLogyg") >= 0)
                            {
                                sb.Append("<div class=\"col-4 col-md-2 mt-1 mb-1\">");
                                sb.Append("<a href=\"HBLogyg\">");
                                sb.Append("<div class=\"icon icon-70 rounded-circle mb-1 bg-default-light text-default\">");
                                sb.Append("<img src=\"img/hbjl.png\" style=\"width:60%;\" />");
                                sb.Append("</div>");
                                sb.Append("<p class=\"text-secondary\"><small>汇报记录</small></p>");
                                sb.Append("</a>");
                                sb.Append("</div>");
                            }

                            // 计划查询
                            if (strPower.IndexOf("wxPlanInfoyg") >= 0)
                            {
                                sb.Append("<div class=\"col-4 col-md-2 mt-1 mb-1\">");
                                sb.Append("<a href=\"PlanInfoyg\">");
                                sb.Append("<div class=\"icon icon-70 rounded-circle mb-1 bg-default-light text-default\">");
                                sb.Append("<img src=\"img/jhcx.png\" style=\"width:72%;\" />");
                                sb.Append("</div>");
                                sb.Append("<p class=\"text-secondary\"><small>计划查询</small></p>");
                                sb.Append("</a>");
                                sb.Append("</div>");
                            }
                            // 如果StringBuilder长度为0（没有任何权限）
                            if (sb.Length == 0)
                            {
                                sb.Append("<div class=\"info\"><img src=\"img/info.png\" width=\"80\" /><br /><br />此账号尚未授权任何功能</div>");
                            }

                            lblMenu.Text = sb.ToString();
                        }
                        else
                        {
                            lblMenu.Text = "<div class=\"info\"><img src=\"img/info.png\" width=\"80\" /><br /><br />此账号尚未授权任何功能</div>";
                        }
                    }
                    else
                    {
                        // 如果此微信号未绑定任何企业，则直接跳转至绑定页面
                        ShowDialog("此微信尚未绑定任何企业账号，请绑定后再操作！", MessageBoxIcon.Warning, "window.location='UserBind.aspx'", "联系管理员");
                    }
            }
            }
        }
    }
}