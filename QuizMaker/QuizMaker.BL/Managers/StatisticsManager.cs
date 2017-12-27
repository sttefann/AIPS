using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizMaker.DAL.DB;
using QuizMaker.Interfaces.BL;
using QuizMaker.Interfaces.Repository;
using QuizMaker.Models.StatisticsModel;

namespace QuizMaker.BL.Managers
{
    public class StatisticsManager : IStatisticsManager
    {
        private readonly IUow _uow;

        public StatisticsManager(IUow uow)
        {
            _uow = uow;
        }
        public bool AddStatistic(StatisticalListModel statistics)
        {
            try
            {
                StatisticalList list = new StatisticalList();
                list.Id = statistics.Id;
                list.Points = statistics.Points;
                list.Quiz_Id = statistics.Quiz_Id;
                list.User_Id = statistics.User.Id;
                list.Vote = statistics.Vote? (byte)1: (byte)0;

                _uow.StatisticalLists.Add(list);
                _uow.Complete();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool AddStatistic(SurveyStatisticModel statistics)
        {
            try
            {
                SurveyStatistic list = new SurveyStatistic();
                list.Id = statistics.Id;
                list.Answer_number = statistics.Answer_number;
                list.Quiz_Id = statistics.Quiz_Id;
                list.User_Id = statistics.User.Id;
                list.Question_Id = statistics.Question_Id;
                

                _uow.SurveyStatistics.Add(list);
                _uow.Complete();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool DeleteStatistic(long statistics)
        {
            try
            {
                StatisticalList list = _uow.StatisticalLists.Get(statistics);
                _uow.StatisticalLists.Remove(list);
                _uow.Complete();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool DeleteSurveyStatistic(long statistics)
        {
            try
            {
                SurveyStatistic list = _uow.SurveyStatistics.Get(statistics);
                _uow.SurveyStatistics.Remove(list);
                _uow.Complete();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public StatisticalListModel GetStatistic(long quizId, long userId)
        {
            try
            {
                StatisticalListModel model = new StatisticalListModel();
                StatisticalList list = _uow.StatisticalLists.GetAll().Where(x => x.Quiz_Id == quizId && x.User_Id == userId).First();

                model.Id = list.Id;
                if(list.Points.HasValue)
                    model.Points = list.Points.Value;
                model.Quiz_Id = list.Quiz_Id;
                model.User = list.AspNetUser;
                model.Vote = (list.Vote == 1) ? true : false;

                return model;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public SurveyStatisticModel GetStatistic(long quizId, long userId, long questionId)
        {
            try
            {
                SurveyStatisticModel model = new SurveyStatisticModel();
                SurveyStatistic list = _uow.SurveyStatistics.GetAll().Where(x => x.Quiz_Id == quizId && x.User_Id == userId && x.Question_Id == questionId).First();

                model.Id = list.Id;
                model.Question_Id = list.Question_Id;
                model.Answer_number = list.Answer_number;
                model.Quiz_Id = list.Quiz_Id;
                model.User = list.AspNetUser;

                return model;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<StatisticalListModel> GetStatisticsForQuiz(long quizId)
        {
            try
            {
                List<StatisticalListModel> models = new List<StatisticalListModel>();
                List<StatisticalList> lists = _uow.StatisticalLists.GetAll().Where(x => x.Quiz_Id == quizId).ToList(); 
                foreach(StatisticalList list in lists)
                {
                    StatisticalListModel model = new StatisticalListModel();
                    model.Id = list.Id;
                    if (list.Points.HasValue)
                        model.Points = list.Points.Value;
                    model.Quiz_Id = list.Quiz_Id;
                    model.User = list.AspNetUser;
                    model.Vote = (list.Vote == 1) ? true : false;
                    models.Add(model);
                }
                

                return models;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<SurveyStatisticModel> GetSurveyStatisticsForQuiz(long surveyId)
        {
            try
            {
                //treba da se doradi
                List<SurveyStatisticModel> models = new List<SurveyStatisticModel>();
                List<SurveyStatistic> lists = _uow.SurveyStatistics.GetAll().Where(x => x.Quiz_Id == surveyId).ToList();
                foreach(SurveyStatistic list in lists)
                {
                    SurveyStatisticModel model = new SurveyStatisticModel();
                    model.Id = list.Id;
                    model.Question_Id = list.Question_Id;
                    model.Answer_number = list.Answer_number;
                    model.Quiz_Id = list.Quiz_Id;
                    model.User = list.AspNetUser;

                    models.Add(model);
                }
                

                return models;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool UpdateStatistic(StatisticalListModel statistics)
        {
            try
            {
                StatisticalList model = _uow.StatisticalLists.Get(statistics.Id);

                model.Id = statistics.Id;
                model.Points = statistics.Points;
                model.Quiz_Id = statistics.Quiz_Id;
                model.AspNetUser = statistics.User;
                model.Vote = statistics.Vote? (byte)1 : (byte)0;

                _uow.Complete();
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool UpdateStatistic(SurveyStatisticModel list)
        {
            try
            {
                SurveyStatistic model = _uow.SurveyStatistics.Get(list.Id);

                model.Id = list.Id;
                model.Question_Id = list.Question_Id;
                model.Answer_number = list.Answer_number;
                model.Quiz_Id = list.Quiz_Id;
                model.AspNetUser = list.User;

                _uow.Complete();
                return true;
            }
            catch (Exception e)
            {
                return false; 
            }
        }
    }
}
