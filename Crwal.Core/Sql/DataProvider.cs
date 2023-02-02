using System;
using System.Data.SqlClient;
using System.Data;
using Crwal.Core.Log;
using Crwal.Core.Base;
using System.Threading.Tasks;

namespace Crwal.Core.Sql
{
    public class DataProvider
    {

        private SqlDataAdapter _myAdapter;
        private string _conStr;
        private static string _error;
        public DataProvider(string conStr)
        {
            _conStr = conStr;
        }
        public static SqlConnection OpenConnection(string _conStr)
        {
            try
            {
                string conString = _conStr;
                SqlConnection msqlCon = new SqlConnection(conString);

                if (msqlCon.State == ConnectionState.Closed || msqlCon.State == ConnectionState.Broken)
                {
                    msqlCon.Open();
                }
                return msqlCon;
            }
            catch (Exception ex)
            {
                _error = ex.Message;
            }
            return null;
        }

        public DataTable Select(string query, SqlParameter[] sqlParameter)
        {
            try
            {
                using (SqlConnection conn = DataProvider.OpenConnection(_conStr))
                {
                    DataTable dataTable = new DataTable();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddRange(sqlParameter);
                    cmd.ExecuteNonQuery();
                    DataSet ds = new DataSet();
                    _myAdapter = new SqlDataAdapter();
                    _myAdapter.SelectCommand = cmd;
                    _myAdapter.Fill(ds);
                    dataTable = ds.Tables[0];
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                _error = ex.Message;
            }
            return null;
        }
        public async Task<DataTable> SelectAsync(string query)
        {
            try
            {
                using (SqlConnection conn = DataProvider.OpenConnection(_conStr))
                {
                    Logging.Infomation("Bắt đầu truy vấn dữ liệu " + query);
                    DataTable dataTable = new DataTable();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    await cmd.ExecuteNonQueryAsync();
                    DataSet ds = new DataSet();
                    _myAdapter = new SqlDataAdapter();
                    _myAdapter.SelectCommand = cmd;
                    _myAdapter.Fill(ds);
                    dataTable = ds.Tables[0];
                    Logging.Infomation("Truy vấn thành công " + dataTable.ToJson());
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                Logging.Error(ex, "Select - " + query);
                _error = ex.Message;
            }
            return null;
        }
        public bool ExecuteNonQuery(string query, SqlParameter[] sqlParameter)
        {
            try
            {
                //string query = "select * from [MD_Customer] where CustomerId = @Cus_id";
                //SqlParameter[] sqlParameters = new SqlParameter[1];
                //sqlParameters[0] = new SqlParameter("@Cus_id", SqlDbType.Int);
                //sqlParameters[0].Value = _id;
                using (SqlConnection conn = DataProvider.OpenConnection(_conStr))
                {
                    SqlCommand myCommand = new SqlCommand(_conStr, conn);
                    myCommand.CommandText = query;
                    myCommand.Parameters.AddRange(sqlParameter);
                    myCommand.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                _error = ex.Message;

                return false;
            }
        }
        public bool ExecuteNonQuery(string query)
        {
            try
            {
                using (SqlConnection conn = DataProvider.OpenConnection(_conStr))
                {
                    SqlCommand myCommand = new SqlCommand(_conStr, conn);
                    myCommand.CommandText = query;
                    myCommand.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                _error = ex.Message;
                return false;
            }
        }
    }
}
