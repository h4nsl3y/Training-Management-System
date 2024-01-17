using BLL.AccountBusinessLogics;
using BLL.AutomaticProcess;
using BLL.EnrollmentBusinesslogics;
using BLL.GenericBusinessLogics;
using BLL.NotificationBusinessLogics;
using BLL.PrerequisiteBusinesslogics;
using BLL.RequiredFileBusinessLogics;
using BLL.TrainingBusinessLogics;
using BLL.ViewModelsBusinessLogics;
using DAL.DataBaseHelpers;
using DAL.Logger;
using DAL.Repository.AccountRepositories;
using DAL.Repository.ApplicationProcessRepositories;
using DAL.Repository.EnrollmentRepositories;
using DAL.Repository.GenericRepositories;
using DAL.Repository.NotificationRepositories;
using DAL.Repository.PrerequisiteRepositories;
using DAL.Repository.RequiredFileRepositories;
using DAL.Repository.TrainingRepositories;
using DAL.Repository.ViewModelRepositories;
using Quartz;
using System;
using Unity;


namespace TrainingManagementSystem
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity GetContainer.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();
            
            //Database Helper
            container.RegisterType(typeof(IDataBaseHelper<>), typeof(DataBaseHelper<>));
            container.RegisterType<ILogger, Logger>();

            //Generic
            container.RegisterType(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            container.RegisterType(typeof(IGenericBusinessLogic<>), typeof(GenericBusinessLogic<>));

            //Account
            container.RegisterType<IAccountRepository, AccountRepository>();
            container.RegisterType<IAccountBusinessLogic, AccountBusinessLogic>();

            //Enrollment
            container.RegisterType<IEnrollmentRepository, EnrollmentRepository>();
            container.RegisterType<IEnrollmentBusinessLogic, EnrollmentBusinesslogic>();

            //Training
            container.RegisterType<ITrainingRepository, TrainingRepository>();
            container.RegisterType<ITrainingBusinessLogic, TrainingBusinesslogic>();

            //Required file
            container.RegisterType<IRequiredFilesRepository, RequiredFilesRepository>();
            container.RegisterType<IRequiredFileBusinessLogic, RequiredFileBusinessLogic>();

            //Prerequisite
            container.RegisterType<IPrerequisiteRepository,PrerequisiteRepository>();
            container.RegisterType<IPrerequisiteBusinessLogic, PrequisiteBusinessLogic>();

            //Notification
            container.RegisterType<INotificationRepository, NotificationRepository>();
            container.RegisterType<INotificationBusinessLogic, NotificationBusinessLogic>();

            //ViewModels
            container.RegisterType(typeof(IViewModelRepository<>), typeof(ViewModelRepository<>));
            container.RegisterType(typeof(IViewModelBusinessLogic<>), typeof(ViewModelBusinessLogic<>));

            //Application process
            container.RegisterType<IApplicationProcessRepository, ApplicationProcessRepository>();

            //Background Job
            container.RegisterType<IJob, BackgroundJob>();
        }
    }
}