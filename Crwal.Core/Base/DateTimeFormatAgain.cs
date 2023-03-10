using System;
using System.Text.RegularExpressions;

namespace Crwal.Core.Base
{
    public class DateTimeFormatAgain
    {
        /// <summary>
        ///     Thuộc tính chứa thông điệp lỗi
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        ///     Tìm giờ phút trong 1 đoạn text
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public string GetHour(string strDate)
        {
            var time = "";

            var regTime = Regex.IsMatch(strDate,
                @"([\d]{1,2} phút trước)|([\d]{1,2} minute ago)|([\d]{1,2} minutes ago)", RegexOptions.IgnoreCase);
            if (regTime)
            {
                try
                {
                    var minute = Convert.ToInt32(Utilities.RegexFilter(strDate, @"[\d]{1,2}"));
                    time = DateTime.Now.AddMinutes(-minute).ToString("hh:mm tt");
                }
                catch
                {
                    time = DateTime.Now.ToString("hh:mm tt");
                }

                return time;
            }

            regTime = Regex.IsMatch(strDate,
                @"([\d]{1,2} tiếng trước)|([\d]{1,2} giờ trước)|([\d]{1,2} hour ago)|([\d]{1,2} hours ago)|([\d]{1,2} hours trước)",
                RegexOptions.IgnoreCase);
            if (regTime)
            {
                try
                {
                    var hour = Convert.ToInt32(Utilities.RegexFilter(strDate, @"[\d]{1,2}"));
                    time = DateTime.Now.AddHours(-hour).ToString("hh:mm tt");
                }
                catch
                {
                    time = DateTime.Now.ToString("hh:mm tt");
                }

                return time;
            }

            // Cách đây 6 giờ
            regTime = Regex.IsMatch(strDate, @"Cách đây [\d]+ giờ", RegexOptions.IgnoreCase);
            if (regTime)
            {
                try
                {
                    var hour = Convert.ToInt32(Utilities.RegexFilter(strDate, @"[\d]{1,2}"));
                    time = DateTime.Now.AddHours(-hour).ToString("hh:mm tt");
                }
                catch
                {
                    time = DateTime.Now.ToString("hh:mm tt");
                }

                return time;
            }

            // nếu tồn tại dạng này: 3:52:15 CH
            regTime = Regex.IsMatch(strDate, @"[\d]+:[\d]+:[\d]+[\s]CH|[\d]+:[\d]+:[\d]+[\s]SA",
                RegexOptions.IgnoreCase);
            if (regTime)
            {
                if (strDate.ToLower().Contains(" sa"))
                    time = Utilities.RegexFilter(strDate, @"[\d]+:[\d]+:[\d]+") + " AM";

                if (strDate.ToLower().Contains(" ch"))
                    time = Utilities.RegexFilter(strDate, @"[\d]+:[\d]+:[\d]+") + " PM";
                return time;
            }

            // nếu tồn tại kiểu 8h11
            regTime = Regex.IsMatch(strDate, @"([\d]+h[\d]+)", RegexOptions.IgnoreCase);
            if (regTime)
            {
                time = Utilities.RegexFilter(strDate, @"([\d]+h[\d]+)"); // cố gắng dò kiểu 8h11
                if (!string.IsNullOrEmpty(time)) time = time.Replace("h", ":");
            }

            // nếu tồn tại kiểu 3:52:52 am
            regTime = Regex.IsMatch(strDate, @"[\d]+:[\d]+:[\d]+ (AM|PM)", RegexOptions.IgnoreCase);
            if (regTime)
            {
                time = Utilities.RegexFilter(strDate, @"[\d]+:[\d]+:[\d]+ (AM|PM)"); //3:52:52 am
                if (!string.IsNullOrEmpty(time))
                {
                    var isAM = time.ToLower().Contains("am");

                    time = Utilities.RegexFilter(strDate, @"[\d]{2,}:[\d]{2,}"); // lấy ra giờ, phút

                    var arrTime = time.Split(':');
                    if (arrTime.Length == 2)
                    {
                        if (isAM) // giờ buổi sáng giữ nguyên
                            return time;

                        // giờ buổi chiều + 12
                        var hour = Convert.ToInt32(arrTime[0]);
                        if (hour < 13) hour = hour + 12;

                        return hour + ":" + arrTime[1];
                    }
                }
            }

            // nếu tồn tại kiểu 28/10/2015 3:52 am - chu y kieu nay dang truoc gio co khoang trang
            regTime = Regex.IsMatch(strDate, @"([\s]+[\d]+:[\d]+)[\s]+(AM|PM)", RegexOptions.IgnoreCase);
            if (regTime)
            {
                time = Utilities.RegexFilter(strDate, @"([\s]+[\d]+:[\d]+)[\s]+(AM|PM)").Trim();
                // 13:42 PM
                if (!string.IsNullOrEmpty(time))
                {
                    var isAM = time.ToLower().Contains("am");

                    time = Utilities.RegexFilter(strDate, @"[\d]{2,}:[\d]{2,}");
                    var arrTime = time.Split(':');
                    if (arrTime.Length == 2)
                    {
                        if (isAM) // giờ buổi sáng giữ nguyên
                            return time;

                        // giờ buổi chiều + 12
                        var hour = Convert.ToInt32(arrTime[0]);
                        if (hour < 13) hour = hour + 12;

                        return hour + ":" + arrTime[1];
                    }
                }
            }

            // nếu tồn tại kiểu 3:52am - chu y kieu nay dang truoc gio co khoang trang
            regTime = Regex.IsMatch(strDate.ToUpper(), @"([\s]+[\d]+:[\d]+)(AM|PM)", RegexOptions.IgnoreCase);
            if (regTime)
            {
                time = Utilities.RegexFilter(strDate, @"([\s]+[\d]+:[\d]+)").Trim(); // 3:52am

                var apm = Utilities.RegexFilter(strDate, @"(AM|PM)").Trim();

                time = time + " " + apm;
            }

            // nếu tồn tại kiểu 4:38 Chiều
            regTime = Regex.IsMatch(strDate, @"[\d]+:[\d]+[\s]+(Chiều|Sáng)", RegexOptions.IgnoreCase);
            if (regTime)
            {
                time = Utilities.RegexFilter(strDate, @"[\d]+:[\d]+[\s]+(Chiều|Sáng)");
                if (time.ToLower().Contains(" chiều")) time = Utilities.RegexFilter(time, @"[\d]+:[\d]+") + " PM";

                if (time.ToLower().Contains(" sáng")) time = Utilities.RegexFilter(time, @"[\d]+:[\d]+") + " AM";
            }

            //8:46:18
            regTime = Regex.IsMatch(strDate, @"[\d]+:[\d]+:[\d]+", RegexOptions.IgnoreCase);
            if (regTime)
            {
                time = Utilities.RegexFilter(strDate, @"[\d]+:[\d]+:[\d]+"); // cố gắng dò kiểu 10:03
                var arrTime = time.Split(':');
                if (arrTime.Length == 3)
                {
                    var hour = arrTime[0];
                    if (hour.Length == 1) hour = "0" + hour;

                    var mimute = arrTime[1];
                    if (mimute.Length == 1) mimute = "0" + mimute;

                    return hour + ":" + mimute;
                }
            }

            // 09:12 AM
            regTime = Regex.IsMatch(strDate, @"[\d]{2,}:[\d]{2,} (am|pm)", RegexOptions.IgnoreCase);
            if (regTime)
            {
                time = Utilities.RegexFilter(strDate, @"[\d]{2,}:[\d]{2,} (am|pm)"); // cố gắng dò kiểu 09:12 AM
                if (!string.IsNullOrEmpty(time))
                {
                    var isAM = time.ToLower().Contains("am");

                    time = Utilities.RegexFilter(strDate, @"[\d]{2,}:[\d]{2,}");
                    var arrTime = time.Split(':');
                    if (arrTime.Length == 2)
                    {
                        if (isAM) // giờ buổi sáng giữ nguyên
                            return time;

                        // giờ buổi chiều + 12
                        var hour = Convert.ToInt32(arrTime[0]);
                        if (hour < 13) hour = hour + 12;

                        return hour + ":" + arrTime[1];
                    }
                }
            }

            // [\d]{2,}:[\d]{2,} -> 10:03
            regTime = Regex.IsMatch(strDate, @"[\d]{2,}:[\d]{2,}", RegexOptions.IgnoreCase);
            if (regTime)
            {
                time = Utilities.RegexFilter(strDate, @"[\d]{2,}:[\d]{2,}"); // cố gắng dò kiểu 10:03
                return time;
            }

            // nếu tồn tại kiểu 08.11
            regTime = Regex.IsMatch(strDate, @"[\s]+([\d]{2}\.[\d]+)", RegexOptions.IgnoreCase);
            if (regTime)
            {
                time = Utilities.RegexFilter(strDate, @"[\s]+([\d]{2}\.[\d]+)"); // cố gắng dò kiểu 8h11
                if (!string.IsNullOrEmpty(time)) time = time.Replace(".", ":");
            }

            if (time == "") // các kiểu rồi mà vẫn chưa tìm ra time thì thử lấy dạng 12:00 xem sao
            {
                time = Utilities.RegexFilter(strDate, @"[\d]+:[\d]+");

                // kiem tra them am hay pm
                var am = Utilities.RegexFilter(strDate, @"(:[\d]+)([\s]+)(am)"); // chua dung am
                if (am != "") time = time + " AM";

                var pm = Utilities.RegexFilter(strDate, @"(:[\d]+)([\s]+)(pm)"); // chua dung am
                if (pm != "") time = time + " PM";
            }

            regTime = Regex.IsMatch(strDate, @"[\d]{6,}");
            if (regTime)
            {
                time = UnixTimeStampToHour(strDate);
                if (!string.IsNullOrEmpty(time)) return time;
            }

            if (strDate.ToLower().Contains("giờ trước"))
            {
                // một giờ trước
                regTime = Regex.IsMatch(strDate, @"một giờ trước", RegexOptions.IgnoreCase);
                if (regTime)
                {
                    try
                    {
                        time = DateTime.Now.AddHours(-1).ToString("hh:mm tt");
                    }
                    catch
                    {
                        time = DateTime.Now.ToString("hh:mm tt");
                    }

                    return time;
                }

                // hai giờ trước
                regTime = Regex.IsMatch(strDate, @"hai giờ trước", RegexOptions.IgnoreCase);
                if (regTime)
                {
                    try
                    {
                        time = DateTime.Now.AddHours(-2).ToString("hh:mm tt");
                    }
                    catch
                    {
                        time = DateTime.Now.ToString("hh:mm tt");
                    }

                    return time;
                }

                // ba giờ trước
                regTime = Regex.IsMatch(strDate, @"ba giờ trước", RegexOptions.IgnoreCase);
                if (regTime)
                {
                    try
                    {
                        time = DateTime.Now.AddHours(-3).ToString("hh:mm tt");
                    }
                    catch
                    {
                        time = DateTime.Now.ToString("hh:mm tt");
                    }

                    return time;
                }
            }

            if (strDate.ToLower().Contains("phút trước"))
            {
                // một phút trước
                regTime = Regex.IsMatch(strDate, @"một phút trước", RegexOptions.IgnoreCase);
                if (regTime)
                {
                    try
                    {
                        time = DateTime.Now.AddMinutes(-1).ToString("hh:mm tt");
                    }
                    catch
                    {
                        time = DateTime.Now.ToString("hh:mm tt");
                    }

                    return time;
                }

                // hai phút trước
                regTime = Regex.IsMatch(strDate, @"hai phút trước", RegexOptions.IgnoreCase);
                if (regTime)
                {
                    try
                    {
                        time = DateTime.Now.AddMinutes(-2).ToString("hh:mm tt");
                    }
                    catch
                    {
                        time = DateTime.Now.ToString("hh:mm tt");
                    }

                    return time;
                }

                // ba phút trước
                regTime = Regex.IsMatch(strDate, @"ba phút trước", RegexOptions.IgnoreCase);
                if (regTime)
                {
                    try
                    {
                        time = DateTime.Now.AddMinutes(-3).ToString("hh:mm tt");
                    }
                    catch
                    {
                        time = DateTime.Now.ToString("hh:mm tt");
                    }

                    return time;
                }
            }

            if (strDate.ToLower().Contains("giây trước")) time = DateTime.Now.ToString("hh:mm tt");

            // chỉ có 1h
            if (Regex.IsMatch(strDate, @"[\d]+h") || Regex.IsMatch(strDate, @"[\d]+ h"))
            {
                var hour = Convert.ToInt32(Utilities.RegexFilter(strDate, @"[\d]{1,2}"));
                time = DateTime.Now.AddHours(-hour).ToString("hh:mm tt");
            }

            // chỉ có 1 ngày
            if (Regex.IsMatch(strDate, @"[\d]+ ngày") || Regex.IsMatch(strDate, @"[\d]+ tháng") ||
                Regex.IsMatch(strDate, @"[\d]+ năm")) time = DateTime.Now.ToString("hh:mm tt");

            return time;
        }

