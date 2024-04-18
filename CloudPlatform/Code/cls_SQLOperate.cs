using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;

namespace CloudPlatform.Code
{
    public class cls_SQLOperate
    {
        cls_SQLFunction function = new cls_SQLFunction();

        #region 通用
        // 使用事物处理SQL语句
        public DataTable DoSqlTRAN(string SQL)
        {
            // 启用事务
            string SQL_Begin = "BEGIN TRAN BEGIN TRY ";
            string SQL_End = " COMMIT TRAN END TRY BEGIN CATCH ROLLBACK TRAN DECLARE @ErrorMsg nvarchar(4000) SET @ErrorMsg=(select ERROR_MESSAGE()) RAISERROR(@ErrorMsg,16,1) END CATCH";
            return function.SQLReadData(SQL_Begin + SQL + SQL_End);
        }
        #endregion

        #region 用户管理
        // 读取用户信息（用户登录）
        public DataTable ReadUserInfo_Login(string FUserID)
        {
            return function.SQLReadData("SELECT * FROM tUsers WHERE FUserID='" + FUserID + "'");
        }

        // 更新用户密码
        public bool ChangeUserPassword(string FUserID, string NewPassword)
        {
            return function.SQLUpdate("UPDATE tUsers SET FPassword='" + NewPassword + "' WHERE FUserID='" + FUserID + "'");
        }

        // 读取用户列表
        public DataTable ReadUserList(string Search)
        {
            string SQL = string.Format(@"SELECT * FROM tUsers WHERE FUserID LIKE '%{0}%' OR FUserName LIKE '%{0}%'", Search);
            return function.SQLReadData(SQL);
        }

        // 添加用户
        public bool AddUser(string FUserID, string FUserName, string FPassword, string FEnabled)
        {
            return function.SQLUpdate("INSERT INTO tUsers(FUserID,FUserName,FPassword,FEnabled)VALUES('" + FUserID + "','" + FUserName + "','" + FPassword + "','" + FEnabled + "')");
        }

        // 更新用户
        public bool UpdateUser(string FUserID, string FUserName, string FPassword, string FEnabled)
        {
            return function.SQLUpdate("UPDATE tUsers SET FUserName='" + FUserName + "',FPassword='" + FPassword + "',FEnabled='" + FEnabled + "' WHERE FUserID='" + FUserID + "'");
        }

        // 读取用户权限
        public DataTable ReadUserPower(string FUserID)
        {
            return function.SQLReadData("SELECT * FROM tUsersPower WHERE FUserID='" + FUserID + "'");
        }

        // 刷新应用Token
        public bool RefreshCustomerToken(string FCusAppKey, string FAccessToken, string FExpTime)
        {
            string SQL = string.Format(@"IF EXISTS(SELECT 1 FROM tCustomerToken WHERE FCusAppKey='{0}') UPDATE tCustomerToken SET FAccessToken='{1}',FExpTime='{2}' WHERE FCusAppKey='{0}' ELSE INSERT INTO tCustomerToken(FCusAppKey,FAccessToken,FExpTime)VALUES('{0}','{1}','{2}')", FCusAppKey, FAccessToken, FExpTime);
            return function.SQLUpdate(SQL);
        }

        // 校验客户AccessToken是否有效
        public DataTable CheckCusAccessToken(string AccessToken)
        {
            return function.SQLReadData("SELECT tCT.FCusAppKey,tCus.FName,tCus.FNameShort,tCus.FAppKey_HQ,tCus.FAppSecret_HQ FROM tCustomerToken tCT LEFT JOIN dbo.tCustomer tCus ON tCT.FCusAppKey=tCus.FAppKey WHERE tCT.FAccessToken='" + AccessToken + "' AND tCT.FExpTime>=GETDATE()");
        }
        #endregion

