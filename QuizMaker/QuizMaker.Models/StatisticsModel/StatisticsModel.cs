using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Models.StatisticsModel
{
    public class StatisticsModel
    {
        public long Question_Id { get; set; }
        public Nullable<long> Answer { get; set; }
        public long Quiz_Id { get; set; }
        public long User_Id { get; set; }
        public string Answer_text { get; set; }
        public Nullable<int> Points { get; set; }
    }
}
