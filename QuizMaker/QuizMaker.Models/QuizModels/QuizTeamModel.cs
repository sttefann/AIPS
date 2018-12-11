using QuizMaker.Models.Item;
using QuizMaker.Models.QuestionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Models.QuizModels
{
    public class QuizTeamModel
    {
        public QuizEditModel Quiz { get; set; }
        public List<QuestionViewModel> Questions { get; set; }
        public bool IsOwner { get; set; }
        public List<ItemModel> Users { get; set; }
        public List<string> Members { get; set; }
        public string Type { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public bool ReadOnly { get; set; }
        public QuizTeamModel()
        {
            Members = new List<string>();
            Users = new List<ItemModel>();
            Questions = new List<QuestionViewModel>();
        }
    }
}
