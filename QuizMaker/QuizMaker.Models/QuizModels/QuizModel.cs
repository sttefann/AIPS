using QuizMaker.DAL.DB;
using QuizMaker.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Models.QuizModels
{
    public class QuizModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long Owner_Id { get; set; }
        public QuizType Type { get; set; }
        public bool ReadOnly { get; set; }
        public Category Category { get; set; }
        public List<Statistic> StatisticsList { get; set; }
        public List<Question> Questions { get; set; }
        public QuizModel()
        {
            Questions = new List<Question>();
            StatisticsList = new List<Statistic>();
            Category = new Category();
        }

    }
}