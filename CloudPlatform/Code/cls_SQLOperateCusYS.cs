using System.Data;

namespace CloudPlatform.Code
{
    public class cls_SQLOperateCusYS
    {
        cls_SQLFunction function = new cls_SQLFunction();

        #region 通用
        // 使用事物处理SQL语句
        public DataTable DoSqlTRAN(string SQL, string DBConStr)
        {
            // 启用事务
            string SQL_Begin = "BEGIN TRAN BEGIN TRY ";
            string SQL_End = " COMMIT TRAN END TRY BEGIN CATCH ROLLBACK TRAN DECLARE @ErrorMsg nvarchar(4000) SET @ErrorMsg=(select ERROR_MESSAGE()) RAISERROR(@ErrorMsg,16,1) END CATCH";
            return function.SQLReadData(SQL_Begin + SQL + SQL_End, DBConStr);
        }
        #endregion

        // 读取用户信息放到开头直接读取职工信息
        public DataTable GetUserInfo(string FUserID, string DBConStr)
        {
            return function.SQLReadData("SELECT * FROM 用户表 WHERE bumen ='印刷厂' and 工号='" + FUserID + "'", DBConStr);
        }
        //public DataTable GetUserInfo(string FUserName, string FPhone, string DBConStr)
        //{
        //    return function.SQLReadData("SELECT * FROM T_Users WHERE FName='" + FUserName + "' AND FPhone='" + FPhone + "'", DBConStr);
        //}

        // 读取用户权限
        public DataTable GetUserPower(string UserID, string ConStr)
        {
            return function.SQLReadData("SELECT FState,FPower FROM 用户表 WHERE bumen ='印刷厂' and  工号='" + UserID + "'", ConStr);
        }

        // 读取手机端需要的系统设置信息
        public DataTable GetSettings(string DBConStr)
        {
            return function.SQLReadData("SELECT FSetItem,FSetInfo FROM T_Settings", DBConStr);
        }

        // 读取用户的员工工序
        public DataTable GetUserProcedure(string FUserID, string DBConStr)
        {
            return function.SQLReadData("SELECT FProcedureID FROM T_Procedure_User WHERE FUserID='" + FUserID + "'", DBConStr);
        }

        // 读取生产计划
        public DataTable ReadWorkPlan(string FID, string DBConStr)
        {
            return function.SQLReadData("SELECT * FROM ysc_作业单 WHERE 印刷排产日期 is not null and  作业单号='" + FID + "'", DBConStr);
        }

        // 读取生产计划包含的工序
        public DataTable ReadWorkPlanProcList(string FID, string UserProcIDList, string DBConStr)
        {
            string ProcStr = string.Empty;
            // 员工工序条件限制（格式：1,3,4），如果未设置则传0，防止SQL语法错误
            if (string.IsNullOrWhiteSpace(UserProcIDList))
            {
                ProcStr = "AND t1.FIDProcedure IN(0)";
            }
            else if (UserProcIDList == "PlanProc")
            {
                ProcStr = string.Empty;
            }
            else
            {
                ProcStr = "AND t1.FIDProcedure IN(" + UserProcIDList + ")";
            }

            string SQL = string.Format(@"", FID, ProcStr);
            return function.SQLReadData(SQL, DBConStr);
        }

