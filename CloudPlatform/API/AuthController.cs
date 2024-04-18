using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace CloudPlatform.API
{
    // 鉴权相关API
    public class AuthController : ApiController
    {
        Code.cls_SQLOperate op = new Code.cls_SQLOperate();

        // 获取Token接口
        [HttpPost]
        public HttpResponseMessage getToken([FromBody] JObject GetJObj)
        {
            string returnStr = string.Empty;

            string AppKey = string.Empty;
            string AppSecret = string.Empty;

            try
            {
                AppKey = GetJObj["AppKey"].ToString();
            }
            catch { }

            try
            {
                AppSecret = GetJObj["AppSecret"].ToString();
            }
            catch { }

            if (!string.IsNullOrWhiteSpace(AppKey) && !string.IsNullOrWhiteSpace(AppSecret))
            {
                DataTable dtCusAuth = op.GetCustomerAuth(AppKey);
                if (dtCusAuth.Rows.Count > 0)
                {
                    if (AppSecret == dtCusAuth.Rows[0]["FAppSecret"].ToString())
                    {
                        // 生成AccessToken
                        string AccessToken = Guid.NewGuid().ToString("N");
                        string ExpTime = DateTime.Now.AddHours(2).ToString("yyyy-MM-dd HH:mm:ss");

                        bool ok = op.RefreshCustomerToken(AppKey, AccessToken, ExpTime);
                        if (ok)
                        {
                            // 拼接回复数据的JSON字符串
                            StringBuilder sbJson = new StringBuilder();

                            sbJson.Append("{");
                            sbJson.Append("\"code\":0,");
                            sbJson.Append("\"msg\":\"ok\",");
                            sbJson.Append("\"data\":{");
                            sbJson.Append("\"accessToken\":\"" + AccessToken + "\",");
                            sbJson.Append("\"ExpTime\":\"" + ExpTime + "\"");
                            sbJson.Append("}}");

                            returnStr = sbJson.ToString();
                        }
                        else
                        {
                            returnStr = "{\"code\":1003,\"msg\":\"[SLC]AccessToken生成失败\",\"data\":null}";
                        }
                    }
                    else
                    {
                        returnStr = "{\"code\":1002,\"msg\":\"[SLC]AppKey数据校验未通过\",\"data\":null}";
                    }
                }
                else
                {
                    returnStr = "{\"code\":1001,\"msg\":\"[SLC]不存在的用户\",\"data\":null}";
                }
            }
            else
            {
                returnStr = "{\"code\":1000,\"msg\":\"[SLC]缺少必要参数\",\"data\":null}";
            }

            var res = new HttpResponseMessage { Content = new StringContent(returnStr, Encoding.UTF8, "application/json") };
            return res;
        }

        // 校验AccessToken是否有效接口
        [HttpPost]
        public HttpResponseMessage checkToken([FromBody] JObject GetJObj)
        {
            string returnStr = string.Empty;

            string AccessToken = string.Empty;

            try
            {
                AccessToken = GetJObj["AccessToken"].ToString();
            }
            catch { }

            if (!string.IsNullOrWhiteSpace(AccessToken))
            {
                DataTable dtCusToken = op.CheckCusAccessToken(AccessToken);
                if (dtCusToken.Rows.Count > 0)
                {
                    returnStr = "{\"code\":0,\"msg\":\"[SLC]AccessToken有效\",\"data\":null}";
                }
                else
                {
                    returnStr = "{\"code\":1001,\"msg\":\"[SLC]不存在的用户\",\"data\":null}";
                }
            }
            else
            {
                returnStr = "{\"code\":1000,\"msg\":\"[SLC]缺少必要参数\",\"data\":null}";
            }

            var res = new HttpResponseMessage { Content = new StringContent(returnStr, Encoding.UTF8, "application/json") };
            return res;
        }
    }
}