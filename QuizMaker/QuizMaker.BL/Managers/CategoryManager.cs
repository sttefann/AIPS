using QuizMaker.DAL.DB;
using QuizMaker.Interfaces.BL;
using QuizMaker.Interfaces.Repository;
using QuizMaker.Models.CategoryModel;
using QuizMaker.Models.Enums;
using QuizMaker.Models.QuizModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.BL.Managers
{

    public class CategoryManager : ICategoryManager
    {
        private readonly IUow _uow;

        public CategoryManager(IUow uow)
        {
            _uow = uow;
        }
        public bool DeleteCategory(long id)
        {
            try
            {
                Category category = _uow.Categories.Get(id);
                _uow.Categories.Remove(category);
                _uow.Complete();
                return true;
            }
            catch(Exception e)
            {
                //Log exception
                var exception = e;
                Debug.Write(exception.Message);
                Debug.Write(e.Message);
                return false;
            }
           
        }

        public GroupCategory GetAllCategories()
        {
            try
            {
                GroupCategory root = new GroupCategory();
                root.SetName("root");
                root.SetId(0);
                root.AddSubCategories(GetSubCategories(0, root));

                return root;
            }
            catch(Exception ex)
            {
                //Log exception
                var exception = ex;
                Debug.Write(exception.Message);
                Debug.Write(ex.Message);

                return null;
            }
        }
        public Category GetCategory(long id)
        {
            try
            {
                Category category = _uow.Categories.Get(id);
                return category;
            }
            catch (Exception e)
            {
                //Log exception
                var exception = e;
                Debug.Write(exception.Message);
                Debug.Write(e.Message);

                return null;
            }
        }

        public List<CategoryModel> GetMainCategories()
        {
            try
            {
                List<Category> categories = _uow.Categories.GetAll().Where(x => x.Parent_id == 0).ToList();
                List<CategoryModel> models = new List<CategoryModel>();
                foreach( Category category in categories)
                {
                    CategoryModel model = new CategoryModel();
                    model.id = category.id;
                    model.Name = category.Name;
                    model.Parent_id = category.Parent_id;
                    model.Partible = (category.Partible == 1)? true : false;
                    model.Quizzes = category.Quizzes.ToList();

                    models.Add(model);
                }
                
                return models;
            }
            catch (Exception e)
            {
                //Log exception
                var exception = e.InnerException;
             //   Debug.Write(exception.Message);
                Debug.Write(e.Message);

                return null;
            }
        }
        public LeafCategory GetLeaf(long id)
        {
            try
            {
                LeafCategory model = new LeafCategory();
                Category category = _uow.Categories.Get(id);
                if (category != null)
                {
                    model.SetId(category.id);
                    model.SetName(category.Name);
                    model.SetParent(category.Parent_id);

                    foreach (Quiz quiz in category.Quizzes)
                    {
                        if(quiz.Questions.Count() > 0)
                        {
                            QuizzCategoryModel quizModel = new QuizzCategoryModel();
                            quizModel.Id = quiz.Id;
                            quizModel.Name = quiz.Name;
                            quizModel.Type = quiz.Type.ToQuizType().ConvertToString();
                            model.AddQuizz(quizModel);
                        }
                        
                    }
                }
                return model;
            }
            catch (Exception e)
            {
                //Log exception
                var exception = e;
                //    Debug.Write(exception.Message);
                Debug.Write(e.Message);

                return null;
            }
        }
        public List<QuizModel> GetQuizzesFromCategory(long category_id)
        {
            try
            {

                List<Quiz> quizzes = _uow.Quizzes.GetAll().Where(x => x.Category_Id == category_id).ToList();
                List<QuizModel> models = new List<QuizModel>();
                foreach(Quiz quiz in quizzes)
                {
                    QuizModel model = new QuizModel();
                    if(quiz.Category_Id.HasValue)
                        model.Category = _uow.Categories.Get(quiz.Category_Id.Value);
                    model.Id = quiz.Id;
                    model.Name = quiz.Name;
                    model.Owner_Id = quiz.Owners_Id;
                    List<Question> questions = new List<Question>();
                    foreach(Question question in _uow.Questions.GetAll().Where(x => x.Quiz_Id == model.Id))
                    {
                        questions.Add(question);
                    }
                    model.Questions = questions;

                    models.Add(model);
                }
   
                return models;
            }
            catch (Exception e)
            {
                //Log exception
                var exception = e;
            //    Debug.Write(exception.Message);
                Debug.Write(e.Message);

                return null; 
            }
        }

        public List<ICategory> GetSubCategories(long id, GroupCategory parent)
        {
            try
            {
                List<Category> categories = _uow.Categories.GetAll().Where(x => x.Parent_id == id).ToList();
                List<ICategory> models = new List<ICategory>();
                foreach (Category category in categories)
                {
                    
                    if(category.Partible == 1)
                    {
                        GroupCategory model = new GroupCategory();
                        model.SetName(category.Name);
                        model.SetId(category.id);
                        model.SetParent(category.Parent_id);
                        model.SetParentNode(parent);
                        model.AddSubCategories(GetSubCategories(category.id, model));

                        models.Add(model);
                    }
                    else
                    {
                        LeafCategory model = new LeafCategory();
                        model.SetId(category.id);
                        model.SetName(category.Name);
                        model.SetParent(category.Parent_id);
                        model.SetParentNode(parent);
                        foreach (Quiz quiz in category.Quizzes)
                        {
                            QuizzCategoryModel quizModel = new QuizzCategoryModel();
                            quizModel.Id = quiz.Id;
                            quizModel.Name = quiz.Name;
                            quizModel.Type = quiz.Type.ToQuizType().ConvertToString();
                            model.AddQuizz(quizModel);
                        }

                        models.Add(model);
                    }
                }

                return models;
            }
            catch (Exception e)
            {
                //Log exception
                var exception = e;
                Debug.Write(exception.Message);
                Debug.Write(e.Message);

                return null;
            }
        }

        public long NewCategory(CategoryModel category)
        {
            try
            {
                Category categori = new Category();
                
                categori.Name = category.Name;
                categori.Parent_id = category.Parent_id;
                categori.Partible = category.Partible ? (byte)1 : (byte)0;
                categori.Quizzes = category.Quizzes;
                
                _uow.Categories.Add(categori);
                _uow.Complete();
                return categori.id;
            }
            catch (Exception e)
            {
                //Log exception
                var exception = e.InnerException;
                Debug.Write(exception.Message);
                Debug.Write(e.Message);

                return -1;
            }
        }

        public bool UpdateCategory(CategoryModel category)
        {
            try
            {
                Category newcategory = _uow.Categories.Get(category.id);

                newcategory.Name = category.Name;
                newcategory.Parent_id = category.Parent_id;
                newcategory.Partible = category.Partible? (byte)1 : (byte)0;
                newcategory.Quizzes = category.Quizzes;
                
                _uow.Complete();
                return true;
            }
            catch (Exception e)
            {
                //Log exception
                var exception = e;
                Debug.Write(exception.Message);
                Debug.Write(e.Message);

                return false;
            }
        }
    }
}
