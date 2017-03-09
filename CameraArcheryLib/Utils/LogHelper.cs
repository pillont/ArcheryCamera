using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CameraArcheryLib.Utils
{
    /// <summary>
    /// Controller to set log
    /// </summary>
    public static class LogHelper
    {
        /// <summary>
        /// path of the log file
        /// </summary>
        public const string PathLogFile = "log.txt";

        public const string ErrorHeader = "/!\\/!\\/!\\  ERROR   /!\\/!\\";

        /// <summary>
        /// write a log in the log file
        /// </summary>
        /// <param name="message">message to write</param>
        public static void Write(string message)
        {
            using (StreamWriter w = File.AppendText(PathLogFile))
            {
                w.Write(DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : ");
                w.WriteLine(message);
            }
        }

        /// <summary>
        /// Log an error
        /// </summary>
        /// <param name="message"></param>
        /// <param name="stackTrace"></param>
        public static void Error(Exception e)
        {
            using (StreamWriter w = File.AppendText(PathLogFile))
            {
                w.WriteLine(ErrorHeader);
                w.WriteLine(e.Message);
                w.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// read the 50 last lines in the log file
        /// </summary>
        /// <returns>Ienumerable of the 50 last lines</returns>
        public static IEnumerable<String> ReadLast()
        {
            try
            {
                var lines = File.ReadLines(PathLogFile);
                lines.Reverse().Take(50);

                return lines;
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }
    }
}