        #region 客户管理
        // 添加客户信息
        public bool AddCustomer(string FAppKey, string FName, string FNameShort, string FCode, string FTel, string FAddress, string FAppSecret, string FEnabled, string FAppKey_HQ, string FAppSecret_HQ, string FDBLink, string FDBUser, string FDBPwd, string FDBName, string FDBConnStr, string FSysType)
        {
            return function.SQLUpdate("INSERT INTO tCustomer(FAppKey,FName,FNameShort,FCode,FTel,FAddress,FAppSecret,FEnabled,FAppKey_HQ,FAppSecret_HQ,FDBLink,FDBUser,FDBPwd,FDBName,FDBConnStr,FSysType)VALUES('"
                + FAppKey + "','" + FName + "','" + FNameShort + "','" + FCode + "','" + FTel + "','" + FAddress + "','" + FAppSecret + "','" + FEnabled + "','" + FAppKey_HQ + "','" + FAppSecret_HQ + "','" + FDBLink + "','" + FDBUser + "','" + FDBPwd + "','" + FDBName + "','" + FDBConnStr + "','" + FSysType + "')");
        }

        // 更新客户信息（删除该客户已存在的AccessToken）
        public bool UpdateCustomer(string FAppKey, string FName, string FNameShort, string FCode, string FTel, string FAddress, string FAppSecret, string FEnabled, string FAppKey_HQ, string FAppSecret_HQ, string FDBLink, string FDBUser, string FDBPwd, string FDBName, string FDBConnStr, string FSysType)
        {
            return function.SQLUpdate("DELETE FROM tCustomerToken WHERE FCusAppKey='" + FAppKey + "' UPDATE tCustomer SET FName='" + FName + "',FNameShort='" + FNameShort + "',FCode='" + FCode + "',FTel='" + FTel + "',FAddress='" + FAddress + "',FAppSecret='" + FAppSecret
                + "',FEnabled='" + FEnabled + "',FAppKey_HQ='" + FAppKey_HQ + "',FAppSecret_HQ='" + FAppSecret_HQ + "',FDBLink='" + FDBLink + "',FDBUser='" + FDBUser + "',FDBPwd='" + FDBPwd + "',FDBName='" + FDBName + "',FDBConnStr='" + FDBConnStr + "',FSysType='" + FSysType + "' WHERE FAppKey='" + FAppKey + "'");
        }

        // 读取客户列表
        public DataTable GetCustomerList(string Search)
        {
            return function.SQLReadData("SELECT * FROM tCustomer WHERE FName LIKE '%" + Search + "%' ORDER BY FName");
        }

        // 读取客户详情
        public DataTable GetCustomerInfo(string FAppKey)
        {
            return function.SQLReadData("SELECT * FROM tCustomer WHERE FAppKey='" + FAppKey + "'");
        }
        public DataTable GetCustomerInfoByName(string FCusName)
        {
            return function.SQLReadData("SELECT * FROM tCustomer WHERE FName='" + FCusName + "'");
        }

        // 读取客户鉴权信息
        public DataTable GetCustomerAuth(string FAppKey)
        {
            return function.SQLReadData("SELECT FAppKey,FAppSecret FROM tCustomer WHERE FAppKey='" + FAppKey + "'");
        }

        // 读取客户授权平台列表
        public DataTable GetCustomerPlatform(string FCusAppKey)
        {
            return function.SQLReadData("SELECT * FROM tCustomerPlatform WHERE FCusAppKey='" + FCusAppKey + "'");
        }

        // 删除客户信息
        public bool DeleteCustomer(string FAppKey)
        {
            return function.SQLUpdate("DELETE FROM tCustomerPlatform WHERE FCusAppKey='" + FAppKey + "' DELETE FROM tCustomer WHERE FAppKey='" + FAppKey + "'");
        }

