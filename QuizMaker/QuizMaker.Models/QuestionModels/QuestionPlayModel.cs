using QuizMaker.Models.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Models.QuestionModels
{
    public class QuestionPlayModel
    {
        public long Id { get; set; }
        public string Question_text { get; set; }
        public Nullable<int> Question_number { get; set; }
        public int Points { get; set; }
        public List<ItemModel> CorrectAnswers { get; set; }
        public List<ItemModel> PossibleAnswers { get; set; }
        public List<ItemModel> UserAnswers { get; set; }
        public QuestionPlayModel()
        {
            PossibleAnswers = new List<ItemModel>();
            CorrectAnswers = new List<ItemModel>();
        }
    }
}
