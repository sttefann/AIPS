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
        public UoW(IQuizMakerEntities entity)
        {
            _entity = entity as QuizMakerEntities;

            Categories = new CategoryRepository(_entity);

            PossibleAnswers = new PossibleAnswerRepository(_entity);
            CorrectAnswers = new CorrectAnswerRepository(_entity);

            Questions = new QuestionRepository(_entity);

            Quizzes = new QuizRepository(_entity);

            Statistics = new StatisticsRepository(_entity);

            TeamMembers = new TeamMemberRepository(_entity);

        }

        public ICategoryRepository Categories { get; set; }
        public IPossibleAnswerRepository PossibleAnswers { get; set; }
        public ICorrectAnswerRepository CorrectAnswers { get; set; }
        public IQuestionRepository Questions { get; set; }
        public IQuizRepository Quizzes { get; set; }
        public IStatisticsRepository Statistics { get; set; }
        public ITeamMemberRepository TeamMembers { get; set; }

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
