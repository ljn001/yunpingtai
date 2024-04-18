using System;
using System.Text;
using System.Web.Configuration;
using FineUIPro;
using SKIT.FlurlHttpClient.Wechat.Api;
using CloudPlatform.Code.wx;
using CloudPlatform.Mobile;
using System.Data;
using System.Collections.Generic;

namespace CloudPlatform.m
{
    public partial class bgyg : PageBase
    {
        CheckWXOpenID ckWXID = new CheckWXOpenID();
        WXApi wx = new WXApi();

        public string wxJsSdkConfig { get; set; } //微信JS-SDK配置信息

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 获取用户OpenID
                lblWXOpenID.Text = ckWXID.Check(Request.Url.AbsoluteUri);

                // 获取微信AccessToken
                string AccessToken = wx.GetAccessToken();

                // 获取Ticket
                string Ticket = wx.GetTicket(AccessToken);

                if(string.IsNullOrWhiteSpace(Ticket))
                {
                    ShowDialog("无效的Ticket", MessageBoxIcon.Error);
                    return;
                }
              
                #region 生成调用微信JS-SDK的配置信息
                var options = new WechatApiClientOptions()
                {
                    AppId = WebConfigurationManager.AppSettings["appid"].ToString(),
                    AppSecret = WebConfigurationManager.AppSettings["appsecret"].ToString(),
                    MidasAppKey = "", //米大师相关服务 AppKey，不用则不填
                    ImmeDeliveryAppKey = "", //即时配送相关服务 AppKey，不用则不填
                    ImmeDeliveryAppSecret = "" //即时配送相关服务 AppSecret，不用则不填
                };
                var client = new WechatApiClient(options);

                var paramMap = client.GenerateParametersForJSSDKConfig(Ticket, Request.Url.ToString());

                StringBuilder sb = new StringBuilder();
                sb.Append("wx.config({");
                sb.Append("debug: false,"); //开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印
                sb.Append("appId: '" + paramMap["appId"] + "',"); //必填，公众号的唯一标识
                sb.Append("timestamp: " + paramMap["timestamp"] + ","); //必填，生成签名的时间戳
                sb.Append("nonceStr: '" + paramMap["nonceStr"] + "',"); //必填，生成签名的随机串
                sb.Append("signature: '" + paramMap["signature"] + "',"); //必填，签名
                sb.Append("jsApiList: [\"scanQRCode\"]"); //必填，需要使用的JS接口列表
                sb.Append("});");
                wxJsSdkConfig = sb.ToString();
                #endregion

                // 根据OpenID获取绑定企业信息
                //DataTable dtCus = op.GetCustomerUserBind(lblWXOpenID.Text);
                //if (dtCus.Rows.Count > 0)
                //{
                //    // 绑定企业下拉列表
                //    ddlFCus.DataValueField = "FAppKey";
                //    ddlFCus.DataTextField = "FCusName";
                //    ddlFCus.DataSource = dtCus;
                //    ddlFCus.DataBind();

                //    // 自动选中当前用户的默认企业
                //    try
                //    {
                //        ddlFCus.SelectedValue = dtCus.Select("FDefault=1")[0]["FAppKey"].ToString();
                //    }
                //    catch { }

                    // 判断URL参数（是否扫码进入）
             
 

                    // 读取客户企业用户信息drCus["FCusUserID"].ToString(), connectionString
                    DataTable dtCus = op.GetCustomerUserBind(lblWXOpenID.Text);
                   
                    //

