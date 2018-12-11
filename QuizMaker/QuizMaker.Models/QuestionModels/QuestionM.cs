using QuizMaker.DAL.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Models.QuestionModels
{
    public class QuestionM
    {
        public long Id { get; set; }
        public string Question_text { get; set; }
        public int Question_number { get; set; }
        public long Quiz_Id { get; set; }
        public int Points { get; set; }
        public bool Team { get; set; }
        public List<CorrectAnswer> Correct_answers { get; set; }
        public List<PossibleAnswer> PossibleAnswers { get; set; }
        public QuestionM()
        {
            PossibleAnswers = new List<PossibleAnswer>();
            Correct_answers = new List<CorrectAnswer>();
        }
    }
}
