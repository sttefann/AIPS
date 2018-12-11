using Newtonsoft.Json;
using QuizMaker.Models.QuizModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Models.CategoryModel
{
    public class LeafCategory : ICategory
    {
        private long id;
        private string name;
        private GroupCategory parentNode;
        private long parent;
        private List<QuizzCategoryModel> quizzes;
        public LeafCategory()
        {
            this.quizzes = new List<QuizzCategoryModel>();
        }
        public string GetName()
        {
            return this.name;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public string Show()
        {
            string html = "";
            html = "<li class=\"dd-item\" data-id=\"" + id + "\"><a href=\"/Home/Category/" + id + "\"> <div class=\"dd-handle\">" + this.name + "</div> </a></li>";
            return html;
        }
        //public string ShowQuizzes()
        //{
        //    string result = JsonConvert.SerializeObject(quizzes);
        //    return result;
        // href=\"/Home/Category/" + id + "\"
        //}

        public bool AddQuizz(QuizzCategoryModel quiz)
        {
            if (quizzes.Contains(quiz))
            {
                return false;
            }
            else
            {
                this.quizzes.Add(quiz);
                return true;
            }
        }
        public void AddQuizzes(List<QuizzCategoryModel> quizzRange)
        {
                this.quizzes.AddRange(quizzRange);
        }
        public bool DeleteQuizz(long quizId)
        {
            var item = this.quizzes.Where(x => x.Id == quizId).FirstOrDefault();
            if (item != null)
            {
                this.quizzes.Remove(item);
                return true;
            }
            return false;
        }

        public void SetId(long Id)
        {
            this.id = Id;
        }

        public long GetId()
        {
            return this.id;
        }

        public bool IsLeaf()
        {
            return true;
        }

        public void SetParent(long id)
        {
            parent = id;
        }

        public long GetParent()
        {
            return parent;
        }

        public void AddChild(long id, string name)
        {
            LeafCategory child = new LeafCategory();
            child.SetId(id);
            child.SetName(name);
            child.SetParent(this.id);
            GroupCategory category = new GroupCategory();
            category.SetId(this.id);
            category.SetName(this.name);
            category.SetParent(this.parent);
            category.SetParentNode(this.parentNode);

            child.SetParentNode(category);
            category.AddSubCategory(child);
            this.parentNode.RemoveChild(this.id);
            this.parentNode.AddSubCategory(category);

        }

        public void RemoveChild(long id)
        {
            return;
        }

        List<QuizzCategoryModel> ICategory.ShowQuizzes()
        {
            return quizzes;
        }

        public ICategory GetChild(long id)
        {
            return null;
        }

        public void SetParentNode(GroupCategory parentt)
        {
            this.parentNode = parentt;
        }
    }
}
