using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace publicClassLibrary.Models
{
    public class SqlSugarModel
    {
        /// <summary>连接字符串</summary>
        public string Connection { get; set; } = "";

        /// <summary>
        /// 数据库类型（SqlServer/MySql/PostgreSql/Sqlite/Oracle）
        /// </summary>
        public SqlSugar.DbType DbType { get; set; } = SqlSugar.DbType.MySql;

        /// <summary>自动关闭连接（推荐)</summary>
        public bool IsAutoCloseConnection { get; set; } = true;

        /// <summary>是否启用日志</summary>
        public bool IsEnableLog { get; set; } = true;
    }
}