                    if (dtCus.Rows.Count > 0)
                    {

                        DataTable dtCusdb = op.GetCustomerUserBindDB(dtCus.Rows[0]["bumendaihao"].ToString());


                        if (dtCusdb.Rows.Count > 0)
                        {
                            string connectionString = dtCusdb.Rows[0]["FDBConnStr"].ToString();
                            ddlFCus.Text = dtCusdb.Rows[0]["FName"].ToString();
                            //     string connectionString = drCus["FDBConnStr"].ToString();
                            lblDBConnStr.Text = eu.DEC_Encrypt(connectionString);

                            DataRow drCus = dtCus.Rows[0];
                            DataTable dtCusUser = opYG.GetUserInfo(drCus["FCusUserID"].ToString(), connectionString);
                            if (dtCusUser.Rows.Count > 0)
                            {
                                DataRow drCusUser = dtCusUser.Rows[0];
                                // 判断账号是否可用
                                if (drCusUser["FIsLeave"].ToString() == "0")
                                {
                                    txtUserID.Text = drCusUser["FStaffNo"].ToString();
                                    txtUserName.Text = drCusUser["FStaffName"].ToString();
                                    if (drCusUser["FWorkShopNM"].ToString().Contains("三车间"))
                                    {
                                        txtChejian.Text = "三车间";
                                    }
                                    else if (drCusUser["FWorkShopNM"].ToString().Contains("二车间"))
                                    {
                                        txtChejian.Text = "二车间";
                                    }
                                    else
                                    {
                                        txtChejian.Text = "一车间";
                                    }

                                    // 读取客户数据库系统设置信息
                                    //  DataTable dtSettings = opPJ.GetSettings(connectionString);
                                    //if (dtSettings != null && dtSettings.Rows.Count > 0)
                                    //{
                                    //    // 手机端默认工序选择范围（UserProc：报工员工已指定的员工工序；PlanProc：报工生产计划的全部工序）
                                    //    try
                                    //    {
                                    //        lblMobileProcList.Text = dtSettings.Select("FSetItem='MobileProcList'")[0]["FSetInfo"].ToString();
                                    //    }
                                    //    catch { }

                                    // 是否允许一次汇报多道工序
                                    //try
                                    //{
                                    //    if (bool.Parse(dtSettings.Select("FSetItem='EnableMultipleProcBG'")[0]["FSetInfo"].ToString()))
                                    //    {
                                    //        ddlProcedure.EnableMultiSelect = true;
                                    //    }
                                    //}
                                    //catch { }
                                    //}

                                    //// 读取用户所设置的员工工序信息
                                    //if (lblMobileProcList.Text == "UserProc")
                                    //{
                                    //    GetUserProcedure(drCusUser["FUserID"].ToString(), connectionString);
                                    //}
                                    //else
                                    //{
                                    //    lblUserProcIDList.Text = "PlanProc";
                                    //}

                                    // LoadWorkPlan();
                                }

                                else
                                {
                                    ShowDialog("此账号已禁用，请联系您所在企业的系统管理人员。", MessageBoxIcon.Error);
                                }
                            }
                        else
                        {
                            ShowDialog("此账号在对应企业库不存在，请联系您所在企业的系统管理人员。", MessageBoxIcon.Error);
                        }
                    }
                      
                    }
                  
