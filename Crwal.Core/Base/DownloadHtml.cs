using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crwal.Core.Base
{
    public class DownloadHtml
    {
        public static string _error; // thuộc tính này sẽ chứa thông điệp lỗi khi hàm trả ra rỗng

        /// <summary>
        /// Download Html không có Decoe
        /// </summary>
        /// <param name="urlAddress"></param>
        /// <returns></returns>
        public static string GetContentHtml_NoHTMLDecode(string urlAddress)
        {
            string source = "";
            try
            {
                // fix lỗi: The request was aborted: Could not create SSL/TLS secure channel
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(urlAddress);
                webRequest.AllowAutoRedirect = true; // tự động chuyển link

                webRequest.Method = "GET";
                webRequest.Accept = "*/*";
                webRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.131 Safari/537.36";

                var response = webRequest.GetResponse();
                var stream = response.GetResponseStream();
                try
                {
                    //// nếu có gzip cần giải nén
                    //if (response.Headers.AllKeys.Contains("Content-Encoding")
                    //    && response.Headers["Content-Encoding"].Contains("gzip"))
                    //{
                    //    stream = new System.IO.Compression.GZipStream(stream, System.IO.Compression.CompressionMode.Decompress);
                    //}

                    using (StreamReader reader = new StreamReader(stream))
                    {
                        source = reader.ReadToEnd();

                        if (!string.IsNullOrEmpty(source))
                        {
                            source = source.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&#13;", "").Replace("&#10;", ""); // xóa \r\n\t và \" thành dấu nháy đơn
                            source = Regex.Replace(source, @"[\s]{2,}", " "); // xóa nhiều khoảng trắng
                        }
                    }
                }
                finally
                {
                    if (stream != null)
                        stream.Dispose();
                }
            }
            catch (Exception ex)
            {
                _error = ex.Message;
            }

            return source;
        }

        /// <summary>
        /// download html bằng HttpWebRequest có gzip
        /// </summary>
        /// <param name="urlAddress"></param>
        /// <returns></returns>
        public static string GetContentHtml(string urlAddress)
        {
            string source = "";

            try
            {
                // fix lỗi: The request was aborted: Could not create SSL/TLS secure channel
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(urlAddress);
                webRequest.AllowAutoRedirect = true; // tự động chuyển link

                webRequest.Method = "GET";
                webRequest.Accept = "*/*";
                webRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36";
                webRequest.Accept = "application/zip";

                // đặt refer từng trang chủ
                //webRequest.Referer = urlAddress;

                var response = webRequest.GetResponse();
                var stream = response.GetResponseStream();
                try
                {
                    //nếu có gzip cần giải nén
                    try
                    {
                        if (response.Headers.AllKeys.Contains("Content-Encoding")
                            && response.Headers["Content-Encoding"].Contains("gzip"))
                        {
                            stream = new System.IO.Compression.GZipStream(stream, System.IO.Compression.CompressionMode.Decompress);
                        }
                    }
                    catch { }

                    using (StreamReader reader = new StreamReader(stream))
                    {
                        source = reader.ReadToEnd();

                        if (!string.IsNullOrEmpty(source))
                        {
                            source = source.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&#13;", "").Replace("&#10;", ""); // xóa \r\n\t và \" thành dấu nháy đơn
                            source = Regex.Replace(source, @"[\s]{2,}", " "); // xóa nhiều khoảng trắng

                            source = System.Web.HttpUtility.HtmlDecode(source);
                        }
                    }
                }
                finally
                {
                    if (stream != null)
                        stream.Dispose();
                }
            }
            catch (Exception ex)
            {
                _error = ex.Message;
            }

            return source;
        }

        public static async Task<string> GetHTMLAsync(string urlAddress)
        {
            string htmlCode = "";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage res = await client.GetAsync(urlAddress);
                htmlCode = await res.Content.ReadAsStringAsync();
            }
            htmlCode = htmlCode.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&#13;", "").Replace("&#10;", ""); // xóa \r\n\t và \" thành dấu nháy đơn
            htmlCode = Regex.Replace(htmlCode, @"[\s]{2,}", " "); // xóa nhiều khoảng trắng

            htmlCode = System.Web.HttpUtility.HtmlDecode(htmlCode);
            return htmlCode;
        }
    }
}
