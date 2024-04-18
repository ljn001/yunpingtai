using FineUIPro;
using NLog;
using CloudPlatform.Code;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

namespace CloudPlatform
{
    public class PageBase : Page
    {
        protected cls_SQLOperate op = new cls_SQLOperate();
        protected cls_SQLOperateCusPJ opPJ = new cls_SQLOperateCusPJ();

        protected cls_SQLOperateCusYG opYG = new cls_SQLOperateCusYG();
        protected cls_SQLOperateCusYS opYS = new cls_SQLOperateCusYS();
        protected cls_EncryptionUntie eu = new cls_EncryptionUntie();
        protected readonly Logger log = LogManager.GetCurrentClassLogger();

        // 获取cookie中的用户ID
        public string GetUserID()
        {
            try
            {
                HttpCookie cookie_id = Request.Cookies["SkyLink_USERID"];
                return eu.DEC_Decrypt(cookie_id.Value);
            }
            catch
            {
                return null;
            }
        }

        protected string GetRequestID()
        {
            try
            {
                return eu.DEC_Decrypt(Request["ID"]);
            }
            catch
            {
                return null;
            }
        }

        // 获取微信扫码结果
        // 扫描格式：码类型|微信扫描结果
        // 示例1：PlanCode|CODE_128,123456789
        // 示例2：QRCode|http://www.baidu.com
        public string GetWXScanCode(string ScanRes)
        {
            string Code = string.Empty;
            //判断扫描结果中是否包含半角逗号，去掉微信条码格式字符，如：CODE_128,123456789
            if (ScanRes.IndexOf(",") >= 0) 
            {
                return ScanRes.Split('|')[1].Split(',')[1];
            }
            else
            {
                return ScanRes.Split('|')[1];
            }
        }

        #region 弹出网页提示（Pxmu.js，官网：https://oct.cn/project/pxmu/）
        //public enum MsgIcon
        //{
        //    Information,
        //    Success,
        //    Error,
        //    Warning,
        //    Question
        //}

        public string getMsgIconSvgName(MessageBoxIcon icon)
        {
            if (icon == MessageBoxIcon.Success)
            {
                return "Success";
            }
            else if (icon == MessageBoxIcon.Error)
            {
                return "Error";
            }
            else if (icon == MessageBoxIcon.Warning)
            {
                return "Warning";
            }
            else if (icon == MessageBoxIcon.Question)
            {
                return "Question";
            }
            else
            {
                return "Information";
            }
        }

        // 弹出提示（单按钮）
        protected void ShowDialog(string msg, MessageBoxIcon icon = MessageBoxIcon.Information, string btnScript = "", string btnText = "确定")
        {
            string svgName = getMsgIconSvgName(icon);
            
            // 判断是否需要添加按钮点击后的处理事件
            string js_then = string.Empty;
            if (!string.IsNullOrWhiteSpace(btnScript))
            {
                js_then = string.Format(@".then(function(res){{ if(res.btn=='right') {{ {0} }} }})", btnScript);
            }

            // 生成对话框JS脚本
            string js = string.Format(@"pxmu.diaglog({{
                title: {{ text: '' }},
                content: {{ text: '<div style=""width:100%;text-align:center;""><img src=""/res/images/svg/{1}.svg"" width=""70"" /><div style=""margin-top:20px"">{0}</div></div>' }},
                line: {{solid: 1, color: '#d4d4d4'}},
                btn: {{ right: {{text: '{2}', bg: '#fff', solidcolor: '#fff', color: '#0054a6'}} }},
                congif: {{ btncount:true,animation:'slideup',anclose:false }}
            }}){3};", msg, svgName, btnText, js_then);

