using QuizMaker.DAL.DB;
using QuizMaker.Interfaces.BL;
using QuizMaker.Interfaces.Repository;
using QuizMaker.Models.QuestionModels;
using System;
using System.Collections.Generic;
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
        public long Add(QuestionModel question)
        {
            try
            {
                Question q = new Question();
                q.Question_number = question.Question_number;
                q.Question_text = question.Question_text;
                q.Quiz_Id = question.Quiz_Id;
                q.Correct_answer = question.Correct_answer;
                q = _uow.Questions.Add(q);
                _uow.Complete();

                return q.Id;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public bool Delete(long id)
        {
            try
            {
                Question question = _uow.Questions.Get(id);
                _uow.Questions.Remove(question);
                _uow.Complete();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public QuestionModel Get(long id)
        {
            try
            {
                Question question = _uow.Questions.Get(id);
                QuestionModel model = new QuestionModel();
                model.Id = question.Id;
                model.PossibleAnswers = question.PossibleAnswers.ToList();
                model.Question_number = question.Question_number;
                model.Question_text = question.Question_text;
                model.Quiz_Id = question.Quiz_Id;
                if(question.Correct_answer.HasValue)
                    model.Correct_answer = question.Correct_answer.Value;

                return model;
            }
            catch (Exception e)
            {
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
                return null;
            }
        }

        public List<QuestionModel> GetQuestionsForQuiz(long id)
        {
            try
            {
                List<QuestionModel> questions = new List<QuestionModel>();
                List<Question> qs = _uow.Questions.GetAll().Where(x => x.Quiz_Id == id).ToList();
                foreach( Question question in qs)
                {
                    QuestionModel model = new QuestionModel();
                    model.Id = question.Id;
                    model.PossibleAnswers = question.PossibleAnswers.ToList();
                    model.Question_number = question.Question_number;
                    model.Question_text = question.Question_text;
                    model.Quiz_Id = question.Quiz_Id;
                    if (question.Correct_answer.HasValue)
                        model.Correct_answer = question.Correct_answer.Value;

                    questions.Add(model);
                }

                return questions;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool Update(QuestionModel model)
        {
            try
            {
                Question question = _uow.Questions.Get(model.Id);
                question.PossibleAnswers = model.PossibleAnswers;
                question.Question_number = model.Question_number;
                question.Question_text = model.Question_text;
                question.Quiz_Id = model.Quiz_Id;
                question.Correct_answer = model.Correct_answer;

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
