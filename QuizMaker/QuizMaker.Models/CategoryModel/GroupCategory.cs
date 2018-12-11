
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizMaker.Models.QuizModels;

namespace QuizMaker.Models.CategoryModel
{
    public class GroupCategory : ICategory
    {
        private long id;
        private string name;
        private long parent;
        private GroupCategory parentNode;
        private List<ICategory> subCategories;
        public GroupCategory()
        {
            this.subCategories = new List<ICategory>();
        }
        public string GetName()
        {
            return this.name;
        }

        public void SetName(string namee)
        {
            this.name = namee;
        }

        public string Show()
        {
            string html = "";
            if(id == 0)
            {
                html = "<div class=\"dd\" id=\"nestable\"> <ol class=\"dd-list\">" + ShowChildren(this) + "</ol> </div>";
            }
            else
            {
                    html = "<li class=\"dd-item\" data-id=\""+ id + "\"> <a href=\"/Home/Category/" + id + "\"> <div class=\"dd-handle\">" + name + "</div></a> <ol class=\"dd-list\">"
                        + ShowChildren(this) +
                        "</li> </ol>";
            }
            return html;
        }
        public void AddSubCategory(ICategory category)
        {
            this.subCategories.Add(category);
        }
        public void AddSubCategories(List<ICategory> categories)
        {
            this.subCategories.AddRange(categories);
        }
        public List<ICategory> GetSubCategories()
        {
            return this.subCategories;
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
            return false;
        }

        public void SetParent(long id)
        {
            this.parent = id;
        }

        public long GetParent()
        {
            return parent;
        }
        private string ShowChildren(GroupCategory tmpRoot)
        {
            string result = "";
            foreach(ICategory category in tmpRoot.GetSubCategories())
            {
                result += category.Show();
            }
            return result;
        }

        public void AddChild(long id, string name)
        {
            LeafCategory child = new LeafCategory();
            child.SetId(id);
            child.SetName(name);
            child.SetParent(this.id);
            child.SetParentNode(this);
            this.subCategories.Add(child);
        }

        public void RemoveChild(long id)
        {
            List<ICategory> children = this.subCategories.Where(x => x.GetId() == id).ToList();
            if(children.Count() != 0)
            {
                ICategory child = children.First();
                this.subCategories.Remove(child);
            }
            
        }

        public ICategory GetChild(long id)
        {
            List<ICategory> children = this.subCategories.Where(x => x.GetId() == id).ToList();
            if (children.Count() != 0)
            {
                ICategory child = children.First();
                return child;
            }
            else
            {
                foreach (ICategory sub in subCategories)
                {
                    ICategory result = sub.GetChild(id);
                    if(result != null)
                    {
                        return result;
                    }
                }
            }
            return null;
        }

        public List<QuizzCategoryModel> ShowQuizzes()
        {
            List<QuizzCategoryModel> result = new List<QuizzCategoryModel>();
            foreach(ICategory child in subCategories)
            {
                result.AddRange(child.ShowQuizzes());
            }
            return result;
        }

        public void SetParentNode(GroupCategory parentt)
        {
            this.parentNode = parentt;
        }
    }
}