        /// <summary>
        ///     Lấy ngày tháng năm
        /// </summary>
        /// <param name="sDate"></param>
        /// <param name="ruleDate"></param>
        /// <returns>yyyy-MM-dd</returns>
        public string GetDate(string sDate, string ruleDate)
        {
            try
            {
                #region có truyền vào Rule date

                if (ruleDate != string.Empty) return GetDateByRule(sDate, ruleDate);

                #endregion có truyền vào Rule date

                var dateBySearch = GetDateBySearchText(sDate, ruleDate);

                if (!string.IsNullOrEmpty(dateBySearch)) // có tìm ra kết quả
                    return dateBySearch;

                // không tìm ra kết quả xử lý tiếp
                dateBySearch = GetDateByPattern(sDate, ruleDate);
                if (!string.IsNullOrEmpty(dateBySearch)) // có tìm ra kết quả
                    return dateBySearch;

                #region chỉ có: 1h, 1 ngày

                // 1h
                if (Regex.IsMatch(sDate, @"[\d]+h") || Regex.IsMatch(sDate, @"[\d]+ h"))
                    return DateTime.Now.ToString("yyyy-MM-dd");

                //1 ngày
                if (Regex.IsMatch(sDate, @"[\d]+ ngày") || Regex.IsMatch(sDate, @"[\d]+ tháng") ||
                    Regex.IsMatch(sDate, @"[\d]+ năm"))
                {
                    var d = Convert.ToInt32(Utilities.RegexFilter(sDate, @"[\d]+"));
                    return DateTime.Now.AddDays(-d).ToString("yyyy-MM-dd");
                }

                //1 tháng
                if (Regex.IsMatch(sDate, @"[\d]+ tháng"))
                {
                    var m = Convert.ToInt32(Utilities.RegexFilter(sDate, @"[\d]+"));
                    return DateTime.Now.AddMonths(-m).ToString("yyyy-MM-dd");
                }

                //1 năm
                if (Regex.IsMatch(sDate, @"[\d]+ năm"))
                {
                    var y = Convert.ToInt32(Utilities.RegexFilter(sDate, @"[\d]+"));
                    return DateTime.Now.AddYears(-y).ToString("yyyy-MM-dd");
                }

                #endregion chỉ có: 1h, 1 ngày

                if (!string.IsNullOrEmpty(dateBySearch)) // vẫn không có thì đưa vào dạng này xem sao
                    return GetDateByRule(sDate, "dd/MM/yyyy");
            }
            catch
            {
            }

            return "";
        }

