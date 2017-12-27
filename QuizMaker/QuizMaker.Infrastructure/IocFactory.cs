using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using QuizMaker.BL.Managers;
using QuizMaker.DAL.DB;
using QuizMaker.Interfaces.BL;
using QuizMaker.Interfaces.Repository;
using QuizMaker.Repository;



namespace QuizMaker.Infrastructure
{
    public static class IocFactory
    {
        public static void RegisterServices(IServiceCollection services)
        {
      //      services.AddScoped<IPersonManager, PersonManager>();
        //    services.AddScoped<ISessionManager, SessionManager>();
           // services.AddScoped<ICompanyManager, CompanyManager>();
            services.AddScoped<ICategoryManager, CategoryManager>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IPossibleAnswerRepository, PossibleAnswerRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IQuizRepository, QuizRepository>();
            services.AddScoped<IStatisticalListRepository, StatisticalListRepository>();
            services.AddScoped<ISurveyStatisticRepository, SurveyStatisticRepository>();
            services.AddScoped<ITeamMemberRepository, TeamMemberRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();

            services.AddScoped<IUow, UoW>();
            services.AddScoped<IQuizMakerEntities, QuizMakerEntities>();
        }
    }
}
