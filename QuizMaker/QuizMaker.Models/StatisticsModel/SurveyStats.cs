using QuizMaker.Models.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Models.StatisticsModel
{
    public class SurveyStats
    {
        public string Question_text { get; set; }
        public List<SurveyItem> Answers { get; set; }
    }
}