        /// <summary>
        ///     lấy ngày tháng, năm theo 1 quy tắc mình đưa vào
        /// </summary>
        /// <param name="sDate">chuỗi chứa ngày tháng</param>
        /// <param name="ruleDate"></param>
        /// <returns></returns>
        public string GetDateByRule(string sDate, string ruleDate)
        {
            try
            {
                #region có truyền vào Rule date dạng dd/MM/yyyy hay dd/MM

                if (ruleDate != string.Empty)
                {
                    string[] arr;
                    var dateTmp = sDate;
                    switch (ruleDate)
                    {
                        case "dd/MM": // chi co ngay va thang
                            if (sDate.Length > 5) dateTmp = Utilities.RegexFilter(dateTmp, @"[\d]+\/[\d]+");

                            arr = dateTmp.Split('/');
                            if (arr.Length == 2) return arr[0] + "-" + arr[1] + "-" + DateTime.Now.Year;
                            break;

                        case "dd.MM": // chi co ngay va thang
                            if (sDate.Length > 5) dateTmp = Utilities.RegexFilter(dateTmp, @"[\d]+\.[\d]+");

                            arr = dateTmp.Split('.');
                            if (arr.Length == 2) return arr[0] + "-" + arr[1] + "-" + DateTime.Now.Year;
                            break;

                        case "dd-MM": // chi co ngay va thang
                            if (sDate.Length > 5) dateTmp = Utilities.RegexFilter(dateTmp, @"[\d]+-[\d]+");

                            arr = dateTmp.Split('-');
                            if (arr.Length == 2) return arr[0] + "-" + arr[1] + "-" + DateTime.Now.Year;
                            break;

                        case "dd/MM/yyyy":
                            if (sDate.Length > 10) dateTmp = Utilities.RegexFilter(dateTmp, @"[\d]+\/[\d]+\/[\d]{4}");

                            arr = dateTmp.Split('/');
                            if (arr.Length >= 3) return arr[2] + "-" + arr[1] + "-" + arr[0];
                            break;

                        case "MM/dd/yyyy":
                            if (sDate.Length > 10) dateTmp = Utilities.RegexFilter(dateTmp, @"[\d]+\/[\d]+\/[\d]{4}");

                            arr = dateTmp.Split('/');
                            if (arr.Length >= 3) return arr[2] + "-" + arr[0] + "-" + arr[1];
                            break;

                        case "dd/MM/yy":
                            if (sDate.Length > 8) dateTmp = Utilities.RegexFilter(dateTmp, @"[\d]+\/[\d]+\/[\d]{2,}");

                            arr = dateTmp.Split('/');
                            if (arr.Length >= 3) return "20" + arr[2] + "-" + arr[1] + "-" + arr[0];
                            break;

                        case "MM/dd/yy":
                            if (sDate.Length > 8) dateTmp = Utilities.RegexFilter(dateTmp, @"[\d]+\/[\d]+\/[\d]{2,}");

                            arr = dateTmp.Split('/');
                            if (arr.Length >= 3) return "20" + arr[2] + "-" + arr[0] + "-" + arr[1];
                            break;

                        case "MMM dd, yyyy": // ex: May 14, 2014
                            if (sDate.Length > 12)
                                dateTmp = Utilities.RegexFilter(dateTmp, @"[\w]{3}[\s]+[\d]+public int \s]+[\d]{4}");

                            arr = dateTmp.Split(' ');
                            if (arr.Length >= 3)
                                return arr[2] + "-" + GetMonthFromText_English(arr[0].ToLower()) + "-" +
                                       arr[1].Replace(",", "");
                            break;

                        case "dd-MM-yyyy":
                            if (sDate.Length > 10) // 23-02-2014, <span class='time'>07:34, hoặc 1-2-2014
                                dateTmp = Utilities.RegexFilter(dateTmp, @"[\d]+-[\d]+-[\d]{4}");

                            arr = dateTmp.Split('-');
                            if (arr.Length >= 3)
                            {
                                var day = arr[0];
                                if (day.Length == 1) day = "0" + day;

                                var month = arr[1];
                                if (month.Length == 1) month = "0" + month;

                                return arr[2] + "-" + month + "-" + day;
                            }

                            break;

                        case "yyyy-MM-dd":
                            if (sDate.Length > 10) // 23-02-2014, <span class='time'>07:34
                                dateTmp = Utilities.RegexFilter(dateTmp, @"[\d]{4}-[\d]+-[\d]+");

                            arr = dateTmp.Split('-');
                            if (arr.Length >= 3) return arr[0] + "-" + arr[1] + "-" + arr[2];
                            break;

                        case "yyyy.MM.dd":
                            if (sDate.Length > 10) dateTmp = Utilities.RegexFilter(dateTmp, @"[\d]{4}\.[\d]+\.[\d]+");
                            return dateTmp.Replace(".", "-");

                        case "MM-dd-yyyy":
                            if (sDate.Length > 10) // 11-04-2011, <span class='time'>11:03 am
                                dateTmp = Utilities.RegexFilter(dateTmp, @"[\d]+-[\d]+-[\d]{4}");

                            arr = dateTmp.Split('-');
                            if (arr.Length >= 3) return arr[2] + "-" + arr[0] + "-" + arr[1];
                            break;

                        case "dd.MM.yyyy":
                            if (sDate.Length > 10) dateTmp = Utilities.RegexFilter(dateTmp, @"[\d]+\.[\d]+\.[\d]{4}");

                            arr = dateTmp.Split('.');
                            if (arr.Length >= 3) return arr[2] + "-" + arr[1] + "-" + arr[0];
                            break;

                        case "dd.MM.yy":
                            if (sDate.Length > 10) dateTmp = Utilities.RegexFilter(dateTmp, @"[\d]+\.[\d]+\.[\d]{2}");

                            arr = dateTmp.Split('.');
                            if (arr.Length >= 3) return "20" + arr[2] + "-" + arr[1] + "-" + arr[0];
                            break;

                        case "yyyy/MM/dd":
                            if (sDate.Length > 10) dateTmp = Utilities.RegexFilter(dateTmp, @"[\d]{4}\/[\d]+\/[\d]+");

                            arr = dateTmp.Split('/');
                            if (arr.Length >= 3) return arr[0] + "-" + arr[1] + "-" + arr[2];
                            break;

                        case "ngày dd tháng MM":

                            if (sDate.Length > 10)
                            {
                                dateTmp = Utilities.RegexFilter(dateTmp, @"ngày [\d]+ tháng [\d]+");

                                if (!string.IsNullOrEmpty(dateTmp))
                                {
                                    var lisDig = Utilities.RegexFilters(dateTmp, @"[\d]+");
                                    if (lisDig != null && lisDig.Count == 2)
                                        return lisDig[0] + "-" + lisDig[1] + "-" + DateTime.Now.Year;
                                }
                            }

                            break;
                    }
                }

                #endregion có truyền vào Rule date dạng dd/MM/yyyy hay dd/MM
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }

            return "";
        }

