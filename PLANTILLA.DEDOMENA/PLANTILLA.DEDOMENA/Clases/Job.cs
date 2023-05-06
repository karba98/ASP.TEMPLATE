namespace PLANTIILLA.DEDOMENA.Clases
{
	using Microsoft.Extensions.Logging;
	using Quartz;
	using System;
    using System.Threading.Tasks;
    public class Job : IJob
	{
        public readonly ILogger Logger;
        public readonly EmpleoUpdater Updater;
        public Job(ILogger<Job> Logger, EmpleoUpdater Updater)
        {
            this.Updater = Updater;
            this.Logger = Logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            int nuevas = await Updater.Update();
            Logger.LogInformation("Ofertas Nuevas: " + nuevas);
            await Updater.SendAndPublish();
            Updater.CleanOfertas();
        }
    }
}