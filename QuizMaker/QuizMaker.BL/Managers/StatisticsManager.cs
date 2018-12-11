using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizMaker.DAL.DB;
using QuizMaker.Interfaces.BL;
using QuizMaker.Interfaces.Repository;
using QuizMaker.Models.Enums;
using QuizMaker.Models.Item;
using QuizMaker.Models.QuestionModels;
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
        public Stats<TestRankList> TestStats(long id)
        {
            try
            {
                Quiz quiz = _uow.Quizzes.Get(id);
                if (quiz != null)
                {
                    Stats<TestRankList> stats = new Stats<TestRankList>();
                    stats.Name = quiz.Name;
                    if (quiz.Type.ToQuizType().ConvertToString() != "test") return new Stats<TestRankList>();
                    Dictionary<long, TestRankList> standings = new Dictionary<long, TestRankList>();
                    List<Statistic> statistics = _uow.Statistics.GetAll().Where(x => x.Quiz_Id == id).ToList();
                    //test
                    foreach (var stat in statistics)
                    {
                        if (stat.Points.HasValue)
                        {
                            if (standings.ContainsKey(stat.User_Id))
                            {
                                standings[stat.User_Id].Points += stat.Points.Value;
                            }
                            else
                            {
                                TestRankList item = new TestRankList();
                                item.Points = stat.Points.Value;
                                item.Username = stat.AspNetUser.Email;
                                standings.Add(stat.User_Id, item);
                            }
                        }
                        else
                        {
                            if (!standings.ContainsKey(stat.User_Id))
                            {
                                TestRankList item = new TestRankList();
                                item.Points = 0;
                                item.Username = stat.AspNetUser.Email;
                                standings.Add(stat.User_Id, item);
                            }
                        }
                    }
                    List<TestRankList> model = new List<TestRankList>();
                    foreach (KeyValuePair<long, TestRankList> element in standings)
                    {
                        model.Add(element.Value);
                    }
                    List<TestRankList> sorted = model.OrderByDescending(x => x.Points).ToList();
                    
                    stats.Standings = sorted;
                    return stats;
                }
                return new Stats<TestRankList>();
            }
            catch(Exception e)
            {
                return new Stats<TestRankList>();
            }
        }
        public Stats<SurveyStats> SurveyStats(long id)
        {
            try
            {
                Stats<SurveyStats> stats = new Stats<SurveyStats>();
                Quiz quiz = _uow.Quizzes.Get(id);
                if (quiz != null)
                {
                    stats.Name = quiz.Name;
                    if (quiz.Type.ToQuizType().ConvertToString() != "survey") return new Stats<SurveyStats>();
                    List<Statistic> statistics = _uow.Statistics.GetAll().Where(x => x.Quiz_Id == id).ToList();
                    //survey
                    Dictionary<long, SurveyStats> survey = new Dictionary<long, SurveyStats>();
                    foreach (var stat in statistics)
                    {
                        if (survey.ContainsKey(stat.Question_Id))
                        {
                            if (stat.Answer.HasValue)
                            {
                                if (survey[stat.Question_Id].Answers.Where(x => x.Text.ToLower() == stat.PossibleAnswer.text.ToLower()).Count() == 0)
                                {
                                    SurveyItem item = new SurveyItem();
                                    item.Number = 1;
                                    item.Text = stat.PossibleAnswer.text;
                                    survey[stat.Question_Id].Answers.Add(item);
                                }
                                else
                                {
                                    survey[stat.Question_Id].Answers.FirstOrDefault(x => x.Text.ToLower() == stat.PossibleAnswer.text.ToLower()).Number += 1;
                                }
                            }
                            else
                            {

                                if (survey[stat.Question_Id].Answers.Where(x => x.Text.ToLower() == stat.Answer_text.ToLower()).Count() == 0)
                                {
                                    SurveyItem item = new SurveyItem();
                                    item.Number = 1;
                                    item.Text = stat.Answer_text;
                                    survey[stat.Question_Id].Answers.Add(item);
                                }
                                else
                                {
                                    survey[stat.Question_Id].Answers.FirstOrDefault(x => x.Text.ToLower() == stat.Answer_text.ToLower()).Number += 1;
                                }
                            }
                        }
                        else
                        {
                            SurveyStats item = new SurveyStats();
                            item.Question_text = stat.Question.Question_text;
                            item.Answers = new List<SurveyItem>();
                            SurveyItem answ = new SurveyItem();
                            answ.Number = 1;
                            if (stat.Answer.HasValue)
                            {
                                answ.Text = stat.PossibleAnswer.text;
                            }
                            else
                            {
                                answ.Text = stat.Answer_text;
                            }
                            survey.Add(stat.Question_Id, item);
                        }
                    }
                    List<SurveyStats> results = new List<SurveyStats>();
                    foreach (var item in survey)
                    {
                        results.Add(item.Value);
                    }

                    stats.Standings = results;
                }
                return stats;
            }
            catch (Exception e)
            {
                return new Stats<SurveyStats>();
            }
        }
        public Stats<PollStats> PollStats(long id)
        {
            try
            {
                Stats<PollStats> stats = new Stats<PollStats>();
                Quiz quiz = _uow.Quizzes.Get(id);
                if (quiz != null)
                {
                    if (quiz.Type.ToQuizType().ConvertToString() != "poll") return new Stats<PollStats>();
                    List<PollStats> results = new List<PollStats>();
                    results = GetPollResults(id);
                    if (quiz.Questions.Count() > 0)
                        stats.Name = quiz.Questions.First().Question_text;
                    else
                        stats.Name = "Results";
                    stats.Standings = results;
                }
                return stats;
            }
            catch(Exception e)
            {
                return new Stats<PollStats>();
            }
        }
     /*   public Stats<T> Standings(long id)
        {
            try
            {
                Quiz quiz = _uow.Quizzes.Get(id);
                if(quiz != null)
                {
                    Stats stats = new Stats();
                    stats.Name = quiz.Name;
                    Dictionary<long, TestRankList> standings = new Dictionary<long, TestRankList>();
                    List<Statistic> statistics = _uow.Statistics.GetAll().Where(x => x.Quiz_Id == id).ToList();
                    if (statistics.Count() == 0)
                    {
                        stats.PollStats = new List<PollStats>();
                        stats.SurveyStats = new List<SurveyStats>();
                        stats.TestStats = new List<TestRankList>();
                        return stats;
                    }
                    if (quiz.Type.ToQuizType().ConvertToString() == "test")
                    {

                        //test
                        foreach (var stat in statistics)
                        {
                            if (stat.Points.HasValue)
                            {
                                if (standings.ContainsKey(stat.User_Id))
                                {
                                    standings[stat.User_Id].Points += stat.Points.Value;
                                }
                                else
                                {
                                    TestRankList item = new TestRankList();
                                    item.Points = stat.Points.Value;
                                    item.Username = stat.AspNetUser.Email;
                                    standings.Add(stat.User_Id, item);
                                }
                            }
                            else
                            {
                                if (!standings.ContainsKey(stat.User_Id))
                                {
                                    TestRankList item = new TestRankList();
                                    item.Points = 0;
                                    item.Username = stat.AspNetUser.Email;
                                    standings.Add(stat.User_Id, item);
                                }
                            }
                        }
                        List<TestRankList> model = new List<TestRankList>();
                        foreach (KeyValuePair<long, TestRankList> element in standings)
                        {
                            model.Add(element.Value);
                        }
                        List<TestRankList> sorted = model.OrderByDescending(x => x.Points).ToList();
                        stats.TestStats = sorted;
                    }
                    else if(quiz.Type.ToQuizType().ConvertToString() == "survey")
                    {
                        stats = new Stats();
                        //survey
                        Dictionary<long, SurveyStats> survey = new Dictionary<long, SurveyStats>();
                        foreach (var stat in statistics)
                        {
                            if (survey.ContainsKey(stat.Question_Id))
                            {
                                if (stat.Answer.HasValue)
                                {
                                    if (survey[stat.Question_Id].Answers.Where(x => x.Text.ToLower() == stat.PossibleAnswer.text.ToLower()).Count() == 0)
                                    {
                                        SurveyItem item = new SurveyItem();
                                        item.Number = 1;
                                        item.Text = stat.PossibleAnswer.text;
                                        survey[stat.Question_Id].Answers.Add(item);
                                    }
                                    else
                                    {
                                        survey[stat.Question_Id].Answers.FirstOrDefault(x => x.Text.ToLower() == stat.PossibleAnswer.text.ToLower()).Number += 1;
                                    }
                                }
                                else
                                {

                                    if (survey[stat.Question_Id].Answers.Where(x => x.Text.ToLower() == stat.Answer_text.ToLower()).Count() == 0)
                                    {
                                        SurveyItem item = new SurveyItem();
                                        item.Number = 1;
                                        item.Text = stat.Answer_text;
                                        survey[stat.Question_Id].Answers.Add(item);
                                    }
                                    else
                                    {
                                        survey[stat.Question_Id].Answers.FirstOrDefault(x => x.Text.ToLower() == stat.Answer_text.ToLower()).Number += 1;
                                    }
                                }
                            }
                            else
                            {
                                SurveyStats item = new SurveyStats();
                                item.Question_text = stat.Question.Question_text;
                                item.Answers = new List<SurveyItem>();
                                SurveyItem answ = new SurveyItem();
                                answ.Number = 1;
                                if (stat.Answer.HasValue)
                                {
                                    answ.Text = stat.PossibleAnswer.text;
                                }
                                else
                                {
                                    answ.Text = stat.Answer_text;
                                }
                                survey.Add(stat.Question_Id, item);
                            }
                        }
                        stats.SurveyStats = new List<SurveyStats>();
                        foreach (var item in survey)
                        {
                            stats.SurveyStats.Add(item.Value);
                        }
                    }
                    else if (quiz.Type.ToQuizType().ConvertToString() == "poll")
                    {
                        stats = new Stats();
                        stats.PollStats = GetPollResults(id);
                        if (quiz.Questions.Count() > 0)
                            stats.Name = quiz.Questions.First().Question_text;
                        else
                            stats.Name = "Results";
                    }
                        return stats;
                }
                return null;            
            }
            catch(Exception e)
            {
                return null;
            }
        }
        */
        public bool AddStatistic(StatisticalListModel statistics)
        {
            try
            {
                Statistic list = new Statistic();
                list.Id = statistics.Id;
                list.Points = statistics.Points;
                list.Quiz_Id = statistics.Quiz_Id;
                list.User_Id = statistics.User_Id;

                
                _uow.Statistics.Add(list);
                _uow.Complete();
                return true;
            }
            catch (Exception e)
            {
                //Log exception
                var exception = e.InnerException;
                Debug.Write(exception.Message);
                Debug.Write(e.Message);

                return false;
            }
        }

        public bool AddStatistic(SurveyStatisticModel statistics)
        {
            try
            {
                Statistic list = new Statistic();
                list.Id = statistics.Id;
                list.Answer = statistics.Answer_id;
                list.Quiz_Id = statistics.Quiz_Id;
                list.User_Id = statistics.UserID;
                list.Question_Id = statistics.Question_Id;
                

                _uow.Statistics.Add(list);
                _uow.Complete();
                return true;
            }
            catch (Exception e)
            {
                //Log exception
                var exception = e.InnerException;
                Debug.Write(exception.Message);
                Debug.Write(e.Message);
                return false;
            }
        }

        public bool AddStatistic(Statistic statistics)
        {
            try
            {
                Statistic list = new Statistic
                {
                    Id = statistics.Id,
                    Quiz_Id = statistics.Quiz_Id,
                    User_Id = statistics.User_Id,
                    Question_Id = statistics.Question_Id,
                    Answer_text = statistics.Answer_text,
                };
                if (statistics.Points.HasValue)
                    list.Points = statistics.Points;
                if (statistics.Answer.HasValue)
                    list.Answer = statistics.Answer.Value;


                _uow.Statistics.Add(list);
                _uow.Complete();
                return true;
            }
            catch (Exception e)
            {
                //Log exception
                var exception = e.InnerException;
                Debug.Write(exception.Message);
                Debug.Write(e.Message);

                return false;
            }
        }
        public int AddStatistics(List<StatisticsModel> answers)
        {
            try
            {
                List<Statistic> statistics = new List<Statistic>();
                foreach (StatisticsModel model in answers)
                {
                    Statistic stat = new Statistic();
                    if (model.Answer.HasValue)
                        stat.Answer = model.Answer.Value;
                    stat.Answer_text = model.Answer_text;
                    stat.Points = 0;
                    stat.Question_Id = model.Question_Id;
                    stat.Quiz_Id = model.Quiz_Id;
                    stat.User_Id = model.User_Id;
                    statistics.Add(stat);
                }
                int total = 0;
                List<long> questionsIds = new List<long>();
                foreach (Statistic stat in statistics)
                {
                    if (!questionsIds.Contains(stat.Question_Id))
                        questionsIds.Add(stat.Question_Id);
                }
                   
                foreach (long qId in questionsIds)
                {
                    Question question = _uow.Questions.Get(qId);
                    if(question != null)
                    {
                        int points = 1;
                        if (question.Points.HasValue)
                            points = question.Points.Value;
                        List<CorrectAnswer> corrects = _uow.CorrectAnswers.GetAll().Where(x => x.Question == qId).ToList();
                        List<Statistic> stats = statistics.Where(x => x.Question_Id == qId).ToList();
                        if (IsCorrect(corrects, stats))
                            statistics.Where(x => x.Question_Id == qId).Last().Points = points;
                        else
                        {
                            points = 0;
                            statistics.Where(x => x.Question_Id == qId).Last().Points = 0;
                        }
                        total += points;
                    }

                }
                _uow.Statistics.AddRange(statistics);
                _uow.Complete();
                return total;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public bool DeleteStatistic(long statistics)
        {
            try
            {
                Statistic list = _uow.Statistics.Get(statistics);
                _uow.Statistics.Remove(list);
                _uow.Complete();
                return true;
            }
            catch (Exception e)
            {
                //Log exception
                var exception = e.InnerException;
                Debug.Write(exception.Message);
                Debug.Write(e.Message);

                return false;
            }
        }

        public bool DeleteSurveyStatistic(long statistics)
        {
            try
            {
                Statistic list = _uow.Statistics.Get(statistics);
                _uow.Statistics.Remove(list);
                _uow.Complete();
                return true;
            }
            catch (Exception e)
            {
                //Log exception
                var exception = e.InnerException;
                Debug.Write(exception.Message);
                Debug.Write(e.Message);

                return false;
            }
        }
        public List<Statistic> GetStatistics(long quizId,long userId)
        {
            try
            {
                List<Statistic> statistics = new List<Statistic>();
                statistics = _uow.Statistics.GetAll().Where(x => x.Quiz_Id == quizId && x.User_Id == userId).ToList();
                return statistics;
            }catch(Exception e)
            {
                return null;
            }
        }

        public StatisticalListModel GetStatistic(long quizId, long userId)
        {
            try
            {
                StatisticalListModel model = new StatisticalListModel();
                Statistic list = _uow.Statistics.GetAll().Where(x => x.Quiz_Id == quizId && x.User_Id == userId).First();

                model.Id = list.Id;
                if(list.Points.HasValue)
                    model.Points = list.Points.Value;
                model.Quiz_Id = list.Quiz_Id;
                model.User_Id = list.AspNetUser.Id;
                

                return model;
            }
            catch (Exception e)
            {
                //Log exception
                var exception = e.InnerException;
                Debug.Write(exception.Message);
                Debug.Write(e.Message);

                return null;
            }
        }

        public SurveyStatisticModel GetStatistic(long quizId, long userId, long questionId)
        {
            try
            {
                SurveyStatisticModel model = new SurveyStatisticModel();
                Statistic list = _uow.Statistics.GetAll().Where(x => x.Quiz_Id == quizId && x.User_Id == userId && x.Question_Id == questionId).First();

                model.Id = list.Id;
                model.Question_Id = list.Question_Id;
                if (list.Answer.HasValue)
                {
                    model.Answer_id = list.Answer.Value;
                }
                model.Quiz_Id = list.Quiz_Id;
                model.UserID = list.AspNetUser.Id;

                return model;
            }
            catch (Exception e)
            {
                //Log exception
                var exception = e.InnerException;
                Debug.Write(exception.Message);
                Debug.Write(e.Message);

                return null;
            }
        }

        public List<StatisticalListModel> GetStatisticsForQuiz(long quizId)
        {
            try
            {
                List<StatisticalListModel> models = new List<StatisticalListModel>();
                List<Statistic> lists = _uow.Statistics.GetAll().Where(x => x.Quiz_Id == quizId).ToList(); 
                foreach(Statistic list in lists)
                {
                    StatisticalListModel model = new StatisticalListModel();
                    model.Id = list.Id;
                    if (list.Points.HasValue)
                        model.Points = list.Points.Value;
                    model.Quiz_Id = list.Quiz_Id;
                    model.User_Id = list.AspNetUser.Id;
                    models.Add(model);
                }
                

                return models;
            }
            catch (Exception e)
            {
                //Log exception
                var exception = e.InnerException;
                Debug.Write(exception.Message);
                Debug.Write(e.Message);

                return null;
            }
        }

        public List<SurveyStatisticModel> GetSurveyStatisticsForQuiz(long surveyId)
        {
            try
            {
                //treba da se doradi
                List<SurveyStatisticModel> models = new List<SurveyStatisticModel>();
                List<Statistic> lists = _uow.Statistics.GetAll().Where(x => x.Quiz_Id == surveyId).ToList();
                foreach(Statistic list in lists)
                {
                    SurveyStatisticModel model = new SurveyStatisticModel();
                    model.Id = list.Id;
                    model.Question_Id = list.Question_Id;
                    if (list.Answer.HasValue)
                    {
                        model.Answer_id = list.Answer.Value;
                    }
                    model.Quiz_Id = list.Quiz_Id;
                    model.UserID = list.AspNetUser.Id;

                    models.Add(model);
                }
                

                return models;
            }
            catch (Exception e)
            {
                //Log exception
                var exception = e.InnerException;
                Debug.Write(exception.Message);
                Debug.Write(e.Message);

                return null;
            }
        }

        public bool UpdateStatistic(StatisticalListModel statistics)
        {
            try
            {
                Statistic model = _uow.Statistics.Get(statistics.Id);

                model.Id = statistics.Id;
                model.Points = statistics.Points;
                model.Quiz_Id = statistics.Quiz_Id;
       //         model.AspNetUser = statistics.User_Id;

                _uow.Complete();
                return true;

            }
            catch (Exception e)
            {
                //Log exception
                var exception = e.InnerException;
                Debug.Write(exception.Message);
                Debug.Write(e.Message);

                return false;
            }
        }

        public bool UpdateStatistic(SurveyStatisticModel list)
        {
            try
            {
                Statistic model = _uow.Statistics.Get(list.Id);

                model.Id = list.Id;
                model.Question_Id = list.Question_Id;
                model.Answer = list.Answer_id;
                model.Quiz_Id = list.Quiz_Id;
          //      model.AspNetUser = list.User;

                _uow.Complete();
                return true;
            }
            catch (Exception e)
            {
                //Log exception
                var exception = e.InnerException;
                Debug.Write(exception.Message);
                Debug.Write(e.Message);

                return false; 
            }
        }
        private bool IsCorrect(List<CorrectAnswer> corrects, List<Statistic> answers)
        {
            bool result = true;
            if (corrects.Count() != answers.Count())
                return false;
            foreach (Statistic stat in answers)
            {
                if (stat.Answer.HasValue)
                {
                    if(!IsCorrect(corrects, stat.Answer.Value))
                    {
                        result = false;
                    }
                }
                else
                {
                    if (stat.Answer_text != corrects.First().Answer)
                        result = false;
                }
            }

            return result;
        }
        private bool IsCorrect(List<CorrectAnswer> corrects, long answer)
        {
            bool result = false;
            foreach(CorrectAnswer correct in corrects)
            {
                if (correct.PossibleAnswer.HasValue)
                {
                    if (correct.PossibleAnswer.Value == answer)
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        public bool IsPlayed(long quiz_id, long user_id)
        {
            try
            {
                List<Statistic> stats = _uow.Statistics.GetAll().Where(x => x.Quiz_Id == quiz_id && x.User_Id == user_id).ToList();
                if(stats.Count() != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }catch(Exception e)
            {
                return true;
            }
        }

        public List<QuestionPlayedModel> GetTestResults(long user_id, long quiz_id)
        {
            try
            {
                List<QuestionPlayedModel> model = new List<QuestionPlayedModel>();
                List<Statistic> statistics = _uow.Statistics.GetAll().Where(x => x.Quiz_Id == quiz_id && x.User_Id == user_id).ToList();
                List<long> questionsIds = new List<long>();
                foreach (Statistic stat in statistics)
                {
                    if (!questionsIds.Contains(stat.Question_Id))
                        questionsIds.Add(stat.Question_Id);
                }
                foreach(long id in questionsIds)
                {
                    QuestionPlayedModel item = new QuestionPlayedModel();
                    Question q = _uow.Questions.Get(id);
                    if(q != null)
                    {
                        item.Text = q.Question_text;
                        item.Points = 0;
                        if(q.Points.HasValue)
                            item.Points = q.Points.Value;
                        int sum = 0;
                        foreach(Statistic stat in statistics.Where(x => x.Question_Id == id))
                        {
                            if(stat.Points.HasValue)
                            {
                                sum += stat.Points.Value;
                            }
                        }
                        if (sum == item.Points)
                            item.Correct = true;
                        else
                            item.Correct = false;
                        model.Add(item);
                    }
                }
                return model;
            }catch(Exception e)
            {
                return null;
            }
        }

        public List<PollStats> GetPollResults(long quiz_id, long user_id)
        {
            try
            {
                List<PollStats> result = new List<PollStats>();

                List<Statistic> statistics = _uow.Statistics.GetAll().Where(x => x.Quiz_Id == quiz_id).ToList();
                long question = _uow.Questions.GetAll().Where(x => x.Quiz_Id == quiz_id).First().Id;
                List<PossibleAnswer> p_answers = _uow.PossibleAnswers.GetAll().Where(x => x.Question_Id == question).ToList();
                foreach(var ans in p_answers)
                {
                    List<Statistic> stats = statistics.Where(x => x.Answer == ans.Id).ToList();
                    PollStats vote = new PollStats();
                    vote.PossibleAnswer = ans.text;
                    vote.Votes = stats.Count();
                    List<Statistic> tmp = stats.Where(x => x.User_Id == user_id).ToList();
                    if(tmp.Count() > 0)
                    {
                        vote.MyAnswer = true;
                    }
                    else
                    {
                        vote.MyAnswer = false;
                    }
                    result.Add(vote);
                }

                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<PollStats> GetPollResults(long quiz_id)
        {
            try
            {
                List<PollStats> result = new List<PollStats>();

                List<Statistic> statistics = _uow.Statistics.GetAll().Where(x => x.Quiz_Id == quiz_id).ToList();
                List<Question> qs = _uow.Questions.GetAll().Where(x => x.Quiz_Id == quiz_id).ToList();
                long question = qs.First().Id;
                List<PossibleAnswer> p_answers = _uow.PossibleAnswers.GetAll().Where(x => x.Question_Id == question).ToList();
                    foreach (var ans in p_answers)
                    {
                        List<Statistic> stats = statistics.Where(x => x.Answer == ans.Id).ToList();
                        PollStats vote = new PollStats();
                        vote.PossibleAnswer = ans.text;
                        vote.Votes = stats.Count();
                        vote.MyAnswer = false;

                        result.Add(vote);
                    }
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
