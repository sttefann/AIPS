using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizMaker.DAL.DB;
using QuizMaker.Models.TeamModel;

namespace QuizMaker.Interfaces.BL
{
    public interface ITeamManager
    {
        TeamModel GetTeam(long id);
        bool Add(TeamModel team);
        bool Update(TeamModel team);
        bool Delete(long id);
        bool AddTeamMember(TeamMember member, long teamId);
        bool DeleteTeamMember(long teamid, long memberId);
        bool AddQuiz(Quiz quiz,long teamId);
        List<Quiz> GetQuizzes(long teamId);

    }
}
