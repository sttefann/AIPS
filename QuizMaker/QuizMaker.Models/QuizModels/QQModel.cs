using QuizMaker.Models.QuestionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Models.QuizModels
{
    public class QQModel
    {
        public QuizEditModel Quiz { get; set; }
        public List<QuestionViewModel> Questions { get; set; }
    }
}