        // 添加客户用户微信绑定信息并记录日志
        public bool AddCustomerUser(string FCusAppKey, string FCusUserID, string FCusUserName, string FCusUserPhone, string FWXOpenID)
        {
            string SQL = string.Format(@"INSERT INTO tCustomerUser(FCusAppKey,FCusUserID,FCusUserName,FCusUserPhone,FWXOpenID,FBindTime)VALUES('{0}','{1}','{2}','{3}','{4}',GETDATE())
                INSERT INTO tCustomerUserLog(FCusAppKey,FCusUserID,FCusUserName,FCusUserPhone,FWXOpenID,FTime,FState)VALUES('{0}','{1}','{2}','{3}','{4}',GETDATE(),'绑定')", FCusAppKey, FCusUserID, FCusUserName, FCusUserPhone, FWXOpenID);
            return function.SQLUpdate(SQL);
        }
      //修改职工信息
        public bool Updateruzhixinxi( string FCusUserID,  string FWXOpenID, string FCusUserName,string Fht,string Fbu)
        {
            
            string SQL = string.Format(@"UPDATE zhigongxinxi SET FWXOpenID='{0}',wxdate=getdate() WHERE gonghao='{1}' 
                            INSERT INTO tCustomerUserLog(ht,gonghao,xingming,bumen,wxopenid,wxdate,fdate,fstate) VALUES('{0}','{1}','{2}','{3}','{4}',GETDATE(),GETDATE(),'绑定')", Fht, FCusUserID, FCusUserName,Fbu,  FWXOpenID);
            return function.SQLUpdate(SQL, "人员");
        }
        // 查询客户用户微信绑定信息
        public DataTable GetCustomerUserBindInfo(string DBConStr, string FCusUserID)
        {
            return function.SQLReadData("SELECT gonghao,FWXOpenID,xingming,lianxifangshi,hetongbianhao,left(bumendaihao,3)+bumen as bumen,bumendaihao,gangwei,gw_id FROM zhigongxinxi WHERE gonghao<>'' AND (leave='未' or leave='在职') AND gonghao='" + FCusUserID + "'", DBConStr);
        }
        public DataTable GetCustomerUserBindInfoxinxi(string Fname, string Fphoto, string DBConStr = "人员")
        {
            return function.SQLReadData("SELECT gonghao,FWXOpenID,xingming,lianxifangshi,hetongbianhao,left(bumendaihao,3)+bumen as bumen,bumendaihao,gangwei,gw_id FROM zhigongxinxi WHERE gonghao<>'' and lianxifangshi='" + Fphoto + "' AND (leave='未' or leave='在职') AND xingming='" + Fname + "'  ", DBConStr);
        }
        // 查询某个微信号已绑定的客户账号列表

        public DataTable GetCustomerUserBind(string FWXOpenID, string DBConStr = "人员")
        {
            //string SearchCus = string.Empty;
            //if(!string.IsNullOrWhiteSpace(FAppKey))
            //{
            //    SearchCus = " AND tCus.FAppKey='" + FAppKey + "'";
            //}

            //string SQL = string.Format(@"SELECT tCusU.FID,tCus.FAppKey,tCus.FName AS FCusName,tCusU.FCusUserID,tCusU.FCusUserName,tCusU.FCusUserPhone,tCusU.FBindTime,
            //    tCus.FDBLink,tCus.FDBUser,tCus.FDBPwd,tCus.FDBName,tCus.FDBConnStr,tCusU.FDefault
            //    FROM tCustomerUser tCusU
            //    LEFT JOIN tCustomer tCus ON tCus.FAppKey=tCusU.FCusAppKey
            //    WHERE tCusU.FWXOpenID='{0}' {1}
            //    ORDER BY tCusU.FBindTime", FWXOpenID, SearchCus);
            string SQL = string.Format(@"SELECT gonghao,FWXOpenID,xingming,lianxifangshi,hetongbianhao,left(bumendaihao,3)+bumen as bumen,wxdate,left(bumendaihao,3) as bumendaihao,gangwei,gw_id FROM zhigongxinxi WHERE gonghao<>'' AND (leave='未' or leave='在职')  and FWXOpenID='{0}' 
                ORDER BY wxdate", FWXOpenID);
            return function.SQLReadData(SQL, DBConStr);
        }

        // 查询某个微信号已绑定的客户账号列表
        public DataTable GetCustomerUserBindDefault(string FWXOpenID, string DBConStr = "人员")
        {
            //string SQL = string.Format(@"IF EXISTS(SELECT 1 FROM tCustomerUser WHERE FWXOpenID='{0}' AND FDefault=1)
            //    BEGIN
            //        SELECT TOP 1 tCusU.FID,tCus.FAppKey,tCus.FName AS FCusName,tCusU.FCusUserID,tCusU.FCusUserName,tCusU.FCusUserPhone,tCusU.FBindTime,
            //        tCus.FDBLink,tCus.FDBUser,tCus.FDBPwd,tCus.FDBName,tCus.FDBConnStr
            //        FROM tCustomerUser tCusU
            //        LEFT JOIN tCustomer tCus ON tCus.FAppKey=tCusU.FCusAppKey
            //        WHERE tCusU.FWXOpenID='{0}' AND tCusU.FDefault=1
            //        ORDER BY tCusU.FBindTime
            //    END
            //    ELSE BEGIN
            //        SELECT TOP 1 tCusU.FID,tCus.FAppKey,tCus.FName AS FCusName,tCusU.FCusUserID,tCusU.FCusUserName,tCusU.FCusUserPhone,tCusU.FBindTime,
            //        tCus.FDBLink,tCus.FDBUser,tCus.FDBPwd,tCus.FDBName,tCus.FDBConnStr
            //        FROM tCustomerUser tCusU
            //        LEFT JOIN tCustomer tCus ON tCus.FAppKey=tCusU.FCusAppKey
            //        WHERE tCusU.FWXOpenID='{0}'
            //        ORDER BY tCusU.FBindTime
            //    END", FWXOpenID);
            string SQL = string.Format(@"SELECT top 1 gonghao,FWXOpenID,xingming,lianxifangshi,hetongbianhao,left(bumendaihao,3)+bumen  as bumen,left(bumendaihao,3) as bumendaihao,gangwei,gw_id FROM zhigongxinxi WHERE gonghao<>'' AND (leave='未' or leave='在职') AND FWXOpenID='{0}'  order by ruchangshijian desc ", FWXOpenID);
            return function.SQLReadData(SQL, DBConStr);
        }
       //取报工数据库
        public DataTable GetCustomerUserBindDB(string Gongid)
        {
            
            string SQL = string.Format(@"SELECT * from tCustomer WHERE  FEnabled=1 and  FNameShort='{0}'  ", Gongid);
            return function.SQLReadData(SQL);
        }
        // 删除客户用户微信绑定信息并记录日志
        public bool DeleteCustomerUser(string FID, string DBConStr = "人员")
        {
            string SQL = string.Format(@"INSERT INTO tCustomerUserLog(ht,gonghao,xingming,bumen,wxopenid,wxdate,fdate,fstate) SELECT hetongbianhao,gonghao,xingming,bumen,FWXOpenID,wxdate,getdate(),'解绑' FROM zhigongxinxi WHERE hetongbianhao={0}
                update  zhigongxinxi set FWXOpenID=''  WHERE hetongbianhao={0}", FID);
            return function.SQLUpdate(SQL, DBConStr);
        }

        // 设置用户微信默认单位,20240207修改直接去职工信息无需设置默认单位
        public bool SetCustomerUserDefault(string FWXOpenID, string DefaultCUID)
        {
            string SQL = string.Format(@"UPDATE tCustomerUser SET FDefault=1 WHERE FWXOpenID='{0}' AND FID={1}
                UPDATE tCustomerUser SET FDefault=0 WHERE FWXOpenID='{0}' AND FID<>{1}", FWXOpenID, DefaultCUID);
            return function.SQLUpdate(SQL);
        }
        #endregion

        #region 客户电视大屏管理
        // 根据设备IMEI获取客户AppKey
        public DataTable GetCusAppKeyByEQIMEI(string EQ_IMEI)
        {
            string SQL = string.Format(@"IF NOT EXISTS(SELECT 1 FROM tCustomerTV WHERE FIMEI='{0}')
                BEGIN INSERT INTO tCustomerTV(FIMEI,FAddTime,FEnabled)VALUES('{0}',GETDATE(),1) END
                ELSE BEGIN
                SELECT tTV.FCusAppKey,tCus.FDBConnStr,tCus.FTVUrl,tCus.FSysType FROM tCustomerTV tTV
                LEFT JOIN tCustomer tCus ON tTV.FCusAppKey=tCus.FAppKey
                WHERE FIMEI='{0}' AND tTV.FEnabled=1
                END", EQ_IMEI);
            return function.SQLReadData(SQL);
        }

        // 读取TV端APP版本信息
        public DataTable GetTVAppVer()
        {
            return function.SQLReadData("SELECT FAppName,FVer,FVerCode,FAppUrl FROM dbo.tAppUpdate WHERE FAppName='SkyLinkTV'");
        }

        // 读取大屏列表
        public DataTable GetCusTVList(string Search)
        {
            string SQL = string.Format(@"SELECT tTV.*,tCus.FName AS FCusName FROM dbo.tCustomerTV tTV
                LEFT JOIN dbo.tCustomer tCus ON tTV.FCusAppKey=tCus.FAppKey
                WHERE tTV.FIMEI<>'' AND(tCus.FName LIKE '%{0}%' OR tTV.FIMEI LIKE '%{0}%')
                ORDER BY FCusName,tTV.FAddTime", Search);
            return function.SQLReadData(SQL); 
        }

        // 读取大屏信息
        public DataTable GetCusTVInfo(string IMEI)
        {
            return function.SQLReadData("SELECT * FROM tCustomerTV WHERE FIMEI='" + IMEI + "'");
        }

        // 更新大屏信息
        public bool UpdateCusTVInfo(string IMEI, string FCusAppKey, string FEnabled, string FRemark)
        {
            return function.SQLUpdate("UPDATE tCustomerTV SET FCusAppKey='" + FCusAppKey + "',FEnabled='" + FEnabled + "',FRemark='" + FRemark + "' WHERE FIMEI='" + IMEI + "'");
        }

        // 删除大屏
        public bool DeleteCusTV(string IMEI)
        {
            return function.SQLUpdate("DELETE FROM tCustomerTV WHERE FIMEI='" + IMEI + "'");
        }
        #endregion

        #region 平台管理
        // 添加接入平台
        public bool AddPlatform(string FName, string FNameShort, string FType, string FAPIUrl, string FAppKey, string FAppSecret, string FDBName, string FDBLoginID, string FDBPwd, string FEnabled)
        {
            return function.SQLUpdate("INSERT INTO tPlatform(FID,FName,FNameShort,FType,FAPIUrl,FAppKey,FAppSecret,FDBName,FDBLoginID,FDBPwd,FEnabled)VALUES(LOWER(REPLACE(NEWID(),'-','')),'"
                + FName + "','" + FNameShort + "','" + FType + "','" + FAPIUrl + "','" + FAppKey + "','" + FAppSecret + "','" + FDBName + "','" + FDBLoginID + "','" + FDBPwd + "','" + FEnabled + "')");
        }

        // 更新接入平台
        public bool UpdatePlatform(string FID, string FName, string FNameShort, string FType, string FAPIUrl, string FAppKey, string FAppSecret, string FDBName, string FDBLoginID, string FDBPwd, string FEnabled)
        {
            return function.SQLUpdate("UPDATE tPlatform SET FName='" + FName + "',FNameShort='" + FNameShort + "',FType='" + FType + "',FAPIUrl='" + FAPIUrl + "',FAppKey='" + FAppKey + "',FAppSecret='" + FAppSecret
                + "',FDBName='" + FDBName + "',FDBLoginID='" + FDBLoginID + "',FDBPwd='" + FDBPwd + "',FEnabled='" + FEnabled + "' WHERE FID='" + FID + "'");
        }

        // 读取接入平台信息
        public DataTable GetPlatformInfo(string FID)
        {
            return function.SQLReadData("SELECT * FROM tPlatform WHERE FID='" + FID + "'");
        }

        // 读取接入平台列表
        public DataTable GetPlatformList(string SearchStr)
        {
            return function.SQLReadData("SELECT FID,FName,FNameShort,FType,FEnabled FROM tPlatform WHERE FName LIKE '%" + SearchStr + "%' ORDER BY FName");
        }

        // 删除接入平台
        public bool DeletePlatform(string FID)
        {
            return function.SQLUpdate("DELETE FROM tPlatform WHERE FID='" + FID + "'");
        }

        // 读取客户可对接的平台列表
        public DataTable GetCusPlatform(string FCusAppKey, bool OnlyEnabled)
        {
            string Search = string.Empty;
            if (OnlyEnabled)
            {
                Search = " AND FEnabled=1";
            }

            string SQL = string.Format(@"SELECT tCusP.FPlatformID,tP.FName,tP.FNameShort FROM tCustomerPlatform tCusP
                LEFT JOIN tPlatform tP ON tP.FID=tCusP.FPlatformID
                WHERE FCusAppKey='{0}' {1} ORDER BY tP.FName", FCusAppKey, Search);
            return function.SQLReadData(SQL);
        }

        // 更新第三方平台AccessToken信息
        public bool UpdatePlatformToken(string FCusAppKey, string FPlatformID, string FAccessToken, string FExpTime)
        {
            string SQL = string.Format(@"IF EXISTS(SELECT 1 FROM tPlatformToken WHERE FCusAppKey='{0}' AND FPlatformID='{1}')
                UPDATE tPlatformToken SET FAccessToken='{2}',FExpTime='{3}' WHERE FCusAppKey='{0}' AND FPlatformID='{1}'
                ELSE INSERT INTO tPlatformToken(FCusAppKey,FPlatformID,FAccessToken,FExpTime)VALUES('{0}','{1}','{2}','{3}')", FCusAppKey, FPlatformID, FAccessToken, FExpTime);
            return function.SQLUpdate(SQL);
        }

        // 读取客户对应第三方平台的AccessToken
        public DataTable GetPlatformToken(string FCusAppKey, string FPlatformID)
        {
            return function.SQLReadData("SELECT FAccessToken FROM tPlatformToken WHERE FCusAppKey='" + FCusAppKey + "' AND FPlatformID='" + FPlatformID + "' AND FExpTime>DATEADD(MINUTE,5,GETDATE())");
        }

        // 读取客户用户绑定信息
        public DataTable GetCustomerUser(string FCusAppKey, string FCusUserID = null)
        {
            string SearchStr = string.Empty;
            if (!string.IsNullOrWhiteSpace(FCusUserID))
            {
                SearchStr = " AND FCusUserID='" + FCusUserID + "'";
            }

            return function.SQLReadData("SELECT FCusUserID,FBindTime FROM dbo.tCustomerUser WHERE FCusAppKey='" + FCusAppKey + "' " + SearchStr);
        }
        #endregion

        #region 微信
        // 读取本地缓存AccessToken
        public DataTable GetWXAccessToken()
        {
            return function.SQLReadData("SELECT FToken FROM tTokens WHERE FTokenType='WeiXin' AND FExpired>=GETDATE()");
        }

        // 更新本地缓存AccessToken
        public bool UpdateWXAccessToken(string FToken, string FExpired)
        {
            string SQL = string.Format(@"IF EXISTS(SELECT 1 FROM tTokens WHERE FTokenType='WeiXin')
                UPDATE tTokens SET FToken='{0}',FExpired='{1}' WHERE FTokenType='WeiXin'
                ELSE INSERT INTO tTokens(FTokenType,FToken,FExpired,FCRTDT)VALUES('WeiXin','{0}','{1}',GETDATE())", FToken, FExpired);
            return function.SQLUpdate(SQL);
        }

        // 读取本地缓存Ticket
        public DataTable GetWXTicket()
        {
            return function.SQLReadData("SELECT FTicket FROM tTickets WHERE FTicketType='WeiXin' AND FExpired>=GETDATE()");
        }

        // 更新本地缓存Ticket
        public bool UpdateWXTicket(string FTicket, string FExpired)
        {
            string SQL = string.Format(@"IF EXISTS(SELECT 1 FROM tTickets WHERE FTicketType='WeiXin')
                UPDATE tTickets SET FTicket='{0}',FExpired='{1}' WHERE FTicketType='WeiXin'
                ELSE INSERT INTO tTickets(FTicketType,FTicket,FExpired,FCRTDT)VALUES('WeiXin','{0}','{1}',GETDATE())", FTicket, FExpired);
            return function.SQLUpdate(SQL);
        }
        #endregion

        #region 手机短信
        // 添加短信发送记录
        public bool AddLogSMS(string FPhone, string FType, string FWXOpenID, string FCode, string FMessage, string FBizID, string FRequestID)
        {
            return function.SQLUpdate("INSERT INTO tLogSMS(FPhone,FType,FTime,FWXOpenID,FCode,FMessage,FBizID,FRequestID)VALUES('"
                + FPhone + "','" + FType + "',GETDATE(),'" + FWXOpenID + "','" + FCode + "','" + FMessage + "','" + FBizID + "','" + FRequestID + "')");
        }

        // 读取某微信OpenID某短信类型的最近一次短信发送记录
        public DataTable GetWXOpenIDLastSMSLog(string FWXOpenID, string FType)
        {
            return function.SQLReadData("SELECT TOP 1 FPhone,FTime FROM tLogSMS WHERE FWXOpenID='"+ FWXOpenID + "' AND FType='"+ FType + "' ORDER BY FID DESC");
        }
        #endregion
    }
}