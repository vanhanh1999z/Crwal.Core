using Crwal.Core.Base;
using System.Collections.Generic;

namespace Crwal.Core.Sql
{
    public class ConnectionString
    {
        internal static string FOLDER_CONFIX = @"C:\CrawlerTeam_Config";

        internal ConnectionString()
        {
        }

        /// <summary>
        /// Đọc File Config theo file
        /// </summary>
        /// <param name="filename">ví dụ: ConnectionString_News_DbLocal.txt</param>
        /// <returns></returns>
        internal static string GetConnectString(string filename)
        {
            string conn = "";

            string fileConfig = FOLDER_CONFIX + @"\" + filename;

            FileReadWrite f = new FileReadWrite(fileConfig);

            List<string> lis = f.GetListLine();

            if (lis != null && lis.Count > 0)
            {
                conn = lis[0];
            }

            return conn;
        }

        /// <summary>
        /// Lấy chuỗi kết nối tới database trên server local - lưu theo đường dẫn C:\CrawlerTeam_Config\
        /// Nếu chưa có file này ConnectionString_Soccer.txt, hãy tạo 1 file rồi đặt nội dung này Data Source=.;Initial Catalog=SoccerDb;User ID=coder;Password=coder, vào đó nhé. Tùy từng SQL Server mà đổi chuỗi kết nối cho đúng
        /// </summary>
        /// <returns>Data Source=.;Initial Catalog=SoccerDb;User ID=coder;Password=coder</returns>
        internal static string GetConnectString_SoccerDb()
        {
            return GetConnectString("ConnectionString_Soccer.txt");
        }
    }
}
