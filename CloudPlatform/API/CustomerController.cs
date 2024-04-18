using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace CloudPlatform.API
{
    // 客户相关API
    public class CustomerController : ApiController
    {
        Code.cls_SQLOperate op = new Code.cls_SQLOperate();

        // 获取Token接口
        [HttpPost]
        public HttpResponseMessage getList([FromBody] JObject GetJObj)
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
                // 校验客户AccessToken
                DataTable dtCusToken = op.CheckCusAccessToken(AccessToken);
                if (dtCusToken.Rows.Count > 0)
                {
                    DataTable dt = op.GetCusPlatform(dtCusToken.Rows[0]["FCusAppKey"].ToString(), true);
                    if (dt.Rows.Count > 0)
                    {
                        // 将DataTable抓换为JSON字符串
                        string jsonStr = JsonConvert.SerializeObject(dt);

                        // 拼接回复数据的JSON字符串
                        StringBuilder sbJson = new StringBuilder();

                        sbJson.Append("{");
                        sbJson.Append("\"code\":0,");
                        sbJson.Append("\"msg\":\"ok\",");
                        sbJson.Append("\"data\":" + jsonStr);
                        sbJson.Append("}");

                        returnStr = sbJson.ToString();
                    }
                    else
                    {
                        returnStr = "{\"code\":3303,\"msg\":\"[SLC]没有任何授权对接的客户\",\"data\":null}";
                    }
                }
                else
                {
                    returnStr = "{\"code\":3302,\"msg\":\"[SLC]AccessToken无效\",\"data\":null}";
                }
            }
            else
            {
                returnStr = "{\"code\":3301,\"msg\":\"[SLC]缺少AccessToken参数\",\"data\":null}";
            }

            var res = new HttpResponseMessage { Content = new StringContent(returnStr, Encoding.UTF8, "application/json") };
            return res;
        }

        // 获取Token接口
        [HttpPost]
        public HttpResponseMessage getCusUserBindList([FromBody] JObject GetJObj)
        {
            string returnStr = string.Empty;

            string AccessToken = string.Empty;

            string CusUserID = string.Empty;

            try
            {
                AccessToken = GetJObj["AccessToken"].ToString();
            }
            catch { }

            try
            {
                CusUserID = GetJObj["CusUserID"].ToString();
            }
            catch { }

            if (!string.IsNullOrWhiteSpace(AccessToken))
            {
                // 校验客户AccessToken
                DataTable dtCusToken = op.CheckCusAccessToken(AccessToken);
                if (dtCusToken.Rows.Count > 0)
                {
                    DataTable dt = op.GetCustomerUser(dtCusToken.Rows[0]["FCusAppKey"].ToString(), CusUserID);
                    if (dt.Rows.Count > 0)
                    {
                        // 将DataTable抓换为JSON字符串
                        string jsonStr = JsonConvert.SerializeObject(dt);

                        // 拼接回复数据的JSON字符串
                        StringBuilder sbJson = new StringBuilder();

                        sbJson.Append("{");
                        sbJson.Append("\"code\":0,");
                        sbJson.Append("\"msg\":\"ok\",");
                        sbJson.Append("\"data\":" + jsonStr);
                        sbJson.Append("}");

                        returnStr = sbJson.ToString();
                    }
                    else
                    {
                        returnStr = "{\"code\":3303,\"msg\":\"[SLC]未查询到任何数据\",\"data\":null}";
                    }
                }
                else
                {
                    returnStr = "{\"code\":3302,\"msg\":\"[SLC]AccessToken无效\",\"data\":null}";
                }
            }
            else
            {
                returnStr = "{\"code\":3301,\"msg\":\"[SLC]缺少AccessToken参数\",\"data\":null}";
            }

            var res = new HttpResponseMessage { Content = new StringContent(returnStr, Encoding.UTF8, "application/json") };
            return res;
        }
    }
}