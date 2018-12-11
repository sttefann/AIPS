using AutoMapper;
using QuizMaker.DAL.DB;
using QuizMaker.Interfaces.BL;
using QuizMaker.Interfaces.Repository;
using QuizMaker.Models.Enums;
using QuizMaker.Models.Item;
using QuizMaker.Models.QuestionModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.BL.Managers
{
    public class QuestionManager : IQuestionManager
    {
        private readonly IUow _uow;

        public QuestionManager(IUow uow)
        {
            _uow = uow;
        }
        public long Add(QuestionCreateModel question)
        {
            try
            {
                
                Question result = null;
                Question q = new Question();
                q.Points = question.Points;
                q.Question_text = question.Question_text;
                q.Quiz_Id = question.Quiz_Id;
                q.Question_number = question.Question_number;
                
                q.ReadOnly = false;
                result = _uow.Questions.Add(q);
                _uow.Complete();
                if (result != null) {
                    if (question.PossibleAnswers.Count() > 1)
                    {
                        foreach (string item in question.PossibleAnswers)
                        {
                            if (item != "")
                            {
                                PossibleAnswer answer = new PossibleAnswer();
                                answer.text = item;
                                answer.Question_Id = result.Id;
                                long possible_id = AddPossibleAnswer(answer);
                                if (possible_id != -1)
                                {
                                    bool isCorrect = false;
                                    string correct = "";
                                    foreach (string correct_item in question.CorrectAnswers)
                                    {
                                        if (correct_item == item)
                                        {
                                            isCorrect = true;
                                            correct = correct_item;
                                        }

                                    }
                                    if (isCorrect)
                                    {
                                        CorrectAnswer correct_answer = new CorrectAnswer()
                                        {
                                            PossibleAnswer = possible_id,
                                            Question = result.Id
                                        };
                                        long correct_id = AddCorrectAnswer(correct_answer);
                                        if (correct_id == -1)
                                        {
                                            //Delete question
                                            _uow.Questions.Remove(result);
                                            //all correct 
                                            DeleteAllCorrectAnswersForQuestion(result.Id);
                                            //all possible answers 
                                            DeleteAllPossibleAnswersForQuestion(result.Id);
                                            return -1;
                                        }

                                    }
                                }
                                else
                                {
                                    //and delete possible answer 
                                    DeleteAllPossibleAnswersForQuestion(result.Id);
                                    //question from db
                                    _uow.Questions.Remove(result);
                                    return -1;
                                }
                            }

                        }
                    }
                    else
                    {
                        if (question.CorrectAnswers.Count() == 1)
                        {
                            CorrectAnswer correct_answer = new CorrectAnswer()
                            {
                                Answer = question.CorrectAnswers.First(),
                                Question = result.Id
                            };
                            long correct_id = AddCorrectAnswer(correct_answer);
                            if (correct_id == -1)
                            {
                                //all correct 
                                DeleteAllCorrectAnswersForQuestion(result.Id);
                                //all possible answers 
                                DeleteAllPossibleAnswersForQuestion(result.Id);
                                //Delete question
                                _uow.Questions.Remove(result);


                                return -1;
                            }
                        }
                        else
                        {

                        }
                    }
            }
                return result.Id;
            }
            catch (Exception e)
            {
                //Log exception
                //var exception = e.InnerException;
                //Debug.Write(exception.Message);
                //Debug.Write(e.Message);

                return -1;
            }
        }
        public bool ReadOnly(long id, bool readOnly)
        {
            try
            {
                Question question = _uow.Questions.Get(id);
                question.ReadOnly = readOnly;

                _uow.Complete();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public bool Delete(long id)
        {
            try
            {
                //delete statistics
                List<Statistic> stat = _uow.Statistics.GetAll().Where(x => x.Question_Id == id).ToList();
                _uow.Statistics.RemoveRange(stat);
                _uow.Complete();
                //all correct 
                DeleteAllCorrectAnswersForQuestion(id);
                //all possible answers 
                DeleteAllPossibleAnswersForQuestion(id);
                Question question = _uow.Questions.Get(id);
                _uow.Questions.Remove(question);
                _uow.Complete();
                return true;
            }
            catch (Exception e)
            {
                //Log exception
                //var exception = e.InnerException;
                //Debug.Write(exception.Message);
                //Debug.Write(e.Message);

                return false;
            }
        }
        public bool DeleteAll(long quizId)
        {
            try
            {

                //delete statistics
                List<Statistic> stat = _uow.Statistics.GetAll().Where(x => x.Quiz_Id == quizId).ToList();
                _uow.Statistics.RemoveRange(stat);
                _uow.Complete();
                List<Question> questions = _uow.Questions.GetAll().Where(x => x.Quiz_Id == quizId).ToList();
                List<long> ids = new List<long>();
                foreach(var q in questions)
                {
                    ids.Add(q.Id);
                }
                //all correct 
                DeleteAllCorrectAnswersForQuiz(ids);
                //all possible answers 
                DeleteAllPossibleAnswersForQuiz(ids);
                
                _uow.Questions.RemoveRange(questions);
                _uow.Complete();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public QuestionM Get(long id)
        {
            try
            {
                Question question = _uow.Questions.Get(id);
                QuestionM model = new QuestionM();
                model.Id = question.Id;
                model.PossibleAnswers = question.PossibleAnswers.ToList();
                if(question.Question_number.HasValue)
                    model.Question_number = question.Question_number.Value;
                model.Question_text = question.Question_text;
                model.Quiz_Id = question.Quiz_Id;
                if(question.CorrectAnswers != null)
                    model.Correct_answers = question.CorrectAnswers.ToList();
                

                return model;
            }
            catch (Exception e)
            {
                //Log exception
                //var exception = e.InnerException;
                //Debug.Write(exception.Message);
                //Debug.Write(e.Message);

                return null;
            }
        }

        public List<PossibleAnswer> GetPossibleAnswers(long questionId)
        {
            try
            {
                List<PossibleAnswer> possibleAnswers = _uow.PossibleAnswers.GetAll().Where(x => x.Question_Id == questionId).ToList();
                return possibleAnswers;
            }
            catch (Exception e)
            {
                //Log exception
                //var exception = e.InnerException;
                //Debug.Write(exception.Message);
                //Debug.Write(e.Message);

                return null;
            }
        }

        public List<QuestionViewModel> GetQuestionsForQuiz(long id)
        {
            try
            {
                List<QuestionViewModel> questions = new List<QuestionViewModel>();
                List<Question> qs = _uow.Questions.GetAll().Where(x => x.Quiz_Id == id).ToList();
                foreach(Question question in qs)
                {
                    QuestionViewModel model = new QuestionViewModel();
                    model.Id = question.Id;
                    if(question.ReadOnly.HasValue)
                    model.ReadOnly = question.ReadOnly.Value;
                    if(question.PossibleAnswers.Count() > 0)
                    {
                        foreach (var item in question.PossibleAnswers)
                        {
                            AnswerItem answer = new AnswerItem();
                            answer.Text = item.text;
                            bool isCorrect = false;
                            foreach (var correct in question.CorrectAnswers)
                            {
                                if (item.Id == correct.PossibleAnswer)
                                {
                                    isCorrect = true;
                                }
                            }
                            answer.IsCorrect = isCorrect;
                            model.Answers.Add(answer);
                        }
                    }
                    else
                    {
                        if(question.CorrectAnswers.Count() > 0)
                        {
                            AnswerItem answer = new AnswerItem();
                            answer.Text = question.CorrectAnswers.First().Answer;
                            answer.IsCorrect = true;
                            model.Answers.Add(answer);
                        }
                        else
                        {

                        }
                    }
                    if(question.Question_number.HasValue)
                        model.Question_number = question.Question_number.Value;
                    model.Question_text = question.Question_text;
                    if (question.Points.HasValue)
                        model.Points = question.Points.Value;
                   
                    questions.Add(model);
                }

                return questions;
            }
            catch (Exception e)
            {
                //Log exception
                //var exception = e.InnerException;
                //Debug.Write(exception.Message);
                //Debug.Write(e.Message);

                return null;
            }
        }
        public QuestionViewModel GetViewModel(long id)
        {
            try
            {
                Question question = _uow.Questions.Get(id);
                QuestionViewModel model = new QuestionViewModel();
                model.Id = question.Id;
                if (question.ReadOnly.HasValue)
                    model.ReadOnly = question.ReadOnly.Value;
                if (question.PossibleAnswers.Count() > 0)
                {
                    foreach (var item in question.PossibleAnswers)
                    {
                        AnswerItem answer = new AnswerItem();
                        answer.Text = item.text;
                        bool isCorrect = false;
                        foreach (var correct in question.CorrectAnswers)
                        {
                            if (item.Id == correct.PossibleAnswer)
                            {
                                isCorrect = true;
                            }
                        }
                        answer.IsCorrect = isCorrect;
                        model.Answers.Add(answer);
                    }
                }
                else
                {
                    if (question.CorrectAnswers.Count() > 0)
                    {
                        AnswerItem answer = new AnswerItem();
                        answer.Text = question.CorrectAnswers.First().Answer;
                        answer.IsCorrect = true;
                        model.Answers.Add(answer);
                    }
                    else
                    {

                    }
                }
                if (question.Question_number.HasValue)
                    model.Question_number = question.Question_number.Value;
                model.Question_text = question.Question_text;
                if (question.Points.HasValue)
                    model.Points = question.Points.Value;
                Quiz q = _uow.Quizzes.Get(question.Quiz_Id);
                model.Type = q.Type.ToQuizType().ConvertToString();
                return model;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool Update(QuestionEditModel model)
        {
            try
            {
                Question q = _uow.Questions.Get(model.Id);
                q.Points = model.Points;
                q.Question_text = model.Question_text;
                q.Question_number = model.Question_number;
                q.ReadOnly = false;
                _uow.Complete();
                    //all correct 
                    DeleteAllCorrectAnswersForQuestion(model.Id);
                    //all possible answers 
                    DeleteAllPossibleAnswersForQuestion(model.Id);
                    if (model.PossibleAnswers.Count() > 1)
                    {
                        foreach (string item in model.PossibleAnswers)
                        {
                            if (item != "")
                            {
                                PossibleAnswer answer = new PossibleAnswer();
                                answer.text = item;
                                answer.Question_Id = model.Id;
                                long possible_id = AddPossibleAnswer(answer);
                                if (possible_id != -1)
                                {
                                    bool isCorrect = false;
                                    string correct = "";
                                    foreach (string correct_item in model.CorrectAnswers)
                                    {
                                        if (correct_item == item)
                                        {
                                            isCorrect = true;
                                            correct = correct_item;
                                        }

                                    }
                                    if (isCorrect)
                                    {
                                        CorrectAnswer correct_answer = new CorrectAnswer()
                                        {
                                            PossibleAnswer = possible_id,
                                            Question = model.Id
                                        };
                                        long correct_id = AddCorrectAnswer(correct_answer);
                                        if (correct_id == -1)
                                        {
                                            //Delete question
                                            //_uow.Questions.Remove(model);
                                            //all correct 
                                            DeleteAllCorrectAnswersForQuestion(model.Id);
                                            //all possible answers 
                                            DeleteAllPossibleAnswersForQuestion(model.Id);
                                            return false;
                                        }

                                    }
                                }
                                else
                                {
                                    //and delete possible answer 
                                    DeleteAllPossibleAnswersForQuestion(model.Id);
                                    //question from db
                                    //_uow.Questions.Remove(result);
                                    return false;
                                }
                            }

                        }
                    }
                    else
                    {
                        if (model.CorrectAnswers.Count() == 1)
                        {
                            CorrectAnswer correct_answer = new CorrectAnswer()
                            {
                                Answer = model.CorrectAnswers.First(),
                                Question = model.Id
                            };
                            long correct_id = AddCorrectAnswer(correct_answer);
                            if (correct_id == -1)
                            {
                                //all correct 
                                DeleteAllCorrectAnswersForQuestion(model.Id);
                                //all possible answers 
                                DeleteAllPossibleAnswersForQuestion(model.Id);
                                //Delete question
                                //_uow.Questions.Remove(result);


                                return false;
                            }
                        }
                    }
                

                _uow.Complete();
                return true;
            }
            catch (Exception e)
            {
                //Log exception
                //var exception = e.InnerException;
                //Debug.Write(exception.Message);
                //Debug.Write(e.Message);

                return false;
            }
        }
        public long GetQuizId(long questionId)
        {
            try
            {
                Question question = _uow.Questions.Get(questionId);
                if (question != null)
                    return question.Quiz_Id;
                else
                    return -1;
            }
            catch (Exception e)
            {
                //Log exception
                //var exception = e.InnerException;
                //Debug.Write(exception.Message);
                //Debug.Write(e.Message);

                return -1;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">is Quiz id</param>
        /// <returns></returns>
        public List<QuestionPlayModel> GetQuestions(long id)
        {
            try
            {
                List<QuestionPlayModel> result = new List<QuestionPlayModel>();
                Question[] questions = _uow.Questions.GetAll().Where(x => x.Quiz_Id == id).ToArray();
                for(int i = 0; i < questions.Count(); i++)
                {
                    QuestionPlayModel model = new QuestionPlayModel();
                    model.Id = questions[i].Id;
                    if (questions[i].Points.HasValue)
                        model.Points = questions[i].Points.Value;
                    model.Question_number = i + 2;
                    model.Question_text = questions[i].Question_text;
                    if(questions[i].PossibleAnswers.Count() == 0)
                    {
                        if(questions[i].CorrectAnswers.Count() == 1)
                        {
                            model.CorrectAnswers = new List<ItemModel>();
                            ItemModel item = new ItemModel();
                            item.Value = questions[i].CorrectAnswers.FirstOrDefault().Answer;
                            item.Id = -1;
                            model.CorrectAnswers.Add(item);
                        }
                    }
                    else
                    {
                        model.PossibleAnswers = new List<ItemModel>();
                        model.CorrectAnswers = new List<ItemModel>();
                        foreach(PossibleAnswer possible in questions[i].PossibleAnswers)
                        {
                            ItemModel item = new ItemModel();
                            item.Id = possible.Id;
                            item.Value = possible.text;
                            model.PossibleAnswers.Add(item);
                        }
                        foreach(CorrectAnswer correct in questions[i].CorrectAnswers)
                        {
                            ItemModel item = new ItemModel();
                            if(correct.PossibleAnswer.HasValue)
                                item.Id = correct.PossibleAnswer.Value;
                            item.Value = correct.Answer;
                            model.CorrectAnswers.Add(item);
                        }

                    }
                    result.Add(model);
                }
                return result;
            }
            catch(Exception e)
            {
                return null;
            }
        }
        private long AddCorrectAnswer(CorrectAnswer answer)
        {
            try
            {
                CorrectAnswer result = new CorrectAnswer();
                CorrectAnswer correct = new CorrectAnswer
                {
                    Question = answer.Question,
                    PossibleAnswer = answer.PossibleAnswer,
                    Answer = answer.Answer
                };
                result = _uow.CorrectAnswers.Add(correct);
                _uow.Complete();

                return result.Id;
            }
            catch (Exception e)
            {
                //Log exception
                //var exception = e.InnerException;
                //Debug.Write(exception.Message);
                //Debug.Write(e.Message);

                return -1;
            }
        }
        private long AddPossibleAnswer(PossibleAnswer answer)
        {
            try
            {
                PossibleAnswer result = new PossibleAnswer();
                PossibleAnswer possible = new PossibleAnswer
                {
                    Question_Id = answer.Question_Id,
                    text = answer.text 
                };
                result = _uow.PossibleAnswers.Add(possible);
                _uow.Complete();

                return result.Id;
            }
            catch (Exception e)
            {
                //Log exception
                //var exception = e.InnerException;
                //Debug.Write(exception.Message);
                //Debug.Write(e.Message);

                return -1;
            }
        }
        private bool DeleteAllPossibleAnswersForQuestion(long questionId)
        {
            try
            {
                //first delete statistics connected to these possible answers
                List<Statistic> stats = new List<Statistic>();
                List<PossibleAnswer> answers = _uow.PossibleAnswers.GetAll().Where(x => x.Question_Id == questionId).ToList();
                foreach(var answer in answers)
                {
                    List<Statistic> tmp = _uow.Statistics.GetAll().Where(x => x.Answer == answer.Id).ToList();
                    stats.AddRange(tmp);
                }
                if(stats.Count() >0)
                {
                    _uow.Statistics.RemoveRange(stats);
                    _uow.Complete();
                }
                
                _uow.PossibleAnswers.RemoveRange(answers);
                _uow.Complete();
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }
        private bool DeleteAllCorrectAnswersForQuestion(long questionId)
        {
            try
            {
                List<CorrectAnswer> answers = _uow.CorrectAnswers.GetAll().Where(x => x.Question == questionId).ToList();
                _uow.CorrectAnswers.RemoveRange(answers);
                _uow.Complete();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private bool DeleteAllPossibleAnswersForQuiz(List<long> questionIds)
        {
            try
            {
                //first delete statistics connected to these possible answers
                List<Statistic> stats = new List<Statistic>();
                List<PossibleAnswer> answers = new List<PossibleAnswer>();
                foreach(long id in questionIds)
                {
                    List<PossibleAnswer> tmp = _uow.PossibleAnswers.GetAll().Where(x => x.Question_Id == id).ToList();
                    answers.AddRange(tmp);
                }
                foreach (var answer in answers)
                {
                    List<Statistic> tmp = _uow.Statistics.GetAll().Where(x => x.Answer == answer.Id).ToList();
                    stats.AddRange(tmp);
                }
                if (stats.Count() > 0)
                {
                    _uow.Statistics.RemoveRange(stats);
                    _uow.Complete();
                }
                _uow.PossibleAnswers.RemoveRange(answers);
                _uow.Complete();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private bool DeleteAllCorrectAnswersForQuiz(List<long> questionIds)
        {
            try
            {
                List<CorrectAnswer> answers = new List<CorrectAnswer>();
                foreach(long id in questionIds)
                {
                    List<CorrectAnswer> tmp = _uow.CorrectAnswers.GetAll().Where(x => x.Question == id).ToList();
                    answers.AddRange(tmp);
                }
                _uow.CorrectAnswers.RemoveRange(answers);
                _uow.Complete();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}
