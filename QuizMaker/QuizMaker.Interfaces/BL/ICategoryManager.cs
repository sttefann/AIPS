
using QuizMaker.DAL.DB;
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
        GroupCategory GetAllCategories();
        List<CategoryModel> GetMainCategories();
        List<ICategory> GetSubCategories(long id, GroupCategory parent);
        List<QuizModel> GetQuizzesFromCategory(long category_id);
        Category GetCategory(long id);
        LeafCategory GetLeaf(long id);
        long NewCategory(CategoryModel category);
        bool UpdateCategory(CategoryModel category);
        bool DeleteCategory(long id);
    }
}
