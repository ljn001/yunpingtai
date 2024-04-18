using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CloudPlatform.Code;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace CloudPlatform.API
{
    // 电视大屏APP接口
    public class TVController : ApiController
    {
        cls_SQLOperate op = new cls_SQLOperate();
        cls_SQLOperateCusPJ oppj = new cls_SQLOperateCusPJ();
        cls_SQLOperateCusYG opyg = new cls_SQLOperateCusYG();

        #region 获取菜单项接口
        [HttpPost]
        public HttpResponseMessage getTVMenu([FromBody] JObject GetJObj)
        {
            string returnStr = string.Empty;

            string IMEI = string.Empty;
            try
            {
                IMEI = GetJObj["IMEI"].ToString();
            }
            catch 
            {
                returnStr = "{\"code\":1001,\"msg\":\"设备ID解析失败\",\"data\":null}";
            }

            if (!string.IsNullOrWhiteSpace(IMEI))
            {
                DataTable dtCusInfo = op.GetCusAppKeyByEQIMEI(IMEI);
                if (dtCusInfo.Rows.Count > 0)
                {
                    if (!string.IsNullOrWhiteSpace(dtCusInfo.Rows[0]["FCusAppKey"].ToString()))
                    {
                        if (dtCusInfo.Rows[0]["FSysType"].ToString() == "配件系统")
                        {
                            DataTable dtScreen = oppj.GetScreenClass(dtCusInfo.Rows[0]["FDBConnStr"].ToString());
                            returnStr = "{\"code\":0,\"msg\":\"\",\"url\":\"" + dtCusInfo.Rows[0]["FTVUrl"].ToString() + "\",\"appKey\":\"" + dtCusInfo.Rows[0]["FCusAppKey"].ToString() + "\",\"MenuData\":" + JsonConvert.SerializeObject(dtScreen) + "}";
                        }
                        else if (dtCusInfo.Rows[0]["FSysType"].ToString() == "鱼竿系统")
                        {
                            DataTable dtScreen = opyg.GetScreenClassYG(dtCusInfo.Rows[0]["FDBConnStr"].ToString());
                            returnStr = "{\"code\":0,\"msg\":\"\",\"url\":\"" + dtCusInfo.Rows[0]["FTVUrl"].ToString() + "\",\"appKey\":\"" + dtCusInfo.Rows[0]["FCusAppKey"].ToString() + "\",\"MenuData\":" + JsonConvert.SerializeObject(dtScreen) + "}";
                        }
                        else
                        {
                            returnStr = "{\"code\":1004,\"msg\":\"系统类型错误\",\"data\":null}";
                        }
                    }
                    else
                    {
                        returnStr = "{\"code\":1004,\"msg\":\"此设备未授权或已禁用\",\"data\":null}";
                    }
                }
                else
                {
                    returnStr = "{\"code\":1003,\"msg\":\"此设备未授权\",\"data\":null}";
                }
            }
            else
            {
                returnStr = "{\"code\":1002,\"msg\":\"服务器未识别到设备ID\",\"data\":null}";
            }

            return new HttpResponseMessage { Content = new StringContent(returnStr, Encoding.UTF8, "application/json") };
        }
        #endregion

        #region 获取版本更新信息接口
        [HttpGet]
        public HttpResponseMessage getTVAppVer() 
        {
            string returnStr = string.Empty;

            DataTable dt = op.GetTVAppVer();
            if (dt.Rows.Count > 0)
            {
                returnStr = "{\"Ver\":\"" + dt.Rows[0]["FVer"].ToString() + "\",\"VerCode\":" + dt.Rows[0]["FVerCode"].ToString() + ",\"AppUrl\":\"" + dt.Rows[0]["FAppUrl"].ToString() + "\"}";
            }
            else
            {
                returnStr = "{\"Ver\":\"0.0.0.0\",\"VerCode\":0,\"AppUrl\":\"\"}";
            }

            return new HttpResponseMessage { Content = new StringContent(returnStr, Encoding.UTF8, "application/json") };
        }
        #endregion

        #region 获取客户大屏网址
        [HttpGet]
        public HttpResponseMessage getTVUrl(string AppKey)
        {
            string returnStr = string.Empty;

            DataTable dt = op.GetCustomerInfo(AppKey);
            if (dt.Rows.Count > 0)
            {
                returnStr = "{\"TVUrl\":\"" + dt.Rows[0]["FTVUrl"].ToString() + "\"}";
            }
            else
            {
                returnStr = "{\"TVUrl\":\"\"}";
            }

            return new HttpResponseMessage { Content = new StringContent(returnStr, Encoding.UTF8, "application/json") };
        }
        #endregion
    }
}