using QuizMaker.DAL.DB;
using QuizMaker.Models.QuestionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Interfaces.BL
{
    public interface IQuestionManager
    {
        QuestionModel Get(long id);
        List<QuestionModel> GetQuestionsForQuiz(long id);
        List<PossibleAnswer> GetPossibleAnswers(long questionId);
        long Add(QuestionModel question);
        bool Update(QuestionModel question);
        bool Delete(long id);
    }
}
