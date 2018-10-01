using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamandCare.Core
{
    public class SSBQuery
    {
        private readonly string _sqlCxnString;
        private readonly string _ssbQuery;
        private readonly Action OnInsert;
        private readonly Action OnUpdate;
        private readonly Action OnDelete;

        public SSBQuery(string sqlCxnString, string query, Action OnInsert = null, Action OnUpdate = null, Action OnDelete = null)
        {
            _sqlCxnString = sqlCxnString;
            _ssbQuery = query;

            this.OnInsert = OnInsert;
            this.OnUpdate = OnUpdate;
            this.OnDelete = OnDelete;

            OnChangeHandler(null, null);
        }

        /// <summary>
        /// Handler for the SqlDependency OnChange event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChangeHandler(object sender, SqlNotificationEventArgs e)
        {
            if (e != null && e.Type == SqlNotificationType.Subscribe &&
                e.Info == SqlNotificationInfo.Invalid && e.Source == SqlNotificationSource.Statement)
            {
                ErrorLog.Write(ErrorLog.Format("Invalid SSB query: {0}", _ssbQuery));
                return;
            }

            try
            {
                // Notices are only a one shot deal
                // so remove the existing one so a new 
                // one can be added
                SqlDependency dependency = sender as SqlDependency;
                if (dependency != null)
                    dependency.OnChange -= OnChangeHandler;

                // Create command
                // Command must use two part names for tables
                // SELECT <field> FROM dbo.Table rather than 
                // SELECT <field> FROM Table
                // Query also can not use *, fields must be designated
                using (SqlConnection cxn = new SqlConnection(_sqlCxnString))
                {
                    SqlCommand cmd = cxn.CreateCommand();
                    cmd.CommandText = _ssbQuery;
                    cmd.CommandType = CommandType.Text;

                    // Clear any existing notifications
                    cmd.Notification = null;

                    // Create the dependency for this command
                    dependency = new SqlDependency(cmd);

                    // Add the event handler
                    dependency.OnChange += OnChangeHandler;

                    // at least read one record in order to hook up with SSB
                    cxn.Open();
                    try
                    {
                        cmd.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.Write(ex, cmd);
                    }
                    cxn.Close();
                }

                // Notify the client about changed records
                if (e != null &&
                    (
                        e.Source == SqlNotificationSource.Data &&
                        e.Type == SqlNotificationType.Change &&
                        (e.Info == SqlNotificationInfo.Merge ||
                        e.Info == SqlNotificationInfo.Insert || e.Info == SqlNotificationInfo.Update ||
                        e.Info == SqlNotificationInfo.Delete || e.Info == SqlNotificationInfo.Truncate
                        )
                     )
                   )
                {
                    Task.Run(() =>
                    {
                        try
                        {
                            if (e.Info == SqlNotificationInfo.Insert && OnInsert != null)
                                OnInsert();
                            else if ((e.Info == SqlNotificationInfo.Update || e.Info == SqlNotificationInfo.Merge) && OnUpdate != null)
                                OnUpdate();
                            else if ((e.Info == SqlNotificationInfo.Delete || e.Info == SqlNotificationInfo.Truncate) && OnDelete != null)
                                OnDelete();
                        }
                        catch (Exception ex)
                        {
                            ErrorLog.Write(ex);
                        }
                    });
                }
            }
            catch (Exception ex) { ErrorLog.Write(ex); }
        }
    }

}
