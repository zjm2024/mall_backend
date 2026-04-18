
using FluentScheduler;

namespace shopmallService.Jobs
{
    public class JobRegistry : Registry
    {
        public JobRegistry(IServiceProvider serviceProvider)
        {
            // 从 DI 容器获取任务实例（支持日志、其他注入服务）
            var job = ActivatorUtilities.CreateInstance<SeckillTimesJob>(serviceProvider);


            // 方案1：每隔 30 秒读取一次 Redis（推荐用于监控）
            Schedule(job).ToRunNow().AndEvery(120).Seconds();


            // 方案2：每天固定时间点执行（如每天 19:50 读取）
            // Schedule(job).ToRunEvery(1).Days().At(19, 50);


            // 方案3：指定具体秒杀时间点执行一次（如 2025-12-31 20:00:00）
            // var seckillRunTime = new DateTime(2025, 12, 31, 20, 0, 0);
            // Schedule(job).ToRunOnceAt(seckillRunTime);
        }
    }
}