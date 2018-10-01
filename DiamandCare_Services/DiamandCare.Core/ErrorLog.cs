using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DiamandCare.Core
{
    public enum ErrorType
    {
        Info = 1,       // Information 
        Warning,    // Warning, needs attention
        Severe,     // Severe, need to fix it immediately
        Critical    // Critical, system can't run any further
    }

    public static class ErrorLog
    {
        private static string APP_NAME = null;
        private static string LOG_FOLDER = null;
        private static string EVENT_LOG_CXN = null;

        static ErrorLog()
        {
            try
            {
                //APP_NAME = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
                //LOG_FOLDER = @"C:\Logs\" + System.Reflection.Assembly.GetEntryAssembly().GetName().Name;

                APP_NAME = "DiamandCare.WebAPI";
                LOG_FOLDER = @"C:\Logs\DiamandCare.WebAPI";

                if (!Directory.Exists(LOG_FOLDER))
                    Directory.CreateDirectory(LOG_FOLDER);

                EVENT_LOG_CXN = ConfigurationManager.ConnectionStrings["EventLog"].ConnectionString;
            }
            catch (Exception ex)
            {
                WriteToLogFile(ex.Message + "\r\n" + ex.StackTrace, ErrorType.Critical, "ErrorLog.ErrorLog()");
            }
        }

        public static void SetAppName(string appName)
        {
            APP_NAME = appName;
        }

        public static string Format(string format, params object[] args)
        {
            // format the message
            string msg = format;
            if (args.Length > 0)
            {
                try { msg = string.Format(format, args); }
                catch { }
            }

            return msg;
        }

        public static string GetStackTrace(StackTrace st)
        {
            StringBuilder sb = new StringBuilder();
            foreach (StackFrame sf in st.GetFrames())
            {
                if (sf.GetFileName() == null)
                    break;
                sb.AppendLine(GetLocation(sf.GetFileName(), sf.GetMethod().Name, sf.GetFileLineNumber()));
            }
            return sb.ToString();
        }

        public static string GetStackTrace(string stackTrace)
        {
            int ndx = stackTrace.IndexOf("at System.AppDomain");
            if (ndx > -1)
                return stackTrace.Substring(0, ndx);
            else
                return stackTrace;
        }

        /// <summary>
        /// Get location of the error
        /// </summary>
        /// <param name="file"></param>
        /// <param name="member"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public static string GetLocation(string file, string member, int line)
        {
            string solutionName = "DiamandCare V2";
            string filePath = Path.GetFileName(file);
            try
            {
                int ndx = file.LastIndexOf(solutionName, StringComparison.OrdinalIgnoreCase);
                if (ndx > -1)
                {
                    ndx += solutionName.Length + 1;
                    filePath = file.Substring(ndx);
                }
            }
            catch { }   // ignore all errors

            return string.Format("{0}: {1}({2})", filePath, member, line);
        }

        public static void Write(string message, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
        {
            WriteToDatabase(message, ErrorType.Info, GetLocation(file, member, line));
        }

        public static void Write(string message, Exception ex, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
        {
            WriteToDatabase(ex.Message + " {" + message + "}", ErrorType.Severe, GetLocation(file, member, line), GetStackTrace(ex.StackTrace));
        }

        public static void Write(Exception ex, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
        {
            WriteToDatabase(ex.Message, ErrorType.Severe, GetLocation(file, member, line), GetStackTrace(ex.StackTrace));
        }

        public static void Write(string message, Exception ex, SqlCommand cmd, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
        {
            WriteToDatabase(ex.Message + " {" + message + "}", ErrorType.Critical, GetLocation(file, member, line), GetStackTrace(ex.StackTrace), cmd.ToSQL());
        }

        public static void Write(Exception ex, SqlCommand cmd, [CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
        {
            WriteToDatabase(ex.Message, ErrorType.Critical, GetLocation(file, member, line), GetStackTrace(ex.StackTrace), cmd.ToSQL());
        }

        /// <summary>
        /// Write to database
        /// </summary>
        public static void WriteToDatabase(string message, ErrorType errType, string location, string stackTrace = null, string sqlScript = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(EVENT_LOG_CXN))
                {
                    using (SqlConnection cxn = new SqlConnection(EVENT_LOG_CXN))
                    {
                        SqlCommand cmd = cxn.CreateCommand();
                        cmd.CommandText = @"
                            INSERT INTO ErrorLog(Application, Location, ErrorTypeID, ErrorMessage, StackTrace, SQLScript)
                            VALUES(@Application, @Location, @ErrorTypeID, @ErrorMessage, @StackTrace, @SQLScript)
                        ";
                        cmd.Parameters.AddWithValue("@Application", APP_NAME);
                        cmd.Parameters.AddWithValue("@Location", location);
                        cmd.Parameters.AddWithValue("@ErrorTypeID", (int)errType);
                        cmd.Parameters.AddWithValue("@ErrorMessage", message);
                        cmd.Parameters.AddWithValue("@StackTrace", stackTrace ?? "");
                        cmd.Parameters.AddWithValue("@SQLScript", sqlScript ?? "");

                        cxn.Open();
                        cmd.ExecuteNonQuery();
                        cxn.Close();
                    }
                }
                else
                {
                    WriteToLogFile(message, errType, location, stackTrace, sqlScript);
                }
            }
            catch (Exception ex)
            {
                WriteToLogFile(message, errType, location, stackTrace, sqlScript);
                WriteToLogFile(ex.Message + "\r\n" + ex.StackTrace, ErrorType.Warning, "DiamandCaretecCore.ErrorLog.WriteToDatabase()");
            }
        }

        /// <summary>
        /// A thread safe function to write to text logs
        /// </summary>
        /// <param name="source">Application source</param>
        /// <param name="message">Log message</param>
        public static void WriteToLogFile(string message, ErrorType errType, string location, string stackTrace = null, string sqlScript = null)
        {
            try
            {
                // Create file path
                string filePath = LOG_FOLDER + @"\" + DateTime.Today.DayOfWeek + ".log";

                // delete the file if it is older than 5 days
                if (File.Exists(filePath))
                {
                    if ((DateTime.Now - File.GetCreationTime(filePath)).TotalDays > 5)
                    {
                        File.Delete(filePath);
                    }
                }

                // Log message
                string msg = DateTime.Now.ToLongTimeString().ToString() + " => " + message + "\r\n" + "ErrorType:" + errType + "\r\n" +
                    "Location: " + location + ((stackTrace != null) ? "\r\n" + stackTrace : "") + "\r\n\r\n" + sqlScript;
                File.AppendAllText(filePath, msg);
            }
            catch { }// Ignorare all errors
        }

        /// <summary>
        /// Write the error message to text log
        /// </summary>
        /// <param name="message"></param>
        public static void WriteToLogFile(string message)
        {
            try
            {
                // Create file path
                string filePath = LOG_FOLDER + @"\" + DateTime.Today.DayOfWeek + ".log";

                // delete the file if it is older than 5 days
                if (File.Exists(filePath))
                {
                    DateTime lastModifiedTime = File.GetLastWriteTime(filePath);
                    if ((DateTime.Now - lastModifiedTime).TotalDays > 5)
                    {
                        File.Delete(filePath);
                    }
                }

                // Log message
                string msg = string.Format("{0} : {1}\r\n", DateTime.Now.ToLongTimeString().ToString(), message);
                File.AppendAllText(filePath, msg);
            }
            catch { }// Ignorare all errors
        }

    }

}
