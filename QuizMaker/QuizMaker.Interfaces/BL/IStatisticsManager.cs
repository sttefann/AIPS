using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizMaker.DAL.DB;
using QuizMaker.Models.StatisticsModel;

namespace QuizMaker.Interfaces.BL
{
    public interface IStatisticsManager
    {
        StatisticalListModel GetStatistic(long quizId, long userId);
        SurveyStatisticModel GetStatistic(long quizId, long userId, long questionId);
        List<StatisticalListModel> GetStatisticsForQuiz(long quizId);
        List<SurveyStatisticModel> GetSurveyStatisticsForQuiz(long surveyId);
        bool AddStatistic(StatisticalListModel statistics);
        bool AddStatistic(SurveyStatisticModel statistics);
        bool UpdateStatistic(StatisticalListModel statistics);
        bool UpdateStatistic(SurveyStatisticModel statistics);
        bool DeleteStatistic(long id);
        bool DeleteSurveyStatistic(long id);
    }
}
