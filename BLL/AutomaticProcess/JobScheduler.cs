using BLL.NotificationBusinessLogics;
using DAL.DataBaseHelpers;
using DAL.Logger;
using DAL.Repository.AccountRepositories;
using DAL.Repository.ApplicationProcessRepositories;
using DAL.Repository.EnrollmentRepositories;
using DAL.Repository.NotificationRepositories;
using DAL.Repository.TrainingRepositories;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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
                      .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(120)
                        .RepeatForever())
                    .Build();
            /*
            .WithIntervalInHours(24)
            .OnEveryDay()
            .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 0)))
            .Build();*/


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
                    _container.RegisterType<ITrainingRepository, TrainingRepository>();
                    _container.RegisterType<IApplicationProcessRepository, ApplicationProcessRepository>();
                    _container.RegisterType<INotificationBusinessLogic, NotificationBusinessLogic>();
                    _container.RegisterType<INotificationRepository, NotificationRepository>();
                    _container.RegisterType(typeof(IDataBaseHelper<>), typeof(DataBaseHelper<>));
                    _container.RegisterType<ILogger, Logger>();
                }
                return _container;
            }
        }
    }
}
