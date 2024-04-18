using System;
using FineUIPro;
using System.Web;
using System.Data;

namespace CloudPlatform
{
    public partial class Default : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserID.Text))
            {
                Alert.Show("请填写用户名");
            }
            else if (string.IsNullOrEmpty(txtPassword.Text))
            {
                Alert.Show("请填写登录密码");
            }
            else
            {
                DataTable dtUser = op.ReadUserInfo_Login(txtUserID.Text);
                if (dtUser.Rows.Count > 0)
                {
                    if (!bool.Parse(dtUser.Rows[0]["FEnabled"].ToString()))
                    {
                        Alert.Show("此用户已禁用");
                    }
                    else
                    {
                        if (eu.DEC_Encrypt(txtPassword.Text) == Convert.ToString(dtUser.Rows[0]["FPassword"]))
                        {
                            // 向Cookie中添加UserID
                            HttpCookie cookies = new HttpCookie("SkyLink_USERID", eu.DEC_Encrypt(dtUser.Rows[0]["FUserID"].ToString()));
                            Response.Cookies.Add(cookies);

                            Response.Redirect("Main.aspx");
                        }
                        else
                        {
                            Alert.Show("用户密码错误");
                        }
                    }
                }
                else
                {
                    Alert.Show("系统不存在此用户");
                }
            }
        }
    }
}