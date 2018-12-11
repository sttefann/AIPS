using QuizMaker.DAL.DB;
using QuizMaker.Interfaces.BL;
using QuizMaker.Interfaces.Repository;
using QuizMaker.Models.Enums;
using QuizMaker.Models.Item;
using QuizMaker.Models.QuizModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public long Add(QuizCreateModel model)
        {
            try
            {
                Quiz quiz = new Quiz();
                quiz.Type = (int)model.Type;
                quiz.Name = model.Name;
                quiz.Owners_Id = model.Owner_Id;
                quiz.Category_Id = model.Category_Id;
                quiz.ReadOnly = false;
                
                Quiz result = _uow.Quizzes.Add(quiz);
                _uow.Complete();
                return result.Id;
            }
            catch (Exception e)
            {
                //Log exception
                var exception = e.InnerException;
            //    Debug.Write(exception.Message);
                //Debug.Write(e.Message);

                return -1;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="quizId"></param>
        /// <param name="userId">Needs to be checked and valid</param>
        /// <returns></returns>
        public bool AddTeamMember(long quizId, long userId)
        {
            try
            {
                Quiz quiz = _uow.Quizzes.Get(quizId);
                if (quiz == null) return false;
                TeamMember teamMember = new TeamMember();
                teamMember.Quiz_Id = quizId;
                teamMember.User_Id = userId;
                _uow.TeamMembers.Add(teamMember);
                _uow.Complete();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void LeaveTeamProject(long quiz_id)
        {
            try
            {
                Quiz quiz = _uow.Quizzes.Get(quiz_id);
                List<TeamMember> members = _uow.TeamMembers.GetAll().Where(x => x.Quiz_Id == quiz_id).ToList();
                quiz.Owners_Id = members.First().Id;
                _uow.Complete();
            }
            catch(Exception e)
            {

            }
        }

        public bool Delete(long id)
        {
            try
            {
                Quiz quiz = _uow.Quizzes.Get(id);
                List<TeamMember> members = _uow.TeamMembers.GetAll().Where(x => x.Quiz_Id == id).ToList();
                if(members.Count() > 0)
                {
                    LeaveTeamProject(id);
                }
                else
                {
                    _uow.Quizzes.Remove(quiz);
                    _uow.Complete();
                }
                
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

        public QuizModel Get(long id)
        {
            try
            {
                QuizModel quiz = new QuizModel();
                Quiz model = _uow.Quizzes.Get(id);


                quiz.Category = _uow.Categories.Get( model.Category.id);
                quiz.Id = model.Id;
                quiz.Name = model.Name;
                if(model.ReadOnly.HasValue)
                    quiz.ReadOnly = model.ReadOnly.Value;
                quiz.Owner_Id = model.Owners_Id;

                return quiz;
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
        public string GetType(long id)
        {
            try
            {
                Quiz model = _uow.Quizzes.Get(id);
                if (model != null)
                {
                    QuizType type = model.Type.ToQuizType();
                    return type.ConvertToString();
                }
                return "";
                
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
                    quiz.Owner_Id = model.Owners_Id;
                    models.Add(quiz);
                }
                

                return models;
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

        public QuizCreateModel GetCreateModel()
        {
            QuizCreateModel result = new QuizCreateModel();
            try
            {
               result.Categories = _uow.Categories.GetLeafCategories().Select(s => new ItemModel
                {
                    Id = s.id,
                    Value = s.Name
                }).ToList();
                foreach (var name in Enum.GetNames(typeof(QuizType)))
                {
                    ItemTypeModel item = new ItemTypeModel();
                    item.Id = (int)(QuizType)Enum.Parse(typeof(QuizType), name);
                    item.Value = name;
                    if (item.Id != 4)
                        result.Types.Add(item);
                }
            }
            catch(Exception e)
            {
                //Log exception
              //  var exception = e.InnerException;
              ////  Debug.Write(exception.Message);
              //  Debug.Write(e.Message);
            }
            return result;
        }

        public QuizEditModel GetEditModel(long id)
        {
            
            try
            {
                QuizEditModel result = new QuizEditModel();
                result.Categories = _uow.Categories.GetLeafCategories().Select(s => new ItemModel
                {
                    Id = s.id,
                    Value = s.Name
                }).ToList();
                var quiz = Get(id);
                if (quiz != null)
                {
                    result.Category_Id = quiz.Category.id;
                    result.Name = quiz.Name;
                }
                else{
                    result.Name = "";
                }
                return result;
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
        public bool Update(long id, QuizEditModel model)
        {
            try
            {
                Quiz quiz = _uow.Quizzes.Get(id);
                quiz.Category_Id = model.Category_Id;
                quiz.Name = model.Name;

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
        

        public bool IsOwner(long quizId, long userId)
        {
            try
            {
                Quiz quiz = _uow.Quizzes.Get(quizId);
                if (quiz == null) return false;
                if (quiz.Owners_Id == userId)
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool MakeReadOnly(long id)
        {
            try
            {
                Quiz quiz = _uow.Quizzes.Get(id);
                quiz.ReadOnly = true;
                _uow.Complete();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool MakeReadOnly(long id, bool value)
        {
            try
            {
                    Quiz quiz = _uow.Quizzes.Get(id);
                    quiz.ReadOnly = value;

                    _uow.Complete();
                    return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public QuizPlayModel GetQuiz(long id)
        {
            try
            {
                QuizPlayModel result = new QuizPlayModel();
                Quiz quiz = _uow.Quizzes.Get(id);
                if(quiz != null)
                {
                    result.Id = id;
                    result.Name = quiz.Name;
                    result.Type = quiz.Type.ToQuizType().ConvertToString();
                    return result;
                }
                return null;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public List<QuizzCategoryModel> GetAll(long user_id)
        {
            try
            {
                List<QuizzCategoryModel> result = new List<QuizzCategoryModel>();
                List<Quiz> quizzes = _uow.Quizzes.GetAll().Where(x => x.Owners_Id == user_id).ToList();
                foreach(var quiz in quizzes)
                {
                    QuizzCategoryModel model = new QuizzCategoryModel();
                    model.Id = quiz.Id;
                    model.Name = quiz.Name;
                    model.Type = quiz.Type.ToQuizType().ConvertToString();
                    result.Add(model);
                }
                return result;
            }catch(Exception e)
            {
                return null;
            }
        }
        public string GetPollText(long id)
        {
            try
            {
                Quiz quiz = _uow.Quizzes.Get(id);
                Question question = quiz.Questions.First();
                if (question != null)
                {
                    return question.Question_text;
                }
                return "";

            }
            catch (Exception e)
            {
                return "";
            }
        }
        public string GetQuizName(long id)
        {
            try
            {
                QuizModel quiz = Get(id);
                if(quiz != null)
                {
                    return quiz.Name;
                }
                return "";

            }catch(Exception e)
            {
                return "";
            }
        }

        public List<QuizzCategoryModel> GetAllTeamProjects(long user_id)
        {
            try
            {
                List<QuizzCategoryModel> result = new List<QuizzCategoryModel>();
                List<Quiz> quizzes = _uow.Quizzes.GetAll();
                foreach (var quiz in quizzes)
                {
                    if (quiz.Owners_Id != user_id)
                    {
                        if (quiz.TeamMembers.Any(x => x.User_Id == user_id))
                        {
                            QuizzCategoryModel model = new QuizzCategoryModel();
                            model.Id = quiz.Id;
                            model.Name = quiz.Name;
                            model.Type = quiz.Type.ToQuizType().ConvertToString();
                            result.Add(model);
                        }
                    }
                    else
                    {
                        if(quiz.TeamMembers.Count() > 0)
                        {
                            QuizzCategoryModel model = new QuizzCategoryModel();
                            model.Id = quiz.Id;
                            model.Name = quiz.Name;
                            model.Type = quiz.Type.ToQuizType().ConvertToString();
                            result.Add(model);
                        }
                        
                    }
                   
                }
                return result;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public bool IsTeamProject(long quizId)
        {
            try
            {
                List<TeamMember> members = _uow.TeamMembers.GetAll().Where(x => x.Quiz_Id == quizId).ToList();
                if (members.Count() > 0)
                {
                    return true;
                }
                return false;
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}
