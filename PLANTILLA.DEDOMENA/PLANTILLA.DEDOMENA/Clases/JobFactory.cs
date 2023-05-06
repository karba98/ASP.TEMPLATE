namespace PLANTIILLA.DEDOMENA.Clases
{
    using Microsoft.Extensions.DependencyInjection;
    using Quartz;
    using Quartz.Spi;
    using System;

    public class JobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public JobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var scope = _serviceProvider.CreateScope();

                var obj = scope.ServiceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
                return obj;
        }

        public void ReturnJob(IJob job) { }
    }
}
