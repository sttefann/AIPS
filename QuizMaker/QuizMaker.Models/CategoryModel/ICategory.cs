using QuizMaker.Models.QuizModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Models.CategoryModel
{
    public interface ICategory
    {
        void SetName(string name);
        string GetName();
        void SetId(long Id);
        long GetId();
        bool IsLeaf();
        void SetParent(long name);
        long GetParent();
        string Show();
        void AddChild(long id, string name);
        void RemoveChild(long id);
        ICategory GetChild(long id);
        List<QuizzCategoryModel> ShowQuizzes();
        void SetParentNode(GroupCategory parent);

    }
}
