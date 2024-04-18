using FineUIPro;
using System;

namespace CloudPlatform.Platform
{
    public partial class Platform : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            gridPlatform.DataSource = op.GetPlatformList(txtSearch.Text.Trim());
            gridPlatform.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            winEdit.IFrameUrl = "PlatformEdit.aspx";
            winEdit.Hidden = false;
        }

        protected void txtSearch_TriggerClick(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void gridPlatform_RowCommand(object sender, FineUIPro.GridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                bool ok = op.DeletePlatform(GetGridCellValue(gridPlatform, "FID"));
                if (ok)
                {
                    ShowNotify("删除成功！", MessageBoxIcon.Success);
                    LoadData();
                }
                else
                {
                    ShowNotify("删除失败！", MessageBoxIcon.Error);
                }
            }
        }

        protected void winEdit_Close(object sender, FineUIPro.WindowCloseEventArgs e)
        {
            LoadData();
        }
    }
}