        // 判断生产计划是否包含某道工序
        public DataTable ReadWorkPlanProcInfo(string WorkPlanID, string ProcID, string DBConStr)
        {
            string SQL = string.Format(@"SELECT tWPP.FItemID,tWPP.FSeriaNumber,tWPP.FIDProcedure,tP.FName FROM T_WorkPlan_Pro tWPP
                LEFT JOIN T_Procedure tP ON tWPP.FIDProcedure=tP.FID
                WHERE tWPP.FIDWorkplan={0} AND tWPP.FIDProcedure={1}", WorkPlanID, ProcID);
            return function.SQLReadData(SQL, DBConStr);
        }

        // 读取辅助工序列表
        public DataTable ReadFuZhuProc(string DBConStr)
        {
            return function.SQLReadData("SELECT FID,FName FROM dbo.T_Procedure WHERE FType='辅助工序' AND FState=1 ORDER BY FName", DBConStr);
        }

        // 读取生产计划某工序的累计已报工数量
        public DataTable ReadWorkPlanProcTotalQty(string WrokPlanProcID,string dingdan, string DBConStr)
        {
            return function.SQLReadData("select  zyid ,ISNULL(SUM(chanliang),0) AS FCounts,ISNULL(SUM(FBad),0) AS FBad,ISNULL(SUM(FLoss),0) AS FLoss   from ysc_人员产量  WHERE zyid ='"+ dingdan + "' and gongxu ='" + WrokPlanProcID + "' group by zyid " , DBConStr);
        }

        // 根据生产计划ID读取销售订单信息
        public DataTable ReadWorkPlanSaleOrderInfo(string FWorkPlanID, string DBConStr)
        {
            string SQL = string.Format(@"SELECT case when 状态  like '%取消%'  then '取消' when 状态  like '%暂停%'  then '暂停' when 印刷排产日期 is  null then '未排产'   else '' end as zt ,isnull(作业人员,'') as 作业人员  FROM ysc_作业单 WHERE   作业单号={0}", FWorkPlanID);
            return function.SQLReadData(SQL, DBConStr);
        }

        // 外协
        public DataTable ReadWorkPlan4Wx(string FID, string DBConStr)
        {
            string SQL = string.Format(@"SELECT 'PL'+RIGHT('000000'+CONVERT(NVARCHAR(10),FID),6) AS Code,wp.* FROM T_WorkPlan wp
                INNER JOIN T_OutProduct op ON op.FIDWorkPlan=wp.FID
                WHERE op.FItemID='{0}'", FID);
            return function.SQLReadData(SQL, DBConStr);
        }

        // 开工汇报（工序开始）
        public bool ProductionDataStart(string PlanID, string FIDWorkPlanPro, string FUserID, string FBiller, string FCount, string FBad, string FRemark, string FIDOutUnit, string FIsOut, string FLoss, string DBConStr)
        {
            string SQL = @"INSERT INTO T_Production_Data(FCounts,FBad,FUserID,FBiller,FBillTime,FDate,FBegin,FRemark,FIDWorkPlanPro,FIDWorkPlan,FIDOutUnit,FIsOut,FLoss) 
                VALUES(" + FCount + "," + FBad + ",'" + FUserID + "','" + FBiller + "',GETDATE(),GETDATE(),GETDATE(),'" + FRemark + "'," + FIDWorkPlanPro + "," + PlanID + ",'" + FIDOutUnit + "','" + FIsOut + "'," + FLoss + ")";
            return function.SQLUpdate(SQL, DBConStr);
        }

        // 完工汇报
        public bool ProductionDataUpdate(string PlanID, string FIDWorkPlanPro, string FUserID, string FBiller, string FCount, string FBad, string FRemark, string FIDUnit, string FIsOut, string FLoss, string DBConStr)
        {
            string SQL = "IF EXISTS(SELECT 1 FROM T_Production_Data WHERE FUserID='" + FUserID + "' AND FIDWorkPlanPro=" + FIDWorkPlanPro + " AND FEnd IS NULL)"
                + " UPDATE T_Production_Data SET FCounts=FCounts+" + FCount + ",FBad=FBad+" + FBad + ",FUserID='" + FUserID + "',FBiller='" + FBiller + "',FIDOutUnit='" + FIDUnit + "',FIsOut='" + FIsOut + "',"
                + "FRemark='" + FRemark + "',FLoss=FLoss+" + FLoss + ",FEnd=GETDATE() WHERE FUserID='" + FUserID + "' AND FIDWorkPlanPro=" + FIDWorkPlanPro + " "
                + "ELSE INSERT INTO T_Production_Data(FCounts,FBad,FUserID,FDate,FEnd,FBegin,FBiller,FBillTime,FRemark,FIDWorkPlanPro,FIDWorkPlan,FIDOutUnit,FIsOut,FLoss)VALUES"
                + "(" + FCount + "," + FBad + ",'" + FUserID + "',GETDATE(),GETDATE(),GETDATE(),'" + FBiller + "',GETDATE(),'" + FRemark + "'," + FIDWorkPlanPro + "," + PlanID + ",'" + FIDUnit + "','" + FIsOut + "'," + FLoss + ")";

            return function.SQLUpdate(SQL, DBConStr);
        }

        // 添加辅助工序汇报
        public bool ProductionDataFuZhu(string DBConStr, string FUserID, string FProcID, string FQty, string FRemark)
        {
            return function.SQLUpdate("INSERT INTO T_Production_Data_FuZhu(FUserID,FProcID,FQty,FBeginTime,FEndTime,FRemark)VALUES('" + FUserID + "','" + FProcID + "'," + FQty + ",GETDATE(),GETDATE(),'" + FRemark + "')", DBConStr);
        }

        // 工序汇报自动创建外协加工单（如果下道工序为外协工序且系统设置通过工序创建外协加工单则自动创建）
        public bool AutoCreateWXBillByProcSave(string DBConStr, string WorkPlanID, string ProcedureID, string OutCount, string Biller)
        {
            string SQL = string.Format(@"--判断是否在报工时自动创建外协加工单
                DECLARE @AutoCreateWXBill NVARCHAR(50),@WorkPlanID INT,@ProcedureID INT,@OutCount DECIMAL(18,2),@Biller NVARCHAR(50)
                SET @WorkPlanID={0};SET @ProcedureID={1};SET @OutCount={2};SET @Biller='{3}'
                SELECT @AutoCreateWXBill=FSetInfo FROM dbo.T_Settings WHERE FSetItem='AutoCreateWXBill' 
                SELECT @AutoCreateWXBill
                IF @AutoCreateWXBill='SaveProc'
                BEGIN
	                --获取汇报工序的下一道工序信息
	                SELECT TOP 1 * INTO #tNextWPPro FROM T_WorkPlan_Pro WHERE FIDWorkplan=@WorkPlanID AND FSeriaNumber>(SELECT FSeriaNumber FROM T_WorkPlan_Pro WHERE FItemID=@ProcedureID) ORDER BY FSeriaNumber
	                --判断下道工序是否为外协工序
	                IF((SELECT FIsOut FROM #tNextWPPro)='Y')
	                BEGIN
		                --外协加工单表头ID
		                DECLARE @PID INT
		                --创建外协加工单表头
		                INSERT INTO T_OutProduct(FIDProcedure,FIDOutUnit,FProCode,FProName,FModel,FOrderCount,FOutCount,FRemark,FBiller,FBillTime,FFinished,FSemiFinished,FHanJie,FFecthDate,FICItemID,FBillNo,FCusNumber,FIDModel,FIsSequence,FIDWorkPlan)
		                SELECT tWPPro.FIDProcedure,tWPPro.FIDOutUnit,tWP.FProName,tWP.FProCode,tWP.FModel,tWP.FOrderCounts,@OutCount,'' AS FRemark,@Biller,GETDATE(),
		                0 AS FFinished,0 AS FSemiFinished,0 AS FHanJie,ISNULL(tWPPro.FOutFetchDate,tWP.FFetchDate),tWP.FICItemID,tSO.FBillNo,tCus.FNumber,0 AS FIDModel,tWP.FIsSequence,tWP.FID FROM #tNextWPPro tWPPro
		                LEFT JOIN dbo.T_WorkPlan tWP ON tWP.FID=tWPPro.FIDWorkplan
		                LEFT JOIN dbo.T_SaleOrderDetail tSOD ON tWP.FItemID=tSOD.FDetailID
		                LEFT JOIN dbo.T_SaleOrder tSO ON tSOD.FInterID=tSO.FInterID
		                LEFT JOIN dbo.T_Customer tCus ON tSO.FCusID=tCus.FID
		                SET @PID=(SELECT SCOPE_IDENTITY())
		                --创建外协加工单表体
		                INSERT INTO T_OutProduct_Order(FDetailID,FBillNo,FInterID,FIDProcedure,FIDOutUnit,FCusName,FProCode,FProName,FModel,FOrderCount,FOutCount,FRemark,FBiller,FBillTime,FFinished,FSemiFinished,FHanJie,FCusNumber,FIDWorkPlan,FFecthDate,FIDOutProduct) 
		                SELECT tSOD.FDetailID,tSO.FBillNo,tSOD.FInterID,tWPPro.FIDProcedure,tWPPro.FIDOutUnit,tCus.FCusName,tWP.FProCode,tWP.FProName,tWP.FModel,tWP.FOrderCounts,@OutCount,'' AS FRemark,@Biller,GETDATE(),
		                0 AS FFinished,0 AS FSemiFinished,0 AS FHanJie,tCus.FNumber,tWP.FID,ISNULL(tWPPro.FOutFetchDate,tWP.FFetchDate),@PID FROM #tNextWPPro tWPPro
		                LEFT JOIN dbo.T_WorkPlan tWP ON tWP.FID=tWPPro.FIDWorkplan
		                LEFT JOIN dbo.T_SaleOrderDetail tSOD ON tWP.FItemID=tSOD.FDetailID
		                LEFT JOIN dbo.T_SaleOrder tSO ON tSOD.FInterID=tSO.FInterID
		                LEFT JOIN dbo.T_Customer tCus ON tSO.FCusID=tCus.FID
	                END
	                DROP TABLE #tNextWPPro
                END", WorkPlanID, ProcedureID, OutCount, Biller);
            return function.SQLUpdate(SQL, DBConStr);
        }

        // 计划查询
        public DataTable RptPlanInfo(string PlanID, string DBConStr)
        {
            string SQL = string.Format(@"--生成完工工序报表
                SELECT tWP.FID AS WPID,tWP.FPlanCode,tWP.FOrderID,tWP.FInterID,tWP.FProName,tWP.FModel,tWP.FDrawNo,tSOD.FFuTu,tSOD.FMTONo,tWP.FCounts,tWP.FOrderCounts,tWP.FFetchDate,ISNULL(tWPP.FItemID,0) AS WPPID,'['+tPr.FName+']' AS GXName,tWPP.FSeriaNumber,tDic.FName AS FItemClassName,
                CASE WHEN tWP.FState=1 THEN 'Y' ELSE(CASE WHEN ISNULL(MAX(tPr.FProgress),0)<100 THEN 'N' ELSE 'Y' END) END AS FinishState,tPD.FEnd,tWP.FRemark,MAX(tPr.FProgress) AS FProgress,CONVERT(NVARCHAR(50),tSOD.FHQWeeklyPlanDate,23) AS FHQWeeklyPlanDate INTO #t1 FROM dbo.T_WorkPlan tWP
                LEFT JOIN dbo.T_Production_Data tPD ON tPD.FIDWorkPlan=tWP.FID
                LEFT JOIN dbo.T_WorkPlan_Pro tWPP ON tWPP.FItemID=tPD.FIDWorkPlanPro
                LEFT JOIN dbo.T_Procedure tPr ON tWPP.FIDProcedure=tPr.FID
                LEFT JOIN dbo.T_OutProduct tPO ON tWPP.FIDProcedure=tPO.FIDProcedure AND tPO.FIDWorkPlan=tWPP.FIDWorkplan
                LEFT JOIN dbo.T_SaleOrderDetail tSOD ON tWP.FItemID=tSOD.FDetailID
                LEFT JOIN dbo.T_ICItem tICI ON tWP.FICItemID=tICI.FItemID
                LEFT JOIN dbo.T_Dict tDic ON tICI.FItemClass=tDic.FItemID
                WHERE tWP.FID={0}
                GROUP BY tWP.FID,tWP.FPlanCode,tWP.FOrderID,tWP.FInterID,tWP.FProName,tWP.FModel,tWP.FDrawNo,tSOD.FFuTu,tSOD.FMTONo,tWP.FOrderCounts,tWP.FFetchDate,tWPP.FItemID,tPr.FName,tWP.FCounts,tWPP.FSeriaNumber,tPD.FEnd,tWP.FRemark,tWP.FState,tDic.FName,tSOD.FHQWeeklyPlanDate
                ORDER BY tWPP.FSeriaNumber
                UPDATE #t1 SET GXName='' WHERE GXName IS NULL
                SELECT a.WPID,a.FInterID,FPlanCode,FOrderID,FProName,FModel,FDrawNo,FFuTu,FMTONo,a.FCounts,FOrderCounts,FFetchDate,a.FRemark,MAX(a.FProgress) AS FProgress,a.FHQWeeklyPlanDate,
                FFinish=STUFF((SELECT CASE WHEN LEN(GXName)>0 THEN ','+GXName ELSE '' END FROM #t1 WHERE FPlanCode=a.FPlanCode ORDER BY FEnd FOR XML path('')),1,1,''),CONVERT(NVARCHAR(MAX),'') AS FNotFinish
                INTO #t3 FROM #t1 a
                GROUP BY a.WPID,a.FInterID,FPlanCode,FOrderID,FProName,FModel,FDrawNo,FFuTu,FMTONo,a.FCounts,FOrderCounts,FFetchDate,a.FRemark,a.FHQWeeklyPlanDate
                ORDER BY a.FPlanCode
                SELECT t1.*,(CASE WHEN t1.FFinish LIKE '%完工%' THEN 'Y' ELSE (CASE WHEN t1.FProgress<100 THEN 'N' ELSE 'Y' END) END) AS FComplete,tCus.FShortName,tSO.FContractNo INTO #t4 FROM #t3 t1
                LEFT JOIN T_SaleOrder tSO ON t1.FInterID=tSO.FInterID
                LEFT JOIN T_Customer tCus ON tSO.FCusID=tCus.FID
                --读取未完工工序
                SELECT tWP.FID AS WPID,'['+tPr.FName+']' AS GXName INTO #tPrNotFinish1 FROM dbo.T_WorkPlan tWP
                LEFT JOIN dbo.T_WorkPlan_Pro tWPP ON tWP.FID=tWPP.FIDWorkplan
                LEFT JOIN dbo.T_Procedure tPr ON tWPP.FIDProcedure=tPr.FID
                WHERE tWP.FID={0} AND tWPP.FItemID NOT IN(SELECT WPPID FROM #t1)
                ORDER BY tWPP.FSeriaNumber
                UPDATE #tPrNotFinish1 SET GXName='' WHERE GXName IS NULL
                SELECT a.WPID,
                FNotFinish=STUFF((SELECT CASE WHEN LEN(GXName)>0 THEN ','+GXName ELSE '' END FROM #tPrNotFinish1 WHERE WPID=a.WPID FOR XML path('')),1,1,'')
                INTO #tPrNotFinish2 FROM #tPrNotFinish1 a
                GROUP BY a.WPID
                --将未完工数据更新至完工工序报表中
                UPDATE #t4 SET FNotFinish=b.FNotFinish FROM #t4 a
                LEFT JOIN #tPrNotFinish2 b ON a.WPID=b.WPID
                SELECT * FROM #t4
                DROP TABLE #tPrNotFinish1 DROP TABLE #tPrNotFinish2
                DROP TABLE #t1 DROP TABLE #t3 DROP TABLE #t4", PlanID);
            return function.SQLReadData(SQL, DBConStr);
        }

        // 读取生产计划完工工序信息（带完工汇报人员及汇报时间）
        public DataTable RptPlanFinishProcList(string PlanID, string DBConStr)
        {
            string SQL = string.Format(@"SELECT tProc.FName AS FProcName,
                STUFF((SELECT ','+tUW.FName+' '+SUBSTRING(CONVERT(NVARCHAR(50),tPD.FEnd,21),6,11) FROM dbo.T_Production_Data tPD LEFT JOIN T_Users tUW ON tUW.FUserID=tPD.FUserID WHERE tPD.FIDWorkPlanPro=tWPP.FItemID GROUP BY tPD.FItemID,tUW.FName,tPD.FEnd FOR XML PATH('')),1, 1, '') AS Worker
                FROM dbo.T_Production_Data tPD
                LEFT JOIN dbo.T_WorkPlan_Pro tWPP ON tPD.FIDWorkPlanPro=tWPP.FItemID
                LEFT JOIN dbo.T_Procedure tProc ON tWPP.FIDProcedure=tProc.FID
                LEFT JOIN dbo.T_Users tU ON tPD.FBiller=tU.FUserID
                WHERE tPD.FIDWorkplan={0}
                GROUP BY tProc.FName,tWPP.FItemID", PlanID);
            return function.SQLReadData(SQL, DBConStr);
        }

        // 读取汇报记录
        public DataTable GetHuiBaoLog(string UserID, string BeginDate, string DBConStr)
        {
            string SQL = string.Format(@"SELECT tPD.FItemID,tPD.FCounts,tPD.FBad,tPD.FEnd,tProc.FName AS FProcName,'PL'+RIGHT('000000'+CONVERT(NVARCHAR(50),tWP.FID),6) AS FPLNo,tSO.FBillNo,tSO.FContractNo INTO #t1 FROM dbo.T_Production_Data tPD
                LEFT JOIN dbo.T_WorkPlan_Pro tWPP ON tPD.FIDWorkPlanPro=tWPP.FItemID
                LEFT JOIN dbo.T_Procedure tProc ON tWPP.FIDProcedure=tProc.FID
                LEFT JOIN dbo.T_WorkPlan tWP ON tPD.FIDWorkPlan=tWP.FID
                LEFT JOIN dbo.T_SaleOrder tSO ON tWP.FInterID=tSO.FInterID
                WHERE tPD.FBiller='{0}' AND tPD.FEnd>='{1}'
                INSERT INTO #t1
                SELECT tPD.FID,tPD.FQty,0,tPD.FEndTime,tProc.FName,'[辅助工序]','-','-' FROM T_Production_Data_FuZhu tPD
                LEFT JOIN dbo.T_Procedure tProc ON tPD.FProcID=tProc.FID
                WHERE tPD.FUserID='{0}' AND tPD.FEndTime>='{1}'
                SELECT * FROM #t1 ORDER BY FEnd DESC", UserID, BeginDate);
            return function.SQLReadData(SQL, DBConStr);
        }

        // 删除工序汇报记录
        public bool DeleteProductionData(string PDID, string DBConStr)
        {
            return function.SQLUpdate("DELETE FROM T_Production_Data WHERE FItemID='" + PDID + "'", DBConStr);
        }

        // 删除工序汇报记录（辅助工序）
        public bool DeleteProductionData_FuZhu(string PDID, string DBConStr)
        {
            return function.SQLUpdate("DELETE FROM T_Production_Data_FuZhu WHERE FID='" + PDID + "'", DBConStr);
        }

        #region 大屏
        // 读取大屏列表
        public DataTable GetScreenClass(string ConStr)
        {
            return function.SQLReadData("SELECT FID,FName,FPageName FROM T_ScreenClass", ConStr);
        }
        #endregion
    }
}