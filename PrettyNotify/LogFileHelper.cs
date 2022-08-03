/*-----------------------------------------------------------------
 * 作  者（Author）：             Dennis
 * 日  期（Create Date）：        2021/9/3 16:11:39
 * 公  司（Copyright）：          www.dennisdong.top
 * 文件名（File Name）：          LogFileHelper
 * ----------------------------------------------------------------
 * 描  述（Description）：		  
 *----------------------------------------------------------------*/

using System;
using System.IO;

namespace PrettyNotify
{
    /// <summary>
    /// 自定义文件写入
    /// </summary>
    public class LogFileHelper
    {
        private static readonly object LogLock = new object();

        /// <summary>
        /// 日志默认存放路径
        /// </summary>
        private static readonly string LogPath = $@"{AppDomain.CurrentDomain.BaseDirectory}\LogFiles\";

        #region WriteLog

        /// <summary>
        /// 只记录信息
        /// </summary>
        /// <param name="msg">内容</param>
        public static void WriteLog(string msg)
        {
            WriteLog(null, null, msg);
        }

        /// <summary>
        /// 路径加信息
        /// </summary>
        /// <param name="filePath">相对路径</param>
        /// <param name="msg">内容</param>
        public static void WriteLog(string filePath, string msg)
        {
            WriteLog(filePath, null, msg);
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="filePath">相对路径</param>
        /// <param name="fileName">文件名</param>
        /// <param name="msg">内容</param>
        public static void WriteLog(string filePath, string fileName, string msg)
        {
            lock (LogLock)
            {
                try
                {
                    if (string.IsNullOrEmpty(filePath))
                    {
                        filePath = "LogDefault";
                    }

                    filePath = LogPath + filePath;
                    if (!Directory.Exists(filePath))//判断是否有该文件  
                        Directory.CreateDirectory(filePath);

                    if (string.IsNullOrEmpty(fileName))
                    {
                        fileName = DateTime.Now.ToString("yyyyMMdd");
                    }
                    var logFileName = filePath + "/" + fileName + ".log";//生成日志文件  

                    var fs = !File.Exists(logFileName) ? new FileStream(logFileName, FileMode.Create) : new FileStream(logFileName, FileMode.Append);

                    var sw = new StreamWriter(fs);
                    sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} \n{msg}");
                    sw.Flush();
                    sw.Close();
                }
                catch (Exception e)
                {
                    var path = LogPath + "/LogError";

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    var logFileName = path + "/" + DateTime.Now.ToString("yyyyMMdd") + ".log";

                    var fs = !File.Exists(logFileName) ? new FileStream(logFileName, FileMode.Create) : new FileStream(logFileName, FileMode.Append);

                    var sw = new StreamWriter(fs);
                    sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} \n{msg} \n {e.Message}");
                    sw.Flush();
                    sw.Close();
                }
            }
        }

        #endregion
    }
}
