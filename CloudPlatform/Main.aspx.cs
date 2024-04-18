using System;
using System.Data;
using System.Linq;

namespace CloudPlatform
{
    public partial class Main : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    DataTable dtUser = op.ReadUserInfo_Login(GetUserID());
                    btnUser.Text = dtUser.Rows[0]["FUserName"].ToString();
                }
                catch
                {
                    Response.Redirect("Default.aspx");
                }

                DataTable dtUserPower = op.ReadUserPower(GetUserID());

                //抽取DataTable中的特定列组合成新的DataTable
                DataTable dat = dtUserPower.DefaultView.ToTable(false, new string[] { "FModule" });
                DataView dv = new DataView(dat);
                //去除dv中的重复行并组合成新的dt2,dt2就是去除重复行后的DataTable
                DataTable dtUserPowerModule = dv.ToTable(true, "FModule");

                for (int i = 0; i < dtUserPowerModule.Rows.Count; i++)
                {
                    FineUIPro.TreeNode tn = new FineUIPro.TreeNode();
                    tn.Text = dtUserPowerModule.Rows[i]["FModule"].ToString();
                    treeMenu.Nodes.Add(tn);

                    DataRow[] drs = dtUserPower.Select("FModule='" + dtUserPowerModule.Rows[i]["FModule"].ToString() + "'");
                    for (int n = 0; n < drs.Count(); n++)
                    {
                        FineUIPro.TreeNode tnc = new FineUIPro.TreeNode();
                        tnc.Text = drs[n]["FMenuName"].ToString();
                        tnc.IconUrl = drs[n]["FMenuIcon"].ToString();
                        tnc.NavigateUrl = drs[n]["FMenuUrl"].ToString();
                        tn.Nodes.Add(tnc);
                    }
                    tn.Expanded = true;
                }
            }
        }

        // 安全退出
        protected void btnQuit_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
    }
}