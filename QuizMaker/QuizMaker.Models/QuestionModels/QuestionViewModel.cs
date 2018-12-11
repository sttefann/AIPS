using QuizMaker.DAL.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Models.QuestionModels
{
    public class QuestionViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Text")]
        public string Question_text { get; set; }
        public int Question_number { get; set; }
        public int Points { get; set; }
        public List<AnswerItem> Answers { get; set; }
        public string Type { get; set; }
        public bool ReadOnly { get; set; }

        public QuestionViewModel()
        {
            Answers = new List<AnswerItem>();
        }
    }
}
