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
        QuestionM Get(long id);
        long GetQuizId(long questionId);
        bool ReadOnly(long id, bool readOnly);
        List<QuestionViewModel> GetQuestionsForQuiz(long id);
        List<QuestionPlayModel> GetQuestions(long id);
        QuestionViewModel GetViewModel(long id);
        List<PossibleAnswer> GetPossibleAnswers(long questionId);
        long Add(QuestionCreateModel question);
        bool Update(QuestionEditModel question);
        bool Delete(long id);
        /// <summary>
        /// Remove all questions from quiz
        /// </summary>
        /// <param name="quizId"></param>
        /// <returns></returns>
        bool DeleteAll(long quizId);
    }
}
