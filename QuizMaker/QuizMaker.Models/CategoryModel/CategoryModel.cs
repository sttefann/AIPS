using QuizMaker.DAL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Models.CategoryModel
{
    public class CategoryModel
    {
        public long id { get; set; }
        public string Name { get; set; }
        public long Parent_id { get; set; }
        public bool Partible { get; set; }
        public List<Quiz> Quizzes { get; set; }
        public CategoryModel()
        {
            this.Quizzes = new List<Quiz>();
        }
    }
}
