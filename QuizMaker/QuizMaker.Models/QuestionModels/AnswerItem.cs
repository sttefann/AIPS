using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Models.QuestionModels
{
    public class AnswerItem
    {
        public bool IsCorrect { get; set; }
        public string Text { get; set; }

        public AnswerItem(bool isCorrect,string text)
        {
            IsCorrect = isCorrect;
            Text = text;
        }

        public AnswerItem()
        {
        }
    }
}
