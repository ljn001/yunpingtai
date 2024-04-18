using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Configuration;

namespace CloudPlatform.Code.wx
{
    public class WXApi
    {
        private readonly Logger log = LogManager.GetCurrentClassLogger();

        cls_SQLOperate op = new cls_SQLOperate();

        // 获取微信用户OpenID
        public string GetWXOpenID(string appid, string secret, string Code)
        {
            string html = string.Empty;
            string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + appid + "&secret=" + secret + "&code=" + Code + "&grant_type=authorization_code";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream ioStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(ioStream, Encoding.UTF8);
            html = sr.ReadToEnd();
            sr.Close();
            ioStream.Close();
            response.Close();
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

        // 获取微信AccessToken
        public string GetAccessToken()
        {
            DataTable dt = op.GetWXAccessToken();
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["FToken"].ToString();
            }
            else
            {
                string appid = ConfigurationManager.AppSettings["appid"];
                string secret = ConfigurationManager.AppSettings["appsecret"];
                string WxTokenUrl = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, secret);
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(WxTokenUrl);
                req.Method = "GET";
                HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string content = reader.ReadToEnd();
                log.Info("GetWXAccessTokenRes：" + content); //记录日志
                if (content.IndexOf("access_token") >= 0) //如果可以返回正常结果则返回
                {
                    try
                    {
                        JObject joRes = JObject.Parse(content);

                        // 计算失效时间（有效时间扣减1800秒，防误差）
                        DateTime dtExpired = DateTime.Now.AddSeconds(int.Parse(joRes["expires_in"].ToString()) - 1800);

                        // 将AccessToken缓存于数据库中
                        bool ok = op.UpdateWXAccessToken(joRes["access_token"].ToString(), dtExpired.ToString());
                        if (ok)
                        {
                            return joRes["access_token"].ToString();
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
                else
                {
                    return null;
                }
            }
        }

        // 获取微信Ticket
        public string GetTicket(string AccessToken)
        {
            DataTable dt = op.GetWXTicket();
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["FTicket"].ToString();
            }
            else
            {
                string WxTicketUrl = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", AccessToken);
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(WxTicketUrl);
                req.Method = "GET";
                HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string content = reader.ReadToEnd();
                log.Info("GetWXTicketRes：" + content); //记录日志
                if (content.IndexOf("ticket") >= 0) //如果可以返回正常结果则返回
                {
                    try
                    {
                        JObject joRes = JObject.Parse(content);

                        // 计算失效时间（有效时间扣减1800秒，防误差）
                        DateTime dtExpired = DateTime.Now.AddSeconds(int.Parse(joRes["expires_in"].ToString()) - 1800);

                        // 将AccessToken缓存于数据库中
                        bool ok = op.UpdateWXTicket(joRes["ticket"].ToString(), dtExpired.ToString());
                        if (ok)
                        {
                            return joRes["ticket"].ToString();
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
                else
                {
                    return null;
                }
            }
        }

        #region 生成随机字符串（NonceStr）
        /// <summary>
        /// 随机字符串数组集合
        /// </summary>
        private readonly string[] NonceStrings = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

        /// <summary>
        /// 生成签名的随机串
        /// </summary>
        /// <returns></returns>
        public string CreateNonceStr()
        {
            Random random = new Random();
            var sb = new StringBuilder();
            var length = NonceStrings.Length;

            //生成15位数的随机字符串，当然也可以通过控制对应字符串大小生成，但是至多不超过32位
            for (int i = 0; i < 15; i++)
            {
                sb.Append(NonceStrings[random.Next(length - 1)]);//通过random获得的随机索引到，NonceStrings数组中获取对应数组值
            }
            return sb.ToString();
        }
        #endregion

        #region 生成时间戳（TimeSpan）
        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <returns></returns>
        public long GetTimeSpan()
        {
            DateTime currentDate = DateTime.Now;//当前时间
            //转化为时间戳
            DateTime localTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return long.Parse((currentDate - localTime).TotalSeconds.ToString().Split('.')[0]);
        }
        #endregion

        #region 模板消息
        // 发送微信模板消息通用方法
        public JObject SendWXMsg(string strJson)
        {
            try
            {
                string url = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + GetAccessToken();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/json";
                request.Method = "POST";
                request.Timeout = 10000;

                byte[] bytes = Encoding.UTF8.GetBytes(strJson);
                request.ContentLength = bytes.Length;
                Stream writer = request.GetRequestStream();
                writer.Write(bytes, 0, bytes.Length);
                writer.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException(), Encoding.UTF8);
                string result = reader.ReadToEnd();
                response.Close();

                // 将JSON转化为JObject
                JObject JORes = JObject.Parse(result);
                return JORes;
            }
            catch
            {
                return null;
            }
        }

        // 订单进度提醒
        public JObject WXMsg_OrderProgessReminder(string UserOpenID, string OrderID_E, string OrderNo, string FirstMsg, string OrderState, string StateTime, string Remark)
        {
            try
            {
                // 获取服务器域名
                string DomainName = WebConfigurationManager.AppSettings["DomainName"].ToString();

                StringBuilder str = new StringBuilder();
                str.Append("{\"touser\":\"" + UserOpenID + "\",");
                str.Append("\"template_id\":\"6qVUVUtCfU3U4r8zW05ZsQRs-kqQlWwl1PriyoLQMd0\",");
                str.Append("\"url\":\"http://" + DomainName + "/Mobile/OrderInfo.aspx?oid=" + OrderID_E + "\",");
                str.Append("\"data\":{");
                str.Append("\"first\":{\"value\":\"" + FirstMsg + "\"},");
                str.Append("\"keyword1\":{\"value\":\"" + OrderNo + "\"},");
                str.Append("\"keyword2\":{\"value\":\"洗护服务\"},");
                str.Append("\"keyword3\":{\"value\":\"" + OrderState + "\"},");
                str.Append("\"keyword4\":{\"value\":\"" + StateTime + "\"},");
                str.Append("\"remark\":{\"value\":\"" + Remark + "\"}}}");

                return SendWXMsg(str.ToString());
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}