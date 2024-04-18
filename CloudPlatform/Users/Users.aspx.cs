using System;
using FineUIPro;

namespace CloudPlatform.Users
{
    public partial class Users : PageBase
    {
        #region 初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }
        #endregion

        #region 加载数据
        private void LoadData()
        {
            gridUsers.DataSource = op.ReadUserList(txtSearch.Text);
            gridUsers.DataBind();
        }
        #endregion

        #region 搜索
        protected void txtSearch_TriggerClick(object sender, EventArgs e)
        {
            LoadData();
        }
        #endregion

        #region 删除用户
        protected void gridUsers_RowCommand(object sender, GridCommandEventArgs e)
        {

        }
        #endregion

        #region 编辑窗口关闭
        protected void winEdit_Close(object sender, WindowCloseEventArgs e)
        {
            LoadData();
        }
        #endregion
    }
}