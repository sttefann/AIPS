
using QuizMaker.Models.CategoryModel;
using QuizMaker.Models.QuizModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Interfaces.BL
{
    public interface ICategoryManager
    {
        List<CategoryModel> GetMainCategories();
        List<CategoryModel> GetSubCategories(long id);
        List<QuizModel> GetQuizzesFromCategory(long category_id);
        CategoryModel GetCategory(long id);
        long NewCategory(CategoryModel category);
        bool UpdateCategory(CategoryModel category);
        bool DeleteCategory(long id);
    }
}
