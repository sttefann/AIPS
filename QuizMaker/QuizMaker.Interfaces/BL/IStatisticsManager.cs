using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizMaker.DAL.DB;
using QuizMaker.Models.QuestionModels;
using QuizMaker.Models.StatisticsModel;

namespace QuizMaker.Interfaces.BL
{
    public interface IStatisticsManager
    {
        StatisticalListModel GetStatistic(long quizId, long userId);
        SurveyStatisticModel GetStatistic(long quizId, long userId, long questionId);
        List<Statistic> GetStatistics(long quizId, long userId);
        List<StatisticalListModel> GetStatisticsForQuiz(long quizId);
        List<SurveyStatisticModel> GetSurveyStatisticsForQuiz(long surveyId);
        List<QuestionPlayedModel> GetTestResults(long user_id, long quiz_id);
        List<PollStats> GetPollResults(long quiz_id, long user_id);
        bool AddStatistic(StatisticalListModel statistics);
        bool AddStatistic(SurveyStatisticModel statistics);
        bool AddStatistic(Statistic statistics);
        int AddStatistics(List<StatisticsModel> answers);
        bool UpdateStatistic(StatisticalListModel statistics);
        bool UpdateStatistic(SurveyStatisticModel statistics);
        bool DeleteStatistic(long id);
        bool DeleteSurveyStatistic(long id);
        bool IsPlayed(long quiz_id, long user_id);
        //Stats Standings(long id);
        Stats<TestRankList> TestStats(long id);
        Stats<SurveyStats> SurveyStats(long id);
        Stats<PollStats> PollStats(long id);

    }
}
