using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Interfaces.Repository
{
    public interface IUow : IDisposable
    {
        ICategoryRepository Categories { get; set; }
        IPossibleAnswerRepository PossibleAnswers { get; set; }
        IQuestionRepository Questions { get; set; }
        IQuizRepository Quizzes { get; set; }
        IStatisticalListRepository StatisticalLists { get; set; }
        ISurveyStatisticRepository SurveyStatistics { get; set; }
        ITeamMemberRepository TeamMembers { get; set; }
        ITeamRepository Teams { get; set; }
        int Complete();
    }
}
