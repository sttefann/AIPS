using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Models.StatisticsModel
{
   public class PollStats
    {
        public string PossibleAnswer { get; set; }
        public int Votes { get; set; }
        public bool MyAnswer { get; set; }
    }
}
