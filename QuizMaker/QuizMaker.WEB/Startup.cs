using Microsoft.Owin;
using Owin;
using Microsoft.Extensions.DependencyInjection;
using QuizMaker.WEB.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using QuizMaker.Interfaces.Repository;
using QuizMaker.DAL.DB;
using QuizMaker.Repository;
using QuizMaker.BL.Managers;
using QuizMaker.Interfaces.BL;
using System.Linq;
namespace QuizMaker.WEB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // We will use Dependency Injection for all controllers and other classes, so we'll need a service collection
            var services = new ServiceCollection();

            // configure all of the services required for DI
            ConfigureServices(services);

            // Configure authentication
            ConfigureAuth(app);
            app.MapSignalR();
            // Create a new resolver from our own default implementation
            var resolver = new DefaultDependencyResolver(services.BuildServiceProvider());

            // Set the application resolver to our default resolver. This comes from "System.Web.Mvc"
            //Other services may be added elsewhere through time
            DependencyResolver.SetResolver(resolver);
        }
        public void ConfigureServices(IServiceCollection services)
        {
            
            //====================================================
            // Create the DB context for the IDENTITY database
            //====================================================
            // Add a database context - this can be instantiated with no parameters
            services.AddTransient(typeof(ApplicationDbContext));

            //====================================================
            // ApplicationUserManager
            //====================================================
            // instantiation requires the following instance of the Identity database
            services.AddTransient(typeof(IUserStore<ApplicationUser,long>), p => new ApplicationUserStore(new ApplicationDbContext()));
            // with the above defined, we can add the user manager class as a type
            services.AddTransient(typeof(ApplicationUserManager));

            //====================================================
            // ApplicationSignInManager
            //====================================================
            // instantiation requires two parameters, [ApplicationUserManager] (defined above) and [IAuthenticationManager]
            services.AddTransient(typeof(Microsoft.Owin.Security.IAuthenticationManager), p => new OwinContext().Authentication);
            services.AddTransient(typeof(ApplicationSignInManager));
            
            //====================================================
            // ApplicationRoleManager
            //====================================================
            // Maps the rolemanager of identity role to the concrete role manager type
            //services.AddTransient<RoleManager<IdentityRole>, ApplicationRoleManager>();

            // Maps the role store role to the implemented type
            //services.AddTransient<IRoleStore<IdentityRole, string>, RoleStore<IdentityRole>>();
            //services.AddTransient(typeof(ApplicationRoleManager));
            
            
            services.AddScoped<ICategoryManager, CategoryManager>();
            services.AddScoped<IQuizManager, QuizManager>();
            services.AddScoped<IQuestionManager, QuestionManager>();
            services.AddScoped<ITeamManager, TeamManager>();
            services.AddScoped<IStatisticsManager, StatisticsManager>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IPossibleAnswerRepository, PossibleAnswerRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IQuizRepository, QuizRepository>();
            services.AddScoped<IStatisticsRepository, StatisticsRepository>();
            services.AddScoped<ITeamMemberRepository, TeamMemberRepository>();

            services.AddScoped<IUow, UoW>();
            services.AddScoped<IQuizMakerEntities, QuizMakerEntities>();

            services.AddControllersAsServices(typeof(Startup).Assembly.GetExportedTypes()
   .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
   .Where(t => typeof(IController).IsAssignableFrom(t)
      || t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)));
        }
        
        /// <summary>
        /// Provides the default dependency resolver for the application - based on IDependencyResolver, which hhas just two methods
        /// </summary>
        public class DefaultDependencyResolver : IDependencyResolver
        {
            /// <summary>
            /// Provides the service that holds the services
            /// </summary>
            protected IServiceProvider serviceProvider;

            /// <summary>
            /// Create the service resolver using the service provided (Direct Injection pattern)
            /// </summary>
            /// <param name="serviceProvider"></param>
            public DefaultDependencyResolver(IServiceProvider serviceProvider)
            {
                this.serviceProvider = serviceProvider;
            }

            /// <summary>
            /// Get a service by type - assume you get the first one encountered
            /// </summary>
            /// <param name="serviceType"></param>
            /// <returns></returns>
            public object GetService(Type serviceType)
            {
                return this.serviceProvider.GetService(serviceType);
            }

            /// <summary>
            /// Get all services of a type
            /// </summary>
            /// <param name="serviceType"></param>
            /// <returns></returns>
            public IEnumerable<object> GetServices(Type serviceType)
            {
                return this.serviceProvider.GetServices(serviceType);
            }
        }
    }
    
}
