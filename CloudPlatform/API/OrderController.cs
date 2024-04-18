using Newtonsoft.Json.Linq;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace CloudPlatform.API
{
    public class OrderController : ApiController
    {
        Code.cls_SQLOperate op = new Code.cls_SQLOperate();
        Code.cls_HttpHelper http = new Code.cls_HttpHelper();
        Code.cls_HuanQiu HQ = new Code.cls_HuanQiu();

        #region 客户读取采购订单（供应商读取其客户采购订单）
        [HttpPost]
        public HttpResponseMessage getCusPurchaseOrder([FromBody] JObject GetJObj)
        {
            string returnStr = string.Empty;

            #region 参数校验
            string AccessToken = string.Empty;
            try
            {
                AccessToken = GetJObj["AccessToken"].ToString();
            }
            catch { }
            if(string.IsNullOrWhiteSpace(AccessToken))
            {
                returnStr = "{\"code\":3201,\"msg\":\"[SLC]缺少参数：AccessToken\",\"data\":null}";
                return new HttpResponseMessage { Content = new StringContent(returnStr, Encoding.UTF8, "application/json") };
            }

            string CusPlatformID = string.Empty;
            try
            {
                CusPlatformID = GetJObj["CusPlatformID"].ToString();
            }
            catch { }
            if (string.IsNullOrWhiteSpace(CusPlatformID))
            {
                returnStr = "{\"code\":3201,\"msg\":\"[SLC]缺少参数：CusPlatformID\",\"data\":null}";
                return new HttpResponseMessage { Content = new StringContent(returnStr, Encoding.UTF8, "application/json") };
            }

            string OrderNo = string.Empty;
            try
            {
                OrderNo = GetJObj["OrderNo"].ToString();
            }
            catch { }
            if (string.IsNullOrWhiteSpace(OrderNo))
            {
                returnStr = "{\"code\":3201,\"msg\":\"[SLC]缺少参数：OrderNo\",\"data\":null}";
                return new HttpResponseMessage { Content = new StringContent(returnStr, Encoding.UTF8, "application/json") };
            }
            #endregion

            // 校验客户AccessToken
            DataTable dtCusInfo = op.CheckCusAccessToken(AccessToken);
            if (dtCusInfo.Rows.Count > 0)
            {
                DataRow drCusInfo = dtCusInfo.Rows[0];
                string FCusAppKey = drCusInfo["FCusAppKey"].ToString();

                // 读取对接平台信息
                DataRow drPlatform;
                try
                {
                    drPlatform = op.GetPlatformInfo(CusPlatformID).Rows[0];
                }
                catch
                {
                    returnStr = "{\"code\":3202,\"msg\":\"[SLC]对接平台ID无效\",\"data\":null}";
                    return new HttpResponseMessage { Content = new StringContent(returnStr, Encoding.UTF8, "application/json") };
                }

                // 判断所要对接的平台是环球渔具还是其他平台
                if (drPlatform["FName"].ToString().IndexOf("环球渔具") >= 0)
                {
                    string AccessToken_HQ = HQ.GetAccessToken(FCusAppKey, CusPlatformID);
                    if (!string.IsNullOrWhiteSpace(AccessToken_HQ))
                    {
                        // 调用环球渔具平台接口读取采购订单
                        string res = http.Post(drPlatform["FAPIUrl"].ToString() + "/API/Purchase/getPOOrder", "{\"AppKey\":\"" + drCusInfo["FAppKey_HQ"] + "\",\"AccessToken\": \"" + AccessToken_HQ + "\",\"OrderNo\":\"" + OrderNo + "\"}");
                        if (!string.IsNullOrWhiteSpace(res))
                        {
                            return new HttpResponseMessage { Content = new StringContent(res, Encoding.UTF8, "application/json") };
                        }
                        else
                        {
                            returnStr = "{\"code\":3205,\"msg\":\"[SLC]环球渔具GFG平台未返回任何数据\",\"data\":null}";
                            return new HttpResponseMessage { Content = new StringContent(returnStr, Encoding.UTF8, "application/json") };
                        }
                    }
                    else
                    {
                        returnStr = "{\"code\":3204,\"msg\":\"[SLC]环球渔具GFG平台AccessToken获取失败\",\"data\":null}";
                        return new HttpResponseMessage { Content = new StringContent(returnStr, Encoding.UTF8, "application/json") };
                    }
                }
                else
                {
                    returnStr = "{\"code\":3203,\"msg\":\"[SLC]此平台尚未开通数据对接服务\",\"data\":null}";
                    return new HttpResponseMessage { Content = new StringContent(returnStr, Encoding.UTF8, "application/json") };
                }
            }
            else
            {
                returnStr = "{\"code\":3202,\"msg\":\"[SLC]AccessToken无效\",\"data\":null}";
                return new HttpResponseMessage { Content = new StringContent(returnStr, Encoding.UTF8, "application/json") };
            }
        }
        #endregion

        #region 读取周计划（环球）
        [HttpPost]
        public HttpResponseMessage getWeekPlan([FromBody] JObject GetJObj)
        {
            string returnStr = string.Empty;

            #region 参数校验
            string AccessToken = string.Empty;
            try
            {
                AccessToken = GetJObj["AccessToken"].ToString();
            }
            catch { }
            if (string.IsNullOrWhiteSpace(AccessToken))
            {
                returnStr = "{\"code\":3201,\"msg\":\"[SLC]缺少参数：AccessToken\",\"data\":null}";
                return new HttpResponseMessage { Content = new StringContent(returnStr, Encoding.UTF8, "application/json") };
            }

            string CusPlatformID = string.Empty;
            try
            {
                CusPlatformID = GetJObj["CusPlatformID"].ToString();
            }
            catch { }
            if (string.IsNullOrWhiteSpace(CusPlatformID))
            {
                returnStr = "{\"code\":3201,\"msg\":\"[SLC]缺少参数：CusPlatformID\",\"data\":null}";
                return new HttpResponseMessage { Content = new StringContent(returnStr, Encoding.UTF8, "application/json") };
            }

            string WeekBeginDate = string.Empty;
            try
            {
                WeekBeginDate = GetJObj["Date"].ToString();
            }
            catch { }
            if (string.IsNullOrWhiteSpace(WeekBeginDate))
            {
                returnStr = "{\"code\":3201,\"msg\":\"[SLC]缺少参数：WeekBeginDate\",\"data\":null}";
                return new HttpResponseMessage { Content = new StringContent(returnStr, Encoding.UTF8, "application/json") };
            }
            #endregion

            // 校验客户AccessToken
            DataTable dtCusInfo = op.CheckCusAccessToken(AccessToken);
            if (dtCusInfo.Rows.Count > 0)
            {
                DataRow drCusInfo = dtCusInfo.Rows[0];
                string FCusAppKey = drCusInfo["FCusAppKey"].ToString();

                // 读取对接平台信息
                DataRow drPlatform;
                try
                {
                    drPlatform = op.GetPlatformInfo(CusPlatformID).Rows[0];
                }
                catch
                {
                    returnStr = "{\"code\":3202,\"msg\":\"[SLC]对接平台ID无效\",\"data\":null}";
                    return new HttpResponseMessage { Content = new StringContent(returnStr, Encoding.UTF8, "application/json") };
                }

                // 判断所要对接的平台是环球渔具还是其他平台
                if (drPlatform["FName"].ToString().IndexOf("环球渔具") >= 0)
                {
                    string AccessToken_HQ = HQ.GetAccessToken(FCusAppKey, CusPlatformID);
                    if (!string.IsNullOrWhiteSpace(AccessToken_HQ))
                    {
                        // 调用环球渔具平台接口读取周计划
                        string res = http.Post(drPlatform["FAPIUrl"].ToString() + "/API/Plan/getWeekPlan", "{\"AppKey\":\"" + drCusInfo["FAppKey_HQ"] + "\",\"AccessToken\": \"" + AccessToken_HQ + "\",\"Date\":\"" + WeekBeginDate + "\"}");
                        if (!string.IsNullOrWhiteSpace(res))
                        {
                            return new HttpResponseMessage { Content = new StringContent(res, Encoding.UTF8, "application/json") };
                        }
                        else
                        {
                            returnStr = "{\"code\":3205,\"msg\":\"[SLC]环球渔具GFG平台未返回任何数据\",\"data\":null}";
                            return new HttpResponseMessage { Content = new StringContent(returnStr, Encoding.UTF8, "application/json") };
                        }
                    }
                    else
                    {
                        returnStr = "{\"code\":3204,\"msg\":\"[SLC]环球渔具GFG平台AccessToken获取失败\",\"data\":null}";
                        return new HttpResponseMessage { Content = new StringContent(returnStr, Encoding.UTF8, "application/json") };
                    }
                }
                else
                {
                    returnStr = "{\"code\":3203,\"msg\":\"[SLC]此平台尚未开通数据对接服务\",\"data\":null}";
                    return new HttpResponseMessage { Content = new StringContent(returnStr, Encoding.UTF8, "application/json") };
                }
            }
            else
            {
                returnStr = "{\"code\":3202,\"msg\":\"[SLC]AccessToken无效\",\"data\":null}";
                return new HttpResponseMessage { Content = new StringContent(returnStr, Encoding.UTF8, "application/json") };
            }
        }
        #endregion
    }
}