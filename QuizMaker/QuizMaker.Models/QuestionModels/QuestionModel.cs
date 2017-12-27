using QuizMaker.DAL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Models.QuestionModels
{
    public class QuestionModel
    {
        public long Id { get; set; }
        public string Question_text { get; set; }
        public int Question_number { get; set; }
        public long Correct_answer { get; set; }
        public long Quiz_Id { get; set; }
        public List<PossibleAnswer> PossibleAnswers { get; set; }
        public QuestionModel()
        {
            PossibleAnswers = new List<PossibleAnswer>();
        }
    }
}
