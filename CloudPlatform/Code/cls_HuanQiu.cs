using Newtonsoft.Json.Linq;
using System.Data;

namespace CloudPlatform.Code
{
    public class cls_HuanQiu
    {
        cls_SQLOperate op = new cls_SQLOperate();
        cls_HttpHelper http = new cls_HttpHelper();

        #region 获取AccessToken
        public string GetAccessToken(string CusAppKey, string PlatformID)
        {
            DataTable dt = op.GetPlatformToken(CusAppKey, PlatformID);
            if (dt.Rows.Count > 0)
            {
                // 校验本地AccessToken是否仍有效（测试调用）
                // 读取平台信息
                DataTable dtPlatform = op.GetPlatformInfo(PlatformID);
                if (dtPlatform.Rows.Count > 0)
                {
                    DataRow drPF = dtPlatform.Rows[0];
                    string res = http.Post(drPF["FAPIUrl"].ToString() + "/API/Auth/checkToken", "{\"AccessToken\": \"" + dt.Rows[0]["FAccessToken"].ToString() + "\"}");
                    if (!string.IsNullOrWhiteSpace(res))
                    {
                        JObject jo = JObject.Parse(res);
                        if (jo["code"].ToString() == "0")
                        {
                            return dt.Rows[0]["FAccessToken"].ToString();
                        }
                        else
                        {
                            // 读取客户信息
                            DataRow drCusInfo = op.GetCustomerInfo(CusAppKey).Rows[0];

                            // 连接环球渔具平台获取AccessToken
                            string resToken = http.Post(drPF["FAPIUrl"].ToString() + "/API/Auth/getToken", "{\"AppKey\": \"" + drCusInfo["FAppKey_HQ"].ToString() + "\",\"AppSecret\": \"" + drCusInfo["FAppSecret_HQ"].ToString() + "\"}");
                            if (!string.IsNullOrWhiteSpace(resToken))
                            {
                                JObject joToken = JObject.Parse(resToken);
                                if (joToken["code"].ToString() == "0")
                                {
                                    string AccessToken = joToken["data"]["accessToken"].ToString();
                                    // 取得环球渔具平台AccessToken，并更新改客户对应的鉴权信息
                                    bool ok = op.UpdatePlatformToken(CusAppKey, PlatformID, AccessToken, joToken["data"]["ExpTime"].ToString());
                                    if (ok)
                                    {
                                        return AccessToken;
                                    }
                                    else
                                    {
                                        return null;
                                    }
                                }
                                else
                                {
                                    return null;
                                }
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                // 根据客户CusAppKey获取其环球平台的AppKey和Secret
                try
                {
                    // 读取客户信息
                    DataRow drCusInfo = op.GetCustomerInfo(CusAppKey).Rows[0];

                    // 读取平台信息
                    DataRow drPlatform = op.GetPlatformInfo(PlatformID).Rows[0];

                    // 连接环球渔具平台获取AccessToken
                    string resToken = http.Post(drPlatform["FAPIUrl"].ToString() + "/API/Auth/getToken", "{\"AppKey\": \"" + drCusInfo["FAppKey_HQ"].ToString() + "\",\"AppSecret\": \"" + drCusInfo["FAppSecret_HQ"].ToString() + "\"}");
                    if (!string.IsNullOrWhiteSpace(resToken))
                    {
                        JObject jo = JObject.Parse(resToken);
                        if (jo["code"].ToString() == "0")
                        {
                            string AccessToken = jo["data"]["accessToken"].ToString();
                            // 取得环球渔具平台AccessToken，并更新改客户对应的鉴权信息
                            bool ok = op.UpdatePlatformToken(CusAppKey, PlatformID, AccessToken, jo["data"]["ExpTime"].ToString());
                            if(ok)
                            {
                                return AccessToken;
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                catch
                {
                    return null;
                }
            }
        }
        #endregion
    }
}