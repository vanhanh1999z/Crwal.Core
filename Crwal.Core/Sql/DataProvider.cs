using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Crwal.Core.Log;

namespace Crwal.Core.Sql
{
    public class DataProvider
    {
        private static string _error;
        private readonly string _conStr;

        private SqlDataAdapter _myAdapter;

        public DataProvider(string conStr)
        {
            _conStr = conStr;
        }

        public static SqlConnection OpenConnection(string _conStr)
        {
            try
            {
                var conString = _conStr;
                var msqlCon = new SqlConnection(conString);

                if (msqlCon.State == ConnectionState.Closed || msqlCon.State == ConnectionState.Broken) msqlCon.Open();
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
                using (var conn = OpenConnection(_conStr))
                {
                    var dataTable = new DataTable();
                    var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddRange(sqlParameter);
                    cmd.ExecuteNonQuery();
                    var ds = new DataSet();
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
                using (var conn = OpenConnection(_conStr))
                {
                    ("Bắt đầu truy vấn dữ liệu " + query).Infomation();
                    var dataTable = new DataTable();
                    var cmd = new SqlCommand(query, conn);
                    await cmd.ExecuteNonQueryAsync();
                    var ds = new DataSet();
                    _myAdapter = new SqlDataAdapter();
                    _myAdapter.SelectCommand = cmd;
                    _myAdapter.Fill(ds);
                    dataTable = ds.Tables[0];
                    //Logging.Infomation("Truy vấn thành công " + dataTable.ToJson());
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
                using (var conn = OpenConnection(_conStr))
                {
                    var myCommand = new SqlCommand(_conStr, conn);
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
                using (var conn = OpenConnection(_conStr))
                {
                    var myCommand = new SqlCommand(_conStr, conn);
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