        public string GetDateBySearchText(string sDate, string ruleDate)
        {
            try
            {
                #region có chữ hôm nay, hôm qua, today, yesterday, cách đây giờ, phút, một ngày trước.... - return luôn

                if (sDate.ToLower().Contains("hôm nay") || sDate.ToLower().Contains("today"))
                    return DateTime.Now.ToString("yyyy-MM-dd");

                if (sDate.ToLower().Contains("hôm qua") || sDate.ToLower().Contains("yesterday"))
                    return DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

                var isMinute = Regex.IsMatch(sDate,
                    @"([\d]{1,2} phút trước)|([\d]{1,2} minutes ago)|(cách đây [\d]{1,2} phút)",
                    RegexOptions.IgnoreCase);
                if (isMinute)
                {
                    var minute = Utilities.RegexFilter(sDate,
                        @"([\d]{1,2} phút trước)|([\d]{1,2} minutes ago)|(cách đây [\d]{1,2} phút)");
                    var mm = Convert.ToInt32(Utilities.RegexFilter(minute, @"[\d]+"));

                    return DateTime.Now.AddMinutes(-mm).ToString("yyyy-MM-dd");
                }

                // 1 tiếng trước
                var isHour = Regex.IsMatch(sDate,
                    @"([\d]{1,2} tiếng trước)|([\d]{1,2} giờ trước)|([\d]{1,2} hours trước)|([\d]{1,2} hours ago)|(cách đây [\d]{1,2} giờ)",
                    RegexOptions.IgnoreCase);
                if (isHour)
                {
                    var hour = Utilities.RegexFilter(sDate,
                        @"([\d]{1,2} tiếng trước)|([\d]{1,2} giờ trước)|([\d]{1,2} hours trước)|([\d]{1,2} hours ago)|(cách đây [\d]{1,2} giờ)");
                    var hh = Convert.ToInt32(Utilities.RegexFilter(hour, @"[\d]+"));

                    return DateTime.Now.AddHours(-hh).ToString("yyyy-MM-dd");
                }

                var isAfterDate = Regex.IsMatch(sDate, @"[\d]+ ngày trước", RegexOptions.IgnoreCase);
                if (isAfterDate)
                {
                    var d = Utilities.RegexFilter(sDate, @"[\d]+");
                    if (!string.IsNullOrEmpty(d))
                    {
                        var iD = Convert.ToInt32(d);
                        return DateTime.Now.AddDays(-iD).ToString("yyyy-MM-dd");
                    }
                }

                // cách đây 8 ngày
                var isAgoDay = Regex.IsMatch(sDate, @"cách đây [\d]+ ngày", RegexOptions.IgnoreCase);
                if (isAgoDay)
                {
                    var d = Utilities.RegexFilter(sDate, @"[\d]+");
                    if (!string.IsNullOrEmpty(d))
                    {
                        var iD = Convert.ToInt32(d);
                        return DateTime.Now.AddDays(-iD).ToString("yyyy-MM-dd");
                    }
                }

                // cách đây 8 tháng
                var isAgoMonth = Regex.IsMatch(sDate, @"cách đây [\d]+ tháng", RegexOptions.IgnoreCase);
                if (isAgoMonth)
                {
                    var d = Utilities.RegexFilter(sDate, @"[\d]+");
                    if (!string.IsNullOrEmpty(d))
                    {
                        var iD = Convert.ToInt32(d);
                        return DateTime.Now.AddMonths(-iD).ToString("yyyy-MM-dd");
                    }
                }

                #region một ngày trước

                if (sDate.ToLower().Contains("ngày trước"))
                {
                    if (sDate.ToLower().Contains("một ngày trước"))
                        return DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

                    // hai ngày trước
                    if (sDate.ToLower().Contains("hai ngày trước"))
                        return DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd");

                    // ba ngày trước
                    if (sDate.ToLower().Contains("ba ngày trước"))
                        return DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd");

                    // ba ngày trước
                    if (sDate.ToLower().Contains("bốn ngày trước"))
                        return DateTime.Now.AddDays(-4).ToString("yyyy-MM-dd");

                    if (sDate.ToLower().Contains("năm ngày trước"))
                        return DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd");

                    if (sDate.ToLower().Contains("sáu ngày trước"))
                        return DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd");

                    if (sDate.ToLower().Contains("bảy ngày trước"))
                        return DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");

                    if (sDate.ToLower().Contains("bảy ngày trước"))
                        return DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");

                    var isNgayTruoc = Regex.IsMatch(sDate, @"[\d]+ ngày trước", RegexOptions.IgnoreCase);
                    if (isNgayTruoc)
                    {
                        var d = Utilities.RegexFilter(sDate, @"[\d]+");
                        if (!string.IsNullOrEmpty(d))
                        {
                            var iD = Convert.ToInt32(d);
                            return DateTime.Now.AddMonths(-iD).ToString("yyyy-MM-dd");
                        }
                    }
                }

                #endregion một ngày trước

                #region một giờ trước, phút trước

                if (sDate.ToLower().Contains("giờ trước") || sDate.ToLower().Contains("phút trước") ||
                    sDate.ToLower().Contains("giây trước"))
                    if (sDate.ToLower().Contains("một giờ trước"))
                        return DateTime.Now.ToString("yyyy-MM-dd");

                #endregion một giờ trước, phút trước

                #region một tháng trước

                if (sDate.ToLower().Contains("tháng trước"))
                {
                    if (sDate.ToLower().Contains("một tháng trước"))
                        return DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");

                    // hai tháng trước
                    if (sDate.ToLower().Contains("hai tháng trước"))
                        return DateTime.Now.AddMonths(-2).ToString("yyyy-MM-dd");

                    // ba tháng trước
                    if (sDate.ToLower().Contains("ba tháng trước"))
                        return DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd");

                    var isThangTruoc = Regex.IsMatch(sDate, @"[\d]+ tháng trước", RegexOptions.IgnoreCase);
                    if (isThangTruoc)
                    {
                        var m = Utilities.RegexFilter(sDate, @"[\d]+");
                        if (!string.IsNullOrEmpty(m))
                        {
                            var iM = Convert.ToInt32(m);
                            return DateTime.Now.AddMonths(-iM).ToString("yyyy-MM-dd");
                        }
                    }
                }

                #endregion một tháng trước

                #region một năm trước

                if (sDate.ToLower().Contains("năm trước"))
                {
                    if (sDate.ToLower().Contains("một năm trước"))
                        return DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");

                    // hai năm trước
                    if (sDate.ToLower().Contains("hai năm trước"))
                        return DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd");

                    // ba năm trước
                    if (sDate.ToLower().Contains("ba năm trước"))
                        return DateTime.Now.AddYears(-3).ToString("yyyy-MM-dd");

                    var isNamTruoc = Regex.IsMatch(sDate, @"[\d]+ năm trước", RegexOptions.IgnoreCase);
                    if (isNamTruoc)
                    {
                        var y = Utilities.RegexFilter(sDate, @"[\d]+");
                        if (!string.IsNullOrEmpty(y))
                        {
                            var iY = Convert.ToInt32(y);
                            return DateTime.Now.AddYears(-iY).ToString("yyyy-MM-dd");
                        }
                    }
                }

                #endregion một năm trước

                #endregion có chữ hôm nay, hôm qua, today, yesterday, cách đây giờ, phút, một ngày trước.... - return luôn
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }

            return "";
        }

