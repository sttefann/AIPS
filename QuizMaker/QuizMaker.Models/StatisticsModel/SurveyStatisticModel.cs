using QuizMaker.DAL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Models.StatisticsModel
{
    public class SurveyStatisticModel
    {
        public long Id { get; set; }
        public long Quiz_Id { get; set; }
        public long UserID { get; set; }
        public long Question_Id { get; set; }
        public long Answer_id { get; set; }
        public SurveyStatisticModel()
        {
        }
    }
}
