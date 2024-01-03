using Microsoft.AspNet.SignalR.Hubs;
using STP.ServiceAccess.Applications;
using STP.ServiceAccess.CommunicationsInterface;
using STP.ServiceAccess.DocumentsAndContents;
using STP.ServiceAccess.HelpdeskTools;
using STP.ServiceAccess.LoggingAndReporting;
using STP.ServiceAccess.MovementsAndNotifications;
using STP.ServiceAccess.RoadNetwork;
using STP.ServiceAccess.RouteAssessment;
using STP.ServiceAccess.RoutePlannerInterface;
using STP.ServiceAccess.Routes;
using STP.ServiceAccess.SecurityAndUsers;
using STP.ServiceAccess.SignalR;
using STP.ServiceAccess.Structures;
using STP.ServiceAccess.StructureUpdates;
using STP.ServiceAccess.VehiclesAndFleets;
using STP.ServiceAccess.Workflows;
using STP.ServiceAccess.Workflows.ApplicationsNotifications;
using STP.ServiceAccess.Workflows.SORTSOProcessing;
using STP.ServiceAccess.Workflows.SORTVR1Processing;
using System;
using System.Configuration;
using System.Net.Http;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace STP.ServiceAccess
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
            container.RegisterType<HttpClient>(
                    new InjectionFactory(x =>
                    new HttpClient { BaseAddress = new Uri(ConfigurationManager.AppSettings["APIGatewayUrl"]) }
                    )
            );

            container.RegisterType<IMovementsService, MovementsService>();
            container.RegisterType<IVehicleConfigService, VehicleConfigService>();
            container.RegisterType<IVehicleComponentService, VehicleComponentService>();
            container.RegisterType<IStructuresService, StructuresService>();
            container.RegisterType<IStructureDeligationService, StructureDeligationService>();

            container.RegisterType<IAuthenticationService, AuthenticationService>();
            container.RegisterType<ILoggingService, LoggingService>();
            container.RegisterType<IAuditLogService, AuditLogService>();
            container.RegisterType<IHolidaysService, HolidaysService>();
            container.RegisterType<IDistributionStatusService, DistributionStatusService>();
            container.RegisterType<IRoadOwnershipService, RoadOwnershipService>();
            container.RegisterType<IRoadDelegationService, RoadDelegationService>();
            container.RegisterType<IApplicationService, ApplicationService>();
            container.RegisterType<IDocumentService, DocumentService>();
            container.RegisterType<IHolidaysService, HolidaysService>();
            container.RegisterType<INENNotificationService, NENNotificationService>();
            container.RegisterType<INotificationService, NotificationService>();
            container.RegisterType<IRoutePlannerInterfaceService, RoutePlannerInterfaceService>();
            container.RegisterType<IRouteAssessmentService, RouteAssessmentService>();
            container.RegisterType<IRoutesService, RoutesService>();
            container.RegisterType<IManageFolderService, ManageFolderService>();
            container.RegisterType<ISORTDocumentService, SORTDocumentService>();
            container.RegisterType<INotificationDocService, NotificationDocService>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IDispensationService, DispensationService>();
            container.RegisterType<ISORTApplicationService, SORTApplicationService>();
            container.RegisterType<IGenericService, GenericService>();
            container.RegisterType<IFeedbackService, FeedbackService>();
            container.RegisterType<IContactService, ContactService>();
            container.RegisterType<IConstraintService, ConstraintService>();
            container.RegisterType<IInformationService, InformationService>();
            container.RegisterType<IReportService, ReportService>();
            container.RegisterType<IQasService, QasService>();
            container.RegisterType<IContentsService, ContentsService>();
            container.RegisterType<IFleetManagementWorkflowService, FleetManagementWorkflowService>();
            container.RegisterType<IApplicationNotificationWorkflowService, ApplicationNotificationWorkflowService>();
            container.RegisterType<ISORTSOProcessingService, SORTSOProcessingService>();
            container.RegisterType<IOrganizationService, OrganizationServices>();
            container.RegisterType<IStructureUpdateService, StructureUpdateService>();
            container.RegisterType<ICommunicationsInterfaceService, CommunicationsInterfaceService>();
            container.RegisterType<ISORTVR1ProcessingService, SORTVR1ProcessingService>();
            container.RegisterType<ISOAPoliceWorkflowService, SOAPoliceWorkflowService>();

            container.RegisterType<NewsHub, NewsHub>(new ContainerControlledLifetimeManager());
            container.RegisterType<IHubActivator, UnityHubActivator>(new ContainerControlledLifetimeManager());

        }
    }
}