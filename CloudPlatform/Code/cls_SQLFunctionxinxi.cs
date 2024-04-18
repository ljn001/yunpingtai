using System.Data;
using System.Data.SqlClient;

namespace CloudPlatform.Code
{
    public class cls_SQLFunctionxinxi
    {
        string connectionString = string.Empty;
        SqlConnection con;
        SqlCommand selectcmd;
        System.Configuration.ConnectionStringSettings connString_Self = System.Configuration.ConfigurationManager.ConnectionStrings["SelfConnectionStringxinxi"];

        // 读取数据库
        public DataTable SQLReadData(string ComTxt, string conStr = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(conStr))
                {
                    connectionString = connString_Self.ConnectionString;
                }
                else
                {
                    connectionString = conStr;
                }
                con = new SqlConnection(connectionString);
                selectcmd = con.CreateCommand();
                selectcmd.CommandText = ComTxt;
                selectcmd.CommandTimeout = 1800;
                DataTable dt = new DataTable();
                con.Open();
                SqlDataReader reader = selectcmd.ExecuteReader();
                dt.Load(reader);
                return dt;
            }
            catch (SqlException e)
            {
                throw e;
            }
            finally
            {
                con.Close();
            }
        }

        // 读取数据库（返回DataSet）
        public DataSet SQLReadDataSet(string ComTxt, string conStr = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(conStr))
                {
                    connectionString = connString_Self.ConnectionString;
                }
                else
                {
                    connectionString = conStr;
                }
                con = new SqlConnection(connectionString);
                selectcmd = con.CreateCommand();
                selectcmd.CommandText = ComTxt;
                selectcmd.CommandTimeout = 1800;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = selectcmd;
                DataSet dataSet = new DataSet();
                SqlCommandBuilder sqlCommandBuilder = new SqlCommandBuilder(sqlDataAdapter);
                sqlDataAdapter.Fill(dataSet);
                return dataSet;
            }
            catch (SqlException e)
            {
                throw e;
            }
            finally
            {
                con.Close();
            }
        }

        // 插入、删除、修改数据库
        public bool SQLUpdate(string ComTxt, string conStr = null)
        {
            try
            {
                // 启用事务
                string SQL_Begin = "BEGIN TRAN BEGIN TRY ";
                string SQL_End = " COMMIT TRAN END TRY BEGIN CATCH ROLLBACK TRAN DECLARE @ErrorMsg nvarchar(4000) SET @ErrorMsg=(select ERROR_MESSAGE()) RAISERROR(@ErrorMsg,16,1) END CATCH";

                if (string.IsNullOrWhiteSpace(conStr))
                {
                    connectionString = connString_Self.ConnectionString;
                }
                else
                {
                    connectionString = conStr;
                }
                con = new SqlConnection(connectionString);
                con.Open();
                selectcmd = new SqlCommand(SQL_Begin + ComTxt + SQL_End, con);
                selectcmd.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // 数据库二进制数据写入
        public bool SQLUpdate_Photo(string ComTxt, byte[] File, string conStr)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(conStr))
                {
                    connectionString = connString_Self.ConnectionString;
                }
                else
                {
                    connectionString = conStr;
                }
                con = new SqlConnection(connectionString);
                con.Open();
                selectcmd = new SqlCommand(ComTxt, con);
                selectcmd.Parameters.Add("@File", SqlDbType.Image);
                selectcmd.Parameters[0].Value = File;
                selectcmd.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}