                string UrlData = GetRequest("data");
                if (!string.IsNullOrEmpty(UrlData))
                {
                    string[] ddxx = UrlData.Split(',');

                    if (ddxx.Length == 3)
                    {
                        txtBillNo.Text = ddxx[0];
                        txtTuzhihao.Text = ddxx[1];
                        txtSucai.Text = ddxx[2];
                        LoadWorkGongxu();


                    }
                } 
                //}
                //else
                //{
                //    ddlFCus_SelectedIndexChanged(null, null);
                //}
                //}
                //else
                //{
                //    // 如果此微信号未绑定任何企业，则直接跳转至绑定页面
                //    ShowDialog("此微信尚未绑定任何企业账号，请绑定后再操作！", MessageBoxIcon.Warning, "window.location='UserBind.aspx'", "去绑定");
                //}
            }
        }
        #endregion

        #region 读取用户所设置的员工工序信息
        public void GetUserProcedure(string UserID, string connectionString)
        {
            DataTable dtUserProc = opPJ.GetUserProcedure(txtUserID.Text, connectionString);
            List<string> listUserProc = new List<string>();
            foreach (DataRow dr in dtUserProc.Rows)
            {
                listUserProc.Add(dr["FProcedureID"].ToString());
            }
            lblUserProcIDList.Text = string.Join(",", listUserProc.ToArray());
        }
        #endregion

        #region 读取生产计划
        private void LoadWorkGongxu()
        {

            DataTable dtProcedure = opYG.GetHuibaogongxu(txtUserID.Text, txtBillNo.Text, txtTuzhihao.Text, txtSucai.Text, eu.DEC_Decrypt(lblDBConnStr.Text));


            //string connectionString = eu.DEC_Decrypt(lblDBConnStr.Text);
            //// 读取生产计划信息
            //DataTable dtWorkPlan = opPJ.ReadWorkPlan(lblWorkPlanID.Text, connectionString);
            if (dtProcedure.Rows.Count > 0)
            {


                ddlProcedure.DataValueField = "fprocessno";
                ddlProcedure.DataTextField = "fprocessnm";
                ddlProcedure.DataSource = dtProcedure;
                ddlProcedure.DataBind();

            }
        }
            #endregion

            #region 客户端回发事件
            protected void pmMain_CustomEvent(object sender, CustomEventArgs e)
        {
            if (e.EventArgument.StartsWith("scanPlan|")) //扫描生产计划码
            {
                string PlanCode = GetWXScanCode(e.EventArgument);
                if (!PlanCode.Contains("="))
                {
                    ShowDialog("二维码不正确，解析失败", MessageBoxIcon.Information);
                }
                string[] ddxx  = PlanCode.Split('=')[1].Split(',');
                 
                if (PlanCode.ToUpper().StartsWith("HQ") || ddxx.Length==3)
                {
                    txtBillNo.Text = ddxx[0];
                    txtTuzhihao.Text = ddxx[1];
                    txtSucai.Text = ddxx[2];
                    LoadWorkGongxu();


                }
                else if(PlanCode.ToUpper().StartsWith("WX"))
                {
                    ShowDialog("手机端暂不支持外协加工汇报", MessageBoxIcon.Information);
                }
                else
                {
                    ShowDialog("无效的生产计划码或不支持的生产计划类型");
                }
            }
            else if (e.EventArgument.StartsWith("scanProcedure|")) //扫描工序码
            {
                string ProcedureCode = GetWXScanCode(e.EventArgument);
                // 根据工序码解析工序ID
                string ProcedureID = Convert.ToInt32(ProcedureCode.Substring(2, ProcedureCode.Length - 2)).ToString();

                // 根据生产计划和所扫描工序读取【生产计划】工序信息
                DataTable dtProc = opPJ.ReadWorkPlanProcInfo(lblWorkPlanID.Text, ProcedureID, eu.DEC_Decrypt(lblDBConnStr.Text));
                if (dtProc.Rows.Count > 0)
                {
                    string FIDWorkPlanPro = dtProc.Rows[0]["FItemID"].ToString();

                    // 判断所扫描工序是否在当前可选工序的下拉列表中
                    bool exists = false;
                    foreach (FineUIPro.ListItem li in ddlProcedure.Items)
                    {
                        if (li.Value == FIDWorkPlanPro)
                        {
                            exists = true;
                            break;
                        }
                    }

                    if (exists) //如果包含此工序，则自动选中
                    {
                        ddlProcedure.SelectedValue = FIDWorkPlanPro;
                    }
                    else //如果不包含此工序，则将扫描工序添加到下拉列表中并选中
                    {
                        // 向工序下拉列表添加新的工序并选中
                        ddlProcedure.Items.Add(dtProc.Rows[0]["FName"].ToString(), FIDWorkPlanPro);
                        ddlProcedure.SelectedValue = FIDWorkPlanPro;
                    }

                    // 强制触发工序变更事件
                    ddlProcedure_SelectedIndexChanged(null, null);
                }
                else
                {
                    ShowDialog("当前生产计划不包含此工序", MessageBoxIcon.Error);
                }
            }
            else
            {
                ShowDialog("未知条码类型");
            }
        }
        #endregion

        #region 工序变更
        protected void ddlProcedure_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 工序变更后自动计算生产计划此工序的剩余数量
            //if (ddlProcedure.SelectedItem != null)
            //{
            //    if (rblReportType.SelectedValue != "FuZhu")
            //    {
            //        // 遍历校验所选工序
            //        foreach(FineUIPro.ListItem item in ddlProcedure.SelectedItemArray)
            //        {
            //            try
            //            {
            //                DataRow dr = opPJ.ReadWorkPlanProcTotalQty(item.Value.ToString(), eu.DEC_Decrypt(lblDBConnStr.Text)).Rows[0];
            //                decimal ShengYu = Convert.ToDecimal(txtPlanQty.Text) - Convert.ToDecimal(dr["FCounts"]);
            //                if (ShengYu > 0)
            //                {
            //                    try
            //                    {
            //                        if (ShengYu < Convert.ToDecimal(txtSCQty.Text))
            //                        {
            //                            txtSCQty.Text = ShengYu.ToString();
            //                        }
            //                    }
            //                    catch { }
            //                }
            //                else
            //                {
            //                    txtSCQty.Text = "0";
            //                    ShowDialog("本生产计划[" + item.Text + "]工序已完工", MessageBoxIcon.Warning);
            //                    return;
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                ShowDialog("工序[" + item.Text + "]未生产数量获取失败！<br/>" + ex.Message, MessageBoxIcon.Error);
            //                return;
            //            }
            //        }
            //    }
            //}
        }
        #endregion

        #region 变更企业（非扫码进入页面，手工录入数据）
        protected void ddlFCus_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtCus = op.GetCustomerUserBind(lblWXOpenID.Text, ddlFCus.SelectedValue);
            if (dtCus.Rows.Count > 0)
            {
                DataRow drCus = dtCus.Rows[0];

                string connectionString = drCus["FDBConnStr"].ToString();
                lblDBConnStr.Text = eu.DEC_Encrypt(connectionString);

                // 读取客户企业用户信息
                DataTable dtCusUser = opPJ.GetUserInfo(drCus["FCusUserID"].ToString(), connectionString);
                if (dtCusUser.Rows.Count > 0)
                {
                    DataRow drCusUser = dtCusUser.Rows[0];
                    // 判断账号是否可用
                    if (drCusUser["FIsLeave"].ToString() == "0")
                    {
                        txtUserID.Text = drCusUser["FStaffNo"].ToString();
                        txtUserName.Text = drCusUser["FStaffName"].ToString();

                        // 读取客户数据库系统设置信息
                        //DataTable dtSettings = opPJ.GetSettings(connectionString);
                        //if (dtSettings != null && dtSettings.Rows.Count > 0)
                        //{
                        //    // 手机端默认工序选择范围（UserProc：报工员工已指定的员工工序；PlanProc：报工生产计划的全部工序）
                        //    try
                        //    {
                        //        lblMobileProcList.Text = dtSettings.Select("FSetItem='MobileProcList'")[0]["FSetInfo"].ToString();
                        //    }
                        //    catch { }

                        //    // 是否允许一次汇报多道工序
                        //    try
                        //    {
                        //        if (bool.Parse(dtSettings.Select("FSetItem='EnableMultipleProcBG'")[0]["FSetInfo"].ToString()))
                        //        {
                        //            ddlProcedure.EnableMultiSelect = true;
                        //        }
                        //    }
                        //    catch { }
                        //}

                        // 读取用户权限
                        //try
                        //{
                        //    if (drCusUser["FPower"].ToString().Contains("wxHuiBaoFuZhu"))
                        //    {
                        //        rblReportType.Items[2].Enabled = true;
                        //    }
                        //    else
                        //    {
                        //        if (rblReportType.SelectedIndex == 2)
                        //        {
                        //            rblReportType.SelectedIndex = 1;
                        //            rblReportType_SelectedIndexChanged(null, null);
                        //        }

                        //        rblReportType.Items[2].Enabled = false;
                        //    }
                        //}
                       // catch { }

                        // 读取用户所设置的员工工序信息
                        GetUserProcedure(drCusUser["FUserID"].ToString(), connectionString);
                    }
                }
            }
            else
            {
                ShowDialog("账号数据异常", MessageBoxIcon.Error);
            }
        }
        #endregion

        #region 变更汇报类型
        protected void rblReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSCQty.Hidden = false;
            
            pnlWorkPlan.Hidden = false;
            txtBillNo.Hidden = false;
         
            

            if (rblReportType.SelectedValue == "Start") //开工汇报
            {
                txtSCQty.Hidden = true;
               
            }
            else if (rblReportType.SelectedValue == "End") //完工汇报
            {
                txtSCQty.Hidden = false;
                
            }
            else if(rblReportType.SelectedValue == "FuZhu") //辅助工序汇报
            {
                pnlWorkPlan.Hidden = true;
                txtBillNo.Hidden = true; 
           
                // 加载辅助工序
                LoadFuZhuProc();
            }
        }
        #endregion

        #region 加载辅助工序
        private void LoadFuZhuProc()
        {
            string connectionString = eu.DEC_Decrypt(lblDBConnStr.Text);
            DataTable dtProcedure = opPJ.ReadFuZhuProc(connectionString);
            DataRow drProcedure = dtProcedure.NewRow();
            drProcedure["FID"] = "0";
            drProcedure["FName"] = "[请选择或扫码...]";
            dtProcedure.Rows.InsertAt(drProcedure, 0);

            ddlProcedure.DataValueField = "FID";
            ddlProcedure.DataTextField = "FName";
            ddlProcedure.DataSource = dtProcedure;
            ddlProcedure.DataBind();

            ddlProcedure.SelectedIndex = 0;
        }
        #endregion

        #region 提交汇报
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ddlProcedure.SelectedItem == null)
            {
                ShowDialog("未指定生产工序", MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(ddlProcedure.SelectedValue) || ddlProcedure.SelectedValue == "0")
            {
                ShowDialog("未指定生产工序", MessageBoxIcon.Warning);
                return;
            }

            //if (rblReportType.SelectedValue != "FuZhu" && string.IsNullOrWhiteSpace(lblWorkPlanID.Text))
            //{
            //    ShowDialog("未指定生产计划", MessageBoxIcon.Warning);
            //    return;
            //}

            #region 数量校验
            decimal SCQty = 0;
            try
            {
                SCQty = decimal.Parse(txtSCQty.Text);
            }
            catch
            {
                ShowDialog("生产数量无效！", MessageBoxIcon.Warning);
                return;
            }
 

            if (SCQty <= 0)
            {
                ShowDialog("生产数量必须大于0", MessageBoxIcon.Warning);
                return;
            }

            //if (rblReportType.SelectedValue != "FuZhu")
            //{
            //    if (decimal.Parse(txtPlanQty.Text) < SCQty)
            //    {
            //        ShowDialog("生产数量不允许超过计划数量", MessageBoxIcon.Warning);
            //        return;
            //    }
            //}

            // 再次从数据库获取已报工数量进行校验（防止多人同时报工导致超计划）
            //if (rblReportType.SelectedValue != "FuZhu")
            //{
            //    try
            //    {
            //        // 遍历校验所选工序
            //        foreach (FineUIPro.ListItem item in ddlProcedure.SelectedItemArray)
            //        {
            //            DataRow dr = opPJ.ReadWorkPlanProcTotalQty(item.Value.ToString(), eu.DEC_Decrypt(lblDBConnStr.Text)).Rows[0];
            //            decimal ShengYu = Convert.ToDecimal(txtPlanQty.Text) - Convert.ToDecimal(dr["FCounts"]);
            //            if (SCQty > ShengYu)
            //            {
            //                ShowDialog("工序[" + item.Text + "]累计数量已超过生产计划总数量，计划剩余数量：" + ShengYu.ToString(), MessageBoxIcon.Warning);
            //                return;
            //            }
            //        }
            //    }
            //    catch { }
            //}
            #endregion

            try
            {
                List<string> listErrorProc = new List<string>();

                // 判断汇报类型（开工汇报，完工汇报）
                if (rblReportType.SelectedValue == "Start") //开工汇报
                {
                    // 遍历校验所选工序
                    //foreach (FineUIPro.ListItem item in ddlProcedure.SelectedItemArray)
                    //{
                    //    bool ok = opPJ.ProductionDataStart(lblWorkPlanID.Text, item.Value, txtUserID.Text, txtUserID.Text, "0", "0", "", null, "N", "0", eu.DEC_Decrypt(lblDBConnStr.Text));
                    //    if (!ok)
                    //    {
                    //        listErrorProc.Add(item.Text);
                    //    }
                    //}
                }
                else if(rblReportType.SelectedValue == "End") //完工汇报
                {
                    // 遍历校验所选工序
                    foreach (FineUIPro.ListItem item in ddlProcedure.SelectedItemArray)
                    {
                        bool ok = opYG.ReportData(txtUserID.Text, txtUserName.Text, txtBillNo.Text, txtTuzhihao.Text,   txtSucai.Text
                         , item.Value, item.Text, txtChejian.Text,  SCQty, eu.DEC_Decrypt(lblDBConnStr.Text));
                        if (!ok)
                        {
                            listErrorProc.Add(item.Text);
                        }
                    }
                }
                else // 辅助汇报
                {
                    // 遍历校验所选工序
                    //foreach (FineUIPro.ListItem item in ddlProcedure.SelectedItemArray)
                    //{
                    //    bool ok = opYG.ReportData(txtUserID.Text , txtUserName.Text, txtBillNo.Text, txtTuzhihao.Text, txtSCQty.Text, txtSucai.Text
                    //     , item.Value,item.Text, txtChejian.Text, SCQty);
                    //    if (!ok)
                    //    {
                    //        listErrorProc.Add(item.Text);
                    //    }
                    //}
                }

                if (listErrorProc.Count == 0)
                {
                    //lblWorkPlanID.Text = null;
                    //txtPlanNo.Text = null;
                    //txtBillNo.Text = null;
                    
                    //if (rblReportType.SelectedValue != "FuZhu")
                    //{
                    //    ddlProcedure.Items.Clear();
                    //}
                    txtSCQty.Text = null;
             
                    ShowDialog("汇报提交成功", MessageBoxIcon.Success);
                }
                else
                {
                    ShowDialog("汇报提交失败，失败工序：<br/>" + string.Join("<br/>", listErrorProc), MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                ShowDialog("系统异常！" + ex.Message, MessageBoxIcon.Error);
            }
        }

        protected void btnScan_Click(object sender, EventArgs e)
        {

        }
        #endregion

        //protected void btnTest_Click(object sender, EventArgs e)
        //{
        //    //ShowDialog("测试啊", "window.location='UserBindInfo.aspx'");
        //    ShowDialog("数据保存成功！", MessageBoxIcon.Error, "F.customEvent('scanRes|嘿嘿嘿');");
        //    //ShowDialog("测试啊");
        //}
    }
}