            PageContext.RegisterStartupScript(js);
        }

        // 弹出提示（双按钮）
        protected void ShowDialogTwoBtn(string msg, MessageBoxIcon icon = MessageBoxIcon.Information, string btnLeftScript = "", string btnRightScript = "", string btnLeftText = "取消", string btnRightText = "确定")
        {
            string svgName = getMsgIconSvgName(icon);

            // 判断是否需要添加按钮点击后的处理事件
            string js_then = string.Empty;
            if (!string.IsNullOrWhiteSpace(btnLeftScript) || !string.IsNullOrWhiteSpace(btnRightScript))
            {
                js_then = string.Format(@".then(function(res){{ if(res.btn=='left') {{ {0} }} else if(res.btn=='right'){{ {1} }} }})", btnLeftScript, btnRightScript);
            }

            string js = string.Format(@"pxmu.diaglog({{
                title: {{ text: '' }},
                content: {{ text: '<div style=""width:100%;text-align:center;""><img src=""/res/images/svg/{1}.svg"" width=""70"" /><div style=""margin-top:20px"">{0}</div></div>' }},
                line: {{solid: 1, color: '#d4d4d4'}},
                btn: {{ left: {{text: '{2}', bg: '#fff', solidcolor: '#fff', color: '#464646'}},right: {{text: '{3}', bg: '#fff', solidcolor: '#fff', color: '#0054a6'}} }},
                congif: {{ btncount:false,animation:'slideup',anclose:false }}
            }}){4};", msg, svgName, btnLeftText, btnRightText, js_then);

            PageContext.RegisterStartupScript(js);
        }
        #endregion

        // 获取URL参数值
        protected string GetRequest(string FName)
        {
            try
            {
                return Request[FName];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 显示通知对话框
        /// </summary>
        /// <param name="message"></param>
        public virtual void ShowNotify(string message)
        {
            ShowNotify(message, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 显示通知对话框
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageIcon"></param>
        public virtual void ShowNotify(string message, MessageBoxIcon messageIcon)
        {
            Notify n = new Notify();
            n.Target = Target.Self;
            n.Message = message;
            n.MessageBoxIcon = messageIcon;
            n.PositionX = Position.Center;
            n.PositionY = Position.Top;
            n.DisplayMilliseconds = 3000;
            n.ShowHeader = false;
            n.Show();
        }

        #region Grid控件相关
        // 获取Grid选中行的值
        public string GetGridCellValue(Grid grid, string ColumnID)
        {
            return grid.Rows[grid.SelectedRowIndexArray[0]].Values[grid.FindColumn(ColumnID).ColumnIndex].ToString();
        }
        public string GetGridCellValue(Grid grid, int RowIndex, string ColumnID)
        {
            return grid.Rows[RowIndex].Values[grid.FindColumn(ColumnID).ColumnIndex].ToString();
        }

        // 根据Grid列生成DataTable
        public DataTable GridSelectedRowToDataTable(Grid grid)
        {
            DataTable dt = new DataTable();
            foreach (GridColumn gc in grid.Columns)
            {
                //if (gc.GetType().Name == "BoundField")
                //{
                dt.Columns.Add(gc.ColumnID);
                //}
            }

            List<string> listCarID = new List<string>();
            for (int i = 0; i < grid.SelectedRowIndexArray.Length; i++)
            {
                DataRow drCar = dt.NewRow();
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    drCar[dt.Columns[c].ColumnName] = GetGridCellValue(grid, grid.SelectedRowIndexArray[i], dt.Columns[c].ColumnName);
                }
                dt.Rows.Add(drCar);
            }

            return dt;
        }

        // 根据Grid控件的ColumnID获取该列的顺序号
        public int GetColumnIndexByColumnID(Grid grid, string ColumnID)
        {
            for (int c = 0; c < grid.Columns.Count; c++)
            {
                if (grid.Columns[c].ColumnID == ColumnID)
                {
                    return c;
                }
            }

            // 如果没有匹配项则返回-1
            return -1;
        }
        #endregion

        // 获取微信用户OpenID
        public string GetWXOpenID(string Code)
        {
            string html = string.Empty;
            string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=wxeeb5f4c1afcd8b07&secret=9a91a686363e5945b0f337afd2fcfa22&code=" + Code + "&grant_type=authorization_code";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream ioStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(ioStream, Encoding.UTF8);
            html = sr.ReadToEnd();
            sr.Close();
            ioStream.Close();
            response.Close();
            //return html;
            string key = "\"openid\":\"";
            int startIndex = html.IndexOf(key);
            if (startIndex >= 0)
            {
                int endIndex = html.IndexOf("\",", startIndex);
                string openid = html.Substring(startIndex + key.Length, endIndex - startIndex - key.Length);
                return openid;
            }
            else
            {
                return null;
            }
        }

        // 去除字符串中HTML标记
        protected string RemoveHtml(string HTMLString)
        {
            //删除脚本  
            HTMLString = Regex.Replace(HTMLString, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML  
            HTMLString = Regex.Replace(HTMLString, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            //HTMLString = Regex.Replace(HTMLString, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            HTMLString = Regex.Replace(HTMLString, @"-->", "", RegexOptions.IgnoreCase);
            HTMLString = Regex.Replace(HTMLString, @"<!--.*", "", RegexOptions.IgnoreCase);

            HTMLString = Regex.Replace(HTMLString, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            HTMLString = Regex.Replace(HTMLString, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            HTMLString = Regex.Replace(HTMLString, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            HTMLString = Regex.Replace(HTMLString, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            HTMLString = Regex.Replace(HTMLString, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            HTMLString = Regex.Replace(HTMLString, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            HTMLString = Regex.Replace(HTMLString, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            HTMLString = Regex.Replace(HTMLString, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            HTMLString = Regex.Replace(HTMLString, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            HTMLString = Regex.Replace(HTMLString, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            HTMLString.Replace("<", "");
            HTMLString.Replace(">", "");
            //HTMLString.Replace("\r\n", "");
            HTMLString = HttpContext.Current.Server.HtmlEncode(HTMLString).Trim();

            return HTMLString;
        }

        // 验证手机号有效性
        public bool IsMobilePhone(string strPhoneNumber)
        {
            return Regex.IsMatch(strPhoneNumber, @"^1[3456789]\d{9}$");
        }

        // 验证身份证号
        public bool IsIDCard(string strIDCard)
        {
            return Regex.IsMatch(strIDCard, @"(^\d{18}$)|(^\d{15}$)");
        }
    }
}