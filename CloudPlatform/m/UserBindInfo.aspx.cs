using System;
using System.Data;
using FineUIPro;

namespace CloudPlatform.Mobile
{
    public partial class UserBindInfo : PageBase
    {
        CheckWXOpenID ckWXID = new CheckWXOpenID();

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                lblWXOpenID.Text = ckWXID.Check(Request.Url.AbsoluteUri);

                DataTable dt = op.GetCustomerUserBind(lblWXOpenID.Text);
                lvBindList.DataSource = dt;
                lvBindList.DataBind();
            }
        }

        protected void btnDeleteBind_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string BindID = eu.DEC_Decrypt(btn.AttributeDataTag.ToString());
            bool ok = op.DeleteCustomerUser(BindID);
            if(ok)
            {
                Response.Redirect(Request.Url.ToString());
            }
            else
            {
                ShowDialog("账号解除绑定失败", MessageBoxIcon.Error);
            }
        }

        // 设置默认单位
        protected void btnDefault_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string BindID = eu.DEC_Decrypt(btn.AttributeDataTag.ToString());

            bool ok = op.SetCustomerUserDefault(lblWXOpenID.Text, BindID);
            if(ok)
            {
                Response.Redirect(Request.Url.ToString());
            }
            else
            {
                ShowDialog("默认单位设置失败", MessageBoxIcon.Error);
            }
        }
    }
}