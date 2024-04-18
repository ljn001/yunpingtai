using CloudPlatform.Code;
using System.Web;
using System.Web.Configuration;
using CloudPlatform.Code.wx;

namespace CloudPlatform
{
    /// <summary>
    /// GetWXOpenID 的摘要说明
    /// </summary>
    public class GetWXOpenID : IHttpHandler
    {
        WXApi wx = new WXApi();

        // 公用获取微信OpenID
        // Cookie名：XiYe_APP_WXOpenID，存储微信OpenID
        // Cookie名：XiYe_APP_RedirectUrl，存储调用页面URL，获取OpenID后自动跳转回原页面

        public void ProcessRequest(HttpContext context)
        {
            string WXCode;
            try
            {
                WXCode = context.Request["code"].ToString();
            }
            catch
            {
                WXCode = string.Empty;
            }

            if (string.IsNullOrEmpty(WXCode))
            {
                // 从Config文件中获取回调域名，防止域名变更修改代码
                string DomainName = WebConfigurationManager.AppSettings["DomainName"].ToString();
                string redirect_uri = "http%3A%2F%2F" + DomainName + "%2FGetWXOpenID.ashx";

                context.Response.Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid="
                    + WebConfigurationManager.AppSettings["appid"].ToString() + "&redirect_uri=" + redirect_uri + "&response_type=code&scope=snsapi_base&state=state#wechat_redirect");
            }
            else
            {
                // 根据Code获取OpenID
                string WXOpenID = wx.GetWXOpenID(WebConfigurationManager.AppSettings["appid"].ToString(), WebConfigurationManager.AppSettings["appsecret"].ToString(), WXCode);
                // 将OpenID存入Cookie中
                CookieHelper.WriteCookie("CloudPlatform_APP_WXOpenID", WXOpenID);
                // 跳转回原调用页面
                context.Response.Redirect(CookieHelper.GetCookie("CloudPlatform_APP_RedirectUrl"));
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}