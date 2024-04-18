using System;
using System.Collections.Generic;
using Tea;
using AlibabaCloud.SDK.Dysmsapi20170525;
using AlibabaCloud.SDK.Dysmsapi20170525.Models;

namespace CloudPlatform.Code
{
    // 短信服务类
    public class cls_SMS
    {
        cls_SQLOperate op = new cls_SQLOperate();

        /**
        * 使用AK&SK初始化账号Client
        * @param accessKeyId
        * @param accessKeySecret
        * @return Client
        * @throws Exception
        */
        public static Client CreateClient(string accessKeyId, string accessKeySecret)
        {
            AlibabaCloud.OpenApiClient.Models.Config config = new AlibabaCloud.OpenApiClient.Models.Config
            {
                // 必填，您的 AccessKey ID
                AccessKeyId = accessKeyId,
                // 必填，您的 AccessKey Secret
                AccessKeySecret = accessKeySecret,
            };
            // 访问的域名
            config.Endpoint = "dysmsapi.aliyuncs.com";
            return new Client(config);
        }

        // 发送短信
        public string SendVerifyCode(string PhoneNumber, string VerifyCode, string WXOpenID = null)
        {
            if (PhoneNumber.Length == 11)
            {
                Client client = CreateClient("accessKeyId", "accessKeySecret");
                SendSmsRequest sendSmsRequest = new SendSmsRequest
                {
                    PhoneNumbers = PhoneNumber,
                    SignName = "短信签名",
                    TemplateCode = "短信模板Code",
                    TemplateParam = "{\"code\":\"" + VerifyCode + "\"}",
                };
                AlibabaCloud.TeaUtil.Models.RuntimeOptions runtime = new AlibabaCloud.TeaUtil.Models.RuntimeOptions();
                try
                {
                    // 复制代码运行请自行打印 API 的返回值
                    SendSmsResponse ssr = client.SendSmsWithOptions(sendSmsRequest, runtime);

                    // 记录短信发送日志
                    op.AddLogSMS(PhoneNumber, "验证码", WXOpenID, ssr.Body.Code, ssr.Body.Message, ssr.Body.BizId, ssr.Body.RequestId);

                    if (ssr.Body.Code == "OK")
                    {
                        return "OK";
                    }
                    else
                    {
                        return ssr.Body.Message;
                    }
                }
                catch (TeaException error)
                {
                    // 如有需要，请打印 error
                    return AlibabaCloud.TeaUtil.Common.AssertAsString(error.Message);
                }
                catch (Exception _error)
                {
                    TeaException error = new TeaException(new Dictionary<string, object>
                    {
                        { "message", _error.Message }
                    });
                    // 如有需要，请打印 error
                    return AlibabaCloud.TeaUtil.Common.AssertAsString(error.Message);
                }
            }
            else
            {
                return "手机号无效！";
            }
        }
    }
}