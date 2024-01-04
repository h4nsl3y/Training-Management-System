using DAL.Repository.EnrollmentRepositories;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace BLL.AutomaticProcess
{
    public class JobScheduler
    {
        private static IUnityContainer _container;
        public async static void Start()
        {
            ISchedulerFactory schedularFactory = new StdSchedulerFactory();
            IScheduler scheduler = await schedularFactory.GetScheduler();
            scheduler.JobFactory = new MyJobFactory(GetContainer);
            
            IJobDetail job = JobBuilder.Create<BackgroundJob>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger", "sqlGroup")
                 .StartNow()
                .WithDailyTimeIntervalSchedule(x => x
                .WithIntervalInHours(24)
                .OnEveryDay()
                .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 0)))
                .Build();


            await scheduler.ScheduleJob(job, trigger);
            await scheduler.Start();
        }
        public static IUnityContainer GetContainer
        {
            get
           {
                if (_container == null)
                {
                    _container = new UnityContainer();
                    _container.RegisterType<IEnrollmentRepository, EnrollmentRepository>();
                }
                return _container;
            }
        }
    }
}
