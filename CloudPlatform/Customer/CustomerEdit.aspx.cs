using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using FineUIPro;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.ScanProductAddV2Request.Types.Product.Types;

namespace CloudPlatform.Customer
{
    public partial class CustomerEdit : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                btnClose.OnClientClick = ActiveWindow.GetHideReference();

                // 加载平台列表
                cblPlatform.DataTextField = "FName";
                cblPlatform.DataValueField = "FID";
                cblPlatform.DataSource = op.GetPlatformList(null);
                cblPlatform.DataBind();

                if (string.IsNullOrEmpty(GetRequest("AppKey")))
                {
                    txtFAppKey.Text = Guid.NewGuid().ToString("N");
                    txtFAppSecret.Text = Guid.NewGuid().ToString("N");
                }
                else
                {
                    txtFAppKey.Text = GetRequest("AppKey");
                    try
                    {
                        // 加载客户信息
                        DataRow drCus = op.GetCustomerInfo(txtFAppKey.Text).Rows[0];
                        txtFName.Text = drCus["FName"].ToString();
                        txtFNameShort.Text = drCus["FNameShort"].ToString();
                        txtFTel.Text = drCus["FTel"].ToString();
                        txtFCode.Text = drCus["FCode"].ToString();
                        txtFAddress.Text = drCus["FAddress"].ToString();
                        txtFAppSecret.Text = drCus["FAppSecret"].ToString();
                        try
                        {
                            cbxFEnabled.Checked = bool.Parse(drCus["FEnabled"].ToString());
                        }
                        catch { }
                        txtFAppKey_HQ.Text = drCus["FAppKey_HQ"].ToString();
                        txtFAppSecret_HQ.Text = drCus["FAppSecret_HQ"].ToString();
                        ddlSysType.SelectedValue = drCus["FSysType"].ToString();
                        txtDBLink.Text = drCus["FDBLink"].ToString();
                        txtDBUser.Text = drCus["FDBUser"].ToString();
                        txtDBPwd.Text = drCus["FDBPwd"].ToString();
                        txtDBName.Text = drCus["FDBName"].ToString();

                        // 加载客户授权平台信息
                        DataTable dtCustomerPlatform = op.GetCustomerPlatform(txtFAppKey.Text);
                        if (dtCustomerPlatform.Rows.Count > 0)
                        {
                            string[] pArray = new string[dtCustomerPlatform.Rows.Count];
                            for (int i = 0; i < dtCustomerPlatform.Rows.Count; i++)
                            {
                                pArray[i] = dtCustomerPlatform.Rows[i]["FPlatformID"].ToString();
                            }
                            cblPlatform.SelectedValueArray = pArray;
                        }
                    }
                    catch
                    {
                        Alert.Show("数据加载失败！", "异常", MessageBoxIcon.Error, ActiveWindow.GetHidePostBackReference());
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            List<string> listSQL = new List<string>();
            listSQL.Add("DELETE FROM tCustomerPlatform WHERE FCusAppKey='" + txtFAppKey.Text + "'");
            foreach (string PlatformID in cblPlatform.SelectedValueArray)
            {
                listSQL.Add("INSERT INTO tCustomerPlatform(FCusAppKey,FPlatformID)VALUES('" + txtFAppKey.Text + "','" + PlatformID + "')");
            }
            try
            {
                string DBConnStr = "Data Source=" + txtDBLink.Text + ";Initial Catalog=" + txtDBName.Text + ";User ID=" + txtDBUser.Text + ";Password=" + txtDBPwd.Text + ";Connection Timeout=0";

                op.DoSqlTRAN(string.Join(" ", listSQL.ToArray()));

                if (string.IsNullOrWhiteSpace(GetRequest("AppKey")))
                {
                    bool ok = op.AddCustomer(txtFAppKey.Text, txtFName.Text, txtFNameShort.Text, txtFCode.Text, txtFTel.Text, txtFAddress.Text, txtFAppSecret.Text, cbxFEnabled.Checked.ToString(), txtFAppKey_HQ.Text.Trim(), txtFAppSecret_HQ.Text.Trim(), txtDBLink.Text, txtDBUser.Text, txtDBPwd.Text, txtDBName.Text, DBConnStr, ddlSysType.SelectedValue);
                    if (ok)
                    {
                        Alert.Show("保存成功", "成功", MessageBoxIcon.Success, ActiveWindow.GetHidePostBackReference());
                    }
                    else
                    {
                        Alert.Show("保存失败！", MessageBoxIcon.Error);
                    }
                }
                else
                {
                    bool ok = op.UpdateCustomer(txtFAppKey.Text, txtFName.Text, txtFNameShort.Text, txtFCode.Text, txtFTel.Text, txtFAddress.Text, txtFAppSecret.Text, cbxFEnabled.Checked.ToString(), txtFAppKey_HQ.Text.Trim(), txtFAppSecret_HQ.Text.Trim(), txtDBLink.Text, txtDBUser.Text, txtDBPwd.Text, txtDBName.Text, DBConnStr, ddlSysType.SelectedValue);
                    if (ok)
                    {
                        Alert.Show("保存成功", "成功", MessageBoxIcon.Success, ActiveWindow.GetHidePostBackReference());
                    }
                    else
                    {
                        Alert.Show("保存失败！", MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                Alert.Show("保存失败！<br/>" + ex.Message, MessageBoxIcon.Error);
            }
        }

        protected void txtFAppSecret_TriggerClick(object sender, EventArgs e)
        {
            txtFAppSecret.Text = Guid.NewGuid().ToString("N");
        }
    }
}