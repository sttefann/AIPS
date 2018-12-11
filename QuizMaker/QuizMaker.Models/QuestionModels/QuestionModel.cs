using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizMaker.DAL.DB;

namespace QuizMaker.Models.QuestionModels
{
    public abstract class QuestionModel
    {
        public long Id { get; set; }
        public string Question_text { get; set; }
        public int Question_number { get; set; }
        public long Quiz_Id { get; set; }
        public int Points { get; set; }
        public List<CorrectAnswer> Correct_answers { get; set; }
    }
}