        public string GetDateByPattern(string sDate, string ruleDate)
        {
            try
            {
                #region không truyền vào Rule date, cố gắng tìm thêm xem có kết quả không?

                var dt = "";

                #region dò kiểu April 4

                var isMatch = Regex.IsMatch(sDate.ToLower(),
                    @"(jan|january|feb|february|mar|march|apr|april|may|jun|june|jul|july|aug|august|sep|september|oct|october|nov|november|dec|december) [\d]+");
                if (isMatch)
                {
                    var tmpDate = Utilities.RegexFilter(sDate,
                        @"(jan|january|feb|february|mar|march|apr|april|may|jun|june|jul|july|aug|august|sep|september|oct|october|nov|november|dec|december) [\d]+");
                    if (tmpDate != "")
                    {
                        var arr = tmpDate.Split(' ');
                        if (arr.Length > 1)
                        {
                            var month = GetMonthFromText_English(arr[0]);
                            return DateTime.Now.ToString("yyyy") + "-" + month + "-" + arr[1].Replace(",", "");
                        }
                    }
                }

                #endregion dò kiểu April 4

                #region Sunday 1 April 2018

                isMatch = Regex.IsMatch(sDate,
                    @"[\d]+ (january|jan|february|feb|march|mar|april|apr|may|may|june|jun|july|jul|august|aug|september|sep|october|oct|november|nov|december|dec) [\s]{0,}[\d]{4,}",
                    RegexOptions.IgnoreCase);
                if (isMatch)
                {
                    var tmpDate = Utilities.RegexFilter(sDate,
                        @"[\d]+ (january|jan|february|feb|march|mar|april|apr|may|may|june|jun|july|jul|august|aug|september|sep|october|oct|november|nov|december|dec) [\s]{0,}[\d]{4,}");
                    if (tmpDate != "")
                    {
                        var arr = tmpDate.Split(' '); // 1 April 2018
                        if (arr.Length > 2)
                        {
                            var day = arr[0];
                            var year = arr[2];
                            var month = GetMonthFromText_English(arr[1]);

                            return year + "-" + month + "-" + day;
                        }
                    }
                }

                #endregion Sunday 1 April 2018

                #region dò kiểu 28/05.2016

                isMatch = Regex.IsMatch(sDate.ToLower(), @"[\d]+\/[\d]+\.[\d]{4}", RegexOptions.IgnoreCase);
                if (isMatch)
                {
                    var subDate = Utilities.RegexFilter(sDate, @"[\d]+\/[\d]+\.[\d]{4}");

                    var lisDig = Utilities.RegexFilters(subDate, @"[\d]+");
                    if (lisDig.Count == 3) return lisDig[2] + "-" + lisDig[1] + "-" + lisDig[0];

                    return subDate;
                }

                #endregion dò kiểu 28/05.2016

                #region gặp kiểu ngày 31 tháng 12 năm 2014

                isMatch = Regex.IsMatch(sDate.ToLower(), @"ngày [\d]+ tháng [\d]+ năm [\d]+", RegexOptions.IgnoreCase);
                if (isMatch)
                {
                    var subDate = Utilities.RegexFilter(sDate, @"ngày[\s\S]+");
                    var lisDig = Utilities.RegexFilters(subDate, @"[\d]+");

                    if (lisDig.Count >= 3) return lisDig[2] + "-" + lisDig[1] + "-" + lisDig[0];
                }

                #endregion gặp kiểu ngày 31 tháng 12 năm 2014

                #region gặp kiểu:   27 Tháng 04, 2016 | 08:56 ->

                isMatch = Regex.IsMatch(sDate.ToLower(), @"[\d]+ Tháng [\d]+, [\d]{4}", RegexOptions.IgnoreCase);
                if (isMatch)
                {
                    var lisDig = Utilities.RegexFilters(sDate, @"[\d]+");
                    if (lisDig.Count >= 3) return lisDig[2] + "-" + lisDig[1] + "-" + lisDig[0];
                }

                #endregion gặp kiểu:   27 Tháng 04, 2016 | 08:56 ->

                #region Ngày 15 Tháng 5, 2015 | 07:49 PM

                isMatch = Regex.IsMatch(sDate.ToLower(), @"ngày [\d]+ tháng [\d]+, [\d]+", RegexOptions.IgnoreCase);
                if (isMatch)
                {
                    var lisDig = Utilities.RegexFilters(sDate, @"[\d]+");

                    if (lisDig.Count >= 3) return lisDig[2] + "-" + lisDig[1] + "-" + lisDig[0];
                }

                #endregion Ngày 15 Tháng 5, 2015 | 07:49 PM

                #region gặp định dạng kiểu: 29 tháng 11 2016

                isMatch = Regex.IsMatch(sDate, @"[\d]+ tháng [\d]+ [\d]{4,}", RegexOptions.IgnoreCase);
                if (isMatch)
                {
                    var lisDig = Utilities.RegexFilters(sDate, @"[\d]+");

                    if (lisDig.Count >= 3) return lisDig[2] + "-" + lisDig[1] + "-" + lisDig[0];
                }

                #endregion gặp định dạng kiểu: 29 tháng 11 2016

                #region gặp định dạng kiểu: 15 Tháng mười hai 2012

                isMatch = Regex.IsMatch(sDate, @"[\d]+ (tháng|Tháng) (.*?) [\d]{4}", RegexOptions.IgnoreCase);
                if (isMatch)
                {
                    var month = GetMonthFromText(sDate);

                    var tmpDate = Utilities.RegexFilter(sDate, @"[\d]+ (tháng|Tháng) (.*?) [\d]{4}");

                    var lisDig = Utilities.RegexFilters(tmpDate, @"[\d]+");
                    if (lisDig != null && lisDig.Count == 2) return lisDig[1] + "-" + month + "-" + lisDig[0];
                }

                #endregion gặp định dạng kiểu: 15 Tháng mười hai 2012

                #region Cập nhật08:45, Thứ hai Ngày 23, Tháng 2, 2015

                isMatch = Regex.IsMatch(sDate, @"Ngày [\d]+, Tháng [\d]+, [\d]{4}", RegexOptions.IgnoreCase);
                if (isMatch)
                {
                    var lisDig = Utilities.RegexFilters(sDate, @"[\d]+");
                    return lisDig[lisDig.Count - 1] + "-" + lisDig[lisDig.Count - 2] + "-" + lisDig[lisDig.Count - 3];
                }

                #endregion Cập nhật08:45, Thứ hai Ngày 23, Tháng 2, 2015

                #region Tháng 9 2, 2016

                isMatch = Regex.IsMatch(sDate, @"(tháng [\d]+ [\d]+, [\d]{4})", RegexOptions.IgnoreCase);
                if (isMatch)
                {
                    var tmpDate = Utilities.RegexFilter(sDate, @"(tháng [\d]+ [\d]+, [\d]{4})");
                    if (tmpDate != "")
                    {
                        var lisD = Utilities.RegexFilters(tmpDate, @"[\d]+");

                        if (lisD != null && lisD.Count > 2) return lisD[2] + "-" + lisD[0] + "-" + lisD[1];
                    }
                }

                #endregion Tháng 9 2, 2016

                #region dò xem kiểu này không:  Thứ ba, 29 Tháng 9 2015 17:24 hay Chủ nhật, 29 Tháng 9 2015 17:24

                isMatch = Regex.IsMatch(sDate.ToLower(),
                    @"(thứ )(.*?), [\d]{1,2} (tháng) [\d]{1,2} [\d]{4}|(chủ nhật,) [\d]{1,2} (tháng) [\d]{1,2} [\d]{4}",
                    RegexOptions.IgnoreCase);
                if (isMatch)
                {
                    var tmpDate = Utilities.RegexFilter(sDate,
                        @"(thứ )(.*?), [\d]{1,2} (tháng) [\d]{1,2} [\d]{4}|(chủ nhật,) [\d]{1,2} (tháng) [\d]{1,2} [\d]{4}");
                    if (tmpDate != "")
                    {
                        var arr = tmpDate.Split(' ');
                        return arr[5] + "-" + arr[4] + "-" + arr[2];
                    }
                }

                #endregion dò xem kiểu này không:  Thứ ba, 29 Tháng 9 2015 17:24 hay Chủ nhật, 29 Tháng 9 2015 17:24

                #region định dạng kiểu Thứ sáu, 31 Tháng 7 2015

                isMatch = Regex.IsMatch(sDate,
                    @"(Thứ|Chủ)([\s]+)(.*?)(,)([\s]+)([\d]+)([\s]+)(Tháng)([\s]+)([\d]+)([\s]+)([\d]{4})",
                    RegexOptions.IgnoreCase);
                if (isMatch)
                {
                    var tmpDate = Utilities.RegexFilter(sDate,
                        @"(Thứ|Chủ)([\s]+)(.*?)(,)([\s]+)([\d]+)([\s]+)(Tháng)([\s]+)([\d]+)([\s]+)([\d]{4})");
                    if (tmpDate != "")
                    {
                        var arr = sDate.Split(' ');
                        return arr[5] + "-" + arr[4] + "-" + arr[2];
                    }
                }

                #endregion định dạng kiểu Thứ sáu, 31 Tháng 7 2015

                #region gap kieu NGÀY 10 THÁNG 11, 2015 | 15:03 => http: //suckhoedoisong.vn/kip-thoi-cuu-nam-thanh-nien-bi-thanh-sat-2m-dam-xuyen-that-lung-qua-hau-mon-n109044.html

                isMatch = Regex.IsMatch(sDate.ToLower(), @"(NGÀY )[\d]+( THÁNG )[\d]+(, )[\d]+",
                    RegexOptions.IgnoreCase);
                if (isMatch)
                {
                    var tmpDate = Utilities.RegexFilter(sDate, @"(NGÀY )[\d]+( THÁNG )[\d]+(, )[\d]+").Replace(",", "")
                        .Trim();
                    if (tmpDate != "")
                    {
                        var ls = Utilities.RegexFilters(tmpDate, @"[\d]+");

                        var arr = tmpDate.Split(' ');
                        if (ls.Count > 2) return ls[2] + "-" + ls[1] + "-" + ls[0];
                    }
                }

                #endregion gap kieu NGÀY 10 THÁNG 11, 2015 | 15:03 => http: //suckhoedoisong.vn/kip-thoi-cuu-nam-thanh-nien-bi-thanh-sat-2m-dam-xuyen-that-lung-qua-hau-mon-n109044.html

                // 22 Th6 2017
                isMatch = Regex.IsMatch(sDate.ToLower(), @"[\d]+ Th[\d]+ [\d]{4}", RegexOptions.IgnoreCase);
                if (isMatch)
                {
                    var tmpDate = Utilities.RegexFilter(sDate, @"[\d]+ Th[\d]+ [\d]{4}").Trim();
                    if (tmpDate != "")
                    {
                        var ls = Utilities.RegexFilters(tmpDate, @"[\d]+");

                        var arr = tmpDate.Split(' ');
                        if (ls.Count > 2) return ls[2] + "-" + ls[1] + "-" + ls[0];
                    }
                }

                #endregion không truyền vào Rule date, cố gắng tìm thêm xem có kết quả không?

                #region dò kiểu 8 Th5, 2017

                isMatch = Regex.IsMatch(sDate.ToLower(), @"[\d]+ Th[\d]+, [\d]{4,}", RegexOptions.IgnoreCase);
                if (isMatch)
                {
                    var tmpDate = Utilities.RegexFilter(sDate, @"[\d]+ Th[\d]+, [\d]{4,}").Trim();
                    if (tmpDate != "")
                    {
                        var ls = Utilities.RegexFilters(tmpDate, @"[\d]+");

                        var arr = tmpDate.Split(' ');
                        if (ls.Count > 2) return ls[2] + "-" + ls[1] + "-" + ls[0];
                    }
                }

                #endregion dò kiểu 8 Th5, 2017

                #region gặp kiểu 00:00 - 15/33

                isMatch = Regex.IsMatch(sDate, @"([\d]+:[\d]+ - [\d]{1,2}\/[\d]{1,2})$", RegexOptions.IgnoreCase);
                if (isMatch)
                {
                    var tmpDate = Utilities.RegexFilter(sDate, @"([\d]+:[\d]+ - [\d]{1,2}\/[\d]{1,2})$");
                    if (tmpDate != "")
                    {
                        var arr = sDate.Split('-');
                        if (arr.Length > 0)
                        {
                            var arrSub = sDate.Split('/');
                            if (arrSub.Length > 0) return DateTime.Now.Year + "-" + arrSub[1] + "-" + arrSub[0];
                        }
                    }
                }

                #endregion gặp kiểu 00:00 - 15/33

                #region gặp kiểu 06:27 PM, 12 11 2015

                isMatch = Regex.IsMatch(sDate, @"([\d]+):([\d]+) (AM|PM), ([\d]+) ([\d]+) ([\d]{4})",
                    RegexOptions.IgnoreCase);
                if (isMatch)
                {
                    var tmpDate = Utilities.RegexFilter(sDate, @"([\d]+):([\d]+) (AM|PM), ([\d]+) ([\d]+) ([\d]{4})");
                    if (tmpDate != "")
                    {
                        var arr = sDate.Split(' ');
                        if (arr.Length > 0) return arr[4] + "-" + arr[3] + "-" + arr[2];
                    }
                }

                #endregion gặp kiểu 06:27 PM, 12 11 2015

                #region gặp kiểu 06:27 PM 12 11 2015

                isMatch = Regex.IsMatch(sDate, @"([\d]+):([\d]+) (AM|PM) ([\d]+) ([\d]+) ([\d]{4})",
                    RegexOptions.IgnoreCase);
                if (isMatch)
                {
                    var tmpDate = Utilities.RegexFilter(sDate, @"([\d]+):([\d]+) (AM|PM) ([\d]+) ([\d]+) ([\d]{4})");
                    if (tmpDate != "")
                    {
                        var arr = sDate.Split(' ');
                        if (arr.Length > 0) return arr[4] + "-" + arr[3] + "-" + arr[2];
                    }
                }

                #endregion gặp kiểu 06:27 PM 12 11 2015

                #region gặp kiểu 15 Th7 2016 [\d]+[\s]+Th[\d]+[\s]+[\d]{4}

                isMatch = Regex.IsMatch(sDate, @"[\d]+[\s]+Th[\d]+[\s]+[\d]{4}", RegexOptions.IgnoreCase);
                if (isMatch)
                {
                    var tmpDate = Utilities.RegexFilter(sDate, @"[\d]+[\s]+Th[\d]+[\s]+[\d]{4} ");
                    if (tmpDate != "")
                    {
                        var arr = sDate.Split(' ');
                        if (arr.Length > 0) return arr[2] + "-" + arr[1].Replace("Th", "") + "-" + arr[0];
                    }
                }

                #endregion gặp kiểu 15 Th7 2016 [\d]+[\s]+Th[\d]+[\s]+[\d]{4}

                #region gặp kiểu: 30 Tháng Mười Hai, 2015

                isMatch = Regex.IsMatch(sDate, @"[\d]+ tháng ([\w\W]+), [\d]{4,}", RegexOptions.IgnoreCase);
                if (isMatch)
                {
                    var tmpDate = Utilities.RegexFilter(sDate, @"[\d]+ tháng ([\w\W]+), [\d]{4,}").ToLower();
                    if (tmpDate != "")
                    {
                        var year = Utilities.RegexFilter(tmpDate, @"[\d]{4,}$");
                        var day = Utilities.RegexFilter(tmpDate, @"^[\d]+");
                        var month = GetMonthFromText(Regex.Replace(tmpDate, @"[\d]+|,", "", RegexOptions.IgnoreCase)
                            .Trim());

                        return year + "-" + month + "-" + day;
                    }
                }

                #endregion gặp kiểu: 30 Tháng Mười Hai, 2015

                #region T3, 07 / 2016

                isMatch = Regex.IsMatch(sDate, @"T[\d]+, [\d]+ \/ [\d]+", RegexOptions.IgnoreCase);
                if (isMatch)
                {
                    var tmpDate = Utilities.RegexFilter(sDate, @"T[\d]+, [\d]+ \/ [\d]+");
                    if (tmpDate != "")
                    {
                        var arr = sDate.Split(' ');
                        if (arr.Length > 3)
                        {
                            var day = Utilities.RegexFilter(arr[0], @"[\d]+");
                            return arr[3] + "-" + arr[1].Replace("Th", "") + "-" + day;
                        }
                    }
                }

                #endregion T3, 07 / 2016

                #region 21st, Tháng Bảy, 2016

                isMatch = Regex.IsMatch(sDate, @"[\d]+(th|st|nd|rd), Tháng [\w\W]+, [\d]{4}", RegexOptions.IgnoreCase);
                if (isMatch)
                {
                    var tmpDate = Utilities.RegexFilter(sDate, @"[\d]+(th|st|nd|rd), Tháng [\w\W]+, [\d]{4}");
                    if (tmpDate != "")
                    {
                        var arr = tmpDate.Split(',');
                        if (arr.Length > 2)
                        {
                            var day = Utilities.RegexFilter(arr[0], @"[\d]+");
                            var month = GetMonthFromText(arr[1].Trim());
                            return arr[2].Trim() + "-" + month + "-" + day;
                        }
                    }
                }

                #endregion 21st, Tháng Bảy, 2016

                #region Tháng Chín 10, 2016 4:29 chiều

                isMatch = Regex.IsMatch(sDate, @"(Tháng) [\w\W]+ [\d]+, [\d]{4}", RegexOptions.IgnoreCase);
                if (isMatch)
                {
                    var tmpDate = Utilities.RegexFilter(sDate, @"(Tháng) [\w\W]+ [\d]+, [\d]{4}");
                    if (tmpDate != "")
                    {
                        //Tháng Chín 10, 2016
                        var day = Utilities.RegexFilter(tmpDate, @"[\d]+");
                        var year = Utilities.RegexFilter(tmpDate, @"[\d]{4}");

                        tmpDate = tmpDate.Replace(",", "");
                        tmpDate = Regex.Replace(tmpDate, @"[\d]+", "", RegexOptions.IgnoreCase).Trim();
                        var month = GetMonthFromText(tmpDate);

                        return year + "-" + month + "-" + day;
                    }
                }

                #endregion Tháng Chín 10, 2016 4:29 chiều

                #region Jul 18, 2017, 11:06 PM ET

                isMatch = Regex.IsMatch(sDate.ToLower(),
                    @"(jan|january|feb|february|mar|march|apr|april|may|jun|june|jul|july|aug|august|sep|september|oct|october|nov|november|dec|december) [\d]+, [\d]{4}");
                if (isMatch)
                {
                    var tmpDate = Utilities.RegexFilter(sDate,
                        @"(jan|january|feb|february|mar|march|apr|april|may|jun|june|jul|july|aug|august|sep|september|oct|october|nov|november|dec|december) [\d]+, [\d]{4}");
                    if (tmpDate != "")
                    {
                        var arr = tmpDate.Split(' ');
                        if (arr.Length > 2)
                        {
                            var month = GetMonthFromText_English(arr[0]);
                            return arr[2] + "-" + month + "-" + arr[1].Replace(",", "");
                        }
                    }
                }

                #endregion Jul 18, 2017, 11:06 PM ET

                #region 1 Jan 2016 hay 1 january 2016

                isMatch = Regex.IsMatch(sDate.ToLower(),
                    @"[\d]+[\s]+(jan|january|feb|february|mar|march|apr|april|may|jun|june|jul|july|aug|august|sep|september|oct|october|nov|november|dec|december)[\s]+[\d]{2,}");
                if (isMatch)
                {
                    var tmpDate = Utilities.RegexFilter(sDate,
                        @"[\d]+[\s]+(jan|january|feb|february|mar|march|apr|april|may|jun|june|jul|july|aug|august|sep|september|oct|october|nov|november|dec|december)[\s]+[\d]{2,}");
                    if (tmpDate != "")
                    {
                        var arr = tmpDate.Split(' ');
                        if (arr.Length > 2)
                        {
                            var month = GetMonthFromText_English(arr[1]);
                            return arr[2] + "-" + month + "-" + arr[0];
                        }
                    }
                }

                #endregion 1 Jan 2016 hay 1 january 2016

                #region Sunday, July 16, 2017

                isMatch = Regex.IsMatch(sDate,
                    @"(July|January|February|March|April|May|June|August|September|October|November|December)[\s]+[\d]{1,}public int \s]+[\d]{4}",
                    RegexOptions.IgnoreCase);
                if (isMatch)
                {
                    var tmpDate = Utilities.RegexFilter(sDate,
                        @"(July|January|February|March|April|May|June|August|September|October|November|December)[\s]+[\d]{1,}public int \s]+[\d]{4}");
                    if (tmpDate != "") // July 16, 2017
                    {
                        var arr = tmpDate.Split(' ');
                        return arr[2] + "-" + GetMonthFromText_English(arr[0]) + "-" + arr[1].Replace(",", "");
                    }
                }

                #endregion Sunday, July 16, 2017

                #region gặp kiểu Nov 3 2015

                isMatch = Regex.IsMatch(sDate,
                    @"(jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec)[\s]+[\d]+[\s]+[\d]{4}",
                    RegexOptions.IgnoreCase);
                if (isMatch)
                {
                    var tmpDate = Utilities.RegexFilter(sDate,
                        @"(jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec)[\s]+[\d]+[\s]+[\d]{4}");
                    if (tmpDate != "")
                    {
                        var arr = tmpDate.Split(' ');
                        return arr[2] + "-" + GetMonthFromText_English(arr[0]) + "-" + arr[1];
                    }
                }

                #endregion gặp kiểu Nov 3 2015

                #region 3 August,2016 hay 3 August, 2016

                isMatch = Regex.IsMatch(sDate,
                    @"[\d]+ (january|jan|february|feb|march|mar|april|apr|may|may|june|jun|july|jul|august|aug|september|sep|october|oct|november|nov|december|dec)public int \s]{0,}[\d]{4,}",
                    RegexOptions.IgnoreCase);
                if (isMatch)
                {
                    var tmpDate = Utilities.RegexFilter(sDate,
                        @"[\d]+ (january|jan|february|feb|march|mar|april|apr|may|may|june|jun|july|jul|august|aug|september|sep|october|oct|november|nov|december|dec)public int \s]{0,}[\d]{4,}");
                    if (tmpDate != "")
                    {
                        var arr = tmpDate.Split(' ');
                        if (arr.Length > 2)
                        {
                            var day = arr[0];

                            var arrSub = arr[1].Split(',');
                            var year = arrSub[1];
                            var month = GetMonthFromText_English(arrSub[0].Replace(",", "").ToLower().Trim());

                            return year + "-" + month + "-" + day;
                        }
                    }
                }

                #endregion 3 August,2016 hay 3 August, 2016

                #region gặp kiểu May 17th, 2016 2:24 am

                isMatch = Regex.IsMatch(sDate,
                    @"(jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec) [\d]+(th|nd|st|rd), [\d]+ [\d]+:[\d]+ (am|pm)",
                    RegexOptions.IgnoreCase);
                if (isMatch)
                {
                    var tmpDate = Utilities.RegexFilter(sDate,
                        @"(jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec) [\d]+(th|nd|st|rd), [\d]+ [\d]+:[\d]+ (am|pm)");
                    if (tmpDate != "")
                    {
                        var year = Utilities.RegexFilter(tmpDate, @"[\d]{4,}");
                        var day = Utilities.RegexFilter(tmpDate, @"[\d]+");
                        var month = Utilities.RegexFilter(tmpDate, "(jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec)");
                        month = GetMonthFromText_English(month);

                        return year + "-" + month + "-" + day;
                    }
                }

                #endregion gặp kiểu May 17th, 2016 2:24 am

                #region gặp kiểu: May 5th, 7:27 am

                isMatch = Regex.IsMatch(sDate,
                    @"(jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec) [\d]+(th|nd|st|rd), [\d]+:[\d]+ (am|pm)",
                    RegexOptions.IgnoreCase);
                if (isMatch)
                {
                    var tmpDate = Utilities.RegexFilter(sDate,
                        @"(jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec) [\d]+(th|nd|st|rd), [\d]+:[\d]+ (am|pm)");
                    if (tmpDate != "")
                    {
                        var year = DateTime.Now.ToString("yyyy");
                        var day = Utilities.RegexFilter(tmpDate, @"[\d]+");
                        var month = Utilities.RegexFilter(tmpDate, "(jan|feb|mar|apr|may|jun|jul|aug|sep|oct|nov|dec)");
                        month = GetMonthFromText_English(month);

                        return year + "-" + month + "-" + day;
                    }
                }

                #endregion gặp kiểu: May 5th, 7:27 am

                #region gặp kiểu Monday, 9:29 am

                isMatch = Regex.IsMatch(sDate,
                    @"(monday|tuesday|wednesday|thursday|friday|saturday|sunday), [\d]+:[\d]+ (am|pm)",
                    RegexOptions.IgnoreCase);
                if (isMatch)
                {
                    var tmpDate = Utilities.RegexFilter(sDate,
                        @"(monday|tuesday|wednesday|thursday|friday|saturday|sunday), [\d]+:[\d]+ (am|pm)");
                    if (tmpDate != "")
                    {
                        var strDay = Utilities.RegexFilter(tmpDate,
                            "(monday|tuesday|wednesday|thursday|friday|saturday|sunday)");
                        var day = GetDayOfWeek(strDay.ToLower());

                        if (!string.IsNullOrEmpty(day)) return day;
                    }
                }

                #endregion gặp kiểu Monday, 9:29 am

                #region kiểu toàn số thì kiểm tra chuyển thử sang kiểu

                isMatch = Regex.IsMatch(sDate, @"[\d]{10,}");
                if (isMatch)
                {
                    dt = UnixTimeStampToDateTime(sDate);
                    if (!string.IsNullOrEmpty(dt)) return dt;
                }

                #endregion kiểu toàn số thì kiểm tra chuyển thử sang kiểu
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }

            return "";
        }

