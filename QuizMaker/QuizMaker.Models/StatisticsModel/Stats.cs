using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Models.StatisticsModel
{
    public class Stats<T>
    {
        public string Name { get; set; }
        public List<T> Standings { get; set; }
        //public List<TestRankList> TestStats { get; set; }
        //public List<SurveyStats> SurveyStats { get; set; }
        //public List<PollStats> PollStats { get; set; }

        public Stats()
        {
            Standings = new List<T>();
            //TestStats = new List<TestRankList>();
            //SurveyStats = new List<SurveyStats>();
            //PollStats = new List<PollStats>();
        }
    }
}
