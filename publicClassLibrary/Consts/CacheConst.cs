using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace publicClassLibrary.Consts
{
    /// <summary>
    /// 缓存相关常量
    /// </summary>
    public class CacheConst
    {
        /// <summary>
        /// 秒杀时间点缓存
        /// </summary>
        public const string KeySeckillTimes = "sys_datadict_seckilltimers";

        /// <summary>
        /// 秒杀时间点通配符缓存
        /// </summary>
        public const string KeySeckillTimesPattern = "*_sys_datadict_seckilltimers"; 
    }
}