        /// <summary>
        ///     Chuyển đổi tháng từ tên dạng Text sang dạng số. Ví dụ: january thành 01
        /// </summary>
        /// <param name="text">january, february, march ....</param>
        /// <returns>01, 02, 03 ...</returns>
        public static string ConvertMonthTextToNumber(string text)
        {
            text = text.ToLower();

            // dùng hàm chứa đựng để đề phòng truyền vào có dư thừa ký tự
            if (text.Contains("january")) return "01";

            if (text.Contains("february")) return "02";

            if (text.Contains("march")) return "03";

            if (text.Contains("april")) return "04";

            if (text.Contains("may")) return "05";

            if (text.Contains("june")) return "06";

            if (text.Contains("july")) return "07";

            if (text.Contains("august")) return "08";

            if (text.Contains("september")) return "09";

            if (text.Contains("october")) return "10";

            if (text.Contains("november")) return "11";

            if (text.Contains("december")) return "12";

            return "";
        }

        public string Detect_ddMMyyyy(string date)
        {
            try
            {
                var arr = date.Split('/');

                if (arr.Length > 0 && arr[2].Length == 4)
                {
                    var itemFirst = Convert.ToInt32(arr[0]);

                    // nếu item đầu lớn hơn 12 => đích thị là dd/MM/yyyy
                    if (itemFirst > 12) return arr[2] + "-" + arr[1] + "-" + arr[0];

                    var itemSecond = Convert.ToInt32(arr[1]);
                    if (itemSecond > 12) // đích thị là kiểu MM/dd/yyyy
                        return arr[2] + "-" + arr[0] + "-" + arr[1];

                    // ưu tiên trả về từ dạng dd/MM/yyyy
                    return arr[2] + "-" + arr[1] + "-" + arr[0];
                }
            }
            catch
            {
            }

            return "";
        }

