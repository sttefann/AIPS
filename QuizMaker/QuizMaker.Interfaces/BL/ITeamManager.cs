using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizMaker.DAL.DB;


namespace QuizMaker.Interfaces.BL
{
    public interface ITeamManager
    {
        bool AddTeamMember(long quizId, long userId, string username);
        bool DeleteTeamMember(long quizId, long userId);
        List<long> GetAll(long quizId);
        List<string> GetAllMembers(long quizId);
    }
}
