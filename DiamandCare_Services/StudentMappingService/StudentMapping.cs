using Dapper;
using DiamandCare.Core;
using StudentMappingService.Models;
using StudentMappingService.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StudentMappingService
{
    class StudentMapping
    {
        private bool _stopProcess;
        private readonly ManualResetEvent _mreMutex;

        private static StudentMapping _studentMapping = null;
        private Task _task = null;

        private readonly TimeSpan DELAY_TIME;
        private const string APP_NAME = "StudentMappingService.StudentMapping";
        private string _dcDb = Settings.Default.DiamandCareConnection;

        private StudentMapping()
        {
            _mreMutex = new ManualResetEvent(true);
            _stopProcess = false;

            DELAY_TIME = (Debugger.IsAttached) ? TimeSpan.FromSeconds(5) : TimeSpan.FromMinutes(1);
            //_context = new DbContext("EmailsEntities");
        }

        public static void Start()
        {
            if (_studentMapping == null)
            {
                _studentMapping = new StudentMapping();
                _studentMapping._task = _studentMapping.processStudentMappingsAsync();
            }
        }

        public static void Stop()
        {
            if (_studentMapping != null)
            {
                if (_studentMapping._task.Status == TaskStatus.Running && !_studentMapping._task.Wait(TimeSpan.FromSeconds(100)))
                    ErrorLog.Write("Failed to stop student mapping process");
                _studentMapping = null;
            }
        }

        async private Task processStudentMappingsAsync(TimeSpan delay = default(TimeSpan))
        {
            if (delay == default(TimeSpan))
                delay = TimeSpan.FromMilliseconds(3);

            await Task.Delay(delay);

            _mreMutex.Reset();
            try
            {
                Process();
            }
            catch (Exception ex)
            {
                ErrorLog.WriteToDatabase(ex.Message,
                               ErrorType.Critical, "StudentMappingService.StudentMapping.processStudentMappingsAsync(TimeSpan delay = default(TimeSpan))",
                               ex.StackTrace,
                               string.Empty);
            }
            finally
            {
                _mreMutex.Set();
            }

            // check for new data once in an hour
            if (!Service.IsStopping)
                _task = processStudentMappingsAsync(TimeSpan.FromHours(6));
        }

        private void Process()
        {
            try
            {
                StudentMappingModel stMap = new StudentMappingModel();

                using (SqlConnection con = new SqlConnection(_dcDb))
                {
                    var item = con.QuerySingleOrDefault<StudentMappingModel>("[dbo].[Select_StudentMappingToProcess]", commandType: CommandType.StoredProcedure);
                    stMap = item as StudentMappingModel;
                }

                while (stMap != null)
                {
                    var parameters = new DynamicParameters();
                    using (SqlConnection con = new SqlConnection(_dcDb))
                    {
                        parameters.Add("@UserID", stMap.UserID, DbType.String);
                        parameters.Add("@GroupsID", stMap.GroupID, DbType.String);
                        var item = con.QuerySingleOrDefault<StudentMappingModel>("[dbo].[Select_StudentMappingToApprove]", parameters, commandType: CommandType.StoredProcedure);
                        stMap = null;
                        stMap = item as StudentMappingModel;
                    }
                }
            }
            catch (Exception ex)
            {
                //ErrorLog.Write(ex);
                ErrorLog.WriteToDatabase(ex.Message,
                               ErrorType.Critical, "StudentMappingService.StudentMapping.Process()",
                               ex.StackTrace,
                               string.Empty);
            }
            finally
            {
                //_mreMutex.Set();
            }
        }

    }
}
