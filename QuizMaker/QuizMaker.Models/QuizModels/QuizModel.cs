using QuizMaker.DAL.DB;
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
        public Team Team { get; set; }
        public Category Category { get; set; }
        public List<StatisticalList> StatisticalList { get; set; }
        public List<SurveyStatistic> SurveyStatisticsList { get; set; }
        public List<Question> Questions { get; set; }
        public QuizModel()
        {
            Questions = new List<Question>();
            SurveyStatisticsList = new List<SurveyStatistic>();
            StatisticalList = new List<StatisticalList>();
            Category = new Category();
            Team = new Team();
        }

    }
}
