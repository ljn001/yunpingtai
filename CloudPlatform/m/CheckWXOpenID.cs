using System.Configuration;
using System.Web;
using System.Web.Configuration;
using CloudPlatform.Code;

namespace CloudPlatform.Mobile
{
    public class CheckWXOpenID
    {
        public string Check(string RedirectUrl)
        {
            string WXOpenID = CookieHelper.GetCookie("CloudPlatform_APP_WXOpenID");
            if (string.IsNullOrEmpty(WXOpenID))
            {
                // 是否是调试模式（调试模式直接返回开发者微信OpenID，不调用腾讯API，方便测试）
                bool IsDebug = true;
                try
                {
                    IsDebug = bool.Parse(WebConfigurationManager.AppSettings["DebugMode"].ToString());
                }
                catch { }

                if (IsDebug)
                {
                    return "oDI_X5uvazB-bUxiuE2BvQ_9JQaY";
                }
                else
                {
                    CookieHelper.WriteCookie("CloudPlatform_APP_RedirectUrl", RedirectUrl);
                    HttpContext.Current.Response.Redirect("/GetWXOpenID.ashx");
                    return null;
                }
            }
            else
            {
                return WXOpenID;
            }
        }
    }
}