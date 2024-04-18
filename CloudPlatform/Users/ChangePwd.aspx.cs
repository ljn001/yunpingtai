using FineUIPro;
using System;

namespace CloudPlatform.Users
{
    public partial class ChangePwd : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 为关闭按钮绑定客户端事件
                btnClose.OnClientClick = ActiveWindow.GetHideReference();
            }
        }

        // 保存
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPwdOld.Text) || string.IsNullOrEmpty(txtPwdNew.Text) || string.IsNullOrEmpty(txtPwdRe.Text))
            {
                Alert.Show("请输入完整的密码信息", MessageBoxIcon.Error);
                return;
            }
            else if (txtPwdNew.Text.Trim() != txtPwdRe.Text.Trim())
            {
                Alert.Show("两次输入的新密码不一致", MessageBoxIcon.Error);
                return;
            }
            else
            {
                // 校验原始密码是否正确
                try
                {
                    // 从数据库读取用户原始密码并解密
                    string OldPwd = eu.DEC_Decrypt(op.ReadUserInfo_Login(GetUserID()).Rows[0]["FPassword"].ToString());
                    if (txtPwdOld.Text.Trim() == OldPwd)
                    {
                        // 修改数据库密码
                        bool ok = op.ChangeUserPassword(GetUserID(), eu.DEC_Encrypt(txtPwdNew.Text.Trim()));
                        if (ok)
                        {
                            // 提示用户修改成功，用户确定后自动关闭窗口
                            Alert.Show("密码修改成功", "成功", MessageBoxIcon.Success, ActiveWindow.GetHideReference());
                        }
                        else
                        {
                            Alert.Show("密码修改失败", MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        Alert.Show("原始密码输入错误", MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    Alert.Show(ex.Message, MessageBoxIcon.Error);
                }
            }
        }
    }
}