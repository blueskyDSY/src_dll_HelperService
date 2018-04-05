using System;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace HelperService
{
    /// <summary>
    /// Sqlite数据库密码帮助类
    /// </summary>
    public static class SqlitePasswordHelper
    {
        /// <summary>
        /// 更改数据库密码
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="password">密码</param>
        public static void ChangePassword(string connectionString, string password)
        {
            SQLiteConnection connection = null;
            try
            {
                connection = new SQLiteConnection(connectionString);
                connection.Open();
                connection.ChangePassword(password);
            }
            catch (Exception ex)
            {
                throw new Exception("Change sqlite database newPassword failed.", ex);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 给一个目录下所有数据库设置统一密码
        /// </summary>
        /// <param name="folderPath">目录路径</param>
        /// <param name="newPassword">新密码</param>
        /// <param name="oldPassword">旧密码</param>
        /// <param name="databaseFileExtention">数据库文件扩展名（一般为".db3"）</param>
        public static void SetDatabasePasswordInAFolder(string folderPath, string newPassword, string oldPassword = "", string databaseFileExtention = ".db3")
        {
            if (string.IsNullOrEmpty(folderPath))
            {
                throw new ArgumentNullException("folderPath");
            }

            var connectionStringTemplate = "Data Source={0};";
            if (!string.IsNullOrEmpty(oldPassword))
            {
                connectionStringTemplate = connectionStringTemplate + string.Format("Password={0};", oldPassword);
            }

            foreach (var connectionString in Directory.EnumerateFiles(folderPath, "*" + databaseFileExtention).Select(database => string.Format(connectionStringTemplate, database)))
            {
                ChangePassword(connectionString, newPassword);
            }
        }
    }
}