        public string Detect_ddMMyy(string date)
        {
            try
            {
                var arr = date.Split('/');

                if (arr.Length > 0 && arr[2].Length == 4)
                {
                    var itemFirst = Convert.ToInt32(arr[0]);

                    // nếu item đầu lớn hơn 12 => đích thị là dd/MM/yy
                    if (itemFirst > 12) return "20" + arr[2] + "-" + arr[1] + "-" + arr[0];

                    var itemSecond = Convert.ToInt32(arr[1]);
                    if (itemSecond > 12) // đích thị là kiểu MM/dd/yy
                        return "20" + arr[2] + "-" + arr[0] + "-" + arr[1];

                    // ưu tiên trả về từ dạng dd/MM/yy
                    return "20" + arr[2] + "-" + arr[1] + "-" + arr[0];
                }
            }
            catch
            {
            }

            return "";
        }

        /// <summary>
        ///     Lấy ngày theo ngày trong tuần
        /// </summary>
        /// <param name="day">là Monday, Tueday...</param>
        /// <returns></returns>
        public string GetDayOfWeek(string day)
        {
            var year = DateTime.Now.ToString("yyyy");
            var month = DateTime.Now.ToString("MM");

            var dayofWeek = DateTime.Now.DayOfWeek.ToString(); // ngày hôm nay
            if (dayofWeek.ToLower() == day) // hôm nay
            {
                day = DateTime.Now.ToString("dd");
                return year + "-" + month + "-" + day;
            }

            dayofWeek = DateTime.Now.AddDays(-1).DayOfWeek.ToString(); // ngày hôm qua
            if (dayofWeek.ToLower() == day) // hôm nay
            {
                day = DateTime.Now.AddDays(-1).ToString("dd");
                return year + "-" + month + "-" + day;
            }

            dayofWeek = DateTime.Now.AddDays(-2).DayOfWeek.ToString(); // ngày hôm kia
            if (dayofWeek.ToLower() == day) // hôm nay
            {
                day = DateTime.Now.AddDays(-2).ToString("dd");
                return year + "-" + month + "-" + day;
            }

            dayofWeek = DateTime.Now.AddDays(-3).DayOfWeek.ToString(); // ngày hôm kia
            if (dayofWeek.ToLower() == day) // hôm nay
            {
                day = DateTime.Now.AddDays(-3).ToString("dd");
                return year + "-" + month + "-" + day;
            }

            dayofWeek = DateTime.Now.AddDays(-4).DayOfWeek.ToString(); // ngày hôm kia
            if (dayofWeek.ToLower() == day) // hôm nay
            {
                day = DateTime.Now.AddDays(-4).ToString("dd");
                return year + "-" + month + "-" + day;
            }

            dayofWeek = DateTime.Now.AddDays(-5).DayOfWeek.ToString(); // ngày hôm kia
            if (dayofWeek.ToLower() == day) // hôm nay
            {
                day = DateTime.Now.AddDays(-5).ToString("dd");
                return year + "-" + month + "-" + day;
            }

            dayofWeek = DateTime.Now.AddDays(-6).DayOfWeek.ToString(); // ngày hôm kia
            if (dayofWeek.ToLower() == day) // hôm nay
            {
                day = DateTime.Now.AddDays(-6).ToString("dd");
                return year + "-" + month + "-" + day;
            }

            return "";
        }

