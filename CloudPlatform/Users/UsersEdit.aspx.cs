using System;
using System.Collections.Generic;
using System.Data;
using FineUIPro;

namespace CloudPlatform.Users
{
    public partial class UsersEdit : PageBase
    {
        #region 初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 绑定关闭按钮前台事件
                btnClose.OnClientClick = ActiveWindow.GetHideReference();

                if (!string.IsNullOrEmpty(GetRequest("ID")))
                {
                    try
                    {
                        // 加载用户信息
                        DataRow drUser = op.ReadUserInfo_Login(GetRequest("ID")).Rows[0];
                        txtFUserID.Text = drUser["FUserID"].ToString();
                        txtFUserID.Enabled = false;
                        txtFUserName.Text = drUser["FUserName"].ToString();
                        txtFPassword.Text = eu.DEC_Decrypt(drUser["FPassword"].ToString());
                        rblFEnabled.SelectedValue = drUser["FEnabled"].ToString();

                        // 加载用户权限
                        DataTable dtPower = op.ReadUserPower(GetRequest("ID"));
                        // 遍历权限树一级节点
                        foreach (TreeNode node in treePower.Nodes)
                        {
                            // 判断用户权限数据中是否有该节点下的子节点权限
                            DataRow[] drs = dtPower.Select("FModule='" + node.Text + "'");
                            if (drs.Length > 0)
                            {
                                // 遍历权限树二级节点
                                foreach (TreeNode nodec in node.Nodes)
                                {
                                    for (int i = 0; i < drs.Length; i++)
                                    {
                                        if (nodec.Text == drs[i]["FMenuName"].ToString())
                                        {
                                            nodec.Checked = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        Alert.Show("用户信息读取失败", "异常", MessageBoxIcon.Error, ActiveWindow.GetHidePostBackReference());
                    }
                }
            }
        }
        #endregion

        #region 保存
        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool ok_info = false;

            if (string.IsNullOrEmpty(GetRequest("ID")))
            {
                // 添加用户信息
                ok_info = op.AddUser(txtFUserID.Text, txtFUserName.Text, eu.DEC_Encrypt(txtFPassword.Text.Trim()), rblFEnabled.SelectedValue);
            }
            else
            {
                // 更新用户信息
                ok_info = op.UpdateUser(txtFUserID.Text, txtFUserName.Text, eu.DEC_Encrypt(txtFPassword.Text.Trim()), rblFEnabled.SelectedValue);
            }

            // 添加用户权限
            if (ok_info)
            {
                try
                {
                    List<string> lPower = new List<string>();

                    TreeNode[] nodes = treePower.GetCheckedNodes();
                    if (nodes.Length > 0)
                    {
                        lPower.Add("DELETE FROM tUsersPower WHERE FUserID='" + txtFUserID.Text + "'");
                        foreach (TreeNode node in nodes)
                        {
                            if (node.AttributeDataTag != null)
                            {
                                string[] pInfo = node.AttributeDataTag.Split('|');
                                lPower.Add("INSERT INTO tUsersPower(FUserID,FModule,FMenuName,FMenuIcon,FMenuUrl)VALUES('" + txtFUserID.Text + "','" + pInfo[0] + "','" + node.Text + "','" + pInfo[1] + "','" + pInfo[2] + "')");
                            }
                        }

                        // 使用事务提交数据库
                        op.DoSqlTRAN(string.Join(";", lPower.ToArray()));
                    }

                    Alert.Show("保存成功", "成功", MessageBoxIcon.Success, ActiveWindow.GetHidePostBackReference());
                }
                catch (Exception ex)
                {
                    Alert.Show(ex.Message, MessageBoxIcon.Error);
                }
            }
            else
            {
                Alert.Show("保存失败", MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}