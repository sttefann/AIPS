using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Models.QuestionModels
{
    public class QuestionCreateModel
    {
        public long Quiz_Id { get; set; }
        public string Question_text { get; set; }
        public Nullable<int> Question_number { get; set; }
        public int Points { get; set; }
        public List<string> CorrectAnswers { get; set; }
        public List<string> PossibleAnswers { get; set; }
        public QuestionCreateModel()
        {
            PossibleAnswers = new List<string>();
            CorrectAnswers = new List<string>();
        }
    }
}
