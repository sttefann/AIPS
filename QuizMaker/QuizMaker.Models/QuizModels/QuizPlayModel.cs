using QuizMaker.Models.Enums;
using QuizMaker.Models.QuestionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Models.QuizModels
{
    public class QuizPlayModel
    {
        public List<QuestionPlayModel> Questions { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public QuizPlayModel()
        {
            Questions = new List<QuestionPlayModel>();
        }
    }
}
