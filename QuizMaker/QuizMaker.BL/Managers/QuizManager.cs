using QuizMaker.DAL.DB;
using QuizMaker.Interfaces.BL;
using QuizMaker.Interfaces.Repository;
using QuizMaker.Models.QuizModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.BL.Managers
{
    public class QuizManager : IQuizManager
    {
        private readonly IUow _uow;

        public QuizManager(IUow uow)
        {
            _uow = uow;
        }
        public bool Add(QuizModel model)
        {
            try
            {
                Quiz quiz = new Quiz();
                quiz.Category_Id = model.Category.id;
                quiz.Id = model.Id;
                quiz.Name = model.Name;
                quiz.Owners_Id = model.Owner_Id;
                quiz.Team_Id = model.Team.id;

                _uow.Quizzes.Add(quiz);
                _uow.Complete();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool Delete(long id)
        {
            try
            {
                Quiz quiz = _uow.Quizzes.Get(id);
                _uow.Quizzes.Remove(quiz);
                _uow.Complete();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public QuizModel Get(long id)
        {
            try
            {
                QuizModel quiz = new QuizModel();
                Quiz model = _uow.Quizzes.Get(id);


                quiz.Category = _uow.Categories.Get( model.Category.id);
                quiz.Id = model.Id;
                quiz.Name = model.Name;
                quiz.Team = _uow.Teams.Get( model.Team.id);
                quiz.Owner_Id = model.Owners_Id;

                return quiz;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<QuizModel> GetAll()
        {
            try
            {
                List<QuizModel> models = new List<QuizModel>();
                List<Quiz> quizzes = _uow.Quizzes.GetAll().ToList();
                foreach(Quiz model in quizzes)
                {
                    QuizModel quiz = new QuizModel();

                    quiz.Category = _uow.Categories.Get(model.Category.id);
                    quiz.Id = model.Id;
                    quiz.Name = model.Name;
                    quiz.Team = _uow.Teams.Get(model.Team.id);
                    quiz.Owner_Id = model.Owners_Id;
                    models.Add(quiz);
                }
                

                return models;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool Update(QuizModel model)
        {
            try
            {
                Quiz quiz = _uow.Quizzes.Get(model.Id);
                quiz.Category_Id = model.Category.id;
                quiz.Id = model.Id;
                quiz.Name = model.Name;
                quiz.Owners_Id = model.Owner_Id;
                quiz.Team_Id = model.Team.id;

                
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
