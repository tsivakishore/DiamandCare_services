using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamandCare.Core
{
    public static class SQLHelper
    {
        public static readonly DateTime DEFAULT_DATE = new DateTime(1900, 1, 1);

        /// <summary>
        /// Gets a boolean value from the result set
        /// </summary>
        public static bool GetBoolean(this SqlDataReader rs, string columnName)
        {
            bool bRet = false;

            try
            {
                if (rs != null && rs[columnName] != DBNull.Value)
                    bRet = (bool)rs[columnName];
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Coulumn '{0}' in table '{1}' doesn't exist!\r\n{2}", columnName, rs.GetSchemaTable().TableName, ex.Message));
            }

            return bRet;
        }

        /// <summary>
        /// Get DateTime value
        /// </summary>
        public static bool GetBoolean(this DataRow row, string columnName)
        {
            bool bRet = false;

            try
            {
                if (row != null && row[columnName] != DBNull.Value)
                    bRet = (bool)row[columnName];
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Coulumn '{0}' in table '{1}' doesn't exist!\r\n{2}", columnName, row.Table.TableName, ex.Message));
            }

            return bRet;
        }

        /// <summary>
        /// Gets a DateTime value from the result set
        /// </summary>
        public static DateTime GetDateTime(this SqlDataReader rs, string columnName)
        {
            DateTime dtRet = DateTime.MinValue;

            try
            {
                if (rs != null && rs[columnName] != DBNull.Value)
                    dtRet = (DateTime)rs[columnName];
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Coulumn '{0}' in table '{1}' doesn't exist!\r\n{2}", columnName, rs.GetSchemaTable().TableName, ex.Message));
            }

            return dtRet;
        }

        /// <summary>
        /// Gets a DateTime value from the DataRow
        /// </summary>
        public static DateTime GetDateTime(this DataRow row, string columnName)
        {
            DateTime dtRet = DateTime.MinValue;

            try
            {
                if (row != null && row[columnName] != DBNull.Value)
                    dtRet = (DateTime)row[columnName];
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Coulumn '{0}' in table '{1}' doesn't exist!\r\n{2}", columnName, row.Table.TableName, ex.Message));
            }

            return dtRet;
        }

        /// <summary>
        /// Get a timespan value from SqlDataReader
        /// </summary>
        public static TimeSpan GetTimeSpan(this SqlDataReader rs, string columnName)
        {
            TimeSpan tsRet = TimeSpan.Zero;

            try
            {
                if (rs != null && rs[columnName] != DBNull.Value)
                    tsRet = (TimeSpan)rs[columnName];
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Coulumn '{0}' in table '{1}' doesn't exist!\r\n{2}", columnName, rs.GetSchemaTable().TableName, ex.Message));
            }

            return tsRet;
        }

        /// <summary>
        /// Gets a TimeSpan value from the DataRow
        /// </summary>
        public static TimeSpan GetTimeSpan(this DataRow row, string columnName)
        {
            TimeSpan tsRet = TimeSpan.Zero;

            try
            {
                if (row != null && row[columnName] != DBNull.Value)
                    tsRet = (TimeSpan)row[columnName];
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Coulumn '{0}' in table '{1}' doesn't exist!\r\n{2}", columnName, row.Table.TableName, ex.Message));
            }

            return tsRet;
        }

        /// <summary>
        /// Gets a Int32 value from the result set
        /// </summary>
        public static int GetInt(this SqlDataReader rs, string columnName)
        {
            int nRet = 0;

            try
            {
                if (rs != null && rs[columnName] != DBNull.Value)
                    nRet = (int)rs[columnName];
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Coulumn '{0}' in table '{1}' doesn't exist!\r\n{2}", columnName, rs.GetSchemaTable().TableName, ex.Message));
            }

            return nRet;
        }

        /// <summary>
        /// Get a Int32 value from a DataRow
        /// </summary>
        public static int GetInt(this DataRow row, string columnName)
        {
            int nRet = 0;

            try
            {
                if (row != null && row[columnName] != DBNull.Value)
                    nRet = (int)row[columnName];
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Coulumn '{0}' in table '{1}' doesn't exist!\r\n{2}", columnName, row.Table.TableName, ex.Message));
            }

            return nRet;
        }

        /// <summary>
        /// Gets a Long (Int64) value from the result set
        /// </summary>
        public static long GetLong(this SqlDataReader rs, string columnName)
        {
            long lRet = 0;

            try
            {
                if (rs != null && rs[columnName] != DBNull.Value)
                    lRet = (long)rs[columnName];
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Coulumn '{0}' in table '{1}' doesn't exist!\r\n{2}", columnName, rs.GetSchemaTable().TableName, ex.Message));
            }

            return lRet;
        }

        /// <summary>
        /// Get a long(Int32) value from a DataRow
        /// </summary>
        public static long GetLong(this DataRow row, string columnName)
        {
            long nRet = 0;

            try
            {
                if (row != null && row[columnName] != DBNull.Value)
                    nRet = (long)row[columnName];
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Coulumn '{0}' in table '{1}' doesn't exist!\r\n{2}", columnName, row.Table.TableName, ex.Message));
            }

            return nRet;
        }

        /// <summary>
        /// Gets a double value from the result set
        /// </summary>
        public static double GetDouble(this SqlDataReader rs, string columnName)
        {
            double dRet = 0;

            try
            {
                if (rs != null && rs[columnName] != DBNull.Value)
                    dRet = (double)rs[columnName];
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Coulumn '{0}' in table '{1}' doesn't exist!\r\n{2}", columnName, rs.GetSchemaTable().TableName, ex.Message));
            }

            return dRet;
        }

        /// <summary>
        /// Get a double value from a data row
        /// </summary>
        public static double GetDouble(this DataRow row, string columnName)
        {
            double nRet = 0;

            try
            {
                if (row != null && row[columnName] != DBNull.Value)
                    nRet = (double)row[columnName];
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Coulumn '{0}' in table '{1}' doesn't exist!\r\n{2}", columnName, row.Table.TableName, ex.Message));
            }

            return nRet;
        }

        /// <summary>
        /// Gets a Decimal(Money) value from the result set
        /// </summary>
        public static decimal GetDecimal(this SqlDataReader rs, string columnName)
        {
            decimal dRet = 0M;

            try
            {
                if (rs != null && rs[columnName] != DBNull.Value)
                    dRet = (decimal)rs[columnName];
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Coulumn '{0}' in table '{1}' doesn't exist!\r\n{2}", columnName, rs.GetSchemaTable().TableName, ex.Message));
            }

            return dRet;
        }

        /// <summary>
        /// Get a Decimal(Money) value from a Data Row
        /// </summary>
        public static decimal GetDecimal(this DataRow row, string columnName)
        {
            decimal dRet = 0;

            try
            {
                if (row != null && row[columnName] != DBNull.Value)
                    dRet = (decimal)row[columnName];
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Coulumn '{0}' in table '{1}' doesn't exist!\r\n{2}", columnName, row.Table.TableName, ex.Message));
            }

            return dRet;
        }

        /// <summary>
        /// Gets a string value from the result set
        /// </summary>
        public static string GetString(this SqlDataReader rs, string columnName)
        {
            string sRet = null;

            try
            {
                if (rs != null && rs[columnName] != DBNull.Value)
                    sRet = (string)rs[columnName];
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Coulumn '{0}' in table '{1}' doesn't exist!\r\n{2}", columnName, rs.GetSchemaTable().TableName, ex.Message));
            }

            return sRet;
        }

        /// <summary>
        /// Gets a string from a DataRow
        /// </summary>
        public static string GetString(this DataRow row, string columnName)
        {
            string sRet = null;

            try
            {
                if (row != null && row[columnName] != DBNull.Value)
                    sRet = (string)row[columnName];
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Coulumn '{0}' in table '{1}' doesn't exist!\r\n{2}", columnName, row.Table.TableName, ex.Message));
            }

            return sRet;
        }

        public static SqlBinary GetSqlBinary(this DataRow row, string columnName)
        {
            SqlBinary retVal = new SqlBinary(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });

            try
            {
                if (row != null && row[columnName] != DBNull.Value)
                {
                    byte[] buff = row[columnName] as byte[];
                    retVal = new SqlBinary(buff);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Coulumn '{0}' in table '{1}' doesn't exist!\r\n{2}", columnName, row.Table.TableName, ex.Message));
            }

            return retVal;
        }

        public static SqlBinary GetSqlBinary(this SqlDataReader rs, string columnName)
        {
            SqlBinary retVal = new SqlBinary(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });

            try
            {
                if (rs != null && rs[columnName] != DBNull.Value)
                {
                    byte[] buff = rs[columnName] as byte[];
                    retVal = new SqlBinary(buff);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Coulumn '{0}' in table '{1}' doesn't exist!\r\n{2}", columnName, rs.GetSchemaTable().TableName, ex.Message));
            }

            return retVal;
        }

        /// <summary>
        /// Convert byte array (eg. rowversion) to hexadecimal value
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static string ToHex(this SqlBinary data)
        {
            StringBuilder sb = new StringBuilder();

            if (data.Value != null && data.Value.Length > 0)
            {
                sb.Append("0x");
                foreach (byte bt in data.Value)
                {
                    sb.AppendFormat("{0:x2}", bt);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Convert to TSQL
        /// </summary>
        /// <remarks>Not efficient function due to string concatnation</remarks>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static string ToSQL(this SqlCommand cmd)
        {
            if (cmd == null)
                return "";

            StringBuilder sb = new StringBuilder();
            sb.Append(cmd.CommandText);
            sb.Append(" ");

            foreach (SqlParameter prm in cmd.Parameters)
            {
                if (cmd.CommandType == CommandType.StoredProcedure)
                {
                    if (prm.SqlDbType.ToString().IndexOf("char", StringComparison.OrdinalIgnoreCase) > -1)
                        sb.AppendFormat("{0}='{1}'", prm.ParameterName, prm.Value);
                    else if (prm.SqlDbType == SqlDbType.Bit)
                        sb.AppendFormat("{0}={1}", prm.ParameterName, (bool)prm.Value ? 1 : 0);
                    else if (prm.SqlDbType == SqlDbType.DateTime)
                        sb.AppendFormat("{0}='{1}'", prm.ParameterName, ((DateTime)prm.Value).ToString("MM/dd/yyyy"));
                    else if (prm.SqlDbType == SqlDbType.VarBinary)
                    {
                        string ver = ToHex((SqlBinary)prm.Value);
                        sb.Replace(prm.ParameterName, string.Format("{0}", ver));
                    }
                    else
                        sb.AppendFormat("{0}={1}", prm.ParameterName, prm.Value);

                    if (prm != cmd.Parameters[cmd.Parameters.Count - 1])
                        sb.Append(", ");
                }
                else
                {
                    if (prm.SqlDbType.ToString().IndexOf("char", StringComparison.OrdinalIgnoreCase) > -1)
                        sb.Replace(prm.ParameterName, string.Format("'{0}'", prm.Value));
                    else if (prm.SqlDbType == SqlDbType.Bit)
                        sb.Replace(prm.ParameterName, string.Format("{0}", (bool)prm.Value ? 1 : 0));
                    else if (prm.SqlDbType == SqlDbType.DateTime)
                        sb.Replace(prm.ParameterName, string.Format("'{0}'", ((DateTime)prm.Value).ToString("MM/dd/yyyy")));
                    else if (prm.SqlDbType == SqlDbType.VarBinary)
                    {
                        string ver = ToHex((SqlBinary)prm.Value);
                        sb.Replace(prm.ParameterName, string.Format("{0}", ver));
                    }
                    else
                        sb.Replace(prm.ParameterName, string.Format("{0}", prm.Value));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Convert the current version to ulong
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static ulong ToULong(byte[] version)
        {
            // If the system architecture is little-endian (that is, little end first),
            // reverse the byte array.
            if (BitConverter.IsLittleEndian)
            {
                byte[] buff = new byte[8];
                for (int ndx = 0; ndx < 8; ndx++)
                {
                    buff[ndx] = version[7 - ndx];
                }
                return BitConverter.ToUInt64(buff, 0);
            }

            return BitConverter.ToUInt64(version, 0);
        }
    }

}
