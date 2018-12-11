using QuizMaker.Models.QuizModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Interfaces.BL
{
    public interface IQuizManager
    {
        List<QuizModel> GetAll();
        List<QuizzCategoryModel> GetAll(long user_id);
        List<QuizzCategoryModel> GetAllTeamProjects(long user_id);
        QuizModel Get(long id);
        string GetType(long id);
        string GetPollText(long id);
        string GetQuizName(long id);
        QuizCreateModel GetCreateModel();
        QuizEditModel GetEditModel(long id);
        bool Update(QuizModel model);
        bool Update(long id, QuizEditModel model);
        bool Delete(long id);
        long Add(QuizCreateModel model);
        bool AddTeamMember(long quizId, long userId);
        bool IsOwner(long quizId, long userId);
        QuizPlayModel GetQuiz(long id);
        bool MakeReadOnly(long id);
        bool MakeReadOnly(long id, bool value);
        bool IsTeamProject(long quizId);
        void LeaveTeamProject(long quiz_id);
    }
}
