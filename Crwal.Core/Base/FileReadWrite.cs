using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Crwal.Core.Base
{
    public class FileReadWrite
    {
        public string _pathFile;

        public FileReadWrite(string pathFile)
        {
            _pathFile = pathFile;
        }

        /// <summary>
        ///     đọc từng dòng của 1 file, trả ra 1 array list
        /// </summary>
        /// <param name="pathFile"></param>
        /// <returns></returns>
        public List<string> GetListLine()
        {
            try
            {
                if (File.Exists(_pathFile))
                {
                    var lis = new List<string>();
                    using (Stream stream = File.OpenRead(_pathFile))
                    {
                        using (var r = new StreamReader(stream))
                        {
                            while (r.EndOfStream == false) lis.Add(r.ReadLine());
                        }
                    }

                    return lis;
                }
            }
            catch
            {
            }

            return null;
        }

        /// <summary>
        ///     đọc từng dòng của 1 file, trả ra 1 Dictionary - kiểu này không trùng
        /// </summary>
        /// <param name="pathFile"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetDictionaryLine()
        {
            try
            {
                if (File.Exists(_pathFile))
                {
                    var dic = new Dictionary<string, string>();

                    using (Stream stream = File.OpenRead(_pathFile))
                    {
                        using (var r = new StreamReader(stream))
                        {
                            while (r.EndOfStream == false)
                            {
                                var line = r.ReadLine().Trim();

                                if (!dic.ContainsKey(line)) dic.Add(line, "");
                            }
                        }
                    }

                    return dic;
                }
            }
            catch
            {
            }

            return null;
        }

        /// <summary>
        ///     Đọc toàn bộ text thông qua hàm ReadAllText
        /// </summary>
        /// <param name="pathFile"></param>
        /// <returns></returns>
        public string GetText()
        {
            try
            {
                if (File.Exists(_pathFile)) return File.ReadAllText(_pathFile).Trim();
            }
            catch
            {
            }

            return "";
        }

        /// <summary>
        ///     Ghi từng dòng text xuống file txt
        /// </summary>
        /// <param name="pathFile"></param>
        /// <param name="textline"></param>
        /// <returns></returns>
        public bool WriteLineToFile_Append(string textline)
        {
            try
            {
                var folder = Path.GetDirectoryName(_pathFile);
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                using (var sw = new StreamWriter(_pathFile, true, Encoding.UTF8)) // ghi kieu appent
                {
                    sw.WriteLine(textline);
                }

                return true;
            }
            catch (Exception ex)
            {
            }

            return false;
        }

        /// <summary>
        ///     Ghi xuống file - nhưng trước khi ghi cần phải xóa file cũ
        /// </summary>
        /// <param name="pathFile"></param>
        /// <param name="textline"></param>
        /// <returns></returns>
        public bool WriteLineToFile_HasDelete(string textline)
        {
            try
            {
                var folder = Path.GetDirectoryName(_pathFile);
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                if (File.Exists(_pathFile)) File.Delete(_pathFile);

                using (var sw = new StreamWriter(_pathFile, true, Encoding.UTF8)) // ghi kieu appent
                {
                    sw.WriteLine(textline);
                }

                return true;
            }
            catch (Exception ex)
            {
            }

            return false;
        }

        /// <summary>
        ///     Xóa file vĩnh viễn
        /// </summary>
        /// <param name="pathFile"></param>
        /// <returns></returns>
        public bool Delete()
        {
            try
            {
                if (File.Exists(_pathFile)) File.Delete(_pathFile);
                return true;
            }
            catch
            {
            }

            return false;
        }
    }
}