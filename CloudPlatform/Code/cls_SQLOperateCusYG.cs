using System.Data;

namespace CloudPlatform.Code
{
    public class cls_SQLOperateCusYG
    {
        cls_SQLFunction function = new cls_SQLFunction();

        #region 通用
        // 使用事物处理SQL语句
        public DataTable DoSqlTRAN(string SQL, string ConStr)
        {
            // 启用事务
            string SQL_Begin = "BEGIN TRAN BEGIN TRY ";
            string SQL_End = " COMMIT TRAN END TRY BEGIN CATCH ROLLBACK TRAN DECLARE @ErrorMsg nvarchar(4000) SET @ErrorMsg=(select ERROR_MESSAGE()) RAISERROR(@ErrorMsg,16,1) END CATCH";
            return function.SQLReadData(SQL_Begin + SQL + SQL_End, ConStr);
        }
        #endregion

        #region 大屏
        // 读取大屏列表
        public DataTable GetScreenClassYG(string ConStr)
        {
            return function.SQLReadData("SELECT FID,FName,FPageName FROM S_ScreenClass", ConStr);
        }
        #endregion


        // 读取用户信息
        public DataTable GetUserInfo(string FUserID, string DBConStr)
        {
            return function.SQLReadData("SELECT * FROM  Mes_Staffs WHERE Fstaffno='" + FUserID + "'", DBConStr);
        }

        public DataTable GetHuibaogongxu (string staffno,string ddh,string tzh,string sc, string DBConStr)
        {
            string sql = string.Format(@"select 
distinct 
h.fprocessno,fprocessnm 
from

Mes_Staffs,
MES_Process t,
Mes_TechRouteProcHis h
where 
charindex(','+  fvulgonm,','+isnull(fmainprocnm,'')+','+isnull(fworkprocnms,'') )>0
and 
fstaffno='{0}' and FOrdeNumber  = '{1}' AND charindex([FTuZhiNo],'{2}') >0  AND [FSuCaiNo] = N'{3}'
and t.fno = h.fprocessno", staffno,   ddh,     tzh,   sc);

            return function.SQLReadData(sql, DBConStr);
        }
        public bool ReportData(string staffno, string staffnm, string ddh, string tzh, string sc, string gx, string gxmc, string chejian, decimal sl, string DBConStr)
        {
            string SQL = string.Format(@"  exec p_report {0},{1},{2},{3},{4},{5},{6},{7},{8} ", staffno,   staffnm,   ddh,   tzh,   sc,   gx,   gxmc,   chejian,   sl);

            return function.SQLUpdate(SQL, DBConStr);
        }

        // 读取汇报记录
        public DataTable GetHuiBaoLog(string UserID, string BeginDate, string DBConStr)
        {
            string SQL = string.Format(@"SELECT   * FROM [dbo].[MES_DailyPlanReport] 
WHERE [FReportDT] >= N'{1}' 
AND [FUserID] = N'{0}' AND [FReportType] = N'Z' ORDER BY [FID] DESC", UserID, BeginDate);
            return function.SQLReadData(SQL, DBConStr);
        }
        public bool DeleteProductionData(string PDID, string DBConStr)
        {
            return function.SQLUpdate("DELETE FROM MES_DailyPlanReport WHERE FID='" + PDID + "'", DBConStr);
        }

    }
}