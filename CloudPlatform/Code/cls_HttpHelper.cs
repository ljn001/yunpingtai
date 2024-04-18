using System.IO;
using System.Net;
using System.Text;

namespace CloudPlatform.Code
{
    public class cls_HttpHelper
    {
        // Http GET方法
        public static string Get(string Url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Proxy = null;
            request.KeepAlive = false;
            request.Method = "GET";
            request.ContentType = "application/json;charset=UTF-8";
            request.AutomaticDecompression = DecompressionMethods.GZip;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();

            myStreamReader.Close();
            myResponseStream.Close();

            if (response != null)
            {
                response.Close();
            }
            if (request != null)
            {
                request.Abort();
            }
            return retString;
        }

        // Http POST方法
        public string Post(string Url, string Data)
        {
            try
            {
                byte[] bytes = Encoding.Default.GetBytes(Data);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Proxy = null;
                request.KeepAlive = false;
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = bytes.Length;
                request.ProtocolVersion = HttpVersion.Version10;
                Stream myResponseStream = request.GetRequestStream();
                myResponseStream.Write(bytes, 0, bytes.Length);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader myStreamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string resStr = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
                return resStr;
            }
            catch
            {
                return null;
            }
        }
    }
}