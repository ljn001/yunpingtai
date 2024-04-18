using FineUIPro;
using System;
using System.Data;

namespace CloudPlatform.Platform
{
    public partial class PlatformEdit : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 绑定关闭按钮前台事件
                btnClose.OnClientClick = ActiveWindow.GetHideReference();

                lblFID.Text = GetRequest("ID");

                if (!string.IsNullOrEmpty(lblFID.Text))
                {
                    try
                    {
                        DataRow dr = op.GetPlatformInfo(lblFID.Text).Rows[0];
                        txtFName.Text = dr["FName"].ToString();
                        txtFNameShort.Text = dr["FNameShort"].ToString();
                        ddlFType.SelectedValue = dr["FType"].ToString();
                        txtFAPIUrl.Text = dr["FAPIUrl"].ToString();
                        txtFAppKey.Text = dr["FAppKey"].ToString();
                        txtFAppSecret.Text = dr["FAppSecret"].ToString();
                        txtFDBName.Text = dr["FDBName"].ToString();
                        txtFDBLoginID.Text = dr["FDBLoginID"].ToString();
                        txtFDBPwd.Text = dr["FDBPwd"].ToString();
                        try
                        {
                            cbxFEnabled.Checked = bool.Parse(dr["FEnabled"].ToString());
                        }
                        catch { }
                    }
                    catch
                    {
                        Alert.Show("数据读取错误！", "异常", MessageBoxIcon.Error, ActiveWindow.GetHidePostBackReference());
                    }
                }
            }
        }

        // 变更平台接入类型
        protected void ddlFType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFType.SelectedValue == "API")
            {
                txtFAPIUrl.Hidden = false;
                txtFAppKey.Hidden = false;
                txtFAppSecret.Hidden = false;
                txtFDBName.Hidden = true;
                txtFDBLoginID.Hidden = true;
                txtFDBPwd.Hidden = true;
            }
            else
            {
                txtFDBName.Hidden = false;
                txtFDBLoginID.Hidden = false;
                txtFDBPwd.Hidden = false;
                txtFAPIUrl.Hidden = true;
                txtFAppKey.Hidden = true;
                txtFAppSecret.Hidden = true;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(lblFID.Text))
            {
                bool ok = op.AddPlatform(txtFName.Text, txtFNameShort.Text, ddlFType.SelectedValue, txtFAPIUrl.Text, txtFAppKey.Text, txtFAppSecret.Text, txtFDBName.Text, txtFDBLoginID.Text, txtFDBPwd.Text, cbxFEnabled.Checked.ToString());
                if (ok)
                {
                    Alert.Show("保存成功！", "成功", MessageBoxIcon.Success, ActiveWindow.GetHidePostBackReference());
                }
                else
                {
                    Alert.Show("保存失败！", MessageBoxIcon.Error);
                }
            }
            else
            {
                bool ok = op.UpdatePlatform(lblFID.Text, txtFName.Text, txtFNameShort.Text, ddlFType.SelectedValue, txtFAPIUrl.Text, txtFAppKey.Text, txtFAppSecret.Text, txtFDBName.Text, txtFDBLoginID.Text, txtFDBPwd.Text, cbxFEnabled.Checked.ToString());
                if (ok)
                {
                    Alert.Show("保存成功！", "成功", MessageBoxIcon.Success, ActiveWindow.GetHidePostBackReference());
                }
                else
                {
                    Alert.Show("保存失败！", MessageBoxIcon.Error);
                }
            }
        }
    }
}