using QuizMaker.DAL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Models.StatisticsModel
{
    public class StatisticsCreateModel
    {
        public long Quiz_Id { get; set; }
        public List<StatisticsModel> Stats { get; set; }
    }
}
