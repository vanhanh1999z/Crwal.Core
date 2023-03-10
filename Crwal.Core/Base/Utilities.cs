using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace Crwal.Core.Base
{
    public static class Utilities
    {
        public static string Md5Encode(string strEnc)
        {
            try
            {
                // Tạo một thể hiện của MD5
                var md5Hasher = MD5.Create();

                // Chuyển chuỗi sang dạng mảng mã hóa
                var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(strEnc));

                // Tạo đối tượng sBuilder để dùng phương thức nối chuỗi đạng byte
                var sBuilder = new StringBuilder();

                for (var i = 0; i < data.Length; i++) sBuilder.Append(data[i].ToString("x2"));

                // Trả về chuỗi mã hóa theo dạng chuỗi hexadecimal
                return sBuilder.ToString();
            }
            catch
            {
                return "";
            }
        }

        public static string RegexFilter(string html, string pattern)
        {
            try
            {
                foreach (Match m in Regex.Matches(html, pattern, RegexOptions.IgnoreCase)) return m.Value;
            }
            catch
            {
            }

            return "";
        }

        public static string RegexFilterLastItem(string html, string pattern)
        {
            var coll = Regex.Matches(html, pattern, RegexOptions.IgnoreCase);

            if (coll.Count > 0) return coll[coll.Count - 1].Value;
            return "";
        }

        public static List<string> RegexFilters(string html, string pattern)
        {
            var ls = new List<string>();
            try
            {
                foreach (Match m in Regex.Matches(html, pattern, RegexOptions.IgnoreCase)) ls.Add(m.Value);
            }
            catch
            {
            }

            return ls;
        }

        public static string RegexRemoveHtml(string html, string pattern)
        {
            try
            {
                return Regex.Replace(html, pattern, "", RegexOptions.IgnoreCase);
            }
            catch
            {
            }

            return "";
        }

        public static string RegexGetTextFromHtml(string html, string pattern, string patternSub,
            string patternRemoveTag, bool isRemoveAllTag)
        {
            var text = "";

            try
            {
                if (!string.IsNullOrEmpty(pattern))
                {
                    text = RegexFilter(html, pattern);
                    if (string.IsNullOrEmpty(text)) return "";

                    if (!string.IsNullOrEmpty(
                            patternSub)) // lấy subject bằng biểu thức thứ 2 sau khi được giới hạn bởi biểu thức 1
                    {
                        text = RegexFilter(text, patternSub);
                        if (string.IsNullOrEmpty(text)) // chạy đoạn 2 có dữ liệu thì gán vào
                            return "";
                    }

                    if (!string.IsNullOrEmpty(text) &&
                        !string.IsNullOrEmpty(patternRemoveTag)) // xóa đi các thẻ không sử dụng
                        text = Regex.Replace(text, patternRemoveTag, "", RegexOptions.IgnoreCase);
                }
                else // không truyền vào pattern thì xóa sạch luôn
                {
                    text = html;
                    if (!string.IsNullOrEmpty(text) &&
                        !string.IsNullOrEmpty(patternRemoveTag)) // xóa đi các thẻ không sử dụng
                        text = Regex.Replace(text, patternRemoveTag, "", RegexOptions.IgnoreCase);
                }

                if (isRemoveAllTag)
                    if (!string.IsNullOrEmpty(text))
                    {
                        text = Regex.Replace(text,
                            @"(<!--)(.*?)(-->)|(<script)(.*?)(script>)|(<style)(.*?)(style>)|(<iframe)(.*?)(iframe>)|(<embed)(.*?)(embed>)|(<object)(.*?)(object>)|(<video)(.*?)(video>)",
                            " ", RegexOptions.IgnoreCase); // xóa cac the khong can thiet (neu co)
                        text = Regex.Replace(text, @"<(.*?)>", " ", RegexOptions.IgnoreCase); // xóa cặp thẻ
                        text = Regex.Replace(text, @"[\s]{2,}", " ", RegexOptions.IgnoreCase)
                            .Trim(); // nếu có nhiều hơn 2 khoảng trắng thì thay bằng 1 khoảng trắng
                    }
            }
            catch
            {
            }

            return text.Trim();
        }

        public static string XpathGetHTML(string html, string xpath, bool isRemoveAllTag)
        {
            var htmlReturn = "";

            try
            {
                var htmlDoc = new HtmlDocument();
                htmlDoc.OptionFixNestedTags = true;
                htmlDoc.OptionCheckSyntax = false;
                htmlDoc.OptionAutoCloseOnEnd = true;
                htmlDoc.LoadHtml(html); //responseString là đoạn html mà bạn lấy được ở phần trước.

                var node = htmlDoc.DocumentNode.SelectNodes(xpath); //div[....]

                if (node != null && node.Count > 0)
                {
                    for (var i = 0; i < node.Count; i++) htmlReturn += node[i].OuterHtml + " ";

                    // có xóa hết các thẻ - chừa lại toàn text
                    if (isRemoveAllTag)
                    {
                        htmlReturn = Regex.Replace(htmlReturn,
                            @"(<!--)(.*?)(-->)|(<script)(.*?)(script>)|(<style)(.*?)(style>)|(<iframe)(.*?)(iframe>)|(<embed)(.*?)(embed>)|(<object)(.*?)(object>)|(<video)(.*?)(video>)",
                            " ", RegexOptions.IgnoreCase); // xóa cac the khong can thiet (neu co)
                        htmlReturn = Regex.Replace(htmlReturn, @"<(.*?)>", " ", RegexOptions.IgnoreCase); // xóa cặp thẻ
                        htmlReturn =
                            Regex.Replace(htmlReturn, @"[\s]{2,}", " ", RegexOptions.IgnoreCase)
                                .Trim(); // nếu có nhiều hơn 2 khoảng trắng thì thay bằng 1 khoảng trắng
                    }
                }
            }
            catch (Exception ex)
            {
                htmlReturn = "error, ex: " + ex.Message;
            }

            return htmlReturn;
        }

        public static List<string> XpathGetListHTML(string html, string xpath)
        {
            var lis = new List<string>();

            try
            {
                var htmlDoc = new HtmlDocument();
                htmlDoc.OptionFixNestedTags = true;
                htmlDoc.OptionCheckSyntax = false;
                htmlDoc.OptionAutoCloseOnEnd = true;
                htmlDoc.LoadHtml(html); //responseString là đoạn html mà bạn lấy được ở phần trước.

                var node = htmlDoc.DocumentNode.SelectNodes(xpath);
                if (node != null && node.Count > 0)
                    for (var i = 0; i < node.Count; i++)
                        lis.Add(node[i].OuterHtml);
            }
            catch (Exception ex)
            {
            }

            return lis;
        }

        public static string XpathRemoveHtml(string html, string xpath)
        {
            var sourceReturn = html;
            try
            {
                var htmlDocRem = new HtmlDocument();
                htmlDocRem.OptionFixNestedTags = true;
                htmlDocRem.OptionCheckSyntax = false;
                htmlDocRem.OptionAutoCloseOnEnd = true;
                htmlDocRem.LoadHtml(html);

                var nodeRem = htmlDocRem.DocumentNode.SelectNodes(xpath);
                if (nodeRem != null && nodeRem.Count > 0)
                    foreach (var n in nodeRem)
                        n.Remove();

                // gán lại cho nó xóa lần tiếp
                sourceReturn = htmlDocRem.DocumentNode.OuterHtml;
            }
            catch (Exception ex)
            {
            }

            return sourceReturn;
        }

        public static string GetSourceFromUrl(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                var source = Regex.Replace(url, @"http:\/\/|https:\/\/|www(.*?).", ""); // xóa đầu http hay https
                source = Regex.Replace(source, @"(\/|\?)([\s\S]+)", ""); // xóa đuôi nếu cuối cùng có dấu /

                var arr = source.Split('/');
                if (arr.Length > 0) source = arr[0];

                return source;
            }

            return "";
        }

        public static string GetDomainFromUrl(string url)
        {
            var uri = new Uri(url);
            return uri.Scheme + "://" + uri.Host;
        }

        public static DateTime UnixTimestampToDateTime(double unixTimeStamp)
        {
            //unixTimeStamp = 1537149600000;
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);
            return dtDateTime;

            // trả ra: 9/17/2018 2:00:00 AM
        }

        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return dateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public static List<string> GetListImgFromHTML(string htmlImage)
        {
            var lisReturn = new List<string>();

            var tagImg = RegexFilters(htmlImage, "<img.*?>");

            if (tagImg != null && tagImg.Count > 0)
                foreach (var img in tagImg)
                {
                    var filePath = "";
                    if (img.ToLower().Contains("src"))
                        filePath = RegexGetTextFromHtml(img, "(src[\\s]{0,}=[\\s]{0,}('|\")).*?('|\")", "",
                            "(src[\\s]{0,}=[\\s]{0,}('|\"))|('|\")", true);

                    if (filePath.ToLower().Contains("data:image"))
                    {
                        filePath = FindImageFromBase64(img);
                        if (!string.IsNullOrEmpty(filePath)) lisReturn.Add(filePath);
                    }
                    else
                    {
                        lisReturn.Add(filePath);
                    }
                }

            return lisReturn;
        }

        public static string FindImageFromBase64(string htmlImage)
        {
            var tagImg = RegexFilter(htmlImage, "<img.*?>");

            if (!string.IsNullOrEmpty(tagImg))
            {
                var lisAtt = RegexFilters(tagImg, "('|\").*?('|\")");
                if (lisAtt != null && lisAtt.Count > 0)
                {
                    var img = "";
                    foreach (var att in lisAtt)
                    {
                        if (att.ToLower().Contains("data:image")) continue;

                        if (att.ToLower().Contains(".jpg") || att.ToLower().Contains(".gif") ||
                            att.ToLower().Contains(".png") ||
                            att.ToLower().Contains(".jpeg"))
                            return img = Regex.Replace(att, "('|\")|('|\")", "").Trim();
                    }
                }
            }

            return "";
        }

        public static string FixPathImage(string src, string domain)
        {
            try
            {
                if (string.IsNullOrEmpty(src)) return "";

                if (src.StartsWith("http")) // bắt đầu với htttp
                    return src;

                if (src.StartsWith(
                        "//")) // //img.giaoduc.net.vn/w1050/uploaded/2019/aslyefjpeag/2019_11_03/5c4fbc86ce3b6cdd18774a99a3371094.jpg
                {
                    // thiếu nghi thức
                    if (domain.StartsWith("https"))
                        return "https:" + src;
                    return "http:" + src;
                }

                if (src.StartsWith("/")) // bắt đầu bằng dấu
                    return domain + src;

                if (src.StartsWith("./")) // bắt đầu bằng dấu ./
                    return domain + src.Remove(0, 1);

                if (src.StartsWith("../")) // bắt đầu bằng dấu ../
                    return domain + src.Remove(0, 2);

                if (src.StartsWith("../../")) // bắt đầu bằng dấu ../../
                    return domain + src.Remove(0, 5);

                // trường hợp không có dấu gì đằng trước
                return domain + "/" + src;
            }
            catch
            {
            }

            return "";
        }

        public static string FixPathURL(string url, string domain)
        {
            try
            {
                if (url.StartsWith("http://") || url.StartsWith("HTTP://") || url.StartsWith("https://") ||
                    url.StartsWith("HTTPS://")) // nó bắt đầu bằng http rồi, chứng tỏ link tuyệt đối
                    return url;

                if (!url.Contains(domain)) // không chứa đựng domain
                {
                    if (url.StartsWith("//"))
                    {
                        // chỉ lấy schema
                        var isHttps = false;
                        if (domain.StartsWith("https")) isHttps = true;

                        if (isHttps)
                            url = "https:" + url;
                        else
                            url = "http:" + url;

                        return url;
                    }

                    if (url.StartsWith("/")) // bắt đầu có dấu / rồi
                    {
                        url = domain + url;
                        return url;
                    }

                    if (url.StartsWith("./"))
                    {
                        url = domain + url.Remove(0, 1); // xóa đi dấu chấm ở đầu
                        return url;
                    }

                    if (url.StartsWith("../../")) // ../../
                    {
                        url = domain + url.Remove(0, 5); // xóa đi dấu chấm ở đầu
                        return url;
                    }

                    if (url.StartsWith("../"))
                    {
                        url = domain + url.Remove(0, 2); // xóa đi dấu chấm ở đầu
                        return url;
                    }

                    if (url.StartsWith("~/"))
                    {
                        url = domain + url.Remove(0, 1); // xóa đi dấu ~ ở đầu
                        return url;
                    }

                    url = domain + "/" + url; // không domain và không có dấu đặc biệt đằng trước
                }

                return url;
            }
            catch (Exception ex)
            {
            }

            return url;
        }

        public static bool CheckUrlInsideWebsite(string url, string domain)
        {
            // xóa đi cái đầu http của cả 2
            var urlRemoved = url.Replace("https://", "").Replace("http://", "").Replace("www.", "");
            var domainRemoved = domain.Replace("https://", "").Replace("http://", "").Replace("www.", "");

            if (urlRemoved.StartsWith(domainRemoved))
                return true;
            return false;
        }

        public static string ReplaceUnicodeWithLine(string str_input)
        {
            try
            {
                #region replace a

                var part_a = @"[á|à|ả|ã|ạ|ă|ắ|ằ|ẳ|ẵ|ặ|â|ấ|ầ|ẩ|ẫ|ậ]";
                str_input = Regex.Replace(str_input, part_a, "a", RegexOptions.IgnoreCase);

                #endregion replace a

                #region replace e

                var part_e = @"[é|è|ẹ|ẻ|ẽ|ê|ế|ề|ệ|ể|ễ]";
                str_input = Regex.Replace(str_input, part_e, "e", RegexOptions.IgnoreCase);

                #endregion replace e

                #region replace o

                var part_o = @"[ó|ò|ỏ|õ|ọ|ơ|ớ|ờ|ở|ỡ|ợ|ô|ố|ồ|ổ|ỗ|ộ|ố]"; // chữ o thường ò
                str_input = Regex.Replace(str_input, part_o, "o", RegexOptions.IgnoreCase);

                #endregion replace o

                #region replace u

                var part_u = @"[ú|ù|ủ|ũ|ụ|ư|ứ|ừ|ử|ữ|ự]";
                str_input = Regex.Replace(str_input, part_u, "u", RegexOptions.IgnoreCase);

                #endregion replace u

                #region replace i

                var part_i = @"[í|ì|ĩ|ị|ỉ]";
                str_input = Regex.Replace(str_input, part_i, "i", RegexOptions.IgnoreCase);

                #endregion replace i

                #region replace d

                var part_d = @"[đ]";
                str_input = Regex.Replace(str_input, part_d, "d", RegexOptions.IgnoreCase);

                #endregion replace d

                #region replace y

                var part_y = @"[ý|ỳ|ỷ|ỹ|ỵ]";
                str_input = Regex.Replace(str_input, part_y, "y", RegexOptions.IgnoreCase);

                #endregion replace y

                #region replace ký tự đặc biệt

                var partSpec = @"[!|@|#|=|$|%|^|&|*|\(|\)|+|\\|\[\]|{|}|:|;|\?|<|>|\.|,]";
                str_input = Regex.Replace(str_input, partSpec, "");

                #endregion replace ký tự đặc biệt

                #region có nhiều khoảng trắng - 1 khoảng trắng

                var partMulSpace = @"[\s]{2,}";
                str_input = Regex.Replace(str_input, partMulSpace, " ").Trim();

                #endregion có nhiều khoảng trắng - 1 khoảng trắng

                #region khỏang trắng thành dấu _

                var partSpace = @"[\s]";
                str_input = Regex.Replace(str_input, partSpace, "-");

                str_input = str_input.Replace('/', '-');

                #endregion khỏang trắng thành dấu _

                #region nhiều gạch thành 1 gạch _

                var partLine = @"[-]{2,}";
                str_input = Regex.Replace(str_input, partLine, "-");

                #endregion nhiều gạch thành 1 gạch _
            }
            catch
            {
                str_input = "";
            }

            str_input = str_input.ToLower();

            return str_input;
        }

        /// <summary>
        ///     Ghi dữ liệu vào Richtextbox
        /// </summary>
        /// <param name="content">Nội dung</param>
        /// <param name="rtbResult">Richtextbox control</param>
        public static void WriteToBox(string content, RichTextBox rtbResult)
        {
            if (rtbResult.Lines.Length > 100) rtbResult.Clear();
            rtbResult.AppendText(content + Environment.NewLine);
        }

        public static void WriteToBox(Exception ex, RichTextBox rtbResult)
        {
            if (rtbResult.Lines.Length > 100) rtbResult.Clear();
            rtbResult.AppendText("\n--- --- Đã xảy ra sự cố: " + ex.Message + Environment.NewLine);
        }
    }
}