        public string FindDateFromText(string text)
        {
            var isMatch = Regex.IsMatch(text, @"(cách đây[\s]+[\d]+[\s])|([\d]+)(.*?)(Ago)|([\d]+)(.*?)(ago)");
            if (isMatch)
            {
                text = text.ToLower();

                #region nếu tồn tại kiểu: cách đây 5 giây

                if (text.Contains("giây") || text.Contains("second"))
                {
                    int t;
                    try
                    {
                        var tt = Utilities.RegexFilter(text, @"[\d]+");
                        t = Convert.ToInt32(tt);

                        return DateTime.Now.AddSeconds(-t).ToString("yyyy-MM-dd hh:mm:ss tt");
                    }
                    catch
                    {
                    }
                }

                #endregion nếu tồn tại kiểu: cách đây 5 giây

                #region nếu tồn tại kiểu: cách đây 5 phút

                if (text.Contains("phút") || text.Contains("minute"))
                {
                    int t;
                    try
                    {
                        var tt = Utilities.RegexFilter(text, @"[\d]+");
                        t = Convert.ToInt32(tt);

                        return DateTime.Now.AddMinutes(-t).ToString("yyyy-MM-dd hh:mm:ss tt");
                    }
                    catch
                    {
                    }
                }

                #endregion nếu tồn tại kiểu: cách đây 5 phút

                #region nếu tồn tại kiểu: cách đây 5 tiếng

                if (text.Contains("tiếng") || text.Contains("hour"))
                {
                    int t;
                    try
                    {
                        var tt = Utilities.RegexFilter(text, @"[\d]+");
                        t = Convert.ToInt32(tt);

                        return DateTime.Now.AddHours(-t).ToString("yyyy-MM-dd hh:mm:ss tt");
                    }
                    catch
                    {
                    }
                }

                #endregion nếu tồn tại kiểu: cách đây 5 tiếng

                #region nếu tồn tại kiểu: cách đây 1 ngày

                if (text.Contains("ngày") || text.Contains("day"))
                {
                    int t;
                    try
                    {
                        var tt = Utilities.RegexFilter(text, @"[\d]+");
                        t = Convert.ToInt32(tt);

                        return DateTime.Now.AddDays(-t).ToString("yyyy-MM-dd hh:mm:ss tt");
                    }
                    catch
                    {
                    }
                }

                #endregion nếu tồn tại kiểu: cách đây 1 ngày

                #region nếu tồn tại kiểu: cách đây 1 tuần

                if (text.Contains("tuần") || text.Contains("week"))
                {
                    int t;
                    try
                    {
                        var tt = Utilities.RegexFilter(text, @"[\d]+");
                        t = Convert.ToInt32(tt) * 7;

                        return DateTime.Now.AddDays(-t).ToString("yyyy-MM-dd hh:mm:ss tt");
                    }
                    catch
                    {
                    }
                }

                #endregion nếu tồn tại kiểu: cách đây 1 tuần
            }

            return "";
        }

        public string GetMonthFromText(string text)
        {
            text = text.ToLower();
            var month = "";
            switch (text)
            {
                case "tháng một":
                    month = "01";
                    break;

                case "tháng hai":
                    month = "02";
                    break;

                case "tháng ba":
                    month = "03";
                    break;

                case "tháng tư":
                    month = "04";
                    break;

                case "tháng năm":
                    month = "05";
                    break;

                case "tháng sáu":
                    month = "06";
                    break;

                case "tháng bảy":
                    month = "07";
                    break;

                case "tháng tám":
                    month = "08";
                    break;

                case "tháng chín":
                    month = "09";
                    break;

                case "tháng mười":
                    month = "10";
                    break;

                case "tháng mười một":
                    month = "11";
                    break;

                case "tháng mười hai":
                    month = "12";
                    break;
            }

            return month;
        }

        /// <summary>
        ///     Chuyển đổi tháng bằng chữ sang tháng bằng số
        /// </summary>
        /// <param name="m">dạng: january, february hay march ...</param>
        /// <returns></returns>
        public string GetMonthFromText_English(string m)
        {
            m = m.ToLower();

            switch (m)
            {
                case "january":
                    return "01";

                case "jan":
                    return "01";

                case "february":
                    return "02";

                case "feb":
                    return "02";

                case "march":
                    return "03";

                case "mar":
                    return "03";

                case "april":
                    return "04";

                case "apr":
                    return "04";

                case "may":
                    return "05";

                case "june":
                    return "06";

                case "jun":
                    return "06";

                case "july":
                    return "07";

                case "jul":
                    return "07";

                case "august":
                    return "08";

                case "aug":
                    return "08";

                case "september":
                    return "09";

                case "sep":
                    return "09";

                case "october":
                    return "10";

                case "oct":
                    return "10";

                case "november":
                    return "11";

                case "nov":
                    return "11";

                case "december":
                    return "12";

                case "dec":
                    return "12";
            }

            return "";
        }

        public string UnixTimeStampToDateTime(string sUnixTimeStamp)
        {
            try
            {
                var unixTimeStamp = Convert.ToDouble(sUnixTimeStamp);

                // Unix timestamp is seconds past epoch
                var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();

                return dtDateTime.ToString("yyyy-MM-dd"); // đổi chỗ này ra kiểu: Your time zone
            }
            catch
            {
            }

            return "";
        }

        public string UnixTimeStampToHour(string sUnixTimeStamp)
        {
            try
            {
                var unixTimeStamp = Convert.ToDouble(sUnixTimeStamp);

                // Unix timestamp is seconds past epoch
                var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();

                return dtDateTime.ToString("HH:mm:ss"); // đổi chỗ này ra kiểu: Your time zone
            }
            catch
            {
            }

            return "";
        }

        /// <summary>
        ///     Lấy ngày tháng của 1 đoạn text theo rule truyền vào, trả ra full ngày tháng năm và giờ
        /// </summary>
        /// <param name="text"></param>
        /// <param name="ruleDateTime"></param>
        /// <returns></returns>
        public DateTime GetDateFromString(string text, string ruleDateTime)
        {
            var dateReturn = DateTime.Now.AddYears(-30); // mặc định cũ hơn 30 năm
            try
            {
                var day = GetDate(text, ruleDateTime);

                var time = GetHour(text);
                if (time == "") // vẫn không tìm được time
                    time = "00:00:00"; // mặc định
                else // để ý có am hay pm thì cần đổi lại giờ cho đúng
                    try
                    {
                        if (!string.IsNullOrEmpty(ruleDateTime) && ruleDateTime.Contains("tt"))
                            if (text.ToLower().Contains(" pm")) // 01:43 PM => 13h
                            {
                                var arrHour = time.Split(':');
                                if (arrHour.Length == 2)
                                {
                                    var hour = Convert.ToInt32(arrHour[0]);

                                    if (hour < 13) hour = hour + 12;

                                    time = hour + ":" + arrHour[1];
                                }
                            }
                    }
                    catch
                    {
                    }

                if (!string.IsNullOrEmpty(day))
                {
                    day = day + " " + time;
                    dateReturn = Convert.ToDateTime(day.Trim());
                }
            }
            catch
            {
            }

            return dateReturn;
        }
    }
}