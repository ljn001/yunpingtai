using FineUIPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CloudPlatform.Customer
{
    public partial class Customer : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            gridCustomer.DataSource = op.GetCustomerList(txtSearch.Text);
            gridCustomer.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            winEdit.IFrameUrl = "CustomerEdit.aspx";
            winEdit.Hidden = false;
        }

        protected void txtSearch_TriggerClick(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void gridCustomer_RowCommand(object sender, FineUIPro.GridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                bool ok = op.DeleteCustomer(GetGridCellValue(gridCustomer, "FAppKey"));
                if (ok)
                {
                    LoadData();
                    Alert.Show("删除成功！", MessageBoxIcon.Success);
                }
                else
                {
                    Alert.Show("删除失败！", MessageBoxIcon.Error);
                }
            }
        }

        protected void winEdit_Close(object sender, FineUIPro.WindowCloseEventArgs e)
        {
            LoadData();
        }
    }
}