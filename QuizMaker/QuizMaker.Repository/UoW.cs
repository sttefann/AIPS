using QuizMaker.DAL.DB;
using QuizMaker.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Repository
{
    public class UoW : IUow
    {
        private readonly QuizMakerEntities _entity;
        public UoW(QuizMakerEntities entity)
        {
            _entity = entity as QuizMakerEntities;

            Categories = new CategoryRepository(_entity);

            PossibleAnswers = new PossibleAnswerRepository(entity);

            Questions = new QuestionRepository(entity);

            StatisticalLists = new StatisticalListRepository(entity);

            SurveyStatistics = new SurveyStatisticRepository(entity);

            TeamMembers = new TeamMemberRepository(entity);

            Teams = new TeamRepository(entity);

        }

        public ICategoryRepository Categories { get; set; }
        public IPossibleAnswerRepository PossibleAnswers { get; set; }
        public IQuestionRepository Questions { get; set; }
        public IQuizRepository Quizzes { get; set; }
        public IStatisticalListRepository StatisticalLists { get; set; }
        public ISurveyStatisticRepository SurveyStatistics { get; set; }
        public ITeamMemberRepository TeamMembers { get; set; }
        public ITeamRepository Teams { get; set; }

        public int Complete()
        {
            return _entity.SaveChanges();
        }
        public void Dispose()
        {
            _entity.Dispose();
        }
    }
}
