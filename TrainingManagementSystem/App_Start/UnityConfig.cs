using BLL.AccountBusinessLogics;
using BLL.GenericBusinessLogics;
using BLL.PrerequisiteBusinesslogics;
using BLL.RequiredFileBusinessLogics;
using BLL.TrainingBusinessLogics;
using BLL.ViewModelsBusinessLogics;
using DAL.DataBaseUtils;
using DAL.Logger;
using DAL.Repository.AccountRepositories;
using DAL.Repository.GenericRepositories;
using DAL.Repository.PrerequisiteRepositories;
using DAL.Repository.RequiredFileRepositories;
using DAL.Repository.TrainingRepositories;
using DAL.Repository.ViewModelRepositories;
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
        /// Configured Unity Container.
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
            container.RegisterType(typeof(IDataBaseUtil<>), typeof(DataBaseUtil<>));
            container.RegisterType<ILogger, Logger>();

            container.RegisterType(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            container.RegisterType<IAccountRepository, AccountRepository>();
            container.RegisterType<ITrainingRepository, TrainingRepository>();
            container.RegisterType<IPrerequisiteRepository,PrerequisiteRepository>();
            container.RegisterType(typeof(IViewModelRepository<>), typeof(ViewModelRepository<>));

            container.RegisterType(typeof(IGenericBusinessLogic<>), typeof(GenericBusinessLogic<>));
            container.RegisterType<IRequiredFilesRepository,RequiredFilesRepository>();
            container.RegisterType<IAccountBusinesslogic,AccountBusinessLogic>();
            container.RegisterType<ITrainingBusinesslogic,TrainingBusinesslogic>();
            container.RegisterType<IPrerequisiteBusinessLogic,PrequisiteBusinessLogic>();
            container.RegisterType<IRequiredFileBusinessLogic,RequiredFileBusinessLogic>();
            container.RegisterType(typeof(IViewModelBusinesslogic<>), typeof(ViewModelBusinessLogic<>));
        }
    }
}