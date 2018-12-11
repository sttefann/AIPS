using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Models.QuestionModels
{
    public class QuestionPlayedModel
    {
        public string Text { get; set; }
        public bool Correct { get; set; }
        public int Points { get; set; }
    }
}
